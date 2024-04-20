using System;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x02000509 RID: 1289
	internal class XamlParser
	{
		// Token: 0x0600401D RID: 16413 RVA: 0x00212EDE File Offset: 0x00211EDE
		internal static void ThrowException(string id, int lineNumber, int linePosition)
		{
			XamlParser.ThrowExceptionWithLine(SR.Get(id), lineNumber, linePosition);
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00212EED File Offset: 0x00211EED
		internal static void ThrowException(string id, string value, int lineNumber, int linePosition)
		{
			XamlParser.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				value
			}), lineNumber, linePosition);
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x00212F06 File Offset: 0x00211F06
		internal static void ThrowException(string id, string value1, string value2, int lineNumber, int linePosition)
		{
			XamlParser.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				value1,
				value2
			}), lineNumber, linePosition);
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x00212F24 File Offset: 0x00211F24
		internal static void ThrowException(string id, string value1, string value2, string value3, int lineNumber, int linePosition)
		{
			XamlParser.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				value1,
				value2,
				value3
			}), lineNumber, linePosition);
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x00212F47 File Offset: 0x00211F47
		internal static void ThrowException(string id, string value1, string value2, string value3, string value4, int lineNumber, int linePosition)
		{
			XamlParser.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				value1,
				value2,
				value3,
				value4
			}), lineNumber, linePosition);
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x00212F70 File Offset: 0x00211F70
		private static void ThrowExceptionWithLine(string message, int lineNumber, int linePosition)
		{
			message += " ";
			message += SR.Get("ParserLineAndOffset", new object[]
			{
				lineNumber.ToString(CultureInfo.CurrentCulture),
				linePosition.ToString(CultureInfo.CurrentCulture)
			});
			throw new XamlParseException(message, lineNumber, linePosition);
		}
	}
}
