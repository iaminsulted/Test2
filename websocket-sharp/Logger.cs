using System;
using System.Diagnostics;
using System.IO;

namespace WebSocketSharp
{
	// Token: 0x02000012 RID: 18
	public class Logger
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000966A File Offset: 0x0000786A
		public Logger() : this(LogLevel.Error, null, null)
		{
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009677 File Offset: 0x00007877
		public Logger(LogLevel level) : this(level, null, null)
		{
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009684 File Offset: 0x00007884
		public Logger(LogLevel level, string file, Action<LogData, string> output)
		{
			this._level = level;
			this._file = file;
			this._output = (output ?? new Action<LogData, string>(Logger.defaultOutput));
			this._sync = new object();
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000096C4 File Offset: 0x000078C4
		// (set) Token: 0x0600013B RID: 315 RVA: 0x000096E0 File Offset: 0x000078E0
		public string File
		{
			get
			{
				return this._file;
			}
			set
			{
				object sync = this._sync;
				lock (sync)
				{
					this._file = value;
					this.Warn(string.Format("The current path to the log file has been changed to {0}.", this._file));
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000973C File Offset: 0x0000793C
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00009758 File Offset: 0x00007958
		public LogLevel Level
		{
			get
			{
				return this._level;
			}
			set
			{
				object sync = this._sync;
				lock (sync)
				{
					this._level = value;
					this.Warn(string.Format("The current logging level has been changed to {0}.", this._level));
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000097B8 File Offset: 0x000079B8
		// (set) Token: 0x0600013F RID: 319 RVA: 0x000097D0 File Offset: 0x000079D0
		public Action<LogData, string> Output
		{
			get
			{
				return this._output;
			}
			set
			{
				object sync = this._sync;
				lock (sync)
				{
					this._output = (value ?? new Action<LogData, string>(Logger.defaultOutput));
					this.Warn("The current output action has been changed.");
				}
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000982C File Offset: 0x00007A2C
		private static void defaultOutput(LogData data, string path)
		{
			string value = data.ToString();
			Console.WriteLine(value);
			bool flag = path != null && path.Length > 0;
			if (flag)
			{
				Logger.writeToFile(value, path);
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00009864 File Offset: 0x00007A64
		private void output(string message, LogLevel level)
		{
			object sync = this._sync;
			lock (sync)
			{
				bool flag = this._level > level;
				if (!flag)
				{
					try
					{
						LogData logData = new LogData(level, new StackFrame(2, true), message);
						this._output(logData, this._file);
					}
					catch (Exception ex)
					{
						LogData logData = new LogData(LogLevel.Fatal, new StackFrame(0, true), ex.Message);
						Console.WriteLine(logData.ToString());
					}
				}
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009908 File Offset: 0x00007B08
		private static void writeToFile(string value, string path)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, true))
			{
				using (TextWriter textWriter = TextWriter.Synchronized(streamWriter))
				{
					textWriter.WriteLine(value);
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009964 File Offset: 0x00007B64
		public void Debug(string message)
		{
			bool flag = this._level > LogLevel.Debug;
			if (!flag)
			{
				this.output(message, LogLevel.Debug);
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000998C File Offset: 0x00007B8C
		public void Error(string message)
		{
			bool flag = this._level > LogLevel.Error;
			if (!flag)
			{
				this.output(message, LogLevel.Error);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000099B4 File Offset: 0x00007BB4
		public void Fatal(string message)
		{
			this.output(message, LogLevel.Fatal);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000099C0 File Offset: 0x00007BC0
		public void Info(string message)
		{
			bool flag = this._level > LogLevel.Info;
			if (!flag)
			{
				this.output(message, LogLevel.Info);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000099E8 File Offset: 0x00007BE8
		public void Trace(string message)
		{
			bool flag = this._level > LogLevel.Trace;
			if (!flag)
			{
				this.output(message, LogLevel.Trace);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009A10 File Offset: 0x00007C10
		public void Warn(string message)
		{
			bool flag = this._level > LogLevel.Warn;
			if (!flag)
			{
				this.output(message, LogLevel.Warn);
			}
		}

		// Token: 0x0400007B RID: 123
		private volatile string _file;

		// Token: 0x0400007C RID: 124
		private volatile LogLevel _level;

		// Token: 0x0400007D RID: 125
		private Action<LogData, string> _output;

		// Token: 0x0400007E RID: 126
		private object _sync;
	}
}
