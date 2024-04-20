using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000164 RID: 356
	internal class ManagedPropSpec
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0012EDF6 File Offset: 0x0012DDF6
		internal PropSpecType PropType
		{
			get
			{
				return this._propType;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0012EDFE File Offset: 0x0012DDFE
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0012EE06 File Offset: 0x0012DE06
		internal string PropName
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._name = value;
				this._id = 0U;
				this._propType = PropSpecType.Name;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0012EE2B File Offset: 0x0012DE2B
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0012EE33 File Offset: 0x0012DE33
		internal uint PropId
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				this._name = null;
				this._propType = PropSpecType.Id;
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0012EE4A File Offset: 0x0012DE4A
		internal ManagedPropSpec(uint id)
		{
			this.PropId = id;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0012EE5C File Offset: 0x0012DE5C
		internal ManagedPropSpec(PROPSPEC propSpec)
		{
			PropSpecType propType = (PropSpecType)propSpec.propType;
			if (propType == PropSpecType.Name)
			{
				this.PropName = Marshal.PtrToStringUni(propSpec.union.name);
				return;
			}
			if (propType == PropSpecType.Id)
			{
				this.PropId = propSpec.union.propId;
				return;
			}
			throw new ArgumentException(SR.Get("FilterPropSpecUnknownUnionSelector"), "propSpec");
		}

		// Token: 0x04000903 RID: 2307
		private PropSpecType _propType;

		// Token: 0x04000904 RID: 2308
		private uint _id;

		// Token: 0x04000905 RID: 2309
		private string _name;
	}
}
