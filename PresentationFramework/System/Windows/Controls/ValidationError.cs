using System;

namespace System.Windows.Controls
{
	// Token: 0x020007F9 RID: 2041
	public class ValidationError
	{
		// Token: 0x060076DB RID: 30427 RVA: 0x002F0C7C File Offset: 0x002EFC7C
		public ValidationError(ValidationRule ruleInError, object bindingInError, object errorContent, Exception exception)
		{
			if (ruleInError == null)
			{
				throw new ArgumentNullException("ruleInError");
			}
			if (bindingInError == null)
			{
				throw new ArgumentNullException("bindingInError");
			}
			this._ruleInError = ruleInError;
			this._bindingInError = bindingInError;
			this._errorContent = errorContent;
			this._exception = exception;
		}

		// Token: 0x060076DC RID: 30428 RVA: 0x002F0CC8 File Offset: 0x002EFCC8
		public ValidationError(ValidationRule ruleInError, object bindingInError) : this(ruleInError, bindingInError, null, null)
		{
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x060076DD RID: 30429 RVA: 0x002F0CD4 File Offset: 0x002EFCD4
		// (set) Token: 0x060076DE RID: 30430 RVA: 0x002F0CDC File Offset: 0x002EFCDC
		public ValidationRule RuleInError
		{
			get
			{
				return this._ruleInError;
			}
			set
			{
				this._ruleInError = value;
			}
		}

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x060076DF RID: 30431 RVA: 0x002F0CE5 File Offset: 0x002EFCE5
		// (set) Token: 0x060076E0 RID: 30432 RVA: 0x002F0CED File Offset: 0x002EFCED
		public object ErrorContent
		{
			get
			{
				return this._errorContent;
			}
			set
			{
				this._errorContent = value;
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x060076E1 RID: 30433 RVA: 0x002F0CF6 File Offset: 0x002EFCF6
		// (set) Token: 0x060076E2 RID: 30434 RVA: 0x002F0CFE File Offset: 0x002EFCFE
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
			set
			{
				this._exception = value;
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x060076E3 RID: 30435 RVA: 0x002F0D07 File Offset: 0x002EFD07
		public object BindingInError
		{
			get
			{
				return this._bindingInError;
			}
		}

		// Token: 0x040038AB RID: 14507
		private ValidationRule _ruleInError;

		// Token: 0x040038AC RID: 14508
		private object _errorContent;

		// Token: 0x040038AD RID: 14509
		private Exception _exception;

		// Token: 0x040038AE RID: 14510
		private object _bindingInError;
	}
}
