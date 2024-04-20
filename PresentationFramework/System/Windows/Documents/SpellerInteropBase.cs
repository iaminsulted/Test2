using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace System.Windows.Documents
{
	// Token: 0x0200068A RID: 1674
	internal abstract class SpellerInteropBase : IDisposable
	{
		// Token: 0x06005306 RID: 21254
		public abstract void Dispose();

		// Token: 0x06005307 RID: 21255
		protected abstract void Dispose(bool disposing);

		// Token: 0x06005308 RID: 21256 RVA: 0x0025AF18 File Offset: 0x00259F18
		public static SpellerInteropBase CreateInstance()
		{
			SpellerInteropBase result = null;
			bool flag = false;
			try
			{
				result = new WinRTSpellerInterop();
				flag = true;
			}
			catch (PlatformNotSupportedException)
			{
				flag = false;
			}
			catch (NotSupportedException)
			{
				flag = true;
			}
			if (!flag)
			{
				try
				{
					result = new NLGSpellerInterop();
				}
				catch (Exception ex) when (ex is DllNotFoundException || ex is EntryPointNotFoundException)
				{
					return null;
				}
				return result;
			}
			return result;
		}

		// Token: 0x06005309 RID: 21257
		internal abstract void SetLocale(CultureInfo culture);

		// Token: 0x0600530A RID: 21258
		internal abstract int EnumTextSegments(char[] text, int count, SpellerInteropBase.EnumSentencesCallback sentenceCallback, SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data);

		// Token: 0x0600530B RID: 21259
		internal abstract void UnloadDictionary(object dictionary);

		// Token: 0x0600530C RID: 21260
		internal abstract object LoadDictionary(string lexiconFilePath);

		// Token: 0x0600530D RID: 21261
		internal abstract object LoadDictionary(Uri item, string trustedFolder);

		// Token: 0x0600530E RID: 21262
		internal abstract void ReleaseAllLexicons();

		// Token: 0x1700138D RID: 5005
		// (set) Token: 0x0600530F RID: 21263
		internal abstract SpellerInteropBase.SpellerMode Mode { set; }

		// Token: 0x1700138E RID: 5006
		// (set) Token: 0x06005310 RID: 21264
		internal abstract bool MultiWordMode { set; }

		// Token: 0x06005311 RID: 21265
		internal abstract void SetReformMode(CultureInfo culture, SpellingReform spellingReform);

		// Token: 0x06005312 RID: 21266
		internal abstract bool CanSpellCheck(CultureInfo culture);

		// Token: 0x02000B54 RID: 2900
		// (Invoke) Token: 0x06008DA3 RID: 36259
		internal delegate bool EnumSentencesCallback(SpellerInteropBase.ISpellerSentence sentence, object data);

		// Token: 0x02000B55 RID: 2901
		// (Invoke) Token: 0x06008DA7 RID: 36263
		internal delegate bool EnumTextSegmentsCallback(SpellerInteropBase.ISpellerSegment textSegment, object data);

		// Token: 0x02000B56 RID: 2902
		internal interface ITextRange
		{
			// Token: 0x17001EED RID: 7917
			// (get) Token: 0x06008DAA RID: 36266
			int Start { get; }

			// Token: 0x17001EEE RID: 7918
			// (get) Token: 0x06008DAB RID: 36267
			int Length { get; }
		}

		// Token: 0x02000B57 RID: 2903
		internal interface ISpellerSegment
		{
			// Token: 0x17001EEF RID: 7919
			// (get) Token: 0x06008DAC RID: 36268
			string SourceString { get; }

			// Token: 0x17001EF0 RID: 7920
			// (get) Token: 0x06008DAD RID: 36269
			IReadOnlyList<SpellerInteropBase.ISpellerSegment> SubSegments { get; }

			// Token: 0x17001EF1 RID: 7921
			// (get) Token: 0x06008DAE RID: 36270
			SpellerInteropBase.ITextRange TextRange { get; }

			// Token: 0x17001EF2 RID: 7922
			// (get) Token: 0x06008DAF RID: 36271
			string Text { get; }

			// Token: 0x17001EF3 RID: 7923
			// (get) Token: 0x06008DB0 RID: 36272
			IReadOnlyList<string> Suggestions { get; }

			// Token: 0x17001EF4 RID: 7924
			// (get) Token: 0x06008DB1 RID: 36273
			bool IsClean { get; }

			// Token: 0x06008DB2 RID: 36274
			void EnumSubSegments(SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data);
		}

		// Token: 0x02000B58 RID: 2904
		internal interface ISpellerSentence
		{
			// Token: 0x17001EF5 RID: 7925
			// (get) Token: 0x06008DB3 RID: 36275
			IReadOnlyList<SpellerInteropBase.ISpellerSegment> Segments { get; }

			// Token: 0x17001EF6 RID: 7926
			// (get) Token: 0x06008DB4 RID: 36276
			int EndOffset { get; }
		}

		// Token: 0x02000B59 RID: 2905
		[Flags]
		internal enum SpellerMode
		{
			// Token: 0x0400489C RID: 18588
			None = 0,
			// Token: 0x0400489D RID: 18589
			WordBreaking = 1,
			// Token: 0x0400489E RID: 18590
			SpellingErrors = 2,
			// Token: 0x0400489F RID: 18591
			Suggestions = 4,
			// Token: 0x040048A0 RID: 18592
			SpellingErrorsWithSuggestions = 6,
			// Token: 0x040048A1 RID: 18593
			All = 7
		}
	}
}
