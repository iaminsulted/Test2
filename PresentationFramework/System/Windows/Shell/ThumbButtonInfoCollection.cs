using System;

namespace System.Windows.Shell
{
	// Token: 0x020003F3 RID: 1011
	public class ThumbButtonInfoCollection : FreezableCollection<ThumbButtonInfo>
	{
		// Token: 0x06002B7E RID: 11134 RVA: 0x001A29A8 File Offset: 0x001A19A8
		protected override Freezable CreateInstanceCore()
		{
			return new ThumbButtonInfoCollection();
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x001A29AF File Offset: 0x001A19AF
		internal static ThumbButtonInfoCollection Empty
		{
			get
			{
				if (ThumbButtonInfoCollection.s_empty == null)
				{
					ThumbButtonInfoCollection thumbButtonInfoCollection = new ThumbButtonInfoCollection();
					thumbButtonInfoCollection.Freeze();
					ThumbButtonInfoCollection.s_empty = thumbButtonInfoCollection;
				}
				return ThumbButtonInfoCollection.s_empty;
			}
		}

		// Token: 0x04001ACA RID: 6858
		private static ThumbButtonInfoCollection s_empty;
	}
}
