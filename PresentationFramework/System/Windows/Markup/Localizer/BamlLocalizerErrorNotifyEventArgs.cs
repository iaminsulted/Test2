using System;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x02000543 RID: 1347
	public class BamlLocalizerErrorNotifyEventArgs : EventArgs
	{
		// Token: 0x06004296 RID: 17046 RVA: 0x0021BB58 File Offset: 0x0021AB58
		internal BamlLocalizerErrorNotifyEventArgs(BamlLocalizableResourceKey key, BamlLocalizerError error)
		{
			this._key = key;
			this._error = error;
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06004297 RID: 17047 RVA: 0x0021BB6E File Offset: 0x0021AB6E
		public BamlLocalizableResourceKey Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06004298 RID: 17048 RVA: 0x0021BB76 File Offset: 0x0021AB76
		public BamlLocalizerError Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x0400250F RID: 9487
		private BamlLocalizableResourceKey _key;

		// Token: 0x04002510 RID: 9488
		private BamlLocalizerError _error;
	}
}
