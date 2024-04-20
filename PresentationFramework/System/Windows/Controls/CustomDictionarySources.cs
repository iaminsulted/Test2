using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MS.Internal.IO.Packaging;

namespace System.Windows.Controls
{
	// Token: 0x0200073C RID: 1852
	internal class CustomDictionarySources : IList<Uri>, ICollection<Uri>, IEnumerable<Uri>, IEnumerable, IList, ICollection
	{
		// Token: 0x06006228 RID: 25128 RVA: 0x0029EC2D File Offset: 0x0029DC2D
		internal CustomDictionarySources(TextBoxBase owner)
		{
			this._owner = owner;
			this._uriList = new List<Uri>();
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x0029EC47 File Offset: 0x0029DC47
		public IEnumerator<Uri> GetEnumerator()
		{
			return this._uriList.GetEnumerator();
		}

		// Token: 0x0600622A RID: 25130 RVA: 0x0029EC47 File Offset: 0x0029DC47
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._uriList.GetEnumerator();
		}

		// Token: 0x0600622B RID: 25131 RVA: 0x0029EC59 File Offset: 0x0029DC59
		int IList<Uri>.IndexOf(Uri item)
		{
			return this._uriList.IndexOf(item);
		}

		// Token: 0x0600622C RID: 25132 RVA: 0x0029EC68 File Offset: 0x0029DC68
		void IList<Uri>.Insert(int index, Uri item)
		{
			if (this._uriList.Contains(item))
			{
				throw new ArgumentException(SR.Get("CustomDictionaryItemAlreadyExists"), "item");
			}
			CustomDictionarySources.ValidateUri(item);
			this._uriList.Insert(index, item);
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriAdded(item);
			}
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x0029ECC0 File Offset: 0x0029DCC0
		void IList<Uri>.RemoveAt(int index)
		{
			Uri uri = this._uriList[index];
			this._uriList.RemoveAt(index);
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriRemoved(uri);
			}
		}

		// Token: 0x170016B5 RID: 5813
		Uri IList<Uri>.this[int index]
		{
			get
			{
				return this._uriList[index];
			}
			set
			{
				CustomDictionarySources.ValidateUri(value);
				Uri uri = this._uriList[index];
				if (this.Speller != null)
				{
					this.Speller.OnDictionaryUriRemoved(uri);
				}
				this._uriList[index] = value;
				if (this.Speller != null)
				{
					this.Speller.OnDictionaryUriAdded(value);
				}
			}
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x0029ED5D File Offset: 0x0029DD5D
		void ICollection<Uri>.Add(Uri item)
		{
			CustomDictionarySources.ValidateUri(item);
			if (!this._uriList.Contains(item))
			{
				this._uriList.Add(item);
			}
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriAdded(item);
			}
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x0029ED93 File Offset: 0x0029DD93
		void ICollection<Uri>.Clear()
		{
			this._uriList.Clear();
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriCollectionCleared();
			}
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x0029EDB3 File Offset: 0x0029DDB3
		bool ICollection<Uri>.Contains(Uri item)
		{
			return this._uriList.Contains(item);
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x0029EDC1 File Offset: 0x0029DDC1
		void ICollection<Uri>.CopyTo(Uri[] array, int arrayIndex)
		{
			this._uriList.CopyTo(array, arrayIndex);
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x06006234 RID: 25140 RVA: 0x0029EDD0 File Offset: 0x0029DDD0
		int ICollection<Uri>.Count
		{
			get
			{
				return this._uriList.Count;
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x06006235 RID: 25141 RVA: 0x0029EDDD File Offset: 0x0029DDDD
		bool ICollection<Uri>.IsReadOnly
		{
			get
			{
				return ((ICollection<Uri>)this._uriList).IsReadOnly;
			}
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x0029EDEA File Offset: 0x0029DDEA
		bool ICollection<Uri>.Remove(Uri item)
		{
			bool flag = this._uriList.Remove(item);
			if (flag && this.Speller != null)
			{
				this.Speller.OnDictionaryUriRemoved(item);
			}
			return flag;
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x0029EE0F File Offset: 0x0029DE0F
		int IList.Add(object value)
		{
			((ICollection<Uri>)this).Add((Uri)value);
			return this._uriList.IndexOf((Uri)value);
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x0029EE2E File Offset: 0x0029DE2E
		void IList.Clear()
		{
			((ICollection<Uri>)this).Clear();
		}

		// Token: 0x06006239 RID: 25145 RVA: 0x0029EE36 File Offset: 0x0029DE36
		bool IList.Contains(object value)
		{
			return ((IList)this._uriList).Contains(value);
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x0029EE44 File Offset: 0x0029DE44
		int IList.IndexOf(object value)
		{
			return ((IList)this._uriList).IndexOf(value);
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x0029EE52 File Offset: 0x0029DE52
		void IList.Insert(int index, object value)
		{
			((IList<Uri>)this).Insert(index, (Uri)value);
		}

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x0600623C RID: 25148 RVA: 0x0029EE61 File Offset: 0x0029DE61
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._uriList).IsFixedSize;
			}
		}

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x0600623D RID: 25149 RVA: 0x0029EE6E File Offset: 0x0029DE6E
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._uriList).IsReadOnly;
			}
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x0029EE7B File Offset: 0x0029DE7B
		void IList.Remove(object value)
		{
			((ICollection<Uri>)this).Remove((Uri)value);
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x0029EE8A File Offset: 0x0029DE8A
		void IList.RemoveAt(int index)
		{
			((IList<Uri>)this).RemoveAt(index);
		}

		// Token: 0x170016BA RID: 5818
		object IList.this[int index]
		{
			get
			{
				return this._uriList[index];
			}
			set
			{
				((IList<Uri>)this)[index] = (Uri)value;
			}
		}

		// Token: 0x06006242 RID: 25154 RVA: 0x0029EEA2 File Offset: 0x0029DEA2
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._uriList).CopyTo(array, index);
		}

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06006243 RID: 25155 RVA: 0x0029EEB1 File Offset: 0x0029DEB1
		int ICollection.Count
		{
			get
			{
				return ((ICollection<Uri>)this).Count;
			}
		}

		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06006244 RID: 25156 RVA: 0x0029EEB9 File Offset: 0x0029DEB9
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)this._uriList).IsSynchronized;
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06006245 RID: 25157 RVA: 0x0029EEC6 File Offset: 0x0029DEC6
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)this._uriList).SyncRoot;
			}
		}

		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06006246 RID: 25158 RVA: 0x0029EED3 File Offset: 0x0029DED3
		private Speller Speller
		{
			get
			{
				if (this._owner.TextEditor == null)
				{
					return null;
				}
				return this._owner.TextEditor.Speller;
			}
		}

		// Token: 0x06006247 RID: 25159 RVA: 0x0029EEF4 File Offset: 0x0029DEF4
		private static void ValidateUri(Uri item)
		{
			if (item == null)
			{
				throw new ArgumentException(SR.Get("CustomDictionaryNullItem"));
			}
			if (item.IsAbsoluteUri && !item.IsUnc && !item.IsFile && !PackUriHelper.IsPackUri(item))
			{
				throw new NotSupportedException(SR.Get("CustomDictionarySourcesUnsupportedURI"));
			}
		}

		// Token: 0x040032B0 RID: 12976
		private readonly List<Uri> _uriList;

		// Token: 0x040032B1 RID: 12977
		private readonly TextBoxBase _owner;
	}
}
