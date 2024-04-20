using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004B5 RID: 1205
	internal class BamlTypeInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003DA8 RID: 15784 RVA: 0x001FD4A3 File Offset: 0x001FC4A3
		internal BamlTypeInfoRecord()
		{
			base.Pin();
			this.TypeId = -1;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x001FD4C0 File Offset: 0x001FC4C0
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
			this.AssemblyId = bamlBinaryReader.ReadInt16();
			this.TypeFullName = bamlBinaryReader.ReadString();
			this._typeInfoFlags = (BamlTypeInfoRecord.TypeInfoFlags)(this.AssemblyId >> 12);
			this._assemblyId &= 4095;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x001FD514 File Offset: 0x001FC514
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
			bamlBinaryWriter.Write((short)((ushort)this.AssemblyId | (ushort)(this._typeInfoFlags << 12)));
			bamlBinaryWriter.Write(this.TypeFullName);
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x001FD546 File Offset: 0x001FC546
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)record;
			bamlTypeInfoRecord._typeInfoFlags = this._typeInfoFlags;
			bamlTypeInfoRecord._assemblyId = this._assemblyId;
			bamlTypeInfoRecord._typeFullName = this._typeFullName;
			bamlTypeInfoRecord._type = this._type;
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x001FD584 File Offset: 0x001FC584
		// (set) Token: 0x06003DAD RID: 15789 RVA: 0x001FD5AC File Offset: 0x001FC5AC
		internal short TypeId
		{
			get
			{
				return (short)this._flags[BamlTypeInfoRecord._typeIdLowSection] | (short)(this._flags[BamlTypeInfoRecord._typeIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlTypeInfoRecord._typeIdLowSection] = (int)(value & 255);
				this._flags[BamlTypeInfoRecord._typeIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x001FD5E0 File Offset: 0x001FC5E0
		// (set) Token: 0x06003DAF RID: 15791 RVA: 0x001FD5E8 File Offset: 0x001FC5E8
		internal short AssemblyId
		{
			get
			{
				return this._assemblyId;
			}
			set
			{
				if (this._assemblyId > 4095)
				{
					throw new XamlParseException(SR.Get("ParserTooManyAssemblies"));
				}
				this._assemblyId = value;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06003DB0 RID: 15792 RVA: 0x001FD60E File Offset: 0x001FC60E
		// (set) Token: 0x06003DB1 RID: 15793 RVA: 0x001FD616 File Offset: 0x001FC616
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
			set
			{
				this._typeFullName = value;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x001FD61F File Offset: 0x001FC61F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TypeInfo;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x001FD623 File Offset: 0x001FC623
		// (set) Token: 0x06003DB4 RID: 15796 RVA: 0x001FD62B File Offset: 0x001FC62B
		internal Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x001FD634 File Offset: 0x001FC634
		internal string ClrNamespace
		{
			get
			{
				int num = this._typeFullName.LastIndexOf('.');
				if (num <= 0)
				{
					return string.Empty;
				}
				return this._typeFullName.Substring(0, num);
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x001FD666 File Offset: 0x001FC666
		// (set) Token: 0x06003DB8 RID: 15800 RVA: 0x001FD673 File Offset: 0x001FC673
		internal bool IsInternalType
		{
			get
			{
				return (this._typeInfoFlags & BamlTypeInfoRecord.TypeInfoFlags.Internal) == BamlTypeInfoRecord.TypeInfoFlags.Internal;
			}
			set
			{
				if (value)
				{
					this._typeInfoFlags |= BamlTypeInfoRecord.TypeInfoFlags.Internal;
				}
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x001FD686 File Offset: 0x001FC686
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlTypeInfoRecord._typeIdHighSection;
			}
		}

		// Token: 0x04001EE6 RID: 7910
		private static BitVector32.Section _typeIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001EE7 RID: 7911
		private static BitVector32.Section _typeIdHighSection = BitVector32.CreateSection(255, BamlTypeInfoRecord._typeIdLowSection);

		// Token: 0x04001EE8 RID: 7912
		private BamlTypeInfoRecord.TypeInfoFlags _typeInfoFlags;

		// Token: 0x04001EE9 RID: 7913
		private short _assemblyId = -1;

		// Token: 0x04001EEA RID: 7914
		private string _typeFullName;

		// Token: 0x04001EEB RID: 7915
		private Type _type;

		// Token: 0x02000AF1 RID: 2801
		[Flags]
		private enum TypeInfoFlags : byte
		{
			// Token: 0x0400473A RID: 18234
			Internal = 1,
			// Token: 0x0400473B RID: 18235
			UnusedTwo = 2,
			// Token: 0x0400473C RID: 18236
			UnusedThree = 4
		}
	}
}
