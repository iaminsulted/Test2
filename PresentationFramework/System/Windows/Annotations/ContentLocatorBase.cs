using System;
using System.ComponentModel;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	// Token: 0x02000870 RID: 2160
	public abstract class ContentLocatorBase : INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		// Token: 0x06007F8E RID: 32654 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal ContentLocatorBase()
		{
		}

		// Token: 0x06007F8F RID: 32655
		public abstract object Clone();

		// Token: 0x14000169 RID: 361
		// (add) Token: 0x06007F90 RID: 32656 RVA: 0x0031F4B6 File Offset: 0x0031E4B6
		// (remove) Token: 0x06007F91 RID: 32657 RVA: 0x0031F4BF File Offset: 0x0031E4BF
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged += value;
			}
			remove
			{
				this._propertyChanged -= value;
			}
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x0031F4C8 File Offset: 0x0031E4C8
		internal void FireLocatorChanged(string name)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x17001D6A RID: 7530
		// (get) Token: 0x06007F93 RID: 32659 RVA: 0x0031F4E4 File Offset: 0x0031E4E4
		// (set) Token: 0x06007F94 RID: 32660 RVA: 0x0031F4EC File Offset: 0x0031E4EC
		bool IOwnedObject.Owned
		{
			get
			{
				return this._owned;
			}
			set
			{
				this._owned = value;
			}
		}

		// Token: 0x06007F95 RID: 32661
		internal abstract ContentLocatorBase Merge(ContentLocatorBase other);

		// Token: 0x1400016A RID: 362
		// (add) Token: 0x06007F96 RID: 32662 RVA: 0x0031F4F8 File Offset: 0x0031E4F8
		// (remove) Token: 0x06007F97 RID: 32663 RVA: 0x0031F530 File Offset: 0x0031E530
		private event PropertyChangedEventHandler _propertyChanged;

		// Token: 0x04003B8B RID: 15243
		private bool _owned;
	}
}
