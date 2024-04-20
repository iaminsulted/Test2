using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x020003D1 RID: 977
	[MarkupExtensionReturnType(typeof(Uri))]
	public class ThemeDictionaryExtension : MarkupExtension
	{
		// Token: 0x060028E1 RID: 10465 RVA: 0x00173A19 File Offset: 0x00172A19
		public ThemeDictionaryExtension()
		{
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x001977AA File Offset: 0x001967AA
		public ThemeDictionaryExtension(string assemblyName)
		{
			if (assemblyName != null)
			{
				this._assemblyName = assemblyName;
				return;
			}
			throw new ArgumentNullException("assemblyName");
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x001977C7 File Offset: 0x001967C7
		// (set) Token: 0x060028E4 RID: 10468 RVA: 0x001977CF File Offset: 0x001967CF
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				this._assemblyName = value;
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x001977D8 File Offset: 0x001967D8
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (string.IsNullOrEmpty(this.AssemblyName))
			{
				throw new InvalidOperationException(SR.Get("ThemeDictionaryExtension_Name"));
			}
			IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValueTarget == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IProvideValueTarget"
				}));
			}
			object targetObject = provideValueTarget.TargetObject;
			object targetProperty = provideValueTarget.TargetProperty;
			ResourceDictionary resourceDictionary = targetObject as ResourceDictionary;
			PropertyInfo left = targetProperty as PropertyInfo;
			if (resourceDictionary == null || (targetProperty != null && left != ThemeDictionaryExtension.SourceProperty))
			{
				throw new InvalidOperationException(SR.Get("ThemeDictionaryExtension_Source"));
			}
			ThemeDictionaryExtension.Register(resourceDictionary, this._assemblyName);
			resourceDictionary.IsSourcedFromThemeDictionary = true;
			return ThemeDictionaryExtension.GenerateUri(this._assemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, UxThemeWrapper.ThemeName);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x001978AC File Offset: 0x001968AC
		private static Uri GenerateUri(string assemblyName, string resourceName, string themeName)
		{
			StringBuilder stringBuilder = new StringBuilder(assemblyName.Length + 50);
			stringBuilder.Append("/");
			stringBuilder.Append(assemblyName);
			if (assemblyName.Equals("PresentationFramework", StringComparison.OrdinalIgnoreCase))
			{
				stringBuilder.Append('.');
				stringBuilder.Append(themeName);
			}
			stringBuilder.Append(";component/");
			stringBuilder.Append(resourceName);
			stringBuilder.Append(".xaml");
			return new Uri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x00197928 File Offset: 0x00196928
		internal static Uri GenerateFallbackUri(ResourceDictionary dictionary, string resourceName)
		{
			for (int i = 0; i < ThemeDictionaryExtension._themeDictionaryInfos.Count; i++)
			{
				ThemeDictionaryExtension.ThemeDictionaryInfo themeDictionaryInfo = ThemeDictionaryExtension._themeDictionaryInfos[i];
				if (!themeDictionaryInfo.DictionaryReference.IsAlive)
				{
					ThemeDictionaryExtension._themeDictionaryInfos.RemoveAt(i);
					i--;
				}
				else if ((ResourceDictionary)themeDictionaryInfo.DictionaryReference.Target == dictionary)
				{
					string themeName = resourceName.Split('/', StringSplitOptions.None)[1];
					return ThemeDictionaryExtension.GenerateUri(themeDictionaryInfo.AssemblyName, resourceName, themeName);
				}
			}
			return null;
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x001979A2 File Offset: 0x001969A2
		private static PropertyInfo SourceProperty
		{
			get
			{
				if (ThemeDictionaryExtension._sourceProperty == null)
				{
					ThemeDictionaryExtension._sourceProperty = typeof(ResourceDictionary).GetProperty("Source");
				}
				return ThemeDictionaryExtension._sourceProperty;
			}
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x001979D0 File Offset: 0x001969D0
		private static void Register(ResourceDictionary dictionary, string assemblyName)
		{
			if (ThemeDictionaryExtension._themeDictionaryInfos == null)
			{
				ThemeDictionaryExtension._themeDictionaryInfos = new List<ThemeDictionaryExtension.ThemeDictionaryInfo>();
			}
			ThemeDictionaryExtension.ThemeDictionaryInfo themeDictionaryInfo;
			for (int i = 0; i < ThemeDictionaryExtension._themeDictionaryInfos.Count; i++)
			{
				themeDictionaryInfo = ThemeDictionaryExtension._themeDictionaryInfos[i];
				if (!themeDictionaryInfo.DictionaryReference.IsAlive)
				{
					ThemeDictionaryExtension._themeDictionaryInfos.RemoveAt(i);
					i--;
				}
				else if (themeDictionaryInfo.DictionaryReference.Target == dictionary)
				{
					themeDictionaryInfo.AssemblyName = assemblyName;
					return;
				}
			}
			themeDictionaryInfo = new ThemeDictionaryExtension.ThemeDictionaryInfo();
			themeDictionaryInfo.DictionaryReference = new WeakReference(dictionary);
			themeDictionaryInfo.AssemblyName = assemblyName;
			ThemeDictionaryExtension._themeDictionaryInfos.Add(themeDictionaryInfo);
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x00197A68 File Offset: 0x00196A68
		internal static void OnThemeChanged()
		{
			if (ThemeDictionaryExtension._themeDictionaryInfos != null)
			{
				for (int i = 0; i < ThemeDictionaryExtension._themeDictionaryInfos.Count; i++)
				{
					ThemeDictionaryExtension.ThemeDictionaryInfo themeDictionaryInfo = ThemeDictionaryExtension._themeDictionaryInfos[i];
					if (!themeDictionaryInfo.DictionaryReference.IsAlive)
					{
						ThemeDictionaryExtension._themeDictionaryInfos.RemoveAt(i);
						i--;
					}
					else
					{
						((ResourceDictionary)themeDictionaryInfo.DictionaryReference.Target).Source = ThemeDictionaryExtension.GenerateUri(themeDictionaryInfo.AssemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, UxThemeWrapper.ThemeName);
					}
				}
			}
		}

		// Token: 0x040014CB RID: 5323
		private string _assemblyName;

		// Token: 0x040014CC RID: 5324
		private static PropertyInfo _sourceProperty;

		// Token: 0x040014CD RID: 5325
		[ThreadStatic]
		private static List<ThemeDictionaryExtension.ThemeDictionaryInfo> _themeDictionaryInfos;

		// Token: 0x02000A96 RID: 2710
		private class ThemeDictionaryInfo
		{
			// Token: 0x04004281 RID: 17025
			public WeakReference DictionaryReference;

			// Token: 0x04004282 RID: 17026
			public string AssemblyName;
		}
	}
}
