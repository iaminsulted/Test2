using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D6 RID: 726
	internal abstract class SubTreeProcessor
	{
		// Token: 0x06001B73 RID: 7027 RVA: 0x00168FE9 File Offset: 0x00167FE9
		protected SubTreeProcessor(LocatorManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			this._manager = manager;
		}

		// Token: 0x06001B74 RID: 7028
		public abstract IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations);

		// Token: 0x06001B75 RID: 7029 RVA: 0x00169006 File Offset: 0x00168006
		public virtual IList<IAttachedAnnotation> PostProcessNode(DependencyObject node, bool childrenCalledProcessAnnotations, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06001B76 RID: 7030
		public abstract ContentLocator GenerateLocator(PathNode node, out bool continueGenerating);

		// Token: 0x06001B77 RID: 7031
		public abstract DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving);

		// Token: 0x06001B78 RID: 7032
		public abstract XmlQualifiedName[] GetLocatorPartTypes();

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x0016901A File Offset: 0x0016801A
		protected LocatorManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x04000E0E RID: 3598
		private LocatorManager _manager;
	}
}
