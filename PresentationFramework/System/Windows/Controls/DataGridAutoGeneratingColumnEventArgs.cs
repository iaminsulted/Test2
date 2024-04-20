using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	// Token: 0x0200073F RID: 1855
	public class DataGridAutoGeneratingColumnEventArgs : EventArgs
	{
		// Token: 0x06006421 RID: 25633 RVA: 0x002A7A57 File Offset: 0x002A6A57
		public DataGridAutoGeneratingColumnEventArgs(string propertyName, Type propertyType, DataGridColumn column) : this(column, propertyName, propertyType, null)
		{
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x002A7A63 File Offset: 0x002A6A63
		internal DataGridAutoGeneratingColumnEventArgs(DataGridColumn column, ItemPropertyInfo itemPropertyInfo) : this(column, itemPropertyInfo.Name, itemPropertyInfo.PropertyType, itemPropertyInfo.Descriptor)
		{
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x002A7A7E File Offset: 0x002A6A7E
		internal DataGridAutoGeneratingColumnEventArgs(DataGridColumn column, string propertyName, Type propertyType, object propertyDescriptor)
		{
			this._column = column;
			this._propertyName = propertyName;
			this._propertyType = propertyType;
			this.PropertyDescriptor = propertyDescriptor;
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06006424 RID: 25636 RVA: 0x002A7AA3 File Offset: 0x002A6AA3
		// (set) Token: 0x06006425 RID: 25637 RVA: 0x002A7AAB File Offset: 0x002A6AAB
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = value;
			}
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06006426 RID: 25638 RVA: 0x002A7AB4 File Offset: 0x002A6AB4
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x06006427 RID: 25639 RVA: 0x002A7ABC File Offset: 0x002A6ABC
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x06006428 RID: 25640 RVA: 0x002A7AC4 File Offset: 0x002A6AC4
		// (set) Token: 0x06006429 RID: 25641 RVA: 0x002A7ACC File Offset: 0x002A6ACC
		public object PropertyDescriptor
		{
			get
			{
				return this._propertyDescriptor;
			}
			private set
			{
				if (value == null)
				{
					this._propertyDescriptor = null;
					return;
				}
				this._propertyDescriptor = value;
			}
		}

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x0600642A RID: 25642 RVA: 0x002A7AE0 File Offset: 0x002A6AE0
		// (set) Token: 0x0600642B RID: 25643 RVA: 0x002A7AE8 File Offset: 0x002A6AE8
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x04003332 RID: 13106
		private DataGridColumn _column;

		// Token: 0x04003333 RID: 13107
		private string _propertyName;

		// Token: 0x04003334 RID: 13108
		private Type _propertyType;

		// Token: 0x04003335 RID: 13109
		private object _propertyDescriptor;

		// Token: 0x04003336 RID: 13110
		private bool _cancel;
	}
}
