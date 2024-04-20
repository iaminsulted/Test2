using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents.MsSpellCheckLib;
using System.Windows.Documents.Tracing;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal.WindowsRuntime.Windows.Data.Text;

namespace System.Windows.Documents
{
	// Token: 0x020006DD RID: 1757
	internal class WinRTSpellerInterop : SpellerInteropBase
	{
		// Token: 0x06005C70 RID: 23664 RVA: 0x0028677C File Offset: 0x0028577C
		internal WinRTSpellerInterop()
		{
			try
			{
				SpellCheckerFactory.Create(false);
			}
			catch (Exception ex) when (ex is InvalidCastException || ex is COMException)
			{
				this.Dispose();
				throw new PlatformNotSupportedException(string.Empty, ex);
			}
			this._spellCheckers = new Dictionary<CultureInfo, Tuple<WordsSegmenter, SpellChecker>>();
			this._customDictionaryFiles = new Dictionary<string, List<string>>();
			InputLanguageManager inputLanguageManager = InputLanguageManager.Current;
			this._defaultCulture = (((inputLanguageManager != null) ? inputLanguageManager.CurrentInputLanguage : null) ?? Thread.CurrentThread.CurrentCulture);
			this._culture = null;
			try
			{
				this.EnsureWordBreakerAndSpellCheckerForCulture(this._defaultCulture, true);
			}
			catch (Exception ex2) when (ex2 is ArgumentException || ex2 is NotSupportedException || ex2 is PlatformNotSupportedException)
			{
				this._spellCheckers = null;
				this.Dispose();
				if (ex2 is PlatformNotSupportedException || ex2 is NotSupportedException)
				{
					throw;
				}
				throw new NotSupportedException(string.Empty, ex2);
			}
			this._dispatcher = new WeakReference<Dispatcher>(Dispatcher.CurrentDispatcher);
			WeakEventManager<AppDomain, UnhandledExceptionEventArgs>.AddHandler(AppDomain.CurrentDomain, "UnhandledException", new EventHandler<UnhandledExceptionEventArgs>(this.ProcessUnhandledException));
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x00243EE0 File Offset: 0x00242EE0
		~WinRTSpellerInterop()
		{
			this.Dispose(false);
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x00243F10 File Offset: 0x00242F10
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x002868C4 File Offset: 0x002858C4
		protected override void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException(SR.Get("TextEditorSpellerInteropHasBeenDisposed"));
			}
			try
			{
				if (this.BeginInvokeOnUIThread(new Action<bool>(this.Dispose), DispatcherPriority.Normal, new object[]
				{
					disposing
				}) == null)
				{
					this.ReleaseAllResources(disposing);
					this._isDisposed = true;
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x00286934 File Offset: 0x00285934
		internal override void SetLocale(CultureInfo culture)
		{
			this.Culture = culture;
		}

		// Token: 0x1700158A RID: 5514
		// (set) Token: 0x06005C75 RID: 23669 RVA: 0x0028693D File Offset: 0x0028593D
		internal override SpellerInteropBase.SpellerMode Mode
		{
			set
			{
				this._mode = value;
			}
		}

		// Token: 0x1700158B RID: 5515
		// (set) Token: 0x06005C76 RID: 23670 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override bool MultiWordMode
		{
			set
			{
			}
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void SetReformMode(CultureInfo culture, SpellingReform spellingReform)
		{
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x00286946 File Offset: 0x00285946
		internal override bool CanSpellCheck(CultureInfo culture)
		{
			return !this._isDisposed && this.EnsureWordBreakerAndSpellCheckerForCulture(culture, false);
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x0028695C File Offset: 0x0028595C
		internal override void UnloadDictionary(object token)
		{
			if (this._isDisposed)
			{
				return;
			}
			Tuple<string, string> tuple = (Tuple<string, string>)token;
			string item = tuple.Item1;
			string item2 = tuple.Item2;
			using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary))
			{
				SpellCheckerFactory.UnregisterUserDictionary(item2, item, true);
			}
			FileHelper.DeleteTemporaryFile(item2);
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x002869B8 File Offset: 0x002859B8
		internal override object LoadDictionary(string lexiconFilePath)
		{
			if (!this._isDisposed)
			{
				return this.LoadDictionaryImpl(lexiconFilePath);
			}
			return null;
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x002869CB File Offset: 0x002859CB
		internal override object LoadDictionary(Uri item, string trustedFolder)
		{
			if (this._isDisposed)
			{
				return null;
			}
			return this.LoadDictionaryImpl(item.LocalPath);
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x002869E3 File Offset: 0x002859E3
		internal override void ReleaseAllLexicons()
		{
			if (!this._isDisposed)
			{
				this.ClearDictionaries(false);
			}
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x002869F4 File Offset: 0x002859F4
		private bool EnsureWordBreakerAndSpellCheckerForCulture(CultureInfo culture, bool throwOnError = false)
		{
			if (this._isDisposed || culture == null)
			{
				return false;
			}
			if (!this._spellCheckers.ContainsKey(culture))
			{
				WordsSegmenter wordsSegmenter = null;
				try
				{
					wordsSegmenter = WordsSegmenter.Create(culture.Name, true);
				}
				catch when (endfilter(!throwOnError > false))
				{
					wordsSegmenter = null;
				}
				if (wordsSegmenter == null)
				{
					this._spellCheckers[culture] = null;
					return false;
				}
				SpellChecker spellChecker = null;
				try
				{
					using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.SpellCheckerCreation))
					{
						spellChecker = new SpellChecker(culture.Name);
					}
				}
				catch (Exception ex)
				{
					spellChecker = null;
					if (throwOnError && ex is ArgumentException)
					{
						throw new NotSupportedException(string.Empty, ex);
					}
				}
				if (spellChecker == null)
				{
					this._spellCheckers[culture] = null;
				}
				else
				{
					this._spellCheckers[culture] = new Tuple<WordsSegmenter, SpellChecker>(wordsSegmenter, spellChecker);
				}
			}
			return this._spellCheckers[culture] != null;
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x00286AF8 File Offset: 0x00285AF8
		internal override int EnumTextSegments(char[] text, int count, SpellerInteropBase.EnumSentencesCallback sentenceCallback, SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
		{
			if (this._isDisposed)
			{
				return 0;
			}
			WordsSegmenter wordsSegmenter = this.CurrentWordBreaker ?? this.DefaultCultureWordBreaker;
			SpellChecker currentSpellChecker = this.CurrentSpellChecker;
			bool flag = this._mode.HasFlag(SpellerInteropBase.SpellerMode.SpellingErrors) || this._mode.HasFlag(SpellerInteropBase.SpellerMode.Suggestions);
			if (wordsSegmenter == null || (flag && currentSpellChecker == null))
			{
				return 0;
			}
			int num = 0;
			bool flag2 = true;
			string[] array = new string[]
			{
				string.Join<char>(string.Empty, text)
			};
			for (int i = 0; i < array.Length; i++)
			{
				WinRTSpellerInterop.SpellerSentence spellerSentence = new WinRTSpellerInterop.SpellerSentence(array[i], wordsSegmenter, this.CurrentSpellChecker, this);
				num += spellerSentence.Segments.Count;
				if (segmentCallback != null)
				{
					int num2 = 0;
					while (flag2 && num2 < spellerSentence.Segments.Count)
					{
						flag2 = segmentCallback(spellerSentence.Segments[num2], data);
						num2++;
					}
				}
				if (sentenceCallback != null)
				{
					flag2 = sentenceCallback(spellerSentence, data);
				}
				if (!flag2)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x00286C0C File Offset: 0x00285C0C
		private Tuple<string, string> LoadDictionaryImpl(string lexiconFilePath)
		{
			if (this._isDisposed)
			{
				return new Tuple<string, string>(null, null);
			}
			if (!File.Exists(lexiconFilePath))
			{
				throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
				{
					lexiconFilePath
				}));
			}
			bool flag = false;
			string text = null;
			Tuple<string, string> result;
			try
			{
				CultureInfo cultureInfo = null;
				using (FileStream fileStream = new FileStream(lexiconFilePath, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						cultureInfo = WinRTSpellerInterop.TryParseLexiconCulture(streamReader.ReadLine());
					}
				}
				string ietfLanguageTag = cultureInfo.IetfLanguageTag;
				using (FileStream fileStream2 = FileHelper.CreateAndOpenTemporaryFile(out text, FileAccess.Write, FileOptions.None, "dic", "WPF"))
				{
					WinRTSpellerInterop.CopyToUnicodeFile(lexiconFilePath, fileStream2);
					flag = true;
				}
				if (!this._customDictionaryFiles.ContainsKey(ietfLanguageTag))
				{
					this._customDictionaryFiles[ietfLanguageTag] = new List<string>();
				}
				this._customDictionaryFiles[ietfLanguageTag].Add(text);
				using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.RegisterUserDictionary))
				{
					SpellCheckerFactory.RegisterUserDictionary(text, ietfLanguageTag, true);
				}
				result = new Tuple<string, string>(ietfLanguageTag, text);
			}
			catch (Exception ex) when (ex is ArgumentException || !flag)
			{
				if (text != null)
				{
					FileHelper.DeleteTemporaryFile(text);
				}
				throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
				{
					lexiconFilePath
				}), ex);
			}
			return result;
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x00286DA8 File Offset: 0x00285DA8
		private void ClearDictionaries(bool disposing = false)
		{
			if (this._isDisposed)
			{
				return;
			}
			if (this._customDictionaryFiles != null)
			{
				foreach (KeyValuePair<string, List<string>> keyValuePair in this._customDictionaryFiles)
				{
					string key = keyValuePair.Key;
					foreach (string text in keyValuePair.Value)
					{
						try
						{
							using (new SpellerCOMActionTraceLogger(this, SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary))
							{
								SpellCheckerFactory.UnregisterUserDictionary(text, key, true);
							}
							FileHelper.DeleteTemporaryFile(text);
						}
						catch
						{
						}
					}
				}
				this._customDictionaryFiles.Clear();
			}
			if (disposing)
			{
				this._customDictionaryFiles = null;
			}
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x00286EA8 File Offset: 0x00285EA8
		private static CultureInfo TryParseLexiconCulture(string line)
		{
			RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant;
			CultureInfo result = CultureInfo.InvariantCulture;
			if (line == null)
			{
				return result;
			}
			string[] array = Regex.Split(line.Trim(), "\\s*\\#LID\\s+(\\d+)\\s*", options);
			if (array.Length != 3)
			{
				return result;
			}
			string a = array[0];
			string s = array[1];
			string a2 = array[2];
			int culture;
			if (a != string.Empty || a2 != string.Empty || !int.TryParse(s, out culture))
			{
				return result;
			}
			try
			{
				result = new CultureInfo(culture);
			}
			catch (CultureNotFoundException)
			{
				result = CultureInfo.InvariantCulture;
			}
			return result;
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x00286F3C File Offset: 0x00285F3C
		private static void CopyToUnicodeFile(string sourcePath, FileStream targetStream)
		{
			using (FileStream fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
			{
				bool flag = fileStream.ReadByte() == 255 && fileStream.ReadByte() == 254;
				fileStream.Seek(0L, SeekOrigin.Begin);
				if (flag)
				{
					fileStream.CopyTo(targetStream);
				}
				else
				{
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						using (StreamWriter streamWriter = new StreamWriter(targetStream, Encoding.Unicode))
						{
							string value;
							while ((value = streamReader.ReadLine()) != null)
							{
								streamWriter.WriteLine(value);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x00286FF4 File Offset: 0x00285FF4
		private void ProcessUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			bool flag = false;
			try
			{
				if (this.BeginInvokeOnUIThread(new Action<bool>(this.ClearDictionaries), DispatcherPriority.Normal, new object[]
				{
					flag
				}) == null)
				{
					this.ClearDictionaries(flag);
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x00287044 File Offset: 0x00286044
		private void ReleaseAllResources(bool disposing)
		{
			if (this._spellCheckers != null)
			{
				foreach (Tuple<WordsSegmenter, SpellChecker> tuple in this._spellCheckers.Values)
				{
					SpellChecker spellChecker = (tuple != null) ? tuple.Item2 : null;
					if (spellChecker != null)
					{
						spellChecker.Dispose();
					}
				}
				this._spellCheckers = null;
			}
			this.ClearDictionaries(disposing);
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x002870C0 File Offset: 0x002860C0
		private DispatcherOperation BeginInvokeOnUIThread(Delegate method, DispatcherPriority priority, params object[] args)
		{
			Dispatcher dispatcher = null;
			if (this._dispatcher == null || !this._dispatcher.TryGetTarget(out dispatcher) || dispatcher == null)
			{
				throw new InvalidOperationException();
			}
			if (!dispatcher.CheckAccess())
			{
				return dispatcher.BeginInvoke(method, priority, args);
			}
			return null;
		}

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06005C86 RID: 23686 RVA: 0x00287102 File Offset: 0x00286102
		// (set) Token: 0x06005C87 RID: 23687 RVA: 0x0028710A File Offset: 0x0028610A
		private CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				this._culture = value;
				this.EnsureWordBreakerAndSpellCheckerForCulture(this._culture, false);
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06005C88 RID: 23688 RVA: 0x00287121 File Offset: 0x00286121
		private WordsSegmenter CurrentWordBreaker
		{
			get
			{
				if (this.Culture == null)
				{
					return null;
				}
				this.EnsureWordBreakerAndSpellCheckerForCulture(this.Culture, false);
				Tuple<WordsSegmenter, SpellChecker> tuple = this._spellCheckers[this.Culture];
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item1;
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06005C89 RID: 23689 RVA: 0x00287157 File Offset: 0x00286157
		private WordsSegmenter DefaultCultureWordBreaker
		{
			get
			{
				if (this._defaultCulture == null)
				{
					return null;
				}
				Tuple<WordsSegmenter, SpellChecker> tuple = this._spellCheckers[this._defaultCulture];
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item1;
			}
		}

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06005C8A RID: 23690 RVA: 0x0028717F File Offset: 0x0028617F
		private SpellChecker CurrentSpellChecker
		{
			get
			{
				if (this.Culture == null)
				{
					return null;
				}
				this.EnsureWordBreakerAndSpellCheckerForCulture(this.Culture, false);
				Tuple<WordsSegmenter, SpellChecker> tuple = this._spellCheckers[this.Culture];
				if (tuple == null)
				{
					return null;
				}
				return tuple.Item2;
			}
		}

		// Token: 0x040030D0 RID: 12496
		private bool _isDisposed;

		// Token: 0x040030D1 RID: 12497
		private SpellerInteropBase.SpellerMode _mode;

		// Token: 0x040030D2 RID: 12498
		private Dictionary<CultureInfo, Tuple<WordsSegmenter, SpellChecker>> _spellCheckers;

		// Token: 0x040030D3 RID: 12499
		private CultureInfo _defaultCulture;

		// Token: 0x040030D4 RID: 12500
		private CultureInfo _culture;

		// Token: 0x040030D5 RID: 12501
		private Dictionary<string, List<string>> _customDictionaryFiles;

		// Token: 0x040030D6 RID: 12502
		private readonly WeakReference<Dispatcher> _dispatcher;

		// Token: 0x02000B81 RID: 2945
		internal readonly struct TextRange : SpellerInteropBase.ITextRange
		{
			// Token: 0x06008E30 RID: 36400 RVA: 0x0034079F File Offset: 0x0033F79F
			public TextRange(TextSegment textSegment)
			{
				this._length = (int)textSegment.Length;
				this._start = (int)textSegment.StartPosition;
			}

			// Token: 0x06008E31 RID: 36401 RVA: 0x003407B9 File Offset: 0x0033F7B9
			public TextRange(int start, int length)
			{
				this._start = start;
				this._length = length;
			}

			// Token: 0x06008E32 RID: 36402 RVA: 0x003407C9 File Offset: 0x0033F7C9
			public TextRange(SpellerInteropBase.ITextRange textRange)
			{
				this = new WinRTSpellerInterop.TextRange(textRange.Start, textRange.Length);
			}

			// Token: 0x06008E33 RID: 36403 RVA: 0x003407DD File Offset: 0x0033F7DD
			public static explicit operator WinRTSpellerInterop.TextRange(TextSegment textSegment)
			{
				return new WinRTSpellerInterop.TextRange(textSegment);
			}

			// Token: 0x17001F1A RID: 7962
			// (get) Token: 0x06008E34 RID: 36404 RVA: 0x003407E5 File Offset: 0x0033F7E5
			public int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001F1B RID: 7963
			// (get) Token: 0x06008E35 RID: 36405 RVA: 0x003407ED File Offset: 0x0033F7ED
			public int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x04004917 RID: 18711
			private readonly int _start;

			// Token: 0x04004918 RID: 18712
			private readonly int _length;
		}

		// Token: 0x02000B82 RID: 2946
		[DebuggerDisplay("SubSegments.Count = {SubSegments.Count} TextRange = {TextRange.Start},{TextRange.Length}")]
		internal class SpellerSegment : SpellerInteropBase.ISpellerSegment
		{
			// Token: 0x06008E36 RID: 36406 RVA: 0x003407F5 File Offset: 0x0033F7F5
			public SpellerSegment(string sourceString, SpellerInteropBase.ITextRange textRange, SpellChecker spellChecker, WinRTSpellerInterop owner)
			{
				this._spellChecker = spellChecker;
				this._suggestions = null;
				this.Owner = owner;
				this.SourceString = sourceString;
				this.TextRange = textRange;
			}

			// Token: 0x06008E38 RID: 36408 RVA: 0x00340834 File Offset: 0x0033F834
			private void EnumerateSuggestions()
			{
				List<string> list = new List<string>();
				this._isClean = new bool?(true);
				if (this._spellChecker == null)
				{
					this._suggestions = list.AsReadOnly();
					return;
				}
				List<SpellChecker.SpellingError> list2 = null;
				using (new SpellerCOMActionTraceLogger(this.Owner, SpellerCOMActionTraceLogger.Actions.ComprehensiveCheck))
				{
					list2 = ((this.Text != null) ? this._spellChecker.ComprehensiveCheck(this.Text, true) : null);
				}
				if (list2 == null)
				{
					this._suggestions = list.AsReadOnly();
					return;
				}
				foreach (SpellChecker.SpellingError spellingError in list2)
				{
					list.AddRange(spellingError.Suggestions);
					if (spellingError.CorrectiveAction != SpellChecker.CorrectiveAction.None)
					{
						this._isClean = new bool?(false);
					}
				}
				this._suggestions = list.AsReadOnly();
			}

			// Token: 0x17001F1C RID: 7964
			// (get) Token: 0x06008E39 RID: 36409 RVA: 0x00340928 File Offset: 0x0033F928
			public string SourceString { get; }

			// Token: 0x17001F1D RID: 7965
			// (get) Token: 0x06008E3A RID: 36410 RVA: 0x00340930 File Offset: 0x0033F930
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

			// Token: 0x17001F1E RID: 7966
			// (get) Token: 0x06008E3B RID: 36411 RVA: 0x00340959 File Offset: 0x0033F959
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> SubSegments
			{
				get
				{
					return WinRTSpellerInterop.SpellerSegment._empty;
				}
			}

			// Token: 0x17001F1F RID: 7967
			// (get) Token: 0x06008E3C RID: 36412 RVA: 0x00340960 File Offset: 0x0033F960
			public SpellerInteropBase.ITextRange TextRange { get; }

			// Token: 0x17001F20 RID: 7968
			// (get) Token: 0x06008E3D RID: 36413 RVA: 0x00340968 File Offset: 0x0033F968
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

			// Token: 0x17001F21 RID: 7969
			// (get) Token: 0x06008E3E RID: 36414 RVA: 0x0034097E File Offset: 0x0033F97E
			public bool IsClean
			{
				get
				{
					if (this._isClean == null)
					{
						this.EnumerateSuggestions();
					}
					return this._isClean.Value;
				}
			}

			// Token: 0x17001F22 RID: 7970
			// (get) Token: 0x06008E3F RID: 36415 RVA: 0x0034099E File Offset: 0x0033F99E
			internal WinRTSpellerInterop Owner { get; }

			// Token: 0x06008E40 RID: 36416 RVA: 0x003409A8 File Offset: 0x0033F9A8
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

			// Token: 0x0400491C RID: 18716
			private SpellChecker _spellChecker;

			// Token: 0x0400491D RID: 18717
			private IReadOnlyList<string> _suggestions;

			// Token: 0x0400491E RID: 18718
			private bool? _isClean;

			// Token: 0x0400491F RID: 18719
			private static readonly IReadOnlyList<SpellerInteropBase.ISpellerSegment> _empty = new List<SpellerInteropBase.ISpellerSegment>().AsReadOnly();
		}

		// Token: 0x02000B83 RID: 2947
		[DebuggerDisplay("Sentence = {_sentence}")]
		private class SpellerSentence : SpellerInteropBase.ISpellerSentence
		{
			// Token: 0x06008E41 RID: 36417 RVA: 0x003409E4 File Offset: 0x0033F9E4
			public SpellerSentence(string sentence, WordsSegmenter wordBreaker, SpellChecker spellChecker, WinRTSpellerInterop owner)
			{
				this._sentence = sentence;
				this._wordBreaker = wordBreaker;
				this._spellChecker = spellChecker;
				this._segments = null;
				this._owner = owner;
			}

			// Token: 0x17001F23 RID: 7971
			// (get) Token: 0x06008E42 RID: 36418 RVA: 0x00340A10 File Offset: 0x0033FA10
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> Segments
			{
				get
				{
					if (this._segments == null)
					{
						this._segments = this._wordBreaker.ComprehensiveGetTokens(this._sentence, this._spellChecker, this._owner);
					}
					return this._segments;
				}
			}

			// Token: 0x17001F24 RID: 7972
			// (get) Token: 0x06008E43 RID: 36419 RVA: 0x00340A44 File Offset: 0x0033FA44
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

			// Token: 0x04004920 RID: 18720
			private string _sentence;

			// Token: 0x04004921 RID: 18721
			private WordsSegmenter _wordBreaker;

			// Token: 0x04004922 RID: 18722
			private SpellChecker _spellChecker;

			// Token: 0x04004923 RID: 18723
			private IReadOnlyList<WinRTSpellerInterop.SpellerSegment> _segments;

			// Token: 0x04004924 RID: 18724
			private WinRTSpellerInterop _owner;
		}
	}
}
