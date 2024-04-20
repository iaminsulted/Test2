using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006F8 RID: 1784
	internal class SpellChecker : IDisposable
	{
		// Token: 0x06005D64 RID: 23908 RVA: 0x0028C8BC File Offset: 0x0028B8BC
		public SpellChecker(string languageTag)
		{
			this._speller = new ChangeNotifyWrapper<RCW.ISpellChecker>(null, false);
			this._languageTag = languageTag;
			this._spellCheckerChangedEventHandler = new SpellChecker.SpellCheckerChangedEventHandler(this);
			if (this.Init(false))
			{
				this._speller.PropertyChanged += this.SpellerInstanceChanged;
			}
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x0028C90F File Offset: 0x0028B90F
		private bool Init(bool shouldSuppressCOMExceptions = true)
		{
			this._speller.Value = SpellCheckerFactory.CreateSpellChecker(this._languageTag, shouldSuppressCOMExceptions);
			return this._speller.Value != null;
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x0028C936 File Offset: 0x0028B936
		public string GetLanguageTag()
		{
			if (!this._disposed)
			{
				return this._languageTag;
			}
			return null;
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x0028C948 File Offset: 0x0028B948
		public List<string> SuggestImpl(string word)
		{
			RCW.IEnumString enumString = this._speller.Value.Suggest(word);
			if (enumString == null)
			{
				return null;
			}
			return enumString.ToList(false, true);
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x0028C974 File Offset: 0x0028B974
		public List<string> SuggestImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(() => this.SuggestImpl(word), out result, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x0028C9D4 File Offset: 0x0028B9D4
		public List<string> Suggest(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.SuggestImplWithRetries(word, shouldSuppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x0028C9E8 File Offset: 0x0028B9E8
		private void AddImpl(string word)
		{
			this._speller.Value.Add(word);
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x0028C9FC File Offset: 0x0028B9FC
		private void AddImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.AddImpl(word);
			}, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false);
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x0028CA54 File Offset: 0x0028BA54
		public void Add(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.AddImplWithRetries(word, shouldSuppressCOMExceptions);
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x0028CA67 File Offset: 0x0028BA67
		private void IgnoreImpl(string word)
		{
			this._speller.Value.Ignore(word);
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x0028CA7C File Offset: 0x0028BA7C
		public void IgnoreImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.IgnoreImpl(word);
			}, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false);
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x0028CAD4 File Offset: 0x0028BAD4
		public void Ignore(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.IgnoreImplWithRetries(word, shouldSuppressCOMExceptions);
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x0028CAE7 File Offset: 0x0028BAE7
		private void AutoCorrectImpl(string from, string to)
		{
			this._speller.Value.AutoCorrect(from, to);
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x0028CAFC File Offset: 0x0028BAFC
		private void AutoCorrectImplWithRetries(string from, string to, bool suppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.AutoCorrectImpl(from, to);
			}, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x0028CB5B File Offset: 0x0028BB5B
		public void AutoCorrect(string from, string to, bool suppressCOMExceptions = true)
		{
			this.AutoCorrectImplWithRetries(from, to, suppressCOMExceptions);
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x0028CB66 File Offset: 0x0028BB66
		private byte GetOptionValueImpl(string optionId)
		{
			return this._speller.Value.GetOptionValue(optionId);
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x0028CB7C File Offset: 0x0028BB7C
		private byte GetOptionValueImplWithRetries(string optionId, bool suppressCOMExceptions = true)
		{
			byte result;
			if (!RetryHelper.TryExecuteFunction<byte>(() => this.GetOptionValueImpl(optionId), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x0028CBDA File Offset: 0x0028BBDA
		public byte GetOptionValue(string optionId, bool suppressCOMExceptions = true)
		{
			return this.GetOptionValueImplWithRetries(optionId, suppressCOMExceptions);
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x0028CBE4 File Offset: 0x0028BBE4
		private List<string> GetOptionIdsImpl()
		{
			RCW.IEnumString optionIds = this._speller.Value.OptionIds;
			if (optionIds == null)
			{
				return null;
			}
			return optionIds.ToList(false, true);
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x0028CC10 File Offset: 0x0028BC10
		private List<string> GetOptionIdsImplWithRetries(bool suppressCOMExceptions)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(new Func<List<string>>(this.GetOptionIdsImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x0028CC69 File Offset: 0x0028BC69
		public List<string> GetOptionIds(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetOptionIdsImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x0028CC7C File Offset: 0x0028BC7C
		private string GetIdImpl()
		{
			return this._speller.Value.Id;
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x0028CC90 File Offset: 0x0028BC90
		private string GetIdImplWithRetries(bool suppressCOMExceptions)
		{
			string result = null;
			if (!RetryHelper.TryExecuteFunction<string>(new Func<string>(this.GetIdImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x0028CCE9 File Offset: 0x0028BCE9
		private string GetId(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetIdImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D7C RID: 23932 RVA: 0x0028CCFC File Offset: 0x0028BCFC
		private string GetLocalizedNameImpl()
		{
			return this._speller.Value.LocalizedName;
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x0028CD10 File Offset: 0x0028BD10
		private string GetLocalizedNameImplWithRetries(bool suppressCOMExceptions)
		{
			string result = null;
			if (!RetryHelper.TryExecuteFunction<string>(new Func<string>(this.GetLocalizedNameImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x0028CD69 File Offset: 0x0028BD69
		public string GetLocalizedName(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetLocalizedNameImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x0028CD7C File Offset: 0x0028BD7C
		private SpellChecker.OptionDescription GetOptionDescriptionImpl(string optionId)
		{
			RCW.IOptionDescription optionDescription = this._speller.Value.GetOptionDescription(optionId);
			if (optionDescription == null)
			{
				return null;
			}
			return SpellChecker.OptionDescription.Create(optionDescription, false, true);
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x0028CDA8 File Offset: 0x0028BDA8
		private SpellChecker.OptionDescription GetOptionDescriptionImplWithRetries(string optionId, bool suppressCOMExceptions)
		{
			SpellChecker.OptionDescription result = null;
			if (!RetryHelper.TryExecuteFunction<SpellChecker.OptionDescription>(() => this.GetOptionDescriptionImpl(optionId), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x0028CE08 File Offset: 0x0028BE08
		public SpellChecker.OptionDescription GetOptionDescription(string optionId, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetOptionDescriptionImplWithRetries(optionId, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D82 RID: 23938 RVA: 0x0028CE1C File Offset: 0x0028BE1C
		private List<SpellChecker.SpellingError> CheckImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.Check(text);
			if (enumSpellingError == null)
			{
				return null;
			}
			return enumSpellingError.ToList(this, text, false, true);
		}

		// Token: 0x06005D83 RID: 23939 RVA: 0x0028CE4C File Offset: 0x0028BE4C
		private List<SpellChecker.SpellingError> CheckImplWithRetries(string text, bool suppressCOMExceptions)
		{
			List<SpellChecker.SpellingError> result = null;
			if (!RetryHelper.TryExecuteFunction<List<SpellChecker.SpellingError>>(() => this.CheckImpl(text), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x0028CEAC File Offset: 0x0028BEAC
		public List<SpellChecker.SpellingError> Check(string text, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.CheckImplWithRetries(text, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x0028CEC0 File Offset: 0x0028BEC0
		public List<SpellChecker.SpellingError> ComprehensiveCheckImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.ComprehensiveCheck(text);
			if (enumSpellingError == null)
			{
				return null;
			}
			return enumSpellingError.ToList(this, text, false, true);
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x0028CEF0 File Offset: 0x0028BEF0
		public List<SpellChecker.SpellingError> ComprehensiveCheckImplWithRetries(string text, bool shouldSuppressCOMExceptions = true)
		{
			List<SpellChecker.SpellingError> result = null;
			if (!RetryHelper.TryExecuteFunction<List<SpellChecker.SpellingError>>(() => this.ComprehensiveCheckImpl(text), out result, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D87 RID: 23943 RVA: 0x0028CF50 File Offset: 0x0028BF50
		public List<SpellChecker.SpellingError> ComprehensiveCheck(string text, bool shouldSuppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.ComprehensiveCheckImplWithRetries(text, shouldSuppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D88 RID: 23944 RVA: 0x0028CF64 File Offset: 0x0028BF64
		private uint? add_SpellCheckerChangedImpl(RCW.ISpellCheckerChangedEventHandler handler)
		{
			if (handler == null)
			{
				return new uint?(this._speller.Value.add_SpellCheckerChanged(handler));
			}
			return null;
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x0028CF94 File Offset: 0x0028BF94
		private uint? addSpellCheckerChangedImplWithRetries(RCW.ISpellCheckerChangedEventHandler handler, bool suppressCOMExceptions)
		{
			uint? result;
			if (!RetryHelper.TryExecuteFunction<uint?>(() => this.add_SpellCheckerChangedImpl(handler), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x0028CFFC File Offset: 0x0028BFFC
		private uint? add_SpellCheckerChanged(RCW.ISpellCheckerChangedEventHandler handler, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.addSpellCheckerChangedImplWithRetries(handler, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06005D8B RID: 23947 RVA: 0x0028D023 File Offset: 0x0028C023
		private void remove_SpellCheckerChangedImpl(uint eventCookie)
		{
			this._speller.Value.remove_SpellCheckerChanged(eventCookie);
		}

		// Token: 0x06005D8C RID: 23948 RVA: 0x0028D038 File Offset: 0x0028C038
		private void remove_SpellCheckerChangedImplWithRetries(uint eventCookie, bool suppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.remove_SpellCheckerChangedImpl(eventCookie);
			}, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x0028D090 File Offset: 0x0028C090
		private void remove_SpellCheckerChanged(uint eventCookie, bool suppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.remove_SpellCheckerChangedImplWithRetries(eventCookie, suppressCOMExceptions);
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x0028D0A4 File Offset: 0x0028C0A4
		private void SpellerInstanceChanged(object sender, PropertyChangedEventArgs args)
		{
			if (this._changed != null)
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					if (this._changed != null)
					{
						this._eventCookie = this.add_SpellCheckerChanged(this._spellCheckerChangedEventHandler, true);
					}
				}
			}
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x0028D104 File Offset: 0x0028C104
		internal virtual void OnChanged(SpellChecker.SpellCheckerChangedEventArgs e)
		{
			EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
			if (changed == null)
			{
				return;
			}
			changed(this, e);
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06005D90 RID: 23952 RVA: 0x0028D118 File Offset: 0x0028C118
		// (remove) Token: 0x06005D91 RID: 23953 RVA: 0x0028D174 File Offset: 0x0028C174
		public event EventHandler<SpellChecker.SpellCheckerChangedEventArgs> Changed
		{
			add
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					if (this._changed == null)
					{
						this._eventCookie = this.add_SpellCheckerChanged(this._spellCheckerChangedEventHandler, true);
					}
					this._changed += value;
				}
			}
			remove
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					this._changed -= value;
					if (this._changed == null && this._eventCookie != null)
					{
						this.remove_SpellCheckerChanged(this._eventCookie.Value, true);
						this._eventCookie = null;
					}
				}
			}
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x0028D1E8 File Offset: 0x0028C1E8
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			ChangeNotifyWrapper<RCW.ISpellChecker> speller = this._speller;
			if (((speller != null) ? speller.Value : null) != null)
			{
				try
				{
					Marshal.ReleaseComObject(this._speller.Value);
				}
				catch
				{
				}
				this._speller = null;
			}
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x0028D248 File Offset: 0x0028C248
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x0028D258 File Offset: 0x0028C258
		~SpellChecker()
		{
			this.Dispose(false);
		}

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06005D95 RID: 23957 RVA: 0x0028D288 File Offset: 0x0028C288
		// (remove) Token: 0x06005D96 RID: 23958 RVA: 0x0028D2C0 File Offset: 0x0028C2C0
		private event EventHandler<SpellChecker.SpellCheckerChangedEventArgs> _changed;

		// Token: 0x0400315B RID: 12635
		private static readonly Dictionary<bool, List<Type>> SuppressedExceptions = new Dictionary<bool, List<Type>>
		{
			{
				false,
				new List<Type>()
			},
			{
				true,
				new List<Type>
				{
					typeof(COMException),
					typeof(UnauthorizedAccessException)
				}
			}
		};

		// Token: 0x0400315C RID: 12636
		private ChangeNotifyWrapper<RCW.ISpellChecker> _speller;

		// Token: 0x0400315D RID: 12637
		private string _languageTag;

		// Token: 0x0400315E RID: 12638
		private SpellChecker.SpellCheckerChangedEventHandler _spellCheckerChangedEventHandler;

		// Token: 0x0400315F RID: 12639
		private uint? _eventCookie;

		// Token: 0x04003161 RID: 12641
		private bool _disposed;

		// Token: 0x02000B9C RID: 2972
		internal class OptionDescription
		{
			// Token: 0x17001F44 RID: 8004
			// (get) Token: 0x06008ED2 RID: 36562 RVA: 0x0034334C File Offset: 0x0034234C
			// (set) Token: 0x06008ED3 RID: 36563 RVA: 0x00343354 File Offset: 0x00342354
			internal string Id { get; private set; }

			// Token: 0x17001F45 RID: 8005
			// (get) Token: 0x06008ED4 RID: 36564 RVA: 0x0034335D File Offset: 0x0034235D
			// (set) Token: 0x06008ED5 RID: 36565 RVA: 0x00343365 File Offset: 0x00342365
			internal string Heading { get; private set; }

			// Token: 0x17001F46 RID: 8006
			// (get) Token: 0x06008ED6 RID: 36566 RVA: 0x0034336E File Offset: 0x0034236E
			// (set) Token: 0x06008ED7 RID: 36567 RVA: 0x00343376 File Offset: 0x00342376
			internal string Description { get; private set; }

			// Token: 0x17001F47 RID: 8007
			// (get) Token: 0x06008ED8 RID: 36568 RVA: 0x0034337F File Offset: 0x0034237F
			internal IReadOnlyCollection<string> Labels
			{
				get
				{
					return this._labels.AsReadOnly();
				}
			}

			// Token: 0x06008ED9 RID: 36569 RVA: 0x0034338C File Offset: 0x0034238C
			private OptionDescription(string id, string heading, string description, List<string> labels = null)
			{
				this.Id = id;
				this.Heading = heading;
				this.Description = description;
				this._labels = (labels ?? new List<string>());
			}

			// Token: 0x06008EDA RID: 36570 RVA: 0x003433BC File Offset: 0x003423BC
			internal static SpellChecker.OptionDescription Create(RCW.IOptionDescription optionDescription, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
			{
				if (optionDescription == null)
				{
					throw new ArgumentNullException("optionDescription");
				}
				SpellChecker.OptionDescription optionDescription2 = new SpellChecker.OptionDescription(optionDescription.Id, optionDescription.Heading, optionDescription.Description, null);
				try
				{
					optionDescription2._labels = optionDescription.Labels.ToList(true, true);
				}
				catch (COMException obj) when (shouldSuppressCOMExceptions)
				{
				}
				finally
				{
					if (shouldReleaseCOMObject)
					{
						Marshal.ReleaseComObject(optionDescription);
					}
				}
				return optionDescription2;
			}

			// Token: 0x0400496B RID: 18795
			private List<string> _labels;
		}

		// Token: 0x02000B9D RID: 2973
		internal class SpellCheckerChangedEventArgs : EventArgs
		{
			// Token: 0x06008EDB RID: 36571 RVA: 0x00343444 File Offset: 0x00342444
			internal SpellCheckerChangedEventArgs(SpellChecker spellChecker)
			{
				this.SpellChecker = spellChecker;
			}

			// Token: 0x17001F48 RID: 8008
			// (get) Token: 0x06008EDC RID: 36572 RVA: 0x00343453 File Offset: 0x00342453
			// (set) Token: 0x06008EDD RID: 36573 RVA: 0x0034345B File Offset: 0x0034245B
			internal SpellChecker SpellChecker { get; private set; }
		}

		// Token: 0x02000B9E RID: 2974
		private class SpellCheckerChangedEventHandler : RCW.ISpellCheckerChangedEventHandler
		{
			// Token: 0x06008EDE RID: 36574 RVA: 0x00343464 File Offset: 0x00342464
			internal SpellCheckerChangedEventHandler(SpellChecker spellChecker)
			{
				this._spellChecker = spellChecker;
				this._eventArgs = new SpellChecker.SpellCheckerChangedEventArgs(this._spellChecker);
			}

			// Token: 0x06008EDF RID: 36575 RVA: 0x00343484 File Offset: 0x00342484
			public void Invoke(RCW.ISpellChecker sender)
			{
				SpellChecker spellChecker = this._spellChecker;
				RCW.ISpellChecker spellChecker2;
				if (spellChecker == null)
				{
					spellChecker2 = null;
				}
				else
				{
					ChangeNotifyWrapper<RCW.ISpellChecker> speller = spellChecker._speller;
					spellChecker2 = ((speller != null) ? speller.Value : null);
				}
				if (sender == spellChecker2)
				{
					SpellChecker spellChecker3 = this._spellChecker;
					if (spellChecker3 == null)
					{
						return;
					}
					spellChecker3.OnChanged(this._eventArgs);
				}
			}

			// Token: 0x0400496D RID: 18797
			private SpellChecker.SpellCheckerChangedEventArgs _eventArgs;

			// Token: 0x0400496E RID: 18798
			private SpellChecker _spellChecker;
		}

		// Token: 0x02000B9F RID: 2975
		internal enum CorrectiveAction
		{
			// Token: 0x04004970 RID: 18800
			None,
			// Token: 0x04004971 RID: 18801
			GetSuggestions,
			// Token: 0x04004972 RID: 18802
			Replace,
			// Token: 0x04004973 RID: 18803
			Delete
		}

		// Token: 0x02000BA0 RID: 2976
		internal class SpellingError
		{
			// Token: 0x17001F49 RID: 8009
			// (get) Token: 0x06008EE0 RID: 36576 RVA: 0x003434BD File Offset: 0x003424BD
			internal uint StartIndex { get; }

			// Token: 0x17001F4A RID: 8010
			// (get) Token: 0x06008EE1 RID: 36577 RVA: 0x003434C5 File Offset: 0x003424C5
			internal uint Length { get; }

			// Token: 0x17001F4B RID: 8011
			// (get) Token: 0x06008EE2 RID: 36578 RVA: 0x003434CD File Offset: 0x003424CD
			internal SpellChecker.CorrectiveAction CorrectiveAction { get; }

			// Token: 0x17001F4C RID: 8012
			// (get) Token: 0x06008EE3 RID: 36579 RVA: 0x003434D5 File Offset: 0x003424D5
			internal string Replacement { get; }

			// Token: 0x17001F4D RID: 8013
			// (get) Token: 0x06008EE4 RID: 36580 RVA: 0x003434DD File Offset: 0x003424DD
			internal IReadOnlyCollection<string> Suggestions
			{
				get
				{
					return this._suggestions.AsReadOnly();
				}
			}

			// Token: 0x06008EE5 RID: 36581 RVA: 0x003434EC File Offset: 0x003424EC
			internal SpellingError(RCW.ISpellingError error, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
			{
				if (error == null)
				{
					throw new ArgumentNullException("error");
				}
				this.StartIndex = error.StartIndex;
				this.Length = error.Length;
				this.CorrectiveAction = error.CorrectiveAction;
				this.Replacement = error.Replacement;
				this.PopulateSuggestions(error, spellChecker, text, shouldSuppressCOMExceptions, shouldReleaseCOMObject);
			}

			// Token: 0x06008EE6 RID: 36582 RVA: 0x0034354C File Offset: 0x0034254C
			private void PopulateSuggestions(RCW.ISpellingError error, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions, bool shouldReleaseCOMObject)
			{
				try
				{
					this._suggestions = new List<string>();
					if (this.CorrectiveAction == SpellChecker.CorrectiveAction.GetSuggestions)
					{
						List<string> collection = spellChecker.Suggest(text, shouldSuppressCOMExceptions);
						this._suggestions.AddRange(collection);
					}
					else if (this.CorrectiveAction == SpellChecker.CorrectiveAction.Replace)
					{
						this._suggestions.Add(this.Replacement);
					}
				}
				finally
				{
					if (shouldReleaseCOMObject)
					{
						Marshal.ReleaseComObject(error);
					}
				}
			}

			// Token: 0x04004978 RID: 18808
			private List<string> _suggestions;
		}
	}
}
