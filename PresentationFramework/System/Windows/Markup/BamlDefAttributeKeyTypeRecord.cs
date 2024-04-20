using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200048B RID: 1163
	internal class BamlDefAttributeKeyTypeRecord : BamlElementStartRecord, IBamlDictionaryKey
	{
		// Token: 0x06003C58 RID: 15448 RVA: 0x001FB9B1 File Offset: 0x001FA9B1
		internal BamlDefAttributeKeyTypeRecord()
		{
			base.Pin();
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x001FB9C7 File Offset: 0x001FA9C7
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this._valuePosition = bamlBinaryReader.ReadInt32();
			((IBamlDictionaryKey)this).Shared = bamlBinaryReader.ReadBoolean();
			((IBamlDictionaryKey)this).SharedSet = bamlBinaryReader.ReadBoolean();
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x001FB9F4 File Offset: 0x001FA9F4
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			this._valuePositionPosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this._valuePosition);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).Shared);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).SharedSet);
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x001FBA30 File Offset: 0x001FAA30
		void IBamlDictionaryKey.UpdateValuePosition(int newPosition, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._valuePositionPosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(newPosition);
			bamlBinaryWriter.Seek(-4 - num2, SeekOrigin.Current);
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x001FBA6C File Offset: 0x001FAA6C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = (BamlDefAttributeKeyTypeRecord)record;
			bamlDefAttributeKeyTypeRecord._valuePosition = this._valuePosition;
			bamlDefAttributeKeyTypeRecord._valuePositionPosition = this._valuePositionPosition;
			bamlDefAttributeKeyTypeRecord._keyObject = this._keyObject;
			bamlDefAttributeKeyTypeRecord._staticResourceValues = this._staticResourceValues;
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x001FBAAA File Offset: 0x001FAAAA
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DefAttributeKeyType;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06003C5E RID: 15454 RVA: 0x001FBAAE File Offset: 0x001FAAAE
		// (set) Token: 0x06003C5F RID: 15455 RVA: 0x001FBAB6 File Offset: 0x001FAAB6
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

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06003C60 RID: 15456 RVA: 0x001FBABF File Offset: 0x001FAABF
		// (set) Token: 0x06003C61 RID: 15457 RVA: 0x001FBAC7 File Offset: 0x001FAAC7
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

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x001FBAD0 File Offset: 0x001FAAD0
		// (set) Token: 0x06003C63 RID: 15459 RVA: 0x001FBAD8 File Offset: 0x001FAAD8
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

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x001FBAE1 File Offset: 0x001FAAE1
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x001FBAF9 File Offset: 0x001FAAF9
		bool IBamlDictionaryKey.Shared
		{
			get
			{
				return this._flags[BamlDefAttributeKeyTypeRecord._sharedSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyTypeRecord._sharedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06003C66 RID: 15462 RVA: 0x001FBB12 File Offset: 0x001FAB12
		// (set) Token: 0x06003C67 RID: 15463 RVA: 0x001FBB2A File Offset: 0x001FAB2A
		bool IBamlDictionaryKey.SharedSet
		{
			get
			{
				return this._flags[BamlDefAttributeKeyTypeRecord._sharedSetSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyTypeRecord._sharedSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06003C68 RID: 15464 RVA: 0x001FBB43 File Offset: 0x001FAB43
		// (set) Token: 0x06003C69 RID: 15465 RVA: 0x001FBB4B File Offset: 0x001FAB4B
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

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003C6A RID: 15466 RVA: 0x001FBB54 File Offset: 0x001FAB54
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlDefAttributeKeyTypeRecord._sharedSetSection;
			}
		}

		// Token: 0x04001E9A RID: 7834
		private static BitVector32.Section _sharedSection = BitVector32.CreateSection(1, BamlElementStartRecord.LastFlagsSection);

		// Token: 0x04001E9B RID: 7835
		private static BitVector32.Section _sharedSetSection = BitVector32.CreateSection(1, BamlDefAttributeKeyTypeRecord._sharedSection);

		// Token: 0x04001E9C RID: 7836
		internal const int ValuePositionSize = 4;

		// Token: 0x04001E9D RID: 7837
		private int _valuePosition;

		// Token: 0x04001E9E RID: 7838
		private long _valuePositionPosition = -1L;

		// Token: 0x04001E9F RID: 7839
		private object _keyObject;

		// Token: 0x04001EA0 RID: 7840
		private object[] _staticResourceValues;
	}
}
