using System;

namespace System.Windows.Controls
{
	// Token: 0x020007FC RID: 2044
	public class ValidationResult
	{
		// Token: 0x060076E8 RID: 30440 RVA: 0x002F0D58 File Offset: 0x002EFD58
		public ValidationResult(bool isValid, object errorContent)
		{
			this._isValid = isValid;
			this._errorContent = errorContent;
		}

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x060076E9 RID: 30441 RVA: 0x002F0D6E File Offset: 0x002EFD6E
		public bool IsValid
		{
			get
			{
				return this._isValid;
			}
		}

		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x060076EA RID: 30442 RVA: 0x002F0D76 File Offset: 0x002EFD76
		public object ErrorContent
		{
			get
			{
				return this._errorContent;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x060076EB RID: 30443 RVA: 0x002F0D7E File Offset: 0x002EFD7E
		public static ValidationResult ValidResult
		{
			get
			{
				return ValidationResult.s_valid;
			}
		}

		// Token: 0x060076EC RID: 30444 RVA: 0x002F0D85 File Offset: 0x002EFD85
		public static bool operator ==(ValidationResult left, ValidationResult right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x060076ED RID: 30445 RVA: 0x002F0D8E File Offset: 0x002EFD8E
		public static bool operator !=(ValidationResult left, ValidationResult right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x060076EE RID: 30446 RVA: 0x002F0D9C File Offset: 0x002EFD9C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ValidationResult validationResult = obj as ValidationResult;
			return validationResult != null && this.IsValid == validationResult.IsValid && this.ErrorContent == validationResult.ErrorContent;
		}

		// Token: 0x060076EF RID: 30447 RVA: 0x002F0DE0 File Offset: 0x002EFDE0
		public override int GetHashCode()
		{
			return this.IsValid.GetHashCode() ^ ((this.ErrorContent == null) ? int.MinValue : this.ErrorContent).GetHashCode();
		}

		// Token: 0x040038B4 RID: 14516
		private bool _isValid;

		// Token: 0x040038B5 RID: 14517
		private object _errorContent;

		// Token: 0x040038B6 RID: 14518
		private static readonly ValidationResult s_valid = new ValidationResult(true, null);
	}
}
