using System;
using System.Reflection;

namespace System
{
	// Token: 0x02000339 RID: 825
	internal static class AppContextDefaultValues
	{
		// Token: 0x06001F21 RID: 7969 RVA: 0x00170E7C File Offset: 0x0016FE7C
		public static void PopulateDefaultValues()
		{
			string platformIdentifier;
			string profile;
			int version;
			AppContextDefaultValues.ParseTargetFrameworkName(out platformIdentifier, out profile, out version);
			AppContextDefaultValues.PopulateDefaultValuesPartial(platformIdentifier, profile, version);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x00170E9C File Offset: 0x0016FE9C
		private static void ParseTargetFrameworkName(out string identifier, out string profile, out int version)
		{
			string text = AppContextDefaultValues.GetTargetFrameworkMoniker();
			if (text == null)
			{
				text = ".NETCoreApp,Version=v3.0";
			}
			if (!AppContextDefaultValues.TryParseFrameworkName(text, out identifier, out version, out profile))
			{
				identifier = string.Empty;
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00170ECC File Offset: 0x0016FECC
		private static string GetTargetFrameworkMoniker()
		{
			string result;
			try
			{
				PropertyInfo property = typeof(AppDomain).GetProperty("SetupInformation");
				object obj = (property != null) ? property.GetValue(AppDomain.CurrentDomain) : null;
				Type type = Type.GetType("System.AppDomainSetup");
				PropertyInfo propertyInfo = (type != null) ? type.GetProperty("TargetFrameworkName") : null;
				result = ((obj != null) ? (((propertyInfo != null) ? propertyInfo.GetValue(obj) : null) as string) : null);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x00170F4C File Offset: 0x0016FF4C
		private static bool TryParseFrameworkName(string frameworkName, out string identifier, out int version, out string profile)
		{
			string empty;
			profile = (empty = string.Empty);
			identifier = empty;
			version = 0;
			if (frameworkName == null || frameworkName.Length == 0)
			{
				return false;
			}
			string[] array = frameworkName.Split(',', StringSplitOptions.None);
			version = 0;
			if (array.Length < 2 || array.Length > 3)
			{
				return false;
			}
			identifier = array[0].Trim();
			if (identifier.Length == 0)
			{
				return false;
			}
			bool result = false;
			profile = null;
			for (int i = 1; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('=', StringSplitOptions.None);
				if (array2.Length != 2)
				{
					return false;
				}
				string text = array2[0].Trim();
				string text2 = array2[1].Trim();
				if (text.Equals("Version", StringComparison.OrdinalIgnoreCase))
				{
					result = true;
					if (text2.Length > 0 && (text2[0] == 'v' || text2[0] == 'V'))
					{
						text2 = text2.Substring(1);
					}
					Version version2 = new Version(text2);
					version = version2.Major * 10000;
					if (version2.Minor > 0)
					{
						version += version2.Minor * 100;
					}
					if (version2.Build > 0)
					{
						version += version2.Build;
					}
				}
				else
				{
					if (!text.Equals("Profile", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
					if (!string.IsNullOrEmpty(text2))
					{
						profile = text2;
					}
				}
			}
			return result;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00171098 File Offset: 0x00170098
		private static void PopulateDefaultValuesPartial(string platformIdentifier, string profile, int version)
		{
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", true);
			if (!(platformIdentifier == ".NETFramework"))
			{
				if (!(platformIdentifier == ".NETCoreApp"))
				{
					return;
				}
				AppContextDefaultValues.InitializeNetFxSwitchDefaultsForNetCoreRuntime();
			}
			else
			{
				if (version <= 40502)
				{
					LocalAppContext.DefineSwitchDefault("Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness", true);
				}
				if (version <= 40602)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace", true);
				}
				if (version <= 40700)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent", true);
				}
				if (version <= 40701)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation", true);
				}
				if (40000 <= version && version <= 40702)
				{
					LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer", true);
					return;
				}
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x00171138 File Offset: 0x00170138
		private static void InitializeNetFxSwitchDefaultsForNetCoreRuntime()
		{
			LocalAppContext.DefineSwitchDefault("Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Baml2006.AppendLocalAssemblyVersionForSourceUri", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Controls.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement", false);
			LocalAppContext.DefineSwitchDefault("Switch.System.Windows.Automation.Peers.ItemAutomationPeerKeepsItsItemAlive", false);
		}
	}
}
