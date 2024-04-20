using System;
using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000839 RID: 2105
	internal class InternalCollectionChangedEventManager : WeakEventManager
	{
		// Token: 0x06007B7F RID: 31615 RVA: 0x0015A0BF File Offset: 0x001590BF
		private InternalCollectionChangedEventManager()
		{
		}

		// Token: 0x06007B80 RID: 31616 RVA: 0x0030C5EC File Offset: 0x0030B5EC
		public static void AddListener(GridViewColumnCollection source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x0030C616 File Offset: 0x0030B616
		public static void RemoveListener(GridViewColumnCollection source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x06007B82 RID: 31618 RVA: 0x0030C640 File Offset: 0x0030B640
		public static void AddHandler(GridViewColumnCollection source, EventHandler<NotifyCollectionChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x06007B83 RID: 31619 RVA: 0x0030C65C File Offset: 0x0030B65C
		public static void RemoveHandler(GridViewColumnCollection source, EventHandler<NotifyCollectionChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x0030C678 File Offset: 0x0030B678
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<NotifyCollectionChangedEventArgs>();
		}

		// Token: 0x06007B85 RID: 31621 RVA: 0x0030C67F File Offset: 0x0030B67F
		protected override void StartListening(object source)
		{
			((GridViewColumnCollection)source).InternalCollectionChanged += this.OnCollectionChanged;
		}

		// Token: 0x06007B86 RID: 31622 RVA: 0x0030C698 File Offset: 0x0030B698
		protected override void StopListening(object source)
		{
			((GridViewColumnCollection)source).InternalCollectionChanged -= this.OnCollectionChanged;
		}

		// Token: 0x17001C8D RID: 7309
		// (get) Token: 0x06007B87 RID: 31623 RVA: 0x0030C6B4 File Offset: 0x0030B6B4
		private static InternalCollectionChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(InternalCollectionChangedEventManager);
				InternalCollectionChangedEventManager internalCollectionChangedEventManager = (InternalCollectionChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (internalCollectionChangedEventManager == null)
				{
					internalCollectionChangedEventManager = new InternalCollectionChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, internalCollectionChangedEventManager);
				}
				return internalCollectionChangedEventManager;
			}
		}

		// Token: 0x06007B88 RID: 31624 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
