using System;

namespace MS.Internal.Globalization
{
	// Token: 0x020001B5 RID: 437
	internal class PropertyComment
	{
		// Token: 0x06000E39 RID: 3641 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal PropertyComment()
		{
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x001388A2 File Offset: 0x001378A2
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x001388AA File Offset: 0x001378AA
		internal string PropertyName
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x001388B3 File Offset: 0x001378B3
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x001388BB File Offset: 0x001378BB
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04000A40 RID: 2624
		private string _target;

		// Token: 0x04000A41 RID: 2625
		private object _value;
	}
}
