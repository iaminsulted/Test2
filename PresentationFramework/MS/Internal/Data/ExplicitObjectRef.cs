using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000234 RID: 564
	internal sealed class ExplicitObjectRef : ObjectRef
	{
		// Token: 0x0600157F RID: 5503 RVA: 0x001555A6 File Offset: 0x001545A6
		internal ExplicitObjectRef(object o)
		{
			if (o is DependencyObject)
			{
				this._element = new WeakReference(o);
				return;
			}
			this._object = o;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x001555CA File Offset: 0x001545CA
		internal override object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			if (this._element == null)
			{
				return this._object;
			}
			return this._element.Target;
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool ProtectedUsesMentor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x001555E6 File Offset: 0x001545E6
		internal override string Identify()
		{
			return "Source";
		}

		// Token: 0x04000C14 RID: 3092
		private object _object;

		// Token: 0x04000C15 RID: 3093
		private WeakReference _element;
	}
}
