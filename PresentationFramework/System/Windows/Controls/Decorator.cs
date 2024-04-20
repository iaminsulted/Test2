using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Controls;

namespace System.Windows.Controls
{
	// Token: 0x02000771 RID: 1905
	[Localizability(LocalizationCategory.Ignore, Readability = Readability.Unreadable)]
	[ContentProperty("Child")]
	public class Decorator : FrameworkElement, IAddChild
	{
		// Token: 0x060067A3 RID: 26531 RVA: 0x002B5CE4 File Offset: 0x002B4CE4
		void IAddChild.AddChild(object value)
		{
			if (!(value is UIElement))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			if (this.Child != null)
			{
				throw new ArgumentException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					base.GetType(),
					value.GetType()
				}));
			}
			this.Child = (UIElement)value;
		}

		// Token: 0x060067A4 RID: 26532 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x060067A5 RID: 26533 RVA: 0x002B5D66 File Offset: 0x002B4D66
		// (set) Token: 0x060067A6 RID: 26534 RVA: 0x002B5D6E File Offset: 0x002B4D6E
		[DefaultValue(null)]
		public virtual UIElement Child
		{
			get
			{
				return this._child;
			}
			set
			{
				if (this._child != value)
				{
					base.RemoveVisualChild(this._child);
					base.RemoveLogicalChild(this._child);
					this._child = value;
					base.AddLogicalChild(value);
					base.AddVisualChild(value);
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x060067A7 RID: 26535 RVA: 0x002B5DAC File Offset: 0x002B4DAC
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this._child == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new SingleChildEnumerator(this._child);
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x060067A8 RID: 26536 RVA: 0x002B5DC7 File Offset: 0x002B4DC7
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x002B5DD4 File Offset: 0x002B4DD4
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		// Token: 0x060067AA RID: 26538 RVA: 0x002B5E04 File Offset: 0x002B4E04
		protected override Size MeasureOverride(Size constraint)
		{
			UIElement child = this.Child;
			if (child != null)
			{
				child.Measure(constraint);
				return child.DesiredSize;
			}
			return default(Size);
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x002B5E34 File Offset: 0x002B4E34
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement child = this.Child;
			if (child != null)
			{
				child.Arrange(new Rect(arrangeSize));
			}
			return arrangeSize;
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x060067AC RID: 26540 RVA: 0x002B5D66 File Offset: 0x002B4D66
		// (set) Token: 0x060067AD RID: 26541 RVA: 0x002B5E58 File Offset: 0x002B4E58
		internal UIElement IntChild
		{
			get
			{
				return this._child;
			}
			set
			{
				this._child = value;
			}
		}

		// Token: 0x0400344F RID: 13391
		private UIElement _child;
	}
}
