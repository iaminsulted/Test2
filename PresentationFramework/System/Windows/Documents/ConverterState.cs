using System;

namespace System.Windows.Documents
{
	// Token: 0x02000680 RID: 1664
	internal class ConverterState
	{
		// Token: 0x0600523A RID: 21050 RVA: 0x00252F74 File Offset: 0x00251F74
		internal ConverterState()
		{
			this._rtfFormatStack = new RtfFormatStack();
			this._documentNodeArray = new DocumentNodeArray();
			this._documentNodeArray.IsMain = true;
			this._fontTable = new FontTable();
			this._colorTable = new ColorTable();
			this._listTable = new ListTable();
			this._listOverrideTable = new ListOverrideTable();
			this._defaultFont = -1L;
			this._defaultLang = -1L;
			this._defaultLangFE = -1L;
			this._bMarkerWhiteSpace = false;
			this._bMarkerPresent = false;
			this._border = null;
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x00253002 File Offset: 0x00252002
		internal FormatState PreviousTopFormatState(int fromTop)
		{
			return this._rtfFormatStack.PrevTop(fromTop);
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x00253010 File Offset: 0x00252010
		internal RtfFormatStack RtfFormatStack
		{
			get
			{
				return this._rtfFormatStack;
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x00253018 File Offset: 0x00252018
		internal FontTable FontTable
		{
			get
			{
				return this._fontTable;
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x0600523E RID: 21054 RVA: 0x00253020 File Offset: 0x00252020
		internal ColorTable ColorTable
		{
			get
			{
				return this._colorTable;
			}
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x00253028 File Offset: 0x00252028
		internal ListTable ListTable
		{
			get
			{
				return this._listTable;
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06005240 RID: 21056 RVA: 0x00253030 File Offset: 0x00252030
		internal ListOverrideTable ListOverrideTable
		{
			get
			{
				return this._listOverrideTable;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06005241 RID: 21057 RVA: 0x00253038 File Offset: 0x00252038
		internal DocumentNodeArray DocumentNodeArray
		{
			get
			{
				return this._documentNodeArray;
			}
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06005242 RID: 21058 RVA: 0x00253040 File Offset: 0x00252040
		internal FormatState TopFormatState
		{
			get
			{
				return this._rtfFormatStack.Top();
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06005243 RID: 21059 RVA: 0x0025304D File Offset: 0x0025204D
		// (set) Token: 0x06005244 RID: 21060 RVA: 0x00253055 File Offset: 0x00252055
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

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x06005245 RID: 21061 RVA: 0x0025305E File Offset: 0x0025205E
		// (set) Token: 0x06005246 RID: 21062 RVA: 0x00253066 File Offset: 0x00252066
		internal long DefaultFont
		{
			get
			{
				return this._defaultFont;
			}
			set
			{
				this._defaultFont = value;
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06005247 RID: 21063 RVA: 0x0025306F File Offset: 0x0025206F
		// (set) Token: 0x06005248 RID: 21064 RVA: 0x00253077 File Offset: 0x00252077
		internal long DefaultLang
		{
			get
			{
				return this._defaultLang;
			}
			set
			{
				this._defaultLang = value;
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06005249 RID: 21065 RVA: 0x00253080 File Offset: 0x00252080
		// (set) Token: 0x0600524A RID: 21066 RVA: 0x00253088 File Offset: 0x00252088
		internal long DefaultLangFE
		{
			get
			{
				return this._defaultLangFE;
			}
			set
			{
				this._defaultLangFE = value;
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x0600524B RID: 21067 RVA: 0x00253091 File Offset: 0x00252091
		// (set) Token: 0x0600524C RID: 21068 RVA: 0x00253099 File Offset: 0x00252099
		internal bool IsMarkerWhiteSpace
		{
			get
			{
				return this._bMarkerWhiteSpace;
			}
			set
			{
				this._bMarkerWhiteSpace = value;
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x0600524D RID: 21069 RVA: 0x002530A2 File Offset: 0x002520A2
		// (set) Token: 0x0600524E RID: 21070 RVA: 0x002530AA File Offset: 0x002520AA
		internal bool IsMarkerPresent
		{
			get
			{
				return this._bMarkerPresent;
			}
			set
			{
				this._bMarkerPresent = value;
			}
		}

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x0600524F RID: 21071 RVA: 0x002530B3 File Offset: 0x002520B3
		// (set) Token: 0x06005250 RID: 21072 RVA: 0x002530BB File Offset: 0x002520BB
		internal BorderFormat CurrentBorder
		{
			get
			{
				return this._border;
			}
			set
			{
				this._border = value;
			}
		}

		// Token: 0x04002E94 RID: 11924
		private RtfFormatStack _rtfFormatStack;

		// Token: 0x04002E95 RID: 11925
		private DocumentNodeArray _documentNodeArray;

		// Token: 0x04002E96 RID: 11926
		private FontTable _fontTable;

		// Token: 0x04002E97 RID: 11927
		private ColorTable _colorTable;

		// Token: 0x04002E98 RID: 11928
		private ListTable _listTable;

		// Token: 0x04002E99 RID: 11929
		private ListOverrideTable _listOverrideTable;

		// Token: 0x04002E9A RID: 11930
		private long _defaultFont;

		// Token: 0x04002E9B RID: 11931
		private long _defaultLang;

		// Token: 0x04002E9C RID: 11932
		private long _defaultLangFE;

		// Token: 0x04002E9D RID: 11933
		private int _codePage;

		// Token: 0x04002E9E RID: 11934
		private bool _bMarkerWhiteSpace;

		// Token: 0x04002E9F RID: 11935
		private bool _bMarkerPresent;

		// Token: 0x04002EA0 RID: 11936
		private BorderFormat _border;
	}
}
