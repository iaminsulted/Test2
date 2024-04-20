using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	// Token: 0x02000620 RID: 1568
	public sealed class Glyphs : FrameworkElement, IUriContext
	{
		// Token: 0x06004D38 RID: 19768 RVA: 0x0023ECCE File Offset: 0x0023DCCE
		public GlyphRun ToGlyphRun()
		{
			this.ComputeMeasurementGlyphRunAndOrigin();
			if (this._measurementGlyphRun == null)
			{
				return null;
			}
			return this._measurementGlyphRun;
		}

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x06004D39 RID: 19769 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06004D3A RID: 19770 RVA: 0x0022A4F5 File Offset: 0x002294F5
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x0023ECE8 File Offset: 0x0023DCE8
		protected override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			Rect rect;
			if (this._measurementGlyphRun != null)
			{
				rect = this._measurementGlyphRun.ComputeInkBoundingBox();
			}
			else
			{
				rect = Rect.Empty;
			}
			if (!rect.IsEmpty)
			{
				rect.X += this._glyphRunOrigin.X;
				rect.Y += this._glyphRunOrigin.Y;
			}
			return finalSize;
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x0023ED58 File Offset: 0x0023DD58
		protected override void OnRender(DrawingContext context)
		{
			if (this._glyphRunProperties == null || this._measurementGlyphRun == null)
			{
				return;
			}
			context.PushGuidelineY1(this._glyphRunOrigin.Y);
			try
			{
				context.DrawGlyphRun(this.Fill, this._measurementGlyphRun);
			}
			finally
			{
				context.Pop();
			}
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0023EDB4 File Offset: 0x0023DDB4
		protected override Size MeasureOverride(Size constraint)
		{
			this.ComputeMeasurementGlyphRunAndOrigin();
			if (this._measurementGlyphRun == null)
			{
				return default(Size);
			}
			Rect rect = this._measurementGlyphRun.ComputeAlignmentBox();
			rect.Offset(this._glyphRunOrigin.X, this._glyphRunOrigin.Y);
			return new Size(Math.Max(0.0, rect.Right), Math.Max(0.0, rect.Bottom));
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0023EE34 File Offset: 0x0023DE34
		private void ComputeMeasurementGlyphRunAndOrigin()
		{
			if (this._glyphRunProperties == null)
			{
				this._measurementGlyphRun = null;
				this.ParseGlyphRunProperties();
				if (this._glyphRunProperties == null)
				{
					return;
				}
			}
			else if (this._measurementGlyphRun != null)
			{
				return;
			}
			bool flag = (this.BidiLevel & 1) == 0;
			bool flag2 = !DoubleUtil.IsNaN(this.OriginX);
			bool flag3 = !DoubleUtil.IsNaN(this.OriginY);
			bool flag4 = false;
			Rect rect = default(Rect);
			if (flag2 && flag3 && flag)
			{
				this._measurementGlyphRun = this._glyphRunProperties.CreateGlyphRun(new Point(this.OriginX, this.OriginY), base.Language);
				flag4 = true;
			}
			else
			{
				this._measurementGlyphRun = this._glyphRunProperties.CreateGlyphRun(default(Point), base.Language);
				rect = this._measurementGlyphRun.ComputeAlignmentBox();
			}
			if (flag2)
			{
				this._glyphRunOrigin.X = this.OriginX;
			}
			else
			{
				this._glyphRunOrigin.X = (flag ? 0.0 : rect.Width);
			}
			if (flag3)
			{
				this._glyphRunOrigin.Y = this.OriginY;
			}
			else
			{
				this._glyphRunOrigin.Y = -rect.Y;
			}
			if (!flag4)
			{
				this._measurementGlyphRun = this._glyphRunProperties.CreateGlyphRun(this._glyphRunOrigin, base.Language);
			}
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0023EF7C File Offset: 0x0023DF7C
		private void ParseCaretStops(Glyphs.LayoutDependentGlyphRunProperties glyphRunProperties)
		{
			string caretStops = this.CaretStops;
			if (string.IsNullOrEmpty(caretStops))
			{
				glyphRunProperties.caretStops = null;
				return;
			}
			int num;
			if (!string.IsNullOrEmpty(glyphRunProperties.unicodeString))
			{
				num = glyphRunProperties.unicodeString.Length + 1;
			}
			else if (glyphRunProperties.clusterMap != null && glyphRunProperties.clusterMap.Length != 0)
			{
				num = glyphRunProperties.clusterMap.Length + 1;
			}
			else
			{
				num = glyphRunProperties.glyphIndices.Length + 1;
			}
			bool[] array = new bool[num];
			int i = 0;
			foreach (char c in caretStops)
			{
				if (!char.IsWhiteSpace(c))
				{
					int num2;
					if ('0' <= c && c <= '9')
					{
						num2 = (int)(c - '0');
					}
					else if ('a' <= c && c <= 'f')
					{
						num2 = (int)(c - 'a' + '\n');
					}
					else
					{
						if ('A' > c || c > 'F')
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsContainsHexDigits"), "CaretStops");
						}
						num2 = (int)(c - 'A' + '\n');
					}
					if ((num2 & 8) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
					if ((num2 & 4) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
					if ((num2 & 2) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
					if ((num2 & 1) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
				}
			}
			while (i < array.Length)
			{
				array[i++] = true;
			}
			glyphRunProperties.caretStops = array;
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0023F13C File Offset: 0x0023E13C
		private void ParseGlyphRunProperties()
		{
			Glyphs.LayoutDependentGlyphRunProperties layoutDependentGlyphRunProperties = null;
			Uri uri = this.FontUri;
			if (uri != null)
			{
				if (string.IsNullOrEmpty(this.UnicodeString) && string.IsNullOrEmpty(this.Indices))
				{
					throw new ArgumentException(SR.Get("GlyphsUnicodeStringAndIndicesCannotBothBeEmpty"));
				}
				layoutDependentGlyphRunProperties = new Glyphs.LayoutDependentGlyphRunProperties(base.GetDpi().PixelsPerDip);
				if (!uri.IsAbsoluteUri)
				{
					uri = BindUriHelper.GetResolvedUri(BaseUriHelper.GetBaseUri(this), uri);
				}
				layoutDependentGlyphRunProperties.glyphTypeface = new GlyphTypeface(uri, this.StyleSimulations);
				layoutDependentGlyphRunProperties.unicodeString = this.UnicodeString;
				layoutDependentGlyphRunProperties.sideways = this.IsSideways;
				layoutDependentGlyphRunProperties.deviceFontName = this.DeviceFontName;
				List<Glyphs.ParsedGlyphData> list;
				int num = this.ParseGlyphsProperty(layoutDependentGlyphRunProperties.glyphTypeface, layoutDependentGlyphRunProperties.unicodeString, layoutDependentGlyphRunProperties.sideways, out list, out layoutDependentGlyphRunProperties.clusterMap);
				layoutDependentGlyphRunProperties.glyphIndices = new ushort[num];
				layoutDependentGlyphRunProperties.advanceWidths = new double[num];
				this.ParseCaretStops(layoutDependentGlyphRunProperties);
				layoutDependentGlyphRunProperties.glyphOffsets = null;
				int num2 = 0;
				layoutDependentGlyphRunProperties.fontRenderingSize = this.FontRenderingEmSize;
				layoutDependentGlyphRunProperties.bidiLevel = this.BidiLevel;
				double num3 = layoutDependentGlyphRunProperties.fontRenderingSize / 100.0;
				foreach (Glyphs.ParsedGlyphData parsedGlyphData in list)
				{
					layoutDependentGlyphRunProperties.glyphIndices[num2] = parsedGlyphData.glyphIndex;
					layoutDependentGlyphRunProperties.advanceWidths[num2] = parsedGlyphData.advanceWidth * num3;
					if (parsedGlyphData.offsetX != 0.0 || parsedGlyphData.offsetY != 0.0)
					{
						if (layoutDependentGlyphRunProperties.glyphOffsets == null)
						{
							layoutDependentGlyphRunProperties.glyphOffsets = new Point[num];
						}
						layoutDependentGlyphRunProperties.glyphOffsets[num2].X = parsedGlyphData.offsetX * num3;
						layoutDependentGlyphRunProperties.glyphOffsets[num2].Y = parsedGlyphData.offsetY * num3;
					}
					num2++;
				}
			}
			this._glyphRunProperties = layoutDependentGlyphRunProperties;
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0023F340 File Offset: 0x0023E340
		private static bool IsEmpty(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0023F374 File Offset: 0x0023E374
		private bool ReadGlyphIndex(string valueSpec, ref bool inCluster, ref int glyphClusterSize, ref int characterClusterSize, ref ushort glyphIndex)
		{
			string s = valueSpec;
			int num = valueSpec.IndexOf('(');
			if (num != -1)
			{
				for (int i = 0; i < num; i++)
				{
					if (!char.IsWhiteSpace(valueSpec[i]))
					{
						throw new ArgumentException(SR.Get("GlyphsClusterBadCharactersBeforeBracket"));
					}
				}
				if (inCluster)
				{
					throw new ArgumentException(SR.Get("GlyphsClusterNoNestedClusters"));
				}
				int num2 = valueSpec.IndexOf(')');
				if (num2 == -1 || num2 <= num + 1)
				{
					throw new ArgumentException(SR.Get("GlyphsClusterNoMatchingBracket"));
				}
				int num3 = valueSpec.IndexOf(':');
				if (num3 == -1)
				{
					string s2 = valueSpec.Substring(num + 1, num2 - (num + 1));
					characterClusterSize = int.Parse(s2, CultureInfo.InvariantCulture);
					glyphClusterSize = 1;
				}
				else
				{
					if (num3 <= num + 1 || num3 >= num2 - 1)
					{
						throw new ArgumentException(SR.Get("GlyphsClusterMisplacedSeparator"));
					}
					string s3 = valueSpec.Substring(num + 1, num3 - (num + 1));
					characterClusterSize = int.Parse(s3, CultureInfo.InvariantCulture);
					string s4 = valueSpec.Substring(num3 + 1, num2 - (num3 + 1));
					glyphClusterSize = int.Parse(s4, CultureInfo.InvariantCulture);
				}
				inCluster = true;
				s = valueSpec.Substring(num2 + 1);
			}
			if (Glyphs.IsEmpty(s))
			{
				return false;
			}
			glyphIndex = ushort.Parse(s, CultureInfo.InvariantCulture);
			return true;
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0023F4A9 File Offset: 0x0023E4A9
		private static double GetAdvanceWidth(GlyphTypeface glyphTypeface, ushort glyphIndex, bool sideways)
		{
			return (sideways ? glyphTypeface.AdvanceHeights[glyphIndex] : glyphTypeface.AdvanceWidths[glyphIndex]) * 100.0;
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0023F4D4 File Offset: 0x0023E4D4
		private ushort GetGlyphFromCharacter(GlyphTypeface glyphTypeface, char character)
		{
			ushort result;
			glyphTypeface.CharacterToGlyphMap.TryGetValue((int)character, out result);
			return result;
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0023F4F1 File Offset: 0x0023E4F1
		private static void SetClusterMapEntry(ushort[] clusterMap, int index, ushort value)
		{
			if (index < 0 || index >= clusterMap.Length)
			{
				throw new ArgumentException(SR.Get("GlyphsUnicodeStringIsTooShort"));
			}
			clusterMap[index] = value;
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0023F514 File Offset: 0x0023E514
		private int ParseGlyphsProperty(GlyphTypeface fontFace, string unicodeString, bool sideways, out List<Glyphs.ParsedGlyphData> parsedGlyphs, out ushort[] clusterMap)
		{
			string indices = this.Indices;
			int num = 0;
			int i = 0;
			int num2 = 1;
			int num3 = 1;
			bool flag = false;
			int num4;
			if (!string.IsNullOrEmpty(unicodeString))
			{
				clusterMap = new ushort[unicodeString.Length];
				num4 = unicodeString.Length;
			}
			else
			{
				clusterMap = null;
				num4 = 8;
			}
			if (!string.IsNullOrEmpty(indices))
			{
				num4 = Math.Max(num4, indices.Length / 5);
			}
			parsedGlyphs = new List<Glyphs.ParsedGlyphData>(num4);
			Glyphs.ParsedGlyphData parsedGlyphData = new Glyphs.ParsedGlyphData();
			if (!string.IsNullOrEmpty(indices))
			{
				int num5 = 0;
				int num6 = 0;
				for (int j = 0; j <= indices.Length; j++)
				{
					char c = (j < indices.Length) ? indices[j] : '\0';
					if (c == ',' || c == ';' || j == indices.Length)
					{
						int length = j - num6;
						string text = indices.Substring(num6, length);
						switch (num5)
						{
						case 0:
						{
							bool flag2 = flag;
							if (!this.ReadGlyphIndex(text, ref flag, ref num3, ref num2, ref parsedGlyphData.glyphIndex))
							{
								if (string.IsNullOrEmpty(unicodeString))
								{
									throw new ArgumentException(SR.Get("GlyphsIndexRequiredIfNoUnicode"));
								}
								if (unicodeString.Length <= i)
								{
									throw new ArgumentException(SR.Get("GlyphsUnicodeStringIsTooShort"));
								}
								parsedGlyphData.glyphIndex = this.GetGlyphFromCharacter(fontFace, unicodeString[i]);
							}
							if (!flag2 && clusterMap != null)
							{
								if (flag)
								{
									for (int k = i; k < i + num2; k++)
									{
										Glyphs.SetClusterMapEntry(clusterMap, k, (ushort)num);
									}
								}
								else
								{
									Glyphs.SetClusterMapEntry(clusterMap, i, (ushort)num);
								}
							}
							parsedGlyphData.advanceWidth = Glyphs.GetAdvanceWidth(fontFace, parsedGlyphData.glyphIndex, sideways);
							break;
						}
						case 1:
							if (!Glyphs.IsEmpty(text))
							{
								parsedGlyphData.advanceWidth = double.Parse(text, CultureInfo.InvariantCulture);
								if (parsedGlyphData.advanceWidth < 0.0)
								{
									throw new ArgumentException(SR.Get("GlyphsAdvanceWidthCannotBeNegative"));
								}
							}
							break;
						case 2:
							if (!Glyphs.IsEmpty(text))
							{
								parsedGlyphData.offsetX = double.Parse(text, CultureInfo.InvariantCulture);
							}
							break;
						case 3:
							if (!Glyphs.IsEmpty(text))
							{
								parsedGlyphData.offsetY = double.Parse(text, CultureInfo.InvariantCulture);
							}
							break;
						default:
							throw new ArgumentException(SR.Get("GlyphsTooManyCommas"));
						}
						num5++;
						num6 = j + 1;
					}
					if (c == ';' || j == indices.Length)
					{
						parsedGlyphs.Add(parsedGlyphData);
						parsedGlyphData = new Glyphs.ParsedGlyphData();
						if (flag)
						{
							num3--;
							if (num3 == 0)
							{
								i += num2;
								flag = false;
							}
						}
						else
						{
							i++;
						}
						num++;
						num5 = 0;
						num6 = j + 1;
					}
				}
			}
			if (unicodeString != null)
			{
				while (i < unicodeString.Length)
				{
					if (flag)
					{
						throw new ArgumentException(SR.Get("GlyphsIndexRequiredWithinCluster"));
					}
					if (unicodeString.Length <= i)
					{
						throw new ArgumentException(SR.Get("GlyphsUnicodeStringIsTooShort"));
					}
					parsedGlyphData.glyphIndex = this.GetGlyphFromCharacter(fontFace, unicodeString[i]);
					parsedGlyphData.advanceWidth = Glyphs.GetAdvanceWidth(fontFace, parsedGlyphData.glyphIndex, sideways);
					parsedGlyphs.Add(parsedGlyphData);
					parsedGlyphData = new Glyphs.ParsedGlyphData();
					Glyphs.SetClusterMapEntry(clusterMap, i, (ushort)num);
					i++;
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0023F831 File Offset: 0x0023E831
		private static void FillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((UIElement)d).InvalidateVisual();
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0023F83E File Offset: 0x0023E83E
		private static void GlyphRunPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Glyphs)d)._glyphRunProperties = null;
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0023F84C File Offset: 0x0023E84C
		private static void OriginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Glyphs)d)._measurementGlyphRun = null;
		}

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x06004D4A RID: 19786 RVA: 0x0023F85A File Offset: 0x0023E85A
		// (set) Token: 0x06004D4B RID: 19787 RVA: 0x0023F86C File Offset: 0x0023E86C
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(Glyphs.FillProperty);
			}
			set
			{
				base.SetValue(Glyphs.FillProperty, value);
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06004D4C RID: 19788 RVA: 0x0023F87A File Offset: 0x0023E87A
		// (set) Token: 0x06004D4D RID: 19789 RVA: 0x0023F88C File Offset: 0x0023E88C
		public string Indices
		{
			get
			{
				return (string)base.GetValue(Glyphs.IndicesProperty);
			}
			set
			{
				base.SetValue(Glyphs.IndicesProperty, value);
			}
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06004D4E RID: 19790 RVA: 0x0023F89A File Offset: 0x0023E89A
		// (set) Token: 0x06004D4F RID: 19791 RVA: 0x0023F8AC File Offset: 0x0023E8AC
		public string UnicodeString
		{
			get
			{
				return (string)base.GetValue(Glyphs.UnicodeStringProperty);
			}
			set
			{
				base.SetValue(Glyphs.UnicodeStringProperty, value);
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06004D50 RID: 19792 RVA: 0x0023F8BA File Offset: 0x0023E8BA
		// (set) Token: 0x06004D51 RID: 19793 RVA: 0x0023F8CC File Offset: 0x0023E8CC
		public string CaretStops
		{
			get
			{
				return (string)base.GetValue(Glyphs.CaretStopsProperty);
			}
			set
			{
				base.SetValue(Glyphs.CaretStopsProperty, value);
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06004D52 RID: 19794 RVA: 0x0023F8DA File Offset: 0x0023E8DA
		// (set) Token: 0x06004D53 RID: 19795 RVA: 0x0023F8EC File Offset: 0x0023E8EC
		[TypeConverter("System.Windows.FontSizeConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double FontRenderingEmSize
		{
			get
			{
				return (double)base.GetValue(Glyphs.FontRenderingEmSizeProperty);
			}
			set
			{
				base.SetValue(Glyphs.FontRenderingEmSizeProperty, value);
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06004D54 RID: 19796 RVA: 0x0023F8FF File Offset: 0x0023E8FF
		// (set) Token: 0x06004D55 RID: 19797 RVA: 0x0023F911 File Offset: 0x0023E911
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double OriginX
		{
			get
			{
				return (double)base.GetValue(Glyphs.OriginXProperty);
			}
			set
			{
				base.SetValue(Glyphs.OriginXProperty, value);
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06004D56 RID: 19798 RVA: 0x0023F924 File Offset: 0x0023E924
		// (set) Token: 0x06004D57 RID: 19799 RVA: 0x0023F936 File Offset: 0x0023E936
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double OriginY
		{
			get
			{
				return (double)base.GetValue(Glyphs.OriginYProperty);
			}
			set
			{
				base.SetValue(Glyphs.OriginYProperty, value);
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06004D58 RID: 19800 RVA: 0x0023F949 File Offset: 0x0023E949
		// (set) Token: 0x06004D59 RID: 19801 RVA: 0x0023F95B File Offset: 0x0023E95B
		public Uri FontUri
		{
			get
			{
				return (Uri)base.GetValue(Glyphs.FontUriProperty);
			}
			set
			{
				base.SetValue(Glyphs.FontUriProperty, value);
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06004D5A RID: 19802 RVA: 0x0023F969 File Offset: 0x0023E969
		// (set) Token: 0x06004D5B RID: 19803 RVA: 0x0023F97B File Offset: 0x0023E97B
		public StyleSimulations StyleSimulations
		{
			get
			{
				return (StyleSimulations)base.GetValue(Glyphs.StyleSimulationsProperty);
			}
			set
			{
				base.SetValue(Glyphs.StyleSimulationsProperty, value);
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06004D5C RID: 19804 RVA: 0x0023F98E File Offset: 0x0023E98E
		// (set) Token: 0x06004D5D RID: 19805 RVA: 0x0023F9A0 File Offset: 0x0023E9A0
		public bool IsSideways
		{
			get
			{
				return (bool)base.GetValue(Glyphs.IsSidewaysProperty);
			}
			set
			{
				base.SetValue(Glyphs.IsSidewaysProperty, value);
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06004D5E RID: 19806 RVA: 0x0023F9AE File Offset: 0x0023E9AE
		// (set) Token: 0x06004D5F RID: 19807 RVA: 0x0023F9C0 File Offset: 0x0023E9C0
		public int BidiLevel
		{
			get
			{
				return (int)base.GetValue(Glyphs.BidiLevelProperty);
			}
			set
			{
				base.SetValue(Glyphs.BidiLevelProperty, value);
			}
		}

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06004D60 RID: 19808 RVA: 0x0023F9D3 File Offset: 0x0023E9D3
		// (set) Token: 0x06004D61 RID: 19809 RVA: 0x0023F9E5 File Offset: 0x0023E9E5
		public string DeviceFontName
		{
			get
			{
				return (string)base.GetValue(Glyphs.DeviceFontNameProperty);
			}
			set
			{
				base.SetValue(Glyphs.DeviceFontNameProperty, value);
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x0023F9F3 File Offset: 0x0023E9F3
		internal GlyphRun MeasurementGlyphRun
		{
			get
			{
				if (this._glyphRunProperties == null || this._measurementGlyphRun == null)
				{
					this.ComputeMeasurementGlyphRunAndOrigin();
				}
				return this._measurementGlyphRun;
			}
		}

		// Token: 0x040027FC RID: 10236
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(Glyphs), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(Glyphs.FillChanged), null));

		// Token: 0x040027FD RID: 10237
		public static readonly DependencyProperty IndicesProperty = DependencyProperty.Register("Indices", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x040027FE RID: 10238
		public static readonly DependencyProperty UnicodeStringProperty = DependencyProperty.Register("UnicodeString", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x040027FF RID: 10239
		public static readonly DependencyProperty CaretStopsProperty = DependencyProperty.Register("CaretStops", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002800 RID: 10240
		public static readonly DependencyProperty FontRenderingEmSizeProperty = DependencyProperty.Register("FontRenderingEmSize", typeof(double), typeof(Glyphs), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002801 RID: 10241
		public static readonly DependencyProperty OriginXProperty = DependencyProperty.Register("OriginX", typeof(double), typeof(Glyphs), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.OriginPropertyChanged)));

		// Token: 0x04002802 RID: 10242
		public static readonly DependencyProperty OriginYProperty = DependencyProperty.Register("OriginY", typeof(double), typeof(Glyphs), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.OriginPropertyChanged)));

		// Token: 0x04002803 RID: 10243
		public static readonly DependencyProperty FontUriProperty = DependencyProperty.Register("FontUri", typeof(Uri), typeof(Glyphs), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002804 RID: 10244
		public static readonly DependencyProperty StyleSimulationsProperty = DependencyProperty.Register("StyleSimulations", typeof(StyleSimulations), typeof(Glyphs), new FrameworkPropertyMetadata(StyleSimulations.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002805 RID: 10245
		public static readonly DependencyProperty IsSidewaysProperty = DependencyProperty.Register("IsSideways", typeof(bool), typeof(Glyphs), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002806 RID: 10246
		public static readonly DependencyProperty BidiLevelProperty = DependencyProperty.Register("BidiLevel", typeof(int), typeof(Glyphs), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002807 RID: 10247
		public static readonly DependencyProperty DeviceFontNameProperty = DependencyProperty.Register("DeviceFontName", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04002808 RID: 10248
		private Glyphs.LayoutDependentGlyphRunProperties _glyphRunProperties;

		// Token: 0x04002809 RID: 10249
		private GlyphRun _measurementGlyphRun;

		// Token: 0x0400280A RID: 10250
		private Point _glyphRunOrigin;

		// Token: 0x0400280B RID: 10251
		private const double EmMultiplier = 100.0;

		// Token: 0x02000B3A RID: 2874
		private class ParsedGlyphData
		{
			// Token: 0x04004841 RID: 18497
			public ushort glyphIndex;

			// Token: 0x04004842 RID: 18498
			public double advanceWidth;

			// Token: 0x04004843 RID: 18499
			public double offsetX;

			// Token: 0x04004844 RID: 18500
			public double offsetY;
		}

		// Token: 0x02000B3B RID: 2875
		private class LayoutDependentGlyphRunProperties
		{
			// Token: 0x06008CBC RID: 36028 RVA: 0x0033E0CF File Offset: 0x0033D0CF
			public LayoutDependentGlyphRunProperties(double pixelsPerDip)
			{
				this._pixelsPerDip = (float)pixelsPerDip;
			}

			// Token: 0x06008CBD RID: 36029 RVA: 0x0033E0E0 File Offset: 0x0033D0E0
			public GlyphRun CreateGlyphRun(Point origin, XmlLanguage language)
			{
				return new GlyphRun(this.glyphTypeface, this.bidiLevel, this.sideways, this.fontRenderingSize, this._pixelsPerDip, this.glyphIndices, origin, this.advanceWidths, this.glyphOffsets, this.unicodeString.ToCharArray(), this.deviceFontName, this.clusterMap, this.caretStops, language);
			}

			// Token: 0x04004845 RID: 18501
			public double fontRenderingSize;

			// Token: 0x04004846 RID: 18502
			public ushort[] glyphIndices;

			// Token: 0x04004847 RID: 18503
			public double[] advanceWidths;

			// Token: 0x04004848 RID: 18504
			public Point[] glyphOffsets;

			// Token: 0x04004849 RID: 18505
			public ushort[] clusterMap;

			// Token: 0x0400484A RID: 18506
			public bool sideways;

			// Token: 0x0400484B RID: 18507
			public int bidiLevel;

			// Token: 0x0400484C RID: 18508
			public GlyphTypeface glyphTypeface;

			// Token: 0x0400484D RID: 18509
			public string unicodeString;

			// Token: 0x0400484E RID: 18510
			public IList<bool> caretStops;

			// Token: 0x0400484F RID: 18511
			public string deviceFontName;

			// Token: 0x04004850 RID: 18512
			private float _pixelsPerDip;
		}
	}
}
