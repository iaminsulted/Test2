using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000249 RID: 585
	internal struct SynchronizationInfo
	{
		// Token: 0x06001684 RID: 5764 RVA: 0x0015AD20 File Offset: 0x00159D20
		public SynchronizationInfo(object context, CollectionSynchronizationCallback callback)
		{
			if (callback == null)
			{
				this._context = context;
				this._callbackMethod = null;
				this._callbackTarget = null;
				return;
			}
			this._context = new WeakReference(context);
			this._callbackMethod = callback.Method;
			object target = callback.Target;
			this._callbackTarget = ((target != null) ? new WeakReference(target) : ViewManager.StaticWeakRef);
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x0015AD7B File Offset: 0x00159D7B
		public bool IsSynchronized
		{
			get
			{
				return this._context != null || this._callbackMethod != null;
			}
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x0015AD94 File Offset: 0x00159D94
		public void AccessCollection(IEnumerable collection, Action accessMethod, bool writeAccess)
		{
			if (!(this._callbackMethod != null))
			{
				if (this._context != null)
				{
					object context = this._context;
					lock (context)
					{
						accessMethod();
						return;
					}
				}
				accessMethod();
				return;
			}
			object obj = this._callbackTarget.Target;
			if (obj == null)
			{
				throw new InvalidOperationException(SR.Get("CollectionView_MissingSynchronizationCallback", new object[]
				{
					collection
				}));
			}
			if (this._callbackTarget == ViewManager.StaticWeakRef)
			{
				obj = null;
			}
			WeakReference weakReference = this._context as WeakReference;
			object obj2 = (weakReference != null) ? weakReference.Target : this._context;
			this._callbackMethod.Invoke(obj, new object[]
			{
				collection,
				obj2,
				accessMethod,
				writeAccess
			});
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x0015AE74 File Offset: 0x00159E74
		public bool IsAlive
		{
			get
			{
				return (this._callbackMethod != null && this._callbackTarget.IsAlive) || (this._callbackMethod == null && this._context != null);
			}
		}

		// Token: 0x04000C64 RID: 3172
		public static readonly SynchronizationInfo None = new SynchronizationInfo(null, null);

		// Token: 0x04000C65 RID: 3173
		private object _context;

		// Token: 0x04000C66 RID: 3174
		private MethodInfo _callbackMethod;

		// Token: 0x04000C67 RID: 3175
		private WeakReference _callbackTarget;
	}
}
