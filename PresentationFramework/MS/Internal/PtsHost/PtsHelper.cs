using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000138 RID: 312
	internal static class PtsHelper
	{
		// Token: 0x060008A0 RID: 2208 RVA: 0x00117708 File Offset: 0x00116708
		internal static void UpdateMirroringTransform(FlowDirection parentFD, FlowDirection childFD, ContainerVisual visualChild, double width)
		{
			if (parentFD != childFD)
			{
				MatrixTransform transform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, width, 0.0);
				visualChild.Transform = transform;
				visualChild.SetValue(FrameworkElement.FlowDirectionProperty, childFD);
				return;
			}
			visualChild.Transform = null;
			visualChild.ClearValue(FrameworkElement.FlowDirectionProperty);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00117778 File Offset: 0x00116778
		internal static void ClipChildrenToRect(ContainerVisual visual, Rect rect)
		{
			VisualCollection children = visual.Children;
			for (int i = 0; i < children.Count; i++)
			{
				((ContainerVisual)children[i]).Clip = new RectangleGeometry(rect);
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x001177B4 File Offset: 0x001167B4
		internal static void UpdateFloatingElementVisuals(ContainerVisual visual, List<BaseParaClient> floatingElementList)
		{
			VisualCollection children = visual.Children;
			int num = 0;
			if (floatingElementList == null || floatingElementList.Count == 0)
			{
				children.Clear();
				return;
			}
			for (int i = 0; i < floatingElementList.Count; i++)
			{
				Visual visual2 = floatingElementList[i].Visual;
				while (num < children.Count && children[num] != visual2)
				{
					children.RemoveAt(num);
				}
				if (num == children.Count)
				{
					children.Add(visual2);
				}
				num++;
			}
			if (children.Count > floatingElementList.Count)
			{
				children.RemoveRange(floatingElementList.Count, children.Count - floatingElementList.Count);
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00117854 File Offset: 0x00116854
		internal static void ArrangeTrack(PtsContext ptsContext, ref PTS.FSTRACKDESCRIPTION trackDesc, uint fswdirTrack)
		{
			if (trackDesc.pfstrack != IntPtr.Zero)
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas != 0)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
					PtsHelper.ArrangeParaList(ptsContext, trackDesc.fsrc, arrayParaDesc, fswdirTrack);
				}
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x001178B4 File Offset: 0x001168B4
		internal static void ArrangeParaList(PtsContext ptsContext, PTS.FSRECT rcTrackContent, PTS.FSPARADESCRIPTION[] arrayParaDesc, uint fswdirTrack)
		{
			int num = 0;
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				if (i == 0)
				{
					uint num2 = PTS.FlowDirectionToFswdir(baseParaClient.PageFlowDirection);
					if (fswdirTrack != num2)
					{
						PTS.FSRECT pageRect = baseParaClient.Paragraph.StructuralCache.CurrentArrangeContext.PageContext.PageRect;
						PTS.Validate(PTS.FsTransformRectangle(fswdirTrack, ref pageRect, ref rcTrackContent, num2, out rcTrackContent));
					}
				}
				int dvrTopSpace = arrayParaDesc[i].dvrTopSpace;
				PTS.FSRECT rcPara = rcTrackContent;
				rcPara.v += num + dvrTopSpace;
				rcPara.dv = arrayParaDesc[i].dvrUsed - dvrTopSpace;
				baseParaClient.Arrange(arrayParaDesc[i].pfspara, rcPara, dvrTopSpace, fswdirTrack);
				num += arrayParaDesc[i].dvrUsed;
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00117994 File Offset: 0x00116994
		internal static void UpdateTrackVisuals(PtsContext ptsContext, VisualCollection visualCollection, PTS.FSKUPDATE fskupdInherited, ref PTS.FSTRACKDESCRIPTION trackDesc)
		{
			PTS.FSKUPDATE fskupdate = trackDesc.fsupdinf.fskupd;
			if (trackDesc.fsupdinf.fskupd == PTS.FSKUPDATE.fskupdInherited)
			{
				fskupdate = fskupdInherited;
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			ErrorHandler.Assert(fskupdate != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
			bool flag = trackDesc.pfstrack == IntPtr.Zero;
			if (!flag)
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
				flag = (fstrackdetails.cParas == 0);
				if (!flag)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
					PtsHelper.UpdateParaListVisuals(ptsContext, visualCollection, fskupdate, arrayParaDesc);
				}
			}
			if (flag)
			{
				visualCollection.Clear();
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00117A30 File Offset: 0x00116A30
		internal static void UpdateParaListVisuals(PtsContext ptsContext, VisualCollection visualCollection, PTS.FSKUPDATE fskupdInherited, PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				PTS.FSKUPDATE fskupdate = arrayParaDesc[i].fsupdinf.fskupd;
				if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
				{
					fskupdate = fskupdInherited;
				}
				if (fskupdate == PTS.FSKUPDATE.fskupdNew)
				{
					Visual visual = VisualTreeHelper.GetParent(baseParaClient.Visual) as Visual;
					if (visual != null)
					{
						ContainerVisual containerVisual = visual as ContainerVisual;
						Invariant.Assert(containerVisual != null, "parent should always derives from ContainerVisual");
						containerVisual.Children.Remove(baseParaClient.Visual);
					}
					visualCollection.Insert(i, baseParaClient.Visual);
					baseParaClient.ValidateVisual(fskupdate);
				}
				else
				{
					while (visualCollection[i] != baseParaClient.Visual)
					{
						visualCollection.RemoveAt(i);
						Invariant.Assert(i < visualCollection.Count);
					}
					if (fskupdate == PTS.FSKUPDATE.fskupdChangeInside || fskupdate == PTS.FSKUPDATE.fskupdShifted)
					{
						baseParaClient.ValidateVisual(fskupdate);
					}
				}
			}
			if (arrayParaDesc.Length < visualCollection.Count)
			{
				visualCollection.RemoveRange(arrayParaDesc.Length, visualCollection.Count - arrayParaDesc.Length);
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00117B2C File Offset: 0x00116B2C
		internal static void UpdateViewportTrack(PtsContext ptsContext, ref PTS.FSTRACKDESCRIPTION trackDesc, ref PTS.FSRECT viewport)
		{
			if (trackDesc.pfstrack != IntPtr.Zero)
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas != 0)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
					PtsHelper.UpdateViewportParaList(ptsContext, arrayParaDesc, ref viewport);
				}
			}
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00117B84 File Offset: 0x00116B84
		internal static void UpdateViewportParaList(PtsContext ptsContext, PTS.FSPARADESCRIPTION[] arrayParaDesc, ref PTS.FSRECT viewport)
		{
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				baseParaClient.UpdateViewport(ref viewport);
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00117BC4 File Offset: 0x00116BC4
		internal static IInputElement InputHitTestTrack(PtsContext ptsContext, PTS.FSPOINT pt, ref PTS.FSTRACKDESCRIPTION trackDesc)
		{
			if (trackDesc.pfstrack == IntPtr.Zero)
			{
				return null;
			}
			IInputElement result = null;
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
				result = PtsHelper.InputHitTestParaList(ptsContext, pt, ref trackDesc.fsrc, arrayParaDesc);
			}
			return result;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00117C28 File Offset: 0x00116C28
		internal static IInputElement InputHitTestParaList(PtsContext ptsContext, PTS.FSPOINT pt, ref PTS.FSRECT rcTrack, PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			IInputElement inputElement = null;
			int num = 0;
			while (num < arrayParaDesc.Length && inputElement == null)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[num].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				if (baseParaClient.Rect.Contains(pt))
				{
					inputElement = baseParaClient.InputHitTest(pt);
				}
				num++;
			}
			return inputElement;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00117C80 File Offset: 0x00116C80
		internal static List<Rect> GetRectanglesInTrack(PtsContext ptsContext, ContentElement e, int start, int length, ref PTS.FSTRACKDESCRIPTION trackDesc)
		{
			List<Rect> result = new List<Rect>();
			if (trackDesc.pfstrack == IntPtr.Zero)
			{
				return result;
			}
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
				result = PtsHelper.GetRectanglesInParaList(ptsContext, e, start, length, arrayParaDesc);
			}
			return result;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00117CE8 File Offset: 0x00116CE8
		internal static List<Rect> GetRectanglesInParaList(PtsContext ptsContext, ContentElement e, int start, int length, PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
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
			return list;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00117D54 File Offset: 0x00116D54
		internal static List<Rect> OffsetRectangleList(List<Rect> rectangleList, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>(rectangleList.Count);
			for (int i = 0; i < rectangleList.Count; i++)
			{
				Rect item = rectangleList[i];
				item.X += xOffset;
				item.Y += yOffset;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00117DAC File Offset: 0x00116DAC
		internal unsafe static void SectionListFromPage(PtsContext ptsContext, IntPtr page, ref PTS.FSPAGEDETAILS pageDetails, out PTS.FSSECTIONDESCRIPTION[] arraySectionDesc)
		{
			arraySectionDesc = new PTS.FSSECTIONDESCRIPTION[pageDetails.u.complex.cSections];
			PTS.FSSECTIONDESCRIPTION[] array;
			PTS.FSSECTIONDESCRIPTION* rgSectionDescription;
			if ((array = arraySectionDesc) == null || array.Length == 0)
			{
				rgSectionDescription = null;
			}
			else
			{
				rgSectionDescription = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryPageSectionList(ptsContext.Context, page, pageDetails.u.complex.cSections, rgSectionDescription, out num));
			array = null;
			ErrorHandler.Assert(pageDetails.u.complex.cSections == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00117E2C File Offset: 0x00116E2C
		internal unsafe static void TrackListFromSubpage(PtsContext ptsContext, IntPtr subpage, ref PTS.FSSUBPAGEDETAILS subpageDetails, out PTS.FSTRACKDESCRIPTION[] arrayTrackDesc)
		{
			arrayTrackDesc = new PTS.FSTRACKDESCRIPTION[subpageDetails.u.complex.cBasicColumns];
			PTS.FSTRACKDESCRIPTION[] array;
			PTS.FSTRACKDESCRIPTION* rgColumnDescription;
			if ((array = arrayTrackDesc) == null || array.Length == 0)
			{
				rgColumnDescription = null;
			}
			else
			{
				rgColumnDescription = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQuerySubpageBasicColumnList(ptsContext.Context, subpage, subpageDetails.u.complex.cBasicColumns, rgColumnDescription, out num));
			array = null;
			ErrorHandler.Assert(subpageDetails.u.complex.cBasicColumns == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00117EAC File Offset: 0x00116EAC
		internal unsafe static void TrackListFromSection(PtsContext ptsContext, IntPtr section, ref PTS.FSSECTIONDETAILS sectionDetails, out PTS.FSTRACKDESCRIPTION[] arrayTrackDesc)
		{
			arrayTrackDesc = new PTS.FSTRACKDESCRIPTION[sectionDetails.u.withpagenotes.cBasicColumns];
			PTS.FSTRACKDESCRIPTION[] array;
			PTS.FSTRACKDESCRIPTION* rgColumnDescription;
			if ((array = arrayTrackDesc) == null || array.Length == 0)
			{
				rgColumnDescription = null;
			}
			else
			{
				rgColumnDescription = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQuerySectionBasicColumnList(ptsContext.Context, section, sectionDetails.u.withpagenotes.cBasicColumns, rgColumnDescription, out num));
			array = null;
			ErrorHandler.Assert(sectionDetails.u.withpagenotes.cBasicColumns == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00117F2C File Offset: 0x00116F2C
		internal unsafe static void ParaListFromTrack(PtsContext ptsContext, IntPtr track, ref PTS.FSTRACKDETAILS trackDetails, out PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			arrayParaDesc = new PTS.FSPARADESCRIPTION[trackDetails.cParas];
			PTS.FSPARADESCRIPTION[] array;
			PTS.FSPARADESCRIPTION* rgParaDesc;
			if ((array = arrayParaDesc) == null || array.Length == 0)
			{
				rgParaDesc = null;
			}
			else
			{
				rgParaDesc = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryTrackParaList(ptsContext.Context, track, trackDetails.cParas, rgParaDesc, out num));
			array = null;
			ErrorHandler.Assert(trackDetails.cParas == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00117F90 File Offset: 0x00116F90
		internal unsafe static void ParaListFromSubtrack(PtsContext ptsContext, IntPtr subtrack, ref PTS.FSSUBTRACKDETAILS subtrackDetails, out PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			arrayParaDesc = new PTS.FSPARADESCRIPTION[subtrackDetails.cParas];
			PTS.FSPARADESCRIPTION[] array;
			PTS.FSPARADESCRIPTION* rgParaDesc;
			if ((array = arrayParaDesc) == null || array.Length == 0)
			{
				rgParaDesc = null;
			}
			else
			{
				rgParaDesc = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQuerySubtrackParaList(ptsContext.Context, subtrack, subtrackDetails.cParas, rgParaDesc, out num));
			array = null;
			ErrorHandler.Assert(subtrackDetails.cParas == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00117FF4 File Offset: 0x00116FF4
		internal unsafe static void LineListSimpleFromTextPara(PtsContext ptsContext, IntPtr para, ref PTS.FSTEXTDETAILSFULL textDetails, out PTS.FSLINEDESCRIPTIONSINGLE[] arrayLineDesc)
		{
			arrayLineDesc = new PTS.FSLINEDESCRIPTIONSINGLE[textDetails.cLines];
			PTS.FSLINEDESCRIPTIONSINGLE[] array;
			PTS.FSLINEDESCRIPTIONSINGLE* rgLineDesc;
			if ((array = arrayLineDesc) == null || array.Length == 0)
			{
				rgLineDesc = null;
			}
			else
			{
				rgLineDesc = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryLineListSingle(ptsContext.Context, para, textDetails.cLines, rgLineDesc, out num));
			array = null;
			ErrorHandler.Assert(textDetails.cLines == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00118058 File Offset: 0x00117058
		internal unsafe static void LineListCompositeFromTextPara(PtsContext ptsContext, IntPtr para, ref PTS.FSTEXTDETAILSFULL textDetails, out PTS.FSLINEDESCRIPTIONCOMPOSITE[] arrayLineDesc)
		{
			arrayLineDesc = new PTS.FSLINEDESCRIPTIONCOMPOSITE[textDetails.cLines];
			PTS.FSLINEDESCRIPTIONCOMPOSITE[] array;
			PTS.FSLINEDESCRIPTIONCOMPOSITE* rgLineDescription;
			if ((array = arrayLineDesc) == null || array.Length == 0)
			{
				rgLineDescription = null;
			}
			else
			{
				rgLineDescription = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryLineListComposite(ptsContext.Context, para, textDetails.cLines, rgLineDescription, out num));
			array = null;
			ErrorHandler.Assert(textDetails.cLines == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x001180BC File Offset: 0x001170BC
		internal unsafe static void LineElementListFromCompositeLine(PtsContext ptsContext, ref PTS.FSLINEDESCRIPTIONCOMPOSITE lineDesc, out PTS.FSLINEELEMENT[] arrayLineElement)
		{
			arrayLineElement = new PTS.FSLINEELEMENT[lineDesc.cElements];
			PTS.FSLINEELEMENT[] array;
			PTS.FSLINEELEMENT* rgLineElement;
			if ((array = arrayLineElement) == null || array.Length == 0)
			{
				rgLineElement = null;
			}
			else
			{
				rgLineElement = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryLineCompositeElementList(ptsContext.Context, lineDesc.pline, lineDesc.cElements, rgLineElement, out num));
			array = null;
			ErrorHandler.Assert(lineDesc.cElements == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00118124 File Offset: 0x00117124
		internal unsafe static void AttachedObjectListFromParagraph(PtsContext ptsContext, IntPtr para, int cAttachedObject, out PTS.FSATTACHEDOBJECTDESCRIPTION[] arrayAttachedObjectDesc)
		{
			arrayAttachedObjectDesc = new PTS.FSATTACHEDOBJECTDESCRIPTION[cAttachedObject];
			PTS.FSATTACHEDOBJECTDESCRIPTION[] array;
			PTS.FSATTACHEDOBJECTDESCRIPTION* rgAttachedObjects;
			if ((array = arrayAttachedObjectDesc) == null || array.Length == 0)
			{
				rgAttachedObjects = null;
			}
			else
			{
				rgAttachedObjects = &array[0];
			}
			int num;
			PTS.Validate(PTS.FsQueryAttachedObjectList(ptsContext.Context, para, cAttachedObject, rgAttachedObjects, out num));
			array = null;
			ErrorHandler.Assert(cAttachedObject == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00118178 File Offset: 0x00117178
		internal static TextContentRange TextContentRangeFromTrack(PtsContext ptsContext, IntPtr pfstrack)
		{
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, pfstrack, out fstrackdetails));
			TextContentRange textContentRange = new TextContentRange();
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromTrack(ptsContext, pfstrack, ref fstrackdetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = ptsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
			}
			return textContentRange;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x001181F0 File Offset: 0x001171F0
		internal static double CalculatePageMarginAdjustment(StructuralCache structuralCache, double pageMarginWidth)
		{
			double result = 0.0;
			DependencyObject element = structuralCache.Section.Element;
			if (element is FlowDocument)
			{
				ColumnPropertiesGroup columnPropertiesGroup = new ColumnPropertiesGroup(element);
				if (!columnPropertiesGroup.IsColumnWidthFlexible)
				{
					double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(element);
					double pageFontSize = (double)structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
					FontFamily pageFontFamily = (FontFamily)structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
					int cColumns = PtsHelper.CalculateColumnCount(columnPropertiesGroup, lineHeightValue, pageMarginWidth, pageFontSize, pageFontFamily, true);
					double num;
					double num2;
					double num3;
					PtsHelper.GetColumnMetrics(columnPropertiesGroup, pageMarginWidth, pageFontSize, pageFontFamily, true, cColumns, ref lineHeightValue, out num, out num2, out num3);
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00118288 File Offset: 0x00117288
		internal static int CalculateColumnCount(ColumnPropertiesGroup columnProperties, double lineHeight, double pageWidth, double pageFontSize, FontFamily pageFontFamily, bool enableColumns)
		{
			int val = 1;
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			if (enableColumns)
			{
				double num;
				if (columnProperties.ColumnGapAuto)
				{
					num = 1.0 * lineHeight;
				}
				else
				{
					num = columnProperties.ColumnGap;
				}
				if (!columnProperties.ColumnWidthAuto)
				{
					double columnWidth = columnProperties.ColumnWidth;
					val = (int)((pageWidth + num) / (columnWidth + num));
				}
				else
				{
					double num2 = 20.0 * pageFontSize;
					val = (int)((pageWidth + num) / (num2 + num));
				}
			}
			return Math.Max(1, Math.Min(999, val));
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00118300 File Offset: 0x00117300
		internal static void GetColumnMetrics(ColumnPropertiesGroup columnProperties, double pageWidth, double pageFontSize, FontFamily pageFontFamily, bool enableColumns, int cColumns, ref double lineHeight, out double columnWidth, out double freeSpace, out double gapSpace)
		{
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			if (!enableColumns)
			{
				Invariant.Assert(cColumns == 1);
				columnWidth = pageWidth;
				gapSpace = 0.0;
				lineHeight = 0.0;
				freeSpace = 0.0;
			}
			else
			{
				if (columnProperties.ColumnWidthAuto)
				{
					columnWidth = 20.0 * pageFontSize;
				}
				else
				{
					columnWidth = columnProperties.ColumnWidth;
				}
				if (columnProperties.ColumnGapAuto)
				{
					gapSpace = 1.0 * lineHeight;
				}
				else
				{
					gapSpace = columnProperties.ColumnGap;
				}
			}
			columnWidth = Math.Max(1.0, Math.Min(columnWidth, pageWidth));
			freeSpace = pageWidth - (double)cColumns * columnWidth - (double)(cColumns - 1) * gapSpace;
			freeSpace = Math.Max(0.0, freeSpace);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x001183D4 File Offset: 0x001173D4
		internal unsafe static void GetColumnsInfo(ColumnPropertiesGroup columnProperties, double lineHeight, double pageWidth, double pageFontSize, FontFamily pageFontFamily, int cColumns, PTS.FSCOLUMNINFO* pfscolinfo, bool enableColumns)
		{
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			double num;
			double num2;
			double num3;
			PtsHelper.GetColumnMetrics(columnProperties, pageWidth, pageFontSize, pageFontFamily, enableColumns, cColumns, ref lineHeight, out num, out num2, out num3);
			if (!columnProperties.IsColumnWidthFlexible)
			{
				for (int i = 0; i < cColumns; i++)
				{
					pfscolinfo[i].durBefore = TextDpi.ToTextDpi((i == 0) ? 0.0 : num3);
					pfscolinfo[i].durWidth = TextDpi.ToTextDpi(num);
					pfscolinfo[i].durBefore = Math.Max(0, pfscolinfo[i].durBefore);
					pfscolinfo[i].durWidth = Math.Max(1, pfscolinfo[i].durWidth);
				}
				return;
			}
			for (int j = 0; j < cColumns; j++)
			{
				if (columnProperties.ColumnSpaceDistribution == ColumnSpaceDistribution.Right)
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi((j == cColumns - 1) ? (num + num2) : num);
				}
				else if (columnProperties.ColumnSpaceDistribution == ColumnSpaceDistribution.Left)
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi((j == 0) ? (num + num2) : num);
				}
				else
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi(num + num2 / (double)cColumns);
				}
				if (pfscolinfo[j].durWidth > TextDpi.ToTextDpi(pageWidth))
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi(pageWidth);
				}
				pfscolinfo[j].durBefore = TextDpi.ToTextDpi((j == 0) ? 0.0 : num3);
				pfscolinfo[j].durBefore = Math.Max(0, pfscolinfo[j].durBefore);
				pfscolinfo[j].durWidth = Math.Max(1, pfscolinfo[j].durWidth);
			}
		}
	}
}
