using System;

namespace System.Windows.Diagnostics
{
	// Token: 0x02000445 RID: 1093
	public class StaticResourceResolvedEventArgs : EventArgs
	{
		// Token: 0x06003540 RID: 13632 RVA: 0x001DD7E0 File Offset: 0x001DC7E0
		internal StaticResourceResolvedEventArgs(object targetObject, object targetProperty, ResourceDictionary rd, object key)
		{
			this.TargetObject = targetObject;
			this.TargetProperty = targetProperty;
			this.ResourceDictionary = rd;
			this.ResourceKey = key;
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003541 RID: 13633 RVA: 0x001DD805 File Offset: 0x001DC805
		// (set) Token: 0x06003542 RID: 13634 RVA: 0x001DD80D File Offset: 0x001DC80D
		public object TargetObject { get; private set; }

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06003543 RID: 13635 RVA: 0x001DD816 File Offset: 0x001DC816
		// (set) Token: 0x06003544 RID: 13636 RVA: 0x001DD81E File Offset: 0x001DC81E
		public object TargetProperty { get; private set; }

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003545 RID: 13637 RVA: 0x001DD827 File Offset: 0x001DC827
		// (set) Token: 0x06003546 RID: 13638 RVA: 0x001DD82F File Offset: 0x001DC82F
		public ResourceDictionary ResourceDictionary { get; private set; }

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003547 RID: 13639 RVA: 0x001DD838 File Offset: 0x001DC838
		// (set) Token: 0x06003548 RID: 13640 RVA: 0x001DD840 File Offset: 0x001DC840
		public object ResourceKey { get; private set; }
	}
}
