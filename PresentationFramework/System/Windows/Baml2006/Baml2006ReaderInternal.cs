using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xaml;
using MS.Internal;

namespace System.Windows.Baml2006
{
	// Token: 0x0200040A RID: 1034
	internal class Baml2006ReaderInternal : Baml2006Reader
	{
		// Token: 0x06002D20 RID: 11552 RVA: 0x001AB337 File Offset: 0x001AA337
		internal Baml2006ReaderInternal(Stream stream, Baml2006SchemaContext schemaContext, Baml2006ReaderSettings settings) : base(stream, schemaContext, settings)
		{
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x001AB342 File Offset: 0x001AA342
		internal Baml2006ReaderInternal(Stream stream, Baml2006SchemaContext baml2006SchemaContext, Baml2006ReaderSettings baml2006ReaderSettings, object root) : base(stream, baml2006SchemaContext, baml2006ReaderSettings, root)
		{
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x001AB34F File Offset: 0x001AA34F
		internal override string GetAssemblyNameForNamespace(Assembly asm)
		{
			return asm.FullName;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x001AB358 File Offset: 0x001AA358
		internal override object CreateTypeConverterMarkupExtension(XamlMember property, TypeConverter converter, object propertyValue, Baml2006ReaderSettings settings)
		{
			if (FrameworkAppContextSwitches.AppendLocalAssemblyVersionForSourceUri && property.DeclaringType.UnderlyingType == typeof(ResourceDictionary) && property.Name.Equals("Source"))
			{
				return new SourceUriTypeConverterMarkupExtension(converter, propertyValue, settings.LocalAssembly);
			}
			return base.CreateTypeConverterMarkupExtension(property, converter, propertyValue, settings);
		}
	}
}
