using System;

namespace WinRT
{
	// Token: 0x0200009D RID: 157
	internal class ActivationFactory<T> : BaseActivationFactory
	{
		// Token: 0x0600023A RID: 570 RVA: 0x000F9959 File Offset: 0x000F8959
		public ActivationFactory() : base(typeof(T).Namespace, typeof(T).FullName)
		{
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000F997F File Offset: 0x000F897F
		public static ObjectReference<I> As<I>()
		{
			return ActivationFactory<T>._factory.Value._As<I>();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000F9990 File Offset: 0x000F8990
		public static ObjectReference<I> ActivateInstance<I>()
		{
			return ActivationFactory<T>._factory.Value._ActivateInstance<I>();
		}

		// Token: 0x04000589 RID: 1417
		private static WeakLazy<ActivationFactory<T>> _factory = new WeakLazy<ActivationFactory<T>>();
	}
}
