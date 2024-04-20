using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007F8 RID: 2040
	public static class Validation
	{
		// Token: 0x060076BE RID: 30398 RVA: 0x002F04A6 File Offset: 0x002EF4A6
		public static void AddErrorHandler(DependencyObject element, EventHandler<ValidationErrorEventArgs> handler)
		{
			UIElement.AddHandler(element, Validation.ErrorEvent, handler);
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x002F04B4 File Offset: 0x002EF4B4
		public static void RemoveErrorHandler(DependencyObject element, EventHandler<ValidationErrorEventArgs> handler)
		{
			UIElement.RemoveHandler(element, Validation.ErrorEvent, handler);
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x002F04C2 File Offset: 0x002EF4C2
		public static ReadOnlyObservableCollection<ValidationError> GetErrors(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ReadOnlyObservableCollection<ValidationError>)element.GetValue(Validation.ErrorsProperty);
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x002F04E4 File Offset: 0x002EF4E4
		private static void OnErrorsInternalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ValidationErrorCollection validationErrorCollection = e.NewValue as ValidationErrorCollection;
			if (validationErrorCollection != null)
			{
				d.SetValue(Validation.ErrorsPropertyKey, new ReadOnlyObservableCollection<ValidationError>(validationErrorCollection));
				return;
			}
			d.ClearValue(Validation.ErrorsPropertyKey);
		}

		// Token: 0x060076C2 RID: 30402 RVA: 0x002F051E File Offset: 0x002EF51E
		internal static ValidationErrorCollection GetErrorsInternal(DependencyObject target)
		{
			return (ValidationErrorCollection)target.GetValue(Validation.ValidationErrorsInternalProperty);
		}

		// Token: 0x060076C3 RID: 30403 RVA: 0x002F0530 File Offset: 0x002EF530
		private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = d as Control;
			if (control != null)
			{
				Control.OnVisualStatePropertyChanged(control, e);
			}
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x002F054E File Offset: 0x002EF54E
		public static bool GetHasError(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Validation.HasErrorProperty);
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x002F056E File Offset: 0x002EF56E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static ControlTemplate GetErrorTemplate(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(Validation.ErrorTemplateProperty) as ControlTemplate;
		}

		// Token: 0x060076C6 RID: 30406 RVA: 0x002F058E File Offset: 0x002EF58E
		public static void SetErrorTemplate(DependencyObject element, ControlTemplate value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!object.Equals(element.ReadLocalValue(Validation.ErrorTemplateProperty), value))
			{
				element.SetValue(Validation.ErrorTemplateProperty, value);
			}
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x002F05BD File Offset: 0x002EF5BD
		private static void OnErrorTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (Validation.GetHasError(d))
			{
				Validation.ShowValidationAdorner(d, false);
				Validation.ShowValidationAdorner(d, true);
			}
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x002F05D5 File Offset: 0x002EF5D5
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static DependencyObject GetValidationAdornerSite(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(Validation.ValidationAdornerSiteProperty) as DependencyObject;
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x002F05F5 File Offset: 0x002EF5F5
		public static void SetValidationAdornerSite(DependencyObject element, DependencyObject value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Validation.ValidationAdornerSiteProperty, value);
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x002F0614 File Offset: 0x002EF614
		private static void OnValidationAdornerSiteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			DependencyObject dependencyObject = (DependencyObject)e.OldValue;
			DependencyObject dependencyObject2 = (DependencyObject)e.NewValue;
			if (dependencyObject != null)
			{
				dependencyObject.ClearValue(Validation.ValidationAdornerSiteForProperty);
			}
			if (dependencyObject2 != null && d != Validation.GetValidationAdornerSiteFor(dependencyObject2))
			{
				Validation.SetValidationAdornerSiteFor(dependencyObject2, d);
			}
			if (Validation.GetHasError(d))
			{
				if (dependencyObject == null)
				{
					dependencyObject = d;
				}
				Validation.ShowValidationAdornerHelper(d, dependencyObject, false);
				Validation.ShowValidationAdorner(d, true);
			}
		}

		// Token: 0x060076CB RID: 30411 RVA: 0x002F0682 File Offset: 0x002EF682
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static DependencyObject GetValidationAdornerSiteFor(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(Validation.ValidationAdornerSiteForProperty) as DependencyObject;
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x002F06A2 File Offset: 0x002EF6A2
		public static void SetValidationAdornerSiteFor(DependencyObject element, DependencyObject value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Validation.ValidationAdornerSiteForProperty, value);
		}

		// Token: 0x060076CD RID: 30413 RVA: 0x002F06C0 File Offset: 0x002EF6C0
		private static void OnValidationAdornerSiteForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			DependencyObject dependencyObject = (DependencyObject)e.OldValue;
			DependencyObject dependencyObject2 = (DependencyObject)e.NewValue;
			if (dependencyObject != null)
			{
				dependencyObject.ClearValue(Validation.ValidationAdornerSiteProperty);
			}
			if (dependencyObject2 != null && d != Validation.GetValidationAdornerSite(dependencyObject2))
			{
				Validation.SetValidationAdornerSite(dependencyObject2, d);
			}
		}

		// Token: 0x060076CE RID: 30414 RVA: 0x002F0714 File Offset: 0x002EF714
		internal static void ShowValidationAdorner(DependencyObject targetElement, bool show)
		{
			if (!Validation.HasValidationGroup(targetElement as FrameworkElement))
			{
				DependencyObject dependencyObject = Validation.GetValidationAdornerSite(targetElement);
				if (dependencyObject == null)
				{
					dependencyObject = targetElement;
				}
				Validation.ShowValidationAdornerHelper(targetElement, dependencyObject, show);
			}
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x002F0742 File Offset: 0x002EF742
		private static bool HasValidationGroup(FrameworkElement fe)
		{
			if (fe != null)
			{
				if (Validation.HasValidationGroup(VisualStateManager.GetVisualStateGroupsInternal(fe)))
				{
					return true;
				}
				if (fe.StateGroupsRoot != null)
				{
					return Validation.HasValidationGroup(VisualStateManager.GetVisualStateGroupsInternal(fe.StateGroupsRoot));
				}
			}
			return false;
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x002F0770 File Offset: 0x002EF770
		private static bool HasValidationGroup(IList<VisualStateGroup> groups)
		{
			if (groups != null)
			{
				for (int i = 0; i < groups.Count; i++)
				{
					if (groups[i].Name == "ValidationStates")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060076D1 RID: 30417 RVA: 0x002F07AC File Offset: 0x002EF7AC
		private static void ShowValidationAdornerHelper(DependencyObject targetElement, DependencyObject adornerSite, bool show)
		{
			Validation.ShowValidationAdornerHelper(targetElement, adornerSite, show, true);
		}

		// Token: 0x060076D2 RID: 30418 RVA: 0x002F07B8 File Offset: 0x002EF7B8
		private static object ShowValidationAdornerOperation(object arg)
		{
			object[] array = (object[])arg;
			DependencyObject targetElement = (DependencyObject)array[0];
			DependencyObject adornerSite = (DependencyObject)array[1];
			bool show = (bool)array[2];
			Validation.ShowValidationAdornerHelper(targetElement, adornerSite, show, false);
			return null;
		}

		// Token: 0x060076D3 RID: 30419 RVA: 0x002F07F0 File Offset: 0x002EF7F0
		private static void ShowValidationAdornerHelper(DependencyObject targetElement, DependencyObject adornerSite, bool show, bool tryAgain)
		{
			UIElement uielement = adornerSite as UIElement;
			if (uielement != null)
			{
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(uielement);
				if (adornerLayer == null)
				{
					if (tryAgain)
					{
						adornerSite.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(Validation.ShowValidationAdornerOperation), new object[]
						{
							targetElement,
							adornerSite,
							show
						});
					}
					return;
				}
				TemplatedAdorner templatedAdorner = uielement.ReadLocalValue(Validation.ValidationAdornerProperty) as TemplatedAdorner;
				if (show && templatedAdorner == null)
				{
					ControlTemplate errorTemplate = Validation.GetErrorTemplate(uielement);
					if (errorTemplate == null)
					{
						errorTemplate = Validation.GetErrorTemplate(targetElement);
					}
					if (errorTemplate != null)
					{
						templatedAdorner = new TemplatedAdorner(uielement, errorTemplate);
						adornerLayer.Add(templatedAdorner);
						uielement.SetValue(Validation.ValidationAdornerProperty, templatedAdorner);
						return;
					}
				}
				else if (!show && templatedAdorner != null)
				{
					templatedAdorner.ClearChild();
					adornerLayer.Remove(templatedAdorner);
					uielement.ClearValue(Validation.ValidationAdornerProperty);
				}
			}
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x002F08AD File Offset: 0x002EF8AD
		public static void MarkInvalid(BindingExpressionBase bindingExpression, ValidationError validationError)
		{
			if (bindingExpression == null)
			{
				throw new ArgumentNullException("bindingExpression");
			}
			if (validationError == null)
			{
				throw new ArgumentNullException("validationError");
			}
			bindingExpression.UpdateValidationError(validationError, false);
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x002F08D3 File Offset: 0x002EF8D3
		public static void ClearInvalid(BindingExpressionBase bindingExpression)
		{
			if (bindingExpression == null)
			{
				throw new ArgumentNullException("bindingExpression");
			}
			bindingExpression.UpdateValidationError(null, false);
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x002F08EC File Offset: 0x002EF8EC
		internal static void AddValidationError(ValidationError validationError, DependencyObject targetElement, bool shouldRaiseEvent)
		{
			if (targetElement == null)
			{
				return;
			}
			ValidationErrorCollection validationErrorCollection = Validation.GetErrorsInternal(targetElement);
			bool flag;
			if (validationErrorCollection == null)
			{
				flag = true;
				validationErrorCollection = new ValidationErrorCollection();
				validationErrorCollection.Add(validationError);
				targetElement.SetValue(Validation.ValidationErrorsInternalProperty, validationErrorCollection);
			}
			else
			{
				flag = (validationErrorCollection.Count == 0);
				validationErrorCollection.Add(validationError);
			}
			if (flag)
			{
				targetElement.SetValue(Validation.HasErrorPropertyKey, BooleanBoxes.TrueBox);
			}
			if (shouldRaiseEvent)
			{
				Validation.OnValidationError(targetElement, validationError, ValidationErrorEventAction.Added);
			}
			if (flag)
			{
				Validation.ShowValidationAdorner(targetElement, true);
			}
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x002F0960 File Offset: 0x002EF960
		internal static void RemoveValidationError(ValidationError validationError, DependencyObject targetElement, bool shouldRaiseEvent)
		{
			if (targetElement == null)
			{
				return;
			}
			ValidationErrorCollection errorsInternal = Validation.GetErrorsInternal(targetElement);
			if (errorsInternal == null || errorsInternal.Count == 0 || !errorsInternal.Contains(validationError))
			{
				return;
			}
			if (errorsInternal.Count == 1)
			{
				targetElement.ClearValue(Validation.HasErrorPropertyKey);
				targetElement.ClearValue(Validation.ValidationErrorsInternalProperty);
				if (shouldRaiseEvent)
				{
					Validation.OnValidationError(targetElement, validationError, ValidationErrorEventAction.Removed);
				}
				Validation.ShowValidationAdorner(targetElement, false);
				return;
			}
			errorsInternal.Remove(validationError);
			if (shouldRaiseEvent)
			{
				Validation.OnValidationError(targetElement, validationError, ValidationErrorEventAction.Removed);
			}
		}

		// Token: 0x060076D8 RID: 30424 RVA: 0x002F09D4 File Offset: 0x002EF9D4
		private static void OnValidationError(DependencyObject source, ValidationError validationError, ValidationErrorEventAction action)
		{
			ValidationErrorEventArgs e = new ValidationErrorEventArgs(validationError, action);
			if (source is ContentElement)
			{
				((ContentElement)source).RaiseEvent(e);
				return;
			}
			if (source is UIElement)
			{
				((UIElement)source).RaiseEvent(e);
				return;
			}
			if (source is UIElement3D)
			{
				((UIElement3D)source).RaiseEvent(e);
			}
		}

		// Token: 0x060076D9 RID: 30425 RVA: 0x002F0A28 File Offset: 0x002EFA28
		private static ControlTemplate CreateDefaultErrorTemplate()
		{
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Control));
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Border), "Border");
			frameworkElementFactory.SetValue(Border.BorderBrushProperty, Brushes.Red);
			frameworkElementFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1.0));
			FrameworkElementFactory child = new FrameworkElementFactory(typeof(AdornedElementPlaceholder), "Placeholder");
			frameworkElementFactory.AppendChild(child);
			controlTemplate.VisualTree = frameworkElementFactory;
			controlTemplate.Seal();
			return controlTemplate;
		}

		// Token: 0x040038A1 RID: 14497
		public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent("ValidationError", RoutingStrategy.Bubble, typeof(EventHandler<ValidationErrorEventArgs>), typeof(Validation));

		// Token: 0x040038A2 RID: 14498
		internal static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Errors", typeof(ReadOnlyObservableCollection<ValidationError>), typeof(Validation), new FrameworkPropertyMetadata(ValidationErrorCollection.Empty, FrameworkPropertyMetadataOptions.NotDataBindable));

		// Token: 0x040038A3 RID: 14499
		public static readonly DependencyProperty ErrorsProperty = Validation.ErrorsPropertyKey.DependencyProperty;

		// Token: 0x040038A4 RID: 14500
		internal static readonly DependencyProperty ValidationErrorsInternalProperty = DependencyProperty.RegisterAttached("ErrorsInternal", typeof(ValidationErrorCollection), typeof(Validation), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Validation.OnErrorsInternalChanged)));

		// Token: 0x040038A5 RID: 14501
		internal static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasError", typeof(bool), typeof(Validation), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(Validation.OnHasErrorChanged)));

		// Token: 0x040038A6 RID: 14502
		public static readonly DependencyProperty HasErrorProperty = Validation.HasErrorPropertyKey.DependencyProperty;

		// Token: 0x040038A7 RID: 14503
		public static readonly DependencyProperty ErrorTemplateProperty = DependencyProperty.RegisterAttached("ErrorTemplate", typeof(ControlTemplate), typeof(Validation), new FrameworkPropertyMetadata(Validation.CreateDefaultErrorTemplate(), FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(Validation.OnErrorTemplateChanged)));

		// Token: 0x040038A8 RID: 14504
		public static readonly DependencyProperty ValidationAdornerSiteProperty = DependencyProperty.RegisterAttached("ValidationAdornerSite", typeof(DependencyObject), typeof(Validation), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Validation.OnValidationAdornerSiteChanged)));

		// Token: 0x040038A9 RID: 14505
		public static readonly DependencyProperty ValidationAdornerSiteForProperty = DependencyProperty.RegisterAttached("ValidationAdornerSiteFor", typeof(DependencyObject), typeof(Validation), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Validation.OnValidationAdornerSiteForChanged)));

		// Token: 0x040038AA RID: 14506
		private static readonly DependencyProperty ValidationAdornerProperty = DependencyProperty.RegisterAttached("ValidationAdorner", typeof(TemplatedAdorner), typeof(Validation), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.NotDataBindable));
	}
}
