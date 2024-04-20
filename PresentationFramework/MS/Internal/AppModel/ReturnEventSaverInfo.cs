using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000297 RID: 663
	[Serializable]
	internal struct ReturnEventSaverInfo
	{
		// Token: 0x0600190A RID: 6410 RVA: 0x001626E0 File Offset: 0x001616E0
		internal ReturnEventSaverInfo(string delegateTypeName, string targetTypeName, string delegateMethodName, bool fSamePf)
		{
			this._delegateTypeName = delegateTypeName;
			this._targetTypeName = targetTypeName;
			this._delegateMethodName = delegateMethodName;
			this._delegateInSamePF = fSamePf;
		}

		// Token: 0x04000D7D RID: 3453
		internal string _delegateTypeName;

		// Token: 0x04000D7E RID: 3454
		internal string _targetTypeName;

		// Token: 0x04000D7F RID: 3455
		internal string _delegateMethodName;

		// Token: 0x04000D80 RID: 3456
		internal bool _delegateInSamePF;
	}
}
