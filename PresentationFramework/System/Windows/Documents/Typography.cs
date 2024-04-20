using System;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x020006D8 RID: 1752
	public sealed class Typography
	{
		// Token: 0x06005BA9 RID: 23465 RVA: 0x0028491D File Offset: 0x0028391D
		internal Typography(DependencyObject owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x0028493C File Offset: 0x0028393C
		static Typography()
		{
			Typography.Default.SetStandardLigatures(true);
			Typography.Default.SetContextualAlternates(true);
			Typography.Default.SetContextualLigatures(true);
			Typography.Default.SetKerning(true);
		}

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x06005BAB RID: 23467 RVA: 0x002851BC File Offset: 0x002841BC
		// (set) Token: 0x06005BAC RID: 23468 RVA: 0x002851D3 File Offset: 0x002841D3
		public bool StandardLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StandardLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.StandardLigaturesProperty, value);
			}
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x06005BAD RID: 23469 RVA: 0x002851E6 File Offset: 0x002841E6
		// (set) Token: 0x06005BAE RID: 23470 RVA: 0x002851FD File Offset: 0x002841FD
		public bool ContextualLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.ContextualLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.ContextualLigaturesProperty, value);
			}
		}

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x06005BAF RID: 23471 RVA: 0x00285210 File Offset: 0x00284210
		// (set) Token: 0x06005BB0 RID: 23472 RVA: 0x00285227 File Offset: 0x00284227
		public bool DiscretionaryLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.DiscretionaryLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.DiscretionaryLigaturesProperty, value);
			}
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x0028523A File Offset: 0x0028423A
		// (set) Token: 0x06005BB2 RID: 23474 RVA: 0x00285251 File Offset: 0x00284251
		public bool HistoricalLigatures
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.HistoricalLigaturesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.HistoricalLigaturesProperty, value);
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x06005BB3 RID: 23475 RVA: 0x00285264 File Offset: 0x00284264
		// (set) Token: 0x06005BB4 RID: 23476 RVA: 0x0028527B File Offset: 0x0028427B
		public int AnnotationAlternates
		{
			get
			{
				return (int)this._owner.GetValue(Typography.AnnotationAlternatesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.AnnotationAlternatesProperty, value);
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x06005BB5 RID: 23477 RVA: 0x00285293 File Offset: 0x00284293
		// (set) Token: 0x06005BB6 RID: 23478 RVA: 0x002852AA File Offset: 0x002842AA
		public bool ContextualAlternates
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.ContextualAlternatesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.ContextualAlternatesProperty, value);
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x06005BB7 RID: 23479 RVA: 0x002852BD File Offset: 0x002842BD
		// (set) Token: 0x06005BB8 RID: 23480 RVA: 0x002852D4 File Offset: 0x002842D4
		public bool HistoricalForms
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.HistoricalFormsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.HistoricalFormsProperty, value);
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x06005BB9 RID: 23481 RVA: 0x002852E7 File Offset: 0x002842E7
		// (set) Token: 0x06005BBA RID: 23482 RVA: 0x002852FE File Offset: 0x002842FE
		public bool Kerning
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.KerningProperty);
			}
			set
			{
				this._owner.SetValue(Typography.KerningProperty, value);
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x06005BBB RID: 23483 RVA: 0x00285311 File Offset: 0x00284311
		// (set) Token: 0x06005BBC RID: 23484 RVA: 0x00285328 File Offset: 0x00284328
		public bool CapitalSpacing
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.CapitalSpacingProperty);
			}
			set
			{
				this._owner.SetValue(Typography.CapitalSpacingProperty, value);
			}
		}

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x06005BBD RID: 23485 RVA: 0x0028533B File Offset: 0x0028433B
		// (set) Token: 0x06005BBE RID: 23486 RVA: 0x00285352 File Offset: 0x00284352
		public bool CaseSensitiveForms
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.CaseSensitiveFormsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.CaseSensitiveFormsProperty, value);
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x06005BBF RID: 23487 RVA: 0x00285365 File Offset: 0x00284365
		// (set) Token: 0x06005BC0 RID: 23488 RVA: 0x0028537C File Offset: 0x0028437C
		public bool StylisticSet1
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet1Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet1Property, value);
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x06005BC1 RID: 23489 RVA: 0x0028538F File Offset: 0x0028438F
		// (set) Token: 0x06005BC2 RID: 23490 RVA: 0x002853A6 File Offset: 0x002843A6
		public bool StylisticSet2
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet2Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet2Property, value);
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x06005BC3 RID: 23491 RVA: 0x002853B9 File Offset: 0x002843B9
		// (set) Token: 0x06005BC4 RID: 23492 RVA: 0x002853D0 File Offset: 0x002843D0
		public bool StylisticSet3
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet3Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet3Property, value);
			}
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x06005BC5 RID: 23493 RVA: 0x002853E3 File Offset: 0x002843E3
		// (set) Token: 0x06005BC6 RID: 23494 RVA: 0x002853FA File Offset: 0x002843FA
		public bool StylisticSet4
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet4Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet4Property, value);
			}
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x06005BC7 RID: 23495 RVA: 0x0028540D File Offset: 0x0028440D
		// (set) Token: 0x06005BC8 RID: 23496 RVA: 0x00285424 File Offset: 0x00284424
		public bool StylisticSet5
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet5Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet5Property, value);
			}
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x06005BC9 RID: 23497 RVA: 0x00285437 File Offset: 0x00284437
		// (set) Token: 0x06005BCA RID: 23498 RVA: 0x0028544E File Offset: 0x0028444E
		public bool StylisticSet6
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet6Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet6Property, value);
			}
		}

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x06005BCB RID: 23499 RVA: 0x00285461 File Offset: 0x00284461
		// (set) Token: 0x06005BCC RID: 23500 RVA: 0x00285478 File Offset: 0x00284478
		public bool StylisticSet7
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet7Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet7Property, value);
			}
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x06005BCD RID: 23501 RVA: 0x0028548B File Offset: 0x0028448B
		// (set) Token: 0x06005BCE RID: 23502 RVA: 0x002854A2 File Offset: 0x002844A2
		public bool StylisticSet8
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet8Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet8Property, value);
			}
		}

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06005BCF RID: 23503 RVA: 0x002854B5 File Offset: 0x002844B5
		// (set) Token: 0x06005BD0 RID: 23504 RVA: 0x002854CC File Offset: 0x002844CC
		public bool StylisticSet9
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet9Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet9Property, value);
			}
		}

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x002854DF File Offset: 0x002844DF
		// (set) Token: 0x06005BD2 RID: 23506 RVA: 0x002854F6 File Offset: 0x002844F6
		public bool StylisticSet10
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet10Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet10Property, value);
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x00285509 File Offset: 0x00284509
		// (set) Token: 0x06005BD4 RID: 23508 RVA: 0x00285520 File Offset: 0x00284520
		public bool StylisticSet11
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet11Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet11Property, value);
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x00285533 File Offset: 0x00284533
		// (set) Token: 0x06005BD6 RID: 23510 RVA: 0x0028554A File Offset: 0x0028454A
		public bool StylisticSet12
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet12Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet12Property, value);
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x0028555D File Offset: 0x0028455D
		// (set) Token: 0x06005BD8 RID: 23512 RVA: 0x00285574 File Offset: 0x00284574
		public bool StylisticSet13
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet13Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet13Property, value);
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x00285587 File Offset: 0x00284587
		// (set) Token: 0x06005BDA RID: 23514 RVA: 0x0028559E File Offset: 0x0028459E
		public bool StylisticSet14
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet14Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet14Property, value);
			}
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x06005BDB RID: 23515 RVA: 0x002855B1 File Offset: 0x002845B1
		// (set) Token: 0x06005BDC RID: 23516 RVA: 0x002855C8 File Offset: 0x002845C8
		public bool StylisticSet15
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet15Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet15Property, value);
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x06005BDD RID: 23517 RVA: 0x002855DB File Offset: 0x002845DB
		// (set) Token: 0x06005BDE RID: 23518 RVA: 0x002855F2 File Offset: 0x002845F2
		public bool StylisticSet16
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet16Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet16Property, value);
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x06005BDF RID: 23519 RVA: 0x00285605 File Offset: 0x00284605
		// (set) Token: 0x06005BE0 RID: 23520 RVA: 0x0028561C File Offset: 0x0028461C
		public bool StylisticSet17
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet17Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet17Property, value);
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06005BE1 RID: 23521 RVA: 0x0028562F File Offset: 0x0028462F
		// (set) Token: 0x06005BE2 RID: 23522 RVA: 0x00285646 File Offset: 0x00284646
		public bool StylisticSet18
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet18Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet18Property, value);
			}
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x06005BE3 RID: 23523 RVA: 0x00285659 File Offset: 0x00284659
		// (set) Token: 0x06005BE4 RID: 23524 RVA: 0x00285670 File Offset: 0x00284670
		public bool StylisticSet19
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet19Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet19Property, value);
			}
		}

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x06005BE5 RID: 23525 RVA: 0x00285683 File Offset: 0x00284683
		// (set) Token: 0x06005BE6 RID: 23526 RVA: 0x0028569A File Offset: 0x0028469A
		public bool StylisticSet20
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.StylisticSet20Property);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticSet20Property, value);
			}
		}

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06005BE7 RID: 23527 RVA: 0x002856AD File Offset: 0x002846AD
		// (set) Token: 0x06005BE8 RID: 23528 RVA: 0x002856C4 File Offset: 0x002846C4
		public FontFraction Fraction
		{
			get
			{
				return (FontFraction)this._owner.GetValue(Typography.FractionProperty);
			}
			set
			{
				this._owner.SetValue(Typography.FractionProperty, value);
			}
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x06005BE9 RID: 23529 RVA: 0x002856DC File Offset: 0x002846DC
		// (set) Token: 0x06005BEA RID: 23530 RVA: 0x002856F3 File Offset: 0x002846F3
		public bool SlashedZero
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.SlashedZeroProperty);
			}
			set
			{
				this._owner.SetValue(Typography.SlashedZeroProperty, value);
			}
		}

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x06005BEB RID: 23531 RVA: 0x00285706 File Offset: 0x00284706
		// (set) Token: 0x06005BEC RID: 23532 RVA: 0x0028571D File Offset: 0x0028471D
		public bool MathematicalGreek
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.MathematicalGreekProperty);
			}
			set
			{
				this._owner.SetValue(Typography.MathematicalGreekProperty, value);
			}
		}

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x00285730 File Offset: 0x00284730
		// (set) Token: 0x06005BEE RID: 23534 RVA: 0x00285747 File Offset: 0x00284747
		public bool EastAsianExpertForms
		{
			get
			{
				return (bool)this._owner.GetValue(Typography.EastAsianExpertFormsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.EastAsianExpertFormsProperty, value);
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06005BEF RID: 23535 RVA: 0x0028575A File Offset: 0x0028475A
		// (set) Token: 0x06005BF0 RID: 23536 RVA: 0x00285771 File Offset: 0x00284771
		public FontVariants Variants
		{
			get
			{
				return (FontVariants)this._owner.GetValue(Typography.VariantsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.VariantsProperty, value);
			}
		}

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06005BF1 RID: 23537 RVA: 0x00285789 File Offset: 0x00284789
		// (set) Token: 0x06005BF2 RID: 23538 RVA: 0x002857A0 File Offset: 0x002847A0
		public FontCapitals Capitals
		{
			get
			{
				return (FontCapitals)this._owner.GetValue(Typography.CapitalsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.CapitalsProperty, value);
			}
		}

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06005BF3 RID: 23539 RVA: 0x002857B8 File Offset: 0x002847B8
		// (set) Token: 0x06005BF4 RID: 23540 RVA: 0x002857CF File Offset: 0x002847CF
		public FontNumeralStyle NumeralStyle
		{
			get
			{
				return (FontNumeralStyle)this._owner.GetValue(Typography.NumeralStyleProperty);
			}
			set
			{
				this._owner.SetValue(Typography.NumeralStyleProperty, value);
			}
		}

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x06005BF5 RID: 23541 RVA: 0x002857E7 File Offset: 0x002847E7
		// (set) Token: 0x06005BF6 RID: 23542 RVA: 0x002857FE File Offset: 0x002847FE
		public FontNumeralAlignment NumeralAlignment
		{
			get
			{
				return (FontNumeralAlignment)this._owner.GetValue(Typography.NumeralAlignmentProperty);
			}
			set
			{
				this._owner.SetValue(Typography.NumeralAlignmentProperty, value);
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x00285816 File Offset: 0x00284816
		// (set) Token: 0x06005BF8 RID: 23544 RVA: 0x0028582D File Offset: 0x0028482D
		public FontEastAsianWidths EastAsianWidths
		{
			get
			{
				return (FontEastAsianWidths)this._owner.GetValue(Typography.EastAsianWidthsProperty);
			}
			set
			{
				this._owner.SetValue(Typography.EastAsianWidthsProperty, value);
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x00285845 File Offset: 0x00284845
		// (set) Token: 0x06005BFA RID: 23546 RVA: 0x0028585C File Offset: 0x0028485C
		public FontEastAsianLanguage EastAsianLanguage
		{
			get
			{
				return (FontEastAsianLanguage)this._owner.GetValue(Typography.EastAsianLanguageProperty);
			}
			set
			{
				this._owner.SetValue(Typography.EastAsianLanguageProperty, value);
			}
		}

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x00285874 File Offset: 0x00284874
		// (set) Token: 0x06005BFC RID: 23548 RVA: 0x0028588B File Offset: 0x0028488B
		public int StandardSwashes
		{
			get
			{
				return (int)this._owner.GetValue(Typography.StandardSwashesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.StandardSwashesProperty, value);
			}
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x06005BFD RID: 23549 RVA: 0x002858A3 File Offset: 0x002848A3
		// (set) Token: 0x06005BFE RID: 23550 RVA: 0x002858BA File Offset: 0x002848BA
		public int ContextualSwashes
		{
			get
			{
				return (int)this._owner.GetValue(Typography.ContextualSwashesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.ContextualSwashesProperty, value);
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x06005BFF RID: 23551 RVA: 0x002858D2 File Offset: 0x002848D2
		// (set) Token: 0x06005C00 RID: 23552 RVA: 0x002858E9 File Offset: 0x002848E9
		public int StylisticAlternates
		{
			get
			{
				return (int)this._owner.GetValue(Typography.StylisticAlternatesProperty);
			}
			set
			{
				this._owner.SetValue(Typography.StylisticAlternatesProperty, value);
			}
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x00285901 File Offset: 0x00284901
		public static void SetStandardLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StandardLigaturesProperty, value);
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x0028591D File Offset: 0x0028491D
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStandardLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StandardLigaturesProperty);
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x0028593D File Offset: 0x0028493D
		public static void SetContextualLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.ContextualLigaturesProperty, value);
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x00285959 File Offset: 0x00284959
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetContextualLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.ContextualLigaturesProperty);
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x00285979 File Offset: 0x00284979
		public static void SetDiscretionaryLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.DiscretionaryLigaturesProperty, value);
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x00285995 File Offset: 0x00284995
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetDiscretionaryLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.DiscretionaryLigaturesProperty);
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x002859B5 File Offset: 0x002849B5
		public static void SetHistoricalLigatures(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.HistoricalLigaturesProperty, value);
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x002859D1 File Offset: 0x002849D1
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHistoricalLigatures(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.HistoricalLigaturesProperty);
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x002859F1 File Offset: 0x002849F1
		public static void SetAnnotationAlternates(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.AnnotationAlternatesProperty, value);
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x00285A12 File Offset: 0x00284A12
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetAnnotationAlternates(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.AnnotationAlternatesProperty);
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x00285A32 File Offset: 0x00284A32
		public static void SetContextualAlternates(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.ContextualAlternatesProperty, value);
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x00285A4E File Offset: 0x00284A4E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetContextualAlternates(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.ContextualAlternatesProperty);
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x00285A6E File Offset: 0x00284A6E
		public static void SetHistoricalForms(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.HistoricalFormsProperty, value);
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x00285A8A File Offset: 0x00284A8A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetHistoricalForms(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.HistoricalFormsProperty);
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x00285AAA File Offset: 0x00284AAA
		public static void SetKerning(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.KerningProperty, value);
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x00285AC6 File Offset: 0x00284AC6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetKerning(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.KerningProperty);
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x00285AE6 File Offset: 0x00284AE6
		public static void SetCapitalSpacing(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.CapitalSpacingProperty, value);
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x00285B02 File Offset: 0x00284B02
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetCapitalSpacing(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.CapitalSpacingProperty);
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x00285B22 File Offset: 0x00284B22
		public static void SetCaseSensitiveForms(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.CaseSensitiveFormsProperty, value);
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x00285B3E File Offset: 0x00284B3E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetCaseSensitiveForms(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.CaseSensitiveFormsProperty);
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x00285B5E File Offset: 0x00284B5E
		public static void SetStylisticSet1(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet1Property, value);
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x00285B7A File Offset: 0x00284B7A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet1(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet1Property);
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x00285B9A File Offset: 0x00284B9A
		public static void SetStylisticSet2(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet2Property, value);
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x00285BB6 File Offset: 0x00284BB6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet2(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet2Property);
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x00285BD6 File Offset: 0x00284BD6
		public static void SetStylisticSet3(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet3Property, value);
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x00285BF2 File Offset: 0x00284BF2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet3(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet3Property);
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x00285C12 File Offset: 0x00284C12
		public static void SetStylisticSet4(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet4Property, value);
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x00285C2E File Offset: 0x00284C2E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet4(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet4Property);
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x00285C4E File Offset: 0x00284C4E
		public static void SetStylisticSet5(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet5Property, value);
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x00285C6A File Offset: 0x00284C6A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet5(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet5Property);
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x00285C8A File Offset: 0x00284C8A
		public static void SetStylisticSet6(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet6Property, value);
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x00285CA6 File Offset: 0x00284CA6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet6(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet6Property);
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x00285CC6 File Offset: 0x00284CC6
		public static void SetStylisticSet7(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet7Property, value);
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x00285CE2 File Offset: 0x00284CE2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet7(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet7Property);
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x00285D02 File Offset: 0x00284D02
		public static void SetStylisticSet8(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet8Property, value);
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x00285D1E File Offset: 0x00284D1E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet8(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet8Property);
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x00285D3E File Offset: 0x00284D3E
		public static void SetStylisticSet9(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet9Property, value);
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x00285D5A File Offset: 0x00284D5A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet9(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet9Property);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x00285D7A File Offset: 0x00284D7A
		public static void SetStylisticSet10(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet10Property, value);
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x00285D96 File Offset: 0x00284D96
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet10(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet10Property);
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x00285DB6 File Offset: 0x00284DB6
		public static void SetStylisticSet11(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet11Property, value);
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x00285DD2 File Offset: 0x00284DD2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet11(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet11Property);
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x00285DF2 File Offset: 0x00284DF2
		public static void SetStylisticSet12(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet12Property, value);
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x00285E0E File Offset: 0x00284E0E
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet12(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet12Property);
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x00285E2E File Offset: 0x00284E2E
		public static void SetStylisticSet13(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet13Property, value);
		}

		// Token: 0x06005C2E RID: 23598 RVA: 0x00285E4A File Offset: 0x00284E4A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet13(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet13Property);
		}

		// Token: 0x06005C2F RID: 23599 RVA: 0x00285E6A File Offset: 0x00284E6A
		public static void SetStylisticSet14(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet14Property, value);
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x00285E86 File Offset: 0x00284E86
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet14(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet14Property);
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x00285EA6 File Offset: 0x00284EA6
		public static void SetStylisticSet15(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet15Property, value);
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x00285EC2 File Offset: 0x00284EC2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet15(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet15Property);
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x00285EE2 File Offset: 0x00284EE2
		public static void SetStylisticSet16(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet16Property, value);
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x00285EFE File Offset: 0x00284EFE
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet16(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet16Property);
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x00285F1E File Offset: 0x00284F1E
		public static void SetStylisticSet17(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet17Property, value);
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x00285F3A File Offset: 0x00284F3A
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet17(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet17Property);
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x00285F5A File Offset: 0x00284F5A
		public static void SetStylisticSet18(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet18Property, value);
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x00285F76 File Offset: 0x00284F76
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet18(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet18Property);
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x00285F96 File Offset: 0x00284F96
		public static void SetStylisticSet19(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet19Property, value);
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x00285FB2 File Offset: 0x00284FB2
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet19(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet19Property);
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x00285FD2 File Offset: 0x00284FD2
		public static void SetStylisticSet20(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticSet20Property, value);
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x00285FEE File Offset: 0x00284FEE
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetStylisticSet20(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.StylisticSet20Property);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x0028600E File Offset: 0x0028500E
		public static void SetFraction(DependencyObject element, FontFraction value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.FractionProperty, value);
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x0028602F File Offset: 0x0028502F
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontFraction GetFraction(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontFraction)element.GetValue(Typography.FractionProperty);
		}

		// Token: 0x06005C3F RID: 23615 RVA: 0x0028604F File Offset: 0x0028504F
		public static void SetSlashedZero(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.SlashedZeroProperty, value);
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x0028606B File Offset: 0x0028506B
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetSlashedZero(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.SlashedZeroProperty);
		}

		// Token: 0x06005C41 RID: 23617 RVA: 0x0028608B File Offset: 0x0028508B
		public static void SetMathematicalGreek(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.MathematicalGreekProperty, value);
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x002860A7 File Offset: 0x002850A7
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetMathematicalGreek(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.MathematicalGreekProperty);
		}

		// Token: 0x06005C43 RID: 23619 RVA: 0x002860C7 File Offset: 0x002850C7
		public static void SetEastAsianExpertForms(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.EastAsianExpertFormsProperty, value);
		}

		// Token: 0x06005C44 RID: 23620 RVA: 0x002860E3 File Offset: 0x002850E3
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetEastAsianExpertForms(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Typography.EastAsianExpertFormsProperty);
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x00286103 File Offset: 0x00285103
		public static void SetVariants(DependencyObject element, FontVariants value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.VariantsProperty, value);
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x00286124 File Offset: 0x00285124
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontVariants GetVariants(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontVariants)element.GetValue(Typography.VariantsProperty);
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x00286144 File Offset: 0x00285144
		public static void SetCapitals(DependencyObject element, FontCapitals value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.CapitalsProperty, value);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x00286165 File Offset: 0x00285165
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontCapitals GetCapitals(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontCapitals)element.GetValue(Typography.CapitalsProperty);
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x00286185 File Offset: 0x00285185
		public static void SetNumeralStyle(DependencyObject element, FontNumeralStyle value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.NumeralStyleProperty, value);
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x002861A6 File Offset: 0x002851A6
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontNumeralStyle GetNumeralStyle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontNumeralStyle)element.GetValue(Typography.NumeralStyleProperty);
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x002861C6 File Offset: 0x002851C6
		public static void SetNumeralAlignment(DependencyObject element, FontNumeralAlignment value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.NumeralAlignmentProperty, value);
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x002861E7 File Offset: 0x002851E7
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontNumeralAlignment GetNumeralAlignment(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontNumeralAlignment)element.GetValue(Typography.NumeralAlignmentProperty);
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x00286207 File Offset: 0x00285207
		public static void SetEastAsianWidths(DependencyObject element, FontEastAsianWidths value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.EastAsianWidthsProperty, value);
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x00286228 File Offset: 0x00285228
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontEastAsianWidths GetEastAsianWidths(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontEastAsianWidths)element.GetValue(Typography.EastAsianWidthsProperty);
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x00286248 File Offset: 0x00285248
		public static void SetEastAsianLanguage(DependencyObject element, FontEastAsianLanguage value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.EastAsianLanguageProperty, value);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x00286269 File Offset: 0x00285269
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static FontEastAsianLanguage GetEastAsianLanguage(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontEastAsianLanguage)element.GetValue(Typography.EastAsianLanguageProperty);
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x00286289 File Offset: 0x00285289
		public static void SetStandardSwashes(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StandardSwashesProperty, value);
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x002862AA File Offset: 0x002852AA
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetStandardSwashes(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.StandardSwashesProperty);
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x002862CA File Offset: 0x002852CA
		public static void SetContextualSwashes(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.ContextualSwashesProperty, value);
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x002862EB File Offset: 0x002852EB
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetContextualSwashes(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.ContextualSwashesProperty);
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x0028630B File Offset: 0x0028530B
		public static void SetStylisticAlternates(DependencyObject element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Typography.StylisticAlternatesProperty, value);
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x0028632C File Offset: 0x0028532C
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetStylisticAlternates(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Typography.StylisticAlternatesProperty);
		}

		// Token: 0x04003097 RID: 12439
		private static readonly Type _typeofThis = typeof(Typography);

		// Token: 0x04003098 RID: 12440
		private static readonly Type _typeofBool = typeof(bool);

		// Token: 0x04003099 RID: 12441
		public static readonly DependencyProperty StandardLigaturesProperty = DependencyProperty.RegisterAttached("StandardLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400309A RID: 12442
		public static readonly DependencyProperty ContextualLigaturesProperty = DependencyProperty.RegisterAttached("ContextualLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400309B RID: 12443
		public static readonly DependencyProperty DiscretionaryLigaturesProperty = DependencyProperty.RegisterAttached("DiscretionaryLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400309C RID: 12444
		public static readonly DependencyProperty HistoricalLigaturesProperty = DependencyProperty.RegisterAttached("HistoricalLigatures", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400309D RID: 12445
		public static readonly DependencyProperty AnnotationAlternatesProperty = DependencyProperty.RegisterAttached("AnnotationAlternates", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400309E RID: 12446
		public static readonly DependencyProperty ContextualAlternatesProperty = DependencyProperty.RegisterAttached("ContextualAlternates", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400309F RID: 12447
		public static readonly DependencyProperty HistoricalFormsProperty = DependencyProperty.RegisterAttached("HistoricalForms", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A0 RID: 12448
		public static readonly DependencyProperty KerningProperty = DependencyProperty.RegisterAttached("Kerning", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A1 RID: 12449
		public static readonly DependencyProperty CapitalSpacingProperty = DependencyProperty.RegisterAttached("CapitalSpacing", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A2 RID: 12450
		public static readonly DependencyProperty CaseSensitiveFormsProperty = DependencyProperty.RegisterAttached("CaseSensitiveForms", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A3 RID: 12451
		public static readonly DependencyProperty StylisticSet1Property = DependencyProperty.RegisterAttached("StylisticSet1", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A4 RID: 12452
		public static readonly DependencyProperty StylisticSet2Property = DependencyProperty.RegisterAttached("StylisticSet2", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A5 RID: 12453
		public static readonly DependencyProperty StylisticSet3Property = DependencyProperty.RegisterAttached("StylisticSet3", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A6 RID: 12454
		public static readonly DependencyProperty StylisticSet4Property = DependencyProperty.RegisterAttached("StylisticSet4", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A7 RID: 12455
		public static readonly DependencyProperty StylisticSet5Property = DependencyProperty.RegisterAttached("StylisticSet5", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A8 RID: 12456
		public static readonly DependencyProperty StylisticSet6Property = DependencyProperty.RegisterAttached("StylisticSet6", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030A9 RID: 12457
		public static readonly DependencyProperty StylisticSet7Property = DependencyProperty.RegisterAttached("StylisticSet7", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030AA RID: 12458
		public static readonly DependencyProperty StylisticSet8Property = DependencyProperty.RegisterAttached("StylisticSet8", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030AB RID: 12459
		public static readonly DependencyProperty StylisticSet9Property = DependencyProperty.RegisterAttached("StylisticSet9", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030AC RID: 12460
		public static readonly DependencyProperty StylisticSet10Property = DependencyProperty.RegisterAttached("StylisticSet10", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030AD RID: 12461
		public static readonly DependencyProperty StylisticSet11Property = DependencyProperty.RegisterAttached("StylisticSet11", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030AE RID: 12462
		public static readonly DependencyProperty StylisticSet12Property = DependencyProperty.RegisterAttached("StylisticSet12", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030AF RID: 12463
		public static readonly DependencyProperty StylisticSet13Property = DependencyProperty.RegisterAttached("StylisticSet13", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B0 RID: 12464
		public static readonly DependencyProperty StylisticSet14Property = DependencyProperty.RegisterAttached("StylisticSet14", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B1 RID: 12465
		public static readonly DependencyProperty StylisticSet15Property = DependencyProperty.RegisterAttached("StylisticSet15", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B2 RID: 12466
		public static readonly DependencyProperty StylisticSet16Property = DependencyProperty.RegisterAttached("StylisticSet16", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B3 RID: 12467
		public static readonly DependencyProperty StylisticSet17Property = DependencyProperty.RegisterAttached("StylisticSet17", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B4 RID: 12468
		public static readonly DependencyProperty StylisticSet18Property = DependencyProperty.RegisterAttached("StylisticSet18", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B5 RID: 12469
		public static readonly DependencyProperty StylisticSet19Property = DependencyProperty.RegisterAttached("StylisticSet19", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B6 RID: 12470
		public static readonly DependencyProperty StylisticSet20Property = DependencyProperty.RegisterAttached("StylisticSet20", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B7 RID: 12471
		public static readonly DependencyProperty FractionProperty = DependencyProperty.RegisterAttached("Fraction", typeof(FontFraction), Typography._typeofThis, new FrameworkPropertyMetadata(FontFraction.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B8 RID: 12472
		public static readonly DependencyProperty SlashedZeroProperty = DependencyProperty.RegisterAttached("SlashedZero", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030B9 RID: 12473
		public static readonly DependencyProperty MathematicalGreekProperty = DependencyProperty.RegisterAttached("MathematicalGreek", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030BA RID: 12474
		public static readonly DependencyProperty EastAsianExpertFormsProperty = DependencyProperty.RegisterAttached("EastAsianExpertForms", Typography._typeofBool, Typography._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030BB RID: 12475
		public static readonly DependencyProperty VariantsProperty = DependencyProperty.RegisterAttached("Variants", typeof(FontVariants), Typography._typeofThis, new FrameworkPropertyMetadata(FontVariants.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030BC RID: 12476
		public static readonly DependencyProperty CapitalsProperty = DependencyProperty.RegisterAttached("Capitals", typeof(FontCapitals), Typography._typeofThis, new FrameworkPropertyMetadata(FontCapitals.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030BD RID: 12477
		public static readonly DependencyProperty NumeralStyleProperty = DependencyProperty.RegisterAttached("NumeralStyle", typeof(FontNumeralStyle), Typography._typeofThis, new FrameworkPropertyMetadata(FontNumeralStyle.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030BE RID: 12478
		public static readonly DependencyProperty NumeralAlignmentProperty = DependencyProperty.RegisterAttached("NumeralAlignment", typeof(FontNumeralAlignment), Typography._typeofThis, new FrameworkPropertyMetadata(FontNumeralAlignment.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030BF RID: 12479
		public static readonly DependencyProperty EastAsianWidthsProperty = DependencyProperty.RegisterAttached("EastAsianWidths", typeof(FontEastAsianWidths), Typography._typeofThis, new FrameworkPropertyMetadata(FontEastAsianWidths.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030C0 RID: 12480
		public static readonly DependencyProperty EastAsianLanguageProperty = DependencyProperty.RegisterAttached("EastAsianLanguage", typeof(FontEastAsianLanguage), Typography._typeofThis, new FrameworkPropertyMetadata(FontEastAsianLanguage.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030C1 RID: 12481
		public static readonly DependencyProperty StandardSwashesProperty = DependencyProperty.RegisterAttached("StandardSwashes", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030C2 RID: 12482
		public static readonly DependencyProperty ContextualSwashesProperty = DependencyProperty.RegisterAttached("ContextualSwashes", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030C3 RID: 12483
		public static readonly DependencyProperty StylisticAlternatesProperty = DependencyProperty.RegisterAttached("StylisticAlternates", typeof(int), Typography._typeofThis, new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040030C4 RID: 12484
		internal static DependencyProperty[] TypographyPropertiesList = new DependencyProperty[]
		{
			Typography.StandardLigaturesProperty,
			Typography.ContextualLigaturesProperty,
			Typography.DiscretionaryLigaturesProperty,
			Typography.HistoricalLigaturesProperty,
			Typography.AnnotationAlternatesProperty,
			Typography.ContextualAlternatesProperty,
			Typography.HistoricalFormsProperty,
			Typography.KerningProperty,
			Typography.CapitalSpacingProperty,
			Typography.CaseSensitiveFormsProperty,
			Typography.StylisticSet1Property,
			Typography.StylisticSet2Property,
			Typography.StylisticSet3Property,
			Typography.StylisticSet4Property,
			Typography.StylisticSet5Property,
			Typography.StylisticSet6Property,
			Typography.StylisticSet7Property,
			Typography.StylisticSet8Property,
			Typography.StylisticSet9Property,
			Typography.StylisticSet10Property,
			Typography.StylisticSet11Property,
			Typography.StylisticSet12Property,
			Typography.StylisticSet13Property,
			Typography.StylisticSet14Property,
			Typography.StylisticSet15Property,
			Typography.StylisticSet16Property,
			Typography.StylisticSet17Property,
			Typography.StylisticSet18Property,
			Typography.StylisticSet19Property,
			Typography.StylisticSet20Property,
			Typography.FractionProperty,
			Typography.SlashedZeroProperty,
			Typography.MathematicalGreekProperty,
			Typography.EastAsianExpertFormsProperty,
			Typography.VariantsProperty,
			Typography.CapitalsProperty,
			Typography.NumeralStyleProperty,
			Typography.NumeralAlignmentProperty,
			Typography.EastAsianWidthsProperty,
			Typography.EastAsianLanguageProperty,
			Typography.StandardSwashesProperty,
			Typography.ContextualSwashesProperty,
			Typography.StylisticAlternatesProperty
		};

		// Token: 0x040030C5 RID: 12485
		internal static readonly TypographyProperties Default = new TypographyProperties();

		// Token: 0x040030C6 RID: 12486
		private DependencyObject _owner;
	}
}
