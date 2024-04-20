using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace System.Windows.Controls
{
	// Token: 0x0200080D RID: 2061
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	[DictionaryKeyProperty("TargetType")]
	public class ControlTemplate : FrameworkTemplate
	{
		// Token: 0x060078B9 RID: 30905 RVA: 0x00174A65 File Offset: 0x00173A65
		public ControlTemplate()
		{
		}

		// Token: 0x060078BA RID: 30906 RVA: 0x003015AE File Offset: 0x003005AE
		public ControlTemplate(Type targetType)
		{
			this.ValidateTargetType(targetType, "targetType");
			this._targetType = targetType;
		}

		// Token: 0x060078BB RID: 30907 RVA: 0x003015CC File Offset: 0x003005CC
		protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (this._targetType != null && !this._targetType.IsInstanceOfType(templatedParent))
			{
				throw new ArgumentException(SR.Get("TemplateTargetTypeMismatch", new object[]
				{
					this._targetType.Name,
					templatedParent.GetType().Name
				}));
			}
			if (templatedParent.TemplateInternal != this)
			{
				throw new ArgumentException(SR.Get("MustNotTemplateUnassociatedControl"));
			}
		}

		// Token: 0x17001BF1 RID: 7153
		// (get) Token: 0x060078BC RID: 30908 RVA: 0x0030164E File Offset: 0x0030064E
		// (set) Token: 0x060078BD RID: 30909 RVA: 0x00301656 File Offset: 0x00300656
		[DefaultValue(null)]
		[Ambient]
		public Type TargetType
		{
			get
			{
				return this._targetType;
			}
			set
			{
				this.ValidateTargetType(value, "value");
				base.CheckSealed();
				this._targetType = value;
			}
		}

		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x060078BE RID: 30910 RVA: 0x00301671 File Offset: 0x00300671
		[DependsOn("Template")]
		[DependsOn("VisualTree")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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

		// Token: 0x060078BF RID: 30911 RVA: 0x003016A0 File Offset: 0x003006A0
		private void ValidateTargetType(Type targetType, string argName)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException(argName);
			}
			if (!typeof(Control).IsAssignableFrom(targetType) && !typeof(Page).IsAssignableFrom(targetType) && !typeof(PageFunctionBase).IsAssignableFrom(targetType))
			{
				throw new ArgumentException(SR.Get("InvalidControlTemplateTargetType", new object[]
				{
					targetType.Name
				}));
			}
		}

		// Token: 0x17001BF3 RID: 7155
		// (get) Token: 0x060078C0 RID: 30912 RVA: 0x00301712 File Offset: 0x00300712
		internal override Type TargetTypeInternal
		{
			get
			{
				if (this.TargetType != null)
				{
					return this.TargetType;
				}
				return ControlTemplate.DefaultTargetType;
			}
		}

		// Token: 0x060078C1 RID: 30913 RVA: 0x0030172E File Offset: 0x0030072E
		internal override void SetTargetTypeInternal(Type targetType)
		{
			this.TargetType = targetType;
		}

		// Token: 0x17001BF4 RID: 7156
		// (get) Token: 0x060078C2 RID: 30914 RVA: 0x00301737 File Offset: 0x00300737
		internal override TriggerCollection TriggersInternal
		{
			get
			{
				return this.Triggers;
			}
		}

		// Token: 0x04003977 RID: 14711
		private Type _targetType;

		// Token: 0x04003978 RID: 14712
		private TriggerCollection _triggers;

		// Token: 0x04003979 RID: 14713
		internal static readonly Type DefaultTargetType = typeof(Control);
	}
}
