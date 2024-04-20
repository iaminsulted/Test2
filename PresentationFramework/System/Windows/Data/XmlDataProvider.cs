using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Utility;

namespace System.Windows.Data
{
	// Token: 0x0200046C RID: 1132
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	[ContentProperty("XmlSerializer")]
	public class XmlDataProvider : DataSourceProvider, IUriContext
	{
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06003A32 RID: 14898 RVA: 0x001EFC10 File Offset: 0x001EEC10
		// (set) Token: 0x06003A33 RID: 14899 RVA: 0x001EFC18 File Offset: 0x001EEC18
		public Uri Source
		{
			get
			{
				return this._source;
			}
			set
			{
				if (this._domSetDocument != null || this._source != value)
				{
					this._domSetDocument = null;
					this._source = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Source"));
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x001EFC67 File Offset: 0x001EEC67
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSource()
		{
			return this._domSetDocument == null && this._source != null;
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06003A35 RID: 14901 RVA: 0x001EFC7F File Offset: 0x001EEC7F
		// (set) Token: 0x06003A36 RID: 14902 RVA: 0x001EFC88 File Offset: 0x001EEC88
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public XmlDocument Document
		{
			get
			{
				return this._document;
			}
			set
			{
				if (this._domSetDocument == null || this._source != null || this._document != value)
				{
					this._domSetDocument = value;
					this._source = null;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Source"));
					this.ChangeDocument(value);
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06003A37 RID: 14903 RVA: 0x001EFCE7 File Offset: 0x001EECE7
		// (set) Token: 0x06003A38 RID: 14904 RVA: 0x001EFCEF File Offset: 0x001EECEF
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		public string XPath
		{
			get
			{
				return this._xPath;
			}
			set
			{
				if (this._xPath != value)
				{
					this._xPath = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("XPath"));
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x001EFD24 File Offset: 0x001EED24
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeXPath()
		{
			return this._xPath != null && this._xPath.Length != 0;
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x001EFD3E File Offset: 0x001EED3E
		// (set) Token: 0x06003A3B RID: 14907 RVA: 0x001EFD46 File Offset: 0x001EED46
		[DefaultValue(null)]
		public XmlNamespaceManager XmlNamespaceManager
		{
			get
			{
				return this._nsMgr;
			}
			set
			{
				if (this._nsMgr != value)
				{
					this._nsMgr = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("XmlNamespaceManager"));
					if (!base.IsRefreshDeferred)
					{
						base.Refresh();
					}
				}
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06003A3C RID: 14908 RVA: 0x001EFD76 File Offset: 0x001EED76
		// (set) Token: 0x06003A3D RID: 14909 RVA: 0x001EFD7E File Offset: 0x001EED7E
		[DefaultValue(true)]
		public bool IsAsynchronous
		{
			get
			{
				return this._isAsynchronous;
			}
			set
			{
				this._isAsynchronous = value;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06003A3E RID: 14910 RVA: 0x001EFD87 File Offset: 0x001EED87
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public IXmlSerializable XmlSerializer
		{
			get
			{
				if (this._xmlSerializer == null)
				{
					this._xmlSerializer = new XmlDataProvider.XmlIslandSerializer(this);
				}
				return this._xmlSerializer;
			}
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x001EFDA3 File Offset: 0x001EEDA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeXmlSerializer()
		{
			return this.DocumentForSerialization != null;
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x001EFDAE File Offset: 0x001EEDAE
		// (set) Token: 0x06003A41 RID: 14913 RVA: 0x001EFDB6 File Offset: 0x001EEDB6
		Uri IUriContext.BaseUri
		{
			get
			{
				return this.BaseUri;
			}
			set
			{
				this.BaseUri = value;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x001EFDBF File Offset: 0x001EEDBF
		// (set) Token: 0x06003A43 RID: 14915 RVA: 0x001EFDC7 File Offset: 0x001EEDC7
		protected virtual Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				this._baseUri = value;
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x001EFDD0 File Offset: 0x001EEDD0
		protected override void BeginQuery()
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.BeginQuery(new object[]
				{
					TraceData.Identify(this),
					this.IsAsynchronous ? "asynchronous" : "synchronous"
				}), null);
			}
			if (this._source != null)
			{
				this.DiscardInline();
				this.LoadFromSource();
				return;
			}
			XmlDocument xmlDocument;
			if (this._domSetDocument != null)
			{
				this.DiscardInline();
				xmlDocument = this._domSetDocument;
			}
			else
			{
				if (this._inEndInit)
				{
					return;
				}
				xmlDocument = this._savedDocument;
			}
			if (this.IsAsynchronous && xmlDocument != null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.BuildNodeCollectionAsynch), xmlDocument);
				return;
			}
			if (xmlDocument != null || base.Data != null)
			{
				this.BuildNodeCollection(xmlDocument);
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x001EFE90 File Offset: 0x001EEE90
		protected override void EndInit()
		{
			try
			{
				this._inEndInit = true;
				base.EndInit();
			}
			finally
			{
				this._inEndInit = false;
			}
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x001EFEC4 File Offset: 0x001EEEC4
		private void LoadFromSource()
		{
			Uri uri = this.Source;
			if (!uri.IsAbsoluteUri)
			{
				uri = BindUriHelper.GetResolvedUri((this._baseUri != null) ? this._baseUri : BindUriHelper.BaseUri, uri);
			}
			WebRequest webRequest = PackWebRequestFactory.CreateWebRequest(uri);
			if (webRequest == null)
			{
				throw new Exception(SR.Get("WebRequestCreationFailed"));
			}
			if (this.IsAsynchronous)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreateDocFromExternalSourceAsynch), webRequest);
				return;
			}
			this.CreateDocFromExternalSource(webRequest);
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x001EFF40 File Offset: 0x001EEF40
		private void ParseInline(XmlReader xmlReader)
		{
			if (this._source == null && this._domSetDocument == null && this._tryInlineDoc)
			{
				if (this.IsAsynchronous)
				{
					this._waitForInlineDoc = new ManualResetEvent(false);
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.CreateDocFromInlineXmlAsync), xmlReader);
					return;
				}
				this.CreateDocFromInlineXml(xmlReader);
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06003A48 RID: 14920 RVA: 0x001EFF9A File Offset: 0x001EEF9A
		private XmlDocument DocumentForSerialization
		{
			get
			{
				if (this._tryInlineDoc || this._savedDocument != null || this._domSetDocument != null)
				{
					if (this._waitForInlineDoc != null)
					{
						this._waitForInlineDoc.WaitOne();
					}
					return this._document;
				}
				return null;
			}
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x001EFFD0 File Offset: 0x001EEFD0
		private void CreateDocFromInlineXmlAsync(object arg)
		{
			XmlReader xmlReader = (XmlReader)arg;
			this.CreateDocFromInlineXml(xmlReader);
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x001EFFEC File Offset: 0x001EEFEC
		private void CreateDocFromInlineXml(XmlReader xmlReader)
		{
			if (!this._tryInlineDoc)
			{
				this._savedDocument = null;
				if (this._waitForInlineDoc != null)
				{
					this._waitForInlineDoc.Set();
				}
				return;
			}
			Exception ex = null;
			XmlDocument xmlDocument;
			try
			{
				xmlDocument = new XmlDocument();
				try
				{
					if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
					{
						TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.XmlLoadInline(new object[]
						{
							TraceData.Identify(this),
							base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous"
						}), null);
					}
					xmlDocument.Load(xmlReader);
				}
				catch (XmlException ex2)
				{
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.XmlDPInlineDocError, ex2);
					}
					ex = ex2;
				}
				if (ex == null)
				{
					this._savedDocument = (XmlDocument)xmlDocument.Clone();
				}
			}
			finally
			{
				xmlReader.Close();
				if (this._waitForInlineDoc != null)
				{
					this._waitForInlineDoc.Set();
				}
			}
			if (TraceData.IsEnabled)
			{
				XmlNode documentElement = xmlDocument.DocumentElement;
				if (documentElement != null && documentElement.NamespaceURI == xmlReader.LookupNamespace(string.Empty))
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.XmlNamespaceNotSet, null);
				}
			}
			if (ex == null)
			{
				this.BuildNodeCollection(xmlDocument);
				return;
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.QueryFinished(new object[]
				{
					TraceData.Identify(this),
					base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
					TraceData.Identify(null),
					TraceData.IdentifyException(ex)
				}), ex);
			}
			this.OnQueryFinished(null, ex, this.CompletedCallback, null);
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x001F0174 File Offset: 0x001EF174
		private void CreateDocFromExternalSourceAsynch(object arg)
		{
			WebRequest request = (WebRequest)arg;
			this.CreateDocFromExternalSource(request);
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x001F0190 File Offset: 0x001EF190
		private void CreateDocFromExternalSource(WebRequest request)
		{
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
			XmlDocument xmlDocument = new XmlDocument();
			Exception ex = null;
			try
			{
				if (flag)
				{
					TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.XmlLoadSource(new object[]
					{
						TraceData.Identify(this),
						base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
						TraceData.Identify(request.RequestUri.ToString())
					}), null);
				}
				WebResponse response = WpfWebRequestHelper.GetResponse(request);
				if (response == null)
				{
					throw new InvalidOperationException(SR.Get("GetResponseFailed"));
				}
				Stream responseStream = response.GetResponseStream();
				if (flag)
				{
					TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.XmlLoadDoc(new object[]
					{
						TraceData.Identify(this)
					}), null);
				}
				xmlDocument.Load(responseStream);
				responseStream.Close();
			}
			catch (Exception ex2)
			{
				if (CriticalExceptions.IsCriticalException(ex2))
				{
					throw;
				}
				ex = ex2;
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.XmlDPAsyncDocError, null, new object[]
					{
						this.Source,
						ex
					}, new object[]
					{
						ex
					});
				}
			}
			catch
			{
				throw;
			}
			if (ex != null)
			{
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.QueryFinished(new object[]
					{
						TraceData.Identify(this),
						base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
						TraceData.Identify(null),
						TraceData.IdentifyException(ex)
					}), ex);
				}
				this.OnQueryFinished(null, ex, this.CompletedCallback, null);
				return;
			}
			this.BuildNodeCollection(xmlDocument);
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x001F0310 File Offset: 0x001EF310
		private void BuildNodeCollectionAsynch(object arg)
		{
			XmlDocument doc = (XmlDocument)arg;
			this.BuildNodeCollection(doc);
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x001F032C File Offset: 0x001EF32C
		private void BuildNodeCollection(XmlDocument doc)
		{
			XmlDataCollection xmlDataCollection = null;
			if (doc != null)
			{
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
				{
					TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.XmlBuildCollection(new object[]
					{
						TraceData.Identify(this)
					}), null);
				}
				XmlNodeList resultNodeList = this.GetResultNodeList(doc);
				xmlDataCollection = new XmlDataCollection(this);
				if (resultNodeList != null)
				{
					xmlDataCollection.SynchronizeCollection(resultNodeList);
				}
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.QueryFinished(new object[]
				{
					TraceData.Identify(this),
					base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
					TraceData.Identify(xmlDataCollection),
					TraceData.IdentifyException(null)
				}), null);
			}
			this.OnQueryFinished(xmlDataCollection, null, this.CompletedCallback, doc);
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x001F03DC File Offset: 0x001EF3DC
		private object OnCompletedCallback(object arg)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.QueryResult(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(base.Data)
				}), null);
			}
			this.ChangeDocument((XmlDocument)arg);
			return null;
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x001F0428 File Offset: 0x001EF428
		private void ChangeDocument(XmlDocument doc)
		{
			if (this._document != doc)
			{
				if (this._document != null)
				{
					this.UnHook();
				}
				this._document = doc;
				if (this._document != null)
				{
					this.Hook();
				}
				this.OnPropertyChanged(new PropertyChangedEventArgs("Document"));
			}
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x001F0466 File Offset: 0x001EF466
		private void DiscardInline()
		{
			this._tryInlineDoc = false;
			this._savedDocument = null;
			if (this._waitForInlineDoc != null)
			{
				this._waitForInlineDoc.Set();
			}
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x001F048C File Offset: 0x001EF48C
		private void Hook()
		{
			if (!this._isListening)
			{
				this._document.NodeInserted += this.NodeChangeHandler;
				this._document.NodeRemoved += this.NodeChangeHandler;
				this._document.NodeChanged += this.NodeChangeHandler;
				this._isListening = true;
			}
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x001F04DC File Offset: 0x001EF4DC
		private void UnHook()
		{
			if (this._isListening)
			{
				this._document.NodeInserted -= this.NodeChangeHandler;
				this._document.NodeRemoved -= this.NodeChangeHandler;
				this._document.NodeChanged -= this.NodeChangeHandler;
				this._isListening = false;
			}
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x001F052C File Offset: 0x001EF52C
		private void OnNodeChanged(object sender, XmlNodeChangedEventArgs e)
		{
			if (this.XmlDataCollection == null)
			{
				return;
			}
			this.UnHook();
			XmlNodeList resultNodeList = this.GetResultNodeList((XmlDocument)sender);
			this.XmlDataCollection.SynchronizeCollection(resultNodeList);
			this.Hook();
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x001F0568 File Offset: 0x001EF568
		private XmlNodeList GetResultNodeList(XmlDocument doc)
		{
			XmlNodeList result = null;
			if (doc.DocumentElement != null)
			{
				string text = string.IsNullOrEmpty(this.XPath) ? "/" : this.XPath;
				try
				{
					if (this.XmlNamespaceManager != null)
					{
						result = doc.SelectNodes(text, this.XmlNamespaceManager);
					}
					else
					{
						result = doc.SelectNodes(text);
					}
				}
				catch (XPathException ex)
				{
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.XmlDPSelectNodesFailed, null, new object[]
						{
							text,
							ex
						}, new object[]
						{
							ex
						});
					}
				}
			}
			return result;
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06003A56 RID: 14934 RVA: 0x001F05FC File Offset: 0x001EF5FC
		private XmlDataCollection XmlDataCollection
		{
			get
			{
				return (XmlDataCollection)base.Data;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06003A57 RID: 14935 RVA: 0x001F0609 File Offset: 0x001EF609
		private DispatcherOperationCallback CompletedCallback
		{
			get
			{
				if (this._onCompletedCallback == null)
				{
					this._onCompletedCallback = new DispatcherOperationCallback(this.OnCompletedCallback);
				}
				return this._onCompletedCallback;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x001F062B File Offset: 0x001EF62B
		private XmlNodeChangedEventHandler NodeChangeHandler
		{
			get
			{
				if (this._nodeChangedHandler == null)
				{
					this._nodeChangedHandler = new XmlNodeChangedEventHandler(this.OnNodeChanged);
				}
				return this._nodeChangedHandler;
			}
		}

		// Token: 0x04001D9A RID: 7578
		private XmlDocument _document;

		// Token: 0x04001D9B RID: 7579
		private XmlDocument _domSetDocument;

		// Token: 0x04001D9C RID: 7580
		private XmlDocument _savedDocument;

		// Token: 0x04001D9D RID: 7581
		private ManualResetEvent _waitForInlineDoc;

		// Token: 0x04001D9E RID: 7582
		private XmlNamespaceManager _nsMgr;

		// Token: 0x04001D9F RID: 7583
		private Uri _source;

		// Token: 0x04001DA0 RID: 7584
		private Uri _baseUri;

		// Token: 0x04001DA1 RID: 7585
		private string _xPath = string.Empty;

		// Token: 0x04001DA2 RID: 7586
		private bool _tryInlineDoc = true;

		// Token: 0x04001DA3 RID: 7587
		private bool _isListening;

		// Token: 0x04001DA4 RID: 7588
		private XmlDataProvider.XmlIslandSerializer _xmlSerializer;

		// Token: 0x04001DA5 RID: 7589
		private bool _isAsynchronous = true;

		// Token: 0x04001DA6 RID: 7590
		private bool _inEndInit;

		// Token: 0x04001DA7 RID: 7591
		private DispatcherOperationCallback _onCompletedCallback;

		// Token: 0x04001DA8 RID: 7592
		private XmlNodeChangedEventHandler _nodeChangedHandler;

		// Token: 0x02000AEB RID: 2795
		private class XmlIslandSerializer : IXmlSerializable
		{
			// Token: 0x06008B54 RID: 35668 RVA: 0x00339C6A File Offset: 0x00338C6A
			internal XmlIslandSerializer(XmlDataProvider host)
			{
				this._host = host;
			}

			// Token: 0x06008B55 RID: 35669 RVA: 0x00109403 File Offset: 0x00108403
			public XmlSchema GetSchema()
			{
				return null;
			}

			// Token: 0x06008B56 RID: 35670 RVA: 0x00339C7C File Offset: 0x00338C7C
			public void WriteXml(XmlWriter writer)
			{
				XmlDocument documentForSerialization = this._host.DocumentForSerialization;
				if (documentForSerialization != null)
				{
					documentForSerialization.Save(writer);
				}
			}

			// Token: 0x06008B57 RID: 35671 RVA: 0x00339C9F File Offset: 0x00338C9F
			public void ReadXml(XmlReader reader)
			{
				this._host.ParseInline(reader);
			}

			// Token: 0x04004729 RID: 18217
			private XmlDataProvider _host;
		}
	}
}
