using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002DA RID: 730
	internal sealed class TreeNodeSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06001BA5 RID: 7077 RVA: 0x00169FFD File Offset: 0x00168FFD
		public override bool MergeSelections(object selection1, object selection2, out object newSelection)
		{
			if (selection1 == null)
			{
				throw new ArgumentNullException("selection1");
			}
			if (selection2 == null)
			{
				throw new ArgumentNullException("selection2");
			}
			newSelection = null;
			return false;
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0016A01F File Offset: 0x0016901F
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			return new DependencyObject[]
			{
				this.GetParent(selection)
			};
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0016A031 File Offset: 0x00169031
		public override UIElement GetParent(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			UIElement uielement = selection as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			return uielement;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0016A060 File Offset: 0x00169060
		public override Point GetAnchorPoint(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			Visual visual = selection as Visual;
			if (visual == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			Rect visualContentBounds = visual.VisualContentBounds;
			return new Point(visualContentBounds.Left, visualContentBounds.Top);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0016A0B2 File Offset: 0x001690B2
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			return new List<ContentLocatorPart>(0);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0016A0D6 File Offset: 0x001690D6
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			attachmentLevel = AttachmentLevel.Full;
			return startNode;
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0016A0F8 File Offset: 0x001690F8
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TreeNodeSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x04000E18 RID: 3608
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = Array.Empty<XmlQualifiedName>();
	}
}
