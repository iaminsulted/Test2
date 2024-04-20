using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	// Token: 0x0200081C RID: 2076
	public class InkCanvasGestureEventArgs : RoutedEventArgs
	{
		// Token: 0x060078F6 RID: 30966 RVA: 0x0030199C File Offset: 0x0030099C
		public InkCanvasGestureEventArgs(StrokeCollection strokes, IEnumerable<GestureRecognitionResult> gestureRecognitionResults) : base(InkCanvas.GestureEvent)
		{
			if (strokes == null)
			{
				throw new ArgumentNullException("strokes");
			}
			if (strokes.Count < 1)
			{
				throw new ArgumentException(SR.Get("InvalidEmptyStrokeCollection"), "strokes");
			}
			if (gestureRecognitionResults == null)
			{
				throw new ArgumentNullException("strokes");
			}
			List<GestureRecognitionResult> list = new List<GestureRecognitionResult>(gestureRecognitionResults);
			if (list.Count == 0)
			{
				throw new ArgumentException(SR.Get("InvalidEmptyArray"), "gestureRecognitionResults");
			}
			this._strokes = strokes;
			this._gestureRecognitionResults = list;
		}

		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x060078F7 RID: 30967 RVA: 0x00301A20 File Offset: 0x00300A20
		public StrokeCollection Strokes
		{
			get
			{
				return this._strokes;
			}
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x00301A28 File Offset: 0x00300A28
		public ReadOnlyCollection<GestureRecognitionResult> GetGestureRecognitionResults()
		{
			return new ReadOnlyCollection<GestureRecognitionResult>(this._gestureRecognitionResults);
		}

		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x060078F9 RID: 30969 RVA: 0x00301A35 File Offset: 0x00300A35
		// (set) Token: 0x060078FA RID: 30970 RVA: 0x00301A3D File Offset: 0x00300A3D
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x060078FB RID: 30971 RVA: 0x00301A46 File Offset: 0x00300A46
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((InkCanvasGestureEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x0400398F RID: 14735
		private StrokeCollection _strokes;

		// Token: 0x04003990 RID: 14736
		private List<GestureRecognitionResult> _gestureRecognitionResults;

		// Token: 0x04003991 RID: 14737
		private bool _cancel;
	}
}
