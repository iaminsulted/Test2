using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000434 RID: 1076
	public abstract class ThicknessAnimationBase : AnimationTimeline
	{
		// Token: 0x06003442 RID: 13378 RVA: 0x001DA934 File Offset: 0x001D9934
		public new ThicknessAnimationBase Clone()
		{
			return (ThicknessAnimationBase)base.Clone();
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x001DA941 File Offset: 0x001D9941
		public sealed override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
		{
			if (defaultOriginValue == null)
			{
				throw new ArgumentNullException("defaultOriginValue");
			}
			if (defaultDestinationValue == null)
			{
				throw new ArgumentNullException("defaultDestinationValue");
			}
			return this.GetCurrentValue((Thickness)defaultOriginValue, (Thickness)defaultDestinationValue, animationClock);
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x001DA977 File Offset: 0x001D9977
		public sealed override Type TargetPropertyType
		{
			get
			{
				base.ReadPreamble();
				return typeof(Thickness);
			}
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x001DA989 File Offset: 0x001D9989
		public Thickness GetCurrentValue(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
		{
			base.ReadPreamble();
			if (animationClock == null)
			{
				throw new ArgumentNullException("animationClock");
			}
			if (animationClock.CurrentState == ClockState.Stopped)
			{
				return defaultDestinationValue;
			}
			return this.GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
		}

		// Token: 0x06003446 RID: 13382
		protected abstract Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock);
	}
}
