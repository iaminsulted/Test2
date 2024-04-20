using System;

namespace System.Windows.Documents
{
	// Token: 0x0200066D RID: 1645
	internal class CellWidth
	{
		// Token: 0x06005106 RID: 20742 RVA: 0x0024E6C8 File Offset: 0x0024D6C8
		internal CellWidth()
		{
			this.Type = WidthType.WidthAuto;
			this.Value = 0L;
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x0024E6DF File Offset: 0x0024D6DF
		internal CellWidth(CellWidth cw)
		{
			this.Type = cw.Type;
			this.Value = cw.Value;
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06005108 RID: 20744 RVA: 0x0024E6FF File Offset: 0x0024D6FF
		// (set) Token: 0x06005109 RID: 20745 RVA: 0x0024E707 File Offset: 0x0024D707
		internal WidthType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x0024E710 File Offset: 0x0024D710
		// (set) Token: 0x0600510B RID: 20747 RVA: 0x0024E718 File Offset: 0x0024D718
		internal long Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x0024E721 File Offset: 0x0024D721
		internal void SetDefaults()
		{
			this.Type = WidthType.WidthAuto;
			this.Value = 0L;
		}

		// Token: 0x04002E3F RID: 11839
		private WidthType _type;

		// Token: 0x04002E40 RID: 11840
		private long _value;
	}
}
