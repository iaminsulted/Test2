using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020004B4 RID: 1204
	internal class BamlAssemblyInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003D9B RID: 15771 RVA: 0x001FD381 File Offset: 0x001FC381
		internal BamlAssemblyInfoRecord()
		{
			base.Pin();
			this.AssemblyId = -1;
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x001FD396 File Offset: 0x001FC396
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AssemblyId = bamlBinaryReader.ReadInt16();
			this.AssemblyFullName = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x001FD3B0 File Offset: 0x001FC3B0
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AssemblyId);
			bamlBinaryWriter.Write(this.AssemblyFullName);
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x001FD3CA File Offset: 0x001FC3CA
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = (BamlAssemblyInfoRecord)record;
			bamlAssemblyInfoRecord._assemblyFullName = this._assemblyFullName;
			bamlAssemblyInfoRecord._assembly = this._assembly;
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x001FD3F0 File Offset: 0x001FC3F0
		// (set) Token: 0x06003DA0 RID: 15776 RVA: 0x001FD418 File Offset: 0x001FC418
		internal short AssemblyId
		{
			get
			{
				return (short)this._flags[BamlAssemblyInfoRecord._assemblyIdLowSection] | (short)(this._flags[BamlAssemblyInfoRecord._assemblyIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlAssemblyInfoRecord._assemblyIdLowSection] = (int)(value & 255);
				this._flags[BamlAssemblyInfoRecord._assemblyIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x001FD44C File Offset: 0x001FC44C
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlAssemblyInfoRecord._assemblyIdHighSection;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x001FD453 File Offset: 0x001FC453
		// (set) Token: 0x06003DA3 RID: 15779 RVA: 0x001FD45B File Offset: 0x001FC45B
		internal string AssemblyFullName
		{
			get
			{
				return this._assemblyFullName;
			}
			set
			{
				this._assemblyFullName = value;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.AssemblyInfo;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x001FD468 File Offset: 0x001FC468
		// (set) Token: 0x06003DA6 RID: 15782 RVA: 0x001FD470 File Offset: 0x001FC470
		internal Assembly Assembly
		{
			get
			{
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}

		// Token: 0x04001EE2 RID: 7906
		private static BitVector32.Section _assemblyIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001EE3 RID: 7907
		private static BitVector32.Section _assemblyIdHighSection = BitVector32.CreateSection(255, BamlAssemblyInfoRecord._assemblyIdLowSection);

		// Token: 0x04001EE4 RID: 7908
		private string _assemblyFullName;

		// Token: 0x04001EE5 RID: 7909
		private Assembly _assembly;
	}
}
