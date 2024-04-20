using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Data
{
	// Token: 0x0200024C RID: 588
	internal class XmlBindingWorker : BindingWorker, IWeakEventListener
	{
		// Token: 0x0600169A RID: 5786 RVA: 0x0015B613 File Offset: 0x0015A613
		internal XmlBindingWorker(ClrBindingWorker worker, bool collectionMode) : base(worker.ParentBindingExpression)
		{
			this._hostWorker = worker;
			this._xpath = base.ParentBinding.XPath;
			this._collectionMode = collectionMode;
			this._xpathType = XmlBindingWorker.GetXPathType(this._xpath);
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0015B654 File Offset: 0x0015A654
		internal override void AttachDataItem()
		{
			if (this.XPath.Length > 0)
			{
				this.CollectionView = (base.DataItem as CollectionView);
				if (this.CollectionView == null && base.DataItem is ICollection)
				{
					this.CollectionView = CollectionViewSource.GetDefaultCollectionView(base.DataItem, base.TargetElement, null);
				}
			}
			if (this.CollectionView != null)
			{
				CurrentChangedEventManager.AddHandler(this.CollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.AddHandler(this.CollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			this.UpdateContextNode(true);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x0015B6FC File Offset: 0x0015A6FC
		internal override void DetachDataItem()
		{
			if (this.CollectionView != null)
			{
				CurrentChangedEventManager.RemoveHandler(this.CollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.RemoveHandler(this.CollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			this.UpdateContextNode(false);
			this.CollectionView = null;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0015B760 File Offset: 0x0015A760
		internal override void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
			if (collectionView == this.CollectionView)
			{
				using (base.ParentBindingExpression.ChangingValue())
				{
					this.UpdateContextNode(true);
					this._hostWorker.UseNewXmlItem(this.RawValue());
				}
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0015B7B8 File Offset: 0x0015A7B8
		internal override object RawValue()
		{
			if (this.XPath.Length == 0)
			{
				return base.DataItem;
			}
			if (this.ContextNode == null)
			{
				this.QueriedCollection = null;
				return null;
			}
			XmlNodeList xmlNodeList = this.SelectNodes();
			if (xmlNodeList == null)
			{
				this.QueriedCollection = null;
				return DependencyProperty.UnsetValue;
			}
			return this.BuildQueriedCollection(xmlNodeList);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0015B808 File Offset: 0x0015A808
		internal void ReportBadXPath(TraceEventType traceType)
		{
			if (TraceData.IsEnabled)
			{
				TraceData.TraceAndNotifyWithNoParameters(traceType, TraceData.BadXPath(new object[]
				{
					this.XPath,
					this.IdentifyNode(this.ContextNode)
				}), base.ParentBindingExpression);
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0015B840 File Offset: 0x0015A840
		// (set) Token: 0x060016A1 RID: 5793 RVA: 0x0015B848 File Offset: 0x0015A848
		private XmlDataCollection QueriedCollection
		{
			get
			{
				return this._queriedCollection;
			}
			set
			{
				this._queriedCollection = value;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0015B851 File Offset: 0x0015A851
		// (set) Token: 0x060016A3 RID: 5795 RVA: 0x0015B859 File Offset: 0x0015A859
		private ICollectionView CollectionView
		{
			get
			{
				return this._collectionView;
			}
			set
			{
				this._collectionView = value;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x0015B862 File Offset: 0x0015A862
		// (set) Token: 0x060016A5 RID: 5797 RVA: 0x0015B86C File Offset: 0x0015A86C
		private XmlNode ContextNode
		{
			get
			{
				return this._contextNode;
			}
			set
			{
				if (this._contextNode != value && TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.Transfer))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.XmlContextNode(new object[]
					{
						TraceData.Identify(base.ParentBindingExpression),
						this.IdentifyNode(value)
					}), base.ParentBindingExpression);
				}
				this._contextNode = value;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x0015B8C6 File Offset: 0x0015A8C6
		private string XPath
		{
			get
			{
				return this._xpath;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060016A7 RID: 5799 RVA: 0x0015B8D0 File Offset: 0x0015A8D0
		private XmlNamespaceManager NamespaceManager
		{
			get
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement == null)
				{
					return null;
				}
				XmlNamespaceManager xmlNamespaceManager = Binding.GetXmlNamespaceManager(targetElement);
				if (xmlNamespaceManager == null && this.XmlDataProvider != null)
				{
					xmlNamespaceManager = this.XmlDataProvider.XmlNamespaceManager;
				}
				return xmlNamespaceManager;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x0015B908 File Offset: 0x0015A908
		private XmlDataProvider XmlDataProvider
		{
			get
			{
				if (this._xmlDataProvider == null && (this._xmlDataProvider = (base.ParentBindingExpression.DataSource as XmlDataProvider)) == null)
				{
					XmlDataCollection xmlDataCollection;
					ItemsControl itemsControl;
					if ((xmlDataCollection = (base.DataItem as XmlDataCollection)) != null)
					{
						this._xmlDataProvider = xmlDataCollection.ParentXmlDataProvider;
					}
					else if (this.CollectionView != null && (xmlDataCollection = (this.CollectionView.SourceCollection as XmlDataCollection)) != null)
					{
						this._xmlDataProvider = xmlDataCollection.ParentXmlDataProvider;
					}
					else if (base.TargetProperty == BindingExpressionBase.NoTargetProperty && (itemsControl = (base.TargetElement as ItemsControl)) != null)
					{
						object itemsSource = itemsControl.ItemsSource;
						if ((xmlDataCollection = (itemsSource as XmlDataCollection)) == null)
						{
							ICollectionView collectionView = itemsSource as ICollectionView;
							xmlDataCollection = (((collectionView != null) ? collectionView.SourceCollection : null) as XmlDataCollection);
						}
						if (xmlDataCollection != null)
						{
							this._xmlDataProvider = xmlDataCollection.ParentXmlDataProvider;
						}
					}
					else
					{
						this._xmlDataProvider = Helper.XmlDataProviderForElement(base.TargetElement);
					}
				}
				return this._xmlDataProvider;
			}
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0015B9FC File Offset: 0x0015A9FC
		private void UpdateContextNode(bool hookNotifications)
		{
			this.UnHookNotifications();
			if (base.DataItem == BindingExpressionBase.DisconnectedItem)
			{
				this.ContextNode = null;
				return;
			}
			if (this.CollectionView != null)
			{
				this.ContextNode = (this.CollectionView.CurrentItem as XmlNode);
				if (this.ContextNode != this.CollectionView.CurrentItem && TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.XmlBindingToNonXmlCollection, base.ParentBindingExpression, new object[]
					{
						this.XPath,
						base.ParentBindingExpression,
						base.DataItem
					}, null);
				}
			}
			else
			{
				this.ContextNode = (base.DataItem as XmlNode);
				if (this.ContextNode != base.DataItem && TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.XmlBindingToNonXml, base.ParentBindingExpression, new object[]
					{
						this.XPath,
						base.ParentBindingExpression,
						base.DataItem
					}, null);
				}
			}
			if (hookNotifications)
			{
				this.HookNotifications();
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0015BAFC File Offset: 0x0015AAFC
		private void HookNotifications()
		{
			if (base.IsDynamic && this.ContextNode != null)
			{
				XmlDocument xmlDocument = this.DocumentFor(this.ContextNode);
				if (xmlDocument != null)
				{
					XmlNodeChangedEventManager.AddHandler(xmlDocument, new EventHandler<XmlNodeChangedEventArgs>(this.OnXmlNodeChanged));
				}
			}
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0015BB3C File Offset: 0x0015AB3C
		private void UnHookNotifications()
		{
			if (base.IsDynamic && this.ContextNode != null)
			{
				XmlDocument xmlDocument = this.DocumentFor(this.ContextNode);
				if (xmlDocument != null)
				{
					XmlNodeChangedEventManager.RemoveHandler(xmlDocument, new EventHandler<XmlNodeChangedEventArgs>(this.OnXmlNodeChanged));
				}
			}
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0015BB7C File Offset: 0x0015AB7C
		private XmlDocument DocumentFor(XmlNode node)
		{
			XmlDocument xmlDocument = node.OwnerDocument;
			if (xmlDocument == null)
			{
				xmlDocument = (node as XmlDocument);
			}
			return xmlDocument;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0015BB9C File Offset: 0x0015AB9C
		private XmlDataCollection BuildQueriedCollection(XmlNodeList nodes)
		{
			if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.CreateExpression))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.XmlNewCollection(new object[]
				{
					TraceData.Identify(base.ParentBindingExpression),
					this.IdentifyNodeList(nodes)
				}), base.ParentBindingExpression);
			}
			this.QueriedCollection = new XmlDataCollection(this.XmlDataProvider);
			this.QueriedCollection.XmlNamespaceManager = this.NamespaceManager;
			this.QueriedCollection.SynchronizeCollection(nodes);
			return this.QueriedCollection;
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
		{
			return false;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0015BC1C File Offset: 0x0015AC1C
		private void OnXmlNodeChanged(object sender, XmlNodeChangedEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(base.ParentBindingExpression),
					"XmlNodeChanged",
					TraceData.Identify(sender)
				}), base.ParentBindingExpression);
			}
			this.ProcessXmlNodeChanged(e);
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0015BC74 File Offset: 0x0015AC74
		private void ProcessXmlNodeChanged(EventArgs args)
		{
			DependencyObject targetElement = base.ParentBindingExpression.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			if (base.IgnoreSourcePropertyChange)
			{
				return;
			}
			if (base.DataItem == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			if (!this.IsChangeRelevant(args))
			{
				return;
			}
			if (this.XPath.Length == 0)
			{
				this._hostWorker.OnXmlValueChanged();
			}
			else if (this.QueriedCollection == null)
			{
				this._hostWorker.UseNewXmlItem(this.RawValue());
			}
			else
			{
				XmlNodeList xmlNodeList = this.SelectNodes();
				if (xmlNodeList == null)
				{
					this.QueriedCollection = null;
					this._hostWorker.UseNewXmlItem(DependencyProperty.UnsetValue);
				}
				else if (this._collectionMode)
				{
					if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.CreateExpression))
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.XmlSynchronizeCollection(new object[]
						{
							TraceData.Identify(base.ParentBindingExpression),
							this.IdentifyNodeList(xmlNodeList)
						}), base.ParentBindingExpression);
					}
					this.QueriedCollection.SynchronizeCollection(xmlNodeList);
				}
				else if (this.QueriedCollection.CollectionHasChanged(xmlNodeList))
				{
					this._hostWorker.UseNewXmlItem(this.BuildQueriedCollection(xmlNodeList));
				}
				else
				{
					this._hostWorker.OnXmlValueChanged();
				}
			}
			GC.KeepAlive(targetElement);
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0015BD9C File Offset: 0x0015AD9C
		private XmlNodeList SelectNodes()
		{
			XmlNamespaceManager namespaceManager = this.NamespaceManager;
			XmlNodeList xmlNodeList = null;
			try
			{
				if (namespaceManager != null)
				{
					xmlNodeList = this.ContextNode.SelectNodes(this.XPath, namespaceManager);
				}
				else
				{
					xmlNodeList = this.ContextNode.SelectNodes(this.XPath);
				}
			}
			catch (XPathException ex)
			{
				base.Status = BindingStatusInternal.PathError;
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.CannotGetXmlNodeCollection, base.ParentBindingExpression, new object[]
					{
						(this.ContextNode != null) ? this.ContextNode.Name : null,
						this.XPath,
						base.ParentBindingExpression,
						ex
					}, new object[]
					{
						ex
					});
				}
			}
			if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.CreateExpression))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.SelectNodes(new object[]
				{
					TraceData.Identify(base.ParentBindingExpression),
					this.IdentifyNode(this.ContextNode),
					TraceData.Identify(this.XPath),
					this.IdentifyNodeList(xmlNodeList)
				}), base.ParentBindingExpression);
			}
			return xmlNodeList;
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0015BEAC File Offset: 0x0015AEAC
		private string IdentifyNode(XmlNode node)
		{
			if (node == null)
			{
				return "<null>";
			}
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0} ({1})", node.GetType().Name, node.Name);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0015BED7 File Offset: 0x0015AED7
		private string IdentifyNodeList(XmlNodeList nodeList)
		{
			if (nodeList == null)
			{
				return "<null>";
			}
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0} (hash={1} Count={2})", nodeList.GetType().Name, AvTrace.GetHashCodeHelper(nodeList), nodeList.Count);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0015BF14 File Offset: 0x0015AF14
		private static XmlBindingWorker.XPathType GetXPathType(string xpath)
		{
			int length = xpath.Length;
			if (length == 0)
			{
				return XmlBindingWorker.XPathType.SimpleName;
			}
			bool flag = xpath[0] == '@';
			int i = flag ? 1 : 0;
			if (i >= length)
			{
				return XmlBindingWorker.XPathType.Default;
			}
			char c = xpath[i];
			if (!char.IsLetter(c) && c != '_' && c != ':')
			{
				return XmlBindingWorker.XPathType.Default;
			}
			for (i++; i < length; i++)
			{
				c = xpath[i];
				if (!char.IsLetterOrDigit(c) && c != '.' && c != '-' && c != '_' && c != ':')
				{
					return XmlBindingWorker.XPathType.Default;
				}
			}
			if (!flag)
			{
				return XmlBindingWorker.XPathType.SimpleName;
			}
			return XmlBindingWorker.XPathType.SimpleAttribute;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0015BFA0 File Offset: 0x0015AFA0
		private bool IsChangeRelevant(EventArgs rawArgs)
		{
			if (this._xpathType == XmlBindingWorker.XPathType.Default)
			{
				return true;
			}
			XmlNodeChangedEventArgs xmlNodeChangedEventArgs = (XmlNodeChangedEventArgs)rawArgs;
			XmlNode xmlNode = null;
			XmlNode xmlNode2 = null;
			switch (xmlNodeChangedEventArgs.Action)
			{
			case XmlNodeChangedAction.Insert:
				xmlNode = xmlNodeChangedEventArgs.NewParent;
				break;
			case XmlNodeChangedAction.Remove:
				xmlNode = xmlNodeChangedEventArgs.OldParent;
				break;
			case XmlNodeChangedAction.Change:
				xmlNode2 = xmlNodeChangedEventArgs.Node;
				break;
			}
			if (this._collectionMode)
			{
				return xmlNode == this.ContextNode;
			}
			if (xmlNode == this.ContextNode)
			{
				return true;
			}
			XmlNode xmlNode3 = this._hostWorker.GetResultNode() as XmlNode;
			if (xmlNode3 == null)
			{
				return false;
			}
			if (xmlNode2 != null)
			{
				xmlNode = xmlNode2;
			}
			while (xmlNode != null)
			{
				if (xmlNode == xmlNode3)
				{
					return true;
				}
				xmlNode = xmlNode.ParentNode;
			}
			return false;
		}

		// Token: 0x04000C71 RID: 3185
		private bool _collectionMode;

		// Token: 0x04000C72 RID: 3186
		private XmlBindingWorker.XPathType _xpathType;

		// Token: 0x04000C73 RID: 3187
		private XmlNode _contextNode;

		// Token: 0x04000C74 RID: 3188
		private XmlDataCollection _queriedCollection;

		// Token: 0x04000C75 RID: 3189
		private ICollectionView _collectionView;

		// Token: 0x04000C76 RID: 3190
		private XmlDataProvider _xmlDataProvider;

		// Token: 0x04000C77 RID: 3191
		private ClrBindingWorker _hostWorker;

		// Token: 0x04000C78 RID: 3192
		private string _xpath;

		// Token: 0x02000A09 RID: 2569
		private enum XPathType : byte
		{
			// Token: 0x0400406E RID: 16494
			Default,
			// Token: 0x0400406F RID: 16495
			SimpleName,
			// Token: 0x04004070 RID: 16496
			SimpleAttribute
		}
	}
}
