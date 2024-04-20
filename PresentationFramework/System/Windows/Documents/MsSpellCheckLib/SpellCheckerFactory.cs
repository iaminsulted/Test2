using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using MS.Internal;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006F9 RID: 1785
	internal class SpellCheckerFactory
	{
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06005D98 RID: 23960 RVA: 0x0028D347 File Offset: 0x0028C347
		// (set) Token: 0x06005D99 RID: 23961 RVA: 0x0028D34F File Offset: 0x0028C34F
		internal RCW.ISpellCheckerFactory ComFactory { get; private set; }

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06005D9A RID: 23962 RVA: 0x0028D358 File Offset: 0x0028C358
		// (set) Token: 0x06005D9B RID: 23963 RVA: 0x0028D35F File Offset: 0x0028C35F
		internal static SpellCheckerFactory Singleton { get; private set; } = new SpellCheckerFactory();

		// Token: 0x06005D9C RID: 23964 RVA: 0x0028D368 File Offset: 0x0028C368
		public static SpellCheckerFactory Create(bool shouldSuppressCOMExceptions = false)
		{
			SpellCheckerFactory result = null;
			bool flag = false;
			if (SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), shouldSuppressCOMExceptions, false, out flag) && flag)
			{
				result = SpellCheckerFactory.Singleton;
			}
			return result;
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private SpellCheckerFactory()
		{
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x0028D3A0 File Offset: 0x0028C3A0
		static SpellCheckerFactory()
		{
			bool flag = false;
			SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), true, true, out flag);
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x0028D424 File Offset: 0x0028C424
		private static bool Reinitalize()
		{
			bool flag = false;
			return SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), false, false, out flag) && flag;
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x0028D450 File Offset: 0x0028C450
		private static bool CreateLockFree(bool suppressCOMExceptions = true, bool suppressOtherExceptions = true)
		{
			if (SpellCheckerFactory.Singleton.ComFactory != null)
			{
				try
				{
					Marshal.ReleaseComObject(SpellCheckerFactory.Singleton.ComFactory);
				}
				catch
				{
				}
				SpellCheckerFactory.Singleton.ComFactory = null;
			}
			bool flag = false;
			RCW.ISpellCheckerFactory comFactory = null;
			try
			{
				comFactory = new RCW.SpellCheckerFactoryCoClass();
				flag = true;
			}
			catch (COMException obj) when (suppressCOMExceptions)
			{
			}
			catch (UnauthorizedAccessException inner)
			{
				if (!suppressCOMExceptions)
				{
					throw new COMException(string.Empty, inner);
				}
			}
			catch (InvalidCastException inner2)
			{
				if (!suppressCOMExceptions)
				{
					throw new COMException(string.Empty, inner2);
				}
			}
			catch (Exception ex) when (suppressOtherExceptions && !(ex is COMException) && !(ex is UnauthorizedAccessException))
			{
			}
			if (flag)
			{
				SpellCheckerFactory.Singleton.ComFactory = comFactory;
			}
			return flag;
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x0028D554 File Offset: 0x0028C554
		private List<string> SupportedLanguagesImpl()
		{
			RCW.ISpellCheckerFactory comFactory = this.ComFactory;
			RCW.IEnumString enumString = (comFactory != null) ? comFactory.SupportedLanguages : null;
			List<string> result = null;
			if (enumString != null)
			{
				result = enumString.ToList(true, true);
			}
			return result;
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x0028D584 File Offset: 0x0028C584
		private List<string> SupportedLanguagesImplWithRetries(bool shouldSuppressCOMExceptions)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(new Func<List<string>>(this.SupportedLanguagesImpl), out result, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x0028D5D8 File Offset: 0x0028C5D8
		private List<string> GetSupportedLanguagesPrivate(bool shouldSuppressCOMExceptions = true)
		{
			List<string> result = null;
			if (!SpellCheckerFactory._factoryLock.WithWriteLock<bool, List<string>>(new Func<bool, List<string>>(this.SupportedLanguagesImplWithRetries), shouldSuppressCOMExceptions, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x0028D605 File Offset: 0x0028C605
		internal static List<string> GetSupportedLanguages(bool shouldSuppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return null;
			}
			return singleton.GetSupportedLanguagesPrivate(shouldSuppressCOMExceptions);
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x0028D618 File Offset: 0x0028C618
		private bool IsSupportedImpl(string languageTag)
		{
			return this.ComFactory != null && this.ComFactory.IsSupported(languageTag) != 0;
		}

		// Token: 0x06005DA6 RID: 23974 RVA: 0x0028D634 File Offset: 0x0028C634
		private bool IsSupportedImplWithRetries(string languageTag, bool suppressCOMExceptions = true)
		{
			bool flag = false;
			return RetryHelper.TryExecuteFunction<bool>(() => this.IsSupportedImpl(languageTag), out flag, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false) && flag;
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x0028D69C File Offset: 0x0028C69C
		private bool IsSupportedPrivate(string languageTag, bool suppressCOMExceptons = true)
		{
			bool flag = false;
			return SpellCheckerFactory._factoryLock.WithWriteLock<string, bool, bool>(new Func<string, bool, bool>(this.IsSupportedImplWithRetries), languageTag, suppressCOMExceptons, out flag) && flag;
		}

		// Token: 0x06005DA8 RID: 23976 RVA: 0x0028D6CA File Offset: 0x0028C6CA
		internal static bool IsSupported(string languageTag, bool suppressCOMExceptons = true)
		{
			return SpellCheckerFactory.Singleton != null && SpellCheckerFactory.Singleton.IsSupportedPrivate(languageTag, suppressCOMExceptons);
		}

		// Token: 0x06005DA9 RID: 23977 RVA: 0x0028D6E1 File Offset: 0x0028C6E1
		private RCW.ISpellChecker CreateSpellCheckerImpl(string languageTag)
		{
			return SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellChecker();
		}

		// Token: 0x06005DAA RID: 23978 RVA: 0x0028D6F0 File Offset: 0x0028C6F0
		private RCW.ISpellChecker CreateSpellCheckerImplWithRetries(string languageTag, bool suppressCOMExceptions = true)
		{
			RCW.ISpellChecker result = null;
			if (!RetryHelper.TryExecuteFunction<RCW.ISpellChecker>(new Func<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellChecker), out result, new RetryHelper.RetryFunctionPreamble<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellCheckerRetryPreamble), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x0028D73C File Offset: 0x0028C73C
		private RCW.ISpellChecker CreateSpellCheckerPrivate(string languageTag, bool suppressCOMExceptions = true)
		{
			RCW.ISpellChecker result = null;
			if (!SpellCheckerFactory._factoryLock.WithWriteLock<string, bool, RCW.ISpellChecker>(new Func<string, bool, RCW.ISpellChecker>(this.CreateSpellCheckerImplWithRetries), languageTag, suppressCOMExceptions, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x0028D76A File Offset: 0x0028C76A
		internal static RCW.ISpellChecker CreateSpellChecker(string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return null;
			}
			return singleton.CreateSpellCheckerPrivate(languageTag, suppressCOMExceptions);
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x0028D77E File Offset: 0x0028C77E
		private void RegisterUserDicionaryImpl(string dictionaryPath, string languageTag)
		{
			RCW.IUserDictionariesRegistrar userDictionariesRegistrar = (RCW.IUserDictionariesRegistrar)this.ComFactory;
			if (userDictionariesRegistrar == null)
			{
				return;
			}
			userDictionariesRegistrar.RegisterUserDictionary(dictionaryPath, languageTag);
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x0028D798 File Offset: 0x0028C798
		private void RegisterUserDictionaryImplWithRetries(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			if (dictionaryPath == null)
			{
				throw new ArgumentNullException("dictionaryPath");
			}
			if (languageTag == null)
			{
				throw new ArgumentNullException("languageTag");
			}
			RetryHelper.TryCallAction(delegate()
			{
				this.RegisterUserDicionaryImpl(dictionaryPath, languageTag);
			}, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x0028D824 File Offset: 0x0028C824
		private void RegisterUserDictionaryPrivate(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory._factoryLock.WithWriteLock(delegate()
			{
				this.RegisterUserDictionaryImplWithRetries(dictionaryPath, languageTag, suppressCOMExceptions);
			});
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x0028D86A File Offset: 0x0028C86A
		internal static void RegisterUserDictionary(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return;
			}
			singleton.RegisterUserDictionaryPrivate(dictionaryPath, languageTag, suppressCOMExceptions);
		}

		// Token: 0x06005DB1 RID: 23985 RVA: 0x0028D87E File Offset: 0x0028C87E
		private void UnregisterUserDictionaryImpl(string dictionaryPath, string languageTag)
		{
			RCW.IUserDictionariesRegistrar userDictionariesRegistrar = (RCW.IUserDictionariesRegistrar)this.ComFactory;
			if (userDictionariesRegistrar == null)
			{
				return;
			}
			userDictionariesRegistrar.UnregisterUserDictionary(dictionaryPath, languageTag);
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x0028D898 File Offset: 0x0028C898
		private void UnregisterUserDictionaryImplWithRetries(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			if (dictionaryPath == null)
			{
				throw new ArgumentNullException("dictionaryPath");
			}
			if (languageTag == null)
			{
				throw new ArgumentNullException("languageTag");
			}
			RetryHelper.TryCallAction(delegate()
			{
				this.UnregisterUserDictionaryImpl(dictionaryPath, languageTag);
			}, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x0028D924 File Offset: 0x0028C924
		private void UnregisterUserDictionaryPrivate(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory._factoryLock.WithWriteLock(delegate()
			{
				this.UnregisterUserDictionaryImplWithRetries(dictionaryPath, languageTag, suppressCOMExceptions);
			});
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x0028D96A File Offset: 0x0028C96A
		internal static void UnregisterUserDictionary(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return;
			}
			singleton.UnregisterUserDictionaryPrivate(dictionaryPath, languageTag, suppressCOMExceptions);
		}

		// Token: 0x04003163 RID: 12643
		private static ReaderWriterLockSlimWrapper _factoryLock = new ReaderWriterLockSlimWrapper(LockRecursionPolicy.SupportsRecursion, true);

		// Token: 0x04003165 RID: 12645
		private static Dictionary<bool, List<Type>> SuppressedExceptions = new Dictionary<bool, List<Type>>
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

		// Token: 0x02000BAE RID: 2990
		private class SpellCheckerCreationHelper
		{
			// Token: 0x17001F4E RID: 8014
			// (get) Token: 0x06008F0B RID: 36619 RVA: 0x0034377B File Offset: 0x0034277B
			public static RCW.ISpellCheckerFactory ComFactory
			{
				get
				{
					return SpellCheckerFactory.Singleton.ComFactory;
				}
			}

			// Token: 0x06008F0C RID: 36620 RVA: 0x00343787 File Offset: 0x00342787
			private SpellCheckerCreationHelper(string language)
			{
				this._language = language;
			}

			// Token: 0x06008F0D RID: 36621 RVA: 0x00343796 File Offset: 0x00342796
			private static void CreateForLanguage(string language)
			{
				SpellCheckerFactory.SpellCheckerCreationHelper._instances[language] = new SpellCheckerFactory.SpellCheckerCreationHelper(language);
			}

			// Token: 0x06008F0E RID: 36622 RVA: 0x003437A9 File Offset: 0x003427A9
			public static SpellCheckerFactory.SpellCheckerCreationHelper Helper(string language)
			{
				if (!SpellCheckerFactory.SpellCheckerCreationHelper._instances.ContainsKey(language))
				{
					SpellCheckerFactory.SpellCheckerCreationHelper._lock.WithWriteLock<string>(new Action<string>(SpellCheckerFactory.SpellCheckerCreationHelper.CreateForLanguage), language);
				}
				return SpellCheckerFactory.SpellCheckerCreationHelper._instances[language];
			}

			// Token: 0x06008F0F RID: 36623 RVA: 0x003437DB File Offset: 0x003427DB
			public RCW.ISpellChecker CreateSpellChecker()
			{
				RCW.ISpellCheckerFactory comFactory = SpellCheckerFactory.SpellCheckerCreationHelper.ComFactory;
				if (comFactory == null)
				{
					return null;
				}
				return comFactory.CreateSpellChecker(this._language);
			}

			// Token: 0x06008F10 RID: 36624 RVA: 0x003437F3 File Offset: 0x003427F3
			public bool CreateSpellCheckerRetryPreamble(out Func<RCW.ISpellChecker> func)
			{
				func = null;
				bool flag = SpellCheckerFactory.Reinitalize();
				if (flag)
				{
					func = new Func<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(this._language).CreateSpellChecker);
				}
				return flag;
			}

			// Token: 0x0400499E RID: 18846
			private static Dictionary<string, SpellCheckerFactory.SpellCheckerCreationHelper> _instances = new Dictionary<string, SpellCheckerFactory.SpellCheckerCreationHelper>();

			// Token: 0x0400499F RID: 18847
			private static ReaderWriterLockSlimWrapper _lock = new ReaderWriterLockSlimWrapper(LockRecursionPolicy.NoRecursion, true);

			// Token: 0x040049A0 RID: 18848
			private string _language;
		}
	}
}
