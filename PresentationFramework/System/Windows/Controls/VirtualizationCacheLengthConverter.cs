using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000805 RID: 2053
	public class VirtualizationCacheLengthConverter : TypeConverter
	{
		// Token: 0x06007731 RID: 30513 RVA: 0x00174764 File Offset: 0x00173764
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		// Token: 0x06007732 RID: 30514 RVA: 0x00174786 File Offset: 0x00173786
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		// Token: 0x06007733 RID: 30515 RVA: 0x002F170F File Offset: 0x002F070F
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return VirtualizationCacheLengthConverter.FromString((string)source, cultureInfo);
			}
			return new VirtualizationCacheLength(Convert.ToDouble(source, cultureInfo));
		}

		// Token: 0x06007734 RID: 30516 RVA: 0x002F1748 File Offset: 0x002F0748
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is VirtualizationCacheLength)
			{
				VirtualizationCacheLength cacheLength = (VirtualizationCacheLength)value;
				if (destinationType == typeof(string))
				{
					return VirtualizationCacheLengthConverter.ToString(cacheLength, cultureInfo);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					return new InstanceDescriptor(typeof(VirtualizationCacheLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(VirtualizationCacheLengthUnit)
					}), new object[]
					{
						cacheLength.CacheBeforeViewport,
						cacheLength.CacheAfterViewport
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06007735 RID: 30517 RVA: 0x002F1814 File Offset: 0x002F0814
		internal static string ToString(VirtualizationCacheLength cacheLength, CultureInfo cultureInfo)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);
			StringBuilder stringBuilder = new StringBuilder(26);
			stringBuilder.Append(cacheLength.CacheBeforeViewport.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cacheLength.CacheAfterViewport.ToString(cultureInfo));
			return stringBuilder.ToString();
		}

		// Token: 0x06007736 RID: 30518 RVA: 0x002F186C File Offset: 0x002F086C
		internal static VirtualizationCacheLength FromString(string s, CultureInfo cultureInfo)
		{
			TokenizerHelper tokenizerHelper = new TokenizerHelper(s, cultureInfo);
			double[] array = new double[2];
			int num = 0;
			while (tokenizerHelper.NextToken())
			{
				if (num >= 2)
				{
					num = 3;
					break;
				}
				array[num] = double.Parse(tokenizerHelper.GetCurrentToken(), cultureInfo);
				num++;
			}
			if (num == 1)
			{
				return new VirtualizationCacheLength(array[0]);
			}
			if (num != 2)
			{
				throw new FormatException(SR.Get("InvalidStringVirtualizationCacheLength", new object[]
				{
					s
				}));
			}
			return new VirtualizationCacheLength(array[0], array[1]);
		}
	}
}
