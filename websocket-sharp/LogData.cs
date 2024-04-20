using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace WebSocketSharp
{
	// Token: 0x02000010 RID: 16
	public class LogData
	{
		// Token: 0x06000131 RID: 305 RVA: 0x0000947C File Offset: 0x0000767C
		internal LogData(LogLevel level, StackFrame caller, string message)
		{
			this._level = level;
			this._caller = caller;
			this._message = (message ?? string.Empty);
			this._date = DateTime.Now;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000094B0 File Offset: 0x000076B0
		public StackFrame Caller
		{
			get
			{
				return this._caller;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000094C8 File Offset: 0x000076C8
		public DateTime Date
		{
			get
			{
				return this._date;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000094E0 File Offset: 0x000076E0
		public LogLevel Level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000094F8 File Offset: 0x000076F8
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009510 File Offset: 0x00007710
		public override string ToString()
		{
			string text = string.Format("{0}|{1,-5}|", this._date, this._level);
			MethodBase method = this._caller.GetMethod();
			Type declaringType = method.DeclaringType;
			int fileLineNumber = this._caller.GetFileLineNumber();
			string arg = string.Format("{0}{1}.{2}:{3}|", new object[]
			{
				text,
				declaringType.Name,
				method.Name,
				fileLineNumber
			});
			string[] array = this._message.Replace("\r\n", "\n").TrimEnd(new char[]
			{
				'\n'
			}).Split(new char[]
			{
				'\n'
			});
			bool flag = array.Length <= 1;
			string result;
			if (flag)
			{
				result = string.Format("{0}{1}", arg, this._message);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(string.Format("{0}{1}\n", arg, array[0]), 64);
				string format = string.Format("{{0,{0}}}{{1}}\n", text.Length);
				for (int i = 1; i < array.Length; i++)
				{
					stringBuilder.AppendFormat(format, "", array[i]);
				}
				StringBuilder stringBuilder2 = stringBuilder;
				int length = stringBuilder2.Length;
				stringBuilder2.Length = length - 1;
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x04000070 RID: 112
		private StackFrame _caller;

		// Token: 0x04000071 RID: 113
		private DateTime _date;

		// Token: 0x04000072 RID: 114
		private LogLevel _level;

		// Token: 0x04000073 RID: 115
		private string _message;
	}
}
