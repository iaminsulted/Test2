using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200052E RID: 1326
	internal class ElementItemsPseudoProperty : ElementPseudoPropertyBase
	{
		// Token: 0x060041C7 RID: 16839 RVA: 0x002194EA File Offset: 0x002184EA
		internal ElementItemsPseudoProperty(IEnumerable value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
			this._value = value;
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x002194FC File Offset: 0x002184FC
		public override string Name
		{
			get
			{
				return "Items";
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x00219503 File Offset: 0x00218503
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				foreach (object instance in this._value)
				{
					yield return new ElementMarkupObject(instance, base.Manager);
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x040024DB RID: 9435
		private IEnumerable _value;
	}
}
