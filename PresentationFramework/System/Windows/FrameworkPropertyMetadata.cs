using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace System.Windows
{
	// Token: 0x02000370 RID: 880
	public class FrameworkPropertyMetadata : UIPropertyMetadata
	{
		// Token: 0x0600235B RID: 9051 RVA: 0x0017FA3B File Offset: 0x0017EA3B
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata()
		{
			this.Initialize();
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0017FA49 File Offset: 0x0017EA49
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue) : base(defaultValue)
		{
			this.Initialize();
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0017FA58 File Offset: 0x0017EA58
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(PropertyChangedCallback propertyChangedCallback) : base(propertyChangedCallback)
		{
			this.Initialize();
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0017FA67 File Offset: 0x0017EA67
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback) : base(propertyChangedCallback)
		{
			this.Initialize();
			base.CoerceValueCallback = coerceValueCallback;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0017FA7D File Offset: 0x0017EA7D
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, propertyChangedCallback)
		{
			this.Initialize();
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0017FA8D File Offset: 0x0017EA8D
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback) : base(defaultValue, propertyChangedCallback, coerceValueCallback)
		{
			this.Initialize();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0017FA9E File Offset: 0x0017EA9E
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags) : base(defaultValue)
		{
			this.TranslateFlags(flags);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0017FAAE File Offset: 0x0017EAAE
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, propertyChangedCallback)
		{
			this.TranslateFlags(flags);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0017FABF File Offset: 0x0017EABF
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback) : base(defaultValue, propertyChangedCallback, coerceValueCallback)
		{
			this.TranslateFlags(flags);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0017FAD2 File Offset: 0x0017EAD2
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback, bool isAnimationProhibited) : base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
		{
			this.TranslateFlags(flags);
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0017FAE8 File Offset: 0x0017EAE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback, bool isAnimationProhibited, UpdateSourceTrigger defaultUpdateSourceTrigger) : base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
		{
			if (!BindingOperations.IsValidUpdateSourceTrigger(defaultUpdateSourceTrigger))
			{
				throw new InvalidEnumArgumentException("defaultUpdateSourceTrigger", (int)defaultUpdateSourceTrigger, typeof(UpdateSourceTrigger));
			}
			if (defaultUpdateSourceTrigger == UpdateSourceTrigger.Default)
			{
				throw new ArgumentException(SR.Get("NoDefaultUpdateSourceTrigger"), "defaultUpdateSourceTrigger");
			}
			this.TranslateFlags(flags);
			this.DefaultUpdateSourceTrigger = defaultUpdateSourceTrigger;
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0017FB49 File Offset: 0x0017EB49
		private void Initialize()
		{
			this._flags = ((this._flags & (PropertyMetadata.MetadataFlags)1073741823U) | PropertyMetadata.MetadataFlags.FW_DefaultUpdateSourceTriggerEnumBit1);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0017FB63 File Offset: 0x0017EB63
		private static bool IsFlagSet(FrameworkPropertyMetadataOptions flag, FrameworkPropertyMetadataOptions flags)
		{
			return (flags & flag) > FrameworkPropertyMetadataOptions.None;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0017FB6C File Offset: 0x0017EB6C
		private void TranslateFlags(FrameworkPropertyMetadataOptions flags)
		{
			this.Initialize();
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsMeasure, flags))
			{
				this.AffectsMeasure = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsArrange, flags))
			{
				this.AffectsArrange = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsParentMeasure, flags))
			{
				this.AffectsParentMeasure = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsParentArrange, flags))
			{
				this.AffectsParentArrange = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsRender, flags))
			{
				this.AffectsRender = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.Inherits, flags))
			{
				base.IsInherited = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior, flags))
			{
				this.OverridesInheritanceBehavior = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.NotDataBindable, flags))
			{
				this.IsNotDataBindable = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, flags))
			{
				this.BindsTwoWayByDefault = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.Journal, flags))
			{
				this.Journal = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, flags))
			{
				this.SubPropertiesDoNotAffectRender = true;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x0017FC42 File Offset: 0x0017EC42
		// (set) Token: 0x0600236A RID: 9066 RVA: 0x0017FC4C File Offset: 0x0017EC4C
		public bool AffectsMeasure
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID, value);
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x0017FC6F File Offset: 0x0017EC6F
		// (set) Token: 0x0600236C RID: 9068 RVA: 0x0017FC7C File Offset: 0x0017EC7C
		public bool AffectsArrange
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID, value);
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x0017FCA2 File Offset: 0x0017ECA2
		// (set) Token: 0x0600236E RID: 9070 RVA: 0x0017FCAF File Offset: 0x0017ECAF
		public bool AffectsParentMeasure
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID, value);
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x0017FCD5 File Offset: 0x0017ECD5
		// (set) Token: 0x06002370 RID: 9072 RVA: 0x0017FCE2 File Offset: 0x0017ECE2
		public bool AffectsParentArrange
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID, value);
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x0017FD08 File Offset: 0x0017ED08
		// (set) Token: 0x06002372 RID: 9074 RVA: 0x0017FD15 File Offset: 0x0017ED15
		public bool AffectsRender
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID, value);
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x0017FD3B File Offset: 0x0017ED3B
		// (set) Token: 0x06002374 RID: 9076 RVA: 0x0017FD43 File Offset: 0x0017ED43
		public bool Inherits
		{
			get
			{
				return base.IsInherited;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.IsInherited = value;
				this.SetModified(PropertyMetadata.MetadataFlags.FW_InheritsModifiedID);
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x0017FD6F File Offset: 0x0017ED6F
		// (set) Token: 0x06002376 RID: 9078 RVA: 0x0017FD7C File Offset: 0x0017ED7C
		public bool OverridesInheritanceBehavior
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorID, value);
				this.SetModified(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorModifiedID);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002377 RID: 9079 RVA: 0x0017FDAD File Offset: 0x0017EDAD
		// (set) Token: 0x06002378 RID: 9080 RVA: 0x0017FDBA File Offset: 0x0017EDBA
		public bool IsNotDataBindable
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID, value);
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x0017FDE0 File Offset: 0x0017EDE0
		// (set) Token: 0x0600237A RID: 9082 RVA: 0x0017FDED File Offset: 0x0017EDED
		public bool BindsTwoWayByDefault
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID, value);
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x0017FE13 File Offset: 0x0017EE13
		// (set) Token: 0x0600237C RID: 9084 RVA: 0x0017FE20 File Offset: 0x0017EE20
		public UpdateSourceTrigger DefaultUpdateSourceTrigger
		{
			get
			{
				return (UpdateSourceTrigger)(this._flags >> 30 & (PropertyMetadata.MetadataFlags)3U);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				if (!BindingOperations.IsValidUpdateSourceTrigger(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(UpdateSourceTrigger));
				}
				if (value == UpdateSourceTrigger.Default)
				{
					throw new ArgumentException(SR.Get("NoDefaultUpdateSourceTrigger"), "value");
				}
				this._flags = ((this._flags & (PropertyMetadata.MetadataFlags)1073741823U) | (PropertyMetadata.MetadataFlags)(value << 30));
				this.SetModified(PropertyMetadata.MetadataFlags.FW_DefaultUpdateSourceTriggerModifiedID);
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x0017FE9D File Offset: 0x0017EE9D
		// (set) Token: 0x0600237E RID: 9086 RVA: 0x0017FEAA File Offset: 0x0017EEAA
		public bool Journal
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledID, value);
				this.SetModified(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledModifiedID);
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x0600237F RID: 9087 RVA: 0x0017FEDB File Offset: 0x0017EEDB
		// (set) Token: 0x06002380 RID: 9088 RVA: 0x0017FEE8 File Offset: 0x0017EEE8
		public bool SubPropertiesDoNotAffectRender
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderID, value);
				this.SetModified(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderModifiedID);
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0017FF19 File Offset: 0x0017EF19
		// (set) Token: 0x06002382 RID: 9090 RVA: 0x0017FF26 File Offset: 0x0017EF26
		private bool ReadOnly
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_ReadOnlyID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_ReadOnlyID, value);
			}
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0017FF4C File Offset: 0x0017EF4C
		internal override PropertyMetadata CreateInstance()
		{
			return new FrameworkPropertyMetadata();
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0017FF54 File Offset: 0x0017EF54
		protected override void Merge(PropertyMetadata baseMetadata, DependencyProperty dp)
		{
			base.Merge(baseMetadata, dp);
			FrameworkPropertyMetadata frameworkPropertyMetadata = baseMetadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID) | frameworkPropertyMetadata.AffectsMeasure);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID) | frameworkPropertyMetadata.AffectsArrange);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID) | frameworkPropertyMetadata.AffectsParentMeasure);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID) | frameworkPropertyMetadata.AffectsParentArrange);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID) | frameworkPropertyMetadata.AffectsRender);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID) | frameworkPropertyMetadata.BindsTwoWayByDefault);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID) | frameworkPropertyMetadata.IsNotDataBindable);
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderModifiedID))
				{
					base.WriteFlag(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderID, frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_InheritsModifiedID))
				{
					base.IsInherited = frameworkPropertyMetadata.Inherits;
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorModifiedID))
				{
					base.WriteFlag(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorID, frameworkPropertyMetadata.OverridesInheritanceBehavior);
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledModifiedID))
				{
					base.WriteFlag(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledID, frameworkPropertyMetadata.Journal);
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_DefaultUpdateSourceTriggerModifiedID))
				{
					this._flags = ((this._flags & (PropertyMetadata.MetadataFlags)1073741823U) | (PropertyMetadata.MetadataFlags)(frameworkPropertyMetadata.DefaultUpdateSourceTrigger << 30));
				}
			}
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x001800D7 File Offset: 0x0017F0D7
		protected override void OnApply(DependencyProperty dp, Type targetType)
		{
			this.ReadOnly = dp.ReadOnly;
			base.OnApply(dp, targetType);
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x001800ED File Offset: 0x0017F0ED
		public bool IsDataBindingAllowed
		{
			get
			{
				return !base.ReadFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID) && !this.ReadOnly;
			}
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x00180107 File Offset: 0x0017F107
		internal void SetModified(PropertyMetadata.MetadataFlags id)
		{
			base.WriteFlag(id, true);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x00180111 File Offset: 0x0017F111
		internal bool IsModified(PropertyMetadata.MetadataFlags id)
		{
			return base.ReadFlag(id);
		}
	}
}
