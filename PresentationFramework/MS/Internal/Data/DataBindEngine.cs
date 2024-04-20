using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000213 RID: 531
	internal class DataBindEngine : DispatcherObject
	{
		// Token: 0x0600141A RID: 5146 RVA: 0x00150A38 File Offset: 0x0014FA38
		private DataBindEngine()
		{
			new DataBindEngine.DataBindEngineShutDownListener(this);
			this._head = new DataBindEngine.Task(null, TaskOps.TransferValue, null);
			this._tail = this._head;
			this._mostRecentTask = new HybridDictionary();
			this._cleanupHelper = new CleanupHelper(new Func<bool, bool>(this.DoCleanup), 400, 10000, 5000);
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x00150B07 File Offset: 0x0014FB07
		internal PathParser PathParser
		{
			get
			{
				return this._pathParser;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x00150B0F File Offset: 0x0014FB0F
		internal ValueConverterContext ValueConverterContext
		{
			get
			{
				return this._valueConverterContext;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x00150B17 File Offset: 0x0014FB17
		internal AccessorTable AccessorTable
		{
			get
			{
				return this._accessorTable;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x00150B1F File Offset: 0x0014FB1F
		internal bool IsShutDown
		{
			get
			{
				return this._viewManager == null;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x00150B2A File Offset: 0x0014FB2A
		// (set) Token: 0x06001420 RID: 5152 RVA: 0x00150B32 File Offset: 0x0014FB32
		internal bool CleanupEnabled
		{
			get
			{
				return this._cleanupEnabled;
			}
			set
			{
				this._cleanupEnabled = value;
				WeakEventManager.SetCleanupEnabled(value);
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x00150B41 File Offset: 0x0014FB41
		internal IAsyncDataDispatcher AsyncDataDispatcher
		{
			get
			{
				if (this._defaultAsyncDataDispatcher == null)
				{
					this._defaultAsyncDataDispatcher = new DefaultAsyncDataDispatcher();
				}
				return this._defaultAsyncDataDispatcher;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00150B5C File Offset: 0x0014FB5C
		internal static DataBindEngine CurrentDataBindEngine
		{
			get
			{
				if (DataBindEngine._currentEngine == null)
				{
					DataBindEngine._currentEngine = new DataBindEngine();
				}
				return DataBindEngine._currentEngine;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x00150B74 File Offset: 0x0014FB74
		internal ViewManager ViewManager
		{
			get
			{
				return this._viewManager;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00150B7C File Offset: 0x0014FB7C
		internal CommitManager CommitManager
		{
			get
			{
				if (!this._commitManager.IsEmpty)
				{
					this.ScheduleCleanup();
				}
				return this._commitManager;
			}
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00150B98 File Offset: 0x0014FB98
		internal void AddTask(IDataBindEngineClient c, TaskOps op)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			if (this._head == this._tail)
			{
				this.RequestRun();
			}
			DataBindEngine.Task previousForClient = (DataBindEngine.Task)this._mostRecentTask[c];
			DataBindEngine.Task task = new DataBindEngine.Task(c, op, previousForClient);
			this._tail.Next = task;
			this._tail = task;
			this._mostRecentTask[c] = task;
			if (op == TaskOps.AttachToContext && this._layoutElement == null && (this._layoutElement = (c.TargetElement as UIElement)) != null)
			{
				this._layoutElement.LayoutUpdated += this.OnLayoutUpdated;
			}
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00150C38 File Offset: 0x0014FC38
		internal void CancelTask(IDataBindEngineClient c, TaskOps op)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			for (DataBindEngine.Task task = (DataBindEngine.Task)this._mostRecentTask[c]; task != null; task = task.PreviousForClient)
			{
				if (task.op == op && task.status == DataBindEngine.Task.Status.Pending)
				{
					task.status = DataBindEngine.Task.Status.Cancelled;
					return;
				}
			}
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00150C88 File Offset: 0x0014FC88
		internal void CancelTasks(IDataBindEngineClient c)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			for (DataBindEngine.Task task = (DataBindEngine.Task)this._mostRecentTask[c]; task != null; task = task.PreviousForClient)
			{
				Invariant.Assert(task.client == c, "task list is corrupt");
				if (task.status == DataBindEngine.Task.Status.Pending)
				{
					task.status = DataBindEngine.Task.Status.Cancelled;
				}
			}
			this._mostRecentTask.Remove(c);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00150CEC File Offset: 0x0014FCEC
		internal object Run(object arg)
		{
			bool flag = (bool)arg;
			DataBindEngine.Task task = flag ? null : new DataBindEngine.Task(null, TaskOps.TransferValue, null);
			DataBindEngine.Task task2 = task;
			if (this._layoutElement != null)
			{
				this._layoutElement.LayoutUpdated -= this.OnLayoutUpdated;
				this._layoutElement = null;
			}
			if (this.IsShutDown)
			{
				return null;
			}
			DataBindEngine.Task next;
			for (DataBindEngine.Task task3 = this._head.Next; task3 != null; task3 = next)
			{
				task3.PreviousForClient = null;
				if (task3.status == DataBindEngine.Task.Status.Pending)
				{
					task3.Run(flag);
					next = task3.Next;
					if (task3.status == DataBindEngine.Task.Status.Retry && !flag)
					{
						task3.status = DataBindEngine.Task.Status.Pending;
						task2.Next = task3;
						task2 = task3;
						task2.Next = null;
					}
				}
				else
				{
					next = task3.Next;
				}
			}
			this._head.Next = null;
			this._tail = this._head;
			this._mostRecentTask.Clear();
			if (!flag)
			{
				DataBindEngine.Task head = this._head;
				this._head = null;
				for (DataBindEngine.Task next2 = task.Next; next2 != null; next2 = next2.Next)
				{
					this.AddTask(next2.client, next2.op);
				}
				this._head = head;
			}
			return null;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00150E14 File Offset: 0x0014FE14
		internal ViewRecord GetViewRecord(object collection, CollectionViewSource key, Type collectionViewType, bool createView, Func<object, object> GetSourceItem)
		{
			if (this.IsShutDown)
			{
				return null;
			}
			ViewRecord viewRecord = this._viewManager.GetViewRecord(collection, key, collectionViewType, createView, GetSourceItem);
			if (viewRecord != null && !viewRecord.IsInitialized)
			{
				this.ScheduleCleanup();
			}
			return viewRecord;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00150E50 File Offset: 0x0014FE50
		internal void RegisterCollectionSynchronizationCallback(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			this._viewManager.RegisterCollectionSynchronizationCallback(collection, context, synchronizationCallback);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00150E60 File Offset: 0x0014FE60
		internal IValueConverter GetDefaultValueConverter(Type sourceType, Type targetType, bool targetToSource)
		{
			IValueConverter valueConverter = this._valueConverterTable[sourceType, targetType, targetToSource];
			if (valueConverter == null)
			{
				valueConverter = DefaultValueConverter.Create(sourceType, targetType, targetToSource, this);
				if (valueConverter != null)
				{
					this._valueConverterTable.Add(sourceType, targetType, targetToSource, valueConverter);
				}
			}
			return valueConverter;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00150E9C File Offset: 0x0014FE9C
		internal void AddAsyncRequest(DependencyObject target, AsyncDataRequest request)
		{
			if (target == null)
			{
				return;
			}
			IAsyncDataDispatcher asyncDataDispatcher = this.AsyncDataDispatcher;
			if (this._asyncDispatchers == null)
			{
				this._asyncDispatchers = new HybridDictionary(1);
			}
			this._asyncDispatchers[asyncDataDispatcher] = null;
			asyncDataDispatcher.AddRequest(request);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x00150EDC File Offset: 0x0014FEDC
		internal object GetValue(object item, PropertyDescriptor pd, bool indexerIsNext)
		{
			return this._valueTable.GetValue(item, pd, indexerIsNext);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00150EEC File Offset: 0x0014FEEC
		internal void RegisterForCacheChanges(object item, object descriptor)
		{
			PropertyDescriptor propertyDescriptor = descriptor as PropertyDescriptor;
			if (item != null && propertyDescriptor != null && ValueTable.ShouldCache(item, propertyDescriptor))
			{
				this._valueTable.RegisterForChanges(item, propertyDescriptor, this);
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x00150F1D File Offset: 0x0014FF1D
		internal void ScheduleCleanup()
		{
			if (!BaseAppContextSwitches.EnableCleanupSchedulingImprovements)
			{
				if (Interlocked.Increment(ref this._cleanupRequests) == 1)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(this.CleanupOperation), null);
					return;
				}
			}
			else
			{
				this._cleanupHelper.ScheduleCleanup();
			}
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00150F5A File Offset: 0x0014FF5A
		private bool DoCleanup(bool forceCleanup)
		{
			return (this.CleanupEnabled || forceCleanup) && this.DoCleanup();
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x00150F6E File Offset: 0x0014FF6E
		internal bool Cleanup()
		{
			if (!BaseAppContextSwitches.EnableCleanupSchedulingImprovements)
			{
				return this.DoCleanup();
			}
			return this._cleanupHelper.DoCleanup(true);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00150F8C File Offset: 0x0014FF8C
		private bool DoCleanup()
		{
			bool flag = false;
			if (!this.IsShutDown)
			{
				flag = (this._viewManager.Purge() || flag);
				flag = (WeakEventManager.Cleanup() || flag);
				flag = (this._valueTable.Purge() || flag);
				flag = (this._commitManager.Purge() || flag);
			}
			return flag;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00150FD8 File Offset: 0x0014FFD8
		internal DataBindOperation Marshal(DispatcherOperationCallback method, object arg, int cost = 1)
		{
			DataBindOperation dataBindOperation = new DataBindOperation(method, arg, cost);
			object crossThreadQueueLock = this._crossThreadQueueLock;
			lock (crossThreadQueueLock)
			{
				this._crossThreadQueue.Enqueue(dataBindOperation);
				this._crossThreadCost += cost;
				if (this._crossThreadDispatcherOperation == null)
				{
					this._crossThreadDispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.ProcessCrossThreadRequests));
				}
			}
			return dataBindOperation;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0015105C File Offset: 0x0015005C
		internal void ChangeCost(DataBindOperation op, int delta)
		{
			object crossThreadQueueLock = this._crossThreadQueueLock;
			lock (crossThreadQueueLock)
			{
				op.Cost += delta;
				this._crossThreadCost += delta;
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x001510B4 File Offset: 0x001500B4
		private void ProcessCrossThreadRequests()
		{
			if (this.IsShutDown)
			{
				return;
			}
			try
			{
				long ticks = DateTime.Now.Ticks;
				do
				{
					object crossThreadQueueLock = this._crossThreadQueueLock;
					DataBindOperation dataBindOperation;
					lock (crossThreadQueueLock)
					{
						if (this._crossThreadQueue.Count > 0)
						{
							dataBindOperation = this._crossThreadQueue.Dequeue();
							this._crossThreadCost -= dataBindOperation.Cost;
						}
						else
						{
							dataBindOperation = null;
						}
					}
					if (dataBindOperation == null)
					{
						break;
					}
					dataBindOperation.Invoke();
				}
				while (DateTime.Now.Ticks - ticks <= 50000L);
			}
			finally
			{
				object crossThreadQueueLock = this._crossThreadQueueLock;
				lock (crossThreadQueueLock)
				{
					if (this._crossThreadQueue.Count > 0)
					{
						this._crossThreadDispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.ProcessCrossThreadRequests));
					}
					else
					{
						this._crossThreadDispatcherOperation = null;
						this._crossThreadCost = 0;
					}
				}
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x001511D4 File Offset: 0x001501D4
		private void RequestRun()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new DispatcherOperationCallback(this.Run), false);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.Run), true);
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00151214 File Offset: 0x00150214
		private object CleanupOperation(object arg)
		{
			Interlocked.Exchange(ref this._cleanupRequests, 0);
			if (!this._cleanupEnabled)
			{
				return null;
			}
			this.Cleanup();
			return null;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00151238 File Offset: 0x00150238
		private void OnShutDown()
		{
			this._viewManager = null;
			this._commitManager = null;
			this._valueConverterTable = null;
			this._mostRecentTask = null;
			this._head = (this._tail = null);
			this._crossThreadQueue.Clear();
			HybridDictionary hybridDictionary = Interlocked.Exchange<HybridDictionary>(ref this._asyncDispatchers, null);
			if (hybridDictionary != null)
			{
				foreach (object obj in hybridDictionary.Keys)
				{
					IAsyncDataDispatcher asyncDataDispatcher = obj as IAsyncDataDispatcher;
					if (asyncDataDispatcher != null)
					{
						asyncDataDispatcher.CancelAllRequests();
					}
				}
			}
			this._defaultAsyncDataDispatcher = null;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x001512E4 File Offset: 0x001502E4
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.Run(false);
		}

		// Token: 0x04000BA3 RID: 2979
		private HybridDictionary _mostRecentTask;

		// Token: 0x04000BA4 RID: 2980
		private DataBindEngine.Task _head;

		// Token: 0x04000BA5 RID: 2981
		private DataBindEngine.Task _tail;

		// Token: 0x04000BA6 RID: 2982
		private UIElement _layoutElement;

		// Token: 0x04000BA7 RID: 2983
		private ViewManager _viewManager = new ViewManager();

		// Token: 0x04000BA8 RID: 2984
		private CommitManager _commitManager = new CommitManager();

		// Token: 0x04000BA9 RID: 2985
		private DataBindEngine.ValueConverterTable _valueConverterTable = new DataBindEngine.ValueConverterTable();

		// Token: 0x04000BAA RID: 2986
		private PathParser _pathParser = new PathParser();

		// Token: 0x04000BAB RID: 2987
		private IAsyncDataDispatcher _defaultAsyncDataDispatcher;

		// Token: 0x04000BAC RID: 2988
		private HybridDictionary _asyncDispatchers;

		// Token: 0x04000BAD RID: 2989
		private ValueConverterContext _valueConverterContext = new ValueConverterContext();

		// Token: 0x04000BAE RID: 2990
		private bool _cleanupEnabled = true;

		// Token: 0x04000BAF RID: 2991
		private ValueTable _valueTable = new ValueTable();

		// Token: 0x04000BB0 RID: 2992
		private AccessorTable _accessorTable = new AccessorTable();

		// Token: 0x04000BB1 RID: 2993
		private int _cleanupRequests;

		// Token: 0x04000BB2 RID: 2994
		private CleanupHelper _cleanupHelper;

		// Token: 0x04000BB3 RID: 2995
		private Queue<DataBindOperation> _crossThreadQueue = new Queue<DataBindOperation>();

		// Token: 0x04000BB4 RID: 2996
		private object _crossThreadQueueLock = new object();

		// Token: 0x04000BB5 RID: 2997
		private int _crossThreadCost;

		// Token: 0x04000BB6 RID: 2998
		private DispatcherOperation _crossThreadDispatcherOperation;

		// Token: 0x04000BB7 RID: 2999
		internal const int CrossThreadThreshold = 50000;

		// Token: 0x04000BB8 RID: 3000
		[ThreadStatic]
		private static DataBindEngine _currentEngine;

		// Token: 0x020009ED RID: 2541
		private class Task
		{
			// Token: 0x0600844E RID: 33870 RVA: 0x003256CE File Offset: 0x003246CE
			public Task(IDataBindEngineClient c, TaskOps o, DataBindEngine.Task previousForClient)
			{
				this.client = c;
				this.op = o;
				this.PreviousForClient = previousForClient;
				this.status = DataBindEngine.Task.Status.Pending;
			}

			// Token: 0x0600844F RID: 33871 RVA: 0x003256F4 File Offset: 0x003246F4
			public void Run(bool lastChance)
			{
				this.status = DataBindEngine.Task.Status.Running;
				DataBindEngine.Task.Status status = DataBindEngine.Task.Status.Completed;
				switch (this.op)
				{
				case TaskOps.TransferValue:
					this.client.TransferValue();
					break;
				case TaskOps.UpdateValue:
					this.client.UpdateValue();
					break;
				case TaskOps.AttachToContext:
					if (!this.client.AttachToContext(lastChance) && !lastChance)
					{
						status = DataBindEngine.Task.Status.Retry;
					}
					break;
				case TaskOps.VerifySourceReference:
					this.client.VerifySourceReference(lastChance);
					break;
				case TaskOps.RaiseTargetUpdatedEvent:
					this.client.OnTargetUpdated();
					break;
				}
				this.status = status;
			}

			// Token: 0x04004010 RID: 16400
			public IDataBindEngineClient client;

			// Token: 0x04004011 RID: 16401
			public TaskOps op;

			// Token: 0x04004012 RID: 16402
			public DataBindEngine.Task.Status status;

			// Token: 0x04004013 RID: 16403
			public DataBindEngine.Task Next;

			// Token: 0x04004014 RID: 16404
			public DataBindEngine.Task PreviousForClient;

			// Token: 0x02000C7F RID: 3199
			public enum Status
			{
				// Token: 0x04004C8C RID: 19596
				Pending,
				// Token: 0x04004C8D RID: 19597
				Running,
				// Token: 0x04004C8E RID: 19598
				Completed,
				// Token: 0x04004C8F RID: 19599
				Retry,
				// Token: 0x04004C90 RID: 19600
				Cancelled
			}
		}

		// Token: 0x020009EE RID: 2542
		private class ValueConverterTable : Hashtable
		{
			// Token: 0x17001DBA RID: 7610
			public IValueConverter this[Type sourceType, Type targetType, bool targetToSource]
			{
				get
				{
					DataBindEngine.ValueConverterTable.Key key = new DataBindEngine.ValueConverterTable.Key(sourceType, targetType, targetToSource);
					return (IValueConverter)base[key];
				}
			}

			// Token: 0x06008451 RID: 33873 RVA: 0x003257A4 File Offset: 0x003247A4
			public void Add(Type sourceType, Type targetType, bool targetToSource, IValueConverter value)
			{
				base.Add(new DataBindEngine.ValueConverterTable.Key(sourceType, targetType, targetToSource), value);
			}

			// Token: 0x02000C80 RID: 3200
			private struct Key
			{
				// Token: 0x0600921E RID: 37406 RVA: 0x0034B1E2 File Offset: 0x0034A1E2
				public Key(Type sourceType, Type targetType, bool targetToSource)
				{
					this._sourceType = sourceType;
					this._targetType = targetType;
					this._targetToSource = targetToSource;
				}

				// Token: 0x0600921F RID: 37407 RVA: 0x0034B1F9 File Offset: 0x0034A1F9
				public override int GetHashCode()
				{
					return this._sourceType.GetHashCode() + this._targetType.GetHashCode();
				}

				// Token: 0x06009220 RID: 37408 RVA: 0x0034B212 File Offset: 0x0034A212
				public override bool Equals(object o)
				{
					return o is DataBindEngine.ValueConverterTable.Key && this == (DataBindEngine.ValueConverterTable.Key)o;
				}

				// Token: 0x06009221 RID: 37409 RVA: 0x0034B22F File Offset: 0x0034A22F
				public static bool operator ==(DataBindEngine.ValueConverterTable.Key k1, DataBindEngine.ValueConverterTable.Key k2)
				{
					return k1._sourceType == k2._sourceType && k1._targetType == k2._targetType && k1._targetToSource == k2._targetToSource;
				}

				// Token: 0x06009222 RID: 37410 RVA: 0x0034B267 File Offset: 0x0034A267
				public static bool operator !=(DataBindEngine.ValueConverterTable.Key k1, DataBindEngine.ValueConverterTable.Key k2)
				{
					return !(k1 == k2);
				}

				// Token: 0x04004C91 RID: 19601
				private Type _sourceType;

				// Token: 0x04004C92 RID: 19602
				private Type _targetType;

				// Token: 0x04004C93 RID: 19603
				private bool _targetToSource;
			}
		}

		// Token: 0x020009EF RID: 2543
		private sealed class DataBindEngineShutDownListener : ShutDownListener
		{
			// Token: 0x06008453 RID: 33875 RVA: 0x0032314F File Offset: 0x0032214F
			public DataBindEngineShutDownListener(DataBindEngine target) : base(target)
			{
			}

			// Token: 0x06008454 RID: 33876 RVA: 0x003257C3 File Offset: 0x003247C3
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				((DataBindEngine)target).OnShutDown();
			}
		}
	}
}
