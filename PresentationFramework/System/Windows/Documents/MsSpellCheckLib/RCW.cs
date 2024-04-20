using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006F7 RID: 1783
	internal class RCW
	{
		// Token: 0x02000B90 RID: 2960
		internal enum WORDLIST_TYPE
		{
			// Token: 0x0400495F RID: 18783
			WORDLIST_TYPE_IGNORE,
			// Token: 0x04004960 RID: 18784
			WORDLIST_TYPE_ADD,
			// Token: 0x04004961 RID: 18785
			WORDLIST_TYPE_EXCLUDE,
			// Token: 0x04004962 RID: 18786
			WORDLIST_TYPE_AUTOCORRECT
		}

		// Token: 0x02000B91 RID: 2961
		internal enum CORRECTIVE_ACTION
		{
			// Token: 0x04004964 RID: 18788
			CORRECTIVE_ACTION_NONE,
			// Token: 0x04004965 RID: 18789
			CORRECTIVE_ACTION_GET_SUGGESTIONS,
			// Token: 0x04004966 RID: 18790
			CORRECTIVE_ACTION_REPLACE,
			// Token: 0x04004967 RID: 18791
			CORRECTIVE_ACTION_DELETE
		}

		// Token: 0x02000B92 RID: 2962
		[Guid("B7C82D61-FBE8-4B47-9B27-6C0D2E0DE0A3")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ISpellingError
		{
			// Token: 0x17001F36 RID: 7990
			// (get) Token: 0x06008EAB RID: 36523
			uint StartIndex { [MethodImpl(MethodImplOptions.InternalCall)] get; }

			// Token: 0x17001F37 RID: 7991
			// (get) Token: 0x06008EAC RID: 36524
			uint Length { [MethodImpl(MethodImplOptions.InternalCall)] get; }

			// Token: 0x17001F38 RID: 7992
			// (get) Token: 0x06008EAD RID: 36525
			RCW.CORRECTIVE_ACTION CorrectiveAction { [MethodImpl(MethodImplOptions.InternalCall)] get; }

			// Token: 0x17001F39 RID: 7993
			// (get) Token: 0x06008EAE RID: 36526
			string Replacement { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
		}

		// Token: 0x02000B93 RID: 2963
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("803E3BD4-2828-4410-8290-418D1D73C762")]
		[ComImport]
		internal interface IEnumSpellingError
		{
			// Token: 0x06008EAF RID: 36527
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.ISpellingError Next();
		}

		// Token: 0x02000B94 RID: 2964
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000101-0000-0000-C000-000000000046")]
		[ComImport]
		internal interface IEnumString
		{
			// Token: 0x06008EB0 RID: 36528
			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteNext([In] uint celt, [MarshalAs(UnmanagedType.LPWStr)] out string rgelt, out uint pceltFetched);

			// Token: 0x06008EB1 RID: 36529
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Skip([In] uint celt);

			// Token: 0x06008EB2 RID: 36530
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Reset();

			// Token: 0x06008EB3 RID: 36531
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clone([MarshalAs(UnmanagedType.Interface)] out RCW.IEnumString ppenum);
		}

		// Token: 0x02000B95 RID: 2965
		[Guid("432E5F85-35CF-4606-A801-6F70277E1D7A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IOptionDescription
		{
			// Token: 0x17001F3A RID: 7994
			// (get) Token: 0x06008EB4 RID: 36532
			string Id { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001F3B RID: 7995
			// (get) Token: 0x06008EB5 RID: 36533
			string Heading { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001F3C RID: 7996
			// (get) Token: 0x06008EB6 RID: 36534
			string Description { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001F3D RID: 7997
			// (get) Token: 0x06008EB7 RID: 36535
			RCW.IEnumString Labels { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }
		}

		// Token: 0x02000B96 RID: 2966
		[Guid("0B83A5B0-792F-4EAB-9799-ACF52C5ED08A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ISpellCheckerChangedEventHandler
		{
			// Token: 0x06008EB8 RID: 36536
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Invoke([MarshalAs(UnmanagedType.Interface)] [In] RCW.ISpellChecker sender);
		}

		// Token: 0x02000B97 RID: 2967
		[Guid("B6FD0B71-E2BC-4653-8D05-F197E412770B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ISpellChecker
		{
			// Token: 0x17001F3E RID: 7998
			// (get) Token: 0x06008EB9 RID: 36537
			string languageTag { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x06008EBA RID: 36538
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IEnumSpellingError Check([MarshalAs(UnmanagedType.LPWStr)] [In] string text);

			// Token: 0x06008EBB RID: 36539
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IEnumString Suggest([MarshalAs(UnmanagedType.LPWStr)] [In] string word);

			// Token: 0x06008EBC RID: 36540
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Add([MarshalAs(UnmanagedType.LPWStr)] [In] string word);

			// Token: 0x06008EBD RID: 36541
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Ignore([MarshalAs(UnmanagedType.LPWStr)] [In] string word);

			// Token: 0x06008EBE RID: 36542
			[MethodImpl(MethodImplOptions.InternalCall)]
			void AutoCorrect([MarshalAs(UnmanagedType.LPWStr)] [In] string from, [MarshalAs(UnmanagedType.LPWStr)] [In] string to);

			// Token: 0x06008EBF RID: 36543
			[MethodImpl(MethodImplOptions.InternalCall)]
			byte GetOptionValue([MarshalAs(UnmanagedType.LPWStr)] [In] string optionId);

			// Token: 0x17001F3F RID: 7999
			// (get) Token: 0x06008EC0 RID: 36544
			RCW.IEnumString OptionIds { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x17001F40 RID: 8000
			// (get) Token: 0x06008EC1 RID: 36545
			string Id { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001F41 RID: 8001
			// (get) Token: 0x06008EC2 RID: 36546
			string LocalizedName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x06008EC3 RID: 36547
			[MethodImpl(MethodImplOptions.InternalCall)]
			uint add_SpellCheckerChanged([MarshalAs(UnmanagedType.Interface)] [In] RCW.ISpellCheckerChangedEventHandler handler);

			// Token: 0x06008EC4 RID: 36548
			[MethodImpl(MethodImplOptions.InternalCall)]
			void remove_SpellCheckerChanged([In] uint eventCookie);

			// Token: 0x06008EC5 RID: 36549
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IOptionDescription GetOptionDescription([MarshalAs(UnmanagedType.LPWStr)] [In] string optionId);

			// Token: 0x06008EC6 RID: 36550
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IEnumSpellingError ComprehensiveCheck([MarshalAs(UnmanagedType.LPWStr)] [In] string text);
		}

		// Token: 0x02000B98 RID: 2968
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("8E018A9D-2415-4677-BF08-794EA61F94BB")]
		[ComImport]
		internal interface ISpellCheckerFactory
		{
			// Token: 0x17001F42 RID: 8002
			// (get) Token: 0x06008EC7 RID: 36551
			RCW.IEnumString SupportedLanguages { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x06008EC8 RID: 36552
			[MethodImpl(MethodImplOptions.InternalCall)]
			int IsSupported([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x06008EC9 RID: 36553
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.ISpellChecker CreateSpellChecker([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);
		}

		// Token: 0x02000B99 RID: 2969
		[Guid("AA176B85-0E12-4844-8E1A-EEF1DA77F586")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IUserDictionariesRegistrar
		{
			// Token: 0x06008ECA RID: 36554
			[MethodImpl(MethodImplOptions.InternalCall)]
			void RegisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x06008ECB RID: 36555
			[MethodImpl(MethodImplOptions.InternalCall)]
			void UnregisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);
		}

		// Token: 0x02000B9A RID: 2970
		[ClassInterface(ClassInterfaceType.None)]
		[TypeLibType(TypeLibTypeFlags.FCanCreate)]
		[Guid("7AB36653-1796-484B-BDFA-E74F1DB7C1DC")]
		[ComImport]
		internal class SpellCheckerFactoryCoClass : RCW.ISpellCheckerFactory, RCW.SpellCheckerFactoryClass, RCW.IUserDictionariesRegistrar
		{
			// Token: 0x06008ECC RID: 36556
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			public virtual extern RCW.ISpellChecker CreateSpellChecker([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x06008ECD RID: 36557
			[MethodImpl(MethodImplOptions.InternalCall)]
			public virtual extern int IsSupported([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x06008ECE RID: 36558
			[MethodImpl(MethodImplOptions.InternalCall)]
			public virtual extern void RegisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x06008ECF RID: 36559
			[MethodImpl(MethodImplOptions.InternalCall)]
			public virtual extern void UnregisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x17001F43 RID: 8003
			// (get) Token: 0x06008ED0 RID: 36560
			public virtual extern RCW.IEnumString SupportedLanguages { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x06008ED1 RID: 36561
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern SpellCheckerFactoryCoClass();
		}

		// Token: 0x02000B9B RID: 2971
		[CoClass(typeof(RCW.SpellCheckerFactoryCoClass))]
		[Guid("8E018A9D-2415-4677-BF08-794EA61F94BB")]
		[ComImport]
		internal interface SpellCheckerFactoryClass : RCW.ISpellCheckerFactory
		{
		}
	}
}
