using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Baml2006;
using System.Xaml;

namespace System.Windows
{
	// Token: 0x02000352 RID: 850
	[TypeConverter(typeof(DeferrableContentConverter))]
	public class DeferrableContent
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x00174E61 File Offset: 0x00173E61
		// (set) Token: 0x06002044 RID: 8260 RVA: 0x00174E69 File Offset: 0x00173E69
		internal Stream Stream { get; private set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x00174E72 File Offset: 0x00173E72
		// (set) Token: 0x06002046 RID: 8262 RVA: 0x00174E7A File Offset: 0x00173E7A
		internal Baml2006SchemaContext SchemaContext { get; private set; }

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x00174E83 File Offset: 0x00173E83
		// (set) Token: 0x06002048 RID: 8264 RVA: 0x00174E8B File Offset: 0x00173E8B
		internal IXamlObjectWriterFactory ObjectWriterFactory { get; private set; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002049 RID: 8265 RVA: 0x00174E94 File Offset: 0x00173E94
		// (set) Token: 0x0600204A RID: 8266 RVA: 0x00174E9C File Offset: 0x00173E9C
		internal XamlObjectWriterSettings ObjectWriterParentSettings { get; private set; }

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x00174EA5 File Offset: 0x00173EA5
		// (set) Token: 0x0600204C RID: 8268 RVA: 0x00174EAD File Offset: 0x00173EAD
		internal object RootObject { get; private set; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600204D RID: 8269 RVA: 0x00174EB6 File Offset: 0x00173EB6
		// (set) Token: 0x0600204E RID: 8270 RVA: 0x00174EBE File Offset: 0x00173EBE
		internal IServiceProvider ServiceProvider { get; private set; }

		// Token: 0x0600204F RID: 8271 RVA: 0x00174EC8 File Offset: 0x00173EC8
		internal DeferrableContent(Stream stream, Baml2006SchemaContext schemaContext, IXamlObjectWriterFactory objectWriterFactory, IServiceProvider serviceProvider, object rootObject)
		{
			this.ObjectWriterParentSettings = objectWriterFactory.GetParentSettings();
			bool flag = false;
			if (schemaContext.LocalAssembly != null)
			{
				flag = schemaContext.LocalAssembly.ImageRuntimeVersion.StartsWith("v2", StringComparison.Ordinal);
			}
			if (flag)
			{
				this.ObjectWriterParentSettings.SkipProvideValueOnRoot = true;
			}
			this.Stream = stream;
			this.SchemaContext = schemaContext;
			this.ObjectWriterFactory = objectWriterFactory;
			this.ServiceProvider = serviceProvider;
			this.RootObject = rootObject;
		}
	}
}
