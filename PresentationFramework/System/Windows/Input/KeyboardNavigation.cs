using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Input
{
	// Token: 0x02000429 RID: 1065
	public sealed class KeyboardNavigation
	{
		// Token: 0x0600336C RID: 13164 RVA: 0x001D6C78 File Offset: 0x001D5C78
		internal KeyboardNavigation()
		{
			InputManager inputManager = InputManager.Current;
			inputManager.PostProcessInput += this.PostProcessInput;
			inputManager.TranslateAccelerator += this.TranslateAccelerator;
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001D6CF4 File Offset: 0x001D5CF4
		internal static DependencyObject GetTabOnceActiveElement(DependencyObject d)
		{
			WeakReference weakReference = (WeakReference)d.GetValue(KeyboardNavigation.TabOnceActiveElementProperty);
			if (weakReference != null && weakReference.IsAlive)
			{
				DependencyObject dependencyObject = weakReference.Target as DependencyObject;
				if (KeyboardNavigation.GetVisualRoot(dependencyObject) == KeyboardNavigation.GetVisualRoot(d))
				{
					return dependencyObject;
				}
				d.SetValue(KeyboardNavigation.TabOnceActiveElementProperty, null);
			}
			return null;
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x001D6D46 File Offset: 0x001D5D46
		internal static void SetTabOnceActiveElement(DependencyObject d, DependencyObject value)
		{
			d.SetValue(KeyboardNavigation.TabOnceActiveElementProperty, new WeakReference(value));
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x001D6D5C File Offset: 0x001D5D5C
		private static DependencyObject GetControlTabOnceActiveElement(DependencyObject d)
		{
			WeakReference weakReference = (WeakReference)d.GetValue(KeyboardNavigation.ControlTabOnceActiveElementProperty);
			if (weakReference != null && weakReference.IsAlive)
			{
				DependencyObject dependencyObject = weakReference.Target as DependencyObject;
				if (KeyboardNavigation.GetVisualRoot(dependencyObject) == KeyboardNavigation.GetVisualRoot(d))
				{
					return dependencyObject;
				}
				d.SetValue(KeyboardNavigation.ControlTabOnceActiveElementProperty, null);
			}
			return null;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x001D6DAE File Offset: 0x001D5DAE
		private static void SetControlTabOnceActiveElement(DependencyObject d, DependencyObject value)
		{
			d.SetValue(KeyboardNavigation.ControlTabOnceActiveElementProperty, new WeakReference(value));
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x001D6DC1 File Offset: 0x001D5DC1
		private DependencyObject GetActiveElement(DependencyObject d)
		{
			if (this._navigationProperty != KeyboardNavigation.ControlTabNavigationProperty)
			{
				return KeyboardNavigation.GetTabOnceActiveElement(d);
			}
			return KeyboardNavigation.GetControlTabOnceActiveElement(d);
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x001D6DDD File Offset: 0x001D5DDD
		private void SetActiveElement(DependencyObject d, DependencyObject value)
		{
			if (this._navigationProperty == KeyboardNavigation.TabNavigationProperty)
			{
				KeyboardNavigation.SetTabOnceActiveElement(d, value);
				return;
			}
			KeyboardNavigation.SetControlTabOnceActiveElement(d, value);
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x001D6DFC File Offset: 0x001D5DFC
		internal static Visual GetVisualRoot(DependencyObject d)
		{
			if (d is Visual || d is Visual3D)
			{
				PresentationSource presentationSource = PresentationSource.CriticalFromVisual(d);
				if (presentationSource != null)
				{
					return presentationSource.RootVisual;
				}
			}
			else
			{
				FrameworkContentElement frameworkContentElement = d as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return KeyboardNavigation.GetVisualRoot(frameworkContentElement.Parent);
				}
			}
			return null;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x001D6E41 File Offset: 0x001D5E41
		private static object CoerceShowKeyboardCues(DependencyObject d, object value)
		{
			if (!SystemParameters.KeyboardCues)
			{
				return value;
			}
			return BooleanBoxes.TrueBox;
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06003375 RID: 13173 RVA: 0x001D6E54 File Offset: 0x001D5E54
		// (remove) Token: 0x06003376 RID: 13174 RVA: 0x001D6E9C File Offset: 0x001D5E9C
		internal event KeyboardFocusChangedEventHandler FocusChanged
		{
			add
			{
				KeyboardNavigation.WeakReferenceList weakFocusChangedHandlers = this._weakFocusChangedHandlers;
				lock (weakFocusChangedHandlers)
				{
					this._weakFocusChangedHandlers.Add(value);
				}
			}
			remove
			{
				KeyboardNavigation.WeakReferenceList weakFocusChangedHandlers = this._weakFocusChangedHandlers;
				lock (weakFocusChangedHandlers)
				{
					this._weakFocusChangedHandlers.Remove(value);
				}
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x001D6EE4 File Offset: 0x001D5EE4
		internal void NotifyFocusChanged(object sender, KeyboardFocusChangedEventArgs e)
		{
			this._weakFocusChangedHandlers.Process(delegate(object item)
			{
				KeyboardFocusChangedEventHandler keyboardFocusChangedEventHandler = item as KeyboardFocusChangedEventHandler;
				if (keyboardFocusChangedEventHandler != null)
				{
					keyboardFocusChangedEventHandler(sender, e);
				}
				return false;
			});
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001D6F1C File Offset: 0x001D5F1C
		public static void SetTabIndex(DependencyObject element, int index)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.TabIndexProperty, index);
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x001D6F3D File Offset: 0x001D5F3D
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetTabIndex(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return KeyboardNavigation.GetTabIndexHelper(element);
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x001D6F53 File Offset: 0x001D5F53
		public static void SetIsTabStop(DependencyObject element, bool isTabStop)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.IsTabStopProperty, BooleanBoxes.Box(isTabStop));
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x001D6F74 File Offset: 0x001D5F74
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsTabStop(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(KeyboardNavigation.IsTabStopProperty);
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x001D6F94 File Offset: 0x001D5F94
		public static void SetTabNavigation(DependencyObject element, KeyboardNavigationMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.TabNavigationProperty, mode);
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x001D6FB5 File Offset: 0x001D5FB5
		[CustomCategory("Accessibility")]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static KeyboardNavigationMode GetTabNavigation(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (KeyboardNavigationMode)element.GetValue(KeyboardNavigation.TabNavigationProperty);
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x001D6FD5 File Offset: 0x001D5FD5
		public static void SetControlTabNavigation(DependencyObject element, KeyboardNavigationMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.ControlTabNavigationProperty, mode);
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x001D6FF6 File Offset: 0x001D5FF6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		[CustomCategory("Accessibility")]
		public static KeyboardNavigationMode GetControlTabNavigation(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (KeyboardNavigationMode)element.GetValue(KeyboardNavigation.ControlTabNavigationProperty);
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x001D7016 File Offset: 0x001D6016
		public static void SetDirectionalNavigation(DependencyObject element, KeyboardNavigationMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.DirectionalNavigationProperty, mode);
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x001D7037 File Offset: 0x001D6037
		[CustomCategory("Accessibility")]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static KeyboardNavigationMode GetDirectionalNavigation(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (KeyboardNavigationMode)element.GetValue(KeyboardNavigation.DirectionalNavigationProperty);
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x001D7057 File Offset: 0x001D6057
		public static void SetAcceptsReturn(DependencyObject element, bool enabled)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.AcceptsReturnProperty, BooleanBoxes.Box(enabled));
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x001D7078 File Offset: 0x001D6078
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetAcceptsReturn(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(KeyboardNavigation.AcceptsReturnProperty);
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x001D7098 File Offset: 0x001D6098
		private static bool IsValidKeyNavigationMode(object o)
		{
			KeyboardNavigationMode keyboardNavigationMode = (KeyboardNavigationMode)o;
			return keyboardNavigationMode == KeyboardNavigationMode.Contained || keyboardNavigationMode == KeyboardNavigationMode.Continue || keyboardNavigationMode == KeyboardNavigationMode.Cycle || keyboardNavigationMode == KeyboardNavigationMode.None || keyboardNavigationMode == KeyboardNavigationMode.Once || keyboardNavigationMode == KeyboardNavigationMode.Local;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x001D70C8 File Offset: 0x001D60C8
		internal static UIElement GetParentUIElementFromContentElement(ContentElement ce)
		{
			IContentHost contentHost = null;
			return KeyboardNavigation.GetParentUIElementFromContentElement(ce, ref contentHost);
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x001D70E0 File Offset: 0x001D60E0
		private static UIElement GetParentUIElementFromContentElement(ContentElement ce, ref IContentHost ichParent)
		{
			if (ce == null)
			{
				return null;
			}
			IContentHost contentHost = ContentHostHelper.FindContentHost(ce);
			if (ichParent == null)
			{
				ichParent = contentHost;
			}
			DependencyObject dependencyObject = contentHost as DependencyObject;
			if (dependencyObject != null)
			{
				UIElement uielement = dependencyObject as UIElement;
				if (uielement != null)
				{
					return uielement;
				}
				Visual visual = dependencyObject as Visual;
				while (visual != null)
				{
					visual = (VisualTreeHelper.GetParent(visual) as Visual);
					UIElement uielement2 = visual as UIElement;
					if (uielement2 != null)
					{
						return uielement2;
					}
				}
				ContentElement contentElement = dependencyObject as ContentElement;
				if (contentElement != null)
				{
					return KeyboardNavigation.GetParentUIElementFromContentElement(contentElement, ref ichParent);
				}
			}
			return null;
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x001D7154 File Offset: 0x001D6154
		internal void HideFocusVisual()
		{
			if (this._focusVisualAdornerCache != null)
			{
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this._focusVisualAdornerCache) as AdornerLayer;
				if (adornerLayer != null)
				{
					adornerLayer.Remove(this._focusVisualAdornerCache);
				}
				this._focusVisualAdornerCache = null;
			}
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x001D7190 File Offset: 0x001D6190
		internal static bool IsKeyboardMostRecentInputDevice()
		{
			return InputManager.Current.MostRecentInputDevice is KeyboardDevice;
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x001D71A4 File Offset: 0x001D61A4
		// (set) Token: 0x0600338A RID: 13194 RVA: 0x001D71AB File Offset: 0x001D61AB
		internal static bool AlwaysShowFocusVisual
		{
			get
			{
				return KeyboardNavigation._alwaysShowFocusVisual;
			}
			set
			{
				KeyboardNavigation._alwaysShowFocusVisual = value;
			}
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x001D71B3 File Offset: 0x001D61B3
		internal static void ShowFocusVisual()
		{
			KeyboardNavigation.Current.ShowFocusVisual(Keyboard.FocusedElement as DependencyObject);
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x001D71CC File Offset: 0x001D61CC
		private void ShowFocusVisual(DependencyObject element)
		{
			this.HideFocusVisual();
			if (!KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				KeyboardNavigation.EnableKeyboardCues(element, false);
			}
			if (KeyboardNavigation.AlwaysShowFocusVisual || KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				FrameworkElement frameworkElement = element as FrameworkElement;
				if (frameworkElement != null)
				{
					AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
					if (adornerLayer == null)
					{
						return;
					}
					Style style = frameworkElement.FocusVisualStyle;
					if (style == FrameworkElement.DefaultFocusVisualStyle)
					{
						style = (SystemResources.FindResourceInternal(SystemParameters.FocusVisualStyleKey) as Style);
					}
					if (style != null)
					{
						this._focusVisualAdornerCache = new KeyboardNavigation.FocusVisualAdorner(frameworkElement, style);
						adornerLayer.Add(this._focusVisualAdornerCache);
						return;
					}
				}
				else
				{
					FrameworkContentElement frameworkContentElement = element as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						IContentHost contentHost = null;
						UIElement parentUIElementFromContentElement = KeyboardNavigation.GetParentUIElementFromContentElement(frameworkContentElement, ref contentHost);
						if (contentHost != null && parentUIElementFromContentElement != null)
						{
							AdornerLayer adornerLayer2 = AdornerLayer.GetAdornerLayer(parentUIElementFromContentElement);
							if (adornerLayer2 != null)
							{
								Style style2 = frameworkContentElement.FocusVisualStyle;
								if (style2 == FrameworkElement.DefaultFocusVisualStyle)
								{
									style2 = (SystemResources.FindResourceInternal(SystemParameters.FocusVisualStyleKey) as Style);
								}
								if (style2 != null)
								{
									this._focusVisualAdornerCache = new KeyboardNavigation.FocusVisualAdorner(frameworkContentElement, parentUIElementFromContentElement, contentHost, style2);
									adornerLayer2.Add(this._focusVisualAdornerCache);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x001D72C4 File Offset: 0x001D62C4
		internal static void UpdateFocusedElement(DependencyObject focusTarget)
		{
			DependencyObject focusScope = FocusManager.GetFocusScope(focusTarget);
			if (focusScope != null && focusScope != focusTarget)
			{
				FocusManager.SetFocusedElement(focusScope, focusTarget as IInputElement);
				Visual visualRoot = KeyboardNavigation.GetVisualRoot(focusTarget);
				if (visualRoot != null && focusScope == visualRoot)
				{
					KeyboardNavigation.Current.NotifyFocusEnterMainFocusScope(visualRoot, EventArgs.Empty);
				}
			}
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x001D7309 File Offset: 0x001D6309
		internal void UpdateActiveElement(DependencyObject activeElement)
		{
			this.UpdateActiveElement(activeElement, KeyboardNavigation.TabNavigationProperty);
			this.UpdateActiveElement(activeElement, KeyboardNavigation.ControlTabNavigationProperty);
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x001D7324 File Offset: 0x001D6324
		private void UpdateActiveElement(DependencyObject activeElement, DependencyProperty dp)
		{
			this._navigationProperty = dp;
			DependencyObject groupParent = this.GetGroupParent(activeElement);
			this.UpdateActiveElement(groupParent, activeElement, dp);
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x001D7349 File Offset: 0x001D6349
		internal void UpdateActiveElement(DependencyObject container, DependencyObject activeElement)
		{
			this.UpdateActiveElement(container, activeElement, KeyboardNavigation.TabNavigationProperty);
			this.UpdateActiveElement(container, activeElement, KeyboardNavigation.ControlTabNavigationProperty);
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x001D7365 File Offset: 0x001D6365
		private void UpdateActiveElement(DependencyObject container, DependencyObject activeElement, DependencyProperty dp)
		{
			this._navigationProperty = dp;
			if (activeElement == container)
			{
				return;
			}
			if (this.GetKeyNavigationMode(container) == KeyboardNavigationMode.Once)
			{
				this.SetActiveElement(container, activeElement);
			}
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x001D7385 File Offset: 0x001D6385
		internal bool Navigate(DependencyObject currentElement, TraversalRequest request)
		{
			return this.Navigate(currentElement, request, Keyboard.Modifiers, false);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x001D7395 File Offset: 0x001D6395
		private bool Navigate(DependencyObject currentElement, TraversalRequest request, ModifierKeys modifierKeys, bool fromProcessInputTabKey = false)
		{
			return this.Navigate(currentElement, request, modifierKeys, null, fromProcessInputTabKey);
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x001D73A4 File Offset: 0x001D63A4
		private bool Navigate(DependencyObject currentElement, TraversalRequest request, ModifierKeys modifierKeys, DependencyObject firstElement, bool fromProcessInputTabKey = false)
		{
			DependencyObject dependencyObject = null;
			switch (request.FocusNavigationDirection)
			{
			case FocusNavigationDirection.Next:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetNextTab(currentElement, this.GetGroupParent(currentElement, true), false);
				break;
			case FocusNavigationDirection.Previous:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetPrevTab(currentElement, null, false);
				break;
			case FocusNavigationDirection.First:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetNextTab(null, currentElement, true);
				break;
			case FocusNavigationDirection.Last:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetPrevTab(null, currentElement, true);
				break;
			case FocusNavigationDirection.Left:
			case FocusNavigationDirection.Right:
			case FocusNavigationDirection.Up:
			case FocusNavigationDirection.Down:
				this._navigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
				dependencyObject = this.GetNextInDirection(currentElement, request.FocusNavigationDirection);
				break;
			}
			if (dependencyObject == null)
			{
				if (request.Wrapped || request.FocusNavigationDirection == FocusNavigationDirection.First || request.FocusNavigationDirection == FocusNavigationDirection.Last)
				{
					return false;
				}
				bool flag = true;
				if (this.NavigateOutsidePresentationSource(currentElement, request, fromProcessInputTabKey, ref flag))
				{
					return true;
				}
				if (flag && (request.FocusNavigationDirection == FocusNavigationDirection.Next || request.FocusNavigationDirection == FocusNavigationDirection.Previous))
				{
					Visual visualRoot = KeyboardNavigation.GetVisualRoot(currentElement);
					if (visualRoot != null)
					{
						return this.Navigate(visualRoot, new TraversalRequest((request.FocusNavigationDirection == FocusNavigationDirection.Next) ? FocusNavigationDirection.First : FocusNavigationDirection.Last));
					}
				}
				return false;
			}
			else
			{
				IKeyboardInputSink keyboardInputSink = dependencyObject as IKeyboardInputSink;
				if (keyboardInputSink == null)
				{
					IInputElement inputElement = dependencyObject as IInputElement;
					inputElement.Focus();
					return inputElement.IsKeyboardFocusWithin;
				}
				bool flag2;
				if (request.FocusNavigationDirection == FocusNavigationDirection.First || request.FocusNavigationDirection == FocusNavigationDirection.Next)
				{
					flag2 = keyboardInputSink.TabInto(new TraversalRequest(FocusNavigationDirection.First));
				}
				else if (request.FocusNavigationDirection == FocusNavigationDirection.Last || request.FocusNavigationDirection == FocusNavigationDirection.Previous)
				{
					flag2 = keyboardInputSink.TabInto(new TraversalRequest(FocusNavigationDirection.Last));
				}
				else
				{
					flag2 = keyboardInputSink.TabInto(new TraversalRequest(request.FocusNavigationDirection)
					{
						Wrapped = true
					});
				}
				if (!flag2 && firstElement != dependencyObject)
				{
					flag2 = this.Navigate(dependencyObject, request, modifierKeys, (firstElement == null) ? dependencyObject : firstElement, false);
				}
				return flag2;
			}
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x001D75AC File Offset: 0x001D65AC
		private bool NavigateOutsidePresentationSource(DependencyObject currentElement, TraversalRequest request, bool fromProcessInput, ref bool shouldCycle)
		{
			Visual visual = currentElement as Visual;
			if (visual == null)
			{
				visual = KeyboardNavigation.GetParentUIElementFromContentElement(currentElement as ContentElement);
				if (visual == null)
				{
					return false;
				}
			}
			IKeyboardInputSink keyboardInputSink = PresentationSource.CriticalFromVisual(visual) as IKeyboardInputSink;
			if (keyboardInputSink != null)
			{
				IKeyboardInputSite keyboardInputSite = keyboardInputSink.KeyboardInputSite;
				if (keyboardInputSite != null && this.ShouldNavigateOutsidePresentationSource(currentElement, request))
				{
					if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
					{
						IAvalonAdapter avalonAdapter = keyboardInputSite as IAvalonAdapter;
						if (avalonAdapter != null && fromProcessInput)
						{
							return avalonAdapter.OnNoMoreTabStops(request, ref shouldCycle);
						}
					}
					return keyboardInputSite.OnNoMoreTabStops(request);
				}
			}
			return false;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x001D7624 File Offset: 0x001D6624
		private bool ShouldNavigateOutsidePresentationSource(DependencyObject currentElement, TraversalRequest request)
		{
			if (request.FocusNavigationDirection == FocusNavigationDirection.Left || request.FocusNavigationDirection == FocusNavigationDirection.Right || request.FocusNavigationDirection == FocusNavigationDirection.Up || request.FocusNavigationDirection == FocusNavigationDirection.Down)
			{
				DependencyObject groupParent;
				while ((groupParent = this.GetGroupParent(currentElement)) != null && groupParent != currentElement)
				{
					KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(groupParent);
					if (keyNavigationMode == KeyboardNavigationMode.Contained || keyNavigationMode == KeyboardNavigationMode.Cycle)
					{
						return false;
					}
					currentElement = groupParent;
				}
			}
			return true;
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x001D767E File Offset: 0x001D667E
		internal static KeyboardNavigation Current
		{
			get
			{
				return FrameworkElement.KeyboardNavigation;
			}
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x001D7685 File Offset: 0x001D6685
		private void PostProcessInput(object sender, ProcessInputEventArgs e)
		{
			this.ProcessInput(e.StagingItem.Input);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x001D7698 File Offset: 0x001D6698
		private void TranslateAccelerator(object sender, KeyEventArgs e)
		{
			this.ProcessInput(e);
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x001D76A4 File Offset: 0x001D66A4
		private void ProcessInput(InputEventArgs inputEventArgs)
		{
			this.ProcessForMenuMode(inputEventArgs);
			this.ProcessForUIState(inputEventArgs);
			if (inputEventArgs.RoutedEvent != Keyboard.KeyDownEvent)
			{
				return;
			}
			KeyEventArgs keyEventArgs = (KeyEventArgs)inputEventArgs;
			if (keyEventArgs.Handled)
			{
				return;
			}
			DependencyObject dependencyObject = keyEventArgs.OriginalSource as DependencyObject;
			DependencyObject dependencyObject2 = keyEventArgs.KeyboardDevice.Target as DependencyObject;
			if (dependencyObject2 != null && dependencyObject != dependencyObject2 && dependencyObject is HwndHost)
			{
				dependencyObject = dependencyObject2;
			}
			if (dependencyObject == null)
			{
				HwndSource hwndSource = keyEventArgs.UnsafeInputSource as HwndSource;
				if (hwndSource == null)
				{
					return;
				}
				dependencyObject = hwndSource.RootVisual;
				if (dependencyObject == null)
				{
					return;
				}
			}
			Key realKey = this.GetRealKey(keyEventArgs);
			if (realKey != Key.Tab && realKey - Key.Left > 3)
			{
				if (realKey - Key.LeftAlt <= 1)
				{
					KeyboardNavigation.ShowFocusVisual();
					KeyboardNavigation.EnableKeyboardCues(dependencyObject, true);
				}
			}
			else
			{
				KeyboardNavigation.ShowFocusVisual();
			}
			keyEventArgs.Handled = this.Navigate(dependencyObject, keyEventArgs.Key, keyEventArgs.KeyboardDevice.Modifiers, true);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x001D777C File Offset: 0x001D677C
		internal static void EnableKeyboardCues(DependencyObject element, bool enable)
		{
			Visual visual = element as Visual;
			if (visual == null)
			{
				visual = KeyboardNavigation.GetParentUIElementFromContentElement(element as ContentElement);
				if (visual == null)
				{
					return;
				}
			}
			Visual visualRoot = KeyboardNavigation.GetVisualRoot(visual);
			if (visualRoot != null)
			{
				visualRoot.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, enable ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			}
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x001D77C7 File Offset: 0x001D67C7
		internal static FocusNavigationDirection KeyToTraversalDirection(Key key)
		{
			switch (key)
			{
			case Key.Left:
				return FocusNavigationDirection.Left;
			case Key.Up:
				return FocusNavigationDirection.Up;
			case Key.Right:
				return FocusNavigationDirection.Right;
			case Key.Down:
				return FocusNavigationDirection.Down;
			default:
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x001D77F1 File Offset: 0x001D67F1
		internal DependencyObject PredictFocusedElement(DependencyObject sourceElement, FocusNavigationDirection direction)
		{
			return this.PredictFocusedElement(sourceElement, direction, false);
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x001D77FC File Offset: 0x001D67FC
		internal DependencyObject PredictFocusedElement(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation)
		{
			return this.PredictFocusedElement(sourceElement, direction, treeViewNavigation, true);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x001D7808 File Offset: 0x001D6808
		internal DependencyObject PredictFocusedElement(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, bool considerDescendants)
		{
			if (sourceElement == null)
			{
				return null;
			}
			this._navigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
			return this.GetNextInDirection(sourceElement, direction, treeViewNavigation, considerDescendants);
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x001D7844 File Offset: 0x001D6844
		internal DependencyObject PredictFocusedElementAtViewportEdge(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, FrameworkElement viewportBoundsElement, DependencyObject container)
		{
			DependencyObject result;
			try
			{
				this._containerHashtable.Clear();
				result = this.PredictFocusedElementAtViewportEdgeRecursive(sourceElement, direction, treeViewNavigation, viewportBoundsElement, container);
			}
			finally
			{
				this._containerHashtable.Clear();
			}
			return result;
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x001D788C File Offset: 0x001D688C
		private DependencyObject PredictFocusedElementAtViewportEdgeRecursive(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, FrameworkElement viewportBoundsElement, DependencyObject container)
		{
			this._navigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
			if (container == null)
			{
				container = this.GetGroupParent(sourceElement);
			}
			if (container == sourceElement)
			{
				return null;
			}
			if (this.IsEndlessLoop(sourceElement, container))
			{
				return null;
			}
			DependencyObject dependencyObject = this.FindElementAtViewportEdge(sourceElement, viewportBoundsElement, container, direction, treeViewNavigation);
			if (dependencyObject != null)
			{
				if (this.IsElementEligible(dependencyObject, treeViewNavigation))
				{
					return dependencyObject;
				}
				DependencyObject sourceElement2 = dependencyObject;
				dependencyObject = this.PredictFocusedElementAtViewportEdgeRecursive(sourceElement, direction, treeViewNavigation, viewportBoundsElement, dependencyObject);
				if (dependencyObject != null)
				{
					return dependencyObject;
				}
				dependencyObject = this.PredictFocusedElementAtViewportEdgeRecursive(sourceElement2, direction, treeViewNavigation, viewportBoundsElement, null);
			}
			return dependencyObject;
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x001D7924 File Offset: 0x001D6924
		internal bool Navigate(DependencyObject sourceElement, Key key, ModifierKeys modifiers, bool fromProcessInput = false)
		{
			bool result = false;
			if (key != Key.Tab)
			{
				switch (key)
				{
				case Key.Left:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Left), modifiers, false);
					break;
				case Key.Up:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Up), modifiers, false);
					break;
				case Key.Right:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Right), modifiers, false);
					break;
				case Key.Down:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Down), modifiers, false);
					break;
				}
			}
			else
			{
				result = this.Navigate(sourceElement, new TraversalRequest(((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next), modifiers, fromProcessInput);
			}
			return result;
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x001D79B8 File Offset: 0x001D69B8
		private static bool IsInNavigationTree(DependencyObject visual)
		{
			UIElement uielement = visual as UIElement;
			if (uielement != null && uielement.IsVisible)
			{
				return true;
			}
			if (visual is IContentHost && !(visual is UIElementIsland))
			{
				return true;
			}
			UIElement3D uielement3D = visual as UIElement3D;
			return uielement3D != null && uielement3D.IsVisible;
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x001D7A00 File Offset: 0x001D6A00
		private DependencyObject GetPreviousSibling(DependencyObject e)
		{
			DependencyObject parent = KeyboardNavigation.GetParent(e);
			IContentHost contentHost = parent as IContentHost;
			if (contentHost != null)
			{
				IInputElement inputElement = null;
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement2 = hostedElements.Current;
					if (inputElement2 == e)
					{
						return inputElement as DependencyObject;
					}
					if (inputElement2 is UIElement || inputElement2 is UIElement3D)
					{
						inputElement = inputElement2;
					}
					else
					{
						ContentElement contentElement = inputElement2 as ContentElement;
						if (contentElement != null && this.IsTabStop(contentElement))
						{
							inputElement = inputElement2;
						}
					}
				}
				return null;
			}
			DependencyObject dependencyObject = parent as UIElement;
			if (dependencyObject == null)
			{
				dependencyObject = (parent as UIElement3D);
			}
			DependencyObject dependencyObject2 = e as Visual;
			if (dependencyObject2 == null)
			{
				dependencyObject2 = (e as Visual3D);
			}
			if (dependencyObject != null && dependencyObject2 != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
				DependencyObject result = null;
				for (int i = 0; i < childrenCount; i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
					if (child == dependencyObject2)
					{
						break;
					}
					if (KeyboardNavigation.IsInNavigationTree(child))
					{
						result = child;
					}
				}
				return result;
			}
			return null;
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x001D7AE8 File Offset: 0x001D6AE8
		private DependencyObject GetNextSibling(DependencyObject e)
		{
			DependencyObject parent = KeyboardNavigation.GetParent(e);
			IContentHost contentHost = parent as IContentHost;
			if (contentHost != null)
			{
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				bool flag = false;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement = hostedElements.Current;
					if (flag)
					{
						if (inputElement is UIElement || inputElement is UIElement3D)
						{
							return inputElement as DependencyObject;
						}
						ContentElement contentElement = inputElement as ContentElement;
						if (contentElement != null && this.IsTabStop(contentElement))
						{
							return contentElement;
						}
					}
					else if (inputElement == e)
					{
						flag = true;
					}
				}
			}
			else
			{
				DependencyObject dependencyObject = parent as UIElement;
				if (dependencyObject == null)
				{
					dependencyObject = (parent as UIElement3D);
				}
				DependencyObject dependencyObject2 = e as Visual;
				if (dependencyObject2 == null)
				{
					dependencyObject2 = (e as Visual3D);
				}
				if (dependencyObject != null && dependencyObject2 != null)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
					int i = 0;
					while (i < childrenCount && VisualTreeHelper.GetChild(dependencyObject, i) != dependencyObject2)
					{
						i++;
					}
					for (i++; i < childrenCount; i++)
					{
						DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
						if (KeyboardNavigation.IsInNavigationTree(child))
						{
							return child;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x001D7BE8 File Offset: 0x001D6BE8
		private DependencyObject FocusedElement(DependencyObject e)
		{
			IInputElement inputElement = e as IInputElement;
			if (inputElement != null && !inputElement.IsKeyboardFocusWithin)
			{
				DependencyObject dependencyObject = FocusManager.GetFocusedElement(e) as DependencyObject;
				if (dependencyObject != null && (this._navigationProperty == KeyboardNavigation.ControlTabNavigationProperty || !this.IsFocusScope(e)))
				{
					Visual visual = dependencyObject as Visual;
					if (visual == null)
					{
						Visual3D visual3D = dependencyObject as Visual3D;
						if (visual3D == null)
						{
							visual = KeyboardNavigation.GetParentUIElementFromContentElement(dependencyObject as ContentElement);
						}
						else if (visual3D != e && visual3D.IsDescendantOf(e))
						{
							return dependencyObject;
						}
					}
					if (visual != null && visual != e && visual.IsDescendantOf(e))
					{
						return dependencyObject;
					}
				}
			}
			return null;
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x001D7C70 File Offset: 0x001D6C70
		private DependencyObject GetFirstChild(DependencyObject e)
		{
			DependencyObject dependencyObject = this.FocusedElement(e);
			if (dependencyObject != null)
			{
				return dependencyObject;
			}
			IContentHost contentHost = e as IContentHost;
			if (contentHost != null)
			{
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement = hostedElements.Current;
					if (inputElement is UIElement || inputElement is UIElement3D)
					{
						return inputElement as DependencyObject;
					}
					ContentElement contentElement = inputElement as ContentElement;
					if (contentElement != null && this.IsTabStop(contentElement))
					{
						return contentElement;
					}
				}
				return null;
			}
			DependencyObject dependencyObject2 = e as UIElement;
			if (dependencyObject2 == null)
			{
				dependencyObject2 = (e as UIElement3D);
			}
			if (dependencyObject2 == null || UIElementHelper.IsVisible(dependencyObject2))
			{
				DependencyObject dependencyObject3 = e as Visual;
				if (dependencyObject3 == null)
				{
					dependencyObject3 = (e as Visual3D);
				}
				if (dependencyObject3 != null)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject3);
					for (int i = 0; i < childrenCount; i++)
					{
						DependencyObject child = VisualTreeHelper.GetChild(dependencyObject3, i);
						if (KeyboardNavigation.IsInNavigationTree(child))
						{
							return child;
						}
						DependencyObject firstChild = this.GetFirstChild(child);
						if (firstChild != null)
						{
							return firstChild;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x001D7D5C File Offset: 0x001D6D5C
		private DependencyObject GetLastChild(DependencyObject e)
		{
			DependencyObject dependencyObject = this.FocusedElement(e);
			if (dependencyObject != null)
			{
				return dependencyObject;
			}
			IContentHost contentHost = e as IContentHost;
			if (contentHost != null)
			{
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				IInputElement inputElement = null;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement2 = hostedElements.Current;
					if (inputElement2 is UIElement || inputElement2 is UIElement3D)
					{
						inputElement = inputElement2;
					}
					else
					{
						ContentElement contentElement = inputElement2 as ContentElement;
						if (contentElement != null && this.IsTabStop(contentElement))
						{
							inputElement = inputElement2;
						}
					}
				}
				return inputElement as DependencyObject;
			}
			DependencyObject dependencyObject2 = e as UIElement;
			if (dependencyObject2 == null)
			{
				dependencyObject2 = (e as UIElement3D);
			}
			if (dependencyObject2 == null || UIElementHelper.IsVisible(dependencyObject2))
			{
				DependencyObject dependencyObject3 = e as Visual;
				if (dependencyObject3 == null)
				{
					dependencyObject3 = (e as Visual3D);
				}
				if (dependencyObject3 != null)
				{
					for (int i = VisualTreeHelper.GetChildrenCount(dependencyObject3) - 1; i >= 0; i--)
					{
						DependencyObject child = VisualTreeHelper.GetChild(dependencyObject3, i);
						if (KeyboardNavigation.IsInNavigationTree(child))
						{
							return child;
						}
						DependencyObject lastChild = this.GetLastChild(child);
						if (lastChild != null)
						{
							return lastChild;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x001D7E4C File Offset: 0x001D6E4C
		internal static DependencyObject GetParent(DependencyObject e)
		{
			if (e is Visual || e is Visual3D)
			{
				DependencyObject dependencyObject = e;
				while ((dependencyObject = VisualTreeHelper.GetParent(dependencyObject)) != null)
				{
					if (KeyboardNavigation.IsInNavigationTree(dependencyObject))
					{
						return dependencyObject;
					}
				}
			}
			else
			{
				ContentElement contentElement = e as ContentElement;
				if (contentElement != null)
				{
					return ContentHostHelper.FindContentHost(contentElement) as DependencyObject;
				}
			}
			return null;
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x001D7E9C File Offset: 0x001D6E9C
		private DependencyObject GetNextInTree(DependencyObject e, DependencyObject container)
		{
			DependencyObject dependencyObject = null;
			if (e == container || !this.IsGroup(e))
			{
				dependencyObject = this.GetFirstChild(e);
			}
			if (dependencyObject != null || e == container)
			{
				return dependencyObject;
			}
			DependencyObject dependencyObject2 = e;
			DependencyObject nextSibling;
			for (;;)
			{
				nextSibling = this.GetNextSibling(dependencyObject2);
				if (nextSibling != null)
				{
					break;
				}
				dependencyObject2 = KeyboardNavigation.GetParent(dependencyObject2);
				if (dependencyObject2 == null || dependencyObject2 == container)
				{
					goto IL_3D;
				}
			}
			return nextSibling;
			IL_3D:
			return null;
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x001D7EE8 File Offset: 0x001D6EE8
		private DependencyObject GetPreviousInTree(DependencyObject e, DependencyObject container)
		{
			if (e == container)
			{
				return null;
			}
			DependencyObject previousSibling = this.GetPreviousSibling(e);
			if (previousSibling == null)
			{
				return KeyboardNavigation.GetParent(e);
			}
			if (this.IsGroup(previousSibling))
			{
				return previousSibling;
			}
			return this.GetLastInTree(previousSibling);
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x001D7F20 File Offset: 0x001D6F20
		private DependencyObject GetLastInTree(DependencyObject container)
		{
			DependencyObject result;
			do
			{
				result = container;
				container = this.GetLastChild(container);
			}
			while (container != null && !this.IsGroup(container));
			if (container != null)
			{
				return container;
			}
			return result;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x001D7F4A File Offset: 0x001D6F4A
		private DependencyObject GetGroupParent(DependencyObject e)
		{
			return this.GetGroupParent(e, false);
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x001D7F54 File Offset: 0x001D6F54
		private DependencyObject GetGroupParent(DependencyObject e, bool includeCurrent)
		{
			DependencyObject result = e;
			if (!includeCurrent)
			{
				result = e;
				e = KeyboardNavigation.GetParent(e);
				if (e == null)
				{
					return result;
				}
			}
			while (e != null)
			{
				if (this.IsGroup(e))
				{
					return e;
				}
				result = e;
				e = KeyboardNavigation.GetParent(e);
			}
			return result;
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x001D7F90 File Offset: 0x001D6F90
		private bool IsTabStop(DependencyObject e)
		{
			FrameworkElement frameworkElement = e as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.Focusable && (bool)frameworkElement.GetValue(KeyboardNavigation.IsTabStopProperty) && frameworkElement.IsEnabled && frameworkElement.IsVisible;
			}
			FrameworkContentElement frameworkContentElement = e as FrameworkContentElement;
			return frameworkContentElement != null && frameworkContentElement.Focusable && (bool)frameworkContentElement.GetValue(KeyboardNavigation.IsTabStopProperty) && frameworkContentElement.IsEnabled;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x001D7FFE File Offset: 0x001D6FFE
		private bool IsGroup(DependencyObject e)
		{
			return this.GetKeyNavigationMode(e) > KeyboardNavigationMode.Continue;
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x001D800C File Offset: 0x001D700C
		internal bool IsFocusableInternal(DependencyObject element)
		{
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				return uielement.Focusable && uielement.IsEnabled && uielement.IsVisible;
			}
			ContentElement contentElement = element as ContentElement;
			return contentElement != null && (contentElement != null && contentElement.Focusable) && contentElement.IsEnabled;
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x001D805B File Offset: 0x001D705B
		private bool IsElementEligible(DependencyObject element, bool treeViewNavigation)
		{
			if (treeViewNavigation)
			{
				return element is TreeViewItem && this.IsFocusableInternal(element);
			}
			return this.IsTabStop(element);
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x001D8079 File Offset: 0x001D7079
		private bool IsGroupElementEligible(DependencyObject element, bool treeViewNavigation)
		{
			if (treeViewNavigation)
			{
				return element is TreeViewItem && this.IsFocusableInternal(element);
			}
			return this.IsTabStopOrGroup(element);
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x001D8097 File Offset: 0x001D7097
		private KeyboardNavigationMode GetKeyNavigationMode(DependencyObject e)
		{
			return (KeyboardNavigationMode)e.GetValue(this._navigationProperty);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x001D80AA File Offset: 0x001D70AA
		private bool IsTabStopOrGroup(DependencyObject e)
		{
			return this.IsTabStop(e) || this.IsGroup(e);
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x001D80BE File Offset: 0x001D70BE
		private static int GetTabIndexHelper(DependencyObject d)
		{
			return (int)d.GetValue(KeyboardNavigation.TabIndexProperty);
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x001D80D0 File Offset: 0x001D70D0
		internal DependencyObject GetFirstTabInGroup(DependencyObject container)
		{
			DependencyObject dependencyObject = null;
			int num = int.MinValue;
			DependencyObject dependencyObject2 = container;
			while ((dependencyObject2 = this.GetNextInTree(dependencyObject2, container)) != null)
			{
				if (this.IsTabStopOrGroup(dependencyObject2))
				{
					int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(dependencyObject2);
					if (tabIndexHelper < num || dependencyObject == null)
					{
						num = tabIndexHelper;
						dependencyObject = dependencyObject2;
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x001D8114 File Offset: 0x001D7114
		private DependencyObject GetNextTabWithSameIndex(DependencyObject e, DependencyObject container)
		{
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			DependencyObject dependencyObject = e;
			while ((dependencyObject = this.GetNextInTree(dependencyObject, container)) != null)
			{
				if (this.IsTabStopOrGroup(dependencyObject) && KeyboardNavigation.GetTabIndexHelper(dependencyObject) == tabIndexHelper)
				{
					return dependencyObject;
				}
			}
			return null;
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x001D8150 File Offset: 0x001D7150
		private DependencyObject GetNextTabWithNextIndex(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = null;
			int num = int.MinValue;
			int num2 = int.MinValue;
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			DependencyObject dependencyObject3 = container;
			while ((dependencyObject3 = this.GetNextInTree(dependencyObject3, container)) != null)
			{
				if (this.IsTabStopOrGroup(dependencyObject3))
				{
					int tabIndexHelper2 = KeyboardNavigation.GetTabIndexHelper(dependencyObject3);
					if (tabIndexHelper2 > tabIndexHelper && (tabIndexHelper2 < num2 || dependencyObject == null))
					{
						num2 = tabIndexHelper2;
						dependencyObject = dependencyObject3;
					}
					if (tabIndexHelper2 < num || dependencyObject2 == null)
					{
						num = tabIndexHelper2;
						dependencyObject2 = dependencyObject3;
					}
				}
			}
			if (tabbingType == KeyboardNavigationMode.Cycle && dependencyObject == null)
			{
				dependencyObject = dependencyObject2;
			}
			return dependencyObject;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x001D81C8 File Offset: 0x001D71C8
		private DependencyObject GetNextTabInGroup(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			if (tabbingType == KeyboardNavigationMode.None)
			{
				return null;
			}
			if (e == null || e == container)
			{
				return this.GetFirstTabInGroup(container);
			}
			if (tabbingType == KeyboardNavigationMode.Once)
			{
				return null;
			}
			DependencyObject nextTabWithSameIndex = this.GetNextTabWithSameIndex(e, container);
			if (nextTabWithSameIndex != null)
			{
				return nextTabWithSameIndex;
			}
			return this.GetNextTabWithNextIndex(e, container, tabbingType);
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x001D8208 File Offset: 0x001D7208
		private DependencyObject GetNextTab(DependencyObject e, DependencyObject container, bool goDownOnly)
		{
			KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(container);
			if (e == null)
			{
				if (this.IsTabStop(container))
				{
					return container;
				}
				DependencyObject activeElement = this.GetActiveElement(container);
				if (activeElement != null)
				{
					return this.GetNextTab(null, activeElement, true);
				}
			}
			else if ((keyNavigationMode == KeyboardNavigationMode.Once || keyNavigationMode == KeyboardNavigationMode.None) && container != e)
			{
				if (goDownOnly)
				{
					return null;
				}
				DependencyObject groupParent = this.GetGroupParent(container);
				return this.GetNextTab(container, groupParent, goDownOnly);
			}
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = e;
			KeyboardNavigationMode keyboardNavigationMode = keyNavigationMode;
			while ((dependencyObject2 = this.GetNextTabInGroup(dependencyObject2, container, keyboardNavigationMode)) != null && dependencyObject != dependencyObject2)
			{
				if (dependencyObject == null)
				{
					dependencyObject = dependencyObject2;
				}
				DependencyObject nextTab = this.GetNextTab(null, dependencyObject2, true);
				if (nextTab != null)
				{
					return nextTab;
				}
				if (keyboardNavigationMode == KeyboardNavigationMode.Once)
				{
					keyboardNavigationMode = KeyboardNavigationMode.Contained;
				}
			}
			if (!goDownOnly && keyboardNavigationMode != KeyboardNavigationMode.Contained && KeyboardNavigation.GetParent(container) != null)
			{
				return this.GetNextTab(container, this.GetGroupParent(container), false);
			}
			return null;
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x001D82C0 File Offset: 0x001D72C0
		internal DependencyObject GetLastTabInGroup(DependencyObject container)
		{
			DependencyObject dependencyObject = null;
			int num = int.MaxValue;
			DependencyObject dependencyObject2 = this.GetLastInTree(container);
			while (dependencyObject2 != null && dependencyObject2 != container)
			{
				if (this.IsTabStopOrGroup(dependencyObject2))
				{
					int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(dependencyObject2);
					if (tabIndexHelper > num || dependencyObject == null)
					{
						num = tabIndexHelper;
						dependencyObject = dependencyObject2;
					}
				}
				dependencyObject2 = this.GetPreviousInTree(dependencyObject2, container);
			}
			return dependencyObject;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x001D830C File Offset: 0x001D730C
		private DependencyObject GetPrevTabWithSameIndex(DependencyObject e, DependencyObject container)
		{
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			for (DependencyObject previousInTree = this.GetPreviousInTree(e, container); previousInTree != null; previousInTree = this.GetPreviousInTree(previousInTree, container))
			{
				if (this.IsTabStopOrGroup(previousInTree) && KeyboardNavigation.GetTabIndexHelper(previousInTree) == tabIndexHelper && previousInTree != container)
				{
					return previousInTree;
				}
			}
			return null;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x001D8350 File Offset: 0x001D7350
		private DependencyObject GetPrevTabWithPrevIndex(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = null;
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			for (DependencyObject dependencyObject3 = this.GetLastInTree(container); dependencyObject3 != null; dependencyObject3 = this.GetPreviousInTree(dependencyObject3, container))
			{
				if (this.IsTabStopOrGroup(dependencyObject3) && dependencyObject3 != container)
				{
					int tabIndexHelper2 = KeyboardNavigation.GetTabIndexHelper(dependencyObject3);
					if (tabIndexHelper2 < tabIndexHelper && (tabIndexHelper2 > num2 || dependencyObject2 == null))
					{
						num2 = tabIndexHelper2;
						dependencyObject2 = dependencyObject3;
					}
					if (tabIndexHelper2 > num || dependencyObject == null)
					{
						num = tabIndexHelper2;
						dependencyObject = dependencyObject3;
					}
				}
			}
			if (tabbingType == KeyboardNavigationMode.Cycle && dependencyObject2 == null)
			{
				dependencyObject2 = dependencyObject;
			}
			return dependencyObject2;
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x001D83D4 File Offset: 0x001D73D4
		private DependencyObject GetPrevTabInGroup(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			if (tabbingType == KeyboardNavigationMode.None)
			{
				return null;
			}
			if (e == null)
			{
				return this.GetLastTabInGroup(container);
			}
			if (tabbingType == KeyboardNavigationMode.Once)
			{
				return null;
			}
			if (e == container)
			{
				return null;
			}
			DependencyObject prevTabWithSameIndex = this.GetPrevTabWithSameIndex(e, container);
			if (prevTabWithSameIndex != null)
			{
				return prevTabWithSameIndex;
			}
			return this.GetPrevTabWithPrevIndex(e, container, tabbingType);
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x001D8418 File Offset: 0x001D7418
		private DependencyObject GetPrevTab(DependencyObject e, DependencyObject container, bool goDownOnly)
		{
			if (container == null)
			{
				container = this.GetGroupParent(e);
			}
			KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(container);
			if (e == null)
			{
				DependencyObject activeElement = this.GetActiveElement(container);
				if (activeElement != null)
				{
					return this.GetPrevTab(null, activeElement, true);
				}
				if (keyNavigationMode == KeyboardNavigationMode.Once)
				{
					DependencyObject nextTabInGroup = this.GetNextTabInGroup(null, container, keyNavigationMode);
					if (nextTabInGroup != null)
					{
						return this.GetPrevTab(null, nextTabInGroup, true);
					}
					if (this.IsTabStop(container))
					{
						return container;
					}
					if (goDownOnly)
					{
						return null;
					}
					return this.GetPrevTab(container, null, false);
				}
			}
			else if (keyNavigationMode == KeyboardNavigationMode.Once || keyNavigationMode == KeyboardNavigationMode.None)
			{
				if (goDownOnly || container == e)
				{
					return null;
				}
				if (this.IsTabStop(container))
				{
					return container;
				}
				return this.GetPrevTab(container, null, false);
			}
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = e;
			while ((dependencyObject2 = this.GetPrevTabInGroup(dependencyObject2, container, keyNavigationMode)) != null && (dependencyObject2 != container || keyNavigationMode != KeyboardNavigationMode.Local))
			{
				if (this.IsTabStop(dependencyObject2) && !this.IsGroup(dependencyObject2))
				{
					return dependencyObject2;
				}
				if (dependencyObject == dependencyObject2)
				{
					break;
				}
				if (dependencyObject == null)
				{
					dependencyObject = dependencyObject2;
				}
				DependencyObject prevTab = this.GetPrevTab(null, dependencyObject2, true);
				if (prevTab != null)
				{
					return prevTab;
				}
			}
			if (keyNavigationMode == KeyboardNavigationMode.Contained)
			{
				return null;
			}
			if (e != container && this.IsTabStop(container))
			{
				return container;
			}
			if (!goDownOnly && KeyboardNavigation.GetParent(container) != null)
			{
				return this.GetPrevTab(container, null, false);
			}
			return null;
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x001D8524 File Offset: 0x001D7524
		internal static Rect GetRectangle(DependencyObject element)
		{
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				if (!uielement.IsArrangeValid)
				{
					uielement.UpdateLayout();
				}
				Visual visualRoot = KeyboardNavigation.GetVisualRoot(uielement);
				if (visualRoot != null)
				{
					GeneralTransform generalTransform = uielement.TransformToAncestor(visualRoot);
					Thickness thickness = (Thickness)uielement.GetValue(KeyboardNavigation.DirectionalNavigationMarginProperty);
					double x = -thickness.Left;
					double y = -thickness.Top;
					double num = uielement.RenderSize.Width + thickness.Left + thickness.Right;
					double num2 = uielement.RenderSize.Height + thickness.Top + thickness.Bottom;
					if (num < 0.0)
					{
						x = uielement.RenderSize.Width * 0.5;
						num = 0.0;
					}
					if (num2 < 0.0)
					{
						y = uielement.RenderSize.Height * 0.5;
						num2 = 0.0;
					}
					return generalTransform.TransformBounds(new Rect(x, y, num, num2));
				}
			}
			else
			{
				ContentElement contentElement = element as ContentElement;
				if (contentElement != null)
				{
					IContentHost contentHost = null;
					UIElement parentUIElementFromContentElement = KeyboardNavigation.GetParentUIElementFromContentElement(contentElement, ref contentHost);
					Visual visual = contentHost as Visual;
					if (contentHost != null && visual != null && parentUIElementFromContentElement != null)
					{
						Visual visualRoot2 = KeyboardNavigation.GetVisualRoot(visual);
						if (visualRoot2 != null)
						{
							if (!parentUIElementFromContentElement.IsMeasureValid)
							{
								parentUIElementFromContentElement.UpdateLayout();
							}
							IEnumerator<Rect> enumerator = contentHost.GetRectangles(contentElement).GetEnumerator();
							if (enumerator.MoveNext())
							{
								GeneralTransform generalTransform2 = visual.TransformToAncestor(visualRoot2);
								Rect rect = enumerator.Current;
								return generalTransform2.TransformBounds(rect);
							}
						}
					}
				}
				else
				{
					UIElement3D uielement3D = element as UIElement3D;
					if (uielement3D != null)
					{
						Visual visualRoot3 = KeyboardNavigation.GetVisualRoot(uielement3D);
						Visual containingVisual2D = VisualTreeHelper.GetContainingVisual2D(uielement3D);
						if (visualRoot3 != null && containingVisual2D != null)
						{
							Rect visual2DContentBounds = uielement3D.Visual2DContentBounds;
							return containingVisual2D.TransformToAncestor(visualRoot3).TransformBounds(visual2DContentBounds);
						}
					}
				}
			}
			return Rect.Empty;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x001D8710 File Offset: 0x001D7710
		private Rect GetRepresentativeRectangle(DependencyObject element)
		{
			Rect rectangle = KeyboardNavigation.GetRectangle(element);
			TreeViewItem treeViewItem = element as TreeViewItem;
			if (treeViewItem != null)
			{
				Panel itemsHost = treeViewItem.ItemsHost;
				if (itemsHost != null && itemsHost.IsVisible)
				{
					Rect rectangle2 = KeyboardNavigation.GetRectangle(itemsHost);
					if (rectangle2 != Rect.Empty)
					{
						bool? flag = null;
						FrameworkElement frameworkElement = treeViewItem.TryGetHeaderElement();
						if (frameworkElement != null && frameworkElement != treeViewItem && frameworkElement.IsVisible)
						{
							Rect rectangle3 = KeyboardNavigation.GetRectangle(frameworkElement);
							if (!rectangle3.IsEmpty)
							{
								if (DoubleUtil.LessThan(rectangle3.Top, rectangle2.Top))
								{
									flag = new bool?(true);
								}
								else if (DoubleUtil.GreaterThan(rectangle3.Bottom, rectangle2.Bottom))
								{
									flag = new bool?(false);
								}
							}
						}
						double num = rectangle2.Top - rectangle.Top;
						double num2 = rectangle.Bottom - rectangle2.Bottom;
						if (flag == null)
						{
							flag = new bool?(DoubleUtil.GreaterThanOrClose(num, num2));
						}
						bool? flag2 = flag;
						bool flag3 = true;
						if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
						{
							rectangle.Height = Math.Min(Math.Max(num, 0.0), rectangle.Height);
						}
						else
						{
							double num3 = Math.Min(Math.Max(num2, 0.0), rectangle.Height);
							rectangle.Y = rectangle.Bottom - num3;
							rectangle.Height = num3;
						}
					}
				}
			}
			return rectangle;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x001D8888 File Offset: 0x001D7888
		private double GetDistance(Point p1, Point p2)
		{
			double num = p1.X - p2.X;
			double num2 = p1.Y - p2.Y;
			return Math.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x001D88C0 File Offset: 0x001D78C0
		private double GetPerpDistance(Rect sourceRect, Rect targetRect, FocusNavigationDirection direction)
		{
			switch (direction)
			{
			case FocusNavigationDirection.Left:
				return sourceRect.Right - targetRect.Right;
			case FocusNavigationDirection.Right:
				return targetRect.Left - sourceRect.Left;
			case FocusNavigationDirection.Up:
				return sourceRect.Bottom - targetRect.Bottom;
			case FocusNavigationDirection.Down:
				return targetRect.Top - sourceRect.Top;
			default:
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x001D893C File Offset: 0x001D793C
		private double GetDistance(Rect sourceRect, Rect targetRect, FocusNavigationDirection direction)
		{
			Point p;
			Point p2;
			switch (direction)
			{
			case FocusNavigationDirection.Left:
				p = sourceRect.TopRight;
				if (this._horizontalBaseline != -1.7976931348623157E+308)
				{
					p.Y = this._horizontalBaseline;
				}
				p2 = targetRect.TopRight;
				break;
			case FocusNavigationDirection.Right:
				p = sourceRect.TopLeft;
				if (this._horizontalBaseline != -1.7976931348623157E+308)
				{
					p.Y = this._horizontalBaseline;
				}
				p2 = targetRect.TopLeft;
				break;
			case FocusNavigationDirection.Up:
				p = sourceRect.BottomLeft;
				if (this._verticalBaseline != -1.7976931348623157E+308)
				{
					p.X = this._verticalBaseline;
				}
				p2 = targetRect.BottomLeft;
				break;
			case FocusNavigationDirection.Down:
				p = sourceRect.TopLeft;
				if (this._verticalBaseline != -1.7976931348623157E+308)
				{
					p.X = this._verticalBaseline;
				}
				p2 = targetRect.TopLeft;
				break;
			default:
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
			}
			return this.GetDistance(p, p2);
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x001D8A48 File Offset: 0x001D7A48
		private bool IsInDirection(Rect fromRect, Rect toRect, FocusNavigationDirection direction)
		{
			switch (direction)
			{
			case FocusNavigationDirection.Left:
				return DoubleUtil.GreaterThanOrClose(fromRect.Left, toRect.Right);
			case FocusNavigationDirection.Right:
				return DoubleUtil.LessThanOrClose(fromRect.Right, toRect.Left);
			case FocusNavigationDirection.Up:
				return DoubleUtil.GreaterThanOrClose(fromRect.Top, toRect.Bottom);
			case FocusNavigationDirection.Down:
				return DoubleUtil.LessThanOrClose(fromRect.Bottom, toRect.Top);
			default:
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x001D8AD4 File Offset: 0x001D7AD4
		private bool IsFocusScope(DependencyObject e)
		{
			return FocusManager.GetIsFocusScope(e) || KeyboardNavigation.GetParent(e) == null;
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x001D8AEC File Offset: 0x001D7AEC
		private bool IsAncestorOf(DependencyObject sourceElement, DependencyObject targetElement)
		{
			Visual visual = sourceElement as Visual;
			Visual visual2 = targetElement as Visual;
			return visual != null && visual2 != null && visual.IsAncestorOf(visual2);
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x001D8B16 File Offset: 0x001D7B16
		internal bool IsAncestorOfEx(DependencyObject sourceElement, DependencyObject targetElement)
		{
			while (targetElement != null && targetElement != sourceElement)
			{
				targetElement = KeyboardNavigation.GetParent(targetElement);
			}
			return targetElement == sourceElement;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x001D8B30 File Offset: 0x001D7B30
		private bool IsInRange(DependencyObject sourceElement, DependencyObject targetElement, Rect sourceRect, Rect targetRect, FocusNavigationDirection direction, double startRange, double endRange)
		{
			if (direction - FocusNavigationDirection.Left > 1)
			{
				if (direction - FocusNavigationDirection.Up > 1)
				{
					throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
				}
				if (this._verticalBaseline != -1.7976931348623157E+308)
				{
					startRange = Math.Min(startRange, this._verticalBaseline);
					endRange = Math.Max(endRange, this._verticalBaseline);
				}
				if (DoubleUtil.GreaterThan(targetRect.Right, startRange) && DoubleUtil.LessThan(targetRect.Left, endRange))
				{
					if (sourceElement == null)
					{
						return true;
					}
					if (direction == FocusNavigationDirection.Down)
					{
						return DoubleUtil.GreaterThan(targetRect.Top, sourceRect.Top) || (DoubleUtil.AreClose(targetRect.Top, sourceRect.Top) && this.IsAncestorOfEx(sourceElement, targetElement));
					}
					return DoubleUtil.LessThan(targetRect.Bottom, sourceRect.Bottom) || (DoubleUtil.AreClose(targetRect.Bottom, sourceRect.Bottom) && this.IsAncestorOfEx(sourceElement, targetElement));
				}
			}
			else
			{
				if (this._horizontalBaseline != -1.7976931348623157E+308)
				{
					startRange = Math.Min(startRange, this._horizontalBaseline);
					endRange = Math.Max(endRange, this._horizontalBaseline);
				}
				if (DoubleUtil.GreaterThan(targetRect.Bottom, startRange) && DoubleUtil.LessThan(targetRect.Top, endRange))
				{
					if (sourceElement == null)
					{
						return true;
					}
					if (direction == FocusNavigationDirection.Right)
					{
						return DoubleUtil.GreaterThan(targetRect.Left, sourceRect.Left) || (DoubleUtil.AreClose(targetRect.Left, sourceRect.Left) && this.IsAncestorOfEx(sourceElement, targetElement));
					}
					return DoubleUtil.LessThan(targetRect.Right, sourceRect.Right) || (DoubleUtil.AreClose(targetRect.Right, sourceRect.Right) && this.IsAncestorOfEx(sourceElement, targetElement));
				}
			}
			return false;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x001D8D05 File Offset: 0x001D7D05
		private DependencyObject GetNextInDirection(DependencyObject sourceElement, FocusNavigationDirection direction)
		{
			return this.GetNextInDirection(sourceElement, direction, false);
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x001D8D10 File Offset: 0x001D7D10
		private DependencyObject GetNextInDirection(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation)
		{
			return this.GetNextInDirection(sourceElement, direction, treeViewNavigation, true);
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x001D8D1C File Offset: 0x001D7D1C
		private DependencyObject GetNextInDirection(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, bool considerDescendants)
		{
			this._containerHashtable.Clear();
			DependencyObject dependencyObject = this.MoveNext(sourceElement, null, direction, double.MinValue, double.MinValue, treeViewNavigation, considerDescendants);
			if (dependencyObject != null)
			{
				UIElement uielement = sourceElement as UIElement;
				if (uielement != null)
				{
					uielement.RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
				}
				else
				{
					ContentElement contentElement = sourceElement as ContentElement;
					if (contentElement != null)
					{
						contentElement.RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
					}
				}
				UIElement uielement2 = dependencyObject as UIElement;
				if (uielement2 == null)
				{
					uielement2 = KeyboardNavigation.GetParentUIElementFromContentElement(dependencyObject as ContentElement);
				}
				else
				{
					ContentElement contentElement2 = dependencyObject as ContentElement;
					if (contentElement2 != null)
					{
						contentElement2.AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus), true);
					}
				}
				if (uielement2 != null)
				{
					uielement2.LayoutUpdated += this.OnLayoutUpdated;
					if (dependencyObject == uielement2)
					{
						uielement2.AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus), true);
					}
				}
			}
			this._containerHashtable.Clear();
			return dependencyObject;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x001D8E18 File Offset: 0x001D7E18
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			UIElement uielement = sender as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated -= this.OnLayoutUpdated;
			}
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x001D8E60 File Offset: 0x001D7E60
		private void _LostFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
			if (sender is UIElement)
			{
				((UIElement)sender).RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
				return;
			}
			if (sender is ContentElement)
			{
				((ContentElement)sender).RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
			}
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x001D8ED4 File Offset: 0x001D7ED4
		private bool IsEndlessLoop(DependencyObject element, DependencyObject container)
		{
			object key = (element != null) ? element : KeyboardNavigation._fakeNull;
			Hashtable hashtable = this._containerHashtable[container] as Hashtable;
			if (hashtable != null)
			{
				if (hashtable[key] != null)
				{
					return true;
				}
			}
			else
			{
				hashtable = new Hashtable(10);
				this._containerHashtable[container] = hashtable;
			}
			hashtable[key] = BooleanBoxes.TrueBox;
			return false;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x001D8F30 File Offset: 0x001D7F30
		private void ResetBaseLines(double value, bool horizontalDirection)
		{
			if (horizontalDirection)
			{
				this._verticalBaseline = double.MinValue;
				if (this._horizontalBaseline == -1.7976931348623157E+308)
				{
					this._horizontalBaseline = value;
					return;
				}
			}
			else
			{
				this._horizontalBaseline = double.MinValue;
				if (this._verticalBaseline == -1.7976931348623157E+308)
				{
					this._verticalBaseline = value;
				}
			}
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x001D8F90 File Offset: 0x001D7F90
		private DependencyObject FindNextInDirection(DependencyObject sourceElement, Rect sourceRect, DependencyObject container, FocusNavigationDirection direction, double startRange, double endRange, bool treeViewNavigation, bool considerDescendants)
		{
			DependencyObject dependencyObject = null;
			Rect targetRect = Rect.Empty;
			double value = 0.0;
			bool flag = sourceElement == null;
			DependencyObject dependencyObject2 = container;
			while ((dependencyObject2 = this.GetNextInTree(dependencyObject2, container)) != null)
			{
				if (dependencyObject2 != sourceElement && this.IsGroupElementEligible(dependencyObject2, treeViewNavigation))
				{
					Rect representativeRectangle = this.GetRepresentativeRectangle(dependencyObject2);
					if (representativeRectangle != Rect.Empty)
					{
						bool flag2 = this.IsInDirection(sourceRect, representativeRectangle, direction);
						bool flag3 = this.IsInRange(sourceElement, dependencyObject2, sourceRect, representativeRectangle, direction, startRange, endRange);
						if (flag || flag2 || flag3)
						{
							double num = flag3 ? this.GetPerpDistance(sourceRect, representativeRectangle, direction) : this.GetDistance(sourceRect, representativeRectangle, direction);
							if (!double.IsNaN(num))
							{
								if (dependencyObject == null && (considerDescendants || !this.IsAncestorOfEx(sourceElement, dependencyObject2)))
								{
									dependencyObject = dependencyObject2;
									targetRect = representativeRectangle;
									value = num;
								}
								else if ((DoubleUtil.LessThan(num, value) || (DoubleUtil.AreClose(num, value) && this.GetDistance(sourceRect, targetRect, direction) > this.GetDistance(sourceRect, representativeRectangle, direction))) && (considerDescendants || !this.IsAncestorOfEx(sourceElement, dependencyObject2)))
								{
									dependencyObject = dependencyObject2;
									targetRect = representativeRectangle;
									value = num;
								}
							}
						}
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x001D90B4 File Offset: 0x001D80B4
		private DependencyObject MoveNext(DependencyObject sourceElement, DependencyObject container, FocusNavigationDirection direction, double startRange, double endRange, bool treeViewNavigation, bool considerDescendants)
		{
			if (container == null)
			{
				container = this.GetGroupParent(sourceElement);
			}
			if (container == sourceElement)
			{
				return null;
			}
			if (this.IsEndlessLoop(sourceElement, container))
			{
				return null;
			}
			KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(container);
			bool flag = sourceElement == null;
			if (keyNavigationMode == KeyboardNavigationMode.None && flag)
			{
				return null;
			}
			Rect sourceRect = flag ? KeyboardNavigation.GetRectangle(container) : this.GetRepresentativeRectangle(sourceElement);
			bool flag2 = direction == FocusNavigationDirection.Right || direction == FocusNavigationDirection.Left;
			this.ResetBaseLines(flag2 ? sourceRect.Top : sourceRect.Left, flag2);
			if (startRange == -1.7976931348623157E+308 || endRange == -1.7976931348623157E+308)
			{
				startRange = (flag2 ? sourceRect.Top : sourceRect.Left);
				endRange = (flag2 ? sourceRect.Bottom : sourceRect.Right);
			}
			if (keyNavigationMode == KeyboardNavigationMode.Once && !flag)
			{
				return this.MoveNext(container, null, direction, startRange, endRange, treeViewNavigation, true);
			}
			DependencyObject dependencyObject = this.FindNextInDirection(sourceElement, sourceRect, container, direction, startRange, endRange, treeViewNavigation, considerDescendants);
			if (dependencyObject == null)
			{
				if (keyNavigationMode == KeyboardNavigationMode.Cycle)
				{
					return this.MoveNext(null, container, direction, startRange, endRange, treeViewNavigation, true);
				}
				if (keyNavigationMode != KeyboardNavigationMode.Contained)
				{
					return this.MoveNext(container, null, direction, startRange, endRange, treeViewNavigation, true);
				}
				return null;
			}
			else
			{
				if (this.IsElementEligible(dependencyObject, treeViewNavigation))
				{
					return dependencyObject;
				}
				DependencyObject activeElementChain = this.GetActiveElementChain(dependencyObject, treeViewNavigation);
				if (activeElementChain != null)
				{
					return activeElementChain;
				}
				DependencyObject dependencyObject2 = this.MoveNext(null, dependencyObject, direction, startRange, endRange, treeViewNavigation, true);
				if (dependencyObject2 != null)
				{
					return dependencyObject2;
				}
				return this.MoveNext(dependencyObject, null, direction, startRange, endRange, treeViewNavigation, true);
			}
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x001D9220 File Offset: 0x001D8220
		private DependencyObject GetActiveElementChain(DependencyObject element, bool treeViewNavigation)
		{
			DependencyObject result = null;
			DependencyObject dependencyObject = element;
			while ((dependencyObject = this.GetActiveElement(dependencyObject)) != null)
			{
				if (this.IsElementEligible(dependencyObject, treeViewNavigation))
				{
					result = dependencyObject;
				}
			}
			return result;
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x001D924C File Offset: 0x001D824C
		private DependencyObject FindElementAtViewportEdge(DependencyObject sourceElement, FrameworkElement viewportBoundsElement, DependencyObject container, FocusNavigationDirection direction, bool treeViewNavigation)
		{
			Rect rect = new Rect(0.0, 0.0, 0.0, 0.0);
			if (sourceElement != null && ItemsControl.GetElementViewportPosition(viewportBoundsElement, ItemsControl.TryGetTreeViewItemHeader(sourceElement) as UIElement, direction, false, out rect) == ElementViewportPosition.None)
			{
				rect = new Rect(0.0, 0.0, 0.0, 0.0);
			}
			DependencyObject dependencyObject = null;
			double value = double.NegativeInfinity;
			double value2 = double.NegativeInfinity;
			DependencyObject dependencyObject2 = null;
			double value3 = double.NegativeInfinity;
			double value4 = double.NegativeInfinity;
			DependencyObject dependencyObject3 = container;
			while ((dependencyObject3 = this.GetNextInTree(dependencyObject3, container)) != null)
			{
				if (this.IsGroupElementEligible(dependencyObject3, treeViewNavigation))
				{
					DependencyObject dependencyObject4 = dependencyObject3;
					if (treeViewNavigation)
					{
						dependencyObject4 = ItemsControl.TryGetTreeViewItemHeader(dependencyObject3);
					}
					Rect rect2;
					ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportBoundsElement, dependencyObject4 as UIElement, direction, false, out rect2);
					if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport || elementViewportPosition == ElementViewportPosition.PartiallyInViewport)
					{
						double num = double.NegativeInfinity;
						switch (direction)
						{
						case FocusNavigationDirection.Left:
							num = -rect2.Left;
							break;
						case FocusNavigationDirection.Right:
							num = rect2.Right;
							break;
						case FocusNavigationDirection.Up:
							num = -rect2.Top;
							break;
						case FocusNavigationDirection.Down:
							num = rect2.Bottom;
							break;
						}
						double num2 = double.NegativeInfinity;
						if (direction - FocusNavigationDirection.Left > 1)
						{
							if (direction - FocusNavigationDirection.Up <= 1)
							{
								num2 = this.ComputeRangeScore(rect.Left, rect.Right, rect2.Left, rect2.Right);
							}
						}
						else
						{
							num2 = this.ComputeRangeScore(rect.Top, rect.Bottom, rect2.Top, rect2.Bottom);
						}
						if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport)
						{
							if (dependencyObject == null || DoubleUtil.GreaterThan(num, value) || (DoubleUtil.AreClose(num, value) && DoubleUtil.GreaterThan(num2, value2)))
							{
								dependencyObject = dependencyObject3;
								value = num;
								value2 = num2;
							}
						}
						else if (dependencyObject2 == null || DoubleUtil.GreaterThan(num, value3) || (DoubleUtil.AreClose(num, value3) && DoubleUtil.GreaterThan(num2, value4)))
						{
							dependencyObject2 = dependencyObject3;
							value3 = num;
							value4 = num2;
						}
					}
				}
			}
			if (dependencyObject == null)
			{
				return dependencyObject2;
			}
			return dependencyObject;
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x001D9471 File Offset: 0x001D8471
		private double ComputeRangeScore(double rangeStart1, double rangeEnd1, double rangeStart2, double rangeEnd2)
		{
			if (DoubleUtil.GreaterThan(rangeStart1, rangeStart2))
			{
				double num = rangeStart1;
				rangeStart1 = rangeStart2;
				rangeStart2 = num;
				double num2 = rangeEnd1;
				rangeEnd1 = rangeEnd2;
				rangeEnd2 = num2;
			}
			if (DoubleUtil.LessThan(rangeEnd1, rangeEnd2))
			{
				return rangeEnd1 - rangeStart2;
			}
			return rangeEnd2 - rangeStart2;
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x001D949C File Offset: 0x001D849C
		private void ProcessForMenuMode(InputEventArgs inputEventArgs)
		{
			if (inputEventArgs.RoutedEvent == Keyboard.LostKeyboardFocusEvent)
			{
				KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs = inputEventArgs as KeyboardFocusChangedEventArgs;
				if ((keyboardFocusChangedEventArgs != null && keyboardFocusChangedEventArgs.NewFocus == null) || inputEventArgs.Handled)
				{
					this._lastKeyPressed = Key.None;
					return;
				}
			}
			else if (inputEventArgs.RoutedEvent == Keyboard.KeyDownEvent)
			{
				if (inputEventArgs.Handled)
				{
					this._lastKeyPressed = Key.None;
					return;
				}
				KeyEventArgs keyEventArgs = inputEventArgs as KeyEventArgs;
				if (!keyEventArgs.IsRepeat)
				{
					if (this._lastKeyPressed == Key.None)
					{
						if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows)) == ModifierKeys.None)
						{
							this._lastKeyPressed = this.GetRealKey(keyEventArgs);
						}
					}
					else
					{
						this._lastKeyPressed = Key.None;
					}
					this._win32MenuModeWorkAround = false;
					return;
				}
			}
			else
			{
				if (inputEventArgs.RoutedEvent == Keyboard.KeyUpEvent)
				{
					if (!inputEventArgs.Handled)
					{
						KeyEventArgs keyEventArgs2 = inputEventArgs as KeyEventArgs;
						Key realKey = this.GetRealKey(keyEventArgs2);
						if (realKey == this._lastKeyPressed && this.IsMenuKey(realKey))
						{
							KeyboardNavigation.EnableKeyboardCues(keyEventArgs2.Source as DependencyObject, true);
							keyEventArgs2.Handled = this.OnEnterMenuMode(keyEventArgs2.Source);
						}
						if (this._win32MenuModeWorkAround)
						{
							if (this.IsMenuKey(realKey))
							{
								this._win32MenuModeWorkAround = false;
								keyEventArgs2.Handled = true;
							}
						}
						else if (keyEventArgs2.Handled)
						{
							this._win32MenuModeWorkAround = true;
						}
					}
					this._lastKeyPressed = Key.None;
					return;
				}
				if (inputEventArgs.RoutedEvent == Mouse.MouseDownEvent || inputEventArgs.RoutedEvent == Mouse.MouseUpEvent)
				{
					this._lastKeyPressed = Key.None;
					this._win32MenuModeWorkAround = false;
				}
			}
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x001D95F6 File Offset: 0x001D85F6
		private bool IsMenuKey(Key key)
		{
			return key == Key.LeftAlt || key == Key.RightAlt || key == Key.F10;
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x001D9609 File Offset: 0x001D8609
		private Key GetRealKey(KeyEventArgs e)
		{
			if (e.Key != Key.System)
			{
				return e.Key;
			}
			return e.SystemKey;
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x001D9628 File Offset: 0x001D8628
		private bool OnEnterMenuMode(object eventSource)
		{
			if (this._weakEnterMenuModeHandlers == null)
			{
				return false;
			}
			KeyboardNavigation.WeakReferenceList weakEnterMenuModeHandlers = this._weakEnterMenuModeHandlers;
			bool flag = false;
			bool result;
			try
			{
				Monitor.Enter(weakEnterMenuModeHandlers, ref flag);
				if (this._weakEnterMenuModeHandlers.Count == 0)
				{
					result = false;
				}
				else
				{
					PresentationSource source = null;
					if (eventSource != null)
					{
						Visual visual = eventSource as Visual;
						source = ((visual != null) ? PresentationSource.CriticalFromVisual(visual) : null);
					}
					else
					{
						IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
						if (activeWindow != IntPtr.Zero)
						{
							source = HwndSource.CriticalFromHwnd(activeWindow);
						}
					}
					if (source == null)
					{
						result = false;
					}
					else
					{
						EventArgs e = EventArgs.Empty;
						bool handled = false;
						this._weakEnterMenuModeHandlers.Process(delegate(object obj)
						{
							KeyboardNavigation.EnterMenuModeEventHandler enterMenuModeEventHandler = obj as KeyboardNavigation.EnterMenuModeEventHandler;
							if (enterMenuModeEventHandler != null && enterMenuModeEventHandler(source, e))
							{
								handled = true;
							}
							return handled;
						});
						result = handled;
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(weakEnterMenuModeHandlers);
				}
			}
			return result;
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060033DB RID: 13275 RVA: 0x001D9710 File Offset: 0x001D8710
		// (remove) Token: 0x060033DC RID: 13276 RVA: 0x001D976C File Offset: 0x001D876C
		internal event KeyboardNavigation.EnterMenuModeEventHandler EnterMenuMode
		{
			add
			{
				if (this._weakEnterMenuModeHandlers == null)
				{
					this._weakEnterMenuModeHandlers = new KeyboardNavigation.WeakReferenceList();
				}
				KeyboardNavigation.WeakReferenceList weakEnterMenuModeHandlers = this._weakEnterMenuModeHandlers;
				lock (weakEnterMenuModeHandlers)
				{
					this._weakEnterMenuModeHandlers.Add(value);
				}
			}
			remove
			{
				if (this._weakEnterMenuModeHandlers != null)
				{
					KeyboardNavigation.WeakReferenceList weakEnterMenuModeHandlers = this._weakEnterMenuModeHandlers;
					lock (weakEnterMenuModeHandlers)
					{
						this._weakEnterMenuModeHandlers.Remove(value);
					}
				}
			}
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x001D97BC File Offset: 0x001D87BC
		private void ProcessForUIState(InputEventArgs inputEventArgs)
		{
			RawUIStateInputReport rawUIStateInputReport = this.ExtractRawUIStateInputReport(inputEventArgs, InputManager.InputReportEvent);
			PresentationSource inputSource;
			if (rawUIStateInputReport != null && (inputSource = rawUIStateInputReport.InputSource) != null && (rawUIStateInputReport.Targets & RawUIStateTargets.HideAccelerators) != RawUIStateTargets.None)
			{
				DependencyObject rootVisual = inputSource.RootVisual;
				bool enable = rawUIStateInputReport.Action == RawUIStateActions.Clear;
				KeyboardNavigation.EnableKeyboardCues(rootVisual, enable);
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x001D9804 File Offset: 0x001D8804
		private RawUIStateInputReport ExtractRawUIStateInputReport(InputEventArgs e, RoutedEvent Event)
		{
			RawUIStateInputReport result = null;
			InputReportEventArgs inputReportEventArgs = e as InputReportEventArgs;
			if (inputReportEventArgs != null && inputReportEventArgs.Report.Type == InputType.Keyboard && inputReportEventArgs.RoutedEvent == Event)
			{
				result = (inputReportEventArgs.Report as RawUIStateInputReport);
			}
			return result;
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060033DF RID: 13279 RVA: 0x001D9840 File Offset: 0x001D8840
		// (remove) Token: 0x060033E0 RID: 13280 RVA: 0x001D9888 File Offset: 0x001D8888
		internal event EventHandler FocusEnterMainFocusScope
		{
			add
			{
				KeyboardNavigation.WeakReferenceList weakFocusEnterMainFocusScopeHandlers = this._weakFocusEnterMainFocusScopeHandlers;
				lock (weakFocusEnterMainFocusScopeHandlers)
				{
					this._weakFocusEnterMainFocusScopeHandlers.Add(value);
				}
			}
			remove
			{
				KeyboardNavigation.WeakReferenceList weakFocusEnterMainFocusScopeHandlers = this._weakFocusEnterMainFocusScopeHandlers;
				lock (weakFocusEnterMainFocusScopeHandlers)
				{
					this._weakFocusEnterMainFocusScopeHandlers.Remove(value);
				}
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x001D98D0 File Offset: 0x001D88D0
		private void NotifyFocusEnterMainFocusScope(object sender, EventArgs e)
		{
			this._weakFocusEnterMainFocusScopeHandlers.Process(delegate(object item)
			{
				EventHandler eventHandler = item as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(sender, e);
				}
				return false;
			});
		}

		// Token: 0x04001C2B RID: 7211
		private static readonly DependencyProperty TabOnceActiveElementProperty = DependencyProperty.RegisterAttached("TabOnceActiveElement", typeof(WeakReference), typeof(KeyboardNavigation));

		// Token: 0x04001C2C RID: 7212
		internal static readonly DependencyProperty ControlTabOnceActiveElementProperty = DependencyProperty.RegisterAttached("ControlTabOnceActiveElement", typeof(WeakReference), typeof(KeyboardNavigation));

		// Token: 0x04001C2D RID: 7213
		internal static readonly DependencyProperty DirectionalNavigationMarginProperty = DependencyProperty.RegisterAttached("DirectionalNavigationMargin", typeof(Thickness), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(default(Thickness)));

		// Token: 0x04001C2E RID: 7214
		public static readonly DependencyProperty TabIndexProperty = DependencyProperty.RegisterAttached("TabIndex", typeof(int), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(int.MaxValue));

		// Token: 0x04001C2F RID: 7215
		public static readonly DependencyProperty IsTabStopProperty = DependencyProperty.RegisterAttached("IsTabStop", typeof(bool), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x04001C30 RID: 7216
		[CommonDependencyProperty]
		[Localizability(LocalizationCategory.NeverLocalize)]
		[CustomCategory("Accessibility")]
		public static readonly DependencyProperty TabNavigationProperty = DependencyProperty.RegisterAttached("TabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue), new ValidateValueCallback(KeyboardNavigation.IsValidKeyNavigationMode));

		// Token: 0x04001C31 RID: 7217
		[Localizability(LocalizationCategory.NeverLocalize)]
		[CustomCategory("Accessibility")]
		[CommonDependencyProperty]
		public static readonly DependencyProperty ControlTabNavigationProperty = DependencyProperty.RegisterAttached("ControlTabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue), new ValidateValueCallback(KeyboardNavigation.IsValidKeyNavigationMode));

		// Token: 0x04001C32 RID: 7218
		[CustomCategory("Accessibility")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		[CommonDependencyProperty]
		public static readonly DependencyProperty DirectionalNavigationProperty = DependencyProperty.RegisterAttached("DirectionalNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue), new ValidateValueCallback(KeyboardNavigation.IsValidKeyNavigationMode));

		// Token: 0x04001C33 RID: 7219
		internal static readonly DependencyProperty ShowKeyboardCuesProperty = DependencyProperty.RegisterAttached("ShowKeyboardCues", typeof(bool), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior, null, new CoerceValueCallback(KeyboardNavigation.CoerceShowKeyboardCues)));

		// Token: 0x04001C34 RID: 7220
		public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.RegisterAttached("AcceptsReturn", typeof(bool), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04001C35 RID: 7221
		private KeyboardNavigation.WeakReferenceList _weakFocusChangedHandlers = new KeyboardNavigation.WeakReferenceList();

		// Token: 0x04001C36 RID: 7222
		private static bool _alwaysShowFocusVisual = SystemParameters.KeyboardCues;

		// Token: 0x04001C37 RID: 7223
		private KeyboardNavigation.FocusVisualAdorner _focusVisualAdornerCache;

		// Token: 0x04001C38 RID: 7224
		private Key _lastKeyPressed;

		// Token: 0x04001C39 RID: 7225
		private KeyboardNavigation.WeakReferenceList _weakEnterMenuModeHandlers;

		// Token: 0x04001C3A RID: 7226
		private bool _win32MenuModeWorkAround;

		// Token: 0x04001C3B RID: 7227
		private KeyboardNavigation.WeakReferenceList _weakFocusEnterMainFocusScopeHandlers = new KeyboardNavigation.WeakReferenceList();

		// Token: 0x04001C3C RID: 7228
		private const double BASELINE_DEFAULT = -1.7976931348623157E+308;

		// Token: 0x04001C3D RID: 7229
		private double _verticalBaseline = double.MinValue;

		// Token: 0x04001C3E RID: 7230
		private double _horizontalBaseline = double.MinValue;

		// Token: 0x04001C3F RID: 7231
		private DependencyProperty _navigationProperty;

		// Token: 0x04001C40 RID: 7232
		private Hashtable _containerHashtable = new Hashtable(10);

		// Token: 0x04001C41 RID: 7233
		private static object _fakeNull = new object();

		// Token: 0x02000ABA RID: 2746
		private sealed class FocusVisualAdorner : Adorner
		{
			// Token: 0x06008AC3 RID: 35523 RVA: 0x0033836C File Offset: 0x0033736C
			public FocusVisualAdorner(UIElement adornedElement, Style focusVisualStyle) : base(adornedElement)
			{
				this._adorderChild = new Control
				{
					Style = focusVisualStyle
				};
				base.IsClipEnabled = true;
				base.IsHitTestVisible = false;
				base.IsEnabled = false;
				base.AddVisualChild(this._adorderChild);
			}

			// Token: 0x06008AC4 RID: 35524 RVA: 0x003383C0 File Offset: 0x003373C0
			public FocusVisualAdorner(ContentElement adornedElement, UIElement adornedElementParent, IContentHost contentHostParent, Style focusVisualStyle) : base(adornedElementParent)
			{
				this._contentHostParent = contentHostParent;
				this._adornedContentElement = adornedElement;
				this._focusVisualStyle = focusVisualStyle;
				Canvas canvas = new Canvas();
				this._canvasChildren = canvas.Children;
				this._adorderChild = canvas;
				base.AddVisualChild(this._adorderChild);
				base.IsClipEnabled = true;
				base.IsHitTestVisible = false;
				base.IsEnabled = false;
			}

			// Token: 0x06008AC5 RID: 35525 RVA: 0x00338430 File Offset: 0x00337430
			protected override Size MeasureOverride(Size constraint)
			{
				Size size = default(Size);
				if (this._adornedContentElement == null)
				{
					size = base.AdornedElement.RenderSize;
					constraint = size;
				}
				((UIElement)this.GetVisualChild(0)).Measure(constraint);
				return size;
			}

			// Token: 0x06008AC6 RID: 35526 RVA: 0x00338470 File Offset: 0x00337470
			protected override Size ArrangeOverride(Size size)
			{
				Size size2 = base.ArrangeOverride(size);
				if (this._adornedContentElement != null)
				{
					if (this._contentRects == null)
					{
						this._canvasChildren.Clear();
					}
					else
					{
						IContentHost contentHost = this.ContentHost;
						if (!(contentHost is Visual) || !base.AdornedElement.IsAncestorOf((Visual)contentHost))
						{
							this._canvasChildren.Clear();
							return default(Size);
						}
						Rect empty = Rect.Empty;
						IEnumerator<Rect> enumerator = this._contentRects.GetEnumerator();
						if (this._canvasChildren.Count == this._contentRects.Count)
						{
							for (int i = 0; i < this._canvasChildren.Count; i++)
							{
								enumerator.MoveNext();
								Rect rect = enumerator.Current;
								rect = this._hostToAdornedElement.TransformBounds(rect);
								Control control = (Control)this._canvasChildren[i];
								control.Width = rect.Width;
								control.Height = rect.Height;
								Canvas.SetLeft(control, rect.X);
								Canvas.SetTop(control, rect.Y);
							}
							this._adorderChild.InvalidateArrange();
						}
						else
						{
							this._canvasChildren.Clear();
							while (enumerator.MoveNext())
							{
								Rect rect2 = enumerator.Current;
								rect2 = this._hostToAdornedElement.TransformBounds(rect2);
								Control control2 = new Control();
								control2.Style = this._focusVisualStyle;
								control2.Width = rect2.Width;
								control2.Height = rect2.Height;
								Canvas.SetLeft(control2, rect2.X);
								Canvas.SetTop(control2, rect2.Y);
								this._canvasChildren.Add(control2);
							}
						}
					}
				}
				((UIElement)this.GetVisualChild(0)).Arrange(new Rect(default(Point), size2));
				return size2;
			}

			// Token: 0x17001E5F RID: 7775
			// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			protected override int VisualChildrenCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x06008AC8 RID: 35528 RVA: 0x00338641 File Offset: 0x00337641
			protected override Visual GetVisualChild(int index)
			{
				if (index == 0)
				{
					return this._adorderChild;
				}
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}

			// Token: 0x17001E60 RID: 7776
			// (get) Token: 0x06008AC9 RID: 35529 RVA: 0x00338667 File Offset: 0x00337667
			private IContentHost ContentHost
			{
				get
				{
					if (this._adornedContentElement != null && (this._contentHostParent == null || VisualTreeHelper.GetParent(this._contentHostParent as Visual) == null))
					{
						this._contentHostParent = ContentHostHelper.FindContentHost(this._adornedContentElement);
					}
					return this._contentHostParent;
				}
			}

			// Token: 0x06008ACA RID: 35530 RVA: 0x003386A4 File Offset: 0x003376A4
			internal override bool NeedsUpdate(Size oldSize)
			{
				if (this._adornedContentElement == null)
				{
					return !DoubleUtil.AreClose(base.AdornedElement.RenderSize, oldSize);
				}
				ReadOnlyCollection<Rect> contentRects = this._contentRects;
				this._contentRects = null;
				IContentHost contentHost = this.ContentHost;
				if (contentHost != null)
				{
					this._contentRects = contentHost.GetRectangles(this._adornedContentElement);
				}
				GeneralTransform hostToAdornedElement = this._hostToAdornedElement;
				if (contentHost is Visual && base.AdornedElement.IsAncestorOf((Visual)contentHost))
				{
					this._hostToAdornedElement = ((Visual)contentHost).TransformToAncestor(base.AdornedElement);
				}
				else
				{
					this._hostToAdornedElement = Transform.Identity;
				}
				if (hostToAdornedElement != this._hostToAdornedElement && (!(hostToAdornedElement is MatrixTransform) || !(this._hostToAdornedElement is MatrixTransform) || !Matrix.Equals(((MatrixTransform)hostToAdornedElement).Matrix, ((MatrixTransform)this._hostToAdornedElement).Matrix)))
				{
					return true;
				}
				if (this._contentRects != null && contentRects != null && this._contentRects.Count == contentRects.Count)
				{
					for (int i = 0; i < contentRects.Count; i++)
					{
						if (!DoubleUtil.AreClose(contentRects[i].Size, this._contentRects[i].Size))
						{
							return true;
						}
					}
					return false;
				}
				return this._contentRects != contentRects;
			}

			// Token: 0x04004636 RID: 17974
			private GeneralTransform _hostToAdornedElement = Transform.Identity;

			// Token: 0x04004637 RID: 17975
			private IContentHost _contentHostParent;

			// Token: 0x04004638 RID: 17976
			private ContentElement _adornedContentElement;

			// Token: 0x04004639 RID: 17977
			private Style _focusVisualStyle;

			// Token: 0x0400463A RID: 17978
			private UIElement _adorderChild;

			// Token: 0x0400463B RID: 17979
			private UIElementCollection _canvasChildren;

			// Token: 0x0400463C RID: 17980
			private ReadOnlyCollection<Rect> _contentRects;
		}

		// Token: 0x02000ABB RID: 2747
		// (Invoke) Token: 0x06008ACC RID: 35532
		internal delegate bool EnterMenuModeEventHandler(object sender, EventArgs e);

		// Token: 0x02000ABC RID: 2748
		private class WeakReferenceList : DispatcherObject
		{
			// Token: 0x17001E61 RID: 7777
			// (get) Token: 0x06008ACF RID: 35535 RVA: 0x003387EE File Offset: 0x003377EE
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x06008AD0 RID: 35536 RVA: 0x003387FB File Offset: 0x003377FB
			public void Add(object item)
			{
				if (this._list.Count == this._list.Capacity)
				{
					this.Purge();
				}
				this._list.Add(new WeakReference(item));
			}

			// Token: 0x06008AD1 RID: 35537 RVA: 0x0033882C File Offset: 0x0033782C
			public void Remove(object target)
			{
				bool flag = false;
				for (int i = 0; i < this._list.Count; i++)
				{
					object target2 = this._list[i].Target;
					if (target2 != null)
					{
						if (target2 == target)
						{
							this._list.RemoveAt(i);
							i--;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.Purge();
				}
			}

			// Token: 0x06008AD2 RID: 35538 RVA: 0x00338888 File Offset: 0x00337888
			public void Process(Func<object, bool> action)
			{
				bool flag = false;
				for (int i = 0; i < this._list.Count; i++)
				{
					object target = this._list[i].Target;
					if (target != null)
					{
						if (action(target))
						{
							break;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.ScheduleCleanup();
				}
			}

			// Token: 0x06008AD3 RID: 35539 RVA: 0x003388D8 File Offset: 0x003378D8
			private void Purge()
			{
				int num = 0;
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					if (this._list[i].IsAlive)
					{
						this._list[num++] = this._list[i];
					}
				}
				if (num < count)
				{
					this._list.RemoveRange(num, count - num);
					int num2 = num << 1;
					if (num2 < this._list.Capacity)
					{
						this._list.Capacity = num2;
					}
				}
			}

			// Token: 0x06008AD4 RID: 35540 RVA: 0x0033895F File Offset: 0x0033795F
			private void ScheduleCleanup()
			{
				if (!this._isCleanupRequested)
				{
					this._isCleanupRequested = true;
					base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(delegate(object unused)
					{
						lock (this)
						{
							this.Purge();
							this._isCleanupRequested = false;
						}
						return null;
					}), null);
				}
			}

			// Token: 0x0400463D RID: 17981
			private List<WeakReference> _list = new List<WeakReference>(1);

			// Token: 0x0400463E RID: 17982
			private bool _isCleanupRequested;
		}
	}
}
