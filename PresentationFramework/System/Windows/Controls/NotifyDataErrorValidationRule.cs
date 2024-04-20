using System;
using System.Globalization;

namespace System.Windows.Controls
{
	// Token: 0x020007B2 RID: 1970
	public sealed class NotifyDataErrorValidationRule : ValidationRule
	{
		// Token: 0x06006FEF RID: 28655 RVA: 0x0029EF4A File Offset: 0x0029DF4A
		public NotifyDataErrorValidationRule() : base(ValidationStep.UpdatedValue, true)
		{
		}

		// Token: 0x06006FF0 RID: 28656 RVA: 0x0029EC1A File Offset: 0x0029DC1A
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x040036BA RID: 14010
		internal static readonly NotifyDataErrorValidationRule Instance = new NotifyDataErrorValidationRule();
	}
}
