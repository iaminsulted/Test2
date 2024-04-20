using System;
using System.IO.Packaging;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200015E RID: 350
	internal class CorePropertyEnumerator
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x0012CE4C File Offset: 0x0012BE4C
		internal CorePropertyEnumerator(PackageProperties coreProperties, IFILTER_INIT grfFlags, ManagedFullPropSpec[] attributes)
		{
			if (attributes != null && attributes.Length != 0)
			{
				this._attributes = attributes;
			}
			else if ((grfFlags & IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES) == IFILTER_INIT.IFILTER_INIT_APPLY_INDEX_ATTRIBUTES)
			{
				this._attributes = new ManagedFullPropSpec[]
				{
					new ManagedFullPropSpec(FormatId.SummaryInformation, 2U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 3U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 4U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 5U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 6U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 8U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 9U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 11U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 12U),
					new ManagedFullPropSpec(FormatId.SummaryInformation, 13U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 2U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 18U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 26U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 27U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 28U),
					new ManagedFullPropSpec(FormatId.DocumentSummaryInformation, 29U)
				};
			}
			this._coreProperties = coreProperties;
			this._currentIndex = -1;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0012CF88 File Offset: 0x0012BF88
		internal bool MoveNext()
		{
			if (this._attributes == null)
			{
				return false;
			}
			this._currentIndex++;
			while (this._currentIndex < this._attributes.Length)
			{
				if (this._attributes[this._currentIndex].Property.PropType == PropSpecType.Id && this.CurrentValue != null)
				{
					return true;
				}
				this._currentIndex++;
			}
			return false;
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0012CFF2 File Offset: 0x0012BFF2
		internal Guid CurrentGuid
		{
			get
			{
				this.ValidateCurrent();
				return this._attributes[this._currentIndex].Guid;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0012D00C File Offset: 0x0012C00C
		internal uint CurrentPropId
		{
			get
			{
				this.ValidateCurrent();
				return this._attributes[this._currentIndex].Property.PropId;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0012D02B File Offset: 0x0012C02B
		internal object CurrentValue
		{
			get
			{
				this.ValidateCurrent();
				return this.GetValue(this.CurrentGuid, this.CurrentPropId);
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0012D045 File Offset: 0x0012C045
		private void ValidateCurrent()
		{
			if (this._currentIndex < 0 || this._currentIndex >= this._attributes.Length)
			{
				throw new InvalidOperationException(SR.Get("CorePropertyEnumeratorPositionedOutOfBounds"));
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0012D070 File Offset: 0x0012C070
		private object GetValue(Guid guid, uint propId)
		{
			if (guid == FormatId.SummaryInformation)
			{
				switch (propId)
				{
				case 2U:
					return this._coreProperties.Title;
				case 3U:
					return this._coreProperties.Subject;
				case 4U:
					return this._coreProperties.Creator;
				case 5U:
					return this._coreProperties.Keywords;
				case 6U:
					return this._coreProperties.Description;
				case 8U:
					return this._coreProperties.LastModifiedBy;
				case 9U:
					return this._coreProperties.Revision;
				case 11U:
					if (this._coreProperties.LastPrinted != null)
					{
						return this._coreProperties.LastPrinted.Value;
					}
					return null;
				case 12U:
					if (this._coreProperties.Created != null)
					{
						return this._coreProperties.Created.Value;
					}
					return null;
				case 13U:
					if (this._coreProperties.Modified != null)
					{
						return this._coreProperties.Modified.Value;
					}
					return null;
				}
			}
			else if (guid == FormatId.DocumentSummaryInformation)
			{
				if (propId == 2U)
				{
					return this._coreProperties.Category;
				}
				if (propId == 18U)
				{
					return this._coreProperties.Identifier;
				}
				switch (propId)
				{
				case 26U:
					return this._coreProperties.ContentType;
				case 27U:
					return this._coreProperties.Language;
				case 28U:
					return this._coreProperties.Version;
				case 29U:
					return this._coreProperties.ContentStatus;
				}
			}
			return null;
		}

		// Token: 0x040008DC RID: 2268
		private PackageProperties _coreProperties;

		// Token: 0x040008DD RID: 2269
		private ManagedFullPropSpec[] _attributes;

		// Token: 0x040008DE RID: 2270
		private int _currentIndex;
	}
}
