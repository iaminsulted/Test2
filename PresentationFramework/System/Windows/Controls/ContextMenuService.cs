using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x02000737 RID: 1847
	public static class ContextMenuService
	{
		// Token: 0x060061C3 RID: 25027 RVA: 0x0029DC7C File Offset: 0x0029CC7C
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static ContextMenu GetContextMenu(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ContextMenu contextMenu = (ContextMenu)element.GetValue(ContextMenuService.ContextMenuProperty);
			if (contextMenu != null && element.Dispatcher != contextMenu.Dispatcher)
			{
				throw new ArgumentException(SR.Get("ContextMenuInDifferentDispatcher"));
			}
			return contextMenu;
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x0029DCCA File Offset: 0x0029CCCA
		public static void SetContextMenu(DependencyObject element, ContextMenu value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.ContextMenuProperty, value);
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x0029DCE6 File Offset: 0x0029CCE6
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetHorizontalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ContextMenuService.HorizontalOffsetProperty);
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x0029DD06 File Offset: 0x0029CD06
		public static void SetHorizontalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.HorizontalOffsetProperty, value);
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x0029DD27 File Offset: 0x0029CD27
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetVerticalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ContextMenuService.VerticalOffsetProperty);
		}

		// Token: 0x060061C8 RID: 25032 RVA: 0x0029DD47 File Offset: 0x0029CD47
		public static void SetVerticalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.VerticalOffsetProperty, value);
		}

		// Token: 0x060061C9 RID: 25033 RVA: 0x0029DD68 File Offset: 0x0029CD68
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHasDropShadow(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ContextMenuService.HasDropShadowProperty);
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x0029DD88 File Offset: 0x0029CD88
		public static void SetHasDropShadow(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.HasDropShadowProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060061CB RID: 25035 RVA: 0x0029DDA9 File Offset: 0x0029CDA9
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static UIElement GetPlacementTarget(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (UIElement)element.GetValue(ContextMenuService.PlacementTargetProperty);
		}

		// Token: 0x060061CC RID: 25036 RVA: 0x0029DDC9 File Offset: 0x0029CDC9
		public static void SetPlacementTarget(DependencyObject element, UIElement value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.PlacementTargetProperty, value);
		}

		// Token: 0x060061CD RID: 25037 RVA: 0x0029DDE5 File Offset: 0x0029CDE5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static Rect GetPlacementRectangle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Rect)element.GetValue(ContextMenuService.PlacementRectangleProperty);
		}

		// Token: 0x060061CE RID: 25038 RVA: 0x0029DE05 File Offset: 0x0029CE05
		public static void SetPlacementRectangle(DependencyObject element, Rect value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.PlacementRectangleProperty, value);
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x0029DE26 File Offset: 0x0029CE26
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static PlacementMode GetPlacement(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PlacementMode)element.GetValue(ContextMenuService.PlacementProperty);
		}

		// Token: 0x060061D0 RID: 25040 RVA: 0x0029DE46 File Offset: 0x0029CE46
		public static void SetPlacement(DependencyObject element, PlacementMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.PlacementProperty, value);
		}

		// Token: 0x060061D1 RID: 25041 RVA: 0x0029DE67 File Offset: 0x0029CE67
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetShowOnDisabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ContextMenuService.ShowOnDisabledProperty);
		}

		// Token: 0x060061D2 RID: 25042 RVA: 0x0029DE87 File Offset: 0x0029CE87
		public static void SetShowOnDisabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.ShowOnDisabledProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060061D3 RID: 25043 RVA: 0x0029DEA8 File Offset: 0x0029CEA8
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ContextMenuService.IsEnabledProperty);
		}

		// Token: 0x060061D4 RID: 25044 RVA: 0x0029DEC8 File Offset: 0x0029CEC8
		public static void SetIsEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ContextMenuService.IsEnabledProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060061D5 RID: 25045 RVA: 0x0029DEE9 File Offset: 0x0029CEE9
		public static void AddContextMenuOpeningHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.AddHandler(element, ContextMenuService.ContextMenuOpeningEvent, handler);
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x0029DEF7 File Offset: 0x0029CEF7
		public static void RemoveContextMenuOpeningHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.RemoveHandler(element, ContextMenuService.ContextMenuOpeningEvent, handler);
		}

		// Token: 0x060061D7 RID: 25047 RVA: 0x0029DF05 File Offset: 0x0029CF05
		public static void AddContextMenuClosingHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.AddHandler(element, ContextMenuService.ContextMenuClosingEvent, handler);
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x0029DF13 File Offset: 0x0029CF13
		public static void RemoveContextMenuClosingHandler(DependencyObject element, ContextMenuEventHandler handler)
		{
			UIElement.RemoveHandler(element, ContextMenuService.ContextMenuClosingEvent, handler);
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x0029DF24 File Offset: 0x0029CF24
		static ContextMenuService()
		{
			EventManager.RegisterClassHandler(typeof(UIElement), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ContextMenuService.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(ContentElement), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ContextMenuService.OnContextMenuOpening));
			EventManager.RegisterClassHandler(typeof(UIElement3D), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ContextMenuService.OnContextMenuOpening));
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x0029E180 File Offset: 0x0029D180
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			if (e.TargetElement == null)
			{
				DependencyObject dependencyObject = sender as DependencyObject;
				if (dependencyObject != null && ContextMenuService.ContextMenuIsEnabled(dependencyObject))
				{
					e.TargetElement = dependencyObject;
				}
			}
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x0029E1B0 File Offset: 0x0029D1B0
		internal static bool ContextMenuIsEnabled(DependencyObject o)
		{
			bool result = false;
			if (ContextMenuService.GetContextMenu(o) != null && ContextMenuService.GetIsEnabled(o) && (PopupControlService.IsElementEnabled(o) || ContextMenuService.GetShowOnDisabled(o)))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0400328E RID: 12942
		public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.RegisterAttached("ContextMenu", typeof(ContextMenu), typeof(ContextMenuService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

		// Token: 0x0400328F RID: 12943
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ContextMenuService), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003290 RID: 12944
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ContextMenuService), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003291 RID: 12945
		public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.RegisterAttached("HasDropShadow", typeof(bool), typeof(ContextMenuService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003292 RID: 12946
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget", typeof(UIElement), typeof(ContextMenuService), new FrameworkPropertyMetadata(null));

		// Token: 0x04003293 RID: 12947
		public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.RegisterAttached("PlacementRectangle", typeof(Rect), typeof(ContextMenuService), new FrameworkPropertyMetadata(Rect.Empty));

		// Token: 0x04003294 RID: 12948
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(PlacementMode), typeof(ContextMenuService), new FrameworkPropertyMetadata(PlacementMode.MousePoint));

		// Token: 0x04003295 RID: 12949
		public static readonly DependencyProperty ShowOnDisabledProperty = DependencyProperty.RegisterAttached("ShowOnDisabled", typeof(bool), typeof(ContextMenuService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003296 RID: 12950
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ContextMenuService), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x04003297 RID: 12951
		public static readonly RoutedEvent ContextMenuOpeningEvent = EventManager.RegisterRoutedEvent("ContextMenuOpening", RoutingStrategy.Bubble, typeof(ContextMenuEventHandler), typeof(ContextMenuService));

		// Token: 0x04003298 RID: 12952
		public static readonly RoutedEvent ContextMenuClosingEvent = EventManager.RegisterRoutedEvent("ContextMenuClosing", RoutingStrategy.Bubble, typeof(ContextMenuEventHandler), typeof(ContextMenuService));
	}
}
