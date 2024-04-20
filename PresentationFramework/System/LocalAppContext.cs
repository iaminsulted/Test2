using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000338 RID: 824
	internal class LocalAppContext
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x00170CDA File Offset: 0x0016FCDA
		// (set) Token: 0x06001F18 RID: 7960 RVA: 0x00170CE1 File Offset: 0x0016FCE1
		private static bool DisableCaching { get; set; }

		// Token: 0x06001F19 RID: 7961 RVA: 0x00170CE9 File Offset: 0x0016FCE9
		static LocalAppContext()
		{
			LocalAppContext.s_canForwardCalls = LocalAppContext.SetupDelegate();
			AppContextDefaultValues.PopulateDefaultValues();
			LocalAppContext.DisableCaching = LocalAppContext.IsSwitchEnabled("TestSwitch.LocalAppContext.DisableCaching");
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00170D20 File Offset: 0x0016FD20
		public static bool IsSwitchEnabled(string switchName)
		{
			bool result;
			if (LocalAppContext.s_canForwardCalls && LocalAppContext.TryGetSwitchFromCentralAppContext(switchName, out result))
			{
				return result;
			}
			return LocalAppContext.IsSwitchEnabledLocal(switchName);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x00170D4C File Offset: 0x0016FD4C
		private static bool IsSwitchEnabledLocal(string switchName)
		{
			Dictionary<string, bool> obj = LocalAppContext.s_switchMap;
			bool flag3;
			bool flag2;
			lock (obj)
			{
				flag2 = LocalAppContext.s_switchMap.TryGetValue(switchName, out flag3);
			}
			return flag2 && flag3;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x00170D9C File Offset: 0x0016FD9C
		private static bool SetupDelegate()
		{
			Type type = typeof(object).Assembly.GetType("System.AppContext");
			if (type == null)
			{
				return false;
			}
			MethodInfo method = type.GetMethod("TryGetSwitch", BindingFlags.Static | BindingFlags.Public, null, new Type[]
			{
				typeof(string),
				typeof(bool).MakeByRefType()
			}, null);
			if (method == null)
			{
				return false;
			}
			LocalAppContext.TryGetSwitchFromCentralAppContext = (LocalAppContext.TryGetSwitchDelegate)Delegate.CreateDelegate(typeof(LocalAppContext.TryGetSwitchDelegate), method);
			return true;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x00170E29 File Offset: 0x0016FE29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool GetCachedSwitchValue(string switchName, ref int switchValue)
		{
			return switchValue >= 0 && (switchValue > 0 || LocalAppContext.GetCachedSwitchValueInternal(switchName, ref switchValue));
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x00170E40 File Offset: 0x0016FE40
		private static bool GetCachedSwitchValueInternal(string switchName, ref int switchValue)
		{
			if (LocalAppContext.DisableCaching)
			{
				return LocalAppContext.IsSwitchEnabled(switchName);
			}
			bool flag = LocalAppContext.IsSwitchEnabled(switchName);
			switchValue = (flag ? 1 : -1);
			return flag;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00170E6C File Offset: 0x0016FE6C
		internal static void DefineSwitchDefault(string switchName, bool initialValue)
		{
			LocalAppContext.s_switchMap[switchName] = initialValue;
		}

		// Token: 0x04000F6A RID: 3946
		private static LocalAppContext.TryGetSwitchDelegate TryGetSwitchFromCentralAppContext;

		// Token: 0x04000F6B RID: 3947
		private static bool s_canForwardCalls;

		// Token: 0x04000F6C RID: 3948
		private static Dictionary<string, bool> s_switchMap = new Dictionary<string, bool>();

		// Token: 0x04000F6D RID: 3949
		private static readonly object s_syncLock = new object();

		// Token: 0x02000A72 RID: 2674
		// (Invoke) Token: 0x06008644 RID: 34372
		private delegate bool TryGetSwitchDelegate(string switchName, out bool value);
	}
}
