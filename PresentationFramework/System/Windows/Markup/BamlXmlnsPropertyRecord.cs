using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000486 RID: 1158
	internal class BamlXmlnsPropertyRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003C29 RID: 15401 RVA: 0x001FB70C File Offset: 0x001FA70C
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.Prefix = bamlBinaryReader.ReadString();
			this.XmlNamespace = bamlBinaryReader.ReadString();
			short num = bamlBinaryReader.ReadInt16();
			if (num > 0)
			{
				this.AssemblyIds = new short[(int)num];
				for (short num2 = 0; num2 < num; num2 += 1)
				{
					this.AssemblyIds[(int)num2] = bamlBinaryReader.ReadInt16();
				}
				return;
			}
			this.AssemblyIds = null;
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x001FB76C File Offset: 0x001FA76C
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.Prefix);
			bamlBinaryWriter.Write(this.XmlNamespace);
			short num = 0;
			if (this.AssemblyIds != null && this.AssemblyIds.Length != 0)
			{
				num = (short)this.AssemblyIds.Length;
			}
			bamlBinaryWriter.Write(num);
			if (num > 0)
			{
				for (short num2 = 0; num2 < num; num2 += 1)
				{
					bamlBinaryWriter.Write(this.AssemblyIds[(int)num2]);
				}
			}
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x001FB7D4 File Offset: 0x001FA7D4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlXmlnsPropertyRecord bamlXmlnsPropertyRecord = (BamlXmlnsPropertyRecord)record;
			bamlXmlnsPropertyRecord._prefix = this._prefix;
			bamlXmlnsPropertyRecord._xmlNamespace = this._xmlNamespace;
			bamlXmlnsPropertyRecord._assemblyIds = this._assemblyIds;
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x001FB806 File Offset: 0x001FA806
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.XmlnsProperty;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x001FB80A File Offset: 0x001FA80A
		// (set) Token: 0x06003C2E RID: 15406 RVA: 0x001FB812 File Offset: 0x001FA812
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x001FB81B File Offset: 0x001FA81B
		// (set) Token: 0x06003C30 RID: 15408 RVA: 0x001FB823 File Offset: 0x001FA823
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
			set
			{
				this._xmlNamespace = value;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x001FB82C File Offset: 0x001FA82C
		// (set) Token: 0x06003C32 RID: 15410 RVA: 0x001FB834 File Offset: 0x001FA834
		internal short[] AssemblyIds
		{
			get
			{
				return this._assemblyIds;
			}
			set
			{
				this._assemblyIds = value;
			}
		}

		// Token: 0x04001E92 RID: 7826
		private string _prefix;

		// Token: 0x04001E93 RID: 7827
		private string _xmlNamespace;

		// Token: 0x04001E94 RID: 7828
		private short[] _assemblyIds;
	}
}
