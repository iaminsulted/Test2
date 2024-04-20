using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Permissions;
using System.Xaml.Schema;
using MS.Internal.Xaml.Context;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020003C5 RID: 965
	[XamlDeferLoad(typeof(TemplateContentLoader), typeof(FrameworkElement))]
	public class TemplateContent
	{
		// Token: 0x0600288E RID: 10382 RVA: 0x00195728 File Offset: 0x00194728
		internal TemplateContent(System.Xaml.XamlReader xamlReader, IXamlObjectWriterFactory factory, IServiceProvider context)
		{
			this.TemplateLoadData = new TemplateLoadData();
			this.ObjectWriterFactory = factory;
			this.SchemaContext = xamlReader.SchemaContext;
			this.ObjectWriterParentSettings = factory.GetParentSettings();
			XamlAccessLevel accessLevel = this.ObjectWriterParentSettings.AccessLevel;
			this.TemplateLoadData.Reader = xamlReader;
			this.Initialize(context);
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x00195784 File Offset: 0x00194784
		private void Initialize(IServiceProvider context)
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = System.Windows.Markup.XamlReader.CreateObjectWriterSettings(this.ObjectWriterParentSettings);
			xamlObjectWriterSettings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				if (this.Stack != null && this.Stack.Depth > 0)
				{
					this.Stack.CurrentFrame.Instance = args.Instance;
				}
			};
			xamlObjectWriterSettings.SkipProvideValueOnRoot = true;
			this.TemplateLoadData.ObjectWriter = this.ObjectWriterFactory.GetXamlObjectWriter(xamlObjectWriterSettings);
			this.TemplateLoadData.ServiceProviderWrapper = new TemplateContent.ServiceProviderWrapper(context, this.SchemaContext);
			IRootObjectProvider rootObjectProvider = context.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootObjectProvider != null)
			{
				this.TemplateLoadData.RootObject = rootObjectProvider.RootObject;
			}
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x00195810 File Offset: 0x00194810
		internal void ParseXaml()
		{
			TemplateContent.StackOfFrames stackOfFrames = new TemplateContent.StackOfFrames();
			this.TemplateLoadData.ServiceProviderWrapper.Frames = stackOfFrames;
			this.OwnerTemplate.StyleConnector = (this.TemplateLoadData.RootObject as IStyleConnector);
			this.TemplateLoadData.RootObject = null;
			List<PropertyValue> list = new List<PropertyValue>();
			int num = 1;
			this.ParseTree(stackOfFrames, list, ref num);
			if (this.OwnerTemplate is ItemsPanelTemplate)
			{
				list.Add(new PropertyValue
				{
					ValueType = PropertyValueType.Set,
					ChildName = this.TemplateLoadData.RootName,
					ValueInternal = true,
					Property = Panel.IsItemsHostProperty
				});
			}
			for (int i = 0; i < list.Count; i++)
			{
				PropertyValue propertyValue = list[i];
				if (propertyValue.ValueInternal is TemplateBindingExtension)
				{
					propertyValue.ValueType = PropertyValueType.TemplateBinding;
				}
				else if (propertyValue.ValueInternal is DynamicResourceExtension)
				{
					DynamicResourceExtension dynamicResourceExtension = propertyValue.Value as DynamicResourceExtension;
					propertyValue.ValueType = PropertyValueType.Resource;
					propertyValue.ValueInternal = dynamicResourceExtension.ResourceKey;
				}
				else
				{
					StyleHelper.SealIfSealable(propertyValue.ValueInternal);
				}
				StyleHelper.UpdateTables(ref propertyValue, ref this.OwnerTemplate.ChildRecordFromChildIndex, ref this.OwnerTemplate.TriggerSourceRecordFromChildIndex, ref this.OwnerTemplate.ResourceDependents, ref this.OwnerTemplate._dataTriggerRecordFromBinding, this.OwnerTemplate.ChildIndexFromChildName, ref this.OwnerTemplate._hasInstanceValues);
			}
			this.TemplateLoadData.ObjectWriter = null;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x0019598E File Offset: 0x0019498E
		internal System.Xaml.XamlReader PlayXaml()
		{
			return this._xamlNodeList.GetReader();
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x0019599B File Offset: 0x0019499B
		internal void ResetTemplateLoadData()
		{
			this.TemplateLoadData = null;
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x001959A4 File Offset: 0x001949A4
		private void UpdateSharedPropertyNames(string name, List<PropertyValue> sharedProperties, XamlType type)
		{
			int key = StyleHelper.CreateChildIndexFromChildName(name, this.OwnerTemplate);
			this.OwnerTemplate.ChildNames.Add(name);
			this.OwnerTemplate.ChildTypeFromChildIndex.Add(key, type.UnderlyingType);
			for (int i = sharedProperties.Count - 1; i >= 0; i--)
			{
				PropertyValue propertyValue = sharedProperties[i];
				if (propertyValue.ChildName != null)
				{
					break;
				}
				propertyValue.ChildName = name;
				sharedProperties[i] = propertyValue;
			}
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x00195A19 File Offset: 0x00194A19
		private void ParseTree(TemplateContent.StackOfFrames stack, List<PropertyValue> sharedProperties, ref int nameNumber)
		{
			this.ParseNodes(stack, sharedProperties, ref nameNumber);
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x00195A24 File Offset: 0x00194A24
		private void ParseNodes(TemplateContent.StackOfFrames stack, List<PropertyValue> sharedProperties, ref int nameNumber)
		{
			this._xamlNodeList = new XamlNodeList(this.SchemaContext);
			System.Xaml.XamlWriter writer = this._xamlNodeList.Writer;
			System.Xaml.XamlReader reader = this.TemplateLoadData.Reader;
			IXamlLineInfoConsumer xamlLineInfoConsumer = null;
			IXamlLineInfo xamlLineInfo = null;
			if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
			{
				xamlLineInfo = (reader as IXamlLineInfo);
				if (xamlLineInfo != null)
				{
					xamlLineInfoConsumer = (writer as IXamlLineInfoConsumer);
				}
			}
			while (reader.Read())
			{
				if (xamlLineInfoConsumer != null)
				{
					xamlLineInfoConsumer.SetLineInfo(xamlLineInfo.LineNumber, xamlLineInfo.LinePosition);
				}
				object obj;
				if (this.ParseNode(reader, stack, sharedProperties, ref nameNumber, out obj))
				{
					if (obj == DependencyProperty.UnsetValue)
					{
						writer.WriteNode(reader);
					}
					else
					{
						writer.WriteValue(obj);
					}
				}
			}
			writer.Close();
			this.TemplateLoadData.Reader = null;
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x00195AD0 File Offset: 0x00194AD0
		private bool ParseNode(System.Xaml.XamlReader xamlReader, TemplateContent.StackOfFrames stack, List<PropertyValue> sharedProperties, ref int nameNumber, out object newValue)
		{
			newValue = DependencyProperty.UnsetValue;
			switch (xamlReader.NodeType)
			{
			case System.Xaml.XamlNodeType.StartObject:
				if (xamlReader.Type.UnderlyingType == typeof(StaticResourceExtension))
				{
					XamlObjectWriter objectWriter = this.TemplateLoadData.ObjectWriter;
					objectWriter.Clear();
					this.WriteNamespaces(objectWriter, stack.InScopeNamespaces, null);
					newValue = this.LoadTimeBindUnshareableStaticResource(xamlReader, objectWriter);
					return true;
				}
				if (stack.Depth > 0 && !stack.CurrentFrame.NameSet && stack.CurrentFrame.Type != null && !stack.CurrentFrame.IsInNameScope && !stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						int num = nameNumber;
						nameNumber = num + 1;
						string name = num.ToString(CultureInfo.InvariantCulture) + "_T";
						this.UpdateSharedPropertyNames(name, sharedProperties, stack.CurrentFrame.Type);
						stack.CurrentFrame.Name = name;
					}
					stack.CurrentFrame.NameSet = true;
				}
				if (this.RootType == null)
				{
					this.RootType = xamlReader.Type;
				}
				stack.Push(xamlReader.Type, null);
				break;
			case System.Xaml.XamlNodeType.GetObject:
			{
				if (stack.Depth > 0 && !stack.CurrentFrame.NameSet && stack.CurrentFrame.Type != null && !stack.CurrentFrame.IsInNameScope && !stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						int num = nameNumber;
						nameNumber = num + 1;
						string name2 = num.ToString(CultureInfo.InvariantCulture) + "_T";
						this.UpdateSharedPropertyNames(name2, sharedProperties, stack.CurrentFrame.Type);
						stack.CurrentFrame.Name = name2;
					}
					stack.CurrentFrame.NameSet = true;
				}
				XamlType type = stack.CurrentFrame.Property.Type;
				if (this.RootType == null)
				{
					this.RootType = type;
				}
				stack.Push(type, null);
				break;
			}
			case System.Xaml.XamlNodeType.EndObject:
				if (!stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (!stack.CurrentFrame.NameSet && !stack.CurrentFrame.IsInNameScope)
					{
						if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
						{
							int num = nameNumber;
							nameNumber = num + 1;
							string name3 = num.ToString(CultureInfo.InvariantCulture) + "_T";
							this.UpdateSharedPropertyNames(name3, sharedProperties, stack.CurrentFrame.Type);
							stack.CurrentFrame.Name = name3;
						}
						stack.CurrentFrame.NameSet = true;
					}
					if (this.TemplateLoadData.RootName == null && stack.Depth == 1)
					{
						this.TemplateLoadData.RootName = stack.CurrentFrame.Name;
					}
					if (typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						TemplateContent.AutoAliasContentPresenter(this.OwnerTemplate.TargetTypeInternal, stack.CurrentFrame.ContentSource, stack.CurrentFrame.Name, ref this.OwnerTemplate.ChildRecordFromChildIndex, ref this.OwnerTemplate.TriggerSourceRecordFromChildIndex, ref this.OwnerTemplate.ResourceDependents, ref this.OwnerTemplate._dataTriggerRecordFromBinding, ref this.OwnerTemplate._hasInstanceValues, this.OwnerTemplate.ChildIndexFromChildName, stack.CurrentFrame.ContentSet, stack.CurrentFrame.ContentSourceSet, stack.CurrentFrame.ContentTemplateSet, stack.CurrentFrame.ContentTemplateSelectorSet, stack.CurrentFrame.ContentStringFormatSet);
					}
					if (typeof(GridViewRowPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						TemplateContent.AutoAliasGridViewRowPresenter(this.OwnerTemplate.TargetTypeInternal, stack.CurrentFrame.ContentSource, stack.CurrentFrame.Name, ref this.OwnerTemplate.ChildRecordFromChildIndex, ref this.OwnerTemplate.TriggerSourceRecordFromChildIndex, ref this.OwnerTemplate.ResourceDependents, ref this.OwnerTemplate._dataTriggerRecordFromBinding, ref this.OwnerTemplate._hasInstanceValues, this.OwnerTemplate.ChildIndexFromChildName, stack.CurrentFrame.ContentSet, stack.CurrentFrame.ColumnsSet);
					}
				}
				stack.PopScope();
				break;
			case System.Xaml.XamlNodeType.StartMember:
				stack.CurrentFrame.Property = xamlReader.Member;
				if (!stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (typeof(GridViewRowPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						if (xamlReader.Member.Name == "Content")
						{
							stack.CurrentFrame.ContentSet = true;
						}
						else if (xamlReader.Member.Name == "Columns")
						{
							stack.CurrentFrame.ColumnsSet = true;
						}
					}
					else if (typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						if (xamlReader.Member.Name == "Content")
						{
							stack.CurrentFrame.ContentSet = true;
						}
						else if (xamlReader.Member.Name == "ContentTemplate")
						{
							stack.CurrentFrame.ContentTemplateSet = true;
						}
						else if (xamlReader.Member.Name == "ContentTemplateSelector")
						{
							stack.CurrentFrame.ContentTemplateSelectorSet = true;
						}
						else if (xamlReader.Member.Name == "ContentStringFormat")
						{
							stack.CurrentFrame.ContentStringFormatSet = true;
						}
						else if (xamlReader.Member.Name == "ContentSource")
						{
							stack.CurrentFrame.ContentSourceSet = true;
						}
					}
					if (!stack.CurrentFrame.IsInNameScope && !xamlReader.Member.IsDirective)
					{
						IXamlIndexingReader xamlIndexingReader = xamlReader as IXamlIndexingReader;
						bool flag = false;
						int currentIndex = xamlIndexingReader.CurrentIndex;
						PropertyValue? propertyValue;
						try
						{
							flag = this.TrySharingProperty(xamlReader, stack.CurrentFrame.Type, stack.CurrentFrame.Name, stack.InScopeNamespaces, out propertyValue);
						}
						catch
						{
							flag = false;
							propertyValue = null;
						}
						if (flag)
						{
							sharedProperties.Add(propertyValue.Value);
							if ((typeof(GridViewRowPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType)) && propertyValue.Value.Property.Name == "ContentSource")
							{
								stack.CurrentFrame.ContentSource = (propertyValue.Value.ValueInternal as string);
								if (!(propertyValue.Value.ValueInternal is string) && propertyValue.Value.ValueInternal != null)
								{
									stack.CurrentFrame.ContentSourceSet = false;
								}
							}
							return false;
						}
						xamlIndexingReader.CurrentIndex = currentIndex;
					}
				}
				break;
			case System.Xaml.XamlNodeType.EndMember:
				stack.CurrentFrame.Property = null;
				break;
			case System.Xaml.XamlNodeType.Value:
			{
				if (!stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (FrameworkTemplate.IsNameProperty(stack.CurrentFrame.Property, stack.CurrentFrame.Type))
					{
						string text = xamlReader.Value as string;
						stack.CurrentFrame.Name = text;
						stack.CurrentFrame.NameSet = true;
						if (this.TemplateLoadData.RootName == null)
						{
							this.TemplateLoadData.RootName = text;
						}
						if (!stack.CurrentFrame.IsInNameScope && (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType)))
						{
							this.TemplateLoadData.NamedTypes.Add(text, stack.CurrentFrame.Type);
							this.UpdateSharedPropertyNames(text, sharedProperties, stack.CurrentFrame.Type);
						}
					}
					if (typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) && stack.CurrentFrame.Property.Name == "ContentSource")
					{
						stack.CurrentFrame.ContentSource = (xamlReader.Value as string);
					}
				}
				StaticResourceExtension staticResourceExtension = xamlReader.Value as StaticResourceExtension;
				if (staticResourceExtension != null)
				{
					object obj = null;
					if (staticResourceExtension.GetType() == typeof(StaticResourceExtension))
					{
						obj = staticResourceExtension.TryProvideValueInternal(this.TemplateLoadData.ServiceProviderWrapper, true, true);
					}
					else if (staticResourceExtension.GetType() == typeof(StaticResourceHolder))
					{
						obj = staticResourceExtension.FindResourceInDeferredContent(this.TemplateLoadData.ServiceProviderWrapper, true, false);
						if (obj == DependencyProperty.UnsetValue)
						{
							obj = null;
						}
					}
					if (obj != null)
					{
						DeferredResourceReference prefetchedValue = obj as DeferredResourceReference;
						newValue = new StaticResourceHolder(staticResourceExtension.ResourceKey, prefetchedValue);
					}
				}
				break;
			}
			case System.Xaml.XamlNodeType.NamespaceDeclaration:
				if (!stack.CurrentFrame.IsInStyleOrTemplate && stack.Depth > 0 && !stack.CurrentFrame.NameSet && stack.CurrentFrame.Type != null && !stack.CurrentFrame.IsInNameScope)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						int num = nameNumber;
						nameNumber = num + 1;
						string name4 = num.ToString(CultureInfo.InvariantCulture) + "_T";
						this.UpdateSharedPropertyNames(name4, sharedProperties, stack.CurrentFrame.Type);
						stack.CurrentFrame.Name = name4;
					}
					stack.CurrentFrame.NameSet = true;
				}
				stack.AddNamespace(xamlReader.Namespace);
				break;
			}
			return true;
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x00196584 File Offset: 0x00195584
		private StaticResourceExtension LoadTimeBindUnshareableStaticResource(System.Xaml.XamlReader xamlReader, XamlObjectWriter writer)
		{
			int num = 0;
			do
			{
				writer.WriteNode(xamlReader);
				System.Xaml.XamlNodeType nodeType = xamlReader.NodeType;
				if (nodeType - System.Xaml.XamlNodeType.StartObject > 1)
				{
					if (nodeType == System.Xaml.XamlNodeType.EndObject)
					{
						num--;
					}
				}
				else
				{
					num++;
				}
			}
			while (num > 0 && xamlReader.Read());
			StaticResourceExtension staticResourceExtension = writer.Result as StaticResourceExtension;
			DeferredResourceReference prefetchedValue = (DeferredResourceReference)staticResourceExtension.TryProvideValueInternal(this.TemplateLoadData.ServiceProviderWrapper, true, true);
			return new StaticResourceHolder(staticResourceExtension.ResourceKey, prefetchedValue);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x001965F4 File Offset: 0x001955F4
		private bool TrySharingProperty(System.Xaml.XamlReader xamlReader, XamlType parentType, string parentName, FrugalObjectList<NamespaceDeclaration> previousNamespaces, out PropertyValue? sharedValue)
		{
			WpfXamlMember wpfXamlMember = xamlReader.Member as WpfXamlMember;
			if (wpfXamlMember == null)
			{
				sharedValue = null;
				return false;
			}
			DependencyProperty dependencyProperty = wpfXamlMember.DependencyProperty;
			if (dependencyProperty == null)
			{
				sharedValue = null;
				return false;
			}
			if (xamlReader.Member == parentType.GetAliasedProperty(XamlLanguage.Name))
			{
				sharedValue = null;
				return false;
			}
			if (!typeof(FrameworkElement).IsAssignableFrom(parentType.UnderlyingType) && !typeof(FrameworkContentElement).IsAssignableFrom(parentType.UnderlyingType))
			{
				sharedValue = null;
				return false;
			}
			xamlReader.Read();
			if (xamlReader.NodeType == System.Xaml.XamlNodeType.Value)
			{
				if (xamlReader.Value == null)
				{
					sharedValue = null;
					return false;
				}
				if (!TemplateContent.CheckSpecialCasesShareable(xamlReader.Value.GetType(), dependencyProperty))
				{
					sharedValue = null;
					return false;
				}
				if (!(xamlReader.Value is string))
				{
					return this.TrySharingValue(dependencyProperty, xamlReader.Value, parentName, xamlReader, true, out sharedValue);
				}
				object value = xamlReader.Value;
				TypeConverter typeConverter = null;
				if (wpfXamlMember.TypeConverter != null)
				{
					typeConverter = wpfXamlMember.TypeConverter.ConverterInstance;
				}
				else if (wpfXamlMember.Type.TypeConverter != null)
				{
					typeConverter = wpfXamlMember.Type.TypeConverter.ConverterInstance;
				}
				if (typeConverter != null)
				{
					value = typeConverter.ConvertFrom(this.TemplateLoadData.ServiceProviderWrapper, CultureInfo.InvariantCulture, value);
				}
				return this.TrySharingValue(dependencyProperty, value, parentName, xamlReader, true, out sharedValue);
			}
			else
			{
				if (xamlReader.NodeType == System.Xaml.XamlNodeType.StartObject || xamlReader.NodeType == System.Xaml.XamlNodeType.NamespaceDeclaration)
				{
					FrugalObjectList<NamespaceDeclaration> frugalObjectList = null;
					if (xamlReader.NodeType == System.Xaml.XamlNodeType.NamespaceDeclaration)
					{
						frugalObjectList = new FrugalObjectList<NamespaceDeclaration>();
						while (xamlReader.NodeType == System.Xaml.XamlNodeType.NamespaceDeclaration)
						{
							frugalObjectList.Add(xamlReader.Namespace);
							xamlReader.Read();
						}
					}
					if (!TemplateContent.CheckSpecialCasesShareable(xamlReader.Type.UnderlyingType, dependencyProperty))
					{
						sharedValue = null;
						return false;
					}
					if (!this.IsTypeShareable(xamlReader.Type.UnderlyingType))
					{
						sharedValue = null;
						return false;
					}
					TemplateContent.StackOfFrames stackOfFrames = new TemplateContent.StackOfFrames();
					stackOfFrames.Push(xamlReader.Type, null);
					bool flag = false;
					bool flag2 = false;
					if (typeof(FrameworkTemplate).IsAssignableFrom(xamlReader.Type.UnderlyingType))
					{
						flag = true;
						this.Stack = stackOfFrames;
					}
					else if (typeof(Style).IsAssignableFrom(xamlReader.Type.UnderlyingType))
					{
						flag2 = true;
						this.Stack = stackOfFrames;
					}
					try
					{
						XamlObjectWriter objectWriter = this.TemplateLoadData.ObjectWriter;
						objectWriter.Clear();
						this.WriteNamespaces(objectWriter, previousNamespaces, frugalObjectList);
						objectWriter.WriteNode(xamlReader);
						bool flag3 = false;
						while (!flag3 && xamlReader.Read())
						{
							TemplateContent.SkipFreeze(xamlReader);
							objectWriter.WriteNode(xamlReader);
							switch (xamlReader.NodeType)
							{
							case System.Xaml.XamlNodeType.StartObject:
								if (typeof(StaticResourceExtension).IsAssignableFrom(xamlReader.Type.UnderlyingType))
								{
									sharedValue = null;
									return false;
								}
								stackOfFrames.Push(xamlReader.Type, null);
								break;
							case System.Xaml.XamlNodeType.GetObject:
							{
								XamlType type = stackOfFrames.CurrentFrame.Property.Type;
								stackOfFrames.Push(type, null);
								break;
							}
							case System.Xaml.XamlNodeType.EndObject:
								if (stackOfFrames.Depth == 1)
								{
									return this.TrySharingValue(dependencyProperty, objectWriter.Result, parentName, xamlReader, true, out sharedValue);
								}
								stackOfFrames.PopScope();
								break;
							case System.Xaml.XamlNodeType.StartMember:
								if (!flag2 && !flag && FrameworkTemplate.IsNameProperty(xamlReader.Member, stackOfFrames.CurrentFrame.Type))
								{
									flag3 = true;
								}
								else
								{
									stackOfFrames.CurrentFrame.Property = xamlReader.Member;
								}
								break;
							case System.Xaml.XamlNodeType.Value:
								if (xamlReader.Value != null && typeof(StaticResourceExtension).IsAssignableFrom(xamlReader.Value.GetType()))
								{
									sharedValue = null;
									return false;
								}
								if (!flag && stackOfFrames.CurrentFrame.Property == XamlLanguage.ConnectionId && this.OwnerTemplate.StyleConnector != null)
								{
									this.OwnerTemplate.StyleConnector.Connect((int)xamlReader.Value, stackOfFrames.CurrentFrame.Instance);
								}
								break;
							}
						}
						sharedValue = null;
						return false;
					}
					finally
					{
						this.Stack = null;
					}
				}
				if (xamlReader.NodeType == System.Xaml.XamlNodeType.GetObject)
				{
					sharedValue = null;
					return false;
				}
				throw new System.Windows.Markup.XamlParseException(SR.Get("ParserUnexpectedEndEle"));
			}
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x00196A78 File Offset: 0x00195A78
		private static bool CheckSpecialCasesShareable(Type typeofValue, DependencyProperty property)
		{
			if (typeofValue != typeof(DynamicResourceExtension) && typeofValue != typeof(TemplateBindingExtension) && typeofValue != typeof(TypeExtension) && typeofValue != typeof(StaticExtension))
			{
				if (typeof(IList).IsAssignableFrom(property.PropertyType))
				{
					return false;
				}
				if (property.PropertyType.IsArray)
				{
					return false;
				}
				if (typeof(IDictionary).IsAssignableFrom(property.PropertyType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x00196B10 File Offset: 0x00195B10
		private static bool IsFreezableDirective(System.Xaml.XamlReader reader)
		{
			if (reader.NodeType == System.Xaml.XamlNodeType.StartMember)
			{
				XamlMember member = reader.Member;
				return member.IsUnknown && member.IsDirective && member.Name == "Freeze";
			}
			return false;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x00196B51 File Offset: 0x00195B51
		private static void SkipFreeze(System.Xaml.XamlReader reader)
		{
			if (TemplateContent.IsFreezableDirective(reader))
			{
				reader.Read();
				reader.Read();
				reader.Read();
			}
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x00196B70 File Offset: 0x00195B70
		private bool TrySharingValue(DependencyProperty property, object value, string parentName, System.Xaml.XamlReader xamlReader, bool allowRecursive, out PropertyValue? sharedValue)
		{
			sharedValue = null;
			if (value != null && !this.IsTypeShareable(value.GetType()))
			{
				return false;
			}
			bool flag = true;
			if (value is Freezable)
			{
				Freezable freezable = value as Freezable;
				if (freezable != null)
				{
					if (freezable.CanFreeze)
					{
						freezable.Freeze();
					}
					else
					{
						flag = false;
					}
				}
			}
			else if (value is CollectionViewSource)
			{
				CollectionViewSource collectionViewSource = value as CollectionViewSource;
				if (collectionViewSource != null)
				{
					flag = collectionViewSource.IsShareableInTemplate();
				}
			}
			else if (value is MarkupExtension)
			{
				if (value is BindingBase || value is TemplateBindingExtension || value is DynamicResourceExtension)
				{
					flag = true;
				}
				else if (value is StaticResourceExtension || value is StaticResourceHolder)
				{
					flag = false;
				}
				else
				{
					this.TemplateLoadData.ServiceProviderWrapper.SetData(TemplateContent._sharedDpInstance, property);
					value = (value as MarkupExtension).ProvideValue(this.TemplateLoadData.ServiceProviderWrapper);
					this.TemplateLoadData.ServiceProviderWrapper.Clear();
					if (allowRecursive)
					{
						return this.TrySharingValue(property, value, parentName, xamlReader, false, out sharedValue);
					}
					flag = true;
				}
			}
			if (flag)
			{
				sharedValue = new PropertyValue?(new PropertyValue
				{
					Property = property,
					ChildName = parentName,
					ValueInternal = value,
					ValueType = PropertyValueType.Set
				});
				xamlReader.Read();
			}
			return flag;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x00196CBC File Offset: 0x00195CBC
		private bool IsTypeShareable(Type type)
		{
			return typeof(Freezable).IsAssignableFrom(type) || type == typeof(string) || type == typeof(Uri) || type == typeof(Type) || (typeof(MarkupExtension).IsAssignableFrom(type) && !typeof(StaticResourceExtension).IsAssignableFrom(type)) || typeof(Style).IsAssignableFrom(type) || typeof(FrameworkTemplate).IsAssignableFrom(type) || typeof(CollectionViewSource).IsAssignableFrom(type) || (type != null && type.IsValueType);
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x00196D88 File Offset: 0x00195D88
		private void WriteNamespaces(System.Xaml.XamlWriter writer, FrugalObjectList<NamespaceDeclaration> previousNamespaces, FrugalObjectList<NamespaceDeclaration> localNamespaces)
		{
			if (previousNamespaces != null)
			{
				for (int i = 0; i < previousNamespaces.Count; i++)
				{
					writer.WriteNamespace(previousNamespaces[i]);
				}
			}
			if (localNamespaces != null)
			{
				for (int j = 0; j < localNamespaces.Count; j++)
				{
					writer.WriteNamespace(localNamespaces[j]);
				}
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x00196DD8 File Offset: 0x00195DD8
		private static void AutoAliasContentPresenter(Type targetType, string contentSource, string templateChildName, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref HybridDictionary dataTriggerRecordFromBinding, ref bool hasInstanceValues, HybridDictionary childIndexFromChildName, bool isContentPropertyDefined, bool isContentSourceSet, bool isContentTemplatePropertyDefined, bool isContentTemplateSelectorPropertyDefined, bool isContentStringFormatPropertyDefined)
		{
			if (string.IsNullOrEmpty(contentSource) && !isContentSourceSet)
			{
				contentSource = "Content";
			}
			if (!string.IsNullOrEmpty(contentSource) && !isContentPropertyDefined)
			{
				DependencyProperty dependencyProperty = DependencyProperty.FromName(contentSource, targetType);
				DependencyProperty dependencyProperty2 = DependencyProperty.FromName(contentSource + "Template", targetType);
				DependencyProperty dependencyProperty3 = DependencyProperty.FromName(contentSource + "TemplateSelector", targetType);
				DependencyProperty dependencyProperty4 = DependencyProperty.FromName(contentSource + "StringFormat", targetType);
				if (dependencyProperty == null && isContentSourceSet)
				{
					throw new InvalidOperationException(SR.Get("MissingContentSource", new object[]
					{
						contentSource,
						targetType
					}));
				}
				if (dependencyProperty != null)
				{
					PropertyValue propertyValue = default(PropertyValue);
					propertyValue.ValueType = PropertyValueType.TemplateBinding;
					propertyValue.ChildName = templateChildName;
					propertyValue.ValueInternal = new TemplateBindingExtension(dependencyProperty);
					propertyValue.Property = ContentPresenter.ContentProperty;
					StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
				}
				if (!isContentTemplatePropertyDefined && !isContentTemplateSelectorPropertyDefined && !isContentStringFormatPropertyDefined)
				{
					if (dependencyProperty2 != null)
					{
						PropertyValue propertyValue2 = default(PropertyValue);
						propertyValue2.ValueType = PropertyValueType.TemplateBinding;
						propertyValue2.ChildName = templateChildName;
						propertyValue2.ValueInternal = new TemplateBindingExtension(dependencyProperty2);
						propertyValue2.Property = ContentPresenter.ContentTemplateProperty;
						StyleHelper.UpdateTables(ref propertyValue2, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
					}
					if (dependencyProperty3 != null)
					{
						PropertyValue propertyValue3 = default(PropertyValue);
						propertyValue3.ValueType = PropertyValueType.TemplateBinding;
						propertyValue3.ChildName = templateChildName;
						propertyValue3.ValueInternal = new TemplateBindingExtension(dependencyProperty3);
						propertyValue3.Property = ContentPresenter.ContentTemplateSelectorProperty;
						StyleHelper.UpdateTables(ref propertyValue3, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
					}
					if (dependencyProperty4 != null)
					{
						PropertyValue propertyValue4 = default(PropertyValue);
						propertyValue4.ValueType = PropertyValueType.TemplateBinding;
						propertyValue4.ChildName = templateChildName;
						propertyValue4.ValueInternal = new TemplateBindingExtension(dependencyProperty4);
						propertyValue4.Property = ContentPresenter.ContentStringFormatProperty;
						StyleHelper.UpdateTables(ref propertyValue4, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
					}
				}
			}
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x00196F9C File Offset: 0x00195F9C
		private static void AutoAliasGridViewRowPresenter(Type targetType, string contentSource, string childName, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref HybridDictionary dataTriggerRecordFromBinding, ref bool hasInstanceValues, HybridDictionary childIndexFromChildID, bool isContentPropertyDefined, bool isColumnsPropertyDefined)
		{
			if (!isContentPropertyDefined)
			{
				DependencyProperty dependencyProperty = DependencyProperty.FromName("Content", targetType);
				if (dependencyProperty != null)
				{
					PropertyValue propertyValue = default(PropertyValue);
					propertyValue.ValueType = PropertyValueType.TemplateBinding;
					propertyValue.ChildName = childName;
					propertyValue.ValueInternal = new TemplateBindingExtension(dependencyProperty);
					propertyValue.Property = GridViewRowPresenter.ContentProperty;
					StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
				}
			}
			if (!isColumnsPropertyDefined)
			{
				PropertyValue propertyValue2 = default(PropertyValue);
				propertyValue2.ValueType = PropertyValueType.TemplateBinding;
				propertyValue2.ChildName = childName;
				propertyValue2.ValueInternal = new TemplateBindingExtension(GridView.ColumnCollectionProperty);
				propertyValue2.Property = GridViewRowPresenterBase.ColumnsProperty;
				StyleHelper.UpdateTables(ref propertyValue2, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x0019704A File Offset: 0x0019604A
		// (set) Token: 0x060028A2 RID: 10402 RVA: 0x00197052 File Offset: 0x00196052
		internal XamlType RootType { get; private set; }

		// Token: 0x060028A3 RID: 10403 RVA: 0x0019705B File Offset: 0x0019605B
		internal XamlType GetTypeForName(string name)
		{
			return this.TemplateLoadData.NamedTypes[name];
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x0019706E File Offset: 0x0019606E
		// (set) Token: 0x060028A5 RID: 10405 RVA: 0x00197076 File Offset: 0x00196076
		internal FrameworkTemplate OwnerTemplate { get; set; }

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x060028A6 RID: 10406 RVA: 0x0019707F File Offset: 0x0019607F
		// (set) Token: 0x060028A7 RID: 10407 RVA: 0x00197087 File Offset: 0x00196087
		internal IXamlObjectWriterFactory ObjectWriterFactory { get; private set; }

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x00197090 File Offset: 0x00196090
		// (set) Token: 0x060028A9 RID: 10409 RVA: 0x00197098 File Offset: 0x00196098
		internal XamlObjectWriterSettings ObjectWriterParentSettings { get; private set; }

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x001970A1 File Offset: 0x001960A1
		// (set) Token: 0x060028AB RID: 10411 RVA: 0x001970A9 File Offset: 0x001960A9
		internal XamlSchemaContext SchemaContext { get; private set; }

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x001970B2 File Offset: 0x001960B2
		// (set) Token: 0x060028AD RID: 10413 RVA: 0x001970BF File Offset: 0x001960BF
		private TemplateContent.StackOfFrames Stack
		{
			get
			{
				return this.TemplateLoadData.Stack;
			}
			set
			{
				this.TemplateLoadData.Stack = value;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x001970CD File Offset: 0x001960CD
		// (set) Token: 0x060028AF RID: 10415 RVA: 0x001970D5 File Offset: 0x001960D5
		internal TemplateLoadData TemplateLoadData { get; set; }

		// Token: 0x04001495 RID: 5269
		internal XamlNodeList _xamlNodeList;

		// Token: 0x04001496 RID: 5270
		private static SharedDp _sharedDpInstance = new SharedDp(null, null, null);

		// Token: 0x02000A92 RID: 2706
		internal class Frame : XamlFrame
		{
			// Token: 0x17001E29 RID: 7721
			// (get) Token: 0x0600869A RID: 34458 RVA: 0x0032B0CB File Offset: 0x0032A0CB
			// (set) Token: 0x0600869B RID: 34459 RVA: 0x0032B0D3 File Offset: 0x0032A0D3
			public XamlType Type
			{
				get
				{
					return this._xamlType;
				}
				set
				{
					this._xamlType = value;
				}
			}

			// Token: 0x17001E2A RID: 7722
			// (get) Token: 0x0600869C RID: 34460 RVA: 0x0032B0DC File Offset: 0x0032A0DC
			// (set) Token: 0x0600869D RID: 34461 RVA: 0x0032B0E4 File Offset: 0x0032A0E4
			public XamlMember Property { get; set; }

			// Token: 0x17001E2B RID: 7723
			// (get) Token: 0x0600869E RID: 34462 RVA: 0x0032B0ED File Offset: 0x0032A0ED
			// (set) Token: 0x0600869F RID: 34463 RVA: 0x0032B0F5 File Offset: 0x0032A0F5
			public string Name { get; set; }

			// Token: 0x17001E2C RID: 7724
			// (get) Token: 0x060086A0 RID: 34464 RVA: 0x0032B0FE File Offset: 0x0032A0FE
			// (set) Token: 0x060086A1 RID: 34465 RVA: 0x0032B106 File Offset: 0x0032A106
			public bool NameSet { get; set; }

			// Token: 0x17001E2D RID: 7725
			// (get) Token: 0x060086A2 RID: 34466 RVA: 0x0032B10F File Offset: 0x0032A10F
			// (set) Token: 0x060086A3 RID: 34467 RVA: 0x0032B117 File Offset: 0x0032A117
			public bool IsInNameScope { get; set; }

			// Token: 0x17001E2E RID: 7726
			// (get) Token: 0x060086A4 RID: 34468 RVA: 0x0032B120 File Offset: 0x0032A120
			// (set) Token: 0x060086A5 RID: 34469 RVA: 0x0032B128 File Offset: 0x0032A128
			public bool IsInStyleOrTemplate { get; set; }

			// Token: 0x17001E2F RID: 7727
			// (get) Token: 0x060086A6 RID: 34470 RVA: 0x0032B131 File Offset: 0x0032A131
			// (set) Token: 0x060086A7 RID: 34471 RVA: 0x0032B139 File Offset: 0x0032A139
			public object Instance { get; set; }

			// Token: 0x17001E30 RID: 7728
			// (get) Token: 0x060086A8 RID: 34472 RVA: 0x0032B142 File Offset: 0x0032A142
			// (set) Token: 0x060086A9 RID: 34473 RVA: 0x0032B14A File Offset: 0x0032A14A
			public bool ContentSet { get; set; }

			// Token: 0x17001E31 RID: 7729
			// (get) Token: 0x060086AA RID: 34474 RVA: 0x0032B153 File Offset: 0x0032A153
			// (set) Token: 0x060086AB RID: 34475 RVA: 0x0032B15B File Offset: 0x0032A15B
			public bool ContentSourceSet { get; set; }

			// Token: 0x17001E32 RID: 7730
			// (get) Token: 0x060086AC RID: 34476 RVA: 0x0032B164 File Offset: 0x0032A164
			// (set) Token: 0x060086AD RID: 34477 RVA: 0x0032B16C File Offset: 0x0032A16C
			public string ContentSource { get; set; }

			// Token: 0x17001E33 RID: 7731
			// (get) Token: 0x060086AE RID: 34478 RVA: 0x0032B175 File Offset: 0x0032A175
			// (set) Token: 0x060086AF RID: 34479 RVA: 0x0032B17D File Offset: 0x0032A17D
			public bool ContentTemplateSet { get; set; }

			// Token: 0x17001E34 RID: 7732
			// (get) Token: 0x060086B0 RID: 34480 RVA: 0x0032B186 File Offset: 0x0032A186
			// (set) Token: 0x060086B1 RID: 34481 RVA: 0x0032B18E File Offset: 0x0032A18E
			public bool ContentTemplateSelectorSet { get; set; }

			// Token: 0x17001E35 RID: 7733
			// (get) Token: 0x060086B2 RID: 34482 RVA: 0x0032B197 File Offset: 0x0032A197
			// (set) Token: 0x060086B3 RID: 34483 RVA: 0x0032B19F File Offset: 0x0032A19F
			public bool ContentStringFormatSet { get; set; }

			// Token: 0x17001E36 RID: 7734
			// (get) Token: 0x060086B4 RID: 34484 RVA: 0x0032B1A8 File Offset: 0x0032A1A8
			// (set) Token: 0x060086B5 RID: 34485 RVA: 0x0032B1B0 File Offset: 0x0032A1B0
			public bool ColumnsSet { get; set; }

			// Token: 0x060086B6 RID: 34486 RVA: 0x0032B1BC File Offset: 0x0032A1BC
			public override void Reset()
			{
				this._xamlType = null;
				this.Property = null;
				this.Name = null;
				this.NameSet = false;
				this.IsInNameScope = false;
				this.Instance = null;
				this.ContentSet = false;
				this.ContentSourceSet = false;
				this.ContentSource = null;
				this.ContentTemplateSet = false;
				this.ContentTemplateSelectorSet = false;
				this.ContentStringFormatSet = false;
				this.IsInNameScope = false;
				if (this.HasNamespaces)
				{
					this._namespaces = null;
				}
			}

			// Token: 0x17001E37 RID: 7735
			// (get) Token: 0x060086B7 RID: 34487 RVA: 0x0032B233 File Offset: 0x0032A233
			public FrugalObjectList<NamespaceDeclaration> Namespaces
			{
				get
				{
					if (this._namespaces == null)
					{
						this._namespaces = new FrugalObjectList<NamespaceDeclaration>();
					}
					return this._namespaces;
				}
			}

			// Token: 0x17001E38 RID: 7736
			// (get) Token: 0x060086B8 RID: 34488 RVA: 0x0032B24E File Offset: 0x0032A24E
			public bool HasNamespaces
			{
				get
				{
					return this._namespaces != null && this._namespaces.Count > 0;
				}
			}

			// Token: 0x060086B9 RID: 34489 RVA: 0x0032B268 File Offset: 0x0032A268
			public override string ToString()
			{
				string arg = (this.Type == null) ? string.Empty : this.Type.Name;
				string arg2 = (this.Property == null) ? "-" : this.Property.Name;
				string arg3 = (this.Instance == null) ? "-" : "*";
				return string.Format(CultureInfo.InvariantCulture, "{0}.{1} inst={2}", arg, arg2, arg3);
			}

			// Token: 0x0400426A RID: 17002
			private FrugalObjectList<NamespaceDeclaration> _namespaces;

			// Token: 0x0400426B RID: 17003
			private XamlType _xamlType;
		}

		// Token: 0x02000A93 RID: 2707
		internal class StackOfFrames : XamlContextStack<TemplateContent.Frame>
		{
			// Token: 0x060086BA RID: 34490 RVA: 0x0032B2DE File Offset: 0x0032A2DE
			public StackOfFrames() : base(() => new TemplateContent.Frame())
			{
			}

			// Token: 0x060086BB RID: 34491 RVA: 0x0032B308 File Offset: 0x0032A308
			public void Push(XamlType xamlType, string name)
			{
				bool isInNameScope = false;
				bool isInStyleOrTemplate = false;
				if (base.Depth > 0)
				{
					isInNameScope = (base.CurrentFrame.IsInNameScope || (base.CurrentFrame.Type != null && FrameworkTemplate.IsNameScope(base.CurrentFrame.Type)));
					isInStyleOrTemplate = (base.CurrentFrame.IsInStyleOrTemplate || (base.CurrentFrame.Type != null && (typeof(FrameworkTemplate).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType) || typeof(Style).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType))));
				}
				if (base.Depth == 0 || base.CurrentFrame.Type != null)
				{
					base.PushScope();
				}
				base.CurrentFrame.Type = xamlType;
				base.CurrentFrame.Name = name;
				base.CurrentFrame.IsInNameScope = isInNameScope;
				base.CurrentFrame.IsInStyleOrTemplate = isInStyleOrTemplate;
			}

			// Token: 0x060086BC RID: 34492 RVA: 0x0032B418 File Offset: 0x0032A418
			public void AddNamespace(NamespaceDeclaration nsd)
			{
				bool isInNameScope = false;
				bool isInStyleOrTemplate = false;
				if (base.Depth > 0)
				{
					isInNameScope = (base.CurrentFrame.IsInNameScope || (base.CurrentFrame.Type != null && FrameworkTemplate.IsNameScope(base.CurrentFrame.Type)));
					isInStyleOrTemplate = (base.CurrentFrame.IsInStyleOrTemplate || (base.CurrentFrame.Type != null && (typeof(FrameworkTemplate).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType) || typeof(Style).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType))));
				}
				if (base.Depth == 0 || base.CurrentFrame.Type != null)
				{
					base.PushScope();
				}
				base.CurrentFrame.Namespaces.Add(nsd);
				base.CurrentFrame.IsInNameScope = isInNameScope;
				base.CurrentFrame.IsInStyleOrTemplate = isInStyleOrTemplate;
			}

			// Token: 0x17001E39 RID: 7737
			// (get) Token: 0x060086BD RID: 34493 RVA: 0x0032B524 File Offset: 0x0032A524
			public FrugalObjectList<NamespaceDeclaration> InScopeNamespaces
			{
				get
				{
					FrugalObjectList<NamespaceDeclaration> frugalObjectList = null;
					for (TemplateContent.Frame frame = base.CurrentFrame; frame != null; frame = (TemplateContent.Frame)frame.Previous)
					{
						if (frame.HasNamespaces)
						{
							if (frugalObjectList == null)
							{
								frugalObjectList = new FrugalObjectList<NamespaceDeclaration>();
							}
							for (int i = 0; i < frame.Namespaces.Count; i++)
							{
								frugalObjectList.Add(frame.Namespaces[i]);
							}
						}
					}
					return frugalObjectList;
				}
			}
		}

		// Token: 0x02000A94 RID: 2708
		internal class ServiceProviderWrapper : ITypeDescriptorContext, IServiceProvider, IXamlTypeResolver, IXamlNamespaceResolver, IProvideValueTarget
		{
			// Token: 0x17001E3A RID: 7738
			// (get) Token: 0x060086BE RID: 34494 RVA: 0x0032B586 File Offset: 0x0032A586
			// (set) Token: 0x060086BF RID: 34495 RVA: 0x0032B58E File Offset: 0x0032A58E
			internal TemplateContent.StackOfFrames Frames { get; set; }

			// Token: 0x060086C0 RID: 34496 RVA: 0x0032B597 File Offset: 0x0032A597
			public ServiceProviderWrapper(IServiceProvider services, XamlSchemaContext schemaContext)
			{
				this._services = services;
				this._schemaContext = schemaContext;
			}

			// Token: 0x060086C1 RID: 34497 RVA: 0x0032B5AD File Offset: 0x0032A5AD
			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IXamlTypeResolver))
				{
					return this;
				}
				if (serviceType == typeof(IProvideValueTarget))
				{
					return this;
				}
				return this._services.GetService(serviceType);
			}

			// Token: 0x060086C2 RID: 34498 RVA: 0x0032B5E3 File Offset: 0x0032A5E3
			Type IXamlTypeResolver.Resolve(string qualifiedTypeName)
			{
				return this._schemaContext.GetXamlType(XamlTypeName.Parse(qualifiedTypeName, this)).UnderlyingType;
			}

			// Token: 0x060086C3 RID: 34499 RVA: 0x0032B5FC File Offset: 0x0032A5FC
			string IXamlNamespaceResolver.GetNamespace(string prefix)
			{
				FrugalObjectList<NamespaceDeclaration> inScopeNamespaces = this.Frames.InScopeNamespaces;
				if (inScopeNamespaces != null)
				{
					for (int i = 0; i < inScopeNamespaces.Count; i++)
					{
						if (inScopeNamespaces[i].Prefix == prefix)
						{
							return inScopeNamespaces[i].Namespace;
						}
					}
				}
				return ((IXamlNamespaceResolver)this._services.GetService(typeof(IXamlNamespaceResolver))).GetNamespace(prefix);
			}

			// Token: 0x060086C4 RID: 34500 RVA: 0x001056E1 File Offset: 0x001046E1
			IEnumerable<NamespaceDeclaration> IXamlNamespaceResolver.GetNamespacePrefixes()
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001E3B RID: 7739
			// (get) Token: 0x060086C5 RID: 34501 RVA: 0x00109403 File Offset: 0x00108403
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001E3C RID: 7740
			// (get) Token: 0x060086C6 RID: 34502 RVA: 0x00109403 File Offset: 0x00108403
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060086C7 RID: 34503 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			// Token: 0x060086C8 RID: 34504 RVA: 0x00105F35 File Offset: 0x00104F35
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			// Token: 0x17001E3D RID: 7741
			// (get) Token: 0x060086C9 RID: 34505 RVA: 0x00109403 File Offset: 0x00108403
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060086CA RID: 34506 RVA: 0x0032B66A File Offset: 0x0032A66A
			public void SetData(object targetObject, object targetProperty)
			{
				this._targetObject = targetObject;
				this._targetProperty = targetProperty;
			}

			// Token: 0x060086CB RID: 34507 RVA: 0x0032B67A File Offset: 0x0032A67A
			public void Clear()
			{
				this._targetObject = null;
				this._targetProperty = null;
			}

			// Token: 0x17001E3E RID: 7742
			// (get) Token: 0x060086CC RID: 34508 RVA: 0x0032B68A File Offset: 0x0032A68A
			object IProvideValueTarget.TargetObject
			{
				get
				{
					return this._targetObject;
				}
			}

			// Token: 0x17001E3F RID: 7743
			// (get) Token: 0x060086CD RID: 34509 RVA: 0x0032B692 File Offset: 0x0032A692
			object IProvideValueTarget.TargetProperty
			{
				get
				{
					return this._targetProperty;
				}
			}

			// Token: 0x04004279 RID: 17017
			private IServiceProvider _services;

			// Token: 0x0400427B RID: 17019
			private XamlSchemaContext _schemaContext;

			// Token: 0x0400427C RID: 17020
			private object _targetObject;

			// Token: 0x0400427D RID: 17021
			private object _targetProperty;
		}
	}
}
