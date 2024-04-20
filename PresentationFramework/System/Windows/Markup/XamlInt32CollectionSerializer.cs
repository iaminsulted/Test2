using System;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x020004E0 RID: 1248
	internal class XamlInt32CollectionSerializer : XamlSerializer
	{
		// Token: 0x06003F87 RID: 16263 RVA: 0x00211F98 File Offset: 0x00210F98
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			Int32Collection int32Collection = Int32Collection.Parse(stringValue);
			int num = 0;
			int count = int32Collection.Count;
			bool flag = true;
			bool flag2 = true;
			for (int i = 1; i < count; i++)
			{
				int num2 = int32Collection.Internal_GetItem(i - 1);
				int num3 = int32Collection.Internal_GetItem(i);
				if (flag && num2 + 1 != num3)
				{
					flag = false;
				}
				if (num3 < 0)
				{
					flag2 = false;
				}
				if (num3 > num)
				{
					num = num3;
				}
			}
			if (flag)
			{
				writer.Write(1);
				writer.Write(count);
				writer.Write(int32Collection.Internal_GetItem(0));
			}
			else
			{
				XamlInt32CollectionSerializer.IntegerCollectionType value;
				if (flag2 && num <= 255)
				{
					value = XamlInt32CollectionSerializer.IntegerCollectionType.Byte;
				}
				else if (flag2 && num <= 65535)
				{
					value = XamlInt32CollectionSerializer.IntegerCollectionType.UShort;
				}
				else
				{
					value = XamlInt32CollectionSerializer.IntegerCollectionType.Integer;
				}
				writer.Write((byte)value);
				writer.Write(count);
				switch (value)
				{
				case XamlInt32CollectionSerializer.IntegerCollectionType.Byte:
					for (int j = 0; j < count; j++)
					{
						writer.Write((byte)int32Collection.Internal_GetItem(j));
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.UShort:
					for (int k = 0; k < count; k++)
					{
						writer.Write((ushort)int32Collection.Internal_GetItem(k));
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.Integer:
					for (int l = 0; l < count; l++)
					{
						writer.Write(int32Collection.Internal_GetItem(l));
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x002120CE File Offset: 0x002110CE
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return XamlInt32CollectionSerializer.DeserializeFrom(reader);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x002120D6 File Offset: 0x002110D6
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return XamlInt32CollectionSerializer.DeserializeFrom(reader);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x002120E0 File Offset: 0x002110E0
		private static Int32Collection DeserializeFrom(BinaryReader reader)
		{
			XamlInt32CollectionSerializer.IntegerCollectionType integerCollectionType = (XamlInt32CollectionSerializer.IntegerCollectionType)reader.ReadByte();
			int num = reader.ReadInt32();
			if (num < 0)
			{
				throw new ArgumentException(SR.Get("IntegerCollectionLengthLessThanZero"));
			}
			Int32Collection int32Collection = new Int32Collection(num);
			if (integerCollectionType == XamlInt32CollectionSerializer.IntegerCollectionType.Consecutive)
			{
				int num2 = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int32Collection.Add(num2 + i);
				}
			}
			else
			{
				switch (integerCollectionType)
				{
				case XamlInt32CollectionSerializer.IntegerCollectionType.Byte:
					for (int j = 0; j < num; j++)
					{
						int32Collection.Add((int)reader.ReadByte());
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.UShort:
					for (int k = 0; k < num; k++)
					{
						int32Collection.Add((int)reader.ReadUInt16());
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.Integer:
					for (int l = 0; l < num; l++)
					{
						int value = reader.ReadInt32();
						int32Collection.Add(value);
					}
					break;
				default:
					throw new ArgumentException(SR.Get("UnknownIndexType"));
				}
			}
			return int32Collection;
		}

		// Token: 0x02000AFC RID: 2812
		internal enum IntegerCollectionType : byte
		{
			// Token: 0x04004755 RID: 18261
			Unknown,
			// Token: 0x04004756 RID: 18262
			Consecutive,
			// Token: 0x04004757 RID: 18263
			Byte,
			// Token: 0x04004758 RID: 18264
			UShort,
			// Token: 0x04004759 RID: 18265
			Integer
		}
	}
}
