using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x02000331 RID: 817
	internal static class SafeSecurityHelper
	{
		// Token: 0x06001EA4 RID: 7844 RVA: 0x0016FF38 File Offset: 0x0016EF38
		internal static string GetAssemblyPartialName(Assembly assembly)
		{
			string name = new AssemblyName(assembly.FullName).Name;
			if (name == null)
			{
				return string.Empty;
			}
			return name;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0016FF60 File Offset: 0x0016EF60
		internal static string GetFullAssemblyNameFromPartialName(Assembly protoAssembly, string partialName)
		{
			return new AssemblyName(protoAssembly.FullName)
			{
				Name = partialName
			}.FullName;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0016FF7C File Offset: 0x0016EF7C
		internal static Point ClientToScreen(UIElement relativeTo, Point point)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(relativeTo);
			if (presentationSource == null)
			{
				return new Point(double.NaN, double.NaN);
			}
			Point point2;
			relativeTo.TransformToAncestor(presentationSource.RootVisual).TryTransform(point, out point2);
			return PointUtil.ClientToScreen(PointUtil.RootToClient(point2, presentationSource), presentationSource);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0016FFD0 File Offset: 0x0016EFD0
		internal static bool IsSameKeyToken(byte[] reqKeyToken, byte[] curKeyToken)
		{
			bool result = false;
			if (reqKeyToken == null && curKeyToken == null)
			{
				result = true;
			}
			else if (reqKeyToken != null && curKeyToken != null && reqKeyToken.Length == curKeyToken.Length)
			{
				result = true;
				for (int i = 0; i < reqKeyToken.Length; i++)
				{
					if (reqKeyToken[i] != curKeyToken[i])
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00170014 File Offset: 0x0016F014
		internal static bool IsFeatureDisabled(SafeSecurityHelper.KeyToRead key)
		{
			bool flag = false;
			switch (key)
			{
			case SafeSecurityHelper.KeyToRead.WebBrowserDisable:
			{
				string name = "WebBrowserDisallow";
				goto IL_76;
			}
			case SafeSecurityHelper.KeyToRead.MediaAudioDisable:
			{
				string name = "MediaAudioDisallow";
				goto IL_76;
			}
			case (SafeSecurityHelper.KeyToRead)3:
			case (SafeSecurityHelper.KeyToRead)5:
			case (SafeSecurityHelper.KeyToRead)7:
				break;
			case SafeSecurityHelper.KeyToRead.MediaVideoDisable:
			{
				string name = "MediaVideoDisallow";
				goto IL_76;
			}
			case SafeSecurityHelper.KeyToRead.MediaAudioOrVideoDisable:
			{
				string name = "MediaAudioDisallow";
				goto IL_76;
			}
			case SafeSecurityHelper.KeyToRead.MediaImageDisable:
			{
				string name = "MediaImageDisallow";
				goto IL_76;
			}
			default:
				if (key == SafeSecurityHelper.KeyToRead.ScriptInteropDisable)
				{
					string name = "ScriptInteropDisallow";
					goto IL_76;
				}
				break;
			}
			throw new ArgumentException(key.ToString());
			IL_76:
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework\\Windows Presentation Foundation\\Features");
			if (registryKey != null)
			{
				string name;
				object value = registryKey.GetValue(name);
				if (value is int && (int)value == 1)
				{
					flag = true;
				}
				if (!flag && key == SafeSecurityHelper.KeyToRead.MediaAudioOrVideoDisable)
				{
					name = "MediaVideoDisallow";
					value = registryKey.GetValue(name);
					if (value is int && (int)value == 1)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x001700FA File Offset: 0x0016F0FA
		internal static bool IsConnectedToPresentationSource(Visual visual)
		{
			return PresentationSource.CriticalFromVisual(visual) != null;
		}

		// Token: 0x04000F3A RID: 3898
		internal const string IMAGE = "image";

		// Token: 0x02000A71 RID: 2673
		internal enum KeyToRead
		{
			// Token: 0x04004163 RID: 16739
			WebBrowserDisable = 1,
			// Token: 0x04004164 RID: 16740
			MediaAudioDisable,
			// Token: 0x04004165 RID: 16741
			MediaVideoDisable = 4,
			// Token: 0x04004166 RID: 16742
			MediaImageDisable = 8,
			// Token: 0x04004167 RID: 16743
			MediaAudioOrVideoDisable = 6,
			// Token: 0x04004168 RID: 16744
			ScriptInteropDisable = 16
		}
	}
}
