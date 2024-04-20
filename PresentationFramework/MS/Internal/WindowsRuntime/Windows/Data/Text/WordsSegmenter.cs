using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using MS.Internal.PresentationFramework.Interop;
using MS.Internal.WindowsRuntime.ABI.Windows.Data.Text;
using MS.Internal.WindowsRuntime.Windows.Globalization;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x0200031C RID: 796
	[WindowsRuntimeType]
	[ProjectedRuntimeClass("_default")]
	internal sealed class WordsSegmenter : ICustomQueryInterface, IEquatable<WordsSegmenter>
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x0016CFA0 File Offset: 0x0016BFA0
		public IntPtr ThisPtr
		{
			get
			{
				return this._default.ThisPtr;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001D76 RID: 7542 RVA: 0x0016CFAD File Offset: 0x0016BFAD
		private IWordsSegmenter _default
		{
			get
			{
				return this._defaultLazy.Value;
			}
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x0016CFBC File Offset: 0x0016BFBC
		public WordsSegmenter(string language) : this(delegate
		{
			IntPtr intPtr = WordsSegmenter._IWordsSegmenterFactory.Instance.CreateWithLanguage(language);
			IWordsSegmenter result;
			try
			{
				result = new IWordsSegmenter(ComWrappersSupport.GetObjectReferenceForInterface(intPtr));
			}
			finally
			{
				MarshalInspectable.DisposeAbi(intPtr);
			}
			return result;
		}())
		{
			ComWrappersSupport.RegisterObjectForInterface(this, this.ThisPtr);
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0016CFFC File Offset: 0x0016BFFC
		public static WordsSegmenter FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			object obj = MarshalInspectable.FromAbi(thisPtr);
			if (!(obj is WordsSegmenter))
			{
				return new WordsSegmenter((IWordsSegmenter)obj);
			}
			return (WordsSegmenter)obj;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0016D03C File Offset: 0x0016C03C
		public WordsSegmenter(IWordsSegmenter ifc)
		{
			this._defaultLazy = new Lazy<IWordsSegmenter>(() => ifc);
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0016D073 File Offset: 0x0016C073
		public static bool operator ==(WordsSegmenter x, WordsSegmenter y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0016D09A File Offset: 0x0016C09A
		public static bool operator !=(WordsSegmenter x, WordsSegmenter y)
		{
			return !(x == y);
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0016D0A6 File Offset: 0x0016C0A6
		public bool Equals(WordsSegmenter other)
		{
			return this == other;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0016D0B0 File Offset: 0x0016C0B0
		public override bool Equals(object obj)
		{
			WordsSegmenter wordsSegmenter = obj as WordsSegmenter;
			return wordsSegmenter != null && this == wordsSegmenter;
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0016D0D0 File Offset: 0x0016C0D0
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0016D0EB File Offset: 0x0016C0EB
		private IObjectReference GetDefaultReference<T>()
		{
			return this._default.AsInterface<T>();
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0016D0F8 File Offset: 0x0016C0F8
		private IObjectReference GetReferenceForQI()
		{
			return this._inner ?? this._default.ObjRef;
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0016D10F File Offset: 0x0016C10F
		private IWordsSegmenter AsInternal(WordsSegmenter.InterfaceTag<IWordsSegmenter> _)
		{
			return this._default;
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x0016D117 File Offset: 0x0016C117
		public WordSegment GetTokenAt(string text, uint startIndex)
		{
			return this._default.GetTokenAt(text, startIndex);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x0016D126 File Offset: 0x0016C126
		public IReadOnlyList<WordSegment> GetTokens(string text)
		{
			return this._default.GetTokens(text);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0016D134 File Offset: 0x0016C134
		public void Tokenize(string text, uint startIndex, WordSegmentsTokenizingHandler handler)
		{
			this._default.Tokenize(text, startIndex, handler);
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x0016D144 File Offset: 0x0016C144
		public string ResolvedLanguage
		{
			get
			{
				return this._default.ResolvedLanguage;
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00105F35 File Offset: 0x00104F35
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0016D154 File Offset: 0x0016C154
		CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			if (this.IsOverridableInterface(iid) || typeof(IInspectable).GUID == iid)
			{
				return CustomQueryInterfaceResult.NotHandled;
			}
			ObjectReference<IUnknownVftbl> objectReference;
			if (this.GetReferenceForQI().TryAs<IUnknownVftbl>(iid, out objectReference) >= 0)
			{
				using (objectReference)
				{
					ppv = objectReference.GetRef();
					return CustomQueryInterfaceResult.Handled;
				}
				return CustomQueryInterfaceResult.NotHandled;
			}
			return CustomQueryInterfaceResult.NotHandled;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x0016D1D8 File Offset: 0x0016C1D8
		public static WordsSegmenter Create(string language, bool shouldPreferNeutralSegmenter = false)
		{
			if (!OSVersionHelper.IsOsWindows8Point1OrGreater)
			{
				throw new PlatformNotSupportedException();
			}
			if (shouldPreferNeutralSegmenter && !WordsSegmenter.ShouldUseDedicatedSegmenter(language))
			{
				language = WordsSegmenter.Undetermined;
			}
			return new WordsSegmenter(language);
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x0016D200 File Offset: 0x0016C200
		private static bool ShouldUseDedicatedSegmenter(string languageTag)
		{
			bool result = true;
			try
			{
				Language language = new Language(languageTag);
				string script = language.Script;
				if (WordsSegmenter.ScriptCodesRequiringDedicatedSegmenter.FindIndex((string s) => s.Equals(script, StringComparison.InvariantCultureIgnoreCase)) == -1)
				{
					result = false;
				}
			}
			catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentException || ex is TargetInvocationException || ex is MissingMemberException)
			{
			}
			return result;
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0016D290 File Offset: 0x0016C290
		private static List<string> ScriptCodesRequiringDedicatedSegmenter { get; } = new List<string>
		{
			"Bopo",
			"Brah",
			"Egyp",
			"Goth",
			"Hang",
			"Hani",
			"Ital",
			"Java",
			"Kana",
			"Khar",
			"Laoo",
			"Lisu",
			"Mymr",
			"Talu",
			"Thai",
			"Tibt",
			"Xsux",
			"Yiii"
		};

		// Token: 0x04000EB9 RID: 3769
		private IObjectReference _inner;

		// Token: 0x04000EBA RID: 3770
		private readonly Lazy<IWordsSegmenter> _defaultLazy;

		// Token: 0x04000EBC RID: 3772
		public static readonly string Undetermined = "und";

		// Token: 0x02000A68 RID: 2664
		internal class _IWordsSegmenterFactory : IWordsSegmenterFactory
		{
			// Token: 0x06008618 RID: 34328 RVA: 0x0032A120 File Offset: 0x00329120
			public _IWordsSegmenterFactory() : base(ActivationFactory<WordsSegmenter>.As<IWordsSegmenterFactory.Vftbl>())
			{
			}

			// Token: 0x17001DF7 RID: 7671
			// (get) Token: 0x06008619 RID: 34329 RVA: 0x0032A12D File Offset: 0x0032912D
			public static WordsSegmenter._IWordsSegmenterFactory Instance
			{
				get
				{
					return WordsSegmenter._IWordsSegmenterFactory._instance.Value;
				}
			}

			// Token: 0x0600861A RID: 34330 RVA: 0x0032A13C File Offset: 0x0032913C
			public new IntPtr CreateWithLanguage(string language)
			{
				MarshalString m = null;
				IntPtr intPtr = 0;
				IntPtr result;
				try
				{
					m = MarshalString.CreateMarshaler(language);
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.CreateWithLanguage_0(base.ThisPtr, MarshalString.GetAbi(m), out intPtr));
					result = intPtr;
				}
				finally
				{
					MarshalString.DisposeMarshaler(m);
				}
				return result;
			}

			// Token: 0x04004138 RID: 16696
			private static WeakLazy<WordsSegmenter._IWordsSegmenterFactory> _instance = new WeakLazy<WordsSegmenter._IWordsSegmenterFactory>();
		}

		// Token: 0x02000A69 RID: 2665
		private struct InterfaceTag<I>
		{
		}
	}
}
