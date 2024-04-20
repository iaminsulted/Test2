using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200012B RID: 299
	internal sealed class ListParaClient : ContainerParaClient
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x00113D3E File Offset: 0x00112D3E
		internal ListParaClient(ListParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00113D48 File Offset: 0x00112D48
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			PTS.FlowDirectionToFswdir((FlowDirection)base.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			TextProperties defaultTextProperties = new TextProperties(base.Paragraph.Element, StaticTextPointer.Null, false, false, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
				using (DrawingContext drawingContext = this._visual.RenderOpen())
				{
					this._visual.DrawBackgroundAndBorderIntoContext(drawingContext, backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
					ListMarkerLine listMarkerLine = new ListMarkerLine(base.Paragraph.StructuralCache.TextFormatterHost, this);
					int num = 0;
					for (int i = 0; i < fssubtrackdetails.cParas; i++)
					{
						List list = base.Paragraph.Element as List;
						BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
						PTS.ValidateHandle(baseParaClient);
						if (i == 0)
						{
							num = list.GetListItemIndex(baseParaClient.Paragraph.Element as ListItem);
						}
						if (baseParaClient.IsFirstChunk)
						{
							int firstTextLineBaseline = baseParaClient.GetFirstTextLineBaseline();
							if (base.PageFlowDirection != base.ThisFlowDirection)
							{
								drawingContext.PushTransform(new MatrixTransform(-1.0, 0.0, 0.0, 1.0, TextDpi.FromTextDpi(2 * baseParaClient.Rect.u + baseParaClient.Rect.du), 0.0));
							}
							int index;
							if (2147483647 - i < num)
							{
								index = int.MaxValue;
							}
							else
							{
								index = num + i;
							}
							LineProperties lineProps = new LineProperties(base.Paragraph.Element, base.Paragraph.StructuralCache.FormattingOwner, defaultTextProperties, new MarkerProperties(list, index));
							listMarkerLine.FormatAndDrawVisual(drawingContext, lineProps, baseParaClient.Rect.u, firstTextLineBaseline);
							if (base.PageFlowDirection != base.ThisFlowDirection)
							{
								drawingContext.Pop();
							}
						}
					}
					listMarkerLine.Dispose();
				}
				PtsHelper.UpdateParaListVisuals(base.PtsContext, this._visual.Children, fskupdInherited, array);
				return;
			}
			this._visual.Children.Clear();
		}
	}
}
