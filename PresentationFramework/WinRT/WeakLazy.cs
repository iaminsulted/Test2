using System;

namespace WinRT
{
	// Token: 0x0200009A RID: 154
	internal class WeakLazy<T> where T : class, new()
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000230 RID: 560 RVA: 0x000F96FC File Offset: 0x000F86FC
		public T Value
		{
			get
			{
				WeakReference<T> instance = this._instance;
				T result;
				lock (instance)
				{
					T t;
					if (!this._instance.TryGetTarget(out t))
					{
						t = Activator.CreateInstance<T>();
						this._instance.SetTarget(t);
					}
					result = t;
				}
				return result;
			}
		}

		// Token: 0x04000585 RID: 1413
		private WeakReference<T> _instance = new WeakReference<T>(default(T));
	}
}
