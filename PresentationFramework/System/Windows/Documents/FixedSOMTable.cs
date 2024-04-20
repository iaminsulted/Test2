using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x02000612 RID: 1554
	internal sealed class FixedSOMTable : FixedSOMPageElement
	{
		// Token: 0x06004B9C RID: 19356 RVA: 0x00238C90 File Offset: 0x00237C90
		public FixedSOMTable(FixedSOMPage page) : base(page)
		{
			this._numCols = 0;
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x00238CA0 File Offset: 0x00237CA0
		public void AddRow(FixedSOMTableRow row)
		{
			base.Add(row);
			int count = row.SemanticBoxes.Count;
			if (count > this._numCols)
			{
				this._numCols = count;
			}
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x00238CD0 File Offset: 0x00237CD0
		public bool AddContainer(FixedSOMContainer container)
		{
			Rect boundingRect = container.BoundingRect;
			double num = boundingRect.Height * 0.2;
			double num2 = boundingRect.Width * 0.2;
			boundingRect.Inflate(-num2, -num);
			if (base.BoundingRect.Contains(boundingRect))
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMTableRow fixedSOMTableRow = (FixedSOMTableRow)fixedSOMSemanticBox;
					if (fixedSOMTableRow.BoundingRect.Contains(boundingRect))
					{
						foreach (FixedSOMSemanticBox fixedSOMSemanticBox2 in fixedSOMTableRow.SemanticBoxes)
						{
							FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMSemanticBox2;
							if (fixedSOMTableCell.BoundingRect.Contains(boundingRect))
							{
								fixedSOMTableCell.AddContainer(container);
								FixedSOMFixedBlock fixedSOMFixedBlock = container as FixedSOMFixedBlock;
								if (fixedSOMFixedBlock != null)
								{
									if (fixedSOMFixedBlock.IsRTL)
									{
										this._RTLCount++;
									}
									else
									{
										this._LTRCount++;
									}
								}
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x00238E24 File Offset: 0x00237E24
		public override void SetRTFProperties(FixedElement element)
		{
			if (element.Type == typeof(Table))
			{
				element.SetValue(Table.CellSpacingProperty, 0.0);
			}
		}

		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x00238E56 File Offset: 0x00237E56
		public override bool IsRTL
		{
			get
			{
				return this._RTLCount > this._LTRCount;
			}
		}

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x06004BA1 RID: 19361 RVA: 0x00238E66 File Offset: 0x00237E66
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.Table,
					FixedElement.ElementType.TableRowGroup
				};
			}
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x00238E78 File Offset: 0x00237E78
		internal bool IsEmpty
		{
			get
			{
				using (List<FixedSOMSemanticBox>.Enumerator enumerator = base.SemanticBoxes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!((FixedSOMTableRow)enumerator.Current).IsEmpty)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06004BA3 RID: 19363 RVA: 0x00238ED8 File Offset: 0x00237ED8
		internal bool IsSingleCelled
		{
			get
			{
				return base.SemanticBoxes.Count == 1 && (base.SemanticBoxes[0] as FixedSOMTableRow).SemanticBoxes.Count == 1;
			}
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x00238F08 File Offset: 0x00237F08
		internal void DeleteEmptyRows()
		{
			int i = 0;
			while (i < base.SemanticBoxes.Count)
			{
				FixedSOMTableRow fixedSOMTableRow = base.SemanticBoxes[i] as FixedSOMTableRow;
				if (fixedSOMTableRow != null && fixedSOMTableRow.IsEmpty && fixedSOMTableRow.BoundingRect.Height < 10.0)
				{
					base.SemanticBoxes.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x00238F70 File Offset: 0x00237F70
		internal void DeleteEmptyColumns()
		{
			int count = base.SemanticBoxes.Count;
			int[] array = new int[count];
			for (;;)
			{
				double num = double.MaxValue;
				bool flag = true;
				for (int i = 0; i < count; i++)
				{
					FixedSOMTableRow fixedSOMTableRow = (FixedSOMTableRow)base.SemanticBoxes[i];
					int num2 = array[i];
					flag = (flag && num2 < fixedSOMTableRow.SemanticBoxes.Count);
					if (flag)
					{
						FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMTableRow.SemanticBoxes[num2];
						flag = (fixedSOMTableCell.IsEmpty && fixedSOMTableCell.BoundingRect.Width < 5.0);
					}
					if (num2 + 1 < fixedSOMTableRow.SemanticBoxes.Count)
					{
						FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMTableRow.SemanticBoxes[num2 + 1];
						double left = fixedSOMTableCell.BoundingRect.Left;
						if (left < num)
						{
							if (num != 1.7976931348623157E+308)
							{
								flag = false;
							}
							num = left;
						}
						else if (left > num)
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					for (int i = 0; i < count; i++)
					{
						((FixedSOMTableRow)base.SemanticBoxes[i]).SemanticBoxes.RemoveAt(array[i]);
					}
					if (num == 1.7976931348623157E+308)
					{
						break;
					}
				}
				else
				{
					if (num == 1.7976931348623157E+308)
					{
						return;
					}
					for (int i = 0; i < count; i++)
					{
						FixedSOMTableRow fixedSOMTableRow2 = (FixedSOMTableRow)base.SemanticBoxes[i];
						int num3 = array[i];
						if (num3 + 1 < fixedSOMTableRow2.SemanticBoxes.Count && fixedSOMTableRow2.SemanticBoxes[num3 + 1].BoundingRect.Left == num)
						{
							array[i] = num3 + 1;
						}
						else
						{
							FixedSOMTableCell fixedSOMTableCell2 = (FixedSOMTableCell)fixedSOMTableRow2.SemanticBoxes[num3];
							int columnSpan = fixedSOMTableCell2.ColumnSpan;
							fixedSOMTableCell2.ColumnSpan = columnSpan + 1;
						}
					}
				}
			}
		}

		// Token: 0x04002799 RID: 10137
		private const double _minColumnWidth = 5.0;

		// Token: 0x0400279A RID: 10138
		private const double _minRowHeight = 10.0;

		// Token: 0x0400279B RID: 10139
		private int _RTLCount;

		// Token: 0x0400279C RID: 10140
		private int _LTRCount;

		// Token: 0x0400279D RID: 10141
		private int _numCols;
	}
}
