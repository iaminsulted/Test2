using System;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x0200040B RID: 1035
	internal class Baml2006ReaderSettings : XamlReaderSettings
	{
		// Token: 0x06002D24 RID: 11556 RVA: 0x001AB3B4 File Offset: 0x001AA3B4
		public Baml2006ReaderSettings()
		{
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x001AB3BC File Offset: 0x001AA3BC
		public Baml2006ReaderSettings(Baml2006ReaderSettings settings) : base(settings)
		{
			this.OwnsStream = settings.OwnsStream;
			this.IsBamlFragment = settings.IsBamlFragment;
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x001AB3DD File Offset: 0x001AA3DD
		public Baml2006ReaderSettings(XamlReaderSettings settings) : base(settings)
		{
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x001AB3E6 File Offset: 0x001AA3E6
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x001AB3EE File Offset: 0x001AA3EE
		internal bool OwnsStream { get; set; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x001AB3F7 File Offset: 0x001AA3F7
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x001AB3FF File Offset: 0x001AA3FF
		internal bool IsBamlFragment { get; set; }
	}
}
