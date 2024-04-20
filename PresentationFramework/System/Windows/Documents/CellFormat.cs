using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x0200066E RID: 1646
	internal class CellFormat
	{
		// Token: 0x0600510D RID: 20749 RVA: 0x0024E734 File Offset: 0x0024D734
		internal CellFormat()
		{
			this.BorderLeft = new BorderFormat();
			this.BorderRight = new BorderFormat();
			this.BorderBottom = new BorderFormat();
			this.BorderTop = new BorderFormat();
			this.Width = new CellWidth();
			this.SetDefaults();
			this.IsPending = true;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x0024E78C File Offset: 0x0024D78C
		internal CellFormat(CellFormat cf)
		{
			this.CellX = cf.CellX;
			this.IsCellXSet = cf.IsCellXSet;
			this.Width = new CellWidth(cf.Width);
			this.CB = cf.CB;
			this.CF = cf.CF;
			this.Shading = cf.Shading;
			this.PaddingTop = cf.PaddingTop;
			this.PaddingBottom = cf.PaddingBottom;
			this.PaddingRight = cf.PaddingRight;
			this.PaddingLeft = cf.PaddingLeft;
			this.BorderLeft = new BorderFormat(cf.BorderLeft);
			this.BorderRight = new BorderFormat(cf.BorderRight);
			this.BorderBottom = new BorderFormat(cf.BorderBottom);
			this.BorderTop = new BorderFormat(cf.BorderTop);
			this.SpacingTop = cf.SpacingTop;
			this.SpacingBottom = cf.SpacingBottom;
			this.SpacingRight = cf.SpacingRight;
			this.SpacingLeft = cf.SpacingLeft;
			this.VAlign = VAlign.AlignTop;
			this.IsPending = true;
			this.IsHMerge = cf.IsHMerge;
			this.IsHMergeFirst = cf.IsHMergeFirst;
			this.IsVMerge = cf.IsVMerge;
			this.IsVMergeFirst = cf.IsVMergeFirst;
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x0600510F RID: 20751 RVA: 0x0024E8CE File Offset: 0x0024D8CE
		// (set) Token: 0x06005110 RID: 20752 RVA: 0x0024E8D6 File Offset: 0x0024D8D6
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

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06005111 RID: 20753 RVA: 0x0024E8DF File Offset: 0x0024D8DF
		// (set) Token: 0x06005112 RID: 20754 RVA: 0x0024E8E7 File Offset: 0x0024D8E7
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

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06005113 RID: 20755 RVA: 0x0024E8F0 File Offset: 0x0024D8F0
		// (set) Token: 0x06005114 RID: 20756 RVA: 0x0024E8F8 File Offset: 0x0024D8F8
		internal long Shading
		{
			get
			{
				return this._nShading;
			}
			set
			{
				this._nShading = Validators.MakeValidShading(value);
			}
		}

		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06005115 RID: 20757 RVA: 0x0024E906 File Offset: 0x0024D906
		// (set) Token: 0x06005116 RID: 20758 RVA: 0x0024E90E File Offset: 0x0024D90E
		internal long PaddingLeft
		{
			get
			{
				return this._padL;
			}
			set
			{
				this._padL = value;
			}
		}

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06005117 RID: 20759 RVA: 0x0024E917 File Offset: 0x0024D917
		// (set) Token: 0x06005118 RID: 20760 RVA: 0x0024E91F File Offset: 0x0024D91F
		internal long PaddingRight
		{
			get
			{
				return this._padR;
			}
			set
			{
				this._padR = value;
			}
		}

		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06005119 RID: 20761 RVA: 0x0024E928 File Offset: 0x0024D928
		// (set) Token: 0x0600511A RID: 20762 RVA: 0x0024E930 File Offset: 0x0024D930
		internal long PaddingTop
		{
			get
			{
				return this._padT;
			}
			set
			{
				this._padT = value;
			}
		}

		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x0600511B RID: 20763 RVA: 0x0024E939 File Offset: 0x0024D939
		// (set) Token: 0x0600511C RID: 20764 RVA: 0x0024E941 File Offset: 0x0024D941
		internal long PaddingBottom
		{
			get
			{
				return this._padB;
			}
			set
			{
				this._padB = value;
			}
		}

		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x0600511D RID: 20765 RVA: 0x0024E94A File Offset: 0x0024D94A
		// (set) Token: 0x0600511E RID: 20766 RVA: 0x0024E952 File Offset: 0x0024D952
		internal BorderFormat BorderTop
		{
			get
			{
				return this._brdT;
			}
			set
			{
				this._brdT = value;
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x0600511F RID: 20767 RVA: 0x0024E95B File Offset: 0x0024D95B
		// (set) Token: 0x06005120 RID: 20768 RVA: 0x0024E963 File Offset: 0x0024D963
		internal BorderFormat BorderBottom
		{
			get
			{
				return this._brdB;
			}
			set
			{
				this._brdB = value;
			}
		}

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x0024E96C File Offset: 0x0024D96C
		// (set) Token: 0x06005122 RID: 20770 RVA: 0x0024E974 File Offset: 0x0024D974
		internal BorderFormat BorderLeft
		{
			get
			{
				return this._brdL;
			}
			set
			{
				this._brdL = value;
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x0024E97D File Offset: 0x0024D97D
		// (set) Token: 0x06005124 RID: 20772 RVA: 0x0024E985 File Offset: 0x0024D985
		internal BorderFormat BorderRight
		{
			get
			{
				return this._brdR;
			}
			set
			{
				this._brdR = value;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x0024E98E File Offset: 0x0024D98E
		// (set) Token: 0x06005126 RID: 20774 RVA: 0x0024E996 File Offset: 0x0024D996
		internal CellWidth Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
			}
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06005127 RID: 20775 RVA: 0x0024E99F File Offset: 0x0024D99F
		// (set) Token: 0x06005128 RID: 20776 RVA: 0x0024E9A7 File Offset: 0x0024D9A7
		internal long CellX
		{
			get
			{
				return this._nCellX;
			}
			set
			{
				this._nCellX = value;
				this._fCellXSet = true;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06005129 RID: 20777 RVA: 0x0024E9B7 File Offset: 0x0024D9B7
		// (set) Token: 0x0600512A RID: 20778 RVA: 0x0024E9BF File Offset: 0x0024D9BF
		internal bool IsCellXSet
		{
			get
			{
				return this._fCellXSet;
			}
			set
			{
				this._fCellXSet = value;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (set) Token: 0x0600512B RID: 20779 RVA: 0x0024E9C8 File Offset: 0x0024D9C8
		internal VAlign VAlign
		{
			set
			{
				this._valign = value;
			}
		}

		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x0600512C RID: 20780 RVA: 0x0024E9D1 File Offset: 0x0024D9D1
		// (set) Token: 0x0600512D RID: 20781 RVA: 0x0024E9D9 File Offset: 0x0024D9D9
		internal long SpacingTop
		{
			get
			{
				return this._spaceT;
			}
			set
			{
				this._spaceT = value;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0024E9E2 File Offset: 0x0024D9E2
		// (set) Token: 0x0600512F RID: 20783 RVA: 0x0024E9EA File Offset: 0x0024D9EA
		internal long SpacingLeft
		{
			get
			{
				return this._spaceL;
			}
			set
			{
				this._spaceL = value;
			}
		}

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x0024E9F3 File Offset: 0x0024D9F3
		// (set) Token: 0x06005131 RID: 20785 RVA: 0x0024E9FB File Offset: 0x0024D9FB
		internal long SpacingBottom
		{
			get
			{
				return this._spaceB;
			}
			set
			{
				this._spaceB = value;
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06005132 RID: 20786 RVA: 0x0024EA04 File Offset: 0x0024DA04
		// (set) Token: 0x06005133 RID: 20787 RVA: 0x0024EA0C File Offset: 0x0024DA0C
		internal long SpacingRight
		{
			get
			{
				return this._spaceR;
			}
			set
			{
				this._spaceR = value;
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06005134 RID: 20788 RVA: 0x0024EA15 File Offset: 0x0024DA15
		// (set) Token: 0x06005135 RID: 20789 RVA: 0x0024EA1D File Offset: 0x0024DA1D
		internal bool IsPending
		{
			get
			{
				return this._fPending;
			}
			set
			{
				this._fPending = value;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06005136 RID: 20790 RVA: 0x0024EA26 File Offset: 0x0024DA26
		// (set) Token: 0x06005137 RID: 20791 RVA: 0x0024EA2E File Offset: 0x0024DA2E
		internal bool IsHMerge
		{
			get
			{
				return this._fHMerge;
			}
			set
			{
				this._fHMerge = value;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06005138 RID: 20792 RVA: 0x0024EA37 File Offset: 0x0024DA37
		// (set) Token: 0x06005139 RID: 20793 RVA: 0x0024EA3F File Offset: 0x0024DA3F
		internal bool IsHMergeFirst
		{
			get
			{
				return this._fHMergeFirst;
			}
			set
			{
				this._fHMergeFirst = value;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x0600513A RID: 20794 RVA: 0x0024EA48 File Offset: 0x0024DA48
		// (set) Token: 0x0600513B RID: 20795 RVA: 0x0024EA50 File Offset: 0x0024DA50
		internal bool IsVMerge
		{
			get
			{
				return this._fVMerge;
			}
			set
			{
				this._fVMerge = value;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x0600513C RID: 20796 RVA: 0x0024EA59 File Offset: 0x0024DA59
		// (set) Token: 0x0600513D RID: 20797 RVA: 0x0024EA61 File Offset: 0x0024DA61
		internal bool IsVMergeFirst
		{
			get
			{
				return this._fVMergeFirst;
			}
			set
			{
				this._fVMergeFirst = value;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x0024EA6A File Offset: 0x0024DA6A
		internal bool HasBorder
		{
			get
			{
				return this.BorderLeft.EffectiveWidth > 0L || this.BorderRight.EffectiveWidth > 0L || this.BorderTop.EffectiveWidth > 0L || this.BorderBottom.EffectiveWidth > 0L;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x0600513F RID: 20799 RVA: 0x0024EAAC File Offset: 0x0024DAAC
		internal string RTFEncodingForWidth
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("\\clftsWidth");
				stringBuilder.Append(((int)this.Width.Type).ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\\clwWidth");
				stringBuilder.Append(this.Width.Value.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\\cellx");
				stringBuilder.Append(this.CellX.ToString(CultureInfo.InvariantCulture));
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x0024EB40 File Offset: 0x0024DB40
		internal void SetDefaults()
		{
			this.CellX = -1L;
			this.IsCellXSet = false;
			this.Width.SetDefaults();
			this.CB = -1L;
			this.CF = -1L;
			this.Shading = -1L;
			this.PaddingTop = 0L;
			this.PaddingBottom = 0L;
			this.PaddingRight = 0L;
			this.PaddingLeft = 0L;
			this.BorderLeft.SetDefaults();
			this.BorderRight.SetDefaults();
			this.BorderBottom.SetDefaults();
			this.BorderTop.SetDefaults();
			this.SpacingTop = 0L;
			this.SpacingBottom = 0L;
			this.SpacingRight = 0L;
			this.SpacingLeft = 0L;
			this.VAlign = VAlign.AlignTop;
			this.IsHMerge = false;
			this.IsHMergeFirst = false;
			this.IsVMerge = false;
			this.IsVMergeFirst = false;
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x0024EC10 File Offset: 0x0024DC10
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" BorderThickness=\"");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderLeft.EffectiveWidth));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderTop.EffectiveWidth));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderRight.EffectiveWidth));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderBottom.EffectiveWidth));
			stringBuilder.Append("\"");
			ColorTableEntry colorTableEntry = null;
			if (this.BorderLeft.CF >= 0L)
			{
				colorTableEntry = converterState.ColorTable.EntryAt((int)this.BorderLeft.CF);
			}
			if (colorTableEntry != null)
			{
				stringBuilder.Append(" BorderBrush=\"");
				stringBuilder.Append(colorTableEntry.Color.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\"");
			}
			else
			{
				stringBuilder.Append(" BorderBrush=\"#FF000000\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x0024ED34 File Offset: 0x0024DD34
		internal string GetPaddingAttributeString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" Padding=\"");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingLeft));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingTop));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingRight));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingBottom));
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x04002E41 RID: 11841
		private long _cb;

		// Token: 0x04002E42 RID: 11842
		private long _cf;

		// Token: 0x04002E43 RID: 11843
		private long _nShading;

		// Token: 0x04002E44 RID: 11844
		private long _padT;

		// Token: 0x04002E45 RID: 11845
		private long _padB;

		// Token: 0x04002E46 RID: 11846
		private long _padR;

		// Token: 0x04002E47 RID: 11847
		private long _padL;

		// Token: 0x04002E48 RID: 11848
		private long _spaceT;

		// Token: 0x04002E49 RID: 11849
		private long _spaceB;

		// Token: 0x04002E4A RID: 11850
		private long _spaceR;

		// Token: 0x04002E4B RID: 11851
		private long _spaceL;

		// Token: 0x04002E4C RID: 11852
		private long _nCellX;

		// Token: 0x04002E4D RID: 11853
		private CellWidth _width;

		// Token: 0x04002E4E RID: 11854
		private VAlign _valign;

		// Token: 0x04002E4F RID: 11855
		private BorderFormat _brdL;

		// Token: 0x04002E50 RID: 11856
		private BorderFormat _brdR;

		// Token: 0x04002E51 RID: 11857
		private BorderFormat _brdT;

		// Token: 0x04002E52 RID: 11858
		private BorderFormat _brdB;

		// Token: 0x04002E53 RID: 11859
		private bool _fPending;

		// Token: 0x04002E54 RID: 11860
		private bool _fHMerge;

		// Token: 0x04002E55 RID: 11861
		private bool _fHMergeFirst;

		// Token: 0x04002E56 RID: 11862
		private bool _fVMerge;

		// Token: 0x04002E57 RID: 11863
		private bool _fVMergeFirst;

		// Token: 0x04002E58 RID: 11864
		private bool _fCellXSet;
	}
}
