using System;
using System.IO;
using System.Text;
using System.Windows.Markup.Primitives;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x0200051D RID: 1309
	public static class XamlWriter
	{
		// Token: 0x06004135 RID: 16693 RVA: 0x00217480 File Offset: 0x00216480
		public static string Save(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			StringBuilder stringBuilder = new StringBuilder();
			TextWriter textWriter = new StringWriter(stringBuilder, TypeConverterHelper.InvariantEnglishUS);
			try
			{
				XamlWriter.Save(obj, textWriter);
			}
			finally
			{
				textWriter.Close();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x002174D4 File Offset: 0x002164D4
		public static void Save(object obj, TextWriter writer)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			MarkupWriter.SaveAsXml(new XmlTextWriter(writer), obj);
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x002174FE File Offset: 0x002164FE
		public static void Save(object obj, Stream stream)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			MarkupWriter.SaveAsXml(new XmlTextWriter(stream, null), obj);
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x0021752C File Offset: 0x0021652C
		public static void Save(object obj, XmlWriter xmlWriter)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (xmlWriter == null)
			{
				throw new ArgumentNullException("xmlWriter");
			}
			try
			{
				MarkupWriter.SaveAsXml(xmlWriter, obj);
			}
			finally
			{
				xmlWriter.Flush();
			}
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x00217578 File Offset: 0x00216578
		public static void Save(object obj, XamlDesignerSerializationManager manager)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			MarkupWriter.SaveAsXml(manager.XmlWriter, obj, manager);
		}
	}
}
