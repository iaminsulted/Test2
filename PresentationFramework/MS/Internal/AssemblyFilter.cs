using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x020000EB RID: 235
	internal class AssemblyFilter
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x000FF3FF File Offset: 0x000FE3FF
		static AssemblyFilter()
		{
			AssemblyFilter._disallowedListExtracted = new SecurityCriticalDataForSet<bool>(false);
			AssemblyFilter._assemblyList = new SecurityCriticalDataForSet<List<string>>(new List<string>());
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000FF428 File Offset: 0x000FE428
		internal void FilterCallback(object sender, AssemblyLoadEventArgs args)
		{
			object @lock = AssemblyFilter._lock;
			lock (@lock)
			{
				Assembly loadedAssembly = args.LoadedAssembly;
				if (!loadedAssembly.ReflectionOnly && loadedAssembly.GlobalAssemblyCache)
				{
					string text = this.AssemblyNameWithFileVersion(loadedAssembly);
					if (this.AssemblyOnDisallowedList(text))
					{
						UnsafeNativeMethods.ProcessUnhandledException_DLL(SR.Get("KillBitEnforcedShutdown") + text);
						try
						{
							Environment.Exit(-1);
						}
						finally
						{
						}
					}
				}
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000FF4B4 File Offset: 0x000FE4B4
		private string AssemblyNameWithFileVersion(Assembly a)
		{
			StringBuilder stringBuilder = new StringBuilder(a.FullName);
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(a.Location);
			if (versionInfo != null && versionInfo.ProductVersion != null)
			{
				stringBuilder.Append(", FileVersion=" + versionInfo.ProductVersion);
			}
			return stringBuilder.ToString().ToLower(CultureInfo.InvariantCulture).Trim();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000FF510 File Offset: 0x000FE510
		private bool AssemblyOnDisallowedList(string assemblyToCheck)
		{
			bool result = false;
			if (!AssemblyFilter._disallowedListExtracted.Value)
			{
				this.ExtractDisallowedRegistryList();
				AssemblyFilter._disallowedListExtracted.Value = true;
			}
			if (AssemblyFilter._assemblyList.Value.Contains(assemblyToCheck))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000FF554 File Offset: 0x000FE554
		private void ExtractDisallowedRegistryList()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NetFramework\\policy\\APTCA");
			if (registryKey != null)
			{
				foreach (string text in registryKey.GetSubKeyNames())
				{
					registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NetFramework\\policy\\APTCA\\" + text);
					object value = registryKey.GetValue("APTCA_FLAG");
					if (value != null && (int)value == 1 && !AssemblyFilter._assemblyList.Value.Contains(text))
					{
						AssemblyFilter._assemblyList.Value.Add(text.ToLower(CultureInfo.InvariantCulture).Trim());
					}
				}
			}
		}

		// Token: 0x04000614 RID: 1556
		private static SecurityCriticalDataForSet<List<string>> _assemblyList;

		// Token: 0x04000615 RID: 1557
		private static SecurityCriticalDataForSet<bool> _disallowedListExtracted;

		// Token: 0x04000616 RID: 1558
		private static object _lock = new object();

		// Token: 0x04000617 RID: 1559
		private const string FILEVERSION_STRING = ", FileVersion=";

		// Token: 0x04000618 RID: 1560
		private const string KILL_BIT_REGISTRY_HIVE = "HKEY_LOCAL_MACHINE\\";

		// Token: 0x04000619 RID: 1561
		private const string KILL_BIT_REGISTRY_LOCATION = "Software\\Microsoft\\.NetFramework\\policy\\APTCA";

		// Token: 0x0400061A RID: 1562
		private const string SUBKEY_VALUE = "APTCA_FLAG";
	}
}
