using System;
using System.Globalization;

namespace System.Windows.Controls
{
	// Token: 0x0200073B RID: 1851
	internal sealed class ConversionValidationRule : ValidationRule
	{
		// Token: 0x06006225 RID: 25125 RVA: 0x0029EC10 File Offset: 0x0029DC10
		internal ConversionValidationRule() : base(ValidationStep.ConvertedProposedValue, false)
		{
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x0029EC1A File Offset: 0x0029DC1A
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x040032AF RID: 12975
		internal static readonly ConversionValidationRule Instance = new ConversionValidationRule();
	}
}
