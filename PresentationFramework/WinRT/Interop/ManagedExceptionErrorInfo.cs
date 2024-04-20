using System;

namespace WinRT.Interop
{
	// Token: 0x020000BC RID: 188
	internal class ManagedExceptionErrorInfo : IErrorInfo, ISupportErrorInfo
	{
		// Token: 0x06000316 RID: 790 RVA: 0x000FD7A8 File Offset: 0x000FC7A8
		public ManagedExceptionErrorInfo(Exception ex)
		{
			this._exception = ex;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public bool InterfaceSupportsErrorInfo(Guid riid)
		{
			return true;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000FD7BC File Offset: 0x000FC7BC
		public Guid GetGuid()
		{
			return default(Guid);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000FD7D2 File Offset: 0x000FC7D2
		public string GetSource()
		{
			return this._exception.Source;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000FD7E0 File Offset: 0x000FC7E0
		public string GetDescription()
		{
			string text = this._exception.Message;
			if (string.IsNullOrEmpty(text))
			{
				text = this._exception.GetType().FullName;
			}
			return text;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000FD813 File Offset: 0x000FC813
		public string GetHelpFile()
		{
			return this._exception.HelpLink;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000FD820 File Offset: 0x000FC820
		public string GetHelpFileContent()
		{
			return string.Empty;
		}

		// Token: 0x040005EA RID: 1514
		private readonly Exception _exception;
	}
}
