using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000235 RID: 565
	internal class ParameterCollection : Collection<object>, IList, ICollection, IEnumerable
	{
		// Token: 0x06001583 RID: 5507 RVA: 0x001555ED File Offset: 0x001545ED
		public ParameterCollection(ParameterCollectionChanged parametersChanged)
		{
			this._parametersChanged = parametersChanged;
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x001555FC File Offset: 0x001545FC
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00155604 File Offset: 0x00154604
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsFixedSize;
			}
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0015560C File Offset: 0x0015460C
		protected override void ClearItems()
		{
			this.CheckReadOnly();
			base.ClearItems();
			this.OnCollectionChanged();
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00155620 File Offset: 0x00154620
		protected override void InsertItem(int index, object value)
		{
			this.CheckReadOnly();
			base.InsertItem(index, value);
			this.OnCollectionChanged();
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00155636 File Offset: 0x00154636
		protected override void RemoveItem(int index)
		{
			this.CheckReadOnly();
			base.RemoveItem(index);
			this.OnCollectionChanged();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0015564B File Offset: 0x0015464B
		protected override void SetItem(int index, object value)
		{
			this.CheckReadOnly();
			base.SetItem(index, value);
			this.OnCollectionChanged();
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00155661 File Offset: 0x00154661
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x00155669 File Offset: 0x00154669
		protected virtual bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
			set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x001555FC File Offset: 0x001545FC
		protected bool IsFixedSize
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00155672 File Offset: 0x00154672
		internal void SetReadOnly(bool isReadOnly)
		{
			this.IsReadOnly = isReadOnly;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0015567B File Offset: 0x0015467B
		internal void ClearInternal()
		{
			base.ClearItems();
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00155683 File Offset: 0x00154683
		private void CheckReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ObjectDataProviderParameterCollectionIsNotInUse"));
			}
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0015569D File Offset: 0x0015469D
		private void OnCollectionChanged()
		{
			this._parametersChanged(this);
		}

		// Token: 0x04000C16 RID: 3094
		private bool _isReadOnly;

		// Token: 0x04000C17 RID: 3095
		private ParameterCollectionChanged _parametersChanged;
	}
}
