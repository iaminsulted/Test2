using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007EF RID: 2031
	public static class ToolTipService
	{
		// Token: 0x060075EB RID: 30187 RVA: 0x002EDCE8 File Offset: 0x002ECCE8
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static object GetToolTip(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(ToolTipService.ToolTipProperty);
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x002EDD03 File Offset: 0x002ECD03
		public static void SetToolTip(DependencyObject element, object value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.ToolTipProperty, value);
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x002EDD1F File Offset: 0x002ECD1F
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetHorizontalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ToolTipService.HorizontalOffsetProperty);
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x002EDD3F File Offset: 0x002ECD3F
		public static void SetHorizontalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.HorizontalOffsetProperty, value);
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x002EDD60 File Offset: 0x002ECD60
		[TypeConverter(typeof(LengthConverter))]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static double GetVerticalOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ToolTipService.VerticalOffsetProperty);
		}

		// Token: 0x060075F0 RID: 30192 RVA: 0x002EDD80 File Offset: 0x002ECD80
		public static void SetVerticalOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.VerticalOffsetProperty, value);
		}

		// Token: 0x060075F1 RID: 30193 RVA: 0x002EDDA1 File Offset: 0x002ECDA1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHasDropShadow(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.HasDropShadowProperty);
		}

		// Token: 0x060075F2 RID: 30194 RVA: 0x002EDDC1 File Offset: 0x002ECDC1
		public static void SetHasDropShadow(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.HasDropShadowProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060075F3 RID: 30195 RVA: 0x002EDDE2 File Offset: 0x002ECDE2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static UIElement GetPlacementTarget(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (UIElement)element.GetValue(ToolTipService.PlacementTargetProperty);
		}

		// Token: 0x060075F4 RID: 30196 RVA: 0x002EDE02 File Offset: 0x002ECE02
		public static void SetPlacementTarget(DependencyObject element, UIElement value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.PlacementTargetProperty, value);
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x002EDE1E File Offset: 0x002ECE1E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static Rect GetPlacementRectangle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Rect)element.GetValue(ToolTipService.PlacementRectangleProperty);
		}

		// Token: 0x060075F6 RID: 30198 RVA: 0x002EDE3E File Offset: 0x002ECE3E
		public static void SetPlacementRectangle(DependencyObject element, Rect value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.PlacementRectangleProperty, value);
		}

		// Token: 0x060075F7 RID: 30199 RVA: 0x002EDE5F File Offset: 0x002ECE5F
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static PlacementMode GetPlacement(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PlacementMode)element.GetValue(ToolTipService.PlacementProperty);
		}

		// Token: 0x060075F8 RID: 30200 RVA: 0x002EDE7F File Offset: 0x002ECE7F
		public static void SetPlacement(DependencyObject element, PlacementMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.PlacementProperty, value);
		}

		// Token: 0x060075F9 RID: 30201 RVA: 0x002EDEA0 File Offset: 0x002ECEA0
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetShowOnDisabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.ShowOnDisabledProperty);
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x002EDEC0 File Offset: 0x002ECEC0
		public static void SetShowOnDisabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.ShowOnDisabledProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060075FB RID: 30203 RVA: 0x002EDEE1 File Offset: 0x002ECEE1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsOpen(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.IsOpenProperty);
		}

		// Token: 0x060075FC RID: 30204 RVA: 0x002EDF01 File Offset: 0x002ECF01
		private static void SetIsOpen(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.IsOpenPropertyKey, BooleanBoxes.Box(value));
		}

		// Token: 0x060075FD RID: 30205 RVA: 0x002EDF22 File Offset: 0x002ECF22
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolTipService.IsEnabledProperty);
		}

		// Token: 0x060075FE RID: 30206 RVA: 0x002EDF42 File Offset: 0x002ECF42
		public static void SetIsEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.IsEnabledProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool PositiveValueValidation(object o)
		{
			return (int)o >= 0;
		}

		// Token: 0x06007600 RID: 30208 RVA: 0x002EDF63 File Offset: 0x002ECF63
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetShowDuration(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ToolTipService.ShowDurationProperty);
		}

		// Token: 0x06007601 RID: 30209 RVA: 0x002EDF83 File Offset: 0x002ECF83
		public static void SetShowDuration(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.ShowDurationProperty, value);
		}

		// Token: 0x06007602 RID: 30210 RVA: 0x002EDFA4 File Offset: 0x002ECFA4
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetInitialShowDelay(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ToolTipService.InitialShowDelayProperty);
		}

		// Token: 0x06007603 RID: 30211 RVA: 0x002EDFC4 File Offset: 0x002ECFC4
		public static void SetInitialShowDelay(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.InitialShowDelayProperty, value);
		}

		// Token: 0x06007604 RID: 30212 RVA: 0x002EDFE5 File Offset: 0x002ECFE5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetBetweenShowDelay(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ToolTipService.BetweenShowDelayProperty);
		}

		// Token: 0x06007605 RID: 30213 RVA: 0x002EE005 File Offset: 0x002ED005
		public static void SetBetweenShowDelay(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolTipService.BetweenShowDelayProperty, value);
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x002EE026 File Offset: 0x002ED026
		public static void AddToolTipOpeningHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.AddHandler(element, ToolTipService.ToolTipOpeningEvent, handler);
		}

		// Token: 0x06007607 RID: 30215 RVA: 0x002EE034 File Offset: 0x002ED034
		public static void RemoveToolTipOpeningHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.RemoveHandler(element, ToolTipService.ToolTipOpeningEvent, handler);
		}

		// Token: 0x06007608 RID: 30216 RVA: 0x002EE042 File Offset: 0x002ED042
		public static void AddToolTipClosingHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.AddHandler(element, ToolTipService.ToolTipClosingEvent, handler);
		}

		// Token: 0x06007609 RID: 30217 RVA: 0x002EE050 File Offset: 0x002ED050
		public static void RemoveToolTipClosingHandler(DependencyObject element, ToolTipEventHandler handler)
		{
			UIElement.RemoveHandler(element, ToolTipService.ToolTipClosingEvent, handler);
		}

		// Token: 0x0600760A RID: 30218 RVA: 0x002EE060 File Offset: 0x002ED060
		static ToolTipService()
		{
			EventManager.RegisterClassHandler(typeof(UIElement), ToolTipService.FindToolTipEvent, new FindToolTipEventHandler(ToolTipService.OnFindToolTip));
			EventManager.RegisterClassHandler(typeof(ContentElement), ToolTipService.FindToolTipEvent, new FindToolTipEventHandler(ToolTipService.OnFindToolTip));
			EventManager.RegisterClassHandler(typeof(UIElement3D), ToolTipService.FindToolTipEvent, new FindToolTipEventHandler(ToolTipService.OnFindToolTip));
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x002EE3D4 File Offset: 0x002ED3D4
		private static void OnFindToolTip(object sender, FindToolTipEventArgs e)
		{
			if (e.TargetElement == null)
			{
				DependencyObject dependencyObject = sender as DependencyObject;
				if (dependencyObject != null)
				{
					if (e.TriggerAction != ToolTip.ToolTipTrigger.KeyboardShortcut && PopupControlService.Current.StopLookingForToolTip(dependencyObject))
					{
						e.Handled = true;
						e.KeepCurrentActive = true;
						return;
					}
					if (ToolTipService.ToolTipIsEnabled(dependencyObject, e.TriggerAction))
					{
						e.TargetElement = dependencyObject;
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x002EE434 File Offset: 0x002ED434
		private static bool ToolTipIsEnabled(DependencyObject o, ToolTip.ToolTipTrigger triggerAction)
		{
			object toolTip = ToolTipService.GetToolTip(o);
			if (toolTip != null && ToolTipService.GetIsEnabled(o))
			{
				ToolTip toolTip2 = toolTip as ToolTip;
				bool flag = toolTip2 == null || toolTip2.ShouldShowOnKeyboardFocus;
				if ((PopupControlService.IsElementEnabled(o) || ToolTipService.GetShowOnDisabled(o)) && (triggerAction != ToolTip.ToolTipTrigger.KeyboardFocus || flag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003874 RID: 14452
		public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached("ToolTip", typeof(object), typeof(ToolTipService), new FrameworkPropertyMetadata(null));

		// Token: 0x04003875 RID: 14453
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ToolTipService), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003876 RID: 14454
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ToolTipService), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04003877 RID: 14455
		public static readonly DependencyProperty HasDropShadowProperty = DependencyProperty.RegisterAttached("HasDropShadow", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003878 RID: 14456
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget", typeof(UIElement), typeof(ToolTipService), new FrameworkPropertyMetadata(null));

		// Token: 0x04003879 RID: 14457
		public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.RegisterAttached("PlacementRectangle", typeof(Rect), typeof(ToolTipService), new FrameworkPropertyMetadata(Rect.Empty));

		// Token: 0x0400387A RID: 14458
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(PlacementMode), typeof(ToolTipService), new FrameworkPropertyMetadata(PlacementMode.Mouse));

		// Token: 0x0400387B RID: 14459
		public static readonly DependencyProperty ShowOnDisabledProperty = DependencyProperty.RegisterAttached("ShowOnDisabled", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400387C RID: 14460
		private static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsOpen", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400387D RID: 14461
		public static readonly DependencyProperty IsOpenProperty = ToolTipService.IsOpenPropertyKey.DependencyProperty;

		// Token: 0x0400387E RID: 14462
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ToolTipService), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x0400387F RID: 14463
		public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDuration", typeof(int), typeof(ToolTipService), new FrameworkPropertyMetadata(5000), new ValidateValueCallback(ToolTipService.PositiveValueValidation));

		// Token: 0x04003880 RID: 14464
		public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay", typeof(int), typeof(ToolTipService), new FrameworkPropertyMetadata(SystemParameters.MouseHoverTimeMilliseconds), new ValidateValueCallback(ToolTipService.PositiveValueValidation));

		// Token: 0x04003881 RID: 14465
		public static readonly DependencyProperty BetweenShowDelayProperty = DependencyProperty.RegisterAttached("BetweenShowDelay", typeof(int), typeof(ToolTipService), new FrameworkPropertyMetadata(100), new ValidateValueCallback(ToolTipService.PositiveValueValidation));

		// Token: 0x04003882 RID: 14466
		public static readonly RoutedEvent ToolTipOpeningEvent = EventManager.RegisterRoutedEvent("ToolTipOpening", RoutingStrategy.Direct, typeof(ToolTipEventHandler), typeof(ToolTipService));

		// Token: 0x04003883 RID: 14467
		public static readonly RoutedEvent ToolTipClosingEvent = EventManager.RegisterRoutedEvent("ToolTipClosing", RoutingStrategy.Direct, typeof(ToolTipEventHandler), typeof(ToolTipService));

		// Token: 0x04003884 RID: 14468
		internal static readonly RoutedEvent FindToolTipEvent = EventManager.RegisterRoutedEvent("FindToolTip", RoutingStrategy.Bubble, typeof(FindToolTipEventHandler), typeof(ToolTipService));
	}
}
