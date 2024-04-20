using System;
using WinRT;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000316 RID: 790
	[WindowsRuntimeType]
	internal struct TextSegment : IEquatable<TextSegment>
	{
		// Token: 0x06001D49 RID: 7497 RVA: 0x0016CC4C File Offset: 0x0016BC4C
		public TextSegment(uint _StartPosition, uint _Length)
		{
			this.StartPosition = _StartPosition;
			this.Length = _Length;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0016CC5C File Offset: 0x0016BC5C
		public static bool operator ==(TextSegment x, TextSegment y)
		{
			return x.StartPosition == y.StartPosition && x.Length == y.Length;
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0016CC7C File Offset: 0x0016BC7C
		public static bool operator !=(TextSegment x, TextSegment y)
		{
			return !(x == y);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0016CC88 File Offset: 0x0016BC88
		public bool Equals(TextSegment other)
		{
			return this == other;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x0016CC98 File Offset: 0x0016BC98
		public override bool Equals(object obj)
		{
			if (obj is TextSegment)
			{
				TextSegment y = (TextSegment)obj;
				return this == y;
			}
			return false;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x0016CCC2 File Offset: 0x0016BCC2
		public override int GetHashCode()
		{
			return this.StartPosition.GetHashCode() ^ this.Length.GetHashCode();
		}

		// Token: 0x04000E91 RID: 3729
		public uint StartPosition;

		// Token: 0x04000E92 RID: 3730
		public uint Length;
	}
}
