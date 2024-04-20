using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public class CookieCollection : ICollection, IEnumerable
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0000C921 File Offset: 0x0000AB21
		public CookieCollection()
		{
			this._list = new List<Cookie>();
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000C938 File Offset: 0x0000AB38
		internal IList<Cookie> List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000C950 File Offset: 0x0000AB50
		internal IEnumerable<Cookie> Sorted
		{
			get
			{
				List<Cookie> list = new List<Cookie>(this._list);
				bool flag = list.Count > 1;
				if (flag)
				{
					list.Sort(new Comparison<Cookie>(CookieCollection.compareCookieWithinSorted));
				}
				return list;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000C990 File Offset: 0x0000AB90
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000C9B0 File Offset: 0x0000ABB0
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000072 RID: 114
		public Cookie this[int index]
		{
			get
			{
				bool flag = index < 0 || index >= this._list.Count;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this._list[index];
			}
		}

		// Token: 0x17000073 RID: 115
		public Cookie this[string name]
		{
			get
			{
				bool flag = name == null;
				if (flag)
				{
					throw new ArgumentNullException("name");
				}
				foreach (Cookie cookie in this.Sorted)
				{
					bool flag2 = cookie.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
					if (flag2)
					{
						return cookie;
					}
				}
				return null;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		public object SyncRoot
		{
			get
			{
				object result;
				if ((result = this._sync) == null)
				{
					result = (this._sync = ((ICollection)this._list).SyncRoot);
				}
				return result;
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000CACC File Offset: 0x0000ACCC
		private static int compareCookieWithinSort(Cookie x, Cookie y)
		{
			return x.Name.Length + x.Value.Length - (y.Name.Length + y.Value.Length);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000CB10 File Offset: 0x0000AD10
		private static int compareCookieWithinSorted(Cookie x, Cookie y)
		{
			int num;
			return ((num = x.Version - y.Version) != 0) ? num : (((num = x.Name.CompareTo(y.Name)) != 0) ? num : (y.Path.Length - x.Path.Length));
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000CB68 File Offset: 0x0000AD68
		private static CookieCollection parseRequest(string value)
		{
			CookieCollection cookieCollection = new CookieCollection();
			Cookie cookie = null;
			int num = 0;
			string[] array = CookieCollection.splitCookieHeaderValue(value);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				bool flag = text.Length == 0;
				if (!flag)
				{
					bool flag2 = text.StartsWith("$version", StringComparison.InvariantCultureIgnoreCase);
					if (flag2)
					{
						num = int.Parse(text.GetValue('=', true));
					}
					else
					{
						bool flag3 = text.StartsWith("$path", StringComparison.InvariantCultureIgnoreCase);
						if (flag3)
						{
							bool flag4 = cookie != null;
							if (flag4)
							{
								cookie.Path = text.GetValue('=');
							}
						}
						else
						{
							bool flag5 = text.StartsWith("$domain", StringComparison.InvariantCultureIgnoreCase);
							if (flag5)
							{
								bool flag6 = cookie != null;
								if (flag6)
								{
									cookie.Domain = text.GetValue('=');
								}
							}
							else
							{
								bool flag7 = text.StartsWith("$port", StringComparison.InvariantCultureIgnoreCase);
								if (flag7)
								{
									string port = text.Equals("$port", StringComparison.InvariantCultureIgnoreCase) ? "\"\"" : text.GetValue('=');
									bool flag8 = cookie != null;
									if (flag8)
									{
										cookie.Port = port;
									}
								}
								else
								{
									bool flag9 = cookie != null;
									if (flag9)
									{
										cookieCollection.Add(cookie);
									}
									string value2 = string.Empty;
									int num2 = text.IndexOf('=');
									bool flag10 = num2 == -1;
									string name;
									if (flag10)
									{
										name = text;
									}
									else
									{
										bool flag11 = num2 == text.Length - 1;
										if (flag11)
										{
											name = text.Substring(0, num2).TrimEnd(new char[]
											{
												' '
											});
										}
										else
										{
											name = text.Substring(0, num2).TrimEnd(new char[]
											{
												' '
											});
											value2 = text.Substring(num2 + 1).TrimStart(new char[]
											{
												' '
											});
										}
									}
									cookie = new Cookie(name, value2);
									bool flag12 = num != 0;
									if (flag12)
									{
										cookie.Version = num;
									}
								}
							}
						}
					}
				}
			}
			bool flag13 = cookie != null;
			if (flag13)
			{
				cookieCollection.Add(cookie);
			}
			return cookieCollection;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000CD84 File Offset: 0x0000AF84
		private static CookieCollection parseResponse(string value)
		{
			CookieCollection cookieCollection = new CookieCollection();
			Cookie cookie = null;
			string[] array = CookieCollection.splitCookieHeaderValue(value);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				bool flag = text.Length == 0;
				if (!flag)
				{
					bool flag2 = text.StartsWith("version", StringComparison.InvariantCultureIgnoreCase);
					if (flag2)
					{
						bool flag3 = cookie != null;
						if (flag3)
						{
							cookie.Version = int.Parse(text.GetValue('=', true));
						}
					}
					else
					{
						bool flag4 = text.StartsWith("expires", StringComparison.InvariantCultureIgnoreCase);
						if (flag4)
						{
							StringBuilder stringBuilder = new StringBuilder(text.GetValue('='), 32);
							bool flag5 = i < array.Length - 1;
							if (flag5)
							{
								stringBuilder.AppendFormat(", {0}", array[++i].Trim());
							}
							DateTime now;
							bool flag6 = !DateTime.TryParseExact(stringBuilder.ToString(), new string[]
							{
								"ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'",
								"r"
							}, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out now);
							if (flag6)
							{
								now = DateTime.Now;
							}
							bool flag7 = cookie != null && cookie.Expires == DateTime.MinValue;
							if (flag7)
							{
								cookie.Expires = now.ToLocalTime();
							}
						}
						else
						{
							bool flag8 = text.StartsWith("max-age", StringComparison.InvariantCultureIgnoreCase);
							if (flag8)
							{
								int num = int.Parse(text.GetValue('=', true));
								DateTime expires = DateTime.Now.AddSeconds((double)num);
								bool flag9 = cookie != null;
								if (flag9)
								{
									cookie.Expires = expires;
								}
							}
							else
							{
								bool flag10 = text.StartsWith("path", StringComparison.InvariantCultureIgnoreCase);
								if (flag10)
								{
									bool flag11 = cookie != null;
									if (flag11)
									{
										cookie.Path = text.GetValue('=');
									}
								}
								else
								{
									bool flag12 = text.StartsWith("domain", StringComparison.InvariantCultureIgnoreCase);
									if (flag12)
									{
										bool flag13 = cookie != null;
										if (flag13)
										{
											cookie.Domain = text.GetValue('=');
										}
									}
									else
									{
										bool flag14 = text.StartsWith("port", StringComparison.InvariantCultureIgnoreCase);
										if (flag14)
										{
											string port = text.Equals("port", StringComparison.InvariantCultureIgnoreCase) ? "\"\"" : text.GetValue('=');
											bool flag15 = cookie != null;
											if (flag15)
											{
												cookie.Port = port;
											}
										}
										else
										{
											bool flag16 = text.StartsWith("comment", StringComparison.InvariantCultureIgnoreCase);
											if (flag16)
											{
												bool flag17 = cookie != null;
												if (flag17)
												{
													cookie.Comment = CookieCollection.urlDecode(text.GetValue('='), Encoding.UTF8);
												}
											}
											else
											{
												bool flag18 = text.StartsWith("commenturl", StringComparison.InvariantCultureIgnoreCase);
												if (flag18)
												{
													bool flag19 = cookie != null;
													if (flag19)
													{
														cookie.CommentUri = text.GetValue('=', true).ToUri();
													}
												}
												else
												{
													bool flag20 = text.StartsWith("discard", StringComparison.InvariantCultureIgnoreCase);
													if (flag20)
													{
														bool flag21 = cookie != null;
														if (flag21)
														{
															cookie.Discard = true;
														}
													}
													else
													{
														bool flag22 = text.StartsWith("secure", StringComparison.InvariantCultureIgnoreCase);
														if (flag22)
														{
															bool flag23 = cookie != null;
															if (flag23)
															{
																cookie.Secure = true;
															}
														}
														else
														{
															bool flag24 = text.StartsWith("httponly", StringComparison.InvariantCultureIgnoreCase);
															if (flag24)
															{
																bool flag25 = cookie != null;
																if (flag25)
																{
																	cookie.HttpOnly = true;
																}
															}
															else
															{
																bool flag26 = cookie != null;
																if (flag26)
																{
																	cookieCollection.Add(cookie);
																}
																string value2 = string.Empty;
																int num2 = text.IndexOf('=');
																bool flag27 = num2 == -1;
																string name;
																if (flag27)
																{
																	name = text;
																}
																else
																{
																	bool flag28 = num2 == text.Length - 1;
																	if (flag28)
																	{
																		name = text.Substring(0, num2).TrimEnd(new char[]
																		{
																			' '
																		});
																	}
																	else
																	{
																		name = text.Substring(0, num2).TrimEnd(new char[]
																		{
																			' '
																		});
																		value2 = text.Substring(num2 + 1).TrimStart(new char[]
																		{
																			' '
																		});
																	}
																}
																cookie = new Cookie(name, value2);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag29 = cookie != null;
			if (flag29)
			{
				cookieCollection.Add(cookie);
			}
			return cookieCollection;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000D1A4 File Offset: 0x0000B3A4
		private int searchCookie(Cookie cookie)
		{
			string name = cookie.Name;
			string path = cookie.Path;
			string domain = cookie.Domain;
			int version = cookie.Version;
			for (int i = this._list.Count - 1; i >= 0; i--)
			{
				Cookie cookie2 = this._list[i];
				bool flag = cookie2.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && cookie2.Path.Equals(path, StringComparison.InvariantCulture) && cookie2.Domain.Equals(domain, StringComparison.InvariantCultureIgnoreCase) && cookie2.Version == version;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D254 File Offset: 0x0000B454
		private static string[] splitCookieHeaderValue(string value)
		{
			return new List<string>(value.SplitHeaderValue(new char[]
			{
				',',
				';'
			})).ToArray();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000D288 File Offset: 0x0000B488
		private static string urlDecode(string s, Encoding encoding)
		{
			bool flag = s == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = s.IndexOfAny(new char[]
				{
					'%',
					'+'
				}) == -1;
				if (flag2)
				{
					result = s;
				}
				else
				{
					try
					{
						result = HttpUtility.UrlDecode(s, encoding);
					}
					catch
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
		internal static CookieCollection Parse(string value, bool response)
		{
			return response ? CookieCollection.parseResponse(value) : CookieCollection.parseRequest(value);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D30C File Offset: 0x0000B50C
		internal void SetOrRemove(Cookie cookie)
		{
			int num = this.searchCookie(cookie);
			bool flag = num == -1;
			if (flag)
			{
				bool flag2 = !cookie.Expired;
				if (flag2)
				{
					this._list.Add(cookie);
				}
			}
			else
			{
				bool flag3 = !cookie.Expired;
				if (flag3)
				{
					this._list[num] = cookie;
				}
				else
				{
					this._list.RemoveAt(num);
				}
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D374 File Offset: 0x0000B574
		internal void SetOrRemove(CookieCollection cookies)
		{
			foreach (object obj in cookies)
			{
				Cookie orRemove = (Cookie)obj;
				this.SetOrRemove(orRemove);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		internal void Sort()
		{
			bool flag = this._list.Count > 1;
			if (flag)
			{
				this._list.Sort(new Comparison<Cookie>(CookieCollection.compareCookieWithinSort));
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D404 File Offset: 0x0000B604
		public void Add(Cookie cookie)
		{
			bool flag = cookie == null;
			if (flag)
			{
				throw new ArgumentNullException("cookie");
			}
			int num = this.searchCookie(cookie);
			bool flag2 = num == -1;
			if (flag2)
			{
				this._list.Add(cookie);
			}
			else
			{
				this._list[num] = cookie;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D454 File Offset: 0x0000B654
		public void Add(CookieCollection cookies)
		{
			bool flag = cookies == null;
			if (flag)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				this.Add(cookie);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public void CopyTo(Array array, int index)
		{
			bool flag = array == null;
			if (flag)
			{
				throw new ArgumentNullException("array");
			}
			bool flag2 = index < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("index", "Less than zero.");
			}
			bool flag3 = array.Rank > 1;
			if (flag3)
			{
				throw new ArgumentException("Multidimensional.", "array");
			}
			bool flag4 = array.Length - index < this._list.Count;
			if (flag4)
			{
				throw new ArgumentException("The number of elements in this collection is greater than the available space of the destination array.");
			}
			bool flag5 = !array.GetType().GetElementType().IsAssignableFrom(typeof(Cookie));
			if (flag5)
			{
				throw new InvalidCastException("The elements in this collection cannot be cast automatically to the type of the destination array.");
			}
			((ICollection)this._list).CopyTo(array, index);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000D578 File Offset: 0x0000B778
		public void CopyTo(Cookie[] array, int index)
		{
			bool flag = array == null;
			if (flag)
			{
				throw new ArgumentNullException("array");
			}
			bool flag2 = index < 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("index", "Less than zero.");
			}
			bool flag3 = array.Length - index < this._list.Count;
			if (flag3)
			{
				throw new ArgumentException("The number of elements in this collection is greater than the available space of the destination array.");
			}
			this._list.CopyTo(array, index);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		public IEnumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		// Token: 0x040000B8 RID: 184
		private List<Cookie> _list;

		// Token: 0x040000B9 RID: 185
		private object _sync;
	}
}
