using System;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200042F RID: 1071
	public class EasingThicknessKeyFrame : ThicknessKeyFrame
	{
		// Token: 0x06003406 RID: 13318 RVA: 0x001D9F78 File Offset: 0x001D8F78
		public EasingThicknessKeyFrame()
		{
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x001D9FB0 File Offset: 0x001D8FB0
		public EasingThicknessKeyFrame(Thickness value) : this()
		{
			base.Value = value;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x001D9FBF File Offset: 0x001D8FBF
		public EasingThicknessKeyFrame(Thickness value, KeyTime keyTime) : this()
		{
			base.Value = value;
			base.KeyTime = keyTime;
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x001D9FD5 File Offset: 0x001D8FD5
		public EasingThicknessKeyFrame(Thickness value, KeyTime keyTime, IEasingFunction easingFunction) : this()
		{
			base.Value = value;
			base.KeyTime = keyTime;
			this.EasingFunction = easingFunction;
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x001D9FF2 File Offset: 0x001D8FF2
		protected override Freezable CreateInstanceCore()
		{
			return new EasingThicknessKeyFrame();
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x001D9FFC File Offset: 0x001D8FFC
		protected override Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress)
		{
			IEasingFunction easingFunction = this.EasingFunction;
			if (easingFunction != null)
			{
				keyFrameProgress = easingFunction.Ease(keyFrameProgress);
			}
			if (keyFrameProgress == 0.0)
			{
				return baseValue;
			}
			if (keyFrameProgress == 1.0)
			{
				return base.Value;
			}
			return AnimatedTypeHelpers.InterpolateThickness(baseValue, base.Value, keyFrameProgress);
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x001DA04A File Offset: 0x001D904A
		// (set) Token: 0x0600340D RID: 13325 RVA: 0x001DA05C File Offset: 0x001D905C
		public IEasingFunction EasingFunction
		{
			get
			{
				return (IEasingFunction)base.GetValue(EasingThicknessKeyFrame.EasingFunctionProperty);
			}
			set
			{
				base.SetValueInternal(EasingThicknessKeyFrame.EasingFunctionProperty, value);
			}
		}

		// Token: 0x04001C4A RID: 7242
		public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(EasingThicknessKeyFrame));
	}
}
