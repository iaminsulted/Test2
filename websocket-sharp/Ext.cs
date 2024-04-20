using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	// Token: 0x02000002 RID: 2
	public static class Ext
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static byte[] compress(this byte[] data)
		{
			bool flag = (long)data.Length == 0L;
			byte[] result;
			if (flag)
			{
				result = data;
			}
			else
			{
				using (MemoryStream memoryStream = new MemoryStream(data))
				{
					result = memoryStream.compressToArray();
				}
			}
			return result;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000209C File Offset: 0x0000029C
		private static MemoryStream compress(this Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream();
			bool flag = stream.Length == 0L;
			MemoryStream result;
			if (flag)
			{
				result = memoryStream;
			}
			else
			{
				stream.Position = 0L;
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
				{
					stream.CopyTo(deflateStream, 1024);
					deflateStream.Close();
					memoryStream.Write(Ext._last, 0, 1);
					memoryStream.Position = 0L;
					result = memoryStream;
				}
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002120 File Offset: 0x00000320
		private static byte[] compressToArray(this Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = stream.compress())
			{
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002164 File Offset: 0x00000364
		private static byte[] decompress(this byte[] data)
		{
			bool flag = (long)data.Length == 0L;
			byte[] result;
			if (flag)
			{
				result = data;
			}
			else
			{
				using (MemoryStream memoryStream = new MemoryStream(data))
				{
					result = memoryStream.decompressToArray();
				}
			}
			return result;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021B0 File Offset: 0x000003B0
		private static MemoryStream decompress(this Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream();
			bool flag = stream.Length == 0L;
			MemoryStream result;
			if (flag)
			{
				result = memoryStream;
			}
			else
			{
				stream.Position = 0L;
				using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
				{
					deflateStream.CopyTo(memoryStream, 1024);
					memoryStream.Position = 0L;
					result = memoryStream;
				}
			}
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002220 File Offset: 0x00000420
		private static byte[] decompressToArray(this Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = stream.decompress())
			{
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002264 File Offset: 0x00000464
		private static bool isHttpMethod(this string value)
		{
			return value == "GET" || value == "HEAD" || value == "POST" || value == "PUT" || value == "DELETE" || value == "CONNECT" || value == "OPTIONS" || value == "TRACE";
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022E0 File Offset: 0x000004E0
		private static bool isHttpMethod10(this string value)
		{
			return value == "GET" || value == "HEAD" || value == "POST";
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000231C File Offset: 0x0000051C
		private static void times(this ulong n, Action action)
		{
			for (ulong num = 0UL; num < n; num += 1UL)
			{
				action();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002344 File Offset: 0x00000544
		internal static byte[] Append(this ushort code, string reason)
		{
			byte[] array = code.InternalToByteArray(ByteOrder.Big);
			bool flag = reason != null && reason.Length > 0;
			if (flag)
			{
				List<byte> list = new List<byte>(array);
				list.AddRange(Encoding.UTF8.GetBytes(reason));
				array = list.ToArray();
			}
			return array;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002394 File Offset: 0x00000594
		internal static void Close(this WebSocketSharp.Net.HttpListenerResponse response, WebSocketSharp.Net.HttpStatusCode code)
		{
			response.StatusCode = (int)code;
			response.OutputStream.Close();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023AB File Offset: 0x000005AB
		internal static void CloseWithAuthChallenge(this WebSocketSharp.Net.HttpListenerResponse response, string challenge)
		{
			response.Headers.InternalSet("WWW-Authenticate", challenge, true);
			response.Close(WebSocketSharp.Net.HttpStatusCode.Unauthorized);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023D0 File Offset: 0x000005D0
		internal static byte[] Compress(this byte[] data, CompressionMethod method)
		{
			return (method == CompressionMethod.Deflate) ? data.compress() : data;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023F0 File Offset: 0x000005F0
		internal static Stream Compress(this Stream stream, CompressionMethod method)
		{
			return (method == CompressionMethod.Deflate) ? stream.compress() : stream;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002410 File Offset: 0x00000610
		internal static byte[] CompressToArray(this Stream stream, CompressionMethod method)
		{
			return (method == CompressionMethod.Deflate) ? stream.compressToArray() : stream.ToByteArray();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002434 File Offset: 0x00000634
		internal static bool Contains(this string value, params char[] anyOf)
		{
			return anyOf != null && anyOf.Length != 0 && value.IndexOfAny(anyOf) > -1;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000245C File Offset: 0x0000065C
		internal static bool Contains(this NameValueCollection collection, string name)
		{
			return collection[name] != null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002478 File Offset: 0x00000678
		internal static bool Contains(this NameValueCollection collection, string name, string value, StringComparison comparisonTypeForValue)
		{
			string text = collection[name];
			bool flag = text == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (string text2 in text.Split(new char[]
				{
					','
				}))
				{
					bool flag2 = text2.Trim().Equals(value, comparisonTypeForValue);
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024E4 File Offset: 0x000006E4
		internal static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> condition)
		{
			foreach (T arg in source)
			{
				bool flag = condition(arg);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002540 File Offset: 0x00000740
		internal static bool ContainsTwice(this string[] values)
		{
			int len = values.Length;
			int end = len - 1;
			Func<int, bool> seek = null;
			seek = delegate(int idx)
			{
				bool flag = idx == end;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					string b = values[idx];
					for (int i = idx + 1; i < len; i++)
					{
						bool flag2 = values[i] == b;
						if (flag2)
						{
							return true;
						}
					}
					result = seek(++idx);
				}
				return result;
			};
			return seek(0);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025A0 File Offset: 0x000007A0
		internal static T[] Copy<T>(this T[] source, int length)
		{
			T[] array = new T[length];
			Array.Copy(source, 0, array, 0, length);
			return array;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025C8 File Offset: 0x000007C8
		internal static T[] Copy<T>(this T[] source, long length)
		{
			T[] array = new T[length];
			Array.Copy(source, 0L, array, 0L, length);
			return array;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025F0 File Offset: 0x000007F0
		internal static void CopyTo(this Stream source, Stream destination, int bufferLength)
		{
			byte[] buffer = new byte[bufferLength];
			int count;
			while ((count = source.Read(buffer, 0, bufferLength)) > 0)
			{
				destination.Write(buffer, 0, count);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002628 File Offset: 0x00000828
		internal static void CopyToAsync(this Stream source, Stream destination, int bufferLength, Action completed, Action<Exception> error)
		{
			byte[] buff = new byte[bufferLength];
			AsyncCallback callback = null;
			callback = delegate(IAsyncResult ar)
			{
				try
				{
					int num = source.EndRead(ar);
					bool flag2 = num <= 0;
					if (flag2)
					{
						bool flag3 = completed != null;
						if (flag3)
						{
							completed();
						}
					}
					else
					{
						destination.Write(buff, 0, num);
						source.BeginRead(buff, 0, bufferLength, callback, null);
					}
				}
				catch (Exception obj2)
				{
					bool flag4 = error != null;
					if (flag4)
					{
						error(obj2);
					}
				}
			};
			try
			{
				source.BeginRead(buff, 0, bufferLength, callback, null);
			}
			catch (Exception obj)
			{
				bool flag = error != null;
				if (flag)
				{
					error(obj);
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026E0 File Offset: 0x000008E0
		internal static byte[] Decompress(this byte[] data, CompressionMethod method)
		{
			return (method == CompressionMethod.Deflate) ? data.decompress() : data;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002700 File Offset: 0x00000900
		internal static Stream Decompress(this Stream stream, CompressionMethod method)
		{
			return (method == CompressionMethod.Deflate) ? stream.decompress() : stream;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002720 File Offset: 0x00000920
		internal static byte[] DecompressToArray(this Stream stream, CompressionMethod method)
		{
			return (method == CompressionMethod.Deflate) ? stream.decompressToArray() : stream.ToByteArray();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002744 File Offset: 0x00000944
		internal static bool EqualsWith(this int value, char c, Action<int> action)
		{
			action(value);
			return value == (int)c;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002764 File Offset: 0x00000964
		internal static string GetAbsolutePath(this Uri uri)
		{
			bool isAbsoluteUri = uri.IsAbsoluteUri;
			string result;
			if (isAbsoluteUri)
			{
				result = uri.AbsolutePath;
			}
			else
			{
				string originalString = uri.OriginalString;
				bool flag = originalString[0] != '/';
				if (flag)
				{
					result = null;
				}
				else
				{
					int num = originalString.IndexOfAny(new char[]
					{
						'?',
						'#'
					});
					result = ((num > 0) ? originalString.Substring(0, num) : originalString);
				}
			}
			return result;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027D0 File Offset: 0x000009D0
		internal static WebSocketSharp.Net.CookieCollection GetCookies(this NameValueCollection headers, bool response)
		{
			string text = headers[response ? "Set-Cookie" : "Cookie"];
			return (text != null) ? WebSocketSharp.Net.CookieCollection.Parse(text, response) : new WebSocketSharp.Net.CookieCollection();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000280C File Offset: 0x00000A0C
		internal static string GetDnsSafeHost(this Uri uri, bool bracketIPv6)
		{
			return (bracketIPv6 && uri.HostNameType == UriHostNameType.IPv6) ? uri.Host : uri.DnsSafeHost;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002838 File Offset: 0x00000A38
		internal static string GetMessage(this CloseStatusCode code)
		{
			return (code == CloseStatusCode.ProtocolError) ? "A WebSocket protocol error has occurred." : ((code == CloseStatusCode.UnsupportedData) ? "Unsupported data has been received." : ((code == CloseStatusCode.Abnormal) ? "An exception has occurred." : ((code == CloseStatusCode.InvalidData) ? "Invalid data has been received." : ((code == CloseStatusCode.PolicyViolation) ? "A policy violation has occurred." : ((code == CloseStatusCode.TooBig) ? "A too big message has been received." : ((code == CloseStatusCode.MandatoryExtension) ? "WebSocket client didn't receive expected extension(s)." : ((code == CloseStatusCode.ServerError) ? "WebSocket server got an internal error." : ((code == CloseStatusCode.TlsHandshakeFailure) ? "An error has occurred during a TLS handshake." : string.Empty))))))));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028D8 File Offset: 0x00000AD8
		internal static string GetName(this string nameAndValue, char separator)
		{
			int num = nameAndValue.IndexOf(separator);
			return (num > 0) ? nameAndValue.Substring(0, num).Trim() : null;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002908 File Offset: 0x00000B08
		internal static string GetValue(this string nameAndValue, char separator)
		{
			return nameAndValue.GetValue(separator, false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002924 File Offset: 0x00000B24
		internal static string GetValue(this string nameAndValue, char separator, bool unquote)
		{
			int num = nameAndValue.IndexOf(separator);
			bool flag = num < 0 || num == nameAndValue.Length - 1;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string text = nameAndValue.Substring(num + 1).Trim();
				result = (unquote ? text.Unquote() : text);
			}
			return result;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002974 File Offset: 0x00000B74
		internal static byte[] InternalToByteArray(this ushort value, ByteOrder order)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			bool flag = !order.IsHostOrder();
			if (flag)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000029A4 File Offset: 0x00000BA4
		internal static byte[] InternalToByteArray(this ulong value, ByteOrder order)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			bool flag = !order.IsHostOrder();
			if (flag)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029D4 File Offset: 0x00000BD4
		internal static bool IsCompressionExtension(this string value, CompressionMethod method)
		{
			return value.StartsWith(method.ToExtensionString(new string[0]));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029F8 File Offset: 0x00000BF8
		internal static bool IsControl(this byte opcode)
		{
			return opcode > 7 && opcode < 16;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A18 File Offset: 0x00000C18
		internal static bool IsControl(this Opcode opcode)
		{
			return opcode >= Opcode.Close;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A34 File Offset: 0x00000C34
		internal static bool IsData(this byte opcode)
		{
			return opcode == 1 || opcode == 2;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A54 File Offset: 0x00000C54
		internal static bool IsData(this Opcode opcode)
		{
			return opcode == Opcode.Text || opcode == Opcode.Binary;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A74 File Offset: 0x00000C74
		internal static bool IsHttpMethod(this string value, Version version)
		{
			return (version == WebSocketSharp.Net.HttpVersion.Version10) ? value.isHttpMethod10() : value.isHttpMethod();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002AA4 File Offset: 0x00000CA4
		internal static bool IsPortNumber(this int value)
		{
			return value > 0 && value < 65536;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AC8 File Offset: 0x00000CC8
		internal static bool IsReserved(this ushort code)
		{
			return code == 1004 || code == 1005 || code == 1006 || code == 1015;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B00 File Offset: 0x00000D00
		internal static bool IsReserved(this CloseStatusCode code)
		{
			return code == CloseStatusCode.Undefined || code == CloseStatusCode.NoStatus || code == CloseStatusCode.Abnormal || code == CloseStatusCode.TlsHandshakeFailure;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B38 File Offset: 0x00000D38
		internal static bool IsSupported(this byte opcode)
		{
			return Enum.IsDefined(typeof(Opcode), opcode);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B60 File Offset: 0x00000D60
		internal static bool IsText(this string value)
		{
			int length = value.Length;
			for (int i = 0; i < length; i++)
			{
				char c = value[i];
				bool flag = c < ' ';
				if (flag)
				{
					bool flag2 = "\r\n\t".IndexOf(c) == -1;
					if (flag2)
					{
						return false;
					}
					bool flag3 = c == '\n';
					if (flag3)
					{
						i++;
						bool flag4 = i == length;
						if (flag4)
						{
							break;
						}
						c = value[i];
						bool flag5 = " \t".IndexOf(c) == -1;
						if (flag5)
						{
							return false;
						}
					}
				}
				else
				{
					bool flag6 = c == '\u007f';
					if (flag6)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002C14 File Offset: 0x00000E14
		internal static bool IsToken(this string value)
		{
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				bool flag = c < ' ';
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = c >= '\u007f';
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = "()<>@,;:\\\"/[]?={} \t".IndexOf(c) > -1;
						if (!flag3)
						{
							i++;
							continue;
						}
						result = false;
					}
				}
				return result;
			}
			return true;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002C84 File Offset: 0x00000E84
		internal static bool KeepsAlive(this NameValueCollection headers, Version version)
		{
			StringComparison comparisonTypeForValue = StringComparison.OrdinalIgnoreCase;
			return (version < WebSocketSharp.Net.HttpVersion.Version11) ? headers.Contains("Connection", "keep-alive", comparisonTypeForValue) : (!headers.Contains("Connection", "close", comparisonTypeForValue));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CCC File Offset: 0x00000ECC
		internal static string Quote(this string value)
		{
			return string.Format("\"{0}\"", value.Replace("\"", "\\\""));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CF8 File Offset: 0x00000EF8
		internal static byte[] ReadBytes(this Stream stream, int length)
		{
			byte[] array = new byte[length];
			int num = 0;
			try
			{
				while (length > 0)
				{
					int num2 = stream.Read(array, num, length);
					bool flag = num2 == 0;
					if (flag)
					{
						break;
					}
					num += num2;
					length -= num2;
				}
			}
			catch
			{
			}
			return array.SubArray(0, num);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002D64 File Offset: 0x00000F64
		internal static byte[] ReadBytes(this Stream stream, long length, int bufferLength)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					byte[] buffer = new byte[bufferLength];
					while (length > 0L)
					{
						bool flag = length < (long)bufferLength;
						if (flag)
						{
							bufferLength = (int)length;
						}
						int num = stream.Read(buffer, 0, bufferLength);
						bool flag2 = num == 0;
						if (flag2)
						{
							break;
						}
						memoryStream.Write(buffer, 0, num);
						length -= (long)num;
					}
				}
				catch
				{
				}
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E08 File Offset: 0x00001008
		internal static void ReadBytesAsync(this Stream stream, int length, Action<byte[]> completed, Action<Exception> error)
		{
			byte[] buff = new byte[length];
			int offset = 0;
			int retry = 0;
			AsyncCallback callback = null;
			callback = delegate(IAsyncResult ar)
			{
				try
				{
					int num = stream.EndRead(ar);
					int retry;
					bool flag2 = num == 0 && retry < Ext._retry;
					if (flag2)
					{
						retry = retry;
						retry++;
						stream.BeginRead(buff, offset, length, callback, null);
					}
					else
					{
						bool flag3 = num == 0 || num == length;
						if (flag3)
						{
							bool flag4 = completed != null;
							if (flag4)
							{
								completed(buff.SubArray(0, offset + num));
							}
						}
						else
						{
							retry = 0;
							offset += num;
							length -= num;
							stream.BeginRead(buff, offset, length, callback, null);
						}
					}
				}
				catch (Exception obj2)
				{
					bool flag5 = error != null;
					if (flag5)
					{
						error(obj2);
					}
				}
			};
			try
			{
				stream.BeginRead(buff, offset, length, callback, null);
			}
			catch (Exception obj)
			{
				bool flag = error != null;
				if (flag)
				{
					error(obj);
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002EC8 File Offset: 0x000010C8
		internal static void ReadBytesAsync(this Stream stream, long length, int bufferLength, Action<byte[]> completed, Action<Exception> error)
		{
			MemoryStream dest = new MemoryStream();
			byte[] buff = new byte[bufferLength];
			int retry = 0;
			Action<long> read = null;
			read = delegate(long len)
			{
				bool flag2 = len < (long)bufferLength;
				if (flag2)
				{
					bufferLength = (int)len;
				}
				stream.BeginRead(buff, 0, bufferLength, delegate(IAsyncResult ar)
				{
					try
					{
						int num = stream.EndRead(ar);
						bool flag3 = num > 0;
						if (flag3)
						{
							dest.Write(buff, 0, num);
						}
						int retry;
						bool flag4 = num == 0 && retry < Ext._retry;
						if (flag4)
						{
							retry = retry;
							retry++;
							read(len);
						}
						else
						{
							bool flag5 = num == 0 || (long)num == len;
							if (flag5)
							{
								bool flag6 = completed != null;
								if (flag6)
								{
									dest.Close();
									completed(dest.ToArray());
								}
								dest.Dispose();
							}
							else
							{
								retry = 0;
								read(len - (long)num);
							}
						}
					}
					catch (Exception obj2)
					{
						dest.Dispose();
						bool flag7 = error != null;
						if (flag7)
						{
							error(obj2);
						}
					}
				}, null);
			};
			try
			{
				read(length);
			}
			catch (Exception obj)
			{
				dest.Dispose();
				bool flag = error != null;
				if (flag)
				{
					error(obj);
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F84 File Offset: 0x00001184
		internal static T[] Reverse<T>(this T[] array)
		{
			int num = array.Length;
			T[] array2 = new T[num];
			int num2 = num - 1;
			for (int i = 0; i <= num2; i++)
			{
				array2[i] = array[num2 - i];
			}
			return array2;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002FCD File Offset: 0x000011CD
		internal static IEnumerable<string> SplitHeaderValue(this string value, params char[] separators)
		{
			int len = value.Length;
			StringBuilder buff = new StringBuilder(32);
			int end = len - 1;
			bool escaped = false;
			bool quoted = false;
			int num;
			for (int i = 0; i <= end; i = num + 1)
			{
				char c = value[i];
				buff.Append(c);
				bool flag = c == '"';
				if (flag)
				{
					bool flag2 = escaped;
					if (flag2)
					{
						escaped = false;
					}
					else
					{
						quoted = !quoted;
					}
				}
				else
				{
					bool flag3 = c == '\\';
					if (flag3)
					{
						bool flag4 = i == end;
						if (flag4)
						{
							break;
						}
						bool flag5 = value[i + 1] == '"';
						if (flag5)
						{
							escaped = true;
						}
					}
					else
					{
						bool flag6 = Array.IndexOf<char>(separators, c) > -1;
						if (flag6)
						{
							bool flag7 = quoted;
							if (!flag7)
							{
								buff.Length--;
								yield return buff.ToString();
								buff.Length = 0;
							}
						}
					}
				}
				num = i;
			}
			yield return buff.ToString();
			yield break;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002FE4 File Offset: 0x000011E4
		internal static byte[] ToByteArray(this Stream stream)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				stream.Position = 0L;
				stream.CopyTo(memoryStream, 1024);
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000303C File Offset: 0x0000123C
		internal static CompressionMethod ToCompressionMethod(this string value)
		{
			foreach (object obj in Enum.GetValues(typeof(CompressionMethod)))
			{
				CompressionMethod compressionMethod = (CompressionMethod)obj;
				bool flag = compressionMethod.ToExtensionString(new string[0]) == value;
				if (flag)
				{
					return compressionMethod;
				}
			}
			return CompressionMethod.None;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000030BC File Offset: 0x000012BC
		internal static string ToExtensionString(this CompressionMethod method, params string[] parameters)
		{
			bool flag = method == CompressionMethod.None;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				string text = string.Format("permessage-{0}", method.ToString().ToLower());
				result = ((parameters != null && parameters.Length != 0) ? string.Format("{0}; {1}", text, parameters.ToString("; ")) : text);
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000311C File Offset: 0x0000131C
		internal static IPAddress ToIPAddress(this string value)
		{
			bool flag = value == null || value.Length == 0;
			IPAddress result;
			if (flag)
			{
				result = null;
			}
			else
			{
				IPAddress ipaddress;
				bool flag2 = IPAddress.TryParse(value, out ipaddress);
				if (flag2)
				{
					result = ipaddress;
				}
				else
				{
					try
					{
						IPAddress[] hostAddresses = Dns.GetHostAddresses(value);
						result = hostAddresses[0];
					}
					catch
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000317C File Offset: 0x0000137C
		internal static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			return new List<TSource>(source);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003194 File Offset: 0x00001394
		internal static string ToString(this IPAddress address, bool bracketIPv6)
		{
			return (bracketIPv6 && address.AddressFamily == AddressFamily.InterNetworkV6) ? string.Format("[{0}]", address.ToString()) : address.ToString();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000031CC File Offset: 0x000013CC
		internal static ushort ToUInt16(this byte[] source, ByteOrder sourceOrder)
		{
			return BitConverter.ToUInt16(source.ToHostOrder(sourceOrder), 0);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000031EC File Offset: 0x000013EC
		internal static ulong ToUInt64(this byte[] source, ByteOrder sourceOrder)
		{
			return BitConverter.ToUInt64(source.ToHostOrder(sourceOrder), 0);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000320B File Offset: 0x0000140B
		internal static IEnumerable<string> Trim(this IEnumerable<string> source)
		{
			foreach (string elm in source)
			{
				yield return elm.Trim();
				elm = null;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000321C File Offset: 0x0000141C
		internal static string TrimSlashFromEnd(this string value)
		{
			string text = value.TrimEnd(new char[]
			{
				'/'
			});
			return (text.Length > 0) ? text : "/";
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003254 File Offset: 0x00001454
		internal static string TrimSlashOrBackslashFromEnd(this string value)
		{
			string text = value.TrimEnd(new char[]
			{
				'/',
				'\\'
			});
			return (text.Length > 0) ? text : value[0].ToString();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003298 File Offset: 0x00001498
		internal static bool TryCreateVersion(this string versionString, out Version result)
		{
			result = null;
			try
			{
				result = new Version(versionString);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000032D0 File Offset: 0x000014D0
		internal static bool TryCreateWebSocketUri(this string uriString, out Uri result, out string message)
		{
			result = null;
			message = null;
			Uri uri = uriString.ToUri();
			bool flag = uri == null;
			bool result2;
			if (flag)
			{
				message = "An invalid URI string.";
				result2 = false;
			}
			else
			{
				bool flag2 = !uri.IsAbsoluteUri;
				if (flag2)
				{
					message = "A relative URI.";
					result2 = false;
				}
				else
				{
					string scheme = uri.Scheme;
					bool flag3 = !(scheme == "ws") && !(scheme == "wss");
					if (flag3)
					{
						message = "The scheme part is not 'ws' or 'wss'.";
						result2 = false;
					}
					else
					{
						int port = uri.Port;
						bool flag4 = port == 0;
						if (flag4)
						{
							message = "The port part is zero.";
							result2 = false;
						}
						else
						{
							bool flag5 = uri.Fragment.Length > 0;
							if (flag5)
							{
								message = "It includes the fragment component.";
								result2 = false;
							}
							else
							{
								result = ((port != -1) ? uri : new Uri(string.Format("{0}://{1}:{2}{3}", new object[]
								{
									scheme,
									uri.Host,
									(scheme == "ws") ? 80 : 443,
									uri.PathAndQuery
								})));
								result2 = true;
							}
						}
					}
				}
			}
			return result2;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000033F8 File Offset: 0x000015F8
		internal static bool TryGetUTF8DecodedString(this byte[] bytes, out string s)
		{
			s = null;
			try
			{
				s = Encoding.UTF8.GetString(bytes);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003438 File Offset: 0x00001638
		internal static bool TryGetUTF8EncodedBytes(this string s, out byte[] bytes)
		{
			bytes = null;
			try
			{
				bytes = Encoding.UTF8.GetBytes(s);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003478 File Offset: 0x00001678
		internal static bool TryOpenRead(this FileInfo fileInfo, out FileStream fileStream)
		{
			fileStream = null;
			try
			{
				fileStream = fileInfo.OpenRead();
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000034B0 File Offset: 0x000016B0
		internal static string Unquote(this string value)
		{
			int num = value.IndexOf('"');
			bool flag = num == -1;
			string result;
			if (flag)
			{
				result = value;
			}
			else
			{
				int num2 = value.LastIndexOf('"');
				bool flag2 = num2 == num;
				if (flag2)
				{
					result = value;
				}
				else
				{
					int num3 = num2 - num - 1;
					result = ((num3 > 0) ? value.Substring(num + 1, num3).Replace("\\\"", "\"") : string.Empty);
				}
			}
			return result;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000351C File Offset: 0x0000171C
		internal static bool Upgrades(this NameValueCollection headers, string protocol)
		{
			StringComparison comparisonTypeForValue = StringComparison.OrdinalIgnoreCase;
			return headers.Contains("Upgrade", protocol, comparisonTypeForValue) && headers.Contains("Connection", "Upgrade", comparisonTypeForValue);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003554 File Offset: 0x00001754
		internal static string UrlDecode(this string value, Encoding encoding)
		{
			return HttpUtility.UrlDecode(value, encoding);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003570 File Offset: 0x00001770
		internal static string UrlEncode(this string value, Encoding encoding)
		{
			return HttpUtility.UrlEncode(value, encoding);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000358C File Offset: 0x0000178C
		internal static string UTF8Decode(this byte[] bytes)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(bytes);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000035C0 File Offset: 0x000017C0
		internal static byte[] UTF8Encode(this string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000035E0 File Offset: 0x000017E0
		internal static void WriteBytes(this Stream stream, byte[] bytes, int bufferLength)
		{
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				memoryStream.CopyTo(stream, bufferLength);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000361C File Offset: 0x0000181C
		internal static void WriteBytesAsync(this Stream stream, byte[] bytes, int bufferLength, Action completed, Action<Exception> error)
		{
			MemoryStream input = new MemoryStream(bytes);
			input.CopyToAsync(stream, bufferLength, delegate
			{
				bool flag = completed != null;
				if (flag)
				{
					completed();
				}
				input.Dispose();
			}, delegate(Exception ex)
			{
				input.Dispose();
				bool flag = error != null;
				if (flag)
				{
					error(ex);
				}
			});
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003674 File Offset: 0x00001874
		public static void Emit(this EventHandler eventHandler, object sender, EventArgs e)
		{
			bool flag = eventHandler != null;
			if (flag)
			{
				eventHandler(sender, e);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003694 File Offset: 0x00001894
		public static void Emit<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			bool flag = eventHandler != null;
			if (flag)
			{
				eventHandler(sender, e);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000036B4 File Offset: 0x000018B4
		public static string GetDescription(this WebSocketSharp.Net.HttpStatusCode code)
		{
			return ((int)code).GetStatusDescription();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000036CC File Offset: 0x000018CC
		public static string GetStatusDescription(this int code)
		{
			if (code <= 207)
			{
				switch (code)
				{
				case 100:
					return "Continue";
				case 101:
					return "Switching Protocols";
				case 102:
					return "Processing";
				default:
					switch (code)
					{
					case 200:
						return "OK";
					case 201:
						return "Created";
					case 202:
						return "Accepted";
					case 203:
						return "Non-Authoritative Information";
					case 204:
						return "No Content";
					case 205:
						return "Reset Content";
					case 206:
						return "Partial Content";
					case 207:
						return "Multi-Status";
					}
					break;
				}
			}
			else
			{
				switch (code)
				{
				case 300:
					return "Multiple Choices";
				case 301:
					return "Moved Permanently";
				case 302:
					return "Found";
				case 303:
					return "See Other";
				case 304:
					return "Not Modified";
				case 305:
					return "Use Proxy";
				case 306:
					break;
				case 307:
					return "Temporary Redirect";
				default:
					switch (code)
					{
					case 400:
						return "Bad Request";
					case 401:
						return "Unauthorized";
					case 402:
						return "Payment Required";
					case 403:
						return "Forbidden";
					case 404:
						return "Not Found";
					case 405:
						return "Method Not Allowed";
					case 406:
						return "Not Acceptable";
					case 407:
						return "Proxy Authentication Required";
					case 408:
						return "Request Timeout";
					case 409:
						return "Conflict";
					case 410:
						return "Gone";
					case 411:
						return "Length Required";
					case 412:
						return "Precondition Failed";
					case 413:
						return "Request Entity Too Large";
					case 414:
						return "Request-Uri Too Long";
					case 415:
						return "Unsupported Media Type";
					case 416:
						return "Requested Range Not Satisfiable";
					case 417:
						return "Expectation Failed";
					case 418:
					case 419:
					case 420:
					case 421:
						break;
					case 422:
						return "Unprocessable Entity";
					case 423:
						return "Locked";
					case 424:
						return "Failed Dependency";
					default:
						switch (code)
						{
						case 500:
							return "Internal Server Error";
						case 501:
							return "Not Implemented";
						case 502:
							return "Bad Gateway";
						case 503:
							return "Service Unavailable";
						case 504:
							return "Gateway Timeout";
						case 505:
							return "Http Version Not Supported";
						case 507:
							return "Insufficient Storage";
						}
						break;
					}
					break;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000039D4 File Offset: 0x00001BD4
		public static bool IsCloseStatusCode(this ushort value)
		{
			return value > 999 && value < 5000;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000039FC File Offset: 0x00001BFC
		public static bool IsEnclosedIn(this string value, char c)
		{
			return value != null && value.Length > 1 && value[0] == c && value[value.Length - 1] == c;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003A38 File Offset: 0x00001C38
		public static bool IsHostOrder(this ByteOrder order)
		{
			return BitConverter.IsLittleEndian == (order == ByteOrder.Little);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003A58 File Offset: 0x00001C58
		public static bool IsLocal(this IPAddress address)
		{
			bool flag = address == null;
			if (flag)
			{
				throw new ArgumentNullException("address");
			}
			bool flag2 = address.Equals(IPAddress.Any);
			bool result;
			if (flag2)
			{
				result = true;
			}
			else
			{
				bool flag3 = address.Equals(IPAddress.Loopback);
				if (flag3)
				{
					result = true;
				}
				else
				{
					bool ossupportsIPv = Socket.OSSupportsIPv6;
					if (ossupportsIPv)
					{
						bool flag4 = address.Equals(IPAddress.IPv6Any);
						if (flag4)
						{
							return true;
						}
						bool flag5 = address.Equals(IPAddress.IPv6Loopback);
						if (flag5)
						{
							return true;
						}
					}
					string hostName = Dns.GetHostName();
					IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
					foreach (IPAddress obj in hostAddresses)
					{
						bool flag6 = address.Equals(obj);
						if (flag6)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003B28 File Offset: 0x00001D28
		public static bool IsNullOrEmpty(this string value)
		{
			return value == null || value.Length == 0;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003B4C File Offset: 0x00001D4C
		public static bool IsPredefinedScheme(this string value)
		{
			bool flag = value == null || value.Length < 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				char c = value[0];
				bool flag2 = c == 'h';
				if (flag2)
				{
					result = (value == "http" || value == "https");
				}
				else
				{
					bool flag3 = c == 'w';
					if (flag3)
					{
						result = (value == "ws" || value == "wss");
					}
					else
					{
						bool flag4 = c == 'f';
						if (flag4)
						{
							result = (value == "file" || value == "ftp");
						}
						else
						{
							bool flag5 = c == 'g';
							if (flag5)
							{
								result = (value == "gopher");
							}
							else
							{
								bool flag6 = c == 'm';
								if (flag6)
								{
									result = (value == "mailto");
								}
								else
								{
									bool flag7 = c == 'n';
									if (flag7)
									{
										c = value[1];
										result = ((c == 'e') ? (value == "news" || value == "net.pipe" || value == "net.tcp") : (value == "nntp"));
									}
									else
									{
										result = false;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003C88 File Offset: 0x00001E88
		public static bool MaybeUri(this string value)
		{
			bool flag = value == null || value.Length == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int num = value.IndexOf(':');
				bool flag2 = num == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = num >= 10;
					if (flag3)
					{
						result = false;
					}
					else
					{
						string value2 = value.Substring(0, num);
						result = value2.IsPredefinedScheme();
					}
				}
			}
			return result;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003CEC File Offset: 0x00001EEC
		public static T[] SubArray<T>(this T[] array, int startIndex, int length)
		{
			int num;
			bool flag = array == null || (num = array.Length) == 0;
			T[] result;
			if (flag)
			{
				result = new T[0];
			}
			else
			{
				bool flag2 = startIndex < 0 || length <= 0 || startIndex + length > num;
				if (flag2)
				{
					result = new T[0];
				}
				else
				{
					bool flag3 = startIndex == 0 && length == num;
					if (flag3)
					{
						result = array;
					}
					else
					{
						T[] array2 = new T[length];
						Array.Copy(array, startIndex, array2, 0, length);
						result = array2;
					}
				}
			}
			return result;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003D60 File Offset: 0x00001F60
		public static T[] SubArray<T>(this T[] array, long startIndex, long length)
		{
			long num;
			bool flag = array == null || (num = (long)array.Length) == 0L;
			T[] result;
			if (flag)
			{
				result = new T[0];
			}
			else
			{
				bool flag2 = startIndex < 0L || length <= 0L || startIndex + length > num;
				if (flag2)
				{
					result = new T[0];
				}
				else
				{
					bool flag3 = startIndex == 0L && length == num;
					if (flag3)
					{
						result = array;
					}
					else
					{
						T[] array2 = new T[length];
						Array.Copy(array, startIndex, array2, 0L, length);
						result = array2;
					}
				}
			}
			return result;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003DDC File Offset: 0x00001FDC
		public static void Times(this int n, Action action)
		{
			bool flag = n > 0 && action != null;
			if (flag)
			{
				((ulong)((long)n)).times(action);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003E04 File Offset: 0x00002004
		public static void Times(this long n, Action action)
		{
			bool flag = n > 0L && action != null;
			if (flag)
			{
				((ulong)n).times(action);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003E2C File Offset: 0x0000202C
		public static void Times(this uint n, Action action)
		{
			bool flag = n > 0U && action != null;
			if (flag)
			{
				((ulong)n).times(action);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003E54 File Offset: 0x00002054
		public static void Times(this ulong n, Action action)
		{
			bool flag = n > 0UL && action != null;
			if (flag)
			{
				n.times(action);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003E7C File Offset: 0x0000207C
		public static void Times(this int n, Action<int> action)
		{
			bool flag = n > 0 && action != null;
			if (flag)
			{
				for (int i = 0; i < n; i++)
				{
					action(i);
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003EB4 File Offset: 0x000020B4
		public static void Times(this long n, Action<long> action)
		{
			bool flag = n > 0L && action != null;
			if (flag)
			{
				for (long num = 0L; num < n; num += 1L)
				{
					action(num);
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003EEC File Offset: 0x000020EC
		public static void Times(this uint n, Action<uint> action)
		{
			bool flag = n > 0U && action != null;
			if (flag)
			{
				for (uint num = 0U; num < n; num += 1U)
				{
					action(num);
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003F24 File Offset: 0x00002124
		public static void Times(this ulong n, Action<ulong> action)
		{
			bool flag = n > 0UL && action != null;
			if (flag)
			{
				for (ulong num = 0UL; num < n; num += 1UL)
				{
					action(num);
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003F5C File Offset: 0x0000215C
		public static T To<T>(this byte[] source, ByteOrder sourceOrder) where T : struct
		{
			bool flag = source == null;
			if (flag)
			{
				throw new ArgumentNullException("source");
			}
			bool flag2 = source.Length == 0;
			T result;
			if (flag2)
			{
				result = default(T);
			}
			else
			{
				Type typeFromHandle = typeof(T);
				byte[] value = source.ToHostOrder(sourceOrder);
				result = ((typeFromHandle == typeof(bool)) ? ((T)((object)BitConverter.ToBoolean(value, 0))) : ((typeFromHandle == typeof(char)) ? ((T)((object)BitConverter.ToChar(value, 0))) : ((typeFromHandle == typeof(double)) ? ((T)((object)BitConverter.ToDouble(value, 0))) : ((typeFromHandle == typeof(short)) ? ((T)((object)BitConverter.ToInt16(value, 0))) : ((typeFromHandle == typeof(int)) ? ((T)((object)BitConverter.ToInt32(value, 0))) : ((typeFromHandle == typeof(long)) ? ((T)((object)BitConverter.ToInt64(value, 0))) : ((typeFromHandle == typeof(float)) ? ((T)((object)BitConverter.ToSingle(value, 0))) : ((typeFromHandle == typeof(ushort)) ? ((T)((object)BitConverter.ToUInt16(value, 0))) : ((typeFromHandle == typeof(uint)) ? ((T)((object)BitConverter.ToUInt32(value, 0))) : ((typeFromHandle == typeof(ulong)) ? ((T)((object)BitConverter.ToUInt64(value, 0))) : default(T)))))))))));
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000410C File Offset: 0x0000230C
		public static byte[] ToByteArray<T>(this T value, ByteOrder order) where T : struct
		{
			Type typeFromHandle = typeof(T);
			byte[] array;
			if (typeFromHandle != typeof(bool))
			{
				if (typeFromHandle != typeof(byte))
				{
					array = ((typeFromHandle == typeof(char)) ? BitConverter.GetBytes((char)((object)value)) : ((typeFromHandle == typeof(double)) ? BitConverter.GetBytes((double)((object)value)) : ((typeFromHandle == typeof(short)) ? BitConverter.GetBytes((short)((object)value)) : ((typeFromHandle == typeof(int)) ? BitConverter.GetBytes((int)((object)value)) : ((typeFromHandle == typeof(long)) ? BitConverter.GetBytes((long)((object)value)) : ((typeFromHandle == typeof(float)) ? BitConverter.GetBytes((float)((object)value)) : ((typeFromHandle == typeof(ushort)) ? BitConverter.GetBytes((ushort)((object)value)) : ((typeFromHandle == typeof(uint)) ? BitConverter.GetBytes((uint)((object)value)) : ((typeFromHandle == typeof(ulong)) ? BitConverter.GetBytes((ulong)((object)value)) : WebSocket.EmptyBytes)))))))));
				}
				else
				{
					(array = new byte[1])[0] = (byte)((object)value);
				}
			}
			else
			{
				array = BitConverter.GetBytes((bool)((object)value));
			}
			byte[] array2 = array;
			bool flag = array2.Length > 1 && !order.IsHostOrder();
			if (flag)
			{
				Array.Reverse(array2);
			}
			return array2;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000042BC File Offset: 0x000024BC
		public static byte[] ToHostOrder(this byte[] source, ByteOrder sourceOrder)
		{
			bool flag = source == null;
			if (flag)
			{
				throw new ArgumentNullException("source");
			}
			bool flag2 = source.Length < 2;
			byte[] result;
			if (flag2)
			{
				result = source;
			}
			else
			{
				result = ((!sourceOrder.IsHostOrder()) ? source.Reverse<byte>() : source);
			}
			return result;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004300 File Offset: 0x00002500
		public static string ToString<T>(this T[] array, string separator)
		{
			bool flag = array == null;
			if (flag)
			{
				throw new ArgumentNullException("array");
			}
			int num = array.Length;
			bool flag2 = num == 0;
			string result;
			if (flag2)
			{
				result = string.Empty;
			}
			else
			{
				bool flag3 = separator == null;
				if (flag3)
				{
					separator = string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder(64);
				for (int i = 0; i < num - 1; i++)
				{
					stringBuilder.AppendFormat("{0}{1}", array[i], separator);
				}
				stringBuilder.Append(array[num - 1].ToString());
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000043AC File Offset: 0x000025AC
		public static Uri ToUri(this string value)
		{
			Uri result;
			Uri.TryCreate(value, value.MaybeUri() ? UriKind.Absolute : UriKind.Relative, out result);
			return result;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000043D4 File Offset: 0x000025D4
		public static void WriteContent(this WebSocketSharp.Net.HttpListenerResponse response, byte[] content)
		{
			bool flag = response == null;
			if (flag)
			{
				throw new ArgumentNullException("response");
			}
			bool flag2 = content == null;
			if (flag2)
			{
				throw new ArgumentNullException("content");
			}
			long num = (long)content.Length;
			bool flag3 = num == 0L;
			if (flag3)
			{
				response.Close();
			}
			else
			{
				response.ContentLength64 = num;
				Stream outputStream = response.OutputStream;
				bool flag4 = num <= 2147483647L;
				if (flag4)
				{
					outputStream.Write(content, 0, (int)num);
				}
				else
				{
					outputStream.WriteBytes(content, 1024);
				}
				outputStream.Close();
			}
		}

		// Token: 0x04000001 RID: 1
		private static readonly byte[] _last = new byte[1];

		// Token: 0x04000002 RID: 2
		private static readonly int _retry = 5;

		// Token: 0x04000003 RID: 3
		private const string _tspecials = "()<>@,;:\\\"/[]?={} \t";
	}
}
