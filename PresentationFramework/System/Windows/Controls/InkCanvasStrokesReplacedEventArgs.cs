using System;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	// Token: 0x02000814 RID: 2068
	public class InkCanvasStrokesReplacedEventArgs : EventArgs
	{
		// Token: 0x060078D6 RID: 30934 RVA: 0x00301831 File Offset: 0x00300831
		internal InkCanvasStrokesReplacedEventArgs(StrokeCollection newStrokes, StrokeCollection previousStrokes)
		{
			if (newStrokes == null)
			{
				throw new ArgumentNullException("newStrokes");
			}
			if (previousStrokes == null)
			{
				throw new ArgumentNullException("previousStrokes");
			}
			this._newStrokes = newStrokes;
			this._previousStrokes = previousStrokes;
		}

		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x060078D7 RID: 30935 RVA: 0x00301863 File Offset: 0x00300863
		public StrokeCollection NewStrokes
		{
			get
			{
				return this._newStrokes;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x060078D8 RID: 30936 RVA: 0x0030186B File Offset: 0x0030086B
		public StrokeCollection PreviousStrokes
		{
			get
			{
				return this._previousStrokes;
			}
		}

		// Token: 0x04003986 RID: 14726
		private StrokeCollection _newStrokes;

		// Token: 0x04003987 RID: 14727
		private StrokeCollection _previousStrokes;
	}
}
