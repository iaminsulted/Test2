using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;

namespace MS.Internal.Globalization
{
	// Token: 0x02000199 RID: 409
	internal sealed class BamlResourceSerializer
	{
		// Token: 0x06000D8C RID: 3468 RVA: 0x00135F9A File Offset: 0x00134F9A
		internal static void Serialize(BamlLocalizer localizer, BamlTree tree, Stream output)
		{
			new BamlResourceSerializer().SerializeImp(localizer, tree, output);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private BamlResourceSerializer()
		{
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00135FAC File Offset: 0x00134FAC
		private void SerializeImp(BamlLocalizer localizer, BamlTree tree, Stream output)
		{
			this._writer = new BamlWriter(output);
			this._bamlTreeStack = new Stack<BamlTreeNode>();
			this._bamlTreeStack.Push(tree.Root);
			while (this._bamlTreeStack.Count > 0)
			{
				BamlTreeNode bamlTreeNode = this._bamlTreeStack.Pop();
				if (!bamlTreeNode.Visited)
				{
					bamlTreeNode.Visited = true;
					bamlTreeNode.Serialize(this._writer);
					this.PushChildrenToStack(bamlTreeNode.Children);
				}
				else
				{
					BamlStartElementNode bamlStartElementNode = bamlTreeNode as BamlStartElementNode;
					if (bamlStartElementNode != null)
					{
						localizer.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(bamlStartElementNode), BamlLocalizerError.DuplicateElement));
					}
				}
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00136044 File Offset: 0x00135044
		private void PushChildrenToStack(List<BamlTreeNode> children)
		{
			if (children == null)
			{
				return;
			}
			for (int i = children.Count - 1; i >= 0; i--)
			{
				this._bamlTreeStack.Push(children[i]);
			}
		}

		// Token: 0x040009F0 RID: 2544
		private BamlWriter _writer;

		// Token: 0x040009F1 RID: 2545
		private Stack<BamlTreeNode> _bamlTreeStack;
	}
}
