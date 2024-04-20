using System;
using System.Collections.Specialized;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x02000038 RID: 56
	internal sealed class QueryStringCollection : NameValueCollection
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00015970 File Offset: 0x00013B70
		public QueryStringCollection()
		{
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00016D79 File Offset: 0x00014F79
		public QueryStringCollection(int capacity) : base(capacity)
		{
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00016D84 File Offset: 0x00014F84
		private static string urlDecode(string s, Encoding encoding)
		{
			return (s.IndexOfAny(new char[]
			{
				'%',
				'+'
			}) > -1) ? HttpUtility.UrlDecode(s, encoding) : s;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00016DBC File Offset: 0x00014FBC
		public static QueryStringCollection Parse(string query)
		{
			return QueryStringCollection.Parse(query, Encoding.UTF8);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00016DDC File Offset: 0x00014FDC
		public static QueryStringCollection Parse(string query, Encoding encoding)
		{
			bool flag = query == null;
			QueryStringCollection result;
			if (flag)
			{
				result = new QueryStringCollection(1);
			}
			else
			{
				int length = query.Length;
				bool flag2 = length == 0;
				if (flag2)
				{
					result = new QueryStringCollection(1);
				}
				else
				{
					bool flag3 = query == "?";
					if (flag3)
					{
						result = new QueryStringCollection(1);
					}
					else
					{
						bool flag4 = query[0] == '?';
						if (flag4)
						{
							query = query.Substring(1);
						}
						bool flag5 = encoding == null;
						if (flag5)
						{
							encoding = Encoding.UTF8;
						}
						QueryStringCollection queryStringCollection = new QueryStringCollection();
						string[] array = query.Split(new char[]
						{
							'&'
						});
						foreach (string text in array)
						{
							length = text.Length;
							bool flag6 = length == 0;
							if (!flag6)
							{
								bool flag7 = text == "=";
								if (!flag7)
								{
									int num = text.IndexOf('=');
									bool flag8 = num < 0;
									if (flag8)
									{
										queryStringCollection.Add(null, QueryStringCollection.urlDecode(text, encoding));
									}
									else
									{
										bool flag9 = num == 0;
										if (flag9)
										{
											queryStringCollection.Add(null, QueryStringCollection.urlDecode(text.Substring(1), encoding));
										}
										else
										{
											string name = QueryStringCollection.urlDecode(text.Substring(0, num), encoding);
											int num2 = num + 1;
											string value = (num2 < length) ? QueryStringCollection.urlDecode(text.Substring(num2), encoding) : string.Empty;
											queryStringCollection.Add(name, value);
										}
									}
								}
							}
						}
						result = queryStringCollection;
					}
				}
			}
			return result;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00016F68 File Offset: 0x00015168
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in this.AllKeys)
			{
				stringBuilder.AppendFormat("{0}={1}&", text, base[text]);
			}
			bool flag = stringBuilder.Length > 0;
			if (flag)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				int length = stringBuilder2.Length;
				stringBuilder2.Length = length - 1;
			}
			return stringBuilder.ToString();
		}
	}
}
