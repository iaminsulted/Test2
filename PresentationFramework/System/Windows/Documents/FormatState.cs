using System;

namespace System.Windows.Documents
{
	// Token: 0x02000668 RID: 1640
	internal class FormatState
	{
		// Token: 0x06005064 RID: 20580 RVA: 0x0024D411 File Offset: 0x0024C411
		internal FormatState()
		{
			this._dest = RtfDestination.DestNormal;
			this._stateSkip = 1;
			this.SetCharDefaults();
			this.SetParaDefaults();
			this.SetRowDefaults();
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x0024D43C File Offset: 0x0024C43C
		internal FormatState(FormatState formatState)
		{
			this.Bold = formatState.Bold;
			this.Italic = formatState.Italic;
			this.Engrave = formatState.Engrave;
			this.Shadow = formatState.Shadow;
			this.SCaps = formatState.SCaps;
			this.Outline = formatState.Outline;
			this.Super = formatState.Super;
			this.Sub = formatState.Sub;
			this.SuperOffset = formatState.SuperOffset;
			this.FontSize = formatState.FontSize;
			this.Font = formatState.Font;
			this.CodePage = formatState.CodePage;
			this.CF = formatState.CF;
			this.CB = formatState.CB;
			this.DirChar = formatState.DirChar;
			this.UL = formatState.UL;
			this.Strike = formatState.Strike;
			this.Expand = formatState.Expand;
			this.Lang = formatState.Lang;
			this.LangFE = formatState.LangFE;
			this.LangCur = formatState.LangCur;
			this.FontSlot = formatState.FontSlot;
			this.SB = formatState.SB;
			this.SA = formatState.SA;
			this.FI = formatState.FI;
			this.RI = formatState.RI;
			this.LI = formatState.LI;
			this.SL = formatState.SL;
			this.SLMult = formatState.SLMult;
			this.HAlign = formatState.HAlign;
			this.ILVL = formatState.ILVL;
			this.ITAP = formatState.ITAP;
			this.ILS = formatState.ILS;
			this.DirPara = formatState.DirPara;
			this.CFPara = formatState.CFPara;
			this.CBPara = formatState.CBPara;
			this.ParaShading = formatState.ParaShading;
			this.Marker = formatState.Marker;
			this.IsContinue = formatState.IsContinue;
			this.StartIndex = formatState.StartIndex;
			this.StartIndexDefault = formatState.StartIndexDefault;
			this.IsInTable = formatState.IsInTable;
			this._pb = (formatState.HasParaBorder ? new ParaBorder(formatState.ParaBorder) : null);
			this.RowFormat = formatState._rowFormat;
			this.RtfDestination = formatState.RtfDestination;
			this.IsHidden = formatState.IsHidden;
			this._stateSkip = formatState.UnicodeSkip;
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x0024D694 File Offset: 0x0024C694
		internal void SetCharDefaults()
		{
			this._fBold = false;
			this._fItalic = false;
			this._fEngrave = false;
			this._fShadow = false;
			this._fScaps = false;
			this._fOutline = false;
			this._fSub = false;
			this._fSuper = false;
			this._superOffset = 0L;
			this._fs = 24L;
			this._font = -1L;
			this._codePage = -1;
			this._cf = -1L;
			this._cb = -1L;
			this._dirChar = DirState.DirLTR;
			this._ul = ULState.ULNone;
			this._strike = StrikeState.StrikeNone;
			this._expand = 0L;
			this._fHidden = false;
			this._lang = -1L;
			this._langFE = -1L;
			this._langCur = -1L;
			this._fontSlot = FontSlot.LOCH;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x0024D74C File Offset: 0x0024C74C
		internal void SetParaDefaults()
		{
			this._sb = 0L;
			this._sa = 0L;
			this._fi = 0L;
			this._ri = 0L;
			this._li = 0L;
			this._align = HAlign.AlignDefault;
			this._ilvl = 0L;
			this._pnlvl = 0L;
			this._itap = 0L;
			this._ils = -1L;
			this._dirPara = DirState.DirLTR;
			this._cbPara = -1L;
			this._nParaShading = -1L;
			this._cfPara = -1L;
			this._marker = MarkerStyle.MarkerNone;
			this._fContinue = false;
			this._nStartIndex = -1L;
			this._nStartIndexDefault = -1L;
			this._sl = 0L;
			this._slMult = false;
			this._pb = null;
			this._fInTable = false;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x0024D802 File Offset: 0x0024C802
		internal void SetRowDefaults()
		{
			this.RowFormat = null;
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x0024D80C File Offset: 0x0024C80C
		internal bool IsEqual(FormatState formatState)
		{
			return this.Bold == formatState.Bold && this.Italic == formatState.Italic && this.Engrave == formatState.Engrave && this.Shadow == formatState.Shadow && this.SCaps == formatState.SCaps && this.Outline == formatState.Outline && this.Super == formatState.Super && this.Sub == formatState.Sub && this.SuperOffset == formatState.SuperOffset && this.FontSize == formatState.FontSize && this.Font == formatState.Font && this.CodePage == formatState.CodePage && this.CF == formatState.CF && this.CB == formatState.CB && this.DirChar == formatState.DirChar && this.UL == formatState.UL && this.Strike == formatState.Strike && this.Expand == formatState.Expand && this.Lang == formatState.Lang && this.LangFE == formatState.LangFE && this.LangCur == formatState.LangCur && this.FontSlot == formatState.FontSlot && this.SB == formatState.SB && this.SA == formatState.SA && this.FI == formatState.FI && this.RI == formatState.RI && this.LI == formatState.LI && this.HAlign == formatState.HAlign && this.ILVL == formatState.ILVL && this.ITAP == formatState.ITAP && this.ILS == formatState.ILS && this.DirPara == formatState.DirPara && this.CFPara == formatState.CFPara && this.CBPara == formatState.CBPara && this.ParaShading == formatState.ParaShading && this.Marker == formatState.Marker && this.IsContinue == formatState.IsContinue && this.StartIndex == formatState.StartIndex && this.StartIndexDefault == formatState.StartIndexDefault && this.SL == formatState.SL && this.SLMult == formatState.SLMult && this.IsInTable == formatState.IsInTable && this.RtfDestination == formatState.RtfDestination && this.IsHidden == formatState.IsHidden && this.UnicodeSkip == formatState.UnicodeSkip;
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x0600506A RID: 20586 RVA: 0x0024DAFA File Offset: 0x0024CAFA
		internal static FormatState EmptyFormatState
		{
			get
			{
				if (FormatState._fsEmptyState == null)
				{
					FormatState._fsEmptyState = new FormatState();
					FormatState._fsEmptyState.FontSize = -1L;
				}
				return FormatState._fsEmptyState;
			}
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x0024DB1E File Offset: 0x0024CB1E
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			if (this.HasParaBorder)
			{
				return this.ParaBorder.GetBorderAttributeString(converterState);
			}
			return string.Empty;
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x0600506C RID: 20588 RVA: 0x0024DB3A File Offset: 0x0024CB3A
		// (set) Token: 0x0600506D RID: 20589 RVA: 0x0024DB42 File Offset: 0x0024CB42
		internal RtfDestination RtfDestination
		{
			get
			{
				return this._dest;
			}
			set
			{
				this._dest = value;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x0600506E RID: 20590 RVA: 0x0024DB4B File Offset: 0x0024CB4B
		// (set) Token: 0x0600506F RID: 20591 RVA: 0x0024DB53 File Offset: 0x0024CB53
		internal bool IsHidden
		{
			get
			{
				return this._fHidden;
			}
			set
			{
				this._fHidden = value;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06005070 RID: 20592 RVA: 0x0024DB5C File Offset: 0x0024CB5C
		internal bool IsContentDestination
		{
			get
			{
				return this._dest == RtfDestination.DestNormal || this._dest == RtfDestination.DestFieldResult || this._dest == RtfDestination.DestShapeResult || this._dest == RtfDestination.DestShape || this._dest == RtfDestination.DestListText;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06005071 RID: 20593 RVA: 0x0024DB90 File Offset: 0x0024CB90
		// (set) Token: 0x06005072 RID: 20594 RVA: 0x0024DB98 File Offset: 0x0024CB98
		internal bool Bold
		{
			get
			{
				return this._fBold;
			}
			set
			{
				this._fBold = value;
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06005073 RID: 20595 RVA: 0x0024DBA1 File Offset: 0x0024CBA1
		// (set) Token: 0x06005074 RID: 20596 RVA: 0x0024DBA9 File Offset: 0x0024CBA9
		internal bool Italic
		{
			get
			{
				return this._fItalic;
			}
			set
			{
				this._fItalic = value;
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06005075 RID: 20597 RVA: 0x0024DBB2 File Offset: 0x0024CBB2
		// (set) Token: 0x06005076 RID: 20598 RVA: 0x0024DBBA File Offset: 0x0024CBBA
		internal bool Engrave
		{
			get
			{
				return this._fEngrave;
			}
			set
			{
				this._fEngrave = value;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06005077 RID: 20599 RVA: 0x0024DBC3 File Offset: 0x0024CBC3
		// (set) Token: 0x06005078 RID: 20600 RVA: 0x0024DBCB File Offset: 0x0024CBCB
		internal bool Shadow
		{
			get
			{
				return this._fShadow;
			}
			set
			{
				this._fShadow = value;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06005079 RID: 20601 RVA: 0x0024DBD4 File Offset: 0x0024CBD4
		// (set) Token: 0x0600507A RID: 20602 RVA: 0x0024DBDC File Offset: 0x0024CBDC
		internal bool SCaps
		{
			get
			{
				return this._fScaps;
			}
			set
			{
				this._fScaps = value;
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x0024DBE5 File Offset: 0x0024CBE5
		// (set) Token: 0x0600507C RID: 20604 RVA: 0x0024DBED File Offset: 0x0024CBED
		internal bool Outline
		{
			get
			{
				return this._fOutline;
			}
			set
			{
				this._fOutline = value;
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x0600507D RID: 20605 RVA: 0x0024DBF6 File Offset: 0x0024CBF6
		// (set) Token: 0x0600507E RID: 20606 RVA: 0x0024DBFE File Offset: 0x0024CBFE
		internal bool Sub
		{
			get
			{
				return this._fSub;
			}
			set
			{
				this._fSub = value;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x0600507F RID: 20607 RVA: 0x0024DC07 File Offset: 0x0024CC07
		// (set) Token: 0x06005080 RID: 20608 RVA: 0x0024DC0F File Offset: 0x0024CC0F
		internal bool Super
		{
			get
			{
				return this._fSuper;
			}
			set
			{
				this._fSuper = value;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06005081 RID: 20609 RVA: 0x0024DC18 File Offset: 0x0024CC18
		// (set) Token: 0x06005082 RID: 20610 RVA: 0x0024DC20 File Offset: 0x0024CC20
		internal long SuperOffset
		{
			get
			{
				return this._superOffset;
			}
			set
			{
				this._superOffset = value;
			}
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06005083 RID: 20611 RVA: 0x0024DC29 File Offset: 0x0024CC29
		// (set) Token: 0x06005084 RID: 20612 RVA: 0x0024DC31 File Offset: 0x0024CC31
		internal long FontSize
		{
			get
			{
				return this._fs;
			}
			set
			{
				this._fs = value;
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06005085 RID: 20613 RVA: 0x0024DC3A File Offset: 0x0024CC3A
		// (set) Token: 0x06005086 RID: 20614 RVA: 0x0024DC42 File Offset: 0x0024CC42
		internal long Font
		{
			get
			{
				return this._font;
			}
			set
			{
				this._font = value;
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06005087 RID: 20615 RVA: 0x0024DC4B File Offset: 0x0024CC4B
		// (set) Token: 0x06005088 RID: 20616 RVA: 0x0024DC53 File Offset: 0x0024CC53
		internal int CodePage
		{
			get
			{
				return this._codePage;
			}
			set
			{
				this._codePage = value;
			}
		}

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x0024DC5C File Offset: 0x0024CC5C
		// (set) Token: 0x0600508A RID: 20618 RVA: 0x0024DC64 File Offset: 0x0024CC64
		internal long CF
		{
			get
			{
				return this._cf;
			}
			set
			{
				this._cf = value;
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x0600508B RID: 20619 RVA: 0x0024DC6D File Offset: 0x0024CC6D
		// (set) Token: 0x0600508C RID: 20620 RVA: 0x0024DC75 File Offset: 0x0024CC75
		internal long CB
		{
			get
			{
				return this._cb;
			}
			set
			{
				this._cb = value;
			}
		}

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x0600508D RID: 20621 RVA: 0x0024DC7E File Offset: 0x0024CC7E
		// (set) Token: 0x0600508E RID: 20622 RVA: 0x0024DC86 File Offset: 0x0024CC86
		internal DirState DirChar
		{
			get
			{
				return this._dirChar;
			}
			set
			{
				this._dirChar = value;
			}
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x0024DC8F File Offset: 0x0024CC8F
		// (set) Token: 0x06005090 RID: 20624 RVA: 0x0024DC97 File Offset: 0x0024CC97
		internal ULState UL
		{
			get
			{
				return this._ul;
			}
			set
			{
				this._ul = value;
			}
		}

		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06005091 RID: 20625 RVA: 0x0024DCA0 File Offset: 0x0024CCA0
		// (set) Token: 0x06005092 RID: 20626 RVA: 0x0024DCA8 File Offset: 0x0024CCA8
		internal StrikeState Strike
		{
			get
			{
				return this._strike;
			}
			set
			{
				this._strike = value;
			}
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06005093 RID: 20627 RVA: 0x0024DCB1 File Offset: 0x0024CCB1
		// (set) Token: 0x06005094 RID: 20628 RVA: 0x0024DCB9 File Offset: 0x0024CCB9
		internal long Expand
		{
			get
			{
				return this._expand;
			}
			set
			{
				this._expand = value;
			}
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06005095 RID: 20629 RVA: 0x0024DCC2 File Offset: 0x0024CCC2
		// (set) Token: 0x06005096 RID: 20630 RVA: 0x0024DCCA File Offset: 0x0024CCCA
		internal long Lang
		{
			get
			{
				return this._lang;
			}
			set
			{
				this._lang = value;
			}
		}

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06005097 RID: 20631 RVA: 0x0024DCD3 File Offset: 0x0024CCD3
		// (set) Token: 0x06005098 RID: 20632 RVA: 0x0024DCDB File Offset: 0x0024CCDB
		internal long LangFE
		{
			get
			{
				return this._langFE;
			}
			set
			{
				this._langFE = value;
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06005099 RID: 20633 RVA: 0x0024DCE4 File Offset: 0x0024CCE4
		// (set) Token: 0x0600509A RID: 20634 RVA: 0x0024DCEC File Offset: 0x0024CCEC
		internal long LangCur
		{
			get
			{
				return this._langCur;
			}
			set
			{
				this._langCur = value;
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x0600509B RID: 20635 RVA: 0x0024DCF5 File Offset: 0x0024CCF5
		// (set) Token: 0x0600509C RID: 20636 RVA: 0x0024DCFD File Offset: 0x0024CCFD
		internal FontSlot FontSlot
		{
			get
			{
				return this._fontSlot;
			}
			set
			{
				this._fontSlot = value;
			}
		}

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x0600509D RID: 20637 RVA: 0x0024DD06 File Offset: 0x0024CD06
		// (set) Token: 0x0600509E RID: 20638 RVA: 0x0024DD0E File Offset: 0x0024CD0E
		internal long SB
		{
			get
			{
				return this._sb;
			}
			set
			{
				this._sb = value;
			}
		}

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x0600509F RID: 20639 RVA: 0x0024DD17 File Offset: 0x0024CD17
		// (set) Token: 0x060050A0 RID: 20640 RVA: 0x0024DD1F File Offset: 0x0024CD1F
		internal long SA
		{
			get
			{
				return this._sa;
			}
			set
			{
				this._sa = value;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x060050A1 RID: 20641 RVA: 0x0024DD28 File Offset: 0x0024CD28
		// (set) Token: 0x060050A2 RID: 20642 RVA: 0x0024DD30 File Offset: 0x0024CD30
		internal long FI
		{
			get
			{
				return this._fi;
			}
			set
			{
				this._fi = value;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x0024DD39 File Offset: 0x0024CD39
		// (set) Token: 0x060050A4 RID: 20644 RVA: 0x0024DD41 File Offset: 0x0024CD41
		internal long RI
		{
			get
			{
				return this._ri;
			}
			set
			{
				this._ri = value;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x060050A5 RID: 20645 RVA: 0x0024DD4A File Offset: 0x0024CD4A
		// (set) Token: 0x060050A6 RID: 20646 RVA: 0x0024DD52 File Offset: 0x0024CD52
		internal long LI
		{
			get
			{
				return this._li;
			}
			set
			{
				this._li = value;
			}
		}

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x060050A7 RID: 20647 RVA: 0x0024DD5B File Offset: 0x0024CD5B
		// (set) Token: 0x060050A8 RID: 20648 RVA: 0x0024DD63 File Offset: 0x0024CD63
		internal HAlign HAlign
		{
			get
			{
				return this._align;
			}
			set
			{
				this._align = value;
			}
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x060050A9 RID: 20649 RVA: 0x0024DD6C File Offset: 0x0024CD6C
		// (set) Token: 0x060050AA RID: 20650 RVA: 0x0024DD74 File Offset: 0x0024CD74
		internal long ILVL
		{
			get
			{
				return this._ilvl;
			}
			set
			{
				if (value >= 0L && value <= 32L)
				{
					this._ilvl = value;
				}
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x060050AB RID: 20651 RVA: 0x0024DD88 File Offset: 0x0024CD88
		// (set) Token: 0x060050AC RID: 20652 RVA: 0x0024DD90 File Offset: 0x0024CD90
		internal long PNLVL
		{
			get
			{
				return this._pnlvl;
			}
			set
			{
				this._pnlvl = value;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x060050AD RID: 20653 RVA: 0x0024DD99 File Offset: 0x0024CD99
		// (set) Token: 0x060050AE RID: 20654 RVA: 0x0024DDA1 File Offset: 0x0024CDA1
		internal long ITAP
		{
			get
			{
				return this._itap;
			}
			set
			{
				if (value >= 0L && value <= 32L)
				{
					this._itap = value;
				}
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x060050AF RID: 20655 RVA: 0x0024DDB5 File Offset: 0x0024CDB5
		// (set) Token: 0x060050B0 RID: 20656 RVA: 0x0024DDBD File Offset: 0x0024CDBD
		internal long ILS
		{
			get
			{
				return this._ils;
			}
			set
			{
				this._ils = value;
			}
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x060050B1 RID: 20657 RVA: 0x0024DDC6 File Offset: 0x0024CDC6
		// (set) Token: 0x060050B2 RID: 20658 RVA: 0x0024DDCE File Offset: 0x0024CDCE
		internal DirState DirPara
		{
			get
			{
				return this._dirPara;
			}
			set
			{
				this._dirPara = value;
			}
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x060050B3 RID: 20659 RVA: 0x0024DDD7 File Offset: 0x0024CDD7
		// (set) Token: 0x060050B4 RID: 20660 RVA: 0x0024DDDF File Offset: 0x0024CDDF
		internal long CFPara
		{
			get
			{
				return this._cfPara;
			}
			set
			{
				this._cfPara = value;
			}
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x0024DDE8 File Offset: 0x0024CDE8
		// (set) Token: 0x060050B6 RID: 20662 RVA: 0x0024DDF0 File Offset: 0x0024CDF0
		internal long CBPara
		{
			get
			{
				return this._cbPara;
			}
			set
			{
				this._cbPara = value;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x060050B7 RID: 20663 RVA: 0x0024DDF9 File Offset: 0x0024CDF9
		// (set) Token: 0x060050B8 RID: 20664 RVA: 0x0024DE01 File Offset: 0x0024CE01
		internal long ParaShading
		{
			get
			{
				return this._nParaShading;
			}
			set
			{
				this._nParaShading = Validators.MakeValidShading(value);
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x060050B9 RID: 20665 RVA: 0x0024DE0F File Offset: 0x0024CE0F
		// (set) Token: 0x060050BA RID: 20666 RVA: 0x0024DE17 File Offset: 0x0024CE17
		internal MarkerStyle Marker
		{
			get
			{
				return this._marker;
			}
			set
			{
				this._marker = value;
			}
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x060050BB RID: 20667 RVA: 0x0024DE20 File Offset: 0x0024CE20
		// (set) Token: 0x060050BC RID: 20668 RVA: 0x0024DE28 File Offset: 0x0024CE28
		internal bool IsContinue
		{
			get
			{
				return this._fContinue;
			}
			set
			{
				this._fContinue = value;
			}
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x0024DE31 File Offset: 0x0024CE31
		// (set) Token: 0x060050BE RID: 20670 RVA: 0x0024DE39 File Offset: 0x0024CE39
		internal long StartIndex
		{
			get
			{
				return this._nStartIndex;
			}
			set
			{
				this._nStartIndex = value;
			}
		}

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x060050BF RID: 20671 RVA: 0x0024DE42 File Offset: 0x0024CE42
		// (set) Token: 0x060050C0 RID: 20672 RVA: 0x0024DE4A File Offset: 0x0024CE4A
		internal long StartIndexDefault
		{
			get
			{
				return this._nStartIndexDefault;
			}
			set
			{
				this._nStartIndexDefault = value;
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x060050C1 RID: 20673 RVA: 0x0024DE53 File Offset: 0x0024CE53
		// (set) Token: 0x060050C2 RID: 20674 RVA: 0x0024DE5B File Offset: 0x0024CE5B
		internal long SL
		{
			get
			{
				return this._sl;
			}
			set
			{
				this._sl = value;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x060050C3 RID: 20675 RVA: 0x0024DE64 File Offset: 0x0024CE64
		// (set) Token: 0x060050C4 RID: 20676 RVA: 0x0024DE6C File Offset: 0x0024CE6C
		internal bool SLMult
		{
			get
			{
				return this._slMult;
			}
			set
			{
				this._slMult = value;
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x060050C5 RID: 20677 RVA: 0x0024DE75 File Offset: 0x0024CE75
		// (set) Token: 0x060050C6 RID: 20678 RVA: 0x0024DE7D File Offset: 0x0024CE7D
		internal bool IsInTable
		{
			get
			{
				return this._fInTable;
			}
			set
			{
				this._fInTable = value;
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x060050C7 RID: 20679 RVA: 0x0024DE86 File Offset: 0x0024CE86
		internal long TableLevel
		{
			get
			{
				if (!this._fInTable && this._itap <= 0L)
				{
					return 0L;
				}
				if (this._itap <= 0L)
				{
					return 1L;
				}
				return this._itap;
			}
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x060050C8 RID: 20680 RVA: 0x0024DEB0 File Offset: 0x0024CEB0
		internal long ListLevel
		{
			get
			{
				if (this._ils >= 0L || this._ilvl > 0L)
				{
					if (this._ilvl <= 0L)
					{
						return 1L;
					}
					return this._ilvl + 1L;
				}
				else
				{
					if (this.PNLVL > 0L)
					{
						return this.PNLVL;
					}
					if (this._marker != MarkerStyle.MarkerNone)
					{
						return 1L;
					}
					return 0L;
				}
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x060050C9 RID: 20681 RVA: 0x0024DF07 File Offset: 0x0024CF07
		// (set) Token: 0x060050CA RID: 20682 RVA: 0x0024DF0F File Offset: 0x0024CF0F
		internal int UnicodeSkip
		{
			get
			{
				return this._stateSkip;
			}
			set
			{
				if (value >= 0 && value < 65535)
				{
					this._stateSkip = value;
				}
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x0024DF24 File Offset: 0x0024CF24
		// (set) Token: 0x060050CC RID: 20684 RVA: 0x0024DF3F File Offset: 0x0024CF3F
		internal RowFormat RowFormat
		{
			get
			{
				if (this._rowFormat == null)
				{
					this._rowFormat = new RowFormat();
				}
				return this._rowFormat;
			}
			set
			{
				this._rowFormat = value;
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x0024DF48 File Offset: 0x0024CF48
		internal bool HasRowFormat
		{
			get
			{
				return this._rowFormat != null;
			}
		}

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x0024DF53 File Offset: 0x0024CF53
		internal ParaBorder ParaBorder
		{
			get
			{
				if (this._pb == null)
				{
					this._pb = new ParaBorder();
				}
				return this._pb;
			}
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x0024DF6E File Offset: 0x0024CF6E
		internal bool HasParaBorder
		{
			get
			{
				return this._pb != null && !this._pb.IsNone;
			}
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0024DF88 File Offset: 0x0024CF88
		// (set) Token: 0x060050D1 RID: 20689 RVA: 0x0024DF90 File Offset: 0x0024CF90
		internal RtfImageFormat ImageFormat
		{
			get
			{
				return this._imageFormat;
			}
			set
			{
				this._imageFormat = value;
			}
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x060050D2 RID: 20690 RVA: 0x0024DF99 File Offset: 0x0024CF99
		// (set) Token: 0x060050D3 RID: 20691 RVA: 0x0024DFA1 File Offset: 0x0024CFA1
		internal string ImageSource
		{
			get
			{
				return this._imageSource;
			}
			set
			{
				this._imageSource = value;
			}
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x060050D4 RID: 20692 RVA: 0x0024DFAA File Offset: 0x0024CFAA
		// (set) Token: 0x060050D5 RID: 20693 RVA: 0x0024DFB2 File Offset: 0x0024CFB2
		internal double ImageWidth
		{
			get
			{
				return this._imageWidth;
			}
			set
			{
				this._imageWidth = value;
			}
		}

		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x060050D6 RID: 20694 RVA: 0x0024DFBB File Offset: 0x0024CFBB
		// (set) Token: 0x060050D7 RID: 20695 RVA: 0x0024DFC3 File Offset: 0x0024CFC3
		internal double ImageHeight
		{
			get
			{
				return this._imageHeight;
			}
			set
			{
				this._imageHeight = value;
			}
		}

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x060050D8 RID: 20696 RVA: 0x0024DFCC File Offset: 0x0024CFCC
		// (set) Token: 0x060050D9 RID: 20697 RVA: 0x0024DFD4 File Offset: 0x0024CFD4
		internal double ImageBaselineOffset
		{
			get
			{
				return this._imageBaselineOffset;
			}
			set
			{
				this._imageBaselineOffset = value;
			}
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x060050DA RID: 20698 RVA: 0x0024DFDD File Offset: 0x0024CFDD
		// (set) Token: 0x060050DB RID: 20699 RVA: 0x0024DFE5 File Offset: 0x0024CFE5
		internal bool IncludeImageBaselineOffset
		{
			get
			{
				return this._isIncludeImageBaselineOffset;
			}
			set
			{
				this._isIncludeImageBaselineOffset = value;
			}
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x060050DC RID: 20700 RVA: 0x0024DFEE File Offset: 0x0024CFEE
		// (set) Token: 0x060050DD RID: 20701 RVA: 0x0024DFF6 File Offset: 0x0024CFF6
		internal double ImageScaleWidth
		{
			get
			{
				return this._imageScaleWidth;
			}
			set
			{
				this._imageScaleWidth = value;
			}
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x060050DE RID: 20702 RVA: 0x0024DFFF File Offset: 0x0024CFFF
		// (set) Token: 0x060050DF RID: 20703 RVA: 0x0024E007 File Offset: 0x0024D007
		internal double ImageScaleHeight
		{
			get
			{
				return this._imageScaleHeight;
			}
			set
			{
				this._imageScaleHeight = value;
			}
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x060050E0 RID: 20704 RVA: 0x0024E010 File Offset: 0x0024D010
		// (set) Token: 0x060050E1 RID: 20705 RVA: 0x0024E018 File Offset: 0x0024D018
		internal bool IsImageDataBinary
		{
			get
			{
				return this._isImageDataBinary;
			}
			set
			{
				this._isImageDataBinary = value;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x060050E2 RID: 20706 RVA: 0x0024E021 File Offset: 0x0024D021
		// (set) Token: 0x060050E3 RID: 20707 RVA: 0x0024E029 File Offset: 0x0024D029
		internal string ImageStretch
		{
			get
			{
				return this._imageStretch;
			}
			set
			{
				this._imageStretch = value;
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x060050E4 RID: 20708 RVA: 0x0024E032 File Offset: 0x0024D032
		// (set) Token: 0x060050E5 RID: 20709 RVA: 0x0024E03A File Offset: 0x0024D03A
		internal string ImageStretchDirection
		{
			get
			{
				return this._imageStretchDirection;
			}
			set
			{
				this._imageStretchDirection = value;
			}
		}

		// Token: 0x04002DEE RID: 11758
		private RtfDestination _dest;

		// Token: 0x04002DEF RID: 11759
		private bool _fBold;

		// Token: 0x04002DF0 RID: 11760
		private bool _fItalic;

		// Token: 0x04002DF1 RID: 11761
		private bool _fSuper;

		// Token: 0x04002DF2 RID: 11762
		private bool _fSub;

		// Token: 0x04002DF3 RID: 11763
		private bool _fOutline;

		// Token: 0x04002DF4 RID: 11764
		private bool _fEngrave;

		// Token: 0x04002DF5 RID: 11765
		private bool _fShadow;

		// Token: 0x04002DF6 RID: 11766
		private bool _fScaps;

		// Token: 0x04002DF7 RID: 11767
		private long _fs;

		// Token: 0x04002DF8 RID: 11768
		private long _font;

		// Token: 0x04002DF9 RID: 11769
		private int _codePage;

		// Token: 0x04002DFA RID: 11770
		private long _superOffset;

		// Token: 0x04002DFB RID: 11771
		private long _cf;

		// Token: 0x04002DFC RID: 11772
		private long _cb;

		// Token: 0x04002DFD RID: 11773
		private DirState _dirChar;

		// Token: 0x04002DFE RID: 11774
		private ULState _ul;

		// Token: 0x04002DFF RID: 11775
		private StrikeState _strike;

		// Token: 0x04002E00 RID: 11776
		private long _expand;

		// Token: 0x04002E01 RID: 11777
		private long _lang;

		// Token: 0x04002E02 RID: 11778
		private long _langFE;

		// Token: 0x04002E03 RID: 11779
		private long _langCur;

		// Token: 0x04002E04 RID: 11780
		private FontSlot _fontSlot;

		// Token: 0x04002E05 RID: 11781
		private long _sa;

		// Token: 0x04002E06 RID: 11782
		private long _sb;

		// Token: 0x04002E07 RID: 11783
		private long _li;

		// Token: 0x04002E08 RID: 11784
		private long _ri;

		// Token: 0x04002E09 RID: 11785
		private long _fi;

		// Token: 0x04002E0A RID: 11786
		private HAlign _align;

		// Token: 0x04002E0B RID: 11787
		private long _ils;

		// Token: 0x04002E0C RID: 11788
		private long _ilvl;

		// Token: 0x04002E0D RID: 11789
		private long _pnlvl;

		// Token: 0x04002E0E RID: 11790
		private long _itap;

		// Token: 0x04002E0F RID: 11791
		private DirState _dirPara;

		// Token: 0x04002E10 RID: 11792
		private long _cfPara;

		// Token: 0x04002E11 RID: 11793
		private long _cbPara;

		// Token: 0x04002E12 RID: 11794
		private long _nParaShading;

		// Token: 0x04002E13 RID: 11795
		private MarkerStyle _marker;

		// Token: 0x04002E14 RID: 11796
		private bool _fContinue;

		// Token: 0x04002E15 RID: 11797
		private long _nStartIndex;

		// Token: 0x04002E16 RID: 11798
		private long _nStartIndexDefault;

		// Token: 0x04002E17 RID: 11799
		private long _sl;

		// Token: 0x04002E18 RID: 11800
		private bool _slMult;

		// Token: 0x04002E19 RID: 11801
		private ParaBorder _pb;

		// Token: 0x04002E1A RID: 11802
		private bool _fInTable;

		// Token: 0x04002E1B RID: 11803
		private bool _fHidden;

		// Token: 0x04002E1C RID: 11804
		private int _stateSkip;

		// Token: 0x04002E1D RID: 11805
		private RowFormat _rowFormat;

		// Token: 0x04002E1E RID: 11806
		private static FormatState _fsEmptyState;

		// Token: 0x04002E1F RID: 11807
		private RtfImageFormat _imageFormat;

		// Token: 0x04002E20 RID: 11808
		private string _imageSource;

		// Token: 0x04002E21 RID: 11809
		private double _imageWidth;

		// Token: 0x04002E22 RID: 11810
		private double _imageHeight;

		// Token: 0x04002E23 RID: 11811
		private double _imageBaselineOffset;

		// Token: 0x04002E24 RID: 11812
		private bool _isIncludeImageBaselineOffset;

		// Token: 0x04002E25 RID: 11813
		private double _imageScaleWidth;

		// Token: 0x04002E26 RID: 11814
		private double _imageScaleHeight;

		// Token: 0x04002E27 RID: 11815
		private bool _isImageDataBinary;

		// Token: 0x04002E28 RID: 11816
		private string _imageStretch;

		// Token: 0x04002E29 RID: 11817
		private string _imageStretchDirection;

		// Token: 0x04002E2A RID: 11818
		private const int MAX_LIST_DEPTH = 32;

		// Token: 0x04002E2B RID: 11819
		private const int MAX_TABLE_DEPTH = 32;
	}
}
