using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Media;

namespace System.Windows.Documents
{
	// Token: 0x020005D8 RID: 1496
	public class AdornerLayer : FrameworkElement
	{
		// Token: 0x0600481F RID: 18463 RVA: 0x0022B159 File Offset: 0x0022A159
		internal AdornerLayer() : this(Dispatcher.CurrentDispatcher)
		{
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x0022B168 File Offset: 0x0022A168
		internal AdornerLayer(Dispatcher context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			base.LayoutUpdated += this.OnLayoutUpdated;
			this._children = new VisualCollection(this);
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x0022B1C1 File Offset: 0x0022A1C1
		public void Add(Adorner adorner)
		{
			this.Add(adorner, int.MaxValue);
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x0022B1D0 File Offset: 0x0022A1D0
		public void Remove(Adorner adorner)
		{
			if (adorner == null)
			{
				throw new ArgumentNullException("adorner");
			}
			ArrayList arrayList = this.ElementMap[adorner.AdornedElement] as ArrayList;
			if (arrayList == null)
			{
				return;
			}
			AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
			if (adornerInfo == null)
			{
				return;
			}
			this.RemoveAdornerInfo(this.ElementMap, adorner, adorner.AdornedElement);
			this.RemoveAdornerInfo(this._zOrderMap, adorner, adornerInfo.ZOrder);
			this._children.Remove(adorner);
			base.RemoveLogicalChild(adorner);
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x0022B254 File Offset: 0x0022A254
		public void Update()
		{
			foreach (object obj in this.ElementMap.Keys)
			{
				UIElement key = (UIElement)obj;
				ArrayList arrayList = (ArrayList)this.ElementMap[key];
				int i = 0;
				if (arrayList != null)
				{
					while (i < arrayList.Count)
					{
						this.InvalidateAdorner((AdornerLayer.AdornerInfo)arrayList[i++]);
					}
				}
			}
			this.UpdateAdorner(null);
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0022B2F0 File Offset: 0x0022A2F0
		public void Update(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			int i = 0;
			while (i < arrayList.Count)
			{
				this.InvalidateAdorner((AdornerLayer.AdornerInfo)arrayList[i++]);
			}
			this.UpdateAdorner(element);
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0022B35C File Offset: 0x0022A35C
		public Adorner[] GetAdorners(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null || arrayList.Count == 0)
			{
				return null;
			}
			Adorner[] array = new Adorner[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = ((AdornerLayer.AdornerInfo)arrayList[i]).Adorner;
			}
			return array;
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0022B3C8 File Offset: 0x0022A3C8
		public AdornerHitTestResult AdornerHitTest(Point point)
		{
			PointHitTestResult pointHitTestResult = VisualTreeUtils.AsNearestPointHitTestResult(VisualTreeHelper.HitTest(this, point, false));
			if (pointHitTestResult != null && pointHitTestResult.VisualHit != null)
			{
				for (Visual visual = pointHitTestResult.VisualHit; visual != this; visual = (Visual)VisualTreeHelper.GetParent(visual))
				{
					if (visual is Adorner)
					{
						return new AdornerHitTestResult(pointHitTestResult.VisualHit, pointHitTestResult.PointHit, visual as Adorner);
					}
				}
				return null;
			}
			return null;
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x0022B42C File Offset: 0x0022A42C
		public static AdornerLayer GetAdornerLayer(Visual visual)
		{
			if (visual == null)
			{
				throw new ArgumentNullException("visual");
			}
			for (Visual visual2 = VisualTreeHelper.GetParent(visual) as Visual; visual2 != null; visual2 = (VisualTreeHelper.GetParent(visual2) as Visual))
			{
				if (visual2 is AdornerDecorator)
				{
					return ((AdornerDecorator)visual2).AdornerLayer;
				}
				if (visual2 is ScrollContentPresenter)
				{
					return ((ScrollContentPresenter)visual2).AdornerLayer;
				}
			}
			return null;
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06004828 RID: 18472 RVA: 0x0022B48D File Offset: 0x0022A48D
		protected override int VisualChildrenCount
		{
			get
			{
				return this._children.Count;
			}
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0022B49A File Offset: 0x0022A49A
		protected override Visual GetVisualChild(int index)
		{
			return this._children[index];
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x0600482A RID: 18474 RVA: 0x0022B4A8 File Offset: 0x0022A4A8
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.VisualChildrenCount == 0)
				{
					return EmptyEnumerator.Instance;
				}
				return this._children.GetEnumerator();
			}
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0022B4C8 File Offset: 0x0022A4C8
		protected override Size MeasureOverride(Size constraint)
		{
			DictionaryEntry[] array = new DictionaryEntry[this._zOrderMap.Count];
			this._zOrderMap.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList = (ArrayList)array[i].Value;
				int j = 0;
				while (j < arrayList.Count)
				{
					((AdornerLayer.AdornerInfo)arrayList[j++]).Adorner.Measure(constraint);
				}
			}
			return default(Size);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0022B548 File Offset: 0x0022A548
		protected override Size ArrangeOverride(Size finalSize)
		{
			DictionaryEntry[] array = new DictionaryEntry[this._zOrderMap.Count];
			this._zOrderMap.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList = (ArrayList)array[i].Value;
				int j = 0;
				while (j < arrayList.Count)
				{
					AdornerLayer.AdornerInfo adornerInfo = (AdornerLayer.AdornerInfo)arrayList[j++];
					if (!adornerInfo.Adorner.IsArrangeValid)
					{
						adornerInfo.Adorner.Arrange(new Rect(default(Point), adornerInfo.Adorner.DesiredSize));
						GeneralTransform desiredTransform = adornerInfo.Adorner.GetDesiredTransform(adornerInfo.Transform);
						GeneralTransform proposedTransform = this.GetProposedTransform(adornerInfo.Adorner, desiredTransform);
						int num = this._children.IndexOf(adornerInfo.Adorner);
						if (num >= 0)
						{
							Transform adornerTransform = (proposedTransform != null) ? proposedTransform.AffineTransform : null;
							((Adorner)this._children[num]).AdornerTransform = adornerTransform;
						}
					}
					if (adornerInfo.Adorner.IsClipEnabled)
					{
						adornerInfo.Adorner.AdornerClip = adornerInfo.Clip;
					}
					else if (adornerInfo.Adorner.AdornerClip != null)
					{
						adornerInfo.Adorner.AdornerClip = null;
					}
				}
			}
			return finalSize;
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0022B6A0 File Offset: 0x0022A6A0
		internal void Add(Adorner adorner, int zOrder)
		{
			if (adorner == null)
			{
				throw new ArgumentNullException("adorner");
			}
			AdornerLayer.AdornerInfo adornerInfo = new AdornerLayer.AdornerInfo(adorner);
			adornerInfo.ZOrder = zOrder;
			this.AddAdornerInfo(this.ElementMap, adornerInfo, adorner.AdornedElement);
			this.AddAdornerToVisualTree(adornerInfo, zOrder);
			base.AddLogicalChild(adorner);
			this.UpdateAdorner(adorner.AdornedElement);
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x0022B6F7 File Offset: 0x0022A6F7
		internal void InvalidateAdorner(AdornerLayer.AdornerInfo adornerInfo)
		{
			adornerInfo.Adorner.InvalidateMeasure();
			adornerInfo.Adorner.InvalidateVisual();
			adornerInfo.RenderSize = new Size(double.NaN, double.NaN);
			adornerInfo.Transform = null;
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x0022B733 File Offset: 0x0022A733
		internal void OnLayoutUpdated(object sender, EventArgs args)
		{
			if (this.ElementMap.Count == 0)
			{
				return;
			}
			this.UpdateAdorner(null);
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0022B74C File Offset: 0x0022A74C
		internal void SetAdornerZOrder(Adorner adorner, int zOrder)
		{
			ArrayList arrayList = this.ElementMap[adorner.AdornedElement] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
			if (adornerInfo == null)
			{
				throw new InvalidOperationException(SR.Get("AdornerNotFound"));
			}
			this.RemoveAdornerInfo(this._zOrderMap, adorner, adornerInfo.ZOrder);
			this._children.Remove(adorner);
			adornerInfo.ZOrder = zOrder;
			this.AddAdornerToVisualTree(adornerInfo, zOrder);
			this.InvalidateAdorner(adornerInfo);
			this.UpdateAdorner(adorner.AdornedElement);
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x0022B7E8 File Offset: 0x0022A7E8
		internal int GetAdornerZOrder(Adorner adorner)
		{
			ArrayList arrayList = this.ElementMap[adorner.AdornedElement] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
			if (adornerInfo == null)
			{
				throw new InvalidOperationException(SR.Get("AdornerNotFound"));
			}
			return adornerInfo.ZOrder;
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06004832 RID: 18482 RVA: 0x0022B83F File Offset: 0x0022A83F
		internal HybridDictionary ElementMap
		{
			get
			{
				return this._elementMap;
			}
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x0022B848 File Offset: 0x0022A848
		private void AddAdornerToVisualTree(AdornerLayer.AdornerInfo adornerInfo, int zOrder)
		{
			Adorner adorner = adornerInfo.Adorner;
			this.AddAdornerInfo(this._zOrderMap, adornerInfo, zOrder);
			ArrayList arrayList = (ArrayList)this._zOrderMap[zOrder];
			if (arrayList.Count > 1)
			{
				int num = arrayList.IndexOf(adornerInfo);
				int index = this._children.IndexOf(((AdornerLayer.AdornerInfo)arrayList[num - 1]).Adorner) + 1;
				this._children.Insert(index, adorner);
				return;
			}
			IList keyList = this._zOrderMap.GetKeyList();
			int num2 = keyList.IndexOf(zOrder) - 1;
			if (num2 < 0)
			{
				this._children.Insert(0, adorner);
				return;
			}
			arrayList = (ArrayList)this._zOrderMap[keyList[num2]];
			int index2 = this._children.IndexOf(((AdornerLayer.AdornerInfo)arrayList[arrayList.Count - 1]).Adorner) + 1;
			this._children.Insert(index2, adorner);
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x0022B948 File Offset: 0x0022A948
		private void Clear(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			while (arrayList.Count > 0)
			{
				AdornerLayer.AdornerInfo adornerInfo = arrayList[0] as AdornerLayer.AdornerInfo;
				this.Remove(adornerInfo.Adorner);
			}
			this.ElementMap.Remove(element);
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x0022B9B8 File Offset: 0x0022A9B8
		private void UpdateElementAdorners(UIElement element)
		{
			Visual visual = VisualTreeHelper.GetParent(this) as Visual;
			if (visual == null)
			{
				return;
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null)
			{
				return;
			}
			bool flag = false;
			GeneralTransform generalTransform = element.TransformToAncestor(visual);
			for (int i = 0; i < arrayList.Count; i++)
			{
				AdornerLayer.AdornerInfo adornerInfo = (AdornerLayer.AdornerInfo)arrayList[i];
				Size renderSize = element.RenderSize;
				Geometry geometry = null;
				bool flag2 = false;
				if (adornerInfo.Adorner.IsClipEnabled)
				{
					geometry = this.GetClipGeometry(adornerInfo.Adorner.AdornedElement, adornerInfo.Adorner);
					if ((adornerInfo.Clip == null && geometry != null) || (adornerInfo.Clip != null && geometry == null) || (adornerInfo.Clip != null && geometry != null && adornerInfo.Clip.Bounds != geometry.Bounds))
					{
						flag2 = true;
					}
				}
				if (adornerInfo.Adorner.NeedsUpdate(adornerInfo.RenderSize) || adornerInfo.Transform == null || generalTransform.AffineTransform == null || adornerInfo.Transform.AffineTransform == null || generalTransform.AffineTransform.Value != adornerInfo.Transform.AffineTransform.Value || flag2)
				{
					this.InvalidateAdorner(adornerInfo);
					adornerInfo.RenderSize = renderSize;
					adornerInfo.Transform = generalTransform;
					if (adornerInfo.Adorner.IsClipEnabled)
					{
						adornerInfo.Clip = geometry;
					}
					flag = true;
				}
			}
			if (flag)
			{
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x0022BB34 File Offset: 0x0022AB34
		private void UpdateAdorner(UIElement element)
		{
			Visual visual = VisualTreeHelper.GetParent(this) as Visual;
			if (visual == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(1);
			if (element != null)
			{
				if (!element.IsDescendantOf(visual))
				{
					arrayList.Add(element);
				}
				else
				{
					this.UpdateElementAdorners(element);
				}
			}
			else
			{
				ICollection keys = this.ElementMap.Keys;
				UIElement[] array = new UIElement[keys.Count];
				keys.CopyTo(array, 0);
				foreach (UIElement uielement in array)
				{
					if (!uielement.IsDescendantOf(visual))
					{
						arrayList.Add(uielement);
					}
					else
					{
						this.UpdateElementAdorners(uielement);
					}
				}
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.Clear((UIElement)arrayList[j]);
			}
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0022BBF0 File Offset: 0x0022ABF0
		private CombinedGeometry GetClipGeometry(Visual element, Adorner adorner)
		{
			Visual visual = null;
			Visual visual2 = VisualTreeHelper.GetParent(this) as Visual;
			if (visual2 == null)
			{
				return null;
			}
			CombinedGeometry combinedGeometry = null;
			if (!visual2.IsAncestorOf(element))
			{
				return null;
			}
			while (element != visual2 && element != null)
			{
				Geometry clip = VisualTreeHelper.GetClip(element);
				if (clip != null)
				{
					if (combinedGeometry == null)
					{
						combinedGeometry = new CombinedGeometry(clip, null);
					}
					else
					{
						GeneralTransform generalTransform = visual.TransformToAncestor(element);
						combinedGeometry.Transform = generalTransform.AffineTransform;
						combinedGeometry = new CombinedGeometry(combinedGeometry, clip);
						combinedGeometry.GeometryCombineMode = GeometryCombineMode.Intersect;
					}
					visual = element;
				}
				element = (Visual)VisualTreeHelper.GetParent(element);
			}
			if (combinedGeometry != null)
			{
				GeneralTransform generalTransform2 = visual.TransformToAncestor(visual2);
				if (generalTransform2 == null)
				{
					combinedGeometry = null;
				}
				else
				{
					TransformGroup transformGroup = new TransformGroup();
					transformGroup.Children.Add(generalTransform2.AffineTransform);
					generalTransform2 = visual2.TransformToDescendant(adorner);
					if (generalTransform2 == null)
					{
						combinedGeometry = null;
					}
					else
					{
						transformGroup.Children.Add(generalTransform2.AffineTransform);
						combinedGeometry.Transform = transformGroup;
					}
				}
			}
			return combinedGeometry;
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x0022BCCC File Offset: 0x0022ACCC
		private bool RemoveAdornerInfo(IDictionary infoMap, Adorner adorner, object key)
		{
			ArrayList arrayList = infoMap[key] as ArrayList;
			if (arrayList != null)
			{
				AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
				if (adornerInfo != null)
				{
					arrayList.Remove(adornerInfo);
					if (arrayList.Count == 0)
					{
						infoMap.Remove(key);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x0022BD10 File Offset: 0x0022AD10
		private AdornerLayer.AdornerInfo GetAdornerInfo(ArrayList adornerInfos, Adorner adorner)
		{
			if (adornerInfos != null)
			{
				for (int i = 0; i < adornerInfos.Count; i++)
				{
					if (((AdornerLayer.AdornerInfo)adornerInfos[i]).Adorner == adorner)
					{
						return (AdornerLayer.AdornerInfo)adornerInfos[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x0022BD54 File Offset: 0x0022AD54
		private void AddAdornerInfo(IDictionary infoMap, AdornerLayer.AdornerInfo adornerInfo, object key)
		{
			ArrayList arrayList;
			if (infoMap[key] == null)
			{
				arrayList = new ArrayList(1);
				infoMap[key] = arrayList;
			}
			else
			{
				arrayList = (ArrayList)infoMap[key];
			}
			arrayList.Add(adornerInfo);
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x0600483B RID: 18491 RVA: 0x001FC019 File Offset: 0x001FB019
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x0022BD90 File Offset: 0x0022AD90
		private GeneralTransform GetProposedTransform(Adorner adorner, GeneralTransform sourceTransform)
		{
			if (adorner.FlowDirection != base.FlowDirection)
			{
				GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
				MatrixTransform value = new MatrixTransform(new Matrix(-1.0, 0.0, 0.0, 1.0, adorner.RenderSize.Width, 0.0));
				generalTransformGroup.Children.Add(value);
				if (sourceTransform != null && sourceTransform != Transform.Identity)
				{
					generalTransformGroup.Children.Add(sourceTransform);
				}
				return generalTransformGroup;
			}
			return sourceTransform;
		}

		// Token: 0x040025F8 RID: 9720
		private HybridDictionary _elementMap = new HybridDictionary(10);

		// Token: 0x040025F9 RID: 9721
		private SortedList _zOrderMap = new SortedList(10);

		// Token: 0x040025FA RID: 9722
		private const int DefaultZOrder = 2147483647;

		// Token: 0x040025FB RID: 9723
		private VisualCollection _children;

		// Token: 0x02000B29 RID: 2857
		internal class AdornerInfo
		{
			// Token: 0x06008C75 RID: 35957 RVA: 0x0033CE1C File Offset: 0x0033BE1C
			internal AdornerInfo(Adorner adorner)
			{
				Invariant.Assert(adorner != null);
				this._adorner = adorner;
			}

			// Token: 0x17001EBF RID: 7871
			// (get) Token: 0x06008C76 RID: 35958 RVA: 0x0033CE34 File Offset: 0x0033BE34
			internal Adorner Adorner
			{
				get
				{
					return this._adorner;
				}
			}

			// Token: 0x17001EC0 RID: 7872
			// (get) Token: 0x06008C77 RID: 35959 RVA: 0x0033CE3C File Offset: 0x0033BE3C
			// (set) Token: 0x06008C78 RID: 35960 RVA: 0x0033CE44 File Offset: 0x0033BE44
			internal Size RenderSize
			{
				get
				{
					return this._computedSize;
				}
				set
				{
					this._computedSize = value;
				}
			}

			// Token: 0x17001EC1 RID: 7873
			// (get) Token: 0x06008C79 RID: 35961 RVA: 0x0033CE4D File Offset: 0x0033BE4D
			// (set) Token: 0x06008C7A RID: 35962 RVA: 0x0033CE55 File Offset: 0x0033BE55
			internal GeneralTransform Transform
			{
				get
				{
					return this._transform;
				}
				set
				{
					this._transform = value;
				}
			}

			// Token: 0x17001EC2 RID: 7874
			// (get) Token: 0x06008C7B RID: 35963 RVA: 0x0033CE5E File Offset: 0x0033BE5E
			// (set) Token: 0x06008C7C RID: 35964 RVA: 0x0033CE66 File Offset: 0x0033BE66
			internal int ZOrder
			{
				get
				{
					return this._zOrder;
				}
				set
				{
					this._zOrder = value;
				}
			}

			// Token: 0x17001EC3 RID: 7875
			// (get) Token: 0x06008C7D RID: 35965 RVA: 0x0033CE6F File Offset: 0x0033BE6F
			// (set) Token: 0x06008C7E RID: 35966 RVA: 0x0033CE77 File Offset: 0x0033BE77
			internal Geometry Clip
			{
				get
				{
					return this._clip;
				}
				set
				{
					this._clip = value;
				}
			}

			// Token: 0x040047E5 RID: 18405
			private Adorner _adorner;

			// Token: 0x040047E6 RID: 18406
			private Size _computedSize;

			// Token: 0x040047E7 RID: 18407
			private GeneralTransform _transform;

			// Token: 0x040047E8 RID: 18408
			private int _zOrder;

			// Token: 0x040047E9 RID: 18409
			private Geometry _clip;
		}
	}
}
