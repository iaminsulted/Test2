using System;
using System.ComponentModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	// Token: 0x0200081A RID: 2074
	public class InkCanvasStrokeErasingEventArgs : CancelEventArgs
	{
		// Token: 0x060078F0 RID: 30960 RVA: 0x00301975 File Offset: 0x00300975
		internal InkCanvasStrokeErasingEventArgs(Stroke stroke)
		{
			if (stroke == null)
			{
				throw new ArgumentNullException("stroke");
			}
			this._stroke = stroke;
		}

		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x060078F1 RID: 30961 RVA: 0x00301992 File Offset: 0x00300992
		public Stroke Stroke
		{
			get
			{
				return this._stroke;
			}
		}

		// Token: 0x0400398E RID: 14734
		private Stroke _stroke;
	}
}
