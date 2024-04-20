using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebSocketSharp
{
	// Token: 0x02000014 RID: 20
	internal class WebSocketFrame : IEnumerable<byte>, IEnumerable
	{
		// Token: 0x0600014A RID: 330 RVA: 0x00009A4B File Offset: 0x00007C4B
		private WebSocketFrame()
		{
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00009A55 File Offset: 0x00007C55
		internal WebSocketFrame(Opcode opcode, PayloadData payloadData, bool mask) : this(Fin.Final, opcode, payloadData, false, mask)
		{
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009A64 File Offset: 0x00007C64
		internal WebSocketFrame(Fin fin, Opcode opcode, byte[] data, bool compressed, bool mask) : this(fin, opcode, new PayloadData(data), compressed, mask)
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009A7C File Offset: 0x00007C7C
		internal WebSocketFrame(Fin fin, Opcode opcode, PayloadData payloadData, bool compressed, bool mask)
		{
			this._fin = fin;
			this._rsv1 = ((opcode.IsData() && compressed) ? Rsv.On : Rsv.Off);
			this._rsv2 = Rsv.Off;
			this._rsv3 = Rsv.Off;
			this._opcode = opcode;
			ulong length = payloadData.Length;
			bool flag = length < 126UL;
			if (flag)
			{
				this._payloadLength = (byte)length;
				this._extPayloadLength = WebSocket.EmptyBytes;
			}
			else
			{
				bool flag2 = length < 65536UL;
				if (flag2)
				{
					this._payloadLength = 126;
					this._extPayloadLength = ((ushort)length).InternalToByteArray(ByteOrder.Big);
				}
				else
				{
					this._payloadLength = 127;
					this._extPayloadLength = length.InternalToByteArray(ByteOrder.Big);
				}
			}
			if (mask)
			{
				this._mask = Mask.On;
				this._maskingKey = WebSocketFrame.createMaskingKey();
				payloadData.Mask(this._maskingKey);
			}
			else
			{
				this._mask = Mask.Off;
				this._maskingKey = WebSocket.EmptyBytes;
			}
			this._payloadData = payloadData;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00009B6C File Offset: 0x00007D6C
		internal int ExtendedPayloadLengthCount
		{
			get
			{
				return (this._payloadLength < 126) ? 0 : ((this._payloadLength == 126) ? 2 : 8);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00009B9C File Offset: 0x00007D9C
		internal ulong FullPayloadLength
		{
			get
			{
				return (this._payloadLength < 126) ? ((ulong)this._payloadLength) : ((this._payloadLength == 126) ? ((ulong)this._extPayloadLength.ToUInt16(ByteOrder.Big)) : this._extPayloadLength.ToUInt64(ByteOrder.Big));
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00009BE8 File Offset: 0x00007DE8
		public byte[] ExtendedPayloadLength
		{
			get
			{
				return this._extPayloadLength;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00009C00 File Offset: 0x00007E00
		public Fin Fin
		{
			get
			{
				return this._fin;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00009C18 File Offset: 0x00007E18
		public bool IsBinary
		{
			get
			{
				return this._opcode == Opcode.Binary;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00009C34 File Offset: 0x00007E34
		public bool IsClose
		{
			get
			{
				return this._opcode == Opcode.Close;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00009C50 File Offset: 0x00007E50
		public bool IsCompressed
		{
			get
			{
				return this._rsv1 == Rsv.On;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00009C6C File Offset: 0x00007E6C
		public bool IsContinuation
		{
			get
			{
				return this._opcode == Opcode.Cont;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00009C88 File Offset: 0x00007E88
		public bool IsControl
		{
			get
			{
				return this._opcode >= Opcode.Close;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00009CA8 File Offset: 0x00007EA8
		public bool IsData
		{
			get
			{
				return this._opcode == Opcode.Text || this._opcode == Opcode.Binary;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00009CD0 File Offset: 0x00007ED0
		public bool IsFinal
		{
			get
			{
				return this._fin == Fin.Final;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00009CEC File Offset: 0x00007EEC
		public bool IsFragment
		{
			get
			{
				return this._fin == Fin.More || this._opcode == Opcode.Cont;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00009D14 File Offset: 0x00007F14
		public bool IsMasked
		{
			get
			{
				return this._mask == Mask.On;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00009D30 File Offset: 0x00007F30
		public bool IsPing
		{
			get
			{
				return this._opcode == Opcode.Ping;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00009D4C File Offset: 0x00007F4C
		public bool IsPong
		{
			get
			{
				return this._opcode == Opcode.Pong;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00009D68 File Offset: 0x00007F68
		public bool IsText
		{
			get
			{
				return this._opcode == Opcode.Text;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00009D84 File Offset: 0x00007F84
		public ulong Length
		{
			get
			{
				return (ulong)(2L + (long)(this._extPayloadLength.Length + this._maskingKey.Length) + (long)this._payloadData.Length);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00009DB8 File Offset: 0x00007FB8
		public Mask Mask
		{
			get
			{
				return this._mask;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00009DD0 File Offset: 0x00007FD0
		public byte[] MaskingKey
		{
			get
			{
				return this._maskingKey;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public Opcode Opcode
		{
			get
			{
				return this._opcode;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00009E00 File Offset: 0x00008000
		public PayloadData PayloadData
		{
			get
			{
				return this._payloadData;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00009E18 File Offset: 0x00008018
		public byte PayloadLength
		{
			get
			{
				return this._payloadLength;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00009E30 File Offset: 0x00008030
		public Rsv Rsv1
		{
			get
			{
				return this._rsv1;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00009E48 File Offset: 0x00008048
		public Rsv Rsv2
		{
			get
			{
				return this._rsv2;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00009E60 File Offset: 0x00008060
		public Rsv Rsv3
		{
			get
			{
				return this._rsv3;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009E78 File Offset: 0x00008078
		private static byte[] createMaskingKey()
		{
			byte[] array = new byte[4];
			WebSocket.RandomNumber.GetBytes(array);
			return array;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009EA0 File Offset: 0x000080A0
		private static string dump(WebSocketFrame frame)
		{
			ulong length = frame.Length;
			long num = (long)(length / 4UL);
			int num2 = (int)(length % 4UL);
			bool flag = num < 10000L;
			int num3;
			string arg;
			if (flag)
			{
				num3 = 4;
				arg = "{0,4}";
			}
			else
			{
				bool flag2 = num < 65536L;
				if (flag2)
				{
					num3 = 4;
					arg = "{0,4:X}";
				}
				else
				{
					bool flag3 = num < 4294967296L;
					if (flag3)
					{
						num3 = 8;
						arg = "{0,8:X}";
					}
					else
					{
						num3 = 16;
						arg = "{0,16:X}";
					}
				}
			}
			string arg2 = string.Format("{{0,{0}}}", num3);
			string format = string.Format("\n{0} 01234567 89ABCDEF 01234567 89ABCDEF\n{0}+--------+--------+--------+--------+\\n", arg2);
			string lineFmt = string.Format("{0}|{{1,8}} {{2,8}} {{3,8}} {{4,8}}|\n", arg);
			string format2 = string.Format("{0}+--------+--------+--------+--------+", arg2);
			StringBuilder output = new StringBuilder(64);
			Func<Action<string, string, string, string>> func = delegate()
			{
				long lineCnt = 0L;
				return delegate(string arg1, string arg2, string arg3, string arg4)
				{
					StringBuilder output = output;
					string lineFmt = lineFmt;
					object[] array2 = new object[5];
					int num6 = 0;
					long num7 = lineCnt + 1L;
					lineCnt = num7;
					array2[num6] = num7;
					array2[1] = arg1;
					array2[2] = arg2;
					array2[3] = arg3;
					array2[4] = arg4;
					output.AppendFormat(lineFmt, array2);
				};
			};
			Action<string, string, string, string> action = func();
			output.AppendFormat(format, string.Empty);
			byte[] array = frame.ToArray();
			for (long num4 = 0L; num4 <= num; num4 += 1L)
			{
				long num5 = num4 * 4L;
				bool flag4 = num4 < num;
				checked
				{
					if (flag4)
					{
						action(Convert.ToString(array[(int)((IntPtr)num5)], 2).PadLeft(8, '0'), Convert.ToString(array[(int)((IntPtr)(unchecked(num5 + 1L)))], 2).PadLeft(8, '0'), Convert.ToString(array[(int)((IntPtr)(unchecked(num5 + 2L)))], 2).PadLeft(8, '0'), Convert.ToString(array[(int)((IntPtr)(unchecked(num5 + 3L)))], 2).PadLeft(8, '0'));
					}
					else
					{
						bool flag5 = num2 > 0;
						if (flag5)
						{
							action(Convert.ToString(array[(int)((IntPtr)num5)], 2).PadLeft(8, '0'), (num2 >= 2) ? Convert.ToString(array[(int)((IntPtr)(unchecked(num5 + 1L)))], 2).PadLeft(8, '0') : string.Empty, (num2 == 3) ? Convert.ToString(array[(int)((IntPtr)(unchecked(num5 + 2L)))], 2).PadLeft(8, '0') : string.Empty, string.Empty);
						}
					}
				}
			}
			output.AppendFormat(format2, string.Empty);
			return output.ToString();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A0DC File Offset: 0x000082DC
		private static string print(WebSocketFrame frame)
		{
			byte payloadLength = frame._payloadLength;
			string text = (payloadLength > 125) ? frame.FullPayloadLength.ToString() : string.Empty;
			string text2 = BitConverter.ToString(frame._maskingKey);
			string text3 = (payloadLength == 0) ? string.Empty : ((payloadLength > 125) ? "---" : ((frame.IsText && !frame.IsFragment && !frame.IsMasked && !frame.IsCompressed) ? frame._payloadData.ApplicationData.UTF8Decode() : frame._payloadData.ToString()));
			string format = "\n                    FIN: {0}\n                   RSV1: {1}\n                   RSV2: {2}\n                   RSV3: {3}\n                 Opcode: {4}\n                   MASK: {5}\n         Payload Length: {6}\nExtended Payload Length: {7}\n            Masking Key: {8}\n           Payload Data: {9}";
			return string.Format(format, new object[]
			{
				frame._fin,
				frame._rsv1,
				frame._rsv2,
				frame._rsv3,
				frame._opcode,
				frame._mask,
				payloadLength,
				text,
				text2,
				text3
			});
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A1F4 File Offset: 0x000083F4
		private static WebSocketFrame processHeader(byte[] header)
		{
			bool flag = header.Length != 2;
			if (flag)
			{
				throw new WebSocketException("The header of a frame cannot be read from the stream.");
			}
			Fin fin = ((header[0] & 128) == 128) ? Fin.Final : Fin.More;
			Rsv rsv = ((header[0] & 64) == 64) ? Rsv.On : Rsv.Off;
			Rsv rsv2 = ((header[0] & 32) == 32) ? Rsv.On : Rsv.Off;
			Rsv rsv3 = ((header[0] & 16) == 16) ? Rsv.On : Rsv.Off;
			byte opcode = header[0] & 15;
			Mask mask = ((header[1] & 128) == 128) ? Mask.On : Mask.Off;
			byte b = header[1] & 127;
			string text = (!opcode.IsSupported()) ? "An unsupported opcode." : ((!opcode.IsData() && rsv == Rsv.On) ? "A non data frame is compressed." : ((opcode.IsControl() && fin == Fin.More) ? "A control frame is fragmented." : ((opcode.IsControl() && b > 125) ? "A control frame has a long payload length." : null)));
			bool flag2 = text != null;
			if (flag2)
			{
				throw new WebSocketException(CloseStatusCode.ProtocolError, text);
			}
			return new WebSocketFrame
			{
				_fin = fin,
				_rsv1 = rsv,
				_rsv2 = rsv2,
				_rsv3 = rsv3,
				_opcode = (Opcode)opcode,
				_mask = mask,
				_payloadLength = b
			};
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A338 File Offset: 0x00008538
		private static WebSocketFrame readExtendedPayloadLength(Stream stream, WebSocketFrame frame)
		{
			int extendedPayloadLengthCount = frame.ExtendedPayloadLengthCount;
			bool flag = extendedPayloadLengthCount == 0;
			WebSocketFrame result;
			if (flag)
			{
				frame._extPayloadLength = WebSocket.EmptyBytes;
				result = frame;
			}
			else
			{
				byte[] array = stream.ReadBytes(extendedPayloadLengthCount);
				bool flag2 = array.Length != extendedPayloadLengthCount;
				if (flag2)
				{
					throw new WebSocketException("The extended payload length of a frame cannot be read from the stream.");
				}
				frame._extPayloadLength = array;
				result = frame;
			}
			return result;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A394 File Offset: 0x00008594
		private static void readExtendedPayloadLengthAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error)
		{
			int len = frame.ExtendedPayloadLengthCount;
			bool flag = len == 0;
			if (flag)
			{
				frame._extPayloadLength = WebSocket.EmptyBytes;
				completed(frame);
			}
			else
			{
				stream.ReadBytesAsync(len, delegate(byte[] bytes)
				{
					bool flag2 = bytes.Length != len;
					if (flag2)
					{
						throw new WebSocketException("The extended payload length of a frame cannot be read from the stream.");
					}
					frame._extPayloadLength = bytes;
					completed(frame);
				}, error);
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A414 File Offset: 0x00008614
		private static WebSocketFrame readHeader(Stream stream)
		{
			return WebSocketFrame.processHeader(stream.ReadBytes(2));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A434 File Offset: 0x00008634
		private static void readHeaderAsync(Stream stream, Action<WebSocketFrame> completed, Action<Exception> error)
		{
			stream.ReadBytesAsync(2, delegate(byte[] bytes)
			{
				completed(WebSocketFrame.processHeader(bytes));
			}, error);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A464 File Offset: 0x00008664
		private static WebSocketFrame readMaskingKey(Stream stream, WebSocketFrame frame)
		{
			int num = frame.IsMasked ? 4 : 0;
			bool flag = num == 0;
			WebSocketFrame result;
			if (flag)
			{
				frame._maskingKey = WebSocket.EmptyBytes;
				result = frame;
			}
			else
			{
				byte[] array = stream.ReadBytes(num);
				bool flag2 = array.Length != num;
				if (flag2)
				{
					throw new WebSocketException("The masking key of a frame cannot be read from the stream.");
				}
				frame._maskingKey = array;
				result = frame;
			}
			return result;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A4C8 File Offset: 0x000086C8
		private static void readMaskingKeyAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error)
		{
			int len = frame.IsMasked ? 4 : 0;
			bool flag = len == 0;
			if (flag)
			{
				frame._maskingKey = WebSocket.EmptyBytes;
				completed(frame);
			}
			else
			{
				stream.ReadBytesAsync(len, delegate(byte[] bytes)
				{
					bool flag2 = bytes.Length != len;
					if (flag2)
					{
						throw new WebSocketException("The masking key of a frame cannot be read from the stream.");
					}
					frame._maskingKey = bytes;
					completed(frame);
				}, error);
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A550 File Offset: 0x00008750
		private static WebSocketFrame readPayloadData(Stream stream, WebSocketFrame frame)
		{
			ulong fullPayloadLength = frame.FullPayloadLength;
			bool flag = fullPayloadLength == 0UL;
			WebSocketFrame result;
			if (flag)
			{
				frame._payloadData = PayloadData.Empty;
				result = frame;
			}
			else
			{
				bool flag2 = fullPayloadLength > PayloadData.MaxLength;
				if (flag2)
				{
					throw new WebSocketException(CloseStatusCode.TooBig, "A frame has a long payload length.");
				}
				long num = (long)fullPayloadLength;
				byte[] array = (frame._payloadLength < 127) ? stream.ReadBytes((int)fullPayloadLength) : stream.ReadBytes(num, 1024);
				bool flag3 = (long)array.Length != num;
				if (flag3)
				{
					throw new WebSocketException("The payload data of a frame cannot be read from the stream.");
				}
				frame._payloadData = new PayloadData(array, num);
				result = frame;
			}
			return result;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A5F0 File Offset: 0x000087F0
		private static void readPayloadDataAsync(Stream stream, WebSocketFrame frame, Action<WebSocketFrame> completed, Action<Exception> error)
		{
			ulong fullPayloadLength = frame.FullPayloadLength;
			bool flag = fullPayloadLength == 0UL;
			if (flag)
			{
				frame._payloadData = PayloadData.Empty;
				completed(frame);
			}
			else
			{
				bool flag2 = fullPayloadLength > PayloadData.MaxLength;
				if (flag2)
				{
					throw new WebSocketException(CloseStatusCode.TooBig, "A frame has a long payload length.");
				}
				long llen = (long)fullPayloadLength;
				Action<byte[]> completed2 = delegate(byte[] bytes)
				{
					bool flag4 = (long)bytes.Length != llen;
					if (flag4)
					{
						throw new WebSocketException("The payload data of a frame cannot be read from the stream.");
					}
					frame._payloadData = new PayloadData(bytes, llen);
					completed(frame);
				};
				bool flag3 = frame._payloadLength < 127;
				if (flag3)
				{
					stream.ReadBytesAsync((int)fullPayloadLength, completed2, error);
				}
				else
				{
					stream.ReadBytesAsync(llen, 1024, completed2, error);
				}
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A6B8 File Offset: 0x000088B8
		internal static WebSocketFrame CreateCloseFrame(PayloadData payloadData, bool mask)
		{
			return new WebSocketFrame(Fin.Final, Opcode.Close, payloadData, false, mask);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A6D4 File Offset: 0x000088D4
		internal static WebSocketFrame CreatePingFrame(bool mask)
		{
			return new WebSocketFrame(Fin.Final, Opcode.Ping, PayloadData.Empty, false, mask);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A6F8 File Offset: 0x000088F8
		internal static WebSocketFrame CreatePingFrame(byte[] data, bool mask)
		{
			return new WebSocketFrame(Fin.Final, Opcode.Ping, new PayloadData(data), false, mask);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A71C File Offset: 0x0000891C
		internal static WebSocketFrame CreatePongFrame(PayloadData payloadData, bool mask)
		{
			return new WebSocketFrame(Fin.Final, Opcode.Pong, payloadData, false, mask);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A73C File Offset: 0x0000893C
		internal static WebSocketFrame ReadFrame(Stream stream, bool unmask)
		{
			WebSocketFrame webSocketFrame = WebSocketFrame.readHeader(stream);
			WebSocketFrame.readExtendedPayloadLength(stream, webSocketFrame);
			WebSocketFrame.readMaskingKey(stream, webSocketFrame);
			WebSocketFrame.readPayloadData(stream, webSocketFrame);
			if (unmask)
			{
				webSocketFrame.Unmask();
			}
			return webSocketFrame;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A77C File Offset: 0x0000897C
		internal static void ReadFrameAsync(Stream stream, bool unmask, Action<WebSocketFrame> completed, Action<Exception> error)
		{
			Action<WebSocketFrame> <>9__3;
			Action<WebSocketFrame> <>9__2;
			Action<WebSocketFrame> <>9__1;
			WebSocketFrame.readHeaderAsync(stream, delegate(WebSocketFrame frame)
			{
				Stream stream2 = stream;
				Action<WebSocketFrame> completed2;
				if ((completed2 = <>9__1) == null)
				{
					completed2 = (<>9__1 = delegate(WebSocketFrame frame1)
					{
						Stream stream3 = stream;
						Action<WebSocketFrame> completed3;
						if ((completed3 = <>9__2) == null)
						{
							completed3 = (<>9__2 = delegate(WebSocketFrame frame2)
							{
								Stream stream4 = stream;
								Action<WebSocketFrame> completed4;
								if ((completed4 = <>9__3) == null)
								{
									completed4 = (<>9__3 = delegate(WebSocketFrame frame3)
									{
										bool unmask2 = unmask;
										if (unmask2)
										{
											frame3.Unmask();
										}
										completed(frame3);
									});
								}
								WebSocketFrame.readPayloadDataAsync(stream4, frame2, completed4, error);
							});
						}
						WebSocketFrame.readMaskingKeyAsync(stream3, frame1, completed3, error);
					});
				}
				WebSocketFrame.readExtendedPayloadLengthAsync(stream2, frame, completed2, error);
			}, error);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A7CC File Offset: 0x000089CC
		internal void Unmask()
		{
			bool flag = this._mask == Mask.Off;
			if (!flag)
			{
				this._mask = Mask.Off;
				this._payloadData.Mask(this._maskingKey);
				this._maskingKey = WebSocket.EmptyBytes;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A80D File Offset: 0x00008A0D
		public IEnumerator<byte> GetEnumerator()
		{
			foreach (byte b in this.ToArray())
			{
				yield return b;
			}
			byte[] array = null;
			yield break;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A81C File Offset: 0x00008A1C
		public void Print(bool dumped)
		{
			Console.WriteLine(dumped ? WebSocketFrame.dump(this) : WebSocketFrame.print(this));
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A838 File Offset: 0x00008A38
		public string PrintToString(bool dumped)
		{
			return dumped ? WebSocketFrame.dump(this) : WebSocketFrame.print(this);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A85C File Offset: 0x00008A5C
		public byte[] ToArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = (int)this._fin;
				num = (int)((byte)(num << 1) + this._rsv1);
				num = (int)((byte)(num << 1) + this._rsv2);
				num = (int)((byte)(num << 1) + this._rsv3);
				num = (int)((byte)(num << 4) + this._opcode);
				num = (int)((byte)(num << 1) + this._mask);
				num = (num << 7) + (int)this._payloadLength;
				memoryStream.Write(((ushort)num).InternalToByteArray(ByteOrder.Big), 0, 2);
				bool flag = this._payloadLength > 125;
				if (flag)
				{
					memoryStream.Write(this._extPayloadLength, 0, (this._payloadLength == 126) ? 2 : 8);
				}
				bool flag2 = this._mask == Mask.On;
				if (flag2)
				{
					memoryStream.Write(this._maskingKey, 0, 4);
				}
				bool flag3 = this._payloadLength > 0;
				if (flag3)
				{
					byte[] array = this._payloadData.ToArray();
					bool flag4 = this._payloadLength < 127;
					if (flag4)
					{
						memoryStream.Write(array, 0, array.Length);
					}
					else
					{
						memoryStream.WriteBytes(array, 1024);
					}
				}
				memoryStream.Close();
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A98C File Offset: 0x00008B8C
		public override string ToString()
		{
			return BitConverter.ToString(this.ToArray());
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A9AC File Offset: 0x00008BAC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000084 RID: 132
		private byte[] _extPayloadLength;

		// Token: 0x04000085 RID: 133
		private Fin _fin;

		// Token: 0x04000086 RID: 134
		private Mask _mask;

		// Token: 0x04000087 RID: 135
		private byte[] _maskingKey;

		// Token: 0x04000088 RID: 136
		private Opcode _opcode;

		// Token: 0x04000089 RID: 137
		private PayloadData _payloadData;

		// Token: 0x0400008A RID: 138
		private byte _payloadLength;

		// Token: 0x0400008B RID: 139
		private Rsv _rsv1;

		// Token: 0x0400008C RID: 140
		private Rsv _rsv2;

		// Token: 0x0400008D RID: 141
		private Rsv _rsv3;

		// Token: 0x0400008E RID: 142
		internal static readonly byte[] EmptyPingBytes = WebSocketFrame.CreatePingFrame(false).ToArray();
	}
}
