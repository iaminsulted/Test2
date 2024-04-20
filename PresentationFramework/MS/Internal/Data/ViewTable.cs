using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000246 RID: 582
	internal class ViewTable : HybridDictionary
	{
		// Token: 0x17000456 RID: 1110
		internal ViewRecord this[CollectionViewSource cvs]
		{
			get
			{
				return (ViewRecord)base[new WeakRefKey(cvs)];
			}
			set
			{
				base[new WeakRefKey(cvs)] = value;
			}
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x0015AB90 File Offset: 0x00159B90
		internal bool Purge()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				WeakRefKey weakRefKey = (WeakRefKey)dictionaryEntry.Key;
				if (weakRefKey.Target == null)
				{
					CollectionView collectionView = ((ViewRecord)dictionaryEntry.Value).View as CollectionView;
					if (collectionView != null)
					{
						if (!collectionView.IsInUse)
						{
							collectionView.DetachFromSourceCollection();
							arrayList.Add(weakRefKey);
						}
					}
					else
					{
						arrayList.Add(weakRefKey);
					}
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				base.Remove(arrayList[i]);
			}
			return arrayList.Count > 0 || base.Count == 0;
		}
	}
}
