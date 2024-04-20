using System;
using System.Runtime.InteropServices;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C6 RID: 1734
	internal class TextServicesPropertyRanges
	{
		// Token: 0x060059FF RID: 23039 RVA: 0x0027F0EA File Offset: 0x0027E0EA
		internal TextServicesPropertyRanges(TextStore textstore, Guid guid)
		{
			this._guid = guid;
			this._textstore = textstore;
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnRange(UnsafeNativeMethods.ITfProperty property, int ecReadonly, UnsafeNativeMethods.ITfRange range)
		{
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x0027F100 File Offset: 0x0027E100
		internal virtual void OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			UnsafeNativeMethods.ITfProperty tfProperty = null;
			UnsafeNativeMethods.IEnumTfRanges propertyUpdate = this.GetPropertyUpdate(editRecord);
			UnsafeNativeMethods.ITfRange[] array = new UnsafeNativeMethods.ITfRange[1];
			int num;
			while (propertyUpdate.Next(1, array, out num) == 0)
			{
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.ConvertToTextPosition(array[0], out textPointer, out textPointer2);
				if (tfProperty == null)
				{
					context.GetProperty(ref this._guid, out tfProperty);
				}
				UnsafeNativeMethods.IEnumTfRanges enumTfRanges;
				if (tfProperty.EnumRanges(ecReadOnly, out enumTfRanges, array[0]) == 0)
				{
					UnsafeNativeMethods.ITfRange[] array2 = new UnsafeNativeMethods.ITfRange[1];
					while (enumTfRanges.Next(1, array2, out num) == 0)
					{
						this.OnRange(tfProperty, ecReadOnly, array2[0]);
						Marshal.ReleaseComObject(array2[0]);
					}
					Marshal.ReleaseComObject(enumTfRanges);
				}
				Marshal.ReleaseComObject(array[0]);
			}
			Marshal.ReleaseComObject(propertyUpdate);
			if (tfProperty != null)
			{
				Marshal.ReleaseComObject(tfProperty);
			}
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x0027F1A8 File Offset: 0x0027E1A8
		protected void ConvertToTextPosition(UnsafeNativeMethods.ITfRange range, out ITextPointer start, out ITextPointer end)
		{
			int num;
			int num2;
			(range as UnsafeNativeMethods.ITfRangeACP).GetExtent(out num, out num2);
			if (num2 < 0)
			{
				start = null;
				end = null;
				return;
			}
			start = this._textstore.CreatePointerAtCharOffset(num, LogicalDirection.Forward);
			end = this._textstore.CreatePointerAtCharOffset(num + num2, LogicalDirection.Forward);
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x0027F1F0 File Offset: 0x0027E1F0
		protected static object GetValue(int ecReadOnly, UnsafeNativeMethods.ITfProperty property, UnsafeNativeMethods.ITfRange range)
		{
			if (property == null)
			{
				return null;
			}
			object result;
			property.GetValue(ecReadOnly, range, out result);
			return result;
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x0027F210 File Offset: 0x0027E210
		private unsafe UnsafeNativeMethods.IEnumTfRanges GetPropertyUpdate(UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			UnsafeNativeMethods.IEnumTfRanges result;
			fixed (Guid* ptr = &this._guid)
			{
				IntPtr intPtr = (IntPtr)((void*)ptr);
				editRecord.GetTextAndPropertyUpdates(0, ref intPtr, 1, out result);
			}
			return result;
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06005A05 RID: 23045 RVA: 0x0027F23C File Offset: 0x0027E23C
		protected Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x06005A06 RID: 23046 RVA: 0x0027F244 File Offset: 0x0027E244
		protected TextStore TextStore
		{
			get
			{
				return this._textstore;
			}
		}

		// Token: 0x0400302D RID: 12333
		private Guid _guid;

		// Token: 0x0400302E RID: 12334
		private TextStore _textstore;
	}
}
