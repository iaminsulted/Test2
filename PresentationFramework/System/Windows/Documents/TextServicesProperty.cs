using System;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C5 RID: 1733
	internal class TextServicesProperty
	{
		// Token: 0x060059FC RID: 23036 RVA: 0x0027F090 File Offset: 0x0027E090
		internal TextServicesProperty(TextStore textstore)
		{
			this._textstore = textstore;
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x0027F09F File Offset: 0x0027E09F
		internal void OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			if (this._propertyRanges == null)
			{
				this._propertyRanges = new TextServicesDisplayAttributePropertyRanges(this._textstore);
			}
			this._propertyRanges.OnEndEdit(context, ecReadOnly, editRecord);
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x0027F0C8 File Offset: 0x0027E0C8
		internal void OnLayoutUpdated()
		{
			TextServicesDisplayAttributePropertyRanges textServicesDisplayAttributePropertyRanges = this._propertyRanges as TextServicesDisplayAttributePropertyRanges;
			if (textServicesDisplayAttributePropertyRanges != null)
			{
				textServicesDisplayAttributePropertyRanges.OnLayoutUpdated();
			}
		}

		// Token: 0x0400302B RID: 12331
		private TextServicesPropertyRanges _propertyRanges;

		// Token: 0x0400302C RID: 12332
		private readonly TextStore _textstore;
	}
}
