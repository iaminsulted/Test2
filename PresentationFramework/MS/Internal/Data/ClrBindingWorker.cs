using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000209 RID: 521
	internal class ClrBindingWorker : BindingWorker
	{
		// Token: 0x0600130A RID: 4874 RVA: 0x0014BF90 File Offset: 0x0014AF90
		internal ClrBindingWorker(BindingExpression b, DataBindEngine engine) : base(b)
		{
			PropertyPath propertyPath = base.ParentBinding.Path;
			if (base.ParentBinding.XPath != null)
			{
				propertyPath = this.PrepareXmlBinding(propertyPath);
			}
			if (propertyPath == null)
			{
				propertyPath = new PropertyPath(string.Empty, Array.Empty<object>());
			}
			if (base.ParentBinding.Path == null)
			{
				base.ParentBinding.UsePath(propertyPath);
			}
			this._pathWorker = new PropertyPathWorker(propertyPath, this, base.IsDynamic, engine);
			this._pathWorker.SetTreeContext(base.ParentBindingExpression.TargetElementReference);
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0014C01C File Offset: 0x0014B01C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private PropertyPath PrepareXmlBinding(PropertyPath path)
		{
			if (path == null)
			{
				DependencyProperty targetProperty = base.TargetProperty;
				Type propertyType = targetProperty.PropertyType;
				string path2;
				if (propertyType == typeof(object))
				{
					if (targetProperty == BindingExpressionBase.NoTargetProperty || targetProperty == Selector.SelectedValueProperty || targetProperty.OwnerType == typeof(LiveShapingList))
					{
						path2 = "/InnerText";
					}
					else if (targetProperty == FrameworkElement.DataContextProperty || targetProperty == CollectionViewSource.SourceProperty)
					{
						path2 = string.Empty;
					}
					else
					{
						path2 = "/";
					}
				}
				else if (propertyType.IsAssignableFrom(typeof(XmlDataCollection)))
				{
					path2 = string.Empty;
				}
				else
				{
					path2 = "/InnerText";
				}
				path = new PropertyPath(path2, Array.Empty<object>());
			}
			if (path.SVI.Length != 0)
			{
				base.SetValue(BindingWorker.Feature.XmlWorker, new XmlBindingWorker(this, path.SVI[0].drillIn == DrillIn.Never));
			}
			return path;
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x0014C0F6 File Offset: 0x0014B0F6
		internal override Type SourcePropertyType
		{
			get
			{
				return this.PW.GetType(this.PW.Length - 1);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x0014C110 File Offset: 0x0014B110
		internal override bool IsDBNullValidForUpdate
		{
			get
			{
				return this.PW.IsDBNullValidForUpdate;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x0014C11D File Offset: 0x0014B11D
		internal override object SourceItem
		{
			get
			{
				return this.PW.SourceItem;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x0014C12A File Offset: 0x0014B12A
		internal override string SourcePropertyName
		{
			get
			{
				return this.PW.SourcePropertyName;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x0014C138 File Offset: 0x0014B138
		internal override bool CanUpdate
		{
			get
			{
				PropertyPathWorker pw = this.PW;
				int num = this.PW.Length - 1;
				if (num < 0)
				{
					return false;
				}
				object item = pw.GetItem(num);
				if (item == null || item == BindingExpression.NullDataItem)
				{
					return false;
				}
				object accessor = pw.GetAccessor(num);
				return accessor != null && (accessor != DependencyProperty.UnsetValue || this.XmlWorker != null);
			}
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0014C194 File Offset: 0x0014B194
		internal override void AttachDataItem()
		{
			object obj;
			if (this.XmlWorker == null)
			{
				obj = base.DataItem;
			}
			else
			{
				this.XmlWorker.AttachDataItem();
				obj = this.XmlWorker.RawValue();
			}
			this.PW.AttachToRootItem(obj);
			if (this.PW.Length == 0)
			{
				base.ParentBindingExpression.SetupDefaultValueConverter(obj.GetType());
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0014C1F4 File Offset: 0x0014B1F4
		internal override void DetachDataItem()
		{
			this.PW.DetachFromRootItem();
			if (this.XmlWorker != null)
			{
				this.XmlWorker.DetachDataItem();
			}
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest != null)
			{
				asyncGetValueRequest.Cancel();
				base.ClearValue(BindingWorker.Feature.PendingGetValueRequest);
			}
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest != null)
			{
				asyncSetValueRequest.Cancel();
				base.ClearValue(BindingWorker.Feature.PendingSetValueRequest);
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0014C25B File Offset: 0x0014B25B
		internal override object RawValue()
		{
			object result = this.PW.RawValue();
			this.SetStatus(this.PW.Status);
			return result;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0014C279 File Offset: 0x0014B279
		internal override void RefreshValue()
		{
			this.PW.RefreshValue();
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0014C288 File Offset: 0x0014B288
		internal override void UpdateValue(object value)
		{
			int level = this.PW.Length - 1;
			object item = this.PW.GetItem(level);
			if (item == null || item == BindingExpression.NullDataItem)
			{
				return;
			}
			if (base.ParentBinding.IsAsync && !(this.PW.GetAccessor(level) is DependencyProperty))
			{
				this.RequestAsyncSetValue(item, value);
				return;
			}
			this.PW.SetValue(item, value);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0014C2F2 File Offset: 0x0014B2F2
		internal override void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
			if (this.XmlWorker != null)
			{
				this.XmlWorker.OnCurrentChanged(collectionView, args);
			}
			this.PW.OnCurrentChanged(collectionView);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0014C315 File Offset: 0x0014B315
		internal override bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			return this.PW.UsesDependencyProperty(d, dp);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0014C324 File Offset: 0x0014B324
		internal override void OnSourceInvalidation(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
			this.PW.OnDependencyPropertyChanged(d, dp, isASubPropertyChange);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0014C334 File Offset: 0x0014B334
		internal override bool IsPathCurrent()
		{
			object rootItem = (this.XmlWorker == null) ? base.DataItem : this.XmlWorker.RawValue();
			return this.PW.IsPathCurrent(rootItem);
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x0014C369 File Offset: 0x0014B369
		internal bool TransfersDefaultValue
		{
			get
			{
				return base.ParentBinding.TransfersDefaultValue;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x0014C376 File Offset: 0x0014B376
		internal bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.ParentBindingExpression.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0014C383 File Offset: 0x0014B383
		internal void CancelPendingTasks()
		{
			base.ParentBindingExpression.CancelPendingTasks();
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0014C390 File Offset: 0x0014B390
		internal bool AsyncGet(object item, int level)
		{
			if (base.ParentBinding.IsAsync)
			{
				this.RequestAsyncGetValue(item, level);
				return true;
			}
			return false;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0014C3AC File Offset: 0x0014B3AC
		internal void ReplaceCurrentItem(ICollectionView oldCollectionView, ICollectionView newCollectionView)
		{
			if (oldCollectionView != null)
			{
				CurrentChangedEventManager.RemoveHandler(oldCollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.RemoveHandler(oldCollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			if (newCollectionView != null)
			{
				CurrentChangedEventManager.AddHandler(newCollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.AddHandler(newCollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0014C42C File Offset: 0x0014B42C
		internal void NewValueAvailable(bool dependencySourcesChanged, bool initialValue, bool isASubPropertyChange)
		{
			this.SetStatus(this.PW.Status);
			BindingExpression parentBindingExpression = base.ParentBindingExpression;
			BindingGroup bindingGroup = parentBindingExpression.BindingGroup;
			if (bindingGroup != null)
			{
				bindingGroup.UpdateTable(parentBindingExpression);
			}
			if (dependencySourcesChanged)
			{
				this.ReplaceDependencySources();
			}
			if (base.Status != BindingStatusInternal.AsyncRequestPending)
			{
				if (!initialValue)
				{
					parentBindingExpression.ScheduleTransfer(isASubPropertyChange);
					return;
				}
				base.SetTransferIsPending(false);
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0014C486 File Offset: 0x0014B486
		internal void SetupDefaultValueConverter(Type type)
		{
			base.ParentBindingExpression.SetupDefaultValueConverter(type);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0014C494 File Offset: 0x0014B494
		internal bool IsValidValue(object value)
		{
			return base.TargetProperty.IsValidValue(value);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0014C4A4 File Offset: 0x0014B4A4
		internal void OnSourcePropertyChanged(object o, string propName)
		{
			int level;
			if (!base.IgnoreSourcePropertyChange && (level = this.PW.LevelForPropertyChange(o, propName)) >= 0)
			{
				if (base.Dispatcher.Thread == Thread.CurrentThread)
				{
					this.PW.OnPropertyChangedAtLevel(level);
					return;
				}
				base.SetTransferIsPending(true);
				if (base.ParentBindingExpression.TargetWantsCrossThreadNotifications)
				{
					LiveShapingItem liveShapingItem = base.TargetElement as LiveShapingItem;
					if (liveShapingItem != null)
					{
						liveShapingItem.OnCrossThreadPropertyChange(base.TargetProperty);
					}
				}
				base.Engine.Marshal(new DispatcherOperationCallback(this.ScheduleTransferOperation), null, 1);
			}
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0014C534 File Offset: 0x0014B534
		internal void OnDataErrorsChanged(INotifyDataErrorInfo indei, string propName)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				base.ParentBindingExpression.UpdateNotifyDataErrors(indei, propName, DependencyProperty.UnsetValue);
				return;
			}
			if (!base.ParentBindingExpression.IsDataErrorsChangedPending)
			{
				base.ParentBindingExpression.IsDataErrorsChangedPending = true;
				base.Engine.Marshal(delegate(object arg)
				{
					object[] array = (object[])arg;
					base.ParentBindingExpression.UpdateNotifyDataErrors((INotifyDataErrorInfo)array[0], (string)array[1], DependencyProperty.UnsetValue);
					return null;
				}, new object[]
				{
					indei,
					propName
				}, 1);
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0014C5A8 File Offset: 0x0014B5A8
		internal void OnXmlValueChanged()
		{
			object item = this.PW.GetItem(0);
			this.OnSourcePropertyChanged(item, null);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0014C5CA File Offset: 0x0014B5CA
		internal void UseNewXmlItem(object item)
		{
			this.PW.DetachFromRootItem();
			this.PW.AttachToRootItem(item);
			if (base.Status != BindingStatusInternal.AsyncRequestPending)
			{
				base.ParentBindingExpression.ScheduleTransfer(false);
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0014C5F8 File Offset: 0x0014B5F8
		internal object GetResultNode()
		{
			return this.PW.GetItem(0);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0014C606 File Offset: 0x0014B606
		internal DependencyObject CheckTarget()
		{
			return base.TargetElement;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0014C610 File Offset: 0x0014B610
		internal void ReportGetValueError(int k, object item, Exception ex)
		{
			if (TraceData.IsEnabled)
			{
				SourceValueInfo sourceValueInfo = this.PW.GetSourceValueInfo(k);
				Type type = this.PW.GetType(k);
				string text = (k > 0) ? this.PW.GetSourceValueInfo(k - 1).name : string.Empty;
				TraceData.TraceAndNotify(base.ParentBindingExpression.TraceLevel, TraceData.CannotGetClrRawValue(new object[]
				{
					sourceValueInfo.propertyName,
					type.Name,
					text,
					AvTrace.TypeName(item)
				}), base.ParentBindingExpression, ex);
			}
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0014C6A0 File Offset: 0x0014B6A0
		internal void ReportSetValueError(int k, object item, object value, Exception ex)
		{
			if (TraceData.IsEnabled)
			{
				SourceValueInfo sourceValueInfo = this.PW.GetSourceValueInfo(k);
				Type type = this.PW.GetType(k);
				TraceData.TraceAndNotify(TraceEventType.Error, TraceData.CannotSetClrRawValue(new object[]
				{
					sourceValueInfo.propertyName,
					type.Name,
					AvTrace.TypeName(item),
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value)
				}), base.ParentBindingExpression, ex);
			}
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0014C714 File Offset: 0x0014B714
		internal void ReportRawValueErrors(int k, object item, object info)
		{
			if (TraceData.IsEnabled)
			{
				if (item == null)
				{
					TraceData.TraceAndNotify(TraceEventType.Information, TraceData.MissingDataItem, base.ParentBindingExpression, null);
				}
				if (info == null)
				{
					TraceData.TraceAndNotify(TraceEventType.Information, TraceData.MissingInfo, base.ParentBindingExpression, null);
				}
				if (item == BindingExpression.NullDataItem)
				{
					TraceData.TraceAndNotify(TraceEventType.Information, TraceData.NullDataItem, base.ParentBindingExpression, null);
				}
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0014C76C File Offset: 0x0014B76C
		internal void ReportBadXPath(TraceEventType traceType)
		{
			XmlBindingWorker xmlWorker = this.XmlWorker;
			if (xmlWorker != null)
			{
				xmlWorker.ReportBadXPath(traceType);
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x0014C78A File Offset: 0x0014B78A
		private PropertyPathWorker PW
		{
			get
			{
				return this._pathWorker;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x0014C792 File Offset: 0x0014B792
		private XmlBindingWorker XmlWorker
		{
			get
			{
				return (XmlBindingWorker)base.GetValue(BindingWorker.Feature.XmlWorker, null);
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0014C7A1 File Offset: 0x0014B7A1
		private void SetStatus(PropertyPathStatus status)
		{
			switch (status)
			{
			case PropertyPathStatus.Inactive:
				base.Status = BindingStatusInternal.Inactive;
				return;
			case PropertyPathStatus.Active:
				base.Status = BindingStatusInternal.Active;
				return;
			case PropertyPathStatus.PathError:
				base.Status = BindingStatusInternal.PathError;
				return;
			case PropertyPathStatus.AsyncRequestPending:
				base.Status = BindingStatusInternal.AsyncRequestPending;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0014C7DC File Offset: 0x0014B7DC
		private void ReplaceDependencySources()
		{
			if (!base.ParentBindingExpression.IsDetaching)
			{
				int num = this.PW.Length;
				if (this.PW.NeedsDirectNotification)
				{
					num++;
				}
				WeakDependencySource[] array = new WeakDependencySource[num];
				int n = 0;
				if (base.IsDynamic)
				{
					for (int i = 0; i < this.PW.Length; i++)
					{
						DependencyProperty dependencyProperty = this.PW.GetAccessor(i) as DependencyProperty;
						if (dependencyProperty != null)
						{
							DependencyObject dependencyObject = this.PW.GetItem(i) as DependencyObject;
							if (dependencyObject != null)
							{
								array[n++] = new WeakDependencySource(dependencyObject, dependencyProperty);
							}
						}
					}
					if (this.PW.NeedsDirectNotification)
					{
						DependencyObject dependencyObject2 = this.PW.RawValue() as Freezable;
						if (dependencyObject2 != null)
						{
							array[n++] = new WeakDependencySource(dependencyObject2, DependencyObject.DirectDependencyProperty);
						}
					}
				}
				base.ParentBindingExpression.ChangeWorkerSources(array, n);
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0014C8C4 File Offset: 0x0014B8C4
		private void RequestAsyncGetValue(object item, int level)
		{
			string nameFromInfo = this.GetNameFromInfo(this.PW.GetAccessor(level));
			Invariant.Assert(nameFromInfo != null, "Async GetValue expects a name");
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest != null)
			{
				asyncGetValueRequest.Cancel();
			}
			asyncGetValueRequest = new AsyncGetValueRequest(item, nameFromInfo, base.ParentBinding.AsyncState, ClrBindingWorker.DoGetValueCallback, ClrBindingWorker.CompleteGetValueCallback, new object[]
			{
				this,
				level
			});
			base.SetValue(BindingWorker.Feature.PendingGetValueRequest, asyncGetValueRequest);
			base.Engine.AddAsyncRequest(base.TargetElement, asyncGetValueRequest);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0014C954 File Offset: 0x0014B954
		private static object OnGetValueCallback(AsyncDataRequest adr)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)adr;
			object value = ((ClrBindingWorker)asyncGetValueRequest.Args[0]).PW.GetValue(asyncGetValueRequest.SourceItem, (int)asyncGetValueRequest.Args[1]);
			if (value == PropertyPathWorker.IListIndexOutOfRange)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return value;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0014C9A8 File Offset: 0x0014B9A8
		private static object OnCompleteGetValueCallback(AsyncDataRequest adr)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)adr;
			DataBindEngine engine = ((ClrBindingWorker)asyncGetValueRequest.Args[0]).Engine;
			if (engine != null)
			{
				engine.Marshal(ClrBindingWorker.CompleteGetValueLocalCallback, asyncGetValueRequest, 1);
			}
			return null;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0014C9E4 File Offset: 0x0014B9E4
		private static object OnCompleteGetValueOperation(object arg)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)arg;
			((ClrBindingWorker)asyncGetValueRequest.Args[0]).CompleteGetValue(asyncGetValueRequest);
			return null;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0014CA0C File Offset: 0x0014BA0C
		private void CompleteGetValue(AsyncGetValueRequest request)
		{
			if ((AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null) == request)
			{
				base.ClearValue(BindingWorker.Feature.PendingGetValueRequest);
				int num = (int)request.Args[1];
				if (this.CheckTarget() == null)
				{
					return;
				}
				AsyncRequestStatus status = request.Status;
				if (status != AsyncRequestStatus.Completed)
				{
					if (status != AsyncRequestStatus.Failed)
					{
						return;
					}
					this.ReportGetValueError(num, request.SourceItem, request.Exception);
					this.PW.OnNewValue(num, DependencyProperty.UnsetValue);
				}
				else
				{
					this.PW.OnNewValue(num, request.Result);
					this.SetStatus(this.PW.Status);
					if (num == this.PW.Length - 1)
					{
						base.ParentBindingExpression.TransferValue(request.Result, false);
						return;
					}
				}
			}
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0014CAC4 File Offset: 0x0014BAC4
		private void RequestAsyncSetValue(object item, object value)
		{
			string nameFromInfo = this.GetNameFromInfo(this.PW.GetAccessor(this.PW.Length - 1));
			Invariant.Assert(nameFromInfo != null, "Async SetValue expects a name");
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest != null)
			{
				asyncSetValueRequest.Cancel();
			}
			asyncSetValueRequest = new AsyncSetValueRequest(item, nameFromInfo, value, base.ParentBinding.AsyncState, ClrBindingWorker.DoSetValueCallback, ClrBindingWorker.CompleteSetValueCallback, new object[]
			{
				this
			});
			base.SetValue(BindingWorker.Feature.PendingSetValueRequest, asyncSetValueRequest);
			base.Engine.AddAsyncRequest(base.TargetElement, asyncSetValueRequest);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0014CB58 File Offset: 0x0014BB58
		private static object OnSetValueCallback(AsyncDataRequest adr)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)adr;
			((ClrBindingWorker)asyncSetValueRequest.Args[0]).PW.SetValue(asyncSetValueRequest.TargetItem, asyncSetValueRequest.Value);
			return null;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0014CB90 File Offset: 0x0014BB90
		private static object OnCompleteSetValueCallback(AsyncDataRequest adr)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)adr;
			DataBindEngine engine = ((ClrBindingWorker)asyncSetValueRequest.Args[0]).Engine;
			if (engine != null)
			{
				engine.Marshal(ClrBindingWorker.CompleteSetValueLocalCallback, asyncSetValueRequest, 1);
			}
			return null;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0014CBCC File Offset: 0x0014BBCC
		private static object OnCompleteSetValueOperation(object arg)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)arg;
			((ClrBindingWorker)asyncSetValueRequest.Args[0]).CompleteSetValue(asyncSetValueRequest);
			return null;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0014CBF4 File Offset: 0x0014BBF4
		private void CompleteSetValue(AsyncSetValueRequest request)
		{
			if ((AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null) == request)
			{
				base.ClearValue(BindingWorker.Feature.PendingSetValueRequest);
				if (this.CheckTarget() == null)
				{
					return;
				}
				AsyncRequestStatus status = request.Status;
				if (status != AsyncRequestStatus.Completed && status == AsyncRequestStatus.Failed)
				{
					object obj = base.ParentBinding.DoFilterException(base.ParentBindingExpression, request.Exception);
					Exception ex = obj as Exception;
					ValidationError validationError;
					if (ex != null)
					{
						if (TraceData.IsEnabled)
						{
							int k = this.PW.Length - 1;
							object value = request.Value;
							this.ReportSetValueError(k, request.TargetItem, request.Value, ex);
							return;
						}
					}
					else if ((validationError = (obj as ValidationError)) != null)
					{
						Validation.MarkInvalid(base.ParentBindingExpression, validationError);
					}
				}
			}
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0014CCA0 File Offset: 0x0014BCA0
		private string GetNameFromInfo(object info)
		{
			MemberInfo memberInfo;
			if ((memberInfo = (info as MemberInfo)) != null)
			{
				return memberInfo.Name;
			}
			PropertyDescriptor propertyDescriptor;
			if ((propertyDescriptor = (info as PropertyDescriptor)) != null)
			{
				return propertyDescriptor.Name;
			}
			DynamicObjectAccessor dynamicObjectAccessor;
			if ((dynamicObjectAccessor = (info as DynamicObjectAccessor)) != null)
			{
				return dynamicObjectAccessor.PropertyName;
			}
			return null;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0014CCE7 File Offset: 0x0014BCE7
		private object ScheduleTransferOperation(object arg)
		{
			this.PW.RefreshValue();
			return null;
		}

		// Token: 0x04000B6D RID: 2925
		private static readonly AsyncRequestCallback DoGetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnGetValueCallback);

		// Token: 0x04000B6E RID: 2926
		private static readonly AsyncRequestCallback CompleteGetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnCompleteGetValueCallback);

		// Token: 0x04000B6F RID: 2927
		private static readonly DispatcherOperationCallback CompleteGetValueLocalCallback = new DispatcherOperationCallback(ClrBindingWorker.OnCompleteGetValueOperation);

		// Token: 0x04000B70 RID: 2928
		private static readonly AsyncRequestCallback DoSetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnSetValueCallback);

		// Token: 0x04000B71 RID: 2929
		private static readonly AsyncRequestCallback CompleteSetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnCompleteSetValueCallback);

		// Token: 0x04000B72 RID: 2930
		private static readonly DispatcherOperationCallback CompleteSetValueLocalCallback = new DispatcherOperationCallback(ClrBindingWorker.OnCompleteSetValueOperation);

		// Token: 0x04000B73 RID: 2931
		private PropertyPathWorker _pathWorker;
	}
}
