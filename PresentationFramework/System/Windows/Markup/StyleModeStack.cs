using System;
using System.Collections.Generic;

namespace System.Windows.Markup
{
	// Token: 0x020004D6 RID: 1238
	internal class StyleModeStack
	{
		// Token: 0x06003F51 RID: 16209 RVA: 0x00210F75 File Offset: 0x0020FF75
		internal StyleModeStack()
		{
			this.Push(StyleMode.Base);
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x00210F91 File Offset: 0x0020FF91
		internal int Depth
		{
			get
			{
				return this._stack.Count - 1;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003F53 RID: 16211 RVA: 0x00210FA0 File Offset: 0x0020FFA0
		internal StyleMode Mode
		{
			get
			{
				return this._stack.Peek();
			}
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x00210FAD File Offset: 0x0020FFAD
		internal void Push(StyleMode mode)
		{
			this._stack.Push(mode);
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x00210FBB File Offset: 0x0020FFBB
		internal void Push()
		{
			this.Push(this.Mode);
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00210FC9 File Offset: 0x0020FFC9
		internal StyleMode Pop()
		{
			return this._stack.Pop();
		}

		// Token: 0x04002374 RID: 9076
		private Stack<StyleMode> _stack = new Stack<StyleMode>(64);
	}
}
