using System;
using System.Collections.ObjectModel;

namespace System.Windows
{
	// Token: 0x0200039A RID: 922
	public sealed class SetterBaseCollection : Collection<SetterBase>
	{
		// Token: 0x06002551 RID: 9553 RVA: 0x00186259 File Offset: 0x00185259
		protected override void ClearItems()
		{
			this.CheckSealed();
			base.ClearItems();
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00186267 File Offset: 0x00185267
		protected override void InsertItem(int index, SetterBase item)
		{
			this.CheckSealed();
			this.SetterBaseValidation(item);
			base.InsertItem(index, item);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x0018627E File Offset: 0x0018527E
		protected override void RemoveItem(int index)
		{
			this.CheckSealed();
			base.RemoveItem(index);
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x0018628D File Offset: 0x0018528D
		protected override void SetItem(int index, SetterBase item)
		{
			this.CheckSealed();
			this.SetterBaseValidation(item);
			base.SetItem(index, item);
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x001862A4 File Offset: 0x001852A4
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x001862AC File Offset: 0x001852AC
		internal void Seal()
		{
			this._sealed = true;
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Seal();
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x001862DD File Offset: 0x001852DD
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"SetterBaseCollection"
				}));
			}
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x00186305 File Offset: 0x00185305
		private void SetterBaseValidation(SetterBase setterBase)
		{
			if (setterBase == null)
			{
				throw new ArgumentNullException("setterBase");
			}
		}

		// Token: 0x04001185 RID: 4485
		private bool _sealed;
	}
}
