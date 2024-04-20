using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000465 RID: 1125
	[ContentProperty("Bindings")]
	public class PriorityBinding : BindingBase, IAddChild
	{
		// Token: 0x060039D8 RID: 14808 RVA: 0x001EEED4 File Offset: 0x001EDED4
		public PriorityBinding()
		{
			this._bindingCollection = new BindingCollection(this, new BindingCollectionChangedCallback(this.OnBindingCollectionChanged));
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x001EEEF4 File Offset: 0x001EDEF4
		void IAddChild.AddChild(object value)
		{
			BindingBase bindingBase = value as BindingBase;
			if (bindingBase != null)
			{
				this.Bindings.Add(bindingBase);
				return;
			}
			throw new ArgumentException(SR.Get("ChildHasWrongType", new object[]
			{
				base.GetType().Name,
				"BindingBase",
				value.GetType().FullName
			}), "value");
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060039DB RID: 14811 RVA: 0x001EEF56 File Offset: 0x001EDF56
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<BindingBase> Bindings
		{
			get
			{
				return this._bindingCollection;
			}
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x001EEF5E File Offset: 0x001EDF5E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBindings()
		{
			return this.Bindings != null && this.Bindings.Count > 0;
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x001EEF78 File Offset: 0x001EDF78
		internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
		{
			return PriorityBindingExpression.CreateBindingExpression(target, dp, this, owner);
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x001EEF83 File Offset: 0x001EDF83
		internal override BindingBase CreateClone()
		{
			return new PriorityBinding();
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x001EEF8C File Offset: 0x001EDF8C
		internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
		{
			PriorityBinding priorityBinding = (PriorityBinding)baseClone;
			for (int i = 0; i <= this._bindingCollection.Count; i++)
			{
				priorityBinding._bindingCollection.Add(this._bindingCollection[i].Clone(mode));
			}
			base.InitializeClone(baseClone, mode);
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x001ED313 File Offset: 0x001EC313
		private void OnBindingCollectionChanged()
		{
			base.CheckSealed();
		}

		// Token: 0x04001D80 RID: 7552
		private BindingCollection _bindingCollection;
	}
}
