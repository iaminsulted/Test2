using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000539 RID: 1337
	public sealed class MarkupWriter : IDisposable
	{
		// Token: 0x06004223 RID: 16931 RVA: 0x00219DE0 File Offset: 0x00218DE0
		public static MarkupObject GetMarkupObjectFor(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return new ElementMarkupObject(instance, new XamlDesignerSerializationManager(null)
			{
				XamlWriterMode = XamlWriterMode.Expression
			});
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x00219E10 File Offset: 0x00218E10
		public static MarkupObject GetMarkupObjectFor(object instance, XamlDesignerSerializationManager manager)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return new ElementMarkupObject(instance, manager);
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x00219E35 File Offset: 0x00218E35
		internal static void SaveAsXml(XmlWriter writer, object instance)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			MarkupWriter.SaveAsXml(writer, MarkupWriter.GetMarkupObjectFor(instance));
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00219E51 File Offset: 0x00218E51
		internal static void SaveAsXml(XmlWriter writer, object instance, XamlDesignerSerializationManager manager)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			manager.ClearXmlWriter();
			MarkupWriter.SaveAsXml(writer, MarkupWriter.GetMarkupObjectFor(instance, manager));
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x00219E84 File Offset: 0x00218E84
		internal static void SaveAsXml(XmlWriter writer, MarkupObject item)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			try
			{
				using (MarkupWriter markupWriter = new MarkupWriter(writer))
				{
					markupWriter.WriteItem(item);
				}
			}
			finally
			{
				writer.Flush();
			}
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x00219EEC File Offset: 0x00218EEC
		internal static void VerifyTypeIsSerializable(Type type)
		{
			if (type.IsNestedPublic)
			{
				throw new InvalidOperationException(SR.Get("MarkupWriter_CannotSerializeNestedPublictype", new object[]
				{
					type.ToString()
				}));
			}
			if (!type.IsPublic)
			{
				throw new InvalidOperationException(SR.Get("MarkupWriter_CannotSerializeNonPublictype", new object[]
				{
					type.ToString()
				}));
			}
			if (type.IsGenericType)
			{
				throw new InvalidOperationException(SR.Get("MarkupWriter_CannotSerializeGenerictype", new object[]
				{
					type.ToString()
				}));
			}
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x00219F6E File Offset: 0x00218F6E
		internal MarkupWriter(XmlWriter writer)
		{
			this._writer = writer;
			this._xmlTextWriter = (writer as XmlTextWriter);
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x00219F89 File Offset: 0x00218F89
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00219F94 File Offset: 0x00218F94
		private bool RecordNamespaces(MarkupWriter.Scope scope, MarkupObject item, IValueSerializerContext context, bool lastWasString)
		{
			bool result = true;
			if (lastWasString || item.ObjectType != typeof(string) || this.HasNonValueProperties(item))
			{
				scope.MakeAddressable(item.ObjectType);
				result = false;
			}
			item.AssignRootContext(context);
			lastWasString = false;
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				if (markupProperty.IsComposite)
				{
					bool flag = this.IsCollectionType(markupProperty.PropertyType);
					using (IEnumerator<MarkupObject> enumerator2 = markupProperty.Items.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MarkupObject item2 = enumerator2.Current;
							lastWasString = this.RecordNamespaces(scope, item2, context, lastWasString || flag);
						}
						goto IL_B7;
					}
					goto IL_AB;
				}
				goto IL_AB;
				IL_B7:
				if (markupProperty.DependencyProperty != null)
				{
					scope.MakeAddressable(markupProperty.DependencyProperty.OwnerType);
				}
				if (markupProperty.IsKey)
				{
					scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
					continue;
				}
				continue;
				IL_AB:
				scope.MakeAddressable(markupProperty.TypeReferences);
				goto IL_B7;
			}
			return result;
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x0021A0BC File Offset: 0x002190BC
		internal void WriteItem(MarkupObject item)
		{
			MarkupWriter.VerifyTypeIsSerializable(item.ObjectType);
			MarkupWriter.Scope scope = new MarkupWriter.Scope(null);
			scope.RecordMapping("", MarkupWriter.NamespaceCache.GetNamespaceUriFor(item.ObjectType));
			this.RecordNamespaces(scope, item, new MarkupWriter.MarkupWriterContext(scope), false);
			item = new ExtensionSimplifierMarkupObject(item, null);
			this.WriteItem(item, scope);
			this._writer = null;
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x0021A11C File Offset: 0x0021911C
		private void WriteItem(MarkupObject item, MarkupWriter.Scope scope)
		{
			MarkupWriter.VerifyTypeIsSerializable(item.ObjectType);
			MarkupWriter.MarkupWriterContext context = new MarkupWriter.MarkupWriterContext(scope);
			item.AssignRootContext(context);
			string text = scope.MakeAddressable(item.ObjectType);
			string prefixOf = scope.GetPrefixOf(text);
			string text2 = item.ObjectType.Name;
			if (typeof(MarkupExtension).IsAssignableFrom(item.ObjectType) && text2.EndsWith("Extension", StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - 9);
			}
			this._writer.WriteStartElement(prefixOf, text2, text);
			ContentPropertyAttribute cpa = item.Attributes[typeof(ContentPropertyAttribute)] as ContentPropertyAttribute;
			XmlLangPropertyAttribute xmlLangPropertyAttribute = item.Attributes[typeof(XmlLangPropertyAttribute)] as XmlLangPropertyAttribute;
			UidPropertyAttribute uidPropertyAttribute = item.Attributes[typeof(UidPropertyAttribute)] as UidPropertyAttribute;
			MarkupProperty markupProperty = null;
			int num = 0;
			List<int> list = null;
			List<MarkupProperty> list2 = null;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			MarkupWriter.PartiallyOrderedList<string, MarkupProperty> partiallyOrderedList = null;
			Formatting formatting = (this._xmlTextWriter != null) ? this._xmlTextWriter.Formatting : Formatting.None;
			foreach (MarkupProperty markupProperty2 in item.GetProperties(false))
			{
				if (markupProperty2.IsConstructorArgument)
				{
					throw new InvalidOperationException(SR.Get("UnserializableKeyValue"));
				}
				if (!this.IsContentProperty(markupProperty2, cpa, ref markupProperty) && !this.IsDeferredProperty(markupProperty2, dictionary, ref partiallyOrderedList))
				{
					if (!markupProperty2.IsComposite)
					{
						if (markupProperty2.IsAttached || markupProperty2.PropertyDescriptor == null)
						{
							if (markupProperty2.IsValueAsString)
							{
								markupProperty = markupProperty2;
							}
							else if (markupProperty2.IsKey)
							{
								scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
								this._writer.WriteAttributeString(scope.GetPrefixOf(MarkupWriter.NamespaceCache.XamlNamespace), "Key", MarkupWriter.NamespaceCache.XamlNamespace, markupProperty2.StringValue);
							}
							else
							{
								DependencyProperty dependencyProperty = markupProperty2.DependencyProperty;
								string text3 = scope.MakeAddressable(dependencyProperty.OwnerType);
								scope.MakeAddressable(markupProperty2.TypeReferences);
								if (markupProperty2.Attributes[typeof(DesignerSerializationOptionsAttribute)] != null && (markupProperty2.Attributes[typeof(DesignerSerializationOptionsAttribute)] as DesignerSerializationOptionsAttribute).DesignerSerializationOptions == DesignerSerializationOptions.SerializeAsAttribute)
								{
									if (dependencyProperty == UIElement.UidProperty)
									{
										string text4 = scope.MakeAddressable(typeof(TypeExtension));
										this._writer.WriteAttributeString(scope.GetPrefixOf(text4), dependencyProperty.Name, text4, markupProperty2.StringValue);
									}
								}
								else
								{
									markupProperty2.VerifyOnlySerializableTypes();
									string prefixOf2 = scope.GetPrefixOf(text3);
									string localName = dependencyProperty.OwnerType.Name + "." + dependencyProperty.Name;
									if (string.IsNullOrEmpty(prefixOf2))
									{
										this._writer.WriteAttributeString(localName, markupProperty2.StringValue);
									}
									else
									{
										this._writer.WriteAttributeString(prefixOf2, localName, text3, markupProperty2.StringValue);
									}
								}
							}
						}
						else
						{
							markupProperty2.VerifyOnlySerializableTypes();
							if (xmlLangPropertyAttribute != null && xmlLangPropertyAttribute.Name == markupProperty2.PropertyDescriptor.Name)
							{
								this._writer.WriteAttributeString("xml", "lang", MarkupWriter.NamespaceCache.XmlNamespace, markupProperty2.StringValue);
							}
							else if (uidPropertyAttribute != null && uidPropertyAttribute.Name == markupProperty2.PropertyDescriptor.Name)
							{
								string text5 = scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
								this._writer.WriteAttributeString(scope.GetPrefixOf(text5), markupProperty2.PropertyDescriptor.Name, text5, markupProperty2.StringValue);
							}
							else
							{
								this._writer.WriteAttributeString(markupProperty2.PropertyDescriptor.Name, markupProperty2.StringValue);
							}
							dictionary[markupProperty2.Name] = markupProperty2.Name;
						}
					}
					else
					{
						if (markupProperty2.DependencyProperty != null)
						{
							scope.MakeAddressable(markupProperty2.DependencyProperty.OwnerType);
						}
						if (markupProperty2.IsKey)
						{
							scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
						}
						else if (markupProperty2.IsConstructorArgument)
						{
							scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
							if (list == null)
							{
								list = new List<int>();
							}
							list.Add(++num);
						}
						if (list2 == null)
						{
							list2 = new List<MarkupProperty>();
						}
						list2.Add(markupProperty2);
					}
				}
			}
			foreach (MarkupWriter.Mapping mapping in scope.EnumerateLocalMappings)
			{
				this._writer.WriteAttributeString("xmlns", mapping.Prefix, MarkupWriter.NamespaceCache.XmlnsNamespace, mapping.Uri);
			}
			if (!scope.XmlnsSpacePreserve && markupProperty != null && !this.HasOnlyNormalizationNeutralStrings(markupProperty, false, false))
			{
				this._writer.WriteAttributeString("xml", "space", MarkupWriter.NamespaceCache.XmlNamespace, "preserve");
				scope.XmlnsSpacePreserve = true;
				this._writer.WriteString(string.Empty);
				if (scope.IsTopOfSpacePreservationScope && this._xmlTextWriter != null)
				{
					this._xmlTextWriter.Formatting = Formatting.None;
				}
			}
			if (list2 != null)
			{
				foreach (MarkupProperty markupProperty3 in list2)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (MarkupObject markupObject in markupProperty3.Items)
					{
						if (!flag)
						{
							flag = true;
							if (markupProperty3.IsAttached || markupProperty3.PropertyDescriptor == null)
							{
								if (markupProperty3.IsKey)
								{
									throw new InvalidOperationException(SR.Get("UnserializableKeyValue", new object[]
									{
										markupProperty3.Value.GetType().FullName
									}));
								}
								string uri = scope.MakeAddressable(markupProperty3.DependencyProperty.OwnerType);
								this.WritePropertyStart(scope.GetPrefixOf(uri), markupProperty3.DependencyProperty.OwnerType.Name + "." + markupProperty3.DependencyProperty.Name, uri);
							}
							else
							{
								this.WritePropertyStart(prefixOf, item.ObjectType.Name + "." + markupProperty3.PropertyDescriptor.Name, text);
								dictionary[markupProperty3.Name] = markupProperty3.Name;
							}
							flag2 = this.NeedToWriteExplicitTag(markupProperty3, markupObject);
							if (flag2)
							{
								this.WriteExplicitTagStart(markupProperty3, scope);
							}
						}
						this.WriteItem(markupObject, new MarkupWriter.Scope(scope));
					}
					if (flag)
					{
						if (flag2)
						{
							this.WriteExplicitTagEnd();
						}
						this.WritePropertyEnd();
					}
				}
			}
			if (markupProperty != null)
			{
				if (markupProperty.IsComposite)
				{
					IXmlSerializable xmlSerializable = markupProperty.Value as IXmlSerializable;
					if (xmlSerializable != null)
					{
						this.WriteXmlIsland(xmlSerializable, scope);
						goto IL_8A0;
					}
					bool flag3 = false;
					List<Type> wrapperTypes = this.GetWrapperTypes(markupProperty.PropertyType);
					if (wrapperTypes == null)
					{
						using (IEnumerator<MarkupObject> enumerator4 = markupProperty.Items.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								MarkupObject markupObject2 = enumerator4.Current;
								if (!flag3 && markupObject2.ObjectType == typeof(string) && !this.IsCollectionType(markupProperty.PropertyType) && !this.HasNonValueProperties(markupObject2))
								{
									this._writer.WriteString(this.TextValue(markupObject2));
									flag3 = true;
								}
								else
								{
									this.WriteItem(markupObject2, new MarkupWriter.Scope(scope));
									flag3 = false;
								}
							}
							goto IL_8A0;
						}
					}
					using (IEnumerator<MarkupObject> enumerator4 = markupProperty.Items.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							MarkupObject markupObject3 = enumerator4.Current;
							MarkupProperty wrappedProperty = this.GetWrappedProperty(wrapperTypes, markupObject3);
							if (wrappedProperty == null)
							{
								this.WriteItem(markupObject3, new MarkupWriter.Scope(scope));
								flag3 = false;
							}
							else
							{
								if (wrappedProperty.IsComposite)
								{
									using (IEnumerator<MarkupObject> enumerator5 = wrappedProperty.Items.GetEnumerator())
									{
										while (enumerator5.MoveNext())
										{
											MarkupObject item2 = enumerator5.Current;
											if (!flag3 && markupObject3.ObjectType == typeof(string) && !this.HasNonValueProperties(markupObject3))
											{
												this._writer.WriteString(this.TextValue(item2));
												flag3 = true;
											}
											else
											{
												this.WriteItem(item2, new MarkupWriter.Scope(scope));
												flag3 = false;
											}
										}
										continue;
									}
								}
								if (!flag3)
								{
									this._writer.WriteString(wrappedProperty.StringValue);
									flag3 = true;
								}
								else
								{
									this.WriteItem(markupObject3, new MarkupWriter.Scope(scope));
									flag3 = false;
								}
							}
						}
						goto IL_8A0;
					}
				}
				string text6 = markupProperty.Value as string;
				if (text6 == null)
				{
					text6 = markupProperty.StringValue;
				}
				this._writer.WriteString(text6);
				IL_8A0:
				dictionary[markupProperty.Name] = markupProperty.Name;
			}
			if (partiallyOrderedList != null)
			{
				foreach (MarkupProperty markupProperty4 in partiallyOrderedList)
				{
					if (!dictionary.ContainsKey(markupProperty4.Name))
					{
						dictionary[markupProperty4.Name] = markupProperty4.Name;
						this._writer.WriteStartElement(prefixOf, item.ObjectType.Name + "." + markupProperty4.PropertyDescriptor.Name, text);
						if (markupProperty4.IsComposite || markupProperty4.StringValue.IndexOf("{", StringComparison.Ordinal) == 0)
						{
							using (IEnumerator<MarkupObject> enumerator4 = markupProperty4.Items.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									MarkupObject item3 = enumerator4.Current;
									this.WriteItem(item3, new MarkupWriter.Scope(scope));
								}
								goto IL_996;
							}
							goto IL_984;
						}
						goto IL_984;
						IL_996:
						this._writer.WriteEndElement();
						continue;
						IL_984:
						this._writer.WriteString(markupProperty4.StringValue);
						goto IL_996;
					}
				}
			}
			this._writer.WriteEndElement();
			if (scope.IsTopOfSpacePreservationScope && this._xmlTextWriter != null && this._xmlTextWriter.Formatting != formatting)
			{
				this._xmlTextWriter.Formatting = formatting;
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x0021ABF8 File Offset: 0x00219BF8
		private bool IsContentProperty(MarkupProperty property, ContentPropertyAttribute cpa, ref MarkupProperty contentProperty)
		{
			bool flag = property.IsContent;
			if (!flag)
			{
				PropertyDescriptor propertyDescriptor = property.PropertyDescriptor;
				if ((propertyDescriptor != null && typeof(FrameworkTemplate).IsAssignableFrom(propertyDescriptor.ComponentType) && property.Name == "Template") || property.Name == "VisualTree")
				{
					flag = true;
				}
				if (cpa != null && contentProperty == null && propertyDescriptor != null && propertyDescriptor.Name == cpa.Name)
				{
					if (property.IsComposite)
					{
						if (propertyDescriptor == null || propertyDescriptor.IsReadOnly || !typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType))
						{
							flag = true;
						}
					}
					else if (property.Value != null && !(property.Value is MarkupExtension) && property.PropertyType.IsAssignableFrom(typeof(string)))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				contentProperty = property;
			}
			return flag;
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x0021ACDC File Offset: 0x00219CDC
		private bool IsDeferredProperty(MarkupProperty property, Dictionary<string, string> writtenAttributes, ref MarkupWriter.PartiallyOrderedList<string, MarkupProperty> deferredProperties)
		{
			bool flag = false;
			if (property.PropertyDescriptor != null)
			{
				foreach (object obj in property.Attributes)
				{
					DependsOnAttribute dependsOnAttribute = ((Attribute)obj) as DependsOnAttribute;
					if (dependsOnAttribute != null && !writtenAttributes.ContainsKey(dependsOnAttribute.Name))
					{
						if (deferredProperties == null)
						{
							deferredProperties = new MarkupWriter.PartiallyOrderedList<string, MarkupProperty>();
						}
						deferredProperties.SetOrder(dependsOnAttribute.Name, property.Name);
						flag = true;
					}
				}
				if (flag)
				{
					deferredProperties.Add(property.Name, property);
				}
			}
			return flag;
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x0021AD80 File Offset: 0x00219D80
		private bool NeedToWriteExplicitTag(MarkupProperty property, MarkupObject firstItem)
		{
			bool result = false;
			if (property.IsCollectionProperty)
			{
				if (MarkupWriter._nullDefaultValueAttribute == null)
				{
					MarkupWriter._nullDefaultValueAttribute = new DefaultValueAttribute(null);
				}
				if (property.Attributes.Contains(MarkupWriter._nullDefaultValueAttribute))
				{
					result = true;
					object instance = firstItem.Instance;
					if (instance is MarkupExtension)
					{
						if (instance is NullExtension)
						{
							result = false;
						}
						else if (property.PropertyType.IsArray)
						{
							ArrayExtension arrayExtension = instance as ArrayExtension;
							if (property.PropertyType.IsAssignableFrom(arrayExtension.Type.MakeArrayType()))
							{
								result = false;
							}
						}
					}
					else if (property.PropertyType.IsAssignableFrom(firstItem.ObjectType))
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x0021AE24 File Offset: 0x00219E24
		private void WriteExplicitTagStart(MarkupProperty property, MarkupWriter.Scope scope)
		{
			Type type = property.Value.GetType();
			string text = scope.MakeAddressable(type);
			string prefixOf = scope.GetPrefixOf(text);
			string text2 = type.Name;
			if (typeof(MarkupExtension).IsAssignableFrom(type) && text2.EndsWith("Extension", StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - 9);
			}
			this._writer.WriteStartElement(prefixOf, text2, text);
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x0021AE93 File Offset: 0x00219E93
		private void WriteExplicitTagEnd()
		{
			this._writer.WriteEndElement();
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x0021AEA0 File Offset: 0x00219EA0
		private void WritePropertyStart(string prefix, string propertyName, string uri)
		{
			this._writer.WriteStartElement(prefix, propertyName, uri);
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x0021AE93 File Offset: 0x00219E93
		private void WritePropertyEnd()
		{
			this._writer.WriteEndElement();
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x0021AEB0 File Offset: 0x00219EB0
		private void WriteXmlIsland(IXmlSerializable xmlSerializable, MarkupWriter.Scope scope)
		{
			scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
			this._writer.WriteStartElement(scope.GetPrefixOf(MarkupWriter.NamespaceCache.XamlNamespace), "XData", MarkupWriter.NamespaceCache.XamlNamespace);
			xmlSerializable.WriteXml(this._writer);
			this._writer.WriteEndElement();
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0021AF00 File Offset: 0x00219F00
		private List<Type> GetWrapperTypes(Type type)
		{
			AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
			if (attributes[typeof(ContentWrapperAttribute)] == null)
			{
				return null;
			}
			List<Type> list = new List<Type>();
			foreach (object obj in attributes)
			{
				ContentWrapperAttribute contentWrapperAttribute = ((Attribute)obj) as ContentWrapperAttribute;
				if (contentWrapperAttribute != null)
				{
					list.Add(contentWrapperAttribute.ContentWrapper);
				}
			}
			return list;
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x0021AF88 File Offset: 0x00219F88
		private MarkupProperty GetWrappedProperty(List<Type> wrapperTypes, MarkupObject item)
		{
			if (!this.IsInTypes(item.ObjectType, wrapperTypes))
			{
				return null;
			}
			ContentPropertyAttribute contentPropertyAttribute = item.Attributes[typeof(ContentPropertyAttribute)] as ContentPropertyAttribute;
			MarkupProperty result = null;
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				if (!markupProperty.IsContent && (contentPropertyAttribute == null || markupProperty.PropertyDescriptor == null || !(markupProperty.PropertyDescriptor.Name == contentPropertyAttribute.Name)))
				{
					result = null;
					break;
				}
				result = markupProperty;
			}
			return result;
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x0021B030 File Offset: 0x0021A030
		private bool IsInTypes(Type type, List<Type> types)
		{
			using (List<Type>.Enumerator enumerator = types.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == type)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x0021B088 File Offset: 0x0021A088
		private string TextValue(MarkupObject item)
		{
			using (IEnumerator<MarkupProperty> enumerator = item.Properties.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					MarkupProperty markupProperty = enumerator.Current;
					if (markupProperty.IsValueAsString)
					{
						return markupProperty.StringValue;
					}
				}
			}
			return null;
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0021B0E4 File Offset: 0x0021A0E4
		private bool HasNonValueProperties(MarkupObject item)
		{
			using (IEnumerator<MarkupProperty> enumerator = item.Properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsValueAsString)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x0021B138 File Offset: 0x0021A138
		private bool IsCollectionType(Type type)
		{
			return typeof(IEnumerable).IsAssignableFrom(type) || typeof(Array).IsAssignableFrom(type);
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x0021B160 File Offset: 0x0021A160
		private bool HasOnlyNormalizationNeutralStrings(MarkupProperty contentProperty, bool keepLeadingSpace, bool keepTrailingSpace)
		{
			if (!contentProperty.IsComposite)
			{
				return this.IsNormalizationNeutralString(contentProperty.StringValue, keepLeadingSpace, keepTrailingSpace);
			}
			bool flag = true;
			bool flag2 = !keepLeadingSpace;
			bool flag3 = !keepLeadingSpace;
			string text = null;
			MarkupProperty markupProperty = null;
			List<Type> wrapperTypes = this.GetWrapperTypes(contentProperty.PropertyType);
			foreach (MarkupObject markupObject in contentProperty.Items)
			{
				flag3 = flag2;
				flag2 = this.ShouldTrimSurroundingWhitespace(markupObject);
				if (text != null)
				{
					flag = this.IsNormalizationNeutralString(text, !flag3, !flag2);
					text = null;
					if (!flag)
					{
						return false;
					}
				}
				if (markupProperty != null)
				{
					flag = this.HasOnlyNormalizationNeutralStrings(markupProperty, !flag3, !flag2);
					markupProperty = null;
					if (!flag)
					{
						return false;
					}
				}
				if (markupObject.ObjectType == typeof(string))
				{
					text = this.TextValue(markupObject);
					if (text != null)
					{
						continue;
					}
				}
				if (wrapperTypes != null)
				{
					MarkupProperty wrappedProperty = this.GetWrappedProperty(wrapperTypes, markupObject);
					if (wrappedProperty != null)
					{
						markupProperty = wrappedProperty;
					}
				}
			}
			if (text != null)
			{
				flag = this.IsNormalizationNeutralString(text, !flag3, keepTrailingSpace);
			}
			else if (markupProperty != null)
			{
				flag = this.HasOnlyNormalizationNeutralStrings(markupProperty, !flag3, keepTrailingSpace);
			}
			return flag;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x0021B298 File Offset: 0x0021A298
		private bool ShouldTrimSurroundingWhitespace(MarkupObject item)
		{
			return item.Attributes[typeof(TrimSurroundingWhitespaceAttribute)] is TrimSurroundingWhitespaceAttribute;
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x0021B2B8 File Offset: 0x0021A2B8
		private bool IsNormalizationNeutralString(string value, bool keepLeadingSpace, bool keepTrailingSpace)
		{
			bool flag = !keepLeadingSpace;
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				switch (c)
				{
				case '\t':
				case '\n':
				case '\f':
				case '\r':
					return false;
				case '\v':
					goto IL_3E;
				default:
					if (c != ' ')
					{
						goto IL_3E;
					}
					if (flag)
					{
						return false;
					}
					flag = true;
					break;
				}
				IL_40:
				i++;
				continue;
				IL_3E:
				flag = false;
				goto IL_40;
			}
			return !flag || keepTrailingSpace;
		}

		// Token: 0x040024EE RID: 9454
		private const string clrUriPrefix = "clr-namespace:";

		// Token: 0x040024EF RID: 9455
		private const int EXTENSIONLENGTH = 9;

		// Token: 0x040024F0 RID: 9456
		private XmlWriter _writer;

		// Token: 0x040024F1 RID: 9457
		private XmlTextWriter _xmlTextWriter;

		// Token: 0x040024F2 RID: 9458
		private static DefaultValueAttribute _nullDefaultValueAttribute;

		// Token: 0x02000B14 RID: 2836
		private class PartiallyOrderedList<TKey, TValue> : IEnumerable<!1>, IEnumerable where TValue : class
		{
			// Token: 0x06008C2A RID: 35882 RVA: 0x0033C38C File Offset: 0x0033B38C
			public void Add(TKey key, TValue value)
			{
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = new MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry(key, value);
				int num = this._entries.IndexOf(entry);
				if (num >= 0)
				{
					entry.Predecessors = this._entries[num].Predecessors;
					this._entries[num] = entry;
					return;
				}
				this._entries.Add(entry);
			}

			// Token: 0x06008C2B RID: 35883 RVA: 0x0033C3E4 File Offset: 0x0033B3E4
			private int GetEntryIndex(TKey key)
			{
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry item = new MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry(key, default(TValue));
				int num = this._entries.IndexOf(item);
				if (num < 0)
				{
					num = this._entries.Count;
					this._entries.Add(item);
				}
				return num;
			}

			// Token: 0x06008C2C RID: 35884 RVA: 0x0033C42C File Offset: 0x0033B42C
			public void SetOrder(TKey predecessor, TKey key)
			{
				int entryIndex = this.GetEntryIndex(predecessor);
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = this._entries[entryIndex];
				int entryIndex2 = this.GetEntryIndex(key);
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry2 = this._entries[entryIndex2];
				if (entry2.Predecessors == null)
				{
					entry2.Predecessors = new List<int>();
				}
				entry2.Predecessors.Add(entryIndex);
				this._firstIndex = -1;
			}

			// Token: 0x06008C2D RID: 35885 RVA: 0x0033C48C File Offset: 0x0033B48C
			private void TopologicalSort()
			{
				this._firstIndex = -1;
				this._lastIndex = -1;
				for (int i = 0; i < this._entries.Count; i++)
				{
					this._entries[i].Link = -1;
				}
				for (int j = 0; j < this._entries.Count; j++)
				{
					this.DepthFirstSearch(j);
				}
			}

			// Token: 0x06008C2E RID: 35886 RVA: 0x0033C4EC File Offset: 0x0033B4EC
			private void DepthFirstSearch(int index)
			{
				if (this._entries[index].Link == -1)
				{
					this._entries[index].Link = -2;
					if (this._entries[index].Predecessors != null)
					{
						foreach (int index2 in this._entries[index].Predecessors)
						{
							this.DepthFirstSearch(index2);
						}
					}
					if (this._lastIndex == -1)
					{
						this._firstIndex = index;
					}
					else
					{
						this._entries[this._lastIndex].Link = index;
					}
					this._lastIndex = index;
				}
			}

			// Token: 0x06008C2F RID: 35887 RVA: 0x0033C5B8 File Offset: 0x0033B5B8
			public IEnumerator<TValue> GetEnumerator()
			{
				if (this._firstIndex < 0)
				{
					this.TopologicalSort();
				}
				int i = this._firstIndex;
				while (i >= 0)
				{
					MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = this._entries[i];
					if (entry.Value != null)
					{
						yield return entry.Value;
					}
					i = entry.Link;
					entry = null;
				}
				yield break;
			}

			// Token: 0x06008C30 RID: 35888 RVA: 0x0033C5C7 File Offset: 0x0033B5C7
			IEnumerator IEnumerable.GetEnumerator()
			{
				foreach (TValue tvalue in this)
				{
					yield return tvalue;
				}
				IEnumerator<TValue> enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x040047BF RID: 18367
			private List<MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry> _entries = new List<MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry>();

			// Token: 0x040047C0 RID: 18368
			private int _firstIndex = -1;

			// Token: 0x040047C1 RID: 18369
			private int _lastIndex;

			// Token: 0x02000C86 RID: 3206
			private class Entry
			{
				// Token: 0x06009526 RID: 38182 RVA: 0x0034D5EE File Offset: 0x0034C5EE
				public Entry(TKey key, TValue value)
				{
					this.Key = key;
					this.Value = value;
					this.Predecessors = null;
					this.Link = 0;
				}

				// Token: 0x06009527 RID: 38183 RVA: 0x0034D614 File Offset: 0x0034C614
				public override bool Equals(object obj)
				{
					MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = obj as MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry;
					if (entry != null)
					{
						TKey key = entry.Key;
						return key.Equals(this.Key);
					}
					return false;
				}

				// Token: 0x06009528 RID: 38184 RVA: 0x0034D64C File Offset: 0x0034C64C
				public override int GetHashCode()
				{
					TKey key = this.Key;
					return key.GetHashCode();
				}

				// Token: 0x04004F98 RID: 20376
				public readonly TKey Key;

				// Token: 0x04004F99 RID: 20377
				public readonly TValue Value;

				// Token: 0x04004F9A RID: 20378
				public List<int> Predecessors;

				// Token: 0x04004F9B RID: 20379
				public int Link;

				// Token: 0x04004F9C RID: 20380
				public const int UNSEEN = -1;

				// Token: 0x04004F9D RID: 20381
				public const int INDFS = -2;
			}
		}

		// Token: 0x02000B15 RID: 2837
		private class Mapping
		{
			// Token: 0x06008C32 RID: 35890 RVA: 0x0033C5F0 File Offset: 0x0033B5F0
			public Mapping(string uri, string prefix)
			{
				this.Uri = uri;
				this.Prefix = prefix;
			}

			// Token: 0x06008C33 RID: 35891 RVA: 0x0033C608 File Offset: 0x0033B608
			public override bool Equals(object obj)
			{
				MarkupWriter.Mapping mapping = obj as MarkupWriter.Mapping;
				return mapping != null && this.Uri.Equals(mapping.Uri) && this.Prefix.Equals(mapping.Prefix);
			}

			// Token: 0x06008C34 RID: 35892 RVA: 0x0033C645 File Offset: 0x0033B645
			public override int GetHashCode()
			{
				return this.Uri.GetHashCode() + this.Prefix.GetHashCode();
			}

			// Token: 0x040047C2 RID: 18370
			public readonly string Uri;

			// Token: 0x040047C3 RID: 18371
			public readonly string Prefix;
		}

		// Token: 0x02000B16 RID: 2838
		private class Scope
		{
			// Token: 0x06008C35 RID: 35893 RVA: 0x0033C65E File Offset: 0x0033B65E
			public Scope(MarkupWriter.Scope containingScope)
			{
				this._containingScope = containingScope;
			}

			// Token: 0x17001EB8 RID: 7864
			// (get) Token: 0x06008C36 RID: 35894 RVA: 0x0033C66D File Offset: 0x0033B66D
			// (set) Token: 0x06008C37 RID: 35895 RVA: 0x0033C69D File Offset: 0x0033B69D
			public bool XmlnsSpacePreserve
			{
				get
				{
					if (this._xmlnsSpacePreserve != null)
					{
						return this._xmlnsSpacePreserve.Value;
					}
					return this._containingScope != null && this._containingScope.XmlnsSpacePreserve;
				}
				set
				{
					this._xmlnsSpacePreserve = new bool?(value);
				}
			}

			// Token: 0x17001EB9 RID: 7865
			// (get) Token: 0x06008C38 RID: 35896 RVA: 0x0033C6AB File Offset: 0x0033B6AB
			public bool IsTopOfSpacePreservationScope
			{
				get
				{
					return this._containingScope == null || (this._xmlnsSpacePreserve != null && this._xmlnsSpacePreserve.Value != this._containingScope.XmlnsSpacePreserve);
				}
			}

			// Token: 0x06008C39 RID: 35897 RVA: 0x0033C6E4 File Offset: 0x0033B6E4
			public string GetPrefixOf(string uri)
			{
				string result;
				if (this._uriToPrefix != null && this._uriToPrefix.TryGetValue(uri, out result))
				{
					return result;
				}
				if (this._containingScope != null)
				{
					return this._containingScope.GetPrefixOf(uri);
				}
				return null;
			}

			// Token: 0x06008C3A RID: 35898 RVA: 0x0033C724 File Offset: 0x0033B724
			public string GetUriOf(string prefix)
			{
				string result;
				if (this._prefixToUri != null && this._prefixToUri.TryGetValue(prefix, out result))
				{
					return result;
				}
				if (this._containingScope != null)
				{
					return this._containingScope.GetUriOf(prefix);
				}
				return null;
			}

			// Token: 0x06008C3B RID: 35899 RVA: 0x0033C764 File Offset: 0x0033B764
			public void RecordMapping(string prefix, string uri)
			{
				if (this._uriToPrefix == null)
				{
					this._uriToPrefix = new Dictionary<string, string>();
				}
				if (this._prefixToUri == null)
				{
					this._prefixToUri = new Dictionary<string, string>();
				}
				this._uriToPrefix[uri] = prefix;
				this._prefixToUri[prefix] = uri;
			}

			// Token: 0x06008C3C RID: 35900 RVA: 0x0033C7B4 File Offset: 0x0033B7B4
			public void MakeAddressable(IEnumerable<Type> types)
			{
				if (types != null)
				{
					foreach (Type type in types)
					{
						this.MakeAddressable(type);
					}
				}
			}

			// Token: 0x06008C3D RID: 35901 RVA: 0x0033C800 File Offset: 0x0033B800
			public string MakeAddressable(Type type)
			{
				return this.MakeAddressable(MarkupWriter.NamespaceCache.GetNamespaceUriFor(type));
			}

			// Token: 0x06008C3E RID: 35902 RVA: 0x0033C810 File Offset: 0x0033B810
			public string MakeAddressable(string uri)
			{
				if (this.GetPrefixOf(uri) == null)
				{
					string defaultPrefixFor = MarkupWriter.NamespaceCache.GetDefaultPrefixFor(uri);
					string prefix = defaultPrefixFor;
					int num = 0;
					while (this.GetUriOf(prefix) != null)
					{
						prefix = defaultPrefixFor + num++;
					}
					this.RecordMapping(prefix, uri);
				}
				return uri;
			}

			// Token: 0x17001EBA RID: 7866
			// (get) Token: 0x06008C3F RID: 35903 RVA: 0x0033C856 File Offset: 0x0033B856
			public IEnumerable<MarkupWriter.Mapping> EnumerateLocalMappings
			{
				get
				{
					if (this._uriToPrefix != null)
					{
						foreach (KeyValuePair<string, string> keyValuePair in this._uriToPrefix)
						{
							yield return new MarkupWriter.Mapping(keyValuePair.Key, keyValuePair.Value);
						}
						Dictionary<string, string>.Enumerator enumerator = default(Dictionary<string, string>.Enumerator);
					}
					yield break;
					yield break;
				}
			}

			// Token: 0x17001EBB RID: 7867
			// (get) Token: 0x06008C40 RID: 35904 RVA: 0x0033C866 File Offset: 0x0033B866
			public IEnumerable<MarkupWriter.Mapping> EnumerateAllMappings
			{
				get
				{
					IEnumerator<MarkupWriter.Mapping> enumerator;
					if (this._containingScope != null)
					{
						foreach (MarkupWriter.Mapping mapping in this._containingScope.EnumerateAllMappings)
						{
							yield return mapping;
						}
						enumerator = null;
					}
					foreach (MarkupWriter.Mapping mapping2 in this.EnumerateLocalMappings)
					{
						yield return mapping2;
					}
					enumerator = null;
					yield break;
					yield break;
				}
			}

			// Token: 0x040047C4 RID: 18372
			private MarkupWriter.Scope _containingScope;

			// Token: 0x040047C5 RID: 18373
			private bool? _xmlnsSpacePreserve;

			// Token: 0x040047C6 RID: 18374
			private Dictionary<string, string> _uriToPrefix;

			// Token: 0x040047C7 RID: 18375
			private Dictionary<string, string> _prefixToUri;
		}

		// Token: 0x02000B17 RID: 2839
		private class MarkupWriterContext : IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06008C41 RID: 35905 RVA: 0x0033C876 File Offset: 0x0033B876
			internal MarkupWriterContext(MarkupWriter.Scope scope)
			{
				this._scope = scope;
			}

			// Token: 0x06008C42 RID: 35906 RVA: 0x0033C885 File Offset: 0x0033B885
			public ValueSerializer GetValueSerializerFor(PropertyDescriptor descriptor)
			{
				if (descriptor.PropertyType == typeof(Type))
				{
					return new MarkupWriter.TypeValueSerializer(this._scope);
				}
				return ValueSerializer.GetSerializerFor(descriptor);
			}

			// Token: 0x06008C43 RID: 35907 RVA: 0x0033C8B0 File Offset: 0x0033B8B0
			public ValueSerializer GetValueSerializerFor(Type type)
			{
				if (type == typeof(Type))
				{
					return new MarkupWriter.TypeValueSerializer(this._scope);
				}
				return ValueSerializer.GetSerializerFor(type);
			}

			// Token: 0x17001EBC RID: 7868
			// (get) Token: 0x06008C44 RID: 35908 RVA: 0x00109403 File Offset: 0x00108403
			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001EBD RID: 7869
			// (get) Token: 0x06008C45 RID: 35909 RVA: 0x00109403 File Offset: 0x00108403
			public object Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06008C46 RID: 35910 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			public void OnComponentChanged()
			{
			}

			// Token: 0x06008C47 RID: 35911 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public bool OnComponentChanging()
			{
				return true;
			}

			// Token: 0x17001EBE RID: 7870
			// (get) Token: 0x06008C48 RID: 35912 RVA: 0x00109403 File Offset: 0x00108403
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06008C49 RID: 35913 RVA: 0x00109403 File Offset: 0x00108403
			public object GetService(Type serviceType)
			{
				return null;
			}

			// Token: 0x040047C8 RID: 18376
			private MarkupWriter.Scope _scope;
		}

		// Token: 0x02000B18 RID: 2840
		private class TypeValueSerializer : ValueSerializer
		{
			// Token: 0x06008C4A RID: 35914 RVA: 0x0033C8D6 File Offset: 0x0033B8D6
			public TypeValueSerializer(MarkupWriter.Scope scope)
			{
				this._scope = scope;
			}

			// Token: 0x06008C4B RID: 35915 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public override bool CanConvertToString(object value, IValueSerializerContext context)
			{
				return true;
			}

			// Token: 0x06008C4C RID: 35916 RVA: 0x0033C8E8 File Offset: 0x0033B8E8
			public override string ConvertToString(object value, IValueSerializerContext context)
			{
				Type type = value as Type;
				if (type == null)
				{
					throw new InvalidOperationException();
				}
				string uri = this._scope.MakeAddressable(type);
				string prefixOf = this._scope.GetPrefixOf(uri);
				if (prefixOf == null || prefixOf == "")
				{
					return type.Name;
				}
				return prefixOf + ":" + type.Name;
			}

			// Token: 0x06008C4D RID: 35917 RVA: 0x0033C950 File Offset: 0x0033B950
			public override IEnumerable<Type> TypeReferences(object value, IValueSerializerContext context)
			{
				Type type = value as Type;
				if (type != null)
				{
					return new Type[]
					{
						type
					};
				}
				return base.TypeReferences(value, context);
			}

			// Token: 0x040047C9 RID: 18377
			private MarkupWriter.Scope _scope;
		}

		// Token: 0x02000B19 RID: 2841
		private static class NamespaceCache
		{
			// Token: 0x06008C4E RID: 35918 RVA: 0x0033C980 File Offset: 0x0033B980
			private static Dictionary<string, string> GetMappingsFor(Assembly assembly)
			{
				object syncObject = MarkupWriter.NamespaceCache.SyncObject;
				Dictionary<string, string> dictionary;
				lock (syncObject)
				{
					if (!MarkupWriter.NamespaceCache.XmlnsDefinitions.TryGetValue(assembly, out dictionary))
					{
						foreach (XmlnsPrefixAttribute xmlnsPrefixAttribute in assembly.GetCustomAttributes(typeof(XmlnsPrefixAttribute), true))
						{
							MarkupWriter.NamespaceCache.DefaultPrefixes[xmlnsPrefixAttribute.XmlNamespace] = xmlnsPrefixAttribute.Prefix;
						}
						dictionary = new Dictionary<string, string>();
						MarkupWriter.NamespaceCache.XmlnsDefinitions[assembly] = dictionary;
						foreach (XmlnsDefinitionAttribute xmlnsDefinitionAttribute in assembly.GetCustomAttributes(typeof(XmlnsDefinitionAttribute), true))
						{
							if (xmlnsDefinitionAttribute.AssemblyName == null)
							{
								string text = null;
								string text2 = null;
								string text3 = null;
								if (dictionary.TryGetValue(xmlnsDefinitionAttribute.ClrNamespace, out text) && MarkupWriter.NamespaceCache.DefaultPrefixes.TryGetValue(text, out text2))
								{
									MarkupWriter.NamespaceCache.DefaultPrefixes.TryGetValue(xmlnsDefinitionAttribute.XmlNamespace, out text3);
								}
								if (text == null || text2 == null || (text3 != null && text2.Length > text3.Length))
								{
									dictionary[xmlnsDefinitionAttribute.ClrNamespace] = xmlnsDefinitionAttribute.XmlNamespace;
								}
							}
							else
							{
								Assembly assembly2 = Assembly.Load(new AssemblyName(xmlnsDefinitionAttribute.AssemblyName));
								if (assembly2 != null)
								{
									MarkupWriter.NamespaceCache.GetMappingsFor(assembly2)[xmlnsDefinitionAttribute.ClrNamespace] = xmlnsDefinitionAttribute.XmlNamespace;
								}
							}
						}
					}
				}
				return dictionary;
			}

			// Token: 0x06008C4F RID: 35919 RVA: 0x0033CB18 File Offset: 0x0033BB18
			public static string GetNamespaceUriFor(Type type)
			{
				object syncObject = MarkupWriter.NamespaceCache.SyncObject;
				string result;
				lock (syncObject)
				{
					if (type.Namespace == null)
					{
						result = string.Format(CultureInfo.InvariantCulture, "clr-namespace:;assembly={0}", type.Assembly.GetName().Name);
					}
					else if (!MarkupWriter.NamespaceCache.GetMappingsFor(type.Assembly).TryGetValue(type.Namespace, out result))
					{
						result = string.Format(CultureInfo.InvariantCulture, "clr-namespace:{0};assembly={1}", type.Namespace, type.Assembly.GetName().Name);
					}
				}
				return result;
			}

			// Token: 0x06008C50 RID: 35920 RVA: 0x0033CBBC File Offset: 0x0033BBBC
			public static string GetDefaultPrefixFor(string uri)
			{
				object syncObject = MarkupWriter.NamespaceCache.SyncObject;
				string text;
				lock (syncObject)
				{
					MarkupWriter.NamespaceCache.DefaultPrefixes.TryGetValue(uri, out text);
					if (text == null)
					{
						text = "assembly";
						if (uri.StartsWith("clr-namespace:", StringComparison.Ordinal))
						{
							string text2 = uri.Substring("clr-namespace:".Length, uri.IndexOf(";", StringComparison.Ordinal) - "clr-namespace:".Length);
							StringBuilder stringBuilder = new StringBuilder();
							foreach (char c in text2)
							{
								if (c >= 'A' && c <= 'Z')
								{
									stringBuilder.Append(c.ToString().ToLower(CultureInfo.InvariantCulture));
								}
							}
							if (stringBuilder.Length > 0)
							{
								text = stringBuilder.ToString();
							}
						}
					}
				}
				return text;
			}

			// Token: 0x040047CA RID: 18378
			private static Dictionary<Assembly, Dictionary<string, string>> XmlnsDefinitions = new Dictionary<Assembly, Dictionary<string, string>>();

			// Token: 0x040047CB RID: 18379
			private static Dictionary<string, string> DefaultPrefixes = new Dictionary<string, string>();

			// Token: 0x040047CC RID: 18380
			private static object SyncObject = new object();

			// Token: 0x040047CD RID: 18381
			public static string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

			// Token: 0x040047CE RID: 18382
			public static string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

			// Token: 0x040047CF RID: 18383
			public static string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";
		}
	}
}
