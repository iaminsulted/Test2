using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x0200066A RID: 1642
	internal class BorderFormat
	{
		// Token: 0x060050E6 RID: 20710 RVA: 0x0024E043 File Offset: 0x0024D043
		internal BorderFormat()
		{
			this.SetDefaults();
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x0024E051 File Offset: 0x0024D051
		internal BorderFormat(BorderFormat cb)
		{
			this.CF = cb.CF;
			this.Width = cb.Width;
			this.Type = cb.Type;
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x060050E8 RID: 20712 RVA: 0x0024E07D File Offset: 0x0024D07D
		// (set) Token: 0x060050E9 RID: 20713 RVA: 0x0024E085 File Offset: 0x0024D085
		internal long CF
		{
			get
			{
				return this._cf;
			}
			set
			{
				this._cf = value;
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x060050EA RID: 20714 RVA: 0x0024E08E File Offset: 0x0024D08E
		// (set) Token: 0x060050EB RID: 20715 RVA: 0x0024E096 File Offset: 0x0024D096
		internal long Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = Validators.MakeValidBorderWidth(value);
			}
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x060050EC RID: 20716 RVA: 0x0024E0A4 File Offset: 0x0024D0A4
		internal long EffectiveWidth
		{
			get
			{
				switch (this.Type)
				{
				case BorderType.BorderNone:
					return 0L;
				case BorderType.BorderDouble:
					return this.Width * 2L;
				}
				return this.Width;
			}
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x060050ED RID: 20717 RVA: 0x0024E0DF File Offset: 0x0024D0DF
		// (set) Token: 0x060050EE RID: 20718 RVA: 0x0024E0E7 File Offset: 0x0024D0E7
		internal BorderType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x060050EF RID: 20719 RVA: 0x0024E0F0 File Offset: 0x0024D0F0
		internal bool IsNone
		{
			get
			{
				return this.EffectiveWidth <= 0L || this.Type == BorderType.BorderNone;
			}
		}

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x060050F0 RID: 20720 RVA: 0x0024E108 File Offset: 0x0024D108
		internal string RTFEncoding
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.IsNone)
				{
					stringBuilder.Append("\\brdrnone");
				}
				else
				{
					stringBuilder.Append("\\brdrs\\brdrw");
					stringBuilder.Append(this.EffectiveWidth.ToString(CultureInfo.InvariantCulture));
					if (this.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.CF.ToString(CultureInfo.InvariantCulture));
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x060050F1 RID: 20721 RVA: 0x0024E18D File Offset: 0x0024D18D
		internal static BorderFormat EmptyBorderFormat
		{
			get
			{
				if (BorderFormat._emptyBorderFormat == null)
				{
					BorderFormat._emptyBorderFormat = new BorderFormat();
				}
				return BorderFormat._emptyBorderFormat;
			}
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x0024E1A5 File Offset: 0x0024D1A5
		internal void SetDefaults()
		{
			this._cf = -1L;
			this._width = 0L;
			this._type = BorderType.BorderNone;
		}

		// Token: 0x04002E30 RID: 11824
		private long _cf;

		// Token: 0x04002E31 RID: 11825
		private long _width;

		// Token: 0x04002E32 RID: 11826
		private BorderType _type;

		// Token: 0x04002E33 RID: 11827
		private static BorderFormat _emptyBorderFormat;
	}
}
