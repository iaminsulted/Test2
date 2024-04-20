using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000646 RID: 1606
	internal class NLGSpellerInterop : SpellerInteropBase
	{
		// Token: 0x06004F55 RID: 20309 RVA: 0x00243E40 File Offset: 0x00242E40
		internal NLGSpellerInterop()
		{
			NLGSpellerInterop.UnsafeNlMethods.NlLoad();
			bool flag = true;
			try
			{
				this._textChunk = NLGSpellerInterop.CreateTextChunk();
				NLGSpellerInterop.ITextContext textContext = NLGSpellerInterop.CreateTextContext();
				try
				{
					this._textChunk.put_Context(textContext);
				}
				finally
				{
					Marshal.ReleaseComObject(textContext);
				}
				this._textChunk.put_ReuseObjects(true);
				this.Mode = SpellerInteropBase.SpellerMode.None;
				this.MultiWordMode = false;
				flag = false;
			}
			finally
			{
				if (flag)
				{
					if (this._textChunk != null)
					{
						Marshal.ReleaseComObject(this._textChunk);
						this._textChunk = null;
					}
					NLGSpellerInterop.UnsafeNlMethods.NlUnload();
				}
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x00243EE0 File Offset: 0x00242EE0
		~NLGSpellerInterop()
		{
			this.Dispose(false);
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x00243F10 File Offset: 0x00242F10
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x00243F20 File Offset: 0x00242F20
		protected override void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException(SR.Get("TextEditorSpellerInteropHasBeenDisposed"));
			}
			if (this._textChunk != null)
			{
				Marshal.ReleaseComObject(this._textChunk);
				this._textChunk = null;
			}
			NLGSpellerInterop.UnsafeNlMethods.NlUnload();
			this._isDisposed = true;
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x00243F6C File Offset: 0x00242F6C
		internal override void SetLocale(CultureInfo culture)
		{
			this._textChunk.put_Locale(culture.LCID);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x00243F80 File Offset: 0x00242F80
		private void SetContextOption(string option, object value)
		{
			NLGSpellerInterop.ITextContext textContext;
			this._textChunk.get_Context(out textContext);
			if (textContext != null)
			{
				try
				{
					NLGSpellerInterop.IProcessingOptions processingOptions;
					textContext.get_Options(out processingOptions);
					if (processingOptions != null)
					{
						try
						{
							processingOptions.put_Item(option, value);
						}
						finally
						{
							Marshal.ReleaseComObject(processingOptions);
						}
					}
				}
				finally
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00243FE0 File Offset: 0x00242FE0
		internal override int EnumTextSegments(char[] text, int count, SpellerInteropBase.EnumSentencesCallback sentenceCallback, SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
		{
			int num = 0;
			IntPtr intPtr = Marshal.AllocHGlobal(count * 2);
			try
			{
				Marshal.Copy(text, 0, intPtr, count);
				this._textChunk.SetInputArray(intPtr, count);
				UnsafeNativeMethods.IEnumVariant enumVariant;
				this._textChunk.GetEnumerator(out enumVariant);
				try
				{
					NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
					int[] array = new int[1];
					bool flag = true;
					enumVariant.Reset();
					do
					{
						variant.Clear();
						if (NLGSpellerInterop.EnumVariantNext(enumVariant, variant, array) != 0 || array[0] == 0)
						{
							break;
						}
						using (NLGSpellerInterop.SpellerSentence spellerSentence = new NLGSpellerInterop.SpellerSentence((NLGSpellerInterop.ISentence)variant.ToObject()))
						{
							num += spellerSentence.Segments.Count;
							if (segmentCallback != null)
							{
								int num2 = 0;
								while (flag && num2 < spellerSentence.Segments.Count)
								{
									flag = segmentCallback(spellerSentence.Segments[num2], data);
									num2++;
								}
							}
							if (sentenceCallback != null)
							{
								flag = sentenceCallback(spellerSentence, data);
							}
						}
					}
					while (flag);
					variant.Clear();
				}
				finally
				{
					Marshal.ReleaseComObject(enumVariant);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return num;
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x00244110 File Offset: 0x00243110
		internal override void UnloadDictionary(object dictionary)
		{
			NLGSpellerInterop.ILexicon lexicon = dictionary as NLGSpellerInterop.ILexicon;
			Invariant.Assert(lexicon != null);
			NLGSpellerInterop.ITextContext textContext = null;
			try
			{
				this._textChunk.get_Context(out textContext);
				textContext.RemoveLexicon(lexicon);
			}
			finally
			{
				Marshal.ReleaseComObject(lexicon);
				if (textContext != null)
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x00244168 File Offset: 0x00243168
		internal override object LoadDictionary(string lexiconFilePath)
		{
			return this.AddLexicon(lexiconFilePath);
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x00244171 File Offset: 0x00243171
		internal override object LoadDictionary(Uri item, string trustedFolder)
		{
			return this.LoadDictionary(item.LocalPath);
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00244180 File Offset: 0x00243180
		internal override void ReleaseAllLexicons()
		{
			NLGSpellerInterop.ITextContext textContext = null;
			try
			{
				this._textChunk.get_Context(out textContext);
				int i = 0;
				textContext.get_LexiconCount(out i);
				while (i > 0)
				{
					NLGSpellerInterop.ILexicon lexicon = null;
					textContext.get_Lexicon(0, out lexicon);
					textContext.RemoveLexicon(lexicon);
					Marshal.ReleaseComObject(lexicon);
					i--;
				}
			}
			finally
			{
				if (textContext != null)
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
		}

		// Token: 0x1700126A RID: 4714
		// (set) Token: 0x06004F60 RID: 20320 RVA: 0x002441E8 File Offset: 0x002431E8
		internal override SpellerInteropBase.SpellerMode Mode
		{
			set
			{
				this._mode = value;
				if (!this._mode.HasFlag(SpellerInteropBase.SpellerMode.SpellingErrors))
				{
					if (this._mode.HasFlag(SpellerInteropBase.SpellerMode.WordBreaking))
					{
						this.SetContextOption("IsSpellChecking", false);
					}
					return;
				}
				this.SetContextOption("IsSpellChecking", true);
				if (this._mode.HasFlag(SpellerInteropBase.SpellerMode.Suggestions))
				{
					this.SetContextOption("IsSpellVerifyOnly", false);
					return;
				}
				this.SetContextOption("IsSpellVerifyOnly", true);
			}
		}

		// Token: 0x1700126B RID: 4715
		// (set) Token: 0x06004F61 RID: 20321 RVA: 0x0024428A File Offset: 0x0024328A
		internal override bool MultiWordMode
		{
			set
			{
				this._multiWordMode = value;
				this.SetContextOption("IsSpellSuggestingMWEs", this._multiWordMode);
			}
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x002442AC File Offset: 0x002432AC
		internal override void SetReformMode(CultureInfo culture, SpellingReform spellingReform)
		{
			string twoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			string text;
			if (!(twoLetterISOLanguageName == "de"))
			{
				if (!(twoLetterISOLanguageName == "fr"))
				{
					text = null;
				}
				else
				{
					text = "FrenchReform";
				}
			}
			else
			{
				text = "GermanReform";
			}
			if (text != null)
			{
				switch (spellingReform)
				{
				case SpellingReform.PreAndPostreform:
					if (text == "GermanReform")
					{
						this.SetContextOption(text, 2);
						return;
					}
					this.SetContextOption(text, 0);
					break;
				case SpellingReform.Prereform:
					this.SetContextOption(text, 1);
					return;
				case SpellingReform.Postreform:
					this.SetContextOption(text, 2);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x00244348 File Offset: 0x00243348
		internal override bool CanSpellCheck(CultureInfo culture)
		{
			string twoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			return twoLetterISOLanguageName == "en" || twoLetterISOLanguageName == "de" || twoLetterISOLanguageName == "fr" || twoLetterISOLanguageName == "es";
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00244398 File Offset: 0x00243398
		private NLGSpellerInterop.ILexicon AddLexicon(string lexiconFilePath)
		{
			NLGSpellerInterop.ITextContext textContext = null;
			NLGSpellerInterop.ILexicon lexicon = null;
			bool flag = true;
			bool flag2 = false;
			try
			{
				flag2 = true;
				lexicon = NLGSpellerInterop.CreateLexicon();
				lexicon.ReadFrom(lexiconFilePath);
				this._textChunk.get_Context(out textContext);
				textContext.AddLexicon(lexicon);
				flag = false;
			}
			catch (Exception innerException)
			{
				if (flag2)
				{
					throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
					{
						lexiconFilePath
					}), innerException);
				}
				throw;
			}
			finally
			{
				if (flag && lexicon != null)
				{
					Marshal.ReleaseComObject(lexicon);
				}
				if (textContext != null)
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
			return lexicon;
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x0024442C File Offset: 0x0024342C
		private static object CreateInstance(Guid clsid, Guid iid)
		{
			object result;
			NLGSpellerInterop.UnsafeNlMethods.NlGetClassObject(ref clsid, ref iid, out result);
			return result;
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x00244445 File Offset: 0x00243445
		private static NLGSpellerInterop.ITextContext CreateTextContext()
		{
			return (NLGSpellerInterop.ITextContext)NLGSpellerInterop.CreateInstance(NLGSpellerInterop.CLSID_ITextContext, NLGSpellerInterop.IID_ITextContext);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x0024445B File Offset: 0x0024345B
		private static NLGSpellerInterop.ITextChunk CreateTextChunk()
		{
			return (NLGSpellerInterop.ITextChunk)NLGSpellerInterop.CreateInstance(NLGSpellerInterop.CLSID_ITextChunk, NLGSpellerInterop.IID_ITextChunk);
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x00244471 File Offset: 0x00243471
		private static NLGSpellerInterop.ILexicon CreateLexicon()
		{
			return (NLGSpellerInterop.ILexicon)NLGSpellerInterop.CreateInstance(NLGSpellerInterop.CLSID_Lexicon, NLGSpellerInterop.IID_ILexicon);
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00244488 File Offset: 0x00243488
		private unsafe static int EnumVariantNext(UnsafeNativeMethods.IEnumVariant variantEnumerator, NativeMethods.VARIANT variant, int[] fetched)
		{
			int result;
			fixed (short* ptr = &variant.vt)
			{
				void* value = (void*)ptr;
				result = variantEnumerator.Next(1, (IntPtr)value, fetched);
			}
			return result;
		}

		// Token: 0x04002853 RID: 10323
		private NLGSpellerInterop.ITextChunk _textChunk;

		// Token: 0x04002854 RID: 10324
		private bool _isDisposed;

		// Token: 0x04002855 RID: 10325
		private SpellerInteropBase.SpellerMode _mode;

		// Token: 0x04002856 RID: 10326
		private bool _multiWordMode;

		// Token: 0x04002857 RID: 10327
		private static readonly Guid CLSID_ITextContext = new Guid(859728164, 17235, 18740, 167, 190, 95, 181, 189, 221, 178, 214);

		// Token: 0x04002858 RID: 10328
		private static readonly Guid IID_ITextContext = new Guid(3061415104U, 4526, 16455, 164, 56, 38, 192, 201, 22, 235, 141);

		// Token: 0x04002859 RID: 10329
		private static readonly Guid CLSID_ITextChunk = new Guid(2313837402U, 53276, 17760, 168, 116, 159, 201, 42, 251, 14, 250);

		// Token: 0x0400285A RID: 10330
		private static readonly Guid IID_ITextChunk = new Guid(1419745662, 3779, 17364, 180, 67, 43, 248, 2, 16, 16, 207);

		// Token: 0x0400285B RID: 10331
		private static readonly Guid CLSID_Lexicon = new Guid("D385FDAD-D394-4812-9CEC-C6575C0B2B38");

		// Token: 0x0400285C RID: 10332
		private static readonly Guid IID_ILexicon = new Guid("004CD7E2-8B63-4ef9-8D46-080CDBBE47AF");

		// Token: 0x02000B3F RID: 2879
		private struct STextRange : SpellerInteropBase.ITextRange
		{
			// Token: 0x17001ED4 RID: 7892
			// (get) Token: 0x06008CC9 RID: 36041 RVA: 0x0033E21E File Offset: 0x0033D21E
			public int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001ED5 RID: 7893
			// (get) Token: 0x06008CCA RID: 36042 RVA: 0x0033E226 File Offset: 0x0033D226
			public int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x04004855 RID: 18517
			private readonly int _start;

			// Token: 0x04004856 RID: 18518
			private readonly int _length;
		}

		// Token: 0x02000B40 RID: 2880
		private enum RangeRole
		{
			// Token: 0x04004858 RID: 18520
			ecrrSimpleSegment,
			// Token: 0x04004859 RID: 18521
			ecrrAlternativeForm,
			// Token: 0x0400485A RID: 18522
			ecrrIncorrect,
			// Token: 0x0400485B RID: 18523
			ecrrAutoReplaceForm,
			// Token: 0x0400485C RID: 18524
			ecrrCorrectForm,
			// Token: 0x0400485D RID: 18525
			ecrrPreferredForm,
			// Token: 0x0400485E RID: 18526
			ecrrNormalizedForm,
			// Token: 0x0400485F RID: 18527
			ecrrCompoundSegment,
			// Token: 0x04004860 RID: 18528
			ecrrPhraseSegment,
			// Token: 0x04004861 RID: 18529
			ecrrNamedEntity,
			// Token: 0x04004862 RID: 18530
			ecrrCompoundWord,
			// Token: 0x04004863 RID: 18531
			ecrrPhrase,
			// Token: 0x04004864 RID: 18532
			ecrrUnknownWord,
			// Token: 0x04004865 RID: 18533
			ecrrContraction,
			// Token: 0x04004866 RID: 18534
			ecrrHyphenatedWord,
			// Token: 0x04004867 RID: 18535
			ecrrContractionSegment,
			// Token: 0x04004868 RID: 18536
			ecrrHyphenatedSegment,
			// Token: 0x04004869 RID: 18537
			ecrrCapitalization,
			// Token: 0x0400486A RID: 18538
			ecrrAccent,
			// Token: 0x0400486B RID: 18539
			ecrrRepeated,
			// Token: 0x0400486C RID: 18540
			ecrrDefinition,
			// Token: 0x0400486D RID: 18541
			ecrrOutOfContext
		}

		// Token: 0x02000B41 RID: 2881
		private class SpellerSegment : SpellerInteropBase.ISpellerSegment, IDisposable
		{
			// Token: 0x06008CCB RID: 36043 RVA: 0x0033E22E File Offset: 0x0033D22E
			public SpellerSegment(NLGSpellerInterop.ITextSegment textSegment)
			{
				this._textSegment = textSegment;
			}

			// Token: 0x06008CCC RID: 36044 RVA: 0x0033E240 File Offset: 0x0033D240
			private void EnumerateSuggestions()
			{
				List<string> list = new List<string>();
				UnsafeNativeMethods.IEnumVariant enumVariant;
				this._textSegment.get_Suggestions(out enumVariant);
				if (enumVariant == null)
				{
					this._suggestions = list.AsReadOnly();
					return;
				}
				try
				{
					NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
					int[] array = new int[1];
					for (;;)
					{
						variant.Clear();
						if (NLGSpellerInterop.EnumVariantNext(enumVariant, variant, array) != 0)
						{
							break;
						}
						if (array[0] == 0)
						{
							break;
						}
						list.Add(Marshal.PtrToStringUni(variant.data1.Value));
					}
				}
				finally
				{
					Marshal.ReleaseComObject(enumVariant);
				}
				this._suggestions = list.AsReadOnly();
			}

			// Token: 0x06008CCD RID: 36045 RVA: 0x0033E2D4 File Offset: 0x0033D2D4
			private void EnumerateSubSegments()
			{
				this._textSegment.get_Count(out this._subSegmentCount);
				List<SpellerInteropBase.ISpellerSegment> list = new List<SpellerInteropBase.ISpellerSegment>();
				for (int i = 0; i < this._subSegmentCount; i++)
				{
					NLGSpellerInterop.ITextSegment textSegment;
					this._textSegment.get_Item(i, out textSegment);
					list.Add(new NLGSpellerInterop.SpellerSegment(textSegment));
				}
				this._subSegments = list.AsReadOnly();
			}

			// Token: 0x17001ED6 RID: 7894
			// (get) Token: 0x06008CCE RID: 36046 RVA: 0x0033E32F File Offset: 0x0033D32F
			public string SourceString { get; }

			// Token: 0x17001ED7 RID: 7895
			// (get) Token: 0x06008CCF RID: 36047 RVA: 0x0033E337 File Offset: 0x0033D337
			public string Text
			{
				get
				{
					string sourceString = this.SourceString;
					if (sourceString == null)
					{
						return null;
					}
					return sourceString.Substring(this.TextRange.Start, this.TextRange.Length);
				}
			}

			// Token: 0x17001ED8 RID: 7896
			// (get) Token: 0x06008CD0 RID: 36048 RVA: 0x0033E360 File Offset: 0x0033D360
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> SubSegments
			{
				get
				{
					if (this._subSegments == null)
					{
						this.EnumerateSubSegments();
					}
					return this._subSegments;
				}
			}

			// Token: 0x17001ED9 RID: 7897
			// (get) Token: 0x06008CD1 RID: 36049 RVA: 0x0033E378 File Offset: 0x0033D378
			public SpellerInteropBase.ITextRange TextRange
			{
				get
				{
					if (this._sTextRange == null)
					{
						NLGSpellerInterop.STextRange value;
						this._textSegment.get_Range(out value);
						this._sTextRange = new NLGSpellerInterop.STextRange?(value);
					}
					return this._sTextRange.Value;
				}
			}

			// Token: 0x17001EDA RID: 7898
			// (get) Token: 0x06008CD2 RID: 36050 RVA: 0x0033E3BB File Offset: 0x0033D3BB
			public IReadOnlyList<string> Suggestions
			{
				get
				{
					if (this._suggestions == null)
					{
						this.EnumerateSuggestions();
					}
					return this._suggestions;
				}
			}

			// Token: 0x17001EDB RID: 7899
			// (get) Token: 0x06008CD3 RID: 36051 RVA: 0x0033E3D1 File Offset: 0x0033D3D1
			public bool IsClean
			{
				get
				{
					return this.RangeRole != NLGSpellerInterop.RangeRole.ecrrIncorrect;
				}
			}

			// Token: 0x06008CD4 RID: 36052 RVA: 0x0033E3E0 File Offset: 0x0033D3E0
			public void EnumSubSegments(SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
			{
				bool flag = true;
				int num = 0;
				while (flag && num < this.SubSegments.Count)
				{
					flag = segmentCallback(this.SubSegments[num], data);
					num++;
				}
			}

			// Token: 0x06008CD5 RID: 36053 RVA: 0x0033E41C File Offset: 0x0033D41C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008CD6 RID: 36054 RVA: 0x0033E42C File Offset: 0x0033D42C
			protected virtual void Dispose(bool disposing)
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("NLGSpellerInterop.SpellerSegment");
				}
				if (this._subSegments != null)
				{
					foreach (SpellerInteropBase.ISpellerSegment spellerSegment in this._subSegments)
					{
						((NLGSpellerInterop.SpellerSegment)spellerSegment).Dispose();
					}
					this._subSegments = null;
				}
				if (this._textSegment != null)
				{
					Marshal.ReleaseComObject(this._textSegment);
					this._textSegment = null;
				}
				this._disposed = true;
			}

			// Token: 0x06008CD7 RID: 36055 RVA: 0x0033E4C0 File Offset: 0x0033D4C0
			~SpellerSegment()
			{
				this.Dispose(false);
			}

			// Token: 0x17001EDC RID: 7900
			// (get) Token: 0x06008CD8 RID: 36056 RVA: 0x0033E4F0 File Offset: 0x0033D4F0
			public NLGSpellerInterop.RangeRole RangeRole
			{
				get
				{
					if (this._rangeRole == null)
					{
						NLGSpellerInterop.RangeRole value;
						this._textSegment.get_Role(out value);
						this._rangeRole = new NLGSpellerInterop.RangeRole?(value);
					}
					return this._rangeRole.Value;
				}
			}

			// Token: 0x0400486F RID: 18543
			private NLGSpellerInterop.STextRange? _sTextRange;

			// Token: 0x04004870 RID: 18544
			private int _subSegmentCount;

			// Token: 0x04004871 RID: 18545
			private IReadOnlyList<SpellerInteropBase.ISpellerSegment> _subSegments;

			// Token: 0x04004872 RID: 18546
			private IReadOnlyList<string> _suggestions;

			// Token: 0x04004873 RID: 18547
			private NLGSpellerInterop.RangeRole? _rangeRole;

			// Token: 0x04004874 RID: 18548
			private NLGSpellerInterop.ITextSegment _textSegment;

			// Token: 0x04004875 RID: 18549
			private bool _disposed;
		}

		// Token: 0x02000B42 RID: 2882
		private class SpellerSentence : SpellerInteropBase.ISpellerSentence, IDisposable
		{
			// Token: 0x06008CD9 RID: 36057 RVA: 0x0033E530 File Offset: 0x0033D530
			public SpellerSentence(NLGSpellerInterop.ISentence sentence)
			{
				this._disposed = false;
				try
				{
					int num;
					sentence.get_Count(out num);
					List<SpellerInteropBase.ISpellerSegment> list = new List<SpellerInteropBase.ISpellerSegment>();
					for (int i = 0; i < num; i++)
					{
						NLGSpellerInterop.ITextSegment textSegment;
						sentence.get_Item(i, out textSegment);
						list.Add(new NLGSpellerInterop.SpellerSegment(textSegment));
					}
					this._segments = list.AsReadOnly();
					Invariant.Assert(this._segments.Count == num);
				}
				finally
				{
					Marshal.ReleaseComObject(sentence);
				}
			}

			// Token: 0x17001EDD RID: 7901
			// (get) Token: 0x06008CDA RID: 36058 RVA: 0x0033E5B4 File Offset: 0x0033D5B4
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> Segments
			{
				get
				{
					return this._segments;
				}
			}

			// Token: 0x17001EDE RID: 7902
			// (get) Token: 0x06008CDB RID: 36059 RVA: 0x0033E5BC File Offset: 0x0033D5BC
			public int EndOffset
			{
				get
				{
					int result = -1;
					if (this.Segments.Count > 0)
					{
						SpellerInteropBase.ITextRange textRange = this.Segments[this.Segments.Count - 1].TextRange;
						result = textRange.Start + textRange.Length;
					}
					return result;
				}
			}

			// Token: 0x06008CDC RID: 36060 RVA: 0x0033E606 File Offset: 0x0033D606
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008CDD RID: 36061 RVA: 0x0033E618 File Offset: 0x0033D618
			protected virtual void Dispose(bool disposing)
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("NLGSpellerInterop.SpellerSentence");
				}
				if (this._segments != null)
				{
					foreach (SpellerInteropBase.ISpellerSegment spellerSegment in this._segments)
					{
						((NLGSpellerInterop.SpellerSegment)spellerSegment).Dispose();
					}
					this._segments = null;
				}
				this._disposed = true;
			}

			// Token: 0x06008CDE RID: 36062 RVA: 0x0033E690 File Offset: 0x0033D690
			~SpellerSentence()
			{
				this.Dispose(false);
			}

			// Token: 0x04004876 RID: 18550
			private IReadOnlyList<SpellerInteropBase.ISpellerSegment> _segments;

			// Token: 0x04004877 RID: 18551
			private bool _disposed;
		}

		// Token: 0x02000B43 RID: 2883
		private static class UnsafeNlMethods
		{
			// Token: 0x06008CDF RID: 36063
			[DllImport("PresentationNative_cor3.dll", PreserveSig = false)]
			internal static extern void NlLoad();

			// Token: 0x06008CE0 RID: 36064
			[DllImport("PresentationNative_cor3.dll")]
			internal static extern void NlUnload();

			// Token: 0x06008CE1 RID: 36065
			[DllImport("PresentationNative_cor3.dll", PreserveSig = false)]
			internal static extern void NlGetClassObject(ref Guid clsid, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object classObject);
		}

		// Token: 0x02000B44 RID: 2884
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("004CD7E2-8B63-4ef9-8D46-080CDBBE47AF")]
		[ComImport]
		internal interface ILexicon
		{
			// Token: 0x06008CE2 RID: 36066
			void ReadFrom([MarshalAs(UnmanagedType.BStr)] string fileName);

			// Token: 0x06008CE3 RID: 36067
			void stub_WriteTo();

			// Token: 0x06008CE4 RID: 36068
			void stub_GetEnumerator();

			// Token: 0x06008CE5 RID: 36069
			void stub_IndexOf();

			// Token: 0x06008CE6 RID: 36070
			void stub_TagFor();

			// Token: 0x06008CE7 RID: 36071
			void stub_ContainsPrefix();

			// Token: 0x06008CE8 RID: 36072
			void stub_Add();

			// Token: 0x06008CE9 RID: 36073
			void stub_Remove();

			// Token: 0x06008CEA RID: 36074
			void stub_Version();

			// Token: 0x06008CEB RID: 36075
			void stub_Count();

			// Token: 0x06008CEC RID: 36076
			void stub__NewEnum();

			// Token: 0x06008CED RID: 36077
			void stub_get_Item();

			// Token: 0x06008CEE RID: 36078
			void stub_set_Item();

			// Token: 0x06008CEF RID: 36079
			void stub_get_ItemByName();

			// Token: 0x06008CF0 RID: 36080
			void stub_set_ItemByName();

			// Token: 0x06008CF1 RID: 36081
			void stub_get0_PropertyCount();

			// Token: 0x06008CF2 RID: 36082
			void stub_get1_Property();

			// Token: 0x06008CF3 RID: 36083
			void stub_set_Property();

			// Token: 0x06008CF4 RID: 36084
			void stub_get_IsSealed();

			// Token: 0x06008CF5 RID: 36085
			void stub_get_IsReadOnly();
		}

		// Token: 0x02000B45 RID: 2885
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B6797CC0-11AE-4047-A438-26C0C916EB8D")]
		[ComImport]
		private interface ITextContext
		{
			// Token: 0x06008CF6 RID: 36086
			void stub_get_PropertyCount();

			// Token: 0x06008CF7 RID: 36087
			void stub_get_Property();

			// Token: 0x06008CF8 RID: 36088
			void stub_put_Property();

			// Token: 0x06008CF9 RID: 36089
			void stub_get_DefaultDialectCount();

			// Token: 0x06008CFA RID: 36090
			void stub_get_DefaultDialect();

			// Token: 0x06008CFB RID: 36091
			void stub_AddDefaultDialect();

			// Token: 0x06008CFC RID: 36092
			void stub_RemoveDefaultDialect();

			// Token: 0x06008CFD RID: 36093
			void get_LexiconCount([MarshalAs(UnmanagedType.I4)] out int lexiconCount);

			// Token: 0x06008CFE RID: 36094
			void get_Lexicon(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ILexicon lexicon);

			// Token: 0x06008CFF RID: 36095
			void AddLexicon([MarshalAs(UnmanagedType.Interface)] [In] NLGSpellerInterop.ILexicon lexicon);

			// Token: 0x06008D00 RID: 36096
			void RemoveLexicon([MarshalAs(UnmanagedType.Interface)] [In] NLGSpellerInterop.ILexicon lexicon);

			// Token: 0x06008D01 RID: 36097
			void stub_get_Version();

			// Token: 0x06008D02 RID: 36098
			void stub_get_ResourceLoader();

			// Token: 0x06008D03 RID: 36099
			void stub_put_ResourceLoader();

			// Token: 0x06008D04 RID: 36100
			void get_Options([MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.IProcessingOptions val);

			// Token: 0x06008D05 RID: 36101
			void get_Capabilities(int locale, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.IProcessingOptions val);

			// Token: 0x06008D06 RID: 36102
			void stub_get_Lexicons();

			// Token: 0x06008D07 RID: 36103
			void stub_put_Lexicons();

			// Token: 0x06008D08 RID: 36104
			void stub_get_MaxSentences();

			// Token: 0x06008D09 RID: 36105
			void stub_put_MaxSentences();

			// Token: 0x06008D0A RID: 36106
			void stub_get_IsSingleLanguage();

			// Token: 0x06008D0B RID: 36107
			void stub_put_IsSingleLanguage();

			// Token: 0x06008D0C RID: 36108
			void stub_get_IsSimpleWordBreaking();

			// Token: 0x06008D0D RID: 36109
			void stub_put_IsSimpleWordBreaking();

			// Token: 0x06008D0E RID: 36110
			void stub_get_UseRelativeTimes();

			// Token: 0x06008D0F RID: 36111
			void stub_put_UseRelativeTimes();

			// Token: 0x06008D10 RID: 36112
			void stub_get_IgnorePunctuation();

			// Token: 0x06008D11 RID: 36113
			void stub_put_IgnorePunctuation();

			// Token: 0x06008D12 RID: 36114
			void stub_get_IsCaching();

			// Token: 0x06008D13 RID: 36115
			void stub_put_IsCaching();

			// Token: 0x06008D14 RID: 36116
			void stub_get_IsShowingGaps();

			// Token: 0x06008D15 RID: 36117
			void stub_put_IsShowingGaps();

			// Token: 0x06008D16 RID: 36118
			void stub_get_IsShowingCharacterNormalizations();

			// Token: 0x06008D17 RID: 36119
			void stub_put_IsShowingCharacterNormalizations();

			// Token: 0x06008D18 RID: 36120
			void stub_get_IsShowingWordNormalizations();

			// Token: 0x06008D19 RID: 36121
			void stub_put_IsShowingWordNormalizations();

			// Token: 0x06008D1A RID: 36122
			void stub_get_IsComputingCompounds();

			// Token: 0x06008D1B RID: 36123
			void stub_put_IsComputingCompounds();

			// Token: 0x06008D1C RID: 36124
			void stub_get_IsComputingInflections();

			// Token: 0x06008D1D RID: 36125
			void stub_put_IsComputingInflections();

			// Token: 0x06008D1E RID: 36126
			void stub_get_IsComputingLemmas();

			// Token: 0x06008D1F RID: 36127
			void stub_put_IsComputingLemmas();

			// Token: 0x06008D20 RID: 36128
			void stub_get_IsComputingExpansions();

			// Token: 0x06008D21 RID: 36129
			void stub_put_IsComputingExpansions();

			// Token: 0x06008D22 RID: 36130
			void stub_get_IsComputingBases();

			// Token: 0x06008D23 RID: 36131
			void stub_put_IsComputingBases();

			// Token: 0x06008D24 RID: 36132
			void stub_get_IsComputingPartOfSpeechTags();

			// Token: 0x06008D25 RID: 36133
			void stub_put_IsComputingPartOfSpeechTags();

			// Token: 0x06008D26 RID: 36134
			void stub_get_IsFindingDefinitions();

			// Token: 0x06008D27 RID: 36135
			void stub_put_IsFindingDefinitions();

			// Token: 0x06008D28 RID: 36136
			void stub_get_IsFindingDateTimeMeasures();

			// Token: 0x06008D29 RID: 36137
			void stub_put_IsFindingDateTimeMeasures();

			// Token: 0x06008D2A RID: 36138
			void stub_get_IsFindingPersons();

			// Token: 0x06008D2B RID: 36139
			void stub_put_IsFindingPersons();

			// Token: 0x06008D2C RID: 36140
			void stub_get_IsFindingLocations();

			// Token: 0x06008D2D RID: 36141
			void stub_put_IsFindingLocations();

			// Token: 0x06008D2E RID: 36142
			void stub_get_IsFindingOrganizations();

			// Token: 0x06008D2F RID: 36143
			void stub_put_IsFindingOrganizations();

			// Token: 0x06008D30 RID: 36144
			void stub_get_IsFindingPhrases();

			// Token: 0x06008D31 RID: 36145
			void stub_put_IsFindingPhrases();
		}

		// Token: 0x02000B46 RID: 2886
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("549F997E-0EC3-43d4-B443-2BF8021010CF")]
		[ComImport]
		private interface ITextChunk
		{
			// Token: 0x06008D32 RID: 36146
			void stub_get_InputText();

			// Token: 0x06008D33 RID: 36147
			void stub_put_InputText();

			// Token: 0x06008D34 RID: 36148
			void SetInputArray([In] IntPtr inputArray, int size);

			// Token: 0x06008D35 RID: 36149
			void stub_RegisterEngine();

			// Token: 0x06008D36 RID: 36150
			void stub_UnregisterEngine();

			// Token: 0x06008D37 RID: 36151
			void stub_get_InputArray();

			// Token: 0x06008D38 RID: 36152
			void stub_get_InputArrayRange();

			// Token: 0x06008D39 RID: 36153
			void stub_put_InputArrayRange();

			// Token: 0x06008D3A RID: 36154
			void get_Count(out int val);

			// Token: 0x06008D3B RID: 36155
			void get_Item(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ISentence val);

			// Token: 0x06008D3C RID: 36156
			void stub_get__NewEnum();

			// Token: 0x06008D3D RID: 36157
			void get_Sentences([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IEnumVariant val);

			// Token: 0x06008D3E RID: 36158
			void stub_get_PropertyCount();

			// Token: 0x06008D3F RID: 36159
			void stub_get_Property();

			// Token: 0x06008D40 RID: 36160
			void stub_put_Property();

			// Token: 0x06008D41 RID: 36161
			void get_Context([MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ITextContext val);

			// Token: 0x06008D42 RID: 36162
			void put_Context([MarshalAs(UnmanagedType.Interface)] NLGSpellerInterop.ITextContext val);

			// Token: 0x06008D43 RID: 36163
			void stub_get_Locale();

			// Token: 0x06008D44 RID: 36164
			void put_Locale(int val);

			// Token: 0x06008D45 RID: 36165
			void stub_get_IsLocaleReliable();

			// Token: 0x06008D46 RID: 36166
			void stub_put_IsLocaleReliable();

			// Token: 0x06008D47 RID: 36167
			void stub_get_IsEndOfDocument();

			// Token: 0x06008D48 RID: 36168
			void stub_put_IsEndOfDocument();

			// Token: 0x06008D49 RID: 36169
			void GetEnumerator([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IEnumVariant val);

			// Token: 0x06008D4A RID: 36170
			void stub_ToString();

			// Token: 0x06008D4B RID: 36171
			void stub_ProcessStream();

			// Token: 0x06008D4C RID: 36172
			void get_ReuseObjects(out bool val);

			// Token: 0x06008D4D RID: 36173
			void put_ReuseObjects(bool val);
		}

		// Token: 0x02000B47 RID: 2887
		[Guid("F0C13A7A-199B-44be-8492-F91EAA50F943")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface ISentence
		{
			// Token: 0x06008D4E RID: 36174
			void stub_get_PropertyCount();

			// Token: 0x06008D4F RID: 36175
			void stub_get_Property();

			// Token: 0x06008D50 RID: 36176
			void stub_put_Property();

			// Token: 0x06008D51 RID: 36177
			void get_Count(out int val);

			// Token: 0x06008D52 RID: 36178
			void stub_get_Parent();

			// Token: 0x06008D53 RID: 36179
			void get_Item(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ITextSegment val);

			// Token: 0x06008D54 RID: 36180
			void stub_get__NewEnum();

			// Token: 0x06008D55 RID: 36181
			void stub_get_Segments();

			// Token: 0x06008D56 RID: 36182
			void stub_GetEnumerator();

			// Token: 0x06008D57 RID: 36183
			void stub_get_IsEndOfParagraph();

			// Token: 0x06008D58 RID: 36184
			void stub_get_IsUnfinished();

			// Token: 0x06008D59 RID: 36185
			void stub_get_IsUnfinishedAtEnd();

			// Token: 0x06008D5A RID: 36186
			void stub_get_Locale();

			// Token: 0x06008D5B RID: 36187
			void stub_get_IsLocaleReliable();

			// Token: 0x06008D5C RID: 36188
			void stub_get_Range();

			// Token: 0x06008D5D RID: 36189
			void stub_get_RequiresNormalization();

			// Token: 0x06008D5E RID: 36190
			void stub_ToString();

			// Token: 0x06008D5F RID: 36191
			void stub_CopyToString();
		}

		// Token: 0x02000B48 RID: 2888
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("AF4656B8-5E5E-4fb2-A2D8-1E977E549A56")]
		[ComImport]
		private interface ITextSegment
		{
			// Token: 0x06008D60 RID: 36192
			void stub_get_IsSurfaceString();

			// Token: 0x06008D61 RID: 36193
			void get_Range([MarshalAs(UnmanagedType.Struct)] out NLGSpellerInterop.STextRange val);

			// Token: 0x06008D62 RID: 36194
			void stub_get_Identifier();

			// Token: 0x06008D63 RID: 36195
			void stub_get_Unit();

			// Token: 0x06008D64 RID: 36196
			void get_Count(out int val);

			// Token: 0x06008D65 RID: 36197
			void get_Item(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ITextSegment val);

			// Token: 0x06008D66 RID: 36198
			void stub_get_Expansions();

			// Token: 0x06008D67 RID: 36199
			void stub_get_Bases();

			// Token: 0x06008D68 RID: 36200
			void stub_get_SuggestionScores();

			// Token: 0x06008D69 RID: 36201
			void stub_get_PropertyCount();

			// Token: 0x06008D6A RID: 36202
			void stub_get_Property();

			// Token: 0x06008D6B RID: 36203
			void stub_put_Property();

			// Token: 0x06008D6C RID: 36204
			void stub_CopyToString();

			// Token: 0x06008D6D RID: 36205
			void get_Role(out NLGSpellerInterop.RangeRole val);

			// Token: 0x06008D6E RID: 36206
			void stub_get_PrimaryType();

			// Token: 0x06008D6F RID: 36207
			void stub_get_SecondaryType();

			// Token: 0x06008D70 RID: 36208
			void stub_get_SpellingVariations();

			// Token: 0x06008D71 RID: 36209
			void stub_get_CharacterNormalizations();

			// Token: 0x06008D72 RID: 36210
			void stub_get_Representations();

			// Token: 0x06008D73 RID: 36211
			void stub_get_Inflections();

			// Token: 0x06008D74 RID: 36212
			void get_Suggestions([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IEnumVariant val);

			// Token: 0x06008D75 RID: 36213
			void stub_get_Lemmas();

			// Token: 0x06008D76 RID: 36214
			void stub_get_SubSegments();

			// Token: 0x06008D77 RID: 36215
			void stub_get_Alternatives();

			// Token: 0x06008D78 RID: 36216
			void stub_ToString();

			// Token: 0x06008D79 RID: 36217
			void stub_get_IsPossiblePhraseStart();

			// Token: 0x06008D7A RID: 36218
			void stub_get_SpellingScore();

			// Token: 0x06008D7B RID: 36219
			void stub_get_IsPunctuation();

			// Token: 0x06008D7C RID: 36220
			void stub_get_IsEndPunctuation();

			// Token: 0x06008D7D RID: 36221
			void stub_get_IsSpace();

			// Token: 0x06008D7E RID: 36222
			void stub_get_IsAbbreviation();

			// Token: 0x06008D7F RID: 36223
			void stub_get_IsSmiley();
		}

		// Token: 0x02000B49 RID: 2889
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("C090356B-A6A5-442a-A204-CFD5415B5902")]
		[ComImport]
		private interface IProcessingOptions
		{
			// Token: 0x06008D80 RID: 36224
			void stub_get__NewEnum();

			// Token: 0x06008D81 RID: 36225
			void stub_GetEnumerator();

			// Token: 0x06008D82 RID: 36226
			void stub_get_Locale();

			// Token: 0x06008D83 RID: 36227
			void stub_get_Count();

			// Token: 0x06008D84 RID: 36228
			void stub_get_Name();

			// Token: 0x06008D85 RID: 36229
			void stub_get_Item();

			// Token: 0x06008D86 RID: 36230
			void put_Item(object index, object val);

			// Token: 0x06008D87 RID: 36231
			void stub_get_IsReadOnly();
		}
	}
}
