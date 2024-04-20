using System;

namespace System.Windows.Baml2006
{
	// Token: 0x02000404 RID: 1028
	internal class OptimizedStaticResource
	{
		// Token: 0x06002C55 RID: 11349 RVA: 0x001A66F7 File Offset: 0x001A56F7
		public OptimizedStaticResource(byte flags, short keyId)
		{
			this._isType = ((flags & OptimizedStaticResource.TypeExtensionValueMask) > 0);
			this._isStatic = ((flags & OptimizedStaticResource.StaticExtensionValueMask) > 0);
			this.KeyId = keyId;
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x001A6726 File Offset: 0x001A5726
		// (set) Token: 0x06002C57 RID: 11351 RVA: 0x001A672E File Offset: 0x001A572E
		public short KeyId { get; set; }

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002C58 RID: 11352 RVA: 0x001A6737 File Offset: 0x001A5737
		// (set) Token: 0x06002C59 RID: 11353 RVA: 0x001A673F File Offset: 0x001A573F
		public object KeyValue { get; set; }

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002C5A RID: 11354 RVA: 0x001A6748 File Offset: 0x001A5748
		public bool IsKeyStaticExtension
		{
			get
			{
				return this._isStatic;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x001A6750 File Offset: 0x001A5750
		public bool IsKeyTypeExtension
		{
			get
			{
				return this._isType;
			}
		}

		// Token: 0x04001B23 RID: 6947
		private bool _isStatic;

		// Token: 0x04001B24 RID: 6948
		private bool _isType;

		// Token: 0x04001B25 RID: 6949
		private static readonly byte TypeExtensionValueMask = 1;

		// Token: 0x04001B26 RID: 6950
		private static readonly byte StaticExtensionValueMask = 2;
	}
}
