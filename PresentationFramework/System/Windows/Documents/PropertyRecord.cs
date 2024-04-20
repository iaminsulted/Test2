using System;

namespace System.Windows.Documents
{
	// Token: 0x02000650 RID: 1616
	internal struct PropertyRecord
	{
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06005011 RID: 20497 RVA: 0x002453C5 File Offset: 0x002443C5
		// (set) Token: 0x06005012 RID: 20498 RVA: 0x002453CD File Offset: 0x002443CD
		internal DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this._property = value;
			}
		}

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06005013 RID: 20499 RVA: 0x002453D6 File Offset: 0x002443D6
		// (set) Token: 0x06005014 RID: 20500 RVA: 0x002453DE File Offset: 0x002443DE
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04002885 RID: 10373
		private DependencyProperty _property;

		// Token: 0x04002886 RID: 10374
		private object _value;
	}
}
