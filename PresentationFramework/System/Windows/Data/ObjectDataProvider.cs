using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000464 RID: 1124
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class ObjectDataProvider : DataSourceProvider
	{
		// Token: 0x060039BF RID: 14783 RVA: 0x001EE73C File Offset: 0x001ED73C
		public ObjectDataProvider()
		{
			this._constructorParameters = new ParameterCollection(new ParameterCollectionChanged(this.OnParametersChanged));
			this._methodParameters = new ParameterCollection(new ParameterCollectionChanged(this.OnParametersChanged));
			this._sourceDataChangedHandler = new EventHandler(this.OnSourceDataChanged);
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x001EE796 File Offset: 0x001ED796
		// (set) Token: 0x060039C1 RID: 14785 RVA: 0x001EE7A0 File Offset: 0x001ED7A0
		public Type ObjectType
		{
			get
			{
				return this._objectType;
			}
			set
			{
				if (this._mode == ObjectDataProvider.SourceMode.FromInstance)
				{
					throw new InvalidOperationException(SR.Get("ObjectDataProviderCanHaveOnlyOneSource"));
				}
				this._mode = ((value == null) ? ObjectDataProvider.SourceMode.NoSource : ObjectDataProvider.SourceMode.FromType);
				this._constructorParameters.SetReadOnly(false);
				if ((this._needNewInstance = this.SetObjectType(value)) && !base.IsRefreshDeferred)
				{
					base.Refresh();
				}
			}
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x001EE805 File Offset: 0x001ED805
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeObjectType()
		{
			return this._mode == ObjectDataProvider.SourceMode.FromType && this.ObjectType != null;
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060039C3 RID: 14787 RVA: 0x001EE81E File Offset: 0x001ED81E
		// (set) Token: 0x060039C4 RID: 14788 RVA: 0x001EE838 File Offset: 0x001ED838
		public object ObjectInstance
		{
			get
			{
				if (this._instanceProvider == null)
				{
					return this._objectInstance;
				}
				return this._instanceProvider;
			}
			set
			{
				if (this._mode == ObjectDataProvider.SourceMode.FromType)
				{
					throw new InvalidOperationException(SR.Get("ObjectDataProviderCanHaveOnlyOneSource"));
				}
				this._mode = ((value == null) ? ObjectDataProvider.SourceMode.NoSource : ObjectDataProvider.SourceMode.FromInstance);
				if (this.ObjectInstance == value)
				{
					return;
				}
				if (value != null)
				{
					this._constructorParameters.SetReadOnly(true);
					this._constructorParameters.ClearInternal();
				}
				else
				{
					this._constructorParameters.SetReadOnly(false);
				}
				value = this.TryInstanceProvider(value);
				if (this.SetObjectInstance(value) && !base.IsRefreshDeferred)
				{
					base.Refresh();
				}
			}
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x001EE8BD File Offset: 0x001ED8BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeObjectInstance()
		{
			return this._mode == ObjectDataProvider.SourceMode.FromInstance && this.ObjectInstance != null;
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x001EE8D3 File Offset: 0x001ED8D3
		// (set) Token: 0x060039C7 RID: 14791 RVA: 0x001EE8DB File Offset: 0x001ED8DB
		[DefaultValue(null)]
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
			set
			{
				this._methodName = value;
				this.OnPropertyChanged("MethodName");
				if (!base.IsRefreshDeferred)
				{
					base.Refresh();
				}
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x001EE8FD File Offset: 0x001ED8FD
		public IList ConstructorParameters
		{
			get
			{
				return this._constructorParameters;
			}
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x001EE905 File Offset: 0x001ED905
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeConstructorParameters()
		{
			return this._mode == ObjectDataProvider.SourceMode.FromType && this._constructorParameters.Count > 0;
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x001EE920 File Offset: 0x001ED920
		public IList MethodParameters
		{
			get
			{
				return this._methodParameters;
			}
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x001EE928 File Offset: 0x001ED928
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeMethodParameters()
		{
			return this._methodParameters.Count > 0;
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x001EE938 File Offset: 0x001ED938
		// (set) Token: 0x060039CD RID: 14797 RVA: 0x001EE940 File Offset: 0x001ED940
		[DefaultValue(false)]
		public bool IsAsynchronous
		{
			get
			{
				return this._isAsynchronous;
			}
			set
			{
				this._isAsynchronous = value;
				this.OnPropertyChanged("IsAsynchronous");
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x001EE954 File Offset: 0x001ED954
		protected override void BeginQuery()
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.BeginQuery(new object[]
				{
					TraceData.Identify(this),
					this.IsAsynchronous ? "asynchronous" : "synchronous"
				}), null);
			}
			if (this.IsAsynchronous)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.QueryWorker), null);
				return;
			}
			this.QueryWorker(null);
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x001EE9C0 File Offset: 0x001ED9C0
		private object TryInstanceProvider(object value)
		{
			if (this._instanceProvider != null)
			{
				this._instanceProvider.DataChanged -= this._sourceDataChangedHandler;
			}
			this._instanceProvider = (value as DataSourceProvider);
			if (this._instanceProvider != null)
			{
				this._instanceProvider.DataChanged += this._sourceDataChangedHandler;
				value = this._instanceProvider.Data;
			}
			return value;
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001EEA19 File Offset: 0x001EDA19
		private bool SetObjectInstance(object value)
		{
			if (this._objectInstance == value)
			{
				return false;
			}
			this._objectInstance = value;
			this.SetObjectType((value != null) ? value.GetType() : null);
			this.OnPropertyChanged("ObjectInstance");
			return true;
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x001EEA4C File Offset: 0x001EDA4C
		private bool SetObjectType(Type newType)
		{
			if (this._objectType != newType)
			{
				this._objectType = newType;
				this.OnPropertyChanged("ObjectType");
				return true;
			}
			return false;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x001EEA74 File Offset: 0x001EDA74
		private void QueryWorker(object obj)
		{
			object obj2 = null;
			Exception ex = null;
			if (this._mode == ObjectDataProvider.SourceMode.NoSource || this._objectType == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.ObjectDataProviderHasNoSource, null);
				}
				ex = new InvalidOperationException(SR.Get("ObjectDataProviderHasNoSource"));
			}
			else
			{
				Exception ex2 = null;
				if (this._needNewInstance && this._mode == ObjectDataProvider.SourceMode.FromType)
				{
					if (this._objectType.GetConstructors().Length != 0)
					{
						this._objectInstance = this.CreateObjectInstance(out ex2);
					}
					this._needNewInstance = false;
				}
				if (string.IsNullOrEmpty(this.MethodName))
				{
					obj2 = this._objectInstance;
				}
				else
				{
					obj2 = this.InvokeMethodOnInstance(out ex);
					if (ex != null && ex2 != null)
					{
						ex = ex2;
					}
				}
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.QueryFinished(new object[]
				{
					TraceData.Identify(this),
					base.Dispatcher.CheckAccess() ? "synchronous" : "asynchronous",
					TraceData.Identify(obj2),
					TraceData.IdentifyException(ex)
				}), null);
			}
			this.OnQueryFinished(obj2, ex, null, null);
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x001EEB7C File Offset: 0x001EDB7C
		private object CreateObjectInstance(out Exception e)
		{
			object result = null;
			string text = null;
			e = null;
			try
			{
				object[] array = new object[this._constructorParameters.Count];
				this._constructorParameters.CopyTo(array, 0);
				result = Activator.CreateInstance(this._objectType, BindingFlags.Default, null, array, CultureInfo.InvariantCulture);
				this.OnPropertyChanged("ObjectInstance");
			}
			catch (ArgumentException ex)
			{
				text = "Cannot create Context Affinity object.";
				e = ex;
			}
			catch (COMException ex2)
			{
				text = "Marshaling issue detected.";
				e = ex2;
			}
			catch (MissingMethodException ex3)
			{
				text = "Wrong parameters for constructor.";
				e = ex3;
			}
			catch (Exception ex4)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex4))
				{
					throw;
				}
				text = null;
				e = ex4;
			}
			catch
			{
				text = null;
				e = new InvalidOperationException(SR.Get("ObjectDataProviderNonCLSException", new object[]
				{
					this._objectType.Name
				}));
			}
			if (e != null || text != null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.ObjDPCreateFailed, null, new object[]
					{
						this._objectType.Name,
						text,
						e
					}, new object[]
					{
						e
					});
				}
				if (!this.IsAsynchronous && text == null)
				{
					throw e;
				}
			}
			return result;
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001EECC8 File Offset: 0x001EDCC8
		private object InvokeMethodOnInstance(out Exception e)
		{
			object result = null;
			string text = null;
			e = null;
			object[] array = new object[this._methodParameters.Count];
			this._methodParameters.CopyTo(array, 0);
			try
			{
				result = this._objectType.InvokeMember(this.MethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, this._objectInstance, array, CultureInfo.InvariantCulture);
			}
			catch (ArgumentException ex)
			{
				text = "Parameter array contains a string that is a null reference.";
				e = ex;
			}
			catch (MethodAccessException ex2)
			{
				text = "The specified member is a class initializer.";
				e = ex2;
			}
			catch (MissingMethodException ex3)
			{
				text = "No method was found with matching parameter signature.";
				e = ex3;
			}
			catch (TargetException ex4)
			{
				text = "The specified member cannot be invoked on target.";
				e = ex4;
			}
			catch (AmbiguousMatchException ex5)
			{
				text = "More than one method matches the binding criteria.";
				e = ex5;
			}
			catch (Exception ex6)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex6))
				{
					throw;
				}
				text = null;
				e = ex6;
			}
			catch
			{
				text = null;
				e = new InvalidOperationException(SR.Get("ObjectDataProviderNonCLSExceptionInvoke", new object[]
				{
					this.MethodName,
					this._objectType.Name
				}));
			}
			if (e != null || text != null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.ObjDPInvokeFailed, null, new object[]
					{
						this.MethodName,
						this._objectType.Name,
						text,
						e
					}, new object[]
					{
						e
					});
				}
				if (!this.IsAsynchronous && text == null)
				{
					throw e;
				}
			}
			return result;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x001EEE64 File Offset: 0x001EDE64
		private void OnParametersChanged(ParameterCollection sender)
		{
			if (sender == this._constructorParameters)
			{
				Invariant.Assert(this._mode != ObjectDataProvider.SourceMode.FromInstance);
				this._needNewInstance = true;
			}
			if (!base.IsRefreshDeferred)
			{
				base.Refresh();
			}
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x001EEE95 File Offset: 0x001EDE95
		private void OnSourceDataChanged(object sender, EventArgs args)
		{
			Invariant.Assert(sender == this._instanceProvider);
			if (this.SetObjectInstance(this._instanceProvider.Data) && !base.IsRefreshDeferred)
			{
				base.Refresh();
			}
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x001EEEC6 File Offset: 0x001EDEC6
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04001D71 RID: 7537
		private Type _objectType;

		// Token: 0x04001D72 RID: 7538
		private object _objectInstance;

		// Token: 0x04001D73 RID: 7539
		private string _methodName;

		// Token: 0x04001D74 RID: 7540
		private DataSourceProvider _instanceProvider;

		// Token: 0x04001D75 RID: 7541
		private ParameterCollection _constructorParameters;

		// Token: 0x04001D76 RID: 7542
		private ParameterCollection _methodParameters;

		// Token: 0x04001D77 RID: 7543
		private bool _isAsynchronous;

		// Token: 0x04001D78 RID: 7544
		private ObjectDataProvider.SourceMode _mode;

		// Token: 0x04001D79 RID: 7545
		private bool _needNewInstance = true;

		// Token: 0x04001D7A RID: 7546
		private EventHandler _sourceDataChangedHandler;

		// Token: 0x04001D7B RID: 7547
		private const string s_instance = "ObjectInstance";

		// Token: 0x04001D7C RID: 7548
		private const string s_type = "ObjectType";

		// Token: 0x04001D7D RID: 7549
		private const string s_method = "MethodName";

		// Token: 0x04001D7E RID: 7550
		private const string s_async = "IsAsynchronous";

		// Token: 0x04001D7F RID: 7551
		private const BindingFlags s_invokeMethodFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding;

		// Token: 0x02000AE9 RID: 2793
		private enum SourceMode
		{
			// Token: 0x04004725 RID: 18213
			NoSource,
			// Token: 0x04004726 RID: 18214
			FromType,
			// Token: 0x04004727 RID: 18215
			FromInstance
		}
	}
}
