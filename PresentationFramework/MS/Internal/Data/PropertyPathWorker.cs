using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace MS.Internal.Data
{
	// Token: 0x0200023C RID: 572
	internal sealed class PropertyPathWorker : IWeakEventListener
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x00155CBC File Offset: 0x00154CBC
		internal PropertyPathWorker(PropertyPath path) : this(path, DataBindEngine.CurrentDataBindEngine)
		{
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00155CCA File Offset: 0x00154CCA
		internal PropertyPathWorker(PropertyPath path, ClrBindingWorker host, bool isDynamic, DataBindEngine engine) : this(path, engine)
		{
			this._host = host;
			this._isDynamic = isDynamic;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00155CE4 File Offset: 0x00154CE4
		private PropertyPathWorker(PropertyPath path, DataBindEngine engine)
		{
			this._parent = path;
			this._arySVS = new PropertyPathWorker.SourceValueState[path.Length];
			this._engine = engine;
			for (int i = this._arySVS.Length - 1; i >= 0; i--)
			{
				this._arySVS[i].item = BindingExpressionBase.CreateReference(BindingExpression.NullDataItem);
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x00155D46 File Offset: 0x00154D46
		internal int Length
		{
			get
			{
				return this._parent.Length;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00155D53 File Offset: 0x00154D53
		internal PropertyPathStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00155D5B File Offset: 0x00154D5B
		// (set) Token: 0x060015A6 RID: 5542 RVA: 0x00155D6D File Offset: 0x00154D6D
		internal DependencyObject TreeContext
		{
			get
			{
				return BindingExpressionBase.GetReference(this._treeContext) as DependencyObject;
			}
			set
			{
				this._treeContext = BindingExpressionBase.CreateReference(value);
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x00155D7B File Offset: 0x00154D7B
		internal void SetTreeContext(WeakReference wr)
		{
			this._treeContext = BindingExpressionBase.CreateReference(wr);
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00155D89 File Offset: 0x00154D89
		internal bool IsDBNullValidForUpdate
		{
			get
			{
				if (this._isDBNullValidForUpdate == null)
				{
					this.DetermineWhetherDBNullIsValid();
				}
				return this._isDBNullValidForUpdate.Value;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00155DAC File Offset: 0x00154DAC
		internal object SourceItem
		{
			get
			{
				int num = this.Length - 1;
				object obj = (num >= 0) ? this.GetItem(num) : null;
				if (obj == BindingExpression.NullDataItem)
				{
					obj = null;
				}
				return obj;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00155DDC File Offset: 0x00154DDC
		internal string SourcePropertyName
		{
			get
			{
				int num = this.Length - 1;
				if (num < 0)
				{
					return null;
				}
				SourceValueType type = this.SVI[num].type;
				if (type != SourceValueType.Property)
				{
					if (type != SourceValueType.Indexer)
					{
						return null;
					}
					string path = this._parent.Path;
					int startIndex = path.LastIndexOf('[');
					return path.Substring(startIndex);
				}
				else
				{
					PropertyInfo propertyInfo;
					PropertyDescriptor propertyDescriptor;
					DependencyProperty dependencyProperty;
					DynamicPropertyAccessor dynamicPropertyAccessor;
					this.SetPropertyInfo(this.GetAccessor(num), out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
					if (dependencyProperty != null)
					{
						return dependencyProperty.Name;
					}
					if (propertyInfo != null)
					{
						return propertyInfo.Name;
					}
					if (propertyDescriptor != null)
					{
						return propertyDescriptor.Name;
					}
					if (dynamicPropertyAccessor == null)
					{
						return null;
					}
					return dynamicPropertyAccessor.PropertyName;
				}
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x00155E7B File Offset: 0x00154E7B
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x00155E83 File Offset: 0x00154E83
		internal bool NeedsDirectNotification
		{
			get
			{
				return this._needsDirectNotification;
			}
			private set
			{
				if (value)
				{
					this._dependencySourcesChanged = true;
				}
				this._needsDirectNotification = value;
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00155E96 File Offset: 0x00154E96
		internal object GetItem(int level)
		{
			return BindingExpressionBase.GetReference(this._arySVS[level].item);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00155EAE File Offset: 0x00154EAE
		internal object GetAccessor(int level)
		{
			return this._arySVS[level].info;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00155EC4 File Offset: 0x00154EC4
		internal object[] GetIndexerArguments(int level)
		{
			object[] args = this._arySVS[level].args;
			PropertyPathWorker.IListIndexerArg listIndexerArg;
			if (args != null && args.Length == 1 && (listIndexerArg = (args[0] as PropertyPathWorker.IListIndexerArg)) != null)
			{
				return new object[]
				{
					listIndexerArg.Value
				};
			}
			return args;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00155F0E File Offset: 0x00154F0E
		internal Type GetType(int level)
		{
			return this._arySVS[level].type;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00155F21 File Offset: 0x00154F21
		internal IDisposable SetContext(object rootItem)
		{
			if (this._contextHelper == null)
			{
				this._contextHelper = new PropertyPathWorker.ContextHelper(this);
			}
			this._contextHelper.SetContext(rootItem);
			return this._contextHelper;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00155F49 File Offset: 0x00154F49
		internal void AttachToRootItem(object rootItem)
		{
			this._rootItem = BindingExpressionBase.CreateReference(rootItem);
			this.UpdateSourceValueState(-1, null);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00155F5F File Offset: 0x00154F5F
		internal void DetachFromRootItem()
		{
			this._rootItem = BindingExpression.NullDataItem;
			this.UpdateSourceValueState(-1, null);
			this._rootItem = null;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00155F7C File Offset: 0x00154F7C
		internal object GetValue(object item, int level)
		{
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.CreateExpression);
			object obj = DependencyProperty.UnsetValue;
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(this._arySVS[level].info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			switch (this.SVI[level].type)
			{
			case SourceValueType.Property:
				if (propertyInfo != null)
				{
					obj = propertyInfo.GetValue(item, null);
				}
				else if (propertyDescriptor != null)
				{
					bool indexerIsNext = level + 1 < this.SVI.Length && this.SVI[level + 1].type == SourceValueType.Indexer;
					obj = this.Engine.GetValue(item, propertyDescriptor, indexerIsNext);
				}
				else if (dependencyProperty != null)
				{
					DependencyObject dependencyObject = (DependencyObject)item;
					if (level != this.Length - 1 || this._host == null || this._host.TransfersDefaultValue)
					{
						obj = dependencyObject.GetValue(dependencyProperty);
					}
					else if (!Helper.HasDefaultValue(dependencyObject, dependencyProperty))
					{
						obj = dependencyObject.GetValue(dependencyProperty);
					}
					else
					{
						obj = BindingExpression.IgnoreDefaultValue;
					}
				}
				else if (dynamicPropertyAccessor != null)
				{
					obj = dynamicPropertyAccessor.GetValue(item);
				}
				break;
			case SourceValueType.Indexer:
				if (propertyInfo != null)
				{
					object[] args = this._arySVS[level].args;
					PropertyPathWorker.IListIndexerArg listIndexerArg;
					if (args != null && args.Length == 1 && (listIndexerArg = (args[0] as PropertyPathWorker.IListIndexerArg)) != null)
					{
						int value = listIndexerArg.Value;
						IList list = (IList)item;
						if (0 <= value && value < list.Count)
						{
							obj = list[value];
						}
						else
						{
							obj = PropertyPathWorker.IListIndexOutOfRange;
						}
					}
					else
					{
						obj = propertyInfo.GetValue(item, BindingFlags.GetProperty, null, args, CultureInfo.InvariantCulture);
					}
				}
				else
				{
					DynamicIndexerAccessor dynamicIndexerAccessor;
					if ((dynamicIndexerAccessor = (this._arySVS[level].info as DynamicIndexerAccessor)) == null)
					{
						throw new NotSupportedException(SR.Get("IndexedPropDescNotImplemented"));
					}
					obj = dynamicIndexerAccessor.GetValue(item, this._arySVS[level].args);
				}
				break;
			case SourceValueType.Direct:
				obj = item;
				break;
			}
			if (flag)
			{
				object obj2 = this._arySVS[level].info;
				if (obj2 == DependencyProperty.UnsetValue)
				{
					obj2 = null;
				}
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetValue(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					level,
					TraceData.Identify(item),
					TraceData.IdentifyAccessor(obj2),
					TraceData.Identify(obj)
				}), this._host.ParentBindingExpression);
			}
			return obj;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x001561F8 File Offset: 0x001551F8
		internal void SetValue(object item, object value)
		{
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.CreateExpression);
			int num = this._arySVS.Length - 1;
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(this._arySVS[num].info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.SetValue(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					num,
					TraceData.Identify(item),
					TraceData.IdentifyAccessor(this._arySVS[num].info),
					TraceData.Identify(value)
				}), this._host.ParentBindingExpression);
			}
			SourceValueType type = this.SVI[num].type;
			if (type != SourceValueType.Property)
			{
				if (type != SourceValueType.Indexer)
				{
					return;
				}
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(item, value, BindingFlags.SetProperty, null, this.GetIndexerArguments(num), CultureInfo.InvariantCulture);
					return;
				}
				DynamicIndexerAccessor dynamicIndexerAccessor;
				if ((dynamicIndexerAccessor = (this._arySVS[num].info as DynamicIndexerAccessor)) != null)
				{
					dynamicIndexerAccessor.SetValue(item, this._arySVS[num].args, value);
					return;
				}
				throw new NotSupportedException(SR.Get("IndexedPropDescNotImplemented"));
			}
			else
			{
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(item, value);
					return;
				}
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(item, value, null);
					return;
				}
				if (dependencyProperty != null)
				{
					((DependencyObject)item).SetValue(dependencyProperty, value);
					return;
				}
				if (dynamicPropertyAccessor != null)
				{
					dynamicPropertyAccessor.SetValue(item, value);
					return;
				}
				return;
			}
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00156368 File Offset: 0x00155368
		internal object RawValue()
		{
			object obj = this.RawValue(this.Length - 1);
			if (obj == PropertyPathWorker.AsyncRequestPending)
			{
				obj = DependencyProperty.UnsetValue;
			}
			return obj;
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00156394 File Offset: 0x00155394
		internal void RefreshValue()
		{
			for (int i = 1; i < this._arySVS.Length; i++)
			{
				if (!ItemsControl.EqualsEx(BindingExpressionBase.GetReference(this._arySVS[i].item), this.RawValue(i - 1)))
				{
					this.UpdateSourceValueState(i - 1, null);
					return;
				}
			}
			this.UpdateSourceValueState(this.Length - 1, null);
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x001563F4 File Offset: 0x001553F4
		internal int LevelForPropertyChange(object item, string propertyName)
		{
			bool flag = propertyName == "Item[]";
			for (int i = 0; i < this._arySVS.Length; i++)
			{
				object obj = BindingExpressionBase.GetReference(this._arySVS[i].item);
				if (obj == BindingExpression.StaticSource)
				{
					obj = null;
				}
				if (obj == item && (string.IsNullOrEmpty(propertyName) || (flag && this.SVI[i].type == SourceValueType.Indexer) || string.Equals(this.SVI[i].propertyName, propertyName, StringComparison.OrdinalIgnoreCase)))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00156481 File Offset: 0x00155481
		internal void OnPropertyChangedAtLevel(int level)
		{
			this.UpdateSourceValueState(level, null);
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0015648C File Offset: 0x0015548C
		internal void OnCurrentChanged(ICollectionView collectionView)
		{
			for (int i = 0; i < this.Length; i++)
			{
				if (this._arySVS[i].collectionView == collectionView)
				{
					this._host.CancelPendingTasks();
					this.UpdateSourceValueState(i, collectionView);
					return;
				}
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x001564D4 File Offset: 0x001554D4
		internal bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			if (dp == DependencyObject.DirectDependencyProperty)
			{
				return true;
			}
			for (int i = 0; i < this._arySVS.Length; i++)
			{
				if (this._arySVS[i].info == dp && BindingExpressionBase.GetReference(this._arySVS[i].item) == d)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x00156530 File Offset: 0x00155530
		internal void OnDependencyPropertyChanged(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
			if (dp == DependencyObject.DirectDependencyProperty)
			{
				this.UpdateSourceValueState(this._arySVS.Length, null, BindingExpression.NullDataItem, isASubPropertyChange);
				return;
			}
			for (int i = 0; i < this._arySVS.Length; i++)
			{
				if (this._arySVS[i].info == dp && BindingExpressionBase.GetReference(this._arySVS[i].item) == d)
				{
					this.UpdateSourceValueState(i, null, BindingExpression.NullDataItem, isASubPropertyChange);
					return;
				}
			}
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x001565AA File Offset: 0x001555AA
		internal void OnNewValue(int level, object value)
		{
			this._status = PropertyPathStatus.Active;
			if (level < this.Length - 1)
			{
				this.UpdateSourceValueState(level, null, value, false);
			}
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x001565C8 File Offset: 0x001555C8
		internal SourceValueInfo GetSourceValueInfo(int level)
		{
			return this.SVI[level];
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x001565D8 File Offset: 0x001555D8
		internal static bool IsIndexedProperty(PropertyInfo pi)
		{
			bool result = false;
			try
			{
				result = (pi != null && pi.GetIndexParameters().Length != 0);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x0015661C File Offset: 0x0015561C
		private bool IsDynamic
		{
			get
			{
				return this._isDynamic;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x00156624 File Offset: 0x00155624
		private SourceValueInfo[] SVI
		{
			get
			{
				return this._parent.SVI;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x00156631 File Offset: 0x00155631
		private DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00156639 File Offset: 0x00155639
		private void UpdateSourceValueState(int k, ICollectionView collectionView)
		{
			this.UpdateSourceValueState(k, collectionView, BindingExpression.NullDataItem, false);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0015664C File Offset: 0x0015564C
		private void UpdateSourceValueState(int k, ICollectionView collectionView, object newValue, bool isASubPropertyChange)
		{
			DependencyObject dependencyObject = null;
			if (this._host != null)
			{
				dependencyObject = this._host.CheckTarget();
				if (this._rootItem != BindingExpression.NullDataItem && dependencyObject == null)
				{
					return;
				}
			}
			int num = k;
			bool flag = this._host == null || k < 0;
			this._status = PropertyPathStatus.Active;
			this._dependencySourcesChanged = false;
			if (collectionView != null)
			{
				this.ReplaceItem(k, collectionView.CurrentItem, PropertyPathWorker.NoParent);
			}
			for (k++; k < this._arySVS.Length; k++)
			{
				isASubPropertyChange = false;
				ICollectionView collectionView2 = this._arySVS[k].collectionView;
				object obj = (newValue == BindingExpression.NullDataItem) ? this.RawValue(k - 1) : newValue;
				newValue = BindingExpression.NullDataItem;
				if (obj == PropertyPathWorker.AsyncRequestPending)
				{
					this._status = PropertyPathStatus.AsyncRequestPending;
					break;
				}
				if (!flag && obj == BindingExpressionBase.DisconnectedItem && this._arySVS[k - 1].info == FrameworkElement.DataContextProperty)
				{
					flag = true;
				}
				this.ReplaceItem(k, BindingExpression.NullDataItem, obj);
				ICollectionView collectionView3 = this._arySVS[k].collectionView;
				if (collectionView2 != collectionView3 && this._host != null)
				{
					this._host.ReplaceCurrentItem(collectionView2, collectionView3);
				}
			}
			if (this._host != null)
			{
				if (num < this._arySVS.Length)
				{
					this.NeedsDirectNotification = (this._status == PropertyPathStatus.Active && this._arySVS.Length != 0 && this.SVI[this._arySVS.Length - 1].type != SourceValueType.Direct && !(this._arySVS[this._arySVS.Length - 1].info is DependencyProperty) && typeof(DependencyObject).IsAssignableFrom(this._arySVS[this._arySVS.Length - 1].type));
				}
				if (!flag && this._arySVS.Length != 0 && this._arySVS[this._arySVS.Length - 1].info == FrameworkElement.DataContextProperty && this.RawValue() == BindingExpressionBase.DisconnectedItem)
				{
					flag = true;
				}
				this._host.NewValueAvailable(this._dependencySourcesChanged, flag, isASubPropertyChange);
			}
			GC.KeepAlive(dependencyObject);
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0015686C File Offset: 0x0015586C
		private void ReplaceItem(int k, object newO, object parent)
		{
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.Transfer);
			PropertyPathWorker.SourceValueState sourceValueState = default(PropertyPathWorker.SourceValueState);
			object reference = BindingExpressionBase.GetReference(this._arySVS[k].item);
			if (this.IsDynamic && this.SVI[k].type != SourceValueType.Direct)
			{
				DependencyProperty dependencyProperty;
				PropertyInfo propertyInfo;
				PropertyDescriptor propertyDescriptor;
				DynamicObjectAccessor dynamicObjectAccessor;
				PropertyPath.DowncastAccessor(this._arySVS[k].info, out dependencyProperty, out propertyInfo, out propertyDescriptor, out dynamicObjectAccessor);
				INotifyPropertyChanged source;
				if (reference == BindingExpression.StaticSource)
				{
					Type type = (propertyInfo != null) ? propertyInfo.DeclaringType : ((propertyDescriptor != null) ? propertyDescriptor.ComponentType : null);
					if (type != null)
					{
						StaticPropertyChangedEventManager.RemoveHandler(type, new EventHandler<PropertyChangedEventArgs>(this.OnStaticPropertyChanged), this.SVI[k].propertyName);
					}
				}
				else if (dependencyProperty != null)
				{
					this._dependencySourcesChanged = true;
				}
				else if ((source = (reference as INotifyPropertyChanged)) != null)
				{
					PropertyChangedEventManager.RemoveHandler(source, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), this.SVI[k].propertyName);
				}
				else if (propertyDescriptor != null && reference != null)
				{
					ValueChangedEventManager.RemoveHandler(reference, new EventHandler<ValueChangedEventArgs>(this.OnValueChanged), propertyDescriptor);
				}
			}
			if (this._host != null && k == this.Length - 1 && this.IsDynamic && this._host.ValidatesOnNotifyDataErrors)
			{
				INotifyDataErrorInfo notifyDataErrorInfo = reference as INotifyDataErrorInfo;
				if (notifyDataErrorInfo != null)
				{
					ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
				}
			}
			this._isDBNullValidForUpdate = null;
			if (newO == null || parent == DependencyProperty.UnsetValue || parent == BindingExpression.NullDataItem || parent == BindingExpressionBase.DisconnectedItem)
			{
				this._arySVS[k].item = BindingExpressionBase.ReplaceReference(this._arySVS[k].item, newO);
				if (parent == DependencyProperty.UnsetValue || parent == BindingExpression.NullDataItem || parent == BindingExpressionBase.DisconnectedItem)
				{
					this._arySVS[k].collectionView = null;
				}
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ReplaceItemShort(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k,
						TraceData.Identify(newO)
					}), this._host.ParentBindingExpression);
				}
				return;
			}
			if (newO != BindingExpression.NullDataItem)
			{
				parent = newO;
				this.GetInfo(k, newO, ref sourceValueState);
				sourceValueState.collectionView = this._arySVS[k].collectionView;
			}
			else
			{
				DrillIn drillIn = this.SVI[k].drillIn;
				ICollectionView collectionView = null;
				if (drillIn != DrillIn.Always)
				{
					this.GetInfo(k, parent, ref sourceValueState);
				}
				if (sourceValueState.info == null)
				{
					collectionView = CollectionViewSource.GetDefaultCollectionView(parent, this.TreeContext, (object x) => BindingExpressionBase.GetReference((k == 0) ? this._rootItem : this._arySVS[k - 1].item));
					if (collectionView != null && drillIn != DrillIn.Always && collectionView != parent)
					{
						this.GetInfo(k, collectionView, ref sourceValueState);
					}
				}
				if (sourceValueState.info == null && drillIn != DrillIn.Never && collectionView != null)
				{
					newO = collectionView.CurrentItem;
					if (newO != null)
					{
						this.GetInfo(k, newO, ref sourceValueState);
						sourceValueState.collectionView = collectionView;
					}
					else
					{
						sourceValueState = this._arySVS[k];
						sourceValueState.collectionView = collectionView;
						if (!SystemXmlHelper.IsEmptyXmlDataCollection(parent))
						{
							sourceValueState.item = BindingExpressionBase.ReplaceReference(sourceValueState.item, BindingExpression.NullDataItem);
							if (sourceValueState.info == null)
							{
								sourceValueState.info = DependencyProperty.UnsetValue;
							}
						}
					}
				}
			}
			if (sourceValueState.info == null)
			{
				sourceValueState.item = BindingExpressionBase.ReplaceReference(sourceValueState.item, BindingExpression.NullDataItem);
				this._arySVS[k] = sourceValueState;
				this._status = PropertyPathStatus.PathError;
				this.ReportNoInfoError(k, parent);
				return;
			}
			this._arySVS[k] = sourceValueState;
			newO = BindingExpressionBase.GetReference(sourceValueState.item);
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ReplaceItemLong(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					k,
					TraceData.Identify(newO),
					TraceData.IdentifyAccessor(sourceValueState.info)
				}), this._host.ParentBindingExpression);
			}
			if (this.IsDynamic && this.SVI[k].type != SourceValueType.Direct)
			{
				this.Engine.RegisterForCacheChanges(newO, sourceValueState.info);
				DependencyProperty dependencyProperty2;
				PropertyInfo propertyInfo2;
				PropertyDescriptor propertyDescriptor2;
				DynamicObjectAccessor dynamicObjectAccessor2;
				PropertyPath.DowncastAccessor(sourceValueState.info, out dependencyProperty2, out propertyInfo2, out propertyDescriptor2, out dynamicObjectAccessor2);
				INotifyPropertyChanged source2;
				if (newO == BindingExpression.StaticSource)
				{
					Type type2 = (propertyInfo2 != null) ? propertyInfo2.DeclaringType : ((propertyDescriptor2 != null) ? propertyDescriptor2.ComponentType : null);
					if (type2 != null)
					{
						StaticPropertyChangedEventManager.AddHandler(type2, new EventHandler<PropertyChangedEventArgs>(this.OnStaticPropertyChanged), this.SVI[k].propertyName);
					}
				}
				else if (dependencyProperty2 != null)
				{
					this._dependencySourcesChanged = true;
				}
				else if ((source2 = (newO as INotifyPropertyChanged)) != null)
				{
					PropertyChangedEventManager.AddHandler(source2, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), this.SVI[k].propertyName);
				}
				else if (propertyDescriptor2 != null && newO != null)
				{
					ValueChangedEventManager.AddHandler(newO, new EventHandler<ValueChangedEventArgs>(this.OnValueChanged), propertyDescriptor2);
				}
			}
			if (this._host != null && k == this.Length - 1)
			{
				this._host.SetupDefaultValueConverter(sourceValueState.type);
				if (this._host.IsReflective)
				{
					this.CheckReadOnly(newO, sourceValueState.info);
				}
				if (this._host.ValidatesOnNotifyDataErrors)
				{
					INotifyDataErrorInfo notifyDataErrorInfo2 = newO as INotifyDataErrorInfo;
					if (notifyDataErrorInfo2 != null)
					{
						if (this.IsDynamic)
						{
							ErrorsChangedEventManager.AddHandler(notifyDataErrorInfo2, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
						}
						this._host.OnDataErrorsChanged(notifyDataErrorInfo2, this.SourcePropertyName);
					}
				}
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00156E6C File Offset: 0x00155E6C
		private void ReportNoInfoError(int k, object parent)
		{
			if (TraceData.IsEnabled)
			{
				BindingExpression bindingExpression = (this._host != null) ? this._host.ParentBindingExpression : null;
				if (bindingExpression == null || !bindingExpression.IsInPriorityBindingExpression)
				{
					if (!SystemXmlHelper.IsEmptyXmlDataCollection(parent))
					{
						SourceValueInfo sourceValueInfo = this.SVI[k];
						bool flag = sourceValueInfo.drillIn == DrillIn.Always;
						string text = (sourceValueInfo.type != SourceValueType.Indexer) ? sourceValueInfo.name : ("[" + sourceValueInfo.name + "]");
						string text2 = TraceData.DescribeSourceObject(parent);
						string text3 = flag ? "current item of collection" : "object";
						if (parent == null)
						{
							TraceData.TraceAndNotify(TraceEventType.Information, TraceData.NullItem(new object[]
							{
								text,
								text3
							}), bindingExpression, null);
							return;
						}
						if (parent == CollectionView.NewItemPlaceholder || parent == DataGrid.NewItemPlaceholder)
						{
							TraceData.TraceAndNotify(TraceEventType.Information, TraceData.PlaceholderItem(new object[]
							{
								text,
								text3
							}), bindingExpression, null);
							return;
						}
						TraceData.TraceAndNotify((bindingExpression != null) ? bindingExpression.TraceLevel : TraceEventType.Error, TraceData.ClrReplaceItem(new object[]
						{
							text,
							text2,
							text3
						}), bindingExpression, new object[]
						{
							bindingExpression
						}, new object[]
						{
							text,
							parent,
							flag
						});
						return;
					}
					else
					{
						TraceEventType traceType = (bindingExpression != null) ? bindingExpression.TraceLevel : TraceEventType.Error;
						this._host.ReportBadXPath(traceType);
					}
				}
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00156FC0 File Offset: 0x00155FC0
		internal bool IsPathCurrent(object rootItem)
		{
			if (this.Status != PropertyPathStatus.Active)
			{
				return false;
			}
			object obj = rootItem;
			int i = 0;
			int length = this.Length;
			while (i < length)
			{
				ICollectionView collectionView = this._arySVS[i].collectionView;
				if (collectionView != null)
				{
					obj = collectionView.CurrentItem;
				}
				if (PropertyPath.IsStaticProperty(this._arySVS[i].info))
				{
					obj = BindingExpression.StaticSource;
				}
				if (!ItemsControl.EqualsEx(obj, BindingExpressionBase.GetReference(this._arySVS[i].item)) && !this.IsNonIdempotentProperty(i - 1))
				{
					return false;
				}
				if (i < length - 1)
				{
					obj = this.GetValue(obj, i);
				}
				i++;
			}
			return true;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00157064 File Offset: 0x00156064
		private bool IsNonIdempotentProperty(int level)
		{
			PropertyDescriptor pd;
			return level >= 0 && (pd = (this._arySVS[level].info as PropertyDescriptor)) != null && SystemXmlLinqHelper.IsXLinqNonIdempotentProperty(pd);
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00157098 File Offset: 0x00156098
		private void GetInfo(int k, object item, ref PropertyPathWorker.SourceValueState svs)
		{
			object reference = BindingExpressionBase.GetReference(this._arySVS[k].item);
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.Transfer);
			Type reflectionType = ReflectionHelper.GetReflectionType(reference);
			Type reflectionType2 = ReflectionHelper.GetReflectionType(item);
			Type type = null;
			if (reflectionType2 == reflectionType && reference != BindingExpression.NullDataItem && !(this._arySVS[k].info is PropertyDescriptor))
			{
				svs = this._arySVS[k];
				svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetInfo_Reuse(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k,
						TraceData.IdentifyAccessor(svs.info)
					}), this._host.ParentBindingExpression);
				}
				return;
			}
			if (reflectionType2 == null && this.SVI[k].type != SourceValueType.Direct)
			{
				svs.info = null;
				svs.args = null;
				svs.type = null;
				svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetInfo_Null(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k
					}), this._host.ParentBindingExpression);
				}
				return;
			}
			int num;
			bool flag2 = !PropertyPath.IsParameterIndex(this.SVI[k].name, out num);
			if (flag2)
			{
				AccessorInfo accessorInfo = this.Engine.AccessorTable[this.SVI[k].type, reflectionType2, this.SVI[k].name];
				if (accessorInfo != null)
				{
					svs.info = accessorInfo.Accessor;
					svs.type = accessorInfo.PropertyType;
					svs.args = accessorInfo.Args;
					if (PropertyPath.IsStaticProperty(svs.info))
					{
						item = BindingExpression.StaticSource;
					}
					svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
					if (this.IsDynamic && this.SVI[k].type == SourceValueType.Property && svs.info is DependencyProperty)
					{
						this._dependencySourcesChanged = true;
					}
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetInfo_Cache(new object[]
						{
							TraceData.Identify(this._host.ParentBindingExpression),
							k,
							reflectionType2.Name,
							this.SVI[k].name,
							TraceData.IdentifyAccessor(svs.info)
						}), this._host.ParentBindingExpression);
					}
					return;
				}
			}
			object obj = null;
			object[] array = null;
			switch (this.SVI[k].type)
			{
			case SourceValueType.Property:
				break;
			case SourceValueType.Indexer:
			{
				IndexerParameterInfo[] array2 = this._parent.ResolveIndexerParams(k, this.TreeContext);
				if (array2.Length == 1 && (array2[0].type == null || array2[0].type == typeof(string)))
				{
					string name = (string)array2[0].value;
					if (this.ShouldConvertIndexerToProperty(item, ref name))
					{
						this._parent.ReplaceIndexerByProperty(k, name);
						break;
					}
				}
				array = new object[array2.Length];
				MemberInfo[][] array3 = new MemberInfo[2][];
				array3[0] = this.GetIndexers(reflectionType2, k);
				MemberInfo[][] array4 = array3;
				bool flag3 = item is IList;
				if (flag3)
				{
					array4[1] = typeof(IList).GetDefaultMembers();
				}
				int num2 = 0;
				while (obj == null && num2 < array4.Length)
				{
					if (array4[num2] != null)
					{
						MemberInfo[] array5 = array4[num2];
						int i = 0;
						while (i < array5.Length)
						{
							PropertyInfo propertyInfo = array5[i] as PropertyInfo;
							if (propertyInfo != null && this.MatchIndexerParameters(propertyInfo, array2, array, flag3))
							{
								obj = propertyInfo;
								type = reflectionType2.GetElementType();
								if (type == null)
								{
									type = propertyInfo.PropertyType;
									break;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
					num2++;
				}
				if (obj == null && SystemCoreHelper.IsIDynamicMetaObjectProvider(item) && this.MatchIndexerParameters(null, array2, array, false))
				{
					obj = SystemCoreHelper.GetIndexerAccessor(array.Length);
					type = typeof(object);
				}
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetInfo_Indexer(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k,
						reflectionType2.Name,
						this.SVI[k].name,
						TraceData.IdentifyAccessor(obj)
					}), this._host.ParentBindingExpression);
					goto IL_5D2;
				}
				goto IL_5D2;
			}
			case SourceValueType.Direct:
				if (item is ICollectionView && this._host != null && !this._host.IsValidValue(item))
				{
					goto IL_5D2;
				}
				obj = DependencyProperty.UnsetValue;
				type = reflectionType2;
				if (this.Length == 1 && item is Freezable && item != this.TreeContext)
				{
					obj = DependencyObject.DirectDependencyProperty;
					this._dependencySourcesChanged = true;
					goto IL_5D2;
				}
				goto IL_5D2;
			default:
				goto IL_5D2;
			}
			obj = this._parent.ResolvePropertyName(k, item, reflectionType2, this.TreeContext);
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetInfo_Property(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					k,
					reflectionType2.Name,
					this.SVI[k].name,
					TraceData.IdentifyAccessor(obj)
				}), this._host.ParentBindingExpression);
			}
			DependencyProperty dependencyProperty;
			PropertyInfo propertyInfo2;
			PropertyDescriptor propertyDescriptor;
			DynamicObjectAccessor dynamicObjectAccessor;
			PropertyPath.DowncastAccessor(obj, out dependencyProperty, out propertyInfo2, out propertyDescriptor, out dynamicObjectAccessor);
			if (dependencyProperty != null)
			{
				type = dependencyProperty.PropertyType;
				if (this.IsDynamic)
				{
					this._dependencySourcesChanged = true;
				}
			}
			else if (propertyInfo2 != null)
			{
				type = propertyInfo2.PropertyType;
			}
			else if (propertyDescriptor != null)
			{
				type = propertyDescriptor.PropertyType;
			}
			else if (dynamicObjectAccessor != null)
			{
				type = dynamicObjectAccessor.PropertyType;
			}
			IL_5D2:
			if (PropertyPath.IsStaticProperty(obj))
			{
				item = BindingExpression.StaticSource;
			}
			svs.info = obj;
			svs.args = array;
			svs.type = type;
			svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
			if (flag2 && obj != null && !(obj is PropertyDescriptor))
			{
				this.Engine.AccessorTable[this.SVI[k].type, reflectionType2, this.SVI[k].name] = new AccessorInfo(obj, type, array);
			}
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00157700 File Offset: 0x00156700
		private MemberInfo[] GetIndexers(Type type, int k)
		{
			if (k > 0 && this._arySVS[k - 1].info == IndexerPropertyInfo.Instance)
			{
				List<MemberInfo> list = new List<MemberInfo>();
				string name = this.SVI[k - 1].name;
				foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
				{
					if (propertyInfo.Name == name && PropertyPathWorker.IsIndexedProperty(propertyInfo))
					{
						list.Add(propertyInfo);
					}
				}
				return list.ToArray();
			}
			return type.GetDefaultMembers();
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00157790 File Offset: 0x00156790
		private bool MatchIndexerParameters(PropertyInfo pi, IndexerParameterInfo[] aryInfo, object[] args, bool isIList)
		{
			ParameterInfo[] array = (pi != null) ? pi.GetIndexParameters() : null;
			if (array != null && array.Length != aryInfo.Length)
			{
				return false;
			}
			for (int i = 0; i < args.Length; i++)
			{
				IndexerParameterInfo indexerParameterInfo = aryInfo[i];
				Type type = (array != null) ? array[i].ParameterType : typeof(object);
				if (indexerParameterInfo.type != null)
				{
					if (!type.IsAssignableFrom(indexerParameterInfo.type))
					{
						return false;
					}
					args.SetValue(indexerParameterInfo.value, i);
				}
				else
				{
					try
					{
						object obj = null;
						if (type == typeof(int))
						{
							int num;
							if (int.TryParse((string)indexerParameterInfo.value, NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out num))
							{
								obj = num;
							}
						}
						else
						{
							TypeConverter converter = TypeDescriptor.GetConverter(type);
							if (converter != null && converter.CanConvertFrom(typeof(string)))
							{
								obj = converter.ConvertFromString(null, TypeConverterHelper.InvariantEnglishUS, (string)indexerParameterInfo.value);
							}
						}
						if (obj == null && type.IsAssignableFrom(typeof(string)))
						{
							obj = indexerParameterInfo.value;
						}
						if (obj == null)
						{
							return false;
						}
						args.SetValue(obj, i);
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex))
						{
							throw;
						}
						return false;
					}
					catch
					{
						return false;
					}
				}
			}
			if (isIList && array.Length == 1 && array[0].ParameterType == typeof(int))
			{
				bool flag = true;
				if (!FrameworkAppContextSwitches.IListIndexerHidesCustomIndexer)
				{
					Type type2 = pi.DeclaringType;
					if (type2.IsGenericType)
					{
						type2 = type2.GetGenericTypeDefinition();
					}
					flag = PropertyPathWorker.IListIndexerWhitelist.Contains(type2);
				}
				if (flag)
				{
					args[0] = new PropertyPathWorker.IListIndexerArg((int)args[0]);
				}
			}
			return true;
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00157968 File Offset: 0x00156968
		private bool ShouldConvertIndexerToProperty(object item, ref string name)
		{
			if (SystemDataHelper.IsDataRowView(item))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
				if (properties[name] != null)
				{
					return true;
				}
				int num;
				if (int.TryParse(name, NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out num) && 0 <= num && num < properties.Count)
				{
					name = properties[num].Name;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x001579C4 File Offset: 0x001569C4
		private object RawValue(int k)
		{
			if (k < 0)
			{
				return BindingExpressionBase.GetReference(this._rootItem);
			}
			if (k >= this._arySVS.Length)
			{
				return DependencyProperty.UnsetValue;
			}
			object reference = BindingExpressionBase.GetReference(this._arySVS[k].item);
			object info = this._arySVS[k].info;
			if (reference == BindingExpression.NullDataItem || info == null || (reference == null && info != DependencyProperty.UnsetValue))
			{
				if (this._host != null)
				{
					this._host.ReportRawValueErrors(k, reference, info);
				}
				return DependencyProperty.UnsetValue;
			}
			object obj = DependencyProperty.UnsetValue;
			if (!(info is DependencyProperty) && this.SVI[k].type != SourceValueType.Direct && this._host != null && this._host.AsyncGet(reference, k))
			{
				this._status = PropertyPathStatus.AsyncRequestPending;
				return PropertyPathWorker.AsyncRequestPending;
			}
			try
			{
				obj = this.GetValue(reference, k);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				BindingOperations.LogException(ex);
				if (this._host != null)
				{
					this._host.ReportGetValueError(k, reference, ex);
				}
			}
			catch
			{
				if (this._host != null)
				{
					this._host.ReportGetValueError(k, reference, new InvalidOperationException(SR.Get("NonCLSException", new object[]
					{
						"GetValue"
					})));
				}
			}
			if (obj == PropertyPathWorker.IListIndexOutOfRange)
			{
				obj = DependencyProperty.UnsetValue;
				if (this._host != null)
				{
					this._host.ReportGetValueError(k, reference, new ArgumentOutOfRangeException("index"));
				}
			}
			return obj;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00157B50 File Offset: 0x00156B50
		private void SetPropertyInfo(object info, out PropertyInfo pi, out PropertyDescriptor pd, out DependencyProperty dp, out DynamicPropertyAccessor dpa)
		{
			pi = null;
			pd = null;
			dpa = null;
			dp = (info as DependencyProperty);
			if (dp == null)
			{
				pi = (info as PropertyInfo);
				if (pi == null)
				{
					pd = (info as PropertyDescriptor);
					if (pd == null)
					{
						dpa = (info as DynamicPropertyAccessor);
					}
				}
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00157B9C File Offset: 0x00156B9C
		private void CheckReadOnly(object item, object info)
		{
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			if (propertyInfo != null)
			{
				if (this.IsPropertyReadOnly(item, propertyInfo))
				{
					throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
					{
						item.GetType(),
						propertyInfo.Name
					}));
				}
			}
			else if (propertyDescriptor != null)
			{
				if (propertyDescriptor.IsReadOnly)
				{
					throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
					{
						item.GetType(),
						propertyDescriptor.Name
					}));
				}
			}
			else if (dependencyProperty != null)
			{
				if (dependencyProperty.ReadOnly)
				{
					throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
					{
						item.GetType(),
						dependencyProperty.Name
					}));
				}
			}
			else if (dynamicPropertyAccessor != null && dynamicPropertyAccessor.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
				{
					item.GetType(),
					dynamicPropertyAccessor.PropertyName
				}));
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00157C94 File Offset: 0x00156C94
		private bool IsPropertyReadOnly(object item, PropertyInfo pi)
		{
			if (!pi.CanWrite)
			{
				return true;
			}
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = pi.GetSetMethod(true);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
			}
			return !(methodInfo == null) && !methodInfo.IsPublic;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00157CE8 File Offset: 0x00156CE8
		private void DetermineWhetherDBNullIsValid()
		{
			bool value = false;
			object item = this.GetItem(this.Length - 1);
			if (item != null && AssemblyHelper.IsLoaded(UncommonAssembly.System_Data_Common))
			{
				value = this.DetermineWhetherDBNullIsValid(item);
			}
			this._isDBNullValidForUpdate = new bool?(value);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00157D28 File Offset: 0x00156D28
		private bool DetermineWhetherDBNullIsValid(object item)
		{
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(this._arySVS[this.Length - 1].info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			string text = (propertyDescriptor != null) ? propertyDescriptor.Name : ((propertyInfo != null) ? propertyInfo.Name : null);
			object arg = (text == "Item" && propertyInfo != null) ? this._arySVS[this.Length - 1].args[0] : null;
			return SystemDataHelper.DetermineWhetherDBNullIsValid(item, text, arg);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00157DBC File Offset: 0x00156DBC
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsExtendedTraceEnabled(TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					"PropertyChanged",
					TraceData.Identify(sender)
				}), this._host.ParentBindingExpression);
			}
			this._host.OnSourcePropertyChanged(sender, e.PropertyName);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00157E24 File Offset: 0x00156E24
		private void OnValueChanged(object sender, ValueChangedEventArgs e)
		{
			if (this.IsExtendedTraceEnabled(TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					"ValueChanged",
					TraceData.Identify(sender)
				}), this._host.ParentBindingExpression);
			}
			this._host.OnSourcePropertyChanged(sender, e.PropertyDescriptor.Name);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00157E91 File Offset: 0x00156E91
		private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			if (e.PropertyName == this.SourcePropertyName)
			{
				this._host.OnDataErrorsChanged((INotifyDataErrorInfo)sender, e.PropertyName);
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00157EC0 File Offset: 0x00156EC0
		private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsExtendedTraceEnabled(TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					"PropertyChanged",
					"(static)"
				}), this._host.ParentBindingExpression);
			}
			this._host.OnSourcePropertyChanged(sender, e.PropertyName);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00157F27 File Offset: 0x00156F27
		private bool IsExtendedTraceEnabled(TraceDataLevel level)
		{
			return this._host != null && TraceData.IsExtendedTraceEnabled(this._host.ParentBindingExpression, level);
		}

		// Token: 0x04000C32 RID: 3122
		private static readonly char[] s_comma = new char[]
		{
			','
		};

		// Token: 0x04000C33 RID: 3123
		private static readonly char[] s_dot = new char[]
		{
			'.'
		};

		// Token: 0x04000C34 RID: 3124
		private static readonly object NoParent = new NamedObject("NoParent");

		// Token: 0x04000C35 RID: 3125
		private static readonly object AsyncRequestPending = new NamedObject("AsyncRequestPending");

		// Token: 0x04000C36 RID: 3126
		internal static readonly object IListIndexOutOfRange = new NamedObject("IListIndexOutOfRange");

		// Token: 0x04000C37 RID: 3127
		private static readonly IList<Type> IListIndexerWhitelist = new Type[]
		{
			typeof(ArrayList),
			typeof(IList),
			typeof(List<>),
			typeof(Collection<>),
			typeof(ReadOnlyCollection<>),
			typeof(StringCollection),
			typeof(LinkTargetCollection)
		};

		// Token: 0x04000C38 RID: 3128
		private PropertyPath _parent;

		// Token: 0x04000C39 RID: 3129
		private PropertyPathStatus _status;

		// Token: 0x04000C3A RID: 3130
		private object _treeContext;

		// Token: 0x04000C3B RID: 3131
		private object _rootItem;

		// Token: 0x04000C3C RID: 3132
		private PropertyPathWorker.SourceValueState[] _arySVS;

		// Token: 0x04000C3D RID: 3133
		private PropertyPathWorker.ContextHelper _contextHelper;

		// Token: 0x04000C3E RID: 3134
		private ClrBindingWorker _host;

		// Token: 0x04000C3F RID: 3135
		private DataBindEngine _engine;

		// Token: 0x04000C40 RID: 3136
		private bool _dependencySourcesChanged;

		// Token: 0x04000C41 RID: 3137
		private bool _isDynamic;

		// Token: 0x04000C42 RID: 3138
		private bool _needsDirectNotification;

		// Token: 0x04000C43 RID: 3139
		private bool? _isDBNullValidForUpdate;

		// Token: 0x020009F9 RID: 2553
		private class ContextHelper : IDisposable
		{
			// Token: 0x06008466 RID: 33894 RVA: 0x00325AEB File Offset: 0x00324AEB
			public ContextHelper(PropertyPathWorker owner)
			{
				this._owner = owner;
			}

			// Token: 0x06008467 RID: 33895 RVA: 0x00325AFA File Offset: 0x00324AFA
			public void SetContext(object rootItem)
			{
				this._owner.TreeContext = (rootItem as DependencyObject);
				this._owner.AttachToRootItem(rootItem);
			}

			// Token: 0x06008468 RID: 33896 RVA: 0x00325B19 File Offset: 0x00324B19
			void IDisposable.Dispose()
			{
				this._owner.DetachFromRootItem();
				this._owner.TreeContext = null;
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004038 RID: 16440
			private PropertyPathWorker _owner;
		}

		// Token: 0x020009FA RID: 2554
		private class IListIndexerArg
		{
			// Token: 0x06008469 RID: 33897 RVA: 0x00325B38 File Offset: 0x00324B38
			public IListIndexerArg(int arg)
			{
				this._arg = arg;
			}

			// Token: 0x17001DBD RID: 7613
			// (get) Token: 0x0600846A RID: 33898 RVA: 0x00325B47 File Offset: 0x00324B47
			public int Value
			{
				get
				{
					return this._arg;
				}
			}

			// Token: 0x04004039 RID: 16441
			private int _arg;
		}

		// Token: 0x020009FB RID: 2555
		private struct SourceValueState
		{
			// Token: 0x0400403A RID: 16442
			public ICollectionView collectionView;

			// Token: 0x0400403B RID: 16443
			public object item;

			// Token: 0x0400403C RID: 16444
			public object info;

			// Token: 0x0400403D RID: 16445
			public Type type;

			// Token: 0x0400403E RID: 16446
			public object[] args;
		}
	}
}
