using System;

namespace System.Windows
{
	// Token: 0x0200039C RID: 924
	public class SizeChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600255B RID: 9563 RVA: 0x0018633C File Offset: 0x0018533C
		internal SizeChangedEventArgs(UIElement element, SizeChangedInfo info)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this._element = element;
			this._previousSize = info.PreviousSize;
			if (info.WidthChanged)
			{
				this._bits |= SizeChangedEventArgs._widthChangedBit;
			}
			if (info.HeightChanged)
			{
				this._bits |= SizeChangedEventArgs._heightChangedBit;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x001863B4 File Offset: 0x001853B4
		public Size PreviousSize
		{
			get
			{
				return this._previousSize;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x001863BC File Offset: 0x001853BC
		public Size NewSize
		{
			get
			{
				return this._element.RenderSize;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x001863C9 File Offset: 0x001853C9
		public bool WidthChanged
		{
			get
			{
				return (this._bits & SizeChangedEventArgs._widthChangedBit) > 0;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x001863DA File Offset: 0x001853DA
		public bool HeightChanged
		{
			get
			{
				return (this._bits & SizeChangedEventArgs._heightChangedBit) > 0;
			}
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x001863EB File Offset: 0x001853EB
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((SizeChangedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04001189 RID: 4489
		private Size _previousSize;

		// Token: 0x0400118A RID: 4490
		private UIElement _element;

		// Token: 0x0400118B RID: 4491
		private byte _bits;

		// Token: 0x0400118C RID: 4492
		private static byte _widthChangedBit = 1;

		// Token: 0x0400118D RID: 4493
		private static byte _heightChangedBit = 2;
	}
}
