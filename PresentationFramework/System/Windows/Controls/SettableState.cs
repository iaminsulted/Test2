using System;

namespace System.Windows.Controls
{
	// Token: 0x0200071A RID: 1818
	internal struct SettableState<T>
	{
		// Token: 0x06005F95 RID: 24469 RVA: 0x0029567C File Offset: 0x0029467C
		internal SettableState(T value)
		{
			this._value = value;
			this._isSet = (this._wasSet = false);
		}

		// Token: 0x040031D7 RID: 12759
		internal T _value;

		// Token: 0x040031D8 RID: 12760
		internal bool _isSet;

		// Token: 0x040031D9 RID: 12761
		internal bool _wasSet;
	}
}
