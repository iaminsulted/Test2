using System;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x0200032A RID: 810
	internal sealed class TypographyProperties : TextRunTypographyProperties
	{
		// Token: 0x06001E3D RID: 7741 RVA: 0x0016F8C4 File Offset: 0x0016E8C4
		public TypographyProperties()
		{
			this.ResetProperties();
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0016F8D2 File Offset: 0x0016E8D2
		public override bool StandardLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StandardLigatures);
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0016F8DB File Offset: 0x0016E8DB
		public void SetStandardLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StandardLigatures, value);
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001E40 RID: 7744 RVA: 0x0016F8E5 File Offset: 0x0016E8E5
		public override bool ContextualLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.ContextualLigatures);
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0016F8EE File Offset: 0x0016E8EE
		public void SetContextualLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.ContextualLigatures, value);
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001E42 RID: 7746 RVA: 0x0016F8F8 File Offset: 0x0016E8F8
		public override bool DiscretionaryLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.DiscretionaryLigatures);
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0016F901 File Offset: 0x0016E901
		public void SetDiscretionaryLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.DiscretionaryLigatures, value);
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001E44 RID: 7748 RVA: 0x0016F90B File Offset: 0x0016E90B
		public override bool HistoricalLigatures
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.HistoricalLigatures);
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0016F914 File Offset: 0x0016E914
		public void SetHistoricalLigatures(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.HistoricalLigatures, value);
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x0016F91E File Offset: 0x0016E91E
		public override bool CaseSensitiveForms
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.CaseSensitiveForms);
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0016F927 File Offset: 0x0016E927
		public void SetCaseSensitiveForms(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.CaseSensitiveForms, value);
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x0016F931 File Offset: 0x0016E931
		public override bool ContextualAlternates
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.ContextualAlternates);
			}
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0016F93A File Offset: 0x0016E93A
		public void SetContextualAlternates(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.ContextualAlternates, value);
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x0016F944 File Offset: 0x0016E944
		public override bool HistoricalForms
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.HistoricalForms);
			}
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x0016F94D File Offset: 0x0016E94D
		public void SetHistoricalForms(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.HistoricalForms, value);
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x0016F957 File Offset: 0x0016E957
		public override bool Kerning
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.Kerning);
			}
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0016F960 File Offset: 0x0016E960
		public void SetKerning(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.Kerning, value);
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x0016F96A File Offset: 0x0016E96A
		public override bool CapitalSpacing
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.CapitalSpacing);
			}
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0016F973 File Offset: 0x0016E973
		public void SetCapitalSpacing(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.CapitalSpacing, value);
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x0016F97D File Offset: 0x0016E97D
		public override bool StylisticSet1
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet1);
			}
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x0016F987 File Offset: 0x0016E987
		public void SetStylisticSet1(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet1, value);
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x0016F992 File Offset: 0x0016E992
		public override bool StylisticSet2
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet2);
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0016F99C File Offset: 0x0016E99C
		public void SetStylisticSet2(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet2, value);
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x0016F9A7 File Offset: 0x0016E9A7
		public override bool StylisticSet3
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet3);
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0016F9B1 File Offset: 0x0016E9B1
		public void SetStylisticSet3(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet3, value);
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x0016F9BC File Offset: 0x0016E9BC
		public override bool StylisticSet4
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet4);
			}
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x0016F9C6 File Offset: 0x0016E9C6
		public void SetStylisticSet4(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet4, value);
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x0016F9D1 File Offset: 0x0016E9D1
		public override bool StylisticSet5
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet5);
			}
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0016F9DB File Offset: 0x0016E9DB
		public void SetStylisticSet5(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet5, value);
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x0016F9E6 File Offset: 0x0016E9E6
		public override bool StylisticSet6
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet6);
			}
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0016F9F0 File Offset: 0x0016E9F0
		public void SetStylisticSet6(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet6, value);
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0016F9FB File Offset: 0x0016E9FB
		public override bool StylisticSet7
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet7);
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0016FA05 File Offset: 0x0016EA05
		public void SetStylisticSet7(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet7, value);
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0016FA10 File Offset: 0x0016EA10
		public override bool StylisticSet8
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet8);
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0016FA1A File Offset: 0x0016EA1A
		public void SetStylisticSet8(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet8, value);
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001E60 RID: 7776 RVA: 0x0016FA25 File Offset: 0x0016EA25
		public override bool StylisticSet9
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet9);
			}
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0016FA2F File Offset: 0x0016EA2F
		public void SetStylisticSet9(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet9, value);
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0016FA3A File Offset: 0x0016EA3A
		public override bool StylisticSet10
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet10);
			}
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0016FA44 File Offset: 0x0016EA44
		public void SetStylisticSet10(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet10, value);
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x0016FA4F File Offset: 0x0016EA4F
		public override bool StylisticSet11
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet11);
			}
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0016FA59 File Offset: 0x0016EA59
		public void SetStylisticSet11(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet11, value);
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x0016FA64 File Offset: 0x0016EA64
		public override bool StylisticSet12
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet12);
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0016FA6E File Offset: 0x0016EA6E
		public void SetStylisticSet12(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet12, value);
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x0016FA79 File Offset: 0x0016EA79
		public override bool StylisticSet13
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet13);
			}
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0016FA83 File Offset: 0x0016EA83
		public void SetStylisticSet13(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet13, value);
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x0016FA8E File Offset: 0x0016EA8E
		public override bool StylisticSet14
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet14);
			}
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0016FA98 File Offset: 0x0016EA98
		public void SetStylisticSet14(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet14, value);
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x0016FAA3 File Offset: 0x0016EAA3
		public override bool StylisticSet15
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet15);
			}
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0016FAAD File Offset: 0x0016EAAD
		public void SetStylisticSet15(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet15, value);
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x0016FAB8 File Offset: 0x0016EAB8
		public override bool StylisticSet16
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet16);
			}
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0016FAC2 File Offset: 0x0016EAC2
		public void SetStylisticSet16(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet16, value);
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x0016FACD File Offset: 0x0016EACD
		public override bool StylisticSet17
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet17);
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0016FAD7 File Offset: 0x0016EAD7
		public void SetStylisticSet17(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet17, value);
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x0016FAE2 File Offset: 0x0016EAE2
		public override bool StylisticSet18
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet18);
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0016FAEC File Offset: 0x0016EAEC
		public void SetStylisticSet18(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet18, value);
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x0016FAF7 File Offset: 0x0016EAF7
		public override bool StylisticSet19
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet19);
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0016FB01 File Offset: 0x0016EB01
		public void SetStylisticSet19(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet19, value);
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x0016FB0C File Offset: 0x0016EB0C
		public override bool StylisticSet20
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.StylisticSet20);
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0016FB16 File Offset: 0x0016EB16
		public void SetStylisticSet20(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.StylisticSet20, value);
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x0016FB21 File Offset: 0x0016EB21
		public override FontFraction Fraction
		{
			get
			{
				return this._fraction;
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0016FB29 File Offset: 0x0016EB29
		public void SetFraction(FontFraction value)
		{
			this._fraction = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x0016FB38 File Offset: 0x0016EB38
		public override bool SlashedZero
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.SlashedZero);
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0016FB42 File Offset: 0x0016EB42
		public void SetSlashedZero(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.SlashedZero, value);
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x0016FB4D File Offset: 0x0016EB4D
		public override bool MathematicalGreek
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.MathematicalGreek);
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0016FB57 File Offset: 0x0016EB57
		public void SetMathematicalGreek(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.MathematicalGreek, value);
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0016FB62 File Offset: 0x0016EB62
		public override bool EastAsianExpertForms
		{
			get
			{
				return this.IsBooleanPropertySet(TypographyProperties.PropertyId.EastAsianExpertForms);
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0016FB6C File Offset: 0x0016EB6C
		public void SetEastAsianExpertForms(bool value)
		{
			this.SetBooleanProperty(TypographyProperties.PropertyId.EastAsianExpertForms, value);
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0016FB77 File Offset: 0x0016EB77
		public override FontVariants Variants
		{
			get
			{
				return this._variant;
			}
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0016FB7F File Offset: 0x0016EB7F
		public void SetVariants(FontVariants value)
		{
			this._variant = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x0016FB8E File Offset: 0x0016EB8E
		public override FontCapitals Capitals
		{
			get
			{
				return this._capitals;
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0016FB96 File Offset: 0x0016EB96
		public void SetCapitals(FontCapitals value)
		{
			this._capitals = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0016FBA5 File Offset: 0x0016EBA5
		public override FontNumeralStyle NumeralStyle
		{
			get
			{
				return this._numeralStyle;
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0016FBAD File Offset: 0x0016EBAD
		public void SetNumeralStyle(FontNumeralStyle value)
		{
			this._numeralStyle = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x0016FBBC File Offset: 0x0016EBBC
		public override FontNumeralAlignment NumeralAlignment
		{
			get
			{
				return this._numeralAlignment;
			}
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0016FBC4 File Offset: 0x0016EBC4
		public void SetNumeralAlignment(FontNumeralAlignment value)
		{
			this._numeralAlignment = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x0016FBD3 File Offset: 0x0016EBD3
		public override FontEastAsianWidths EastAsianWidths
		{
			get
			{
				return this._eastAsianWidths;
			}
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0016FBDB File Offset: 0x0016EBDB
		public void SetEastAsianWidths(FontEastAsianWidths value)
		{
			this._eastAsianWidths = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0016FBEA File Offset: 0x0016EBEA
		public override FontEastAsianLanguage EastAsianLanguage
		{
			get
			{
				return this._eastAsianLanguage;
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0016FBF2 File Offset: 0x0016EBF2
		public void SetEastAsianLanguage(FontEastAsianLanguage value)
		{
			this._eastAsianLanguage = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x0016FC01 File Offset: 0x0016EC01
		public override int StandardSwashes
		{
			get
			{
				return this._standardSwashes;
			}
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0016FC09 File Offset: 0x0016EC09
		public void SetStandardSwashes(int value)
		{
			this._standardSwashes = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x0016FC18 File Offset: 0x0016EC18
		public override int ContextualSwashes
		{
			get
			{
				return this._contextualSwashes;
			}
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0016FC20 File Offset: 0x0016EC20
		public void SetContextualSwashes(int value)
		{
			this._contextualSwashes = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x0016FC2F File Offset: 0x0016EC2F
		public override int StylisticAlternates
		{
			get
			{
				return this._stylisticAlternates;
			}
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x0016FC37 File Offset: 0x0016EC37
		public void SetStylisticAlternates(int value)
		{
			this._stylisticAlternates = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x0016FC46 File Offset: 0x0016EC46
		public override int AnnotationAlternates
		{
			get
			{
				return this._annotationAlternates;
			}
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0016FC4E File Offset: 0x0016EC4E
		public void SetAnnotationAlternates(int value)
		{
			this._annotationAlternates = value;
			base.OnPropertiesChanged();
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0016FC60 File Offset: 0x0016EC60
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (base.GetType() != other.GetType())
			{
				return false;
			}
			TypographyProperties typographyProperties = (TypographyProperties)other;
			return this._idPropertySetFlags == typographyProperties._idPropertySetFlags && this._variant == typographyProperties._variant && this._capitals == typographyProperties._capitals && this._fraction == typographyProperties._fraction && this._numeralStyle == typographyProperties._numeralStyle && this._numeralAlignment == typographyProperties._numeralAlignment && this._eastAsianWidths == typographyProperties._eastAsianWidths && this._eastAsianLanguage == typographyProperties._eastAsianLanguage && this._standardSwashes == typographyProperties._standardSwashes && this._contextualSwashes == typographyProperties._contextualSwashes && this._stylisticAlternates == typographyProperties._stylisticAlternates && this._annotationAlternates == typographyProperties._annotationAlternates;
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0016FD40 File Offset: 0x0016ED40
		public override int GetHashCode()
		{
			return (int)(this._idPropertySetFlags ^ (this._idPropertySetFlags & uint.MaxValue) ^ (uint)((uint)this._variant << 28) ^ (uint)((uint)this._capitals << 24) ^ (uint)((uint)this._numeralStyle << 20) ^ (uint)((uint)this._numeralAlignment << 18) ^ (uint)((uint)this._eastAsianWidths << 14) ^ (uint)((uint)this._eastAsianLanguage << 10) ^ (uint)((uint)this._standardSwashes << 6) ^ (uint)((uint)this._contextualSwashes << 2) ^ (uint)this._stylisticAlternates ^ (uint)((uint)this._fraction << 16) ^ (uint)((uint)this._annotationAlternates << 12));
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x001641A9 File Offset: 0x001631A9
		public static bool operator ==(TypographyProperties first, TypographyProperties second)
		{
			if (first == null)
			{
				return second == null;
			}
			return first.Equals(second);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0016FDC5 File Offset: 0x0016EDC5
		public static bool operator !=(TypographyProperties first, TypographyProperties second)
		{
			return !(first == second);
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0016FDD4 File Offset: 0x0016EDD4
		private void ResetProperties()
		{
			this._idPropertySetFlags = 0U;
			this._standardSwashes = 0;
			this._contextualSwashes = 0;
			this._stylisticAlternates = 0;
			this._annotationAlternates = 0;
			this._variant = FontVariants.Normal;
			this._capitals = FontCapitals.Normal;
			this._numeralStyle = FontNumeralStyle.Normal;
			this._numeralAlignment = FontNumeralAlignment.Normal;
			this._eastAsianWidths = FontEastAsianWidths.Normal;
			this._eastAsianLanguage = FontEastAsianLanguage.Normal;
			this._fraction = FontFraction.Normal;
			base.OnPropertiesChanged();
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0016FE3C File Offset: 0x0016EE3C
		private bool IsBooleanPropertySet(TypographyProperties.PropertyId propertyId)
		{
			uint num = 1U << (int)propertyId;
			return (this._idPropertySetFlags & num) > 0U;
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0016FE5C File Offset: 0x0016EE5C
		private void SetBooleanProperty(TypographyProperties.PropertyId propertyId, bool flagValue)
		{
			uint num = 1U << (int)propertyId;
			if (flagValue)
			{
				this._idPropertySetFlags |= num;
			}
			else
			{
				this._idPropertySetFlags &= ~num;
			}
			base.OnPropertiesChanged();
		}

		// Token: 0x04000F03 RID: 3843
		private uint _idPropertySetFlags;

		// Token: 0x04000F04 RID: 3844
		private int _standardSwashes;

		// Token: 0x04000F05 RID: 3845
		private int _contextualSwashes;

		// Token: 0x04000F06 RID: 3846
		private int _stylisticAlternates;

		// Token: 0x04000F07 RID: 3847
		private int _annotationAlternates;

		// Token: 0x04000F08 RID: 3848
		private FontVariants _variant;

		// Token: 0x04000F09 RID: 3849
		private FontCapitals _capitals;

		// Token: 0x04000F0A RID: 3850
		private FontFraction _fraction;

		// Token: 0x04000F0B RID: 3851
		private FontNumeralStyle _numeralStyle;

		// Token: 0x04000F0C RID: 3852
		private FontNumeralAlignment _numeralAlignment;

		// Token: 0x04000F0D RID: 3853
		private FontEastAsianWidths _eastAsianWidths;

		// Token: 0x04000F0E RID: 3854
		private FontEastAsianLanguage _eastAsianLanguage;

		// Token: 0x02000A70 RID: 2672
		private enum PropertyId
		{
			// Token: 0x04004141 RID: 16705
			StandardLigatures,
			// Token: 0x04004142 RID: 16706
			ContextualLigatures,
			// Token: 0x04004143 RID: 16707
			DiscretionaryLigatures,
			// Token: 0x04004144 RID: 16708
			HistoricalLigatures,
			// Token: 0x04004145 RID: 16709
			CaseSensitiveForms,
			// Token: 0x04004146 RID: 16710
			ContextualAlternates,
			// Token: 0x04004147 RID: 16711
			HistoricalForms,
			// Token: 0x04004148 RID: 16712
			Kerning,
			// Token: 0x04004149 RID: 16713
			CapitalSpacing,
			// Token: 0x0400414A RID: 16714
			StylisticSet1,
			// Token: 0x0400414B RID: 16715
			StylisticSet2,
			// Token: 0x0400414C RID: 16716
			StylisticSet3,
			// Token: 0x0400414D RID: 16717
			StylisticSet4,
			// Token: 0x0400414E RID: 16718
			StylisticSet5,
			// Token: 0x0400414F RID: 16719
			StylisticSet6,
			// Token: 0x04004150 RID: 16720
			StylisticSet7,
			// Token: 0x04004151 RID: 16721
			StylisticSet8,
			// Token: 0x04004152 RID: 16722
			StylisticSet9,
			// Token: 0x04004153 RID: 16723
			StylisticSet10,
			// Token: 0x04004154 RID: 16724
			StylisticSet11,
			// Token: 0x04004155 RID: 16725
			StylisticSet12,
			// Token: 0x04004156 RID: 16726
			StylisticSet13,
			// Token: 0x04004157 RID: 16727
			StylisticSet14,
			// Token: 0x04004158 RID: 16728
			StylisticSet15,
			// Token: 0x04004159 RID: 16729
			StylisticSet16,
			// Token: 0x0400415A RID: 16730
			StylisticSet17,
			// Token: 0x0400415B RID: 16731
			StylisticSet18,
			// Token: 0x0400415C RID: 16732
			StylisticSet19,
			// Token: 0x0400415D RID: 16733
			StylisticSet20,
			// Token: 0x0400415E RID: 16734
			SlashedZero,
			// Token: 0x0400415F RID: 16735
			MathematicalGreek,
			// Token: 0x04004160 RID: 16736
			EastAsianExpertForms,
			// Token: 0x04004161 RID: 16737
			PropertyCount
		}
	}
}
