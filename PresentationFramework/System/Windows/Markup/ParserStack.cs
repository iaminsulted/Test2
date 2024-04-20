using System;
using System.Collections;

namespace System.Windows.Markup
{
	// Token: 0x020004CE RID: 1230
	internal class ParserStack : ArrayList
	{
		// Token: 0x06003F05 RID: 16133 RVA: 0x00210368 File Offset: 0x0020F368
		internal ParserStack()
		{
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00210370 File Offset: 0x0020F370
		private ParserStack(ICollection collection) : base(collection)
		{
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00210379 File Offset: 0x0020F379
		public void Push(object o)
		{
			this.Add(o);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00210383 File Offset: 0x0020F383
		public object Pop()
		{
			object result = this[this.Count - 1];
			this.RemoveAt(this.Count - 1);
			return result;
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x002103A1 File Offset: 0x0020F3A1
		public object Peek()
		{
			return this[this.Count - 1];
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x002103B1 File Offset: 0x0020F3B1
		public override object Clone()
		{
			return new ParserStack(this);
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06003F0B RID: 16139 RVA: 0x002103B9 File Offset: 0x0020F3B9
		internal object CurrentContext
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this[this.Count - 1];
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003F0C RID: 16140 RVA: 0x002103D4 File Offset: 0x0020F3D4
		internal object ParentContext
		{
			get
			{
				if (this.Count <= 1)
				{
					return null;
				}
				return this[this.Count - 2];
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x002103EF File Offset: 0x0020F3EF
		internal object GrandParentContext
		{
			get
			{
				if (this.Count <= 2)
				{
					return null;
				}
				return this[this.Count - 3];
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x0021040A File Offset: 0x0020F40A
		internal object GreatGrandParentContext
		{
			get
			{
				if (this.Count <= 3)
				{
					return null;
				}
				return this[this.Count - 4];
			}
		}
	}
}
