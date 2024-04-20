using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.PtsTable;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000148 RID: 328
	internal sealed class TableParaClient : BaseParaClient
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x0010A829 File Offset: 0x00109829
		internal TableParaClient(TableParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x001217F0 File Offset: 0x001207F0
		protected override void OnArrange()
		{
			base.OnArrange();
			this._columnRect = base.Paragraph.StructuralCache.CurrentArrangeContext.ColumnRect;
			CalculatedColumn[] calculatedColumns = this.CalculatedColumns;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT rect;
			if (!this.QueryTableDetails(out array, out fskupdate, out rect) || fskupdate == PTS.FSKUPDATE.fskupdNoChange || fskupdate == PTS.FSKUPDATE.fskupdShifted)
			{
				return;
			}
			this._rect = rect;
			this.UpdateChunkInfo(array);
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ParentFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect = this._pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ParentFlowDirection), ref pageRect, ref this._rect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out this._rect));
				mbpInfo.MirrorMargin();
			}
			this._rect.u = this._rect.u + mbpInfo.MarginLeft;
			this._rect.du = this._rect.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight);
			int num = this.GetTableOffsetFirstRowTop() + TextDpi.ToTextDpi(this.Table.InternalCellSpacing) / 2;
			for (int i = 0; i < array.Length; i++)
			{
				if (((array[i].fsupdinf.fskupd != PTS.FSKUPDATE.fskupdInherited) ? array[i].fsupdinf.fskupd : fskupdate) == PTS.FSKUPDATE.fskupdNoChange)
				{
					num += array[i].u.dvrRow;
				}
				else
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero) && (i == 0 || (array4[j] != PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle && array4[j] != PTS.FSTABLEKCELLMERGE.fskcellmergeLast)))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							double urOffset = calculatedColumns[cellParaClient.ColumnIndex].UrOffset;
							cellParaClient.Arrange(TextDpi.ToTextDpi(urOffset), num, this._rect, base.ThisFlowDirection, this._pageContext);
						}
					}
					num += array[i].u.dvrRow;
					if (i == 0 && this.IsFirstChunk)
					{
						num -= mbpInfo.BPTop;
					}
				}
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00121A40 File Offset: 0x00120A40
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			Invariant.Assert(fskupdInherited > PTS.FSKUPDATE.fskupdInherited);
			Invariant.Assert(this.TableParagraph.Table != null && this.CalculatedColumns != null);
			Table table = this.TableParagraph.Table;
			this.Visual.Clip = new RectangleGeometry(this._columnRect.FromTextDpi());
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (!this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				this._visual.Children.Clear();
				return;
			}
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
			{
				fskupdate = fskupdInherited;
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdShifted)
			{
				fskupdate = PTS.FSKUPDATE.fskupdNew;
			}
			VisualCollection children = this._visual.Children;
			if (fskupdate == PTS.FSKUPDATE.fskupdNew)
			{
				children.Clear();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			using (DrawingContext drawingContext = this._visual.RenderOpen())
			{
				Rect tableContentRect = this.GetTableContentRect(mbpInfo).FromTextDpi();
				this._visual.DrawBackgroundAndBorderIntoContext(drawingContext, backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
				this.DrawColumnBackgrounds(drawingContext, tableContentRect);
				this.DrawRowGroupBackgrounds(drawingContext, array, tableContentRect, mbpInfo);
				this.DrawRowBackgrounds(drawingContext, array, tableContentRect, mbpInfo);
			}
			TableRow tableRow = null;
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSKUPDATE fskupdate2 = (array[i].fsupdinf.fskupd != PTS.FSKUPDATE.fskupdInherited) ? array[i].fsupdinf.fskupd : fskupdate;
				RowParagraph rowParagraph = (RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow);
				TableRow row = rowParagraph.Row;
				if (fskupdate2 == PTS.FSKUPDATE.fskupdNew)
				{
					RowVisual visual = new RowVisual(row);
					children.Insert(i, visual);
				}
				else
				{
					this.SynchronizeRowVisualsCollection(children, i, row);
				}
				Invariant.Assert(((RowVisual)children[i]).Row == row);
				if (fskupdate2 == PTS.FSKUPDATE.fskupdNew || fskupdate2 == PTS.FSKUPDATE.fskupdChangeInside)
				{
					if (rowParagraph.Row.HasForeignCells && (tableRow == null || tableRow.RowGroup != row.RowGroup))
					{
						this.ValidateRowVisualComplex((RowVisual)children[i], array[i].pfstablerow, this.CalculatedColumns.Length, fskupdate2, this.CalculatedColumns);
					}
					else
					{
						this.ValidateRowVisualSimple((RowVisual)children[i], array[i].pfstablerow, fskupdate2, this.CalculatedColumns);
					}
				}
				tableRow = row;
			}
			if (children.Count > array.Length)
			{
				children.RemoveRange(array.Length, children.Count - array.Length);
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00121D28 File Offset: 0x00120D28
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							((CellParaClient)base.PtsContext.HandleToObject(array2[j])).UpdateViewport(ref viewport);
						}
					}
				}
			}
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00121DA8 File Offset: 0x00120DA8
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				int num = this.GetTableOffsetFirstRowTop() + fsrect.v;
				for (int i = 0; i < array.Length; i++)
				{
					if (pt.v >= num && pt.v <= num + array[i].u.dvrRow)
					{
						IntPtr[] array2;
						PTS.FSKUPDATE[] array3;
						PTS.FSTABLEKCELLMERGE[] array4;
						this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
						for (int j = 0; j < array2.Length; j++)
						{
							if (!(array2[j] == IntPtr.Zero))
							{
								CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
								PTS.FSRECT rect = cellParaClient.Rect;
								if (cellParaClient.Rect.Contains(pt))
								{
									inputElement = cellParaClient.InputHitTest(pt);
									break;
								}
							}
						}
						break;
					}
					num += array[i].u.dvrRow;
				}
			}
			if (inputElement == null && this._rect.Contains(pt))
			{
				inputElement = this.TableParagraph.Table;
			}
			return inputElement;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00121ECC File Offset: 0x00120ECC
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			if (this.TableParagraph.Table == e)
			{
				this.GetRectanglesForParagraphElement(out list);
			}
			else
			{
				list = new List<Rect>();
				PTS.FSTABLEROWDESCRIPTION[] array;
				PTS.FSKUPDATE fskupdate;
				PTS.FSRECT fsrect;
				if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
				{
					for (int i = 0; i < array.Length; i++)
					{
						IntPtr[] array2;
						PTS.FSKUPDATE[] array3;
						PTS.FSTABLEKCELLMERGE[] array4;
						this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
						for (int j = 0; j < array2.Length; j++)
						{
							if (!(array2[j] == IntPtr.Zero))
							{
								CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
								if (start < cellParaClient.Paragraph.ParagraphEndCharacterPosition)
								{
									list = cellParaClient.GetRectangles(e, start, length);
									Invariant.Assert(list != null);
									if (list.Count != 0)
									{
										break;
									}
								}
							}
						}
						if (list.Count != 0)
						{
							break;
						}
					}
				}
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00121FBA File Offset: 0x00120FBA
		internal override ParagraphResult CreateParagraphResult()
		{
			return new TableParagraphResult(this);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00121FC4 File Offset: 0x00120FC4
		internal override TextContentRange GetTextContentRange()
		{
			TextContentRange textContentRange = null;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				textContentRange = new TextContentRange();
				this.UpdateChunkInfo(array);
				TextElement textElement = base.Paragraph.Element as TextElement;
				if (this._isFirstChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.BeforeStart));
				}
				for (int i = 0; i < array.Length; i++)
				{
					TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow)).Row;
					PTS.FSTABLEROWDETAILS fstablerowdetails;
					PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, array[i].pfstablerow, out fstablerowdetails));
					if (fstablerowdetails.fskboundaryAbove != PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryBreak)
					{
						if (row.Index == 0)
						{
							textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row.RowGroup, ElementEdge.BeforeStart));
						}
						textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row, ElementEdge.BeforeStart));
					}
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							textContentRange.Merge(cellParaClient.GetTextContentRange());
						}
					}
					if (fstablerowdetails.fskboundaryBelow != PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryBreak)
					{
						textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row, ElementEdge.AfterEnd));
						if (row.Index == row.RowGroup.Rows.Count - 1)
						{
							textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row.RowGroup, ElementEdge.AfterEnd));
						}
					}
				}
				if (this._isLastChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.AfterEnd));
				}
			}
			if (textContentRange == null)
			{
				textContentRange = TextContainerHelper.GetTextContentRangeForTextElement(this.TableParagraph.Table);
			}
			return textContentRange;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00122180 File Offset: 0x00121180
		internal CellParaClient GetCellParaClientFromPoint(Point point, bool snapToText)
		{
			int num = TextDpi.ToTextDpi(point.X);
			int num2 = TextDpi.ToTextDpi(point.Y);
			CellParaClient cellParaClient = null;
			CellParaClient cellParaClient2 = null;
			int num3 = int.MaxValue;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				int num4 = 0;
				while (num4 < array.Length && cellParaClient == null)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[num4].pfstablerow, out array2, out array3, out array4);
					int num5 = 0;
					while (num5 < array2.Length && cellParaClient == null)
					{
						if (!(array2[num5] == IntPtr.Zero))
						{
							CellParaClient cellParaClient3 = (CellParaClient)base.PtsContext.HandleToObject(array2[num5]);
							PTS.FSRECT rect = cellParaClient3.Rect;
							if (num >= rect.u && num <= rect.u + rect.du && num2 >= rect.v && num2 <= rect.v + rect.dv)
							{
								cellParaClient = cellParaClient3;
							}
							else if (snapToText)
							{
								int num6 = Math.Min(Math.Abs(rect.u - num), Math.Abs(rect.u + rect.du - num));
								int num7 = Math.Min(Math.Abs(rect.v - num2), Math.Abs(rect.v + rect.dv - num2));
								if (num6 + num7 < num3)
								{
									num3 = num6 + num7;
									cellParaClient2 = cellParaClient3;
								}
							}
						}
						num5++;
					}
					num4++;
				}
			}
			if (snapToText && cellParaClient == null)
			{
				cellParaClient = cellParaClient2;
			}
			return cellParaClient;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00122308 File Offset: 0x00121308
		internal ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResults(out bool hasTextContent)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			Rect rect = this.GetTableContentRect(mbpInfo).FromTextDpi();
			double num = rect.Y;
			Rect rowRect = rect;
			hasTextContent = false;
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					RowParagraph rowParagraph = (RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow);
					rowRect.Y = num;
					rowRect.Height = this.GetActualRowHeight(array, i, mbpInfo);
					RowParagraphResult rowParagraphResult = new RowParagraphResult(this, i, rowRect, rowParagraph);
					if (rowParagraphResult.HasTextContent)
					{
						hasTextContent = true;
					}
					list.Add(rowParagraphResult);
					num += rowRect.Height;
				}
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00122404 File Offset: 0x00121404
		internal ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResultsForRow(int rowIndex, out bool hasTextContent)
		{
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			hasTextContent = false;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				IntPtr[] array2;
				PTS.FSKUPDATE[] array3;
				PTS.FSTABLEKCELLMERGE[] array4;
				this.QueryRowDetails(array[rowIndex].pfstablerow, out array2, out array3, out array4);
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] != IntPtr.Zero && (rowIndex == 0 || (array4[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle && array4[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeLast)))
					{
						ParagraphResult paragraphResult = ((CellParaClient)base.PtsContext.HandleToObject(array2[i])).CreateParagraphResult();
						if (paragraphResult.HasTextContent)
						{
							hasTextContent = true;
						}
						list.Add(paragraphResult);
					}
				}
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x001224B0 File Offset: 0x001214B0
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPoint(Point point, bool snapToText)
		{
			CellParaClient cellParaClientFromPoint = this.GetCellParaClientFromPoint(point, snapToText);
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			if (cellParaClientFromPoint != null)
			{
				list.Add(cellParaClientFromPoint.CreateParagraphResult());
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x001224E4 File Offset: 0x001214E4
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPosition(ITextPointer position)
		{
			CellParaClient cellParaClientFromPosition = this.GetCellParaClientFromPosition(position);
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			if (cellParaClientFromPosition != null)
			{
				list.Add(cellParaClientFromPosition.CreateParagraphResult());
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00122518 File Offset: 0x00121518
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			Geometry geometry = null;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				bool flag = false;
				int num = 0;
				while (num < array.Length && !flag)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[num].pfstablerow, out array2, out array3, out array4);
					for (int i = 0; i < array2.Length; i++)
					{
						if (!(array2[i] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[i]);
							if (endPosition.CompareTo(cellParaClient.Cell.ContentStart) <= 0)
							{
								flag = true;
							}
							else if (startPosition.CompareTo(cellParaClient.Cell.ContentEnd) <= 0)
							{
								Geometry tightBoundingGeometryFromTextPositions = cellParaClient.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
								CaretElement.AddGeometry(ref geometry, tightBoundingGeometryFromTextPositions);
							}
						}
					}
					num++;
				}
			}
			if (geometry != null)
			{
				geometry = Geometry.Combine(geometry, this.Visual.Clip, GeometryCombineMode.Intersect, null);
			}
			return geometry;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00122608 File Offset: 0x00121608
		internal CellParaClient GetCellParaClientFromPosition(ITextPointer position)
		{
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							if (position.CompareTo(cellParaClient.Cell.ContentStart) >= 0 && position.CompareTo(cellParaClient.Cell.ContentEnd) <= 0)
							{
								return cellParaClient;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x001226BC File Offset: 0x001216BC
		internal CellParaClient GetCellAbove(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			int num = TextDpi.ToTextDpi(suggestedX);
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					CellParaClient cellParaClient = null;
					int num2 = int.MaxValue;
					for (int j = array2.Length - 1; j >= 0; j--)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient2 = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							if ((cellParaClient2.Cell.RowIndex + cellParaClient2.Cell.RowSpan - 1 < rowIndex && cellParaClient2.Cell.RowGroupIndex == rowGroupIndex) || cellParaClient2.Cell.RowGroupIndex < rowGroupIndex)
							{
								if (num >= cellParaClient2.Rect.u && num <= cellParaClient2.Rect.u + cellParaClient2.Rect.du)
								{
									return cellParaClient2;
								}
								int num3 = Math.Abs(cellParaClient2.Rect.u + cellParaClient2.Rect.du / 2 - num);
								if (num3 < num2)
								{
									num2 = num3;
									cellParaClient = cellParaClient2;
								}
							}
						}
					}
					if (cellParaClient != null)
					{
						return cellParaClient;
					}
				}
			}
			return null;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00122830 File Offset: 0x00121830
		internal CellParaClient GetCellBelow(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			int num = TextDpi.ToTextDpi(suggestedX);
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					CellParaClient cellParaClient = null;
					int num2 = int.MaxValue;
					for (int j = array2.Length - 1; j >= 0; j--)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient2 = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							if ((cellParaClient2.Cell.RowIndex > rowIndex && cellParaClient2.Cell.RowGroupIndex == rowGroupIndex) || cellParaClient2.Cell.RowGroupIndex > rowGroupIndex)
							{
								if (num >= cellParaClient2.Rect.u && num <= cellParaClient2.Rect.u + cellParaClient2.Rect.du)
								{
									return cellParaClient2;
								}
								int num3 = Math.Abs(cellParaClient2.Rect.u + cellParaClient2.Rect.du / 2 - num);
								if (num3 < num2)
								{
									num2 = num3;
									cellParaClient = cellParaClient2;
								}
							}
						}
					}
					if (cellParaClient != null)
					{
						return cellParaClient;
					}
				}
			}
			return null;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0012296C File Offset: 0x0012196C
		internal CellInfo GetCellInfoFromPoint(Point point)
		{
			CellParaClient cellParaClientFromPoint = this.GetCellParaClientFromPoint(point, true);
			if (cellParaClientFromPoint != null)
			{
				return new CellInfo(this, cellParaClientFromPoint);
			}
			return null;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00122990 File Offset: 0x00121990
		internal Rect GetRectangleFromRowEndPosition(ITextPointer position)
		{
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				int num = this.GetTableOffsetFirstRowTop() + fsrect.v;
				for (int i = 0; i < array.Length; i++)
				{
					TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow)).Row;
					if (((TextPointer)position).CompareTo(row.ContentEnd) == 0)
					{
						return new Rect(TextDpi.FromTextDpi(fsrect.u + fsrect.du), TextDpi.FromTextDpi(num), 1.0, TextDpi.FromTextDpi(array[i].u.dvrRow));
					}
					num += array[i].u.dvrRow;
				}
			}
			return System.Windows.Rect.Empty;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00122A64 File Offset: 0x00121A64
		internal void AutofitTable(uint fswdirTrack, int durAvailableSpace, out int durTableWidth)
		{
			double availableWidth = TextDpi.FromTextDpi(durAvailableSpace);
			double d;
			this.Autofit(availableWidth, out d);
			durTableWidth = TextDpi.ToTextDpi(d);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00122A8C File Offset: 0x00121A8C
		internal void UpdAutofitTable(uint fswdirTrack, int durAvailableSpace, out int durTableWidth, out int fNoChangeInCellWidths)
		{
			double availableWidth = TextDpi.FromTextDpi(durAvailableSpace);
			double d;
			fNoChangeInCellWidths = this.Autofit(availableWidth, out d);
			durTableWidth = TextDpi.ToTextDpi(d);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00122AB4 File Offset: 0x00121AB4
		internal int Autofit(double availableWidth, out double tableWidth)
		{
			int result = 1;
			this.ValidateCalculatedColumns();
			if (!DoubleUtil.AreClose(availableWidth, this._previousAutofitWidth))
			{
				result = this.ValidateTableWidths(availableWidth, out tableWidth);
			}
			else
			{
				tableWidth = this._previousTableWidth;
			}
			this._previousAutofitWidth = availableWidth;
			this._previousTableWidth = tableWidth;
			return result;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00122AFA File Offset: 0x00121AFA
		internal TableParagraph TableParagraph
		{
			get
			{
				return (TableParagraph)this._paragraph;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00122B07 File Offset: 0x00121B07
		internal Table Table
		{
			get
			{
				return this.TableParagraph.Table;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00122B14 File Offset: 0x00121B14
		internal double TableDesiredWidth
		{
			get
			{
				double num = 0.0;
				CalculatedColumn[] calculatedColumns = this.CalculatedColumns;
				for (int i = 0; i < calculatedColumns.Length; i++)
				{
					num += calculatedColumns[i].DurWidth + this.Table.InternalCellSpacing;
				}
				return num;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00122B5C File Offset: 0x00121B5C
		internal CalculatedColumn[] CalculatedColumns
		{
			get
			{
				return this._calculatedColumns;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x00122B64 File Offset: 0x00121B64
		internal double AutofitWidth
		{
			get
			{
				return this._previousAutofitWidth;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x00122B6C File Offset: 0x00121B6C
		internal override bool IsFirstChunk
		{
			get
			{
				return this._isFirstChunk;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00122B74 File Offset: 0x00121B74
		internal override bool IsLastChunk
		{
			get
			{
				return this._isLastChunk;
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00122B7C File Offset: 0x00121B7C
		private void UpdateChunkInfo(PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc)
		{
			TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[0].fsnmRow)).Row;
			PTS.FSTABLEROWDETAILS fstablerowdetails;
			PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, arrayTableRowDesc[0].pfstablerow, out fstablerowdetails));
			this._isFirstChunk = (fstablerowdetails.fskboundaryAbove == PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryOuter && row.Index == 0 && this.Table.IsFirstNonEmptyRowGroup(row.RowGroup.Index));
			row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[arrayTableRowDesc.Length - 1].fsnmRow)).Row;
			PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, arrayTableRowDesc[arrayTableRowDesc.Length - 1].pfstablerow, out fstablerowdetails));
			this._isLastChunk = (fstablerowdetails.fskboundaryBelow == PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryOuter && row.Index == row.RowGroup.Rows.Count - 1 && this.Table.IsLastNonEmptyRowGroup(row.RowGroup.Index));
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00122C8C File Offset: 0x00121C8C
		private unsafe bool QueryTableDetails(out PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, out PTS.FSKUPDATE fskupdTable, out PTS.FSRECT rect)
		{
			PTS.FSTABLEOBJDETAILS fstableobjdetails;
			PTS.Validate(PTS.FsQueryTableObjDetails(base.PtsContext.Context, this._paraHandle.Value, out fstableobjdetails));
			fskupdTable = fstableobjdetails.fskupdTableProper;
			rect = fstableobjdetails.fsrcTableObj;
			PTS.FSTABLEDETAILS fstabledetails;
			PTS.Validate(PTS.FsQueryTableObjTableProperDetails(base.PtsContext.Context, fstableobjdetails.pfstableProper, out fstabledetails));
			if (fstabledetails.cRows == 0)
			{
				arrayTableRowDesc = null;
				return false;
			}
			arrayTableRowDesc = new PTS.FSTABLEROWDESCRIPTION[fstabledetails.cRows];
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSTABLEROWDESCRIPTION* rgtablerowdescr;
			if ((array = arrayTableRowDesc) == null || array.Length == 0)
			{
				rgtablerowdescr = null;
			}
			else
			{
				rgtablerowdescr = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryTableObjRowList(base.PtsContext.Context, fstableobjdetails.pfstableProper, fstabledetails.cRows, rgtablerowdescr, out num));
			array = null;
			return true;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00122D48 File Offset: 0x00121D48
		private unsafe void QueryRowDetails(IntPtr pfstablerow, out IntPtr[] arrayFsCell, out PTS.FSKUPDATE[] arrayUpdate, out PTS.FSTABLEKCELLMERGE[] arrayTableCellMerge)
		{
			PTS.FSTABLEROWDETAILS fstablerowdetails;
			PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, pfstablerow, out fstablerowdetails));
			arrayUpdate = new PTS.FSKUPDATE[fstablerowdetails.cCells];
			arrayFsCell = new IntPtr[fstablerowdetails.cCells];
			arrayTableCellMerge = new PTS.FSTABLEKCELLMERGE[fstablerowdetails.cCells];
			if (fstablerowdetails.cCells > 0)
			{
				PTS.FSKUPDATE[] array;
				PTS.FSKUPDATE* rgfskupd;
				if ((array = arrayUpdate) == null || array.Length == 0)
				{
					rgfskupd = null;
				}
				else
				{
					rgfskupd = &array[0];
				}
				IntPtr[] array2;
				IntPtr* rgpfscell;
				if ((array2 = arrayFsCell) == null || array2.Length == 0)
				{
					rgpfscell = null;
				}
				else
				{
					rgpfscell = &array2[0];
				}
				PTS.FSTABLEKCELLMERGE[] array3;
				PTS.FSTABLEKCELLMERGE* rgkcellmerge;
				if ((array3 = arrayTableCellMerge) == null || array3.Length == 0)
				{
					rgkcellmerge = null;
				}
				else
				{
					rgkcellmerge = &array3[0];
				}
				int num;
				PTS.Validate(PTS.FsQueryTableObjCellList(base.PtsContext.Context, pfstablerow, fstablerowdetails.cCells, rgfskupd, rgpfscell, rgkcellmerge, out num));
				array3 = null;
				array2 = null;
				array = null;
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00122E1C File Offset: 0x00121E1C
		private void SynchronizeRowVisualsCollection(VisualCollection rowVisualsCollection, int firstIndex, TableRow row)
		{
			if (((RowVisual)rowVisualsCollection[firstIndex]).Row != row)
			{
				int num = firstIndex;
				int count = rowVisualsCollection.Count;
				while (++num < count && ((RowVisual)rowVisualsCollection[num]).Row != row)
				{
				}
				rowVisualsCollection.RemoveRange(firstIndex, num - firstIndex);
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00122E70 File Offset: 0x00121E70
		private void SynchronizeCellVisualsCollection(VisualCollection cellVisualsCollection, int firstIndex, Visual visual)
		{
			if (cellVisualsCollection[firstIndex] != visual)
			{
				int num = firstIndex;
				int count = cellVisualsCollection.Count;
				while (++num < count && cellVisualsCollection[num] != visual)
				{
				}
				cellVisualsCollection.RemoveRange(firstIndex, num - firstIndex);
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00122EB0 File Offset: 0x00121EB0
		private void ValidateRowVisualSimple(RowVisual rowVisual, IntPtr pfstablerow, PTS.FSKUPDATE fskupdRow, CalculatedColumn[] calculatedColumns)
		{
			IntPtr[] array;
			PTS.FSKUPDATE[] array2;
			PTS.FSTABLEKCELLMERGE[] array3;
			this.QueryRowDetails(pfstablerow, out array, out array2, out array3);
			VisualCollection children = rowVisual.Children;
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == IntPtr.Zero) && array3[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle && array3[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeLast)
				{
					PTS.FSKUPDATE fskupdate = (array2[i] != PTS.FSKUPDATE.fskupdInherited) ? array2[i] : fskupdRow;
					if (fskupdate != PTS.FSKUPDATE.fskupdNoChange)
					{
						CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array[i]);
						double urOffset = calculatedColumns[cellParaClient.ColumnIndex].UrOffset;
						cellParaClient.ValidateVisual();
						if (fskupdate == PTS.FSKUPDATE.fskupdNew || VisualTreeHelper.GetParent(cellParaClient.Visual) == null)
						{
							Visual visual = VisualTreeHelper.GetParent(cellParaClient.Visual) as Visual;
							if (visual != null)
							{
								ContainerVisual containerVisual = visual as ContainerVisual;
								Invariant.Assert(containerVisual != null, "parent should always derives from ContainerVisual");
								containerVisual.Children.Remove(cellParaClient.Visual);
							}
							children.Insert(num, cellParaClient.Visual);
						}
						else
						{
							this.SynchronizeCellVisualsCollection(children, num, cellParaClient.Visual);
						}
					}
					num++;
				}
			}
			if (children.Count > num)
			{
				children.RemoveRange(num, children.Count - num);
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00122FF0 File Offset: 0x00121FF0
		private void ValidateRowVisualComplex(RowVisual rowVisual, IntPtr pfstablerow, int tableColumnCount, PTS.FSKUPDATE fskupdRow, CalculatedColumn[] calculatedColumns)
		{
			IntPtr[] array;
			PTS.FSKUPDATE[] array2;
			PTS.FSTABLEKCELLMERGE[] array3;
			this.QueryRowDetails(pfstablerow, out array, out array2, out array3);
			TableParaClient.CellParaClientEntry[] array4 = new TableParaClient.CellParaClientEntry[tableColumnCount];
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == IntPtr.Zero))
				{
					PTS.FSKUPDATE fskupdCell = (array2[i] != PTS.FSKUPDATE.fskupdInherited) ? array2[i] : fskupdRow;
					CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array[i]);
					int columnIndex = cellParaClient.ColumnIndex;
					array4[columnIndex].cellParaClient = cellParaClient;
					array4[columnIndex].fskupdCell = fskupdCell;
				}
			}
			VisualCollection children = rowVisual.Children;
			int num = 0;
			for (int j = 0; j < array4.Length; j++)
			{
				CellParaClient cellParaClient2 = array4[j].cellParaClient;
				if (cellParaClient2 != null)
				{
					PTS.FSKUPDATE fskupdCell2 = array4[j].fskupdCell;
					if (fskupdCell2 != PTS.FSKUPDATE.fskupdNoChange)
					{
						double urOffset = calculatedColumns[j].UrOffset;
						cellParaClient2.ValidateVisual();
						if (fskupdCell2 == PTS.FSKUPDATE.fskupdNew)
						{
							children.Insert(num, cellParaClient2.Visual);
						}
						else
						{
							this.SynchronizeCellVisualsCollection(children, num, cellParaClient2.Visual);
						}
					}
					num++;
				}
			}
			if (children.Count > num)
			{
				children.RemoveRange(num, children.Count - num);
			}
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00123128 File Offset: 0x00122128
		private void DrawColumnBackgrounds(DrawingContext dc, Rect tableContentRect)
		{
			double num = tableContentRect.X;
			Rect rectangle = tableContentRect;
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				for (int i = this.CalculatedColumns.Length - 1; i >= 0; i--)
				{
					Brush brush = (i < this.Table.Columns.Count) ? this.Table.Columns[i].Background : null;
					rectangle.Width = this.CalculatedColumns[i].DurWidth + this.Table.InternalCellSpacing;
					if (brush != null)
					{
						rectangle.X = num;
						dc.DrawRectangle(brush, null, rectangle);
					}
					num += rectangle.Width;
				}
				return;
			}
			for (int j = 0; j < this.CalculatedColumns.Length; j++)
			{
				Brush brush = (j < this.Table.Columns.Count) ? this.Table.Columns[j].Background : null;
				rectangle.Width = this.CalculatedColumns[j].DurWidth + this.Table.InternalCellSpacing;
				if (brush != null)
				{
					rectangle.X = num;
					dc.DrawRectangle(brush, null, rectangle);
				}
				num += rectangle.Width;
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00123264 File Offset: 0x00122264
		private double GetActualRowHeight(PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, int rowIndex, MbpInfo mbpInfo)
		{
			int num = 0;
			if (this.IsFirstChunk && rowIndex == 0)
			{
				num = -mbpInfo.BPTop;
			}
			if (this.IsLastChunk && rowIndex == arrayTableRowDesc.Length - 1)
			{
				num = -mbpInfo.BPBottom;
			}
			return TextDpi.FromTextDpi(arrayTableRowDesc[rowIndex].u.dvrRow + num);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x001232B8 File Offset: 0x001222B8
		private void DrawRowGroupBackgrounds(DrawingContext dc, PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, Rect tableContentRect, MbpInfo mbpInfo)
		{
			double num = tableContentRect.Y;
			double num2 = 0.0;
			Rect rectangle = tableContentRect;
			if (arrayTableRowDesc.Length != 0)
			{
				TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[0].fsnmRow)).Row;
				TableRowGroup rowGroup = row.RowGroup;
				Brush brush;
				for (int i = 0; i < arrayTableRowDesc.Length; i++)
				{
					row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[i].fsnmRow)).Row;
					if (rowGroup != row.RowGroup)
					{
						brush = (Brush)rowGroup.GetValue(TextElement.BackgroundProperty);
						if (brush != null)
						{
							rectangle.Y = num;
							rectangle.Height = num2;
							dc.DrawRectangle(brush, null, rectangle);
						}
						num += num2;
						rowGroup = row.RowGroup;
						num2 = this.GetActualRowHeight(arrayTableRowDesc, i, mbpInfo);
					}
					else
					{
						num2 += this.GetActualRowHeight(arrayTableRowDesc, i, mbpInfo);
					}
				}
				brush = (Brush)rowGroup.GetValue(TextElement.BackgroundProperty);
				if (brush != null)
				{
					rectangle.Y = num;
					rectangle.Height = num2;
					dc.DrawRectangle(brush, null, rectangle);
				}
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x001233D8 File Offset: 0x001223D8
		private void DrawRowBackgrounds(DrawingContext dc, PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, Rect tableContentRect, MbpInfo mbpInfo)
		{
			double num = tableContentRect.Y;
			Rect rectangle = tableContentRect;
			for (int i = 0; i < arrayTableRowDesc.Length; i++)
			{
				Brush brush = (Brush)((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[i].fsnmRow)).Row.GetValue(TextElement.BackgroundProperty);
				rectangle.Y = num;
				rectangle.Height = this.GetActualRowHeight(arrayTableRowDesc, i, mbpInfo);
				if (brush != null)
				{
					dc.DrawRectangle(brush, null, rectangle);
				}
				num += rectangle.Height;
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00123460 File Offset: 0x00122460
		private void ValidateCalculatedColumns()
		{
			int columnCount = this.Table.ColumnCount;
			if (this._calculatedColumns == null)
			{
				this._calculatedColumns = new CalculatedColumn[columnCount];
			}
			else if (this._calculatedColumns.Length != columnCount)
			{
				CalculatedColumn[] array = new CalculatedColumn[columnCount];
				Array.Copy(this._calculatedColumns, array, Math.Min(this._calculatedColumns.Length, columnCount));
				this._calculatedColumns = array;
			}
			if (this._calculatedColumns.Length != 0)
			{
				int i;
				for (i = 0; i < this._calculatedColumns.Length; i++)
				{
					if (i >= this.Table.Columns.Count)
					{
						break;
					}
					this._calculatedColumns[i].UserWidth = this.Table.Columns[i].Width;
				}
				while (i < this._calculatedColumns.Length)
				{
					this._calculatedColumns[i].UserWidth = TableColumn.DefaultWidth;
					i++;
				}
			}
			this._durMinWidth = (this._durMaxWidth = 0.0);
			for (int j = 0; j < this._calculatedColumns.Length; j++)
			{
				switch (this._calculatedColumns[j].UserWidth.GridUnitType)
				{
				case GridUnitType.Auto:
					this._calculatedColumns[j].ValidateAuto(1.0, 1000000.0);
					break;
				case GridUnitType.Pixel:
					this._calculatedColumns[j].ValidateAuto(this._calculatedColumns[j].UserWidth.Value, this._calculatedColumns[j].UserWidth.Value);
					break;
				case GridUnitType.Star:
					this._calculatedColumns[j].ValidateAuto(1.0, 1000000.0);
					break;
				}
				this._durMinWidth += this._calculatedColumns[j].DurMinWidth;
				this._durMaxWidth += this._calculatedColumns[j].DurMaxWidth;
			}
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			double num = this.Table.InternalCellSpacing * (double)this.Table.ColumnCount + mbpInfo.Margin.Left + mbpInfo.Border.Left + mbpInfo.Padding.Left + mbpInfo.Padding.Right + mbpInfo.Border.Right + mbpInfo.Margin.Right;
			this._durMinWidth += num;
			this._durMaxWidth += num;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00123744 File Offset: 0x00122744
		private int ValidateTableWidths(double durAvailableWidth, out double durTableWidth)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			double internalCellSpacing = this.Table.InternalCellSpacing;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			double num = internalCellSpacing * (double)this.Table.ColumnCount + TextDpi.FromTextDpi(mbpInfo.MBPLeft + mbpInfo.MBPRight);
			int result = 1;
			durTableWidth = 0.0;
			double num2 = 0.0;
			double num4;
			double num3 = num4 = 0.0;
			double num5 = 0.0;
			double num7;
			double num6 = num7 = 0.0;
			double num8 = 0.0;
			double num9 = 1.0;
			double num10;
			for (int i = 0; i < this._calculatedColumns.Length; i++)
			{
				if (this._calculatedColumns[i].UserWidth.IsAuto)
				{
					num7 += this._calculatedColumns[i].DurMinWidth;
					num6 += this._calculatedColumns[i].DurMaxWidth;
				}
				else if (this._calculatedColumns[i].UserWidth.IsStar)
				{
					num10 = this._calculatedColumns[i].UserWidth.Value;
					if (num10 < 0.0)
					{
						num10 = 0.0;
					}
					if (num2 + num10 > 100.0)
					{
						num10 = 100.0 - num2;
						num2 = 100.0;
						this._calculatedColumns[i].UserWidth = new GridLength(num10, GridUnitType.Star);
					}
					else
					{
						num2 += num10;
					}
					if (num10 == 0.0)
					{
						num10 = 1.0;
					}
					if (this._calculatedColumns[i].DurMaxWidth * num9 > num10 * num8)
					{
						num8 = this._calculatedColumns[i].DurMaxWidth;
						num9 = num10;
					}
					num5 += this._calculatedColumns[i].DurMinWidth;
				}
				else
				{
					num4 += this._calculatedColumns[i].DurMinWidth;
					num3 += this._calculatedColumns[i].DurMaxWidth;
				}
			}
			num10 = 100.0 - num2;
			double num11;
			if (flag)
			{
				num11 = durAvailableWidth;
				if (num11 < this._durMinWidth && !DoubleUtil.AreClose(num11, this._durMinWidth))
				{
					num11 = this._durMinWidth;
				}
			}
			else if (0.0 < num2)
			{
				if (0.0 < num10)
				{
					double num12 = (num3 + num6) * num9;
					double num13 = num10 * num8;
					if (num12 > num13 && !DoubleUtil.AreClose(num12, num13))
					{
						num8 = num3 + num6;
						num9 = num10;
					}
				}
				if (0.0 < num10 || DoubleUtil.IsZero(num3 + num6))
				{
					num11 = num8 * 100.0 / num9 + num;
					if (num11 > durAvailableWidth && !DoubleUtil.AreClose(num11, durAvailableWidth))
					{
						num11 = durAvailableWidth;
					}
				}
				else
				{
					num11 = durAvailableWidth;
				}
				if (num11 < this._durMinWidth && !DoubleUtil.AreClose(num11, this._durMinWidth))
				{
					num11 = this._durMinWidth;
				}
			}
			else if (this._durMaxWidth < durAvailableWidth && !DoubleUtil.AreClose(this._durMaxWidth, durAvailableWidth))
			{
				num11 = this._durMaxWidth;
			}
			else if (this._durMinWidth > durAvailableWidth && !DoubleUtil.AreClose(this._durMinWidth, durAvailableWidth))
			{
				num11 = this._durMinWidth;
			}
			else
			{
				num11 = durAvailableWidth;
			}
			if (num11 > num || DoubleUtil.AreClose(num11, num))
			{
				num11 -= num;
			}
			double num14;
			if (0.0 < num6 + num3 && !DoubleUtil.IsZero(num6 + num3))
			{
				num14 = num10 * num11 / 100.0;
				if (num14 < num4 + num7 && !DoubleUtil.AreClose(num14, num4 + num7))
				{
					num14 = num4 + num7;
				}
				if (num14 > num11 - num5 && !DoubleUtil.AreClose(num14, num11 - num5))
				{
					num14 = num11 - num5;
				}
			}
			else
			{
				num14 = 0.0;
			}
			double num15;
			double num16;
			if (0.0 < num3 && !DoubleUtil.IsZero(num3))
			{
				num15 = num3;
				if (num15 > num14 && !DoubleUtil.AreClose(num15, num14))
				{
					num15 = num14;
				}
				if (0.0 < num6 && !DoubleUtil.IsZero(num6))
				{
					num16 = num7;
					if (num15 + num16 < num14 || DoubleUtil.AreClose(num15 + num16, num14))
					{
						num16 = num14 - num15;
					}
					else
					{
						num15 = num4;
						if (num15 + num16 < num14 || DoubleUtil.AreClose(num15 + num16, num14))
						{
							num15 = num14 - num16;
						}
					}
				}
				else
				{
					num16 = 0.0;
					if (num15 < num14 && !DoubleUtil.AreClose(num15, num14))
					{
						num15 = num14;
					}
				}
			}
			else
			{
				num15 = 0.0;
				if (0.0 < num6 && !DoubleUtil.IsZero(num6))
				{
					num16 = num7;
					if (num16 < num14 && !DoubleUtil.AreClose(num16, num14))
					{
						num16 = num14;
					}
				}
				else
				{
					num16 = 0.0;
				}
			}
			if (num16 > num6 && !DoubleUtil.AreClose(num16, num6))
			{
				flag4 = true;
			}
			else if (DoubleUtil.AreClose(num16, num6))
			{
				flag2 = true;
			}
			else if (DoubleUtil.AreClose(num16, num7))
			{
				flag3 = true;
			}
			else if (num16 < num6 && !DoubleUtil.AreClose(num16, num6))
			{
				flag8 = true;
			}
			if (num15 > num3 && !DoubleUtil.AreClose(num15, num3))
			{
				flag7 = true;
			}
			else if (DoubleUtil.AreClose(num15, num3))
			{
				flag5 = true;
			}
			else if (DoubleUtil.AreClose(num15, num4))
			{
				flag6 = true;
			}
			else if (num15 < num3 && !DoubleUtil.AreClose(num15, num3))
			{
				flag9 = true;
			}
			double num17 = (0.0 < num11) ? (100.0 * (num11 - num15 - num16) / num11) : 0.0;
			bool flag10 = !DoubleUtil.AreClose(num3, num4);
			durTableWidth = TextDpi.FromTextDpi(mbpInfo.BPLeft);
			for (int j = 0; j < this._calculatedColumns.Length; j++)
			{
				if (this._calculatedColumns[j].UserWidth.IsAuto)
				{
					this._calculatedColumns[j].DurWidth = (flag8 ? (this._calculatedColumns[j].DurMaxWidth - (this._calculatedColumns[j].DurMaxWidth - this._calculatedColumns[j].DurMinWidth) * (num6 - num16) / (num6 - num7)) : (flag4 ? (this._calculatedColumns[j].DurMaxWidth + this._calculatedColumns[j].DurMaxWidth * (num16 - num6) / num6) : (flag2 ? this._calculatedColumns[j].DurMaxWidth : (flag3 ? this._calculatedColumns[j].DurMinWidth : ((0.0 < num6 && !DoubleUtil.IsZero(num6)) ? (this._calculatedColumns[j].DurMinWidth + this._calculatedColumns[j].DurMaxWidth * (num16 - num7) / num6) : 0.0)))));
				}
				else if (this._calculatedColumns[j].UserWidth.IsStar)
				{
					num14 = ((0.0 < num2) ? (num11 * (num17 * this._calculatedColumns[j].UserWidth.Value / num2) / 100.0) : 0.0);
					num14 -= this._calculatedColumns[j].DurMinWidth;
					if (num14 < 0.0 && !DoubleUtil.IsZero(num14))
					{
						num14 = 0.0;
					}
					this._calculatedColumns[j].DurWidth = this._calculatedColumns[j].DurMinWidth + num14;
				}
				else
				{
					this._calculatedColumns[j].DurWidth = (flag9 ? (flag10 ? (this._calculatedColumns[j].DurMaxWidth - (this._calculatedColumns[j].DurMaxWidth - this._calculatedColumns[j].DurMinWidth) * (num3 - num15) / (num3 - num4)) : (this._calculatedColumns[j].DurMaxWidth - this._calculatedColumns[j].DurMaxWidth * (num3 - num15) / num3)) : (flag7 ? (this._calculatedColumns[j].DurMaxWidth + this._calculatedColumns[j].DurMaxWidth * (num15 - num3) / num3) : (flag5 ? this._calculatedColumns[j].DurMaxWidth : (flag6 ? this._calculatedColumns[j].DurMinWidth : ((0.0 < num3 && !DoubleUtil.IsZero(num3)) ? (this._calculatedColumns[j].DurMinWidth + this._calculatedColumns[j].DurMaxWidth * (num15 - num4) / num3) : 0.0)))));
				}
				this._calculatedColumns[j].UrOffset = durTableWidth + internalCellSpacing / 2.0;
				durTableWidth += this._calculatedColumns[j].DurWidth + internalCellSpacing;
				if (this._calculatedColumns[j].PtsWidthChanged == 1)
				{
					result = 0;
				}
			}
			durTableWidth += mbpInfo.Margin.Left + TextDpi.FromTextDpi(mbpInfo.MBPRight);
			return result;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0012412C File Offset: 0x0012312C
		private PTS.FSRECT GetTableContentRect(MbpInfo mbpInfo)
		{
			int num = this.IsFirstChunk ? mbpInfo.BPTop : 0;
			int num2 = this.IsLastChunk ? mbpInfo.BPBottom : 0;
			return new PTS.FSRECT(this._rect.u + mbpInfo.BPLeft, this._rect.v + num, Math.Max(this._rect.du - (mbpInfo.BPRight + mbpInfo.BPLeft), 1), Math.Max(this._rect.dv - num2 - num, 1));
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x001241B8 File Offset: 0x001231B8
		private int GetTableOffsetFirstRowTop()
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (!this.IsFirstChunk)
			{
				return 0;
			}
			return mbpInfo.BPTop;
		}

		// Token: 0x0400080E RID: 2062
		private bool _isFirstChunk;

		// Token: 0x0400080F RID: 2063
		private bool _isLastChunk;

		// Token: 0x04000810 RID: 2064
		private PTS.FSRECT _columnRect;

		// Token: 0x04000811 RID: 2065
		private CalculatedColumn[] _calculatedColumns;

		// Token: 0x04000812 RID: 2066
		private double _durMinWidth;

		// Token: 0x04000813 RID: 2067
		private double _durMaxWidth;

		// Token: 0x04000814 RID: 2068
		private double _previousAutofitWidth;

		// Token: 0x04000815 RID: 2069
		private double _previousTableWidth;

		// Token: 0x020008CA RID: 2250
		private struct CellParaClientEntry
		{
			// Token: 0x04003C4E RID: 15438
			internal CellParaClient cellParaClient;

			// Token: 0x04003C4F RID: 15439
			internal PTS.FSKUPDATE fskupdCell;
		}
	}
}
