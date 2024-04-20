using System;

namespace System.Windows
{
	// Token: 0x02000371 RID: 881
	internal class ReadOnlyFrameworkPropertyMetadata : FrameworkPropertyMetadata
	{
		// Token: 0x06002389 RID: 9097 RVA: 0x0018011A File Offset: 0x0017F11A
		public ReadOnlyFrameworkPropertyMetadata(object defaultValue, GetReadOnlyValueCallback getValueCallback) : base(defaultValue)
		{
			this._getValueCallback = getValueCallback;
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x0018012A File Offset: 0x0017F12A
		internal override GetReadOnlyValueCallback GetReadOnlyValueCallback
		{
			get
			{
				return this._getValueCallback;
			}
		}

		// Token: 0x040010CE RID: 4302
		private GetReadOnlyValueCallback _getValueCallback;
	}
}
