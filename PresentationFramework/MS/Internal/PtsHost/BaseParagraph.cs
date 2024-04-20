using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200010C RID: 268
	internal abstract class BaseParagraph : UnmanagedHandle
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00109492 File Offset: 0x00108492
		protected BaseParagraph(DependencyObject element, StructuralCache structuralCache) : base(structuralCache.PtsContext)
		{
			this._element = element;
			this._structuralCache = structuralCache;
			this._changeType = PTS.FSKCHANGE.fskchNone;
			this._stopAsking = false;
			this.UpdateLastFormatPositions();
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x001094C2 File Offset: 0x001084C2
		internal virtual void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			fskch = this._changeType;
			fNoFurtherChanges = PTS.FromBoolean(this._stopAsking);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x001094D9 File Offset: 0x001084D9
		internal virtual void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr)
		{
			dvr = ((mcs == null || suppressTopSpace) ? 0 : mcs.Margin);
		}

		// Token: 0x060006BB RID: 1723
		internal abstract void GetParaProperties(ref PTS.FSPAP fspap);

		// Token: 0x060006BC RID: 1724
		internal abstract void CreateParaclient(out IntPtr pfsparaclient);

		// Token: 0x060006BD RID: 1725 RVA: 0x001094F0 File Offset: 0x001084F0
		internal virtual void SetUpdateInfo(PTS.FSKCHANGE fskch, bool stopAsking)
		{
			this._changeType = fskch;
			this._stopAsking = stopAsking;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00109500 File Offset: 0x00108500
		internal virtual void ClearUpdateInfo()
		{
			this._changeType = PTS.FSKCHANGE.fskchNone;
			this._stopAsking = true;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00109510 File Offset: 0x00108510
		internal virtual bool InvalidateStructure(int startPosition)
		{
			return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.BeforeStart) == startPosition;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void InvalidateFormatCache()
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0010952C File Offset: 0x0010852C
		internal void UpdateLastFormatPositions()
		{
			this._lastFormatCch = this.Cch;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0010953C File Offset: 0x0010853C
		protected void GetParaProperties(ref PTS.FSPAP fspap, bool ignoreElementProps)
		{
			if (!ignoreElementProps)
			{
				fspap.fKeepWithNext = PTS.FromBoolean(DynamicPropertyReader.GetKeepWithNext(this._element));
				fspap.fBreakPageBefore = ((this._element is Block) ? PTS.FromBoolean(this.StructuralCache.CurrentFormatContext.FinitePage && ((Block)this._element).BreakPageBefore) : PTS.FromBoolean(false));
				fspap.fBreakColumnBefore = ((this._element is Block) ? PTS.FromBoolean(((Block)this._element).BreakColumnBefore) : PTS.FromBoolean(false));
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x001095DA File Offset: 0x001085DA
		internal int ParagraphStartCharacterPosition
		{
			get
			{
				if (this is TextParagraph)
				{
					return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.AfterStart);
				}
				return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.BeforeStart);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00109613 File Offset: 0x00108613
		internal int ParagraphEndCharacterPosition
		{
			get
			{
				if (this is TextParagraph)
				{
					return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.BeforeEnd);
				}
				return TextContainerHelper.GetCPFromElement(this.StructuralCache.TextContainer, this.Element, ElementEdge.AfterEnd);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0010964C File Offset: 0x0010864C
		internal int Cch
		{
			get
			{
				int num = TextContainerHelper.GetCchFromElement(this.StructuralCache.TextContainer, this.Element);
				if (this is TextParagraph && this.Element is TextElement)
				{
					Invariant.Assert(num >= 2);
					num -= 2;
				}
				return num;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00109696 File Offset: 0x00108696
		internal int LastFormatCch
		{
			get
			{
				return this._lastFormatCch;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0010969E File Offset: 0x0010869E
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x001096A6 File Offset: 0x001086A6
		internal DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x04000724 RID: 1828
		protected PTS.FSKCHANGE _changeType;

		// Token: 0x04000725 RID: 1829
		protected bool _stopAsking;

		// Token: 0x04000726 RID: 1830
		protected int _lastFormatCch;

		// Token: 0x04000727 RID: 1831
		internal BaseParagraph Next;

		// Token: 0x04000728 RID: 1832
		internal BaseParagraph Previous;

		// Token: 0x04000729 RID: 1833
		protected readonly StructuralCache _structuralCache;

		// Token: 0x0400072A RID: 1834
		protected readonly DependencyObject _element;
	}
}
