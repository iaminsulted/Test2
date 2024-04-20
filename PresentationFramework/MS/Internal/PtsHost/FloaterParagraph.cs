using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000122 RID: 290
	internal sealed class FloaterParagraph : FloaterBaseParagraph
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x00110073 File Offset: 0x0010F073
		internal FloaterParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0011007D File Offset: 0x0010F07D
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			base.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			fskch = PTS.FSKCHANGE.fskchNew;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0011008A File Offset: 0x0010F08A
		public override void Dispose()
		{
			base.Dispose();
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x001100AC File Offset: 0x0010F0AC
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			FloaterParaClient floaterParaClient = new FloaterParaClient(this);
			paraClientHandle = floaterParaClient.Handle;
			if (this._mainTextSegment == null)
			{
				this._mainTextSegment = new ContainerParagraph(base.Element, base.StructuralCache);
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x001100E7 File Offset: 0x0010F0E7
		internal override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			dvr = 0;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x001100F0 File Offset: 0x0010F0F0
		internal override void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops)
		{
			fsfloaterprops = default(PTS.FSFLOATERPROPS);
			fsfloaterprops.fFloat = 1;
			fsfloaterprops.fskclear = PTS.WrapDirectionToFskclear((WrapDirection)base.Element.GetValue(Block.ClearFloatersProperty));
			switch (this.HorizontalAlignment)
			{
			case HorizontalAlignment.Center:
				fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignCenter;
				goto IL_62;
			case HorizontalAlignment.Right:
				fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignMax;
				goto IL_62;
			}
			fsfloaterprops.fskfloatalignment = PTS.FSKFLOATALIGNMENT.fskfloatalignMin;
			IL_62:
			fsfloaterprops.fskwr = PTS.WrapDirectionToFskwrap(this.WrapDirection);
			fsfloaterprops.fDelayNoProgress = 1;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00110178 File Offset: 0x0010F178
		internal override void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Invariant.Assert(paraClient is FloaterParaClient);
			if (this.IsFloaterRejected(PTS.ToBoolean(fAtMaxWidth), TextDpi.FromTextDpi(durAvailable)))
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
			}
			else
			{
				if (!base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					if (double.IsInfinity(base.StructuralCache.CurrentFormatContext.PageHeight))
					{
						if (dvrAvailable > 1073741823)
						{
							dvrAvailable = Math.Min(dvrAvailable, 1073741823);
							fEmptyOk = 0;
						}
					}
					else
					{
						dvrAvailable = Math.Min(dvrAvailable, TextDpi.ToTextDpi(base.StructuralCache.CurrentFormatContext.PageHeight));
					}
				}
				MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				double num = this.CalculateWidth(TextDpi.FromTextDpi(durAvailable));
				int num2;
				this.AdjustDurAvailable(num, ref durAvailable, out num2);
				int num3 = Math.Max(1, dvrAvailable - (mbpInfo.MBPTop + mbpInfo.MBPBottom));
				PTS.FSRECT fsrect = default(PTS.FSRECT);
				fsrect.du = num2;
				fsrect.dv = num3;
				int num4 = 1;
				PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num4];
				array[0].durBefore = 0;
				array[0].durWidth = num2;
				IntPtr zero;
				int num5;
				this.CreateSubpageFiniteHelper(base.PtsContext, pbrkrecIn, fBRFromPreviousPage, this._mainTextSegment.Handle, footnoteRejector, fEmptyOk, 1, fswdir, num2, num3, ref fsrect, num4, array, 0, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfsFloatContent, out pbrkrecOut, out dvrFloaterHeight, out fsbbox, out zero, out num5);
				if (fsfmtr.kstop >= PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace)
				{
					durFloaterWidth = (dvrFloaterHeight = 0);
					cPolygons = (cVertices = 0);
				}
				else
				{
					if (PTS.ToBoolean(fsbbox.fDefined))
					{
						if (fsbbox.fsrc.du < num2 && double.IsNaN(num) && this.HorizontalAlignment != HorizontalAlignment.Stretch)
						{
							if (pfsFloatContent != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
								pfsFloatContent = IntPtr.Zero;
							}
							if (pbrkrecOut != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpageBreakRecord(base.PtsContext.Context, pbrkrecOut), base.PtsContext);
								pbrkrecOut = IntPtr.Zero;
							}
							if (zero != IntPtr.Zero)
							{
								MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
								PTS.ValidateHandle(marginCollapsingState);
								marginCollapsingState.Dispose();
								zero = IntPtr.Zero;
							}
							num2 = fsbbox.fsrc.du + 1;
							fsrect.du = num2;
							fsrect.dv = num3;
							array[0].durWidth = num2;
							this.CreateSubpageFiniteHelper(base.PtsContext, pbrkrecIn, fBRFromPreviousPage, this._mainTextSegment.Handle, footnoteRejector, fEmptyOk, 1, fswdir, num2, num3, ref fsrect, num4, array, 0, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfsFloatContent, out pbrkrecOut, out dvrFloaterHeight, out fsbbox, out zero, out num5);
						}
					}
					else
					{
						num2 = TextDpi.ToTextDpi(TextDpi.MinWidth);
					}
					if (zero != IntPtr.Zero)
					{
						MarginCollapsingState marginCollapsingState2 = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
						PTS.ValidateHandle(marginCollapsingState2);
						marginCollapsingState2.Dispose();
						zero = IntPtr.Zero;
					}
					durFloaterWidth = num2 + mbpInfo.MBPLeft + mbpInfo.MBPRight;
					dvrFloaterHeight += mbpInfo.MBPTop + mbpInfo.MBPBottom;
					fsbbox.fsrc.u = 0;
					fsbbox.fsrc.v = 0;
					fsbbox.fsrc.du = durFloaterWidth;
					fsbbox.fsrc.dv = dvrFloaterHeight;
					fsbbox.fDefined = 1;
					cPolygons = (cVertices = 0);
					if (durFloaterWidth > durAvailable || dvrFloaterHeight > dvrAvailable)
					{
						if (PTS.ToBoolean(fEmptyOk))
						{
							if (pfsFloatContent != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
								pfsFloatContent = IntPtr.Zero;
							}
							if (pbrkrecOut != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpageBreakRecord(base.PtsContext.Context, pbrkrecOut), base.PtsContext);
								pbrkrecOut = IntPtr.Zero;
							}
							cPolygons = (cVertices = 0);
							fsfmtr.kstop = PTS.FSFMTRKSTOP.fmtrNoProgressOutOfSpace;
						}
						else
						{
							fsfmtr.fForcedProgress = 1;
						}
					}
				}
			}
			((FloaterParaClient)paraClient).SubpageHandle = pfsFloatContent;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00110618 File Offset: 0x0010F618
		internal override void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Invariant.Assert(paraClient is FloaterParaClient);
			if (this.IsFloaterRejected(PTS.ToBoolean(fAtMaxWidth), TextDpi.FromTextDpi(durAvailable)))
			{
				durFloaterWidth = durAvailable + 1;
				dvrFloaterHeight = dvrAvailable + 1;
				cPolygons = (cVertices = 0);
				fsfmtrbl = PTS.FSFMTRBL.fmtrblInterrupted;
				fsbbox = default(PTS.FSBBOX);
				fsbbox.fDefined = 0;
				pfsFloatContent = IntPtr.Zero;
			}
			else
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				double num = this.CalculateWidth(TextDpi.FromTextDpi(durAvailable));
				int num2;
				this.AdjustDurAvailable(num, ref durAvailable, out num2);
				int durMargin = num2;
				int urMargin;
				int vrMargin = urMargin = 0;
				int num3 = 1;
				PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num3];
				array[0].durBefore = 0;
				array[0].durWidth = num2;
				this.InvalidateMainTextSegment();
				IntPtr zero;
				int num4;
				int num5;
				this.CreateSubpageBottomlessHelper(base.PtsContext, this._mainTextSegment.Handle, 1, fswdir, num2, urMargin, durMargin, vrMargin, num3, array, out fsfmtrbl, out pfsFloatContent, out dvrFloaterHeight, out fsbbox, out zero, out num4, out num5);
				if (fsfmtrbl != PTS.FSFMTRBL.fmtrblCollision)
				{
					if (PTS.ToBoolean(fsbbox.fDefined))
					{
						if (fsbbox.fsrc.du < num2 && double.IsNaN(num) && this.HorizontalAlignment != HorizontalAlignment.Stretch)
						{
							if (pfsFloatContent != IntPtr.Zero)
							{
								PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
							}
							if (zero != IntPtr.Zero)
							{
								MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
								PTS.ValidateHandle(marginCollapsingState);
								marginCollapsingState.Dispose();
								zero = IntPtr.Zero;
							}
							durMargin = (num2 = fsbbox.fsrc.du + 1);
							array[0].durWidth = num2;
							this.CreateSubpageBottomlessHelper(base.PtsContext, this._mainTextSegment.Handle, 1, fswdir, num2, urMargin, durMargin, vrMargin, num3, array, out fsfmtrbl, out pfsFloatContent, out dvrFloaterHeight, out fsbbox, out zero, out num4, out num5);
						}
					}
					else
					{
						num2 = TextDpi.ToTextDpi(TextDpi.MinWidth);
					}
					if (zero != IntPtr.Zero)
					{
						MarginCollapsingState marginCollapsingState2 = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
						PTS.ValidateHandle(marginCollapsingState2);
						marginCollapsingState2.Dispose();
						zero = IntPtr.Zero;
					}
					durFloaterWidth = num2 + mbpInfo.MBPLeft + mbpInfo.MBPRight;
					dvrFloaterHeight += mbpInfo.MBPTop + mbpInfo.MBPBottom;
					if (dvrFloaterHeight > dvrAvailable || (durFloaterWidth > durAvailable && !PTS.ToBoolean(fAtMaxWidth)))
					{
						if (pfsFloatContent != IntPtr.Zero)
						{
							PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, pfsFloatContent), base.PtsContext);
						}
						cPolygons = (cVertices = 0);
						pfsFloatContent = IntPtr.Zero;
					}
					else
					{
						fsbbox.fsrc.u = 0;
						fsbbox.fsrc.v = 0;
						fsbbox.fsrc.du = durFloaterWidth;
						fsbbox.fsrc.dv = dvrFloaterHeight;
						cPolygons = (cVertices = 0);
					}
				}
				else
				{
					durFloaterWidth = (dvrFloaterHeight = 0);
					cPolygons = (cVertices = 0);
					pfsFloatContent = IntPtr.Zero;
				}
			}
			((FloaterParaClient)paraClient).SubpageHandle = pfsFloatContent;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00110964 File Offset: 0x0010F964
		internal override void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
			durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
			fsbbox = default(PTS.FSBBOX);
			Invariant.Assert(false, "No appropriate handling for update in attached object floater.");
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0011099E File Offset: 0x0010F99E
		internal override void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut)
		{
			if (mcs != null)
			{
				pmcsclientOut = mcs.Handle;
				return;
			}
			pmcsclientOut = IntPtr.Zero;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x001109B3 File Offset: 0x0010F9B3
		internal override void ClearUpdateInfo()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.ClearUpdateInfo();
			}
			base.ClearUpdateInfo();
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x001109CE File Offset: 0x0010F9CE
		internal override bool InvalidateStructure(int startPosition)
		{
			if (this._mainTextSegment != null && this._mainTextSegment.InvalidateStructure(startPosition))
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
			return this._mainTextSegment == null;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00110A01 File Offset: 0x0010FA01
		internal override void InvalidateFormatCache()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.InvalidateFormatCache();
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00110A16 File Offset: 0x0010FA16
		internal void UpdateSegmentLastFormatPositions()
		{
			this._mainTextSegment.UpdateLastFormatPositions();
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00110A24 File Offset: 0x0010FA24
		private void AdjustDurAvailable(double specifiedWidth, ref int durAvailable, out int subpageWidth)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (double.IsNaN(specifiedWidth))
			{
				subpageWidth = Math.Max(1, durAvailable - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
				return;
			}
			TextDpi.EnsureValidPageWidth(ref specifiedWidth);
			int num = TextDpi.ToTextDpi(specifiedWidth);
			if (num + mbpInfo.MarginRight + mbpInfo.MarginLeft <= durAvailable)
			{
				durAvailable = num + mbpInfo.MarginLeft + mbpInfo.MarginRight;
				subpageWidth = Math.Max(1, num - (mbpInfo.BPLeft + mbpInfo.BPRight));
				return;
			}
			subpageWidth = Math.Max(1, durAvailable - (mbpInfo.MBPLeft + mbpInfo.MBPRight));
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00110AD0 File Offset: 0x0010FAD0
		private unsafe void CreateSubpageFiniteHelper(PtsContext ptsContext, IntPtr brParaIn, int fFromPreviousPage, IntPtr nSeg, IntPtr pFtnRej, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int lWidth, int lHeight, ref PTS.FSRECT rcMargin, int cColumns, PTS.FSCOLUMNINFO[] columnInfoCollection, int fApplyColumnBalancing, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pSubPage, out IntPtr brParaOut, out int dvrUsed, out PTS.FSBBOX fsBBox, out IntPtr pfsMcsClient, out int topSpace)
		{
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(lWidth), TextDpi.FromTextDpi(lHeight)), default(Thickness), false, true);
			fixed (PTS.FSCOLUMNINFO[] array = columnInfoCollection)
			{
				PTS.FSCOLUMNINFO* rgColumnInfo;
				if (columnInfoCollection == null || array.Length == 0)
				{
					rgColumnInfo = null;
				}
				else
				{
					rgColumnInfo = &array[0];
				}
				PTS.Validate(PTS.FsCreateSubpageFinite(ptsContext.Context, brParaIn, fFromPreviousPage, nSeg, pFtnRej, fEmptyOk, fSuppressTopSpace, fswdir, lWidth, lHeight, ref rcMargin, cColumns, rgColumnInfo, 0, 0, null, null, 0, null, null, 0, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pSubPage, out brParaOut, out dvrUsed, out fsBBox, out pfsMcsClient, out topSpace), ptsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00110B78 File Offset: 0x0010FB78
		private unsafe void CreateSubpageBottomlessHelper(PtsContext ptsContext, IntPtr nSeg, int fSuppressTopSpace, uint fswdir, int lWidth, int urMargin, int durMargin, int vrMargin, int cColumns, PTS.FSCOLUMNINFO[] columnInfoCollection, out PTS.FSFMTRBL pfsfmtr, out IntPtr ppSubPage, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr pfsMcsClient, out int pTopSpace, out int fPageBecomesUninterruptible)
		{
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(lWidth), TextDpi.MaxWidth), default(Thickness), false, false);
			fixed (PTS.FSCOLUMNINFO[] array = columnInfoCollection)
			{
				PTS.FSCOLUMNINFO* rgColumnInfo;
				if (columnInfoCollection == null || array.Length == 0)
				{
					rgColumnInfo = null;
				}
				else
				{
					rgColumnInfo = &array[0];
				}
				PTS.Validate(PTS.FsCreateSubpageBottomless(ptsContext.Context, nSeg, fSuppressTopSpace, fswdir, lWidth, urMargin, durMargin, vrMargin, cColumns, rgColumnInfo, 0, null, null, 0, null, null, 0, out pfsfmtr, out ppSubPage, out pdvrUsed, out pfsBBox, out pfsMcsClient, out pTopSpace, out fPageBecomesUninterruptible), ptsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00110C18 File Offset: 0x0010FC18
		private void InvalidateMainTextSegment()
		{
			DtrList dtrList = base.StructuralCache.DtrsFromRange(base.ParagraphStartCharacterPosition, base.LastFormatCch);
			if (dtrList != null && dtrList.Length > 0)
			{
				this._mainTextSegment.InvalidateStructure(dtrList[0].StartIndex);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00110C64 File Offset: 0x0010FC64
		private HorizontalAlignment HorizontalAlignment
		{
			get
			{
				if (base.Element is Floater)
				{
					return ((Floater)base.Element).HorizontalAlignment;
				}
				FigureHorizontalAnchor horizontalAnchor = ((Figure)base.Element).HorizontalAnchor;
				if (horizontalAnchor == FigureHorizontalAnchor.PageLeft || horizontalAnchor == FigureHorizontalAnchor.ContentLeft || horizontalAnchor == FigureHorizontalAnchor.ColumnLeft)
				{
					return HorizontalAlignment.Left;
				}
				if (horizontalAnchor == FigureHorizontalAnchor.PageRight || horizontalAnchor == FigureHorizontalAnchor.ContentRight || horizontalAnchor == FigureHorizontalAnchor.ColumnRight)
				{
					return HorizontalAlignment.Right;
				}
				if (horizontalAnchor == FigureHorizontalAnchor.PageCenter || horizontalAnchor != FigureHorizontalAnchor.ContentCenter)
				{
				}
				return HorizontalAlignment.Center;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00110CC8 File Offset: 0x0010FCC8
		private WrapDirection WrapDirection
		{
			get
			{
				if (!(base.Element is Floater))
				{
					return ((Figure)base.Element).WrapDirection;
				}
				if (this.HorizontalAlignment == HorizontalAlignment.Stretch)
				{
					return WrapDirection.None;
				}
				return WrapDirection.Both;
			}
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00110CF4 File Offset: 0x0010FCF4
		private double CalculateWidth(double spaceAvailable)
		{
			if (base.Element is Floater)
			{
				return ((Floater)base.Element).Width;
			}
			bool flag;
			double val = FigureHelper.CalculateFigureWidth(base.StructuralCache, (Figure)base.Element, ((Figure)base.Element).Width, out flag);
			if (flag)
			{
				return double.NaN;
			}
			return Math.Min(val, spaceAvailable);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00110D60 File Offset: 0x0010FD60
		private bool IsFloaterRejected(bool fAtMaxWidth, double availableSpace)
		{
			if (fAtMaxWidth)
			{
				return false;
			}
			if (base.Element is Floater && this.HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				return false;
			}
			if (base.Element is Figure)
			{
				FigureLength width = ((Figure)base.Element).Width;
				if (width.IsAuto)
				{
					return false;
				}
				if (width.IsAbsolute && width.Value < availableSpace)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000786 RID: 1926
		private BaseParagraph _mainTextSegment;
	}
}
