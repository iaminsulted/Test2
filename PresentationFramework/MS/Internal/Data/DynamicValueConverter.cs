using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000225 RID: 549
	internal class DynamicValueConverter : IValueConverter
	{
		// Token: 0x06001487 RID: 5255 RVA: 0x00152456 File Offset: 0x00151456
		internal DynamicValueConverter(bool targetToSourceNeeded)
		{
			this._targetToSourceNeeded = targetToSourceNeeded;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00152465 File Offset: 0x00151465
		internal DynamicValueConverter(bool targetToSourceNeeded, Type sourceType, Type targetType)
		{
			this._targetToSourceNeeded = targetToSourceNeeded;
			this.EnsureConverter(sourceType, targetType);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0015247C File Offset: 0x0015147C
		internal object Convert(object value, Type targetType)
		{
			return this.Convert(value, targetType, null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0015248C File Offset: 0x0015148C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Type type = value.GetType();
				this.EnsureConverter(type, targetType);
				if (this._converter != null)
				{
					result = this._converter.Convert(value, targetType, parameter, culture);
				}
			}
			else if (!targetType.IsValueType)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x001524D8 File Offset: 0x001514D8
		public object ConvertBack(object value, Type sourceType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Type type = value.GetType();
				this.EnsureConverter(sourceType, type);
				if (this._converter != null)
				{
					result = this._converter.ConvertBack(value, sourceType, parameter, culture);
				}
			}
			else if (!sourceType.IsValueType)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00152524 File Offset: 0x00151524
		private void EnsureConverter(Type sourceType, Type targetType)
		{
			if (this._sourceType != sourceType || this._targetType != targetType)
			{
				if (sourceType != null && targetType != null)
				{
					if (this._engine == null)
					{
						this._engine = DataBindEngine.CurrentDataBindEngine;
					}
					Invariant.Assert(this._engine != null);
					this._converter = this._engine.GetDefaultValueConverter(sourceType, targetType, this._targetToSourceNeeded);
				}
				else
				{
					this._converter = null;
				}
				this._sourceType = sourceType;
				this._targetType = targetType;
			}
		}

		// Token: 0x04000BD5 RID: 3029
		private Type _sourceType;

		// Token: 0x04000BD6 RID: 3030
		private Type _targetType;

		// Token: 0x04000BD7 RID: 3031
		private IValueConverter _converter;

		// Token: 0x04000BD8 RID: 3032
		private bool _targetToSourceNeeded;

		// Token: 0x04000BD9 RID: 3033
		private DataBindEngine _engine;
	}
}
