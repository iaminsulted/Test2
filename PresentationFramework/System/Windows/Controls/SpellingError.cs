using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200080E RID: 2062
	public class SpellingError
	{
		// Token: 0x060078C4 RID: 30916 RVA: 0x00301750 File Offset: 0x00300750
		internal SpellingError(Speller speller, ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start.CompareTo(end) < 0);
			this._speller = speller;
			this._start = start.GetFrozenPointer(LogicalDirection.Forward);
			this._end = end.GetFrozenPointer(LogicalDirection.Backward);
		}

		// Token: 0x060078C5 RID: 30917 RVA: 0x00301788 File Offset: 0x00300788
		public void Correct(string correctedText)
		{
			if (correctedText == null)
			{
				correctedText = string.Empty;
			}
			((ITextRange)new TextRange(this._start, this._end)).Text = correctedText;
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x003017AB File Offset: 0x003007AB
		public void IgnoreAll()
		{
			this._speller.IgnoreAll(TextRangeBase.GetTextInternal(this._start, this._end));
		}

		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x060078C7 RID: 30919 RVA: 0x003017C9 File Offset: 0x003007C9
		public IEnumerable<string> Suggestions
		{
			get
			{
				IList suggestions = this._speller.GetSuggestionsForError(this);
				int num;
				for (int i = 0; i < suggestions.Count; i = num + 1)
				{
					yield return (string)suggestions[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x060078C8 RID: 30920 RVA: 0x003017D9 File Offset: 0x003007D9
		internal ITextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001BF7 RID: 7159
		// (get) Token: 0x060078C9 RID: 30921 RVA: 0x003017E1 File Offset: 0x003007E1
		internal ITextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x0400397A RID: 14714
		private readonly Speller _speller;

		// Token: 0x0400397B RID: 14715
		private readonly ITextPointer _start;

		// Token: 0x0400397C RID: 14716
		private readonly ITextPointer _end;
	}
}
