using System;

namespace System.Windows.Data
{
	// Token: 0x0200045A RID: 1114
	public class DataChangedEventManager : WeakEventManager
	{
		// Token: 0x060038C7 RID: 14535 RVA: 0x0015A0BF File Offset: 0x001590BF
		private DataChangedEventManager()
		{
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x001E9F10 File Offset: 0x001E8F10
		public static void AddListener(DataSourceProvider source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			DataChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x001E9F3A File Offset: 0x001E8F3A
		public static void RemoveListener(DataSourceProvider source, IWeakEventListener listener)
		{
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			DataChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x001E9F56 File Offset: 0x001E8F56
		public static void AddHandler(DataSourceProvider source, EventHandler<EventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			DataChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x001E9F72 File Offset: 0x001E8F72
		public static void RemoveHandler(DataSourceProvider source, EventHandler<EventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			DataChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x001E9F8E File Offset: 0x001E8F8E
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<EventArgs>();
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x001E9F95 File Offset: 0x001E8F95
		protected override void StartListening(object source)
		{
			((DataSourceProvider)source).DataChanged += this.OnDataChanged;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x001E9FAE File Offset: 0x001E8FAE
		protected override void StopListening(object source)
		{
			((DataSourceProvider)source).DataChanged -= this.OnDataChanged;
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060038CF RID: 14543 RVA: 0x001E9FC8 File Offset: 0x001E8FC8
		private static DataChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(DataChangedEventManager);
				DataChangedEventManager dataChangedEventManager = (DataChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (dataChangedEventManager == null)
				{
					dataChangedEventManager = new DataChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, dataChangedEventManager);
				}
				return dataChangedEventManager;
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
		private void OnDataChanged(object sender, EventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
