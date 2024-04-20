using System;
using System.Text;
using System.Windows.Media;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x02000672 RID: 1650
	internal class FontTableEntry
	{
		// Token: 0x06005166 RID: 20838 RVA: 0x0024F29C File Offset: 0x0024E29C
		internal FontTableEntry()
		{
			this._index = -1;
			this._codePage = -1;
			this._charSet = 0;
			this._bNameSealed = false;
			this._bPending = true;
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06005167 RID: 20839 RVA: 0x0024F2C7 File Offset: 0x0024E2C7
		// (set) Token: 0x06005168 RID: 20840 RVA: 0x0024F2CF File Offset: 0x0024E2CF
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06005169 RID: 20841 RVA: 0x0024F2D8 File Offset: 0x0024E2D8
		// (set) Token: 0x0600516A RID: 20842 RVA: 0x0024F2E0 File Offset: 0x0024E2E0
		internal string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x0600516B RID: 20843 RVA: 0x0024F2E9 File Offset: 0x0024E2E9
		// (set) Token: 0x0600516C RID: 20844 RVA: 0x0024F2F1 File Offset: 0x0024E2F1
		internal bool IsNameSealed
		{
			get
			{
				return this._bNameSealed;
			}
			set
			{
				this._bNameSealed = value;
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x0600516D RID: 20845 RVA: 0x0024F2FA File Offset: 0x0024E2FA
		// (set) Token: 0x0600516E RID: 20846 RVA: 0x0024F302 File Offset: 0x0024E302
		internal bool IsPending
		{
			get
			{
				return this._bPending;
			}
			set
			{
				this._bPending = value;
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x0600516F RID: 20847 RVA: 0x0024F30B File Offset: 0x0024E30B
		// (set) Token: 0x06005170 RID: 20848 RVA: 0x0024F313 File Offset: 0x0024E313
		internal int CodePage
		{
			get
			{
				return this._codePage;
			}
			set
			{
				this._codePage = value;
			}
		}

		// Token: 0x1700132C RID: 4908
		// (set) Token: 0x06005171 RID: 20849 RVA: 0x0024F31C File Offset: 0x0024E31C
		internal int CodePageFromCharSet
		{
			set
			{
				int num = FontTableEntry.CharSetToCodePage(value);
				if (num != 0)
				{
					this.CodePage = num;
				}
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06005172 RID: 20850 RVA: 0x0024F33A File Offset: 0x0024E33A
		// (set) Token: 0x06005173 RID: 20851 RVA: 0x0024F342 File Offset: 0x0024E342
		internal int CharSet
		{
			get
			{
				return this._charSet;
			}
			set
			{
				this._charSet = value;
			}
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0024F34C File Offset: 0x0024E34C
		internal static int CharSetToCodePage(int cs)
		{
			if (cs <= 163)
			{
				if (cs > 77)
				{
					if (cs != 78)
					{
						switch (cs)
						{
						case 128:
							break;
						case 129:
							return 949;
						case 130:
							return 1361;
						case 131:
						case 132:
						case 133:
						case 135:
							return 0;
						case 134:
							return 936;
						case 136:
							return 950;
						default:
							switch (cs)
							{
							case 161:
								return 1253;
							case 162:
								return 1254;
							case 163:
								return 1258;
							default:
								return 0;
							}
							break;
						}
					}
					return 932;
				}
				switch (cs)
				{
				case 0:
					return 1252;
				case 1:
					return -1;
				case 2:
					return 1252;
				case 3:
					return -1;
				default:
					if (cs == 77)
					{
						return 10000;
					}
					break;
				}
			}
			else if (cs <= 222)
			{
				switch (cs)
				{
				case 177:
					return 1255;
				case 178:
					return 1256;
				case 179:
					return 1256;
				case 180:
					return 1256;
				case 181:
					return 1255;
				case 182:
				case 183:
				case 184:
				case 185:
					break;
				case 186:
					return 1257;
				default:
					if (cs == 204)
					{
						return 1251;
					}
					if (cs == 222)
					{
						return 874;
					}
					break;
				}
			}
			else
			{
				if (cs == 238)
				{
					return 1250;
				}
				if (cs == 254)
				{
					return 437;
				}
				if (cs == 255)
				{
					return 850;
				}
			}
			return 0;
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0024F4E4 File Offset: 0x0024E4E4
		internal void ComputePreferredCodePage()
		{
			int[] array = new int[]
			{
				1252,
				932,
				949,
				1361,
				936,
				950,
				1253,
				1254,
				1258,
				1255,
				1256,
				1257,
				1251,
				874,
				1250,
				437,
				850
			};
			this.CodePage = 1252;
			this.CharSet = 0;
			if (this.Name != null && this.Name.Length > 0)
			{
				byte[] bytes = new byte[this.Name.Length * 6];
				char[] array2 = new char[this.Name.Length * 6];
				for (int i = 0; i < array.Length; i++)
				{
					Encoding encoding = InternalEncoding.GetEncoding(array[i]);
					int bytes2 = encoding.GetBytes(this.Name, 0, this.Name.Length, bytes, 0);
					int chars = encoding.GetChars(bytes, 0, bytes2, array2, 0);
					if (chars == this.Name.Length)
					{
						int num = 0;
						while (num < chars && array2[num] == this.Name[num])
						{
							num++;
						}
						if (num == chars)
						{
							this.CodePage = array[i];
							this.CharSet = FontTableEntry.CodePageToCharSet(this.CodePage);
							break;
						}
					}
				}
				if (FontTableEntry.IsSymbolFont(this.Name))
				{
					this.CharSet = 2;
				}
			}
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x0024F60C File Offset: 0x0024E60C
		private static int CodePageToCharSet(int cp)
		{
			if (cp <= 936)
			{
				if (cp <= 850)
				{
					if (cp == 437)
					{
						return 254;
					}
					if (cp == 850)
					{
						return 255;
					}
				}
				else
				{
					if (cp == 874)
					{
						return 222;
					}
					if (cp == 932)
					{
						return 128;
					}
					if (cp == 936)
					{
						return 134;
					}
				}
			}
			else if (cp <= 950)
			{
				if (cp == 949)
				{
					return 129;
				}
				if (cp == 950)
				{
					return 136;
				}
			}
			else
			{
				switch (cp)
				{
				case 1250:
					return 238;
				case 1251:
					return 204;
				case 1252:
					return 0;
				case 1253:
					return 161;
				case 1254:
					return 162;
				case 1255:
					return 177;
				case 1256:
					return 178;
				case 1257:
					return 186;
				case 1258:
					return 163;
				default:
					if (cp == 1361)
					{
						return 130;
					}
					if (cp == 10000)
					{
						return 77;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x0024F72C File Offset: 0x0024E72C
		private static bool IsSymbolFont(string typefaceName)
		{
			bool result = false;
			Typeface typeface = new Typeface(typefaceName);
			if (typeface != null)
			{
				GlyphTypeface glyphTypeface = typeface.TryGetGlyphTypeface();
				if (glyphTypeface != null && glyphTypeface.Symbol)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04002E66 RID: 11878
		private string _name;

		// Token: 0x04002E67 RID: 11879
		private int _index;

		// Token: 0x04002E68 RID: 11880
		private int _codePage;

		// Token: 0x04002E69 RID: 11881
		private int _charSet;

		// Token: 0x04002E6A RID: 11882
		private bool _bNameSealed;

		// Token: 0x04002E6B RID: 11883
		private bool _bPending;
	}
}
