using System;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x02000732 RID: 1842
	[Localizability(LocalizationCategory.ComboBox)]
	public class ComboBoxItem : ListBoxItem
	{
		// Token: 0x0600611F RID: 24863 RVA: 0x0029C2C0 File Offset: 0x0029B2C0
		static ComboBoxItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxItem), new FrameworkPropertyMetadata(typeof(ComboBoxItem)));
			ComboBoxItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ComboBoxItem));
		}

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x06006120 RID: 24864 RVA: 0x0029C340 File Offset: 0x0029B340
		// (set) Token: 0x06006121 RID: 24865 RVA: 0x0029C352 File Offset: 0x0029B352
		public bool IsHighlighted
		{
			get
			{
				return (bool)base.GetValue(ComboBoxItem.IsHighlightedProperty);
			}
			protected set
			{
				base.SetValue(ComboBoxItem.IsHighlightedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006122 RID: 24866 RVA: 0x0029C368 File Offset: 0x0029B368
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemMouseDown(this);
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x0029C394 File Offset: 0x0029B394
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemMouseUp(this);
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x06006124 RID: 24868 RVA: 0x0029C3C0 File Offset: 0x0029B3C0
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemEnter(this);
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x06006125 RID: 24869 RVA: 0x0029C3EC File Offset: 0x0029B3EC
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			ComboBox parentComboBox;
			if (base.IsSelected && (parentComboBox = this.ParentComboBox) != null)
			{
				parentComboBox.SelectedItemUpdated();
			}
			base.SetFlags(newContent is UIElement, VisualFlags.IsLayoutIslandRoot);
		}

		// Token: 0x06006126 RID: 24870 RVA: 0x0029C430 File Offset: 0x0029B430
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemEnter(this);
			}
			base.OnGotKeyboardFocus(e);
		}

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06006127 RID: 24871 RVA: 0x0029C45C File Offset: 0x0029B45C
		private ComboBox ParentComboBox
		{
			get
			{
				return base.ParentSelector as ComboBox;
			}
		}

		// Token: 0x06006128 RID: 24872 RVA: 0x0029C469 File Offset: 0x0029B469
		internal void SetIsHighlighted(bool isHighlighted)
		{
			this.IsHighlighted = isHighlighted;
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06006129 RID: 24873 RVA: 0x0029C472 File Offset: 0x0029B472
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ComboBoxItem._dType;
			}
		}

		// Token: 0x0400325E RID: 12894
		private static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(ComboBoxItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400325F RID: 12895
		public static readonly DependencyProperty IsHighlightedProperty = ComboBoxItem.IsHighlightedPropertyKey.DependencyProperty;

		// Token: 0x04003260 RID: 12896
		private static DependencyObjectType _dType;
	}
}
