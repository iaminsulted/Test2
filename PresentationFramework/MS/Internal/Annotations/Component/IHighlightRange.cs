using System;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002CC RID: 716
	internal interface IHighlightRange
	{
		// Token: 0x06001AD9 RID: 6873
		void AddChild(Shape child);

		// Token: 0x06001ADA RID: 6874
		void RemoveChild(Shape child);

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001ADB RID: 6875
		Color Background { get; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001ADC RID: 6876
		Color SelectedBackground { get; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001ADD RID: 6877
		TextAnchor Range { get; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001ADE RID: 6878
		int Priority { get; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001ADF RID: 6879
		bool HighlightContent { get; }
	}
}
