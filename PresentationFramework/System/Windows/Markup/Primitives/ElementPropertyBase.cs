using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000527 RID: 1319
	internal abstract class ElementPropertyBase : MarkupProperty
	{
		// Token: 0x06004196 RID: 16790 RVA: 0x00218DD2 File Offset: 0x00217DD2
		public ElementPropertyBase(XamlDesignerSerializationManager manager)
		{
			this._manager = manager;
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x00218DE4 File Offset: 0x00217DE4
		public override bool IsComposite
		{
			get
			{
				if (!this._isCompositeCalculated)
				{
					this._isCompositeCalculated = true;
					object value = this.Value;
					if (value == null)
					{
						this._isComposite = true;
					}
					else if (value is string && this.PropertyType.IsAssignableFrom(typeof(object)))
					{
						this._isComposite = false;
					}
					else if (value is MarkupExtension)
					{
						this._isComposite = true;
					}
					else
					{
						this._isComposite = !this.CanConvertToString(value);
					}
				}
				return this._isComposite;
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x00218E64 File Offset: 0x00217E64
		public override string StringValue
		{
			get
			{
				if (this.IsComposite)
				{
					return string.Empty;
				}
				object value = this.Value;
				string text = value as string;
				if (text != null)
				{
					return text;
				}
				ValueSerializer valueSerializer = this.GetValueSerializer();
				if (valueSerializer == null)
				{
					return string.Empty;
				}
				return valueSerializer.ConvertToString(value, this.Context);
			}
		}

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x00218EAF File Offset: 0x00217EAF
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				object value = this.Value;
				if (value != null)
				{
					if (this.PropertyDescriptor != null && (this.PropertyDescriptor.IsReadOnly || (!this.PropertyIsAttached(this.PropertyDescriptor) && this.PropertyType == value.GetType() && (typeof(IList).IsAssignableFrom(this.PropertyType) || typeof(IDictionary).IsAssignableFrom(this.PropertyType) || typeof(Freezable).IsAssignableFrom(this.PropertyType) || typeof(FrameworkElementFactory).IsAssignableFrom(this.PropertyType)) && this.HasNoSerializableProperties(value) && !this.IsEmpty(value))))
					{
						IDictionary dictionary = value as IDictionary;
						if (dictionary != null)
						{
							Type keyType = ElementPropertyBase.GetDictionaryKeyType(dictionary);
							DictionaryEntry[] array = new DictionaryEntry[dictionary.Count];
							dictionary.CopyTo(array, 0);
							Array.Sort<DictionaryEntry>(array, (DictionaryEntry one, DictionaryEntry two) => string.Compare(one.Key.ToString(), two.Key.ToString()));
							foreach (DictionaryEntry dictionaryEntry in array)
							{
								ElementMarkupObject elementMarkupObject = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), dictionaryEntry.Value, this.Context, false), this.Manager);
								elementMarkupObject.SetKey(new ElementKey(dictionaryEntry.Key, keyType, elementMarkupObject));
								yield return elementMarkupObject;
							}
							DictionaryEntry[] array2 = null;
							keyType = null;
						}
						else
						{
							IEnumerable enumerable = value as IEnumerable;
							if (enumerable != null)
							{
								foreach (object value2 in enumerable)
								{
									MarkupObject markupObject = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), value2, this.Context, false), this.Manager);
									yield return markupObject;
								}
								IEnumerator enumerator = null;
							}
							else if (this.PropertyType == typeof(FrameworkElementFactory) && value is FrameworkElementFactory)
							{
								MarkupObject markupObject2 = new FrameworkElementFactoryMarkupObject(value as FrameworkElementFactory, this.Manager);
								yield return markupObject2;
							}
							else
							{
								MarkupObject markupObject3 = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), value, this.Context, true), this.Manager);
								yield return markupObject3;
							}
						}
					}
					else
					{
						MarkupObject markupObject4 = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), value, this.Context, true), this.Manager);
						yield return markupObject4;
					}
				}
				else
				{
					MarkupObject markupObject5 = new ElementMarkupObject(new NullExtension(), this.Manager);
					yield return markupObject5;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x00218EC0 File Offset: 0x00217EC0
		private bool PropertyIsAttached(PropertyDescriptor descriptor)
		{
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(this.PropertyDescriptor);
			return dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached;
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x00218EE4 File Offset: 0x00217EE4
		private bool HasNoSerializableProperties(object value)
		{
			if (value is FrameworkElementFactory)
			{
				return true;
			}
			using (IEnumerator<MarkupProperty> enumerator = new ElementMarkupObject(value, this.Manager).Properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsContent)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x00218F4C File Offset: 0x00217F4C
		private bool IsEmpty(object value)
		{
			IEnumerable enumerable = value as IEnumerable;
			if (enumerable != null)
			{
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x00218FA8 File Offset: 0x00217FA8
		protected IValueSerializerContext Context
		{
			get
			{
				if (this._context == null)
				{
					this._context = new ElementPropertyBase.ElementPropertyContext(this, this.GetItemContext());
				}
				return this._context;
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x0600419E RID: 16798 RVA: 0x00218FCA File Offset: 0x00217FCA
		internal XamlDesignerSerializationManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x0600419F RID: 16799 RVA: 0x00218FD4 File Offset: 0x00217FD4
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				ValueSerializer valueSerializer = this.GetValueSerializer();
				if (valueSerializer == null)
				{
					return ElementPropertyBase.EmptyTypes;
				}
				return valueSerializer.TypeReferences(this.Value, this.Context);
			}
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x00219004 File Offset: 0x00218004
		protected bool CanConvertToString(object value)
		{
			if (value == null)
			{
				return false;
			}
			ValueSerializer valueSerializer = this.GetValueSerializer();
			return valueSerializer != null && valueSerializer.CanConvertToString(value, this.Context);
		}

		// Token: 0x060041A1 RID: 16801
		protected abstract IValueSerializerContext GetItemContext();

		// Token: 0x060041A2 RID: 16802
		protected abstract Type GetObjectType();

		// Token: 0x060041A3 RID: 16803 RVA: 0x00219030 File Offset: 0x00218030
		private ValueSerializer GetValueSerializer()
		{
			PropertyDescriptor propertyDescriptor = this.PropertyDescriptor;
			if (propertyDescriptor == null)
			{
				DependencyProperty dependencyProperty = this.DependencyProperty;
				if (dependencyProperty != null)
				{
					propertyDescriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, this.GetObjectType());
				}
			}
			if (propertyDescriptor != null)
			{
				return ValueSerializer.GetSerializerFor(propertyDescriptor, this.GetItemContext());
			}
			return ValueSerializer.GetSerializerFor(this.PropertyType, this.GetItemContext());
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x00219080 File Offset: 0x00218080
		private static Type GetDictionaryKeyType(IDictionary value)
		{
			Type type = value.GetType();
			if (ElementPropertyBase._keyTypeMap == null)
			{
				ElementPropertyBase._keyTypeMap = new Dictionary<Type, Type>();
			}
			Type typeFromHandle;
			if (!ElementPropertyBase._keyTypeMap.TryGetValue(type, out typeFromHandle))
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(IDictionary<, >))
					{
						return type2.GetGenericArguments()[0];
					}
				}
				typeFromHandle = typeof(object);
				ElementPropertyBase._keyTypeMap[type] = typeFromHandle;
			}
			return typeFromHandle;
		}

		// Token: 0x040024CD RID: 9421
		private static readonly List<Type> EmptyTypes = new List<Type>();

		// Token: 0x040024CE RID: 9422
		private static Dictionary<Type, Type> _keyTypeMap;

		// Token: 0x040024CF RID: 9423
		private bool _isComposite;

		// Token: 0x040024D0 RID: 9424
		private bool _isCompositeCalculated;

		// Token: 0x040024D1 RID: 9425
		private IValueSerializerContext _context;

		// Token: 0x040024D2 RID: 9426
		private XamlDesignerSerializationManager _manager;

		// Token: 0x02000B0B RID: 2827
		private sealed class ElementPropertyContext : ValueSerializerContextWrapper, IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06008BE7 RID: 35815 RVA: 0x0033B427 File Offset: 0x0033A427
			public ElementPropertyContext(ElementPropertyBase property, IValueSerializerContext baseContext) : base(baseContext)
			{
				this._property = property;
			}

			// Token: 0x17001EA9 RID: 7849
			// (get) Token: 0x06008BE8 RID: 35816 RVA: 0x0033B437 File Offset: 0x0033A437
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return this._property.PropertyDescriptor;
				}
			}

			// Token: 0x04004792 RID: 18322
			private ElementPropertyBase _property;
		}
	}
}
