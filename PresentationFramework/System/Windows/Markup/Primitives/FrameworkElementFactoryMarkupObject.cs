using System;
using System.Collections.Generic;
using System.ComponentModel;
using MS.Utility;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000535 RID: 1333
	internal class FrameworkElementFactoryMarkupObject : MarkupObject
	{
		// Token: 0x060041FC RID: 16892 RVA: 0x00219B3C File Offset: 0x00218B3C
		internal FrameworkElementFactoryMarkupObject(FrameworkElementFactory factory, XamlDesignerSerializationManager manager)
		{
			this._factory = factory;
			this._manager = manager;
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x00219B52 File Offset: 0x00218B52
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = context;
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x00218907 File Offset: 0x00217907
		public override AttributeCollection Attributes
		{
			get
			{
				return TypeDescriptor.GetAttributes(this.ObjectType);
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x00219B5B File Offset: 0x00218B5B
		public override Type ObjectType
		{
			get
			{
				if (this._factory.Type != null)
				{
					return this._factory.Type;
				}
				return typeof(string);
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x00219B86 File Offset: 0x00218B86
		public override object Instance
		{
			get
			{
				return this._factory;
			}
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x00219B8E File Offset: 0x00218B8E
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			if (this._factory.Type == null)
			{
				if (this._factory.Text != null)
				{
					yield return new FrameworkElementFactoryStringContent(this._factory, this);
				}
			}
			else
			{
				FrugalStructList<PropertyValue> propertyValues = this._factory.PropertyValues;
				int num;
				for (int i = 0; i < propertyValues.Count; i = num + 1)
				{
					if (propertyValues[i].Property != XmlAttributeProperties.XmlnsDictionaryProperty)
					{
						yield return new FrameworkElementFactoryProperty(propertyValues[i], this);
					}
					num = i;
				}
				ElementMarkupObject elementMarkupObject = new ElementMarkupObject(this._factory, this.Manager);
				foreach (MarkupProperty markupProperty in elementMarkupObject.Properties)
				{
					if (markupProperty.Name == "Triggers" && markupProperty.Name == "Storyboard")
					{
						yield return markupProperty;
					}
				}
				IEnumerator<MarkupProperty> enumerator = null;
				if (this._factory.FirstChild != null)
				{
					if (this._factory.FirstChild.Type == null)
					{
						yield return new FrameworkElementFactoryStringContent(this._factory.FirstChild, this);
					}
					else
					{
						yield return new FrameworkElementFactoryContent(this._factory, this);
					}
				}
				propertyValues = default(FrugalStructList<PropertyValue>);
			}
			yield break;
			yield break;
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x00219B9E File Offset: 0x00218B9E
		internal IValueSerializerContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x00219BA6 File Offset: 0x00218BA6
		internal XamlDesignerSerializationManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x040024E3 RID: 9443
		private FrameworkElementFactory _factory;

		// Token: 0x040024E4 RID: 9444
		private IValueSerializerContext _context;

		// Token: 0x040024E5 RID: 9445
		private XamlDesignerSerializationManager _manager;
	}
}
