using System;
using System.Windows.Input;

namespace System.Windows.Documents
{
	// Token: 0x0200061E RID: 1566
	public sealed class FrameworkRichTextComposition : FrameworkTextComposition
	{
		// Token: 0x06004D21 RID: 19745 RVA: 0x0023E9E1 File Offset: 0x0023D9E1
		internal FrameworkRichTextComposition(InputManager inputManager, IInputElement source, object owner) : base(inputManager, source, owner)
		{
		}

		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x06004D22 RID: 19746 RVA: 0x0023E9EC File Offset: 0x0023D9EC
		public TextPointer ResultStart
		{
			get
			{
				if (base._ResultStart != null)
				{
					return (TextPointer)base._ResultStart.GetFrozenPointer(LogicalDirection.Backward);
				}
				return null;
			}
		}

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x06004D23 RID: 19747 RVA: 0x0023EA09 File Offset: 0x0023DA09
		public TextPointer ResultEnd
		{
			get
			{
				if (base._ResultEnd != null)
				{
					return (TextPointer)base._ResultEnd.GetFrozenPointer(LogicalDirection.Forward);
				}
				return null;
			}
		}

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x06004D24 RID: 19748 RVA: 0x0023EA26 File Offset: 0x0023DA26
		public TextPointer CompositionStart
		{
			get
			{
				if (base._CompositionStart != null)
				{
					return (TextPointer)base._CompositionStart.GetFrozenPointer(LogicalDirection.Backward);
				}
				return null;
			}
		}

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x06004D25 RID: 19749 RVA: 0x0023EA43 File Offset: 0x0023DA43
		public TextPointer CompositionEnd
		{
			get
			{
				if (base._CompositionEnd != null)
				{
					return (TextPointer)base._CompositionEnd.GetFrozenPointer(LogicalDirection.Forward);
				}
				return null;
			}
		}
	}
}
