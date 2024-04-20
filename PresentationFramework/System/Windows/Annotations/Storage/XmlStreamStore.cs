using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using System.Xml.XPath;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Storage;
using MS.Utility;

namespace System.Windows.Annotations.Storage
{
	// Token: 0x02000878 RID: 2168
	public sealed class XmlStreamStore : AnnotationStore
	{
		// Token: 0x06007FDD RID: 32733 RVA: 0x00320908 File Offset: 0x0031F908
		static XmlStreamStore()
		{
			XmlStreamStore._predefinedNamespaces = new Dictionary<Uri, IList<Uri>>(6);
			XmlStreamStore._predefinedNamespaces.Add(new Uri("http://schemas.microsoft.com/windows/annotations/2003/11/core"), null);
			XmlStreamStore._predefinedNamespaces.Add(new Uri("http://schemas.microsoft.com/windows/annotations/2003/11/base"), null);
			XmlStreamStore._predefinedNamespaces.Add(new Uri("http://schemas.microsoft.com/winfx/2006/xaml/presentation"), null);
		}

		// Token: 0x06007FDE RID: 32734 RVA: 0x00320974 File Offset: 0x0031F974
		public XmlStreamStore(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException(SR.Get("StreamDoesNotSupportSeek"));
			}
			this.SetStream(stream, null);
		}

		// Token: 0x06007FDF RID: 32735 RVA: 0x003209C0 File Offset: 0x0031F9C0
		public XmlStreamStore(Stream stream, IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.SetStream(stream, knownNamespaces);
		}

