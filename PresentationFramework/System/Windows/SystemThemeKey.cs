using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x020003C0 RID: 960
	[TypeConverter(typeof(SystemKeyConverter))]
	internal class SystemThemeKey : ResourceKey
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x001954D0 File Offset: 0x001944D0
		internal SystemThemeKey(SystemResourceKeyID id)
		{
			this._id = id;
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x001954DF File Offset: 0x001944DF
		public override Assembly Assembly
		{
			get
			{
				if (SystemThemeKey._presentationFrameworkAssembly == null)
				{
					SystemThemeKey._presentationFrameworkAssembly = typeof(FrameworkElement).Assembly;
				}
				return SystemThemeKey._presentationFrameworkAssembly;
			}
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x00195508 File Offset: 0x00194508
		public override bool Equals(object o)
		{
			SystemThemeKey systemThemeKey = o as SystemThemeKey;
			return systemThemeKey != null && systemThemeKey._id == this._id;
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x0019552F File Offset: 0x0019452F
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x00195537 File Offset: 0x00194537
		public override string ToString()
		{
			return this._id.ToString();
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x0019552F File Offset: 0x0019452F
		internal SystemResourceKeyID InternalKey
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x0400148A RID: 5258
		private SystemResourceKeyID _id;

		// Token: 0x0400148B RID: 5259
		private static Assembly _presentationFrameworkAssembly;
	}
}
