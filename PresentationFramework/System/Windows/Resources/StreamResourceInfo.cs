using System;
using System.IO;

namespace System.Windows.Resources
{
	// Token: 0x02000400 RID: 1024
	public class StreamResourceInfo
	{
		// Token: 0x06002C3B RID: 11323 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		public StreamResourceInfo()
		{
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x001A6592 File Offset: 0x001A5592
		public StreamResourceInfo(Stream stream, string contentType)
		{
			this._stream = stream;
			this._contentType = contentType;
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002C3D RID: 11325 RVA: 0x001A65A8 File Offset: 0x001A55A8
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002C3E RID: 11326 RVA: 0x001A65B0 File Offset: 0x001A55B0
		public Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x04001B16 RID: 6934
		private string _contentType;

		// Token: 0x04001B17 RID: 6935
		private Stream _stream;
	}
}
