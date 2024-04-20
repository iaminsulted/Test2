using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000208 RID: 520
	internal abstract class BindingWorker
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x0014BE6F File Offset: 0x0014AE6F
		protected BindingWorker(BindingExpression b)
		{
			this._bindingExpression = b;
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual Type SourcePropertyType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool CanUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x0014BE7E File Offset: 0x0014AE7E
		internal BindingExpression ParentBindingExpression
		{
			get
			{
				return this._bindingExpression;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x0014BE86 File Offset: 0x0014AE86
		internal Type TargetPropertyType
		{
			get
			{
				return this.TargetProperty.PropertyType;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsDBNullValidForUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual object SourceItem
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual string SourcePropertyName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void AttachDataItem()
		{
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void DetachDataItem()
		{
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual object RawValue()
		{
			return null;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void UpdateValue(object value)
		{
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void RefreshValue()
		{
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			return false;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnSourceInvalidation(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool IsPathCurrent()
		{
			return true;
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0014BE93 File Offset: 0x0014AE93
		protected Binding ParentBinding
		{
			get
			{
				return this.ParentBindingExpression.ParentBinding;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0014BEA0 File Offset: 0x0014AEA0
		protected bool IsDynamic
		{
			get
			{
				return this.ParentBindingExpression.IsDynamic;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0014BEAD File Offset: 0x0014AEAD
		internal bool IsReflective
		{
			get
			{
				return this.ParentBindingExpression.IsReflective;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x0014BEBA File Offset: 0x0014AEBA
		protected bool IgnoreSourcePropertyChange
		{
			get
			{
				return this.ParentBindingExpression.IgnoreSourcePropertyChange;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0014BEC7 File Offset: 0x0014AEC7
		protected object DataItem
		{
			get
			{
				return this.ParentBindingExpression.DataItem;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x0014BED4 File Offset: 0x0014AED4
		protected DependencyObject TargetElement
		{
			get
			{
				return this.ParentBindingExpression.TargetElement;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0014BEE1 File Offset: 0x0014AEE1
		protected DependencyProperty TargetProperty
		{
			get
			{
				return this.ParentBindingExpression.TargetProperty;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0014BEEE File Offset: 0x0014AEEE
		protected DataBindEngine Engine
		{
			get
			{
				return this.ParentBindingExpression.Engine;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0014BEFB File Offset: 0x0014AEFB
		protected Dispatcher Dispatcher
		{
			get
			{
				return this.ParentBindingExpression.Dispatcher;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x0014BF08 File Offset: 0x0014AF08
		// (set) Token: 0x06001303 RID: 4867 RVA: 0x0014BF15 File Offset: 0x0014AF15
		protected BindingStatusInternal Status
		{
			get
			{
				return this.ParentBindingExpression.StatusInternal;
			}
			set
			{
				this.ParentBindingExpression.SetStatus(value);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0014BF23 File Offset: 0x0014AF23
		protected void SetTransferIsPending(bool value)
		{
			this.ParentBindingExpression.IsTransferPending = value;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0014BF31 File Offset: 0x0014AF31
		internal bool HasValue(BindingWorker.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0014BF3F File Offset: 0x0014AF3F
		internal object GetValue(BindingWorker.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0014BF4E File Offset: 0x0014AF4E
		internal void SetValue(BindingWorker.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0014BF5D File Offset: 0x0014AF5D
		internal void SetValue(BindingWorker.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0014BF82 File Offset: 0x0014AF82
		internal void ClearValue(BindingWorker.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x04000B6B RID: 2923
		private BindingExpression _bindingExpression;

		// Token: 0x04000B6C RID: 2924
		private UncommonValueTable _values;

		// Token: 0x020009E5 RID: 2533
		internal enum Feature
		{
			// Token: 0x04003FF7 RID: 16375
			XmlWorker,
			// Token: 0x04003FF8 RID: 16376
			PendingGetValueRequest,
			// Token: 0x04003FF9 RID: 16377
			PendingSetValueRequest,
			// Token: 0x04003FFA RID: 16378
			LastFeatureId
		}
	}
}
