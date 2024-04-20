using System;

namespace System.Windows.Documents
{
	// Token: 0x02000609 RID: 1545
	internal class FixedSOMGroup : FixedSOMPageElement, IComparable
	{
		// Token: 0x06004B48 RID: 19272 RVA: 0x0023651F File Offset: 0x0023551F
		public FixedSOMGroup(FixedSOMPage page) : base(page)
		{
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x00236860 File Offset: 0x00235860
		int IComparable.CompareTo(object comparedObj)
		{
			int result = int.MinValue;
			FixedSOMGroup fixedSOMGroup = comparedObj as FixedSOMGroup;
			if (fixedSOMGroup == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					comparedObj.GetType(),
					typeof(FixedSOMGroup)
				}), "comparedObj");
			}
			bool rtl = this.IsRTL && fixedSOMGroup.IsRTL;
			FixedSOMSemanticBox.SpatialComparison spatialComparison = base._CompareHorizontal(fixedSOMGroup, rtl);
			switch (base._CompareVertical(fixedSOMGroup))
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
				result = -1;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				if (spatialComparison <= FixedSOMSemanticBox.SpatialComparison.Equal)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.Equal:
				switch (spatialComparison)
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
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				if (spatialComparison >= FixedSOMSemanticBox.SpatialComparison.Equal)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.After:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x0023693C File Offset: 0x0023593C
		public void AddContainer(FixedSOMPageElement pageElement)
		{
			FixedSOMFixedBlock fixedSOMFixedBlock = pageElement as FixedSOMFixedBlock;
			if (fixedSOMFixedBlock == null || (!fixedSOMFixedBlock.IsFloatingImage && !fixedSOMFixedBlock.IsWhiteSpace))
			{
				if (pageElement.IsRTL)
				{
					this._RTLCount++;
				}
				else
				{
					this._LTRCount++;
				}
			}
			this._semanticBoxes.Add(pageElement);
			if (this._boundingRect.IsEmpty)
			{
				this._boundingRect = pageElement.BoundingRect;
				return;
			}
			this._boundingRect.Union(pageElement.BoundingRect);
		}

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x06004B4B RID: 19275 RVA: 0x002369C0 File Offset: 0x002359C0
		public override bool IsRTL
		{
			get
			{
				return this._RTLCount > this._LTRCount;
			}
		}

		// Token: 0x04002775 RID: 10101
		private int _RTLCount;

		// Token: 0x04002776 RID: 10102
		private int _LTRCount;
	}
}
