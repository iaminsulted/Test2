using System;

namespace WebSocketSharp.Server
{
	// Token: 0x0200004E RID: 78
	internal class WebSocketServiceHost<TBehavior> : WebSocketServiceHost where TBehavior : WebSocketBehavior
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		internal WebSocketServiceHost(string path, Func<TBehavior> creator, Logger log) : this(path, creator, null, log)
		{
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001EAE2 File Offset: 0x0001CCE2
		internal WebSocketServiceHost(string path, Func<TBehavior> creator, Action<TBehavior> initializer, Logger log) : base(path, log)
		{
			this._creator = this.createCreator(creator, initializer);
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001EB00 File Offset: 0x0001CD00
		public override Type BehaviorType
		{
			get
			{
				return typeof(TBehavior);
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001EB1C File Offset: 0x0001CD1C
		private Func<TBehavior> createCreator(Func<TBehavior> creator, Action<TBehavior> initializer)
		{
			bool flag = initializer == null;
			Func<TBehavior> result;
			if (flag)
			{
				result = creator;
			}
			else
			{
				result = delegate()
				{
					TBehavior tbehavior = creator();
					initializer(tbehavior);
					return tbehavior;
				};
			}
			return result;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001EB64 File Offset: 0x0001CD64
		protected override WebSocketBehavior CreateSession()
		{
			return this._creator();
		}

		// Token: 0x04000260 RID: 608
		private Func<TBehavior> _creator;
	}
}
