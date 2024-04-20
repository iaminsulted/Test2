using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200042D RID: 1069
	public abstract class ControllableStoryboardAction : TriggerAction
	{
		// Token: 0x060033FA RID: 13306 RVA: 0x001D9CB2 File Offset: 0x001D8CB2
		internal ControllableStoryboardAction()
		{
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060033FB RID: 13307 RVA: 0x001D9EA5 File Offset: 0x001D8EA5
		// (set) Token: 0x060033FC RID: 13308 RVA: 0x001D9EAD File Offset: 0x001D8EAD
		[DefaultValue(null)]
		public string BeginStoryboardName
		{
			get
			{
				return this._beginStoryboardName;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"ControllableStoryboardAction"
					}));
				}
				this._beginStoryboardName = value;
			}
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x001D9EDC File Offset: 0x001D8EDC
		internal sealed override void Invoke(FrameworkElement fe, FrameworkContentElement fce, Style targetStyle, FrameworkTemplate frameworkTemplate, long layer)
		{
			INameScope nameScope;
			if (targetStyle != null)
			{
				nameScope = targetStyle;
			}
			else
			{
				nameScope = frameworkTemplate;
			}
			this.Invoke(fe, fce, this.GetStoryboard(fe, fce, nameScope));
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x001D9F06 File Offset: 0x001D8F06
		internal sealed override void Invoke(FrameworkElement fe)
		{
			this.Invoke(fe, null, this.GetStoryboard(fe, null, null));
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x001D9F1C File Offset: 0x001D8F1C
		private Storyboard GetStoryboard(FrameworkElement fe, FrameworkContentElement fce, INameScope nameScope)
		{
			if (this.BeginStoryboardName == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_BeginStoryboardNameRequired"));
			}
			Storyboard storyboard = Storyboard.ResolveBeginStoryboardName(this.BeginStoryboardName, nameScope, fe, fce).Storyboard;
			if (storyboard == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_BeginStoryboardNoStoryboard", new object[]
				{
					this.BeginStoryboardName
				}));
			}
			return storyboard;
		}

		// Token: 0x04001C49 RID: 7241
		private string _beginStoryboardName;
	}
}
