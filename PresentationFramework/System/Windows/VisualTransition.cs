using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows
{
	// Token: 0x020003E0 RID: 992
	[ContentProperty("Storyboard")]
	public class VisualTransition : DependencyObject
	{
		// Token: 0x060029C0 RID: 10688 RVA: 0x0019ACE4 File Offset: 0x00199CE4
		public VisualTransition()
		{
			this.DynamicStoryboardCompleted = true;
			this.ExplicitStoryboardCompleted = true;
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060029C1 RID: 10689 RVA: 0x0019AD19 File Offset: 0x00199D19
		// (set) Token: 0x060029C2 RID: 10690 RVA: 0x0019AD21 File Offset: 0x00199D21
		public string From { get; set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x0019AD2A File Offset: 0x00199D2A
		// (set) Token: 0x060029C4 RID: 10692 RVA: 0x0019AD32 File Offset: 0x00199D32
		public string To { get; set; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x0019AD3B File Offset: 0x00199D3B
		// (set) Token: 0x060029C6 RID: 10694 RVA: 0x0019AD43 File Offset: 0x00199D43
		public Storyboard Storyboard { get; set; }

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x0019AD4C File Offset: 0x00199D4C
		// (set) Token: 0x060029C8 RID: 10696 RVA: 0x0019AD54 File Offset: 0x00199D54
		[TypeConverter(typeof(DurationConverter))]
		public Duration GeneratedDuration
		{
			get
			{
				return this._generatedDuration;
			}
			set
			{
				this._generatedDuration = value;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x0019AD5D File Offset: 0x00199D5D
		// (set) Token: 0x060029CA RID: 10698 RVA: 0x0019AD65 File Offset: 0x00199D65
		public IEasingFunction GeneratedEasingFunction { get; set; }

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x0019AD6E File Offset: 0x00199D6E
		internal bool IsDefault
		{
			get
			{
				return this.From == null && this.To == null;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x0019AD83 File Offset: 0x00199D83
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x0019AD8B File Offset: 0x00199D8B
		internal bool DynamicStoryboardCompleted { get; set; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x0019AD94 File Offset: 0x00199D94
		// (set) Token: 0x060029CF RID: 10703 RVA: 0x0019AD9C File Offset: 0x00199D9C
		internal bool ExplicitStoryboardCompleted { get; set; }

		// Token: 0x0400150E RID: 5390
		private Duration _generatedDuration = new Duration(default(TimeSpan));
	}
}
