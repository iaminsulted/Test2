using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000568 RID: 1384
	public class GridViewColumnHeaderAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider, ITransformProvider
	{
		// Token: 0x06004449 RID: 17481 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public GridViewColumnHeaderAutomationPeer(GridViewColumnHeader owner) : base(owner)
		{
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x001FC275 File Offset: 0x001FB275
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x00220B84 File Offset: 0x0021FB84
		protected override string GetClassNameCore()
		{
			return "GridViewColumnHeader";
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x00220B8B File Offset: 0x0021FB8B
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke || patternInterface == PatternInterface.Transform)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x00220B9E File Offset: 0x0021FB9E
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((GridViewColumnHeader)base.Owner).AutomationClick();
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x0600444F RID: 17487 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITransformProvider.CanMove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06004450 RID: 17488 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITransformProvider.CanResize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06004451 RID: 17489 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITransformProvider.CanRotate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x00220488 File Offset: 0x0021F488
		void ITransformProvider.Move(double x, double y)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x00220BC0 File Offset: 0x0021FBC0
		void ITransformProvider.Resize(double width, double height)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			if (width < 0.0)
			{
				throw new ArgumentOutOfRangeException("width");
			}
			if (height < 0.0)
			{
				throw new ArgumentOutOfRangeException("height");
			}
			GridViewColumnHeader gridViewColumnHeader = base.Owner as GridViewColumnHeader;
			if (gridViewColumnHeader != null)
			{
				if (gridViewColumnHeader.Column != null)
				{
					gridViewColumnHeader.Column.Width = width;
				}
				gridViewColumnHeader.Height = height;
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x00220488 File Offset: 0x0021F488
		void ITransformProvider.Rotate(double degrees)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}
	}
}
