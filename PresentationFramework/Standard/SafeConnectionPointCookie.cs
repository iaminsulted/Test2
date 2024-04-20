using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000042 RID: 66
	internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000062 RID: 98 RVA: 0x000F7A98 File Offset: 0x000F6A98
		public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId) : base(true)
		{
			Verify.IsNotNull<IConnectionPointContainer>(target, "target");
			Verify.IsNotNull<object>(sink, "sink");
			Verify.IsNotDefault<Guid>(eventId, "eventId");
			this.handle = IntPtr.Zero;
			IConnectionPoint connectionPoint = null;
			try
			{
				target.FindConnectionPoint(ref eventId, out connectionPoint);
				int num;
				connectionPoint.Advise(sink, out num);
				if (num == 0)
				{
					throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
				}
				this.handle = new IntPtr(num);
				this._cp = connectionPoint;
				connectionPoint = null;
			}
			finally
			{
				Utility.SafeRelease<IConnectionPoint>(ref connectionPoint);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000F7B2C File Offset: 0x000F6B2C
		public void Disconnect()
		{
			this.ReleaseHandle();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000F7B38 File Offset: 0x000F6B38
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			bool result;
			try
			{
				if (!this.IsInvalid)
				{
					int dwCookie = this.handle.ToInt32();
					this.handle = IntPtr.Zero;
					try
					{
						this._cp.Unadvise(dwCookie);
					}
					finally
					{
						Utility.SafeRelease<IConnectionPoint>(ref this._cp);
					}
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x040003FC RID: 1020
		private IConnectionPoint _cp;
	}
}
