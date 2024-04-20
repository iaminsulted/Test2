using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x0200070D RID: 1805
	[ContentProperty("StoryFragmentList")]
	public class StoryFragments : IAddChild, IEnumerable<StoryFragment>, IEnumerable
	{
		// Token: 0x06005E10 RID: 24080 RVA: 0x0028E39B File Offset: 0x0028D39B
		public StoryFragments()
		{
			this._elementList = new List<StoryFragment>();
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x0028E3AE File Offset: 0x0028D3AE
		public void Add(StoryFragment storyFragment)
		{
			if (storyFragment == null)
			{
				throw new ArgumentNullException("storyFragment");
			}
			((IAddChild)this).AddChild(storyFragment);
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x0028E3C8 File Offset: 0x0028D3C8
		void IAddChild.AddChild(object value)
		{
			if (value is StoryFragment)
			{
				this._elementList.Add((StoryFragment)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(StoryFragment)
			}), "value");
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<StoryFragment> IEnumerable<StoryFragment>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x0028E41F File Offset: 0x0028D41F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StoryFragment>)this).GetEnumerator();
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06005E16 RID: 24086 RVA: 0x0028E427 File Offset: 0x0028D427
		internal List<StoryFragment> StoryFragmentList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x0400316F RID: 12655
		private List<StoryFragment> _elementList;
	}
}
