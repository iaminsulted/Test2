using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D5 RID: 725
	internal abstract class SelectionProcessor
	{
		// Token: 0x06001B6C RID: 7020
		public abstract bool MergeSelections(object selection1, object selection2, out object newSelection);

		// Token: 0x06001B6D RID: 7021
		public abstract IList<DependencyObject> GetSelectedNodes(object selection);

		// Token: 0x06001B6E RID: 7022
		public abstract UIElement GetParent(object selection);

		// Token: 0x06001B6F RID: 7023
		public abstract Point GetAnchorPoint(object selection);

		// Token: 0x06001B70 RID: 7024
		public abstract IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode);

		// Token: 0x06001B71 RID: 7025
		public abstract object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel);

		// Token: 0x06001B72 RID: 7026
		public abstract XmlQualifiedName[] GetLocatorPartTypes();
	}
}
