using System;
using System.ComponentModel;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200043B RID: 1083
	public sealed class SetStoryboardSpeedRatio : ControllableStoryboardAction
	{
		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x001DB99C File Offset: 0x001DA99C
		// (set) Token: 0x06003495 RID: 13461 RVA: 0x001DB9A4 File Offset: 0x001DA9A4
		[DefaultValue(1.0)]
		public double SpeedRatio
		{
			get
			{
				return this._speedRatio;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"SetStoryboardSpeedRatio"
					}));
				}
				this._speedRatio = value;
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x001DB9D3 File Offset: 0x001DA9D3
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.SetSpeedRatio(containingFE, this.SpeedRatio);
				return;
			}
			storyboard.SetSpeedRatio(containingFCE, this.SpeedRatio);
		}

		// Token: 0x04001C5C RID: 7260
		private double _speedRatio = 1.0;
	}
}
