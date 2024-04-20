using System;
using System.Collections;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000657 RID: 1623
	internal class RtfFormatStack : ArrayList
	{
		// Token: 0x06005023 RID: 20515 RVA: 0x0024BDD2 File Offset: 0x0024ADD2
		internal RtfFormatStack() : base(20)
		{
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x0024BDDC File Offset: 0x0024ADDC
		internal void Push()
		{
			FormatState formatState = this.Top();
			FormatState value = (formatState != null) ? new FormatState(formatState) : new FormatState();
			this.Add(value);
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x0024BE09 File Offset: 0x0024AE09
		internal void Pop()
		{
			Invariant.Assert(this.Count != 0);
			if (this.Count > 0)
			{
				this.RemoveAt(this.Count - 1);
			}
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x0024BE30 File Offset: 0x0024AE30
		internal FormatState Top()
		{
			if (this.Count <= 0)
			{
				return null;
			}
			return this.EntryAt(this.Count - 1);
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x0024BE4C File Offset: 0x0024AE4C
		internal FormatState PrevTop(int fromTop)
		{
			int num = this.Count - 1 - fromTop;
			if (num < 0 || num >= this.Count)
			{
				return null;
			}
			return this.EntryAt(num);
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x0024BE7A File Offset: 0x0024AE7A
		internal FormatState EntryAt(int index)
		{
			return (FormatState)this[index];
		}
	}
}
