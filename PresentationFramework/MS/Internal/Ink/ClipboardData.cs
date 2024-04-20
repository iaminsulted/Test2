using System;
using System.Windows;

namespace MS.Internal.Ink
{
	// Token: 0x0200017E RID: 382
	internal abstract class ClipboardData
	{
		// Token: 0x06000C79 RID: 3193 RVA: 0x001302AD File Offset: 0x0012F2AD
		internal bool CopyToDataObject(IDataObject dataObject)
		{
			if (this.CanCopy())
			{
				this.DoCopy(dataObject);
				return true;
			}
			return false;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x001302C1 File Offset: 0x0012F2C1
		internal void PasteFromDataObject(IDataObject dataObject)
		{
			if (this.CanPaste(dataObject))
			{
				this.DoPaste(dataObject);
			}
		}

		// Token: 0x06000C7B RID: 3195
		internal abstract bool CanPaste(IDataObject dataObject);

		// Token: 0x06000C7C RID: 3196
		protected abstract bool CanCopy();

		// Token: 0x06000C7D RID: 3197
		protected abstract void DoCopy(IDataObject dataObject);

		// Token: 0x06000C7E RID: 3198
		protected abstract void DoPaste(IDataObject dataObject);
	}
}
