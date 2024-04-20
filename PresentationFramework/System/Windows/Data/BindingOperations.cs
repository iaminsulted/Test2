using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000451 RID: 1105
	public static class BindingOperations
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060037A2 RID: 14242 RVA: 0x001E6931 File Offset: 0x001E5931
		public static object DisconnectedSource
		{
			get
			{
				return BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x001E6938 File Offset: 0x001E5938
		public static BindingExpressionBase SetBinding(DependencyObject target, DependencyProperty dp, BindingBase binding)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (binding == null)
			{
				throw new ArgumentNullException("binding");
			}
			BindingExpressionBase bindingExpressionBase = binding.CreateBindingExpression(target, dp);
			target.SetValue(dp, bindingExpressionBase);
			return bindingExpressionBase;
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x001E6984 File Offset: 0x001E5984
		public static BindingBase GetBindingBase(DependencyObject target, DependencyProperty dp)
		{
			BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(target, dp);
			if (bindingExpressionBase == null)
			{
				return null;
			}
			return bindingExpressionBase.ParentBindingBase;
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x001E69A4 File Offset: 0x001E59A4
		public static Binding GetBinding(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingBase(target, dp) as Binding;
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x001E69B2 File Offset: 0x001E59B2
		public static PriorityBinding GetPriorityBinding(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingBase(target, dp) as PriorityBinding;
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x001E69C0 File Offset: 0x001E59C0
		public static MultiBinding GetMultiBinding(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingBase(target, dp) as MultiBinding;
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x001E69CE File Offset: 0x001E59CE
		public static BindingExpressionBase GetBindingExpressionBase(DependencyObject target, DependencyProperty dp)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			return StyleHelper.GetExpression(target, dp) as BindingExpressionBase;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x001E69F8 File Offset: 0x001E59F8
		public static BindingExpression GetBindingExpression(DependencyObject target, DependencyProperty dp)
		{
			BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(target, dp);
			PriorityBindingExpression priorityBindingExpression = bindingExpressionBase as PriorityBindingExpression;
			if (priorityBindingExpression != null)
			{
				bindingExpressionBase = priorityBindingExpression.ActiveBindingExpression;
			}
			return bindingExpressionBase as BindingExpression;
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x001E6A24 File Offset: 0x001E5A24
		public static MultiBindingExpression GetMultiBindingExpression(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpressionBase(target, dp) as MultiBindingExpression;
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x001E6A32 File Offset: 0x001E5A32
		public static PriorityBindingExpression GetPriorityBindingExpression(DependencyObject target, DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpressionBase(target, dp) as PriorityBindingExpression;
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x001E6A40 File Offset: 0x001E5A40
		public static void ClearBinding(DependencyObject target, DependencyProperty dp)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (BindingOperations.IsDataBound(target, dp))
			{
				target.ClearValue(dp);
			}
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x001E6A70 File Offset: 0x001E5A70
		public static void ClearAllBindings(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			LocalValueEnumerator localValueEnumerator = target.GetLocalValueEnumerator();
			ArrayList arrayList = new ArrayList(8);
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				if (BindingOperations.IsDataBound(target, localValueEntry.Property))
				{
					arrayList.Add(localValueEntry.Property);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				target.ClearValue((DependencyProperty)arrayList[i]);
			}
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x001E6AEC File Offset: 0x001E5AEC
		public static bool IsDataBound(DependencyObject target, DependencyProperty dp)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			return StyleHelper.GetExpression(target, dp) is BindingExpressionBase;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x001E6B19 File Offset: 0x001E5B19
		public static void EnableCollectionSynchronization(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (synchronizationCallback == null)
			{
				throw new ArgumentNullException("synchronizationCallback");
			}
			ViewManager.Current.RegisterCollectionSynchronizationCallback(collection, context, synchronizationCallback);
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x001E6B44 File Offset: 0x001E5B44
		public static void EnableCollectionSynchronization(IEnumerable collection, object lockObject)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (lockObject == null)
			{
				throw new ArgumentNullException("lockObject");
			}
			ViewManager.Current.RegisterCollectionSynchronizationCallback(collection, lockObject, null);
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x001E6B6F File Offset: 0x001E5B6F
		public static void DisableCollectionSynchronization(IEnumerable collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			ViewManager.Current.RegisterCollectionSynchronizationCallback(collection, null, null);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x001E6B8C File Offset: 0x001E5B8C
		public static void AccessCollection(IEnumerable collection, Action accessMethod, bool writeAccess)
		{
			ViewManager viewManager = ViewManager.Current;
			if (viewManager == null)
			{
				throw new InvalidOperationException(SR.Get("AccessCollectionAfterShutDown", new object[]
				{
					collection
				}));
			}
			viewManager.AccessCollection(collection, accessMethod, writeAccess);
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x001E6BC5 File Offset: 0x001E5BC5
		public static ReadOnlyCollection<BindingExpressionBase> GetSourceUpdatingBindings(DependencyObject root)
		{
			return new ReadOnlyCollection<BindingExpressionBase>(DataBindEngine.CurrentDataBindEngine.CommitManager.GetBindingsInScope(root));
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x001E6BDC File Offset: 0x001E5BDC
		public static ReadOnlyCollection<BindingGroup> GetSourceUpdatingBindingGroups(DependencyObject root)
		{
			return new ReadOnlyCollection<BindingGroup>(DataBindEngine.CurrentDataBindEngine.CommitManager.GetBindingGroupsInScope(root));
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x060037B5 RID: 14261 RVA: 0x001E6BF4 File Offset: 0x001E5BF4
		// (remove) Token: 0x060037B6 RID: 14262 RVA: 0x001E6C28 File Offset: 0x001E5C28
		public static event EventHandler<CollectionRegisteringEventArgs> CollectionRegistering;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x060037B7 RID: 14263 RVA: 0x001E6C5C File Offset: 0x001E5C5C
		// (remove) Token: 0x060037B8 RID: 14264 RVA: 0x001E6C90 File Offset: 0x001E5C90
		public static event EventHandler<CollectionViewRegisteringEventArgs> CollectionViewRegistering;

		// Token: 0x060037B9 RID: 14265 RVA: 0x001E6CC3 File Offset: 0x001E5CC3
		internal static bool IsValidUpdateSourceTrigger(UpdateSourceTrigger value)
		{
			return value <= UpdateSourceTrigger.Explicit;
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x001E6CCC File Offset: 0x001E5CCC
		// (set) Token: 0x060037BB RID: 14267 RVA: 0x001E6CD8 File Offset: 0x001E5CD8
		internal static bool IsCleanupEnabled
		{
			get
			{
				return DataBindEngine.CurrentDataBindEngine.CleanupEnabled;
			}
			set
			{
				DataBindEngine.CurrentDataBindEngine.CleanupEnabled = value;
			}
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x001E6CE5 File Offset: 0x001E5CE5
		internal static bool Cleanup()
		{
			return DataBindEngine.CurrentDataBindEngine.Cleanup();
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x001E6CF1 File Offset: 0x001E5CF1
		internal static void PrintStats()
		{
			DataBindEngine.CurrentDataBindEngine.AccessorTable.PrintStats();
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x001E6D02 File Offset: 0x001E5D02
		// (set) Token: 0x060037BF RID: 14271 RVA: 0x001E6D13 File Offset: 0x001E5D13
		internal static bool TraceAccessorTableSize
		{
			get
			{
				return DataBindEngine.CurrentDataBindEngine.AccessorTable.TraceSize;
			}
			set
			{
				DataBindEngine.CurrentDataBindEngine.AccessorTable.TraceSize = value;
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x001E6D25 File Offset: 0x001E5D25
		internal static void OnCollectionRegistering(IEnumerable collection, object parent)
		{
			if (BindingOperations.CollectionRegistering != null)
			{
				BindingOperations.CollectionRegistering(null, new CollectionRegisteringEventArgs(collection, parent));
			}
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x001E6D40 File Offset: 0x001E5D40
		internal static void OnCollectionViewRegistering(CollectionView view)
		{
			if (BindingOperations.CollectionViewRegistering != null)
			{
				BindingOperations.CollectionViewRegistering(null, new CollectionViewRegisteringEventArgs(view));
			}
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x001E6D5C File Offset: 0x001E5D5C
		internal static IDisposable EnableExceptionLogging()
		{
			BindingOperations.ExceptionLogger value = new BindingOperations.ExceptionLogger();
			Interlocked.CompareExchange<BindingOperations.ExceptionLogger>(ref BindingOperations._exceptionLogger, value, null);
			return BindingOperations._exceptionLogger;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x001E6D84 File Offset: 0x001E5D84
		internal static void LogException(Exception ex)
		{
			BindingOperations.ExceptionLogger exceptionLogger = BindingOperations._exceptionLogger;
			if (exceptionLogger != null)
			{
				exceptionLogger.LogException(ex);
			}
		}

		// Token: 0x04001CF8 RID: 7416
		private static BindingOperations.ExceptionLogger _exceptionLogger;

		// Token: 0x02000ADE RID: 2782
		internal class ExceptionLogger : IDisposable
		{
			// Token: 0x06008B37 RID: 35639 RVA: 0x003396DF File Offset: 0x003386DF
			internal void LogException(Exception ex)
			{
				this._log.Add(ex);
			}

			// Token: 0x06008B38 RID: 35640 RVA: 0x003396ED File Offset: 0x003386ED
			void IDisposable.Dispose()
			{
				Interlocked.CompareExchange<BindingOperations.ExceptionLogger>(ref BindingOperations._exceptionLogger, null, this);
			}

			// Token: 0x17001E7D RID: 7805
			// (get) Token: 0x06008B39 RID: 35641 RVA: 0x003396FC File Offset: 0x003386FC
			internal List<Exception> Log
			{
				get
				{
					return this._log;
				}
			}

			// Token: 0x04004704 RID: 18180
			private List<Exception> _log = new List<Exception>();
		}
	}
}
