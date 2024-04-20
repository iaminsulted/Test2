using System;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.Windows.Data.Text;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x02000310 RID: 784
	[ProjectedRuntimeClass("_default")]
	[WindowsRuntimeType]
	internal sealed class AlternateWordForm : ICustomQueryInterface, IEquatable<AlternateWordForm>
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x0016CA68 File Offset: 0x0016BA68
		public IntPtr ThisPtr
		{
			get
			{
				return this._default.ThisPtr;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x0016CA75 File Offset: 0x0016BA75
		private IAlternateWordForm _default
		{
			get
			{
				return this._defaultLazy.Value;
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0016CA84 File Offset: 0x0016BA84
		public static AlternateWordForm FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			object obj = MarshalInspectable.FromAbi(thisPtr);
			if (!(obj is AlternateWordForm))
			{
				return new AlternateWordForm((IAlternateWordForm)obj);
			}
			return (AlternateWordForm)obj;
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0016CAC4 File Offset: 0x0016BAC4
		public AlternateWordForm(IAlternateWordForm ifc)
		{
			this._defaultLazy = new Lazy<IAlternateWordForm>(() => ifc);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0016CAFB File Offset: 0x0016BAFB
		public static bool operator ==(AlternateWordForm x, AlternateWordForm y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x0016CB22 File Offset: 0x0016BB22
		public static bool operator !=(AlternateWordForm x, AlternateWordForm y)
		{
			return !(x == y);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x0016CB2E File Offset: 0x0016BB2E
		public bool Equals(AlternateWordForm other)
		{
			return this == other;
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x0016CB38 File Offset: 0x0016BB38
		public override bool Equals(object obj)
		{
			AlternateWordForm alternateWordForm = obj as AlternateWordForm;
			return alternateWordForm != null && this == alternateWordForm;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x0016CB58 File Offset: 0x0016BB58
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0016CB73 File Offset: 0x0016BB73
		private IObjectReference GetDefaultReference<T>()
		{
			return this._default.AsInterface<T>();
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x0016CB80 File Offset: 0x0016BB80
		private IObjectReference GetReferenceForQI()
		{
			return this._inner ?? this._default.ObjRef;
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0016CB97 File Offset: 0x0016BB97
		private IAlternateWordForm AsInternal(AlternateWordForm.InterfaceTag<IAlternateWordForm> _)
		{
			return this._default;
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001D28 RID: 7464 RVA: 0x0016CB9F File Offset: 0x0016BB9F
		public string AlternateText
		{
			get
			{
				return this._default.AlternateText;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x0016CBAC File Offset: 0x0016BBAC
		public AlternateNormalizationFormat NormalizationFormat
		{
			get
			{
				return this._default.NormalizationFormat;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x0016CBB9 File Offset: 0x0016BBB9
		public TextSegment SourceTextSegment
		{
			get
			{
				return this._default.SourceTextSegment;
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00105F35 File Offset: 0x00104F35
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x0016CBC8 File Offset: 0x0016BBC8
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

		// Token: 0x04000E8F RID: 3727
		private IObjectReference _inner;

		// Token: 0x04000E90 RID: 3728
		private readonly Lazy<IAlternateWordForm> _defaultLazy;

		// Token: 0x02000A63 RID: 2659
		private struct InterfaceTag<I>
		{
		}
	}
}
