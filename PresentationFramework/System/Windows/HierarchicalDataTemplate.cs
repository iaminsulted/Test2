using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Windows
{
	// Token: 0x02000376 RID: 886
	public class HierarchicalDataTemplate : DataTemplate
	{
		// Token: 0x060023EA RID: 9194 RVA: 0x001813FF File Offset: 0x001803FF
		public HierarchicalDataTemplate()
		{
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00181407 File Offset: 0x00180407
		public HierarchicalDataTemplate(object dataType) : base(dataType)
		{
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x00181410 File Offset: 0x00180410
		// (set) Token: 0x060023ED RID: 9197 RVA: 0x00181418 File Offset: 0x00180418
		public BindingBase ItemsSource
		{
			get
			{
				return this._itemsSourceBinding;
			}
			set
			{
				base.CheckSealed();
				this._itemsSourceBinding = value;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x00181427 File Offset: 0x00180427
		// (set) Token: 0x060023EF RID: 9199 RVA: 0x0018142F File Offset: 0x0018042F
		public DataTemplate ItemTemplate
		{
			get
			{
				return this._itemTemplate;
			}
			set
			{
				base.CheckSealed();
				this._itemTemplate = value;
				this._itemTemplateSet = true;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x00181445 File Offset: 0x00180445
		// (set) Token: 0x060023F1 RID: 9201 RVA: 0x0018144D File Offset: 0x0018044D
		public DataTemplateSelector ItemTemplateSelector
		{
			get
			{
				return this._itemTemplateSelector;
			}
			set
			{
				base.CheckSealed();
				this._itemTemplateSelector = value;
				this._itemTemplateSelectorSet = true;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x00181463 File Offset: 0x00180463
		// (set) Token: 0x060023F3 RID: 9203 RVA: 0x0018146B File Offset: 0x0018046B
		public Style ItemContainerStyle
		{
			get
			{
				return this._itemContainerStyle;
			}
			set
			{
				base.CheckSealed();
				this._itemContainerStyle = value;
				this._itemContainerStyleSet = true;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x00181481 File Offset: 0x00180481
		// (set) Token: 0x060023F5 RID: 9205 RVA: 0x00181489 File Offset: 0x00180489
		public StyleSelector ItemContainerStyleSelector
		{
			get
			{
				return this._itemContainerStyleSelector;
			}
			set
			{
				base.CheckSealed();
				this._itemContainerStyleSelector = value;
				this._itemContainerStyleSelectorSet = true;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x0018149F File Offset: 0x0018049F
		// (set) Token: 0x060023F7 RID: 9207 RVA: 0x001814A7 File Offset: 0x001804A7
		public string ItemStringFormat
		{
			get
			{
				return this._itemStringFormat;
			}
			set
			{
				base.CheckSealed();
				this._itemStringFormat = value;
				this._itemStringFormatSet = true;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x001814BD File Offset: 0x001804BD
		// (set) Token: 0x060023F9 RID: 9209 RVA: 0x001814C5 File Offset: 0x001804C5
		public int AlternationCount
		{
			get
			{
				return this._alternationCount;
			}
			set
			{
				base.CheckSealed();
				this._alternationCount = value;
				this._alternationCountSet = true;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x001814DB File Offset: 0x001804DB
		// (set) Token: 0x060023FB RID: 9211 RVA: 0x001814E3 File Offset: 0x001804E3
		public BindingGroup ItemBindingGroup
		{
			get
			{
				return this._itemBindingGroup;
			}
			set
			{
				base.CheckSealed();
				this._itemBindingGroup = value;
				this._itemBindingGroupSet = true;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x001814F9 File Offset: 0x001804F9
		internal bool IsItemTemplateSet
		{
			get
			{
				return this._itemTemplateSet;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x00181501 File Offset: 0x00180501
		internal bool IsItemTemplateSelectorSet
		{
			get
			{
				return this._itemTemplateSelectorSet;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x00181509 File Offset: 0x00180509
		internal bool IsItemContainerStyleSet
		{
			get
			{
				return this._itemContainerStyleSet;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x00181511 File Offset: 0x00180511
		internal bool IsItemContainerStyleSelectorSet
		{
			get
			{
				return this._itemContainerStyleSelectorSet;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x00181519 File Offset: 0x00180519
		internal bool IsItemStringFormatSet
		{
			get
			{
				return this._itemStringFormatSet;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x00181521 File Offset: 0x00180521
		internal bool IsAlternationCountSet
		{
			get
			{
				return this._alternationCountSet;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x00181529 File Offset: 0x00180529
		internal bool IsItemBindingGroupSet
		{
			get
			{
				return this._itemBindingGroupSet;
			}
		}

		// Token: 0x040010F3 RID: 4339
		private BindingBase _itemsSourceBinding;

		// Token: 0x040010F4 RID: 4340
		private DataTemplate _itemTemplate;

		// Token: 0x040010F5 RID: 4341
		private DataTemplateSelector _itemTemplateSelector;

		// Token: 0x040010F6 RID: 4342
		private Style _itemContainerStyle;

		// Token: 0x040010F7 RID: 4343
		private StyleSelector _itemContainerStyleSelector;

		// Token: 0x040010F8 RID: 4344
		private string _itemStringFormat;

		// Token: 0x040010F9 RID: 4345
		private int _alternationCount;

		// Token: 0x040010FA RID: 4346
		private BindingGroup _itemBindingGroup;

		// Token: 0x040010FB RID: 4347
		private bool _itemTemplateSet;

		// Token: 0x040010FC RID: 4348
		private bool _itemTemplateSelectorSet;

		// Token: 0x040010FD RID: 4349
		private bool _itemContainerStyleSet;

		// Token: 0x040010FE RID: 4350
		private bool _itemContainerStyleSelectorSet;

		// Token: 0x040010FF RID: 4351
		private bool _itemStringFormatSet;

		// Token: 0x04001100 RID: 4352
		private bool _alternationCountSet;

		// Token: 0x04001101 RID: 4353
		private bool _itemBindingGroupSet;
	}
}
