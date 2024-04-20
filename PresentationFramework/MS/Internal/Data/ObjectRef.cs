using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000231 RID: 561
	internal abstract class ObjectRef
	{
		// Token: 0x06001568 RID: 5480 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			return null;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00154EC9 File Offset: 0x00153EC9
		internal virtual object GetDataObject(DependencyObject d, ObjectRefArgs args)
		{
			return this.GetObject(d, args);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00154ED3 File Offset: 0x00153ED3
		internal bool TreeContextIsRequired(DependencyObject target)
		{
			return this.ProtectedTreeContextIsRequired(target);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool ProtectedTreeContextIsRequired(DependencyObject target)
		{
			return false;
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x00154EDC File Offset: 0x00153EDC
		internal bool UsesMentor
		{
			get
			{
				return this.ProtectedUsesMentor;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600156D RID: 5485 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected virtual bool ProtectedUsesMentor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600156E RID: 5486
		internal abstract string Identify();
	}
}
