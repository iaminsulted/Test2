using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000526 RID: 1318
	internal class ElementMarkupObject : MarkupObject
	{
		// Token: 0x06004188 RID: 16776 RVA: 0x002188B8 File Offset: 0x002178B8
		internal ElementMarkupObject(object instance, XamlDesignerSerializationManager manager)
		{
			this._instance = instance;
			this._context = new ElementMarkupObject.ElementObjectContext(this, null);
			this._manager = manager;
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x002188DB File Offset: 0x002178DB
		public override Type ObjectType
		{
			get
			{
				return this._instance.GetType();
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x002188E8 File Offset: 0x002178E8
		public override object Instance
		{
			get
			{
				return this._instance;
			}
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x002188F0 File Offset: 0x002178F0
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(this.ObjectType, this.Context);
			if (serializerFor != null && serializerFor.CanConvertToString(this._instance, this.Context))
			{
				yield return new ElementStringValueProperty(this);
				if (this._key != null)
				{
					yield return this._key;
				}
			}
			else
			{
				Dictionary<string, string> constructorArguments = null;
				IEnumerator enumerator;
				if (mapToConstructorArgs && this._instance is MarkupExtension)
				{
					ParameterInfo[] parameters;
					ICollection collection;
					if (this.TryGetConstructorInfoArguments(this._instance, out parameters, out collection))
					{
						int i = 0;
						foreach (object obj in collection)
						{
							if (constructorArguments == null)
							{
								constructorArguments = new Dictionary<string, string>();
							}
							constructorArguments.Add(parameters[i].Name, parameters[i].Name);
							object value = obj;
							ParameterInfo[] array = parameters;
							int num = i;
							i = num + 1;
							yield return new ElementConstructorArgument(value, array[num].ParameterType, this);
						}
						enumerator = null;
					}
					parameters = null;
				}
				foreach (object obj2 in TypeDescriptor.GetProperties(this._instance))
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
					DesignerSerializationVisibility serializationVisibility = propertyDescriptor.SerializationVisibility;
					if (serializationVisibility != DesignerSerializationVisibility.Hidden && (!propertyDescriptor.IsReadOnly || serializationVisibility == DesignerSerializationVisibility.Content) && ElementMarkupObject.ShouldSerialize(propertyDescriptor, this._instance, this._manager))
					{
						if (serializationVisibility == DesignerSerializationVisibility.Content)
						{
							object value2 = propertyDescriptor.GetValue(this._instance);
							if (value2 == null)
							{
								continue;
							}
							ICollection collection2 = value2 as ICollection;
							if (collection2 != null && collection2.Count < 1)
							{
								continue;
							}
							IEnumerable enumerable = value2 as IEnumerable;
							if (enumerable != null && !enumerable.GetEnumerator().MoveNext())
							{
								continue;
							}
						}
						if (constructorArguments != null)
						{
							ConstructorArgumentAttribute constructorArgumentAttribute = propertyDescriptor.Attributes[typeof(ConstructorArgumentAttribute)] as ConstructorArgumentAttribute;
							if (constructorArgumentAttribute != null && constructorArguments.ContainsKey(constructorArgumentAttribute.ArgumentName))
							{
								continue;
							}
						}
						yield return new ElementProperty(this, propertyDescriptor);
					}
				}
				enumerator = null;
				IDictionary dictionary = this._instance as IDictionary;
				if (dictionary != null)
				{
					yield return new ElementDictionaryItemsPseudoProperty(dictionary, typeof(IDictionary), this);
				}
				else
				{
					IEnumerable enumerable2 = this._instance as IEnumerable;
					if (enumerable2 != null && enumerable2.GetEnumerator().MoveNext())
					{
						yield return new ElementItemsPseudoProperty(enumerable2, typeof(IEnumerable), this);
					}
				}
				if (this._key != null)
				{
					yield return this._key;
				}
				constructorArguments = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x00218907 File Offset: 0x00217907
		public override AttributeCollection Attributes
		{
			get
			{
				return TypeDescriptor.GetAttributes(this.ObjectType);
			}
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x00218914 File Offset: 0x00217914
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = new ElementMarkupObject.ElementObjectContext(this, context);
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x00218923 File Offset: 0x00217923
		internal IValueSerializerContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x0021892B File Offset: 0x0021792B
		internal XamlDesignerSerializationManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x00218933 File Offset: 0x00217933
		internal void SetKey(ElementKey key)
		{
			this._key = key;
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x0021893C File Offset: 0x0021793C
		private static bool ShouldSerialize(PropertyDescriptor pd, object instance, XamlDesignerSerializationManager manager)
		{
			object obj = instance;
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(pd);
			MethodInfo methodInfo;
			if (dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached)
			{
				Type ownerType = dependencyPropertyDescriptor.DependencyProperty.OwnerType;
				string name = dependencyPropertyDescriptor.DependencyProperty.Name;
				string propertyName = name + "!";
				if (!ElementMarkupObject.TryGetShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(ownerType, propertyName), out methodInfo))
				{
					string name2 = "ShouldSerialize" + name;
					methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObject, null);
					if (methodInfo == null)
					{
						methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsManager, null);
					}
					if (methodInfo == null)
					{
						methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsMode, null);
					}
					if (methodInfo == null)
					{
						methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObjectManager, null);
					}
					if (methodInfo != null && methodInfo.ReturnType != typeof(bool))
					{
						methodInfo = null;
					}
					ElementMarkupObject.CacheShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(ownerType, propertyName), methodInfo);
				}
				obj = null;
			}
			else if (!ElementMarkupObject.TryGetShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(instance.GetType(), pd.Name), out methodInfo))
			{
				Type type = instance.GetType();
				string name3 = "ShouldSerialize" + pd.Name;
				methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObject, null);
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsManager, null);
				}
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsMode, null);
				}
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObjectManager, null);
				}
				if (methodInfo != null && methodInfo.ReturnType != typeof(bool))
				{
					methodInfo = null;
				}
				ElementMarkupObject.CacheShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(type, pd.Name), methodInfo);
			}
			if (methodInfo != null)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters != null)
				{
					object[] parameters2;
					if (parameters.Length == 1)
					{
						if (parameters[0].ParameterType == typeof(DependencyObject))
						{
							parameters2 = new object[]
							{
								instance as DependencyObject
							};
						}
						else if (parameters[0].ParameterType == typeof(XamlWriterMode))
						{
							parameters2 = new object[]
							{
								manager.XamlWriterMode
							};
						}
						else
						{
							parameters2 = new object[]
							{
								manager
							};
						}
					}
					else
					{
						parameters2 = new object[]
						{
							instance as DependencyObject,
							manager
						};
					}
					return (bool)methodInfo.Invoke(obj, parameters2);
				}
			}
			return pd.ShouldSerializeValue(instance);
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x00218BD4 File Offset: 0x00217BD4
		private static bool TryGetShouldSerializeMethod(ElementMarkupObject.ShouldSerializeKey key, out MethodInfo methodInfo)
		{
			object obj = ElementMarkupObject._shouldSerializeCache[key];
			if (obj == null || obj == ElementMarkupObject._shouldSerializeCacheLock)
			{
				methodInfo = null;
				return obj != null;
			}
			methodInfo = (obj as MethodInfo);
			return true;
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x00218C10 File Offset: 0x00217C10
		private static void CacheShouldSerializeMethod(ElementMarkupObject.ShouldSerializeKey key, MethodInfo methodInfo)
		{
			object value = (methodInfo == null) ? ElementMarkupObject._shouldSerializeCacheLock : methodInfo;
			object shouldSerializeCacheLock = ElementMarkupObject._shouldSerializeCacheLock;
			lock (shouldSerializeCacheLock)
			{
				ElementMarkupObject._shouldSerializeCache[key] = value;
			}
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x00218C6C File Offset: 0x00217C6C
		private bool TryGetConstructorInfoArguments(object instance, out ParameterInfo[] parameters, out ICollection arguments)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(instance);
			if (converter != null && converter.CanConvertTo(this.Context, typeof(InstanceDescriptor)))
			{
				InstanceDescriptor instanceDescriptor;
				try
				{
					instanceDescriptor = (converter.ConvertTo(this._context, TypeConverterHelper.InvariantEnglishUS, instance, typeof(InstanceDescriptor)) as InstanceDescriptor);
				}
				catch (InvalidOperationException)
				{
					instanceDescriptor = null;
				}
				catch (NotSupportedException)
				{
					instanceDescriptor = null;
				}
				if (instanceDescriptor != null)
				{
					ConstructorInfo constructorInfo = instanceDescriptor.MemberInfo as ConstructorInfo;
					if (constructorInfo != null)
					{
						ParameterInfo[] parameters2 = constructorInfo.GetParameters();
						if (parameters2 != null && parameters2.Length == instanceDescriptor.Arguments.Count)
						{
							parameters = parameters2;
							arguments = instanceDescriptor.Arguments;
							return true;
						}
					}
				}
			}
			parameters = null;
			arguments = null;
			return false;
		}

		// Token: 0x040024C2 RID: 9410
		private static object _shouldSerializeCacheLock = new object();

		// Token: 0x040024C3 RID: 9411
		private static Hashtable _shouldSerializeCache = new Hashtable();

		// Token: 0x040024C4 RID: 9412
		private static Type[] _shouldSerializeArgsObject = new Type[]
		{
			typeof(DependencyObject)
		};

		// Token: 0x040024C5 RID: 9413
		private static Type[] _shouldSerializeArgsManager = new Type[]
		{
			typeof(XamlDesignerSerializationManager)
		};

		// Token: 0x040024C6 RID: 9414
		private static Type[] _shouldSerializeArgsMode = new Type[]
		{
			typeof(XamlWriterMode)
		};

		// Token: 0x040024C7 RID: 9415
		private static Type[] _shouldSerializeArgsObjectManager = new Type[]
		{
			typeof(DependencyObject),
			typeof(XamlDesignerSerializationManager)
		};

		// Token: 0x040024C8 RID: 9416
		private static Attribute[] _propertyAttributes = new Attribute[]
		{
			new PropertyFilterAttribute(PropertyFilterOptions.SetValues)
		};

		// Token: 0x040024C9 RID: 9417
		private object _instance;

		// Token: 0x040024CA RID: 9418
		private IValueSerializerContext _context;

		// Token: 0x040024CB RID: 9419
		private ElementKey _key;

		// Token: 0x040024CC RID: 9420
		private XamlDesignerSerializationManager _manager;

		// Token: 0x02000B08 RID: 2824
		private sealed class ElementObjectContext : ValueSerializerContextWrapper, IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06008BD6 RID: 35798 RVA: 0x0033AE77 File Offset: 0x00339E77
			public ElementObjectContext(ElementMarkupObject obj, IValueSerializerContext baseContext) : base(baseContext)
			{
				this._object = obj;
			}

			// Token: 0x17001EA6 RID: 7846
			// (get) Token: 0x06008BD7 RID: 35799 RVA: 0x0033AE87 File Offset: 0x00339E87
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return this._object.Instance;
				}
			}

			// Token: 0x04004785 RID: 18309
			private ElementMarkupObject _object;
		}

		// Token: 0x02000B09 RID: 2825
		private struct ShouldSerializeKey
		{
			// Token: 0x06008BD8 RID: 35800 RVA: 0x0033AE94 File Offset: 0x00339E94
			public ShouldSerializeKey(Type type, string propertyName)
			{
				this._type = type;
				this._propertyName = propertyName;
			}

			// Token: 0x06008BD9 RID: 35801 RVA: 0x0033AEA4 File Offset: 0x00339EA4
			public override bool Equals(object obj)
			{
				if (!(obj is ElementMarkupObject.ShouldSerializeKey))
				{
					return false;
				}
				ElementMarkupObject.ShouldSerializeKey shouldSerializeKey = (ElementMarkupObject.ShouldSerializeKey)obj;
				return shouldSerializeKey._type == this._type && shouldSerializeKey._propertyName == this._propertyName;
			}

			// Token: 0x06008BDA RID: 35802 RVA: 0x0033AEE8 File Offset: 0x00339EE8
			public static bool operator ==(ElementMarkupObject.ShouldSerializeKey key1, ElementMarkupObject.ShouldSerializeKey key2)
			{
				return key1.Equals(key2);
			}

			// Token: 0x06008BDB RID: 35803 RVA: 0x0033AEFD File Offset: 0x00339EFD
			public static bool operator !=(ElementMarkupObject.ShouldSerializeKey key1, ElementMarkupObject.ShouldSerializeKey key2)
			{
				return !key1.Equals(key2);
			}

			// Token: 0x06008BDC RID: 35804 RVA: 0x0033AF15 File Offset: 0x00339F15
			public override int GetHashCode()
			{
				return this._type.GetHashCode() ^ this._propertyName.GetHashCode();
			}

			// Token: 0x04004786 RID: 18310
			private Type _type;

			// Token: 0x04004787 RID: 18311
			private string _propertyName;
		}
	}
}
