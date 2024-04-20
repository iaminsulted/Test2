using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x0200034F RID: 847
	[DictionaryKeyProperty("DataTemplateKey")]
	public class DataTemplate : FrameworkTemplate
	{
		// Token: 0x0600202A RID: 8234 RVA: 0x00174A65 File Offset: 0x00173A65
		public DataTemplate()
		{
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00174A70 File Offset: 0x00173A70
		public DataTemplate(object dataType)
		{
			Exception ex = TemplateKey.ValidateDataType(dataType, "dataType");
			if (ex != null)
			{
				throw ex;
			}
			this._dataType = dataType;
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x00174A9B File Offset: 0x00173A9B
		// (set) Token: 0x0600202D RID: 8237 RVA: 0x00174AA4 File Offset: 0x00173AA4
		[Ambient]
		[DefaultValue(null)]
		public object DataType
		{
			get
			{
				return this._dataType;
			}
			set
			{
				Exception ex = TemplateKey.ValidateDataType(value, "value");
				if (ex != null)
				{
					throw ex;
				}
				base.CheckSealed();
				this._dataType = value;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x00174ACF File Offset: 0x00173ACF
		[DependsOn("VisualTree")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DependsOn("Template")]
		public TriggerCollection Triggers
		{
			get
			{
				if (this._triggers == null)
				{
					this._triggers = new TriggerCollection();
					if (base.IsSealed)
					{
						this._triggers.Seal();
					}
				}
				return this._triggers;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x00174AFD File Offset: 0x00173AFD
		public object DataTemplateKey
		{
			get
			{
				if (this.DataType == null)
				{
					return null;
				}
				return new DataTemplateKey(this.DataType);
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x00174B14 File Offset: 0x00173B14
		internal override Type TargetTypeInternal
		{
			get
			{
				return DataTemplate.DefaultTargetType;
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00174B1B File Offset: 0x00173B1B
		internal override void SetTargetTypeInternal(Type targetType)
		{
			throw new InvalidOperationException(SR.Get("TemplateNotTargetType"));
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x00174B2C File Offset: 0x00173B2C
		internal override object DataTypeInternal
		{
			get
			{
				return this.DataType;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x00174B34 File Offset: 0x00173B34
		internal override TriggerCollection TriggersInternal
		{
			get
			{
				return this.Triggers;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002034 RID: 8244 RVA: 0x00174B3C File Offset: 0x00173B3C
		internal static Type DefaultTargetType
		{
			get
			{
				return typeof(ContentPresenter);
			}
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00174B48 File Offset: 0x00173B48
		protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (!(templatedParent is ContentPresenter))
			{
				throw new ArgumentException(SR.Get("TemplateTargetTypeMismatch", new object[]
				{
					"ContentPresenter",
					templatedParent.GetType().Name
				}));
			}
		}

		// Token: 0x04000FBE RID: 4030
		private object _dataType;

		// Token: 0x04000FBF RID: 4031
		private TriggerCollection _triggers;
	}
}
