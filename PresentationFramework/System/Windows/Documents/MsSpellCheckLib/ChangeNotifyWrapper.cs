using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006FC RID: 1788
	internal class ChangeNotifyWrapper<T> : IChangeNotifyWrapper<T>, IChangeNotifyWrapper, INotifyPropertyChanged
	{
		// Token: 0x06005DB9 RID: 23993 RVA: 0x0028D97E File Offset: 0x0028C97E
		internal ChangeNotifyWrapper(T value = default(T), bool shouldThrowInvalidCastException = false)
		{
			this.Value = value;
			this._shouldThrowInvalidCastException = shouldThrowInvalidCastException;
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x06005DBA RID: 23994 RVA: 0x0028D994 File Offset: 0x0028C994
		// (set) Token: 0x06005DBB RID: 23995 RVA: 0x0028D99C File Offset: 0x0028C99C
		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06005DBC RID: 23996 RVA: 0x0028D9C0 File Offset: 0x0028C9C0
		// (set) Token: 0x06005DBD RID: 23997 RVA: 0x0028D9D0 File Offset: 0x0028C9D0
		object IChangeNotifyWrapper.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				T value2 = default(T);
				try
				{
					value2 = (T)((object)value);
				}
				catch (InvalidCastException obj) when (!this._shouldThrowInvalidCastException)
				{
					return;
				}
				this.Value = value2;
			}
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06005DBE RID: 23998 RVA: 0x0028DA24 File Offset: 0x0028CA24
		// (remove) Token: 0x06005DBF RID: 23999 RVA: 0x0028DA5C File Offset: 0x0028CA5C
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04003167 RID: 12647
		private T _value;

		// Token: 0x04003168 RID: 12648
		private bool _shouldThrowInvalidCastException;
	}
}
