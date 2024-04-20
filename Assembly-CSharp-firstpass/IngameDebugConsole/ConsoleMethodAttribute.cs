using System;

namespace IngameDebugConsole
{
	// Token: 0x020001E7 RID: 487
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class ConsoleMethodAttribute : Attribute
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0002C777 File Offset: 0x0002A977
		public string Command
		{
			get
			{
				return this.m_command;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0002C77F File Offset: 0x0002A97F
		public string Description
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0002C787 File Offset: 0x0002A987
		public ConsoleMethodAttribute(string command, string description)
		{
			this.m_command = command;
			this.m_description = description;
		}

		// Token: 0x04000AB3 RID: 2739
		private string m_command;

		// Token: 0x04000AB4 RID: 2740
		private string m_description;
	}
}
