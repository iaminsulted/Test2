using System;
using System.Xaml;
using MS.Internal.Xaml.Context;

namespace System.Windows.Markup
{
	// Token: 0x0200050E RID: 1294
	internal class WpfXamlFrame : XamlFrame
	{
		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06004064 RID: 16484 RVA: 0x00214000 File Offset: 0x00213000
		// (set) Token: 0x06004065 RID: 16485 RVA: 0x00214008 File Offset: 0x00213008
		public bool FreezeFreezable { get; set; }

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x00214011 File Offset: 0x00213011
		// (set) Token: 0x06004067 RID: 16487 RVA: 0x00214019 File Offset: 0x00213019
		public XamlMember Property { get; set; }

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06004068 RID: 16488 RVA: 0x00214022 File Offset: 0x00213022
		// (set) Token: 0x06004069 RID: 16489 RVA: 0x0021402A File Offset: 0x0021302A
		public XamlType Type { get; set; }

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x0600406A RID: 16490 RVA: 0x00214033 File Offset: 0x00213033
		// (set) Token: 0x0600406B RID: 16491 RVA: 0x0021403B File Offset: 0x0021303B
		public object Instance { get; set; }

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x0600406C RID: 16492 RVA: 0x00214044 File Offset: 0x00213044
		// (set) Token: 0x0600406D RID: 16493 RVA: 0x0021404C File Offset: 0x0021304C
		public XmlnsDictionary XmlnsDictionary { get; set; }

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x0600406E RID: 16494 RVA: 0x00214055 File Offset: 0x00213055
		// (set) Token: 0x0600406F RID: 16495 RVA: 0x0021405D File Offset: 0x0021305D
		public bool? XmlSpace { get; set; }

		// Token: 0x06004070 RID: 16496 RVA: 0x00214068 File Offset: 0x00213068
		public override void Reset()
		{
			this.Type = null;
			this.Property = null;
			this.Instance = null;
			this.XmlnsDictionary = null;
			this.XmlSpace = null;
			if (this.FreezeFreezable)
			{
				this.FreezeFreezable = false;
			}
		}
	}
}
