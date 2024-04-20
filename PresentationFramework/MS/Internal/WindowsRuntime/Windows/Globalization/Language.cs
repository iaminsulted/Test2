using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.Windows.Globalization;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x0200030D RID: 781
	[ProjectedRuntimeClass("_default")]
	[WindowsRuntimeType]
	internal sealed class Language : ICustomQueryInterface, IEquatable<Language>
	{
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0016C7B4 File Offset: 0x0016B7B4
		public IntPtr ThisPtr
		{
			get
			{
				return this._default.ThisPtr;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x0016C7C1 File Offset: 0x0016B7C1
		private ILanguage _default
		{
			get
			{
				return this._defaultLazy.Value;
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x0016C7D0 File Offset: 0x0016B7D0
		public Language(string languageTag) : this(delegate
		{
			IntPtr intPtr = Language._ILanguageFactory.Instance.CreateLanguage(languageTag);
			ILanguage result;
			try
			{
				result = new ILanguage(ComWrappersSupport.GetObjectReferenceForInterface(intPtr));
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

		// Token: 0x06001D05 RID: 7429 RVA: 0x0016C80D File Offset: 0x0016B80D
		public static bool IsWellFormed(string languageTag)
		{
			return Language._ILanguageStatics.Instance.IsWellFormed(languageTag);
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x0016C81A File Offset: 0x0016B81A
		public static string CurrentInputMethodLanguageTag
		{
			get
			{
				return Language._ILanguageStatics.Instance.CurrentInputMethodLanguageTag;
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x0016C826 File Offset: 0x0016B826
		public static bool TrySetInputMethodLanguageTag(string languageTag)
		{
			return Language._ILanguageStatics2.Instance.TrySetInputMethodLanguageTag(languageTag);
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x0016C834 File Offset: 0x0016B834
		public static Language FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			object obj = MarshalInspectable.FromAbi(thisPtr);
			if (!(obj is Language))
			{
				return new Language((ILanguage)obj);
			}
			return (Language)obj;
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x0016C874 File Offset: 0x0016B874
		public Language(ILanguage ifc)
		{
			this._defaultLazy = new Lazy<ILanguage>(() => ifc);
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x0016C8AB File Offset: 0x0016B8AB
		public static bool operator ==(Language x, Language y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x0016C8D2 File Offset: 0x0016B8D2
		public static bool operator !=(Language x, Language y)
		{
			return !(x == y);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x0016C8DE File Offset: 0x0016B8DE
		public bool Equals(Language other)
		{
			return this == other;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x0016C8E8 File Offset: 0x0016B8E8
		public override bool Equals(object obj)
		{
			Language language = obj as Language;
			return language != null && this == language;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x0016C908 File Offset: 0x0016B908
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x0016C923 File Offset: 0x0016B923
		private IObjectReference GetDefaultReference<T>()
		{
			return this._default.AsInterface<T>();
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x0016C930 File Offset: 0x0016B930
		private IObjectReference GetReferenceForQI()
		{
			return this._inner ?? this._default.ObjRef;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x0016C947 File Offset: 0x0016B947
		private ILanguage AsInternal(Language.InterfaceTag<ILanguage> _)
		{
			return this._default;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x0016C94F File Offset: 0x0016B94F
		private ILanguageExtensionSubtags AsInternal(Language.InterfaceTag<ILanguageExtensionSubtags> _)
		{
			return new ILanguageExtensionSubtags(this.GetReferenceForQI());
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x0016C95C File Offset: 0x0016B95C
		public IReadOnlyList<string> GetExtensionSubtags(string singleton)
		{
			return this.AsInternal(default(Language.InterfaceTag<ILanguageExtensionSubtags>)).GetExtensionSubtags(singleton);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x0016C97E File Offset: 0x0016B97E
		private ILanguage2 AsInternal(Language.InterfaceTag<ILanguage2> _)
		{
			return new ILanguage2(this.GetReferenceForQI());
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x0016C98B File Offset: 0x0016B98B
		public string DisplayName
		{
			get
			{
				return this._default.DisplayName;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x0016C998 File Offset: 0x0016B998
		public string LanguageTag
		{
			get
			{
				return this._default.LanguageTag;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0016C9A8 File Offset: 0x0016B9A8
		public LanguageLayoutDirection LayoutDirection
		{
			get
			{
				return this.AsInternal(default(Language.InterfaceTag<ILanguage2>)).LayoutDirection;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x0016C9C9 File Offset: 0x0016B9C9
		public string NativeName
		{
			get
			{
				return this._default.NativeName;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0016C9D6 File Offset: 0x0016B9D6
		public string Script
		{
			get
			{
				return this._default.Script;
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00105F35 File Offset: 0x00104F35
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x0016C9E4 File Offset: 0x0016B9E4
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

		// Token: 0x04000E82 RID: 3714
		private IObjectReference _inner;

		// Token: 0x04000E83 RID: 3715
		private readonly Lazy<ILanguage> _defaultLazy;

		// Token: 0x02000A5D RID: 2653
		internal class _ILanguageFactory : ILanguageFactory
		{
			// Token: 0x06008603 RID: 34307 RVA: 0x00329F9E File Offset: 0x00328F9E
			public _ILanguageFactory() : base(ActivationFactory<Language>.As<ILanguageFactory.Vftbl>())
			{
			}

			// Token: 0x17001DF3 RID: 7667
			// (get) Token: 0x06008604 RID: 34308 RVA: 0x00329FAB File Offset: 0x00328FAB
			public static Language._ILanguageFactory Instance
			{
				get
				{
					return Language._ILanguageFactory._instance.Value;
				}
			}

			// Token: 0x06008605 RID: 34309 RVA: 0x00329FB8 File Offset: 0x00328FB8
			public new IntPtr CreateLanguage(string languageTag)
			{
				MarshalString m = null;
				IntPtr intPtr = 0;
				IntPtr result;
				try
				{
					m = MarshalString.CreateMarshaler(languageTag);
					ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.CreateLanguage_0(base.ThisPtr, MarshalString.GetAbi(m), out intPtr));
					result = intPtr;
				}
				finally
				{
					MarshalString.DisposeMarshaler(m);
				}
				return result;
			}

			// Token: 0x04004130 RID: 16688
			private static WeakLazy<Language._ILanguageFactory> _instance = new WeakLazy<Language._ILanguageFactory>();
		}

		// Token: 0x02000A5E RID: 2654
		internal class _ILanguageStatics : ILanguageStatics
		{
			// Token: 0x06008607 RID: 34311 RVA: 0x0032A028 File Offset: 0x00329028
			public _ILanguageStatics() : base(new BaseActivationFactory("Windows.Globalization", "Windows.Globalization.Language")._As<ILanguageStatics.Vftbl>())
			{
			}

			// Token: 0x17001DF4 RID: 7668
			// (get) Token: 0x06008608 RID: 34312 RVA: 0x0032A044 File Offset: 0x00329044
			public static ILanguageStatics Instance
			{
				get
				{
					return Language._ILanguageStatics._instance.Value;
				}
			}

			// Token: 0x04004131 RID: 16689
			private static WeakLazy<Language._ILanguageStatics> _instance = new WeakLazy<Language._ILanguageStatics>();
		}

		// Token: 0x02000A5F RID: 2655
		internal class _ILanguageStatics2 : ILanguageStatics2
		{
			// Token: 0x0600860A RID: 34314 RVA: 0x0032A05C File Offset: 0x0032905C
			public _ILanguageStatics2() : base(new BaseActivationFactory("Windows.Globalization", "Windows.Globalization.Language")._As<ILanguageStatics2.Vftbl>())
			{
			}

			// Token: 0x17001DF5 RID: 7669
			// (get) Token: 0x0600860B RID: 34315 RVA: 0x0032A078 File Offset: 0x00329078
			public static ILanguageStatics2 Instance
			{
				get
				{
					return Language._ILanguageStatics2._instance.Value;
				}
			}

			// Token: 0x04004132 RID: 16690
			private static WeakLazy<Language._ILanguageStatics2> _instance = new WeakLazy<Language._ILanguageStatics2>();
		}

		// Token: 0x02000A60 RID: 2656
		private struct InterfaceTag<I>
		{
		}
	}
}
