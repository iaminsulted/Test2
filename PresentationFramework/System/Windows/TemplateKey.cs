using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x020003C8 RID: 968
	[TypeConverter(typeof(TemplateKeyConverter))]
	public abstract class TemplateKey : ResourceKey, ISupportInitialize
	{
		// Token: 0x060028C4 RID: 10436 RVA: 0x00197270 File Offset: 0x00196270
		protected TemplateKey(TemplateKey.TemplateType templateType)
		{
			this._dataType = null;
			this._templateType = templateType;
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x00197288 File Offset: 0x00196288
		protected TemplateKey(TemplateKey.TemplateType templateType, object dataType)
		{
			Exception ex = TemplateKey.ValidateDataType(dataType, "dataType");
			if (ex != null)
			{
				throw ex;
			}
			this._dataType = dataType;
			this._templateType = templateType;
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x001972BA File Offset: 0x001962BA
		void ISupportInitialize.BeginInit()
		{
			this._initializing = true;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x001972C3 File Offset: 0x001962C3
		void ISupportInitialize.EndInit()
		{
			if (this._dataType == null)
			{
				throw new InvalidOperationException(SR.Get("PropertyMustHaveValue", new object[]
				{
					"DataType",
					base.GetType().Name
				}));
			}
			this._initializing = false;
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060028C8 RID: 10440 RVA: 0x00197300 File Offset: 0x00196300
		// (set) Token: 0x060028C9 RID: 10441 RVA: 0x00197308 File Offset: 0x00196308
		public object DataType
		{
			get
			{
				return this._dataType;
			}
			set
			{
				if (!this._initializing)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsInitializeOnly", new object[]
					{
						"DataType",
						base.GetType().Name
					}));
				}
				if (this._dataType != null && value != this._dataType)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
					{
						"DataType",
						base.GetType().Name
					}));
				}
				Exception ex = TemplateKey.ValidateDataType(value, "value");
				if (ex != null)
				{
					throw ex;
				}
				this._dataType = value;
			}
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x001973A0 File Offset: 0x001963A0
		public override int GetHashCode()
		{
			int num = (int)this._templateType;
			if (this._dataType != null)
			{
				num += this._dataType.GetHashCode();
			}
			return num;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x001973CC File Offset: 0x001963CC
		public override bool Equals(object o)
		{
			TemplateKey templateKey = o as TemplateKey;
			return templateKey != null && this._templateType == templateKey._templateType && object.Equals(this._dataType, templateKey._dataType);
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x00197408 File Offset: 0x00196408
		public override string ToString()
		{
			object dataType = this.DataType;
			if (this.DataType == null)
			{
				return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}(null)", base.GetType().Name);
			}
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}({1})", base.GetType().Name, this.DataType);
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x00197460 File Offset: 0x00196460
		public override Assembly Assembly
		{
			get
			{
				Type type = this._dataType as Type;
				if (type != null)
				{
					return type.Assembly;
				}
				return null;
			}
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x0019748C File Offset: 0x0019648C
		internal static Exception ValidateDataType(object dataType, string argName)
		{
			Exception result = null;
			if (dataType == null)
			{
				result = new ArgumentNullException(argName);
			}
			else if (!(dataType is Type) && !(dataType is string))
			{
				result = new ArgumentException(SR.Get("MustBeTypeOrString", new object[]
				{
					dataType.GetType().Name
				}), argName);
			}
			else if (typeof(object).Equals(dataType))
			{
				result = new ArgumentException(SR.Get("DataTypeCannotBeObject"), argName);
			}
			return result;
		}

		// Token: 0x0400149F RID: 5279
		private object _dataType;

		// Token: 0x040014A0 RID: 5280
		private TemplateKey.TemplateType _templateType;

		// Token: 0x040014A1 RID: 5281
		private bool _initializing;

		// Token: 0x02000A95 RID: 2709
		protected enum TemplateType
		{
			// Token: 0x0400427F RID: 17023
			DataTemplate,
			// Token: 0x04004280 RID: 17024
			TableTemplate
		}
	}
}
