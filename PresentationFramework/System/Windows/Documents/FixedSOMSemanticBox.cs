using System;

namespace System.Windows.Documents
{
	// Token: 0x02000611 RID: 1553
	internal abstract class FixedSOMSemanticBox : IComparable
	{
		// Token: 0x06004B91 RID: 19345 RVA: 0x002389D1 File Offset: 0x002379D1
		public FixedSOMSemanticBox()
		{
			this._boundingRect = Rect.Empty;
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x002389E4 File Offset: 0x002379E4
		public FixedSOMSemanticBox(Rect boundingRect)
		{
			this._boundingRect = boundingRect;
		}

		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x06004B93 RID: 19347 RVA: 0x002389F3 File Offset: 0x002379F3
		// (set) Token: 0x06004B94 RID: 19348 RVA: 0x002389FB File Offset: 0x002379FB
		public Rect BoundingRect
		{
			get
			{
				return this._boundingRect;
			}
			set
			{
				this._boundingRect = value;
			}
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public virtual void SetRTFProperties(FixedElement element)
		{
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x00238A04 File Offset: 0x00237A04
		public int CompareTo(object o)
		{
			if (!(o is FixedSOMSemanticBox))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedSOMSemanticBox)
				}), "o");
			}
			FixedSOMSemanticBox.SpatialComparison spatialComparison = this._CompareHorizontal(o as FixedSOMSemanticBox, false);
			FixedSOMSemanticBox.SpatialComparison spatialComparison2 = this._CompareVertical(o as FixedSOMSemanticBox);
			int result;
			if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Equal && spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				result = 0;
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Before || spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.OverlapBefore)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before || spatialComparison == FixedSOMSemanticBox.SpatialComparison.OverlapBefore)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before)
			{
				result = -1;
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.After)
			{
				result = 1;
			}
			else if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.Before)
			{
				result = -1;
			}
			else if (spatialComparison2 == FixedSOMSemanticBox.SpatialComparison.After)
			{
				result = 1;
			}
			else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.OverlapBefore)
			{
				result = -1;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x00238AC4 File Offset: 0x00237AC4
		int IComparable.CompareTo(object o)
		{
			return this.CompareTo(o);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x00238AD0 File Offset: 0x00237AD0
		protected FixedSOMSemanticBox.SpatialComparison _CompareHorizontal(FixedSOMSemanticBox otherBox, bool RTL)
		{
			Rect boundingRect = this.BoundingRect;
			Rect boundingRect2 = otherBox.BoundingRect;
			double num = RTL ? boundingRect.Right : boundingRect.Left;
			double num2 = RTL ? boundingRect2.Right : boundingRect2.Left;
			FixedSOMSemanticBox.SpatialComparison spatialComparison;
			if (num == num2)
			{
				spatialComparison = FixedSOMSemanticBox.SpatialComparison.Equal;
			}
			else if (boundingRect.Right < boundingRect2.Left)
			{
				spatialComparison = FixedSOMSemanticBox.SpatialComparison.Before;
			}
			else if (boundingRect2.Right < boundingRect.Left)
			{
				spatialComparison = FixedSOMSemanticBox.SpatialComparison.After;
			}
			else
			{
				double num3 = Math.Abs(num - num2);
				double num4 = (boundingRect.Width > boundingRect2.Width) ? boundingRect.Width : boundingRect2.Width;
				if (num3 / num4 < 0.1)
				{
					spatialComparison = FixedSOMSemanticBox.SpatialComparison.Equal;
				}
				else if (boundingRect.Left < boundingRect2.Left)
				{
					spatialComparison = FixedSOMSemanticBox.SpatialComparison.OverlapBefore;
				}
				else
				{
					spatialComparison = FixedSOMSemanticBox.SpatialComparison.OverlapAfter;
				}
			}
			if (RTL && spatialComparison != FixedSOMSemanticBox.SpatialComparison.Equal)
			{
				spatialComparison = this._InvertSpatialComparison(spatialComparison);
			}
			return spatialComparison;
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x00238BAC File Offset: 0x00237BAC
		protected FixedSOMSemanticBox.SpatialComparison _CompareVertical(FixedSOMSemanticBox otherBox)
		{
			Rect boundingRect = this.BoundingRect;
			Rect boundingRect2 = otherBox.BoundingRect;
			FixedSOMSemanticBox.SpatialComparison result;
			if (boundingRect.Top == boundingRect2.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.Equal;
			}
			else if (boundingRect.Bottom <= boundingRect2.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.Before;
			}
			else if (boundingRect2.Bottom <= boundingRect.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.After;
			}
			else if (boundingRect.Top < boundingRect2.Top)
			{
				result = FixedSOMSemanticBox.SpatialComparison.OverlapBefore;
			}
			else
			{
				result = FixedSOMSemanticBox.SpatialComparison.OverlapAfter;
			}
			return result;
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x00238C1C File Offset: 0x00237C1C
		protected int _SpatialToAbsoluteComparison(FixedSOMSemanticBox.SpatialComparison comparison)
		{
			int result = 0;
			switch (comparison)
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				result = -1;
				break;
			case FixedSOMSemanticBox.SpatialComparison.Equal:
				result = 0;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
			case FixedSOMSemanticBox.SpatialComparison.After:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x00238C54 File Offset: 0x00237C54
		protected FixedSOMSemanticBox.SpatialComparison _InvertSpatialComparison(FixedSOMSemanticBox.SpatialComparison comparison)
		{
			FixedSOMSemanticBox.SpatialComparison result = comparison;
			switch (comparison)
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
				result = FixedSOMSemanticBox.SpatialComparison.After;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				result = FixedSOMSemanticBox.SpatialComparison.OverlapAfter;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				result = FixedSOMSemanticBox.SpatialComparison.OverlapBefore;
				break;
			case FixedSOMSemanticBox.SpatialComparison.After:
				result = FixedSOMSemanticBox.SpatialComparison.Before;
				break;
			}
			return result;
		}

		// Token: 0x04002798 RID: 10136
		protected Rect _boundingRect;

		// Token: 0x02000B37 RID: 2871
		protected enum SpatialComparison
		{
			// Token: 0x04004823 RID: 18467
			None,
			// Token: 0x04004824 RID: 18468
			Before,
			// Token: 0x04004825 RID: 18469
			OverlapBefore,
			// Token: 0x04004826 RID: 18470
			Equal,
			// Token: 0x04004827 RID: 18471
			OverlapAfter,
			// Token: 0x04004828 RID: 18472
			After
		}
	}
}
