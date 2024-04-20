using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000117 RID: 279
	internal class AttachedObject : EmbeddedObject
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x0010CBF5 File Offset: 0x0010BBF5
		internal AttachedObject(int dcp, BaseParagraph para) : base(dcp)
		{
			this.Para = para;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0010CC05 File Offset: 0x0010BC05
		internal override void Dispose()
		{
			this.Para.Dispose();
			this.Para = null;
			base.Dispose();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0010CC20 File Offset: 0x0010BC20
		internal override void Update(EmbeddedObject newObject)
		{
			AttachedObject attachedObject = newObject as AttachedObject;
			ErrorHandler.Assert(attachedObject != null, ErrorHandler.EmbeddedObjectTypeMismatch);
			ErrorHandler.Assert(attachedObject.Element.Equals(this.Element), ErrorHandler.EmbeddedObjectOwnerMismatch);
			this.Dcp = attachedObject.Dcp;
			this.Para.SetUpdateInfo(PTS.FSKCHANGE.fskchInside, false);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0010CC76 File Offset: 0x0010BC76
		internal override DependencyObject Element
		{
			get
			{
				return this.Para.Element;
			}
		}

		// Token: 0x04000747 RID: 1863
		internal BaseParagraph Para;
	}
}
