using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Internal.Xaml.Parser
{
	// Token: 0x02000337 RID: 823
	internal class SpecialBracketCharacters : ISupportInitialize
	{
		// Token: 0x06001F09 RID: 7945 RVA: 0x00170AA0 File Offset: 0x0016FAA0
		internal SpecialBracketCharacters()
		{
			this.BeginInit();
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x00170AAE File Offset: 0x0016FAAE
		internal SpecialBracketCharacters(IReadOnlyDictionary<char, char> attributeList)
		{
			this.BeginInit();
			if (attributeList != null && attributeList.Count > 0)
			{
				this.Tokenize(attributeList);
			}
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x00170ACF File Offset: 0x0016FACF
		internal void AddBracketCharacters(char openingBracket, char closingBracket)
		{
			if (this._initializing)
			{
				this._startCharactersStringBuilder.Append(openingBracket);
				this._endCharactersStringBuilder.Append(closingBracket);
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x00170AFC File Offset: 0x0016FAFC
		private void Tokenize(IReadOnlyDictionary<char, char> attributeList)
		{
			if (this._initializing)
			{
				foreach (char c in attributeList.Keys)
				{
					char c2 = attributeList[c];
					string empty = string.Empty;
					if (this.IsValidBracketCharacter(c, c2))
					{
						this._startCharactersStringBuilder.Append(c);
						this._endCharactersStringBuilder.Append(c2);
					}
				}
			}
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x00170B7C File Offset: 0x0016FB7C
		private bool IsValidBracketCharacter(char openingBracket, char closingBracket)
		{
			if (openingBracket == closingBracket)
			{
				throw new InvalidOperationException("Opening bracket character cannot be the same as closing bracket character.");
			}
			if (char.IsLetterOrDigit(openingBracket) || char.IsLetterOrDigit(closingBracket) || char.IsWhiteSpace(openingBracket) || char.IsWhiteSpace(closingBracket))
			{
				throw new InvalidOperationException("Bracket characters cannot be alpha-numeric or whitespace.");
			}
			if (SpecialBracketCharacters._restrictedCharSet.Contains(openingBracket) || SpecialBracketCharacters._restrictedCharSet.Contains(closingBracket))
			{
				throw new InvalidOperationException("Bracket characters cannot be one of the following: '=' , ',', ''', '\"', '{ ', ' }', '\\'");
			}
			return true;
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x00170BE9 File Offset: 0x0016FBE9
		internal bool IsSpecialCharacter(char ch)
		{
			return this._startChars.Contains(ch.ToString()) || this._endChars.Contains(ch.ToString());
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x00170C13 File Offset: 0x0016FC13
		internal bool StartsEscapeSequence(char ch)
		{
			return this._startChars.Contains(ch.ToString());
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x00170C27 File Offset: 0x0016FC27
		internal bool EndsEscapeSequence(char ch)
		{
			return this._endChars.Contains(ch.ToString());
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x00170C3B File Offset: 0x0016FC3B
		internal bool Match(char start, char end)
		{
			return this._endChars.IndexOf(end.ToString()) == this._startChars.IndexOf(start.ToString());
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00170C63 File Offset: 0x0016FC63
		internal string StartBracketCharacters
		{
			get
			{
				return this._startChars;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x00170C6B File Offset: 0x0016FC6B
		internal string EndBracketCharacters
		{
			get
			{
				return this._endChars;
			}
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x00170C73 File Offset: 0x0016FC73
		public void BeginInit()
		{
			this._initializing = true;
			this._startCharactersStringBuilder = new StringBuilder();
			this._endCharactersStringBuilder = new StringBuilder();
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x00170C92 File Offset: 0x0016FC92
		public void EndInit()
		{
			this._startChars = this._startCharactersStringBuilder.ToString();
			this._endChars = this._endCharactersStringBuilder.ToString();
			this._initializing = false;
		}

		// Token: 0x04000F64 RID: 3940
		private string _startChars;

		// Token: 0x04000F65 RID: 3941
		private string _endChars;

		// Token: 0x04000F66 RID: 3942
		private static readonly ISet<char> _restrictedCharSet = new SortedSet<char>(new char[]
		{
			'=',
			',',
			'\'',
			'"',
			'{',
			'}',
			'\\'
		});

		// Token: 0x04000F67 RID: 3943
		private bool _initializing;

		// Token: 0x04000F68 RID: 3944
		private StringBuilder _startCharactersStringBuilder;

		// Token: 0x04000F69 RID: 3945
		private StringBuilder _endCharactersStringBuilder;
	}
}
