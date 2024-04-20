using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200052D RID: 1325
	internal class ElementConstructorArgument : ElementPseudoPropertyBase
	{
		// Token: 0x060041C4 RID: 16836 RVA: 0x002194D1 File Offset: 0x002184D1
		internal ElementConstructorArgument(object value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x002194E3 File Offset: 0x002184E3
		public override string Name
		{
			get
			{
				return "Argument";
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsConstructorArgument
		{
			get
			{
				return true;
			}
		}
	}
}
