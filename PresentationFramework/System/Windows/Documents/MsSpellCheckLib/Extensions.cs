using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006FD RID: 1789
	internal static class Extensions
	{
		// Token: 0x06005DC0 RID: 24000 RVA: 0x0028DA94 File Offset: 0x0028CA94
		internal static List<string> ToList(this RCW.IEnumString enumString, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			List<string> list = new List<string>();
			if (enumString == null)
			{
				throw new ArgumentNullException("enumString");
			}
			try
			{
				uint num = 0U;
				string empty = string.Empty;
				do
				{
					enumString.RemoteNext(1U, out empty, out num);
					if (num > 0U)
					{
						list.Add(empty);
					}
				}
				while (num > 0U);
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(enumString);
				}
			}
			return list;
		}

		// Token: 0x06005DC1 RID: 24001 RVA: 0x0028DB18 File Offset: 0x0028CB18
		internal static List<SpellChecker.SpellingError> ToList(this RCW.IEnumSpellingError spellingErrors, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			if (spellingErrors == null)
			{
				throw new ArgumentNullException("spellingErrors");
			}
			List<SpellChecker.SpellingError> list = new List<SpellChecker.SpellingError>();
			try
			{
				for (;;)
				{
					RCW.ISpellingError spellingError = spellingErrors.Next();
					if (spellingError == null)
					{
						break;
					}
					SpellChecker.SpellingError item = new SpellChecker.SpellingError(spellingError, spellChecker, text, shouldSuppressCOMExceptions, true);
					list.Add(item);
				}
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(spellingErrors);
				}
			}
			return list;
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x0028DB98 File Offset: 0x0028CB98
		internal static bool IsClean(this List<SpellChecker.SpellingError> errors)
		{
			if (errors == null)
			{
				throw new ArgumentNullException("errors");
			}
			bool result = true;
			using (List<SpellChecker.SpellingError>.Enumerator enumerator = errors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CorrectiveAction != SpellChecker.CorrectiveAction.None)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}
	}
}
