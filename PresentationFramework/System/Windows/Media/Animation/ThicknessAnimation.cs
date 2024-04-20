using System;
using System.Globalization;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000433 RID: 1075
	public class ThicknessAnimation : ThicknessAnimationBase
	{
		// Token: 0x06003429 RID: 13353 RVA: 0x001DA2E8 File Offset: 0x001D92E8
		static ThicknessAnimation()
		{
			Type typeFromHandle = typeof(Thickness?);
			Type typeFromHandle2 = typeof(ThicknessAnimation);
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(ThicknessAnimation.AnimationFunction_Changed);
			ValidateValueCallback validateValueCallback = new ValidateValueCallback(ThicknessAnimation.ValidateFromToOrByValue);
			ThicknessAnimation.FromProperty = DependencyProperty.Register("From", typeFromHandle, typeFromHandle2, new PropertyMetadata(null, propertyChangedCallback), validateValueCallback);
			ThicknessAnimation.ToProperty = DependencyProperty.Register("To", typeFromHandle, typeFromHandle2, new PropertyMetadata(null, propertyChangedCallback), validateValueCallback);
			ThicknessAnimation.ByProperty = DependencyProperty.Register("By", typeFromHandle, typeFromHandle2, new PropertyMetadata(null, propertyChangedCallback), validateValueCallback);
			ThicknessAnimation.EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeFromHandle2);
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x001DA38A File Offset: 0x001D938A
		public ThicknessAnimation()
		{
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x001DA392 File Offset: 0x001D9392
		public ThicknessAnimation(Thickness toValue, Duration duration) : this()
		{
			this.To = new Thickness?(toValue);
			base.Duration = duration;
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x001DA3AD File Offset: 0x001D93AD
		public ThicknessAnimation(Thickness toValue, Duration duration, FillBehavior fillBehavior) : this()
		{
			this.To = new Thickness?(toValue);
			base.Duration = duration;
			base.FillBehavior = fillBehavior;
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x001DA3CF File Offset: 0x001D93CF
		public ThicknessAnimation(Thickness fromValue, Thickness toValue, Duration duration) : this()
		{
			this.From = new Thickness?(fromValue);
			this.To = new Thickness?(toValue);
			base.Duration = duration;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x001DA3F6 File Offset: 0x001D93F6
		public ThicknessAnimation(Thickness fromValue, Thickness toValue, Duration duration, FillBehavior fillBehavior) : this()
		{
			this.From = new Thickness?(fromValue);
			this.To = new Thickness?(toValue);
			base.Duration = duration;
			base.FillBehavior = fillBehavior;
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x001DA425 File Offset: 0x001D9425
		public new ThicknessAnimation Clone()
		{
			return (ThicknessAnimation)base.Clone();
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x001DA432 File Offset: 0x001D9432
		protected override Freezable CreateInstanceCore()
		{
			return new ThicknessAnimation();
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x001DA43C File Offset: 0x001D943C
		protected override Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
		{
			if (!this._isAnimationFunctionValid)
			{
				this.ValidateAnimationFunction();
			}
			double num = animationClock.CurrentProgress.Value;
			IEasingFunction easingFunction = this.EasingFunction;
			if (easingFunction != null)
			{
				num = easingFunction.Ease(num);
			}
			Thickness thickness = default(Thickness);
			Thickness thickness2 = default(Thickness);
			Thickness value = default(Thickness);
			Thickness value2 = default(Thickness);
			bool flag = false;
			bool flag2 = false;
			switch (this._animationType)
			{
			case AnimationType.Automatic:
				thickness = defaultOriginValue;
				thickness2 = defaultDestinationValue;
				flag = true;
				flag2 = true;
				break;
			case AnimationType.From:
				thickness = this._keyValues[0];
				thickness2 = defaultDestinationValue;
				flag2 = true;
				break;
			case AnimationType.To:
				thickness = defaultOriginValue;
				thickness2 = this._keyValues[0];
				flag = true;
				break;
			case AnimationType.By:
				thickness2 = this._keyValues[0];
				value2 = defaultOriginValue;
				flag = true;
				break;
			case AnimationType.FromTo:
				thickness = this._keyValues[0];
				thickness2 = this._keyValues[1];
				if (this.IsAdditive)
				{
					value2 = defaultOriginValue;
					flag = true;
				}
				break;
			case AnimationType.FromBy:
				thickness = this._keyValues[0];
				thickness2 = AnimatedTypeHelpers.AddThickness(this._keyValues[0], this._keyValues[1]);
				if (this.IsAdditive)
				{
					value2 = defaultOriginValue;
					flag = true;
				}
				break;
			}
			if (flag && !AnimatedTypeHelpers.IsValidAnimationValueThickness(defaultOriginValue))
			{
				throw new InvalidOperationException(SR.Get("Animation_Invalid_DefaultValue", new object[]
				{
					base.GetType(),
					"origin",
					defaultOriginValue.ToString(CultureInfo.InvariantCulture)
				}));
			}
			if (flag2 && !AnimatedTypeHelpers.IsValidAnimationValueThickness(defaultDestinationValue))
			{
				throw new InvalidOperationException(SR.Get("Animation_Invalid_DefaultValue", new object[]
				{
					base.GetType(),
					"destination",
					defaultDestinationValue.ToString(CultureInfo.InvariantCulture)
				}));
			}
			if (this.IsCumulative)
			{
				double num2 = (double)(animationClock.CurrentIteration - 1).Value;
				if (num2 > 0.0)
				{
					value = AnimatedTypeHelpers.ScaleThickness(AnimatedTypeHelpers.SubtractThickness(thickness2, thickness), num2);
				}
			}
			return AnimatedTypeHelpers.AddThickness(value2, AnimatedTypeHelpers.AddThickness(value, AnimatedTypeHelpers.InterpolateThickness(thickness, thickness2, num)));
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x001DA674 File Offset: 0x001D9674
		private void ValidateAnimationFunction()
		{
			this._animationType = AnimationType.Automatic;
			this._keyValues = null;
			if (this.From != null)
			{
				if (this.To != null)
				{
					this._animationType = AnimationType.FromTo;
					this._keyValues = new Thickness[2];
					this._keyValues[0] = this.From.Value;
					this._keyValues[1] = this.To.Value;
				}
				else if (this.By != null)
				{
					this._animationType = AnimationType.FromBy;
					this._keyValues = new Thickness[2];
					this._keyValues[0] = this.From.Value;
					this._keyValues[1] = this.By.Value;
				}
				else
				{
					this._animationType = AnimationType.From;
					this._keyValues = new Thickness[1];
					this._keyValues[0] = this.From.Value;
				}
			}
			else if (this.To != null)
			{
				this._animationType = AnimationType.To;
				this._keyValues = new Thickness[1];
				this._keyValues[0] = this.To.Value;
			}
			else if (this.By != null)
			{
				this._animationType = AnimationType.By;
				this._keyValues = new Thickness[1];
				this._keyValues[0] = this.By.Value;
			}
			this._isAnimationFunctionValid = true;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x001DA80C File Offset: 0x001D980C
		private static void AnimationFunction_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ThicknessAnimation thicknessAnimation = (ThicknessAnimation)d;
			thicknessAnimation._isAnimationFunctionValid = false;
			thicknessAnimation.PropertyChanged(e.Property);
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x001DA828 File Offset: 0x001D9828
		private static bool ValidateFromToOrByValue(object value)
		{
			Thickness? thickness = (Thickness?)value;
			return thickness == null || AnimatedTypeHelpers.IsValidAnimationValueThickness(thickness.Value);
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x001DA853 File Offset: 0x001D9853
		// (set) Token: 0x06003436 RID: 13366 RVA: 0x001DA865 File Offset: 0x001D9865
		public Thickness? From
		{
			get
			{
				return (Thickness?)base.GetValue(ThicknessAnimation.FromProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.FromProperty, value);
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003437 RID: 13367 RVA: 0x001DA878 File Offset: 0x001D9878
		// (set) Token: 0x06003438 RID: 13368 RVA: 0x001DA88A File Offset: 0x001D988A
		public Thickness? To
		{
			get
			{
				return (Thickness?)base.GetValue(ThicknessAnimation.ToProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.ToProperty, value);
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003439 RID: 13369 RVA: 0x001DA89D File Offset: 0x001D989D
		// (set) Token: 0x0600343A RID: 13370 RVA: 0x001DA8AF File Offset: 0x001D98AF
		public Thickness? By
		{
			get
			{
				return (Thickness?)base.GetValue(ThicknessAnimation.ByProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.ByProperty, value);
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x001DA8C2 File Offset: 0x001D98C2
		// (set) Token: 0x0600343C RID: 13372 RVA: 0x001DA8D4 File Offset: 0x001D98D4
		public IEasingFunction EasingFunction
		{
			get
			{
				return (IEasingFunction)base.GetValue(ThicknessAnimation.EasingFunctionProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessAnimation.EasingFunctionProperty, value);
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x001DA8E2 File Offset: 0x001D98E2
		// (set) Token: 0x0600343E RID: 13374 RVA: 0x001DA8F4 File Offset: 0x001D98F4
		public bool IsAdditive
		{
			get
			{
				return (bool)base.GetValue(AnimationTimeline.IsAdditiveProperty);
			}
			set
			{
				base.SetValueInternal(AnimationTimeline.IsAdditiveProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600343F RID: 13375 RVA: 0x001DA907 File Offset: 0x001D9907
		// (set) Token: 0x06003440 RID: 13376 RVA: 0x001DA919 File Offset: 0x001D9919
		public bool IsCumulative
		{
			get
			{
				return (bool)base.GetValue(AnimationTimeline.IsCumulativeProperty);
			}
			set
			{
				base.SetValueInternal(AnimationTimeline.IsCumulativeProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x04001C4E RID: 7246
		private Thickness[] _keyValues;

		// Token: 0x04001C4F RID: 7247
		private AnimationType _animationType;

		// Token: 0x04001C50 RID: 7248
		private bool _isAnimationFunctionValid;

		// Token: 0x04001C51 RID: 7249
		public static readonly DependencyProperty FromProperty;

		// Token: 0x04001C52 RID: 7250
		public static readonly DependencyProperty ToProperty;

		// Token: 0x04001C53 RID: 7251
		public static readonly DependencyProperty ByProperty;

		// Token: 0x04001C54 RID: 7252
		public static readonly DependencyProperty EasingFunctionProperty;
	}
}
