using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000204 RID: 516
	internal class BindingCollection : Collection<BindingBase>
	{
		// Token: 0x060012D5 RID: 4821 RVA: 0x0014BD06 File Offset: 0x0014AD06
		internal BindingCollection(BindingBase owner, BindingCollectionChangedCallback callback)
		{
			Invariant.Assert(owner != null && callback != null);
			this._owner = owner;
			this._collectionChangedCallback = callback;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0014BD2B File Offset: 0x0014AD2B
		private BindingCollection()
		{
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0014BD33 File Offset: 0x0014AD33
		protected override void ClearItems()
		{
			this._owner.CheckSealed();
			base.ClearItems();
			this.OnBindingCollectionChanged();
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0014BD4C File Offset: 0x0014AD4C
		protected override void RemoveItem(int index)
		{
			this._owner.CheckSealed();
			base.RemoveItem(index);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0014BD66 File Offset: 0x0014AD66
		protected override void InsertItem(int index, BindingBase item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateItem(item);
			this._owner.CheckSealed();
			base.InsertItem(index, item);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0014BD96 File Offset: 0x0014AD96
		protected override void SetItem(int index, BindingBase item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateItem(item);
			this._owner.CheckSealed();
			base.SetItem(index, item);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0014BDC6 File Offset: 0x0014ADC6
		private void ValidateItem(BindingBase binding)
		{
			if (!(binding is Binding))
			{
				throw new NotSupportedException(SR.Get("BindingCollectionContainsNonBinding", new object[]
				{
					binding.GetType().Name
				}));
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0014BDF4 File Offset: 0x0014ADF4
		private void OnBindingCollectionChanged()
		{
			if (this._collectionChangedCallback != null)
			{
				this._collectionChangedCallback();
			}
		}

		// Token: 0x04000B67 RID: 2919
		private BindingBase _owner;

		// Token: 0x04000B68 RID: 2920
		private BindingCollectionChangedCallback _collectionChangedCallback;
	}
}
