using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200073D RID: 1853
	public sealed class DataErrorValidationRule : ValidationRule
	{
		// Token: 0x06006248 RID: 25160 RVA: 0x0029EF4A File Offset: 0x0029DF4A
		public DataErrorValidationRule() : base(ValidationStep.UpdatedValue, true)
		{
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x0029EF54 File Offset: 0x0029DF54
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			BindingGroup bindingGroup;
			if ((bindingGroup = (value as BindingGroup)) != null)
			{
				IList items = bindingGroup.Items;
				for (int i = items.Count - 1; i >= 0; i--)
				{
					IDataErrorInfo dataErrorInfo = items[i] as IDataErrorInfo;
					if (dataErrorInfo != null)
					{
						string error = dataErrorInfo.Error;
						if (!string.IsNullOrEmpty(error))
						{
							return new ValidationResult(false, error);
						}
					}
				}
			}
			else
			{
				BindingExpression bindingExpression;
				if ((bindingExpression = (value as BindingExpression)) == null)
				{
					throw new InvalidOperationException(SR.Get("ValidationRule_UnexpectedValue", new object[]
					{
						this,
						value
					}));
				}
				IDataErrorInfo dataErrorInfo2 = bindingExpression.SourceItem as IDataErrorInfo;
				string text = (dataErrorInfo2 != null) ? bindingExpression.SourcePropertyName : null;
				if (!string.IsNullOrEmpty(text))
				{
					string text2;
					try
					{
						text2 = dataErrorInfo2[text];
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex))
						{
							throw;
						}
						text2 = null;
						if (TraceData.IsEnabled)
						{
							TraceData.TraceAndNotify(TraceEventType.Error, TraceData.DataErrorInfoFailed(new object[]
							{
								text,
								dataErrorInfo2.GetType().FullName,
								ex.GetType().FullName,
								ex.Message
							}), bindingExpression, null);
						}
					}
					if (!string.IsNullOrEmpty(text2))
					{
						return new ValidationResult(false, text2);
					}
				}
			}
			return ValidationResult.ValidResult;
		}

		// Token: 0x040032B2 RID: 12978
		internal static readonly DataErrorValidationRule Instance = new DataErrorValidationRule();
	}
}
