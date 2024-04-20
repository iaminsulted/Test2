using System;
using System.IO;
using System.Windows;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x0200018A RID: 394
	internal class ISFClipboardData : ClipboardData
	{
		// Token: 0x06000D19 RID: 3353 RVA: 0x001316C6 File Offset: 0x001306C6
		internal ISFClipboardData()
		{
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00132E5D File Offset: 0x00131E5D
		internal ISFClipboardData(StrokeCollection strokes)
		{
			this._strokes = strokes;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00132E6C File Offset: 0x00131E6C
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(StrokeCollection.InkSerializedFormat, false);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00132E7A File Offset: 0x00131E7A
		protected override bool CanCopy()
		{
			return this.Strokes != null && this.Strokes.Count != 0;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00132E94 File Offset: 0x00131E94
		protected override void DoCopy(IDataObject dataObject)
		{
			MemoryStream memoryStream = new MemoryStream();
			this.Strokes.Save(memoryStream);
			memoryStream.Position = 0L;
			dataObject.SetData(StrokeCollection.InkSerializedFormat, memoryStream);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00132EC8 File Offset: 0x00131EC8
		protected override void DoPaste(IDataObject dataObject)
		{
			MemoryStream memoryStream = dataObject.GetData(StrokeCollection.InkSerializedFormat) as MemoryStream;
			StrokeCollection strokeCollection = null;
			bool flag = false;
			if (memoryStream != null && memoryStream != Stream.Null)
			{
				try
				{
					strokeCollection = new StrokeCollection(memoryStream);
					flag = true;
				}
				catch (ArgumentException)
				{
					flag = false;
				}
			}
			this._strokes = (flag ? strokeCollection : new StrokeCollection());
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00132F28 File Offset: 0x00131F28
		internal StrokeCollection Strokes
		{
			get
			{
				return this._strokes;
			}
		}

		// Token: 0x040009BB RID: 2491
		private StrokeCollection _strokes;
	}
}
