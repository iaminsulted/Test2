using System;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x0200053C RID: 1340
	public class BamlLocalizableResource
	{
		// Token: 0x0600424A RID: 16970 RVA: 0x0021B350 File Offset: 0x0021A350
		public BamlLocalizableResource() : this(null, null, LocalizationCategory.None, true, true)
		{
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0021B35D File Offset: 0x0021A35D
		public BamlLocalizableResource(string content, string comments, LocalizationCategory category, bool modifiable, bool readable)
		{
			this._content = content;
			this._comments = comments;
			this._category = category;
			this.Modifiable = modifiable;
			this.Readable = readable;
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0021B38A File Offset: 0x0021A38A
		internal BamlLocalizableResource(BamlLocalizableResource other)
		{
			this._content = other._content;
			this._comments = other._comments;
			this._flags = other._flags;
			this._category = other._category;
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x0021B3C2 File Offset: 0x0021A3C2
		// (set) Token: 0x0600424E RID: 16974 RVA: 0x0021B3CA File Offset: 0x0021A3CA
		public string Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x0021B3D3 File Offset: 0x0021A3D3
		// (set) Token: 0x06004250 RID: 16976 RVA: 0x0021B3DB File Offset: 0x0021A3DB
		public string Comments
		{
			get
			{
				return this._comments;
			}
			set
			{
				this._comments = value;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x0021B3E4 File Offset: 0x0021A3E4
		// (set) Token: 0x06004252 RID: 16978 RVA: 0x0021B3F1 File Offset: 0x0021A3F1
		public bool Modifiable
		{
			get
			{
				return (this._flags & BamlLocalizableResource.LocalizationFlags.Modifiable) > (BamlLocalizableResource.LocalizationFlags)0;
			}
			set
			{
				if (value)
				{
					this._flags |= BamlLocalizableResource.LocalizationFlags.Modifiable;
					return;
				}
				this._flags &= ~BamlLocalizableResource.LocalizationFlags.Modifiable;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06004253 RID: 16979 RVA: 0x0021B417 File Offset: 0x0021A417
		// (set) Token: 0x06004254 RID: 16980 RVA: 0x0021B424 File Offset: 0x0021A424
		public bool Readable
		{
			get
			{
				return (this._flags & BamlLocalizableResource.LocalizationFlags.Readable) > (BamlLocalizableResource.LocalizationFlags)0;
			}
			set
			{
				if (value)
				{
					this._flags |= BamlLocalizableResource.LocalizationFlags.Readable;
					return;
				}
				this._flags &= ~BamlLocalizableResource.LocalizationFlags.Readable;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x0021B44A File Offset: 0x0021A44A
		// (set) Token: 0x06004256 RID: 16982 RVA: 0x0021B452 File Offset: 0x0021A452
		public LocalizationCategory Category
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x0021B45C File Offset: 0x0021A45C
		public override bool Equals(object other)
		{
			BamlLocalizableResource bamlLocalizableResource = other as BamlLocalizableResource;
			return bamlLocalizableResource != null && (this._content == bamlLocalizableResource._content && this._comments == bamlLocalizableResource._comments && this._flags == bamlLocalizableResource._flags) && this._category == bamlLocalizableResource._category;
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0021B4B9 File Offset: 0x0021A4B9
		public override int GetHashCode()
		{
			return ((this._content == null) ? 0 : this._content.GetHashCode()) ^ ((this._comments == null) ? 0 : this._comments.GetHashCode()) ^ (int)this._flags ^ (int)this._category;
		}

		// Token: 0x040024F5 RID: 9461
		private string _content;

		// Token: 0x040024F6 RID: 9462
		private string _comments;

		// Token: 0x040024F7 RID: 9463
		private BamlLocalizableResource.LocalizationFlags _flags;

		// Token: 0x040024F8 RID: 9464
		private LocalizationCategory _category;

		// Token: 0x02000B1A RID: 2842
		[Flags]
		private enum LocalizationFlags : byte
		{
			// Token: 0x040047D1 RID: 18385
			Readable = 1,
			// Token: 0x040047D2 RID: 18386
			Modifiable = 2
		}
	}
}
