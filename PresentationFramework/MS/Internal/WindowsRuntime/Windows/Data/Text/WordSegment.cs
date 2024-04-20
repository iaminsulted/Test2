using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.Windows.Data.Text;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x0200031A RID: 794
	[WindowsRuntimeType]
	[ProjectedRuntimeClass("_default")]
	internal sealed class WordSegment : ICustomQueryInterface, IEquatable<WordSegment>
	{
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001D60 RID: 7520 RVA: 0x0016CDBB File Offset: 0x0016BDBB
		public IntPtr ThisPtr
		{
			get
			{
				return this._default.ThisPtr;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0016CDC8 File Offset: 0x0016BDC8
		private IWordSegment _default
		{
			get
			{
				return this._defaultLazy.Value;
			}
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x0016CDD8 File Offset: 0x0016BDD8
		public static WordSegment FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			object obj = MarshalInspectable.FromAbi(thisPtr);
			if (!(obj is WordSegment))
			{
				return new WordSegment((IWordSegment)obj);
			}
			return (WordSegment)obj;
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0016CE18 File Offset: 0x0016BE18
		public WordSegment(IWordSegment ifc)
		{
			this._defaultLazy = new Lazy<IWordSegment>(() => ifc);
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0016CE4F File Offset: 0x0016BE4F
		public static bool operator ==(WordSegment x, WordSegment y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0016CE76 File Offset: 0x0016BE76
		public static bool operator !=(WordSegment x, WordSegment y)
		{
			return !(x == y);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x0016CE82 File Offset: 0x0016BE82
		public bool Equals(WordSegment other)
		{
			return this == other;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0016CE8C File Offset: 0x0016BE8C
		public override bool Equals(object obj)
		{
			WordSegment wordSegment = obj as WordSegment;
			return wordSegment != null && this == wordSegment;
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0016CEAC File Offset: 0x0016BEAC
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0016CEC7 File Offset: 0x0016BEC7
		private IObjectReference GetDefaultReference<T>()
		{
			return this._default.AsInterface<T>();
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x0016CED4 File Offset: 0x0016BED4
		private IObjectReference GetReferenceForQI()
		{
			return this._inner ?? this._default.ObjRef;
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x0016CEEB File Offset: 0x0016BEEB
		private IWordSegment AsInternal(WordSegment.InterfaceTag<IWordSegment> _)
		{
			return this._default;
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x0016CEF3 File Offset: 0x0016BEF3
		public IReadOnlyList<AlternateWordForm> AlternateForms
		{
			get
			{
				return this._default.AlternateForms;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0016CF00 File Offset: 0x0016BF00
		public TextSegment SourceTextSegment
		{
			get
			{
				return this._default.SourceTextSegment;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x0016CF0D File Offset: 0x0016BF0D
		public string Text
		{
			get
			{
				return this._default.Text;
			}
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x00105F35 File Offset: 0x00104F35
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x0016CF1C File Offset: 0x0016BF1C
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

		// Token: 0x04000EB7 RID: 3767
		private IObjectReference _inner;

		// Token: 0x04000EB8 RID: 3768
		private readonly Lazy<IWordSegment> _defaultLazy;

		// Token: 0x02000A66 RID: 2662
		private struct InterfaceTag<I>
		{
		}
	}
}
