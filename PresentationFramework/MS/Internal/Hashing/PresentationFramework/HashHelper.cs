using System;
using System.Windows;
using System.Windows.Markup.Localizer;
using MS.Internal.Hashing.PresentationCore;

namespace MS.Internal.Hashing.PresentationFramework
{
	// Token: 0x0200032B RID: 811
	internal static class HashHelper
	{
		// Token: 0x06001E9B RID: 7835 RVA: 0x0016FE98 File Offset: 0x0016EE98
		static HashHelper()
		{
			HashHelper.Initialize();
			Type[] types = new Type[]
			{
				typeof(BamlLocalizableResource),
				typeof(ComponentResourceKey)
			};
			BaseHashHelper.RegisterTypes(typeof(HashHelper).Assembly, types);
			HashHelper.Initialize();
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0016FEE5 File Offset: 0x0016EEE5
		internal static bool HasReliableHashCode(object item)
		{
			return BaseHashHelper.HasReliableHashCode(item);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal static void Initialize()
		{
		}
	}
}
