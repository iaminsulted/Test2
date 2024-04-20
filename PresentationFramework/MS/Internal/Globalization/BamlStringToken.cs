using System;

namespace MS.Internal.Globalization
{
	// Token: 0x02000197 RID: 407
	internal struct BamlStringToken
	{
		// Token: 0x06000D82 RID: 3458 RVA: 0x0013578F File Offset: 0x0013478F
		internal BamlStringToken(BamlStringToken.TokenType type, string value)
		{
			this.Type = type;
			this.Value = value;
		}

		// Token: 0x040009E8 RID: 2536
		internal readonly BamlStringToken.TokenType Type;

		// Token: 0x040009E9 RID: 2537
		internal readonly string Value;

		// Token: 0x020009CA RID: 2506
		internal enum TokenType
		{
			// Token: 0x04003FA7 RID: 16295
			Text,
			// Token: 0x04003FA8 RID: 16296
			ChildPlaceHolder
		}
	}
}
