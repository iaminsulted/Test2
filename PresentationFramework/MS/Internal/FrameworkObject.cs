using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MS.Internal
{
	// Token: 0x020000EE RID: 238
	internal struct FrameworkObject
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x000FF6BC File Offset: 0x000FE6BC
		internal FrameworkObject(DependencyObject d)
		{
			this._do = d;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				this._fe = (FrameworkElement)d;
				this._fce = null;
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				this._fe = null;
				this._fce = (FrameworkContentElement)d;
				return;
			}
			this._fe = null;
			this._fce = null;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000FF720 File Offset: 0x000FE720
		internal FrameworkObject(DependencyObject d, bool throwIfNeither)
		{
			this = new FrameworkObject(d);
			if (throwIfNeither && this._fe == null && this._fce == null)
			{
				object obj = (d != null) ? d.GetType() : "NULL";
				throw new InvalidOperationException(SR.Get("MustBeFrameworkDerived", new object[]
				{
					obj
				}));
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000FF772 File Offset: 0x000FE772
		internal FrameworkObject(FrameworkElement fe, FrameworkContentElement fce)
		{
			this._fe = fe;
			this._fce = fce;
			if (fe != null)
			{
				this._do = fe;
				return;
			}
			this._do = fce;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000FF6BC File Offset: 0x000FE6BC
		internal void Reset(DependencyObject d)
		{
			this._do = d;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				this._fe = (FrameworkElement)d;
				this._fce = null;
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				this._fe = null;
				this._fce = (FrameworkContentElement)d;
				return;
			}
			this._fe = null;
			this._fce = null;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x000FF794 File Offset: 0x000FE794
		internal FrameworkElement FE
		{
			get
			{
				return this._fe;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x000FF79C File Offset: 0x000FE79C
		internal FrameworkContentElement FCE
		{
			get
			{
				return this._fce;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000FF7A4 File Offset: 0x000FE7A4
		internal DependencyObject DO
		{
			get
			{
				return this._do;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000FF7AC File Offset: 0x000FE7AC
		internal bool IsFE
		{
			get
			{
				return this._fe != null;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000FF7B7 File Offset: 0x000FE7B7
		internal bool IsFCE
		{
			get
			{
				return this._fce != null;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x000FF7C2 File Offset: 0x000FE7C2
		internal bool IsValid
		{
			get
			{
				return this._fe != null || this._fce != null;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x000FF7D7 File Offset: 0x000FE7D7
		internal DependencyObject Parent
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.Parent;
				}
				if (this.IsFCE)
				{
					return this._fce.Parent;
				}
				return null;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x000FF802 File Offset: 0x000FE802
		internal int TemplateChildIndex
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.TemplateChildIndex;
				}
				if (this.IsFCE)
				{
					return this._fce.TemplateChildIndex;
				}
				return -1;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000FF82D File Offset: 0x000FE82D
		internal DependencyObject TemplatedParent
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.TemplatedParent;
				}
				if (this.IsFCE)
				{
					return this._fce.TemplatedParent;
				}
				return null;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x000FF858 File Offset: 0x000FE858
		internal Style ThemeStyle
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.ThemeStyle;
				}
				if (this.IsFCE)
				{
					return this._fce.ThemeStyle;
				}
				return null;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x000FF883 File Offset: 0x000FE883
		internal XmlLanguage Language
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.Language;
				}
				if (this.IsFCE)
				{
					return this._fce.Language;
				}
				return null;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000FF8AE File Offset: 0x000FE8AE
		internal FrameworkTemplate TemplateInternal
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.TemplateInternal;
				}
				return null;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x000FF8C8 File Offset: 0x000FE8C8
		internal FrameworkObject FrameworkParent
		{
			get
			{
				if (this.IsFE)
				{
					DependencyObject dependencyObject = this._fe.ContextVerifiedGetParent();
					if (dependencyObject != null)
					{
						Invariant.Assert(dependencyObject is FrameworkElement || dependencyObject is FrameworkContentElement);
						if (this._fe.IsParentAnFE)
						{
							return new FrameworkObject((FrameworkElement)dependencyObject, null);
						}
						return new FrameworkObject(null, (FrameworkContentElement)dependencyObject);
					}
					else
					{
						FrameworkObject containingFrameworkElement = FrameworkObject.GetContainingFrameworkElement(this._fe.InternalVisualParent);
						if (containingFrameworkElement.IsValid)
						{
							return containingFrameworkElement;
						}
						containingFrameworkElement.Reset(this._fe.GetUIParentCore());
						if (containingFrameworkElement.IsValid)
						{
							return containingFrameworkElement;
						}
						containingFrameworkElement.Reset(Helper.FindMentor(this._fe.InheritanceContext));
						return containingFrameworkElement;
					}
				}
				else
				{
					if (!this.IsFCE)
					{
						return FrameworkObject.GetContainingFrameworkElement(this._do);
					}
					DependencyObject parent = this._fce.Parent;
					if (parent != null)
					{
						Invariant.Assert(parent is FrameworkElement || parent is FrameworkContentElement);
						if (this._fce.IsParentAnFE)
						{
							return new FrameworkObject((FrameworkElement)parent, null);
						}
						return new FrameworkObject(null, (FrameworkContentElement)parent);
					}
					else
					{
						parent = ContentOperations.GetParent(this._fce);
						FrameworkObject containingFrameworkElement2 = FrameworkObject.GetContainingFrameworkElement(parent);
						if (containingFrameworkElement2.IsValid)
						{
							return containingFrameworkElement2;
						}
						containingFrameworkElement2.Reset(Helper.FindMentor(this._fce.InheritanceContext));
						return containingFrameworkElement2;
					}
				}
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000FFA20 File Offset: 0x000FEA20
		internal static FrameworkObject GetContainingFrameworkElement(DependencyObject current)
		{
			FrameworkObject result = new FrameworkObject(current);
			while (!result.IsValid && result.DO != null)
			{
				Visual reference;
				ContentElement reference2;
				Visual3D reference3;
				if ((reference = (result.DO as Visual)) != null)
				{
					result.Reset(VisualTreeHelper.GetParent(reference));
				}
				else if ((reference2 = (result.DO as ContentElement)) != null)
				{
					result.Reset(ContentOperations.GetParent(reference2));
				}
				else if ((reference3 = (result.DO as Visual3D)) != null)
				{
					result.Reset(VisualTreeHelper.GetParent(reference3));
				}
				else
				{
					result.Reset(null);
				}
			}
			return result;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x000FFAAF File Offset: 0x000FEAAF
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x000FFADA File Offset: 0x000FEADA
		internal Style Style
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.Style;
				}
				if (this.IsFCE)
				{
					return this._fce.Style;
				}
				return null;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.Style = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Style = value;
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x000FFB05 File Offset: 0x000FEB05
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x000FFB30 File Offset: 0x000FEB30
		internal bool IsStyleSetFromGenerator
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.IsStyleSetFromGenerator;
				}
				return this.IsFCE && this._fce.IsStyleSetFromGenerator;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.IsStyleSetFromGenerator = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.IsStyleSetFromGenerator = value;
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x000FFB5C File Offset: 0x000FEB5C
		internal DependencyObject EffectiveParent
		{
			get
			{
				DependencyObject dependencyObject;
				Visual reference;
				ContentElement reference2;
				Visual3D reference3;
				if (this.IsFE)
				{
					dependencyObject = VisualTreeHelper.GetParent(this._fe);
				}
				else if (this.IsFCE)
				{
					dependencyObject = this._fce.Parent;
				}
				else if ((reference = (this._do as Visual)) != null)
				{
					dependencyObject = VisualTreeHelper.GetParent(reference);
				}
				else if ((reference2 = (this._do as ContentElement)) != null)
				{
					dependencyObject = ContentOperations.GetParent(reference2);
				}
				else if ((reference3 = (this._do as Visual3D)) != null)
				{
					dependencyObject = VisualTreeHelper.GetParent(reference3);
				}
				else
				{
					dependencyObject = null;
				}
				if (dependencyObject == null && this._do != null)
				{
					dependencyObject = this._do.InheritanceContext;
				}
				return dependencyObject;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x000FFBF7 File Offset: 0x000FEBF7
		internal FrameworkObject PreferVisualParent
		{
			get
			{
				return this.GetPreferVisualParent(false);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x000FFC00 File Offset: 0x000FEC00
		internal bool IsLoaded
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.IsLoaded;
				}
				if (this.IsFCE)
				{
					return this._fce.IsLoaded;
				}
				return BroadcastEventHelper.IsParentLoaded(this._do);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x000FFC35 File Offset: 0x000FEC35
		internal bool IsInitialized
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.IsInitialized;
				}
				return !this.IsFCE || this._fce.IsInitialized;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000FFC60 File Offset: 0x000FEC60
		internal bool ThisHasLoadedChangeEventHandler
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.ThisHasLoadedChangeEventHandler;
				}
				return this.IsFCE && this._fce.ThisHasLoadedChangeEventHandler;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x000FFC8B File Offset: 0x000FEC8B
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x000FFCB6 File Offset: 0x000FECB6
		internal bool SubtreeHasLoadedChangeHandler
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.SubtreeHasLoadedChangeHandler;
				}
				return this.IsFCE && this._fce.SubtreeHasLoadedChangeHandler;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.SubtreeHasLoadedChangeHandler = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.SubtreeHasLoadedChangeHandler = value;
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000FFCE1 File Offset: 0x000FECE1
		internal InheritanceBehavior InheritanceBehavior
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.InheritanceBehavior;
				}
				if (this.IsFCE)
				{
					return this._fce.InheritanceBehavior;
				}
				return InheritanceBehavior.Default;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000FFD0C File Offset: 0x000FED0C
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x000FFD37 File Offset: 0x000FED37
		internal bool StoresParentTemplateValues
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.StoresParentTemplateValues;
				}
				return this.IsFCE && this._fce.StoresParentTemplateValues;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.StoresParentTemplateValues = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.StoresParentTemplateValues = value;
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x000FFD62 File Offset: 0x000FED62
		internal bool HasResourceReference
		{
			set
			{
				if (this.IsFE)
				{
					this._fe.HasResourceReference = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.HasResourceReference = value;
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x000FFD8D File Offset: 0x000FED8D
		internal bool HasTemplateChanged
		{
			set
			{
				if (this.IsFE)
				{
					this._fe.HasTemplateChanged = value;
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000FFDA3 File Offset: 0x000FEDA3
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x000FFDCE File Offset: 0x000FEDCE
		internal bool ShouldLookupImplicitStyles
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.ShouldLookupImplicitStyles;
				}
				return this.IsFCE && this._fce.ShouldLookupImplicitStyles;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.ShouldLookupImplicitStyles = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.ShouldLookupImplicitStyles = value;
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000FFDFC File Offset: 0x000FEDFC
		internal static bool IsEffectiveAncestor(DependencyObject d1, DependencyObject d2)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d2);
			while (frameworkObject.DO != null)
			{
				if (frameworkObject.DO == d1)
				{
					return true;
				}
				frameworkObject.Reset(frameworkObject.EffectiveParent);
			}
			return false;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000FFE37 File Offset: 0x000FEE37
		internal void ChangeLogicalParent(DependencyObject newParent)
		{
			if (this.IsFE)
			{
				this._fe.ChangeLogicalParent(newParent);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.ChangeLogicalParent(newParent);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000FFE62 File Offset: 0x000FEE62
		internal void BeginInit()
		{
			if (this.IsFE)
			{
				this._fe.BeginInit();
				return;
			}
			if (this.IsFCE)
			{
				this._fce.BeginInit();
				return;
			}
			this.UnexpectedCall();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000FFE92 File Offset: 0x000FEE92
		internal void EndInit()
		{
			if (this.IsFE)
			{
				this._fe.EndInit();
				return;
			}
			if (this.IsFCE)
			{
				this._fce.EndInit();
				return;
			}
			this.UnexpectedCall();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000FFEC2 File Offset: 0x000FEEC2
		internal object FindName(string name, out DependencyObject scopeOwner)
		{
			if (this.IsFE)
			{
				return this._fe.FindName(name, out scopeOwner);
			}
			if (this.IsFCE)
			{
				return this._fce.FindName(name, out scopeOwner);
			}
			scopeOwner = null;
			return null;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000FFEF4 File Offset: 0x000FEEF4
		internal FrameworkObject GetPreferVisualParent(bool force)
		{
			if (!force && this.InheritanceBehavior != InheritanceBehavior.Default)
			{
				return new FrameworkObject(null);
			}
			FrameworkObject rawPreferVisualParent = this.GetRawPreferVisualParent();
			switch (rawPreferVisualParent.InheritanceBehavior)
			{
			case InheritanceBehavior.SkipToAppNow:
			case InheritanceBehavior.SkipToThemeNow:
			case InheritanceBehavior.SkipAllNow:
				rawPreferVisualParent.Reset(null);
				break;
			}
			return rawPreferVisualParent;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000FFF4C File Offset: 0x000FEF4C
		private FrameworkObject GetRawPreferVisualParent()
		{
			if (this._do == null)
			{
				return new FrameworkObject(null);
			}
			DependencyObject dependencyObject;
			if (this.IsFE)
			{
				dependencyObject = VisualTreeHelper.GetParent(this._fe);
			}
			else if (this.IsFCE)
			{
				dependencyObject = null;
			}
			else if (this._do != null)
			{
				Visual visual = this._do as Visual;
				dependencyObject = ((visual != null) ? VisualTreeHelper.GetParent(visual) : null);
			}
			else
			{
				dependencyObject = null;
			}
			if (dependencyObject != null)
			{
				return new FrameworkObject(dependencyObject);
			}
			DependencyObject dependencyObject2;
			if (this.IsFE)
			{
				dependencyObject2 = this._fe.Parent;
			}
			else if (this.IsFCE)
			{
				dependencyObject2 = this._fce.Parent;
			}
			else if (this._do != null)
			{
				ContentElement contentElement = this._do as ContentElement;
				dependencyObject2 = ((contentElement != null) ? ContentOperations.GetParent(contentElement) : null);
			}
			else
			{
				dependencyObject2 = null;
			}
			if (dependencyObject2 != null)
			{
				return new FrameworkObject(dependencyObject2);
			}
			UIElement uielement;
			DependencyObject dependencyObject3;
			ContentElement contentElement2;
			if ((uielement = (this._do as UIElement)) != null)
			{
				dependencyObject3 = uielement.GetUIParentCore();
			}
			else if ((contentElement2 = (this._do as ContentElement)) != null)
			{
				dependencyObject3 = contentElement2.GetUIParentCore();
			}
			else
			{
				dependencyObject3 = null;
			}
			if (dependencyObject3 != null)
			{
				return new FrameworkObject(dependencyObject3);
			}
			return new FrameworkObject(this._do.InheritanceContext);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0010006B File Offset: 0x000FF06B
		internal void RaiseEvent(RoutedEventArgs args)
		{
			if (this.IsFE)
			{
				this._fe.RaiseEvent(args);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.RaiseEvent(args);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00100096 File Offset: 0x000FF096
		internal void OnLoaded(RoutedEventArgs args)
		{
			if (this.IsFE)
			{
				this._fe.OnLoaded(args);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.OnLoaded(args);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x001000C1 File Offset: 0x000FF0C1
		internal void OnUnloaded(RoutedEventArgs args)
		{
			if (this.IsFE)
			{
				this._fe.OnUnloaded(args);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.OnUnloaded(args);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x001000EC File Offset: 0x000FF0EC
		internal void ChangeSubtreeHasLoadedChangedHandler(DependencyObject mentor)
		{
			if (this.IsFE)
			{
				this._fe.ChangeSubtreeHasLoadedChangedHandler(mentor);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.ChangeSubtreeHasLoadedChangedHandler(mentor);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00100117 File Offset: 0x000FF117
		internal void OnInheritedPropertyChanged(ref InheritablePropertyChangeInfo info)
		{
			if (this.IsFE)
			{
				this._fe.RaiseInheritedPropertyChangedEvent(ref info);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.RaiseInheritedPropertyChangedEvent(ref info);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00100144 File Offset: 0x000FF144
		internal void SetShouldLookupImplicitStyles()
		{
			if (!this.ShouldLookupImplicitStyles)
			{
				FrameworkObject frameworkParent = this.FrameworkParent;
				if (frameworkParent.IsValid && frameworkParent.ShouldLookupImplicitStyles)
				{
					this.ShouldLookupImplicitStyles = true;
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000477 RID: 1143 RVA: 0x00100179 File Offset: 0x000FF179
		// (remove) Token: 0x06000478 RID: 1144 RVA: 0x001001AB File Offset: 0x000FF1AB
		internal event RoutedEventHandler Loaded
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.Loaded += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Loaded += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.Loaded -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Loaded -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000479 RID: 1145 RVA: 0x001001DD File Offset: 0x000FF1DD
		// (remove) Token: 0x0600047A RID: 1146 RVA: 0x0010020F File Offset: 0x000FF20F
		internal event RoutedEventHandler Unloaded
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.Unloaded += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Unloaded += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.Unloaded -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Unloaded -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600047B RID: 1147 RVA: 0x00100241 File Offset: 0x000FF241
		// (remove) Token: 0x0600047C RID: 1148 RVA: 0x00100273 File Offset: 0x000FF273
		internal event InheritedPropertyChangedEventHandler InheritedPropertyChanged
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.InheritedPropertyChanged += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.InheritedPropertyChanged += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.InheritedPropertyChanged -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.InheritedPropertyChanged -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600047D RID: 1149 RVA: 0x001002A5 File Offset: 0x000FF2A5
		// (remove) Token: 0x0600047E RID: 1150 RVA: 0x001002D7 File Offset: 0x000FF2D7
		internal event EventHandler ResourcesChanged
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.ResourcesChanged += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.ResourcesChanged += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.ResourcesChanged -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.ResourcesChanged -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00100309 File Offset: 0x000FF309
		private void UnexpectedCall()
		{
			Invariant.Assert(false, "Call to FrameworkObject expects either FE or FCE");
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00100316 File Offset: 0x000FF316
		public override string ToString()
		{
			if (this.IsFE)
			{
				return this._fe.ToString();
			}
			if (this.IsFCE)
			{
				return this._fce.ToString();
			}
			return "Null";
		}

		// Token: 0x0400062D RID: 1581
		private FrameworkElement _fe;

		// Token: 0x0400062E RID: 1582
		private FrameworkContentElement _fce;

		// Token: 0x0400062F RID: 1583
		private DependencyObject _do;
	}
}
