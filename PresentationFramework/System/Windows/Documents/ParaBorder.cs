using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x0200066B RID: 1643
	internal class ParaBorder
	{
		// Token: 0x060050F3 RID: 20723 RVA: 0x0024E1C0 File Offset: 0x0024D1C0
		internal ParaBorder()
		{
			this.BorderLeft = new BorderFormat();
			this.BorderTop = new BorderFormat();
			this.BorderRight = new BorderFormat();
			this.BorderBottom = new BorderFormat();
			this.BorderAll = new BorderFormat();
			this.Spacing = 0L;
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x0024E214 File Offset: 0x0024D214
		internal ParaBorder(ParaBorder pb)
		{
			this.BorderLeft = new BorderFormat(pb.BorderLeft);
			this.BorderTop = new BorderFormat(pb.BorderTop);
			this.BorderRight = new BorderFormat(pb.BorderRight);
			this.BorderBottom = new BorderFormat(pb.BorderBottom);
			this.BorderAll = new BorderFormat(pb.BorderAll);
			this.Spacing = pb.Spacing;
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x060050F5 RID: 20725 RVA: 0x0024E288 File Offset: 0x0024D288
		// (set) Token: 0x060050F6 RID: 20726 RVA: 0x0024E290 File Offset: 0x0024D290
		internal BorderFormat BorderLeft
		{
			get
			{
				return this._bfLeft;
			}
			set
			{
				this._bfLeft = value;
			}
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x060050F7 RID: 20727 RVA: 0x0024E299 File Offset: 0x0024D299
		// (set) Token: 0x060050F8 RID: 20728 RVA: 0x0024E2A1 File Offset: 0x0024D2A1
		internal BorderFormat BorderTop
		{
			get
			{
				return this._bfTop;
			}
			set
			{
				this._bfTop = value;
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x0024E2AA File Offset: 0x0024D2AA
		// (set) Token: 0x060050FA RID: 20730 RVA: 0x0024E2B2 File Offset: 0x0024D2B2
		internal BorderFormat BorderRight
		{
			get
			{
				return this._bfRight;
			}
			set
			{
				this._bfRight = value;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x060050FB RID: 20731 RVA: 0x0024E2BB File Offset: 0x0024D2BB
		// (set) Token: 0x060050FC RID: 20732 RVA: 0x0024E2C3 File Offset: 0x0024D2C3
		internal BorderFormat BorderBottom
		{
			get
			{
				return this._bfBottom;
			}
			set
			{
				this._bfBottom = value;
			}
		}

		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x0024E2CC File Offset: 0x0024D2CC
		// (set) Token: 0x060050FE RID: 20734 RVA: 0x0024E2D4 File Offset: 0x0024D2D4
		internal BorderFormat BorderAll
		{
			get
			{
				return this._bfAll;
			}
			set
			{
				this._bfAll = value;
			}
		}

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x060050FF RID: 20735 RVA: 0x0024E2DD File Offset: 0x0024D2DD
		// (set) Token: 0x06005100 RID: 20736 RVA: 0x0024E2E5 File Offset: 0x0024D2E5
		internal long Spacing
		{
			get
			{
				return this._nSpacing;
			}
			set
			{
				this._nSpacing = value;
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06005101 RID: 20737 RVA: 0x0024E2EE File Offset: 0x0024D2EE
		// (set) Token: 0x06005102 RID: 20738 RVA: 0x0024E2FB File Offset: 0x0024D2FB
		internal long CF
		{
			get
			{
				return this.BorderLeft.CF;
			}
			set
			{
				this.BorderLeft.CF = value;
				this.BorderTop.CF = value;
				this.BorderRight.CF = value;
				this.BorderBottom.CF = value;
				this.BorderAll.CF = value;
			}
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06005103 RID: 20739 RVA: 0x0024E33C File Offset: 0x0024D33C
		internal bool IsNone
		{
			get
			{
				return this.BorderLeft.IsNone && this.BorderTop.IsNone && this.BorderRight.IsNone && this.BorderBottom.IsNone && this.BorderAll.IsNone;
			}
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x0024E38C File Offset: 0x0024D38C
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			if (this.IsNone)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" BorderThickness=\"");
			if (!this.BorderAll.IsNone)
			{
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderAll.EffectiveWidth));
			}
			else
			{
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderLeft.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderTop.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderRight.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderBottom.EffectiveWidth));
			}
			stringBuilder.Append("\"");
			ColorTableEntry colorTableEntry = null;
			if (this.CF >= 0L)
			{
				colorTableEntry = converterState.ColorTable.EntryAt((int)this.CF);
			}
			if (colorTableEntry != null)
			{
				stringBuilder.Append(" BorderBrush=\"");
				stringBuilder.Append(colorTableEntry.Color.ToString());
				stringBuilder.Append("\"");
			}
			else
			{
				stringBuilder.Append(" BorderBrush=\"#FF000000\"");
			}
			if (this.Spacing != 0L)
			{
				stringBuilder.Append(" Padding=\"");
				stringBuilder.Append(Converters.TwipToPositivePxString((double)this.Spacing));
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06005105 RID: 20741 RVA: 0x0024E510 File Offset: 0x0024D510
		internal string RTFEncoding
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.IsNone)
				{
					stringBuilder.Append("\\brdrnil");
				}
				else
				{
					stringBuilder.Append("\\brdrl");
					stringBuilder.Append(this.BorderLeft.RTFEncoding);
					if (this.BorderLeft.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderLeft.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrt");
					stringBuilder.Append(this.BorderTop.RTFEncoding);
					if (this.BorderTop.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderTop.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrr");
					stringBuilder.Append(this.BorderRight.RTFEncoding);
					if (this.BorderRight.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderRight.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrb");
					stringBuilder.Append(this.BorderBottom.RTFEncoding);
					if (this.BorderBottom.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderBottom.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brsp");
					stringBuilder.Append(this.Spacing.ToString(CultureInfo.InvariantCulture));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x04002E34 RID: 11828
		private BorderFormat _bfLeft;

		// Token: 0x04002E35 RID: 11829
		private BorderFormat _bfTop;

		// Token: 0x04002E36 RID: 11830
		private BorderFormat _bfRight;

		// Token: 0x04002E37 RID: 11831
		private BorderFormat _bfBottom;

		// Token: 0x04002E38 RID: 11832
		private BorderFormat _bfAll;

		// Token: 0x04002E39 RID: 11833
		private long _nSpacing;
	}
}
