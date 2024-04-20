using System;
using System.Collections;
using System.Text;
using System.Windows;
using MS.Utility;

namespace MS.Internal.Data
{
	// Token: 0x0200023B RID: 571
	internal class PathParser
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00155705 File Offset: 0x00154705
		public string Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0015570D File Offset: 0x0015470D
		private void SetError(string id, params object[] args)
		{
			this._error = SR.Get(id, args);
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0015571C File Offset: 0x0015471C
		public SourceValueInfo[] Parse(string path)
		{
			this._path = ((path != null) ? path.Trim() : string.Empty);
			this._n = this._path.Length;
			if (this._n == 0)
			{
				return new SourceValueInfo[]
				{
					new SourceValueInfo(SourceValueType.Direct, DrillIn.Never, null)
				};
			}
			this._index = 0;
			this._drillIn = DrillIn.IfNeeded;
			this._al.Clear();
			this._error = null;
			this._state = PathParser.State.Init;
			while (this._state != PathParser.State.Done)
			{
				char c = (this._index < this._n) ? this._path[this._index] : '\0';
				if (char.IsWhiteSpace(c))
				{
					this._index++;
				}
				else
				{
					switch (this._state)
					{
					case PathParser.State.Init:
						if (c == '\0' || c == '.' || c == '/')
						{
							this._state = PathParser.State.DrillIn;
						}
						else
						{
							this._state = PathParser.State.Prop;
						}
						break;
					case PathParser.State.DrillIn:
						if (c <= '.')
						{
							if (c != '\0')
							{
								if (c != '.')
								{
									goto IL_13E;
								}
								this._drillIn = DrillIn.Never;
								this._index++;
							}
						}
						else if (c != '/')
						{
							if (c != '[')
							{
								goto IL_13E;
							}
						}
						else
						{
							this._drillIn = DrillIn.Always;
							this._index++;
						}
						this._state = PathParser.State.Prop;
						break;
						IL_13E:
						this.SetError("PathSyntax", new object[]
						{
							this._path.Substring(0, this._index),
							this._path.Substring(this._index)
						});
						return PathParser.EmptyInfo;
					case PathParser.State.Prop:
					{
						bool flag = false;
						if (c == '[')
						{
							flag = true;
						}
						if (flag)
						{
							this.AddIndexer();
						}
						else
						{
							this.AddProperty();
						}
						break;
					}
					}
				}
			}
			SourceValueInfo[] array;
			if (this._error == null)
			{
				array = new SourceValueInfo[this._al.Count];
				this._al.CopyTo(array);
			}
			else
			{
				array = PathParser.EmptyInfo;
			}
			return array;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00155904 File Offset: 0x00154904
		private void AddProperty()
		{
			int index = this._index;
			int num = 0;
			while (this._index < this._n)
			{
				if (this._path[this._index] != '.')
				{
					break;
				}
				this._index++;
			}
			while (this._index < this._n && (num > 0 || PathParser.SpecialChars.IndexOf(this._path[this._index]) < 0))
			{
				if (this._path[this._index] == '(')
				{
					num++;
				}
				else if (this._path[this._index] == ')')
				{
					num--;
				}
				this._index++;
			}
			if (num > 0)
			{
				this.SetError("UnmatchedParen", new object[]
				{
					this._path.Substring(index)
				});
				return;
			}
			if (num < 0)
			{
				this.SetError("UnmatchedParen", new object[]
				{
					this._path.Substring(0, this._index)
				});
				return;
			}
			string text = this._path.Substring(index, this._index - index).Trim();
			SourceValueInfo sourceValueInfo = (text.Length > 0) ? new SourceValueInfo(SourceValueType.Property, this._drillIn, text) : new SourceValueInfo(SourceValueType.Direct, this._drillIn, null);
			this._al.Add(sourceValueInfo);
			this.StartNewLevel();
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00155A6C File Offset: 0x00154A6C
		private void AddIndexer()
		{
			int num = this._index + 1;
			this._index = num;
			int num2 = num;
			int num3 = 1;
			bool flag = false;
			bool flag2 = false;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			FrugalObjectList<IndexerParamInfo> frugalObjectList = new FrugalObjectList<IndexerParamInfo>();
			PathParser.IndexerState indexerState = PathParser.IndexerState.BeginParam;
			while (indexerState != PathParser.IndexerState.Done)
			{
				if (this._index >= this._n)
				{
					this.SetError("UnmatchedBracket", new object[]
					{
						this._path.Substring(num2 - 1)
					});
					return;
				}
				string path = this._path;
				num = this._index;
				this._index = num + 1;
				char c = path[num];
				if (c == '^' && !flag)
				{
					flag = true;
				}
				else
				{
					switch (indexerState)
					{
					case PathParser.IndexerState.BeginParam:
						if (flag)
						{
							indexerState = PathParser.IndexerState.ValueString;
							goto IL_108;
						}
						if (c == '(')
						{
							indexerState = PathParser.IndexerState.ParenString;
						}
						else if (!char.IsWhiteSpace(c))
						{
							indexerState = PathParser.IndexerState.ValueString;
							goto IL_108;
						}
						break;
					case PathParser.IndexerState.ParenString:
						if (flag)
						{
							stringBuilder.Append(c);
						}
						else if (c == ')')
						{
							indexerState = PathParser.IndexerState.ValueString;
						}
						else
						{
							stringBuilder.Append(c);
						}
						break;
					case PathParser.IndexerState.ValueString:
						goto IL_108;
					}
					IL_1C6:
					flag = false;
					continue;
					IL_108:
					if (flag)
					{
						stringBuilder2.Append(c);
						flag2 = false;
						goto IL_1C6;
					}
					if (num3 > 1)
					{
						stringBuilder2.Append(c);
						flag2 = false;
						if (c == ']')
						{
							num3--;
							goto IL_1C6;
						}
						goto IL_1C6;
					}
					else
					{
						if (char.IsWhiteSpace(c))
						{
							stringBuilder2.Append(c);
							flag2 = true;
							goto IL_1C6;
						}
						if (c == ',' || c == ']')
						{
							string paren = stringBuilder.ToString();
							string text = stringBuilder2.ToString();
							if (flag2)
							{
								text = text.TrimEnd();
							}
							frugalObjectList.Add(new IndexerParamInfo(paren, text));
							stringBuilder.Length = 0;
							stringBuilder2.Length = 0;
							flag2 = false;
							indexerState = ((c == ']') ? PathParser.IndexerState.Done : PathParser.IndexerState.BeginParam);
							goto IL_1C6;
						}
						stringBuilder2.Append(c);
						flag2 = false;
						if (c == '[')
						{
							num3++;
							goto IL_1C6;
						}
						goto IL_1C6;
					}
				}
			}
			SourceValueInfo sourceValueInfo = new SourceValueInfo(SourceValueType.Indexer, this._drillIn, frugalObjectList);
			this._al.Add(sourceValueInfo);
			this.StartNewLevel();
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00155C72 File Offset: 0x00154C72
		private void StartNewLevel()
		{
			this._state = ((this._index < this._n) ? PathParser.State.DrillIn : PathParser.State.Done);
			this._drillIn = DrillIn.Never;
		}

		// Token: 0x04000C27 RID: 3111
		private string _error;

		// Token: 0x04000C28 RID: 3112
		private PathParser.State _state;

		// Token: 0x04000C29 RID: 3113
		private string _path;

		// Token: 0x04000C2A RID: 3114
		private int _index;

		// Token: 0x04000C2B RID: 3115
		private int _n;

		// Token: 0x04000C2C RID: 3116
		private DrillIn _drillIn;

		// Token: 0x04000C2D RID: 3117
		private ArrayList _al = new ArrayList();

		// Token: 0x04000C2E RID: 3118
		private const char NullChar = '\0';

		// Token: 0x04000C2F RID: 3119
		private const char EscapeChar = '^';

		// Token: 0x04000C30 RID: 3120
		private static SourceValueInfo[] EmptyInfo = Array.Empty<SourceValueInfo>();

		// Token: 0x04000C31 RID: 3121
		private static string SpecialChars = "./[]";

		// Token: 0x020009F7 RID: 2551
		private enum State
		{
			// Token: 0x0400402F RID: 16431
			Init,
			// Token: 0x04004030 RID: 16432
			DrillIn,
			// Token: 0x04004031 RID: 16433
			Prop,
			// Token: 0x04004032 RID: 16434
			Done
		}

		// Token: 0x020009F8 RID: 2552
		private enum IndexerState
		{
			// Token: 0x04004034 RID: 16436
			BeginParam,
			// Token: 0x04004035 RID: 16437
			ParenString,
			// Token: 0x04004036 RID: 16438
			ValueString,
			// Token: 0x04004037 RID: 16439
			Done
		}
	}
}
