using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x0200065D RID: 1629
	internal class RtfToXamlLexer
	{
		// Token: 0x06005036 RID: 20534 RVA: 0x0024BF43 File Offset: 0x0024AF43
		internal RtfToXamlLexer(byte[] rtfBytes)
		{
			this._rtfBytes = rtfBytes;
			this._currentCodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
			this._currentEncoding = InternalEncoding.GetEncoding(this._currentCodePage);
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x0024BF78 File Offset: 0x0024AF78
		internal RtfToXamlError Next(RtfToken token, FormatState formatState)
		{
			RtfToXamlError result = RtfToXamlError.None;
			this._rtfLastIndex = this._rtfIndex;
			token.Empty();
			if (this._rtfIndex >= this._rtfBytes.Length)
			{
				token.Type = RtfTokenType.TokenEOF;
				return result;
			}
			int rtfIndex = this._rtfIndex;
			byte[] rtfBytes = this._rtfBytes;
			int rtfIndex2 = this._rtfIndex;
			this._rtfIndex = rtfIndex2 + 1;
			byte b = rtfBytes[rtfIndex2];
			if (b <= 13)
			{
				if (b == 0)
				{
					token.Type = RtfTokenType.TokenNullChar;
					return result;
				}
				if (b == 10 || b == 13)
				{
					token.Type = RtfTokenType.TokenNewline;
					return result;
				}
			}
			else if (b != 92)
			{
				if (b == 123)
				{
					token.Type = RtfTokenType.TokenGroupStart;
					return result;
				}
				if (b == 125)
				{
					token.Type = RtfTokenType.TokenGroupEnd;
					return result;
				}
			}
			else
			{
				if (this._rtfIndex >= this._rtfBytes.Length)
				{
					token.Type = RtfTokenType.TokenInvalid;
					return result;
				}
				if (this.IsControlCharValid(this.CurByte))
				{
					int rtfIndex3 = this._rtfIndex;
					this.SetRtfIndex(token, rtfIndex3);
					token.Text = this.CurrentEncoding.GetString(this._rtfBytes, rtfIndex3 - 1, this._rtfIndex - rtfIndex);
					return result;
				}
				if (this.CurByte == 39)
				{
					this._rtfIndex--;
					return this.NextText(token);
				}
				if (this.CurByte == 42)
				{
					this._rtfIndex++;
					token.Type = RtfTokenType.TokenDestination;
					return result;
				}
				token.Type = RtfTokenType.TokenTextSymbol;
				token.Text = this.CurrentEncoding.GetString(this._rtfBytes, this._rtfIndex, 1);
				this._rtfIndex++;
				return result;
			}
			this._rtfIndex--;
			if (formatState == null || formatState.RtfDestination != RtfDestination.DestPicture)
			{
				return this.NextText(token);
			}
			token.Type = RtfTokenType.TokenPictureData;
			return result;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x0024C138 File Offset: 0x0024B138
		internal RtfToXamlError AdvanceForUnicode(long nSkip)
		{
			RtfToXamlError rtfToXamlError = RtfToXamlError.None;
			RtfToken rtfToken = new RtfToken();
			while (nSkip > 0L && rtfToXamlError == RtfToXamlError.None)
			{
				rtfToXamlError = this.Next(rtfToken, null);
				if (rtfToXamlError != RtfToXamlError.None)
				{
					break;
				}
				switch (rtfToken.Type)
				{
				default:
					this.Backup();
					nSkip = 0L;
					break;
				case RtfTokenType.TokenText:
				{
					int rtfIndex = this._rtfIndex;
					this.Backup();
					while (nSkip > 0L && this._rtfIndex < rtfIndex)
					{
						if (this.CurByte == 92)
						{
							this._rtfIndex += 4;
						}
						else
						{
							this._rtfIndex++;
						}
						nSkip -= 1L;
					}
					break;
				}
				case RtfTokenType.TokenNewline:
				case RtfTokenType.TokenNullChar:
					break;
				case RtfTokenType.TokenControl:
					if (rtfToken.RtfControlWordInfo != null && rtfToken.RtfControlWordInfo.Control == RtfControlWord.Ctrl_BIN)
					{
						this.AdvanceForBinary((int)rtfToken.Parameter);
					}
					nSkip -= 1L;
					break;
				}
			}
			return rtfToXamlError;
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x0024C22F File Offset: 0x0024B22F
		internal void AdvanceForBinary(int skip)
		{
			if (this._rtfIndex + skip < this._rtfBytes.Length)
			{
				this._rtfIndex += skip;
				return;
			}
			this._rtfIndex = this._rtfBytes.Length - 1;
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x0024C264 File Offset: 0x0024B264
		internal void AdvanceForImageData()
		{
			byte[] rtfBytes;
			int rtfIndex;
			for (byte b = this._rtfBytes[this._rtfIndex]; b != 125; b = rtfBytes[rtfIndex])
			{
				rtfBytes = this._rtfBytes;
				rtfIndex = this._rtfIndex;
				this._rtfIndex = rtfIndex + 1;
			}
			this._rtfIndex--;
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x0024C2B0 File Offset: 0x0024B2B0
		internal void WriteImageData(Stream imageStream, bool isBinary)
		{
			byte b = this._rtfBytes[this._rtfIndex];
			while (b != 123 && b != 125 && b != 92)
			{
				if (isBinary)
				{
					imageStream.WriteByte(b);
				}
				else
				{
					byte b2 = this._rtfBytes[this._rtfIndex + 1];
					if (this.IsHex(b) && this.IsHex(b2))
					{
						byte b3 = this.HexToByte(b);
						byte b4 = this.HexToByte(b2);
						imageStream.WriteByte((byte)((int)b3 << 4 | (int)b4));
						this._rtfIndex++;
					}
				}
				this._rtfIndex++;
				b = this._rtfBytes[this._rtfIndex];
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (set) Token: 0x0600503C RID: 20540 RVA: 0x0024C353 File Offset: 0x0024B353
		internal int CodePage
		{
			set
			{
				if (this._currentCodePage != value)
				{
					this._currentCodePage = value;
					this._currentEncoding = InternalEncoding.GetEncoding(this._currentCodePage);
				}
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x0600503D RID: 20541 RVA: 0x0024C376 File Offset: 0x0024B376
		internal Encoding CurrentEncoding
		{
			get
			{
				return this._currentEncoding;
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x0024C37E File Offset: 0x0024B37E
		internal byte CurByte
		{
			get
			{
				return this._rtfBytes[this._rtfIndex];
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x0024C390 File Offset: 0x0024B390
		private RtfToXamlError NextText(RtfToken token)
		{
			RtfToXamlError result = RtfToXamlError.None;
			this._rtfLastIndex = this._rtfIndex;
			token.Empty();
			token.Type = RtfTokenType.TokenText;
			int i = this._rtfIndex;
			int j = i;
			bool flag = false;
			while (j < this._rtfBytes.Length)
			{
				if (this.IsControl(this._rtfBytes[j]))
				{
					if (this._rtfBytes[j] != 92 || j + 3 >= this._rtfBytes.Length || this._rtfBytes[j + 1] != 39 || !this.IsHex(this._rtfBytes[j + 2]) || !this.IsHex(this._rtfBytes[j + 3]))
					{
						break;
					}
					j += 4;
					flag = true;
				}
				else
				{
					if (this._rtfBytes[j] == 13 || this._rtfBytes[j] == 10 || this._rtfBytes[j] == 0)
					{
						break;
					}
					j++;
				}
			}
			if (i == j)
			{
				token.Type = RtfTokenType.TokenInvalid;
			}
			else
			{
				this._rtfIndex = j;
				if (flag)
				{
					int count = 0;
					byte[] array = new byte[j - i];
					while (i < j)
					{
						if (this._rtfBytes[i] == 92)
						{
							array[count++] = (byte)(this.HexToByte(this._rtfBytes[i + 2]) << 4) + this.HexToByte(this._rtfBytes[i + 3]);
							i += 4;
						}
						else
						{
							array[count++] = this._rtfBytes[i++];
						}
					}
					token.Text = this.CurrentEncoding.GetString(array, 0, count);
				}
				else
				{
					token.Text = this.CurrentEncoding.GetString(this._rtfBytes, i, j - i);
				}
			}
			return result;
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x0024C51A File Offset: 0x0024B51A
		private RtfToXamlError Backup()
		{
			if (this._rtfLastIndex == 0)
			{
				return RtfToXamlError.InvalidFormat;
			}
			this._rtfIndex = this._rtfLastIndex;
			this._rtfLastIndex = 0;
			return RtfToXamlError.None;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0024C53C File Offset: 0x0024B53C
		private void SetRtfIndex(RtfToken token, int controlStartIndex)
		{
			while (this._rtfIndex < this._rtfBytes.Length && this.IsControlCharValid(this.CurByte))
			{
				this._rtfIndex++;
			}
			int num = this._rtfIndex - controlStartIndex;
			string @string = this.CurrentEncoding.GetString(this._rtfBytes, controlStartIndex, num);
			if (num > 32)
			{
				token.Type = RtfTokenType.TokenInvalid;
				return;
			}
			token.Type = RtfTokenType.TokenControl;
			token.RtfControlWordInfo = RtfToXamlLexer.RtfControlWordLookup(@string);
			if (this._rtfIndex < this._rtfBytes.Length)
			{
				if (this.CurByte == 32)
				{
					this._rtfIndex++;
					return;
				}
				if (this.IsParameterStart(this.CurByte))
				{
					bool flag = false;
					if (this.CurByte == 45)
					{
						flag = true;
						this._rtfIndex++;
					}
					long num2 = 0L;
					int rtfIndex = this._rtfIndex;
					while (this._rtfIndex < this._rtfBytes.Length && this.IsParameterFollow(this.CurByte))
					{
						num2 = num2 * 10L + (long)(this.CurByte - 48);
						this._rtfIndex++;
					}
					int num3 = this._rtfIndex - rtfIndex;
					if (this._rtfIndex < this._rtfBytes.Length && this.CurByte == 32)
					{
						this._rtfIndex++;
					}
					if (flag)
					{
						num2 = -num2;
					}
					if (num3 > 10)
					{
						token.Type = RtfTokenType.TokenInvalid;
						return;
					}
					token.Parameter = num2;
				}
			}
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x0024C6A1 File Offset: 0x0024B6A1
		private bool IsControl(byte controlChar)
		{
			return controlChar == 92 || controlChar == 123 || controlChar == 125;
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x0024C6B4 File Offset: 0x0024B6B4
		private bool IsControlCharValid(byte controlChar)
		{
			return (controlChar >= 97 && controlChar <= 122) || (controlChar >= 65 && controlChar <= 90);
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x0024C6D1 File Offset: 0x0024B6D1
		private bool IsParameterStart(byte controlChar)
		{
			return controlChar == 45 || (controlChar >= 48 && controlChar <= 57);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x0024C6E9 File Offset: 0x0024B6E9
		private bool IsParameterFollow(byte controlChar)
		{
			return controlChar >= 48 && controlChar <= 57;
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x0024C6FA File Offset: 0x0024B6FA
		private bool IsHex(byte controlChar)
		{
			return (controlChar >= 48 && controlChar <= 57) || (controlChar >= 97 && controlChar <= 102) || (controlChar >= 65 && controlChar <= 70);
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x0024C721 File Offset: 0x0024B721
		private byte HexToByte(byte hexByte)
		{
			if (hexByte >= 48 && hexByte <= 57)
			{
				return hexByte - 48;
			}
			if (hexByte >= 97 && hexByte <= 102)
			{
				return 10 + hexByte - 97;
			}
			if (hexByte >= 65 && hexByte <= 70)
			{
				return 10 + hexByte - 65;
			}
			return 0;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x0024C75C File Offset: 0x0024B75C
		private static RtfControlWordInfo RtfControlWordLookup(string controlName)
		{
			object rtfControlTableMutex = RtfToXamlLexer._rtfControlTableMutex;
			lock (rtfControlTableMutex)
			{
				if (RtfToXamlLexer._rtfControlTable == null)
				{
					RtfControlWordInfo[] controlTable = RtfControls.ControlTable;
					RtfToXamlLexer._rtfControlTable = new Hashtable(controlTable.Length);
					for (int i = 0; i < controlTable.Length; i++)
					{
						RtfToXamlLexer._rtfControlTable.Add(controlTable[i].ControlName, controlTable[i]);
					}
				}
			}
			RtfControlWordInfo rtfControlWordInfo = (RtfControlWordInfo)RtfToXamlLexer._rtfControlTable[controlName];
			if (rtfControlWordInfo == null)
			{
				controlName = controlName.ToLower(CultureInfo.InvariantCulture);
				rtfControlWordInfo = (RtfControlWordInfo)RtfToXamlLexer._rtfControlTable[controlName];
			}
			return rtfControlWordInfo;
		}

		// Token: 0x04002D9A RID: 11674
		private byte[] _rtfBytes;

		// Token: 0x04002D9B RID: 11675
		private int _rtfIndex;

		// Token: 0x04002D9C RID: 11676
		private int _rtfLastIndex;

		// Token: 0x04002D9D RID: 11677
		private int _currentCodePage;

		// Token: 0x04002D9E RID: 11678
		private Encoding _currentEncoding;

		// Token: 0x04002D9F RID: 11679
		private static object _rtfControlTableMutex = new object();

		// Token: 0x04002DA0 RID: 11680
		private static Hashtable _rtfControlTable = null;

		// Token: 0x04002DA1 RID: 11681
		private const int MAX_CONTROL_LENGTH = 32;

		// Token: 0x04002DA2 RID: 11682
		private const int MAX_PARAM_LENGTH = 10;
	}
}
