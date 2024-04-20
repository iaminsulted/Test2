using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Utility;

namespace System.Windows.Documents
{
	// Token: 0x02000616 RID: 1558
	internal sealed class FixedTextBuilder
	{
		// Token: 0x06004BCA RID: 19402 RVA: 0x00239A68 File Offset: 0x00238A68
		internal static bool AlwaysAdjacent(CultureInfo ci)
		{
			foreach (CultureInfo obj in FixedTextBuilder.AdjacentLanguage)
			{
				if (ci.Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x00239A9C File Offset: 0x00238A9C
		internal static bool IsHyphen(char target)
		{
			char[] hyphenSet = FixedTextBuilder.HyphenSet;
			for (int i = 0; i < hyphenSet.Length; i++)
			{
				if (hyphenSet[i] == target)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x00239AC6 File Offset: 0x00238AC6
		internal static bool IsSpace(char target)
		{
			return target == ' ';
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x00239ACD File Offset: 0x00238ACD
		internal FixedTextBuilder(FixedTextContainer container)
		{
			this._container = container;
			this._Init();
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00239AE4 File Offset: 0x00238AE4
		internal void AddVirtualPage()
		{
			FixedPageStructure fixedPageStructure = new FixedPageStructure(this._pageStructures.Count);
			this._pageStructures.Add(fixedPageStructure);
			this._fixedFlowMap.FlowOrderInsertBefore(this._fixedFlowMap.FlowEndEdge, fixedPageStructure.FlowStart);
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x00239B2C File Offset: 0x00238B2C
		internal bool EnsureTextOMForPage(int pageIndex)
		{
			FixedPageStructure fixedPageStructure = this._pageStructures[pageIndex];
			if (!fixedPageStructure.Loaded)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXEnsureOMBegin);
				try
				{
					FixedPage fixedPage = this._container.FixedDocument.SyncGetPage(pageIndex, false);
					if (fixedPage == null)
					{
						return false;
					}
					Size size = this._container.FixedDocument.ComputePageSize(fixedPage);
					fixedPage.Measure(size);
					fixedPage.Arrange(new Rect(new Point(0.0, 0.0), size));
					bool flag = true;
					StoryFragments pageStructure = fixedPage.GetPageStructure();
					if (pageStructure != null)
					{
						flag = false;
						FixedDSBuilder fixedDSBuilder = new FixedDSBuilder(fixedPage, pageStructure);
						fixedPageStructure.FixedDSBuilder = fixedDSBuilder;
					}
					if (flag)
					{
						FixedSOMPageConstructor fixedSOMPageConstructor = new FixedSOMPageConstructor(fixedPage, pageIndex);
						fixedPageStructure.PageConstructor = fixedSOMPageConstructor;
						fixedPageStructure.FixedSOMPage = fixedSOMPageConstructor.FixedSOMPage;
					}
					this._CreateFixedMappingAndElementForPage(fixedPageStructure, fixedPage, flag);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXEnsureOMEnd);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x00239C28 File Offset: 0x00238C28
		internal FixedPage GetFixedPage(FixedNode node)
		{
			return this._container.FixedDocument.SyncGetPageWithCheck(node.Page);
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x00239C44 File Offset: 0x00238C44
		internal Glyphs GetGlyphsElement(FixedNode node)
		{
			FixedPage fixedPage = this.GetFixedPage(node);
			if (fixedPage != null)
			{
				return fixedPage.GetGlyphsElement(node);
			}
			return null;
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x00239C68 File Offset: 0x00238C68
		internal FixedNode[] GetNextLine(FixedNode currentNode, bool forward, ref int count)
		{
			if (this._IsBoundaryPage(currentNode.Page))
			{
				return null;
			}
			this.EnsureTextOMForPage(currentNode.Page);
			FixedPageStructure fixedPageStructure = this._pageStructures[currentNode.Page];
			if (this._IsStartVisual(currentNode[1]))
			{
				FixedNode[] firstLine = fixedPageStructure.FirstLine;
				if (firstLine == null)
				{
					return null;
				}
				currentNode = firstLine[0];
				count--;
			}
			else if (this._IsEndVisual(currentNode[1]))
			{
				FixedNode[] lastLine = fixedPageStructure.LastLine;
				if (lastLine == null)
				{
					return null;
				}
				currentNode = lastLine[0];
				count--;
			}
			FixedSOMTextRun fixedSOMTextRun = this._fixedFlowMap.MappingGetFixedSOMElement(currentNode, 0) as FixedSOMTextRun;
			if (fixedSOMTextRun == null)
			{
				return null;
			}
			int lineIndex = fixedSOMTextRun.LineIndex;
			return fixedPageStructure.GetNextLine(lineIndex, forward, ref count);
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x00239D2A File Offset: 0x00238D2A
		internal FixedNode[] GetLine(int pageIndex, Point pt)
		{
			this.EnsureTextOMForPage(pageIndex);
			return this._pageStructures[pageIndex].FindSnapToLine(pt);
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x00239D46 File Offset: 0x00238D46
		internal FixedNode[] GetFirstLine(int pageIndex)
		{
			this.EnsureTextOMForPage(pageIndex);
			return this._pageStructures[pageIndex].FirstLine;
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x00239D64 File Offset: 0x00238D64
		internal FlowPosition CreateFlowPosition(FixedPosition fixedPosition)
		{
			this.EnsureTextOMForPage(fixedPosition.Page);
			FixedSOMElement fixedSOMElement = this._fixedFlowMap.MappingGetFixedSOMElement(fixedPosition.Node, fixedPosition.Offset);
			if (fixedSOMElement != null)
			{
				FlowNode flowNode = fixedSOMElement.FlowNode;
				int num = fixedPosition.Offset;
				FixedSOMTextRun fixedSOMTextRun = fixedSOMElement as FixedSOMTextRun;
				if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
				{
					num = fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex - num;
				}
				int offset = fixedSOMElement.OffsetInFlowNode + num - fixedSOMElement.StartIndex;
				return new FlowPosition(this._container, flowNode, offset);
			}
			return null;
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x00239DF0 File Offset: 0x00238DF0
		internal FlowPosition GetPageStartFlowPosition(int pageIndex)
		{
			this.EnsureTextOMForPage(pageIndex);
			FlowNode flowStart = this._pageStructures[pageIndex].FlowStart;
			return new FlowPosition(this._container, flowStart, 0);
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00239E24 File Offset: 0x00238E24
		internal FlowPosition GetPageEndFlowPosition(int pageIndex)
		{
			this.EnsureTextOMForPage(pageIndex);
			FlowNode flowEnd = this._pageStructures[pageIndex].FlowEnd;
			return new FlowPosition(this._container, flowEnd, 1);
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00239E58 File Offset: 0x00238E58
		internal bool GetFixedPosition(FlowPosition position, LogicalDirection textdir, out FixedPosition fixedp)
		{
			fixedp = new FixedPosition(this.FixedFlowMap.FixedStartEdge, 0);
			FlowNode flowNode;
			int num;
			position.GetFlowNode(textdir, out flowNode, out num);
			FixedSOMElement[] fixedSOMElements = flowNode.FixedSOMElements;
			if (fixedSOMElements == null)
			{
				return false;
			}
			int num2 = 0;
			int i = fixedSOMElements.Length - 1;
			while (i > num2)
			{
				int num3 = i + num2 + 1 >> 1;
				if (fixedSOMElements[num3].OffsetInFlowNode > num)
				{
					i = num3 - 1;
				}
				else
				{
					num2 = num3;
				}
			}
			FixedSOMElement fixedSOMElement = fixedSOMElements[num2];
			FixedSOMTextRun fixedSOMTextRun;
			if (num2 > 0 && textdir == LogicalDirection.Backward && fixedSOMElement.OffsetInFlowNode == num)
			{
				FixedSOMElement fixedSOMElement2 = fixedSOMElements[num2 - 1];
				int num4 = num - fixedSOMElement2.OffsetInFlowNode + fixedSOMElement2.StartIndex;
				if (num4 == fixedSOMElement2.EndIndex)
				{
					fixedSOMTextRun = (fixedSOMElement2 as FixedSOMTextRun);
					if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
					{
						num4 = fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex - num4;
					}
					fixedp = new FixedPosition(fixedSOMElement2.FixedNode, num4);
					return true;
				}
			}
			fixedSOMTextRun = (fixedSOMElement as FixedSOMTextRun);
			int num5 = num - fixedSOMElement.OffsetInFlowNode + fixedSOMElement.StartIndex;
			if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
			{
				num5 = fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex - num5;
			}
			fixedp = new FixedPosition(fixedSOMElement.FixedNode, num5);
			return true;
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x00239F94 File Offset: 0x00238F94
		internal bool GetFixedNodesForFlowRange(FlowPosition pStart, FlowPosition pEnd, out FixedSOMElement[] somElements, out int firstElementStart, out int lastElementEnd)
		{
			somElements = null;
			firstElementStart = 0;
			lastElementEnd = 0;
			int num = 0;
			int num2 = -1;
			FlowNode[] array;
			int num3;
			int num4;
			pStart.GetFlowNodes(pEnd, out array, out num3, out num4);
			if (array.Length == 0)
			{
				return false;
			}
			ArrayList arrayList = new ArrayList();
			FlowNode flowNode = array[0];
			FlowNode flowNode2 = array[array.Length - 1];
			foreach (FlowNode flowNode3 in array)
			{
				int num5 = 0;
				int num6 = int.MaxValue;
				if (flowNode3 == flowNode)
				{
					num5 = num3;
				}
				if (flowNode3 == flowNode2)
				{
					num6 = num4;
				}
				if (flowNode3.Type == FlowNodeType.Object)
				{
					FixedSOMElement[] fixedSOMElements = flowNode3.FixedSOMElements;
					arrayList.Add(fixedSOMElements[0]);
				}
				if (flowNode3.Type == FlowNodeType.Run)
				{
					foreach (FixedSOMElement fixedSOMElement in flowNode3.FixedSOMElements)
					{
						int offsetInFlowNode = fixedSOMElement.OffsetInFlowNode;
						if (offsetInFlowNode >= num6)
						{
							break;
						}
						int num7 = offsetInFlowNode + fixedSOMElement.EndIndex - fixedSOMElement.StartIndex;
						if (num7 > num5)
						{
							arrayList.Add(fixedSOMElement);
							if (num5 >= offsetInFlowNode && flowNode3 == flowNode)
							{
								num = fixedSOMElement.StartIndex + num5 - offsetInFlowNode;
							}
							if (num6 <= num7 && flowNode3 == flowNode2)
							{
								num2 = fixedSOMElement.StartIndex + num6 - offsetInFlowNode;
								break;
							}
							if (num6 == num7 + 1)
							{
								num2 = fixedSOMElement.EndIndex;
							}
						}
					}
				}
			}
			somElements = (FixedSOMElement[])arrayList.ToArray(typeof(FixedSOMElement));
			if (somElements.Length == 0)
			{
				return false;
			}
			if (flowNode.Type == FlowNodeType.Object)
			{
				firstElementStart = num3;
			}
			else
			{
				firstElementStart = num;
			}
			if (flowNode2.Type == FlowNodeType.Object)
			{
				lastElementEnd = num4;
			}
			else
			{
				lastElementEnd = num2;
			}
			return true;
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x0023A130 File Offset: 0x00239130
		internal string GetFlowText(FlowNode flowNode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FixedSOMTextRun fixedSOMTextRun in flowNode.FixedSOMElements)
			{
				stringBuilder.Append(fixedSOMTextRun.Text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0023A174 File Offset: 0x00239174
		internal static bool MostlyRTL(string s)
		{
			int num = 0;
			int num2 = 0;
			foreach (char c in s)
			{
				if (FixedTextBuilder._IsRTL(c))
				{
					num++;
				}
				else if (c != ' ')
				{
					num2++;
				}
			}
			return num > 0 && (num2 == 0 || num / num2 >= 2);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x0023A1D0 File Offset: 0x002391D0
		internal static bool IsSameLine(double verticalDistance, double fontSize1, double fontSize2)
		{
			double num = (fontSize1 < fontSize2) ? fontSize1 : fontSize2;
			return ((verticalDistance > 0.0) ? (fontSize1 - verticalDistance) : (fontSize2 + verticalDistance)) / num > 0.5;
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0023A208 File Offset: 0x00239208
		internal static bool IsNonContiguous(CultureInfo ciPrev, CultureInfo ciCurrent, bool isSidewaysPrev, bool isSidewaysCurrent, string strPrev, string strCurrent, FixedTextBuilder.GlyphComparison comparison)
		{
			if (ciPrev != ciCurrent)
			{
				return true;
			}
			if (FixedTextBuilder.AlwaysAdjacent(ciPrev))
			{
				return false;
			}
			if (isSidewaysPrev != isSidewaysCurrent)
			{
				return true;
			}
			if (strPrev.Length == 0 || strCurrent.Length == 0)
			{
				return false;
			}
			if (!isSidewaysPrev)
			{
				int length = strPrev.Length;
				char target = strPrev[length - 1];
				if (FixedTextBuilder.IsSpace(target))
				{
					return false;
				}
				if (comparison != FixedTextBuilder.GlyphComparison.DifferentLine && comparison != FixedTextBuilder.GlyphComparison.Unknown)
				{
					return comparison != FixedTextBuilder.GlyphComparison.Adjacent;
				}
				if (!FixedTextBuilder.IsHyphen(target))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0023A27C File Offset: 0x0023927C
		internal FixedFlowMap FixedFlowMap
		{
			get
			{
				return this._fixedFlowMap;
			}
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x0023A284 File Offset: 0x00239284
		private void _Init()
		{
			this._nextScopeId = 0;
			this._fixedFlowMap = new FixedFlowMap();
			this._pageStructures = new List<FixedPageStructure>();
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0023A2A4 File Offset: 0x002392A4
		private FixedNode _NewFixedNode(int pageIndex, int nestingLevel, int level1Index, int[] pathPrefix, int childIndex)
		{
			if (nestingLevel == 1)
			{
				return FixedNode.Create(pageIndex, nestingLevel, childIndex, -1, null);
			}
			if (nestingLevel == 2)
			{
				return FixedNode.Create(pageIndex, nestingLevel, level1Index, childIndex, null);
			}
			int[] array = new int[pathPrefix.Length + 1];
			pathPrefix.CopyTo(array, 0);
			array[array.Length - 1] = childIndex;
			return FixedNode.Create(pageIndex, nestingLevel, -1, -1, array);
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0023A2FC File Offset: 0x002392FC
		private bool _IsImage(object o)
		{
			Path path = o as Path;
			if (path != null)
			{
				return path.Fill is ImageBrush && path.Data != null;
			}
			return o is Image;
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x0023A338 File Offset: 0x00239338
		private bool _IsNonContiguous(FixedSOMTextRun prevRun, FixedSOMTextRun currentRun, FixedTextBuilder.GlyphComparison comparison)
		{
			if (prevRun.FixedNode == currentRun.FixedNode)
			{
				return currentRun.StartIndex != prevRun.EndIndex;
			}
			return FixedTextBuilder.IsNonContiguous(prevRun.CultureInfo, currentRun.CultureInfo, prevRun.IsSideways, currentRun.IsSideways, prevRun.Text, currentRun.Text, comparison);
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x0023A394 File Offset: 0x00239394
		private FixedTextBuilder.GlyphComparison _CompareGlyphs(Glyphs glyph1, Glyphs glyph2)
		{
			FixedTextBuilder.GlyphComparison result = FixedTextBuilder.GlyphComparison.DifferentLine;
			if (glyph1 == glyph2)
			{
				result = FixedTextBuilder.GlyphComparison.SameLine;
			}
			else if (glyph1 != null && glyph2 != null)
			{
				GlyphRun glyphRun = glyph1.ToGlyphRun();
				GlyphRun glyphRun2 = glyph2.ToGlyphRun();
				if (glyphRun != null && glyphRun2 != null)
				{
					Rect rect = glyphRun.ComputeAlignmentBox();
					rect.Offset(glyph1.OriginX, glyph1.OriginY);
					Rect rect2 = glyphRun2.ComputeAlignmentBox();
					rect2.Offset(glyph2.OriginX, glyph2.OriginY);
					bool flag = (glyph1.BidiLevel & 1) == 0;
					bool flag2 = (glyph2.BidiLevel & 1) == 0;
					GeneralTransform generalTransform = glyph2.TransformToVisual(glyph1);
					Point point = flag ? rect.TopRight : rect.TopLeft;
					Point inPoint = flag2 ? rect2.TopLeft : rect2.TopRight;
					if (generalTransform != null)
					{
						generalTransform.TryTransform(inPoint, out inPoint);
					}
					if (FixedTextBuilder.IsSameLine(inPoint.Y - point.Y, rect.Height, rect2.Height))
					{
						result = FixedTextBuilder.GlyphComparison.SameLine;
						if (flag == flag2)
						{
							double num = Math.Abs(inPoint.X - point.X);
							double num2 = Math.Max(rect.Height, rect2.Height);
							if (num / num2 < 0.05)
							{
								result = FixedTextBuilder.GlyphComparison.Adjacent;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0023A4D0 File Offset: 0x002394D0
		private void _CreateFixedMappingAndElementForPage(FixedPageStructure pageStructure, FixedPage page, bool constructSOM)
		{
			List<FixedNode> list = new List<FixedNode>();
			this._GetFixedNodes(pageStructure, page.Children, 1, -1, null, constructSOM, list, Matrix.Identity);
			FixedTextBuilder.FlowModelBuilder flowModelBuilder = new FixedTextBuilder.FlowModelBuilder(this, pageStructure, page);
			flowModelBuilder.FindHyperlinkPaths(page);
			if (constructSOM)
			{
				pageStructure.FixedSOMPage.MarkupOrder = list;
				pageStructure.ConstructFixedSOMPage(list);
				this._CreateFlowNodes(pageStructure.FixedSOMPage, flowModelBuilder);
				pageStructure.PageConstructor = null;
			}
			else
			{
				pageStructure.FixedDSBuilder.ConstructFlowNodes(flowModelBuilder, list);
			}
			flowModelBuilder.FinishMapping();
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0023A54C File Offset: 0x0023954C
		private void _GetFixedNodes(FixedPageStructure pageStructure, IEnumerable oneLevel, int nestingLevel, int level1Index, int[] pathPrefix, bool constructLines, List<FixedNode> fixedNodes, Matrix transform)
		{
			int pageIndex = pageStructure.PageIndex;
			this._NewScopeId();
			int num = 0;
			IEnumerator enumerator = oneLevel.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!constructLines)
				{
					IFrameworkInputElement frameworkInputElement = enumerator.Current as IFrameworkInputElement;
					if (frameworkInputElement != null && frameworkInputElement.Name != null && frameworkInputElement.Name.Length != 0)
					{
						pageStructure.FixedDSBuilder.BuildNameHashTable(frameworkInputElement.Name, enumerator.Current as UIElement, fixedNodes.Count);
					}
				}
				if (this._IsImage(enumerator.Current) || (enumerator.Current is Glyphs && (enumerator.Current as Glyphs).MeasurementGlyphRun != null))
				{
					fixedNodes.Add(this._NewFixedNode(pageIndex, nestingLevel, level1Index, pathPrefix, num));
				}
				else if (constructLines && enumerator.Current is Path)
				{
					pageStructure.PageConstructor.ProcessPath(enumerator.Current as Path, transform);
				}
				else if (enumerator.Current is Canvas)
				{
					Transform transform2 = Transform.Identity;
					Canvas canvas = enumerator.Current as Canvas;
					IEnumerable children = canvas.Children;
					transform2 = canvas.RenderTransform;
					if (transform2 == null)
					{
						transform2 = Transform.Identity;
					}
					if (children != null)
					{
						int[] array = null;
						if (nestingLevel >= 2)
						{
							if (nestingLevel == 2)
							{
								array = new int[2];
								array[0] = level1Index;
							}
							else
							{
								array = new int[pathPrefix.Length + 1];
								pathPrefix.CopyTo(array, 0);
							}
							array[array.Length - 1] = num;
						}
						this._GetFixedNodes(pageStructure, children, nestingLevel + 1, (nestingLevel == 1) ? num : -1, array, constructLines, fixedNodes, transform2.Value * transform);
					}
				}
				num++;
			}
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0023A6E8 File Offset: 0x002396E8
		private void _CreateFlowNodes(FixedSOMPage somPage, FixedTextBuilder.FlowModelBuilder flowBuilder)
		{
			flowBuilder.AddStartNode(FixedElement.ElementType.Section);
			somPage.SetRTFProperties(flowBuilder.FixedElement);
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in somPage.SemanticBoxes)
			{
				FixedSOMContainer node = (FixedSOMContainer)fixedSOMSemanticBox;
				this._CreateFlowNodes(node, flowBuilder);
			}
			flowBuilder.AddLeftoverHyperlinks();
			flowBuilder.AddEndNode();
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x0023A764 File Offset: 0x00239764
		private void _CreateFlowNodes(FixedSOMContainer node, FixedTextBuilder.FlowModelBuilder flowBuilder)
		{
			FixedElement.ElementType[] elementTypes = node.ElementTypes;
			foreach (FixedElement.ElementType type in elementTypes)
			{
				flowBuilder.AddStartNode(type);
				node.SetRTFProperties(flowBuilder.FixedElement);
			}
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in node.SemanticBoxes)
			{
				if (fixedSOMSemanticBox is FixedSOMElement)
				{
					flowBuilder.AddElement((FixedSOMElement)fixedSOMSemanticBox);
				}
				else if (fixedSOMSemanticBox is FixedSOMContainer)
				{
					this._CreateFlowNodes((FixedSOMContainer)fixedSOMSemanticBox, flowBuilder);
				}
			}
			foreach (FixedElement.ElementType elementType in elementTypes)
			{
				flowBuilder.AddEndNode();
			}
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x0023A82C File Offset: 0x0023982C
		private bool _IsStartVisual(int visualIndex)
		{
			return visualIndex == int.MinValue;
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x0023A836 File Offset: 0x00239836
		private bool _IsEndVisual(int visualIndex)
		{
			return visualIndex == int.MaxValue;
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x0023A840 File Offset: 0x00239840
		private bool _IsBoundaryPage(int pageIndex)
		{
			return pageIndex == int.MinValue || pageIndex == int.MaxValue;
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x0023A854 File Offset: 0x00239854
		private int _NewScopeId()
		{
			int nextScopeId = this._nextScopeId;
			this._nextScopeId = nextScopeId + 1;
			return nextScopeId;
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x0023A874 File Offset: 0x00239874
		private static bool _IsRTL(char c)
		{
			return (c >= 'א' && c <= '؋') || c == '؍' || (c >= '؛' && c <= 'ي') || (c >= '٭' && c <= 'ە' && c != 'ٰ') || c == '۝' || c == 'ۥ' || c == 'ۦ' || c == 'ۮ' || c == 'ۯ' || (c >= 'ۺ' && c <= '܍') || c == 'ܐ' || (c >= 'ܒ' && c <= 'ܯ') || (c >= 'ݍ' && c <= 'ޥ') || c == 'ޱ' || c == 'יִ' || (c >= 'ײַ' && c <= 'ﴽ' && c != '﬩') || (c >= 'ﵐ' && c <= '﷼') || (c >= 'ﹰ' && c <= 'ﻼ');
		}

		// Token: 0x040027B0 RID: 10160
		internal const char FLOWORDER_SEPARATOR = ' ';

		// Token: 0x040027B1 RID: 10161
		internal static CultureInfo[] AdjacentLanguage = new CultureInfo[]
		{
			new CultureInfo("zh-HANS"),
			new CultureInfo("zh-HANT"),
			new CultureInfo("zh-HK"),
			new CultureInfo("zh-MO"),
			new CultureInfo("zh-CN"),
			new CultureInfo("zh-SG"),
			new CultureInfo("zh-TW"),
			new CultureInfo("ja-JP"),
			new CultureInfo("ko-KR"),
			new CultureInfo("th-TH")
		};

		// Token: 0x040027B2 RID: 10162
		internal static char[] HyphenSet = new char[]
		{
			'-',
			'‐',
			'‑',
			'‒',
			'–',
			'−',
			'­'
		};

		// Token: 0x040027B3 RID: 10163
		private readonly FixedTextContainer _container;

		// Token: 0x040027B4 RID: 10164
		private List<FixedPageStructure> _pageStructures;

		// Token: 0x040027B5 RID: 10165
		private int _nextScopeId;

		// Token: 0x040027B6 RID: 10166
		private FixedFlowMap _fixedFlowMap;

		// Token: 0x040027B7 RID: 10167
		private static bool[] _cTable = new bool[]
		{
			true,
			false,
			true,
			true,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true
		};

		// Token: 0x02000B38 RID: 2872
		internal sealed class FlowModelBuilder
		{
			// Token: 0x06008CAC RID: 36012 RVA: 0x0033D63C File Offset: 0x0033C63C
			public FlowModelBuilder(FixedTextBuilder builder, FixedPageStructure pageStructure, FixedPage page)
			{
				this._builder = builder;
				this._container = builder._container;
				this._pageIndex = pageStructure.PageIndex;
				this._textRuns = new List<FixedSOMTextRun>();
				this._flowNodes = new List<FlowNode>();
				this._fixedNodes = new List<FixedNode>();
				this._nodesInLine = new List<FixedNode>();
				this._lineResults = new List<FixedLineResult>();
				this._endNodes = new Stack();
				this._fixedElements = new Stack();
				this._mapping = builder._fixedFlowMap;
				this._pageStructure = pageStructure;
				this._currentFixedElement = this._container.ContainerElement;
				this._lineLayoutBox = Rect.Empty;
				this._logicalHyperlinkContainer = new FixedTextBuilder.FlowModelBuilder.LogicalHyperlinkContainer();
				this._fixedPage = page;
			}

			// Token: 0x06008CAD RID: 36013 RVA: 0x0033D6FC File Offset: 0x0033C6FC
			public void FindHyperlinkPaths(FrameworkElement elem)
			{
				foreach (object obj in LogicalTreeHelper.GetChildren(elem))
				{
					UIElement uielement = (UIElement)obj;
					Canvas canvas = uielement as Canvas;
					if (canvas != null)
					{
						this.FindHyperlinkPaths(canvas);
					}
					if (uielement is Path && !(((Path)uielement).Fill is ImageBrush))
					{
						Uri navigateUri = FixedPage.GetNavigateUri(uielement);
						if (navigateUri != null && ((Path)uielement).Data != null)
						{
							Transform transform = uielement.TransformToAncestor(this._fixedPage) as Transform;
							Geometry geometry = ((Path)uielement).Data;
							if (transform != null && !transform.Value.IsIdentity)
							{
								geometry = PathGeometry.CreateFromGeometry(geometry);
								geometry.Transform = transform;
							}
							this._logicalHyperlinkContainer.AddLogicalHyperlink(navigateUri, geometry, uielement);
						}
					}
				}
			}

			// Token: 0x06008CAE RID: 36014 RVA: 0x0033D7FC File Offset: 0x0033C7FC
			public void AddLeftoverHyperlinks()
			{
				foreach (FixedTextBuilder.FlowModelBuilder.LogicalHyperlink logicalHyperlink in ((IEnumerable<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>)this._logicalHyperlinkContainer))
				{
					if (!logicalHyperlink.Used)
					{
						this._AddStartNode(FixedElement.ElementType.Paragraph);
						this._AddStartNode(FixedElement.ElementType.Hyperlink);
						this._currentFixedElement.SetValue(Hyperlink.NavigateUriProperty, logicalHyperlink.Uri);
						this._currentFixedElement.SetValue(FixedElement.HelpTextProperty, (string)logicalHyperlink.UIElement.GetValue(AutomationProperties.HelpTextProperty));
						this._currentFixedElement.SetValue(FixedElement.NameProperty, (string)logicalHyperlink.UIElement.GetValue(AutomationProperties.NameProperty));
						this._AddEndNode();
						this._AddEndNode();
					}
				}
			}

			// Token: 0x06008CAF RID: 36015 RVA: 0x0033D8CC File Offset: 0x0033C8CC
			public void AddStartNode(FixedElement.ElementType type)
			{
				this._FinishTextRun(true);
				this._FinishHyperlink();
				this._AddStartNode(type);
			}

			// Token: 0x06008CB0 RID: 36016 RVA: 0x0033D8E2 File Offset: 0x0033C8E2
			public void AddEndNode()
			{
				this._FinishTextRun(false);
				this._FinishHyperlink();
				this._AddEndNode();
			}

			// Token: 0x06008CB1 RID: 36017 RVA: 0x0033D8F8 File Offset: 0x0033C8F8
			public void AddElement(FixedSOMElement element)
			{
				FixedPage fixedPage = this._builder.GetFixedPage(element.FixedNode);
				UIElement shadowHyperlink;
				Uri uri = this._logicalHyperlinkContainer.GetUri(element, fixedPage, out shadowHyperlink);
				if (element is FixedSOMTextRun)
				{
					FixedSOMTextRun fixedSOMTextRun = element as FixedSOMTextRun;
					if (this._currentRun == null || !fixedSOMTextRun.HasSameRichProperties(this._currentRun) || uri != this._currentNavUri || (uri != null && uri.ToString() != this._currentNavUri.ToString()))
					{
						if (this._currentRun != null)
						{
							FixedSOMFixedBlock fixedBlock = fixedSOMTextRun.FixedBlock;
							FixedSOMTextRun fixedSOMTextRun2 = this._textRuns[this._textRuns.Count - 1];
							Glyphs glyphsElement = this._builder.GetGlyphsElement(fixedSOMTextRun2.FixedNode);
							Glyphs glyphsElement2 = this._builder.GetGlyphsElement(fixedSOMTextRun.FixedNode);
							FixedTextBuilder.GlyphComparison comparison = this._builder._CompareGlyphs(glyphsElement, glyphsElement2);
							bool addSpace = false;
							if (this._builder._IsNonContiguous(fixedSOMTextRun2, fixedSOMTextRun, comparison))
							{
								addSpace = true;
							}
							this._FinishTextRun(addSpace);
						}
						this._SetHyperlink(uri, fixedSOMTextRun.FixedNode, shadowHyperlink);
						this._AddStartNode(FixedElement.ElementType.Run);
						fixedSOMTextRun.SetRTFProperties(this._currentFixedElement);
						this._currentRun = fixedSOMTextRun;
					}
					this._textRuns.Add((FixedSOMTextRun)element);
					if (this._fixedNodes.Count == 0 || this._fixedNodes[this._fixedNodes.Count - 1] != element.FixedNode)
					{
						this._fixedNodes.Add(element.FixedNode);
						return;
					}
				}
				else if (element is FixedSOMImage)
				{
					FixedSOMImage fixedSOMImage = (FixedSOMImage)element;
					this._FinishTextRun(true);
					this._SetHyperlink(uri, fixedSOMImage.FixedNode, shadowHyperlink);
					this._AddStartNode(FixedElement.ElementType.InlineUIContainer);
					FlowNode flowNode = new FlowNode(this._NewScopeId(), FlowNodeType.Object, null);
					this._container.OnNewFlowElement(this._currentFixedElement, FixedElement.ElementType.Object, new FlowPosition(this._container, flowNode, 0), new FlowPosition(this._container, flowNode, 1), fixedSOMImage.Source, this._pageIndex);
					this._flowNodes.Add(flowNode);
					element.FlowNode = flowNode;
					flowNode.FixedSOMElements = new FixedSOMElement[]
					{
						element
					};
					this._mapping.AddFixedElement(element);
					this._fixedNodes.Add(element.FixedNode);
					FixedElement fixedElement = (FixedElement)flowNode.Cookie;
					fixedElement.SetValue(FixedElement.NameProperty, fixedSOMImage.Name);
					fixedElement.SetValue(FixedElement.HelpTextProperty, fixedSOMImage.HelpText);
					this._AddEndNode();
				}
			}

			// Token: 0x06008CB2 RID: 36018 RVA: 0x0033DB7C File Offset: 0x0033CB7C
			public void FinishMapping()
			{
				this._FinishLine();
				this._mapping.MappingReplace(this._pageStructure.FlowStart, this._flowNodes);
				this._pageStructure.SetFlowBoundary(this._flowNodes[0], this._flowNodes[this._flowNodes.Count - 1]);
				this._pageStructure.SetupLineResults(this._lineResults.ToArray());
			}

			// Token: 0x06008CB3 RID: 36019 RVA: 0x0033DBF0 File Offset: 0x0033CBF0
			private void _AddStartNode(FixedElement.ElementType type)
			{
				FlowNode flowNode = new FlowNode(this._NewScopeId(), FlowNodeType.Start, this._pageIndex);
				FlowNode flowNode2 = new FlowNode(this._NewScopeId(), FlowNodeType.End, this._pageIndex);
				this._container.OnNewFlowElement(this._currentFixedElement, type, new FlowPosition(this._container, flowNode, 1), new FlowPosition(this._container, flowNode2, 0), null, this._pageIndex);
				this._fixedElements.Push(this._currentFixedElement);
				this._currentFixedElement = (FixedElement)flowNode.Cookie;
				this._flowNodes.Add(flowNode);
				this._endNodes.Push(flowNode2);
			}

			// Token: 0x06008CB4 RID: 36020 RVA: 0x0033DC9A File Offset: 0x0033CC9A
			private void _AddEndNode()
			{
				this._flowNodes.Add((FlowNode)this._endNodes.Pop());
				this._currentFixedElement = (FixedElement)this._fixedElements.Pop();
			}

			// Token: 0x06008CB5 RID: 36021 RVA: 0x0033DCD0 File Offset: 0x0033CCD0
			private void _FinishTextRun(bool addSpace)
			{
				if (this._textRuns.Count > 0)
				{
					int num = 0;
					FixedSOMTextRun fixedSOMTextRun = null;
					for (int i = 0; i < this._textRuns.Count; i++)
					{
						fixedSOMTextRun = this._textRuns[i];
						Glyphs glyphsElement = this._builder.GetGlyphsElement(fixedSOMTextRun.FixedNode);
						FixedTextBuilder.GlyphComparison glyphComparison = this._builder._CompareGlyphs(this._lastGlyphs, glyphsElement);
						if (glyphComparison == FixedTextBuilder.GlyphComparison.DifferentLine)
						{
							this._FinishLine();
						}
						this._lastGlyphs = glyphsElement;
						this._lineLayoutBox.Union(fixedSOMTextRun.BoundingRect);
						fixedSOMTextRun.LineIndex = this._lineResults.Count;
						if (this._nodesInLine.Count == 0 || this._nodesInLine[this._nodesInLine.Count - 1] != fixedSOMTextRun.FixedNode)
						{
							this._nodesInLine.Add(fixedSOMTextRun.FixedNode);
						}
						num += fixedSOMTextRun.EndIndex - fixedSOMTextRun.StartIndex;
						if (i > 0 && this._builder._IsNonContiguous(this._textRuns[i - 1], fixedSOMTextRun, glyphComparison))
						{
							this._textRuns[i - 1].Text = this._textRuns[i - 1].Text + " ";
							num++;
						}
					}
					if (addSpace && fixedSOMTextRun.Text.Length > 0 && !fixedSOMTextRun.Text.EndsWith(" ", StringComparison.Ordinal) && !FixedTextBuilder.IsHyphen(fixedSOMTextRun.Text[fixedSOMTextRun.Text.Length - 1]))
					{
						fixedSOMTextRun.Text += " ";
						num++;
					}
					if (num != 0)
					{
						FlowNode flowNode = new FlowNode(this._NewScopeId(), FlowNodeType.Run, num);
						FlowNode flowNode2 = flowNode;
						FixedSOMElement[] fixedSOMElements = this._textRuns.ToArray();
						flowNode2.FixedSOMElements = fixedSOMElements;
						int num2 = 0;
						foreach (FixedSOMTextRun fixedSOMTextRun2 in this._textRuns)
						{
							fixedSOMTextRun2.FlowNode = flowNode;
							fixedSOMTextRun2.OffsetInFlowNode = num2;
							this._mapping.AddFixedElement(fixedSOMTextRun2);
							num2 += fixedSOMTextRun2.Text.Length;
						}
						this._flowNodes.Add(flowNode);
						this._textRuns.Clear();
					}
				}
				if (this._currentRun != null)
				{
					this._AddEndNode();
					this._currentRun = null;
				}
			}

			// Token: 0x06008CB6 RID: 36022 RVA: 0x0033DF4C File Offset: 0x0033CF4C
			private void _FinishHyperlink()
			{
				if (this._currentNavUri != null)
				{
					this._AddEndNode();
					this._currentNavUri = null;
				}
			}

			// Token: 0x06008CB7 RID: 36023 RVA: 0x0033DF6C File Offset: 0x0033CF6C
			private void _SetHyperlink(Uri navUri, FixedNode node, UIElement shadowHyperlink)
			{
				if (navUri != this._currentNavUri || (navUri != null && navUri.ToString() != this._currentNavUri.ToString()))
				{
					if (this._currentNavUri != null)
					{
						this._AddEndNode();
					}
					if (navUri != null)
					{
						this._AddStartNode(FixedElement.ElementType.Hyperlink);
						this._currentFixedElement.SetValue(Hyperlink.NavigateUriProperty, navUri);
						UIElement uielement = this._fixedPage.GetElement(node) as UIElement;
						if (uielement != null)
						{
							this._currentFixedElement.SetValue(FixedElement.HelpTextProperty, (string)uielement.GetValue(AutomationProperties.HelpTextProperty));
							this._currentFixedElement.SetValue(FixedElement.NameProperty, (string)uielement.GetValue(AutomationProperties.NameProperty));
							if (shadowHyperlink != null)
							{
								this._logicalHyperlinkContainer.MarkAsUsed(shadowHyperlink);
							}
						}
					}
					this._currentNavUri = navUri;
				}
			}

			// Token: 0x06008CB8 RID: 36024 RVA: 0x0033E050 File Offset: 0x0033D050
			private void _FinishLine()
			{
				if (this._nodesInLine.Count > 0)
				{
					FixedLineResult item = new FixedLineResult(this._nodesInLine.ToArray(), this._lineLayoutBox);
					this._lineResults.Add(item);
					this._nodesInLine.Clear();
					this._lineLayoutBox = Rect.Empty;
				}
			}

			// Token: 0x06008CB9 RID: 36025 RVA: 0x0033E0A4 File Offset: 0x0033D0A4
			private int _NewScopeId()
			{
				FixedTextBuilder builder = this._builder;
				int nextScopeId = builder._nextScopeId;
				builder._nextScopeId = nextScopeId + 1;
				return nextScopeId;
			}

			// Token: 0x17001ECF RID: 7887
			// (get) Token: 0x06008CBA RID: 36026 RVA: 0x0033E0C7 File Offset: 0x0033D0C7
			public FixedElement FixedElement
			{
				get
				{
					return this._currentFixedElement;
				}
			}

			// Token: 0x04004829 RID: 18473
			private int _pageIndex;

			// Token: 0x0400482A RID: 18474
			private FixedTextContainer _container;

			// Token: 0x0400482B RID: 18475
			private FixedTextBuilder _builder;

			// Token: 0x0400482C RID: 18476
			private List<FixedSOMTextRun> _textRuns;

			// Token: 0x0400482D RID: 18477
			private List<FlowNode> _flowNodes;

			// Token: 0x0400482E RID: 18478
			private List<FixedNode> _fixedNodes;

			// Token: 0x0400482F RID: 18479
			private List<FixedNode> _nodesInLine;

			// Token: 0x04004830 RID: 18480
			private List<FixedLineResult> _lineResults;

			// Token: 0x04004831 RID: 18481
			private Rect _lineLayoutBox;

			// Token: 0x04004832 RID: 18482
			private Stack _endNodes;

			// Token: 0x04004833 RID: 18483
			private Stack _fixedElements;

			// Token: 0x04004834 RID: 18484
			private FixedElement _currentFixedElement;

			// Token: 0x04004835 RID: 18485
			private FixedFlowMap _mapping;

			// Token: 0x04004836 RID: 18486
			private FixedPageStructure _pageStructure;

			// Token: 0x04004837 RID: 18487
			private Glyphs _lastGlyphs;

			// Token: 0x04004838 RID: 18488
			private FixedSOMTextRun _currentRun;

			// Token: 0x04004839 RID: 18489
			private FixedTextBuilder.FlowModelBuilder.LogicalHyperlinkContainer _logicalHyperlinkContainer;

			// Token: 0x0400483A RID: 18490
			private FixedPage _fixedPage;

			// Token: 0x0400483B RID: 18491
			private Uri _currentNavUri;

			// Token: 0x02000C8B RID: 3211
			private sealed class LogicalHyperlink
			{
				// Token: 0x06009549 RID: 38217 RVA: 0x0034DBE3 File Offset: 0x0034CBE3
				public LogicalHyperlink(Uri uri, Geometry geom, UIElement uiElement)
				{
					this._uiElement = uiElement;
					this._uri = uri;
					this._geometry = geom;
					this._boundingRect = geom.Bounds;
					this._used = false;
				}

				// Token: 0x17001FF8 RID: 8184
				// (get) Token: 0x0600954A RID: 38218 RVA: 0x0034DC13 File Offset: 0x0034CC13
				public Uri Uri
				{
					get
					{
						return this._uri;
					}
				}

				// Token: 0x17001FF9 RID: 8185
				// (get) Token: 0x0600954B RID: 38219 RVA: 0x0034DC1B File Offset: 0x0034CC1B
				public Geometry Geometry
				{
					get
					{
						return this._geometry;
					}
				}

				// Token: 0x17001FFA RID: 8186
				// (get) Token: 0x0600954C RID: 38220 RVA: 0x0034DC23 File Offset: 0x0034CC23
				public Rect BoundingRect
				{
					get
					{
						return this._boundingRect;
					}
				}

				// Token: 0x17001FFB RID: 8187
				// (get) Token: 0x0600954D RID: 38221 RVA: 0x0034DC2B File Offset: 0x0034CC2B
				public UIElement UIElement
				{
					get
					{
						return this._uiElement;
					}
				}

				// Token: 0x17001FFC RID: 8188
				// (get) Token: 0x0600954E RID: 38222 RVA: 0x0034DC33 File Offset: 0x0034CC33
				// (set) Token: 0x0600954F RID: 38223 RVA: 0x0034DC3B File Offset: 0x0034CC3B
				public bool Used
				{
					get
					{
						return this._used;
					}
					set
					{
						this._used = value;
					}
				}

				// Token: 0x04004FB0 RID: 20400
				private UIElement _uiElement;

				// Token: 0x04004FB1 RID: 20401
				private Uri _uri;

				// Token: 0x04004FB2 RID: 20402
				private Geometry _geometry;

				// Token: 0x04004FB3 RID: 20403
				private Rect _boundingRect;

				// Token: 0x04004FB4 RID: 20404
				private bool _used;
			}

			// Token: 0x02000C8C RID: 3212
			private sealed class LogicalHyperlinkContainer : IEnumerable<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>, IEnumerable
			{
				// Token: 0x06009550 RID: 38224 RVA: 0x0034DC44 File Offset: 0x0034CC44
				public LogicalHyperlinkContainer()
				{
					this._hyperlinks = new List<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>();
				}

				// Token: 0x06009551 RID: 38225 RVA: 0x0034DC57 File Offset: 0x0034CC57
				IEnumerator<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink> IEnumerable<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink>.GetEnumerator()
				{
					return this._hyperlinks.GetEnumerator();
				}

				// Token: 0x06009552 RID: 38226 RVA: 0x0034DC57 File Offset: 0x0034CC57
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this._hyperlinks.GetEnumerator();
				}

				// Token: 0x06009553 RID: 38227 RVA: 0x0034DC6C File Offset: 0x0034CC6C
				public void AddLogicalHyperlink(Uri uri, Geometry geometry, UIElement uiElement)
				{
					FixedTextBuilder.FlowModelBuilder.LogicalHyperlink item = new FixedTextBuilder.FlowModelBuilder.LogicalHyperlink(uri, geometry, uiElement);
					this._hyperlinks.Add(item);
				}

				// Token: 0x06009554 RID: 38228 RVA: 0x0034DC90 File Offset: 0x0034CC90
				public Uri GetUri(FixedSOMElement element, FixedPage p, out UIElement shadowElement)
				{
					shadowElement = null;
					UIElement uielement = p.GetElement(element.FixedNode) as UIElement;
					if (uielement == null)
					{
						return null;
					}
					Uri uri = FixedPage.GetNavigateUri(uielement);
					if (uri == null && this._hyperlinks.Count > 0)
					{
						Transform t = uielement.TransformToAncestor(p) as Transform;
						Geometry geom;
						if (uielement is Glyphs)
						{
							GlyphRun glyphRun = ((Glyphs)uielement).ToGlyphRun();
							Rect rect = glyphRun.ComputeAlignmentBox();
							rect.Offset(glyphRun.BaselineOrigin.X, glyphRun.BaselineOrigin.Y);
							geom = new RectangleGeometry(rect);
						}
						else if (uielement is Path)
						{
							geom = ((Path)uielement).Data;
						}
						else
						{
							Image image = (Image)uielement;
							geom = new RectangleGeometry(new Rect(0.0, 0.0, image.Width, image.Height));
						}
						FixedTextBuilder.FlowModelBuilder.LogicalHyperlink logicalHyperlink = this._GetHyperlinkFromGeometry(geom, t);
						if (logicalHyperlink != null)
						{
							uri = logicalHyperlink.Uri;
							shadowElement = logicalHyperlink.UIElement;
						}
					}
					if (uri == null)
					{
						return null;
					}
					return FixedPage.GetLinkUri(p, uri);
				}

				// Token: 0x06009555 RID: 38229 RVA: 0x0034DDB4 File Offset: 0x0034CDB4
				public void MarkAsUsed(UIElement uiElement)
				{
					for (int i = 0; i < this._hyperlinks.Count; i++)
					{
						FixedTextBuilder.FlowModelBuilder.LogicalHyperlink logicalHyperlink = this._hyperlinks[i];
						if (logicalHyperlink.UIElement == uiElement)
						{
							logicalHyperlink.Used = true;
							return;
						}
					}
				}

				// Token: 0x06009556 RID: 38230 RVA: 0x0034DDF8 File Offset: 0x0034CDF8
				private FixedTextBuilder.FlowModelBuilder.LogicalHyperlink _GetHyperlinkFromGeometry(Geometry geom, Transform t)
				{
					Geometry geometry = geom;
					if (t != null && !t.Value.IsIdentity)
					{
						geometry = PathGeometry.CreateFromGeometry(geom);
						geometry.Transform = t;
					}
					double num = geometry.GetArea() * 0.99;
					Rect bounds = geometry.Bounds;
					for (int i = 0; i < this._hyperlinks.Count; i++)
					{
						if (bounds.IntersectsWith(this._hyperlinks[i].BoundingRect) && Geometry.Combine(geometry, this._hyperlinks[i].Geometry, GeometryCombineMode.Intersect, Transform.Identity).GetArea() > num)
						{
							return this._hyperlinks[i];
						}
					}
					return null;
				}

				// Token: 0x04004FB5 RID: 20405
				private List<FixedTextBuilder.FlowModelBuilder.LogicalHyperlink> _hyperlinks;
			}
		}

		// Token: 0x02000B39 RID: 2873
		internal enum GlyphComparison
		{
			// Token: 0x0400483D RID: 18493
			DifferentLine,
			// Token: 0x0400483E RID: 18494
			SameLine,
			// Token: 0x0400483F RID: 18495
			Adjacent,
			// Token: 0x04004840 RID: 18496
			Unknown
		}
	}
}
