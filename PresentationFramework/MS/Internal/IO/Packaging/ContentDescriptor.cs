using System;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200015C RID: 348
	internal class ContentDescriptor
	{
		// Token: 0x06000B92 RID: 2962 RVA: 0x0012CC96 File Offset: 0x0012BC96
		internal ContentDescriptor(bool hasIndexableContent, bool isInline, string contentProp, string titleProp)
		{
			this.HasIndexableContent = hasIndexableContent;
			this.IsInline = isInline;
			this.ContentProp = contentProp;
			this.TitleProp = titleProp;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0012CCBB File Offset: 0x0012BCBB
		internal ContentDescriptor(bool hasIndexableContent)
		{
			this.HasIndexableContent = hasIndexableContent;
			this.IsInline = false;
			this.ContentProp = null;
			this.TitleProp = null;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0012CCDF File Offset: 0x0012BCDF
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0012CCE7 File Offset: 0x0012BCE7
		internal bool HasIndexableContent
		{
			get
			{
				return this._hasIndexableContent;
			}
			set
			{
				this._hasIndexableContent = value;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0012CCF0 File Offset: 0x0012BCF0
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0012CCF8 File Offset: 0x0012BCF8
		internal bool IsInline
		{
			get
			{
				return this._isInline;
			}
			set
			{
				this._isInline = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0012CD01 File Offset: 0x0012BD01
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x0012CD09 File Offset: 0x0012BD09
		internal string ContentProp
		{
			get
			{
				return this._contentProp;
			}
			set
			{
				this._contentProp = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0012CD12 File Offset: 0x0012BD12
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0012CD1A File Offset: 0x0012BD1A
		internal string TitleProp
		{
			get
			{
				return this._titleProp;
			}
			set
			{
				this._titleProp = value;
			}
		}

		// Token: 0x040008D0 RID: 2256
		internal const string ResourceKeyName = "Dictionary";

		// Token: 0x040008D1 RID: 2257
		internal const string ResourceName = "ElementTable";

		// Token: 0x040008D2 RID: 2258
		private bool _hasIndexableContent;

		// Token: 0x040008D3 RID: 2259
		private bool _isInline;

		// Token: 0x040008D4 RID: 2260
		private string _contentProp;

		// Token: 0x040008D5 RID: 2261
		private string _titleProp;
	}
}
