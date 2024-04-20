using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x02000802 RID: 2050
	[ContentProperty("Children")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class Viewport3D : FrameworkElement, IAddChild
	{
		// Token: 0x0600771B RID: 30491 RVA: 0x002F13D0 File Offset: 0x002F03D0
		static Viewport3D()
		{
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(Viewport3D), new PropertyMetadata(BooleanBoxes.TrueBox));
		}

		// Token: 0x0600771C RID: 30492 RVA: 0x002F1468 File Offset: 0x002F0468
		public Viewport3D()
		{
			this._viewport3DVisual = new Viewport3DVisual();
			this._viewport3DVisual.CanBeInheritanceContext = false;
			base.AddVisualChild(this._viewport3DVisual);
			base.SetValue(Viewport3D.ChildrenPropertyKey, this._viewport3DVisual.Children);
			this._viewport3DVisual.SetInheritanceContextForChildren(this);
		}

		// Token: 0x0600771D RID: 30493 RVA: 0x002F14C0 File Offset: 0x002F04C0
		private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Viewport3D viewport3D = (Viewport3D)d;
			if (!e.IsASubPropertyChange)
			{
				viewport3D._viewport3DVisual.Camera = (Camera)e.NewValue;
			}
		}

		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x0600771E RID: 30494 RVA: 0x002F14F4 File Offset: 0x002F04F4
		// (set) Token: 0x0600771F RID: 30495 RVA: 0x002F1506 File Offset: 0x002F0506
		public Camera Camera
		{
			get
			{
				return (Camera)base.GetValue(Viewport3D.CameraProperty);
			}
			set
			{
				base.SetValue(Viewport3D.CameraProperty, value);
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x06007720 RID: 30496 RVA: 0x002F1514 File Offset: 0x002F0514
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Visual3DCollection Children
		{
			get
			{
				return (Visual3DCollection)base.GetValue(Viewport3D.ChildrenProperty);
			}
		}

		// Token: 0x06007721 RID: 30497 RVA: 0x002F1526 File Offset: 0x002F0526
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new Viewport3DAutomationPeer(this);
		}

		// Token: 0x06007722 RID: 30498 RVA: 0x002F1530 File Offset: 0x002F0530
		protected override Size ArrangeOverride(Size finalSize)
		{
			Rect viewport = new Rect(default(Point), finalSize);
			this._viewport3DVisual.Viewport = viewport;
			return finalSize;
		}

		// Token: 0x06007723 RID: 30499 RVA: 0x002F155B File Offset: 0x002F055B
		protected override Visual GetVisualChild(int index)
		{
			if (index == 0)
			{
				return this._viewport3DVisual;
			}
			throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
		}

		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x06007724 RID: 30500 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007725 RID: 30501 RVA: 0x002F1584 File Offset: 0x002F0584
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Visual3D visual3D = value as Visual3D;
			if (visual3D == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(Visual3D)
				}), "value");
			}
			this.Children.Add(visual3D);
		}

		// Token: 0x06007726 RID: 30502 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x040038C7 RID: 14535
		public static readonly DependencyProperty CameraProperty = Viewport3DVisual.CameraProperty.AddOwner(typeof(Viewport3D), new FrameworkPropertyMetadata(FreezableOperations.GetAsFrozen(new PerspectiveCamera()), new PropertyChangedCallback(Viewport3D.OnCameraChanged)));

		// Token: 0x040038C8 RID: 14536
		private static readonly DependencyPropertyKey ChildrenPropertyKey = DependencyProperty.RegisterReadOnly("Children", typeof(Visual3DCollection), typeof(Viewport3D), new FrameworkPropertyMetadata(null));

		// Token: 0x040038C9 RID: 14537
		public static readonly DependencyProperty ChildrenProperty = Viewport3D.ChildrenPropertyKey.DependencyProperty;

		// Token: 0x040038CA RID: 14538
		private readonly Viewport3DVisual _viewport3DVisual;
	}
}
