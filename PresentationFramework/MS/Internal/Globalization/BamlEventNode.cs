using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001AB RID: 427
	internal sealed class BamlEventNode : BamlTreeNode
	{
		// Token: 0x06000E0A RID: 3594 RVA: 0x0013722E File Offset: 0x0013622E
		internal BamlEventNode(string eventName, string handlerName) : base(BamlNodeType.Event)
		{
			this._eventName = eventName;
			this._handlerName = handlerName;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00137246 File Offset: 0x00136246
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEvent(this._eventName, this._handlerName);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0013725A File Offset: 0x0013625A
		internal override BamlTreeNode Copy()
		{
			return new BamlEventNode(this._eventName, this._handlerName);
		}

		// Token: 0x04000A25 RID: 2597
		private string _eventName;

		// Token: 0x04000A26 RID: 2598
		private string _handlerName;
	}
}
