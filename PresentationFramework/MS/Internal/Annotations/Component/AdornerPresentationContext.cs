using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002C6 RID: 710
	internal class AdornerPresentationContext : PresentationContext
	{
		// Token: 0x06001A6C RID: 6764 RVA: 0x00163E84 File Offset: 0x00162E84
		private AdornerPresentationContext(AdornerLayer adornerLayer, AnnotationAdorner adorner)
		{
			if (adornerLayer == null)
			{
				throw new ArgumentNullException("adornerLayer");
			}
			this._adornerLayer = adornerLayer;
			if (adorner != null)
			{
				if (adorner.AnnotationComponent == null)
				{
					throw new ArgumentNullException("annotation component");
				}
				if (adorner.AnnotationComponent.PresentationContext != null)
				{
					throw new InvalidOperationException(SR.Get("ComponentAlreadyInPresentationContext", new object[]
					{
						adorner.AnnotationComponent
					}));
				}
				this._annotationAdorner = adorner;
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00163EF8 File Offset: 0x00162EF8
		internal static void HostComponent(AdornerLayer adornerLayer, IAnnotationComponent component, UIElement annotatedElement, bool reorder)
		{
			AnnotationAdorner annotationAdorner = new AnnotationAdorner(component, annotatedElement);
			annotationAdorner.AnnotationComponent.PresentationContext = new AdornerPresentationContext(adornerLayer, annotationAdorner);
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			if (reorder)
			{
				component.ZOrder = AdornerPresentationContext.GetNextZOrder(adornerLayer, componentLevel);
			}
			adornerLayer.Add(annotationAdorner, AdornerPresentationContext.ComponentToAdorner(component.ZOrder, componentLevel));
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x00163F4C File Offset: 0x00162F4C
		internal static void SetTypeZLevel(Type type, int level)
		{
			Invariant.Assert(level >= 0, "level is < 0");
			Invariant.Assert(type != null, "type is null");
			if (AdornerPresentationContext._ZLevel.ContainsKey(type))
			{
				AdornerPresentationContext._ZLevel[type] = level;
				return;
			}
			AdornerPresentationContext._ZLevel.Add(type, level);
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00163FAB File Offset: 0x00162FAB
		internal static void SetZLevelRange(int level, int min, int max)
		{
			if (AdornerPresentationContext._ZRanges[level] == null)
			{
				AdornerPresentationContext._ZRanges.Add(level, new AdornerPresentationContext.ZRange(min, max));
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x00163FD6 File Offset: 0x00162FD6
		public override UIElement Host
		{
			get
			{
				return this._adornerLayer;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x00163FE0 File Offset: 0x00162FE0
		public override PresentationContext EnclosingContext
		{
			get
			{
				Visual visual = VisualTreeHelper.GetParent(this._adornerLayer) as Visual;
				if (visual == null)
				{
					return null;
				}
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((UIElement)visual);
				if (adornerLayer == null)
				{
					return null;
				}
				return new AdornerPresentationContext(adornerLayer, null);
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0016401B File Offset: 0x0016301B
		public override void AddToHost(IAnnotationComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			AdornerPresentationContext.HostComponent(this._adornerLayer, component, component.AnnotatedElement, false);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00164040 File Offset: 0x00163040
		public override void RemoveFromHost(IAnnotationComponent component, bool reorder)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (this.IsInternalComponent(component))
			{
				this._annotationAdorner.AnnotationComponent.PresentationContext = null;
				this._adornerLayer.Remove(this._annotationAdorner);
				this._annotationAdorner.RemoveChildren();
				this._annotationAdorner = null;
				return;
			}
			AnnotationAdorner annotationAdorner = this.FindAnnotationAdorner(component);
			if (annotationAdorner == null)
			{
				throw new InvalidOperationException(SR.Get("ComponentNotInPresentationContext", new object[]
				{
					component
				}));
			}
			this._adornerLayer.Remove(annotationAdorner);
			annotationAdorner.RemoveChildren();
			AdornerPresentationContext adornerPresentationContext = component.PresentationContext as AdornerPresentationContext;
			if (adornerPresentationContext != null)
			{
				adornerPresentationContext.ResetInternalAnnotationAdorner();
			}
			component.PresentationContext = null;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x001640F2 File Offset: 0x001630F2
		public override void InvalidateTransform(IAnnotationComponent component)
		{
			this.GetAnnotationAdorner(component).InvalidateTransform();
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00164100 File Offset: 0x00163100
		public override void BringToFront(IAnnotationComponent component)
		{
			AnnotationAdorner annotationAdorner = this.GetAnnotationAdorner(component);
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			int nextZOrder = AdornerPresentationContext.GetNextZOrder(this._adornerLayer, componentLevel);
			if (nextZOrder != component.ZOrder + 1)
			{
				component.ZOrder = nextZOrder;
				this._adornerLayer.SetAdornerZOrder(annotationAdorner, AdornerPresentationContext.ComponentToAdorner(component.ZOrder, componentLevel));
			}
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00164153 File Offset: 0x00163153
		public override void SendToBack(IAnnotationComponent component)
		{
			this.GetAnnotationAdorner(component);
			AdornerPresentationContext.GetComponentLevel(component);
			if (component.ZOrder != 0)
			{
				component.ZOrder = 0;
				this.UpdateComponentZOrder(component);
			}
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0016417C File Offset: 0x0016317C
		public override bool Equals(object o)
		{
			AdornerPresentationContext adornerPresentationContext = o as AdornerPresentationContext;
			return adornerPresentationContext != null && adornerPresentationContext._adornerLayer == this._adornerLayer;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x001641A9 File Offset: 0x001631A9
		public static bool operator ==(AdornerPresentationContext left, AdornerPresentationContext right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x001641BA File Offset: 0x001631BA
		public static bool operator !=(AdornerPresentationContext c1, AdornerPresentationContext c2)
		{
			return !(c1 == c2);
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x001641C6 File Offset: 0x001631C6
		public override int GetHashCode()
		{
			return this._adornerLayer.GetHashCode();
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x001641D4 File Offset: 0x001631D4
		public void UpdateComponentZOrder(IAnnotationComponent component)
		{
			Invariant.Assert(component != null, "null component");
			int componentLevel = AdornerPresentationContext.GetComponentLevel(component);
			AnnotationAdorner annotationAdorner = this.FindAnnotationAdorner(component);
			if (annotationAdorner == null)
			{
				return;
			}
			this._adornerLayer.SetAdornerZOrder(annotationAdorner, AdornerPresentationContext.ComponentToAdorner(component.ZOrder, componentLevel));
			List<AnnotationAdorner> topAnnotationAdorners = this.GetTopAnnotationAdorners(componentLevel, component);
			if (topAnnotationAdorners == null)
			{
				return;
			}
			int num = component.ZOrder + 1;
			foreach (AnnotationAdorner annotationAdorner2 in topAnnotationAdorners)
			{
				annotationAdorner2.AnnotationComponent.ZOrder = num;
				this._adornerLayer.SetAdornerZOrder(annotationAdorner2, AdornerPresentationContext.ComponentToAdorner(num, componentLevel));
				num++;
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00164294 File Offset: 0x00163294
		private void ResetInternalAnnotationAdorner()
		{
			this._annotationAdorner = null;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x0016429D File Offset: 0x0016329D
		private bool IsInternalComponent(IAnnotationComponent component)
		{
			return this._annotationAdorner != null && component == this._annotationAdorner.AnnotationComponent;
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x001642B8 File Offset: 0x001632B8
		private AnnotationAdorner FindAnnotationAdorner(IAnnotationComponent component)
		{
			if (this._adornerLayer == null)
			{
				return null;
			}
			Adorner[] adorners = this._adornerLayer.GetAdorners(component.AnnotatedElement);
			for (int i = 0; i < adorners.Length; i++)
			{
				AnnotationAdorner annotationAdorner = adorners[i] as AnnotationAdorner;
				if (annotationAdorner != null && annotationAdorner.AnnotationComponent == component)
				{
					return annotationAdorner;
				}
			}
			return null;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00164308 File Offset: 0x00163308
		private List<AnnotationAdorner> GetTopAnnotationAdorners(int level, IAnnotationComponent component)
		{
			List<AnnotationAdorner> list = new List<AnnotationAdorner>();
			int childrenCount = VisualTreeHelper.GetChildrenCount(this._adornerLayer);
			if (childrenCount == 0)
			{
				return list;
			}
			for (int i = 0; i < childrenCount; i++)
			{
				AnnotationAdorner annotationAdorner = VisualTreeHelper.GetChild(this._adornerLayer, i) as AnnotationAdorner;
				if (annotationAdorner != null)
				{
					IAnnotationComponent annotationComponent = annotationAdorner.AnnotationComponent;
					if (annotationComponent != component && AdornerPresentationContext.GetComponentLevel(annotationComponent) == level && annotationComponent.ZOrder >= component.ZOrder)
					{
						this.AddAdorner(list, annotationAdorner);
					}
				}
			}
			return list;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0016437C File Offset: 0x0016337C
		private void AddAdorner(List<AnnotationAdorner> adorners, AnnotationAdorner adorner)
		{
			int num = 0;
			if (adorners.Count > 0)
			{
				num = adorners.Count;
				while (num > 0 && adorners[num - 1].AnnotationComponent.ZOrder > adorner.AnnotationComponent.ZOrder)
				{
					num--;
				}
			}
			adorners.Insert(num, adorner);
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x001643D0 File Offset: 0x001633D0
		private static int GetNextZOrder(AdornerLayer adornerLayer, int level)
		{
			Invariant.Assert(adornerLayer != null, "null adornerLayer");
			int num = 0;
			int childrenCount = VisualTreeHelper.GetChildrenCount(adornerLayer);
			if (childrenCount == 0)
			{
				return num;
			}
			for (int i = 0; i < childrenCount; i++)
			{
				AnnotationAdorner annotationAdorner = VisualTreeHelper.GetChild(adornerLayer, i) as AnnotationAdorner;
				if (annotationAdorner != null && AdornerPresentationContext.GetComponentLevel(annotationAdorner.AnnotationComponent) == level && annotationAdorner.AnnotationComponent.ZOrder >= num)
				{
					num = annotationAdorner.AnnotationComponent.ZOrder + 1;
				}
			}
			return num;
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00164440 File Offset: 0x00163440
		private AnnotationAdorner GetAnnotationAdorner(IAnnotationComponent component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			AnnotationAdorner annotationAdorner = this._annotationAdorner;
			if (!this.IsInternalComponent(component))
			{
				annotationAdorner = this.FindAnnotationAdorner(component);
				if (annotationAdorner == null)
				{
					throw new InvalidOperationException(SR.Get("ComponentNotInPresentationContext", new object[]
					{
						component
					}));
				}
			}
			return annotationAdorner;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00164494 File Offset: 0x00163494
		private static int GetComponentLevel(IAnnotationComponent component)
		{
			int result = 0;
			Type type = component.GetType();
			if (AdornerPresentationContext._ZLevel.ContainsKey(type))
			{
				result = (int)AdornerPresentationContext._ZLevel[type];
			}
			return result;
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x001644CC File Offset: 0x001634CC
		private static int ComponentToAdorner(int zOrder, int level)
		{
			int num = zOrder;
			AdornerPresentationContext.ZRange zrange = (AdornerPresentationContext.ZRange)AdornerPresentationContext._ZRanges[level];
			if (zrange != null)
			{
				num += zrange.Min;
				if (num < zrange.Min)
				{
					num = zrange.Min;
				}
				if (num > zrange.Max)
				{
					num = zrange.Max;
				}
			}
			return num;
		}

		// Token: 0x04000DB8 RID: 3512
		private AnnotationAdorner _annotationAdorner;

		// Token: 0x04000DB9 RID: 3513
		private AdornerLayer _adornerLayer;

		// Token: 0x04000DBA RID: 3514
		private static Hashtable _ZLevel = new Hashtable();

		// Token: 0x04000DBB RID: 3515
		private static Hashtable _ZRanges = new Hashtable();

		// Token: 0x02000A16 RID: 2582
		private class ZRange
		{
			// Token: 0x060084CE RID: 33998 RVA: 0x00326F34 File Offset: 0x00325F34
			public ZRange(int min, int max)
			{
				if (min > max)
				{
					int num = min;
					min = max;
					max = num;
				}
				this._min = min;
				this._max = max;
			}

			// Token: 0x17001DD9 RID: 7641
			// (get) Token: 0x060084CF RID: 33999 RVA: 0x00326F54 File Offset: 0x00325F54
			public int Min
			{
				get
				{
					return this._min;
				}
			}

			// Token: 0x17001DDA RID: 7642
			// (get) Token: 0x060084D0 RID: 34000 RVA: 0x00326F5C File Offset: 0x00325F5C
			public int Max
			{
				get
				{
					return this._max;
				}
			}

			// Token: 0x04004095 RID: 16533
			private int _min;

			// Token: 0x04004096 RID: 16534
			private int _max;
		}
	}
}
