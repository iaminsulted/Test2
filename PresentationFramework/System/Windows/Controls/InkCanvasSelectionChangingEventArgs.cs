using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	// Token: 0x02000816 RID: 2070
	public class InkCanvasSelectionChangingEventArgs : CancelEventArgs
	{
		// Token: 0x060078DD RID: 30941 RVA: 0x00301874 File Offset: 0x00300874
		internal InkCanvasSelectionChangingEventArgs(StrokeCollection selectedStrokes, IEnumerable<UIElement> selectedElements)
		{
			if (selectedStrokes == null)
			{
				throw new ArgumentNullException("selectedStrokes");
			}
			if (selectedElements == null)
			{
				throw new ArgumentNullException("selectedElements");
			}
			this._strokes = selectedStrokes;
			List<UIElement> elements = new List<UIElement>(selectedElements);
			this._elements = elements;
			this._strokesChanged = false;
			this._elementsChanged = false;
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x060078DE RID: 30942 RVA: 0x003018C6 File Offset: 0x003008C6
		internal bool StrokesChanged
		{
			get
			{
				return this._strokesChanged;
			}
		}

		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x060078DF RID: 30943 RVA: 0x003018CE File Offset: 0x003008CE
		internal bool ElementsChanged
		{
			get
			{
				return this._elementsChanged;
			}
		}

		// Token: 0x060078E0 RID: 30944 RVA: 0x003018D8 File Offset: 0x003008D8
		public void SetSelectedElements(IEnumerable<UIElement> selectedElements)
		{
			if (selectedElements == null)
			{
				throw new ArgumentNullException("selectedElements");
			}
			List<UIElement> elements = new List<UIElement>(selectedElements);
			this._elements = elements;
			this._elementsChanged = true;
		}

		// Token: 0x060078E1 RID: 30945 RVA: 0x00301908 File Offset: 0x00300908
		public ReadOnlyCollection<UIElement> GetSelectedElements()
		{
			return new ReadOnlyCollection<UIElement>(this._elements);
		}

		// Token: 0x060078E2 RID: 30946 RVA: 0x00301915 File Offset: 0x00300915
		public void SetSelectedStrokes(StrokeCollection selectedStrokes)
		{
			if (selectedStrokes == null)
			{
				throw new ArgumentNullException("selectedStrokes");
			}
			this._strokes = selectedStrokes;
			this._strokesChanged = true;
		}

		// Token: 0x060078E3 RID: 30947 RVA: 0x00301933 File Offset: 0x00300933
		public StrokeCollection GetSelectedStrokes()
		{
			return new StrokeCollection
			{
				this._strokes
			};
		}

		// Token: 0x04003988 RID: 14728
		private StrokeCollection _strokes;

		// Token: 0x04003989 RID: 14729
		private List<UIElement> _elements;

		// Token: 0x0400398A RID: 14730
		private bool _strokesChanged;

		// Token: 0x0400398B RID: 14731
		private bool _elementsChanged;
	}
}
