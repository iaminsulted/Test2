using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x020005F2 RID: 1522
	internal sealed class FixedDSBuilder
	{
		// Token: 0x06004A63 RID: 19043 RVA: 0x00232DAD File Offset: 0x00231DAD
		public FixedDSBuilder(FixedPage fp, StoryFragments sf)
		{
			this._nameHashTable = new Dictionary<string, FixedDSBuilder.NameHashFixedNode>();
			this._fixedPage = fp;
			this._storyFragments = sf;
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x00232DCE File Offset: 0x00231DCE
		public void BuildNameHashTable(string Name, UIElement e, int indexToFixedNodes)
		{
			if (!this._nameHashTable.ContainsKey(Name))
			{
				this._nameHashTable.Add(Name, new FixedDSBuilder.NameHashFixedNode(e, indexToFixedNodes));
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x00232DF1 File Offset: 0x00231DF1
		public StoryFragments StoryFragments
		{
			get
			{
				return this._storyFragments;
			}
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x00232DFC File Offset: 0x00231DFC
		public void ConstructFlowNodes(FixedTextBuilder.FlowModelBuilder flowBuilder, List<FixedNode> fixedNodes)
		{
			this._fixedNodes = fixedNodes;
			this._visitedArray = new BitArray(fixedNodes.Count);
			this._flowBuilder = flowBuilder;
			foreach (StoryFragment storyFragment in this.StoryFragments.StoryFragmentList)
			{
				foreach (BlockElement be in storyFragment.BlockElementList)
				{
					this._CreateFlowNodes(be);
				}
			}
			this._flowBuilder.AddStartNode(FixedElement.ElementType.Paragraph);
			for (int i = 0; i < this._visitedArray.Count; i++)
			{
				if (!this._visitedArray[i])
				{
					this.AddFixedNodeInFlow(i, null);
				}
			}
			this._flowBuilder.AddEndNode();
			this._flowBuilder.AddLeftoverHyperlinks();
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x00232EFC File Offset: 0x00231EFC
		private void AddFixedNodeInFlow(int index, UIElement e)
		{
			if (this._visitedArray[index])
			{
				return;
			}
			FixedNode fixedNode = this._fixedNodes[index];
			if (e == null)
			{
				e = (this._fixedPage.GetElement(fixedNode) as UIElement);
			}
			this._visitedArray[index] = true;
			FixedSOMElement fixedSOMElement = FixedSOMElement.CreateFixedSOMElement(this._fixedPage, e, fixedNode, -1, -1);
			if (fixedSOMElement != null)
			{
				this._flowBuilder.AddElement(fixedSOMElement);
			}
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x00232F68 File Offset: 0x00231F68
		private void _CreateFlowNodes(BlockElement be)
		{
			NamedElement namedElement = be as NamedElement;
			if (namedElement != null)
			{
				this.ConstructSomElement(namedElement);
				return;
			}
			SemanticBasicElement semanticBasicElement = be as SemanticBasicElement;
			if (semanticBasicElement != null)
			{
				this._flowBuilder.AddStartNode(be.ElementType);
				XmlLanguage value = (XmlLanguage)this._fixedPage.GetValue(FrameworkElement.LanguageProperty);
				this._flowBuilder.FixedElement.SetValue(FixedElement.LanguageProperty, value);
				this.SpecialProcessing(semanticBasicElement);
				foreach (BlockElement be2 in semanticBasicElement.BlockElementList)
				{
					this._CreateFlowNodes(be2);
				}
				this._flowBuilder.AddEndNode();
			}
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x0023302C File Offset: 0x0023202C
		private void AddChildofFixedNodeinFlow(int[] childIndex, NamedElement ne)
		{
			FixedNode item = FixedNode.Create(this._fixedNodes[0].Page, childIndex);
			int num = this._fixedNodes.BinarySearch(item);
			if (num >= 0)
			{
				int num2 = num - 1;
				while (num2 >= 0 && this._fixedNodes[num2].ComparetoIndex(childIndex) == 0)
				{
					num2--;
				}
				int num3 = num2 + 1;
				while (num3 < this._fixedNodes.Count && this._fixedNodes[num3].ComparetoIndex(childIndex) == 0)
				{
					this.AddFixedNodeInFlow(num3, null);
					num3++;
				}
			}
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x002330C8 File Offset: 0x002320C8
		private void SpecialProcessing(SemanticBasicElement sbe)
		{
			ListItemStructure listItemStructure = sbe as ListItemStructure;
			FixedDSBuilder.NameHashFixedNode nameHashFixedNode;
			if (listItemStructure != null && listItemStructure.Marker != null && this._nameHashTable.TryGetValue(listItemStructure.Marker, out nameHashFixedNode))
			{
				this._visitedArray[nameHashFixedNode.index] = true;
			}
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x00233110 File Offset: 0x00232110
		private void ConstructSomElement(NamedElement ne)
		{
			FixedDSBuilder.NameHashFixedNode nameHashFixedNode;
			if (this._nameHashTable.TryGetValue(ne.NameReference, out nameHashFixedNode))
			{
				if (nameHashFixedNode.uiElement is Glyphs || nameHashFixedNode.uiElement is Path || nameHashFixedNode.uiElement is Image)
				{
					this.AddFixedNodeInFlow(nameHashFixedNode.index, nameHashFixedNode.uiElement);
					return;
				}
				if (nameHashFixedNode.uiElement is Canvas)
				{
					int[] childIndex = this._fixedPage._CreateChildIndex(nameHashFixedNode.uiElement);
					this.AddChildofFixedNodeinFlow(childIndex, ne);
				}
			}
		}

		// Token: 0x040026F8 RID: 9976
		private StoryFragments _storyFragments;

		// Token: 0x040026F9 RID: 9977
		private FixedPage _fixedPage;

		// Token: 0x040026FA RID: 9978
		private List<FixedNode> _fixedNodes;

		// Token: 0x040026FB RID: 9979
		private BitArray _visitedArray;

		// Token: 0x040026FC RID: 9980
		private Dictionary<string, FixedDSBuilder.NameHashFixedNode> _nameHashTable;

		// Token: 0x040026FD RID: 9981
		private FixedTextBuilder.FlowModelBuilder _flowBuilder;

		// Token: 0x02000B33 RID: 2867
		private class NameHashFixedNode
		{
			// Token: 0x06008CA0 RID: 36000 RVA: 0x0033D3AC File Offset: 0x0033C3AC
			internal NameHashFixedNode(UIElement e, int i)
			{
				this.uiElement = e;
				this.index = i;
			}

			// Token: 0x04004802 RID: 18434
			internal UIElement uiElement;

			// Token: 0x04004803 RID: 18435
			internal int index;
		}
	}
}
