using System;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000268 RID: 616
	internal class LockHelper
	{
		// Token: 0x060017E3 RID: 6115 RVA: 0x0015FA2C File Offset: 0x0015EA2C
		public bool IsLocked(LockHelper.LockFlag flag)
		{
			return (this._backingStore & flag) > (LockHelper.LockFlag)0;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0015FA39 File Offset: 0x0015EA39
		private void Lock(LockHelper.LockFlag flag)
		{
			this._backingStore |= flag;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0015FA49 File Offset: 0x0015EA49
		private void Unlock(LockHelper.LockFlag flag)
		{
			this._backingStore &= ~flag;
		}

		// Token: 0x04000CCA RID: 3274
		private LockHelper.LockFlag _backingStore;

		// Token: 0x02000A0D RID: 2573
		[Flags]
		public enum LockFlag
		{
			// Token: 0x04004079 RID: 16505
			AnnotationChanged = 1,
			// Token: 0x0400407A RID: 16506
			DataChanged = 2
		}

		// Token: 0x02000A0E RID: 2574
		public class AutoLocker : IDisposable
		{
			// Token: 0x060084C1 RID: 33985 RVA: 0x00326EAE File Offset: 0x00325EAE
			public AutoLocker(LockHelper helper, LockHelper.LockFlag flag)
			{
				if (helper == null)
				{
					throw new ArgumentNullException("helper");
				}
				this._helper = helper;
				this._flag = flag;
				this._helper.Lock(this._flag);
			}

			// Token: 0x060084C2 RID: 33986 RVA: 0x00326EE3 File Offset: 0x00325EE3
			public void Dispose()
			{
				this._helper.Unlock(this._flag);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060084C3 RID: 33987 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
			private AutoLocker()
			{
			}

			// Token: 0x0400407B RID: 16507
			private LockHelper _helper;

			// Token: 0x0400407C RID: 16508
			private LockHelper.LockFlag _flag;
		}
	}
}
