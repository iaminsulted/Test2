using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;
using System.Xml;

namespace MS.Internal.Globalization
{
	// Token: 0x020001B2 RID: 434
	internal static class BamlTreeUpdater
	{
		// Token: 0x06000E1F RID: 3615 RVA: 0x001373C0 File Offset: 0x001363C0
		internal static void UpdateTree(BamlTree tree, BamlTreeMap treeMap, BamlLocalizationDictionary dictionary)
		{
			if (dictionary.Count <= 0)
			{
				return;
			}
			BamlTreeUpdater.BamlTreeUpdateMap treeMap2 = new BamlTreeUpdater.BamlTreeUpdateMap(treeMap, tree);
			BamlTreeUpdater.CreateMissingBamlTreeNode(dictionary, treeMap2);
			BamlLocalizationDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				if (!BamlTreeUpdater.ApplyChangeToBamlTree(enumerator.Key, enumerator.Value, treeMap2))
				{
					arrayList.Add(enumerator.Entry);
				}
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)arrayList[i];
				BamlTreeUpdater.ApplyChangeToBamlTree((BamlLocalizableResourceKey)dictionaryEntry.Key, (BamlLocalizableResource)dictionaryEntry.Value, treeMap2);
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00137464 File Offset: 0x00136464
		private static void CreateMissingBamlTreeNode(BamlLocalizationDictionary dictionary, BamlTreeUpdater.BamlTreeUpdateMap treeMap)
		{
			BamlLocalizationDictionaryEnumerator enumerator = dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				BamlLocalizableResourceKey key = enumerator.Key;
				BamlLocalizableResource value = enumerator.Value;
				if (treeMap.MapKeyToBamlTreeNode(key) == null)
				{
					if (key.PropertyName == "$Content")
					{
						if (treeMap.MapUidToBamlTreeElementNode(key.Uid) == null)
						{
							BamlStartElementNode bamlStartElementNode = new BamlStartElementNode(treeMap.Resolver.ResolveAssemblyFromClass(key.ClassName), key.ClassName, false, false);
							bamlStartElementNode.AddChild(new BamlDefAttributeNode("Uid", key.Uid));
							BamlTreeUpdater.TryAddContentPropertyToNewElement(treeMap, bamlStartElementNode);
							bamlStartElementNode.AddChild(new BamlEndElementNode());
							treeMap.AddBamlTreeNode(key.Uid, key, bamlStartElementNode);
						}
					}
					else
					{
						BamlTreeNode node;
						if (key.PropertyName == "$LiteralContent")
						{
							node = new BamlLiteralContentNode(value.Content);
						}
						else
						{
							node = new BamlPropertyNode(treeMap.Resolver.ResolveAssemblyFromClass(key.ClassName), key.ClassName, key.PropertyName, value.Content, BamlAttributeUsage.Default);
						}
						treeMap.AddBamlTreeNode(null, key, node);
					}
				}
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00137574 File Offset: 0x00136574
		private static bool ApplyChangeToBamlTree(BamlLocalizableResourceKey key, BamlLocalizableResource resource, BamlTreeUpdater.BamlTreeUpdateMap treeMap)
		{
			if (resource == null || resource.Content == null || !resource.Modifiable)
			{
				return true;
			}
			if (!treeMap.LocalizationDictionary.Contains(key) && !treeMap.IsNewBamlTreeNode(key))
			{
				return true;
			}
			BamlTreeNode bamlTreeNode = treeMap.MapKeyToBamlTreeNode(key);
			Invariant.Assert(bamlTreeNode != null);
			BamlNodeType nodeType = bamlTreeNode.NodeType;
			if (nodeType != BamlNodeType.StartElement)
			{
				if (nodeType != BamlNodeType.Property)
				{
					if (nodeType == BamlNodeType.LiteralContent)
					{
						BamlLiteralContentNode bamlLiteralContentNode = (BamlLiteralContentNode)bamlTreeNode;
						bamlLiteralContentNode.Content = BamlResourceContentUtil.UnescapeString(resource.Content);
						if (bamlLiteralContentNode.Parent == null)
						{
							BamlTreeNode bamlTreeNode2 = treeMap.MapUidToBamlTreeElementNode(key.Uid);
							if (bamlTreeNode2 == null)
							{
								return false;
							}
							bamlTreeNode2.AddChild(bamlLiteralContentNode);
						}
					}
				}
				else
				{
					BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)bamlTreeNode;
					bamlPropertyNode.Value = BamlResourceContentUtil.UnescapeString(resource.Content);
					if (bamlPropertyNode.Parent == null)
					{
						BamlStartElementNode bamlStartElementNode = treeMap.MapUidToBamlTreeElementNode(key.Uid);
						if (bamlStartElementNode == null)
						{
							return false;
						}
						bamlStartElementNode.InsertProperty(bamlTreeNode);
					}
				}
			}
			else
			{
				string b = null;
				if (treeMap.LocalizationDictionary.Contains(key))
				{
					b = treeMap.LocalizationDictionary[key].Content;
				}
				if (resource.Content != b)
				{
					BamlTreeUpdater.ReArrangeChildren(key, bamlTreeNode, resource.Content, treeMap);
				}
			}
			return true;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00137698 File Offset: 0x00136698
		private static void ReArrangeChildren(BamlLocalizableResourceKey key, BamlTreeNode node, string translation, BamlTreeUpdater.BamlTreeUpdateMap treeMap)
		{
			IList<BamlTreeNode> newChildren = BamlTreeUpdater.SplitXmlContent(key, translation, treeMap);
			BamlTreeUpdater.MergeChildrenList(key, treeMap, node, newChildren);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x001376B8 File Offset: 0x001366B8
		private static void MergeChildrenList(BamlLocalizableResourceKey key, BamlTreeUpdater.BamlTreeUpdateMap treeMap, BamlTreeNode parent, IList<BamlTreeNode> newChildren)
		{
			if (newChildren == null)
			{
				return;
			}
			List<BamlTreeNode> children = parent.Children;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (children != null)
			{
				Hashtable hashtable = new Hashtable(newChildren.Count);
				foreach (BamlTreeNode bamlTreeNode in newChildren)
				{
					if (bamlTreeNode.NodeType == BamlNodeType.StartElement)
					{
						BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)bamlTreeNode;
						if (bamlStartElementNode.Uid != null)
						{
							if (hashtable.ContainsKey(bamlStartElementNode.Uid))
							{
								treeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.DuplicateElement));
								return;
							}
							hashtable[bamlStartElementNode.Uid] = null;
						}
					}
				}
				parent.Children = null;
				for (int j = 0; j < children.Count - 1; j++)
				{
					BamlTreeNode bamlTreeNode2 = children[j];
					BamlNodeType nodeType = bamlTreeNode2.NodeType;
					if (nodeType != BamlNodeType.StartElement)
					{
						if (nodeType != BamlNodeType.Text)
						{
							parent.AddChild(bamlTreeNode2);
						}
					}
					else
					{
						BamlStartElementNode bamlStartElementNode2 = (BamlStartElementNode)bamlTreeNode2;
						if (bamlStartElementNode2.Uid != null)
						{
							if (!hashtable.ContainsKey(bamlStartElementNode2.Uid))
							{
								parent.Children = children;
								treeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.MismatchedElements));
								return;
							}
							hashtable.Remove(bamlStartElementNode2.Uid);
						}
						while (i < newChildren.Count)
						{
							BamlTreeNode bamlTreeNode3 = newChildren[i++];
							Invariant.Assert(bamlTreeNode3 != null);
							if (bamlTreeNode3.NodeType == BamlNodeType.Text)
							{
								stringBuilder.Append(((BamlTextNode)bamlTreeNode3).Content);
							}
							else
							{
								BamlTreeUpdater.TryFlushTextToBamlNode(parent, stringBuilder);
								parent.AddChild(bamlTreeNode3);
								if (bamlTreeNode3.NodeType == BamlNodeType.StartElement)
								{
									break;
								}
							}
						}
					}
				}
			}
			while (i < newChildren.Count)
			{
				BamlTreeNode bamlTreeNode4 = newChildren[i];
				Invariant.Assert(bamlTreeNode4 != null);
				if (bamlTreeNode4.NodeType == BamlNodeType.Text)
				{
					stringBuilder.Append(((BamlTextNode)bamlTreeNode4).Content);
				}
				else
				{
					BamlTreeUpdater.TryFlushTextToBamlNode(parent, stringBuilder);
					parent.AddChild(bamlTreeNode4);
				}
				i++;
			}
			BamlTreeUpdater.TryFlushTextToBamlNode(parent, stringBuilder);
			parent.AddChild(new BamlEndElementNode());
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x001378D4 File Offset: 0x001368D4
		private static void TryFlushTextToBamlNode(BamlTreeNode parent, StringBuilder textContent)
		{
			if (textContent.Length > 0)
			{
				BamlTreeNode child = new BamlTextNode(textContent.ToString());
				parent.AddChild(child);
				textContent.Length = 0;
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00137904 File Offset: 0x00136904
		private static IList<BamlTreeNode> SplitXmlContent(BamlLocalizableResourceKey key, string content, BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<ROOT>");
			stringBuilder.Append(content);
			stringBuilder.Append("</ROOT>");
			IList<BamlTreeNode> list = new List<BamlTreeNode>(4);
			XmlDocument xmlDocument = new XmlDocument();
			bool flag = true;
			try
			{
				xmlDocument.LoadXml(stringBuilder.ToString());
				XmlElement xmlElement = xmlDocument.FirstChild as XmlElement;
				if (xmlElement != null && xmlElement.HasChildNodes)
				{
					int num = 0;
					while (num < xmlElement.ChildNodes.Count && flag)
					{
						flag = BamlTreeUpdater.GetBamlTreeNodeFromXmlNode(key, xmlElement.ChildNodes[num], bamlTreeMap, list);
						num++;
					}
				}
			}
			catch (XmlException)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.SubstitutionAsPlaintext));
				flag = BamlTreeUpdater.GetBamlTreeNodeFromText(key, content, bamlTreeMap, list);
			}
			if (!flag)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x001379DC File Offset: 0x001369DC
		private static bool GetBamlTreeNodeFromXmlNode(BamlLocalizableResourceKey key, XmlNode node, BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap, IList<BamlTreeNode> newChildrenList)
		{
			if (node.NodeType == XmlNodeType.Text)
			{
				return BamlTreeUpdater.GetBamlTreeNodeFromText(key, node.Value, bamlTreeMap, newChildrenList);
			}
			if (node.NodeType != XmlNodeType.Element)
			{
				return true;
			}
			XmlElement xmlElement = node as XmlElement;
			string text = bamlTreeMap.Resolver.ResolveFormattingTagToClass(xmlElement.Name);
			bool flag = string.IsNullOrEmpty(text);
			string text2 = null;
			if (!flag)
			{
				text2 = bamlTreeMap.Resolver.ResolveAssemblyFromClass(text);
				flag = string.IsNullOrEmpty(text2);
			}
			if (flag)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.UnknownFormattingTag));
				return false;
			}
			string text3 = null;
			if (xmlElement.HasAttributes)
			{
				text3 = xmlElement.GetAttribute("Uid");
				if (!string.IsNullOrEmpty(text3))
				{
					text3 = BamlResourceContentUtil.UnescapeString(text3);
				}
			}
			BamlStartElementNode bamlStartElementNode = null;
			if (text3 != null)
			{
				bamlStartElementNode = bamlTreeMap.MapUidToBamlTreeElementNode(text3);
			}
			if (bamlStartElementNode == null)
			{
				bamlStartElementNode = new BamlStartElementNode(text2, text, false, false);
				if (text3 != null)
				{
					bamlTreeMap.AddBamlTreeNode(text3, new BamlLocalizableResourceKey(text3, text, "$Content", text2), bamlStartElementNode);
					bamlStartElementNode.AddChild(new BamlDefAttributeNode("Uid", text3));
				}
				BamlTreeUpdater.TryAddContentPropertyToNewElement(bamlTreeMap, bamlStartElementNode);
				bamlStartElementNode.AddChild(new BamlEndElementNode());
			}
			else if (bamlStartElementNode.TypeFullName != text)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.DuplicateUid));
				return false;
			}
			newChildrenList.Add(bamlStartElementNode);
			bool flag2 = true;
			if (xmlElement.HasChildNodes)
			{
				IList<BamlTreeNode> list = new List<BamlTreeNode>();
				int num = 0;
				while (num < xmlElement.ChildNodes.Count && flag2)
				{
					flag2 = BamlTreeUpdater.GetBamlTreeNodeFromXmlNode(key, xmlElement.ChildNodes[num], bamlTreeMap, list);
					num++;
				}
				if (flag2)
				{
					BamlTreeUpdater.MergeChildrenList(key, bamlTreeMap, bamlStartElementNode, list);
				}
			}
			return flag2;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00137B74 File Offset: 0x00136B74
		private static bool GetBamlTreeNodeFromText(BamlLocalizableResourceKey key, string content, BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap, IList<BamlTreeNode> newChildrenList)
		{
			BamlStringToken[] array = BamlResourceContentUtil.ParseChildPlaceholder(content);
			if (array == null)
			{
				bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.IncompleteElementPlaceholder));
				return false;
			}
			bool result = true;
			for (int i = 0; i < array.Length; i++)
			{
				BamlStringToken.TokenType type = array[i].Type;
				if (type != BamlStringToken.TokenType.Text)
				{
					if (type == BamlStringToken.TokenType.ChildPlaceHolder)
					{
						BamlTreeNode bamlTreeNode = bamlTreeMap.MapUidToBamlTreeElementNode(array[i].Value);
						if (bamlTreeNode != null)
						{
							newChildrenList.Add(bamlTreeNode);
						}
						else
						{
							bamlTreeMap.Resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(new BamlLocalizableResourceKey(array[i].Value, string.Empty, string.Empty), BamlLocalizerError.InvalidUid));
							result = false;
						}
					}
				}
				else
				{
					BamlTreeNode item = new BamlTextNode(array[i].Value);
					newChildrenList.Add(item);
				}
			}
			return result;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00137C3C File Offset: 0x00136C3C
		private static void TryAddContentPropertyToNewElement(BamlTreeUpdater.BamlTreeUpdateMap bamlTreeMap, BamlStartElementNode bamlNode)
		{
			string contentProperty = bamlTreeMap.GetContentProperty(bamlNode.AssemblyName, bamlNode.TypeFullName);
			if (!string.IsNullOrEmpty(contentProperty))
			{
				bamlNode.AddChild(new BamlContentPropertyNode(bamlNode.AssemblyName, bamlNode.TypeFullName, contentProperty));
			}
		}

		// Token: 0x020009CD RID: 2509
		private class BamlTreeUpdateMap
		{
			// Token: 0x060083EB RID: 33771 RVA: 0x0032498A File Offset: 0x0032398A
			internal BamlTreeUpdateMap(BamlTreeMap map, BamlTree tree)
			{
				this._uidToNewBamlNodeIndexMap = new Hashtable(8);
				this._keyToNewBamlNodeIndexMap = new Hashtable(8);
				this._originalMap = map;
				this._tree = tree;
			}

			// Token: 0x060083EC RID: 33772 RVA: 0x003249B8 File Offset: 0x003239B8
			internal BamlTreeNode MapKeyToBamlTreeNode(BamlLocalizableResourceKey key)
			{
				BamlTreeNode bamlTreeNode = this._originalMap.MapKeyToBamlTreeNode(key, this._tree);
				if (bamlTreeNode == null && this._keyToNewBamlNodeIndexMap.Contains(key))
				{
					bamlTreeNode = this._tree[(int)this._keyToNewBamlNodeIndexMap[key]];
				}
				return bamlTreeNode;
			}

			// Token: 0x060083ED RID: 33773 RVA: 0x00324A07 File Offset: 0x00323A07
			internal bool IsNewBamlTreeNode(BamlLocalizableResourceKey key)
			{
				return this._keyToNewBamlNodeIndexMap.Contains(key);
			}

			// Token: 0x060083EE RID: 33774 RVA: 0x00324A18 File Offset: 0x00323A18
			internal BamlStartElementNode MapUidToBamlTreeElementNode(string uid)
			{
				BamlStartElementNode bamlStartElementNode = this._originalMap.MapUidToBamlTreeElementNode(uid, this._tree);
				if (bamlStartElementNode == null && this._uidToNewBamlNodeIndexMap.Contains(uid))
				{
					bamlStartElementNode = (this._tree[(int)this._uidToNewBamlNodeIndexMap[uid]] as BamlStartElementNode);
				}
				return bamlStartElementNode;
			}

			// Token: 0x060083EF RID: 33775 RVA: 0x00324A6C File Offset: 0x00323A6C
			internal void AddBamlTreeNode(string uid, BamlLocalizableResourceKey key, BamlTreeNode node)
			{
				this._tree.AddTreeNode(node);
				if (uid != null)
				{
					this._uidToNewBamlNodeIndexMap[uid] = this._tree.Size - 1;
				}
				this._keyToNewBamlNodeIndexMap[key] = this._tree.Size - 1;
			}

			// Token: 0x17001DA9 RID: 7593
			// (get) Token: 0x060083F0 RID: 33776 RVA: 0x00324AC4 File Offset: 0x00323AC4
			internal BamlLocalizationDictionary LocalizationDictionary
			{
				get
				{
					return this._originalMap.LocalizationDictionary;
				}
			}

			// Token: 0x17001DAA RID: 7594
			// (get) Token: 0x060083F1 RID: 33777 RVA: 0x00324AD1 File Offset: 0x00323AD1
			internal InternalBamlLocalizabilityResolver Resolver
			{
				get
				{
					return this._originalMap.Resolver;
				}
			}

			// Token: 0x060083F2 RID: 33778 RVA: 0x00324AE0 File Offset: 0x00323AE0
			internal string GetContentProperty(string assemblyName, string fullTypeName)
			{
				string clrNamespace = string.Empty;
				string typeShortName = fullTypeName;
				int num = fullTypeName.LastIndexOf('.');
				if (num >= 0)
				{
					clrNamespace = fullTypeName.Substring(0, num);
					typeShortName = fullTypeName.Substring(num + 1);
				}
				short knownTypeIdFromName = BamlMapTable.GetKnownTypeIdFromName(assemblyName, clrNamespace, typeShortName);
				if (knownTypeIdFromName != 0)
				{
					return KnownTypes.GetContentPropertyName((KnownElements)(-(KnownElements)knownTypeIdFromName));
				}
				string text = null;
				if (this._contentPropertyTable != null && this._contentPropertyTable.TryGetValue(fullTypeName, out text))
				{
					return text;
				}
				Type type = Assembly.Load(assemblyName).GetType(fullTypeName);
				if (type != null)
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(ContentPropertyAttribute), true);
					if (customAttributes.Length != 0)
					{
						text = (customAttributes[0] as ContentPropertyAttribute).Name;
						if (this._contentPropertyTable == null)
						{
							this._contentPropertyTable = new Dictionary<string, string>(8);
						}
						this._contentPropertyTable.Add(fullTypeName, text);
					}
				}
				return text;
			}

			// Token: 0x04003FB1 RID: 16305
			private BamlTreeMap _originalMap;

			// Token: 0x04003FB2 RID: 16306
			private BamlTree _tree;

			// Token: 0x04003FB3 RID: 16307
			private Hashtable _uidToNewBamlNodeIndexMap;

			// Token: 0x04003FB4 RID: 16308
			private Hashtable _keyToNewBamlNodeIndexMap;

			// Token: 0x04003FB5 RID: 16309
			private Dictionary<string, string> _contentPropertyTable;
		}
	}
}
