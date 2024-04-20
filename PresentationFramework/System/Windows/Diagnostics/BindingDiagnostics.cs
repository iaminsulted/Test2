using System;
using System.Collections.Generic;

namespace System.Windows.Diagnostics
{
	// Token: 0x0200043F RID: 1087
	public static class BindingDiagnostics
	{
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003502 RID: 13570 RVA: 0x001DCD2D File Offset: 0x001DBD2D
		// (set) Token: 0x06003503 RID: 13571 RVA: 0x001DCD34 File Offset: 0x001DBD34
		internal static bool IsEnabled { get; private set; } = VisualDiagnostics.IsEnabled && VisualDiagnostics.IsEnvironmentVariableSet(null, "ENABLE_XAML_DIAGNOSTICS_SOURCE_INFO");

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06003504 RID: 13572 RVA: 0x001DCD3C File Offset: 0x001DBD3C
		// (remove) Token: 0x06003505 RID: 13573 RVA: 0x001DCD70 File Offset: 0x001DBD70
		private static event EventHandler<BindingFailedEventArgs> s_bindingFailed;

		// Token: 0x06003506 RID: 13574 RVA: 0x001DCDA3 File Offset: 0x001DBDA3
		static BindingDiagnostics()
		{
			if (BindingDiagnostics.IsEnabled)
			{
				BindingDiagnostics.s_pendingEvents = new List<BindingFailedEventArgs>();
				BindingDiagnostics.s_pendingEventsLock = new object();
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06003507 RID: 13575 RVA: 0x001DCDDA File Offset: 0x001DBDDA
		// (remove) Token: 0x06003508 RID: 13576 RVA: 0x001DCDEE File Offset: 0x001DBDEE
		public static event EventHandler<BindingFailedEventArgs> BindingFailed
		{
			add
			{
				if (BindingDiagnostics.IsEnabled)
				{
					BindingDiagnostics.s_bindingFailed += value;
					BindingDiagnostics.FlushPendingBindingFailedEvents();
				}
			}
			remove
			{
				BindingDiagnostics.s_bindingFailed -= value;
			}
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x001DCDF8 File Offset: 0x001DBDF8
		private static void FlushPendingBindingFailedEvents()
		{
			if (BindingDiagnostics.s_pendingEvents != null)
			{
				List<BindingFailedEventArgs> list = null;
				object obj = BindingDiagnostics.s_pendingEventsLock;
				lock (obj)
				{
					list = BindingDiagnostics.s_pendingEvents;
					BindingDiagnostics.s_pendingEvents = null;
				}
				if (list != null)
				{
					foreach (BindingFailedEventArgs e in list)
					{
						EventHandler<BindingFailedEventArgs> eventHandler = BindingDiagnostics.s_bindingFailed;
						if (eventHandler != null)
						{
							eventHandler(null, e);
						}
					}
				}
			}
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x001DCE94 File Offset: 0x001DBE94
		internal static void NotifyBindingFailed(BindingFailedEventArgs args)
		{
			if (!BindingDiagnostics.IsEnabled)
			{
				return;
			}
			if (BindingDiagnostics.s_pendingEvents != null)
			{
				object obj = BindingDiagnostics.s_pendingEventsLock;
				lock (obj)
				{
					if (BindingDiagnostics.s_pendingEvents != null)
					{
						if (BindingDiagnostics.s_pendingEvents.Count < 2000)
						{
							BindingDiagnostics.s_pendingEvents.Add(args);
						}
						return;
					}
				}
			}
			EventHandler<BindingFailedEventArgs> eventHandler = BindingDiagnostics.s_bindingFailed;
			if (eventHandler == null)
			{
				return;
			}
			eventHandler(null, args);
		}

		// Token: 0x04001C64 RID: 7268
		private static List<BindingFailedEventArgs> s_pendingEvents;

		// Token: 0x04001C65 RID: 7269
		private static readonly object s_pendingEventsLock;

		// Token: 0x04001C66 RID: 7270
		private const int MaxPendingEvents = 2000;
	}
}
