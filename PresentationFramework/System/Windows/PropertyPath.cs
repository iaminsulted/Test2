using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x02000389 RID: 905
	[TypeConverter(typeof(PropertyPathConverter))]
	public sealed class PropertyPath
	{
		// Token: 0x0600245B RID: 9307 RVA: 0x00182424 File Offset: 0x00181424
		public PropertyPath(string path, params object[] pathParameters)
		{
			if (Dispatcher.CurrentDispatcher == null)
			{
				throw new InvalidOperationException();
			}
			this._path = path;
			if (pathParameters != null && pathParameters.Length != 0)
			{
				PropertyPath.PathParameterCollection pathParameterCollection = new PropertyPath.PathParameterCollection(pathParameters);
				this.SetPathParameterCollection(pathParameterCollection);
			}
			this.PrepareSourceValueInfo(null);
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0018247D File Offset: 0x0018147D
		public PropertyPath(object parameter) : this("(0)", new object[]
		{
			parameter
		})
		{
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x00182494 File Offset: 0x00181494
		internal PropertyPath(string path, ITypeDescriptorContext typeDescriptorContext)
		{
			this._path = path;
			this.PrepareSourceValueInfo(typeDescriptorContext);
			this.NormalizePath();
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x001824C6 File Offset: 0x001814C6
		// (set) Token: 0x0600245F RID: 9311 RVA: 0x001824CE File Offset: 0x001814CE
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				this.PrepareSourceValueInfo(null);
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x001824DE File Offset: 0x001814DE
		public Collection<object> PathParameters
		{
			get
			{
				if (this._parameters == null)
				{
					this.SetPathParameterCollection(new PropertyPath.PathParameterCollection());
				}
				return this._parameters;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x001824F9 File Offset: 0x001814F9
		internal int Length
		{
			get
			{
				return this._arySVI.Length;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x00182503 File Offset: 0x00181503
		internal PropertyPathStatus Status
		{
			get
			{
				return this.SingleWorker.Status;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x00182510 File Offset: 0x00181510
		internal string LastError
		{
			get
			{
				return this._lastError;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x00182518 File Offset: 0x00181518
		internal object LastItem
		{
			get
			{
				return this.GetItem(this.Length - 1);
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x00182528 File Offset: 0x00181528
		internal object LastAccessor
		{
			get
			{
				return this.GetAccessor(this.Length - 1);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x00182538 File Offset: 0x00181538
		internal object[] LastIndexerArguments
		{
			get
			{
				return this.GetIndexerArguments(this.Length - 1);
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x00182548 File Offset: 0x00181548
		internal bool StartsWithStaticProperty
		{
			get
			{
				return this.Length > 0 && PropertyPath.IsStaticProperty(this._earlyBoundPathParts[0]);
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00182564 File Offset: 0x00181564
		internal static bool IsStaticProperty(object accessor)
		{
			DependencyProperty dependencyProperty;
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DynamicObjectAccessor dynamicObjectAccessor;
			PropertyPath.DowncastAccessor(accessor, out dependencyProperty, out propertyInfo, out propertyDescriptor, out dynamicObjectAccessor);
			if (propertyInfo != null)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				return getMethod != null && getMethod.IsStatic;
			}
			return false;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x001825A4 File Offset: 0x001815A4
		internal static void DowncastAccessor(object accessor, out DependencyProperty dp, out PropertyInfo pi, out PropertyDescriptor pd, out DynamicObjectAccessor doa)
		{
			DependencyProperty dependencyProperty;
			dp = (dependencyProperty = (accessor as DependencyProperty));
			if (dependencyProperty != null)
			{
				pd = null;
				pi = null;
				doa = null;
				return;
			}
			PropertyInfo left;
			pi = (left = (accessor as PropertyInfo));
			if (left != null)
			{
				pd = null;
				doa = null;
				return;
			}
			PropertyDescriptor propertyDescriptor;
			pd = (propertyDescriptor = (accessor as PropertyDescriptor));
			if (propertyDescriptor != null)
			{
				doa = null;
				return;
			}
			doa = (accessor as DynamicObjectAccessor);
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x001825FF File Offset: 0x001815FF
		internal IDisposable SetContext(object rootItem)
		{
			return this.SingleWorker.SetContext(rootItem);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0018260D File Offset: 0x0018160D
		internal object GetItem(int k)
		{
			return this.SingleWorker.GetItem(k);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0018261C File Offset: 0x0018161C
		internal object GetAccessor(int k)
		{
			object obj = this._earlyBoundPathParts[k];
			if (obj == null)
			{
				obj = this.SingleWorker.GetAccessor(k);
			}
			return obj;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x00182643 File Offset: 0x00181643
		internal object[] GetIndexerArguments(int k)
		{
			return this.SingleWorker.GetIndexerArguments(k);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x00182651 File Offset: 0x00181651
		internal object GetValue()
		{
			return this.SingleWorker.RawValue();
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x00182660 File Offset: 0x00181660
		internal int ComputeUnresolvedAttachedPropertiesInPath()
		{
			int num = 0;
			for (int i = this.Length - 1; i >= 0; i--)
			{
				if (this._earlyBoundPathParts[i] == null)
				{
					string name = this._arySVI[i].name;
					if (PropertyPath.IsPropertyReference(name) && name.IndexOf('.') >= 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x001826B6 File Offset: 0x001816B6
		internal SourceValueInfo[] SVI
		{
			get
			{
				return this._arySVI;
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x001826C0 File Offset: 0x001816C0
		internal object ResolvePropertyName(int level, object item, Type ownerType, object context)
		{
			object obj = this._earlyBoundPathParts[level];
			if (obj == null)
			{
				obj = this.ResolvePropertyName(this._arySVI[level].name, item, ownerType, context, false);
			}
			return obj;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x001826F8 File Offset: 0x001816F8
		internal IndexerParameterInfo[] ResolveIndexerParams(int level, object context)
		{
			IndexerParameterInfo[] array = this._earlyBoundPathParts[level] as IndexerParameterInfo[];
			if (array == null)
			{
				array = this.ResolveIndexerParams(this._arySVI[level].paramList, context, false);
			}
			return array;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00182734 File Offset: 0x00181734
		internal void ReplaceIndexerByProperty(int level, string name)
		{
			this._arySVI[level].name = name;
			this._arySVI[level].propertyName = name;
			this._arySVI[level].type = SourceValueType.Property;
			this._earlyBoundPathParts[level] = null;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x00182780 File Offset: 0x00181780
		private PropertyPathWorker SingleWorker
		{
			get
			{
				if (this._singleWorker == null)
				{
					this._singleWorker = new PropertyPathWorker(this);
				}
				return this._singleWorker;
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0018279C File Offset: 0x0018179C
		private void PrepareSourceValueInfo(ITypeDescriptorContext typeDescriptorContext)
		{
			PathParser pathParser = DataBindEngine.CurrentDataBindEngine.PathParser;
			this._arySVI = pathParser.Parse(this.Path);
			if (this._arySVI.Length == 0)
			{
				string text = pathParser.Error;
				if (text == null)
				{
					text = this.Path;
				}
				throw new InvalidOperationException(SR.Get("PropertyPathSyntaxError", new object[]
				{
					text
				}));
			}
			this.ResolvePathParts(typeDescriptorContext);
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x00182804 File Offset: 0x00181804
		private void NormalizePath()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PropertyPath.PathParameterCollection pathParameterCollection = new PropertyPath.PathParameterCollection();
			for (int i = 0; i < this._arySVI.Length; i++)
			{
				switch (this._arySVI[i].drillIn)
				{
				case DrillIn.Never:
					if (this._arySVI[i].type == SourceValueType.Property)
					{
						stringBuilder.Append('.');
					}
					break;
				case DrillIn.Always:
					stringBuilder.Append('/');
					break;
				}
				switch (this._arySVI[i].type)
				{
				case SourceValueType.Property:
					if (this._earlyBoundPathParts[i] != null)
					{
						stringBuilder.Append('(');
						stringBuilder.Append(pathParameterCollection.Count.ToString(TypeConverterHelper.InvariantEnglishUS.NumberFormat));
						stringBuilder.Append(')');
						pathParameterCollection.Add(this._earlyBoundPathParts[i]);
					}
					else
					{
						stringBuilder.Append(this._arySVI[i].name);
					}
					break;
				case SourceValueType.Indexer:
					stringBuilder.Append('[');
					if (this._earlyBoundPathParts[i] != null)
					{
						IndexerParameterInfo[] array = (IndexerParameterInfo[])this._earlyBoundPathParts[i];
						int num = 0;
						for (;;)
						{
							IndexerParameterInfo indexerParameterInfo = array[num];
							if (indexerParameterInfo.type != null)
							{
								stringBuilder.Append('(');
								stringBuilder.Append(pathParameterCollection.Count.ToString(TypeConverterHelper.InvariantEnglishUS.NumberFormat));
								stringBuilder.Append(')');
								pathParameterCollection.Add(indexerParameterInfo.value);
							}
							else
							{
								stringBuilder.Append(indexerParameterInfo.value);
							}
							num++;
							if (num >= array.Length)
							{
								break;
							}
							stringBuilder.Append(',');
						}
					}
					else
					{
						stringBuilder.Append(this._arySVI[i].name);
					}
					stringBuilder.Append(']');
					break;
				}
			}
			if (pathParameterCollection.Count > 0)
			{
				this._path = stringBuilder.ToString();
				this.SetPathParameterCollection(pathParameterCollection);
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00182A04 File Offset: 0x00181A04
		private void SetPathParameterCollection(PropertyPath.PathParameterCollection parameters)
		{
			if (this._parameters != null)
			{
				this._parameters.CollectionChanged -= this.ParameterCollectionChanged;
			}
			this._parameters = parameters;
			if (this._parameters != null)
			{
				this._parameters.CollectionChanged += this.ParameterCollectionChanged;
			}
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00182A56 File Offset: 0x00181A56
		private void ParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.PrepareSourceValueInfo(null);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x00182A60 File Offset: 0x00181A60
		private void ResolvePathParts(ITypeDescriptorContext typeDescriptorContext)
		{
			bool throwOnError = typeDescriptorContext != null;
			object obj = null;
			TypeConvertContext typeConvertContext = typeDescriptorContext as TypeConvertContext;
			if (typeConvertContext != null)
			{
				obj = typeConvertContext.ParserContext;
			}
			if (obj == null)
			{
				obj = typeDescriptorContext;
			}
			this._earlyBoundPathParts = new object[this.Length];
			for (int i = this.Length - 1; i >= 0; i--)
			{
				if (this._arySVI[i].type == SourceValueType.Property)
				{
					string name = this._arySVI[i].name;
					if (PropertyPath.IsPropertyReference(name))
					{
						object obj2 = this.ResolvePropertyName(name, null, null, obj, throwOnError);
						this._earlyBoundPathParts[i] = obj2;
						if (obj2 != null)
						{
							this._arySVI[i].propertyName = PropertyPath.GetPropertyName(obj2);
						}
					}
					else
					{
						this._arySVI[i].propertyName = name;
					}
				}
				else if (this._arySVI[i].type == SourceValueType.Indexer)
				{
					IndexerParameterInfo[] array = this.ResolveIndexerParams(this._arySVI[i].paramList, obj, throwOnError);
					this._earlyBoundPathParts[i] = array;
					this._arySVI[i].propertyName = "Item[]";
				}
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00182B80 File Offset: 0x00181B80
		private object ResolvePropertyName(string name, object item, Type ownerType, object context, bool throwOnError)
		{
			string text = name;
			int num;
			if (PropertyPath.IsParameterIndex(name, out num))
			{
				if (0 <= num && num < this.PathParameters.Count)
				{
					object obj = this.PathParameters[num];
					if (!PropertyPath.IsValidAccessor(obj))
					{
						throw new InvalidOperationException(SR.Get("PropertyPathInvalidAccessor", new object[]
						{
							(obj != null) ? obj.GetType().FullName : "null"
						}));
					}
					return obj;
				}
				else
				{
					if (throwOnError)
					{
						throw new InvalidOperationException(SR.Get("PathParametersIndexOutOfRange", new object[]
						{
							num,
							this.PathParameters.Count
						}));
					}
					return null;
				}
			}
			else
			{
				if (PropertyPath.IsPropertyReference(name))
				{
					name = name.Substring(1, name.Length - 2);
					int num2 = name.LastIndexOf('.');
					if (num2 >= 0)
					{
						text = name.Substring(num2 + 1).Trim();
						string text2 = name.Substring(0, num2).Trim();
						ownerType = this.GetTypeFromName(text2, context);
						if (ownerType == null && throwOnError)
						{
							throw new InvalidOperationException(SR.Get("PropertyPathNoOwnerType", new object[]
							{
								text2
							}));
						}
					}
					else
					{
						text = name;
					}
				}
				if (!(ownerType != null))
				{
					return null;
				}
				object obj2 = DependencyProperty.FromName(text, ownerType);
				if (obj2 == null && item is ICustomTypeDescriptor)
				{
					obj2 = TypeDescriptor.GetProperties(item)[text];
				}
				if (obj2 == null && (item is INotifyPropertyChanged || item is DependencyObject))
				{
					obj2 = this.GetPropertyHelper(ownerType, text);
				}
				if (obj2 == null && item != null)
				{
					obj2 = TypeDescriptor.GetProperties(item)[text];
				}
				if (obj2 == null)
				{
					obj2 = this.GetPropertyHelper(ownerType, text);
				}
				if (obj2 == null && SystemCoreHelper.IsIDynamicMetaObjectProvider(item))
				{
					obj2 = SystemCoreHelper.NewDynamicPropertyAccessor(item.GetType(), text);
				}
				if (obj2 == null && throwOnError)
				{
					throw new InvalidOperationException(SR.Get("PropertyPathNoProperty", new object[]
					{
						ownerType.Name,
						text
					}));
				}
				return obj2;
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00182D60 File Offset: 0x00181D60
		private PropertyInfo GetPropertyHelper(Type ownerType, string propertyName)
		{
			PropertyInfo propertyInfo = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				propertyInfo = ownerType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			}
			catch (AmbiguousMatchException)
			{
				flag = true;
			}
			if (flag)
			{
				try
				{
					propertyInfo = null;
					while (propertyInfo == null && ownerType != null)
					{
						propertyInfo = ownerType.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
						ownerType = ownerType.BaseType;
					}
				}
				catch (AmbiguousMatchException)
				{
					flag2 = true;
				}
			}
			if (PropertyPathWorker.IsIndexedProperty(propertyInfo))
			{
				flag2 = true;
			}
			if (flag2)
			{
				propertyInfo = IndexerPropertyInfo.Instance;
			}
			return propertyInfo;
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x00182DE8 File Offset: 0x00181DE8
		private IndexerParameterInfo[] ResolveIndexerParams(FrugalObjectList<IndexerParamInfo> paramList, object context, bool throwOnError)
		{
			IndexerParameterInfo[] array = new IndexerParameterInfo[paramList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (string.IsNullOrEmpty(paramList[i].parenString))
				{
					array[i].value = paramList[i].valueString;
				}
				else if (string.IsNullOrEmpty(paramList[i].valueString))
				{
					int num;
					if (int.TryParse(paramList[i].parenString.Trim(), NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out num))
					{
						if (0 <= num && num < this.PathParameters.Count)
						{
							object obj = this.PathParameters[num];
							if (obj != null)
							{
								array[i].value = obj;
								array[i].type = obj.GetType();
							}
							else if (throwOnError)
							{
								throw new InvalidOperationException(SR.Get("PathParameterIsNull", new object[]
								{
									num
								}));
							}
						}
						else if (throwOnError)
						{
							throw new InvalidOperationException(SR.Get("PathParametersIndexOutOfRange", new object[]
							{
								num,
								this.PathParameters.Count
							}));
						}
					}
					else
					{
						array[i].value = "(" + paramList[i].parenString + ")";
					}
				}
				else
				{
					array[i].type = this.GetTypeFromName(paramList[i].parenString, context);
					if (array[i].type != null)
					{
						object typedParamValue = this.GetTypedParamValue(paramList[i].valueString.Trim(), array[i].type, throwOnError);
						if (typedParamValue != null)
						{
							array[i].value = typedParamValue;
						}
						else
						{
							if (throwOnError)
							{
								throw new InvalidOperationException(SR.Get("PropertyPathIndexWrongType", new object[]
								{
									paramList[i].parenString,
									paramList[i].valueString
								}));
							}
							array[i].type = null;
						}
					}
					else
					{
						array[i].value = "(" + paramList[i].parenString + ")" + paramList[i].valueString;
					}
				}
			}
			return array;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0018303C File Offset: 0x0018203C
		private object GetTypedParamValue(string param, Type type, bool throwOnError)
		{
			object obj = null;
			if (type == typeof(string))
			{
				return param;
			}
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			if (converter != null && converter.CanConvertFrom(typeof(string)))
			{
				try
				{
					obj = converter.ConvertFromString(null, CultureInfo.InvariantCulture, param);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalApplicationException(ex) || throwOnError)
					{
						throw;
					}
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}
				}
			}
			if (obj == null && type.IsAssignableFrom(typeof(string)))
			{
				obj = param;
			}
			return obj;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x001830D4 File Offset: 0x001820D4
		private Type GetTypeFromName(string name, object context)
		{
			ParserContext parserContext = context as ParserContext;
			if (parserContext == null)
			{
				if (context is IServiceProvider)
				{
					IXamlTypeResolver xamlTypeResolver = (context as IServiceProvider).GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
					if (xamlTypeResolver != null)
					{
						return xamlTypeResolver.Resolve(name);
					}
				}
				IValueSerializerContext valueSerializerContext = context as IValueSerializerContext;
				if (valueSerializerContext != null)
				{
					ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(typeof(Type), valueSerializerContext);
					if (serializerFor != null)
					{
						return serializerFor.ConvertFromString(name, valueSerializerContext) as Type;
					}
				}
				DependencyObject dependencyObject = context as DependencyObject;
				if (dependencyObject == null)
				{
					if (FrameworkCompatibilityPreferences.TargetsDesktop_V4_0)
					{
						return null;
					}
					dependencyObject = new DependencyObject();
				}
				return XamlReader.BamlSharedSchemaContext.ResolvePrefixedNameWithAdditionalWpfSemantics(name, dependencyObject);
			}
			int num = name.IndexOf(':');
			string text;
			if (num == -1)
			{
				text = string.Empty;
			}
			else
			{
				text = name.Substring(0, num).TrimEnd();
				name = name.Substring(num + 1).TrimStart();
			}
			string text2 = parserContext.XmlnsDictionary[text];
			if (text2 == null)
			{
				throw new ArgumentException(SR.Get("ParserPrefixNSProperty", new object[]
				{
					text,
					name
				}));
			}
			TypeAndSerializer typeOnly = parserContext.XamlTypeMapper.GetTypeOnly(text2, name);
			if (typeOnly == null)
			{
				return null;
			}
			return typeOnly.ObjectType;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x001831F8 File Offset: 0x001821F8
		internal static bool IsPropertyReference(string name)
		{
			return name != null && name.Length > 1 && name[0] == '(' && name[name.Length - 1] == ')';
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00183225 File Offset: 0x00182225
		internal static bool IsParameterIndex(string name, out int index)
		{
			if (PropertyPath.IsPropertyReference(name))
			{
				name = name.Substring(1, name.Length - 2);
				return int.TryParse(name, NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out index);
			}
			index = -1;
			return false;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00183259 File Offset: 0x00182259
		private static bool IsValidAccessor(object accessor)
		{
			return accessor is DependencyProperty || accessor is PropertyInfo || accessor is PropertyDescriptor || accessor is DynamicObjectAccessor;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00183280 File Offset: 0x00182280
		private static string GetPropertyName(object accessor)
		{
			DependencyProperty dependencyProperty;
			if ((dependencyProperty = (accessor as DependencyProperty)) != null)
			{
				return dependencyProperty.Name;
			}
			PropertyInfo propertyInfo;
			if ((propertyInfo = (accessor as PropertyInfo)) != null)
			{
				return propertyInfo.Name;
			}
			PropertyDescriptor propertyDescriptor;
			if ((propertyDescriptor = (accessor as PropertyDescriptor)) != null)
			{
				return propertyDescriptor.Name;
			}
			DynamicObjectAccessor dynamicObjectAccessor;
			if ((dynamicObjectAccessor = (accessor as DynamicObjectAccessor)) != null)
			{
				return dynamicObjectAccessor.PropertyName;
			}
			Invariant.Assert(false, "Unknown accessor type");
			return null;
		}

		// Token: 0x04001145 RID: 4421
		private const string SingleStepPath = "(0)";

		// Token: 0x04001146 RID: 4422
		private static readonly char[] s_comma = new char[]
		{
			','
		};

		// Token: 0x04001147 RID: 4423
		private string _path = string.Empty;

		// Token: 0x04001148 RID: 4424
		private PropertyPath.PathParameterCollection _parameters;

		// Token: 0x04001149 RID: 4425
		private SourceValueInfo[] _arySVI;

		// Token: 0x0400114A RID: 4426
		private string _lastError = string.Empty;

		// Token: 0x0400114B RID: 4427
		private object[] _earlyBoundPathParts;

		// Token: 0x0400114C RID: 4428
		private PropertyPathWorker _singleWorker;

		// Token: 0x02000A84 RID: 2692
		private class PathParameterCollection : ObservableCollection<object>
		{
			// Token: 0x0600866E RID: 34414 RVA: 0x0032A7A7 File Offset: 0x003297A7
			public PathParameterCollection()
			{
			}

			// Token: 0x0600866F RID: 34415 RVA: 0x0032A7B0 File Offset: 0x003297B0
			public PathParameterCollection(object[] parameters)
			{
				IList<object> items = base.Items;
				foreach (object item in parameters)
				{
					items.Add(item);
				}
			}
		}
	}
}
