using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200052F RID: 1327
	internal class ElementDictionaryItemsPseudoProperty : ElementPseudoPropertyBase
	{
		// Token: 0x060041CC RID: 16844 RVA: 0x00219513 File Offset: 0x00218513
		internal ElementDictionaryItemsPseudoProperty(IDictionary value, Type type, ElementMarkupObject obj) : base(value, type, obj)
		{
			this._value = value;
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x00219525 File Offset: 0x00218525
		public override string Name
		{
			get
			{
				return "Entries";
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060041CF RID: 16847 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x0021952C File Offset: 0x0021852C
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				foreach (object obj in this._value)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ElementMarkupObject elementMarkupObject = new ElementMarkupObject(dictionaryEntry.Value, base.Manager);
					elementMarkupObject.SetKey(new ElementKey(dictionaryEntry.Key, typeof(object), elementMarkupObject));
					yield return elementMarkupObject;
				}
				IDictionaryEnumerator dictionaryEnumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x040024DC RID: 9436
		private IDictionary _value;
	}
}
