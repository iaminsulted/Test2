using System;
using System.Collections;
using System.Runtime.InteropServices;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C3 RID: 1731
	internal class TextServicesDisplayAttributePropertyRanges : TextServicesPropertyRanges
	{
		// Token: 0x060059E9 RID: 23017 RVA: 0x0027E9DA File Offset: 0x0027D9DA
		internal TextServicesDisplayAttributePropertyRanges(TextStore textstore) : base(textstore, UnsafeNativeMethods.GUID_PROP_ATTRIBUTE)
		{
		}

		// Token: 0x060059EA RID: 23018 RVA: 0x0027E9E8 File Offset: 0x0027D9E8
		internal override void OnRange(UnsafeNativeMethods.ITfProperty property, int ecReadOnly, UnsafeNativeMethods.ITfRange range)
		{
			int int32Value = this.GetInt32Value(ecReadOnly, property, range);
			if (int32Value != 0)
			{
				TextServicesDisplayAttribute displayAttribute = TextServicesDisplayAttributePropertyRanges.GetDisplayAttribute(int32Value);
				if (displayAttribute != null)
				{
					ITextPointer start;
					ITextPointer end;
					base.ConvertToTextPosition(range, out start, out end);
					displayAttribute.Apply(start, end);
				}
			}
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x0027EA20 File Offset: 0x0027DA20
		internal override void OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			if (this._compositionAdorner != null)
			{
				this._compositionAdorner.Uninitialize();
				this._compositionAdorner = null;
			}
			Guid guid = base.Guid;
			UnsafeNativeMethods.ITfProperty tfProperty;
			context.GetProperty(ref guid, out tfProperty);
			UnsafeNativeMethods.IEnumTfRanges enumTfRanges;
			if (tfProperty.EnumRanges(ecReadOnly, out enumTfRanges, null) == 0)
			{
				UnsafeNativeMethods.ITfRange[] array = new UnsafeNativeMethods.ITfRange[1];
				int num;
				while (enumTfRanges.Next(1, array, out num) == 0)
				{
					TextServicesDisplayAttribute displayAttribute = TextServicesDisplayAttributePropertyRanges.GetDisplayAttribute(this.GetInt32Value(ecReadOnly, tfProperty, array[0]));
					if (displayAttribute != null && !displayAttribute.IsEmptyAttribute())
					{
						ITextPointer textPointer;
						ITextPointer end;
						base.ConvertToTextPosition(array[0], out textPointer, out end);
						if (textPointer != null)
						{
							if (this._compositionAdorner == null)
							{
								this._compositionAdorner = new CompositionAdorner(base.TextStore.TextView);
								this._compositionAdorner.Initialize(base.TextStore.TextView);
							}
							this._compositionAdorner.AddAttributeRange(textPointer, end, displayAttribute);
						}
					}
					Marshal.ReleaseComObject(array[0]);
				}
				if (this._compositionAdorner != null)
				{
					base.TextStore.RenderScope.UpdateLayout();
					this._compositionAdorner.InvalidateAdorner();
				}
				Marshal.ReleaseComObject(enumTfRanges);
			}
			Marshal.ReleaseComObject(tfProperty);
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x0027EB2E File Offset: 0x0027DB2E
		internal void OnLayoutUpdated()
		{
			if (this._compositionAdorner != null)
			{
				this._compositionAdorner.InvalidateAdorner();
			}
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x0027EB44 File Offset: 0x0027DB44
		private static TextServicesDisplayAttribute GetDisplayAttribute(int guidatom)
		{
			if (TextServicesDisplayAttributePropertyRanges._attributes == null)
			{
				TextServicesDisplayAttributePropertyRanges._attributes = new Hashtable();
			}
			TextServicesDisplayAttribute textServicesDisplayAttribute = (TextServicesDisplayAttribute)TextServicesDisplayAttributePropertyRanges._attributes[guidatom];
			if (textServicesDisplayAttribute != null)
			{
				return textServicesDisplayAttribute;
			}
			UnsafeNativeMethods.ITfCategoryMgr tfCategoryMgr;
			if (UnsafeNativeMethods.TF_CreateCategoryMgr(out tfCategoryMgr) != 0)
			{
				return null;
			}
			if (tfCategoryMgr == null)
			{
				return null;
			}
			Guid guid;
			tfCategoryMgr.GetGUID(guidatom, out guid);
			Marshal.ReleaseComObject(tfCategoryMgr);
			if (guid.Equals(UnsafeNativeMethods.Guid_Null))
			{
				return null;
			}
			UnsafeNativeMethods.ITfDisplayAttributeMgr tfDisplayAttributeMgr;
			if (UnsafeNativeMethods.TF_CreateDisplayAttributeMgr(out tfDisplayAttributeMgr) != 0)
			{
				return null;
			}
			if (tfDisplayAttributeMgr == null)
			{
				return null;
			}
			UnsafeNativeMethods.ITfDisplayAttributeInfo tfDisplayAttributeInfo;
			Guid guid2;
			tfDisplayAttributeMgr.GetDisplayAttributeInfo(ref guid, out tfDisplayAttributeInfo, out guid2);
			if (tfDisplayAttributeInfo != null)
			{
				UnsafeNativeMethods.TF_DISPLAYATTRIBUTE attr;
				tfDisplayAttributeInfo.GetAttributeInfo(out attr);
				textServicesDisplayAttribute = new TextServicesDisplayAttribute(attr);
				Marshal.ReleaseComObject(tfDisplayAttributeInfo);
				TextServicesDisplayAttributePropertyRanges._attributes[guidatom] = textServicesDisplayAttribute;
			}
			Marshal.ReleaseComObject(tfDisplayAttributeMgr);
			return textServicesDisplayAttribute;
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x0027EC04 File Offset: 0x0027DC04
		private int GetInt32Value(int ecReadOnly, UnsafeNativeMethods.ITfProperty property, UnsafeNativeMethods.ITfRange range)
		{
			object value = TextServicesPropertyRanges.GetValue(ecReadOnly, property, range);
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}

		// Token: 0x04003023 RID: 12323
		private static Hashtable _attributes;

		// Token: 0x04003024 RID: 12324
		private CompositionAdorner _compositionAdorner;
	}
}
