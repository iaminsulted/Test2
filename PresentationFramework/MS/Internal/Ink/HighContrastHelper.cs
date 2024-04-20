using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Ink
{
	// Token: 0x02000187 RID: 391
	internal static class HighContrastHelper
	{
		// Token: 0x06000CE7 RID: 3303 RVA: 0x00131BE0 File Offset: 0x00130BE0
		internal static void RegisterHighContrastCallback(HighContrastCallback highContrastCallback)
		{
			object _lock = HighContrastHelper.__lock;
			lock (_lock)
			{
				int count = HighContrastHelper.__highContrastCallbackList.Count;
				int i = 0;
				int num = 0;
				if (HighContrastHelper.__increaseCount > 100)
				{
					while (i < count)
					{
						if (HighContrastHelper.__highContrastCallbackList[num].IsAlive)
						{
							num++;
						}
						else
						{
							HighContrastHelper.__highContrastCallbackList.RemoveAt(num);
						}
						i++;
					}
					HighContrastHelper.__increaseCount = 0;
				}
				HighContrastHelper.__highContrastCallbackList.Add(new WeakReference(highContrastCallback));
				HighContrastHelper.__increaseCount++;
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00131C88 File Offset: 0x00130C88
		internal static void OnSettingChanged()
		{
			HighContrastHelper.UpdateHighContrast();
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00131C90 File Offset: 0x00130C90
		private static void UpdateHighContrast()
		{
			object _lock = HighContrastHelper.__lock;
			lock (_lock)
			{
				int count = HighContrastHelper.__highContrastCallbackList.Count;
				int i = 0;
				int num = 0;
				while (i < count)
				{
					WeakReference weakReference = HighContrastHelper.__highContrastCallbackList[num];
					if (weakReference.IsAlive)
					{
						HighContrastCallback highContrastCallback = weakReference.Target as HighContrastCallback;
						if (highContrastCallback.Dispatcher != null)
						{
							highContrastCallback.Dispatcher.BeginInvoke(DispatcherPriority.Background, new HighContrastHelper.UpdateHighContrastCallback(HighContrastHelper.OnUpdateHighContrast), highContrastCallback);
						}
						else
						{
							HighContrastHelper.OnUpdateHighContrast(highContrastCallback);
						}
						num++;
					}
					else
					{
						HighContrastHelper.__highContrastCallbackList.RemoveAt(num);
					}
					i++;
				}
				HighContrastHelper.__increaseCount = 0;
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00131D50 File Offset: 0x00130D50
		private static void OnUpdateHighContrast(HighContrastCallback highContrastCallback)
		{
			bool highContrast = SystemParameters.HighContrast;
			Color windowTextColor = SystemColors.WindowTextColor;
			if (highContrast)
			{
				highContrastCallback.TurnHighContrastOn(windowTextColor);
				return;
			}
			highContrastCallback.TurnHighContrastOff();
		}

		// Token: 0x040009AA RID: 2474
		private static object __lock = new object();

		// Token: 0x040009AB RID: 2475
		private static List<WeakReference> __highContrastCallbackList = new List<WeakReference>();

		// Token: 0x040009AC RID: 2476
		private static int __increaseCount = 0;

		// Token: 0x040009AD RID: 2477
		private const int CleanTolerance = 100;

		// Token: 0x020009C6 RID: 2502
		// (Invoke) Token: 0x060083DC RID: 33756
		private delegate void UpdateHighContrastCallback(HighContrastCallback highContrastCallback);
	}
}
