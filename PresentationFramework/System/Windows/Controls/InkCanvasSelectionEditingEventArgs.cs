using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	// Token: 0x02000818 RID: 2072
	public class InkCanvasSelectionEditingEventArgs : CancelEventArgs
	{
		// Token: 0x060078E8 RID: 30952 RVA: 0x00301946 File Offset: 0x00300946
		internal InkCanvasSelectionEditingEventArgs(Rect oldRectangle, Rect newRectangle)
		{
			this._oldRectangle = oldRectangle;
			this._newRectangle = newRectangle;
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x060078E9 RID: 30953 RVA: 0x0030195C File Offset: 0x0030095C
		public Rect OldRectangle
		{
			get
			{
				return this._oldRectangle;
			}
		}

		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x060078EA RID: 30954 RVA: 0x00301964 File Offset: 0x00300964
		// (set) Token: 0x060078EB RID: 30955 RVA: 0x0030196C File Offset: 0x0030096C
		public Rect NewRectangle
		{
			get
			{
				return this._newRectangle;
			}
			set
			{
				this._newRectangle = value;
			}
		}

		// Token: 0x0400398C RID: 14732
		private Rect _oldRectangle;

		// Token: 0x0400398D RID: 14733
		private Rect _newRectangle;
	}
}
