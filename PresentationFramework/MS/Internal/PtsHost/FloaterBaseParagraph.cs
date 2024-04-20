using System;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000120 RID: 288
	internal abstract class FloaterBaseParagraph : BaseParagraph
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x0010AEA3 File Offset: 0x00109EA3
		protected FloaterBaseParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0010EF86 File Offset: 0x0010DF86
		public override void Dispose()
		{
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0010EF94 File Offset: 0x0010DF94
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			fskch = PTS.FSKCHANGE.fskchNew;
			fNoFurtherChanges = PTS.FromBoolean(this._stopAsking);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0010EFA6 File Offset: 0x0010DFA6
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			base.GetParaProperties(ref fspap, false);
			fspap.idobj = PtsHost.FloaterParagraphId;
		}

		// Token: 0x06000774 RID: 1908
		internal abstract override void CreateParaclient(out IntPtr paraClientHandle);

		// Token: 0x06000775 RID: 1909
		internal abstract override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr);

		// Token: 0x06000776 RID: 1910
		internal abstract void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops);

		// Token: 0x06000777 RID: 1911 RVA: 0x0010E994 File Offset: 0x0010D994
		internal unsafe virtual void GetFloaterPolygons(FloaterBaseParaClient paraClient, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			ccVertices = (cfspt = (fWrapThrough = 0));
		}

		// Token: 0x06000778 RID: 1912
		internal abstract void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06000779 RID: 1913
		internal abstract void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x0600077A RID: 1914
		internal abstract void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x0600077B RID: 1915
		internal abstract void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut);

		// Token: 0x0600077C RID: 1916 RVA: 0x0010EFBB File Offset: 0x0010DFBB
		internal virtual void GetDvrUsedForFloater(uint fswdirTrack, MarginCollapsingState mcs, int dvrDisplaced, out int dvrUsed)
		{
			dvrUsed = 0;
		}
	}
}
