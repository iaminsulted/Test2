using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000212 RID: 530
	internal interface IDataBindEngineClient
	{
		// Token: 0x06001414 RID: 5140
		void TransferValue();

		// Token: 0x06001415 RID: 5141
		void UpdateValue();

		// Token: 0x06001416 RID: 5142
		bool AttachToContext(bool lastChance);

		// Token: 0x06001417 RID: 5143
		void VerifySourceReference(bool lastChance);

		// Token: 0x06001418 RID: 5144
		void OnTargetUpdated();

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001419 RID: 5145
		DependencyObject TargetElement { get; }
	}
}
