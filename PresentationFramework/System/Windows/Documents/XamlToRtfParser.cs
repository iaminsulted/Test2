using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020006E4 RID: 1764
	internal class XamlToRtfParser
	{
		// Token: 0x06005CAD RID: 23725 RVA: 0x0028802A File Offset: 0x0028702A
		internal XamlToRtfParser(string xaml)
		{
			this._xaml = xaml;
			this._xamlLexer = new XamlToRtfParser.XamlLexer(this._xaml);
			this._xamlTagStack = new XamlToRtfParser.XamlTagStack();
			this._xamlAttributes = new XamlToRtfParser.XamlAttributes(this._xaml);
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x00288068 File Offset: 0x00287068
		internal XamlToRtfError Parse()
		{
			if (this._xamlContent == null || this._xamlError == null)
			{
				return XamlToRtfError.Unknown;
			}
			XamlToRtfParser.XamlToken xamlToken = new XamlToRtfParser.XamlToken();
			string empty = string.Empty;
			XamlToRtfError xamlToRtfError;
			for (xamlToRtfError = this._xamlContent.StartDocument(); xamlToRtfError == XamlToRtfError.None; xamlToRtfError = XamlToRtfError.Unknown)
			{
				xamlToRtfError = this._xamlLexer.Next(xamlToken);
				if (xamlToRtfError != XamlToRtfError.None || xamlToken.TokenType == XamlTokenType.XTokEOF)
				{
					break;
				}
				switch (xamlToken.TokenType)
				{
				case XamlTokenType.XTokInvalid:
					xamlToRtfError = XamlToRtfError.Unknown;
					continue;
				case XamlTokenType.XTokCharacters:
					xamlToRtfError = this._xamlContent.Characters(xamlToken.Text);
					continue;
				case XamlTokenType.XTokEntity:
					xamlToRtfError = this._xamlContent.SkippedEntity(xamlToken.Text);
					continue;
				case XamlTokenType.XTokStartElement:
					xamlToRtfError = this.ParseXTokStartElement(xamlToken, ref empty);
					continue;
				case XamlTokenType.XTokEndElement:
					xamlToRtfError = this.ParseXTokEndElement(xamlToken, ref empty);
					continue;
				case XamlTokenType.XTokCData:
				case XamlTokenType.XTokPI:
				case XamlTokenType.XTokComment:
					continue;
				case XamlTokenType.XTokWS:
					xamlToRtfError = this._xamlContent.IgnorableWhitespace(xamlToken.Text);
					continue;
				}
			}
			if (xamlToRtfError == XamlToRtfError.None && this._xamlTagStack.Count != 0)
			{
				xamlToRtfError = XamlToRtfError.Unknown;
			}
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlContent.EndDocument();
			}
			return xamlToRtfError;
		}

		// Token: 0x06005CAF RID: 23727 RVA: 0x0028817E File Offset: 0x0028717E
		internal void SetCallbacks(IXamlContentHandler xamlContent, IXamlErrorHandler xamlError)
		{
			this._xamlContent = xamlContent;
			this._xamlError = xamlError;
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x00288190 File Offset: 0x00287190
		private XamlToRtfError ParseXTokStartElement(XamlToRtfParser.XamlToken xamlToken, ref string name)
		{
			XamlToRtfError xamlToRtfError = this._xamlAttributes.Init(xamlToken.Text);
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlAttributes.GetTag(ref name);
				if (xamlToRtfError == XamlToRtfError.None)
				{
					xamlToRtfError = this._xamlContent.StartElement(string.Empty, name, name, this._xamlAttributes);
					if (xamlToRtfError == XamlToRtfError.None)
					{
						if (this._xamlAttributes.IsEmpty)
						{
							xamlToRtfError = this._xamlContent.EndElement(string.Empty, name, name);
						}
						else
						{
							xamlToRtfError = (XamlToRtfError)this._xamlTagStack.Push(name);
						}
					}
				}
			}
			return xamlToRtfError;
		}

		// Token: 0x06005CB1 RID: 23729 RVA: 0x00288214 File Offset: 0x00287214
		private XamlToRtfError ParseXTokEndElement(XamlToRtfParser.XamlToken xamlToken, ref string name)
		{
			XamlToRtfError xamlToRtfError = this._xamlAttributes.Init(xamlToken.Text);
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlAttributes.GetTag(ref name);
				if (xamlToRtfError == XamlToRtfError.None && this._xamlTagStack.IsMatchTop(name))
				{
					this._xamlTagStack.Pop();
					xamlToRtfError = this._xamlContent.EndElement(string.Empty, name, name);
				}
			}
			return xamlToRtfError;
		}

		// Token: 0x04003127 RID: 12583
		private string _xaml;

		// Token: 0x04003128 RID: 12584
		private XamlToRtfParser.XamlLexer _xamlLexer;

		// Token: 0x04003129 RID: 12585
		private XamlToRtfParser.XamlTagStack _xamlTagStack;

		// Token: 0x0400312A RID: 12586
		private XamlToRtfParser.XamlAttributes _xamlAttributes;

		// Token: 0x0400312B RID: 12587
		private IXamlContentHandler _xamlContent;

		// Token: 0x0400312C RID: 12588
		private IXamlErrorHandler _xamlError;

		// Token: 0x02000B84 RID: 2948
		internal class XamlLexer
		{
			// Token: 0x06008E44 RID: 36420 RVA: 0x00340A8E File Offset: 0x0033FA8E
			internal XamlLexer(string xaml)
			{
				this._xaml = xaml;
			}

			// Token: 0x06008E45 RID: 36421 RVA: 0x00340AA0 File Offset: 0x0033FAA0
			internal XamlToRtfError Next(XamlToRtfParser.XamlToken token)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int xamlIndex = this._xamlIndex;
				if (this._xamlIndex < this._xaml.Length)
				{
					char c = this._xaml[this._xamlIndex];
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
						case '\n':
						case '\r':
							break;
						case '\v':
						case '\f':
							goto IL_124;
						default:
							if (c != ' ')
							{
								goto IL_124;
							}
							break;
						}
						token.TokenType = XamlTokenType.XTokWS;
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (!this.IsSpace(this._xaml[this._xamlIndex]))
							{
								break;
							}
							this._xamlIndex++;
						}
						goto IL_17C;
					}
					if (c == '&')
					{
						token.TokenType = XamlTokenType.XTokInvalid;
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (this._xaml[this._xamlIndex] == ';')
							{
								this._xamlIndex++;
								token.TokenType = XamlTokenType.XTokEntity;
								break;
							}
							this._xamlIndex++;
						}
						goto IL_17C;
					}
					if (c == '<')
					{
						this.NextLessThanToken(token);
						goto IL_17C;
					}
					IL_124:
					token.TokenType = XamlTokenType.XTokCharacters;
					this._xamlIndex++;
					while (this.IsCharsAvailable(1) && this._xaml[this._xamlIndex] != '&' && this._xaml[this._xamlIndex] != '<')
					{
						this._xamlIndex++;
					}
				}
				IL_17C:
				token.Text = this._xaml.Substring(xamlIndex, this._xamlIndex - xamlIndex);
				if (token.Text.Length == 0)
				{
					token.TokenType = XamlTokenType.XTokEOF;
				}
				return result;
			}

			// Token: 0x06008E46 RID: 36422 RVA: 0x00340C58 File Offset: 0x0033FC58
			private bool IsSpace(char character)
			{
				return character == ' ' || character == '\t' || character == '\n' || character == '\r';
			}

			// Token: 0x06008E47 RID: 36423 RVA: 0x00340C70 File Offset: 0x0033FC70
			private bool IsCharsAvailable(int index)
			{
				return this._xamlIndex + index <= this._xaml.Length;
			}

			// Token: 0x06008E48 RID: 36424 RVA: 0x00340C8C File Offset: 0x0033FC8C
			private void NextLessThanToken(XamlToRtfParser.XamlToken token)
			{
				this._xamlIndex++;
				if (!this.IsCharsAvailable(1))
				{
					token.TokenType = XamlTokenType.XTokInvalid;
					return;
				}
				token.TokenType = XamlTokenType.XTokInvalid;
				char c = this._xaml[this._xamlIndex];
				if (c <= '/')
				{
					if (c == '!')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(3))
						{
							if (this._xaml[this._xamlIndex] == '-' && this._xaml[this._xamlIndex + 1] == '-' && this._xaml[this._xamlIndex + 2] == '>')
							{
								this._xamlIndex += 3;
								token.TokenType = XamlTokenType.XTokComment;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
					if (c == '/')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (this._xaml[this._xamlIndex] == '>')
							{
								this._xamlIndex++;
								token.TokenType = XamlTokenType.XTokEndElement;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
				}
				else
				{
					if (c == '>')
					{
						this._xamlIndex++;
						token.TokenType = XamlTokenType.XTokInvalid;
						return;
					}
					if (c == '?')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(2))
						{
							if (this._xaml[this._xamlIndex] == '?' && this._xaml[this._xamlIndex + 1] == '>')
							{
								this._xamlIndex += 2;
								token.TokenType = XamlTokenType.XTokPI;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
				}
				char c2 = '\0';
				while (this.IsCharsAvailable(1))
				{
					if (c2 != '\0')
					{
						if (this._xaml[this._xamlIndex] == c2)
						{
							c2 = '\0';
						}
					}
					else if (this._xaml[this._xamlIndex] == '"' || this._xaml[this._xamlIndex] == '\'')
					{
						c2 = this._xaml[this._xamlIndex];
					}
					else if (this._xaml[this._xamlIndex] == '>')
					{
						this._xamlIndex++;
						token.TokenType = XamlTokenType.XTokStartElement;
						return;
					}
					this._xamlIndex++;
				}
			}

			// Token: 0x04004925 RID: 18725
			private string _xaml;

			// Token: 0x04004926 RID: 18726
			private int _xamlIndex;
		}

		// Token: 0x02000B85 RID: 2949
		internal class XamlTagStack : ArrayList
		{
			// Token: 0x06008E49 RID: 36425 RVA: 0x00340EF4 File Offset: 0x0033FEF4
			internal XamlTagStack() : base(10)
			{
			}

			// Token: 0x06008E4A RID: 36426 RVA: 0x00340EFE File Offset: 0x0033FEFE
			internal RtfToXamlError Push(string xamlTag)
			{
				this.Add(xamlTag);
				return RtfToXamlError.None;
			}

			// Token: 0x06008E4B RID: 36427 RVA: 0x00340F09 File Offset: 0x0033FF09
			internal void Pop()
			{
				if (this.Count > 0)
				{
					this.RemoveAt(this.Count - 1);
				}
			}

			// Token: 0x06008E4C RID: 36428 RVA: 0x00340F24 File Offset: 0x0033FF24
			internal bool IsMatchTop(string xamlTag)
			{
				if (this.Count == 0)
				{
					return false;
				}
				string text = (string)this[this.Count - 1];
				return text.Length != 0 && string.Compare(xamlTag, xamlTag.Length, text, text.Length, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
			}
		}

		// Token: 0x02000B86 RID: 2950
		internal class XamlAttributes : IXamlAttributes
		{
			// Token: 0x06008E4D RID: 36429 RVA: 0x00340F78 File Offset: 0x0033FF78
			internal XamlAttributes(string xaml)
			{
				this._xamlParsePoints = new XamlToRtfParser.XamlParsePoints();
			}

			// Token: 0x06008E4E RID: 36430 RVA: 0x00340F8B File Offset: 0x0033FF8B
			internal XamlToRtfError Init(string xaml)
			{
				return this._xamlParsePoints.Init(xaml);
			}

			// Token: 0x06008E4F RID: 36431 RVA: 0x00340F9C File Offset: 0x0033FF9C
			internal XamlToRtfError GetTag(ref string xamlTag)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (!this._xamlParsePoints.IsValid)
				{
					return XamlToRtfError.Unknown;
				}
				xamlTag = (string)this._xamlParsePoints[0];
				return result;
			}

			// Token: 0x06008E50 RID: 36432 RVA: 0x00340FD0 File Offset: 0x0033FFD0
			XamlToRtfError IXamlAttributes.GetLength(ref int length)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (this._xamlParsePoints.IsValid)
				{
					length = (this._xamlParsePoints.Count - 1) / 2;
					return result;
				}
				return XamlToRtfError.Unknown;
			}

			// Token: 0x06008E51 RID: 36433 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetUri(int index, ref string uri)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E52 RID: 36434 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetLocalName(int index, ref string localName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E53 RID: 36435 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetQName(int index, ref string qName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E54 RID: 36436 RVA: 0x00341000 File Offset: 0x00340000
			XamlToRtfError IXamlAttributes.GetName(int index, ref string uri, ref string localName, ref string qName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int num = (this._xamlParsePoints.Count - 1) / 2;
				if (index < 0 || index > num - 1)
				{
					return XamlToRtfError.Unknown;
				}
				localName = (string)this._xamlParsePoints[index * 2 + 1];
				qName = (string)this._xamlParsePoints[index * 2 + 2];
				return result;
			}

			// Token: 0x06008E55 RID: 36437 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetIndexFromName(string uri, string localName, ref int index)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E56 RID: 36438 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetIndexFromQName(string qName, ref int index)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E57 RID: 36439 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetType(int index, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E58 RID: 36440 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetTypeFromName(string uri, string localName, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E59 RID: 36441 RVA: 0x0034105C File Offset: 0x0034005C
			XamlToRtfError IXamlAttributes.GetValue(int index, ref string valueName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int num = (this._xamlParsePoints.Count - 1) / 2;
				if (index < 0 || index > num - 1)
				{
					return XamlToRtfError.OutOfRange;
				}
				valueName = (string)this._xamlParsePoints[index * 2 + 2];
				return result;
			}

			// Token: 0x06008E5A RID: 36442 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetValueFromName(string uri, string localName, ref string valueName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E5B RID: 36443 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetValueFromQName(string qName, ref string valueName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E5C RID: 36444 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlAttributes.GetTypeFromQName(string qName, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x17001F25 RID: 7973
			// (get) Token: 0x06008E5D RID: 36445 RVA: 0x0034109F File Offset: 0x0034009F
			internal bool IsEmpty
			{
				get
				{
					return this._xamlParsePoints.IsEmpty;
				}
			}

			// Token: 0x04004927 RID: 18727
			private XamlToRtfParser.XamlParsePoints _xamlParsePoints;
		}

		// Token: 0x02000B87 RID: 2951
		internal class XamlParsePoints : ArrayList
		{
			// Token: 0x06008E5E RID: 36446 RVA: 0x00340EF4 File Offset: 0x0033FEF4
			internal XamlParsePoints() : base(10)
			{
			}

			// Token: 0x06008E5F RID: 36447 RVA: 0x003410AC File Offset: 0x003400AC
			internal XamlToRtfError Init(string xaml)
			{
				XamlToRtfError result = XamlToRtfError.None;
				this._empty = false;
				this._valid = false;
				this.Clear();
				int i = 0;
				if (xaml.Length < 2 || xaml[0] != '<' || xaml[xaml.Length - 1] != '>')
				{
					return XamlToRtfError.Unknown;
				}
				i++;
				if (this.IsSpace(xaml[i]))
				{
					return XamlToRtfError.Unknown;
				}
				if (xaml[i] == '/')
				{
					return this.HandleEndTag(xaml, i);
				}
				int num = i;
				i++;
				while (this.IsNameChar(xaml[i]))
				{
					i++;
				}
				this.AddParseData(xaml.Substring(num, i - num));
				while (i < xaml.Length)
				{
					while (this.IsSpace(xaml[i]))
					{
						i++;
					}
					if (i == xaml.Length - 1)
					{
						break;
					}
					if (xaml[i] == '/')
					{
						if (i == xaml.Length - 2)
						{
							this._empty = true;
							break;
						}
						return XamlToRtfError.Unknown;
					}
					else
					{
						num = i;
						i++;
						while (this.IsNameChar(xaml[i]))
						{
							i++;
						}
						this.AddParseData(xaml.Substring(num, i - num));
						if (i < xaml.Length)
						{
							while (this.IsSpace(xaml[i]))
							{
								i++;
							}
						}
						if (i == xaml.Length || xaml[i] != '=')
						{
							return XamlToRtfError.Unknown;
						}
						i++;
						while (this.IsSpace(xaml[i]))
						{
							i++;
						}
						if (xaml[i] != '\'' && xaml[i] != '"')
						{
							return XamlToRtfError.Unknown;
						}
						char c = xaml[i++];
						num = i;
						while (i < xaml.Length && xaml[i] != c)
						{
							i++;
						}
						if (i == xaml.Length)
						{
							return XamlToRtfError.Unknown;
						}
						this.AddParseData(xaml.Substring(num, i - num));
						i++;
					}
				}
				this._valid = true;
				return result;
			}

			// Token: 0x06008E60 RID: 36448 RVA: 0x00210379 File Offset: 0x0020F379
			internal void AddParseData(string parseData)
			{
				this.Add(parseData);
			}

			// Token: 0x17001F26 RID: 7974
			// (get) Token: 0x06008E61 RID: 36449 RVA: 0x00341281 File Offset: 0x00340281
			internal bool IsEmpty
			{
				get
				{
					return this._empty;
				}
			}

			// Token: 0x17001F27 RID: 7975
			// (get) Token: 0x06008E62 RID: 36450 RVA: 0x00341289 File Offset: 0x00340289
			internal bool IsValid
			{
				get
				{
					return this._valid;
				}
			}

			// Token: 0x06008E63 RID: 36451 RVA: 0x00340C58 File Offset: 0x0033FC58
			private bool IsSpace(char character)
			{
				return character == ' ' || character == '\t' || character == '\n' || character == '\r';
			}

			// Token: 0x06008E64 RID: 36452 RVA: 0x00341291 File Offset: 0x00340291
			private bool IsNameChar(char character)
			{
				return !this.IsSpace(character) && character != '=' && character != '>' && character != '/';
			}

			// Token: 0x06008E65 RID: 36453 RVA: 0x003412B0 File Offset: 0x003402B0
			private XamlToRtfError HandleEndTag(string xaml, int xamlIndex)
			{
				xamlIndex++;
				while (this.IsSpace(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				int num = xamlIndex;
				xamlIndex++;
				while (this.IsNameChar(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				this.AddParseData(xaml.Substring(num, xamlIndex - num));
				while (this.IsSpace(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				if (xamlIndex == xaml.Length - 1)
				{
					this._valid = true;
					return XamlToRtfError.None;
				}
				return XamlToRtfError.Unknown;
			}

			// Token: 0x04004928 RID: 18728
			private bool _empty;

			// Token: 0x04004929 RID: 18729
			private bool _valid;
		}

		// Token: 0x02000B88 RID: 2952
		internal class XamlToken
		{
			// Token: 0x06008E66 RID: 36454 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
			internal XamlToken()
			{
			}

			// Token: 0x17001F28 RID: 7976
			// (get) Token: 0x06008E67 RID: 36455 RVA: 0x00341330 File Offset: 0x00340330
			// (set) Token: 0x06008E68 RID: 36456 RVA: 0x00341338 File Offset: 0x00340338
			internal XamlTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
				set
				{
					this._tokenType = value;
				}
			}

			// Token: 0x17001F29 RID: 7977
			// (get) Token: 0x06008E69 RID: 36457 RVA: 0x00341341 File Offset: 0x00340341
			// (set) Token: 0x06008E6A RID: 36458 RVA: 0x00341349 File Offset: 0x00340349
			internal string Text
			{
				get
				{
					return this._text;
				}
				set
				{
					this._text = value;
				}
			}

			// Token: 0x0400492A RID: 18730
			private XamlTokenType _tokenType;

			// Token: 0x0400492B RID: 18731
			private string _text;
		}
	}
}
