using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004AD RID: 1197
	internal class BamlTextWithIdRecord : BamlTextRecord
	{
		// Token: 0x06003D76 RID: 15734 RVA: 0x001FD17F File Offset: 0x001FC17F
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ValueId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x001FD18D File Offset: 0x001FC18D
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x001FD19B File Offset: 0x001FC19B
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlTextWithIdRecord)record)._valueId = this._valueId;
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003D79 RID: 15737 RVA: 0x001FD1B5 File Offset: 0x001FC1B5
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TextWithId;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06003D7A RID: 15738 RVA: 0x001FD1B9 File Offset: 0x001FC1B9
		// (set) Token: 0x06003D7B RID: 15739 RVA: 0x001FD1C1 File Offset: 0x001FC1C1
		internal short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
			}
		}

		// Token: 0x04001EDC RID: 7900
		private short _valueId;
	}
}
