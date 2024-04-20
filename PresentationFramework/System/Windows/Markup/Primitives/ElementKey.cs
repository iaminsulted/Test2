using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200052C RID: 1324
	internal class ElementKey : ElementPseudoPropertyBase
	{
		// Token: 0x060041C1 RID: 16833 RVA: 0x002194D1 File Offset: 0x002184D1
		internal ElementKey(object value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x002194DC File Offset: 0x002184DC
		public override string Name
		{
			get
			{
				return "Key";
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsKey
		{
			get
			{
				return true;
			}
		}
	}
}
