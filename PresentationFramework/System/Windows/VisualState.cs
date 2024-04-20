using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows
{
	// Token: 0x020003DC RID: 988
	[ContentProperty("Storyboard")]
	[RuntimeNameProperty("Name")]
	public class VisualState : DependencyObject
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x0600298B RID: 10635 RVA: 0x00199E3A File Offset: 0x00198E3A
		// (set) Token: 0x0600298C RID: 10636 RVA: 0x00199E42 File Offset: 0x00198E42
		public string Name { get; set; }

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x00199E4B File Offset: 0x00198E4B
		// (set) Token: 0x0600298E RID: 10638 RVA: 0x00199E5D File Offset: 0x00198E5D
		public Storyboard Storyboard
		{
			get
			{
				return (Storyboard)base.GetValue(VisualState.StoryboardProperty);
			}
			set
			{
				base.SetValue(VisualState.StoryboardProperty, value);
			}
		}

		// Token: 0x040014F8 RID: 5368
		private static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(VisualState));
	}
}
