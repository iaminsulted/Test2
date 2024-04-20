using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;

namespace MS.Internal.Globalization
{
	// Token: 0x0200019A RID: 410
	internal class BamlTreeMap
	{
		// Token: 0x06000D90 RID: 3472 RVA: 0x0013607A File Offset: 0x0013507A
		internal BamlTreeMap(BamlLocalizer localizer, BamlTree tree, BamlLocalizabilityResolver resolver, TextReader comments)
		{
			this._tree = tree;
			this._resolver = new InternalBamlLocalizabilityResolver(localizer, resolver, comments);
			this._localizableResourceBuilder = new LocalizableResourceBuilder(this._resolver);
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x001360A9 File Offset: 0x001350A9
		internal BamlLocalizationDictionary LocalizationDictionary
		{
			get
			{
				this.EnsureMap();
				return this._localizableResources;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x001360B7 File Offset: 0x001350B7
		internal InternalBamlLocalizabilityResolver Resolver
		{
			get
			{
				return this._resolver;
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x001360BF File Offset: 0x001350BF
		internal BamlTreeNode MapKeyToBamlTreeNode(BamlLocalizableResourceKey key, BamlTree tree)
		{
			if (this._keyToBamlNodeIndexMap.Contains(key))
			{
				return tree[(int)this._keyToBamlNodeIndexMap[key]];
			}
			return null;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x001360E8 File Offset: 0x001350E8
		internal BamlStartElementNode MapUidToBamlTreeElementNode(string uid, BamlTree tree)
		{
			if (this._uidToBamlNodeIndexMap.Contains(uid))
			{
				return tree[(int)this._uidToBamlNodeIndexMap[uid]] as BamlStartElementNode;
			}
			return null;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00136118 File Offset: 0x00135118
		internal void EnsureMap()
		{
			if (this._localizableResources != null)
			{
				return;
			}
			this._resolver.InitLocalizabilityCache();
			this._keyToBamlNodeIndexMap = new Hashtable(this._tree.Size);
			this._uidToBamlNodeIndexMap = new Hashtable(this._tree.Size / 2);
			this._localizableResources = new BamlLocalizationDictionary();
			for (int i = 0; i < this._tree.Size; i++)
			{
				BamlTreeNode bamlTreeNode = this._tree[i];
				if (!bamlTreeNode.Unidentifiable)
				{
					if (bamlTreeNode.NodeType == BamlNodeType.StartElement)
					{
						BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)bamlTreeNode;
						this._resolver.AddClassAndAssembly(bamlStartElementNode.TypeFullName, bamlStartElementNode.AssemblyName);
					}
					BamlLocalizableResourceKey key = BamlTreeMap.GetKey(bamlTreeNode);
					if (key != null)
					{
						if (bamlTreeNode.NodeType == BamlNodeType.StartElement)
						{
							if (this._uidToBamlNodeIndexMap.ContainsKey(key.Uid))
							{
								this._resolver.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(key, BamlLocalizerError.DuplicateUid));
								bamlTreeNode.Unidentifiable = true;
								if (bamlTreeNode.Children == null)
								{
									goto IL_1AB;
								}
								using (List<BamlTreeNode>.Enumerator enumerator = bamlTreeNode.Children.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										BamlTreeNode bamlTreeNode2 = enumerator.Current;
										if (bamlTreeNode2.NodeType != BamlNodeType.StartElement)
										{
											bamlTreeNode2.Unidentifiable = true;
										}
									}
									goto IL_1AB;
								}
							}
							this._uidToBamlNodeIndexMap.Add(key.Uid, i);
						}
						this._keyToBamlNodeIndexMap.Add(key, i);
						if (this._localizableResources.RootElementKey == null && bamlTreeNode.NodeType == BamlNodeType.StartElement && bamlTreeNode.Parent != null && bamlTreeNode.Parent.NodeType == BamlNodeType.StartDocument)
						{
							this._localizableResources.SetRootElementKey(key);
						}
						BamlLocalizableResource bamlLocalizableResource = this._localizableResourceBuilder.BuildFromNode(key, bamlTreeNode);
						if (bamlLocalizableResource != null)
						{
							this._localizableResources.Add(key, bamlLocalizableResource);
						}
					}
				}
				IL_1AB:;
			}
			this._resolver.ReleaseLocalizabilityCache();
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00136300 File Offset: 0x00135300
		internal static BamlLocalizableResourceKey GetKey(BamlTreeNode node)
		{
			BamlLocalizableResourceKey result = null;
			BamlNodeType nodeType = node.NodeType;
			if (nodeType != BamlNodeType.StartElement)
			{
				if (nodeType != BamlNodeType.Property)
				{
					if (nodeType == BamlNodeType.LiteralContent)
					{
						BamlLiteralContentNode bamlLiteralContentNode = (BamlLiteralContentNode)node;
						BamlStartElementNode bamlStartElementNode = (BamlStartElementNode)node.Parent;
						if (bamlStartElementNode.Uid != null)
						{
							result = new BamlLocalizableResourceKey(bamlStartElementNode.Uid, bamlStartElementNode.TypeFullName, "$LiteralContent", bamlStartElementNode.AssemblyName);
						}
					}
				}
				else
				{
					BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)node;
					BamlStartElementNode bamlStartElementNode2 = (BamlStartElementNode)bamlPropertyNode.Parent;
					if (bamlStartElementNode2.Uid != null)
					{
						string uid;
						if (bamlPropertyNode.Index <= 0)
						{
							uid = bamlStartElementNode2.Uid;
						}
						else
						{
							uid = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}.{1}_{2}", bamlStartElementNode2.Uid, bamlPropertyNode.PropertyName, bamlPropertyNode.Index);
						}
						result = new BamlLocalizableResourceKey(uid, bamlPropertyNode.OwnerTypeFullName, bamlPropertyNode.PropertyName, bamlPropertyNode.AssemblyName);
					}
				}
			}
			else
			{
				BamlStartElementNode bamlStartElementNode3 = (BamlStartElementNode)node;
				if (bamlStartElementNode3.Uid != null)
				{
					result = new BamlLocalizableResourceKey(bamlStartElementNode3.Uid, bamlStartElementNode3.TypeFullName, "$Content", bamlStartElementNode3.AssemblyName);
				}
			}
			return result;
		}

		// Token: 0x040009F2 RID: 2546
		private Hashtable _keyToBamlNodeIndexMap;

		// Token: 0x040009F3 RID: 2547
		private Hashtable _uidToBamlNodeIndexMap;

		// Token: 0x040009F4 RID: 2548
		private LocalizableResourceBuilder _localizableResourceBuilder;

		// Token: 0x040009F5 RID: 2549
		private BamlLocalizationDictionary _localizableResources;

		// Token: 0x040009F6 RID: 2550
		private BamlTree _tree;

		// Token: 0x040009F7 RID: 2551
		private InternalBamlLocalizabilityResolver _resolver;
	}
}
