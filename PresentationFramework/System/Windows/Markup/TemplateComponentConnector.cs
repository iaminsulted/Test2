using System;

namespace System.Windows.Markup
{
	// Token: 0x020004D7 RID: 1239
	internal class TemplateComponentConnector : IComponentConnector
	{
		// Token: 0x06003F57 RID: 16215 RVA: 0x00210FD6 File Offset: 0x0020FFD6
		internal TemplateComponentConnector(IComponentConnector componentConnector, IStyleConnector styleConnector)
		{
			this._styleConnector = styleConnector;
			this._componentConnector = componentConnector;
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00210FEC File Offset: 0x0020FFEC
		public void InitializeComponent()
		{
			this._componentConnector.InitializeComponent();
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00210FF9 File Offset: 0x0020FFF9
		public void Connect(int connectionId, object target)
		{
			if (this._styleConnector != null)
			{
				this._styleConnector.Connect(connectionId, target);
				return;
			}
			this._componentConnector.Connect(connectionId, target);
		}

		// Token: 0x04002375 RID: 9077
		private IStyleConnector _styleConnector;

		// Token: 0x04002376 RID: 9078
		private IComponentConnector _componentConnector;
	}
}
