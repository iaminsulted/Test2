using System;
using System.Diagnostics;

namespace System.Windows.Controls
{
	// Token: 0x02000733 RID: 1843
	internal class ContainerTracking<T>
	{
		// Token: 0x0600612A RID: 24874 RVA: 0x0029C479 File Offset: 0x0029B479
		internal ContainerTracking(T container)
		{
			this._container = container;
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x0600612B RID: 24875 RVA: 0x0029C488 File Offset: 0x0029B488
		internal T Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x0600612C RID: 24876 RVA: 0x0029C490 File Offset: 0x0029B490
		internal ContainerTracking<T> Next
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x0600612D RID: 24877 RVA: 0x0029C498 File Offset: 0x0029B498
		internal ContainerTracking<T> Previous
		{
			get
			{
				return this._previous;
			}
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x0029C4A0 File Offset: 0x0029B4A0
		internal void StartTracking(ref ContainerTracking<T> root)
		{
			if (root != null)
			{
				root._previous = this;
			}
			this._next = root;
			root = this;
		}

		// Token: 0x0600612F RID: 24879 RVA: 0x0029C4BC File Offset: 0x0029B4BC
		internal void StopTracking(ref ContainerTracking<T> root)
		{
			if (this._previous != null)
			{
				this._previous._next = this._next;
			}
			if (this._next != null)
			{
				this._next._previous = this._previous;
			}
			if (root == this)
			{
				root = this._next;
			}
			this._previous = null;
			this._next = null;
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		internal void Debug_AssertIsInList(ContainerTracking<T> root)
		{
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		internal void Debug_AssertNotInList(ContainerTracking<T> root)
		{
		}

		// Token: 0x04003261 RID: 12897
		private T _container;

		// Token: 0x04003262 RID: 12898
		private ContainerTracking<T> _next;

		// Token: 0x04003263 RID: 12899
		private ContainerTracking<T> _previous;
	}
}
