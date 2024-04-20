using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000785 RID: 1925
	[ContentProperty("Header")]
	[StyleTypedProperty(Property = "HeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GridViewColumn : DependencyObject, INotifyPropertyChanged
	{
		// Token: 0x06006A5F RID: 27231 RVA: 0x002C1C9D File Offset: 0x002C0C9D
		public GridViewColumn()
		{
			this.ResetPrivateData();
			this._state = (double.IsNaN(this.Width) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth);
		}

		// Token: 0x06006A60 RID: 27232 RVA: 0x002C1CC2 File Offset: 0x002C0CC2
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridViewColumn", new object[]
			{
				base.GetType(),
				this.Header
			});
		}

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06006A61 RID: 27233 RVA: 0x002C1CE6 File Offset: 0x002C0CE6
		// (set) Token: 0x06006A62 RID: 27234 RVA: 0x002C1CF3 File Offset: 0x002C0CF3
		public object Header
		{
			get
			{
				return base.GetValue(GridViewColumn.HeaderProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderProperty, value);
			}
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x002C1D01 File Offset: 0x002C0D01
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((GridViewColumn)d).OnPropertyChanged(GridViewColumn.HeaderProperty.Name);
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06006A64 RID: 27236 RVA: 0x002C1D18 File Offset: 0x002C0D18
		// (set) Token: 0x06006A65 RID: 27237 RVA: 0x002C1D2A File Offset: 0x002C0D2A
		public Style HeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridViewColumn.HeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderContainerStyleProperty, value);
			}
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x002C1D38 File Offset: 0x002C0D38
		private static void OnHeaderContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((GridViewColumn)d).OnPropertyChanged(GridViewColumn.HeaderContainerStyleProperty.Name);
		}

		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06006A67 RID: 27239 RVA: 0x002C1D4F File Offset: 0x002C0D4F
		// (set) Token: 0x06006A68 RID: 27240 RVA: 0x002C1D61 File Offset: 0x002C0D61
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridViewColumn.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x002C1D70 File Offset: 0x002C0D70
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			Helper.CheckTemplateAndTemplateSelector("Header", GridViewColumn.HeaderTemplateProperty, GridViewColumn.HeaderTemplateSelectorProperty, gridViewColumn);
			gridViewColumn.OnPropertyChanged(GridViewColumn.HeaderTemplateProperty.Name);
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06006A6A RID: 27242 RVA: 0x002C1DA9 File Offset: 0x002C0DA9
		// (set) Token: 0x06006A6B RID: 27243 RVA: 0x002C1DBB File Offset: 0x002C0DBB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridViewColumn.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x002C1DCC File Offset: 0x002C0DCC
		private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			Helper.CheckTemplateAndTemplateSelector("Header", GridViewColumn.HeaderTemplateProperty, GridViewColumn.HeaderTemplateSelectorProperty, gridViewColumn);
			gridViewColumn.OnPropertyChanged(GridViewColumn.HeaderTemplateSelectorProperty.Name);
		}

		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06006A6D RID: 27245 RVA: 0x002C1E05 File Offset: 0x002C0E05
		// (set) Token: 0x06006A6E RID: 27246 RVA: 0x002C1E17 File Offset: 0x002C0E17
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridViewColumn.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x002C1E25 File Offset: 0x002C0E25
		private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((GridViewColumn)d).OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x06006A70 RID: 27248 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat)
		{
		}

		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x06006A71 RID: 27249 RVA: 0x002C1E4A File Offset: 0x002C0E4A
		// (set) Token: 0x06006A72 RID: 27250 RVA: 0x002C1E52 File Offset: 0x002C0E52
		public BindingBase DisplayMemberBinding
		{
			get
			{
				return this._displayMemberBinding;
			}
			set
			{
				if (this._displayMemberBinding != value)
				{
					this._displayMemberBinding = value;
					this.OnDisplayMemberBindingChanged();
				}
			}
		}

		// Token: 0x06006A73 RID: 27251 RVA: 0x002C1E6A File Offset: 0x002C0E6A
		private void OnDisplayMemberBindingChanged()
		{
			this.OnPropertyChanged("DisplayMemberBinding");
		}

		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x06006A74 RID: 27252 RVA: 0x002C1E77 File Offset: 0x002C0E77
		// (set) Token: 0x06006A75 RID: 27253 RVA: 0x002C1E89 File Offset: 0x002C0E89
		public DataTemplate CellTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridViewColumn.CellTemplateProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.CellTemplateProperty, value);
			}
		}

		// Token: 0x06006A76 RID: 27254 RVA: 0x002C1E97 File Offset: 0x002C0E97
		private static void OnCellTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((GridViewColumn)d).OnPropertyChanged(GridViewColumn.CellTemplateProperty.Name);
		}

		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06006A77 RID: 27255 RVA: 0x002C1EAE File Offset: 0x002C0EAE
		// (set) Token: 0x06006A78 RID: 27256 RVA: 0x002C1EC0 File Offset: 0x002C0EC0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector CellTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridViewColumn.CellTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.CellTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006A79 RID: 27257 RVA: 0x002C1ECE File Offset: 0x002C0ECE
		private static void OnCellTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((GridViewColumn)d).OnPropertyChanged(GridViewColumn.CellTemplateSelectorProperty.Name);
		}

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06006A7A RID: 27258 RVA: 0x002C1EE5 File Offset: 0x002C0EE5
		// (set) Token: 0x06006A7B RID: 27259 RVA: 0x002C1EF7 File Offset: 0x002C0EF7
		[TypeConverter(typeof(LengthConverter))]
		public double Width
		{
			get
			{
				return (double)base.GetValue(GridViewColumn.WidthProperty);
			}
			set
			{
				base.SetValue(GridViewColumn.WidthProperty, value);
			}
		}

		// Token: 0x06006A7C RID: 27260 RVA: 0x002C1F0C File Offset: 0x002C0F0C
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = (GridViewColumn)d;
			double d2 = (double)e.NewValue;
			gridViewColumn.State = (double.IsNaN(d2) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth);
			gridViewColumn.OnPropertyChanged(GridViewColumn.WidthProperty.Name);
		}

		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x06006A7D RID: 27261 RVA: 0x002C1F4D File Offset: 0x002C0F4D
		// (set) Token: 0x06006A7E RID: 27262 RVA: 0x002C1F55 File Offset: 0x002C0F55
		public double ActualWidth
		{
			get
			{
				return this._actualWidth;
			}
			private set
			{
				if (!double.IsNaN(value) && !double.IsInfinity(value) && value >= 0.0 && this._actualWidth != value)
				{
					this._actualWidth = value;
					this.OnPropertyChanged("ActualWidth");
				}
			}
		}

		// Token: 0x14000118 RID: 280
		// (add) Token: 0x06006A7F RID: 27263 RVA: 0x002C1F8E File Offset: 0x002C0F8E
		// (remove) Token: 0x06006A80 RID: 27264 RVA: 0x002C1F97 File Offset: 0x002C0F97
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

		// Token: 0x14000119 RID: 281
		// (add) Token: 0x06006A81 RID: 27265 RVA: 0x002C1FA0 File Offset: 0x002C0FA0
		// (remove) Token: 0x06006A82 RID: 27266 RVA: 0x002C1FD8 File Offset: 0x002C0FD8
		private event PropertyChangedEventHandler _propertyChanged;

		// Token: 0x06006A83 RID: 27267 RVA: 0x002C200D File Offset: 0x002C100D
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, e);
			}
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x002C2024 File Offset: 0x002C1024
		internal void OnThemeChanged()
		{
			if (this.Header != null)
			{
				DependencyObject dependencyObject = this.Header as DependencyObject;
				if (dependencyObject != null)
				{
					FrameworkElement frameworkElement;
					FrameworkContentElement frameworkContentElement;
					Helper.DowncastToFEorFCE(dependencyObject, out frameworkElement, out frameworkContentElement, false);
					if (frameworkElement != null || frameworkContentElement != null)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, frameworkContentElement, ResourcesChangeInfo.ThemeChangeInfo);
					}
				}
			}
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x002C2065 File Offset: 0x002C1065
		internal double EnsureWidth(double width)
		{
			if (width > this.DesiredWidth)
			{
				this.DesiredWidth = width;
			}
			return this.DesiredWidth;
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x002C207D File Offset: 0x002C107D
		internal void ResetPrivateData()
		{
			this._actualIndex = -1;
			this._desiredWidth = 0.0;
			this._state = (double.IsNaN(this.Width) ? ColumnMeasureState.Init : ColumnMeasureState.SpecificWidth);
		}

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x06006A87 RID: 27271 RVA: 0x002C20AC File Offset: 0x002C10AC
		// (set) Token: 0x06006A88 RID: 27272 RVA: 0x002C20B4 File Offset: 0x002C10B4
		internal ColumnMeasureState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state == value)
				{
					if (value == ColumnMeasureState.SpecificWidth)
					{
						this.UpdateActualWidth();
					}
					return;
				}
				this._state = value;
				if (value != ColumnMeasureState.Init)
				{
					this.UpdateActualWidth();
					return;
				}
				this.DesiredWidth = 0.0;
			}
		}

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x06006A89 RID: 27273 RVA: 0x002C20EA File Offset: 0x002C10EA
		// (set) Token: 0x06006A8A RID: 27274 RVA: 0x002C20F2 File Offset: 0x002C10F2
		internal int ActualIndex
		{
			get
			{
				return this._actualIndex;
			}
			set
			{
				this._actualIndex = value;
			}
		}

		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x06006A8B RID: 27275 RVA: 0x002C20FB File Offset: 0x002C10FB
		// (set) Token: 0x06006A8C RID: 27276 RVA: 0x002C2103 File Offset: 0x002C1103
		internal double DesiredWidth
		{
			get
			{
				return this._desiredWidth;
			}
			private set
			{
				this._desiredWidth = value;
			}
		}

		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x06006A8D RID: 27277 RVA: 0x002C210C File Offset: 0x002C110C
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x002C2114 File Offset: 0x002C1114
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext == null && context != null)
			{
				this._inheritanceContext = context;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x002C2133 File Offset: 0x002C1133
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext == context)
			{
				this._inheritanceContext = null;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x002C2150 File Offset: 0x002C1150
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x002C215E File Offset: 0x002C115E
		private void UpdateActualWidth()
		{
			this.ActualWidth = ((this.State == ColumnMeasureState.SpecificWidth) ? this.Width : this.DesiredWidth);
		}

		// Token: 0x04003545 RID: 13637
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderChanged)));

		// Token: 0x04003546 RID: 13638
		public static readonly DependencyProperty HeaderContainerStyleProperty = DependencyProperty.Register("HeaderContainerStyle", typeof(Style), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderContainerStyleChanged)));

		// Token: 0x04003547 RID: 13639
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderTemplateChanged)));

		// Token: 0x04003548 RID: 13640
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(GridViewColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnHeaderTemplateSelectorChanged)));

		// Token: 0x04003549 RID: 13641
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(GridViewColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(GridViewColumn.OnHeaderStringFormatChanged)));

		// Token: 0x0400354A RID: 13642
		private BindingBase _displayMemberBinding;

		// Token: 0x0400354B RID: 13643
		internal const string c_DisplayMemberBindingName = "DisplayMemberBinding";

		// Token: 0x0400354C RID: 13644
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(GridViewColumn), new PropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnCellTemplateChanged)));

		// Token: 0x0400354D RID: 13645
		public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.Register("CellTemplateSelector", typeof(DataTemplateSelector), typeof(GridViewColumn), new PropertyMetadata(new PropertyChangedCallback(GridViewColumn.OnCellTemplateSelectorChanged)));

		// Token: 0x0400354E RID: 13646
		public static readonly DependencyProperty WidthProperty = FrameworkElement.WidthProperty.AddOwner(typeof(GridViewColumn), new PropertyMetadata(double.NaN, new PropertyChangedCallback(GridViewColumn.OnWidthChanged)));

		// Token: 0x04003550 RID: 13648
		internal const string c_ActualWidthName = "ActualWidth";

		// Token: 0x04003551 RID: 13649
		private DependencyObject _inheritanceContext;

		// Token: 0x04003552 RID: 13650
		private double _desiredWidth;

		// Token: 0x04003553 RID: 13651
		private int _actualIndex;

		// Token: 0x04003554 RID: 13652
		private double _actualWidth;

		// Token: 0x04003555 RID: 13653
		private ColumnMeasureState _state;
	}
}
