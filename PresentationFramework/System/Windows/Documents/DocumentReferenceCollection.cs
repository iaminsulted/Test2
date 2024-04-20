using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Windows.Documents
{
	// Token: 0x020005E5 RID: 1509
	[CLSCompliant(false)]
	public sealed class DocumentReferenceCollection : IEnumerable<DocumentReference>, IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x060048D8 RID: 18648 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal DocumentReferenceCollection()
		{
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x0022E593 File Offset: 0x0022D593
		public IEnumerator<DocumentReference> GetEnumerator()
		{
			return this._InternalList.GetEnumerator();
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0022E5A0 File Offset: 0x0022D5A0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DocumentReference>)this).GetEnumerator();
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0022E5A8 File Offset: 0x0022D5A8
		public void Add(DocumentReference item)
		{
			int count = this._InternalList.Count;
			this._InternalList.Add(item);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, count);
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x0022E5D6 File Offset: 0x0022D5D6
		public void CopyTo(DocumentReference[] array, int arrayIndex)
		{
			this._InternalList.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x060048DD RID: 18653 RVA: 0x0022E5E5 File Offset: 0x0022D5E5
		public int Count
		{
			get
			{
				return this._InternalList.Count;
			}
		}

		// Token: 0x1700105D RID: 4189
		public DocumentReference this[int index]
		{
			get
			{
				return this._InternalList[index];
			}
		}

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060048DF RID: 18655 RVA: 0x0022E600 File Offset: 0x0022D600
		// (remove) Token: 0x060048E0 RID: 18656 RVA: 0x0022E638 File Offset: 0x0022D638
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x060048E1 RID: 18657 RVA: 0x0022E66D File Offset: 0x0022D66D
		private IList<DocumentReference> _InternalList
		{
			get
			{
				if (this._internalList == null)
				{
					this._internalList = new List<DocumentReference>();
				}
				return this._internalList;
			}
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0022E688 File Offset: 0x0022D688
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			if (this.CollectionChanged != null)
			{
				NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(action, item, index);
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x04002650 RID: 9808
		private List<DocumentReference> _internalList;
	}
}
