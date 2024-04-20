using System;

namespace MS.Internal.Xaml.Context
{
	// Token: 0x02000336 RID: 822
	internal abstract class XamlFrame
	{
		// Token: 0x06001F02 RID: 7938 RVA: 0x00170A46 File Offset: 0x0016FA46
		protected XamlFrame()
		{
			this._depth = -1;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00170A55 File Offset: 0x0016FA55
		protected XamlFrame(XamlFrame source)
		{
			this._depth = source._depth;
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x001056E1 File Offset: 0x001046E1
		public virtual XamlFrame Clone()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F05 RID: 7941
		public abstract void Reset();

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00170A69 File Offset: 0x0016FA69
		public int Depth
		{
			get
			{
				return this._depth;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x00170A71 File Offset: 0x0016FA71
		// (set) Token: 0x06001F08 RID: 7944 RVA: 0x00170A79 File Offset: 0x0016FA79
		public XamlFrame Previous
		{
			get
			{
				return this._previous;
			}
			set
			{
				this._previous = value;
				this._depth = ((this._previous == null) ? 0 : (this._previous._depth + 1));
			}
		}

		// Token: 0x04000F62 RID: 3938
		private int _depth;

		// Token: 0x04000F63 RID: 3939
		private XamlFrame _previous;
	}
}
