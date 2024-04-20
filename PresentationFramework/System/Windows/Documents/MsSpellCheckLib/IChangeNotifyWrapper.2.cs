using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006FB RID: 1787
	internal interface IChangeNotifyWrapper<T> : IChangeNotifyWrapper, INotifyPropertyChanged
	{
		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x06005DB7 RID: 23991
		// (set) Token: 0x06005DB8 RID: 23992
		T Value { get; set; }
	}
}
