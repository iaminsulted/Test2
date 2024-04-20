using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D1 RID: 721
	internal class FixedPageProcessor : SubTreeProcessor
	{
		// Token: 0x06001B2C RID: 6956 RVA: 0x00166C70 File Offset: 0x00165C70
		public FixedPageProcessor(LocatorManager manager) : base(manager)
		{
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x001670A8 File Offset: 0x001660A8
		public override IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			DocumentPageView documentPageView = node as DocumentPageView;
			if (documentPageView != null && (documentPageView.DocumentPage is FixedDocumentPage || documentPageView.DocumentPage is FixedDocumentSequenceDocumentPage))
			{
				calledProcessAnnotations = true;
				return base.Manager.ProcessAnnotations(documentPageView);
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x001670FC File Offset: 0x001660FC
		public override ContentLocator GenerateLocator(PathNode node, out bool continueGenerating)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			continueGenerating = true;
			ContentLocator contentLocator = null;
			DocumentPageView documentPageView = node.Node as DocumentPageView;
			int num = -1;
			if (documentPageView != null)
			{
				if (documentPageView.DocumentPage is FixedDocumentPage || documentPageView.DocumentPage is FixedDocumentSequenceDocumentPage)
				{
					num = documentPageView.PageNumber;
				}
			}
			else
			{
				FixedTextSelectionProcessor.FixedPageProxy fixedPageProxy = node.Node as FixedTextSelectionProcessor.FixedPageProxy;
				if (fixedPageProxy != null)
				{
					num = fixedPageProxy.Page;
				}
			}
			if (num >= 0)
			{
				contentLocator = new ContentLocator();
				ContentLocatorPart item = FixedPageProcessor.CreateLocatorPart(num);
				contentLocator.Parts.Add(item);
			}
			return contentLocator;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x00167188 File Offset: 0x00166188
		public override DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (FixedPageProcessor.PageNumberElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			continueResolving = true;
			string text = locatorPart.NameValuePairs[FixedPageProcessor.ValueAttributeName];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
			FixedDocumentPage fixedDocumentPage = null;
			if (this._useLogicalTree)
			{
				IDocumentPaginatorSource documentPaginatorSource = startNode as FixedDocument;
				if (documentPaginatorSource != null)
				{
					fixedDocumentPage = (documentPaginatorSource.DocumentPaginator.GetPage(num) as FixedDocumentPage);
				}
				else
				{
					documentPaginatorSource = (startNode as FixedDocumentSequence);
					if (documentPaginatorSource != null)
					{
						FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage = documentPaginatorSource.DocumentPaginator.GetPage(num) as FixedDocumentSequenceDocumentPage;
						if (fixedDocumentSequenceDocumentPage != null)
						{
							fixedDocumentPage = (fixedDocumentSequenceDocumentPage.ChildDocumentPage as FixedDocumentPage);
						}
					}
				}
			}
			else
			{
				DocumentPageView documentPageView = startNode as DocumentPageView;
				if (documentPageView != null)
				{
					fixedDocumentPage = (documentPageView.DocumentPage as FixedDocumentPage);
					if (fixedDocumentPage == null)
					{
						FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage2 = documentPageView.DocumentPage as FixedDocumentSequenceDocumentPage;
						if (fixedDocumentSequenceDocumentPage2 != null)
						{
							fixedDocumentPage = (fixedDocumentSequenceDocumentPage2.ChildDocumentPage as FixedDocumentPage);
						}
					}
					if (fixedDocumentPage != null && documentPageView.PageNumber != num)
					{
						continueResolving = false;
						fixedDocumentPage = null;
					}
				}
			}
			if (fixedDocumentPage != null)
			{
				return fixedDocumentPage.FixedPage;
			}
			return null;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0016731B File Offset: 0x0016631B
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])FixedPageProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x1700050E RID: 1294
		// (set) Token: 0x06001B31 RID: 6961 RVA: 0x0016732C File Offset: 0x0016632C
		internal bool UseLogicalTree
		{
			set
			{
				this._useLogicalTree = value;
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x00167335 File Offset: 0x00166335
		internal static ContentLocatorPart CreateLocatorPart(int page)
		{
			return new ContentLocatorPart(FixedPageProcessor.PageNumberElementName)
			{
				NameValuePairs = 
				{
					{
						FixedPageProcessor.ValueAttributeName,
						page.ToString(NumberFormatInfo.InvariantInfo)
					}
				}
			};
		}

		// Token: 0x04000DFE RID: 3582
		public static readonly string Id = "FixedPage";

		// Token: 0x04000DFF RID: 3583
		private static readonly string ValueAttributeName = "Value";

		// Token: 0x04000E00 RID: 3584
		private static readonly XmlQualifiedName PageNumberElementName = new XmlQualifiedName("PageNumber", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04000E01 RID: 3585
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			FixedPageProcessor.PageNumberElementName
		};

		// Token: 0x04000E02 RID: 3586
		private bool _useLogicalTree;
	}
}
