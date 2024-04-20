using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Baml2006;
using System.Windows.Markup;
using System.Xaml;

namespace System.Windows
{
	// Token: 0x02000353 RID: 851
	public class DeferrableContentConverter : TypeConverter
	{
		// Token: 0x06002050 RID: 8272 RVA: 0x00174F42 File Offset: 0x00173F42
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(Stream).IsAssignableFrom(sourceType) || sourceType == typeof(byte[]) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00174F74 File Offset: 0x00173F74
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			Baml2006SchemaContext baml2006SchemaContext = DeferrableContentConverter.RequireService<IXamlSchemaContextProvider>(context).SchemaContext as Baml2006SchemaContext;
			if (baml2006SchemaContext == null)
			{
				throw new InvalidOperationException(SR.Get("ExpectedBamlSchemaContext"));
			}
			IXamlObjectWriterFactory objectWriterFactory = DeferrableContentConverter.RequireService<IXamlObjectWriterFactory>(context);
			IProvideValueTarget provideValueTarget = DeferrableContentConverter.RequireService<IProvideValueTarget>(context);
			IRootObjectProvider rootObjectProvider = DeferrableContentConverter.RequireService<IRootObjectProvider>(context);
			if (!(provideValueTarget.TargetObject is ResourceDictionary))
			{
				throw new InvalidOperationException(SR.Get("ExpectedResourceDictionaryTarget"));
			}
			Stream stream = value as Stream;
			if (stream == null)
			{
				byte[] array = value as byte[];
				if (array != null)
				{
					stream = new MemoryStream(array);
				}
			}
			if (stream == null)
			{
				throw new InvalidOperationException(SR.Get("ExpectedBinaryContent"));
			}
			return new DeferrableContent(stream, baml2006SchemaContext, objectWriterFactory, context, rootObjectProvider.RootObject);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00175034 File Offset: 0x00174034
		private static T RequireService<T>(IServiceProvider provider) where T : class
		{
			T t = provider.GetService(typeof(T)) as T;
			if (t == null)
			{
				throw new InvalidOperationException(SR.Get("DeferringLoaderNoContext", new object[]
				{
					typeof(DeferrableContentConverter).Name,
					typeof(T).Name
				}));
			}
			return t;
		}
	}
}
