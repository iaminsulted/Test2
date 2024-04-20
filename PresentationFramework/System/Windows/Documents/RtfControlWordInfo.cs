using System;

namespace System.Windows.Documents
{
	// Token: 0x02000655 RID: 1621
	internal class RtfControlWordInfo
	{
		// Token: 0x0600501F RID: 20511 RVA: 0x0024BD9D File Offset: 0x0024AD9D
		internal RtfControlWordInfo(RtfControlWord controlWord, string controlName, uint flags)
		{
			this._controlWord = controlWord;
			this._controlName = controlName;
			this._flags = flags;
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06005020 RID: 20512 RVA: 0x0024BDBA File Offset: 0x0024ADBA
		internal RtfControlWord Control
		{
			get
			{
				return this._controlWord;
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x0024BDC2 File Offset: 0x0024ADC2
		internal string ControlName
		{
			get
			{
				return this._controlName;
			}
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06005022 RID: 20514 RVA: 0x0024BDCA File Offset: 0x0024ADCA
		internal uint Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x04002D56 RID: 11606
		private RtfControlWord _controlWord;

		// Token: 0x04002D57 RID: 11607
		private string _controlName;

		// Token: 0x04002D58 RID: 11608
		private uint _flags;
	}
}
