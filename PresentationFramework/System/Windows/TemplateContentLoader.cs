using System;
using System.Xaml;

namespace System.Windows
{
	// Token: 0x020003C7 RID: 967
	public class TemplateContentLoader : XamlDeferringLoader
	{
		// Token: 0x060028C0 RID: 10432 RVA: 0x0019719C File Offset: 0x0019619C
		public override object Load(XamlReader xamlReader, IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			if (xamlReader == null)
			{
				throw new ArgumentNullException("xamlReader");
			}
			IXamlObjectWriterFactory factory = TemplateContentLoader.RequireService<IXamlObjectWriterFactory>(serviceProvider);
			return new TemplateContent(xamlReader, factory, serviceProvider);
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x001971D4 File Offset: 0x001961D4
		private static T RequireService<T>(IServiceProvider provider) where T : class
		{
			T t = provider.GetService(typeof(T)) as T;
			if (t == null)
			{
				throw new InvalidOperationException(SR.Get("DeferringLoaderNoContext", new object[]
				{
					typeof(TemplateContentLoader).Name,
					typeof(T).Name
				}));
			}
			return t;
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x0019723F File Offset: 0x0019623F
		public override XamlReader Save(object value, IServiceProvider serviceProvider)
		{
			throw new NotSupportedException(SR.Get("DeferringLoaderNoSave", new object[]
			{
				typeof(TemplateContentLoader).Name
			}));
		}
	}
}
