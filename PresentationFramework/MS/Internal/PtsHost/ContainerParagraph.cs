﻿using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000113 RID: 275
	internal class ContainerParagraph : BaseParagraph, ISegment
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0010AEA3 File Offset: 0x00109EA3
		internal ContainerParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0010AEB0 File Offset: 0x00109EB0
		public override void Dispose()
		{
			BaseParagraph baseParagraph = this._firstChild;
			while (baseParagraph != null)
			{
				BaseParagraph baseParagraph2 = baseParagraph;
				baseParagraph = baseParagraph.Next;
				baseParagraph2.Dispose();
				baseParagraph2.Next = null;
				baseParagraph2.Previous = null;
			}
			this._firstChild = (this._lastFetchedChild = null);
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0010AF04 File Offset: 0x00109F04
		void ISegment.GetFirstPara(out int fSuccessful, out IntPtr firstParaName)
		{
			if (this._ur != null)
			{
				int cpfromElement = TextContainerHelper.GetCPFromElement(base.StructuralCache.TextContainer, base.Element, ElementEdge.AfterStart);
				if (this._ur.SyncPara != null && cpfromElement == this._ur.SyncPara.ParagraphStartCharacterPosition)
				{
					this._ur.SyncPara.Previous = null;
					if (this._ur.Next != null && this._ur.Next.FirstPara == this._ur.SyncPara)
					{
						this._ur.SyncPara.SetUpdateInfo(this._ur.Next.ChangeType, false);
					}
					else
					{
						this._ur.SyncPara.SetUpdateInfo(PTS.FSKCHANGE.fskchNone, this._ur.Next == null);
					}
					Invariant.Assert(this._firstChild == null);
					this._firstChild = this._ur.SyncPara;
					this._ur = this._ur.Next;
				}
			}
			if (this._firstChild != null)
			{
				if (base.StructuralCache.CurrentFormatContext.IncrementalUpdate && this._ur == null && this.NeedsUpdate() && !this._firstParaValidInUpdateMode)
				{
					if (!base.StructuralCache.CurrentFormatContext.FinitePage)
					{
						for (BaseParagraph baseParagraph = this._firstChild; baseParagraph != null; baseParagraph = baseParagraph.Next)
						{
							baseParagraph.Dispose();
						}
						this._firstChild = null;
					}
					this._firstParaValidInUpdateMode = true;
				}
				else if (this._ur != null && this._ur.InProcessing && this._ur.FirstPara == this._firstChild)
				{
					this._firstChild.SetUpdateInfo(PTS.FSKCHANGE.fskchInside, false);
				}
			}
			if (this._firstChild == null)
			{
				ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
				this._firstChild = this.GetParagraph(contentStart, false);
				if (this._ur != null && this._firstChild != null)
				{
					this._firstChild.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
				}
			}
			if (base.StructuralCache.CurrentFormatContext.IncrementalUpdate)
			{
				this._firstParaValidInUpdateMode = true;
			}
			this._lastFetchedChild = this._firstChild;
			fSuccessful = PTS.FromBoolean(this._firstChild != null);
			firstParaName = ((this._firstChild != null) ? this._firstChild.Handle : IntPtr.Zero);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0010B148 File Offset: 0x0010A148
		void ISegment.GetNextPara(BaseParagraph prevParagraph, out int fFound, out IntPtr nextParaName)
		{
			if (this._ur != null)
			{
				int paragraphEndCharacterPosition = prevParagraph.ParagraphEndCharacterPosition;
				if (this._ur.SyncPara != null && paragraphEndCharacterPosition == this._ur.SyncPara.ParagraphStartCharacterPosition)
				{
					this._ur.SyncPara.Previous = prevParagraph;
					prevParagraph.Next = this._ur.SyncPara;
					if (this._ur.Next != null && this._ur.Next.FirstPara == this._ur.SyncPara)
					{
						this._ur.SyncPara.SetUpdateInfo(this._ur.Next.ChangeType, false);
					}
					else
					{
						this._ur.SyncPara.SetUpdateInfo(PTS.FSKCHANGE.fskchNone, this._ur.Next == null);
					}
					this._ur = this._ur.Next;
				}
				else
				{
					Invariant.Assert(this._ur.SyncPara == null || paragraphEndCharacterPosition < this._ur.SyncPara.ParagraphStartCharacterPosition);
					if (!this._ur.InProcessing && this._ur.FirstPara != prevParagraph.Next && prevParagraph.Next != null)
					{
						prevParagraph.Next.SetUpdateInfo(PTS.FSKCHANGE.fskchNone, false);
					}
					else if (this._ur.FirstPara != null && this._ur.FirstPara == prevParagraph.Next)
					{
						this._ur.InProcessing = true;
						prevParagraph.Next.SetUpdateInfo(PTS.FSKCHANGE.fskchInside, false);
					}
				}
			}
			BaseParagraph baseParagraph = prevParagraph.Next;
			if (baseParagraph == null)
			{
				ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(base.StructuralCache.TextContainer, prevParagraph.ParagraphEndCharacterPosition, LogicalDirection.Forward);
				baseParagraph = this.GetParagraph(textPointerFromCP, true);
				if (baseParagraph != null)
				{
					baseParagraph.Previous = prevParagraph;
					prevParagraph.Next = baseParagraph;
					if (this._changeType == PTS.FSKCHANGE.fskchInside)
					{
						baseParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
					}
				}
			}
			if (baseParagraph != null)
			{
				fFound = 1;
				nextParaName = baseParagraph.Handle;
				this._lastFetchedChild = baseParagraph;
				return;
			}
			fFound = 0;
			nextParaName = IntPtr.Zero;
			this._lastFetchedChild = prevParagraph;
			this._ur = null;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0010B344 File Offset: 0x0010A344
		void ISegment.UpdGetFirstChangeInSegment(out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange)
		{
			this.BuildUpdateRecord();
			fFound = PTS.FromBoolean(this._ur != null);
			fChangeFirst = PTS.FromBoolean(this._ur != null && (this._firstChild == null || this._firstChild == this._ur.FirstPara));
			if (PTS.ToBoolean(fFound) && !PTS.ToBoolean(fChangeFirst))
			{
				if (this._ur.FirstPara == null)
				{
					BaseParagraph baseParagraph = this._lastFetchedChild;
					while (baseParagraph.Next != null)
					{
						baseParagraph = baseParagraph.Next;
					}
					nmpBeforeChange = baseParagraph.Handle;
				}
				else
				{
					if (this._ur.ChangeType == PTS.FSKCHANGE.fskchNew)
					{
						this._ur.FirstPara.Previous.Next = null;
					}
					nmpBeforeChange = this._ur.FirstPara.Previous.Handle;
				}
			}
			else
			{
				nmpBeforeChange = IntPtr.Zero;
			}
			if (PTS.ToBoolean(fFound))
			{
				this._ur.InProcessing = PTS.ToBoolean(fChangeFirst);
				this._changeType = PTS.FSKCHANGE.fskchInside;
				this._stopAsking = false;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0010B448 File Offset: 0x0010A448
		internal void UpdGetSegmentChange(out PTS.FSKCHANGE fskch)
		{
			if (base.StructuralCache.CurrentFormatContext.FinitePage)
			{
				DtrList dtrList = base.StructuralCache.DtrsFromRange(TextContainerHelper.GetCPFromElement(base.StructuralCache.TextContainer, base.Element, ElementEdge.BeforeStart), base.LastFormatCch);
				if (dtrList != null)
				{
					int cpfromElement = TextContainerHelper.GetCPFromElement(base.StructuralCache.TextContainer, base.Element, ElementEdge.AfterStart);
					DirtyTextRange dirtyTextRange = dtrList[0];
					int num = cpfromElement;
					BaseParagraph baseParagraph = this._firstChild;
					if (num < dirtyTextRange.StartIndex)
					{
						while (baseParagraph != null && num + baseParagraph.LastFormatCch <= dirtyTextRange.StartIndex && (num + baseParagraph.LastFormatCch != dirtyTextRange.StartIndex || !(baseParagraph is TextParagraph)))
						{
							num += baseParagraph.Cch;
							baseParagraph = baseParagraph.Next;
						}
						if (baseParagraph != null)
						{
							baseParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchInside, false);
						}
					}
					else
					{
						baseParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
					}
					if (baseParagraph != null)
					{
						for (baseParagraph = baseParagraph.Next; baseParagraph != null; baseParagraph = baseParagraph.Next)
						{
							baseParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
						}
					}
					this._changeType = PTS.FSKCHANGE.fskchInside;
				}
			}
			fskch = this._changeType;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0010B54B File Offset: 0x0010A54B
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			base.GetParaProperties(ref fspap, false);
			fspap.idobj = PtsHost.ContainerParagraphId;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0010B560 File Offset: 0x0010A560
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			ContainerParaClient containerParaClient = new ContainerParaClient(this);
			paraClientHandle = containerParaClient.Handle;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0010B57C File Offset: 0x0010A57C
		internal void FormatParaFinite(ContainerParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, int iArea, IntPtr footnoteRejector, IntPtr geometry, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, MarginCollapsingState mcs, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecOut, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			if (mcs != null && pbrkrecIn != IntPtr.Zero)
			{
				mcs = null;
			}
			int num2 = 0;
			int num3 = 0;
			MarginCollapsingState marginCollapsingState = null;
			Invariant.Assert(base.Element is Block || base.Element is ListItem);
			fskclearIn = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			PTS.FSRECT fsrect = fsrcToFill;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (num != fswdir)
			{
				PTS.FSRECT pageRect = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrect, num, out fsrect));
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrcToFill, num, out fsrcToFill));
				mbpInfo.MirrorMargin();
			}
			fsrect.u += mbpInfo.MBPLeft;
			fsrect.du -= mbpInfo.MBPLeft + mbpInfo.MBPRight;
			fsrect.u = Math.Max(Math.Min(fsrect.u, fsrcToFill.u + fsrcToFill.du - 1), fsrcToFill.u);
			fsrect.du = Math.Max(fsrect.du, 0);
			if (pbrkrecIn == IntPtr.Zero)
			{
				MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbpInfo, mcs, out marginCollapsingState, out num2);
				if (PTS.ToBoolean(fSuppressTopSpace))
				{
					num2 = 0;
				}
				fsrect.v += num2 + mbpInfo.BPTop;
				fsrect.dv -= num2 + mbpInfo.BPTop;
				fsrect.v = Math.Max(Math.Min(fsrect.v, fsrcToFill.v + fsrcToFill.dv - 1), fsrcToFill.v);
				fsrect.dv = Math.Max(fsrect.dv, 0);
			}
			int num4 = 0;
			try
			{
				PTS.Validate(PTS.FsFormatSubtrackFinite(base.PtsContext.Context, pbrkrecIn, fBRFromPreviousPage, base.Handle, iArea, footnoteRejector, geometry, fEmptyOk, fSuppressTopSpace, num, ref fsrect, (marginCollapsingState != null) ? marginCollapsingState.Handle : IntPtr.Zero, fskclearIn, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfspara, out pbrkrecOut, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out num4), base.PtsContext);
			}
			finally
			{
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
				if (num4 > 1073741823)
				{
					num4 = 0;
				}
			}
			dvrTopSpace = ((mbpInfo.BPTop != 0) ? num2 : num4);
			dvrUsed += fsrect.v - fsrcToFill.v;
			if (fsfmtr.kstop >= PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace)
			{
				dvrUsed = 0;
			}
			if (pmcsclientOut != IntPtr.Zero)
			{
				marginCollapsingState = (base.PtsContext.HandleToObject(pmcsclientOut) as MarginCollapsingState);
				PTS.ValidateHandle(marginCollapsingState);
				pmcsclientOut = IntPtr.Zero;
			}
			if (fsfmtr.kstop == PTS.FSFMTRKSTOP.fmtrGoalReached)
			{
				MarginCollapsingState marginCollapsingState2 = null;
				MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbpInfo, marginCollapsingState, out marginCollapsingState2, out num3);
				pmcsclientOut = ((marginCollapsingState2 != null) ? marginCollapsingState2.Handle : IntPtr.Zero);
				dvrUsed += num3 + mbpInfo.BPBottom;
				dvrUsed = Math.Min(fsrcToFill.dv, dvrUsed);
			}
			if (marginCollapsingState != null)
			{
				marginCollapsingState.Dispose();
				marginCollapsingState = null;
			}
			fsbbox.fsrc.u = fsbbox.fsrc.u - mbpInfo.MBPLeft;
			fsbbox.fsrc.du = fsbbox.fsrc.du + (mbpInfo.MBPLeft + mbpInfo.MBPRight);
			if (num != fswdir)
			{
				PTS.FSRECT pageRect2 = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformBbox(num, ref pageRect2, ref fsbbox, fswdir, out fsbbox));
			}
			paraClient.SetChunkInfo(pbrkrecIn == IntPtr.Zero, pbrkrecOut == IntPtr.Zero);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0010B938 File Offset: 0x0010A938
		internal void FormatParaBottomless(ContainerParaClient paraClient, int iArea, IntPtr geometry, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, MarginCollapsingState mcs, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState;
			int num2;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbpInfo, mcs, out marginCollapsingState, out num2);
			if (PTS.ToBoolean(fSuppressTopSpace))
			{
				num2 = 0;
			}
			Invariant.Assert(base.Element is Block || base.Element is ListItem);
			fskclearIn = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			if (num != fswdir)
			{
				PTS.FSRECT fsrect = new PTS.FSRECT(urTrack, 0, durTrack, 0);
				PTS.FSRECT pageRect = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrect, num, out fsrect));
				urTrack = fsrect.u;
				durTrack = fsrect.du;
				mbpInfo.MirrorMargin();
			}
			int num3 = 0;
			int ur = Math.Max(Math.Min(urTrack + mbpInfo.MBPLeft, urTrack + durTrack - 1), urTrack);
			int dur = Math.Max(durTrack - (mbpInfo.MBPLeft + mbpInfo.MBPRight), 0);
			int num4 = vrTrack + (num2 + mbpInfo.BPTop);
			try
			{
				PTS.Validate(PTS.FsFormatSubtrackBottomless(base.PtsContext.Context, base.Handle, iArea, geometry, fSuppressTopSpace, num, ur, dur, num4, (marginCollapsingState != null) ? marginCollapsingState.Handle : IntPtr.Zero, fskclearIn, fInterruptable, out fsfmtrbl, out pfspara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out num3, out fPageBecomesUninterruptable), base.PtsContext);
			}
			finally
			{
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
			}
			if (fsfmtrbl != PTS.FSFMTRBL.fmtrblCollision)
			{
				if (pmcsclientOut != IntPtr.Zero)
				{
					marginCollapsingState = (base.PtsContext.HandleToObject(pmcsclientOut) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
					pmcsclientOut = IntPtr.Zero;
				}
				MarginCollapsingState marginCollapsingState2;
				int num5;
				MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbpInfo, marginCollapsingState, out marginCollapsingState2, out num5);
				pmcsclientOut = ((marginCollapsingState2 != null) ? marginCollapsingState2.Handle : IntPtr.Zero);
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
				dvrTopSpace = ((mbpInfo.BPTop != 0) ? num2 : num3);
				dvrUsed += num4 - vrTrack + num5 + mbpInfo.BPBottom;
			}
			else
			{
				pfspara = IntPtr.Zero;
				dvrTopSpace = 0;
			}
			fsbbox.fsrc.u = fsbbox.fsrc.u - mbpInfo.MBPLeft;
			fsbbox.fsrc.du = fsbbox.fsrc.du + (mbpInfo.MBPLeft + mbpInfo.MBPRight);
			if (num != fswdir)
			{
				PTS.FSRECT pageRect2 = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformBbox(num, ref pageRect2, ref fsbbox, fswdir, out fsbbox));
			}
			paraClient.SetChunkInfo(true, true);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0010BBE0 File Offset: 0x0010ABE0
		internal void UpdateBottomlessPara(IntPtr pfspara, ContainerParaClient paraClient, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, MarginCollapsingState mcs, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState;
			int num2;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbpInfo, mcs, out marginCollapsingState, out num2);
			if (PTS.ToBoolean(fSuppressTopSpace))
			{
				num2 = 0;
			}
			Invariant.Assert(base.Element is Block || base.Element is ListItem);
			fskclearIn = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			if (num != fswdir)
			{
				PTS.FSRECT fsrect = new PTS.FSRECT(urTrack, 0, durTrack, 0);
				PTS.FSRECT pageRect = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(fswdir, ref pageRect, ref fsrect, num, out fsrect));
				urTrack = fsrect.u;
				durTrack = fsrect.du;
				mbpInfo.MirrorMargin();
			}
			int num3 = 0;
			int ur = Math.Max(Math.Min(urTrack + mbpInfo.MBPLeft, urTrack + durTrack - 1), urTrack);
			int dur = Math.Max(durTrack - (mbpInfo.MBPLeft + mbpInfo.MBPRight), 0);
			int num4 = vrTrack + (num2 + mbpInfo.BPTop);
			try
			{
				PTS.Validate(PTS.FsUpdateBottomlessSubtrack(base.PtsContext.Context, pfspara, base.Handle, iArea, pfsgeom, fSuppressTopSpace, num, ur, dur, num4, (marginCollapsingState != null) ? marginCollapsingState.Handle : IntPtr.Zero, fskclearIn, fInterruptable, out fsfmtrbl, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out num3, out fPageBecomesUninterruptable), base.PtsContext);
			}
			finally
			{
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
			}
			if (fsfmtrbl != PTS.FSFMTRBL.fmtrblCollision)
			{
				if (pmcsclientOut != IntPtr.Zero)
				{
					marginCollapsingState = (base.PtsContext.HandleToObject(pmcsclientOut) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
					pmcsclientOut = IntPtr.Zero;
				}
				MarginCollapsingState marginCollapsingState2;
				int num5;
				MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbpInfo, marginCollapsingState, out marginCollapsingState2, out num5);
				pmcsclientOut = ((marginCollapsingState2 != null) ? marginCollapsingState2.Handle : IntPtr.Zero);
				if (marginCollapsingState != null)
				{
					marginCollapsingState.Dispose();
					marginCollapsingState = null;
				}
				dvrTopSpace = ((mbpInfo.BPTop != 0) ? num2 : num3);
				dvrUsed += num4 - vrTrack + num5 + mbpInfo.BPBottom;
			}
			else
			{
				pfspara = IntPtr.Zero;
				dvrTopSpace = 0;
			}
			fsbbox.fsrc.u = fsbbox.fsrc.u - mbpInfo.MBPLeft;
			fsbbox.fsrc.du = fsbbox.fsrc.du + (mbpInfo.MBPLeft + mbpInfo.MBPRight);
			if (num != fswdir)
			{
				PTS.FSRECT pageRect2 = base.StructuralCache.CurrentFormatContext.PageRect;
				PTS.Validate(PTS.FsTransformBbox(num, ref pageRect2, ref fsbbox, fswdir, out fsbbox));
			}
			paraClient.SetChunkInfo(true, true);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0010BE88 File Offset: 0x0010AE88
		internal override void ClearUpdateInfo()
		{
			for (BaseParagraph baseParagraph = this._firstChild; baseParagraph != null; baseParagraph = baseParagraph.Next)
			{
				baseParagraph.ClearUpdateInfo();
			}
			base.ClearUpdateInfo();
			this._ur = null;
			this._firstParaValidInUpdateMode = false;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0010BEC4 File Offset: 0x0010AEC4
		internal override bool InvalidateStructure(int startPosition)
		{
			int paragraphStartCharacterPosition = base.ParagraphStartCharacterPosition;
			if (startPosition <= paragraphStartCharacterPosition + TextContainerHelper.ElementEdgeCharacterLength)
			{
				BaseParagraph baseParagraph = this._firstChild;
				while (baseParagraph != null)
				{
					BaseParagraph baseParagraph2 = baseParagraph;
					baseParagraph = baseParagraph.Next;
					baseParagraph2.Dispose();
					baseParagraph2.Next = null;
					baseParagraph2.Previous = null;
				}
				this._firstChild = (this._lastFetchedChild = null);
			}
			else
			{
				BaseParagraph baseParagraph = this._firstChild;
				while (baseParagraph != null)
				{
					if (baseParagraph.ParagraphStartCharacterPosition + baseParagraph.LastFormatCch >= startPosition)
					{
						if (!baseParagraph.InvalidateStructure(startPosition))
						{
							baseParagraph = baseParagraph.Next;
						}
						if (baseParagraph != null)
						{
							if (baseParagraph.Previous != null)
							{
								baseParagraph.Previous.Next = null;
								this._lastFetchedChild = baseParagraph.Previous;
							}
							else
							{
								this._firstChild = (this._lastFetchedChild = null);
							}
							while (baseParagraph != null)
							{
								BaseParagraph baseParagraph3 = baseParagraph;
								baseParagraph = baseParagraph.Next;
								baseParagraph3.Dispose();
								baseParagraph3.Next = null;
								baseParagraph3.Previous = null;
							}
							break;
						}
						break;
					}
					else
					{
						baseParagraph = baseParagraph.Next;
					}
				}
			}
			return startPosition < paragraphStartCharacterPosition + TextContainerHelper.ElementEdgeCharacterLength;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0010BFB8 File Offset: 0x0010AFB8
		internal override void InvalidateFormatCache()
		{
			for (BaseParagraph baseParagraph = this._firstChild; baseParagraph != null; baseParagraph = baseParagraph.Next)
			{
				baseParagraph.InvalidateFormatCache();
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0010BFE0 File Offset: 0x0010AFE0
		protected virtual BaseParagraph GetParagraph(ITextPointer textPointer, bool fEmptyOk)
		{
			BaseParagraph baseParagraph = null;
			switch (textPointer.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				Invariant.Assert(textPointer.CompareTo(textPointer.TextContainer.End) == 0);
				if (!fEmptyOk)
				{
					baseParagraph = new TextParagraph(base.Element, base.StructuralCache);
				}
				break;
			case TextPointerContext.Text:
				if (textPointer.TextContainer.Start.CompareTo(textPointer) > 0 && (!(base.Element is TextElement) || ((TextElement)base.Element).ContentStart != textPointer))
				{
					throw new InvalidOperationException(SR.Get("TextSchema_TextIsNotAllowedInThisContext", new object[]
					{
						base.Element.GetType().Name
					}));
				}
				baseParagraph = new TextParagraph(base.Element, base.StructuralCache);
				break;
			case TextPointerContext.EmbeddedElement:
				baseParagraph = new TextParagraph(base.Element, base.StructuralCache);
				break;
			case TextPointerContext.ElementStart:
			{
				TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointer).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				if (adjacentElementFromOuterPosition is List)
				{
					baseParagraph = new ListParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
				}
				else if (adjacentElementFromOuterPosition is Table)
				{
					baseParagraph = new TableParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
				}
				else if (adjacentElementFromOuterPosition is BlockUIContainer)
				{
					baseParagraph = new UIElementParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
				}
				else if (adjacentElementFromOuterPosition is Block || adjacentElementFromOuterPosition is ListItem)
				{
					baseParagraph = new ContainerParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
				}
				else if (adjacentElementFromOuterPosition is Inline)
				{
					baseParagraph = new TextParagraph(base.Element, base.StructuralCache);
				}
				else
				{
					Invariant.Assert(false);
				}
				break;
			}
			case TextPointerContext.ElementEnd:
				Invariant.Assert(textPointer is TextPointer);
				Invariant.Assert(base.Element == ((TextPointer)textPointer).Parent);
				if (!fEmptyOk)
				{
					baseParagraph = new TextParagraph(base.Element, base.StructuralCache);
				}
				break;
			}
			if (baseParagraph != null)
			{
				base.StructuralCache.CurrentFormatContext.DependentMax = (TextPointer)textPointer;
			}
			return baseParagraph;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0010C1C7 File Offset: 0x0010B1C7
		private bool NeedsUpdate()
		{
			return base.StructuralCache.DtrsFromRange(base.ParagraphStartCharacterPosition, base.LastFormatCch) != null;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0010C1E4 File Offset: 0x0010B1E4
		private void BuildUpdateRecord()
		{
			this._ur = null;
			DtrList dtrList = base.StructuralCache.DtrsFromRange(base.ParagraphStartCharacterPosition, base.LastFormatCch);
			UpdateRecord updateRecord2;
			if (dtrList != null)
			{
				UpdateRecord updateRecord = null;
				for (int i = 0; i < dtrList.Length; i++)
				{
					int cpfromElement = TextContainerHelper.GetCPFromElement(base.StructuralCache.TextContainer, base.Element, ElementEdge.AfterStart);
					updateRecord2 = this.UpdateRecordFromDtr(dtrList, dtrList[i], cpfromElement);
					if (updateRecord == null)
					{
						this._ur = updateRecord2;
					}
					else
					{
						updateRecord.Next = updateRecord2;
					}
					updateRecord = updateRecord2;
				}
				updateRecord2 = this._ur;
				while (updateRecord2.Next != null)
				{
					if (updateRecord2.SyncPara != null)
					{
						if (updateRecord2.SyncPara.Previous == updateRecord2.Next.FirstPara)
						{
							updateRecord2.MergeWithNext();
						}
						else if (updateRecord2.SyncPara == updateRecord2.Next.FirstPara && updateRecord2.Next.ChangeType == PTS.FSKCHANGE.fskchNew)
						{
							updateRecord2.MergeWithNext();
						}
						else
						{
							updateRecord2 = updateRecord2.Next;
						}
					}
					else
					{
						updateRecord2.MergeWithNext();
					}
				}
			}
			updateRecord2 = this._ur;
			while (updateRecord2 != null && updateRecord2.FirstPara != null)
			{
				BaseParagraph baseParagraph;
				if (updateRecord2.ChangeType == PTS.FSKCHANGE.fskchInside)
				{
					baseParagraph = updateRecord2.FirstPara.Next;
					updateRecord2.FirstPara.Next = null;
				}
				else
				{
					baseParagraph = updateRecord2.FirstPara;
				}
				while (baseParagraph != updateRecord2.SyncPara)
				{
					if (baseParagraph.Next != null)
					{
						baseParagraph.Next.Previous = null;
					}
					if (baseParagraph.Previous != null)
					{
						baseParagraph.Previous.Next = null;
					}
					baseParagraph.Dispose();
					baseParagraph = baseParagraph.Next;
				}
				updateRecord2 = updateRecord2.Next;
			}
			if (this._ur != null && this._ur.FirstPara == this._firstChild && this._ur.ChangeType == PTS.FSKCHANGE.fskchNew)
			{
				this._firstChild = null;
			}
			this._firstParaValidInUpdateMode = true;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0010C3A8 File Offset: 0x0010B3A8
		private UpdateRecord UpdateRecordFromDtr(DtrList dtrs, DirtyTextRange dtr, int dcpContent)
		{
			UpdateRecord updateRecord = new UpdateRecord();
			updateRecord.Dtr = dtr;
			BaseParagraph baseParagraph = this._firstChild;
			int num = dcpContent;
			if (num < updateRecord.Dtr.StartIndex)
			{
				while (baseParagraph != null && num + baseParagraph.LastFormatCch <= updateRecord.Dtr.StartIndex && (num + baseParagraph.LastFormatCch != updateRecord.Dtr.StartIndex || !(baseParagraph is TextParagraph)))
				{
					num += baseParagraph.LastFormatCch;
					baseParagraph = baseParagraph.Next;
				}
			}
			updateRecord.FirstPara = baseParagraph;
			if (baseParagraph == null)
			{
				updateRecord.ChangeType = PTS.FSKCHANGE.fskchNew;
			}
			else if (num < updateRecord.Dtr.StartIndex)
			{
				updateRecord.ChangeType = PTS.FSKCHANGE.fskchInside;
			}
			else
			{
				updateRecord.ChangeType = PTS.FSKCHANGE.fskchNew;
			}
			updateRecord.SyncPara = null;
			while (baseParagraph != null)
			{
				if (num + baseParagraph.LastFormatCch > updateRecord.Dtr.StartIndex + updateRecord.Dtr.PositionsRemoved || (num + baseParagraph.LastFormatCch == updateRecord.Dtr.StartIndex + updateRecord.Dtr.PositionsRemoved && updateRecord.ChangeType != PTS.FSKCHANGE.fskchNew))
				{
					updateRecord.SyncPara = baseParagraph.Next;
					break;
				}
				num += baseParagraph.LastFormatCch;
				baseParagraph = baseParagraph.Next;
			}
			return updateRecord;
		}

		// Token: 0x0400073B RID: 1851
		private BaseParagraph _firstChild;

		// Token: 0x0400073C RID: 1852
		private BaseParagraph _lastFetchedChild;

		// Token: 0x0400073D RID: 1853
		private UpdateRecord _ur;

		// Token: 0x0400073E RID: 1854
		private bool _firstParaValidInUpdateMode;
	}
}
