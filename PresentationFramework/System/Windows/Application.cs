using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Threading;
using Microsoft.Win32;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.IO.Packaging;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;
using MS.Utility;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x02000341 RID: 833
	public class Application : DispatcherObject, IHaveResources, IQueryAmbient
	{
		// Token: 0x06001F42 RID: 8002 RVA: 0x001713A0 File Offset: 0x001703A0
		static Application()
		{
			Application.ApplicationInit();
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x001713CC File Offset: 0x001703CC
		public Application()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Event.WClientAppCtor);
			object globalLock = Application._globalLock;
			lock (globalLock)
			{
				if (Application._appCreatedInThisAppDomain)
				{
					throw new InvalidOperationException(SR.Get("MultiSingleton"));
				}
				Application._appInstance = this;
				Application.IsShuttingDown = false;
				Application._appCreatedInThisAppDomain = true;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
			{
				if (Application.IsShuttingDown)
				{
					return null;
				}
				StartupEventArgs startupEventArgs = new StartupEventArgs();
				this.OnStartup(startupEventArgs);
				if (startupEventArgs.PerformDefaultAction)
				{
					this.DoStartup();
				}
				return null;
			}), null);
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x00171458 File Offset: 0x00170458
		public int Run()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Event.WClientAppRun);
			return this.Run(null);
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x00171468 File Offset: 0x00170468
		public int Run(Window window)
		{
			base.VerifyAccess();
			return this.RunInternal(window);
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x00171478 File Offset: 0x00170478
		internal object GetService(Type serviceType)
		{
			base.VerifyAccess();
			object result = null;
			if (this.ServiceProvider != null)
			{
				result = this.ServiceProvider.GetService(serviceType);
			}
			return result;
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x001714A3 File Offset: 0x001704A3
		public void Shutdown()
		{
			this.Shutdown(0);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x001714AC File Offset: 0x001704AC
		public void Shutdown(int exitCode)
		{
			this.CriticalShutdown(exitCode);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x001714B5 File Offset: 0x001704B5
		internal void CriticalShutdown(int exitCode)
		{
			base.VerifyAccess();
			if (Application.IsShuttingDown)
			{
				return;
			}
			ControlsTraceLogger.LogUsedControlsDetails();
			this.SetExitCode(exitCode);
			Application.IsShuttingDown = true;
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.ShutdownCallback), null);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x001714F4 File Offset: 0x001704F4
		public object FindResource(object resourceKey)
		{
			ResourceDictionary resources = this._resources;
			object obj = null;
			if (resources != null)
			{
				obj = resources[resourceKey];
			}
			if (obj == DependencyProperty.UnsetValue || obj == null)
			{
				obj = SystemResources.FindResourceInternal(resourceKey);
			}
			if (obj == null)
			{
				Helper.ResourceFailureThrow(resourceKey);
			}
			return obj;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x00171534 File Offset: 0x00170534
		public object TryFindResource(object resourceKey)
		{
			ResourceDictionary resources = this._resources;
			object obj = null;
			if (resources != null)
			{
				obj = resources[resourceKey];
			}
			if (obj == DependencyProperty.UnsetValue || obj == null)
			{
				obj = SystemResources.FindResourceInternal(resourceKey);
			}
			return obj;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x00171568 File Offset: 0x00170568
		internal object FindResourceInternal(object resourceKey)
		{
			return this.FindResourceInternal(resourceKey, false, false);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x00171574 File Offset: 0x00170574
		internal object FindResourceInternal(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resources = this._resources;
			if (resources == null)
			{
				return null;
			}
			bool flag;
			return resources.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x00171598 File Offset: 0x00170598
		public static void LoadComponent(object component, Uri resourceLocator)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (resourceLocator == null)
			{
				throw new ArgumentNullException("resourceLocator");
			}
			if (resourceLocator.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"resourceLocator",
					"OriginalString"
				}));
			}
			if (resourceLocator.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.Get("AbsoluteUriNotAllowed"));
			}
			Uri uri = new Uri(BaseUriHelper.PackAppBaseUri, resourceLocator);
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = uri;
			Stream stream;
			bool closeStream;
			if (Application.IsComponentBeingLoadedFromOuterLoadBaml(uri))
			{
				NestedBamlLoadInfo nestedBamlLoadInfo = Application.s_NestedBamlLoadInfo.Peek();
				stream = nestedBamlLoadInfo.BamlStream;
				stream.Seek(0L, SeekOrigin.Begin);
				parserContext.SkipJournaledProperties = nestedBamlLoadInfo.SkipJournaledProperties;
				nestedBamlLoadInfo.BamlUri = null;
				closeStream = false;
			}
			else
			{
				PackagePart resourceOrContentPart = Application.GetResourceOrContentPart(resourceLocator);
				ContentType contentType = new ContentType(resourceOrContentPart.ContentType);
				stream = resourceOrContentPart.GetSeekableStream();
				closeStream = true;
				if (!MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
				{
					throw new Exception(SR.Get("ContentTypeNotSupported", new object[]
					{
						contentType
					}));
				}
			}
			IStreamInfo streamInfo = stream as IStreamInfo;
			if (streamInfo == null || streamInfo.Assembly != component.GetType().Assembly)
			{
				throw new Exception(SR.Get("UriNotMatchWithRootType", new object[]
				{
					component.GetType(),
					resourceLocator
				}));
			}
			XamlReader.LoadBaml(stream, parserContext, component, closeStream);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x00171700 File Offset: 0x00170700
		public static object LoadComponent(Uri resourceLocator)
		{
			if (resourceLocator == null)
			{
				throw new ArgumentNullException("resourceLocator");
			}
			if (resourceLocator.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"resourceLocator",
					"OriginalString"
				}));
			}
			if (resourceLocator.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.Get("AbsoluteUriNotAllowed"));
			}
			return Application.LoadComponent(resourceLocator, false);
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x00171770 File Offset: 0x00170770
		internal static object LoadComponent(Uri resourceLocator, bool bSkipJournaledProperties)
		{
			Uri resolvedUri = BindUriHelper.GetResolvedUri(BaseUriHelper.PackAppBaseUri, resourceLocator);
			PackagePart resourceOrContentPart = Application.GetResourceOrContentPart(resolvedUri);
			ContentType contentType = new ContentType(resourceOrContentPart.ContentType);
			Stream seekableStream = resourceOrContentPart.GetSeekableStream();
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = resolvedUri;
			parserContext.SkipJournaledProperties = bSkipJournaledProperties;
			if (MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
			{
				return Application.LoadBamlStreamWithSyncInfo(seekableStream, parserContext);
			}
			if (MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(contentType))
			{
				return XamlReader.Load(seekableStream, parserContext);
			}
			throw new Exception(SR.Get("ContentTypeNotSupported", new object[]
			{
				contentType.ToString()
			}));
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x00171800 File Offset: 0x00170800
		internal static object LoadBamlStreamWithSyncInfo(Stream stream, ParserContext pc)
		{
			object result = null;
			if (Application.s_NestedBamlLoadInfo == null)
			{
				Application.s_NestedBamlLoadInfo = new Stack<NestedBamlLoadInfo>();
			}
			NestedBamlLoadInfo item = new NestedBamlLoadInfo(pc.BaseUri, stream, pc.SkipJournaledProperties);
			Application.s_NestedBamlLoadInfo.Push(item);
			try
			{
				result = XamlReader.LoadBaml(stream, pc, null, true);
			}
			finally
			{
				Application.s_NestedBamlLoadInfo.Pop();
				if (Application.s_NestedBamlLoadInfo.Count == 0)
				{
					Application.s_NestedBamlLoadInfo = null;
				}
			}
			return result;
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0017187C File Offset: 0x0017087C
		public static StreamResourceInfo GetResourceStream(Uri uriResource)
		{
			if (uriResource == null)
			{
				throw new ArgumentNullException("uriResource");
			}
			if (uriResource.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"uriResource",
					"OriginalString"
				}));
			}
			if (uriResource.IsAbsoluteUri && !BaseUriHelper.IsPackApplicationUri(uriResource))
			{
				throw new ArgumentException(SR.Get("NonPackAppAbsoluteUriNotAllowed"));
			}
			ResourcePart resourcePart = Application.GetResourceOrContentPart(uriResource) as ResourcePart;
			if (resourcePart != null)
			{
				return new StreamResourceInfo(resourcePart.GetSeekableStream(), resourcePart.ContentType);
			}
			return null;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x00171910 File Offset: 0x00170910
		public static StreamResourceInfo GetContentStream(Uri uriContent)
		{
			if (uriContent == null)
			{
				throw new ArgumentNullException("uriContent");
			}
			if (uriContent.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"uriContent",
					"OriginalString"
				}));
			}
			if (uriContent.IsAbsoluteUri && !BaseUriHelper.IsPackApplicationUri(uriContent))
			{
				throw new ArgumentException(SR.Get("NonPackAppAbsoluteUriNotAllowed"));
			}
			ContentFilePart contentFilePart = Application.GetResourceOrContentPart(uriContent) as ContentFilePart;
			if (contentFilePart != null)
			{
				return new StreamResourceInfo(contentFilePart.GetSeekableStream(), contentFilePart.ContentType);
			}
			return null;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x001719A4 File Offset: 0x001709A4
		public static StreamResourceInfo GetRemoteStream(Uri uriRemote)
		{
			SiteOfOriginPart siteOfOriginPart = null;
			if (uriRemote == null)
			{
				throw new ArgumentNullException("uriRemote");
			}
			if (uriRemote.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"uriRemote",
					"OriginalString"
				}));
			}
			if (uriRemote.IsAbsoluteUri && !BaseUriHelper.SiteOfOriginBaseUri.IsBaseOf(uriRemote))
			{
				throw new ArgumentException(SR.Get("NonPackSooAbsoluteUriNotAllowed"));
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(BaseUriHelper.SiteOfOriginBaseUri, uriRemote);
			Uri packageUri = PackUriHelper.GetPackageUri(resolvedUri);
			Uri partUri = PackUriHelper.GetPartUri(resolvedUri);
			SiteOfOriginContainer siteOfOriginContainer = (SiteOfOriginContainer)Application.GetResourcePackage(packageUri);
			SiteOfOriginContainer obj = siteOfOriginContainer;
			lock (obj)
			{
				siteOfOriginPart = (siteOfOriginContainer.GetPart(partUri) as SiteOfOriginPart);
			}
			Stream stream = null;
			if (siteOfOriginPart != null)
			{
				try
				{
					stream = siteOfOriginPart.GetSeekableStream();
					if (stream == null)
					{
						siteOfOriginPart = null;
					}
				}
				catch (WebException)
				{
					siteOfOriginPart = null;
				}
			}
			if (stream != null)
			{
				return new StreamResourceInfo(stream, siteOfOriginPart.ContentType);
			}
			return null;
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x00171AB4 File Offset: 0x00170AB4
		public static string GetCookie(Uri uri)
		{
			return CookieHandler.GetCookie(uri, true);
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x00171ABD File Offset: 0x00170ABD
		public static void SetCookie(Uri uri, string value)
		{
			CookieHandler.SetCookie(uri, value);
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x00171AC7 File Offset: 0x00170AC7
		public static Application Current
		{
			get
			{
				return Application._appInstance;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x00171ACE File Offset: 0x00170ACE
		public WindowCollection Windows
		{
			get
			{
				base.VerifyAccess();
				return this.WindowsInternal.Clone();
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001F59 RID: 8025 RVA: 0x00171AE1 File Offset: 0x00170AE1
		// (set) Token: 0x06001F5A RID: 8026 RVA: 0x00171AEF File Offset: 0x00170AEF
		public Window MainWindow
		{
			get
			{
				base.VerifyAccess();
				return this._mainWindow;
			}
			set
			{
				base.VerifyAccess();
				if (value != this._mainWindow)
				{
					this._mainWindow = value;
				}
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x00171B07 File Offset: 0x00170B07
		// (set) Token: 0x06001F5C RID: 8028 RVA: 0x00171B18 File Offset: 0x00170B18
		public ShutdownMode ShutdownMode
		{
			get
			{
				base.VerifyAccess();
				return this._shutdownMode;
			}
			set
			{
				base.VerifyAccess();
				if (!Application.IsValidShutdownMode(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ShutdownMode));
				}
				if (Application.IsShuttingDown || this._appIsShutdown)
				{
					throw new InvalidOperationException(SR.Get("ShutdownModeWhenAppShutdown"));
				}
				this._shutdownMode = value;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F5D RID: 8029 RVA: 0x00171B70 File Offset: 0x00170B70
		// (set) Token: 0x06001F5E RID: 8030 RVA: 0x00171BD4 File Offset: 0x00170BD4
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				bool flag = false;
				object globalLock = Application._globalLock;
				ResourceDictionary resources;
				lock (globalLock)
				{
					if (this._resources == null)
					{
						this._resources = new ResourceDictionary();
						flag = true;
					}
					resources = this._resources;
				}
				if (flag)
				{
					resources.AddOwner(this);
				}
				return resources;
			}
			set
			{
				bool flag = false;
				object globalLock = Application._globalLock;
				ResourceDictionary resources;
				lock (globalLock)
				{
					resources = this._resources;
					this._resources = value;
				}
				if (resources != null)
				{
					resources.RemoveOwner(this);
				}
				if (value != null && !value.ContainsOwner(this))
				{
					value.AddOwner(this);
				}
				if (resources != value)
				{
					flag = true;
				}
				if (flag)
				{
					this.InvalidateResourceReferences(new ResourcesChangeInfo(resources, value));
				}
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F5F RID: 8031 RVA: 0x00171C50 File Offset: 0x00170C50
		// (set) Token: 0x06001F60 RID: 8032 RVA: 0x00171C58 File Offset: 0x00170C58
		ResourceDictionary IHaveResources.Resources
		{
			get
			{
				return this.Resources;
			}
			set
			{
				this.Resources = value;
			}
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x00171C61 File Offset: 0x00170C61
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return propertyName == "Resources" && this._resources != null;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x00171C7B File Offset: 0x00170C7B
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x00171C83 File Offset: 0x00170C83
		internal bool HasImplicitStylesInResources
		{
			get
			{
				return this._hasImplicitStylesInResources;
			}
			set
			{
				this._hasImplicitStylesInResources = value;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x00171C8C File Offset: 0x00170C8C
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x00171C94 File Offset: 0x00170C94
		public Uri StartupUri
		{
			get
			{
				return this._startupUri;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._startupUri = value;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x00171CB8 File Offset: 0x00170CB8
		public IDictionary Properties
		{
			get
			{
				object globalLock = Application._globalLock;
				IDictionary htProps;
				lock (globalLock)
				{
					if (this._htProps == null)
					{
						this._htProps = new HybridDictionary(5);
					}
					htProps = this._htProps;
				}
				return htProps;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x00171D10 File Offset: 0x00170D10
		// (set) Token: 0x06001F68 RID: 8040 RVA: 0x00171D68 File Offset: 0x00170D68
		public static Assembly ResourceAssembly
		{
			get
			{
				if (Application._resourceAssembly == null)
				{
					object globalLock = Application._globalLock;
					lock (globalLock)
					{
						Application._resourceAssembly = Assembly.GetEntryAssembly();
					}
				}
				return Application._resourceAssembly;
			}
			set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					if (Application._resourceAssembly != value)
					{
						if (!(Application._resourceAssembly == null) || !(Assembly.GetEntryAssembly() == null))
						{
							throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
							{
								"ResourceAssembly",
								"Application"
							}));
						}
						Application._resourceAssembly = value;
						BaseUriHelper.ResourceAssembly = value;
					}
				}
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06001F69 RID: 8041 RVA: 0x00171DFC File Offset: 0x00170DFC
		// (remove) Token: 0x06001F6A RID: 8042 RVA: 0x00171E15 File Offset: 0x00170E15
		public event StartupEventHandler Startup
		{
			add
			{
				base.VerifyAccess();
				this.Events.AddHandler(Application.EVENT_STARTUP, value);
			}
			remove
			{
				base.VerifyAccess();
				this.Events.RemoveHandler(Application.EVENT_STARTUP, value);
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06001F6B RID: 8043 RVA: 0x00171E2E File Offset: 0x00170E2E
		// (remove) Token: 0x06001F6C RID: 8044 RVA: 0x00171E47 File Offset: 0x00170E47
		public event ExitEventHandler Exit
		{
			add
			{
				base.VerifyAccess();
				this.Events.AddHandler(Application.EVENT_EXIT, value);
			}
			remove
			{
				base.VerifyAccess();
				this.Events.RemoveHandler(Application.EVENT_EXIT, value);
			}
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06001F6D RID: 8045 RVA: 0x00171E60 File Offset: 0x00170E60
		// (remove) Token: 0x06001F6E RID: 8046 RVA: 0x00171E98 File Offset: 0x00170E98
		public event EventHandler Activated;

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06001F6F RID: 8047 RVA: 0x00171ED0 File Offset: 0x00170ED0
		// (remove) Token: 0x06001F70 RID: 8048 RVA: 0x00171F08 File Offset: 0x00170F08
		public event EventHandler Deactivated;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06001F71 RID: 8049 RVA: 0x00171F3D File Offset: 0x00170F3D
		// (remove) Token: 0x06001F72 RID: 8050 RVA: 0x00171F56 File Offset: 0x00170F56
		public event SessionEndingCancelEventHandler SessionEnding
		{
			add
			{
				base.VerifyAccess();
				this.Events.AddHandler(Application.EVENT_SESSIONENDING, value);
			}
			remove
			{
				base.VerifyAccess();
				this.Events.RemoveHandler(Application.EVENT_SESSIONENDING, value);
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06001F73 RID: 8051 RVA: 0x00171F70 File Offset: 0x00170F70
		// (remove) Token: 0x06001F74 RID: 8052 RVA: 0x00171FAC File Offset: 0x00170FAC
		public event DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException
		{
			add
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this.Dispatcher.UnhandledException += value;
					return null;
				}), null);
			}
			remove
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this.Dispatcher.UnhandledException -= value;
					return null;
				}), null);
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06001F75 RID: 8053 RVA: 0x00171FE8 File Offset: 0x00170FE8
		// (remove) Token: 0x06001F76 RID: 8054 RVA: 0x00172020 File Offset: 0x00171020
		public event NavigatingCancelEventHandler Navigating;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06001F77 RID: 8055 RVA: 0x00172058 File Offset: 0x00171058
		// (remove) Token: 0x06001F78 RID: 8056 RVA: 0x00172090 File Offset: 0x00171090
		public event NavigatedEventHandler Navigated;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06001F79 RID: 8057 RVA: 0x001720C8 File Offset: 0x001710C8
		// (remove) Token: 0x06001F7A RID: 8058 RVA: 0x00172100 File Offset: 0x00171100
		public event NavigationProgressEventHandler NavigationProgress;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06001F7B RID: 8059 RVA: 0x00172138 File Offset: 0x00171138
		// (remove) Token: 0x06001F7C RID: 8060 RVA: 0x00172170 File Offset: 0x00171170
		public event NavigationFailedEventHandler NavigationFailed;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06001F7D RID: 8061 RVA: 0x001721A8 File Offset: 0x001711A8
		// (remove) Token: 0x06001F7E RID: 8062 RVA: 0x001721E0 File Offset: 0x001711E0
		public event LoadCompletedEventHandler LoadCompleted;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06001F7F RID: 8063 RVA: 0x00172218 File Offset: 0x00171218
		// (remove) Token: 0x06001F80 RID: 8064 RVA: 0x00172250 File Offset: 0x00171250
		public event NavigationStoppedEventHandler NavigationStopped;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06001F81 RID: 8065 RVA: 0x00172288 File Offset: 0x00171288
		// (remove) Token: 0x06001F82 RID: 8066 RVA: 0x001722C0 File Offset: 0x001712C0
		public event FragmentNavigationEventHandler FragmentNavigation;

		// Token: 0x06001F83 RID: 8067 RVA: 0x001722F8 File Offset: 0x001712F8
		protected virtual void OnStartup(StartupEventArgs e)
		{
			base.VerifyAccess();
			StartupEventHandler startupEventHandler = (StartupEventHandler)this.Events[Application.EVENT_STARTUP];
			if (startupEventHandler != null)
			{
				startupEventHandler(this, e);
			}
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0017232C File Offset: 0x0017132C
		protected virtual void OnExit(ExitEventArgs e)
		{
			base.VerifyAccess();
			ExitEventHandler exitEventHandler = (ExitEventHandler)this.Events[Application.EVENT_EXIT];
			if (exitEventHandler != null)
			{
				exitEventHandler(this, e);
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x00172360 File Offset: 0x00171360
		protected virtual void OnActivated(EventArgs e)
		{
			base.VerifyAccess();
			if (this.Activated != null)
			{
				this.Activated(this, e);
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0017237D File Offset: 0x0017137D
		protected virtual void OnDeactivated(EventArgs e)
		{
			base.VerifyAccess();
			if (this.Deactivated != null)
			{
				this.Deactivated(this, e);
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0017239C File Offset: 0x0017139C
		protected virtual void OnSessionEnding(SessionEndingCancelEventArgs e)
		{
			base.VerifyAccess();
			SessionEndingCancelEventHandler sessionEndingCancelEventHandler = (SessionEndingCancelEventHandler)this.Events[Application.EVENT_SESSIONENDING];
			if (sessionEndingCancelEventHandler != null)
			{
				sessionEndingCancelEventHandler(this, e);
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x001723D0 File Offset: 0x001713D0
		protected virtual void OnNavigating(NavigatingCancelEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x001723ED File Offset: 0x001713ED
		protected virtual void OnNavigated(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0017240A File Offset: 0x0017140A
		protected virtual void OnNavigationProgress(NavigationProgressEventArgs e)
		{
			base.VerifyAccess();
			if (this.NavigationProgress != null)
			{
				this.NavigationProgress(this, e);
			}
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00172427 File Offset: 0x00171427
		protected virtual void OnNavigationFailed(NavigationFailedEventArgs e)
		{
			base.VerifyAccess();
			if (this.NavigationFailed != null)
			{
				this.NavigationFailed(this, e);
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x00172444 File Offset: 0x00171444
		protected virtual void OnLoadCompleted(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.LoadCompleted != null)
			{
				this.LoadCompleted(this, e);
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x00172461 File Offset: 0x00171461
		protected virtual void OnNavigationStopped(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.NavigationStopped != null)
			{
				this.NavigationStopped(this, e);
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0017247E File Offset: 0x0017147E
		protected virtual void OnFragmentNavigation(FragmentNavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.FragmentNavigation != null)
			{
				this.FragmentNavigation(this, e);
			}
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0017249B File Offset: 0x0017149B
		internal virtual void PerformNavigationStateChangeTasks(bool isNavigationInitiator, bool playNavigatingSound, Application.NavigationStateChange state)
		{
			if (isNavigationInitiator)
			{
				switch (state)
				{
				case Application.NavigationStateChange.Navigating:
					if (playNavigatingSound)
					{
						this.PlaySound("Navigating");
						return;
					}
					break;
				case Application.NavigationStateChange.Completed:
					this.PlaySound("ActivatingDocument");
					break;
				case Application.NavigationStateChange.Stopped:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x001724D0 File Offset: 0x001714D0
		internal void DoStartup()
		{
			if (this.StartupUri != null)
			{
				if (!this.StartupUri.IsAbsoluteUri)
				{
					this.StartupUri = new Uri(this.ApplicationMarkupBaseUri, this.StartupUri);
				}
				if (BaseUriHelper.IsPackApplicationUri(this.StartupUri))
				{
					NavigatingCancelEventArgs navigatingCancelEventArgs = new NavigatingCancelEventArgs(BindUriHelper.GetUriRelativeToPackAppBase(this.StartupUri), null, null, null, NavigationMode.New, null, null, true);
					this.FireNavigating(navigatingCancelEventArgs, true);
					if (!navigatingCancelEventArgs.Cancel)
					{
						object root = Application.LoadComponent(this.StartupUri, false);
						this.ConfigAppWindowAndRootElement(root, this.StartupUri);
						return;
					}
				}
				else
				{
					this.NavService = new NavigationService(null);
					this.NavService.AllowWindowNavigation = true;
					this.NavService.PreBPReady += this.OnPreBPReady;
					this.NavService.Navigate(this.StartupUri);
				}
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x001725A4 File Offset: 0x001715A4
		internal virtual void DoShutdown()
		{
			while (this.WindowsInternal.Count > 0)
			{
				if (!this.WindowsInternal[0].IsDisposed)
				{
					this.WindowsInternal[0].InternalClose(true, true);
				}
				else
				{
					this.WindowsInternal.RemoveAt(0);
				}
			}
			this.WindowsInternal = null;
			ExitEventArgs exitEventArgs = new ExitEventArgs(this._exitCode);
			try
			{
				this.OnExit(exitEventArgs);
			}
			finally
			{
				this.SetExitCode(exitEventArgs._exitCode);
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					Application._appInstance = null;
				}
				this._mainWindow = null;
				this._htProps = null;
				this.NonAppWindowsInternal = null;
				if (this._parkingHwnd != null)
				{
					this._parkingHwnd.Dispose();
				}
				if (this._events != null)
				{
					this._events.Dispose();
				}
				PreloadedPackages.Clear();
				AppSecurityManager.ClearSecurityManager();
				this._appIsShutdown = true;
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x001726A8 File Offset: 0x001716A8
		internal int RunInternal(Window window)
		{
			base.VerifyAccess();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Event.WClientAppRun);
			if (this._appIsShutdown)
			{
				throw new InvalidOperationException(SR.Get("CannotCallRunMultipleTimes", new object[]
				{
					base.GetType().FullName
				}));
			}
			if (window != null)
			{
				if (!window.CheckAccess())
				{
					throw new ArgumentException(SR.Get("WindowPassedShouldBeOnApplicationThread", new object[]
					{
						window.GetType().FullName,
						base.GetType().FullName
					}));
				}
				if (!this.WindowsInternal.HasItem(window))
				{
					this.WindowsInternal.Add(window);
				}
				if (this.MainWindow == null)
				{
					this.MainWindow = window;
				}
				if (window.Visibility != Visibility.Visible)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object obj)
					{
						(obj as Window).Show();
						return null;
					}), window);
				}
			}
			this.EnsureHwndSource();
			this.RunDispatcher(null);
			return this._exitCode;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x001727A2 File Offset: 0x001717A2
		internal void InvalidateResourceReferences(ResourcesChangeInfo info)
		{
			this.InvalidateResourceReferenceOnWindowCollection(this.WindowsInternal.Clone(), info);
			this.InvalidateResourceReferenceOnWindowCollection(this.NonAppWindowsInternal.Clone(), info);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x001727C8 File Offset: 0x001717C8
		internal NavigationWindow GetAppWindow()
		{
			NavigationWindow navigationWindow = new NavigationWindow();
			new WindowInteropHelper(navigationWindow).EnsureHandle();
			return navigationWindow;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x001727DB File Offset: 0x001717DB
		internal void FireNavigating(NavigatingCancelEventArgs e, bool isInitialNavigation)
		{
			this.PerformNavigationStateChangeTasks(e.IsNavigationInitiator, !isInitialNavigation, Application.NavigationStateChange.Navigating);
			this.OnNavigating(e);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x001727F5 File Offset: 0x001717F5
		internal void FireNavigated(NavigationEventArgs e)
		{
			this.OnNavigated(e);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x001727FE File Offset: 0x001717FE
		internal void FireNavigationProgress(NavigationProgressEventArgs e)
		{
			this.OnNavigationProgress(e);
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x00172807 File Offset: 0x00171807
		internal void FireNavigationFailed(NavigationFailedEventArgs e)
		{
			this.PerformNavigationStateChangeTasks(true, false, Application.NavigationStateChange.Stopped);
			this.OnNavigationFailed(e);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x00172819 File Offset: 0x00171819
		internal void FireLoadCompleted(NavigationEventArgs e)
		{
			this.PerformNavigationStateChangeTasks(e.IsNavigationInitiator, false, Application.NavigationStateChange.Completed);
			this.OnLoadCompleted(e);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x00172830 File Offset: 0x00171830
		internal void FireNavigationStopped(NavigationEventArgs e)
		{
			this.PerformNavigationStateChangeTasks(e.IsNavigationInitiator, false, Application.NavigationStateChange.Stopped);
			this.OnNavigationStopped(e);
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x00172847 File Offset: 0x00171847
		internal void FireFragmentNavigation(FragmentNavigationEventArgs e)
		{
			this.OnFragmentNavigation(e);
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x00172850 File Offset: 0x00171850
		// (set) Token: 0x06001F9D RID: 8093 RVA: 0x001728A4 File Offset: 0x001718A4
		internal WindowCollection WindowsInternal
		{
			get
			{
				object globalLock = Application._globalLock;
				WindowCollection appWindowList;
				lock (globalLock)
				{
					if (this._appWindowList == null)
					{
						this._appWindowList = new WindowCollection();
					}
					appWindowList = this._appWindowList;
				}
				return appWindowList;
			}
			private set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					this._appWindowList = value;
				}
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x001728E4 File Offset: 0x001718E4
		// (set) Token: 0x06001F9F RID: 8095 RVA: 0x00172938 File Offset: 0x00171938
		internal WindowCollection NonAppWindowsInternal
		{
			get
			{
				object globalLock = Application._globalLock;
				WindowCollection nonAppWindowList;
				lock (globalLock)
				{
					if (this._nonAppWindowList == null)
					{
						this._nonAppWindowList = new WindowCollection();
					}
					nonAppWindowList = this._nonAppWindowList;
				}
				return nonAppWindowList;
			}
			private set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					this._nonAppWindowList = value;
				}
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x00172978 File Offset: 0x00171978
		// (set) Token: 0x06001FA1 RID: 8097 RVA: 0x00172985 File Offset: 0x00171985
		internal MimeType MimeType
		{
			get
			{
				return this._appMimeType.Value;
			}
			set
			{
				this._appMimeType = new SecurityCriticalDataForSet<MimeType>(value);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x00172993 File Offset: 0x00171993
		// (set) Token: 0x06001FA3 RID: 8099 RVA: 0x001729AB File Offset: 0x001719AB
		internal IServiceProvider ServiceProvider
		{
			private get
			{
				base.VerifyAccess();
				if (this._serviceProvider != null)
				{
					return this._serviceProvider;
				}
				return null;
			}
			set
			{
				base.VerifyAccess();
				this._serviceProvider = value;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x001729BA File Offset: 0x001719BA
		// (set) Token: 0x06001FA5 RID: 8101 RVA: 0x001729C8 File Offset: 0x001719C8
		internal NavigationService NavService
		{
			get
			{
				base.VerifyAccess();
				return this._navService;
			}
			set
			{
				base.VerifyAccess();
				this._navService = value;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x001729D7 File Offset: 0x001719D7
		// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x001729E8 File Offset: 0x001719E8
		internal static bool IsShuttingDown
		{
			get
			{
				return Application._isShuttingDown && Application._isShuttingDown;
			}
			set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					Application._isShuttingDown = value;
				}
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x00172A28 File Offset: 0x00171A28
		internal static bool IsApplicationObjectShuttingDown
		{
			get
			{
				return Application._isShuttingDown;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x00172A2F File Offset: 0x00171A2F
		internal IntPtr ParkingHwnd
		{
			get
			{
				if (this._parkingHwnd != null)
				{
					return this._parkingHwnd.Handle;
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x00172A4A File Offset: 0x00171A4A
		// (set) Token: 0x06001FAB RID: 8107 RVA: 0x00172A6B File Offset: 0x00171A6B
		internal Uri ApplicationMarkupBaseUri
		{
			get
			{
				if (this._applicationMarkupBaseUri == null)
				{
					this._applicationMarkupBaseUri = BaseUriHelper.BaseUri;
				}
				return this._applicationMarkupBaseUri;
			}
			set
			{
				this._applicationMarkupBaseUri = value;
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x00172A74 File Offset: 0x00171A74
		private static void ApplicationInit()
		{
			Application._globalLock = new object();
			PreloadedPackages.AddPackage(PackUriHelper.GetPackageUri(BaseUriHelper.PackAppBaseUri), new ResourceContainer(), true);
			MimeObjectFactory.Register(MimeTypeMapper.BamlMime, new StreamToObjectFactoryDelegate(AppModelKnownContentFactory.BamlConverter));
			StreamToObjectFactoryDelegate method = new StreamToObjectFactoryDelegate(AppModelKnownContentFactory.XamlConverter);
			MimeObjectFactory.Register(MimeTypeMapper.XamlMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.FixedDocumentMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.FixedDocumentSequenceMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.FixedPageMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.ResourceDictionaryMime, method);
			StreamToObjectFactoryDelegate method2 = new StreamToObjectFactoryDelegate(AppModelKnownContentFactory.HtmlXappConverter);
			MimeObjectFactory.Register(MimeTypeMapper.HtmMime, method2);
			MimeObjectFactory.Register(MimeTypeMapper.HtmlMime, method2);
			MimeObjectFactory.Register(MimeTypeMapper.XbapMime, method2);
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x00172B28 File Offset: 0x00171B28
		private static PackagePart GetResourceOrContentPart(Uri uri)
		{
			Uri resolvedUri = BindUriHelper.GetResolvedUri(BaseUriHelper.PackAppBaseUri, uri);
			Uri packageUri = PackUriHelper.GetPackageUri(resolvedUri);
			Uri partUri = PackUriHelper.GetPartUri(resolvedUri);
			ResourceContainer resourceContainer = (ResourceContainer)Application.GetResourcePackage(packageUri);
			PackagePart result = null;
			ResourceContainer obj = resourceContainer;
			lock (obj)
			{
				result = resourceContainer.GetPart(partUri);
			}
			return result;
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x00172B94 File Offset: 0x00171B94
		private static Package GetResourcePackage(Uri packageUri)
		{
			Package package = PreloadedPackages.GetPackage(packageUri);
			if (package == null)
			{
				Uri uri = PackUriHelper.Create(packageUri);
				Invariant.Assert(uri == BaseUriHelper.PackAppBaseUri || uri == BaseUriHelper.SiteOfOriginBaseUri, "Unknown packageUri passed: " + ((packageUri != null) ? packageUri.ToString() : null));
				Invariant.Assert(Application.IsApplicationObjectShuttingDown);
				throw new InvalidOperationException(SR.Get("ApplicationShuttingDown"));
			}
			return package;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00172C04 File Offset: 0x00171C04
		private void EnsureHwndSource()
		{
			if (this._parkingHwnd == null)
			{
				this._appFilterHook = new HwndWrapperHook(this.AppFilterMessage);
				HwndWrapperHook[] hooks = new HwndWrapperHook[]
				{
					this._appFilterHook
				};
				this._parkingHwnd = new HwndWrapper(0, 0, 0, 0, 0, 0, 0, "", IntPtr.Zero, hooks);
			}
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x00172C58 File Offset: 0x00171C58
		private IntPtr AppFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			if (msg != 17)
			{
				if (msg == 28)
				{
					handled = this.WmActivateApp(NativeMethods.IntPtrToInt32(wParam));
				}
				else
				{
					handled = false;
				}
			}
			else
			{
				handled = this.WmQueryEndSession(lParam, ref zero);
			}
			return zero;
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x00172C9C File Offset: 0x00171C9C
		private bool WmActivateApp(int wParam)
		{
			if (wParam != 0)
			{
				this.OnActivated(EventArgs.Empty);
			}
			else
			{
				this.OnDeactivated(EventArgs.Empty);
			}
			return false;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x00172CC0 File Offset: 0x00171CC0
		private bool WmQueryEndSession(IntPtr lParam, ref IntPtr refInt)
		{
			SessionEndingCancelEventArgs sessionEndingCancelEventArgs = new SessionEndingCancelEventArgs(((NativeMethods.IntPtrToInt32(lParam) & int.MinValue) != 0) ? ReasonSessionEnding.Logoff : ReasonSessionEnding.Shutdown);
			this.OnSessionEnding(sessionEndingCancelEventArgs);
			bool result;
			if (!sessionEndingCancelEventArgs.Cancel)
			{
				this.Shutdown();
				refInt = new IntPtr(1);
				result = false;
			}
			else
			{
				refInt = IntPtr.Zero;
				result = true;
			}
			return result;
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x00172D14 File Offset: 0x00171D14
		private void InvalidateResourceReferenceOnWindowCollection(WindowCollection wc, ResourcesChangeInfo info)
		{
			bool hasImplicitStyles = info.IsResourceAddOperation && this.HasImplicitStylesInResources;
			DispatcherOperationCallback <>9__0;
			for (int i = 0; i < wc.Count; i++)
			{
				if (wc[i].CheckAccess())
				{
					if (hasImplicitStyles)
					{
						wc[i].ShouldLookupImplicitStyles = true;
					}
					TreeWalkHelper.InvalidateOnResourcesChange(wc[i], null, info);
				}
				else
				{
					Dispatcher dispatcher = wc[i].Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Send;
					DispatcherOperationCallback method;
					if ((method = <>9__0) == null)
					{
						method = (<>9__0 = delegate(object obj)
						{
							object[] array = obj as object[];
							if (hasImplicitStyles)
							{
								((FrameworkElement)array[0]).ShouldLookupImplicitStyles = true;
							}
							TreeWalkHelper.InvalidateOnResourcesChange((FrameworkElement)array[0], null, (ResourcesChangeInfo)array[1]);
							return null;
						});
					}
					dispatcher.BeginInvoke(priority, method, new object[]
					{
						wc[i],
						info
					});
				}
			}
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x00172DD5 File Offset: 0x00171DD5
		private void SetExitCode(int exitCode)
		{
			if (this._exitCode != exitCode)
			{
				this._exitCode = exitCode;
				Environment.ExitCode = exitCode;
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x00172DED File Offset: 0x00171DED
		private object ShutdownCallback(object arg)
		{
			this.ShutdownImpl();
			return null;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x00172DF8 File Offset: 0x00171DF8
		private void ShutdownImpl()
		{
			try
			{
				this.DoShutdown();
			}
			finally
			{
				if (this._ownDispatcherStarted)
				{
					base.Dispatcher.CriticalInvokeShutdown();
				}
				this.ServiceProvider = null;
			}
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x00172E38 File Offset: 0x00171E38
		private static bool IsValidShutdownMode(ShutdownMode value)
		{
			return value == ShutdownMode.OnExplicitShutdown || value == ShutdownMode.OnLastWindowClose || value == ShutdownMode.OnMainWindowClose;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00172E48 File Offset: 0x00171E48
		private void OnPreBPReady(object sender, BPReadyEventArgs e)
		{
			this.NavService.PreBPReady -= this.OnPreBPReady;
			this.NavService.AllowWindowNavigation = false;
			this.ConfigAppWindowAndRootElement(e.Content, e.Uri);
			this.NavService = null;
			e.Cancel = true;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x00172E98 File Offset: 0x00171E98
		private void ConfigAppWindowAndRootElement(object root, Uri uri)
		{
			Window window2 = root as Window;
			if (window2 == null)
			{
				NavigationWindow appWindow = this.GetAppWindow();
				appWindow.Navigate(root, new NavigateInfo(uri));
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new SendOrPostCallback(delegate(object window)
				{
					if (!((Window)window).IsDisposed)
					{
						((Window)window).Show();
					}
				}), appWindow);
				return;
			}
			if (!window2.IsVisibilitySet && !window2.IsDisposed)
			{
				window2.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x00172F0C File Offset: 0x00171F0C
		private void PlaySound(string soundName)
		{
			string systemSound = this.GetSystemSound(soundName);
			if (!string.IsNullOrEmpty(systemSound))
			{
				UnsafeNativeMethods.PlaySound(systemSound, IntPtr.Zero, SafeNativeMethods.PlaySoundFlags.SND_ASYNC | SafeNativeMethods.PlaySoundFlags.SND_NODEFAULT | SafeNativeMethods.PlaySoundFlags.SND_NOSTOP | SafeNativeMethods.PlaySoundFlags.SND_FILENAME);
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00172F3C File Offset: 0x00171F3C
		private string GetSystemSound(string soundName)
		{
			string result = null;
			string name = string.Format(CultureInfo.InvariantCulture, "AppEvents\\Schemes\\Apps\\Explorer\\{0}\\.current\\", soundName);
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name))
				{
					if (registryKey != null)
					{
						result = (string)registryKey.GetValue("");
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
			}
			return result;
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x00172FAC File Offset: 0x00171FAC
		private EventHandlerList Events
		{
			get
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				return this._events;
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00172FC8 File Offset: 0x00171FC8
		private static bool IsComponentBeingLoadedFromOuterLoadBaml(Uri curComponentUri)
		{
			bool result = false;
			Invariant.Assert(curComponentUri != null, "curComponentUri should not be null");
			if (Application.s_NestedBamlLoadInfo != null && Application.s_NestedBamlLoadInfo.Count > 0)
			{
				NestedBamlLoadInfo nestedBamlLoadInfo = Application.s_NestedBamlLoadInfo.Peek();
				if (nestedBamlLoadInfo != null && nestedBamlLoadInfo.BamlUri != null && nestedBamlLoadInfo.BamlStream != null && BindUriHelper.DoSchemeAndHostMatch(nestedBamlLoadInfo.BamlUri, curComponentUri))
				{
					string localPath = nestedBamlLoadInfo.BamlUri.LocalPath;
					string localPath2 = curComponentUri.LocalPath;
					Invariant.Assert(localPath != null, "fileInBamlConvert should not be null");
					Invariant.Assert(localPath2 != null, "fileCurrent should not be null");
					if (string.Compare(localPath, localPath2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						result = true;
					}
					else
					{
						string[] array = localPath.Split(new char[]
						{
							'/',
							'\\'
						});
						string[] array2 = localPath2.Split(new char[]
						{
							'/',
							'\\'
						});
						int num = array.Length;
						int num2 = array2.Length;
						Invariant.Assert(num >= 2 && num2 >= 2);
						int num3 = num - num2;
						if (Math.Abs(num3) == 1 && string.Compare(array[num - 1], array2[num2 - 1], StringComparison.OrdinalIgnoreCase) == 0)
						{
							result = BaseUriHelper.IsComponentEntryAssembly((num3 == 1) ? array[1] : array2[1]);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0017310E File Offset: 0x0017210E
		private object RunDispatcher(object ignore)
		{
			if (this._ownDispatcherStarted)
			{
				throw new InvalidOperationException(SR.Get("ApplicationAlreadyRunning"));
			}
			this._ownDispatcherStarted = true;
			Dispatcher.Run();
			return null;
		}

		// Token: 0x04000F79 RID: 3961
		private static object _globalLock;

		// Token: 0x04000F7A RID: 3962
		private static bool _isShuttingDown;

		// Token: 0x04000F7B RID: 3963
		private static bool _appCreatedInThisAppDomain;

		// Token: 0x04000F7C RID: 3964
		private static Application _appInstance;

		// Token: 0x04000F7D RID: 3965
		private static Assembly _resourceAssembly;

		// Token: 0x04000F7E RID: 3966
		[ThreadStatic]
		private static Stack<NestedBamlLoadInfo> s_NestedBamlLoadInfo = null;

		// Token: 0x04000F7F RID: 3967
		private Uri _startupUri;

		// Token: 0x04000F80 RID: 3968
		private Uri _applicationMarkupBaseUri;

		// Token: 0x04000F81 RID: 3969
		private HybridDictionary _htProps;

		// Token: 0x04000F82 RID: 3970
		private WindowCollection _appWindowList;

		// Token: 0x04000F83 RID: 3971
		private WindowCollection _nonAppWindowList;

		// Token: 0x04000F84 RID: 3972
		private Window _mainWindow;

		// Token: 0x04000F85 RID: 3973
		private ResourceDictionary _resources;

		// Token: 0x04000F86 RID: 3974
		private bool _ownDispatcherStarted;

		// Token: 0x04000F87 RID: 3975
		private NavigationService _navService;

		// Token: 0x04000F88 RID: 3976
		private SecurityCriticalDataForSet<MimeType> _appMimeType;

		// Token: 0x04000F89 RID: 3977
		private IServiceProvider _serviceProvider;

		// Token: 0x04000F8A RID: 3978
		private bool _appIsShutdown;

		// Token: 0x04000F8B RID: 3979
		private int _exitCode;

		// Token: 0x04000F8C RID: 3980
		private ShutdownMode _shutdownMode;

		// Token: 0x04000F8D RID: 3981
		private HwndWrapper _parkingHwnd;

		// Token: 0x04000F8E RID: 3982
		private HwndWrapperHook _appFilterHook;

		// Token: 0x04000F8F RID: 3983
		private EventHandlerList _events;

		// Token: 0x04000F90 RID: 3984
		private bool _hasImplicitStylesInResources;

		// Token: 0x04000F91 RID: 3985
		private static readonly object EVENT_STARTUP = new object();

		// Token: 0x04000F92 RID: 3986
		private static readonly object EVENT_EXIT = new object();

		// Token: 0x04000F93 RID: 3987
		private static readonly object EVENT_SESSIONENDING = new object();

		// Token: 0x04000F94 RID: 3988
		private const SafeNativeMethods.PlaySoundFlags PLAYSOUND_FLAGS = SafeNativeMethods.PlaySoundFlags.SND_ASYNC | SafeNativeMethods.PlaySoundFlags.SND_NODEFAULT | SafeNativeMethods.PlaySoundFlags.SND_NOSTOP | SafeNativeMethods.PlaySoundFlags.SND_FILENAME;

		// Token: 0x04000F95 RID: 3989
		private const string SYSTEM_SOUNDS_REGISTRY_LOCATION = "AppEvents\\Schemes\\Apps\\Explorer\\{0}\\.current\\";

		// Token: 0x04000F96 RID: 3990
		private const string SYSTEM_SOUNDS_REGISTRY_BASE = "HKEY_CURRENT_USER\\AppEvents\\Schemes\\Apps\\Explorer\\";

		// Token: 0x04000F97 RID: 3991
		private const string SOUND_NAVIGATING = "Navigating";

		// Token: 0x04000F98 RID: 3992
		private const string SOUND_COMPLETE_NAVIGATION = "ActivatingDocument";

		// Token: 0x02000A73 RID: 2675
		internal enum NavigationStateChange : byte
		{
			// Token: 0x0400416A RID: 16746
			Navigating,
			// Token: 0x0400416B RID: 16747
			Completed,
			// Token: 0x0400416C RID: 16748
			Stopped
		}
	}
}
