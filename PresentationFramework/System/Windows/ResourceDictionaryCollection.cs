using System;
using System.Collections.ObjectModel;

namespace System.Windows
{
	// Token: 0x0200038E RID: 910
	internal class ResourceDictionaryCollection : ObservableCollection<ResourceDictionary>
	{
		// Token: 0x060024F5 RID: 9461 RVA: 0x0018540E File Offset: 0x0018440E
		internal ResourceDictionaryCollection(ResourceDictionary owner)
		{
			this._owner = owner;
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00185420 File Offset: 0x00184420
		protected override void ClearItems()
		{
			for (int i = 0; i < base.Count; i++)
			{
				this._owner.RemoveParentOwners(base[i]);
			}
			base.ClearItems();
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00185456 File Offset: 0x00184456
		protected override void InsertItem(int index, ResourceDictionary item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x0018546E File Offset: 0x0018446E
		protected override void SetItem(int index, ResourceDictionary item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x04001167 RID: 4455
		private ResourceDictionary _owner;
	}
}
