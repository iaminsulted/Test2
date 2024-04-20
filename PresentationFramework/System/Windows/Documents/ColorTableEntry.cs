using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000674 RID: 1652
	internal class ColorTableEntry
	{
		// Token: 0x06005181 RID: 20865 RVA: 0x0024FB58 File Offset: 0x0024EB58
		internal ColorTableEntry()
		{
			this._color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			this._bAuto = false;
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06005182 RID: 20866 RVA: 0x0024FB7A File Offset: 0x0024EB7A
		// (set) Token: 0x06005183 RID: 20867 RVA: 0x0024FB82 File Offset: 0x0024EB82
		internal Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06005184 RID: 20868 RVA: 0x0024FB8B File Offset: 0x0024EB8B
		// (set) Token: 0x06005185 RID: 20869 RVA: 0x0024FB93 File Offset: 0x0024EB93
		internal bool IsAuto
		{
			get
			{
				return this._bAuto;
			}
			set
			{
				this._bAuto = value;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (set) Token: 0x06005186 RID: 20870 RVA: 0x0024FB9C File Offset: 0x0024EB9C
		internal byte Red
		{
			set
			{
				this._color = Color.FromArgb(byte.MaxValue, value, this._color.G, this._color.B);
			}
		}

		// Token: 0x17001333 RID: 4915
		// (set) Token: 0x06005187 RID: 20871 RVA: 0x0024FBC5 File Offset: 0x0024EBC5
		internal byte Green
		{
			set
			{
				this._color = Color.FromArgb(byte.MaxValue, this._color.R, value, this._color.B);
			}
		}

		// Token: 0x17001334 RID: 4916
		// (set) Token: 0x06005188 RID: 20872 RVA: 0x0024FBEE File Offset: 0x0024EBEE
		internal byte Blue
		{
			set
			{
				this._color = Color.FromArgb(byte.MaxValue, this._color.R, this._color.G, value);
			}
		}

		// Token: 0x04002E6D RID: 11885
		private Color _color;

		// Token: 0x04002E6E RID: 11886
		private bool _bAuto;
	}
}
