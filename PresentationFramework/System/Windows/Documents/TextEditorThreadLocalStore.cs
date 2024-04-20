using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020006A9 RID: 1705
	internal class TextEditorThreadLocalStore
	{
		// Token: 0x06005651 RID: 22097 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal TextEditorThreadLocalStore()
		{
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06005652 RID: 22098 RVA: 0x00268973 File Offset: 0x00267973
		// (set) Token: 0x06005653 RID: 22099 RVA: 0x0026897B File Offset: 0x0026797B
		internal int InputLanguageChangeEventHandlerCount
		{
			get
			{
				return this._inputLanguageChangeEventHandlerCount;
			}
			set
			{
				this._inputLanguageChangeEventHandlerCount = value;
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06005654 RID: 22100 RVA: 0x00268984 File Offset: 0x00267984
		// (set) Token: 0x06005655 RID: 22101 RVA: 0x0026898C File Offset: 0x0026798C
		internal ArrayList PendingInputItems
		{
			get
			{
				return this._pendingInputItems;
			}
			set
			{
				this._pendingInputItems = value;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06005656 RID: 22102 RVA: 0x00268995 File Offset: 0x00267995
		// (set) Token: 0x06005657 RID: 22103 RVA: 0x0026899D File Offset: 0x0026799D
		internal bool PureControlShift
		{
			get
			{
				return this._pureControlShift;
			}
			set
			{
				this._pureControlShift = value;
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06005658 RID: 22104 RVA: 0x002689A6 File Offset: 0x002679A6
		// (set) Token: 0x06005659 RID: 22105 RVA: 0x002689AE File Offset: 0x002679AE
		internal bool Bidi
		{
			get
			{
				return this._bidi;
			}
			set
			{
				this._bidi = value;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x0600565A RID: 22106 RVA: 0x002689B7 File Offset: 0x002679B7
		// (set) Token: 0x0600565B RID: 22107 RVA: 0x002689BF File Offset: 0x002679BF
		internal TextSelection FocusedTextSelection
		{
			get
			{
				return this._focusedTextSelection;
			}
			set
			{
				this._focusedTextSelection = value;
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x0600565C RID: 22108 RVA: 0x002689C8 File Offset: 0x002679C8
		// (set) Token: 0x0600565D RID: 22109 RVA: 0x002689D0 File Offset: 0x002679D0
		internal TextServicesHost TextServicesHost
		{
			get
			{
				return this._textServicesHost;
			}
			set
			{
				this._textServicesHost = value;
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x0600565E RID: 22110 RVA: 0x002689D9 File Offset: 0x002679D9
		// (set) Token: 0x0600565F RID: 22111 RVA: 0x002689E1 File Offset: 0x002679E1
		internal bool HideCursor
		{
			get
			{
				return this._hideCursor;
			}
			set
			{
				this._hideCursor = value;
			}
		}

		// Token: 0x04002F8E RID: 12174
		private int _inputLanguageChangeEventHandlerCount;

		// Token: 0x04002F8F RID: 12175
		private ArrayList _pendingInputItems;

		// Token: 0x04002F90 RID: 12176
		private bool _pureControlShift;

		// Token: 0x04002F91 RID: 12177
		private bool _bidi;

		// Token: 0x04002F92 RID: 12178
		private TextSelection _focusedTextSelection;

		// Token: 0x04002F93 RID: 12179
		private TextServicesHost _textServicesHost;

		// Token: 0x04002F94 RID: 12180
		private bool _hideCursor;
	}
}
