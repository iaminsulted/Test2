using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200014F RID: 335
	internal sealed class UIElementParagraph : FloaterBaseParagraph
	{
		// Token: 0x06000AD8 RID: 2776 RVA: 0x00110073 File Offset: 0x0010F073
		internal UIElementParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0012B52C File Offset: 0x0012A52C
		public override void Dispose()
		{
			this.ClearUIElementIsland();
			base.Dispose();
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0012B53A File Offset: 0x0012A53A
		internal override bool InvalidateStructure(int startPosition)
		{
			if (this._uiElementIsland != null)
			{
				this._uiElementIsland.DesiredSizeChanged -= this.OnUIElementDesiredSizeChanged;
				this._uiElementIsland.Dispose();
				this._uiElementIsland = null;
			}
			return base.InvalidateStructure(startPosition);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0012B574 File Offset: 0x0012A574
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			UIElementParaClient uielementParaClient = new UIElementParaClient(this);
			paraClientHandle = uielementParaClient.Handle;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0012B590 File Offset: 0x0012A590
		internal override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			MbpInfo mbp = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState;
			int num;
			MarginCollapsingState.CollapseTopMargin(base.PtsContext, mbp, mcs, out marginCollapsingState, out num);
			if (suppressTopSpace)
			{
				dvr = 0;
			}
			else
			{
				dvr = num;
				if (marginCollapsingState != null)
				{
					dvr += marginCollapsingState.Margin;
				}
			}
			if (marginCollapsingState != null)
			{
				marginCollapsingState.Dispose();
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0012B5F4 File Offset: 0x0012A5F4
		internal override void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops)
		{
			fsfloaterprops = default(PTS.FSFLOATERPROPS);
			fsfloaterprops.fFloat = 0;
			fsfloaterprops.fskclear = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignMin;
			fsfloaterprops.fskwr = PTS.FSKWRAP.fskwrNone;
			fsfloaterprops.fDelayNoProgress = 1;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0012B644 File Offset: 0x0012A644
		internal override void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			Invariant.Assert(paraClient is UIElementParaClient);
			Invariant.Assert(base.Element is BlockUIContainer);
			if (fAtMaxWidth == 0 && fEmptyOk == 1)
			{
				durFloaterWidth = (dvrFloaterHeight = 0);
				cPolygons = (cVertices = 0);
				fsfmtr = default(PTS.FSFMTR);
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
				fsfmtr.fContainsItemThatStoppedBeforeFootnote = 0;
				fsfmtr.fForcedProgress = 0;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pbrkrecOut = IntPtr.Zero;
				pfsFloatContent = IntPtr.Zero;
				return;
			}
			cPolygons = (cVertices = 0);
			fsfmtr.fForcedProgress = PTS.FromBoolean(fAtMaxWidth == 0);
			if (((BlockUIContainer)base.Element).Child != null)
			{
				this.EnsureUIElementIsland();
				this.FormatUIElement(durAvailable, out fsbbox);
			}
			else
			{
				this.ClearUIElementIsland();
				MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				fsbbox.fsrc = default(PTS.FSRECT);
				fsbbox.fsrc.du = durAvailable;
				fsbbox.fsrc.dv = mbpInfo.BPTop + mbpInfo.BPBottom;
			}
			durFloaterWidth = fsbbox.fsrc.du;
			dvrFloaterHeight = fsbbox.fsrc.dv;
			if (dvrAvailable < dvrFloaterHeight && fEmptyOk == 1)
			{
				durFloaterWidth = (dvrFloaterHeight = 0);
				fsfmtr = default(PTS.FSFMTR);
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pfsFloatContent = IntPtr.Zero;
			}
			else
			{
				fsbbox.fDefined = 1;
				pfsFloatContent = paraClient.Handle;
				if (dvrAvailable < dvrFloaterHeight)
				{
					Invariant.Assert(fEmptyOk == 0);
					fsfmtr.fForcedProgress = 1;
				}
				fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrGoalReached;
			}
			pbrkrecOut = IntPtr.Zero;
			fsfmtr.fContainsItemThatStoppedBeforeFootnote = 0;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0012B808 File Offset: 0x0012A808
		internal override void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			Invariant.Assert(paraClient is UIElementParaClient);
			Invariant.Assert(base.Element is BlockUIContainer);
			if (fAtMaxWidth == 0)
			{
				durFloaterWidth = durAvailable + 1;
				dvrFloaterHeight = dvrAvailable + 1;
				cPolygons = (cVertices = 0);
				fsfmtrbl = PTS.FSFMTRBL.fmtrblInterrupted;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pfsFloatContent = IntPtr.Zero;
				return;
			}
			cPolygons = (cVertices = 0);
			if (((BlockUIContainer)base.Element).Child != null)
			{
				this.EnsureUIElementIsland();
				this.FormatUIElement(durAvailable, out fsbbox);
				pfsFloatContent = paraClient.Handle;
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				fsbbox.fDefined = 1;
				durFloaterWidth = fsbbox.fsrc.du;
				dvrFloaterHeight = fsbbox.fsrc.dv;
				return;
			}
			this.ClearUIElementIsland();
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			fsbbox.fsrc = default(PTS.FSRECT);
			fsbbox.fsrc.du = durAvailable;
			fsbbox.fsrc.dv = mbpInfo.BPTop + mbpInfo.BPBottom;
			fsbbox.fDefined = 1;
			pfsFloatContent = paraClient.Handle;
			fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
			durFloaterWidth = fsbbox.fsrc.du;
			dvrFloaterHeight = fsbbox.fsrc.dv;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0012B958 File Offset: 0x0012A958
		internal override void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			this.FormatFloaterContentBottomless(paraClient, fSuppressTopSpace, fswdir, fAtMaxWidth, durAvailable, dvrAvailable, out fsfmtrbl, out pfsFloatContent, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0012B984 File Offset: 0x0012A984
		internal override void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut)
		{
			MbpInfo mbp = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			MarginCollapsingState marginCollapsingState;
			int num;
			MarginCollapsingState.CollapseBottomMargin(base.PtsContext, mbp, null, out marginCollapsingState, out num);
			if (marginCollapsingState != null)
			{
				pmcsclientOut = marginCollapsingState.Handle;
				return;
			}
			pmcsclientOut = IntPtr.Zero;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0012B9D4 File Offset: 0x0012A9D4
		private void FormatUIElement(int durAvailable, out PTS.FSBBOX fsbbox)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			double width = TextDpi.FromTextDpi(Math.Max(1, durAvailable - (mbpInfo.MBPLeft + mbpInfo.MBPRight)));
			double num;
			if (this.SizeToFigureParent)
			{
				if (base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					num = base.StructuralCache.CurrentFormatContext.PageHeight;
				}
				else
				{
					Figure figure = (Figure)((BlockUIContainer)base.Element).Parent;
					Invariant.Assert(figure.Height.IsAbsolute);
					num = figure.Height.Value;
				}
				num = Math.Max(TextDpi.FromTextDpi(1), num - TextDpi.FromTextDpi(mbpInfo.MBPTop + mbpInfo.MBPBottom));
				this.UIElementIsland.DoLayout(new Size(width, num), false, false);
				fsbbox.fsrc = default(PTS.FSRECT);
				fsbbox.fsrc.du = durAvailable;
				fsbbox.fsrc.dv = TextDpi.ToTextDpi(num) + mbpInfo.BPTop + mbpInfo.BPBottom;
				fsbbox.fDefined = 1;
				return;
			}
			if (base.StructuralCache.CurrentFormatContext.FinitePage)
			{
				Thickness documentPageMargin = base.StructuralCache.CurrentFormatContext.DocumentPageMargin;
				num = base.StructuralCache.CurrentFormatContext.DocumentPageSize.Height - documentPageMargin.Top - documentPageMargin.Bottom - TextDpi.FromTextDpi(mbpInfo.MBPTop + mbpInfo.MBPBottom);
				num = Math.Max(TextDpi.FromTextDpi(1), num);
			}
			else
			{
				num = double.PositiveInfinity;
			}
			Size size = this.UIElementIsland.DoLayout(new Size(width, num), false, true);
			fsbbox.fsrc = default(PTS.FSRECT);
			fsbbox.fsrc.du = durAvailable;
			fsbbox.fsrc.dv = TextDpi.ToTextDpi(size.Height) + mbpInfo.BPTop + mbpInfo.BPBottom;
			fsbbox.fDefined = 1;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0012BBC6 File Offset: 0x0012ABC6
		private void EnsureUIElementIsland()
		{
			if (this._uiElementIsland == null)
			{
				this._uiElementIsland = new UIElementIsland(((BlockUIContainer)base.Element).Child);
				this._uiElementIsland.DesiredSizeChanged += this.OnUIElementDesiredSizeChanged;
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0012BC04 File Offset: 0x0012AC04
		private void ClearUIElementIsland()
		{
			try
			{
				if (this._uiElementIsland != null)
				{
					this._uiElementIsland.DesiredSizeChanged -= this.OnUIElementDesiredSizeChanged;
					this._uiElementIsland.Dispose();
				}
			}
			finally
			{
				this._uiElementIsland = null;
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0012ABB1 File Offset: 0x00129BB1
		private void OnUIElementDesiredSizeChanged(object sender, DesiredSizeChangedEventArgs e)
		{
			base.StructuralCache.FormattingOwner.OnChildDesiredSizeChanged(e.Child);
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0012BC58 File Offset: 0x0012AC58
		internal UIElementIsland UIElementIsland
		{
			get
			{
				return this._uiElementIsland;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0012BC60 File Offset: 0x0012AC60
		private bool SizeToFigureParent
		{
			get
			{
				if (!this.IsOnlyChildOfFigure)
				{
					return false;
				}
				Figure figure = (Figure)((BlockUIContainer)base.Element).Parent;
				return !figure.Height.IsAuto && (base.StructuralCache.CurrentFormatContext.FinitePage || figure.Height.IsAbsolute);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x0012BCC4 File Offset: 0x0012ACC4
		private bool IsOnlyChildOfFigure
		{
			get
			{
				DependencyObject parent = ((BlockUIContainer)base.Element).Parent;
				if (parent is Figure)
				{
					Figure figure = parent as Figure;
					if (figure.Blocks.FirstChild == figure.Blocks.LastChild && figure.Blocks.FirstChild == base.Element)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x04000828 RID: 2088
		private UIElementIsland _uiElementIsland;
	}
}
