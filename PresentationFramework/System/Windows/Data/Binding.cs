using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000448 RID: 1096
	public class Binding : BindingBase
	{
		// Token: 0x06003549 RID: 13641 RVA: 0x001DD849 File Offset: 0x001DC849
		public static void AddSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.AddHandler(element, Binding.SourceUpdatedEvent, handler);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x001DD857 File Offset: 0x001DC857
		public static void RemoveSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.RemoveHandler(element, Binding.SourceUpdatedEvent, handler);
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x001DD865 File Offset: 0x001DC865
		public static void AddTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.AddHandler(element, Binding.TargetUpdatedEvent, handler);
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x001DD873 File Offset: 0x001DC873
		public static void RemoveTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.RemoveHandler(element, Binding.TargetUpdatedEvent, handler);
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x001DD881 File Offset: 0x001DC881
		public static XmlNamespaceManager GetXmlNamespaceManager(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return (XmlNamespaceManager)target.GetValue(Binding.XmlNamespaceManagerProperty);
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x001DD8A1 File Offset: 0x001DC8A1
		public static void SetXmlNamespaceManager(DependencyObject target, XmlNamespaceManager value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			target.SetValue(Binding.XmlNamespaceManagerProperty, value);
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x001DD8BD File Offset: 0x001DC8BD
		private static bool IsValidXmlNamespaceManager(object value)
		{
			return value == null || SystemXmlHelper.IsXmlNamespaceManager(value);
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x001DD8CA File Offset: 0x001DC8CA
		public Binding()
		{
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x001DD8DD File Offset: 0x001DC8DD
		public Binding(string path)
		{
			if (path != null)
			{
				if (Dispatcher.CurrentDispatcher == null)
				{
					throw new InvalidOperationException();
				}
				this.Path = new PropertyPath(path, null);
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003552 RID: 13650 RVA: 0x001DD90D File Offset: 0x001DC90D
		public Collection<ValidationRule> ValidationRules
		{
			get
			{
				if (!base.HasValue(BindingBase.Feature.ValidationRules))
				{
					base.SetValue(BindingBase.Feature.ValidationRules, new ValidationRuleCollection());
				}
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x001DD934 File Offset: 0x001DC934
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeValidationRules()
		{
			return base.HasValue(BindingBase.Feature.ValidationRules) && this.ValidationRules.Count > 0;
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003554 RID: 13652 RVA: 0x001DD950 File Offset: 0x001DC950
		// (set) Token: 0x06003555 RID: 13653 RVA: 0x001DD95D File Offset: 0x001DC95D
		[DefaultValue(false)]
		public bool ValidatesOnExceptions
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnExceptions, value);
				}
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06003556 RID: 13654 RVA: 0x001DD97F File Offset: 0x001DC97F
		// (set) Token: 0x06003557 RID: 13655 RVA: 0x001DD98C File Offset: 0x001DC98C
		[DefaultValue(false)]
		public bool ValidatesOnDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnDataErrors, value);
				}
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003558 RID: 13656 RVA: 0x001DD9AE File Offset: 0x001DC9AE
		// (set) Token: 0x06003559 RID: 13657 RVA: 0x001DD9BB File Offset: 0x001DC9BB
		[DefaultValue(true)]
		public bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors, value);
				}
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x0600355A RID: 13658 RVA: 0x001DD9DD File Offset: 0x001DC9DD
		// (set) Token: 0x0600355B RID: 13659 RVA: 0x001DD9E8 File Offset: 0x001DC9E8
		public PropertyPath Path
		{
			get
			{
				return this._ppath;
			}
			set
			{
				base.CheckSealed();
				this._ppath = value;
				this._attachedPropertiesInPath = -1;
				base.ClearFlag(BindingBase.BindingFlags.PathGeneratedInternally);
				if (this._ppath == null || !this._ppath.StartsWithStaticProperty)
				{
					return;
				}
				if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.StaticSource || FrameworkCompatibilityPreferences.TargetsDesktop_V4_0)
				{
					this.SourceReference = Binding.StaticSourceRef;
					return;
				}
				throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
				{
					Binding.SourceProperties.StaticSource,
					this._sourceInUse
				}));
			}
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x001DDA7A File Offset: 0x001DCA7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializePath()
		{
			return this._ppath != null && !base.TestFlag(BindingBase.BindingFlags.PathGeneratedInternally);
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600355D RID: 13661 RVA: 0x001DDA94 File Offset: 0x001DCA94
		// (set) Token: 0x0600355E RID: 13662 RVA: 0x001DDAA3 File Offset: 0x001DCAA3
		[DefaultValue(null)]
		public string XPath
		{
			get
			{
				return (string)base.GetValue(BindingBase.Feature.XPath, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.XPath, value, null);
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600355F RID: 13663 RVA: 0x001DDAB4 File Offset: 0x001DCAB4
		// (set) Token: 0x06003560 RID: 13664 RVA: 0x001DDAFC File Offset: 0x001DCAFC
		[DefaultValue(BindingMode.Default)]
		public BindingMode Mode
		{
			get
			{
				switch (base.GetFlagsWithinMask(BindingBase.BindingFlags.PropagationMask))
				{
				case BindingBase.BindingFlags.OneTime:
					return BindingMode.OneTime;
				case BindingBase.BindingFlags.OneWay:
					return BindingMode.OneWay;
				case BindingBase.BindingFlags.OneWayToSource:
					return BindingMode.OneWayToSource;
				case BindingBase.BindingFlags.TwoWay:
					return BindingMode.TwoWay;
				case BindingBase.BindingFlags.PropDefault:
					return BindingMode.Default;
				default:
					Invariant.Assert(false, "Unexpected BindingMode value");
					return BindingMode.TwoWay;
				}
			}
			set
			{
				base.CheckSealed();
				BindingBase.BindingFlags bindingFlags = BindingBase.FlagsFrom(value);
				if (bindingFlags == BindingBase.BindingFlags.IllegalInput)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BindingMode));
				}
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.PropagationMask, bindingFlags);
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06003561 RID: 13665 RVA: 0x001DDB3C File Offset: 0x001DCB3C
		// (set) Token: 0x06003562 RID: 13666 RVA: 0x001DDB90 File Offset: 0x001DCB90
		[DefaultValue(UpdateSourceTrigger.Default)]
		public UpdateSourceTrigger UpdateSourceTrigger
		{
			get
			{
				BindingBase.BindingFlags flagsWithinMask = base.GetFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault);
				if (flagsWithinMask <= BindingBase.BindingFlags.UpdateOnLostFocus)
				{
					if (flagsWithinMask == BindingBase.BindingFlags.OneTime)
					{
						return UpdateSourceTrigger.PropertyChanged;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateOnLostFocus)
					{
						return UpdateSourceTrigger.LostFocus;
					}
				}
				else
				{
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateExplicitly)
					{
						return UpdateSourceTrigger.Explicit;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateDefault)
					{
						return UpdateSourceTrigger.Default;
					}
				}
				Invariant.Assert(false, "Unexpected UpdateSourceTrigger value");
				return UpdateSourceTrigger.Default;
			}
			set
			{
				base.CheckSealed();
				BindingBase.BindingFlags bindingFlags = BindingBase.FlagsFrom(value);
				if (bindingFlags == BindingBase.BindingFlags.IllegalInput)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(UpdateSourceTrigger));
				}
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault, bindingFlags);
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003563 RID: 13667 RVA: 0x001DDBD4 File Offset: 0x001DCBD4
		// (set) Token: 0x06003564 RID: 13668 RVA: 0x001DDBE1 File Offset: 0x001DCBE1
		[DefaultValue(false)]
		public bool NotifyOnSourceUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated, value);
				}
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003565 RID: 13669 RVA: 0x001DDC03 File Offset: 0x001DCC03
		// (set) Token: 0x06003566 RID: 13670 RVA: 0x001DDC0C File Offset: 0x001DCC0C
		[DefaultValue(false)]
		public bool NotifyOnTargetUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated, value);
				}
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x001DDC26 File Offset: 0x001DCC26
		// (set) Token: 0x06003568 RID: 13672 RVA: 0x001DDC33 File Offset: 0x001DCC33
		[DefaultValue(false)]
		public bool NotifyOnValidationError
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnValidationError, value);
				}
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003569 RID: 13673 RVA: 0x001DDC55 File Offset: 0x001DCC55
		// (set) Token: 0x0600356A RID: 13674 RVA: 0x001DDC65 File Offset: 0x001DCC65
		[DefaultValue(null)]
		public IValueConverter Converter
		{
			get
			{
				return (IValueConverter)base.GetValue(BindingBase.Feature.Converter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Converter, value, null);
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x001DDC77 File Offset: 0x001DCC77
		// (set) Token: 0x0600356C RID: 13676 RVA: 0x001DDC82 File Offset: 0x001DCC82
		[DefaultValue(null)]
		public object ConverterParameter
		{
			get
			{
				return base.GetValue(BindingBase.Feature.ConverterParameter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.ConverterParameter, value, null);
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x0600356D RID: 13677 RVA: 0x001DDC94 File Offset: 0x001DCC94
		// (set) Token: 0x0600356E RID: 13678 RVA: 0x001DDCA3 File Offset: 0x001DCCA3
		[DefaultValue(null)]
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo ConverterCulture
		{
			get
			{
				return (CultureInfo)base.GetValue(BindingBase.Feature.Culture, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Culture, value, null);
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600356F RID: 13679 RVA: 0x001DDCB4 File Offset: 0x001DCCB4
		// (set) Token: 0x06003570 RID: 13680 RVA: 0x001DDCE4 File Offset: 0x001DCCE4
		public object Source
		{
			get
			{
				WeakReference<object> weakReference = (WeakReference<object>)base.GetValue(BindingBase.Feature.ObjectSource, null);
				if (weakReference == null)
				{
					return null;
				}
				object result;
				if (!weakReference.TryGetTarget(out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				base.CheckSealed();
				if (this._sourceInUse != Binding.SourceProperties.None && this._sourceInUse != Binding.SourceProperties.Source)
				{
					throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
					{
						Binding.SourceProperties.Source,
						this._sourceInUse
					}));
				}
				if (value != DependencyProperty.UnsetValue)
				{
					base.SetValue(BindingBase.Feature.ObjectSource, new WeakReference<object>(value));
					this.SourceReference = new ExplicitObjectRef(value);
					return;
				}
				base.ClearValue(BindingBase.Feature.ObjectSource);
				this.SourceReference = null;
			}
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x00105F35 File Offset: 0x00104F35
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSource()
		{
			return false;
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06003572 RID: 13682 RVA: 0x001DDD65 File Offset: 0x001DCD65
		// (set) Token: 0x06003573 RID: 13683 RVA: 0x001DDD78 File Offset: 0x001DCD78
		[DefaultValue(null)]
		public RelativeSource RelativeSource
		{
			get
			{
				return (RelativeSource)base.GetValue(BindingBase.Feature.RelativeSource, null);
			}
			set
			{
				base.CheckSealed();
				if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.RelativeSource)
				{
					base.SetValue(BindingBase.Feature.RelativeSource, value, null);
					this.SourceReference = ((value != null) ? new RelativeObjectRef(value) : null);
					return;
				}
				throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
				{
					Binding.SourceProperties.RelativeSource,
					this._sourceInUse
				}));
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x001DDDE5 File Offset: 0x001DCDE5
		// (set) Token: 0x06003575 RID: 13685 RVA: 0x001DDDF8 File Offset: 0x001DCDF8
		[DefaultValue(null)]
		public string ElementName
		{
			get
			{
				return (string)base.GetValue(BindingBase.Feature.ElementSource, null);
			}
			set
			{
				base.CheckSealed();
				if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.ElementName)
				{
					base.SetValue(BindingBase.Feature.ElementSource, value, null);
					this.SourceReference = ((value != null) ? new ElementObjectRef(value) : null);
					return;
				}
				throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
				{
					Binding.SourceProperties.ElementName,
					this._sourceInUse
				}));
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x001DDE65 File Offset: 0x001DCE65
		// (set) Token: 0x06003577 RID: 13687 RVA: 0x001DDE6D File Offset: 0x001DCE6D
		[DefaultValue(false)]
		public bool IsAsync
		{
			get
			{
				return this._isAsync;
			}
			set
			{
				base.CheckSealed();
				this._isAsync = value;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x001DDE7C File Offset: 0x001DCE7C
		// (set) Token: 0x06003579 RID: 13689 RVA: 0x001DDE86 File Offset: 0x001DCE86
		[DefaultValue(null)]
		public object AsyncState
		{
			get
			{
				return base.GetValue(BindingBase.Feature.AsyncState, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.AsyncState, value, null);
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x001DDE97 File Offset: 0x001DCE97
		// (set) Token: 0x0600357B RID: 13691 RVA: 0x001DDE9F File Offset: 0x001DCE9F
		[DefaultValue(false)]
		public bool BindsDirectlyToSource
		{
			get
			{
				return this._bindsDirectlyToSource;
			}
			set
			{
				base.CheckSealed();
				this._bindsDirectlyToSource = value;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x0600357C RID: 13692 RVA: 0x001DDEAE File Offset: 0x001DCEAE
		// (set) Token: 0x0600357D RID: 13693 RVA: 0x001DDEBE File Offset: 0x001DCEBE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
		{
			get
			{
				return (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			}
			set
			{
				base.SetValue(BindingBase.Feature.ExceptionFilterCallback, value, null);
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x001DDECA File Offset: 0x001DCECA
		internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
		{
			return BindingExpression.CreateBindingExpression(target, dp, this, owner);
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x001DDED5 File Offset: 0x001DCED5
		internal override ValidationRule LookupValidationRule(Type type)
		{
			return BindingBase.LookupValidationRule(type, this.ValidationRulesInternal);
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x001DDEE4 File Offset: 0x001DCEE4
		internal object DoFilterException(object bindExpr, Exception exception)
		{
			UpdateSourceExceptionFilterCallback updateSourceExceptionFilterCallback = (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			if (updateSourceExceptionFilterCallback != null)
			{
				return updateSourceExceptionFilterCallback(bindExpr, exception);
			}
			return exception;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x001DDF0D File Offset: 0x001DCF0D
		internal void UsePath(PropertyPath path)
		{
			this._ppath = path;
			base.SetFlag(BindingBase.BindingFlags.PathGeneratedInternally);
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x001DDF21 File Offset: 0x001DCF21
		internal override BindingBase CreateClone()
		{
			return new Binding();
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x001DDF28 File Offset: 0x001DCF28
		internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
		{
			Binding binding = (Binding)baseClone;
			binding._ppath = this._ppath;
			base.CopyValue(BindingBase.Feature.XPath, binding);
			binding._source = this._source;
			base.CopyValue(BindingBase.Feature.Culture, binding);
			binding._isAsync = this._isAsync;
			base.CopyValue(BindingBase.Feature.AsyncState, binding);
			binding._bindsDirectlyToSource = this._bindsDirectlyToSource;
			binding._doesNotTransferDefaultValue = this._doesNotTransferDefaultValue;
			base.CopyValue(BindingBase.Feature.ObjectSource, binding);
			base.CopyValue(BindingBase.Feature.RelativeSource, binding);
			base.CopyValue(BindingBase.Feature.Converter, binding);
			base.CopyValue(BindingBase.Feature.ConverterParameter, binding);
			binding._attachedPropertiesInPath = this._attachedPropertiesInPath;
			base.CopyValue(BindingBase.Feature.ValidationRules, binding);
			base.InitializeClone(baseClone, mode);
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x001DDFD0 File Offset: 0x001DCFD0
		internal override CultureInfo ConverterCultureInternal
		{
			get
			{
				return this.ConverterCulture;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x001DDFD8 File Offset: 0x001DCFD8
		// (set) Token: 0x06003586 RID: 13702 RVA: 0x001DDFEF File Offset: 0x001DCFEF
		internal ObjectRef SourceReference
		{
			get
			{
				if (this._source != Binding.UnsetSource)
				{
					return this._source;
				}
				return null;
			}
			set
			{
				base.CheckSealed();
				this._source = value;
				this.DetermineSource();
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x001DE004 File Offset: 0x001DD004
		internal bool TreeContextIsRequired
		{
			get
			{
				if (this._attachedPropertiesInPath < 0)
				{
					if (this._ppath != null)
					{
						this._attachedPropertiesInPath = this._ppath.ComputeUnresolvedAttachedPropertiesInPath();
					}
					else
					{
						this._attachedPropertiesInPath = 0;
					}
				}
				bool flag = this._attachedPropertiesInPath > 0;
				if (!flag && base.HasValue(BindingBase.Feature.XPath) && this.XPath.IndexOf(':') >= 0)
				{
					flag = true;
				}
				return flag;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x001DE065 File Offset: 0x001DD065
		internal override Collection<ValidationRule> ValidationRulesInternal
		{
			get
			{
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x001DE075 File Offset: 0x001DD075
		// (set) Token: 0x0600358A RID: 13706 RVA: 0x001DE080 File Offset: 0x001DD080
		internal bool TransfersDefaultValue
		{
			get
			{
				return !this._doesNotTransferDefaultValue;
			}
			set
			{
				base.CheckSealed();
				this._doesNotTransferDefaultValue = !value;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x001DE092 File Offset: 0x001DD092
		internal override bool ValidatesOnNotifyDataErrorsInternal
		{
			get
			{
				return this.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x001DE09C File Offset: 0x001DD09C
		private void DetermineSource()
		{
			this._sourceInUse = ((this._source == Binding.UnsetSource) ? Binding.SourceProperties.None : (base.HasValue(BindingBase.Feature.RelativeSource) ? Binding.SourceProperties.RelativeSource : (base.HasValue(BindingBase.Feature.ElementSource) ? Binding.SourceProperties.ElementName : (base.HasValue(BindingBase.Feature.ObjectSource) ? Binding.SourceProperties.Source : ((this._source == Binding.StaticSourceRef) ? Binding.SourceProperties.StaticSource : Binding.SourceProperties.InternalSource)))));
		}

		// Token: 0x04001C95 RID: 7317
		public static readonly RoutedEvent SourceUpdatedEvent = EventManager.RegisterRoutedEvent("SourceUpdated", RoutingStrategy.Bubble, typeof(EventHandler<DataTransferEventArgs>), typeof(Binding));

		// Token: 0x04001C96 RID: 7318
		public static readonly RoutedEvent TargetUpdatedEvent = EventManager.RegisterRoutedEvent("TargetUpdated", RoutingStrategy.Bubble, typeof(EventHandler<DataTransferEventArgs>), typeof(Binding));

		// Token: 0x04001C97 RID: 7319
		public static readonly DependencyProperty XmlNamespaceManagerProperty = DependencyProperty.RegisterAttached("XmlNamespaceManager", typeof(object), typeof(Binding), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Binding.IsValidXmlNamespaceManager));

		// Token: 0x04001C98 RID: 7320
		public static readonly object DoNothing = new NamedObject("Binding.DoNothing");

		// Token: 0x04001C99 RID: 7321
		public const string IndexerName = "Item[]";

		// Token: 0x04001C9A RID: 7322
		private Binding.SourceProperties _sourceInUse;

		// Token: 0x04001C9B RID: 7323
		private PropertyPath _ppath;

		// Token: 0x04001C9C RID: 7324
		private ObjectRef _source = Binding.UnsetSource;

		// Token: 0x04001C9D RID: 7325
		private bool _isAsync;

		// Token: 0x04001C9E RID: 7326
		private bool _bindsDirectlyToSource;

		// Token: 0x04001C9F RID: 7327
		private bool _doesNotTransferDefaultValue;

		// Token: 0x04001CA0 RID: 7328
		private int _attachedPropertiesInPath;

		// Token: 0x04001CA1 RID: 7329
		private static readonly ObjectRef UnsetSource = new ExplicitObjectRef(null);

		// Token: 0x04001CA2 RID: 7330
		private static readonly ObjectRef StaticSourceRef = new ExplicitObjectRef(BindingExpression.StaticSource);

		// Token: 0x02000AC7 RID: 2759
		private enum SourceProperties : byte
		{
			// Token: 0x04004662 RID: 18018
			None,
			// Token: 0x04004663 RID: 18019
			RelativeSource,
			// Token: 0x04004664 RID: 18020
			ElementName,
			// Token: 0x04004665 RID: 18021
			Source,
			// Token: 0x04004666 RID: 18022
			StaticSource,
			// Token: 0x04004667 RID: 18023
			InternalSource
		}
	}
}