		// Token: 0x06007FE0 RID: 32736 RVA: 0x003209EC File Offset: 0x0031F9EC
		public override void AddAnnotation(Annotation newAnnotation)
		{
			if (newAnnotation == null)
			{
				throw new ArgumentNullException("newAnnotation");
			}
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAnnotationBegin);
				try
				{
					this.CheckStatus();
					if (this.GetAnnotationNodeForId(newAnnotation.Id) != null)
					{
						throw new ArgumentException(SR.Get("AnnotationAlreadyExists"), "newAnnotation");
					}
					if (this._storeAnnotationsMap.FindAnnotation(newAnnotation.Id) != null)
					{
						throw new ArgumentException(SR.Get("AnnotationAlreadyExists"), "newAnnotation");
					}
					this._storeAnnotationsMap.AddAnnotation(newAnnotation, true);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAnnotationEnd);
				}
			}
			this.OnStoreContentChanged(new StoreContentChangedEventArgs(StoreContentAction.Added, newAnnotation));
		}

		// Token: 0x06007FE1 RID: 32737 RVA: 0x00320AC4 File Offset: 0x0031FAC4
		public override Annotation DeleteAnnotation(Guid annotationId)
		{
			Annotation annotation = null;
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteAnnotationBegin);
				try
				{
					this.CheckStatus();
					annotation = this._storeAnnotationsMap.FindAnnotation(annotationId);
					XPathNavigator annotationNodeForId = this.GetAnnotationNodeForId(annotationId);
					if (annotationNodeForId != null)
					{
						if (annotation == null)
						{
							annotation = (Annotation)XmlStreamStore._serializer.Deserialize(annotationNodeForId.ReadSubtree());
						}
						annotationNodeForId.DeleteSelf();
					}
					this._storeAnnotationsMap.RemoveAnnotation(annotationId);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeleteAnnotationEnd);
				}
			}
			if (annotation != null)
			{
				this.OnStoreContentChanged(new StoreContentChangedEventArgs(StoreContentAction.Deleted, annotation));
			}
			return annotation;
		}

		// Token: 0x06007FE2 RID: 32738 RVA: 0x00320B80 File Offset: 0x0031FB80
		public override IList<Annotation> GetAnnotations(ContentLocator anchorLocator)
		{
			if (anchorLocator == null)
			{
				throw new ArgumentNullException("anchorLocator");
			}
			if (anchorLocator.Parts == null)
			{
				throw new ArgumentNullException("anchorLocator.Parts");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByLocBegin);
			IList<Annotation> result = null;
			try
			{
				string text = "//anc:ContentLocator";
				if (anchorLocator.Parts.Count > 0)
				{
					text += "/child::*[1]/self::";
					for (int i = 0; i < anchorLocator.Parts.Count; i++)
					{
						if (anchorLocator.Parts[i] != null)
						{
							if (i > 0)
							{
								text += "/following-sibling::";
							}
							string queryFragment = anchorLocator.Parts[i].GetQueryFragment(this._namespaceManager);
							if (queryFragment != null)
							{
								text += queryFragment;
							}
							else
							{
								text += "*";
							}
						}
					}
				}
				text += "/ancestor::anc:Anchors/ancestor::anc:Annotation";
				result = this.InternalGetAnnotations(text, anchorLocator);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByLocEnd);
			}
			return result;
		}

		// Token: 0x06007FE3 RID: 32739 RVA: 0x00320C78 File Offset: 0x0031FC78
		public override IList<Annotation> GetAnnotations()
		{
			IList<Annotation> result = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationsBegin);
			try
			{
				string query = "//anc:Annotation";
				result = this.InternalGetAnnotations(query, null);
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationsEnd);
			}
			return result;
		}

		// Token: 0x06007FE4 RID: 32740 RVA: 0x00320CC4 File Offset: 0x0031FCC4
		public override Annotation GetAnnotation(Guid annotationId)
		{
			object syncRoot = base.SyncRoot;
			Annotation result;
			lock (syncRoot)
			{
				Annotation annotation = null;
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByIdBegin);
				try
				{
					this.CheckStatus();
					annotation = this._storeAnnotationsMap.FindAnnotation(annotationId);
					if (annotation != null)
					{
						return annotation;
					}
					XPathNavigator annotationNodeForId = this.GetAnnotationNodeForId(annotationId);
					if (annotationNodeForId != null)
					{
						annotation = (Annotation)XmlStreamStore._serializer.Deserialize(annotationNodeForId.ReadSubtree());
						this._storeAnnotationsMap.AddAnnotation(annotation, false);
					}
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.GetAnnotationByIdEnd);
				}
				result = annotation;
			}
			return result;
		}

		// Token: 0x06007FE5 RID: 32741 RVA: 0x00320D78 File Offset: 0x0031FD78
		public override void Flush()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this.CheckStatus();
				if (!this._stream.CanWrite)
				{
					throw new UnauthorizedAccessException(SR.Get("StreamCannotBeWritten"));
				}
				if (this._dirty)
				{
					this.SerializeAnnotations();
					this._stream.Position = 0L;
					this._stream.SetLength(0L);
					this._document.PreserveWhitespace = true;
					this._document.Save(this._stream);
					this._stream.Flush();
					this._dirty = false;
				}
			}
		}

		// Token: 0x06007FE6 RID: 32742 RVA: 0x00320E2C File Offset: 0x0031FE2C
		public static IList<Uri> GetWellKnownCompatibleNamespaces(Uri name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (XmlStreamStore._predefinedNamespaces.ContainsKey(name))
			{
				return XmlStreamStore._predefinedNamespaces[name];
			}
			return null;
		}

		// Token: 0x17001D75 RID: 7541
		// (get) Token: 0x06007FE7 RID: 32743 RVA: 0x00320E5C File Offset: 0x0031FE5C
		// (set) Token: 0x06007FE8 RID: 32744 RVA: 0x00320EA0 File Offset: 0x0031FEA0
		public override bool AutoFlush
		{
			get
			{
				object syncRoot = base.SyncRoot;
				bool autoFlush;
				lock (syncRoot)
				{
					autoFlush = this._autoFlush;
				}
				return autoFlush;
			}
			set
			{
				object syncRoot = base.SyncRoot;
				lock (syncRoot)
				{
					this._autoFlush = value;
					if (this._autoFlush)
					{
						this.Flush();
					}
				}
			}
		}

		// Token: 0x17001D76 RID: 7542
		// (get) Token: 0x06007FE9 RID: 32745 RVA: 0x00320EF0 File Offset: 0x0031FEF0
		public IList<Uri> IgnoredNamespaces
		{
			get
			{
				return this._ignoredNamespaces;
			}
		}

		// Token: 0x17001D77 RID: 7543
		// (get) Token: 0x06007FEA RID: 32746 RVA: 0x00320EF8 File Offset: 0x0031FEF8
		public static IList<Uri> WellKnownNamespaces
		{
			get
			{
				Uri[] array = new Uri[XmlStreamStore._predefinedNamespaces.Keys.Count];
				XmlStreamStore._predefinedNamespaces.Keys.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x00320F2C File Offset: 0x0031FF2C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06007FEC RID: 32748 RVA: 0x00320F40 File Offset: 0x0031FF40
		protected override void OnStoreContentChanged(StoreContentChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			base.OnStoreContentChanged(e);
		}

		// Token: 0x06007FED RID: 32749 RVA: 0x00320F88 File Offset: 0x0031FF88
		private List<Guid> FindAnnotationIds(string queryExpression)
		{
			Invariant.Assert(queryExpression != null && queryExpression.Length > 0, "Invalid query expression");
			List<Guid> list = null;
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this.CheckStatus();
				XPathNodeIterator xpathNodeIterator = this._document.CreateNavigator().Select(queryExpression, this._namespaceManager);
				if (xpathNodeIterator != null && xpathNodeIterator.Count > 0)
				{
					list = new List<Guid>(xpathNodeIterator.Count);
					using (IEnumerator enumerator = xpathNodeIterator.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							string attribute = ((XPathNavigator)obj).GetAttribute("Id", "");
							if (string.IsNullOrEmpty(attribute))
							{
								throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
								{
									"Id",
									"Annotation"
								}));
							}
							Guid item;
							try
							{
								item = XmlConvert.ToGuid(attribute);
							}
							catch (FormatException innerException)
							{
								throw new InvalidOperationException(SR.Get("CannotParseId"), innerException);
							}
							list.Add(item);
						}
						return list;
					}
				}
				list = new List<Guid>(0);
			}
			return list;
		}

		// Token: 0x06007FEE RID: 32750 RVA: 0x003210DC File Offset: 0x003200DC
		private void HandleAuthorChanged(object sender, AnnotationAuthorChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			this.OnAuthorChanged(e);
		}

		// Token: 0x06007FEF RID: 32751 RVA: 0x00321124 File Offset: 0x00320124
		private void HandleAnchorChanged(object sender, AnnotationResourceChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			this.OnAnchorChanged(e);
		}

		// Token: 0x06007FF0 RID: 32752 RVA: 0x0032116C File Offset: 0x0032016C
		private void HandleCargoChanged(object sender, AnnotationResourceChangedEventArgs e)
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._dirty = true;
			}
			this.OnCargoChanged(e);
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x003211B4 File Offset: 0x003201B4
		private IList<Annotation> MergeAndCacheAnnotations(Dictionary<Guid, Annotation> mapAnnotations, List<Guid> storeAnnotationsId)
		{
			List<Annotation> list = new List<Annotation>(mapAnnotations.Values);
			foreach (Guid guid in storeAnnotationsId)
			{
				Annotation annotation;
				if (!mapAnnotations.TryGetValue(guid, out annotation))
				{
					annotation = this.GetAnnotation(guid);
					list.Add(annotation);
				}
			}
			return list;
		}

		// Token: 0x06007FF2 RID: 32754 RVA: 0x00321224 File Offset: 0x00320224
		private IList<Annotation> InternalGetAnnotations(string query, ContentLocator anchorLocator)
		{
			Invariant.Assert(query != null, "Parameter 'query' is null.");
			object syncRoot = base.SyncRoot;
			IList<Annotation> result;
			lock (syncRoot)
			{
				this.CheckStatus();
				List<Guid> storeAnnotationsId = this.FindAnnotationIds(query);
				Dictionary<Guid, Annotation> mapAnnotations;
				if (anchorLocator == null)
				{
					mapAnnotations = this._storeAnnotationsMap.FindAnnotations();
				}
				else
				{
					mapAnnotations = this._storeAnnotationsMap.FindAnnotations(anchorLocator);
				}
				result = this.MergeAndCacheAnnotations(mapAnnotations, storeAnnotationsId);
			}
			return result;
		}

		// Token: 0x06007FF3 RID: 32755 RVA: 0x003212A8 File Offset: 0x003202A8
		private void LoadStream(IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			this.CheckKnownNamespaces(knownNamespaces);
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._document = new XmlDocument();
				this._document.PreserveWhitespace = false;
				if (this._stream.Length == 0L)
				{
					this._document.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?> <anc:Annotations xmlns:anc=\"http://schemas.microsoft.com/windows/annotations/2003/11/core\" xmlns:anb=\"http://schemas.microsoft.com/windows/annotations/2003/11/base\" />");
				}
				else
				{
					this._xmlCompatibilityReader = this.SetupReader(knownNamespaces);
					this._document.Load(this._xmlCompatibilityReader);
				}
				this._namespaceManager = new XmlNamespaceManager(this._document.NameTable);
				this._namespaceManager.AddNamespace("anc", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
				this._namespaceManager.AddNamespace("anb", "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				XPathNodeIterator xpathNodeIterator = this._document.CreateNavigator().Select("//anc:Annotations", this._namespaceManager);
				Invariant.Assert(xpathNodeIterator.Count == 1, "More than one annotation returned for the query");
				xpathNodeIterator.MoveNext();
				this._rootNavigator = xpathNodeIterator.Current;
			}
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x003213C0 File Offset: 0x003203C0
		private void CheckKnownNamespaces(IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			if (knownNamespaces == null)
			{
				return;
			}
			IList<Uri> list = new List<Uri>();
			foreach (Uri item in XmlStreamStore._predefinedNamespaces.Keys)
			{
				list.Add(item);
			}
			foreach (Uri uri in knownNamespaces.Keys)
			{
				if (uri == null)
				{
					throw new ArgumentException(SR.Get("NullUri"), "knownNamespaces");
				}
				if (list.Contains(uri))
				{
					throw new ArgumentException(SR.Get("DuplicatedUri"), "knownNamespaces");
				}
				list.Add(uri);
			}
			foreach (KeyValuePair<Uri, IList<Uri>> keyValuePair in knownNamespaces)
			{
				if (keyValuePair.Value != null)
				{
					foreach (Uri uri2 in keyValuePair.Value)
					{
						if (uri2 == null)
						{
							throw new ArgumentException(SR.Get("NullUri"), "knownNamespaces");
						}
						if (list.Contains(uri2))
						{
							throw new ArgumentException(SR.Get("DuplicatedCompatibleUri"), "knownNamespaces");
						}
						list.Add(uri2);
					}
				}
			}
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x00321560 File Offset: 0x00320560
		private XmlCompatibilityReader SetupReader(IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			IList<string> list = new List<string>();
			foreach (Uri uri in XmlStreamStore._predefinedNamespaces.Keys)
			{
				list.Add(uri.ToString());
			}
			if (knownNamespaces != null)
			{
				foreach (Uri uri2 in knownNamespaces.Keys)
				{
					list.Add(uri2.ToString());
				}
			}
			XmlCompatibilityReader xmlCompatibilityReader = new XmlCompatibilityReader(new XmlTextReader(this._stream), new IsXmlNamespaceSupportedCallback(this.IsXmlNamespaceSupported), list);
			if (knownNamespaces != null)
			{
				foreach (KeyValuePair<Uri, IList<Uri>> keyValuePair in knownNamespaces)
				{
					if (keyValuePair.Value != null)
					{
						foreach (Uri uri3 in keyValuePair.Value)
						{
							xmlCompatibilityReader.DeclareNamespaceCompatibility(keyValuePair.Key.ToString(), uri3.ToString());
						}
					}
				}
			}
			this._ignoredNamespaces.Clear();
			return xmlCompatibilityReader;
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x003216D0 File Offset: 0x003206D0
		private bool IsXmlNamespaceSupported(string xmlNamespace, out string newXmlNamespace)
		{
			if (!string.IsNullOrEmpty(xmlNamespace))
			{
				if (!Uri.IsWellFormedUriString(xmlNamespace, UriKind.RelativeOrAbsolute))
				{
					throw new ArgumentException(SR.Get("InvalidNamespace", new object[]
					{
						xmlNamespace
					}), "xmlNamespace");
				}
				Uri item = new Uri(xmlNamespace, UriKind.RelativeOrAbsolute);
				if (!this._ignoredNamespaces.Contains(item))
				{
					this._ignoredNamespaces.Add(item);
				}
			}
			newXmlNamespace = null;
			return false;
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x00321734 File Offset: 0x00320734
		private XPathNavigator GetAnnotationNodeForId(Guid id)
		{
			XPathNavigator result = null;
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				XPathNodeIterator xpathNodeIterator = this._document.CreateNavigator().Select("//anc:Annotation[@Id=\"" + XmlConvert.ToString(id) + "\"]", this._namespaceManager);
				if (xpathNodeIterator.MoveNext())
				{
					result = xpathNodeIterator.Current;
				}
			}
			return result;
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x003217AC File Offset: 0x003207AC
		private void CheckStatus()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				if (base.IsDisposed)
				{
					throw new ObjectDisposedException(null, SR.Get("ObjectDisposed_StoreClosed"));
				}
				if (this._stream == null)
				{
					throw new InvalidOperationException(SR.Get("StreamNotSet"));
				}
			}
		}

		// Token: 0x06007FF9 RID: 32761 RVA: 0x00321818 File Offset: 0x00320818
		private void SerializeAnnotations()
		{
			foreach (Annotation annotation in this._storeAnnotationsMap.FindDirtyAnnotations())
			{
				XPathNavigator xpathNavigator = this.GetAnnotationNodeForId(annotation.Id);
				if (xpathNavigator == null)
				{
					xpathNavigator = this._rootNavigator.CreateNavigator();
					XmlWriter xmlWriter = xpathNavigator.AppendChild();
					XmlStreamStore._serializer.Serialize(xmlWriter, annotation);
					xmlWriter.Close();
				}
				else
				{
					XmlWriter xmlWriter2 = xpathNavigator.InsertBefore();
					XmlStreamStore._serializer.Serialize(xmlWriter2, annotation);
					xmlWriter2.Close();
					xpathNavigator.DeleteSelf();
				}
			}
			this._storeAnnotationsMap.ValidateDirtyAnnotations();
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x003218D0 File Offset: 0x003208D0
		private void Cleanup()
		{
			object syncRoot = base.SyncRoot;
			lock (syncRoot)
			{
				this._xmlCompatibilityReader = null;
				this._ignoredNamespaces = null;
				this._stream = null;
				this._document = null;
				this._rootNavigator = null;
				this._storeAnnotationsMap = null;
			}
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x00321934 File Offset: 0x00320934
		private void SetStream(Stream stream, IDictionary<Uri, IList<Uri>> knownNamespaces)
		{
			try
			{
				object syncRoot = base.SyncRoot;
				lock (syncRoot)
				{
					this._storeAnnotationsMap = new StoreAnnotationsMap(new AnnotationAuthorChangedEventHandler(this.HandleAuthorChanged), new AnnotationResourceChangedEventHandler(this.HandleAnchorChanged), new AnnotationResourceChangedEventHandler(this.HandleCargoChanged));
					this._stream = stream;
					this.LoadStream(knownNamespaces);
				}
			}
			catch
			{
				this.Cleanup();
				throw;
			}
		}

		// Token: 0x04003B9E RID: 15262
		private bool _dirty;

		// Token: 0x04003B9F RID: 15263
		private bool _autoFlush;

		// Token: 0x04003BA0 RID: 15264
		private XmlDocument _document;

		// Token: 0x04003BA1 RID: 15265
		private XmlNamespaceManager _namespaceManager;

		// Token: 0x04003BA2 RID: 15266
		private Stream _stream;

		// Token: 0x04003BA3 RID: 15267
		private XPathNavigator _rootNavigator;

		// Token: 0x04003BA4 RID: 15268
		private StoreAnnotationsMap _storeAnnotationsMap;

		// Token: 0x04003BA5 RID: 15269
		private List<Uri> _ignoredNamespaces = new List<Uri>();

		// Token: 0x04003BA6 RID: 15270
		private XmlCompatibilityReader _xmlCompatibilityReader;

		// Token: 0x04003BA7 RID: 15271
		private static readonly Dictionary<Uri, IList<Uri>> _predefinedNamespaces;

		// Token: 0x04003BA8 RID: 15272
		private static readonly Serializer _serializer = new Serializer(typeof(Annotation));
	}
}
