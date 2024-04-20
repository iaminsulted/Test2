using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000608 RID: 1544
	internal sealed class FixedSOMFixedBlock : FixedSOMPageElement
	{
		// Token: 0x06004B3B RID: 19259 RVA: 0x0023651F File Offset: 0x0023551F
		public FixedSOMFixedBlock(FixedSOMPage page) : base(page)
		{
		}

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06004B3C RID: 19260 RVA: 0x00236528 File Offset: 0x00235528
		public double LineHeight
		{
			get
			{
				FixedSOMTextRun lastTextRun = this.LastTextRun;
				if (lastTextRun != null)
				{
					if (base.SemanticBoxes.Count > 1)
					{
						FixedSOMTextRun fixedSOMTextRun = base.SemanticBoxes[base.SemanticBoxes.Count - 2] as FixedSOMTextRun;
						if (fixedSOMTextRun != null && lastTextRun.BoundingRect.Height / fixedSOMTextRun.BoundingRect.Height < 0.75 && fixedSOMTextRun.BoundingRect.Left != lastTextRun.BoundingRect.Left && fixedSOMTextRun.BoundingRect.Right != lastTextRun.BoundingRect.Right && fixedSOMTextRun.BoundingRect.Top != lastTextRun.BoundingRect.Top && fixedSOMTextRun.BoundingRect.Bottom != lastTextRun.BoundingRect.Bottom)
						{
							return fixedSOMTextRun.BoundingRect.Height;
						}
					}
					return lastTextRun.BoundingRect.Height;
				}
				return 0.0;
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x00236641 File Offset: 0x00235641
		public bool IsFloatingImage
		{
			get
			{
				return this._semanticBoxes.Count == 1 && this._semanticBoxes[0] is FixedSOMImage;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x06004B3E RID: 19262 RVA: 0x00236667 File Offset: 0x00235667
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[1];
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x00236670 File Offset: 0x00235670
		public bool IsWhiteSpace
		{
			get
			{
				if (this._semanticBoxes.Count == 0)
				{
					return false;
				}
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._semanticBoxes)
				{
					FixedSOMTextRun fixedSOMTextRun = fixedSOMSemanticBox as FixedSOMTextRun;
					if (fixedSOMTextRun == null || !fixedSOMTextRun.IsWhiteSpace)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06004B40 RID: 19264 RVA: 0x002366E4 File Offset: 0x002356E4
		public override bool IsRTL
		{
			get
			{
				return this._RTLCount > this._LTRCount;
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x002366F4 File Offset: 0x002356F4
		public Matrix Matrix
		{
			get
			{
				return this._matrix;
			}
		}

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x002366FC File Offset: 0x002356FC
		private FixedSOMTextRun LastTextRun
		{
			get
			{
				FixedSOMTextRun fixedSOMTextRun = null;
				int num = this._semanticBoxes.Count - 1;
				while (num >= 0 && fixedSOMTextRun == null)
				{
					fixedSOMTextRun = (this._semanticBoxes[num] as FixedSOMTextRun);
					num--;
				}
				return fixedSOMTextRun;
			}
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x0023673C File Offset: 0x0023573C
		public void CombineWith(FixedSOMFixedBlock block)
		{
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in block.SemanticBoxes)
			{
				FixedSOMTextRun fixedSOMTextRun = fixedSOMSemanticBox as FixedSOMTextRun;
				if (fixedSOMTextRun != null)
				{
					this.AddTextRun(fixedSOMTextRun);
				}
				else
				{
					base.Add(fixedSOMSemanticBox);
				}
			}
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x002367A4 File Offset: 0x002357A4
		public void AddTextRun(FixedSOMTextRun textRun)
		{
			this._AddElement(textRun);
			textRun.FixedBlock = this;
			if (!textRun.IsWhiteSpace)
			{
				if (textRun.IsLTR)
				{
					this._LTRCount++;
					return;
				}
				this._RTLCount++;
			}
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x002367E1 File Offset: 0x002357E1
		public void AddImage(FixedSOMImage image)
		{
			this._AddElement(image);
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x002367EA File Offset: 0x002357EA
		public override void SetRTFProperties(FixedElement element)
		{
			if (this.IsRTL)
			{
				element.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
			}
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x00236808 File Offset: 0x00235808
		private void _AddElement(FixedSOMElement element)
		{
			base.Add(element);
			if (this._semanticBoxes.Count == 1)
			{
				this._matrix = element.Matrix;
				this._matrix.OffsetX = 0.0;
				this._matrix.OffsetY = 0.0;
			}
		}

		// Token: 0x04002772 RID: 10098
		private int _RTLCount;

		// Token: 0x04002773 RID: 10099
		private int _LTRCount;

		// Token: 0x04002774 RID: 10100
		private Matrix _matrix;
	}
}
