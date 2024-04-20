using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000266 RID: 614
	internal abstract class StickyNoteContentControl
	{
		// Token: 0x060017D9 RID: 6105 RVA: 0x0015F961 File Offset: 0x0015E961
		protected StickyNoteContentControl(FrameworkElement innerControl)
		{
			this.SetInnerControl(innerControl);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private StickyNoteContentControl()
		{
		}

		// Token: 0x060017DB RID: 6107
		public abstract void Save(XmlNode node);

		// Token: 0x060017DC RID: 6108
		public abstract void Load(XmlNode node);

		// Token: 0x060017DD RID: 6109
		public abstract void Clear();

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060017DE RID: 6110
		public abstract bool IsEmpty { get; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060017DF RID: 6111
		public abstract StickyNoteType Type { get; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x0015F970 File Offset: 0x0015E970
		public FrameworkElement InnerControl
		{
			get
			{
				return this._innerControl;
			}
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0015F978 File Offset: 0x0015E978
		protected void SetInnerControl(FrameworkElement innerControl)
		{
			this._innerControl = innerControl;
		}

		// Token: 0x04000CC8 RID: 3272
		protected FrameworkElement _innerControl;

		// Token: 0x04000CC9 RID: 3273
		protected const long MaxBufferSize = 1610612733L;
	}
}
