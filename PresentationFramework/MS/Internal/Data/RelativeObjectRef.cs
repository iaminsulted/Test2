using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace MS.Internal.Data
{
	// Token: 0x02000233 RID: 563
	internal sealed class RelativeObjectRef : ObjectRef
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x00155150 File Offset: 0x00154150
		internal RelativeObjectRef(RelativeSource relativeSource)
		{
			if (relativeSource == null)
			{
				throw new ArgumentNullException("relativeSource");
			}
			this._relativeSource = relativeSource;
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00155170 File Offset: 0x00154170
		public override string ToString()
		{
			string result;
			if (this._relativeSource.Mode == RelativeSourceMode.FindAncestor)
			{
				result = string.Format(CultureInfo.InvariantCulture, "RelativeSource {0}, AncestorType='{1}', AncestorLevel='{2}'", this._relativeSource.Mode, this._relativeSource.AncestorType, this._relativeSource.AncestorLevel);
			}
			else
			{
				result = string.Format(CultureInfo.InvariantCulture, "RelativeSource {0}", this._relativeSource.Mode);
			}
			return result;
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x001551E9 File Offset: 0x001541E9
		internal override object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			return this.GetDataObjectImpl(d, args);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x001551F4 File Offset: 0x001541F4
		internal override object GetDataObject(DependencyObject d, ObjectRefArgs args)
		{
			object obj = this.GetDataObjectImpl(d, args);
			DependencyObject dependencyObject = obj as DependencyObject;
			if (dependencyObject != null && this.ReturnsDataContext)
			{
				obj = dependencyObject.GetValue(ItemContainerGenerator.ItemForItemContainerProperty);
				if (obj == null)
				{
					obj = dependencyObject.GetValue(FrameworkElement.DataContextProperty);
				}
			}
			return obj;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00155238 File Offset: 0x00154238
		private object GetDataObjectImpl(DependencyObject d, ObjectRefArgs args)
		{
			if (d == null)
			{
				return null;
			}
			switch (this._relativeSource.Mode)
			{
			case RelativeSourceMode.PreviousData:
				return this.GetPreviousData(d);
			case RelativeSourceMode.TemplatedParent:
				d = Helper.GetTemplatedParent(d);
				break;
			case RelativeSourceMode.Self:
				break;
			case RelativeSourceMode.FindAncestor:
				d = this.FindAncestorOfType(this._relativeSource.AncestorType, this._relativeSource.AncestorLevel, d, args.IsTracing);
				if (d == null)
				{
					return DependencyProperty.UnsetValue;
				}
				break;
			default:
				return null;
			}
			if (args.IsTracing)
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.RelativeSource(new object[]
				{
					this._relativeSource.Mode,
					TraceData.Identify(d)
				}), null);
			}
			return d;
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x001552E7 File Offset: 0x001542E7
		internal bool ReturnsDataContext
		{
			get
			{
				return this._relativeSource.Mode == RelativeSourceMode.PreviousData;
			}
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x001552F7 File Offset: 0x001542F7
		protected override bool ProtectedTreeContextIsRequired(DependencyObject target)
		{
			return this._relativeSource.Mode == RelativeSourceMode.FindAncestor || this._relativeSource.Mode == RelativeSourceMode.PreviousData;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x00155318 File Offset: 0x00154318
		protected override bool ProtectedUsesMentor
		{
			get
			{
				RelativeSourceMode mode = this._relativeSource.Mode;
				return mode <= RelativeSourceMode.TemplatedParent;
			}
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00155338 File Offset: 0x00154338
		internal override string Identify()
		{
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "RelativeSource ({0})", this._relativeSource.Mode);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0015535C File Offset: 0x0015435C
		private object GetPreviousData(DependencyObject d)
		{
			while (d != null)
			{
				if (BindingExpression.HasLocalDataContext(d))
				{
					ContentPresenter contentPresenter;
					FrameworkElement frameworkElement;
					FrameworkElement frameworkElement2;
					if ((contentPresenter = (d as ContentPresenter)) != null)
					{
						frameworkElement = contentPresenter;
						frameworkElement2 = (contentPresenter.TemplatedParent as FrameworkElement);
						if (!(frameworkElement2 is ContentControl) && !(frameworkElement2 is HeaderedItemsControl))
						{
							frameworkElement2 = (contentPresenter.Parent as GridViewRowPresenterBase);
						}
					}
					else
					{
						frameworkElement = (d as FrameworkElement);
						frameworkElement2 = (((frameworkElement != null) ? frameworkElement.Parent : null) as GridViewRowPresenterBase);
					}
					if (frameworkElement == null || frameworkElement2 == null || !ItemsControl.EqualsEx(frameworkElement.DataContext, frameworkElement2.DataContext))
					{
						break;
					}
					d = frameworkElement2;
					if (BindingExpression.HasLocalDataContext(frameworkElement2))
					{
						break;
					}
				}
				d = FrameworkElement.GetFrameworkParent(d);
			}
			if (d == null)
			{
				return DependencyProperty.UnsetValue;
			}
			Visual visual = d as Visual;
			DependencyObject dependencyObject = (visual != null) ? VisualTreeHelper.GetParent(visual) : null;
			if (ItemsControl.GetItemsOwner(dependencyObject) == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.RefPreviousNotInContext, null);
				}
				return null;
			}
			Visual visual2 = dependencyObject as Visual;
			bool flag = visual2 != null && visual2.InternalVisualChildrenCount != 0;
			int num = -1;
			Visual visual3 = null;
			if (flag)
			{
				num = this.IndexOf(visual2, visual, out visual3);
			}
			if (num > 0)
			{
				d = visual3;
			}
			else
			{
				d = null;
				if (num < 0 && TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.RefNoWrapperInChildren, null);
				}
			}
			return d;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00155494 File Offset: 0x00154494
		private DependencyObject FindAncestorOfType(Type type, int level, DependencyObject d, bool isTracing)
		{
			if (type == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.RefAncestorTypeNotSpecified, null);
				}
				return null;
			}
			if (level < 1)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.RefAncestorLevelInvalid, null);
				}
				return null;
			}
			FrameworkObject frameworkObject = new FrameworkObject(d);
			frameworkObject.Reset(frameworkObject.GetPreferVisualParent(true).DO);
			while (frameworkObject.DO != null)
			{
				if (isTracing)
				{
					TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.AncestorLookup(new object[]
					{
						type.Name,
						TraceData.Identify(frameworkObject.DO)
					}), null);
				}
				if (type.IsInstanceOfType(frameworkObject.DO) && --level <= 0)
				{
					break;
				}
				frameworkObject.Reset(frameworkObject.PreferVisualParent.DO);
			}
			return frameworkObject.DO;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00155568 File Offset: 0x00154568
		private int IndexOf(Visual parent, Visual child, out Visual prevChild)
		{
			bool flag = false;
			prevChild = null;
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			int i;
			for (i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = parent.InternalGetVisualChild(i);
				if (child == visual)
				{
					flag = true;
					break;
				}
				prevChild = visual;
			}
			if (flag)
			{
				return i;
			}
			return -1;
		}

		// Token: 0x04000C13 RID: 3091
		private RelativeSource _relativeSource;
	}
}
