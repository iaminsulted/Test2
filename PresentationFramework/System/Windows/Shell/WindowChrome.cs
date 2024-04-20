using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Standard;

namespace System.Windows.Shell
{
	// Token: 0x020003F6 RID: 1014
	public class WindowChrome : Freezable
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06002B81 RID: 11137 RVA: 0x001A29D5 File Offset: 0x001A19D5
		public static Thickness GlassFrameCompleteThickness
		{
			get
			{
				return new Thickness(-1.0);
			}
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x001A29E8 File Offset: 0x001A19E8
		private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return;
			}
			Window window = (Window)d;
			WindowChrome windowChrome = (WindowChrome)e.NewValue;
			WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
			if (windowChromeWorker == null)
			{
				windowChromeWorker = new WindowChromeWorker();
				WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
			}
			windowChromeWorker.SetWindowChrome(windowChrome);
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x001A2A30 File Offset: 0x001A1A30
		public static WindowChrome GetWindowChrome(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChrome)window.GetValue(WindowChrome.WindowChromeProperty);
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x001A2A4D File Offset: 0x001A1A4D
		public static void SetWindowChrome(Window window, WindowChrome chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChrome.WindowChromeProperty, chrome);
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x001A2A66 File Offset: 0x001A1A66
		public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (bool)dependencyObject.GetValue(WindowChrome.IsHitTestVisibleInChromeProperty);
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x001A2A9B File Offset: 0x001A1A9B
		public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			dependencyObject.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, hitTestVisible);
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x001A2ACC File Offset: 0x001A1ACC
		public static ResizeGripDirection GetResizeGripDirection(IInputElement inputElement)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (ResizeGripDirection)dependencyObject.GetValue(WindowChrome.ResizeGripDirectionProperty);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x001A2B01 File Offset: 0x001A1B01
		public static void SetResizeGripDirection(IInputElement inputElement, ResizeGripDirection direction)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			dependencyObject.SetValue(WindowChrome.ResizeGripDirectionProperty, direction);
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x001A2B37 File Offset: 0x001A1B37
		// (set) Token: 0x06002B8A RID: 11146 RVA: 0x001A2B49 File Offset: 0x001A1B49
		public double CaptionHeight
		{
			get
			{
				return (double)base.GetValue(WindowChrome.CaptionHeightProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CaptionHeightProperty, value);
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x001A2B5C File Offset: 0x001A1B5C
		// (set) Token: 0x06002B8C RID: 11148 RVA: 0x001A2B6E File Offset: 0x001A1B6E
		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.ResizeBorderThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.ResizeBorderThicknessProperty, value);
			}
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x001A2B81 File Offset: 0x001A1B81
		private static object _CoerceGlassFrameThickness(Thickness thickness)
		{
			if (!Utility.IsThicknessNonNegative(thickness))
			{
				return WindowChrome.GlassFrameCompleteThickness;
			}
			return thickness;
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06002B8E RID: 11150 RVA: 0x001A2B9C File Offset: 0x001A1B9C
		// (set) Token: 0x06002B8F RID: 11151 RVA: 0x001A2BAE File Offset: 0x001A1BAE
		public Thickness GlassFrameThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.GlassFrameThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.GlassFrameThicknessProperty, value);
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06002B90 RID: 11152 RVA: 0x001A2BC1 File Offset: 0x001A1BC1
		// (set) Token: 0x06002B91 RID: 11153 RVA: 0x001A2BD3 File Offset: 0x001A1BD3
		public bool UseAeroCaptionButtons
		{
			get
			{
				return (bool)base.GetValue(WindowChrome.UseAeroCaptionButtonsProperty);
			}
			set
			{
				base.SetValue(WindowChrome.UseAeroCaptionButtonsProperty, value);
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06002B92 RID: 11154 RVA: 0x001A2BE1 File Offset: 0x001A1BE1
		// (set) Token: 0x06002B93 RID: 11155 RVA: 0x001A2BF3 File Offset: 0x001A1BF3
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(WindowChrome.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CornerRadiusProperty, value);
			}
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x001A2C08 File Offset: 0x001A1C08
		private static bool _NonClientFrameEdgesAreValid(object value)
		{
			NonClientFrameEdges nonClientFrameEdges = NonClientFrameEdges.None;
			try
			{
				nonClientFrameEdges = (NonClientFrameEdges)value;
			}
			catch (InvalidCastException)
			{
				return false;
			}
			return nonClientFrameEdges == NonClientFrameEdges.None || ((nonClientFrameEdges | WindowChrome.NonClientFrameEdges_All) == WindowChrome.NonClientFrameEdges_All && nonClientFrameEdges != WindowChrome.NonClientFrameEdges_All);
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x001A2C58 File Offset: 0x001A1C58
		// (set) Token: 0x06002B96 RID: 11158 RVA: 0x001A2C6A File Offset: 0x001A1C6A
		public NonClientFrameEdges NonClientFrameEdges
		{
			get
			{
				return (NonClientFrameEdges)base.GetValue(WindowChrome.NonClientFrameEdgesProperty);
			}
			set
			{
				base.SetValue(WindowChrome.NonClientFrameEdgesProperty, value);
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x001A2C7D File Offset: 0x001A1C7D
		protected override Freezable CreateInstanceCore()
		{
			return new WindowChrome();
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x001A2C84 File Offset: 0x001A1C84
		public WindowChrome()
		{
			foreach (WindowChrome._SystemParameterBoundProperty systemParameterBoundProperty in WindowChrome._BoundProperties)
			{
				BindingOperations.SetBinding(this, systemParameterBoundProperty.DependencyProperty, new Binding
				{
					Path = new PropertyPath("(SystemParameters." + systemParameterBoundProperty.SystemParameterPropertyName + ")", Array.Empty<object>()),
					Mode = BindingMode.OneWay,
					UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
				});
			}
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x001A2D1C File Offset: 0x001A1D1C
		private void _OnPropertyChangedThatRequiresRepaint()
		{
			EventHandler propertyChangedThatRequiresRepaint = this.PropertyChangedThatRequiresRepaint;
			if (propertyChangedThatRequiresRepaint != null)
			{
				propertyChangedThatRequiresRepaint(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06002B9A RID: 11162 RVA: 0x001A2D40 File Offset: 0x001A1D40
		// (remove) Token: 0x06002B9B RID: 11163 RVA: 0x001A2D78 File Offset: 0x001A1D78
		internal event EventHandler PropertyChangedThatRequiresRepaint;

		// Token: 0x04001ADB RID: 6875
		public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));

		// Token: 0x04001ADC RID: 6876
		public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04001ADD RID: 6877
		public static readonly DependencyProperty ResizeGripDirectionProperty = DependencyProperty.RegisterAttached("ResizeGripDirection", typeof(ResizeGripDirection), typeof(WindowChrome), new FrameworkPropertyMetadata(ResizeGripDirection.None, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04001ADE RID: 6878
		public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => (double)value >= 0.0);

		// Token: 0x04001ADF RID: 6879
		public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness)), (object value) => Utility.IsThicknessNonNegative((Thickness)value));

		// Token: 0x04001AE0 RID: 6880
		public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}, (DependencyObject d, object o) => WindowChrome._CoerceGlassFrameThickness((Thickness)o)));

		// Token: 0x04001AE1 RID: 6881
		public static readonly DependencyProperty UseAeroCaptionButtonsProperty = DependencyProperty.Register("UseAeroCaptionButtons", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(true));

		// Token: 0x04001AE2 RID: 6882
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowChrome), new PropertyMetadata(default(CornerRadius), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => Utility.IsCornerRadiusValid((CornerRadius)value));

		// Token: 0x04001AE3 RID: 6883
		public static readonly DependencyProperty NonClientFrameEdgesProperty = DependencyProperty.Register("NonClientFrameEdges", typeof(NonClientFrameEdges), typeof(WindowChrome), new PropertyMetadata(NonClientFrameEdges.None, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), new ValidateValueCallback(WindowChrome._NonClientFrameEdgesAreValid));

		// Token: 0x04001AE4 RID: 6884
		private static readonly NonClientFrameEdges NonClientFrameEdges_All = NonClientFrameEdges.Left | NonClientFrameEdges.Top | NonClientFrameEdges.Right | NonClientFrameEdges.Bottom;

		// Token: 0x04001AE5 RID: 6885
		private static readonly List<WindowChrome._SystemParameterBoundProperty> _BoundProperties = new List<WindowChrome._SystemParameterBoundProperty>
		{
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.CornerRadiusProperty,
				SystemParameterPropertyName = "WindowCornerRadius"
			},
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.CaptionHeightProperty,
				SystemParameterPropertyName = "WindowCaptionHeight"
			},
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.ResizeBorderThicknessProperty,
				SystemParameterPropertyName = "WindowResizeBorderThickness"
			},
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.GlassFrameThicknessProperty,
				SystemParameterPropertyName = "WindowNonClientFrameThickness"
			}
		};

		// Token: 0x02000AA5 RID: 2725
		private struct _SystemParameterBoundProperty
		{
			// Token: 0x17001E56 RID: 7766
			// (get) Token: 0x0600871C RID: 34588 RVA: 0x0032C1EA File Offset: 0x0032B1EA
			// (set) Token: 0x0600871D RID: 34589 RVA: 0x0032C1F2 File Offset: 0x0032B1F2
			public string SystemParameterPropertyName { readonly get; set; }

			// Token: 0x17001E57 RID: 7767
			// (get) Token: 0x0600871E RID: 34590 RVA: 0x0032C1FB File Offset: 0x0032B1FB
			// (set) Token: 0x0600871F RID: 34591 RVA: 0x0032C203 File Offset: 0x0032B203
			public DependencyProperty DependencyProperty { readonly get; set; }
		}
	}
}
