using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	// Token: 0x0200078F RID: 1935
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GroupStyle : INotifyPropertyChanged
	{
		// Token: 0x06006B72 RID: 27506 RVA: 0x002C60D0 File Offset: 0x002C50D0
		static GroupStyle()
		{
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
			itemsPanelTemplate.Seal();
			GroupStyle.DefaultGroupPanel = itemsPanelTemplate;
			GroupStyle.DefaultStackPanel = itemsPanelTemplate;
			ItemsPanelTemplate itemsPanelTemplate2 = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
			itemsPanelTemplate2.Seal();
			GroupStyle.DefaultVirtualizingStackPanel = itemsPanelTemplate2;
			GroupStyle.s_DefaultGroupStyle = new GroupStyle();
		}

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x06006B73 RID: 27507 RVA: 0x002C612B File Offset: 0x002C512B
		// (remove) Token: 0x06006B74 RID: 27508 RVA: 0x002C6134 File Offset: 0x002C5134
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		// Token: 0x1400011D RID: 285
		// (add) Token: 0x06006B75 RID: 27509 RVA: 0x002C6140 File Offset: 0x002C5140
		// (remove) Token: 0x06006B76 RID: 27510 RVA: 0x002C6178 File Offset: 0x002C5178
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06006B77 RID: 27511 RVA: 0x002C61AD File Offset: 0x002C51AD
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x06006B78 RID: 27512 RVA: 0x002C61C4 File Offset: 0x002C51C4
		// (set) Token: 0x06006B79 RID: 27513 RVA: 0x002C61CC File Offset: 0x002C51CC
		public ItemsPanelTemplate Panel
		{
			get
			{
				return this._panel;
			}
			set
			{
				this._panel = value;
				this.OnPropertyChanged("Panel");
			}
		}

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x06006B7A RID: 27514 RVA: 0x002C61E0 File Offset: 0x002C51E0
		// (set) Token: 0x06006B7B RID: 27515 RVA: 0x002C61E8 File Offset: 0x002C51E8
		[DefaultValue(null)]
		public Style ContainerStyle
		{
			get
			{
				return this._containerStyle;
			}
			set
			{
				this._containerStyle = value;
				this.OnPropertyChanged("ContainerStyle");
			}
		}

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x06006B7C RID: 27516 RVA: 0x002C61FC File Offset: 0x002C51FC
		// (set) Token: 0x06006B7D RID: 27517 RVA: 0x002C6204 File Offset: 0x002C5204
		[DefaultValue(null)]
		public StyleSelector ContainerStyleSelector
		{
			get
			{
				return this._containerStyleSelector;
			}
			set
			{
				this._containerStyleSelector = value;
				this.OnPropertyChanged("ContainerStyleSelector");
			}
		}

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x06006B7E RID: 27518 RVA: 0x002C6218 File Offset: 0x002C5218
		// (set) Token: 0x06006B7F RID: 27519 RVA: 0x002C6220 File Offset: 0x002C5220
		[DefaultValue(null)]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return this._headerTemplate;
			}
			set
			{
				this._headerTemplate = value;
				this.OnPropertyChanged("HeaderTemplate");
			}
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x002C6234 File Offset: 0x002C5234
		// (set) Token: 0x06006B81 RID: 27521 RVA: 0x002C623C File Offset: 0x002C523C
		[DefaultValue(null)]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return this._headerTemplateSelector;
			}
			set
			{
				this._headerTemplateSelector = value;
				this.OnPropertyChanged("HeaderTemplateSelector");
			}
		}

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x06006B82 RID: 27522 RVA: 0x002C6250 File Offset: 0x002C5250
		// (set) Token: 0x06006B83 RID: 27523 RVA: 0x002C6258 File Offset: 0x002C5258
		[DefaultValue(null)]
		public string HeaderStringFormat
		{
			get
			{
				return this._headerStringFormat;
			}
			set
			{
				this._headerStringFormat = value;
				this.OnPropertyChanged("HeaderStringFormat");
			}
		}

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x06006B84 RID: 27524 RVA: 0x002C626C File Offset: 0x002C526C
		// (set) Token: 0x06006B85 RID: 27525 RVA: 0x002C6274 File Offset: 0x002C5274
		[DefaultValue(false)]
		public bool HidesIfEmpty
		{
			get
			{
				return this._hidesIfEmpty;
			}
			set
			{
				this._hidesIfEmpty = value;
				this.OnPropertyChanged("HidesIfEmpty");
			}
		}

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x06006B86 RID: 27526 RVA: 0x002C6288 File Offset: 0x002C5288
		// (set) Token: 0x06006B87 RID: 27527 RVA: 0x002C6290 File Offset: 0x002C5290
		[DefaultValue(0)]
		public int AlternationCount
		{
			get
			{
				return this._alternationCount;
			}
			set
			{
				this._alternationCount = value;
				this._isAlternationCountSet = true;
				this.OnPropertyChanged("AlternationCount");
			}
		}

		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x06006B88 RID: 27528 RVA: 0x002C62AB File Offset: 0x002C52AB
		public static GroupStyle Default
		{
			get
			{
				return GroupStyle.s_DefaultGroupStyle;
			}
		}

		// Token: 0x06006B89 RID: 27529 RVA: 0x002C62B2 File Offset: 0x002C52B2
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x06006B8A RID: 27530 RVA: 0x002C62C0 File Offset: 0x002C52C0
		internal bool IsAlternationCountSet
		{
			get
			{
				return this._isAlternationCountSet;
			}
		}

		// Token: 0x040035AB RID: 13739
		public static readonly ItemsPanelTemplate DefaultGroupPanel;

		// Token: 0x040035AC RID: 13740
		private ItemsPanelTemplate _panel;

		// Token: 0x040035AD RID: 13741
		private Style _containerStyle;

		// Token: 0x040035AE RID: 13742
		private StyleSelector _containerStyleSelector;

		// Token: 0x040035AF RID: 13743
		private DataTemplate _headerTemplate;

		// Token: 0x040035B0 RID: 13744
		private DataTemplateSelector _headerTemplateSelector;

		// Token: 0x040035B1 RID: 13745
		private string _headerStringFormat;

		// Token: 0x040035B2 RID: 13746
		private bool _hidesIfEmpty;

		// Token: 0x040035B3 RID: 13747
		private bool _isAlternationCountSet;

		// Token: 0x040035B4 RID: 13748
		private int _alternationCount;

		// Token: 0x040035B5 RID: 13749
		private static GroupStyle s_DefaultGroupStyle;

		// Token: 0x040035B6 RID: 13750
		internal static ItemsPanelTemplate DefaultStackPanel;

		// Token: 0x040035B7 RID: 13751
		internal static ItemsPanelTemplate DefaultVirtualizingStackPanel;
	}
}
