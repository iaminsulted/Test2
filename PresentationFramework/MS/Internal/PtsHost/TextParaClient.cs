using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200014B RID: 331
	internal sealed class TextParaClient : BaseParaClient
	{
		// Token: 0x06000A57 RID: 2647 RVA: 0x00124800 File Offset: 0x00123800
		internal TextParaClient(TextParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00124810 File Offset: 0x00123810
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			VisualCollection children = this._visual.Children;
			ContainerVisual visual = this._visual;
			bool ignoreUpdateInfo = false;
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				if (children.Count != 0 && !(children[0] is LineVisual))
				{
					children.Clear();
					ignoreUpdateInfo = true;
				}
				if (this.IsDeferredVisualCreationSupported(ref fstextdetails.u.full))
				{
					if (this._lineIndexFirstVisual == -1 && visual.Children.Count > 0)
					{
						ignoreUpdateInfo = true;
					}
					this.SyncUpdateDeferredLineVisuals(visual.Children, ref fstextdetails.u.full, ignoreUpdateInfo);
				}
				else
				{
					if (this._lineIndexFirstVisual != -1)
					{
						this._lineIndexFirstVisual = -1;
						visual.Children.Clear();
					}
					if (visual.Children.Count == 0)
					{
						ignoreUpdateInfo = true;
					}
					if (fstextdetails.u.full.cLines > 0)
					{
						if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
						{
							this.RenderSimpleLines(visual, ref fstextdetails.u.full, ignoreUpdateInfo);
						}
						else
						{
							this.RenderCompositeLines(visual, ref fstextdetails.u.full, ignoreUpdateInfo);
						}
					}
					else
					{
						visual.Children.Clear();
					}
				}
				if (fstextdetails.u.full.cAttachedObjects > 0)
				{
					this.ValidateVisualFloatersAndFigures(fskupdInherited, fstextdetails.u.full.cAttachedObjects);
				}
			}
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect = this._pageContext.PageRect;
				PtsHelper.UpdateMirroringTransform(base.PageFlowDirection, base.ThisFlowDirection, visual, TextDpi.FromTextDpi(2 * pageRect.u + pageRect.du));
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x001249C4 File Offset: 0x001239C4
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull, "Only 'full' text paragraph type is expected.");
			if (this.IsDeferredVisualCreationSupported(ref fstextdetails.u.full))
			{
				ContainerVisual visual = this._visual;
				this.UpdateViewportSimpleLines(visual, ref fstextdetails.u.full, ref viewport);
			}
			int cAttachedObjects = fstextdetails.u.full.cAttachedObjects;
			if (cAttachedObjects > 0)
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
				PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, cAttachedObjects, out array);
				foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					baseParaClient.UpdateViewport(ref viewport);
				}
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00124AA4 File Offset: 0x00123AA4
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				PTS.FSPOINT fspoint = pt;
				if (base.ThisFlowDirection != base.PageFlowDirection)
				{
					fspoint.u = this._pageContext.PageRect.du - fspoint.u;
				}
				if (fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						inputElement = this.InputHitTestSimpleLines(fspoint, ref fstextdetails.u.full);
					}
					else
					{
						inputElement = this.InputHitTestCompositeLines(fspoint, ref fstextdetails.u.full);
					}
				}
			}
			if (inputElement == null)
			{
				inputElement = (base.Paragraph.Element as IInputElement);
			}
			return inputElement;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00124B78 File Offset: 0x00123B78
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				if (fstextdetails.u.full.cAttachedObjects > 0)
				{
					PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
					PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, fstextdetails.u.full.cAttachedObjects, out array);
					foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array)
					{
						BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
						PTS.ValidateHandle(baseParaClient);
						if (start < baseParaClient.Paragraph.ParagraphEndCharacterPosition)
						{
							list = baseParaClient.GetRectangles(e, start, length);
							Invariant.Assert(list != null);
							if (list.Count != 0)
							{
								break;
							}
						}
					}
				}
				if (list.Count == 0 && fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						list = this.GetRectanglesInSimpleLines(e, start, length, ref fstextdetails.u.full);
					}
					else
					{
						list = this.GetRectanglesInCompositeLines(e, start, length, ref fstextdetails.u.full);
					}
					if (list.Count > 0 && base.ThisFlowDirection != base.PageFlowDirection)
					{
						PTS.FSRECT pageRect = this._pageContext.PageRect;
						for (int j = 0; j < list.Count; j++)
						{
							PTS.FSRECT fsrect = new PTS.FSRECT(list[j]);
							PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ThisFlowDirection), ref pageRect, ref fsrect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out fsrect));
							list[j] = fsrect.FromTextDpi();
						}
					}
				}
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00124D46 File Offset: 0x00123D46
		internal override ParagraphResult CreateParagraphResult()
		{
			return new TextParagraphResult(this);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00124D50 File Offset: 0x00123D50
		internal ReadOnlyCollection<LineResult> GetLineResults()
		{
			ReadOnlyCollection<LineResult> result = new ReadOnlyCollection<LineResult>(new List<LineResult>(0));
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cLines > 0)
			{
				if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
				{
					result = this.LineResultsFromSimpleLines(ref fstextdetails.u.full);
				}
				else
				{
					result = this.LineResultsFromCompositeLines(ref fstextdetails.u.full);
				}
			}
			return result;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00124DE8 File Offset: 0x00123DE8
		internal ReadOnlyCollection<ParagraphResult> GetFloaters()
		{
			List<ParagraphResult> list = null;
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cAttachedObjects > 0)
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
				PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, fstextdetails.u.full.cAttachedObjects, out array);
				list = new List<ParagraphResult>(array.Length);
				foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					if (baseParaClient is FloaterParaClient)
					{
						list.Add(baseParaClient.CreateParagraphResult());
					}
				}
			}
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00124ECC File Offset: 0x00123ECC
		internal ReadOnlyCollection<ParagraphResult> GetFigures()
		{
			List<ParagraphResult> list = null;
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cAttachedObjects > 0)
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
				PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, fstextdetails.u.full.cAttachedObjects, out array);
				list = new List<ParagraphResult>(array.Length);
				foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					if (baseParaClient is FigureParaClient)
					{
						list.Add(baseParaClient.CreateParagraphResult());
					}
				}
			}
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00124FB0 File Offset: 0x00123FB0
		internal override TextContentRange GetTextContentRange()
		{
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull, "Only 'full' text paragraph type is expected.");
			int dcpFirst = fstextdetails.u.full.dcpFirst;
			int num = fstextdetails.u.full.dcpLim;
			if (this.HasEOP && num > base.Paragraph.Cch)
			{
				ErrorHandler.Assert(num == base.Paragraph.Cch + LineBase.SyntheticCharacterLength, ErrorHandler.ParagraphCharacterCountMismatch);
				num -= LineBase.SyntheticCharacterLength;
			}
			int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
			TextContentRange textContentRange;
			if (this.TextParagraph.HasFiguresOrFloaters())
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION[] arrayAttachedObjectDesc = null;
				int cAttachedObjects = fstextdetails.u.full.cAttachedObjects;
				textContentRange = new TextContentRange();
				if (cAttachedObjects > 0)
				{
					PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, cAttachedObjects, out arrayAttachedObjectDesc);
				}
				this.TextParagraph.UpdateTextContentRangeFromAttachedObjects(textContentRange, paragraphStartCharacterPosition + dcpFirst, paragraphStartCharacterPosition + num, arrayAttachedObjectDesc);
			}
			else
			{
				textContentRange = new TextContentRange(paragraphStartCharacterPosition + dcpFirst, paragraphStartCharacterPosition + num, base.Paragraph.StructuralCache.TextContainer);
			}
			return textContentRange;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x001250E0 File Offset: 0x001240E0
		internal void GetLineDetails(int dcpLine, out int cchContent, out int cchEllipses)
		{
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			int width = 0;
			bool firstLine = dcpLine == 0;
			int num = 0;
			IntPtr pbrLineIn = IntPtr.Zero;
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				if (fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						PTS.FSLINEDESCRIPTIONSINGLE[] array;
						PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref fstextdetails.u.full, out array);
						foreach (PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle in array)
						{
							if (dcpLine == fslinedescriptionsingle.dcpFirst)
							{
								width = fslinedescriptionsingle.dur;
								num = fslinedescriptionsingle.dcpLim;
								pbrLineIn = fslinedescriptionsingle.pfsbreakreclineclient;
								break;
							}
						}
					}
					else
					{
						PTS.FSLINEDESCRIPTIONCOMPOSITE[] array2;
						PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref fstextdetails.u.full, out array2);
						for (int j = 0; j < array2.Length; j++)
						{
							PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array2[j];
							if (fslinedescriptioncomposite.cElements != 0)
							{
								PTS.FSLINEELEMENT[] array3;
								PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array3);
								int k;
								for (k = 0; k < array3.Length; k++)
								{
									PTS.FSLINEELEMENT fslineelement = array3[k];
									if (fslineelement.dcpFirst == dcpLine)
									{
										width = fslineelement.dur;
										num = fslineelement.dcpLim;
										pbrLineIn = fslineelement.pfsbreakreclineclient;
										break;
									}
								}
								if (k < array3.Length)
								{
									firstLine = (j == 0);
									break;
								}
							}
						}
					}
				}
			}
			else
			{
				Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdCached);
				Invariant.Assert(false, "Should not get here. ParaCache is not currently used.");
			}
			Line.FormattingContext formattingContext = new Line.FormattingContext(false, true, true, this.TextParagraph.TextRunCache);
			Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
			if (this.IsOptimalParagraph)
			{
				formattingContext.LineFormatLengthTarget = num - dcpLine;
			}
			this.TextParagraph.FormatLineCore(line, pbrLineIn, formattingContext, dcpLine, width, firstLine, dcpLine);
			Invariant.Assert(line.SafeLength == num - dcpLine, "Line length is out of sync");
			cchContent = line.ContentLength;
			cchEllipses = line.GetEllipsesLength();
			line.Dispose();
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00125320 File Offset: 0x00124320
		internal override int GetFirstTextLineBaseline()
		{
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull, "Only 'full' text paragraph type is expected.");
			Rect empty = System.Windows.Rect.Empty;
			int result = 0;
			if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
			{
				this.RectFromDcpSimpleLines(0, 0, LogicalDirection.Forward, TextPointerContext.Text, ref fstextdetails.u.full, ref empty, ref result);
			}
			else
			{
				this.RectFromDcpCompositeLines(0, 0, LogicalDirection.Forward, TextPointerContext.Text, ref fstextdetails.u.full, ref empty, ref result);
			}
			return result;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x001253B8 File Offset: 0x001243B8
		internal ITextPointer GetTextPosition(int dcp, LogicalDirection direction)
		{
			return TextContainerHelper.GetTextPointerFromCP(base.Paragraph.StructuralCache.TextContainer, dcp + base.Paragraph.ParagraphStartCharacterPosition, direction);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x001253E0 File Offset: 0x001243E0
		internal Rect GetRectangleFromTextPosition(ITextPointer position)
		{
			Rect rect = System.Windows.Rect.Empty;
			int num = base.Paragraph.StructuralCache.TextContainer.Start.GetOffsetToPosition((TextPointer)position) - base.Paragraph.ParagraphStartCharacterPosition;
			int originalDcp = num;
			if (position.LogicalDirection == LogicalDirection.Backward && num > 0)
			{
				num--;
			}
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cLines > 0)
			{
				int num2 = 0;
				if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
				{
					this.RectFromDcpSimpleLines(num, originalDcp, position.LogicalDirection, position.GetPointerContext(position.LogicalDirection), ref fstextdetails.u.full, ref rect, ref num2);
				}
				else
				{
					this.RectFromDcpCompositeLines(num, originalDcp, position.LogicalDirection, position.GetPointerContext(position.LogicalDirection), ref fstextdetails.u.full, ref rect, ref num2);
				}
			}
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect = this._pageContext.PageRect;
				PTS.FSRECT fsrect = new PTS.FSRECT(rect);
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ThisFlowDirection), ref pageRect, ref fsrect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out fsrect));
				rect = fsrect.FromTextDpi();
			}
			return rect;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00125538 File Offset: 0x00124538
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect)
		{
			Geometry geometry = null;
			Geometry geometry2 = null;
			int offset = startPosition.Offset;
			int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
			int dcpStart = Math.Max(offset, paragraphStartCharacterPosition) - paragraphStartCharacterPosition;
			int offset2 = endPosition.Offset;
			int paragraphEndCharacterPosition = base.Paragraph.ParagraphEndCharacterPosition;
			int dcpEnd = Math.Min(offset2, paragraphEndCharacterPosition) - paragraphStartCharacterPosition;
			double paragraphTopSpace2 = (offset < paragraphStartCharacterPosition) ? paragraphTopSpace : 0.0;
			bool handleEndOfPara = offset2 > paragraphEndCharacterPosition;
			Transform transform = null;
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				transform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, TextDpi.FromTextDpi(2 * this._pageContext.PageRect.u + this._pageContext.PageRect.du), 0.0);
				visibleRect = transform.TransformBounds(visibleRect);
			}
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				if (fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						geometry = this.PathGeometryFromDcpRangeSimpleLines(dcpStart, dcpEnd, paragraphTopSpace2, handleEndOfPara, ref fstextdetails.u.full, visibleRect);
					}
					else
					{
						geometry = this.PathGeometryFromDcpRangeCompositeLines(dcpStart, dcpEnd, paragraphTopSpace2, handleEndOfPara, ref fstextdetails.u.full, visibleRect);
					}
				}
				if (fstextdetails.u.full.cAttachedObjects > 0)
				{
					geometry2 = this.PathGeometryFromDcpRangeFloatersAndFigures(offset, offset2, ref fstextdetails.u.full);
				}
			}
			if (geometry != null && transform != null)
			{
				CaretElement.AddTransformToGeometry(geometry, transform);
			}
			if (geometry2 != null)
			{
				CaretElement.AddGeometry(ref geometry, geometry2);
			}
			return geometry;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x001256F8 File Offset: 0x001246F8
		internal bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			bool result = false;
			int dcp = base.Paragraph.StructuralCache.TextContainer.Start.GetOffsetToPosition(position as TextPointer) - base.Paragraph.ParagraphStartCharacterPosition;
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cLines > 0)
			{
				if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
				{
					result = this.IsAtCaretUnitBoundaryFromDcpSimpleLines(dcp, position, ref fstextdetails.u.full);
				}
				else
				{
					result = this.IsAtCaretUnitBoundaryFromDcpCompositeLines(dcp, position, ref fstextdetails.u.full);
				}
			}
			return result;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x001257B8 File Offset: 0x001247B8
		internal ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			ITextPointer result = position;
			int dcp = base.Paragraph.StructuralCache.TextContainer.Start.GetOffsetToPosition(position as TextPointer) - base.Paragraph.ParagraphStartCharacterPosition;
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cLines > 0)
			{
				if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
				{
					result = this.NextCaretUnitPositionFromDcpSimpleLines(dcp, position, direction, ref fstextdetails.u.full);
				}
				else
				{
					result = this.NextCaretUnitPositionFromDcpCompositeLines(dcp, position, direction, ref fstextdetails.u.full);
				}
			}
			return result;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00125878 File Offset: 0x00124878
		internal ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			ITextPointer result = position;
			Invariant.Assert(position is TextPointer);
			int dcp = base.Paragraph.StructuralCache.TextContainer.Start.GetOffsetToPosition(position as TextPointer) - base.Paragraph.ParagraphStartCharacterPosition;
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				if (fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						result = this.BackspaceCaretUnitPositionFromDcpSimpleLines(dcp, position, ref fstextdetails.u.full);
					}
					else
					{
						result = this.BackspaceCaretUnitPositionFromDcpCompositeLines(dcp, position, ref fstextdetails.u.full);
					}
				}
			}
			else
			{
				Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdCached);
				Invariant.Assert(false, "Should not get here. ParaCache is not currently used.");
			}
			return result;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00125960 File Offset: 0x00124960
		internal ITextPointer GetTextPositionFromDistance(int dcpLine, double distance)
		{
			int num = TextDpi.ToTextDpi(distance);
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				num = this._pageContext.PageRect.du - num;
			}
			int width = 0;
			bool firstLine = dcpLine == 0;
			int num2 = 0;
			IntPtr pbrLineIn = IntPtr.Zero;
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull && fstextdetails.u.full.cLines > 0)
			{
				if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
				{
					PTS.FSLINEDESCRIPTIONSINGLE[] array;
					PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref fstextdetails.u.full, out array);
					foreach (PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle in array)
					{
						if (dcpLine == fslinedescriptionsingle.dcpFirst)
						{
							width = fslinedescriptionsingle.dur;
							num -= fslinedescriptionsingle.urStart;
							num2 = fslinedescriptionsingle.dcpLim;
							pbrLineIn = fslinedescriptionsingle.pfsbreakreclineclient;
							break;
						}
					}
				}
				else
				{
					PTS.FSLINEDESCRIPTIONCOMPOSITE[] array2;
					PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref fstextdetails.u.full, out array2);
					for (int j = 0; j < array2.Length; j++)
					{
						PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array2[j];
						if (fslinedescriptioncomposite.cElements != 0)
						{
							PTS.FSLINEELEMENT[] array3;
							PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array3);
							int k;
							for (k = 0; k < array3.Length; k++)
							{
								PTS.FSLINEELEMENT fslineelement = array3[k];
								if (fslineelement.dcpFirst == dcpLine)
								{
									width = fslineelement.dur;
									num -= fslineelement.urStart;
									num2 = fslineelement.dcpLim;
									pbrLineIn = fslineelement.pfsbreakreclineclient;
									break;
								}
							}
							if (k < array3.Length)
							{
								firstLine = (j == 0);
								break;
							}
						}
					}
				}
			}
			Line.FormattingContext formattingContext = new Line.FormattingContext(false, true, true, this.TextParagraph.TextRunCache);
			Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
			if (this.IsOptimalParagraph)
			{
				formattingContext.LineFormatLengthTarget = num2 - dcpLine;
			}
			this.TextParagraph.FormatLineCore(line, pbrLineIn, formattingContext, dcpLine, width, firstLine, dcpLine);
			Invariant.Assert(line.SafeLength == num2 - dcpLine, "Line length is out of sync");
			CharacterHit textPositionFromDistance = line.GetTextPositionFromDistance(num);
			int num3 = textPositionFromDistance.FirstCharacterIndex + textPositionFromDistance.TrailingLength;
			int lastDcpAttachedObjectBeforeLine = this.TextParagraph.GetLastDcpAttachedObjectBeforeLine(dcpLine);
			if (num3 < lastDcpAttachedObjectBeforeLine)
			{
				num3 = lastDcpAttachedObjectBeforeLine;
			}
			StaticTextPointer staticTextPointerFromCP = TextContainerHelper.GetStaticTextPointerFromCP(base.Paragraph.StructuralCache.TextContainer, num3 + base.Paragraph.ParagraphStartCharacterPosition);
			LogicalDirection direction = (textPositionFromDistance.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
			line.Dispose();
			return staticTextPointerFromCP.CreateDynamicTextPointer(direction);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00125C28 File Offset: 0x00124C28
		internal void GetGlyphRuns(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end)
		{
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				int num = base.Paragraph.StructuralCache.TextContainer.Start.GetOffsetToPosition((TextPointer)start) - base.Paragraph.ParagraphStartCharacterPosition;
				int num2 = base.Paragraph.StructuralCache.TextContainer.Start.GetOffsetToPosition((TextPointer)end) - base.Paragraph.ParagraphStartCharacterPosition;
				Invariant.Assert(num >= fstextdetails.u.full.dcpFirst && num2 <= fstextdetails.u.full.dcpLim);
				if (fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						this.GetGlyphRunsFromSimpleLines(glyphRuns, num, num2, ref fstextdetails.u.full);
						return;
					}
					this.GetGlyphRunsFromCompositeLines(glyphRuns, num, num2, ref fstextdetails.u.full);
					return;
				}
			}
			else
			{
				Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdCached);
				Invariant.Assert(false, "Should not get here. ParaCache is not currently used.");
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x00125D5D File Offset: 0x00124D5D
		internal TextParagraph TextParagraph
		{
			get
			{
				return (TextParagraph)this._paragraph;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00125D6A File Offset: 0x00124D6A
		internal bool HasEOP
		{
			get
			{
				return this.IsLastChunk;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00125D74 File Offset: 0x00124D74
		internal override bool IsFirstChunk
		{
			get
			{
				PTS.FSTEXTDETAILS fstextdetails;
				PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
				Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull, "Only 'full' text paragraph type is expected.");
				return fstextdetails.u.full.cLines > 0 && fstextdetails.u.full.dcpFirst == 0;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00125DE0 File Offset: 0x00124DE0
		internal override bool IsLastChunk
		{
			get
			{
				bool result = false;
				PTS.FSTEXTDETAILS fstextdetails;
				PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
				Invariant.Assert(fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull, "Only 'full' text paragraph type is expected.");
				if (fstextdetails.u.full.cLines > 0)
				{
					if (base.Paragraph.Cch > 0)
					{
						result = (fstextdetails.u.full.dcpLim >= base.Paragraph.Cch);
					}
					else
					{
						result = (fstextdetails.u.full.dcpLim == LineBase.SyntheticCharacterLength);
					}
				}
				return result;
			}
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00125E84 File Offset: 0x00124E84
		protected override void OnArrange()
		{
			base.OnArrange();
			if (!this.TextParagraph.HasFiguresFloatersOrInlineObjects())
			{
				return;
			}
			PTS.FSTEXTDETAILS fstextdetails;
			PTS.Validate(PTS.FsQueryTextDetails(base.PtsContext.Context, this._paraHandle.Value, out fstextdetails));
			if (fstextdetails.fsktd == PTS.FSKTEXTDETAILS.fsktdFull)
			{
				if (fstextdetails.u.full.cLines > 0)
				{
					if (!PTS.ToBoolean(fstextdetails.u.full.fLinesComposite))
					{
						PTS.FSLINEDESCRIPTIONSINGLE[] array;
						PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref fstextdetails.u.full, out array);
						foreach (PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle in array)
						{
							List<InlineObject> list = this.TextParagraph.InlineObjectsFromRange(fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dcpLim);
							if (list != null)
							{
								for (int j = 0; j < list.Count; j++)
								{
									UIElement uielement = (UIElement)list[j].Element;
									if (uielement.IsMeasureValid && !uielement.IsArrangeValid)
									{
										uielement.Arrange(new Rect(uielement.DesiredSize));
									}
								}
							}
						}
					}
					else
					{
						PTS.FSLINEDESCRIPTIONCOMPOSITE[] array2;
						PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref fstextdetails.u.full, out array2);
						foreach (PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite in array2)
						{
							PTS.FSLINEELEMENT[] array3;
							PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array3);
							foreach (PTS.FSLINEELEMENT fslineelement in array3)
							{
								List<InlineObject> list2 = this.TextParagraph.InlineObjectsFromRange(fslineelement.dcpFirst, fslineelement.dcpLim);
								if (list2 != null)
								{
									for (int m = 0; m < list2.Count; m++)
									{
										UIElement uielement2 = (UIElement)list2[m].Element;
										if (uielement2.IsMeasureValid && !uielement2.IsArrangeValid)
										{
											uielement2.Arrange(new Rect(uielement2.DesiredSize));
										}
									}
								}
							}
						}
					}
				}
				if (fstextdetails.u.full.cAttachedObjects > 0)
				{
					PTS.FSATTACHEDOBJECTDESCRIPTION[] array4;
					PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, fstextdetails.u.full.cAttachedObjects, out array4);
					foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array4)
					{
						BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
						PTS.ValidateHandle(baseParaClient);
						if (baseParaClient is FloaterParaClient)
						{
							PTS.FSFLOATERDETAILS fsfloaterdetails;
							PTS.Validate(PTS.FsQueryFloaterDetails(base.PtsContext.Context, fsattachedobjectdescription.pfspara, out fsfloaterdetails));
							PTS.FSRECT fsrcFloater = fsfloaterdetails.fsrcFloater;
							if (base.ThisFlowDirection != base.PageFlowDirection)
							{
								PTS.FSRECT pageRect = this._pageContext.PageRect;
								PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ThisFlowDirection), ref pageRect, ref fsrcFloater, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out fsrcFloater));
							}
							((FloaterParaClient)baseParaClient).ArrangeFloater(fsrcFloater, this._rect, PTS.FlowDirectionToFswdir(base.ThisFlowDirection), this._pageContext);
						}
						else if (baseParaClient is FigureParaClient)
						{
							PTS.FSFIGUREDETAILS fsfiguredetails;
							PTS.Validate(PTS.FsQueryFigureObjectDetails(base.PtsContext.Context, fsattachedobjectdescription.pfspara, out fsfiguredetails));
							PTS.FSRECT fsrcFlowAround = fsfiguredetails.fsrcFlowAround;
							if (base.ThisFlowDirection != base.PageFlowDirection)
							{
								PTS.FSRECT pageRect2 = this._pageContext.PageRect;
								PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ThisFlowDirection), ref pageRect2, ref fsrcFlowAround, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out fsrcFlowAround));
							}
							((FigureParaClient)baseParaClient).ArrangeFigure(fsrcFlowAround, this._rect, PTS.FlowDirectionToFswdir(base.ThisFlowDirection), this._pageContext);
						}
						else
						{
							Invariant.Assert(false, "Attached object not figure or floater.");
						}
					}
				}
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00126260 File Offset: 0x00125260
		private void SyncUpdateDeferredLineVisuals(VisualCollection lineVisuals, ref PTS.FSTEXTDETAILSFULL textDetails, bool ignoreUpdateInfo)
		{
			try
			{
				if (!PTS.ToBoolean(textDetails.fUpdateInfoForLinesPresent) || ignoreUpdateInfo || textDetails.cLines == 0)
				{
					lineVisuals.Clear();
				}
				else if (this._lineIndexFirstVisual != -1)
				{
					PTS.FSLINEDESCRIPTIONSINGLE[] array;
					PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
					int cLinesBeforeChange = textDetails.cLinesBeforeChange;
					int num = textDetails.cLinesChanged - textDetails.dcLinesChanged;
					int num2 = -1;
					if (textDetails.dvrShiftBeforeChange != 0)
					{
						int num3 = Math.Min(Math.Max(cLinesBeforeChange - this._lineIndexFirstVisual, 0), lineVisuals.Count);
						for (int i = 0; i < num3; i++)
						{
							ContainerVisual containerVisual = (ContainerVisual)lineVisuals[i];
							Vector offset = containerVisual.Offset;
							offset.Y += TextDpi.FromTextDpi(textDetails.dvrShiftBeforeChange);
							containerVisual.Offset = offset;
						}
					}
					if (cLinesBeforeChange < this._lineIndexFirstVisual)
					{
						int num4 = Math.Min(Math.Max(cLinesBeforeChange - this._lineIndexFirstVisual + num, 0), lineVisuals.Count);
						if (num4 > 0)
						{
							lineVisuals.RemoveRange(0, num4);
						}
						if (lineVisuals.Count == 0)
						{
							lineVisuals.Clear();
							this._lineIndexFirstVisual = -1;
						}
						else
						{
							num2 = 0;
							this._lineIndexFirstVisual = cLinesBeforeChange;
						}
					}
					else if (cLinesBeforeChange < this._lineIndexFirstVisual + lineVisuals.Count)
					{
						int count = Math.Min(num, lineVisuals.Count - (cLinesBeforeChange - this._lineIndexFirstVisual));
						lineVisuals.RemoveRange(cLinesBeforeChange - this._lineIndexFirstVisual, count);
						num2 = cLinesBeforeChange - this._lineIndexFirstVisual;
					}
					int num5 = -1;
					if (num2 != -1)
					{
						for (int j = textDetails.cLinesBeforeChange; j < textDetails.cLinesBeforeChange + textDetails.cLinesChanged; j++)
						{
							PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[j];
							ContainerVisual containerVisual2 = this.CreateLineVisual(ref array[j], base.Paragraph.ParagraphStartCharacterPosition);
							lineVisuals.Insert(num2 + (j - textDetails.cLinesBeforeChange), containerVisual2);
							containerVisual2.Offset = new Vector(TextDpi.FromTextDpi(fslinedescriptionsingle.urStart), TextDpi.FromTextDpi(fslinedescriptionsingle.vrStart));
						}
						num5 = num2 + textDetails.cLinesChanged;
					}
					if (num5 != -1)
					{
						for (int k = num5; k < lineVisuals.Count; k++)
						{
							ContainerVisual containerVisual3 = (ContainerVisual)lineVisuals[k];
							Vector offset2 = containerVisual3.Offset;
							offset2.Y += TextDpi.FromTextDpi(textDetails.dvrShiftAfterChange);
							containerVisual3.Offset = offset2;
						}
					}
				}
			}
			finally
			{
				if (lineVisuals.Count == 0)
				{
					this._lineIndexFirstVisual = -1;
				}
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x001264DC File Offset: 0x001254DC
		private ReadOnlyCollection<LineResult> LineResultsFromSimpleLines(ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return null;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			List<LineResult> list = new List<LineResult>(array.Length);
			foreach (PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle in array)
			{
				Rect rect = new Rect(TextDpi.FromTextDpi(fslinedescriptionsingle.urBBox), TextDpi.FromTextDpi(fslinedescriptionsingle.vrStart), TextDpi.FromTextDpi(fslinedescriptionsingle.durBBox), TextDpi.FromTextDpi(fslinedescriptionsingle.dvrAscent + fslinedescriptionsingle.dvrDescent));
				if (base.PageFlowDirection != base.ThisFlowDirection)
				{
					PTS.FSRECT pageRect = this._pageContext.PageRect;
					PTS.FSRECT fsrect = new PTS.FSRECT(rect);
					PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ThisFlowDirection), ref pageRect, ref fsrect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out fsrect));
					rect = fsrect.FromTextDpi();
				}
				list.Add(new TextParaLineResult(this, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, rect, TextDpi.FromTextDpi(fslinedescriptionsingle.dvrAscent)));
			}
			if (list.Count != 0)
			{
				TextParaLineResult textParaLineResult = (TextParaLineResult)list[list.Count - 1];
				if (this.HasEOP && textParaLineResult.DcpLast > base.Paragraph.Cch)
				{
					ErrorHandler.Assert(textParaLineResult.DcpLast - LineBase.SyntheticCharacterLength == base.Paragraph.Cch, ErrorHandler.ParagraphCharacterCountMismatch);
					textParaLineResult.DcpLast -= LineBase.SyntheticCharacterLength;
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			return new ReadOnlyCollection<LineResult>(list);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00126684 File Offset: 0x00125684
		private ReadOnlyCollection<LineResult> LineResultsFromCompositeLines(ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return null;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			List<LineResult> list = new List<LineResult>(array.Length);
			foreach (PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite in array)
			{
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					foreach (PTS.FSLINEELEMENT fslineelement in array2)
					{
						Rect rect = new Rect(TextDpi.FromTextDpi(fslineelement.urBBox), TextDpi.FromTextDpi(fslinedescriptioncomposite.vrStart), TextDpi.FromTextDpi(fslineelement.durBBox), TextDpi.FromTextDpi(fslineelement.dvrAscent + fslineelement.dvrDescent));
						if (base.ThisFlowDirection != base.PageFlowDirection)
						{
							PTS.FSRECT pageRect = this._pageContext.PageRect;
							PTS.FSRECT fsrect = new PTS.FSRECT(rect);
							PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ThisFlowDirection), ref pageRect, ref fsrect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out fsrect));
							rect = fsrect.FromTextDpi();
						}
						list.Add(new TextParaLineResult(this, fslineelement.dcpFirst, fslineelement.dcpLim - fslineelement.dcpFirst, rect, TextDpi.FromTextDpi(fslineelement.dvrAscent)));
					}
				}
			}
			if (list.Count != 0)
			{
				TextParaLineResult textParaLineResult = (TextParaLineResult)list[list.Count - 1];
				if (this.HasEOP && textParaLineResult.DcpLast > base.Paragraph.Cch)
				{
					ErrorHandler.Assert(textParaLineResult.DcpLast - LineBase.SyntheticCharacterLength == base.Paragraph.Cch, ErrorHandler.ParagraphCharacterCountMismatch);
					textParaLineResult.DcpLast -= LineBase.SyntheticCharacterLength;
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			return new ReadOnlyCollection<LineResult>(list);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00126874 File Offset: 0x00125874
		private void RectFromDcpSimpleLines(int dcp, int originalDcp, LogicalDirection orientation, TextPointerContext context, ref PTS.FSTEXTDETAILSFULL textDetails, ref Rect rect, ref int vrBaseline)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
				if ((fslinedescriptionsingle.dcpFirst <= dcp && fslinedescriptionsingle.dcpLim > dcp) || (fslinedescriptionsingle.dcpLim == dcp && i == array.Length - 1))
				{
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
					Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
					FlowDirection flowDirection;
					rect = line.GetBoundsFromTextPosition(dcp, out flowDirection);
					rect.X += TextDpi.FromTextDpi(fslinedescriptionsingle.urStart);
					rect.Y += TextDpi.FromTextDpi(fslinedescriptionsingle.vrStart);
					if (base.ThisFlowDirection != flowDirection)
					{
						if (orientation == LogicalDirection.Forward)
						{
							rect.X = rect.Right;
						}
					}
					else if (orientation == LogicalDirection.Backward && originalDcp > 0 && (context == TextPointerContext.Text || context == TextPointerContext.EmbeddedElement))
					{
						rect.X = rect.Right;
					}
					rect.Width = 0.0;
					vrBaseline = line.Baseline + fslinedescriptionsingle.vrStart;
					line.Dispose();
					return;
				}
			}
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00126A60 File Offset: 0x00125A60
		private void RectFromDcpCompositeLines(int dcp, int originalDcp, LogicalDirection orientation, TextPointerContext context, ref PTS.FSTEXTDETAILSFULL textDetails, ref Rect rect, ref int vrBaseline)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array[i];
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					for (int j = 0; j < array2.Length; j++)
					{
						PTS.FSLINEELEMENT fslineelement = array2[j];
						if ((fslineelement.dcpFirst <= dcp && fslineelement.dcpLim > dcp) || (fslineelement.dcpLim == dcp && j == array2.Length - 1 && i == array.Length - 1))
						{
							Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
							Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
							if (this.IsOptimalParagraph)
							{
								formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
							}
							this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
							Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
							FlowDirection flowDirection;
							rect = line.GetBoundsFromTextPosition(dcp, out flowDirection);
							rect.X += TextDpi.FromTextDpi(fslineelement.urStart);
							rect.Y += TextDpi.FromTextDpi(fslinedescriptioncomposite.vrStart);
							if (base.ThisFlowDirection != flowDirection)
							{
								if (orientation == LogicalDirection.Forward)
								{
									rect.X = rect.Right;
								}
							}
							else if (orientation == LogicalDirection.Backward && originalDcp > 0 && (context == TextPointerContext.Text || context == TextPointerContext.EmbeddedElement))
							{
								rect.X = rect.Right;
							}
							rect.Width = 0.0;
							vrBaseline = line.Baseline + fslinedescriptioncomposite.vrStart;
							line.Dispose();
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00126CAC File Offset: 0x00125CAC
		private Geometry PathGeometryFromDcpRangeSimpleLines(int dcpStart, int dcpEnd, double paragraphTopSpace, bool handleEndOfPara, ref PTS.FSTEXTDETAILSFULL textDetails, Rect visibleRect)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return null;
			}
			Geometry result = null;
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			int num = 0;
			int num2 = array.Length;
			if (this._lineIndexFirstVisual != -1)
			{
				num = this._lineIndexFirstVisual;
				num2 = this._visual.Children.Count;
			}
			for (int i = num; i < num + num2; i++)
			{
				PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
				if (handleEndOfPara)
				{
					if (dcpEnd < fslinedescriptionsingle.dcpFirst)
					{
						break;
					}
				}
				else if (dcpEnd <= fslinedescriptionsingle.dcpFirst)
				{
					break;
				}
				if (fslinedescriptionsingle.dcpLim > dcpStart || (i == array.Length - 1 && fslinedescriptionsingle.dcpLim == dcpStart))
				{
					int num3 = Math.Max(fslinedescriptionsingle.dcpFirst, dcpStart);
					int cchRange = Math.Max(Math.Min(fslinedescriptionsingle.dcpLim, dcpEnd) - num3, 1);
					double lineTopSpace = (i == 0) ? paragraphTopSpace : 0.0;
					double lineRightSpace;
					if ((handleEndOfPara && i == array.Length - 1) || (dcpEnd >= fslinedescriptionsingle.dcpLim && this.HasAnyLineBreakAtCp(fslinedescriptionsingle.dcpLim)))
					{
						lineRightSpace = (double)this.TextParagraph.Element.GetValue(TextElement.FontSizeProperty) * 0.5;
					}
					else
					{
						lineRightSpace = 0.0;
					}
					IList<Rect> list = this.RectanglesFromDcpRangeOfSimpleLine(num3, cchRange, lineTopSpace, lineRightSpace, ref fslinedescriptionsingle, i, visibleRect);
					if (list != null)
					{
						int j = 0;
						int count = list.Count;
						while (j < count)
						{
							RectangleGeometry addedGeometry = new RectangleGeometry(list[j]);
							CaretElement.AddGeometry(ref result, addedGeometry);
							j++;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00126E68 File Offset: 0x00125E68
		private Geometry PathGeometryFromDcpRangeCompositeLines(int dcpStart, int dcpEnd, double paragraphTopSpace, bool handleEndOfPara, ref PTS.FSTEXTDETAILSFULL textDetails, Rect visibleRect)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return null;
			}
			Geometry result = null;
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array[i];
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					for (int j = 0; j < array2.Length; j++)
					{
						PTS.FSLINEELEMENT fslineelement = array2[j];
						if (handleEndOfPara)
						{
							if (dcpEnd < fslineelement.dcpFirst)
							{
								break;
							}
						}
						else if (dcpEnd <= fslineelement.dcpFirst)
						{
							break;
						}
						if (fslineelement.dcpLim > dcpStart || (fslineelement.dcpLim == dcpStart && j == array2.Length - 1 && i == array.Length - 1))
						{
							int num = Math.Max(fslineelement.dcpFirst, dcpStart);
							int cchRange = Math.Max(Math.Min(fslineelement.dcpLim, dcpEnd) - num, 1);
							double lineTopSpace = (i == 0) ? paragraphTopSpace : 0.0;
							double lineRightSpace;
							if ((handleEndOfPara && i == array.Length - 1) || (dcpEnd >= fslineelement.dcpLim && this.HasAnyLineBreakAtCp(fslineelement.dcpLim)))
							{
								lineRightSpace = (double)this.TextParagraph.Element.GetValue(TextElement.FontSizeProperty) * 0.5;
							}
							else
							{
								lineRightSpace = 0.0;
							}
							IList<Rect> list = this.RectanglesFromDcpRangeOfCompositeLineElement(num, cchRange, lineTopSpace, lineRightSpace, ref fslinedescriptioncomposite, i, ref fslineelement, j, visibleRect);
							if (list != null)
							{
								int k = 0;
								int count = list.Count;
								while (k < count)
								{
									RectangleGeometry addedGeometry = new RectangleGeometry(list[k]);
									CaretElement.AddGeometry(ref result, addedGeometry);
									k++;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0012703F File Offset: 0x0012603F
		private bool HasAnyLineBreakAtCp(int dcp)
		{
			return TextPointerBase.IsNextToAnyBreak(base.Paragraph.StructuralCache.TextContainer.CreatePointerAtOffset(base.Paragraph.ParagraphStartCharacterPosition + dcp, LogicalDirection.Forward), LogicalDirection.Backward);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0012706C File Offset: 0x0012606C
		private List<Rect> RectanglesFromDcpRangeOfSimpleLine(int dcpRangeStart, int cchRange, double lineTopSpace, double lineRightSpace, ref PTS.FSLINEDESCRIPTIONSINGLE lineDesc, int lineIndex, Rect visibleRect)
		{
			List<Rect> list = null;
			Invariant.Assert(lineDesc.dcpFirst <= dcpRangeStart && dcpRangeStart <= lineDesc.dcpLim && cchRange > 0);
			Rect rect = new PTS.FSRECT(lineDesc.urBBox, lineDesc.vrStart, lineDesc.durBBox, lineDesc.dvrAscent + lineDesc.dvrDescent).FromTextDpi();
			LineVisual lineVisual = this.FetchLineVisual(lineIndex);
			if (lineVisual != null)
			{
				rect.Width = Math.Max(lineVisual.WidthIncludingTrailingWhitespace, 0.0);
			}
			rect.Y -= lineTopSpace;
			rect.Height += lineTopSpace;
			rect.Width += lineRightSpace;
			Rect rect2 = rect;
			rect2.X = visibleRect.X;
			if (rect2.IntersectsWith(visibleRect))
			{
				if (dcpRangeStart == lineDesc.dcpFirst && lineDesc.dcpLim <= dcpRangeStart + cchRange)
				{
					list = new List<Rect>(1);
					list.Add(rect);
				}
				else
				{
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(lineDesc.fClearOnLeft), PTS.ToBoolean(lineDesc.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = lineDesc.dcpLim - lineDesc.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, lineDesc.pfsbreakreclineclient, formattingContext, lineDesc.dcpFirst, lineDesc.dur, PTS.ToBoolean(lineDesc.fTreatedAsFirst), lineDesc.dcpFirst);
					Invariant.Assert(line.SafeLength == lineDesc.dcpLim - lineDesc.dcpFirst, "Line length is out of sync");
					double num = TextDpi.FromTextDpi(lineDesc.urStart);
					double num2 = TextDpi.FromTextDpi(lineDesc.vrStart);
					list = line.GetRangeBounds(dcpRangeStart, cchRange, num, num2);
					if (!DoubleUtil.IsZero(lineTopSpace))
					{
						int i = 0;
						int count = list.Count;
						while (i < count)
						{
							Rect value = list[i];
							value.Y -= lineTopSpace;
							value.Height += lineTopSpace;
							list[i] = value;
							i++;
						}
					}
					if (!DoubleUtil.IsZero(lineRightSpace))
					{
						list.Add(new Rect(num + TextDpi.FromTextDpi(line.Start + line.Width), num2 - lineTopSpace, lineRightSpace, TextDpi.FromTextDpi(line.Height) + lineTopSpace));
					}
					line.Dispose();
				}
			}
			return list;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x001272F8 File Offset: 0x001262F8
		private List<Rect> RectanglesFromDcpRangeOfCompositeLineElement(int dcpRangeStart, int cchRange, double lineTopSpace, double lineRightSpace, ref PTS.FSLINEDESCRIPTIONCOMPOSITE lineDesc, int lineIndex, ref PTS.FSLINEELEMENT elemDesc, int elemIndex, Rect visibleRect)
		{
			List<Rect> list = null;
			Rect rect = new PTS.FSRECT(elemDesc.urBBox, lineDesc.vrStart, elemDesc.durBBox, lineDesc.dvrAscent + lineDesc.dvrDescent).FromTextDpi();
			LineVisual lineVisual = this.FetchLineVisualComposite(lineIndex, elemIndex);
			if (lineVisual != null)
			{
				rect.Width = Math.Max(lineVisual.WidthIncludingTrailingWhitespace, 0.0);
			}
			rect.Y -= lineTopSpace;
			rect.Height += lineTopSpace;
			rect.Width += lineRightSpace;
			Rect rect2 = rect;
			rect2.X = visibleRect.X;
			if (rect2.IntersectsWith(visibleRect))
			{
				if (dcpRangeStart == elemDesc.dcpFirst && elemDesc.dcpLim <= dcpRangeStart + cchRange)
				{
					list = new List<Rect>(1);
					list.Add(rect);
				}
				else
				{
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(elemDesc.fClearOnLeft), PTS.ToBoolean(elemDesc.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = elemDesc.dcpLim - elemDesc.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, elemDesc.pfsbreakreclineclient, formattingContext, elemDesc.dcpFirst, elemDesc.dur, PTS.ToBoolean(lineDesc.fTreatedAsFirst), elemDesc.dcpFirst);
					Invariant.Assert(line.SafeLength == elemDesc.dcpLim - elemDesc.dcpFirst, "Line length is out of sync");
					double num = TextDpi.FromTextDpi(elemDesc.urStart);
					double num2 = TextDpi.FromTextDpi(lineDesc.vrStart);
					list = line.GetRangeBounds(dcpRangeStart, cchRange, num, num2);
					if (!DoubleUtil.IsZero(lineTopSpace))
					{
						int i = 0;
						int count = list.Count;
						while (i < count)
						{
							Rect value = list[i];
							value.Y -= lineTopSpace;
							value.Height += lineTopSpace;
							list[i] = value;
							i++;
						}
					}
					if (!DoubleUtil.IsZero(lineRightSpace))
					{
						list.Add(new Rect(num + TextDpi.FromTextDpi(line.Start + line.Width), num2 - lineTopSpace, lineRightSpace, TextDpi.FromTextDpi(line.Height) + lineTopSpace));
					}
					line.Dispose();
				}
			}
			return list;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00127568 File Offset: 0x00126568
		private LineVisual FetchLineVisual(int index)
		{
			LineVisual lineVisual = null;
			int childrenCount = VisualTreeHelper.GetChildrenCount(this.Visual);
			if (childrenCount != 0)
			{
				int num = index;
				if (this._lineIndexFirstVisual != -1)
				{
					num -= this._lineIndexFirstVisual;
				}
				if (0 <= num && num < childrenCount)
				{
					lineVisual = (VisualTreeHelper.GetChild(this.Visual, num) as LineVisual);
					Invariant.Assert(lineVisual != null || VisualTreeHelper.GetChild(this.Visual, num) == null);
				}
			}
			return lineVisual;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x001275D0 File Offset: 0x001265D0
		private LineVisual FetchLineVisualComposite(int lineIndex, int elemIndex)
		{
			LineVisual lineVisual = null;
			Visual reference = this.Visual;
			if (VisualTreeHelper.GetChildrenCount(this.Visual) != 0)
			{
				int childIndex = lineIndex;
				if (VisualTreeHelper.GetChild(this.Visual, childIndex) is ParagraphElementVisual)
				{
					reference = this.Visual.InternalGetVisualChild(lineIndex);
					childIndex = elemIndex;
				}
				lineVisual = (VisualTreeHelper.GetChild(reference, childIndex) as LineVisual);
				Invariant.Assert(lineVisual != null || VisualTreeHelper.GetChild(reference, childIndex) == null);
			}
			return lineVisual;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0012763C File Offset: 0x0012663C
		private Geometry PathGeometryFromDcpRangeFloatersAndFigures(int dcpStart, int dcpEnd, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			Geometry result = null;
			if (textDetails.cAttachedObjects > 0)
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
				PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, textDetails.cAttachedObjects, out array);
				foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					BaseParagraph paragraph = baseParaClient.Paragraph;
					if (dcpEnd <= paragraph.ParagraphStartCharacterPosition)
					{
						break;
					}
					if (paragraph.ParagraphEndCharacterPosition > dcpStart)
					{
						RectangleGeometry addedGeometry = new RectangleGeometry(baseParaClient.Rect.FromTextDpi());
						CaretElement.AddGeometry(ref result, addedGeometry);
					}
				}
			}
			return result;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x001276E8 File Offset: 0x001266E8
		private bool IsAtCaretUnitBoundaryFromDcpSimpleLines(int dcp, ITextPointer position, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return false;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			bool result = false;
			int i = 0;
			while (i < array.Length)
			{
				PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
				if ((fslinedescriptionsingle.dcpFirst <= dcp && fslinedescriptionsingle.dcpLim > dcp) || (fslinedescriptionsingle.dcpLim == dcp && i == array.Length - 1))
				{
					CharacterHit charHit = default(CharacterHit);
					if (dcp >= fslinedescriptionsingle.dcpLim - 1 && i == array.Length - 1)
					{
						return true;
					}
					if (position.LogicalDirection == LogicalDirection.Backward)
					{
						if (fslinedescriptionsingle.dcpFirst == dcp)
						{
							if (i == 0)
							{
								return false;
							}
							i--;
							fslinedescriptionsingle = array[i];
							Invariant.Assert(dcp > 0);
							charHit = new CharacterHit(dcp - 1, 1);
						}
						else
						{
							Invariant.Assert(dcp > 0);
							charHit = new CharacterHit(dcp - 1, 1);
						}
					}
					else if (position.LogicalDirection == LogicalDirection.Forward)
					{
						charHit = new CharacterHit(dcp, 0);
					}
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
					Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
					result = line.IsAtCaretCharacterHit(charHit);
					line.Dispose();
					break;
				}
				else
				{
					i++;
				}
			}
			return result;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x001278C4 File Offset: 0x001268C4
		private bool IsAtCaretUnitBoundaryFromDcpCompositeLines(int dcp, ITextPointer position, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return false;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			bool result = false;
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array[i];
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					int j = 0;
					while (j < array2.Length)
					{
						PTS.FSLINEELEMENT fslineelement = array2[j];
						if ((fslineelement.dcpFirst <= dcp && fslineelement.dcpLim > dcp) || (fslineelement.dcpLim == dcp && j == array2.Length - 1 && i == array.Length - 1))
						{
							CharacterHit charHit = default(CharacterHit);
							if (dcp >= fslineelement.dcpLim - 1 && j == array2.Length - 1 && i == array.Length - 1)
							{
								return true;
							}
							if (position.LogicalDirection == LogicalDirection.Backward)
							{
								if (dcp == fslineelement.dcpFirst)
								{
									if (j > 0)
									{
										j--;
										fslineelement = array2[j];
										charHit = new CharacterHit(dcp - 1, 1);
									}
									else
									{
										if (i == 0)
										{
											return false;
										}
										i--;
										fslinedescriptioncomposite = array[i];
										if (fslinedescriptioncomposite.cElements == 0)
										{
											return false;
										}
										PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
										fslineelement = array2[array2.Length - 1];
										charHit = new CharacterHit(dcp - 1, 1);
									}
								}
								else
								{
									Invariant.Assert(dcp > 0);
									charHit = new CharacterHit(dcp - 1, 1);
								}
							}
							else if (position.LogicalDirection == LogicalDirection.Forward)
							{
								charHit = new CharacterHit(dcp, 0);
							}
							Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
							Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
							if (this.IsOptimalParagraph)
							{
								formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
							}
							this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
							Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
							result = line.IsAtCaretCharacterHit(charHit);
							line.Dispose();
							return result;
						}
						else
						{
							j++;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00127B48 File Offset: 0x00126B48
		private ITextPointer NextCaretUnitPositionFromDcpSimpleLines(int dcp, ITextPointer position, LogicalDirection direction, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return position;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			ITextPointer result = position;
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
				if ((fslinedescriptionsingle.dcpFirst <= dcp && fslinedescriptionsingle.dcpLim > dcp) || (fslinedescriptionsingle.dcpLim == dcp && i == array.Length - 1))
				{
					if (dcp == fslinedescriptionsingle.dcpFirst && direction == LogicalDirection.Backward)
					{
						if (i == 0)
						{
							return position;
						}
						i--;
						fslinedescriptionsingle = array[i];
					}
					else if (dcp >= fslinedescriptionsingle.dcpLim - 1 && direction == LogicalDirection.Forward && i == array.Length - 1)
					{
						return position;
					}
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
					Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
					CharacterHit index = new CharacterHit(dcp, 0);
					CharacterHit characterHit;
					if (direction == LogicalDirection.Forward)
					{
						characterHit = line.GetNextCaretCharacterHit(index);
					}
					else
					{
						characterHit = line.GetPreviousCaretCharacterHit(index);
					}
					LogicalDirection direction2;
					if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == fslinedescriptionsingle.dcpLim && direction == LogicalDirection.Forward)
					{
						if (i == array.Length - 1)
						{
							direction2 = LogicalDirection.Backward;
						}
						else
						{
							direction2 = LogicalDirection.Forward;
						}
					}
					else if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == fslinedescriptionsingle.dcpFirst && direction == LogicalDirection.Backward)
					{
						if (i == 0)
						{
							direction2 = LogicalDirection.Forward;
						}
						else
						{
							direction2 = LogicalDirection.Backward;
						}
					}
					else
					{
						direction2 = ((characterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
					}
					result = this.GetTextPosition(characterHit.FirstCharacterIndex + characterHit.TrailingLength, direction2);
					line.Dispose();
					break;
				}
			}
			return result;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00127D78 File Offset: 0x00126D78
		private ITextPointer NextCaretUnitPositionFromDcpCompositeLines(int dcp, ITextPointer position, LogicalDirection direction, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return position;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array[i];
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					for (int j = 0; j < array2.Length; j++)
					{
						PTS.FSLINEELEMENT fslineelement = array2[j];
						if ((fslineelement.dcpFirst <= dcp && fslineelement.dcpLim > dcp) || (fslineelement.dcpLim == dcp && j == array2.Length - 1 && i == array.Length - 1))
						{
							if (dcp == fslineelement.dcpFirst && direction == LogicalDirection.Backward)
							{
								if (dcp == 0)
								{
									return position;
								}
								if (j > 0)
								{
									j--;
									fslineelement = array2[j];
								}
								else
								{
									i--;
									fslinedescriptioncomposite = array[i];
									if (fslinedescriptioncomposite.cElements == 0)
									{
										return position;
									}
									PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
									fslineelement = array2[array2.Length - 1];
								}
							}
							else if (dcp >= fslineelement.dcpLim - 1 && direction == LogicalDirection.Forward)
							{
								if (dcp == fslineelement.dcpLim)
								{
									return position;
								}
								if (dcp == fslineelement.dcpLim - 1 && j == array2.Length - 1 && i == array.Length - 1)
								{
									return position;
								}
							}
							Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
							Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
							if (this.IsOptimalParagraph)
							{
								formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
							}
							this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
							Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
							CharacterHit index = new CharacterHit(dcp, 0);
							CharacterHit characterHit;
							if (direction == LogicalDirection.Forward)
							{
								characterHit = line.GetNextCaretCharacterHit(index);
							}
							else
							{
								characterHit = line.GetPreviousCaretCharacterHit(index);
							}
							LogicalDirection direction2;
							if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == fslineelement.dcpLim && direction == LogicalDirection.Forward)
							{
								if (i == array.Length - 1)
								{
									direction2 = LogicalDirection.Backward;
								}
								else
								{
									direction2 = LogicalDirection.Forward;
								}
							}
							else if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == fslineelement.dcpFirst && direction == LogicalDirection.Backward)
							{
								if (i == 0)
								{
									direction2 = LogicalDirection.Forward;
								}
								else
								{
									direction2 = LogicalDirection.Backward;
								}
							}
							else
							{
								direction2 = ((characterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
							}
							ITextPointer textPosition = this.GetTextPosition(characterHit.FirstCharacterIndex + characterHit.TrailingLength, direction2);
							line.Dispose();
							return textPosition;
						}
					}
				}
			}
			return position;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00128068 File Offset: 0x00127068
		private ITextPointer BackspaceCaretUnitPositionFromDcpSimpleLines(int dcp, ITextPointer position, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return position;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			ITextPointer result = position;
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
				if ((fslinedescriptionsingle.dcpFirst <= dcp && fslinedescriptionsingle.dcpLim > dcp) || (fslinedescriptionsingle.dcpLim == dcp && i == array.Length - 1))
				{
					if (dcp == fslinedescriptionsingle.dcpFirst)
					{
						if (i == 0)
						{
							return position;
						}
						i--;
						fslinedescriptionsingle = array[i];
					}
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
					Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
					CharacterHit index = new CharacterHit(dcp, 0);
					CharacterHit backspaceCaretCharacterHit = line.GetBackspaceCaretCharacterHit(index);
					LogicalDirection direction;
					if (backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength == fslinedescriptionsingle.dcpFirst)
					{
						if (i == 0)
						{
							direction = LogicalDirection.Forward;
						}
						else
						{
							direction = LogicalDirection.Backward;
						}
					}
					else
					{
						direction = ((backspaceCaretCharacterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
					}
					result = this.GetTextPosition(backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength, direction);
					line.Dispose();
					break;
				}
			}
			return result;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00128238 File Offset: 0x00127238
		private ITextPointer BackspaceCaretUnitPositionFromDcpCompositeLines(int dcp, ITextPointer position, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return position;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array[i];
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					for (int j = 0; j < array2.Length; j++)
					{
						PTS.FSLINEELEMENT fslineelement = array2[j];
						if ((fslineelement.dcpFirst <= dcp && fslineelement.dcpLim > dcp) || (fslineelement.dcpLim == dcp && j == array2.Length - 1 && i == array.Length - 1))
						{
							if (dcp == fslineelement.dcpFirst)
							{
								if (dcp == 0)
								{
									return position;
								}
								if (j > 0)
								{
									j--;
									fslineelement = array2[j];
								}
								else
								{
									i--;
									fslinedescriptioncomposite = array[i];
									if (fslinedescriptioncomposite.cElements == 0)
									{
										return position;
									}
									PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
									fslineelement = array2[array2.Length - 1];
								}
							}
							Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
							Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
							if (this.IsOptimalParagraph)
							{
								formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
							}
							this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
							Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
							CharacterHit index = new CharacterHit(dcp, 0);
							CharacterHit backspaceCaretCharacterHit = line.GetBackspaceCaretCharacterHit(index);
							LogicalDirection direction;
							if (backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength == fslineelement.dcpFirst)
							{
								if (i == 0)
								{
									direction = LogicalDirection.Forward;
								}
								else
								{
									direction = LogicalDirection.Backward;
								}
							}
							else
							{
								direction = ((backspaceCaretCharacterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
							}
							ITextPointer textPosition = this.GetTextPosition(backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength, direction);
							line.Dispose();
							return textPosition;
						}
					}
				}
			}
			return position;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x001284A0 File Offset: 0x001274A0
		private void GetGlyphRunsFromSimpleLines(List<GlyphRun> glyphRuns, int dcpStart, int dcpEnd, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			foreach (PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle in array)
			{
				if (dcpStart < fslinedescriptionsingle.dcpLim && dcpEnd > fslinedescriptionsingle.dcpFirst)
				{
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
					Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
					line.GetGlyphRuns(glyphRuns, Math.Max(dcpStart, fslinedescriptionsingle.dcpFirst), Math.Min(dcpEnd, fslinedescriptionsingle.dcpLim));
					line.Dispose();
				}
				if (dcpEnd < fslinedescriptionsingle.dcpLim)
				{
					break;
				}
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00128604 File Offset: 0x00127604
		private void GetGlyphRunsFromCompositeLines(List<GlyphRun> glyphRuns, int dcpStart, int dcpEnd, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			foreach (PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite in array)
			{
				if (fslinedescriptioncomposite.cElements != 0)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					foreach (PTS.FSLINEELEMENT fslineelement in array2)
					{
						if (dcpStart < fslineelement.dcpLim && dcpEnd > fslineelement.dcpFirst)
						{
							Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
							Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
							if (this.IsOptimalParagraph)
							{
								formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
							}
							this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
							Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
							line.GetGlyphRuns(glyphRuns, Math.Max(dcpStart, fslineelement.dcpFirst), Math.Min(dcpEnd, fslineelement.dcpLim));
							line.Dispose();
						}
						if (dcpEnd < fslineelement.dcpLim)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x001287B8 File Offset: 0x001277B8
		private void RenderSimpleLines(ContainerVisual visual, ref PTS.FSTEXTDETAILSFULL textDetails, bool ignoreUpdateInfo)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
			if (textDetails.cLines == 0)
			{
				return;
			}
			VisualCollection children = visual.Children;
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			if (!PTS.ToBoolean(textDetails.fUpdateInfoForLinesPresent) || ignoreUpdateInfo)
			{
				children.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, paragraphStartCharacterPosition);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
					Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
					ContainerVisual containerVisual = line.CreateVisual();
					children.Insert(i, containerVisual);
					containerVisual.Offset = new Vector(TextDpi.FromTextDpi(fslinedescriptionsingle.urStart), TextDpi.FromTextDpi(fslinedescriptionsingle.vrStart));
					line.Dispose();
				}
				return;
			}
			if (textDetails.dvrShiftBeforeChange != 0)
			{
				for (int j = 0; j < textDetails.cLinesBeforeChange; j++)
				{
					ContainerVisual containerVisual2 = (ContainerVisual)children[j];
					Vector offset = containerVisual2.Offset;
					offset.Y += TextDpi.FromTextDpi(textDetails.dvrShiftBeforeChange);
					containerVisual2.Offset = offset;
				}
			}
			children.RemoveRange(textDetails.cLinesBeforeChange, textDetails.cLinesChanged - textDetails.dcLinesChanged);
			for (int k = textDetails.cLinesBeforeChange; k < textDetails.cLinesBeforeChange + textDetails.cLinesChanged; k++)
			{
				PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle2 = array[k];
				Line.FormattingContext formattingContext2 = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle2.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle2.fClearOnRight), this.TextParagraph.TextRunCache);
				Line line2 = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, paragraphStartCharacterPosition);
				if (this.IsOptimalParagraph)
				{
					formattingContext2.LineFormatLengthTarget = fslinedescriptionsingle2.dcpLim - fslinedescriptionsingle2.dcpFirst;
				}
				this.TextParagraph.FormatLineCore(line2, fslinedescriptionsingle2.pfsbreakreclineclient, formattingContext2, fslinedescriptionsingle2.dcpFirst, fslinedescriptionsingle2.dur, PTS.ToBoolean(fslinedescriptionsingle2.fTreatedAsFirst), fslinedescriptionsingle2.dcpFirst);
				Invariant.Assert(line2.SafeLength == fslinedescriptionsingle2.dcpLim - fslinedescriptionsingle2.dcpFirst, "Line length is out of sync");
				ContainerVisual containerVisual3 = line2.CreateVisual();
				children.Insert(k, containerVisual3);
				containerVisual3.Offset = new Vector(TextDpi.FromTextDpi(fslinedescriptionsingle2.urStart), TextDpi.FromTextDpi(fslinedescriptionsingle2.vrStart));
				line2.Dispose();
			}
			for (int l = textDetails.cLinesBeforeChange + textDetails.cLinesChanged; l < array.Length; l++)
			{
				ContainerVisual containerVisual4 = (ContainerVisual)children[l];
				Vector offset2 = containerVisual4.Offset;
				offset2.Y += TextDpi.FromTextDpi(textDetails.dvrShiftAfterChange);
				containerVisual4.Offset = offset2;
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00128B2B File Offset: 0x00127B2B
		private bool IntersectsWithRectOnV(ref PTS.FSRECT rect)
		{
			return this._rect.v <= rect.v + rect.dv && this._rect.v + this._rect.dv >= rect.v;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00128B6B File Offset: 0x00127B6B
		private bool ContainedInRectOnV(ref PTS.FSRECT rect)
		{
			return rect.v <= this._rect.v && rect.v + rect.dv >= this._rect.v + this._rect.dv;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00128BAC File Offset: 0x00127BAC
		private ContainerVisual CreateLineVisual(ref PTS.FSLINEDESCRIPTIONSINGLE lineDesc, int cpTextParaStart)
		{
			Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(lineDesc.fClearOnLeft), PTS.ToBoolean(lineDesc.fClearOnRight), this.TextParagraph.TextRunCache);
			Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, cpTextParaStart);
			if (this.IsOptimalParagraph)
			{
				formattingContext.LineFormatLengthTarget = lineDesc.dcpLim - lineDesc.dcpFirst;
			}
			this.TextParagraph.FormatLineCore(line, lineDesc.pfsbreakreclineclient, formattingContext, lineDesc.dcpFirst, lineDesc.dur, PTS.ToBoolean(lineDesc.fTreatedAsFirst), lineDesc.dcpFirst);
			Invariant.Assert(line.SafeLength == lineDesc.dcpLim - lineDesc.dcpFirst, "Line length is out of sync");
			ContainerVisual result = line.CreateVisual();
			line.Dispose();
			return result;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00128C70 File Offset: 0x00127C70
		private void UpdateViewportSimpleLines(ContainerVisual visual, ref PTS.FSTEXTDETAILSFULL textDetails, ref PTS.FSRECT viewport)
		{
			VisualCollection children = visual.Children;
			try
			{
				if (!this.IntersectsWithRectOnV(ref viewport) || textDetails.cLines == 0)
				{
					children.Clear();
				}
				else if (!this.ContainedInRectOnV(ref viewport) || this._lineIndexFirstVisual != 0 || children.Count != textDetails.cLines)
				{
					int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
					PTS.FSLINEDESCRIPTIONSINGLE[] array;
					PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
					int num;
					int num2;
					if (this.ContainedInRectOnV(ref viewport))
					{
						num = 0;
						num2 = textDetails.cLines;
					}
					else
					{
						int i;
						for (i = 0; i < array.Length; i++)
						{
							PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle = array[i];
							if (fslinedescriptionsingle.vrStart + fslinedescriptionsingle.dvrAscent + fslinedescriptionsingle.dvrDescent > viewport.v)
							{
								break;
							}
						}
						num = i;
						i = num;
						while (i < array.Length && array[i].vrStart <= viewport.v + viewport.dv)
						{
							i++;
						}
						num2 = i;
					}
					if (this._lineIndexFirstVisual != -1 && (num > this._lineIndexFirstVisual + children.Count || num2 < this._lineIndexFirstVisual))
					{
						children.Clear();
						this._lineIndexFirstVisual = -1;
					}
					if (this._lineIndexFirstVisual == -1)
					{
						for (int j = num; j < num2; j++)
						{
							PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle2 = array[j];
							ContainerVisual containerVisual = this.CreateLineVisual(ref fslinedescriptionsingle2, paragraphStartCharacterPosition);
							children.Add(containerVisual);
							containerVisual.Offset = new Vector(TextDpi.FromTextDpi(fslinedescriptionsingle2.urStart), TextDpi.FromTextDpi(fslinedescriptionsingle2.vrStart));
						}
						this._lineIndexFirstVisual = num;
					}
					else if (num != this._lineIndexFirstVisual || num2 - num != children.Count)
					{
						if (num < this._lineIndexFirstVisual)
						{
							for (int k = num; k < this._lineIndexFirstVisual; k++)
							{
								PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle3 = array[k];
								ContainerVisual containerVisual2 = this.CreateLineVisual(ref fslinedescriptionsingle3, paragraphStartCharacterPosition);
								children.Insert(k - num, containerVisual2);
								containerVisual2.Offset = new Vector(TextDpi.FromTextDpi(fslinedescriptionsingle3.urStart), TextDpi.FromTextDpi(fslinedescriptionsingle3.vrStart));
							}
						}
						else if (num != this._lineIndexFirstVisual)
						{
							children.RemoveRange(0, num - this._lineIndexFirstVisual);
						}
						this._lineIndexFirstVisual = num;
					}
					if (num2 - num < children.Count)
					{
						int num3 = children.Count - (num2 - num);
						children.RemoveRange(children.Count - num3, num3);
					}
					else if (num2 - num > children.Count)
					{
						for (int l = this._lineIndexFirstVisual + children.Count; l < num2; l++)
						{
							PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle4 = array[l];
							ContainerVisual containerVisual3 = this.CreateLineVisual(ref fslinedescriptionsingle4, paragraphStartCharacterPosition);
							children.Add(containerVisual3);
							containerVisual3.Offset = new Vector(TextDpi.FromTextDpi(fslinedescriptionsingle4.urStart), TextDpi.FromTextDpi(fslinedescriptionsingle4.vrStart));
						}
					}
				}
			}
			finally
			{
				if (children.Count == 0)
				{
					this._lineIndexFirstVisual = -1;
				}
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00128F6C File Offset: 0x00127F6C
		private void RenderCompositeLines(ContainerVisual visual, ref PTS.FSTEXTDETAILSFULL textDetails, bool ignoreUpdateInfo)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			VisualCollection children = visual.Children;
			int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			if (!PTS.ToBoolean(textDetails.fUpdateInfoForLinesPresent) || ignoreUpdateInfo)
			{
				children.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite = array[i];
					int num;
					VisualCollection visualCollection;
					if (fslinedescriptioncomposite.cElements == 1)
					{
						num = i;
						visualCollection = children;
					}
					else
					{
						num = 0;
						ParagraphElementVisual paragraphElementVisual = new ParagraphElementVisual();
						children.Add(paragraphElementVisual);
						visualCollection = paragraphElementVisual.Children;
					}
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					for (int j = 0; j < array2.Length; j++)
					{
						PTS.FSLINEELEMENT fslineelement = array2[j];
						Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
						Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, paragraphStartCharacterPosition);
						if (this.IsOptimalParagraph)
						{
							formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
						}
						this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
						Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
						ContainerVisual containerVisual = line.CreateVisual();
						visualCollection.Insert(num + j, containerVisual);
						containerVisual.Offset = new Vector(TextDpi.FromTextDpi(fslineelement.urStart), TextDpi.FromTextDpi(fslinedescriptioncomposite.vrStart));
						line.Dispose();
					}
				}
				return;
			}
			if (textDetails.dvrShiftBeforeChange != 0)
			{
				for (int k = 0; k < textDetails.cLinesBeforeChange; k++)
				{
					ContainerVisual containerVisual2 = (ContainerVisual)children[k];
					Vector offset = containerVisual2.Offset;
					offset.Y += TextDpi.FromTextDpi(textDetails.dvrShiftBeforeChange);
					containerVisual2.Offset = offset;
				}
			}
			children.RemoveRange(textDetails.cLinesBeforeChange, textDetails.cLinesChanged - textDetails.dcLinesChanged);
			for (int l = textDetails.cLinesBeforeChange; l < textDetails.cLinesBeforeChange + textDetails.cLinesChanged; l++)
			{
				PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite2 = array[l];
				int num2;
				VisualCollection visualCollection2;
				if (fslinedescriptioncomposite2.cElements == 1)
				{
					num2 = l;
					visualCollection2 = children;
				}
				else
				{
					num2 = 0;
					ParagraphElementVisual paragraphElementVisual2 = new ParagraphElementVisual();
					children.Add(paragraphElementVisual2);
					visualCollection2 = paragraphElementVisual2.Children;
				}
				PTS.FSLINEELEMENT[] array3;
				PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite2, out array3);
				for (int m = 0; m < array3.Length; m++)
				{
					PTS.FSLINEELEMENT fslineelement2 = array3[m];
					Line.FormattingContext formattingContext2 = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement2.fClearOnLeft), PTS.ToBoolean(fslineelement2.fClearOnRight), this.TextParagraph.TextRunCache);
					Line line2 = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, paragraphStartCharacterPosition);
					if (this.IsOptimalParagraph)
					{
						formattingContext2.LineFormatLengthTarget = fslineelement2.dcpLim - fslineelement2.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line2, fslineelement2.pfsbreakreclineclient, formattingContext2, fslineelement2.dcpFirst, fslineelement2.dur, PTS.ToBoolean(fslinedescriptioncomposite2.fTreatedAsFirst), fslineelement2.dcpFirst);
					Invariant.Assert(line2.SafeLength == fslineelement2.dcpLim - fslineelement2.dcpFirst, "Line length is out of sync");
					ContainerVisual containerVisual3 = line2.CreateVisual();
					visualCollection2.Insert(num2 + m, containerVisual3);
					containerVisual3.Offset = new Vector(TextDpi.FromTextDpi(fslineelement2.urStart), TextDpi.FromTextDpi(fslinedescriptioncomposite2.vrStart));
					line2.Dispose();
				}
			}
			for (int n = textDetails.cLinesBeforeChange + textDetails.cLinesChanged; n < array.Length; n++)
			{
				ContainerVisual containerVisual4 = (ContainerVisual)children[n];
				Vector offset2 = containerVisual4.Offset;
				offset2.Y += TextDpi.FromTextDpi(textDetails.dvrShiftAfterChange);
				containerVisual4.Offset = offset2;
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x001293A4 File Offset: 0x001283A4
		private void ValidateVisualFloatersAndFigures(PTS.FSKUPDATE fskupdInherited, int cAttachedObjects)
		{
			if (cAttachedObjects > 0)
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
				PtsHelper.AttachedObjectListFromParagraph(base.PtsContext, this._paraHandle.Value, cAttachedObjects, out array);
				foreach (PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription in array)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(fsattachedobjectdescription.pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					PTS.FSKUPDATE fskupdate = fsattachedobjectdescription.fsupdinf.fskupd;
					if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
					{
						fskupdate = fskupdInherited;
					}
					if (fskupdate != PTS.FSKUPDATE.fskupdNoChange)
					{
						baseParaClient.ValidateVisual(fskupdate);
					}
				}
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00129420 File Offset: 0x00128420
		private IInputElement InputHitTestSimpleLines(PTS.FSPOINT pt, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return null;
			}
			IInputElement result = null;
			int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			foreach (PTS.FSLINEDESCRIPTIONSINGLE fslinedescriptionsingle in array)
			{
				if (fslinedescriptionsingle.vrStart + fslinedescriptionsingle.dvrAscent + fslinedescriptionsingle.dvrDescent > pt.v)
				{
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslinedescriptionsingle.fClearOnLeft), PTS.ToBoolean(fslinedescriptionsingle.fClearOnRight), this.TextParagraph.TextRunCache);
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, paragraphStartCharacterPosition);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst;
					}
					using (line)
					{
						this.TextParagraph.FormatLineCore(line, fslinedescriptionsingle.pfsbreakreclineclient, formattingContext, fslinedescriptionsingle.dcpFirst, fslinedescriptionsingle.dur, PTS.ToBoolean(fslinedescriptionsingle.fTreatedAsFirst), fslinedescriptionsingle.dcpFirst);
						Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
						if (fslinedescriptionsingle.urStart + line.CalculateUOffsetShift() <= pt.u && pt.u <= fslinedescriptionsingle.urStart + line.CalculateUOffsetShift() + fslinedescriptionsingle.dur)
						{
							int num = pt.u - fslinedescriptionsingle.urStart;
							Invariant.Assert(line.SafeLength == fslinedescriptionsingle.dcpLim - fslinedescriptionsingle.dcpFirst, "Line length is out of sync");
							if (line.Start <= num && num <= line.Start + line.Width)
							{
								result = line.InputHitTest(num);
							}
						}
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00129624 File Offset: 0x00128624
		private bool IsDeferredVisualCreationSupported(ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			return base.Paragraph.StructuralCache.IsDeferredVisualCreationSupported && !PTS.ToBoolean(textDetails.fLinesComposite) && !this.TextParagraph.HasFiguresFloatersOrInlineObjects();
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0012965C File Offset: 0x0012865C
		private List<Rect> GetRectanglesInSimpleLines(ContentElement e, int start, int length, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			List<Rect> list = new List<Rect>();
			int num = start - base.Paragraph.ParagraphStartCharacterPosition;
			if (num < 0 || textDetails.cLines == 0)
			{
				return list;
			}
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PtsHelper.LineListSimpleFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			foreach (PTS.FSLINEDESCRIPTIONSINGLE lineDesc in array)
			{
				List<Rect> rectanglesInSingleLine = this.GetRectanglesInSingleLine(lineDesc, e, num, length);
				Invariant.Assert(rectanglesInSingleLine != null);
				if (rectanglesInSingleLine.Count != 0)
				{
					list.AddRange(rectanglesInSingleLine);
				}
			}
			return list;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00129700 File Offset: 0x00128700
		private List<Rect> GetRectanglesInSingleLine(PTS.FSLINEDESCRIPTIONSINGLE lineDesc, ContentElement e, int start, int length)
		{
			int num = start + length;
			List<Rect> list = new List<Rect>();
			if (start >= lineDesc.dcpLim)
			{
				return list;
			}
			if (num <= lineDesc.dcpFirst)
			{
				return list;
			}
			int num2 = (start < lineDesc.dcpFirst) ? lineDesc.dcpFirst : start;
			int num3 = (num < lineDesc.dcpLim) ? num : lineDesc.dcpLim;
			Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
			Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(lineDesc.fClearOnLeft), PTS.ToBoolean(lineDesc.fClearOnRight), this.TextParagraph.TextRunCache);
			if (this.IsOptimalParagraph)
			{
				formattingContext.LineFormatLengthTarget = lineDesc.dcpLim - lineDesc.dcpFirst;
			}
			this.TextParagraph.FormatLineCore(line, lineDesc.pfsbreakreclineclient, formattingContext, lineDesc.dcpFirst, lineDesc.dur, PTS.ToBoolean(lineDesc.fTreatedAsFirst), lineDesc.dcpFirst);
			Invariant.Assert(line.SafeLength == lineDesc.dcpLim - lineDesc.dcpFirst, "Line length is out of sync");
			list = line.GetRangeBounds(num2, num3 - num2, TextDpi.FromTextDpi(lineDesc.urStart), TextDpi.FromTextDpi(lineDesc.vrStart));
			Invariant.Assert(list.Count > 0);
			line.Dispose();
			return list;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00129848 File Offset: 0x00128848
		private IInputElement InputHitTestCompositeLines(PTS.FSPOINT pt, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			if (textDetails.cLines == 0)
			{
				return null;
			}
			IInputElement result = null;
			int paragraphStartCharacterPosition = base.Paragraph.ParagraphStartCharacterPosition;
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			foreach (PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite in array)
			{
				if (fslinedescriptioncomposite.cElements != 0 && fslinedescriptioncomposite.vrStart + fslinedescriptioncomposite.dvrAscent + fslinedescriptioncomposite.dvrDescent > pt.v)
				{
					PTS.FSLINEELEMENT[] array2;
					PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref fslinedescriptioncomposite, out array2);
					foreach (PTS.FSLINEELEMENT fslineelement in array2)
					{
						Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
						Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, paragraphStartCharacterPosition);
						if (this.IsOptimalParagraph)
						{
							formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
						}
						using (line)
						{
							this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(fslinedescriptioncomposite.fTreatedAsFirst), fslineelement.dcpFirst);
							Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
							if (fslineelement.urStart + line.CalculateUOffsetShift() <= pt.u && pt.u <= fslineelement.urStart + line.CalculateUOffsetShift() + fslineelement.dur)
							{
								int num = pt.u - fslineelement.urStart;
								Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
								Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
								if (line.Start <= num && num <= line.Start + line.Width)
								{
									result = line.InputHitTest(num);
									break;
								}
							}
						}
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00129AC0 File Offset: 0x00128AC0
		private List<Rect> GetRectanglesInCompositeLines(ContentElement e, int start, int length, ref PTS.FSTEXTDETAILSFULL textDetails)
		{
			ErrorHandler.Assert(!PTS.ToBoolean(textDetails.fDropCapPresent), ErrorHandler.NotSupportedDropCap);
			List<Rect> list = new List<Rect>();
			int num = start - base.Paragraph.ParagraphStartCharacterPosition;
			if (num < 0 || textDetails.cLines == 0)
			{
				return list;
			}
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PtsHelper.LineListCompositeFromTextPara(base.PtsContext, this._paraHandle.Value, ref textDetails, out array);
			foreach (PTS.FSLINEDESCRIPTIONCOMPOSITE fslinedescriptioncomposite in array)
			{
				if (fslinedescriptioncomposite.cElements != 0)
				{
					List<Rect> rectanglesInCompositeLine = this.GetRectanglesInCompositeLine(fslinedescriptioncomposite, e, num, length);
					Invariant.Assert(rectanglesInCompositeLine != null);
					if (rectanglesInCompositeLine.Count != 0)
					{
						list.AddRange(rectanglesInCompositeLine);
					}
				}
			}
			return list;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00129B6C File Offset: 0x00128B6C
		private List<Rect> GetRectanglesInCompositeLine(PTS.FSLINEDESCRIPTIONCOMPOSITE lineDesc, ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			int num = start + length;
			PTS.FSLINEELEMENT[] array;
			PtsHelper.LineElementListFromCompositeLine(base.PtsContext, ref lineDesc, out array);
			foreach (PTS.FSLINEELEMENT fslineelement in array)
			{
				if (start < fslineelement.dcpLim && num > fslineelement.dcpFirst)
				{
					int num2 = (start < fslineelement.dcpFirst) ? fslineelement.dcpFirst : start;
					int num3 = (num < fslineelement.dcpLim) ? num : fslineelement.dcpLim;
					Line line = new Line(base.Paragraph.StructuralCache.TextFormatterHost, this, base.Paragraph.ParagraphStartCharacterPosition);
					Line.FormattingContext formattingContext = new Line.FormattingContext(false, PTS.ToBoolean(fslineelement.fClearOnLeft), PTS.ToBoolean(fslineelement.fClearOnRight), this.TextParagraph.TextRunCache);
					if (this.IsOptimalParagraph)
					{
						formattingContext.LineFormatLengthTarget = fslineelement.dcpLim - fslineelement.dcpFirst;
					}
					this.TextParagraph.FormatLineCore(line, fslineelement.pfsbreakreclineclient, formattingContext, fslineelement.dcpFirst, fslineelement.dur, PTS.ToBoolean(lineDesc.fTreatedAsFirst), fslineelement.dcpFirst);
					Invariant.Assert(line.SafeLength == fslineelement.dcpLim - fslineelement.dcpFirst, "Line length is out of sync");
					List<Rect> rangeBounds = line.GetRangeBounds(num2, num3 - num2, TextDpi.FromTextDpi(fslineelement.urStart), TextDpi.FromTextDpi(lineDesc.vrStart));
					Invariant.Assert(rangeBounds.Count > 0);
					list.AddRange(rangeBounds);
					line.Dispose();
				}
			}
			return list;
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00129D00 File Offset: 0x00128D00
		private bool IsOptimalParagraph
		{
			get
			{
				return this.TextParagraph.IsOptimalParagraph;
			}
		}

		// Token: 0x04000819 RID: 2073
		private int _lineIndexFirstVisual = -1;
	}
}
