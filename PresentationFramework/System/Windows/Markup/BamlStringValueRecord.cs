using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000488 RID: 1160
	internal abstract class BamlStringValueRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003C41 RID: 15425 RVA: 0x001FB96A File Offset: 0x001FA96A
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x001FB978 File Offset: 0x001FA978
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.Value);
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x001FB986 File Offset: 0x001FA986
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			((BamlStringValueRecord)record)._value = this._value;
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x001FB9A0 File Offset: 0x001FA9A0
		// (set) Token: 0x06003C45 RID: 15429 RVA: 0x001FB9A8 File Offset: 0x001FA9A8
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04001E99 RID: 7833
		private string _value;
	}
}
