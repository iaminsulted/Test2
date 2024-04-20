using System;

namespace System.Windows.Documents
{
	// Token: 0x0200065A RID: 1626
	internal class RtfToken
	{
		// Token: 0x06005029 RID: 20521 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal RtfToken()
		{
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x0024BE88 File Offset: 0x0024AE88
		internal void Empty()
		{
			this._type = RtfTokenType.TokenInvalid;
			this._rtfControlWordInfo = null;
			this._parameter = 268435456L;
			this._text = "";
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x0024BEAF File Offset: 0x0024AEAF
		// (set) Token: 0x0600502C RID: 20524 RVA: 0x0024BEB7 File Offset: 0x0024AEB7
		internal RtfTokenType Type
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

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x0600502D RID: 20525 RVA: 0x0024BEC0 File Offset: 0x0024AEC0
		// (set) Token: 0x0600502E RID: 20526 RVA: 0x0024BEC8 File Offset: 0x0024AEC8
		internal RtfControlWordInfo RtfControlWordInfo
		{
			get
			{
				return this._rtfControlWordInfo;
			}
			set
			{
				this._rtfControlWordInfo = value;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x0600502F RID: 20527 RVA: 0x0024BED1 File Offset: 0x0024AED1
		// (set) Token: 0x06005030 RID: 20528 RVA: 0x0024BEE4 File Offset: 0x0024AEE4
		internal long Parameter
		{
			get
			{
				if (!this.HasParameter)
				{
					return 0L;
				}
				return this._parameter;
			}
			set
			{
				this._parameter = value;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06005031 RID: 20529 RVA: 0x0024BEED File Offset: 0x0024AEED
		// (set) Token: 0x06005032 RID: 20530 RVA: 0x0024BEF5 File Offset: 0x0024AEF5
		internal string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06005033 RID: 20531 RVA: 0x0024BEFE File Offset: 0x0024AEFE
		internal long ToggleValue
		{
			get
			{
				if (!this.HasParameter)
				{
					return 1L;
				}
				return this.Parameter;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06005034 RID: 20532 RVA: 0x0024BF11 File Offset: 0x0024AF11
		internal bool FlagValue
		{
			get
			{
				return !this.HasParameter || (this.HasParameter && this.Parameter > 0L);
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x0024BF30 File Offset: 0x0024AF30
		internal bool HasParameter
		{
			get
			{
				return this._parameter != 268435456L;
			}
		}

		// Token: 0x04002D82 RID: 11650
		internal const long INVALID_PARAMETER = 268435456L;

		// Token: 0x04002D83 RID: 11651
		private RtfTokenType _type;

		// Token: 0x04002D84 RID: 11652
		private RtfControlWordInfo _rtfControlWordInfo;

		// Token: 0x04002D85 RID: 11653
		private long _parameter;

		// Token: 0x04002D86 RID: 11654
		private string _text;
	}
}
