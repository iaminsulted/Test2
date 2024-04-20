using System;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	// Token: 0x02000812 RID: 2066
	public class InkCanvasStrokeCollectedEventArgs : RoutedEventArgs
	{
		// Token: 0x060078CF RID: 30927 RVA: 0x003017F8 File Offset: 0x003007F8
		public InkCanvasStrokeCollectedEventArgs(Stroke stroke) : base(InkCanvas.StrokeCollectedEvent)
		{
			if (stroke == null)
			{
				throw new ArgumentNullException("stroke");
			}
			this._stroke = stroke;
		}

		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x060078D0 RID: 30928 RVA: 0x0030181A File Offset: 0x0030081A
		public Stroke Stroke
		{
			get
			{
				return this._stroke;
			}
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x00301822 File Offset: 0x00300822
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((InkCanvasStrokeCollectedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003985 RID: 14725
		private Stroke _stroke;
	}
}
