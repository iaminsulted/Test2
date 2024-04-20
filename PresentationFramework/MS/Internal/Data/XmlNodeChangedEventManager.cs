using System;
using System.Windows;
using System.Xml;

namespace MS.Internal.Data
{
	// Token: 0x0200024E RID: 590
	internal class XmlNodeChangedEventManager : WeakEventManager
	{
		// Token: 0x060016BC RID: 5820 RVA: 0x0015A0BF File Offset: 0x001590BF
		private XmlNodeChangedEventManager()
		{
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x0015C1B3 File Offset: 0x0015B1B3
		public static void AddListener(XmlDocument source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0015C1DD File Offset: 0x0015B1DD
		public static void RemoveListener(XmlDocument source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x0015C207 File Offset: 0x0015B207
		public static void AddHandler(XmlDocument source, EventHandler<XmlNodeChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0015C223 File Offset: 0x0015B223
		public static void RemoveHandler(XmlDocument source, EventHandler<XmlNodeChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0015C23F File Offset: 0x0015B23F
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<XmlNodeChangedEventArgs>();
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x0015C248 File Offset: 0x0015B248
		protected override void StartListening(object source)
		{
			XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnXmlNodeChanged);
			XmlDocument xmlDocument = (XmlDocument)source;
			xmlDocument.NodeInserted += value;
			xmlDocument.NodeRemoved += value;
			xmlDocument.NodeChanged += value;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x0015C27C File Offset: 0x0015B27C
		protected override void StopListening(object source)
		{
			XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnXmlNodeChanged);
			XmlDocument xmlDocument = (XmlDocument)source;
			xmlDocument.NodeInserted -= value;
			xmlDocument.NodeRemoved -= value;
			xmlDocument.NodeChanged -= value;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x0015C2B0 File Offset: 0x0015B2B0
		private static XmlNodeChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(XmlNodeChangedEventManager);
				XmlNodeChangedEventManager xmlNodeChangedEventManager = (XmlNodeChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (xmlNodeChangedEventManager == null)
				{
					xmlNodeChangedEventManager = new XmlNodeChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, xmlNodeChangedEventManager);
				}
				return xmlNodeChangedEventManager;
			}
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
		private void OnXmlNodeChanged(object sender, XmlNodeChangedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
