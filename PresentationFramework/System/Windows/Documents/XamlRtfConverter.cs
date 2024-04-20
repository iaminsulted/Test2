using System;

namespace System.Windows.Documents
{
	// Token: 0x020006E1 RID: 1761
	internal class XamlRtfConverter
	{
		// Token: 0x06005CA6 RID: 23718 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal XamlRtfConverter()
		{
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x00287F4C File Offset: 0x00286F4C
		internal string ConvertXamlToRtf(string xamlContent)
		{
			if (xamlContent == null)
			{
				throw new ArgumentNullException("xamlContent");
			}
			string result = string.Empty;
			if (xamlContent != string.Empty)
			{
				XamlToRtfWriter xamlToRtfWriter = new XamlToRtfWriter(xamlContent);
				if (this.WpfPayload != null)
				{
					xamlToRtfWriter.WpfPayload = this.WpfPayload;
				}
				xamlToRtfWriter.Process();
				result = xamlToRtfWriter.Output;
			}
			return result;
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x00287FA4 File Offset: 0x00286FA4
		internal string ConvertRtfToXaml(string rtfContent)
		{
			if (rtfContent == null)
			{
				throw new ArgumentNullException("rtfContent");
			}
			string result = string.Empty;
			if (rtfContent != string.Empty)
			{
				RtfToXamlReader rtfToXamlReader = new RtfToXamlReader(rtfContent);
				rtfToXamlReader.ForceParagraph = this.ForceParagraph;
				if (this.WpfPayload != null)
				{
					rtfToXamlReader.WpfPayload = this.WpfPayload;
				}
				rtfToXamlReader.Process();
				result = rtfToXamlReader.Output;
			}
			return result;
		}

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x06005CA9 RID: 23721 RVA: 0x00288008 File Offset: 0x00287008
		// (set) Token: 0x06005CAA RID: 23722 RVA: 0x00288010 File Offset: 0x00287010
		internal bool ForceParagraph
		{
			get
			{
				return this._forceParagraph;
			}
			set
			{
				this._forceParagraph = value;
			}
		}

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x06005CAB RID: 23723 RVA: 0x00288019 File Offset: 0x00287019
		// (set) Token: 0x06005CAC RID: 23724 RVA: 0x00288021 File Offset: 0x00287021
		internal WpfPayload WpfPayload
		{
			get
			{
				return this._wpfPayload;
			}
			set
			{
				this._wpfPayload = value;
			}
		}

		// Token: 0x04003112 RID: 12562
		internal const int RtfCodePage = 1252;

		// Token: 0x04003113 RID: 12563
		private bool _forceParagraph;

		// Token: 0x04003114 RID: 12564
		private WpfPayload _wpfPayload;
	}
}
