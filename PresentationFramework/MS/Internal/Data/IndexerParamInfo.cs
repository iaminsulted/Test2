using System;

namespace MS.Internal.Data
{
	// Token: 0x0200023A RID: 570
	internal struct IndexerParamInfo
	{
		// Token: 0x06001597 RID: 5527 RVA: 0x001556F5 File Offset: 0x001546F5
		public IndexerParamInfo(string paren, string value)
		{
			this.parenString = paren;
			this.valueString = value;
		}

		// Token: 0x04000C25 RID: 3109
		public string parenString;

		// Token: 0x04000C26 RID: 3110
		public string valueString;
	}
}
