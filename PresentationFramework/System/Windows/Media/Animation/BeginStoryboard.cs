using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200042C RID: 1068
	[RuntimeNameProperty("Name")]
	[ContentProperty("Storyboard")]
	public sealed class BeginStoryboard : TriggerAction
	{
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x001D9CBA File Offset: 0x001D8CBA
		// (set) Token: 0x060033EF RID: 13295 RVA: 0x001D9CCC File Offset: 0x001D8CCC
		[DefaultValue(null)]
		public Storyboard Storyboard
		{
			get
			{
				return base.GetValue(BeginStoryboard.StoryboardProperty) as Storyboard;
			}
			set
			{
				this.ThrowIfSealed();
				base.SetValue(BeginStoryboard.StoryboardProperty, value);
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060033F0 RID: 13296 RVA: 0x001D9CE0 File Offset: 0x001D8CE0
		// (set) Token: 0x060033F1 RID: 13297 RVA: 0x001D9CE8 File Offset: 0x001D8CE8
		[DefaultValue(HandoffBehavior.SnapshotAndReplace)]
		public HandoffBehavior HandoffBehavior
		{
			get
			{
				return this._handoffBehavior;
			}
			set
			{
				this.ThrowIfSealed();
				if (HandoffBehaviorEnum.IsDefined(value))
				{
					this._handoffBehavior = value;
					return;
				}
				throw new ArgumentException(SR.Get("Storyboard_UnrecognizedHandoffBehavior"));
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060033F2 RID: 13298 RVA: 0x001D9D0F File Offset: 0x001D8D0F
		// (set) Token: 0x060033F3 RID: 13299 RVA: 0x001D9D17 File Offset: 0x001D8D17
		[DefaultValue(null)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this.ThrowIfSealed();
				if (value != null && !NameValidationHelper.IsValidIdentifierName(value))
				{
					throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
					{
						value,
						"Name"
					}));
				}
				this._name = value;
			}
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x001D9D53 File Offset: 0x001D8D53
		private void ThrowIfSealed()
		{
			if (base.IsSealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"BeginStoryboard"
				}));
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x001D9D7C File Offset: 0x001D8D7C
		internal override void Seal()
		{
			if (!base.IsSealed)
			{
				Storyboard storyboard = base.GetValue(BeginStoryboard.StoryboardProperty) as Storyboard;
				if (storyboard == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_StoryboardReferenceRequired"));
				}
				if (!storyboard.CanFreeze)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_UnableToFreeze"));
				}
				if (!storyboard.IsFrozen)
				{
					storyboard.Freeze();
				}
				this.Storyboard = storyboard;
			}
			base.Seal();
			base.DetachFromDispatcher();
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x001D9DF0 File Offset: 0x001D8DF0
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
			this.Begin((fe != null) ? fe : fce, nameScope, layer);
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x001D9E19 File Offset: 0x001D8E19
		internal sealed override void Invoke(FrameworkElement fe)
		{
			this.Begin(fe, null, Storyboard.Layers.ElementEventTrigger);
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x001D9E28 File Offset: 0x001D8E28
		private void Begin(DependencyObject targetObject, INameScope nameScope, long layer)
		{
			if (this.Storyboard == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_StoryboardReferenceRequired"));
			}
			if (this.Name != null)
			{
				this.Storyboard.BeginCommon(targetObject, nameScope, this._handoffBehavior, true, layer);
				return;
			}
			this.Storyboard.BeginCommon(targetObject, nameScope, this._handoffBehavior, false, layer);
		}

		// Token: 0x04001C46 RID: 7238
		public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(BeginStoryboard));

		// Token: 0x04001C47 RID: 7239
		private HandoffBehavior _handoffBehavior;

		// Token: 0x04001C48 RID: 7240
		private string _name;
	}
}
