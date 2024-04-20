using System;

namespace System.Windows.Markup
{
	// Token: 0x02000511 RID: 1297
	internal static class XamlSerializerUtil
	{
		// Token: 0x0600407A RID: 16506 RVA: 0x002140FC File Offset: 0x002130FC
		internal static void ThrowIfNonWhiteSpaceInAddText(string s, object parent)
		{
			if (s != null)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (!char.IsWhiteSpace(s[i]))
					{
						throw new ArgumentException(SR.Get("NonWhiteSpaceInAddText", new object[]
						{
							s
						}));
					}
				}
			}
		}
	}
}
