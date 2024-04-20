using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x020007FD RID: 2045
	public abstract class ValidationRule
	{
		// Token: 0x060076F1 RID: 30449 RVA: 0x002F0E29 File Offset: 0x002EFE29
		protected ValidationRule() : this(ValidationStep.RawProposedValue, false)
		{
		}

		// Token: 0x060076F2 RID: 30450 RVA: 0x002F0E33 File Offset: 0x002EFE33
		protected ValidationRule(ValidationStep validationStep, bool validatesOnTargetUpdated)
		{
			this._validationStep = validationStep;
			this._validatesOnTargetUpdated = validatesOnTargetUpdated;
		}

		// Token: 0x060076F3 RID: 30451
		public abstract ValidationResult Validate(object value, CultureInfo cultureInfo);

		// Token: 0x060076F4 RID: 30452 RVA: 0x002F0E4C File Offset: 0x002EFE4C
		public virtual ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
		{
			ValidationStep validationStep = this._validationStep;
			if (validationStep - ValidationStep.UpdatedValue <= 1)
			{
				value = owner;
			}
			return this.Validate(value, cultureInfo);
		}

		// Token: 0x060076F5 RID: 30453 RVA: 0x002F0E71 File Offset: 0x002EFE71
		public virtual ValidationResult Validate(object value, CultureInfo cultureInfo, BindingGroup owner)
		{
			return this.Validate(owner, cultureInfo);
		}

		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x060076F6 RID: 30454 RVA: 0x002F0E7B File Offset: 0x002EFE7B
		// (set) Token: 0x060076F7 RID: 30455 RVA: 0x002F0E83 File Offset: 0x002EFE83
		public ValidationStep ValidationStep
		{
			get
			{
				return this._validationStep;
			}
			set
			{
				this._validationStep = value;
			}
		}

		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x060076F8 RID: 30456 RVA: 0x002F0E8C File Offset: 0x002EFE8C
		// (set) Token: 0x060076F9 RID: 30457 RVA: 0x002F0E94 File Offset: 0x002EFE94
		public bool ValidatesOnTargetUpdated
		{
			get
			{
				return this._validatesOnTargetUpdated;
			}
			set
			{
				this._validatesOnTargetUpdated = value;
			}
		}

		// Token: 0x040038B7 RID: 14519
		private ValidationStep _validationStep;

		// Token: 0x040038B8 RID: 14520
		private bool _validatesOnTargetUpdated;
	}
}
