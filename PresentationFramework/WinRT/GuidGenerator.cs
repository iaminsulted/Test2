using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace WinRT
{
	// Token: 0x020000A7 RID: 167
	internal static class GuidGenerator
	{
		// Token: 0x0600026E RID: 622 RVA: 0x000FA6B1 File Offset: 0x000F96B1
		public static Guid GetGUID(Type type)
		{
			return type.GetGuidType().GUID;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000FA6BE File Offset: 0x000F96BE
		public static Guid GetIID(Type type)
		{
			type = type.GetGuidType();
			if (!type.IsGenericType)
			{
				return type.GUID;
			}
			return (Guid)type.GetField("PIID").GetValue(null);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x000FA6F0 File Offset: 0x000F96F0
		public static string GetSignature(Type type)
		{
			Type type2 = type.FindHelperType();
			if (type2 != null)
			{
				MethodInfo method = type2.GetMethod("GetGuidSignature", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					MethodBase methodBase = method;
					object obj = null;
					object[] parameters = new Type[0];
					return (string)methodBase.Invoke(obj, parameters);
				}
			}
			if (type == typeof(object))
			{
				return "cinterface(IInspectable)";
			}
			if (type.IsGenericType)
			{
				IEnumerable<string> values = from t in type.GetGenericArguments()
				select GuidGenerator.GetSignature(t);
				return string.Concat(new string[]
				{
					"pinterface({",
					GuidGenerator.GetGUID(type).ToString(),
					"};",
					string.Join(";", values),
					")"
				});
			}
			if (type.IsValueType)
			{
				string name = type.Name;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1324880019U)
				{
					if (num <= 697196164U)
					{
						if (num != 423635464U)
						{
							if (num != 679076413U)
							{
								if (num == 697196164U)
								{
									if (name == "Int64")
									{
										return "i8";
									}
								}
							}
							else if (name == "Char")
							{
								return "c2";
							}
						}
						else if (name == "SByte")
						{
							return "i1";
						}
					}
					else if (num != 765439473U)
					{
						if (num != 1323747186U)
						{
							if (num == 1324880019U)
							{
								if (name == "UInt64")
								{
									return "u8";
								}
							}
						}
						else if (name == "UInt16")
						{
							return "u2";
						}
					}
					else if (name == "Int16")
					{
						return "i2";
					}
				}
				else if (num <= 2898774828U)
				{
					if (num != 2386971688U)
					{
						if (num != 2711245919U)
						{
							if (num == 2898774828U)
							{
								if (name == "Guid")
								{
									return "g16";
								}
							}
						}
						else if (name == "Int32")
						{
							return "i4";
						}
					}
					else if (name == "Double")
					{
						return "f8";
					}
				}
				else if (num <= 3538687084U)
				{
					if (num != 3409549631U)
					{
						if (num == 3538687084U)
						{
							if (name == "UInt32")
							{
								return "u4";
							}
						}
					}
					else if (name == "Byte")
					{
						return "u1";
					}
				}
				else if (num != 3969205087U)
				{
					if (num == 4051133705U)
					{
						if (name == "Single")
						{
							return "f4";
						}
					}
				}
				else if (name == "Boolean")
				{
					return "b1";
				}
				if (type.IsEnum)
				{
					bool flag = type.CustomAttributes.Any((CustomAttributeData cad) => cad.AttributeType == typeof(FlagsAttribute));
					return string.Concat(new string[]
					{
						"enum(",
						TypeExtensions.RemoveNamespacePrefix(type.FullName),
						";",
						flag ? "u4" : "i4",
						")"
					});
				}
				if (!type.IsPrimitive)
				{
					IEnumerable<string> values2 = from fi in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
					select GuidGenerator.GetSignature(fi.FieldType);
					return string.Concat(new string[]
					{
						"struct(",
						TypeExtensions.RemoveNamespacePrefix(type.FullName),
						";",
						string.Join(";", values2),
						")"
					});
				}
				throw new InvalidOperationException("unsupported value type");
			}
			else
			{
				if (type == typeof(string))
				{
					return "string";
				}
				Type type3;
				if (Projections.TryGetDefaultInterfaceTypeForRuntimeClassType(type, out type3))
				{
					return string.Concat(new string[]
					{
						"rc(",
						TypeExtensions.RemoveNamespacePrefix(type.FullName),
						";",
						GuidGenerator.GetSignature(type3),
						")"
					});
				}
				if (type.IsDelegate())
				{
					return "delegate({" + GuidGenerator.GetGUID(type).ToString() + "})";
				}
				return "{" + type.GUID.ToString() + "}";
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x000FABBC File Offset: 0x000F9BBC
		private static Guid encode_guid(byte[] data)
		{
			if (BitConverter.IsLittleEndian)
			{
				byte b = data[0];
				data[0] = data[3];
				data[3] = b;
				b = data[1];
				data[1] = data[2];
				data[2] = b;
				b = data[4];
				data[4] = data[5];
				data[5] = b;
				b = data[6];
				data[6] = data[7];
				data[7] = ((b & 15) | 80);
				data[8] = ((data[8] & 63) | 128);
			}
			return new Guid(data.Take(16).ToArray<byte>());
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000FAC34 File Offset: 0x000F9C34
		public static Guid CreateIID(Type type)
		{
			string signature = GuidGenerator.GetSignature(type);
			if (!type.IsGenericType)
			{
				return new Guid(signature);
			}
			byte[] buffer = GuidGenerator.wrt_pinterface_namespace.ToByteArray().Concat(Encoding.UTF8.GetBytes(signature)).ToArray<byte>();
			Guid result;
			using (SHA1 sha = new SHA1CryptoServiceProvider())
			{
				result = GuidGenerator.encode_guid(sha.ComputeHash(buffer));
			}
			return result;
		}

		// Token: 0x040005B5 RID: 1461
		private static Guid wrt_pinterface_namespace = new Guid("d57af411-737b-c042-abae-878b1e16adee");
	}
}
