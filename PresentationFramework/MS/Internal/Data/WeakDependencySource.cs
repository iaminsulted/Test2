using System;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200020A RID: 522
	internal class WeakDependencySource
	{
		// Token: 0x0600133E RID: 4926 RVA: 0x0014CDA1 File Offset: 0x0014BDA1
		internal WeakDependencySource(DependencyObject item, DependencyProperty dp)
		{
			this._item = BindingExpressionBase.CreateReference(item);
			this._dp = dp;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0014CDBC File Offset: 0x0014BDBC
		internal WeakDependencySource(WeakReference wr, DependencyProperty dp)
		{
			this._item = wr;
			this._dp = dp;
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x0014CDD2 File Offset: 0x0014BDD2
		internal DependencyObject DependencyObject
		{
			get
			{
				return (DependencyObject)BindingExpressionBase.GetReference(this._item);
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x0014CDE4 File Offset: 0x0014BDE4
		internal DependencyProperty DependencyProperty
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x04000B74 RID: 2932
		private object _item;

		// Token: 0x04000B75 RID: 2933
		private DependencyProperty _dp;
	}
}
