using System;

namespace System.Windows
{
	// Token: 0x0200039B RID: 923
	internal class SharedDp
	{
		// Token: 0x0600255A RID: 9562 RVA: 0x0018631D File Offset: 0x0018531D
		internal SharedDp(DependencyProperty dp, object value, string elementName)
		{
			this.Dp = dp;
			this.Value = value;
			this.ElementName = elementName;
		}

		// Token: 0x04001186 RID: 4486
		internal DependencyProperty Dp;

		// Token: 0x04001187 RID: 4487
		internal object Value;

		// Token: 0x04001188 RID: 4488
		internal string ElementName;
	}
}
