using System;
using System.Globalization;

namespace System.Windows.Controls
{
	// Token: 0x02000779 RID: 1913
	public sealed class ExceptionValidationRule : ValidationRule
	{
		// Token: 0x06006866 RID: 26726 RVA: 0x0029EC1A File Offset: 0x0029DC1A
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return ValidationResult.ValidResult;
		}

		// Token: 0x040034A5 RID: 13477
		internal static readonly ExceptionValidationRule Instance = new ExceptionValidationRule();
	}
}
