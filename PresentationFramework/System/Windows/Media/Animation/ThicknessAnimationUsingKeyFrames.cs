using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000435 RID: 1077
	[ContentProperty("KeyFrames")]
	public class ThicknessAnimationUsingKeyFrames : ThicknessAnimationBase, IKeyFrameAnimation, IAddChild
	{
		// Token: 0x06003447 RID: 13383 RVA: 0x001DA9B3 File Offset: 0x001D99B3
		public ThicknessAnimationUsingKeyFrames()
		{
			this._areKeyTimesValid = true;
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x001DA9C2 File Offset: 0x001D99C2
		public new ThicknessAnimationUsingKeyFrames Clone()
		{
			return (ThicknessAnimationUsingKeyFrames)base.Clone();
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x001DA9CF File Offset: 0x001D99CF
		public new ThicknessAnimationUsingKeyFrames CloneCurrentValue()
		{
			return (ThicknessAnimationUsingKeyFrames)base.CloneCurrentValue();
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x001DA9DC File Offset: 0x001D99DC
		protected override bool FreezeCore(bool isChecking)
		{
			bool flag = base.FreezeCore(isChecking) & Freezable.Freeze(this._keyFrames, isChecking);
			if (flag & !this._areKeyTimesValid)
			{
				this.ResolveKeyTimes();
			}
			return flag;
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x001DAA05 File Offset: 0x001D9A05
		protected override void OnChanged()
		{
			this._areKeyTimesValid = false;
			base.OnChanged();
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x001DAA14 File Offset: 0x001D9A14
		protected override Freezable CreateInstanceCore()
		{
			return new ThicknessAnimationUsingKeyFrames();
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x001DAA1C File Offset: 0x001D9A1C
		protected override void CloneCore(Freezable sourceFreezable)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)sourceFreezable;
			base.CloneCore(sourceFreezable);
			this.CopyCommon(sourceAnimation, false);
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x001DAA40 File Offset: 0x001D9A40
		protected override void CloneCurrentValueCore(Freezable sourceFreezable)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)sourceFreezable;
			base.CloneCurrentValueCore(sourceFreezable);
			this.CopyCommon(sourceAnimation, true);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x001DAA64 File Offset: 0x001D9A64
		protected override void GetAsFrozenCore(Freezable source)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)source;
			base.GetAsFrozenCore(source);
			this.CopyCommon(sourceAnimation, false);
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x001DAA88 File Offset: 0x001D9A88
		protected override void GetCurrentValueAsFrozenCore(Freezable source)
		{
			ThicknessAnimationUsingKeyFrames sourceAnimation = (ThicknessAnimationUsingKeyFrames)source;
			base.GetCurrentValueAsFrozenCore(source);
			this.CopyCommon(sourceAnimation, true);
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x001DAAAC File Offset: 0x001D9AAC
		private void CopyCommon(ThicknessAnimationUsingKeyFrames sourceAnimation, bool isCurrentValueClone)
		{
			this._areKeyTimesValid = sourceAnimation._areKeyTimesValid;
			if (this._areKeyTimesValid && sourceAnimation._sortedResolvedKeyFrames != null)
			{
				this._sortedResolvedKeyFrames = (ResolvedKeyFrameEntry[])sourceAnimation._sortedResolvedKeyFrames.Clone();
			}
			if (sourceAnimation._keyFrames != null)
			{
				if (isCurrentValueClone)
				{
					this._keyFrames = (ThicknessKeyFrameCollection)sourceAnimation._keyFrames.CloneCurrentValue();
				}
				else
				{
					this._keyFrames = sourceAnimation._keyFrames.Clone();
				}
				base.OnFreezablePropertyChanged(null, this._keyFrames);
			}
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x001DAB2C File Offset: 0x001D9B2C
		void IAddChild.AddChild(object child)
		{
			base.WritePreamble();
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			this.AddChild(child);
			base.WritePostscript();
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x001DAB50 File Offset: 0x001D9B50
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void AddChild(object child)
		{
			ThicknessKeyFrame thicknessKeyFrame = child as ThicknessKeyFrame;
			if (thicknessKeyFrame != null)
			{
				this.KeyFrames.Add(thicknessKeyFrame);
				return;
			}
			throw new ArgumentException(SR.Get("Animation_ChildMustBeKeyFrame"), "child");
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x001DAB89 File Offset: 0x001D9B89
		void IAddChild.AddText(string childText)
		{
			if (childText == null)
			{
				throw new ArgumentNullException("childText");
			}
			this.AddText(childText);
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x001DABA0 File Offset: 0x001D9BA0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void AddText(string childText)
		{
			throw new InvalidOperationException(SR.Get("Animation_NoTextChildren"));
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x001DABB4 File Offset: 0x001D9BB4
		protected sealed override Thickness GetCurrentValueCore(Thickness defaultOriginValue, Thickness defaultDestinationValue, AnimationClock animationClock)
		{
			if (this._keyFrames == null)
			{
				return defaultDestinationValue;
			}
			if (!this._areKeyTimesValid)
			{
				this.ResolveKeyTimes();
			}
			if (this._sortedResolvedKeyFrames == null)
			{
				return defaultDestinationValue;
			}
			TimeSpan value = animationClock.CurrentTime.Value;
			int num = this._sortedResolvedKeyFrames.Length;
			int num2 = num - 1;
			int i;
			for (i = 0; i < num; i++)
			{
				if (!(value > this._sortedResolvedKeyFrames[i]._resolvedKeyTime))
				{
					break;
				}
			}
			while (i < num2 && value == this._sortedResolvedKeyFrames[i + 1]._resolvedKeyTime)
			{
				i++;
			}
			Thickness thickness;
			if (i == num)
			{
				thickness = this.GetResolvedKeyFrameValue(num2);
			}
			else if (value == this._sortedResolvedKeyFrames[i]._resolvedKeyTime)
			{
				thickness = this.GetResolvedKeyFrameValue(i);
			}
			else
			{
				Thickness baseValue;
				double keyFrameProgress;
				if (i == 0)
				{
					if (this.IsAdditive)
					{
						baseValue = AnimatedTypeHelpers.GetZeroValueThickness(defaultOriginValue);
					}
					else
					{
						baseValue = defaultOriginValue;
					}
					keyFrameProgress = value.TotalMilliseconds / this._sortedResolvedKeyFrames[0]._resolvedKeyTime.TotalMilliseconds;
				}
				else
				{
					int num3 = i - 1;
					TimeSpan resolvedKeyTime = this._sortedResolvedKeyFrames[num3]._resolvedKeyTime;
					baseValue = this.GetResolvedKeyFrameValue(num3);
					TimeSpan timeSpan = value - resolvedKeyTime;
					TimeSpan timeSpan2 = this._sortedResolvedKeyFrames[i]._resolvedKeyTime - resolvedKeyTime;
					keyFrameProgress = timeSpan.TotalMilliseconds / timeSpan2.TotalMilliseconds;
				}
				thickness = this.GetResolvedKeyFrame(i).InterpolateValue(baseValue, keyFrameProgress);
			}
			if (this.IsCumulative)
			{
				double num4 = (double)(animationClock.CurrentIteration - 1).Value;
				if (num4 > 0.0)
				{
					thickness = AnimatedTypeHelpers.AddThickness(thickness, AnimatedTypeHelpers.ScaleThickness(this.GetResolvedKeyFrameValue(num2), num4));
				}
			}
			if (this.IsAdditive)
			{
				return AnimatedTypeHelpers.AddThickness(defaultOriginValue, thickness);
			}
			return thickness;
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x001DADB1 File Offset: 0x001D9DB1
		protected sealed override Duration GetNaturalDurationCore(Clock clock)
		{
			return new Duration(this.LargestTimeSpanKeyTime);
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x001DADBE File Offset: 0x001D9DBE
		// (set) Token: 0x06003459 RID: 13401 RVA: 0x001DADC6 File Offset: 0x001D9DC6
		IList IKeyFrameAnimation.KeyFrames
		{
			get
			{
				return this.KeyFrames;
			}
			set
			{
				this.KeyFrames = (ThicknessKeyFrameCollection)value;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x001DADD4 File Offset: 0x001D9DD4
		// (set) Token: 0x0600345B RID: 13403 RVA: 0x001DAE2E File Offset: 0x001D9E2E
		public ThicknessKeyFrameCollection KeyFrames
		{
			get
			{
				base.ReadPreamble();
				if (this._keyFrames == null)
				{
					if (base.IsFrozen)
					{
						this._keyFrames = ThicknessKeyFrameCollection.Empty;
					}
					else
					{
						base.WritePreamble();
						this._keyFrames = new ThicknessKeyFrameCollection();
						base.OnFreezablePropertyChanged(null, this._keyFrames);
						base.WritePostscript();
					}
				}
				return this._keyFrames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.WritePreamble();
				if (value != this._keyFrames)
				{
					base.OnFreezablePropertyChanged(this._keyFrames, value);
					this._keyFrames = value;
					base.WritePostscript();
				}
			}
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x001DAE67 File Offset: 0x001D9E67
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeKeyFrames()
		{
			base.ReadPreamble();
			return this._keyFrames != null && this._keyFrames.Count > 0;
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x001DA8E2 File Offset: 0x001D98E2
		// (set) Token: 0x0600345E RID: 13406 RVA: 0x001DA8F4 File Offset: 0x001D98F4
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

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x001DA907 File Offset: 0x001D9907
		// (set) Token: 0x06003460 RID: 13408 RVA: 0x001DA919 File Offset: 0x001D9919
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

		// Token: 0x06003461 RID: 13409 RVA: 0x001DAE87 File Offset: 0x001D9E87
		private Thickness GetResolvedKeyFrameValue(int resolvedKeyFrameIndex)
		{
			return this.GetResolvedKeyFrame(resolvedKeyFrameIndex).Value;
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x001DAE95 File Offset: 0x001D9E95
		private ThicknessKeyFrame GetResolvedKeyFrame(int resolvedKeyFrameIndex)
		{
			return this._keyFrames[this._sortedResolvedKeyFrames[resolvedKeyFrameIndex]._originalKeyFrameIndex];
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x001DAEB4 File Offset: 0x001D9EB4
		private TimeSpan LargestTimeSpanKeyTime
		{
			get
			{
				bool flag = false;
				TimeSpan timeSpan = TimeSpan.Zero;
				if (this._keyFrames != null)
				{
					int count = this._keyFrames.Count;
					for (int i = 0; i < count; i++)
					{
						KeyTime keyTime = this._keyFrames[i].KeyTime;
						if (keyTime.Type == KeyTimeType.TimeSpan)
						{
							flag = true;
							if (keyTime.TimeSpan > timeSpan)
							{
								timeSpan = keyTime.TimeSpan;
							}
						}
					}
				}
				if (flag)
				{
					return timeSpan;
				}
				return TimeSpan.FromSeconds(1.0);
			}
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x001DAF34 File Offset: 0x001D9F34
		private void ResolveKeyTimes()
		{
			int num = 0;
			if (this._keyFrames != null)
			{
				num = this._keyFrames.Count;
			}
			if (num == 0)
			{
				this._sortedResolvedKeyFrames = null;
				this._areKeyTimesValid = true;
				return;
			}
			this._sortedResolvedKeyFrames = new ResolvedKeyFrameEntry[num];
			int i;
			for (i = 0; i < num; i++)
			{
				this._sortedResolvedKeyFrames[i]._originalKeyFrameIndex = i;
			}
			TimeSpan resolvedKeyTime = TimeSpan.Zero;
			Duration duration = base.Duration;
			if (duration.HasTimeSpan)
			{
				resolvedKeyTime = duration.TimeSpan;
			}
			else
			{
				resolvedKeyTime = this.LargestTimeSpanKeyTime;
			}
			int num2 = num - 1;
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			i = 0;
			while (i < num)
			{
				KeyTime keyTime = this._keyFrames[i].KeyTime;
				switch (keyTime.Type)
				{
				case KeyTimeType.Uniform:
				case KeyTimeType.Paced:
					if (i == num2)
					{
						this._sortedResolvedKeyFrames[i]._resolvedKeyTime = resolvedKeyTime;
						i++;
					}
					else if (i == 0 && keyTime.Type == KeyTimeType.Paced)
					{
						this._sortedResolvedKeyFrames[i]._resolvedKeyTime = TimeSpan.Zero;
						i++;
					}
					else
					{
						if (keyTime.Type == KeyTimeType.Paced)
						{
							flag = true;
						}
						ThicknessAnimationUsingKeyFrames.KeyTimeBlock keyTimeBlock = default(ThicknessAnimationUsingKeyFrames.KeyTimeBlock);
						keyTimeBlock.BeginIndex = i;
						while (++i < num2)
						{
							KeyTimeType type = this._keyFrames[i].KeyTime.Type;
							if (type == KeyTimeType.Percent || type == KeyTimeType.TimeSpan)
							{
								break;
							}
							if (type == KeyTimeType.Paced)
							{
								flag = true;
							}
						}
						keyTimeBlock.EndIndex = i;
						arrayList.Add(keyTimeBlock);
					}
					break;
				case KeyTimeType.Percent:
					this._sortedResolvedKeyFrames[i]._resolvedKeyTime = TimeSpan.FromMilliseconds(keyTime.Percent * resolvedKeyTime.TotalMilliseconds);
					i++;
					break;
				case KeyTimeType.TimeSpan:
					this._sortedResolvedKeyFrames[i]._resolvedKeyTime = keyTime.TimeSpan;
					i++;
					break;
				}
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				ThicknessAnimationUsingKeyFrames.KeyTimeBlock keyTimeBlock2 = (ThicknessAnimationUsingKeyFrames.KeyTimeBlock)arrayList[j];
				TimeSpan timeSpan = TimeSpan.Zero;
				if (keyTimeBlock2.BeginIndex > 0)
				{
					timeSpan = this._sortedResolvedKeyFrames[keyTimeBlock2.BeginIndex - 1]._resolvedKeyTime;
				}
				long num3 = (long)(keyTimeBlock2.EndIndex - keyTimeBlock2.BeginIndex + 1);
				TimeSpan t = TimeSpan.FromTicks((this._sortedResolvedKeyFrames[keyTimeBlock2.EndIndex]._resolvedKeyTime - timeSpan).Ticks / num3);
				i = keyTimeBlock2.BeginIndex;
				TimeSpan timeSpan2 = timeSpan + t;
				while (i < keyTimeBlock2.EndIndex)
				{
					this._sortedResolvedKeyFrames[i]._resolvedKeyTime = timeSpan2;
					timeSpan2 += t;
					i++;
				}
			}
			if (flag)
			{
				this.ResolvePacedKeyTimes();
			}
			Array.Sort<ResolvedKeyFrameEntry>(this._sortedResolvedKeyFrames);
			this._areKeyTimesValid = true;
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x001DB210 File Offset: 0x001DA210
		private void ResolvePacedKeyTimes()
		{
			int num = 1;
			int num2 = this._sortedResolvedKeyFrames.Length - 1;
			do
			{
				if (this._keyFrames[num].KeyTime.Type == KeyTimeType.Paced)
				{
					int num3 = num;
					List<double> list = new List<double>();
					TimeSpan resolvedKeyTime = this._sortedResolvedKeyFrames[num - 1]._resolvedKeyTime;
					double num4 = 0.0;
					Thickness from = this._keyFrames[num - 1].Value;
					do
					{
						Thickness value = this._keyFrames[num].Value;
						num4 += AnimatedTypeHelpers.GetSegmentLengthThickness(from, value);
						list.Add(num4);
						from = value;
						num++;
					}
					while (num < num2 && this._keyFrames[num].KeyTime.Type == KeyTimeType.Paced);
					num4 += AnimatedTypeHelpers.GetSegmentLengthThickness(from, this._keyFrames[num].Value);
					TimeSpan timeSpan = this._sortedResolvedKeyFrames[num]._resolvedKeyTime - resolvedKeyTime;
					int i = 0;
					int num5 = num3;
					while (i < list.Count)
					{
						this._sortedResolvedKeyFrames[num5]._resolvedKeyTime = resolvedKeyTime + TimeSpan.FromMilliseconds(list[i] / num4 * timeSpan.TotalMilliseconds);
						i++;
						num5++;
					}
				}
				else
				{
					num++;
				}
			}
			while (num < num2);
		}

		// Token: 0x04001C55 RID: 7253
		private ThicknessKeyFrameCollection _keyFrames;

		// Token: 0x04001C56 RID: 7254
		private ResolvedKeyFrameEntry[] _sortedResolvedKeyFrames;

		// Token: 0x04001C57 RID: 7255
		private bool _areKeyTimesValid;

		// Token: 0x02000AC0 RID: 2752
		private struct KeyTimeBlock
		{
			// Token: 0x04004646 RID: 17990
			public int BeginIndex;

			// Token: 0x04004647 RID: 17991
			public int EndIndex;
		}
	}
}
