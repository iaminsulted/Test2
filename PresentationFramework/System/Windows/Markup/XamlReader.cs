using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Baml2006;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xaml;
using System.Xaml.Permissions;
using System.Xml;
using MS.Internal;
using MS.Internal.Utility;
using MS.Internal.WindowsBase;
using MS.Internal.Xaml.Context;
using MS.Utility;
using MS.Win32;

namespace System.Windows.Markup
{
	// Token: 0x0200050D RID: 1293
	public class XamlReader
	{
		// Token: 0x06004030 RID: 16432 RVA: 0x00213014 File Offset: 0x00212014
		public static object Parse(string xamlText)
		{
			return XamlReader.Parse(xamlText, false);
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x0021301D File Offset: 0x0021201D
		public static object Parse(string xamlText, bool useRestrictiveXamlReader)
		{
			return XamlReader.Load(XmlReader.Create(new StringReader(xamlText)), useRestrictiveXamlReader);
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00213030 File Offset: 0x00212030
		public static object Parse(string xamlText, ParserContext parserContext)
		{
			return XamlReader.Parse(xamlText, parserContext, false);
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x0021303A File Offset: 0x0021203A
		public static object Parse(string xamlText, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			return XamlReader.Load(new MemoryStream(Encoding.Default.GetBytes(xamlText)), parserContext, useRestrictiveXamlReader);
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x00213053 File Offset: 0x00212053
		public static object Load(Stream stream)
		{
			return XamlReader.Load(stream, null, false);
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x0021305D File Offset: 0x0021205D
		public static object Load(Stream stream, bool useRestrictiveXamlReader)
		{
			return XamlReader.Load(stream, null, useRestrictiveXamlReader);
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x00213067 File Offset: 0x00212067
		public static object Load(XmlReader reader)
		{
			return XamlReader.Load(reader, false);
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x00213070 File Offset: 0x00212070
		public static object Load(XmlReader reader, bool useRestrictiveXamlReader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return XamlReader.Load(reader, null, XamlParseMode.Synchronous, useRestrictiveXamlReader);
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x00213089 File Offset: 0x00212089
		public static object Load(Stream stream, ParserContext parserContext)
		{
			return XamlReader.Load(stream, parserContext, false);
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00213093 File Offset: 0x00212093
		public static object Load(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			object result = XamlReader.Load(XmlReader.Create(stream, null, parserContext), parserContext, XamlParseMode.Synchronous, useRestrictiveXamlReader);
			stream.Close();
			return result;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x002130C8 File Offset: 0x002120C8
		public object LoadAsync(Stream stream)
		{
			return this.LoadAsync(stream, false);
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x002130D2 File Offset: 0x002120D2
		public object LoadAsync(Stream stream, bool useRestrictiveXamlReader)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = stream;
			if (this._objectWriter != null)
			{
				throw new InvalidOperationException(SR.Get("ParserCannotReuseXamlReader"));
			}
			return this.LoadAsync(stream, null, useRestrictiveXamlReader);
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0021310A File Offset: 0x0021210A
		public object LoadAsync(XmlReader reader)
		{
			return this.LoadAsync(reader, null, false);
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x00213115 File Offset: 0x00212115
		public object LoadAsync(XmlReader reader, bool useRestrictiveXamlReader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return this.LoadAsync(reader, null, useRestrictiveXamlReader);
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0021312E File Offset: 0x0021212E
		public object LoadAsync(Stream stream, ParserContext parserContext)
		{
			return this.LoadAsync(stream, parserContext, false);
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0021313C File Offset: 0x0021213C
		public object LoadAsync(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = stream;
			if (this._objectWriter != null)
			{
				throw new InvalidOperationException(SR.Get("ParserCannotReuseXamlReader"));
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			return this.LoadAsync(new XmlTextReader(stream, XmlNodeType.Document, parserContext)
			{
				DtdProcessing = DtdProcessing.Prohibit
			}, parserContext, useRestrictiveXamlReader);
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x002131A0 File Offset: 0x002121A0
		internal static bool ShouldReWrapException(Exception e, Uri baseUri)
		{
			XamlParseException ex = e as XamlParseException;
			return ex == null || (ex.BaseUri == null && baseUri != null);
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x002131D0 File Offset: 0x002121D0
		private object LoadAsync(XmlReader reader, ParserContext parserContext)
		{
			return this.LoadAsync(reader, parserContext, false);
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x002131DC File Offset: 0x002121DC
		private object LoadAsync(XmlReader reader, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			this._xmlReader = reader;
			object rootObject = null;
			if (parserContext.BaseUri == null || string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
			{
				if (reader.BaseURI == null || string.IsNullOrEmpty(reader.BaseURI.ToString()))
				{
					parserContext.BaseUri = BaseUriHelper.PackAppBaseUri;
				}
				else
				{
					parserContext.BaseUri = new Uri(reader.BaseURI);
				}
			}
			this._baseUri = parserContext.BaseUri;
			XamlXmlReaderSettings xamlXmlReaderSettings = new XamlXmlReaderSettings();
			xamlXmlReaderSettings.IgnoreUidsOnPropertyElements = true;
			xamlXmlReaderSettings.BaseUri = parserContext.BaseUri;
			xamlXmlReaderSettings.ProvideLineInfo = true;
			XamlSchemaContext schemaContext = (parserContext.XamlTypeMapper != null) ? parserContext.XamlTypeMapper.SchemaContext : XamlReader.GetWpfSchemaContext();
			try
			{
				this._textReader = (useRestrictiveXamlReader ? new RestrictiveXamlXmlReader(reader, schemaContext, xamlXmlReaderSettings) : new XamlXmlReader(reader, schemaContext, xamlXmlReaderSettings));
				this._stack = new XamlContextStack<WpfXamlFrame>(() => new WpfXamlFrame());
				XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettings();
				xamlObjectWriterSettings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
				{
					if (rootObject == null)
					{
						rootObject = args.Instance;
						this._styleConnector = (rootObject as IStyleConnector);
					}
					UIElement uielement = args.Instance as UIElement;
					if (uielement != null)
					{
						UIElement uielement2 = uielement;
						XamlReader <>4__this = this;
						int persistId = this._persistId;
						<>4__this._persistId = persistId + 1;
						uielement2.SetPersistId(persistId);
					}
					DependencyObject dependencyObject = args.Instance as DependencyObject;
					if (dependencyObject != null && this._stack.CurrentFrame.XmlnsDictionary != null)
					{
						XmlnsDictionary xmlnsDictionary = this._stack.CurrentFrame.XmlnsDictionary;
						xmlnsDictionary.Seal();
						XmlAttributeProperties.SetXmlnsDictionary(dependencyObject, xmlnsDictionary);
					}
				};
				this._objectWriter = new XamlObjectWriter(this._textReader.SchemaContext, xamlObjectWriterSettings);
				this._parseCancelled = false;
				this._skipJournaledProperties = parserContext.SkipJournaledProperties;
				XamlMember xamlDirective = this._textReader.SchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", "SynchronousMode");
				XamlMember xamlDirective2 = this._textReader.SchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", "AsyncRecords");
				XamlReader textReader = this._textReader;
				IXamlLineInfo xamlLineInfo = textReader as IXamlLineInfo;
				IXamlLineInfoConsumer objectWriter = this._objectWriter;
				bool shouldPassLineNumberInfo = false;
				if (xamlLineInfo != null && xamlLineInfo.HasLineInfo && objectWriter != null && objectWriter.ShouldProvideLineInfo)
				{
					shouldPassLineNumberInfo = true;
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				while (!this._textReader.IsEof)
				{
					WpfXamlLoader.TransformNodes(textReader, this._objectWriter, true, this._skipJournaledProperties, shouldPassLineNumberInfo, xamlLineInfo, objectWriter, this._stack, this._styleConnector);
					if (textReader.NodeType == XamlNodeType.StartMember)
					{
						if (textReader.Member == xamlDirective)
						{
							flag2 = true;
						}
						else if (textReader.Member == xamlDirective2)
						{
							flag3 = true;
						}
					}
					else if (textReader.NodeType == XamlNodeType.Value)
					{
						if (flag2)
						{
							if (textReader.Value as string == "Async")
							{
								flag = true;
							}
						}
						else if (flag3)
						{
							if (textReader.Value is int)
							{
								this._maxAsynxRecords = (int)textReader.Value;
							}
							else if (textReader.Value is string)
							{
								this._maxAsynxRecords = int.Parse(textReader.Value as string, TypeConverterHelper.InvariantEnglishUS);
							}
						}
					}
					else if (textReader.NodeType == XamlNodeType.EndMember)
					{
						flag2 = false;
						flag3 = false;
					}
					if (flag && rootObject != null)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, parserContext.BaseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, parserContext.BaseUri);
			}
			if (!this._textReader.IsEof)
			{
				this.Post();
			}
			else
			{
				this.TreeBuildComplete();
			}
			if (rootObject is DependencyObject)
			{
				if (parserContext.BaseUri != null && !string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
				{
					(rootObject as DependencyObject).SetValue(BaseUriHelper.BaseUriProperty, parserContext.BaseUri);
				}
				WpfXamlLoader.EnsureXmlNamespaceMaps(rootObject, schemaContext);
			}
			Application application = rootObject as Application;
			if (application != null)
			{
				application.ApplicationMarkupBaseUri = XamlReader.GetBaseUri(xamlXmlReaderSettings.BaseUri);
			}
			return rootObject;
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x002135B0 File Offset: 0x002125B0
		internal static void RewrapException(Exception e, Uri baseUri)
		{
			XamlReader.RewrapException(e, null, baseUri);
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x002135BA File Offset: 0x002125BA
		internal static void RewrapException(Exception e, IXamlLineInfo lineInfo, Uri baseUri)
		{
			throw XamlReader.WrapException(e, lineInfo, baseUri);
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x002135C4 File Offset: 0x002125C4
		internal static XamlParseException WrapException(Exception e, IXamlLineInfo lineInfo, Uri baseUri)
		{
			Exception ex = (e.InnerException == null) ? e : e.InnerException;
			if (ex is XamlParseException)
			{
				XamlParseException ex2 = (XamlParseException)ex;
				ex2.BaseUri = (ex2.BaseUri ?? baseUri);
				if (lineInfo != null && ex2.LinePosition == 0 && ex2.LineNumber == 0)
				{
					ex2.LinePosition = lineInfo.LinePosition;
					ex2.LineNumber = lineInfo.LineNumber;
				}
				return ex2;
			}
			if (e is XamlException)
			{
				XamlException ex3 = (XamlException)e;
				return new XamlParseException(ex3.Message, ex3.LineNumber, ex3.LinePosition, baseUri, ex);
			}
			if (e is XmlException)
			{
				XmlException ex4 = (XmlException)e;
				return new XamlParseException(ex4.Message, ex4.LineNumber, ex4.LinePosition, baseUri, ex);
			}
			if (lineInfo != null)
			{
				return new XamlParseException(e.Message, lineInfo.LineNumber, lineInfo.LinePosition, baseUri, ex);
			}
			return new XamlParseException(e.Message, ex);
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x002136AB File Offset: 0x002126AB
		internal void Post()
		{
			this.Post(DispatcherPriority.Background);
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x002136B4 File Offset: 0x002126B4
		internal void Post(DispatcherPriority priority)
		{
			DispatcherOperationCallback method = new DispatcherOperationCallback(this.Dispatch);
			Dispatcher.CurrentDispatcher.BeginInvoke(priority, method, this);
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x002136DC File Offset: 0x002126DC
		private object Dispatch(object o)
		{
			this.DispatchParserQueueEvent((XamlReader)o);
			return null;
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x002136EB File Offset: 0x002126EB
		private void DispatchParserQueueEvent(XamlReader xamlReader)
		{
			xamlReader.HandleAsyncQueueItem();
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x002136F4 File Offset: 0x002126F4
		internal virtual void HandleAsyncQueueItem()
		{
			try
			{
				int num = SafeNativeMethods.GetTickCount();
				int num2 = this._maxAsynxRecords;
				XamlReader textReader = this._textReader;
				IXamlLineInfo xamlLineInfo = textReader as IXamlLineInfo;
				IXamlLineInfoConsumer objectWriter = this._objectWriter;
				bool shouldPassLineNumberInfo = false;
				if (xamlLineInfo != null && xamlLineInfo.HasLineInfo && objectWriter != null && objectWriter.ShouldProvideLineInfo)
				{
					shouldPassLineNumberInfo = true;
				}
				XamlMember xamlDirective = this._textReader.SchemaContext.GetXamlDirective("http://schemas.microsoft.com/winfx/2006/xaml", "AsyncRecords");
				while (!textReader.IsEof && !this._parseCancelled)
				{
					WpfXamlLoader.TransformNodes(textReader, this._objectWriter, true, this._skipJournaledProperties, shouldPassLineNumberInfo, xamlLineInfo, objectWriter, this._stack, this._styleConnector);
					if (textReader.NodeType == XamlNodeType.Value && this._stack.CurrentFrame.Property == xamlDirective)
					{
						if (textReader.Value is int)
						{
							this._maxAsynxRecords = (int)textReader.Value;
						}
						else if (textReader.Value is string)
						{
							this._maxAsynxRecords = int.Parse(textReader.Value as string, TypeConverterHelper.InvariantEnglishUS);
						}
						num2 = this._maxAsynxRecords;
					}
					int num3 = SafeNativeMethods.GetTickCount() - num;
					if (num3 < 0)
					{
						num = 0;
					}
					else if (num3 > 200)
					{
						break;
					}
					if (--num2 == 0)
					{
						break;
					}
				}
			}
			catch (XamlParseException parseException)
			{
				this._parseException = parseException;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, this._baseUri))
				{
					this._parseException = ex;
				}
				else
				{
					this._parseException = XamlReader.WrapException(ex, null, this._baseUri);
				}
			}
			finally
			{
				if (this._parseException != null || this._parseCancelled)
				{
					this.TreeBuildComplete();
				}
				else if (!this._textReader.IsEof)
				{
					this.Post();
				}
				else
				{
					this.TreeBuildComplete();
				}
			}
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x002138F8 File Offset: 0x002128F8
		internal void TreeBuildComplete()
		{
			if (this.LoadCompleted != null)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object obj)
				{
					this.LoadCompleted(this, new AsyncCompletedEventArgs(this._parseException, this._parseCancelled, null));
					return null;
				}), null);
			}
			this._xmlReader.Close();
			this._objectWriter = null;
			this._stream = null;
			this._textReader = null;
			this._stack = null;
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0021394E File Offset: 0x0021294E
		public void CancelAsync()
		{
			this._parseCancelled = true;
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x0600404D RID: 16461 RVA: 0x00213958 File Offset: 0x00212958
		// (remove) Token: 0x0600404E RID: 16462 RVA: 0x00213990 File Offset: 0x00212990
		public event AsyncCompletedEventHandler LoadCompleted;

		// Token: 0x0600404F RID: 16463 RVA: 0x002139C5 File Offset: 0x002129C5
		internal static XamlObjectWriterSettings CreateObjectWriterSettings()
		{
			return new XamlObjectWriterSettings
			{
				IgnoreCanConvert = true,
				PreferUnconvertedDictionaryKeys = true
			};
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x002139DC File Offset: 0x002129DC
		internal static XamlObjectWriterSettings CreateObjectWriterSettings(XamlObjectWriterSettings parentSettings)
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettings();
			if (parentSettings != null)
			{
				xamlObjectWriterSettings.SkipDuplicatePropertyCheck = parentSettings.SkipDuplicatePropertyCheck;
				xamlObjectWriterSettings.AccessLevel = parentSettings.AccessLevel;
				xamlObjectWriterSettings.SkipProvideValueOnRoot = parentSettings.SkipProvideValueOnRoot;
				xamlObjectWriterSettings.SourceBamlUri = parentSettings.SourceBamlUri;
			}
			return xamlObjectWriterSettings;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00213A23 File Offset: 0x00212A23
		internal static XamlObjectWriterSettings CreateObjectWriterSettingsForBaml()
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettings();
			xamlObjectWriterSettings.SkipDuplicatePropertyCheck = true;
			return xamlObjectWriterSettings;
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00213A31 File Offset: 0x00212A31
		internal static Baml2006ReaderSettings CreateBamlReaderSettings()
		{
			return new Baml2006ReaderSettings
			{
				IgnoreUidsOnPropertyElements = true
			};
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x00213A3F File Offset: 0x00212A3F
		internal static XamlSchemaContextSettings CreateSchemaContextSettings()
		{
			return new XamlSchemaContextSettings
			{
				SupportMarkupExtensionsWithDuplicateArity = true
			};
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06004054 RID: 16468 RVA: 0x00213A4D File Offset: 0x00212A4D
		internal static WpfSharedBamlSchemaContext BamlSharedSchemaContext
		{
			get
			{
				return XamlReader._bamlSharedContext.Value;
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x00213A59 File Offset: 0x00212A59
		internal static WpfSharedBamlSchemaContext XamlV3SharedSchemaContext
		{
			get
			{
				return XamlReader._xamlV3SharedContext.Value;
			}
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00213A65 File Offset: 0x00212A65
		public static XamlSchemaContext GetWpfSchemaContext()
		{
			return XamlReader._xamlSharedContext.Value;
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x00213A71 File Offset: 0x00212A71
		internal static object Load(XmlReader reader, ParserContext parserContext, XamlParseMode parseMode)
		{
			return XamlReader.Load(reader, parserContext, parseMode, false);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00213A7C File Offset: 0x00212A7C
		internal static object Load(XmlReader reader, ParserContext parserContext, XamlParseMode parseMode, bool useRestrictiveXamlReader)
		{
			return XamlReader.Load(reader, parserContext, parseMode, useRestrictiveXamlReader, null);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x00213A88 File Offset: 0x00212A88
		internal static object Load(XmlReader reader, ParserContext parserContext, XamlParseMode parseMode, bool useRestrictiveXamlReader, List<Type> safeTypes)
		{
			if (parseMode == XamlParseMode.Uninitialized || parseMode == XamlParseMode.Asynchronous)
			{
				return new XamlReader().LoadAsync(reader, parserContext, useRestrictiveXamlReader);
			}
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXmlBegin, parserContext.BaseUri);
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.Load);
			}
			object obj = null;
			try
			{
				if (parserContext.BaseUri == null || string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
				{
					if (reader.BaseURI == null || string.IsNullOrEmpty(reader.BaseURI.ToString()))
					{
						parserContext.BaseUri = BaseUriHelper.PackAppBaseUri;
					}
					else
					{
						parserContext.BaseUri = new Uri(reader.BaseURI);
					}
				}
				XamlXmlReaderSettings xamlXmlReaderSettings = new XamlXmlReaderSettings();
				xamlXmlReaderSettings.IgnoreUidsOnPropertyElements = true;
				xamlXmlReaderSettings.BaseUri = parserContext.BaseUri;
				xamlXmlReaderSettings.ProvideLineInfo = true;
				XamlSchemaContext schemaContext = (parserContext.XamlTypeMapper != null) ? parserContext.XamlTypeMapper.SchemaContext : XamlReader.GetWpfSchemaContext();
				obj = XamlReader.Load(useRestrictiveXamlReader ? new RestrictiveXamlXmlReader(reader, schemaContext, xamlXmlReaderSettings, safeTypes) : new XamlXmlReader(reader, schemaContext, xamlXmlReaderSettings), parserContext);
				reader.Close();
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, parserContext.BaseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, parserContext.BaseUri);
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, obj);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXmlEnd, parserContext.BaseUri);
			}
			return obj;
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x00213C08 File Offset: 0x00212C08
		internal static object Load(XamlReader xamlReader, ParserContext parserContext)
		{
			if (parserContext == null)
			{
				parserContext = new ParserContext();
			}
			SecurityHelper.RunClassConstructor(typeof(Application));
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXamlBegin, parserContext.BaseUri);
			object obj = WpfXamlLoader.Load(xamlReader, parserContext.SkipJournaledProperties, parserContext.BaseUri);
			DependencyObject dependencyObject = obj as DependencyObject;
			if (dependencyObject != null && parserContext.BaseUri != null && !string.IsNullOrEmpty(parserContext.BaseUri.ToString()))
			{
				dependencyObject.SetValue(BaseUriHelper.BaseUriProperty, parserContext.BaseUri);
			}
			Application application = obj as Application;
			if (application != null)
			{
				application.ApplicationMarkupBaseUri = XamlReader.GetBaseUri(parserContext.BaseUri);
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXamlEnd, parserContext.BaseUri);
			return obj;
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00213CBC File Offset: 0x00212CBC
		public static object Load(XamlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			object obj = null;
			try
			{
				obj = XamlReader.Load(reader, null);
			}
			catch (Exception ex)
			{
				IUriContext uriContext = reader as IUriContext;
				Uri baseUri = (uriContext != null) ? uriContext.BaseUri : null;
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, baseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, baseUri);
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, obj);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseXmlEnd);
			}
			return obj;
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x00213D58 File Offset: 0x00212D58
		internal static object LoadBaml(Stream stream, ParserContext parserContext, object parent, bool closeStream)
		{
			object obj = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseBamlBegin, parserContext.BaseUri);
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.Load);
			}
			try
			{
				IStreamInfo streamInfo = stream as IStreamInfo;
				if (streamInfo != null)
				{
					parserContext.StreamCreatedAssembly = streamInfo.Assembly;
				}
				Baml2006ReaderSettings baml2006ReaderSettings = XamlReader.CreateBamlReaderSettings();
				baml2006ReaderSettings.BaseUri = parserContext.BaseUri;
				baml2006ReaderSettings.LocalAssembly = streamInfo.Assembly;
				if (baml2006ReaderSettings.BaseUri == null || string.IsNullOrEmpty(baml2006ReaderSettings.BaseUri.ToString()))
				{
					baml2006ReaderSettings.BaseUri = BaseUriHelper.PackAppBaseUri;
				}
				Baml2006ReaderInternal xamlReader = new Baml2006ReaderInternal(stream, new Baml2006SchemaContext(baml2006ReaderSettings.LocalAssembly), baml2006ReaderSettings, parent);
				Type left = null;
				if (streamInfo.Assembly != null)
				{
					try
					{
						left = XamlTypeMapper.GetInternalTypeHelperTypeFromAssembly(parserContext);
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalException(ex))
						{
							throw;
						}
					}
				}
				if (left != null)
				{
					XamlAccessLevel accessLevel = XamlAccessLevel.AssemblyAccessTo(streamInfo.Assembly);
					obj = WpfXamlLoader.LoadBaml(xamlReader, parserContext.SkipJournaledProperties, parent, accessLevel, parserContext.BaseUri);
				}
				else
				{
					obj = WpfXamlLoader.LoadBaml(xamlReader, parserContext.SkipJournaledProperties, parent, null, parserContext.BaseUri);
				}
				DependencyObject dependencyObject = obj as DependencyObject;
				if (dependencyObject != null)
				{
					dependencyObject.SetValue(BaseUriHelper.BaseUriProperty, baml2006ReaderSettings.BaseUri);
				}
				Application application = obj as Application;
				if (application != null)
				{
					application.ApplicationMarkupBaseUri = XamlReader.GetBaseUri(baml2006ReaderSettings.BaseUri);
				}
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, obj);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Event.WClientParseBamlEnd, parserContext.BaseUri);
				if (closeStream && stream != null)
				{
					stream.Close();
				}
			}
			return obj;
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x00213F18 File Offset: 0x00212F18
		private static Uri GetBaseUri(Uri uri)
		{
			if (uri == null)
			{
				return BindUriHelper.BaseUri;
			}
			if (!uri.IsAbsoluteUri)
			{
				return new Uri(BindUriHelper.BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x00213F3E File Offset: 0x00212F3E
		private static WpfSharedBamlSchemaContext CreateBamlSchemaContext()
		{
			return new WpfSharedBamlSchemaContext(new XamlSchemaContextSettings
			{
				SupportMarkupExtensionsWithDuplicateArity = true
			});
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x00213F51 File Offset: 0x00212F51
		private static WpfSharedXamlSchemaContext CreateXamlSchemaContext(bool useV3Rules)
		{
			return new WpfSharedXamlSchemaContext(new XamlSchemaContextSettings
			{
				SupportMarkupExtensionsWithDuplicateArity = true
			}, useV3Rules);
		}

		// Token: 0x040023FF RID: 9215
		private const int AsyncLoopTimeout = 200;

		// Token: 0x04002401 RID: 9217
		private Uri _baseUri;

		// Token: 0x04002402 RID: 9218
		private XamlReader _textReader;

		// Token: 0x04002403 RID: 9219
		private XmlReader _xmlReader;

		// Token: 0x04002404 RID: 9220
		private XamlObjectWriter _objectWriter;

		// Token: 0x04002405 RID: 9221
		private Stream _stream;

		// Token: 0x04002406 RID: 9222
		private bool _parseCancelled;

		// Token: 0x04002407 RID: 9223
		private Exception _parseException;

		// Token: 0x04002408 RID: 9224
		private int _persistId = 1;

		// Token: 0x04002409 RID: 9225
		private bool _skipJournaledProperties;

		// Token: 0x0400240A RID: 9226
		private XamlContextStack<WpfXamlFrame> _stack;

		// Token: 0x0400240B RID: 9227
		private int _maxAsynxRecords = -1;

		// Token: 0x0400240C RID: 9228
		private IStyleConnector _styleConnector;

		// Token: 0x0400240D RID: 9229
		private static readonly Lazy<WpfSharedBamlSchemaContext> _bamlSharedContext = new Lazy<WpfSharedBamlSchemaContext>(() => XamlReader.CreateBamlSchemaContext());

		// Token: 0x0400240E RID: 9230
		private static readonly Lazy<WpfSharedXamlSchemaContext> _xamlSharedContext = new Lazy<WpfSharedXamlSchemaContext>(() => XamlReader.CreateXamlSchemaContext(false));

		// Token: 0x0400240F RID: 9231
		private static readonly Lazy<WpfSharedXamlSchemaContext> _xamlV3SharedContext = new Lazy<WpfSharedXamlSchemaContext>(() => XamlReader.CreateXamlSchemaContext(true));
	}
}
