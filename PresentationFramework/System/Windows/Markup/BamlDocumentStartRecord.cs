using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004AF RID: 1199
	internal class BamlDocumentStartRecord : BamlRecord
	{
		// Token: 0x06003D84 RID: 15748 RVA: 0x001FD22B File Offset: 0x001FC22B
		internal override void Write(BinaryWriter bamlBinaryWriter)
		{
			if (this.FilePos == -1L && bamlBinaryWriter != null)
			{
				this.FilePos = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			}
			base.Write(bamlBinaryWriter);
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x001FD250 File Offset: 0x001FC250
		internal virtual void UpdateWrite(BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Seek((int)this.FilePos, SeekOrigin.Begin);
			this.Write(bamlBinaryWriter);
			bamlBinaryWriter.Seek((int)num, SeekOrigin.Begin);
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x001FD286 File Offset: 0x001FC286
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.LoadAsync = bamlBinaryReader.ReadBoolean();
			this.MaxAsyncRecords = bamlBinaryReader.ReadInt32();
			this.DebugBaml = bamlBinaryReader.ReadBoolean();
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x001FD2AC File Offset: 0x001FC2AC
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.LoadAsync);
			bamlBinaryWriter.Write(this.MaxAsyncRecords);
			bamlBinaryWriter.Write(this.DebugBaml);
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x001FD2D2 File Offset: 0x001FC2D2
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDocumentStartRecord bamlDocumentStartRecord = (BamlDocumentStartRecord)record;
			bamlDocumentStartRecord._maxAsyncRecords = this._maxAsyncRecords;
			bamlDocumentStartRecord._loadAsync = this._loadAsync;
			bamlDocumentStartRecord._filePos = this._filePos;
			bamlDocumentStartRecord._debugBaml = this._debugBaml;
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06003D89 RID: 15753 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DocumentStart;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x001FD310 File Offset: 0x001FC310
		// (set) Token: 0x06003D8B RID: 15755 RVA: 0x001FD318 File Offset: 0x001FC318
		internal bool LoadAsync
		{
			get
			{
				return this._loadAsync;
			}
			set
			{
				this._loadAsync = value;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x001FD321 File Offset: 0x001FC321
		// (set) Token: 0x06003D8D RID: 15757 RVA: 0x001FD329 File Offset: 0x001FC329
		internal int MaxAsyncRecords
		{
			get
			{
				return this._maxAsyncRecords;
			}
			set
			{
				this._maxAsyncRecords = value;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x001FD332 File Offset: 0x001FC332
		// (set) Token: 0x06003D8F RID: 15759 RVA: 0x001FD33A File Offset: 0x001FC33A
		internal long FilePos
		{
			get
			{
				return this._filePos;
			}
			set
			{
				this._filePos = value;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x001FD343 File Offset: 0x001FC343
		// (set) Token: 0x06003D91 RID: 15761 RVA: 0x001FD34B File Offset: 0x001FC34B
		internal bool DebugBaml
		{
			get
			{
				return this._debugBaml;
			}
			set
			{
				this._debugBaml = value;
			}
		}

		// Token: 0x04001EDE RID: 7902
		private int _maxAsyncRecords = -1;

		// Token: 0x04001EDF RID: 7903
		private bool _loadAsync;

		// Token: 0x04001EE0 RID: 7904
		private long _filePos = -1L;

		// Token: 0x04001EE1 RID: 7905
		private bool _debugBaml;
	}
}
