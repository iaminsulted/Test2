using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000536 RID: 1334
	internal class FrameworkElementFactoryProperty : ElementPropertyBase
	{
		// Token: 0x06004204 RID: 16900 RVA: 0x00219BAE File Offset: 0x00218BAE
		public FrameworkElementFactoryProperty(PropertyValue propertyValue, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._propertyValue = propertyValue;
			this._item = item;
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x00219BCC File Offset: 0x00218BCC
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				if (!this._descriptorCalculated)
				{
					this._descriptorCalculated = true;
					if (DependencyProperty.FromName(this._propertyValue.Property.Name, this._item.ObjectType) == this._propertyValue.Property)
					{
						this._descriptor = DependencyPropertyDescriptor.FromProperty(this._propertyValue.Property, this._item.ObjectType);
					}
				}
				return this._descriptor;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x00219C3C File Offset: 0x00218C3C
		public override bool IsAttached
		{
			get
			{
				DependencyPropertyDescriptor dependencyPropertyDescriptor = this.PropertyDescriptor as DependencyPropertyDescriptor;
				return dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x00219C60 File Offset: 0x00218C60
		public override AttributeCollection Attributes
		{
			get
			{
				if (this._descriptor != null)
				{
					return this._descriptor.Attributes;
				}
				return DependencyPropertyDescriptor.FromProperty(this._propertyValue.Property, this._item.ObjectType).Attributes;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x00219C96 File Offset: 0x00218C96
		public override string Name
		{
			get
			{
				return this._propertyValue.Property.Name;
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06004209 RID: 16905 RVA: 0x00219CA8 File Offset: 0x00218CA8
		public override Type PropertyType
		{
			get
			{
				return this._propertyValue.Property.PropertyType;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x00219CBA File Offset: 0x00218CBA
		public override DependencyProperty DependencyProperty
		{
			get
			{
				return this._propertyValue.Property;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x00219CC8 File Offset: 0x00218CC8
		public override object Value
		{
			get
			{
				PropertyValueType valueType = this._propertyValue.ValueType;
				if (valueType == PropertyValueType.Set || valueType == PropertyValueType.TemplateBinding)
				{
					return this._propertyValue.Value;
				}
				if (valueType != PropertyValueType.Resource)
				{
					return null;
				}
				return new DynamicResourceExtension(this._propertyValue.Value);
			}
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x00219D0C File Offset: 0x00218D0C
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x00219D19 File Offset: 0x00218D19
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x040024E6 RID: 9446
		private PropertyValue _propertyValue;

		// Token: 0x040024E7 RID: 9447
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x040024E8 RID: 9448
		private bool _descriptorCalculated;

		// Token: 0x040024E9 RID: 9449
		private PropertyDescriptor _descriptor;
	}
}
