using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000613 RID: 1555
	internal sealed class FixedSOMTableCell : FixedSOMContainer
	{
		// Token: 0x06004BA6 RID: 19366 RVA: 0x00239165 File Offset: 0x00238165
		public FixedSOMTableCell(double left, double top, double right, double bottom)
		{
			this._boundingRect = new Rect(new Point(left, top), new Point(right, bottom));
			this._containsTable = false;
			this._columnSpan = 1;
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x00239195 File Offset: 0x00238195
		public void AddContainer(FixedSOMContainer container)
		{
			if (!this._containsTable || !this._AddToInnerTable(container))
			{
				base.Add(container);
			}
			if (container is FixedSOMTable)
			{
				this._containsTable = true;
			}
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x002391C0 File Offset: 0x002381C0
		public override void SetRTFProperties(FixedElement element)
		{
			element.SetValue(Block.BorderThicknessProperty, new Thickness(1.0));
			element.SetValue(Block.BorderBrushProperty, Brushes.Black);
			element.SetValue(TableCell.ColumnSpanProperty, this._columnSpan);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x00239214 File Offset: 0x00238214
		private bool _AddToInnerTable(FixedSOMContainer container)
		{
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._semanticBoxes)
			{
				FixedSOMTable fixedSOMTable = fixedSOMSemanticBox as FixedSOMTable;
				if (fixedSOMTable != null && fixedSOMTable.AddContainer(container))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06004BAA RID: 19370 RVA: 0x00239278 File Offset: 0x00238278
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.TableCell
				};
			}
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06004BAB RID: 19371 RVA: 0x00239288 File Offset: 0x00238288
		internal bool IsEmpty
		{
			get
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMContainer fixedSOMContainer = (FixedSOMContainer)fixedSOMSemanticBox;
					FixedSOMTable fixedSOMTable = fixedSOMContainer as FixedSOMTable;
					if (fixedSOMTable != null && !fixedSOMTable.IsEmpty)
					{
						return false;
					}
					FixedSOMFixedBlock fixedSOMFixedBlock = fixedSOMContainer as FixedSOMFixedBlock;
					if (fixedSOMFixedBlock != null && !fixedSOMFixedBlock.IsWhiteSpace)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x06004BAC RID: 19372 RVA: 0x0023930C File Offset: 0x0023830C
		// (set) Token: 0x06004BAD RID: 19373 RVA: 0x00239314 File Offset: 0x00238314
		internal int ColumnSpan
		{
			get
			{
				return this._columnSpan;
			}
			set
			{
				this._columnSpan = value;
			}
		}

		// Token: 0x0400279E RID: 10142
		private bool _containsTable;

		// Token: 0x0400279F RID: 10143
		private int _columnSpan;
	}
}
