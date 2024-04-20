using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200048C RID: 1164
	internal class BamlDefAttributeKeyStringRecord : BamlStringValueRecord, IBamlDictionaryKey
	{
		// Token: 0x06003C6C RID: 15468 RVA: 0x001FBB7D File Offset: 0x001FAB7D
		internal BamlDefAttributeKeyStringRecord()
		{
			base.Pin();
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x001FBB93 File Offset: 0x001FAB93
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ValueId = bamlBinaryReader.ReadInt16();
			this._valuePosition = bamlBinaryReader.ReadInt32();
			((IBamlDictionaryKey)this).Shared = bamlBinaryReader.ReadBoolean();
			((IBamlDictionaryKey)this).SharedSet = bamlBinaryReader.ReadBoolean();
			this._keyObject = null;
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x001FBBCC File Offset: 0x001FABCC
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ValueId);
			this._valuePositionPosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this._valuePosition);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).Shared);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).SharedSet);
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x001FBC0C File Offset: 0x001FAC0C
		void IBamlDictionaryKey.UpdateValuePosition(int newPosition, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._valuePositionPosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(newPosition);
			bamlBinaryWriter.Seek(-4 - num2, SeekOrigin.Current);
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x001FBC48 File Offset: 0x001FAC48
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = (BamlDefAttributeKeyStringRecord)record;
			bamlDefAttributeKeyStringRecord._valuePosition = this._valuePosition;
			bamlDefAttributeKeyStringRecord._valuePositionPosition = this._valuePositionPosition;
			bamlDefAttributeKeyStringRecord._keyObject = this._keyObject;
			bamlDefAttributeKeyStringRecord._valueId = this._valueId;
			bamlDefAttributeKeyStringRecord._staticResourceValues = this._staticResourceValues;
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06003C71 RID: 15473 RVA: 0x001FBC9D File Offset: 0x001FAC9D
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DefAttributeKeyString;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06003C72 RID: 15474 RVA: 0x001FBCA1 File Offset: 0x001FACA1
		// (set) Token: 0x06003C73 RID: 15475 RVA: 0x001FBCA9 File Offset: 0x001FACA9
		int IBamlDictionaryKey.ValuePosition
		{
			get
			{
				return this._valuePosition;
			}
			set
			{
				this._valuePosition = value;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x001FBCB2 File Offset: 0x001FACB2
		// (set) Token: 0x06003C75 RID: 15477 RVA: 0x001FBCCA File Offset: 0x001FACCA
		bool IBamlDictionaryKey.Shared
		{
			get
			{
				return this._flags[BamlDefAttributeKeyStringRecord._sharedSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyStringRecord._sharedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06003C76 RID: 15478 RVA: 0x001FBCE3 File Offset: 0x001FACE3
		// (set) Token: 0x06003C77 RID: 15479 RVA: 0x001FBCFB File Offset: 0x001FACFB
		bool IBamlDictionaryKey.SharedSet
		{
			get
			{
				return this._flags[BamlDefAttributeKeyStringRecord._sharedSetSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyStringRecord._sharedSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x001FBD14 File Offset: 0x001FAD14
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlDefAttributeKeyStringRecord._sharedSetSection;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06003C79 RID: 15481 RVA: 0x001FBD1B File Offset: 0x001FAD1B
		// (set) Token: 0x06003C7A RID: 15482 RVA: 0x001FBD23 File Offset: 0x001FAD23
		object IBamlDictionaryKey.KeyObject
		{
			get
			{
				return this._keyObject;
			}
			set
			{
				this._keyObject = value;
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06003C7B RID: 15483 RVA: 0x001FBD2C File Offset: 0x001FAD2C
		// (set) Token: 0x06003C7C RID: 15484 RVA: 0x001FBD34 File Offset: 0x001FAD34
		long IBamlDictionaryKey.ValuePositionPosition
		{
			get
			{
				return this._valuePositionPosition;
			}
			set
			{
				this._valuePositionPosition = value;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x001FBD3D File Offset: 0x001FAD3D
		// (set) Token: 0x06003C7E RID: 15486 RVA: 0x001FBD45 File Offset: 0x001FAD45
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

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06003C7F RID: 15487 RVA: 0x001FBD4E File Offset: 0x001FAD4E
		// (set) Token: 0x06003C80 RID: 15488 RVA: 0x001FBD56 File Offset: 0x001FAD56
		object[] IBamlDictionaryKey.StaticResourceValues
		{
			get
			{
				return this._staticResourceValues;
			}
			set
			{
				this._staticResourceValues = value;
			}
		}

		// Token: 0x04001EA1 RID: 7841
		private static BitVector32.Section _sharedSection = BitVector32.CreateSection(1, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001EA2 RID: 7842
		private static BitVector32.Section _sharedSetSection = BitVector32.CreateSection(1, BamlDefAttributeKeyStringRecord._sharedSection);

		// Token: 0x04001EA3 RID: 7843
		internal const int ValuePositionSize = 4;

		// Token: 0x04001EA4 RID: 7844
		private int _valuePosition;

		// Token: 0x04001EA5 RID: 7845
		private long _valuePositionPosition = -1L;

		// Token: 0x04001EA6 RID: 7846
		private object _keyObject;

		// Token: 0x04001EA7 RID: 7847
		private short _valueId;

		// Token: 0x04001EA8 RID: 7848
		private object[] _staticResourceValues;
	}
}
