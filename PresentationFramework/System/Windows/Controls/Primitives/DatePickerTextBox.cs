using System;
using System.Windows.Data;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200082F RID: 2095
	[TemplatePart(Name = "PART_Watermark", Type = typeof(ContentControl))]
	public sealed class DatePickerTextBox : TextBox
	{
		// Token: 0x06007AC9 RID: 31433 RVA: 0x00309ADC File Offset: 0x00308ADC
		static DatePickerTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePickerTextBox), new FrameworkPropertyMetadata(typeof(DatePickerTextBox)));
			TextBox.TextProperty.OverrideMetadata(typeof(DatePickerTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x06007ACA RID: 31434 RVA: 0x00309B68 File Offset: 0x00308B68
		public DatePickerTextBox()
		{
			base.SetCurrentValue(DatePickerTextBox.WatermarkProperty, SR.Get("DatePickerTextBox_DefaultWatermarkText"));
			base.Loaded += this.OnLoaded;
			base.IsEnabledChanged += this.OnDatePickerTextBoxIsEnabledChanged;
		}

		// Token: 0x17001C6A RID: 7274
		// (get) Token: 0x06007ACB RID: 31435 RVA: 0x00309BB4 File Offset: 0x00308BB4
		// (set) Token: 0x06007ACC RID: 31436 RVA: 0x00309BC1 File Offset: 0x00308BC1
		internal object Watermark
		{
			get
			{
				return base.GetValue(DatePickerTextBox.WatermarkProperty);
			}
			set
			{
				base.SetValue(DatePickerTextBox.WatermarkProperty, value);
			}
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x00309BD0 File Offset: 0x00308BD0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.elementContent = this.ExtractTemplatePart<ContentControl>("PART_Watermark");
			if (this.elementContent != null)
			{
				Binding binding = new Binding("Watermark");
				binding.Source = this;
				this.elementContent.SetBinding(ContentControl.ContentProperty, binding);
			}
			this.OnWatermarkChanged();
		}

		// Token: 0x06007ACE RID: 31438 RVA: 0x00309C26 File Offset: 0x00308C26
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			if (base.IsEnabled && !string.IsNullOrEmpty(base.Text))
			{
				base.Select(0, base.Text.Length);
			}
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x00309C56 File Offset: 0x00308C56
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.ApplyTemplate();
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x00309C60 File Offset: 0x00308C60
		internal override void ChangeVisualState(bool useTransitions)
		{
			base.ChangeVisualState(useTransitions);
			if (this.Watermark != null && string.IsNullOrEmpty(base.Text))
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Watermarked",
					"Unwatermarked"
				});
				return;
			}
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Unwatermarked"
			});
		}

		// Token: 0x06007AD1 RID: 31441 RVA: 0x00309CBC File Offset: 0x00308CBC
		private T ExtractTemplatePart<T>(string partName) where T : DependencyObject
		{
			DependencyObject templateChild = base.GetTemplateChild(partName);
			return DatePickerTextBox.ExtractTemplatePart<T>(partName, templateChild);
		}

		// Token: 0x06007AD2 RID: 31442 RVA: 0x00309CD8 File Offset: 0x00308CD8
		private static T ExtractTemplatePart<T>(string partName, DependencyObject obj) where T : DependencyObject
		{
			return obj as T;
		}

		// Token: 0x06007AD3 RID: 31443 RVA: 0x00309CE8 File Offset: 0x00308CE8
		private void OnDatePickerTextBoxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.NewValue;
			base.SetCurrentValueInternal(TextBoxBase.IsReadOnlyProperty, BooleanBoxes.Box(!flag));
		}

		// Token: 0x06007AD4 RID: 31444 RVA: 0x00309D18 File Offset: 0x00308D18
		private void OnWatermarkChanged()
		{
			if (this.elementContent != null)
			{
				Control control = this.Watermark as Control;
				if (control != null)
				{
					control.IsTabStop = false;
					control.IsHitTestVisible = false;
				}
			}
		}

		// Token: 0x06007AD5 RID: 31445 RVA: 0x00309D4A File Offset: 0x00308D4A
		private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			DatePickerTextBox datePickerTextBox = sender as DatePickerTextBox;
			datePickerTextBox.OnWatermarkChanged();
			datePickerTextBox.UpdateVisualState();
		}

		// Token: 0x04003A1A RID: 14874
		private const string ElementContentName = "PART_Watermark";

		// Token: 0x04003A1B RID: 14875
		private ContentControl elementContent;

		// Token: 0x04003A1C RID: 14876
		internal static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(DatePickerTextBox), new PropertyMetadata(new PropertyChangedCallback(DatePickerTextBox.OnWatermarkPropertyChanged)));
	}
}
