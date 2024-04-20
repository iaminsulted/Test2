using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace MS.Internal.Annotations
{
	// Token: 0x020002C3 RID: 707
	internal class Serializer
	{
		// Token: 0x06001A55 RID: 6741 RVA: 0x00163638 File Offset: 0x00162638
		public Serializer(Type type)
		{
			Invariant.Assert(type != null);
			foreach (object obj in type.GetCustomAttributes(false))
			{
				this._attribute = (obj as XmlRootAttribute);
				if (this._attribute != null)
				{
					break;
				}
			}
			Invariant.Assert(this._attribute != null, "Internal Serializer used for a type with no XmlRootAttribute.");
			this._ctor = type.GetConstructor(new Type[0]);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x001636AC File Offset: 0x001626AC
		public void Serialize(XmlWriter writer, object obj)
		{
			Invariant.Assert(writer != null && obj != null);
			IXmlSerializable xmlSerializable = obj as IXmlSerializable;
			Invariant.Assert(xmlSerializable != null, "Internal Serializer used for a type that isn't IXmlSerializable.");
			writer.WriteStartElement(this._attribute.ElementName, this._attribute.Namespace);
			xmlSerializable.WriteXml(writer);
			writer.WriteEndElement();
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00163704 File Offset: 0x00162704
		public object Deserialize(XmlReader reader)
		{
			Invariant.Assert(reader != null);
			IXmlSerializable xmlSerializable = (IXmlSerializable)this._ctor.Invoke(new object[0]);
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
			}
			xmlSerializable.ReadXml(reader);
			return xmlSerializable;
		}

		// Token: 0x04000DB1 RID: 3505
		private XmlRootAttribute _attribute;

		// Token: 0x04000DB2 RID: 3506
		private ConstructorInfo _ctor;
	}
}
