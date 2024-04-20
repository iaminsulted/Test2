using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x0200070E RID: 1806
	[ContentProperty("BlockElementList")]
	public class StoryFragment : IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		// Token: 0x06005E17 RID: 24087 RVA: 0x0028E42F File Offset: 0x0028D42F
		public StoryFragment()
		{
			this._elementList = new List<BlockElement>();
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x0028DEF4 File Offset: 0x0028CEF4
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x0028E444 File Offset: 0x0028D444
		void IAddChild.AddChild(object value)
		{
			if (value is SectionStructure || value is ParagraphStructure || value is FigureStructure || value is ListStructure || value is TableStructure || value is StoryBreak)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType6", new object[]
			{
				value.GetType(),
				typeof(SectionStructure),
				typeof(ParagraphStructure),
				typeof(FigureStructure),
				typeof(ListStructure),
				typeof(TableStructure),
				typeof(StoryBreak)
			}), "value");
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x0028DFA2 File Offset: 0x0028CFA2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06005E1D RID: 24093 RVA: 0x0028E504 File Offset: 0x0028D504
		// (set) Token: 0x06005E1E RID: 24094 RVA: 0x0028E50C File Offset: 0x0028D50C
		public string StoryName
		{
			get
			{
				return this._storyName;
			}
			set
			{
				this._storyName = value;
			}
		}

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06005E1F RID: 24095 RVA: 0x0028E515 File Offset: 0x0028D515
		// (set) Token: 0x06005E20 RID: 24096 RVA: 0x0028E51D File Offset: 0x0028D51D
		public string FragmentName
		{
			get
			{
				return this._fragmentName;
			}
			set
			{
				this._fragmentName = value;
			}
		}

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06005E21 RID: 24097 RVA: 0x0028E526 File Offset: 0x0028D526
		// (set) Token: 0x06005E22 RID: 24098 RVA: 0x0028E52E File Offset: 0x0028D52E
		public string FragmentType
		{
			get
			{
				return this._fragmentType;
			}
			set
			{
				this._fragmentType = value;
			}
		}

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06005E23 RID: 24099 RVA: 0x0028E537 File Offset: 0x0028D537
		internal List<BlockElement> BlockElementList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x04003170 RID: 12656
		private List<BlockElement> _elementList;

		// Token: 0x04003171 RID: 12657
		private string _storyName;

		// Token: 0x04003172 RID: 12658
		private string _fragmentName;

		// Token: 0x04003173 RID: 12659
		private string _fragmentType;
	}
}
