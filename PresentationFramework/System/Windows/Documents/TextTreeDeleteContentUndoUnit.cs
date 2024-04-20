using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006C8 RID: 1736
	internal class TextTreeDeleteContentUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06005A7D RID: 23165 RVA: 0x002829B4 File Offset: 0x002819B4
		internal TextTreeDeleteContentUndoUnit(TextContainer tree, TextPointer start, TextPointer end) : base(tree, start.GetSymbolOffset())
		{
			start.DebugAssertGeneration();
			end.DebugAssertGeneration();
			Invariant.Assert(start.GetScopingNode() == end.GetScopingNode(), "start/end have different scope!");
			TextTreeNode adjacentNode = start.GetAdjacentNode(LogicalDirection.Forward);
			TextTreeNode adjacentNode2 = end.GetAdjacentNode(LogicalDirection.Forward);
			this._content = this.CopyContent(adjacentNode, adjacentNode2);
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x00282A10 File Offset: 0x00281A10
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer navigator = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			for (TextTreeDeleteContentUndoUnit.ContentContainer contentContainer = this._content; contentContainer != null; contentContainer = contentContainer.NextContainer)
			{
				contentContainer.Do(navigator);
			}
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x00282A50 File Offset: 0x00281A50
		internal static TableColumn[] SaveColumns(Table table)
		{
			TableColumn[] array;
			if (table.Columns.Count > 0)
			{
				array = new TableColumn[table.Columns.Count];
				for (int i = 0; i < table.Columns.Count; i++)
				{
					array[i] = TextTreeDeleteContentUndoUnit.CopyColumn(table.Columns[i]);
				}
			}
			else
			{
				array = null;
			}
			return array;
		}

		// Token: 0x06005A80 RID: 23168 RVA: 0x00282AAC File Offset: 0x00281AAC
		internal static void RestoreColumns(Table table, TableColumn[] savedColumns)
		{
			if (savedColumns != null)
			{
				for (int i = 0; i < savedColumns.Length; i++)
				{
					if (table.Columns.Count <= i)
					{
						table.Columns.Add(TextTreeDeleteContentUndoUnit.CopyColumn(savedColumns[i]));
					}
				}
			}
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x00282AEC File Offset: 0x00281AEC
		private static TableColumn CopyColumn(TableColumn sourceTableColumn)
		{
			TableColumn tableColumn = new TableColumn();
			LocalValueEnumerator localValueEnumerator = sourceTableColumn.GetLocalValueEnumerator();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				if (!localValueEntry.Property.ReadOnly)
				{
					tableColumn.SetValue(localValueEntry.Property, localValueEntry.Value);
				}
			}
			return tableColumn;
		}

		// Token: 0x06005A82 RID: 23170 RVA: 0x00282B3C File Offset: 0x00281B3C
		private TextTreeDeleteContentUndoUnit.ContentContainer CopyContent(TextTreeNode node, TextTreeNode haltNode)
		{
			TextTreeDeleteContentUndoUnit.ContentContainer result = null;
			TextTreeDeleteContentUndoUnit.ContentContainer contentContainer = null;
			while (node != haltNode && node != null)
			{
				TextTreeTextNode textTreeTextNode = node as TextTreeTextNode;
				TextTreeDeleteContentUndoUnit.ContentContainer contentContainer2;
				if (textTreeTextNode != null)
				{
					node = this.CopyTextNode(textTreeTextNode, haltNode, out contentContainer2);
				}
				else
				{
					TextTreeObjectNode textTreeObjectNode = node as TextTreeObjectNode;
					if (textTreeObjectNode != null)
					{
						node = this.CopyObjectNode(textTreeObjectNode, out contentContainer2);
					}
					else
					{
						Invariant.Assert(node is TextTreeTextElementNode, "Unexpected TextTreeNode type!");
						TextTreeTextElementNode elementNode = (TextTreeTextElementNode)node;
						node = this.CopyElementNode(elementNode, out contentContainer2);
					}
				}
				if (contentContainer == null)
				{
					result = contentContainer2;
				}
				else
				{
					contentContainer.NextContainer = contentContainer2;
				}
				contentContainer = contentContainer2;
			}
			return result;
		}

		// Token: 0x06005A83 RID: 23171 RVA: 0x00282BC0 File Offset: 0x00281BC0
		private TextTreeNode CopyTextNode(TextTreeTextNode textNode, TextTreeNode haltNode, out TextTreeDeleteContentUndoUnit.ContentContainer container)
		{
			Invariant.Assert(textNode != haltNode, "Expect at least one node to copy!");
			int symbolOffset = textNode.GetSymbolOffset(base.TextContainer.Generation);
			int num = 0;
			SplayTreeNode nextNode;
			do
			{
				num += textNode.SymbolCount;
				nextNode = textNode.GetNextNode();
				textNode = (nextNode as TextTreeTextNode);
			}
			while (textNode != null && textNode != haltNode);
			char[] array = new char[num];
			TextTreeText.ReadText(base.TextContainer.RootTextBlock, symbolOffset, num, array, 0);
			container = new TextTreeDeleteContentUndoUnit.TextContentContainer(array);
			return (TextTreeNode)nextNode;
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x00282C3C File Offset: 0x00281C3C
		private TextTreeNode CopyObjectNode(TextTreeObjectNode objectNode, out TextTreeDeleteContentUndoUnit.ContentContainer container)
		{
			string xml = XamlWriter.Save(objectNode.EmbeddedElement);
			container = new TextTreeDeleteContentUndoUnit.ObjectContentContainer(xml, objectNode.EmbeddedElement);
			return (TextTreeNode)objectNode.GetNextNode();
		}

		// Token: 0x06005A85 RID: 23173 RVA: 0x00282C70 File Offset: 0x00281C70
		private TextTreeNode CopyElementNode(TextTreeTextElementNode elementNode, out TextTreeDeleteContentUndoUnit.ContentContainer container)
		{
			if (elementNode.TextElement is Table)
			{
				container = new TextTreeDeleteContentUndoUnit.TableElementContentContainer(elementNode.TextElement as Table, TextTreeUndoUnit.GetPropertyRecordArray(elementNode.TextElement), this.CopyContent((TextTreeNode)elementNode.GetFirstContainedNode(), null));
			}
			else
			{
				container = new TextTreeDeleteContentUndoUnit.ElementContentContainer(elementNode.TextElement.GetType(), TextTreeUndoUnit.GetPropertyRecordArray(elementNode.TextElement), elementNode.TextElement.Resources, this.CopyContent((TextTreeNode)elementNode.GetFirstContainedNode(), null));
			}
			return (TextTreeNode)elementNode.GetNextNode();
		}

		// Token: 0x04003052 RID: 12370
		private readonly TextTreeDeleteContentUndoUnit.ContentContainer _content;

		// Token: 0x02000B7B RID: 2939
		private abstract class ContentContainer
		{
			// Token: 0x06008E22 RID: 36386
			internal abstract void Do(TextPointer navigator);

			// Token: 0x17001F19 RID: 7961
			// (get) Token: 0x06008E23 RID: 36387 RVA: 0x003405C6 File Offset: 0x0033F5C6
			// (set) Token: 0x06008E24 RID: 36388 RVA: 0x003405CE File Offset: 0x0033F5CE
			internal TextTreeDeleteContentUndoUnit.ContentContainer NextContainer
			{
				get
				{
					return this._nextContainer;
				}
				set
				{
					this._nextContainer = value;
				}
			}

			// Token: 0x0400490D RID: 18701
			private TextTreeDeleteContentUndoUnit.ContentContainer _nextContainer;
		}

		// Token: 0x02000B7C RID: 2940
		private class TextContentContainer : TextTreeDeleteContentUndoUnit.ContentContainer
		{
			// Token: 0x06008E26 RID: 36390 RVA: 0x003405D7 File Offset: 0x0033F5D7
			internal TextContentContainer(char[] text)
			{
				this._text = text;
			}

			// Token: 0x06008E27 RID: 36391 RVA: 0x003405E6 File Offset: 0x0033F5E6
			internal override void Do(TextPointer navigator)
			{
				navigator.TextContainer.InsertTextInternal(navigator, this._text);
			}

			// Token: 0x0400490E RID: 18702
			private readonly char[] _text;
		}

		// Token: 0x02000B7D RID: 2941
		private class ObjectContentContainer : TextTreeDeleteContentUndoUnit.ContentContainer
		{
			// Token: 0x06008E28 RID: 36392 RVA: 0x003405FA File Offset: 0x0033F5FA
			internal ObjectContentContainer(string xml, object element)
			{
				this._xml = xml;
				this._element = element;
			}

			// Token: 0x06008E29 RID: 36393 RVA: 0x00340610 File Offset: 0x0033F610
			internal override void Do(TextPointer navigator)
			{
				DependencyObject dependencyObject = null;
				if (this._xml != null)
				{
					try
					{
						dependencyObject = (DependencyObject)XamlReader.Load(new XmlTextReader(new StringReader(this._xml)));
					}
					catch (XamlParseException ex)
					{
						Invariant.Assert(ex != null);
					}
				}
				if (dependencyObject == null)
				{
					dependencyObject = new Grid();
				}
				navigator.TextContainer.InsertEmbeddedObjectInternal(navigator, dependencyObject);
			}

			// Token: 0x0400490F RID: 18703
			private readonly string _xml;

			// Token: 0x04004910 RID: 18704
			private readonly object _element;
		}

		// Token: 0x02000B7E RID: 2942
		private class ElementContentContainer : TextTreeDeleteContentUndoUnit.ContentContainer
		{
			// Token: 0x06008E2A RID: 36394 RVA: 0x00340674 File Offset: 0x0033F674
			internal ElementContentContainer(Type elementType, PropertyRecord[] localValues, ResourceDictionary resources, TextTreeDeleteContentUndoUnit.ContentContainer childContainer)
			{
				this._elementType = elementType;
				this._localValues = localValues;
				this._childContainer = childContainer;
				this._resources = resources;
			}

			// Token: 0x06008E2B RID: 36395 RVA: 0x0034069C File Offset: 0x0033F69C
			internal override void Do(TextPointer navigator)
			{
				TextElement textElement = (TextElement)Activator.CreateInstance(this._elementType);
				textElement.Reposition(navigator, navigator);
				navigator.MoveToNextContextPosition(LogicalDirection.Backward);
				navigator.TextContainer.SetValues(navigator, TextTreeUndoUnit.ArrayToLocalValueEnumerator(this._localValues));
				textElement.Resources = this._resources;
				for (TextTreeDeleteContentUndoUnit.ContentContainer contentContainer = this._childContainer; contentContainer != null; contentContainer = contentContainer.NextContainer)
				{
					contentContainer.Do(navigator);
				}
				navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			}

			// Token: 0x04004911 RID: 18705
			private readonly Type _elementType;

			// Token: 0x04004912 RID: 18706
			private readonly PropertyRecord[] _localValues;

			// Token: 0x04004913 RID: 18707
			private readonly ResourceDictionary _resources;

			// Token: 0x04004914 RID: 18708
			private readonly TextTreeDeleteContentUndoUnit.ContentContainer _childContainer;
		}

		// Token: 0x02000B7F RID: 2943
		private class TableElementContentContainer : TextTreeDeleteContentUndoUnit.ElementContentContainer
		{
			// Token: 0x06008E2C RID: 36396 RVA: 0x0034070D File Offset: 0x0033F70D
			internal TableElementContentContainer(Table table, PropertyRecord[] localValues, TextTreeDeleteContentUndoUnit.ContentContainer childContainer) : base(table.GetType(), localValues, table.Resources, childContainer)
			{
				this._cpTable = table.TextContainer.Start.GetOffsetToPosition(table.ContentStart);
				this._columns = TextTreeDeleteContentUndoUnit.SaveColumns(table);
			}

			// Token: 0x06008E2D RID: 36397 RVA: 0x0034074B File Offset: 0x0033F74B
			internal override void Do(TextPointer navigator)
			{
				base.Do(navigator);
				if (this._columns != null)
				{
					TextTreeDeleteContentUndoUnit.RestoreColumns((Table)new TextPointer(navigator.TextContainer.Start, this._cpTable, LogicalDirection.Forward).Parent, this._columns);
				}
			}

			// Token: 0x04004915 RID: 18709
			private TableColumn[] _columns;

			// Token: 0x04004916 RID: 18710
			private int _cpTable;
		}
	}
}
