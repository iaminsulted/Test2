using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x02000773 RID: 1907
	internal class DeferredSelectedIndexReference : DeferredReference
	{
		// Token: 0x060067B1 RID: 26545 RVA: 0x002B5E8D File Offset: 0x002B4E8D
		internal DeferredSelectedIndexReference(Selector selector)
		{
			this._selector = selector;
		}

		// Token: 0x060067B2 RID: 26546 RVA: 0x002B5E9C File Offset: 0x002B4E9C
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return this._selector.InternalSelectedIndex;
		}

		// Token: 0x060067B3 RID: 26547 RVA: 0x002B5EAE File Offset: 0x002B4EAE
		internal override Type GetValueType()
		{
			return typeof(int);
		}

		// Token: 0x04003451 RID: 13393
		private readonly Selector _selector;
	}
}
