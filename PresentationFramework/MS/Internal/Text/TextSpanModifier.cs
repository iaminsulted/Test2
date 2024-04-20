using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000329 RID: 809
	internal class TextSpanModifier : TextModifier
	{
		// Token: 0x06001E35 RID: 7733 RVA: 0x0016F724 File Offset: 0x0016E724
		public TextSpanModifier(int length, TextDecorationCollection textDecorations, Brush foregroundBrush)
		{
			this._length = length;
			this._modifierDecorations = textDecorations;
			this._modifierBrush = foregroundBrush;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0016F741 File Offset: 0x0016E741
		public TextSpanModifier(int length, TextDecorationCollection textDecorations, Brush foregroundBrush, FlowDirection flowDirection) : this(length, textDecorations, foregroundBrush)
		{
			this._hasDirectionalEmbedding = true;
			this._flowDirection = flowDirection;
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x0016F75B File Offset: 0x0016E75B
		public sealed override int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x00109403 File Offset: 0x00108403
		public sealed override TextRunProperties Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0016F764 File Offset: 0x0016E764
		public sealed override TextRunProperties ModifyProperties(TextRunProperties properties)
		{
			if (properties == null || this._modifierDecorations == null || this._modifierDecorations.Count == 0)
			{
				return properties;
			}
			Brush brush = this._modifierBrush;
			if (brush == properties.ForegroundBrush)
			{
				brush = null;
			}
			TextDecorationCollection textDecorations = properties.TextDecorations;
			TextDecorationCollection textDecorationCollection;
			if (textDecorations == null || textDecorations.Count == 0)
			{
				if (brush == null)
				{
					textDecorationCollection = this._modifierDecorations;
				}
				else
				{
					textDecorationCollection = this.CopyTextDecorations(this._modifierDecorations, brush);
				}
			}
			else
			{
				textDecorationCollection = this.CopyTextDecorations(this._modifierDecorations, brush);
				foreach (TextDecoration value in textDecorations)
				{
					textDecorationCollection.Add(value);
				}
			}
			return new TextSpanModifier.MergedTextRunProperties(properties, textDecorationCollection);
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0016F824 File Offset: 0x0016E824
		public override bool HasDirectionalEmbedding
		{
			get
			{
				return this._hasDirectionalEmbedding;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0016F82C File Offset: 0x0016E82C
		public override FlowDirection FlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0016F834 File Offset: 0x0016E834
		private TextDecorationCollection CopyTextDecorations(TextDecorationCollection textDecorations, Brush brush)
		{
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			Pen pen = null;
			foreach (TextDecoration textDecoration in textDecorations)
			{
				if (textDecoration.Pen == null && brush != null)
				{
					if (pen == null)
					{
						pen = new Pen(brush, 1.0);
					}
					TextDecoration textDecoration2 = textDecoration.Clone();
					textDecoration2.Pen = pen;
					textDecorationCollection.Add(textDecoration2);
				}
				else
				{
					textDecorationCollection.Add(textDecoration);
				}
			}
			return textDecorationCollection;
		}

		// Token: 0x04000EFE RID: 3838
		private int _length;

		// Token: 0x04000EFF RID: 3839
		private TextDecorationCollection _modifierDecorations;

		// Token: 0x04000F00 RID: 3840
		private Brush _modifierBrush;

		// Token: 0x04000F01 RID: 3841
		private FlowDirection _flowDirection;

		// Token: 0x04000F02 RID: 3842
		private bool _hasDirectionalEmbedding;

		// Token: 0x02000A6F RID: 2671
		private class MergedTextRunProperties : TextRunProperties
		{
			// Token: 0x06008637 RID: 34359 RVA: 0x0032A30E File Offset: 0x0032930E
			internal MergedTextRunProperties(TextRunProperties runProperties, TextDecorationCollection textDecorations)
			{
				this._runProperties = runProperties;
				this._textDecorations = textDecorations;
				base.PixelsPerDip = this._runProperties.PixelsPerDip;
			}

			// Token: 0x17001E0B RID: 7691
			// (get) Token: 0x06008638 RID: 34360 RVA: 0x0032A335 File Offset: 0x00329335
			public override Typeface Typeface
			{
				get
				{
					return this._runProperties.Typeface;
				}
			}

			// Token: 0x17001E0C RID: 7692
			// (get) Token: 0x06008639 RID: 34361 RVA: 0x0032A342 File Offset: 0x00329342
			public override double FontRenderingEmSize
			{
				get
				{
					return this._runProperties.FontRenderingEmSize;
				}
			}

			// Token: 0x17001E0D RID: 7693
			// (get) Token: 0x0600863A RID: 34362 RVA: 0x0032A34F File Offset: 0x0032934F
			public override double FontHintingEmSize
			{
				get
				{
					return this._runProperties.FontHintingEmSize;
				}
			}

			// Token: 0x17001E0E RID: 7694
			// (get) Token: 0x0600863B RID: 34363 RVA: 0x0032A35C File Offset: 0x0032935C
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._textDecorations;
				}
			}

			// Token: 0x17001E0F RID: 7695
			// (get) Token: 0x0600863C RID: 34364 RVA: 0x0032A364 File Offset: 0x00329364
			public override Brush ForegroundBrush
			{
				get
				{
					return this._runProperties.ForegroundBrush;
				}
			}

			// Token: 0x17001E10 RID: 7696
			// (get) Token: 0x0600863D RID: 34365 RVA: 0x0032A371 File Offset: 0x00329371
			public override Brush BackgroundBrush
			{
				get
				{
					return this._runProperties.BackgroundBrush;
				}
			}

			// Token: 0x17001E11 RID: 7697
			// (get) Token: 0x0600863E RID: 34366 RVA: 0x0032A37E File Offset: 0x0032937E
			public override CultureInfo CultureInfo
			{
				get
				{
					return this._runProperties.CultureInfo;
				}
			}

			// Token: 0x17001E12 RID: 7698
			// (get) Token: 0x0600863F RID: 34367 RVA: 0x0032A38B File Offset: 0x0032938B
			public override TextEffectCollection TextEffects
			{
				get
				{
					return this._runProperties.TextEffects;
				}
			}

			// Token: 0x17001E13 RID: 7699
			// (get) Token: 0x06008640 RID: 34368 RVA: 0x0032A398 File Offset: 0x00329398
			public override BaselineAlignment BaselineAlignment
			{
				get
				{
					return this._runProperties.BaselineAlignment;
				}
			}

			// Token: 0x17001E14 RID: 7700
			// (get) Token: 0x06008641 RID: 34369 RVA: 0x0032A3A5 File Offset: 0x003293A5
			public override TextRunTypographyProperties TypographyProperties
			{
				get
				{
					return this._runProperties.TypographyProperties;
				}
			}

			// Token: 0x17001E15 RID: 7701
			// (get) Token: 0x06008642 RID: 34370 RVA: 0x0032A3B2 File Offset: 0x003293B2
			public override NumberSubstitution NumberSubstitution
			{
				get
				{
					return this._runProperties.NumberSubstitution;
				}
			}

			// Token: 0x0400413E RID: 16702
			private TextRunProperties _runProperties;

			// Token: 0x0400413F RID: 16703
			private TextDecorationCollection _textDecorations;
		}
	}
}
