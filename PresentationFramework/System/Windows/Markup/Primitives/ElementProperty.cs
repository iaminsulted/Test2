using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000529 RID: 1321
	internal class ElementProperty : ElementObjectPropertyBase
	{
		// Token: 0x060041A9 RID: 16809 RVA: 0x0021914C File Offset: 0x0021814C
		internal ElementProperty(ElementMarkupObject obj, PropertyDescriptor descriptor) : base(obj)
		{
			this._descriptor = descriptor;
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x0021915C File Offset: 0x0021815C
		public override string Name
		{
			get
			{
				return this._descriptor.Name;
			}
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x060041AB RID: 16811 RVA: 0x00219169 File Offset: 0x00218169
		public override Type PropertyType
		{
			get
			{
				return this._descriptor.PropertyType;
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x00219176 File Offset: 0x00218176
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._descriptor;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x0021917E File Offset: 0x0021817E
		public override bool IsAttached
		{
			get
			{
				this.UpdateDependencyProperty();
				return this._isAttached;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x0021918C File Offset: 0x0021818C
		public override DependencyProperty DependencyProperty
		{
			get
			{
				this.UpdateDependencyProperty();
				return this._dependencyProperty;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x0021919C File Offset: 0x0021819C
		public override object Value
		{
			get
			{
				DependencyProperty dependencyProperty = this.DependencyProperty;
				object obj;
				if (dependencyProperty != null)
				{
					DependencyObject dependencyObject = this._object.Instance as DependencyObject;
					obj = dependencyObject.ReadLocalValue(dependencyProperty);
					Expression expression = obj as Expression;
					if (expression != null)
					{
						TypeConverter converter = TypeDescriptor.GetConverter(obj);
						if (base.Manager.XamlWriterMode == XamlWriterMode.Expression && converter.CanConvertTo(typeof(MarkupExtension)))
						{
							obj = converter.ConvertTo(expression, typeof(MarkupExtension));
						}
						else
						{
							obj = expression.GetValue(dependencyObject, dependencyProperty);
						}
					}
					if (obj == DependencyProperty.UnsetValue)
					{
						obj = dependencyProperty.GetDefaultValue(dependencyObject.DependencyObjectType);
					}
				}
				else if ((this.Name == "Template" || this.Name == "VisualTree") && base.Context.Instance is FrameworkTemplate && (base.Context.Instance as FrameworkTemplate).HasContent)
				{
					obj = (base.Context.Instance as FrameworkTemplate).LoadContent();
				}
				else
				{
					obj = this._descriptor.GetValue(this._object.Instance);
				}
				if (!(obj is MarkupExtension) && !base.CanConvertToString(obj))
				{
					obj = ElementProperty.CheckForMarkupExtension(this.PropertyType, obj, base.Context, true);
				}
				return obj;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x002192DB File Offset: 0x002182DB
		public override AttributeCollection Attributes
		{
			get
			{
				return this._descriptor.Attributes;
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x002192E8 File Offset: 0x002182E8
		private void UpdateDependencyProperty()
		{
			if (!this._isDependencyPropertyCached)
			{
				DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(this._descriptor);
				if (dependencyPropertyDescriptor != null)
				{
					this._dependencyProperty = dependencyPropertyDescriptor.DependencyProperty;
					this._isAttached = dependencyPropertyDescriptor.IsAttached;
				}
				this._isDependencyPropertyCached = true;
			}
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0021932C File Offset: 0x0021832C
		internal static object CheckForMarkupExtension(Type propertyType, object value, IValueSerializerContext context, bool convertEnums)
		{
			if (value == null)
			{
				return new NullExtension();
			}
			TypeConverter converter = TypeDescriptor.GetConverter(value);
			if (converter.CanConvertTo(context, typeof(MarkupExtension)))
			{
				return converter.ConvertTo(context, TypeConverterHelper.InvariantEnglishUS, value, typeof(MarkupExtension));
			}
			Type type = value as Type;
			if (type != null)
			{
				if (propertyType == typeof(Type))
				{
					return value;
				}
				return new TypeExtension(type);
			}
			else
			{
				if (convertEnums)
				{
					Enum @enum = value as Enum;
					if (@enum != null)
					{
						return new StaticExtension(context.GetValueSerializerFor(typeof(Type)).ConvertToString(@enum.GetType(), context) + "." + @enum.ToString());
					}
				}
				Array array = value as Array;
				if (array != null)
				{
					return new ArrayExtension(array);
				}
				return value;
			}
		}

		// Token: 0x040024D4 RID: 9428
		private PropertyDescriptor _descriptor;

		// Token: 0x040024D5 RID: 9429
		private bool _isDependencyPropertyCached;

		// Token: 0x040024D6 RID: 9430
		private DependencyProperty _dependencyProperty;

		// Token: 0x040024D7 RID: 9431
		private bool _isAttached;
	}
}
