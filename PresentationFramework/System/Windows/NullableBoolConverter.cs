using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows
{
	// Token: 0x02000386 RID: 902
	public class NullableBoolConverter : NullableConverter
	{
		// Token: 0x06002457 RID: 9303 RVA: 0x001823BF File Offset: 0x001813BF
		public NullableBoolConverter() : base(typeof(bool?))
		{
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x001823D4 File Offset: 0x001813D4
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (NullableBoolConverter._standardValues == null)
			{
				NullableBoolConverter._standardValues = new TypeConverter.StandardValuesCollection(new ArrayList(3)
				{
					true,
					false,
					null
				}.ToArray());
			}
			return NullableBoolConverter._standardValues;
		}

		// Token: 0x0400113D RID: 4413
		[ThreadStatic]
		private static TypeConverter.StandardValuesCollection _standardValues;
	}
}
