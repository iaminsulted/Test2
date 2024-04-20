using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Annotations;

namespace MS.Internal.Annotations
{
	// Token: 0x020002BA RID: 698
	internal sealed class AnnotationResourceCollection : AnnotationObservableCollection<AnnotationResource>
	{
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06001A22 RID: 6690 RVA: 0x001631F4 File Offset: 0x001621F4
		// (remove) Token: 0x06001A23 RID: 6691 RVA: 0x0016322C File Offset: 0x0016222C
		public event PropertyChangedEventHandler ItemChanged;

		// Token: 0x06001A24 RID: 6692 RVA: 0x00163264 File Offset: 0x00162264
		protected override void ProtectedClearItems()
		{
			List<AnnotationResource> list = new List<AnnotationResource>(this);
			base.Items.Clear();
			this.OnPropertyChanged(this.CountString);
			this.OnPropertyChanged(this.IndexerName);
			this.OnCollectionCleared(list);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x001632A2 File Offset: 0x001622A2
		protected override void ProtectedSetItem(int index, AnnotationResource item)
		{
			base.ObservableCollectionSetItem(index, item);
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x001632AC File Offset: 0x001622AC
		private void OnCollectionCleared(IEnumerable<AnnotationResource> list)
		{
			foreach (object changedItem in list)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, 0));
			}
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x001632FC File Offset: 0x001622FC
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0016330A File Offset: 0x0016230A
		protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.ItemChanged != null)
			{
				this.ItemChanged(sender, e);
			}
		}
	}
}
