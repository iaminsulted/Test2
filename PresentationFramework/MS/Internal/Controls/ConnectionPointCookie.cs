using System;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000252 RID: 594
	internal class ConnectionPointCookie
	{
		// Token: 0x06001707 RID: 5895 RVA: 0x0015C9A8 File Offset: 0x0015B9A8
		internal ConnectionPointCookie(object source, object sink, Type eventInterface)
		{
			Exception ex = null;
			if (source is UnsafeNativeMethods.IConnectionPointContainer)
			{
				UnsafeNativeMethods.IConnectionPointContainer connectionPointContainer = (UnsafeNativeMethods.IConnectionPointContainer)source;
				try
				{
					Guid guid = eventInterface.GUID;
					if (connectionPointContainer.FindConnectionPoint(ref guid, out this.connectionPoint) != 0)
					{
						this.connectionPoint = null;
					}
				}
				catch (Exception ex2)
				{
					if (CriticalExceptions.IsCriticalException(ex2))
					{
						throw;
					}
					this.connectionPoint = null;
				}
				if (this.connectionPoint == null)
				{
					ex = new ArgumentException(SR.Get("AxNoEventInterface", new object[]
					{
						eventInterface.Name
					}));
				}
				else if (sink == null || (!eventInterface.IsInstanceOfType(sink) && !Marshal.IsComObject(sink)))
				{
					ex = new InvalidCastException(SR.Get("AxNoSinkImplementation", new object[]
					{
						eventInterface.Name
					}));
				}
				else
				{
					int num = this.connectionPoint.Advise(sink, ref this.cookie);
					if (num != 0)
					{
						this.cookie = 0;
						Marshal.FinalReleaseComObject(this.connectionPoint);
						this.connectionPoint = null;
						ex = new InvalidOperationException(SR.Get("AxNoSinkAdvise", new object[]
						{
							eventInterface.Name,
							num
						}));
					}
				}
			}
			else
			{
				ex = new InvalidCastException(SR.Get("AxNoConnectionPointContainer"));
			}
			if (this.connectionPoint != null && this.cookie != 0)
			{
				return;
			}
			if (this.connectionPoint != null)
			{
				Marshal.FinalReleaseComObject(this.connectionPoint);
			}
			if (ex == null)
			{
				throw new ArgumentException(SR.Get("AxNoConnectionPoint", new object[]
				{
					eventInterface.Name
				}));
			}
			throw ex;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0015CB24 File Offset: 0x0015BB24
		internal void Disconnect()
		{
			if (this.connectionPoint != null && this.cookie != 0)
			{
				try
				{
					this.connectionPoint.Unadvise(this.cookie);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.cookie = 0;
				}
				try
				{
					Marshal.FinalReleaseComObject(this.connectionPoint);
				}
				catch (Exception ex2)
				{
					if (CriticalExceptions.IsCriticalException(ex2))
					{
						throw;
					}
				}
				finally
				{
					this.connectionPoint = null;
				}
			}
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0015CBC0 File Offset: 0x0015BBC0
		~ConnectionPointCookie()
		{
			this.Disconnect();
		}

		// Token: 0x04000C85 RID: 3205
		private UnsafeNativeMethods.IConnectionPoint connectionPoint;

		// Token: 0x04000C86 RID: 3206
		private int cookie;
	}
}
