using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x02000295 RID: 661
	internal class ResourceContainer : Package
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x001622A0 File Offset: 0x001612A0
		internal static ResourceManagerWrapper ApplicationResourceManagerWrapper
		{
			get
			{
				if (ResourceContainer._applicationResourceManagerWrapper == null)
				{
					Assembly resourceAssembly = Application.ResourceAssembly;
					if (resourceAssembly != null)
					{
						ResourceContainer._applicationResourceManagerWrapper = new ResourceManagerWrapper(resourceAssembly);
					}
				}
				return ResourceContainer._applicationResourceManagerWrapper;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x001622D3 File Offset: 0x001612D3
		internal static FileShare FileShare
		{
			get
			{
				return ResourceContainer._fileShare;
			}
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x001622DA File Offset: 0x001612DA
		internal ResourceContainer() : base(FileAccess.Read)
		{
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool PartExists(Uri uri)
		{
			return true;
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x001622E4 File Offset: 0x001612E4
		protected override PackagePart GetPartCore(Uri uri)
		{
			if (!ResourceContainer.assemblyLoadhandlerAttached)
			{
				AppDomain.CurrentDomain.AssemblyLoad += this.OnAssemblyLoadEventHandler;
				ResourceContainer.assemblyLoadhandlerAttached = true;
			}
			string resourceIDFromRelativePath;
			bool flag;
			ResourceManagerWrapper resourceManagerWrapper = this.GetResourceManagerWrapper(uri, out resourceIDFromRelativePath, out flag);
			if (flag)
			{
				return new ContentFilePart(this, uri);
			}
			resourceIDFromRelativePath = ResourceIDHelper.GetResourceIDFromRelativePath(resourceIDFromRelativePath);
			return new ResourcePart(this, uri, resourceIDFromRelativePath, resourceManagerWrapper);
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0016233C File Offset: 0x0016133C
		private void OnAssemblyLoadEventHandler(object sender, AssemblyLoadEventArgs args)
		{
			Assembly loadedAssembly = args.LoadedAssembly;
			if (!loadedAssembly.ReflectionOnly && !loadedAssembly.GlobalAssemblyCache)
			{
				AssemblyName assemblyName = new AssemblyName(loadedAssembly.FullName);
				string text = assemblyName.Name.ToLowerInvariant();
				string text2 = string.Empty;
				string text3 = text;
				this.UpdateCachedRMW(text3, args.LoadedAssembly);
				string text4 = assemblyName.Version.ToString();
				if (!string.IsNullOrEmpty(text4))
				{
					text3 += text4;
					this.UpdateCachedRMW(text3, args.LoadedAssembly);
				}
				byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
				for (int i = 0; i < publicKeyToken.Length; i++)
				{
					text2 += publicKeyToken[i].ToString("x", NumberFormatInfo.InvariantInfo);
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text3 += text2;
					this.UpdateCachedRMW(text3, args.LoadedAssembly);
					text3 = text + text2;
					this.UpdateCachedRMW(text3, args.LoadedAssembly);
				}
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00162429 File Offset: 0x00161429
		private void UpdateCachedRMW(string key, Assembly assembly)
		{
			if (ResourceContainer._registeredResourceManagers.ContainsKey(key))
			{
				ResourceContainer._registeredResourceManagers[key].Assembly = assembly;
			}
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0016244C File Offset: 0x0016144C
		private ResourceManagerWrapper GetResourceManagerWrapper(Uri uri, out string partName, out bool isContentFile)
		{
			ResourceManagerWrapper resourceManagerWrapper = ResourceContainer.ApplicationResourceManagerWrapper;
			isContentFile = false;
			string text;
			string text2;
			string text3;
			BaseUriHelper.GetAssemblyNameAndPart(uri, out partName, out text, out text2, out text3);
			if (!string.IsNullOrEmpty(text))
			{
				string text4 = text + text2 + text3;
				ResourceContainer._registeredResourceManagers.TryGetValue(text4.ToLowerInvariant(), out resourceManagerWrapper);
				if (resourceManagerWrapper == null)
				{
					Assembly loadedAssembly = BaseUriHelper.GetLoadedAssembly(text, text2, text3);
					if (loadedAssembly.Equals(Application.ResourceAssembly))
					{
						resourceManagerWrapper = ResourceContainer.ApplicationResourceManagerWrapper;
					}
					else
					{
						resourceManagerWrapper = new ResourceManagerWrapper(loadedAssembly);
					}
					ResourceContainer._registeredResourceManagers[text4.ToLowerInvariant()] = resourceManagerWrapper;
				}
			}
			if (resourceManagerWrapper == ResourceContainer.ApplicationResourceManagerWrapper)
			{
				if (resourceManagerWrapper == null)
				{
					throw new IOException(SR.Get("EntryAssemblyIsNull"));
				}
				if (ContentFileHelper.IsContentFile(partName))
				{
					isContentFile = true;
					resourceManagerWrapper = null;
				}
			}
			return resourceManagerWrapper;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00109403 File Offset: 0x00108403
		protected override PackagePart CreatePartCore(Uri uri, string contentType, CompressionOption compressionOption)
		{
			return null;
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0012F160 File Offset: 0x0012E160
		protected override void DeletePartCore(Uri uri)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0012F160 File Offset: 0x0012E160
		protected override PackagePart[] GetPartsCore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0012F160 File Offset: 0x0012E160
		protected override void FlushCore()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000D73 RID: 3443
		internal const string XamlExt = ".xaml";

		// Token: 0x04000D74 RID: 3444
		internal const string BamlExt = ".baml";

		// Token: 0x04000D75 RID: 3445
		private static Dictionary<string, ResourceManagerWrapper> _registeredResourceManagers = new Dictionary<string, ResourceManagerWrapper>();

		// Token: 0x04000D76 RID: 3446
		private static ResourceManagerWrapper _applicationResourceManagerWrapper = null;

		// Token: 0x04000D77 RID: 3447
		private static FileShare _fileShare = FileShare.Read;

		// Token: 0x04000D78 RID: 3448
		private static bool assemblyLoadhandlerAttached = false;
	}
}
