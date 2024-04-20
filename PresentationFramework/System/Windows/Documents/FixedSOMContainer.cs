using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x02000606 RID: 1542
	internal abstract class FixedSOMContainer : FixedSOMSemanticBox, IComparable
	{
		// Token: 0x06004B26 RID: 19238 RVA: 0x0023601D File Offset: 0x0023501D
		protected FixedSOMContainer()
		{
			this._semanticBoxes = new List<FixedSOMSemanticBox>();
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x00236030 File Offset: 0x00235030
		int IComparable.CompareTo(object comparedObj)
		{
			int num = int.MinValue;
			FixedSOMPageElement fixedSOMPageElement = comparedObj as FixedSOMPageElement;
			FixedSOMPageElement fixedSOMPageElement2 = this as FixedSOMPageElement;
			if (fixedSOMPageElement == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					comparedObj.GetType(),
					typeof(FixedSOMContainer)
				}), "comparedObj");
			}
			FixedSOMSemanticBox.SpatialComparison comparison = base._CompareHorizontal(fixedSOMPageElement, false);
			FixedSOMSemanticBox.SpatialComparison spatialComparison = base._CompareVertical(fixedSOMPageElement);
			switch (comparison)
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
				if (spatialComparison != FixedSOMSemanticBox.SpatialComparison.After)
				{
					num = -1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before)
				{
					num = -1;
				}
				else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.After)
				{
					num = 1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.Equal:
				switch (spatialComparison)
				{
				case FixedSOMSemanticBox.SpatialComparison.Before:
				case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
					num = -1;
					break;
				case FixedSOMSemanticBox.SpatialComparison.Equal:
					num = 0;
					break;
				case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				case FixedSOMSemanticBox.SpatialComparison.After:
					num = 1;
					break;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.After)
				{
					num = 1;
				}
				else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before)
				{
					num = -1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.After:
				if (spatialComparison != FixedSOMSemanticBox.SpatialComparison.Before)
				{
					num = 1;
				}
				break;
			}
			if (num == -2147483648)
			{
				if (fixedSOMPageElement2.FixedNodes.Count == 0 || fixedSOMPageElement.FixedNodes.Count == 0)
				{
					num = 0;
				}
				else
				{
					FixedNode item = fixedSOMPageElement2.FixedNodes[0];
					FixedNode item2 = fixedSOMPageElement2.FixedNodes[fixedSOMPageElement2.FixedNodes.Count - 1];
					FixedNode item3 = fixedSOMPageElement.FixedNodes[0];
					FixedNode item4 = fixedSOMPageElement.FixedNodes[fixedSOMPageElement.FixedNodes.Count - 1];
					if (fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item3) - fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item2) == 1)
					{
						num = -1;
					}
					else if (fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item4) - fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item) == 1)
					{
						num = 1;
					}
					else
					{
						int num2 = base._SpatialToAbsoluteComparison(spatialComparison);
						num = ((num2 != 0) ? num2 : base._SpatialToAbsoluteComparison(comparison));
					}
				}
			}
			return num;
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x00236204 File Offset: 0x00235204
		protected void AddSorted(FixedSOMSemanticBox box)
		{
			int num = this._semanticBoxes.Count - 1;
			while (num >= 0 && box.CompareTo(this._semanticBoxes[num]) != 1)
			{
				num--;
			}
			this._semanticBoxes.Insert(num + 1, box);
			this._UpdateBoundingRect(box.BoundingRect);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x00236259 File Offset: 0x00235259
		protected void Add(FixedSOMSemanticBox box)
		{
			this._semanticBoxes.Add(box);
			this._UpdateBoundingRect(box.BoundingRect);
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x06004B2A RID: 19242 RVA: 0x00236273 File Offset: 0x00235273
		internal virtual FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return Array.Empty<FixedElement.ElementType>();
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x0023627A File Offset: 0x0023527A
		// (set) Token: 0x06004B2C RID: 19244 RVA: 0x00236282 File Offset: 0x00235282
		public List<FixedSOMSemanticBox> SemanticBoxes
		{
			get
			{
				return this._semanticBoxes;
			}
			set
			{
				this._semanticBoxes = value;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0023628B File Offset: 0x0023528B
		public List<FixedNode> FixedNodes
		{
			get
			{
				if (this._fixedNodes == null)
				{
					this._ConstructFixedNodes();
				}
				return this._fixedNodes;
			}
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x002362A4 File Offset: 0x002352A4
		private void _ConstructFixedNodes()
		{
			this._fixedNodes = new List<FixedNode>();
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._semanticBoxes)
			{
				FixedSOMElement fixedSOMElement = fixedSOMSemanticBox as FixedSOMElement;
				if (fixedSOMElement != null)
				{
					this._fixedNodes.Add(fixedSOMElement.FixedNode);
				}
				else
				{
					foreach (FixedNode item in (fixedSOMSemanticBox as FixedSOMContainer).FixedNodes)
					{
						this._fixedNodes.Add(item);
					}
				}
			}
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x00236368 File Offset: 0x00235368
		private void _UpdateBoundingRect(Rect rect)
		{
			if (this._boundingRect.IsEmpty)
			{
				this._boundingRect = rect;
				return;
			}
			this._boundingRect.Union(rect);
		}

		// Token: 0x0400276A RID: 10090
		protected List<FixedSOMSemanticBox> _semanticBoxes;

		// Token: 0x0400276B RID: 10091
		protected List<FixedNode> _fixedNodes;
	}
}
