using System;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Windows
{
	// Token: 0x0200033C RID: 828
	internal static class SR
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x00171248 File Offset: 0x00170248
		private static ResourceManager ResourceManager
		{
			get
			{
				return SRID.ResourceManager;
			}
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x00105F35 File Offset: 0x00104F35
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool UsingResourceKeys()
		{
			return false;
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x00171250 File Offset: 0x00170250
		internal static string GetResourceString(string resourceKey, string defaultString)
		{
			string text = null;
			try
			{
				text = SR.ResourceManager.GetString(resourceKey);
			}
			catch (MissingManifestResourceException)
			{
			}
			if (defaultString != null && resourceKey.Equals(text, StringComparison.Ordinal))
			{
				return defaultString;
			}
			return text;
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x00171290 File Offset: 0x00170290
		internal static string Format(string resourceFormat, params object[] args)
		{
			if (args == null)
			{
				return resourceFormat;
			}
			if (SR.UsingResourceKeys())
			{
				return resourceFormat + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x001712B7 File Offset: 0x001702B7
		internal static string Format(string resourceFormat, object p1)
		{
			if (SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[]
				{
					resourceFormat,
					p1
				});
			}
			return string.Format(resourceFormat, p1);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x001712E0 File Offset: 0x001702E0
		internal static string Format(string resourceFormat, object p1, object p2)
		{
			if (SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[]
				{
					resourceFormat,
					p1,
					p2
				});
			}
			return string.Format(resourceFormat, p1, p2);
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0017130E File Offset: 0x0017030E
		internal static string Format(string resourceFormat, object p1, object p2, object p3)
		{
			if (SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[]
				{
					resourceFormat,
					p1,
					p2,
					p3
				});
			}
			return string.Format(resourceFormat, p1, p2, p3);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00171341 File Offset: 0x00170341
		public static string Get(string name)
		{
			return SR.GetResourceString(name, null);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0017134A File Offset: 0x0017034A
		public static string Get(string name, params object[] args)
		{
			return SR.Format(SR.GetResourceString(name, null), args);
		}
	}
}
