using System;
using System.Windows.Markup;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200083D RID: 2109
	public struct GeneratorPosition
	{
		// Token: 0x17001C94 RID: 7316
		// (get) Token: 0x06007BA2 RID: 31650 RVA: 0x0030C6E9 File Offset: 0x0030B6E9
		// (set) Token: 0x06007BA3 RID: 31651 RVA: 0x0030C6F1 File Offset: 0x0030B6F1
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x06007BA4 RID: 31652 RVA: 0x0030C6FA File Offset: 0x0030B6FA
		// (set) Token: 0x06007BA5 RID: 31653 RVA: 0x0030C702 File Offset: 0x0030B702
		public int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x06007BA6 RID: 31654 RVA: 0x0030C70B File Offset: 0x0030B70B
		public GeneratorPosition(int index, int offset)
		{
			this._index = index;
			this._offset = offset;
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x0030C71B File Offset: 0x0030B71B
		public override int GetHashCode()
		{
			return this._index.GetHashCode() + this._offset.GetHashCode();
		}

		// Token: 0x06007BA8 RID: 31656 RVA: 0x0030C734 File Offset: 0x0030B734
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"GeneratorPosition (",
				this._index.ToString(TypeConverterHelper.InvariantEnglishUS),
				",",
				this._offset.ToString(TypeConverterHelper.InvariantEnglishUS),
				")"
			});
		}

		// Token: 0x06007BA9 RID: 31657 RVA: 0x0030C78C File Offset: 0x0030B78C
		public override bool Equals(object o)
		{
			if (o is GeneratorPosition)
			{
				GeneratorPosition generatorPosition = (GeneratorPosition)o;
				return this._index == generatorPosition._index && this._offset == generatorPosition._offset;
			}
			return false;
		}

		// Token: 0x06007BAA RID: 31658 RVA: 0x0030C7C8 File Offset: 0x0030B7C8
		public static bool operator ==(GeneratorPosition gp1, GeneratorPosition gp2)
		{
			return gp1._index == gp2._index && gp1._offset == gp2._offset;
		}

		// Token: 0x06007BAB RID: 31659 RVA: 0x0030C7E8 File Offset: 0x0030B7E8
		public static bool operator !=(GeneratorPosition gp1, GeneratorPosition gp2)
		{
			return !(gp1 == gp2);
		}

		// Token: 0x04003A4D RID: 14925
		private int _index;

		// Token: 0x04003A4E RID: 14926
		private int _offset;
	}
}
