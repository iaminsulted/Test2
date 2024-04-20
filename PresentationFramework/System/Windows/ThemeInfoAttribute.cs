using System;
using System.Reflection;

namespace System.Windows
{
	// Token: 0x020003D2 RID: 978
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class ThemeInfoAttribute : Attribute
	{
		// Token: 0x060028EB RID: 10475 RVA: 0x00197AE5 File Offset: 0x00196AE5
		public ThemeInfoAttribute(ResourceDictionaryLocation themeDictionaryLocation, ResourceDictionaryLocation genericDictionaryLocation)
		{
			this._themeDictionaryLocation = themeDictionaryLocation;
			this._genericDictionaryLocation = genericDictionaryLocation;
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x00197AFB File Offset: 0x00196AFB
		public ResourceDictionaryLocation ThemeDictionaryLocation
		{
			get
			{
				return this._themeDictionaryLocation;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x060028ED RID: 10477 RVA: 0x00197B03 File Offset: 0x00196B03
		public ResourceDictionaryLocation GenericDictionaryLocation
		{
			get
			{
				return this._genericDictionaryLocation;
			}
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x00197B0B File Offset: 0x00196B0B
		internal static ThemeInfoAttribute FromAssembly(Assembly assembly)
		{
			return Attribute.GetCustomAttribute(assembly, typeof(ThemeInfoAttribute)) as ThemeInfoAttribute;
		}

		// Token: 0x040014CE RID: 5326
		private ResourceDictionaryLocation _themeDictionaryLocation;

		// Token: 0x040014CF RID: 5327
		private ResourceDictionaryLocation _genericDictionaryLocation;
	}
}
