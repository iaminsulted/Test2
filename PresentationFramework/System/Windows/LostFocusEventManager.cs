using System;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x0200037E RID: 894
	public class LostFocusEventManager : WeakEventManager
	{
		// Token: 0x0600242B RID: 9259 RVA: 0x0015A0BF File Offset: 0x001590BF
		private LostFocusEventManager()
		{
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x00181B94 File Offset: 0x00180B94
		public static void AddListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			LostFocusEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x00181BBE File Offset: 0x00180BBE
		public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			LostFocusEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x00181BE8 File Offset: 0x00180BE8
		public static void AddHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			LostFocusEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x00181C04 File Offset: 0x00180C04
		public static void RemoveHandler(DependencyObject source, EventHandler<RoutedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			LostFocusEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x00181C20 File Offset: 0x00180C20
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<RoutedEventArgs>();
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x00181C28 File Offset: 0x00180C28
		protected override void StartListening(object source)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE((DependencyObject)source, out frameworkElement, out frameworkContentElement, true);
			if (frameworkElement != null)
			{
				frameworkElement.LostFocus += this.OnLostFocus;
				return;
			}
			if (frameworkContentElement != null)
			{
				frameworkContentElement.LostFocus += this.OnLostFocus;
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x00181C70 File Offset: 0x00180C70
		protected override void StopListening(object source)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE((DependencyObject)source, out frameworkElement, out frameworkContentElement, true);
			if (frameworkElement != null)
			{
				frameworkElement.LostFocus -= this.OnLostFocus;
				return;
			}
			if (frameworkContentElement != null)
			{
				frameworkContentElement.LostFocus -= this.OnLostFocus;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x00181CB8 File Offset: 0x00180CB8
		private static LostFocusEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(LostFocusEventManager);
				LostFocusEventManager lostFocusEventManager = (LostFocusEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (lostFocusEventManager == null)
				{
					lostFocusEventManager = new LostFocusEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, lostFocusEventManager);
				}
				return lostFocusEventManager;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
		private void OnLostFocus(object sender, RoutedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
