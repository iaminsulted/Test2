using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200062B RID: 1579
	internal class InputScopeAttribute : UnsafeNativeMethods.ITfInputScope
	{
		// Token: 0x06004E17 RID: 19991 RVA: 0x00243143 File Offset: 0x00242143
		internal InputScopeAttribute(InputScope inputscope)
		{
			this._inputScope = inputscope;
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x00243154 File Offset: 0x00242154
		public void GetInputScopes(out IntPtr ppinputscopes, out int count)
		{
			if (this._inputScope != null)
			{
				int num = 0;
				count = this._inputScope.Names.Count;
				try
				{
					ppinputscopes = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * count);
				}
				catch (OutOfMemoryException)
				{
					throw new COMException(SR.Get("InputScopeAttribute_E_OUTOFMEMORY"), -2147024882);
				}
				for (int i = 0; i < count; i++)
				{
					Marshal.WriteInt32(ppinputscopes, num, (int)((InputScopeName)this._inputScope.Names[i]).NameValue);
					num += Marshal.SizeOf(typeof(int));
				}
				return;
			}
			ppinputscopes = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
			Marshal.WriteInt32(ppinputscopes, 0);
			count = 1;
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x00243228 File Offset: 0x00242228
		public int GetPhrase(out IntPtr ppbstrPhrases, out int count)
		{
			count = ((this._inputScope == null) ? 0 : this._inputScope.PhraseList.Count);
			try
			{
				ppbstrPhrases = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)) * count);
			}
			catch (OutOfMemoryException)
			{
				throw new COMException(SR.Get("InputScopeAttribute_E_OUTOFMEMORY"), -2147024882);
			}
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IntPtr val;
				try
				{
					val = Marshal.StringToBSTR(((InputScopePhrase)this._inputScope.PhraseList[i]).Name);
				}
				catch (OutOfMemoryException)
				{
					num = 0;
					for (int j = 0; j < i; j++)
					{
						Marshal.FreeBSTR(Marshal.ReadIntPtr(ppbstrPhrases, num));
						num += Marshal.SizeOf(typeof(IntPtr));
					}
					throw new COMException(SR.Get("InputScopeAttribute_E_OUTOFMEMORY"), -2147024882);
				}
				Marshal.WriteIntPtr(ppbstrPhrases, num, val);
				num += Marshal.SizeOf(typeof(IntPtr));
			}
			if (count <= 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x00243340 File Offset: 0x00242340
		public int GetRegularExpression(out string desc)
		{
			desc = null;
			if (this._inputScope != null)
			{
				desc = this._inputScope.RegularExpression;
			}
			if (desc == null)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x00243361 File Offset: 0x00242361
		public int GetSRGC(out string desc)
		{
			desc = null;
			if (this._inputScope != null)
			{
				desc = this._inputScope.SrgsMarkup;
			}
			if (desc == null)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x00243382 File Offset: 0x00242382
		public int GetXML(out string desc)
		{
			desc = null;
			return 1;
		}

		// Token: 0x04002836 RID: 10294
		private InputScope _inputScope;
	}
}
