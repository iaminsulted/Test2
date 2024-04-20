using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A3 RID: 1187
	internal class BamlConnectionIdRecord : BamlRecord
	{
		// Token: 0x06003D1F RID: 15647 RVA: 0x001FCB53 File Offset: 0x001FBB53
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ConnectionId = bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x001FCB61 File Offset: 0x001FBB61
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ConnectionId);
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x001FCB6F File Offset: 0x001FBB6F
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlConnectionIdRecord)record)._connectionId = this._connectionId;
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06003D22 RID: 15650 RVA: 0x001FCB89 File Offset: 0x001FBB89
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ConnectionId;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x001FCB8D File Offset: 0x001FBB8D
		// (set) Token: 0x06003D24 RID: 15652 RVA: 0x001FCB95 File Offset: 0x001FBB95
		internal int ConnectionId
		{
			get
			{
				return this._connectionId;
			}
			set
			{
				this._connectionId = value;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x001FC019 File Offset: 0x001FB019
		// (set) Token: 0x06003D26 RID: 15654 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
			set
			{
			}
		}

		// Token: 0x04001ECA RID: 7882
		private int _connectionId = -1;
	}
}
