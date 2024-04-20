using System;
using System.IO;
using System.Reflection;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x0200026E RID: 622
	internal class BamlStream : Stream, IStreamInfo
	{
		// Token: 0x06001809 RID: 6153 RVA: 0x001601DA File Offset: 0x0015F1DA
		internal BamlStream(Stream stream, Assembly assembly)
		{
			this._assembly.Value = assembly;
			this._stream = stream;
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600180A RID: 6154 RVA: 0x001601F5 File Offset: 0x0015F1F5
		Assembly IStreamInfo.Assembly
		{
			get
			{
				return this._assembly.Value;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x00160202 File Offset: 0x0015F202
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x0016020F File Offset: 0x0015F20F
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x0016021C File Offset: 0x0015F21C
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x00160229 File Offset: 0x0015F229
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x00160236 File Offset: 0x0015F236
		// (set) Token: 0x06001810 RID: 6160 RVA: 0x00160243 File Offset: 0x0015F243
		public override long Position
		{
			get
			{
				return this._stream.Position;
			}
			set
			{
				this._stream.Position = value;
			}
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00160251 File Offset: 0x0015F251
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00160265 File Offset: 0x0015F265
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00160279 File Offset: 0x0015F279
		public override void Close()
		{
			this._stream.Close();
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00160286 File Offset: 0x0015F286
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._stream.EndRead(asyncResult);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00160294 File Offset: 0x0015F294
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._stream.EndWrite(asyncResult);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x001602A2 File Offset: 0x0015F2A2
		public override bool Equals(object obj)
		{
			return this._stream.Equals(obj);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x001602B0 File Offset: 0x0015F2B0
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x001602BD File Offset: 0x0015F2BD
		public override int GetHashCode()
		{
			return this._stream.GetHashCode();
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x001602CA File Offset: 0x0015F2CA
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._stream.Read(buffer, offset, count);
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x001602DA File Offset: 0x0015F2DA
		public override int ReadByte()
		{
			return this._stream.ReadByte();
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x001602E7 File Offset: 0x0015F2E7
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._stream.Seek(offset, origin);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x001602F6 File Offset: 0x0015F2F6
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00160304 File Offset: 0x0015F304
		public override string ToString()
		{
			return this._stream.ToString();
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00160311 File Offset: 0x0015F311
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(buffer, offset, count);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00160321 File Offset: 0x0015F321
		public override void WriteByte(byte value)
		{
			this._stream.WriteByte(value);
		}

		// Token: 0x04000CE3 RID: 3299
		private SecurityCriticalDataForSet<Assembly> _assembly;

		// Token: 0x04000CE4 RID: 3300
		private Stream _stream;
	}
}
