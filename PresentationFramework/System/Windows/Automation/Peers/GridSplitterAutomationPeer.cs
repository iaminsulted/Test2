using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000565 RID: 1381
	public class GridSplitterAutomationPeer : ThumbAutomationPeer, ITransformProvider
	{
		// Token: 0x0600441E RID: 17438 RVA: 0x00220401 File Offset: 0x0021F401
		public GridSplitterAutomationPeer(GridSplitter owner) : base(owner)
		{
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0022040A File Offset: 0x0021F40A
		protected override string GetClassNameCore()
		{
			return "GridSplitter";
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x00220411 File Offset: 0x0021F411
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Transform)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06004421 RID: 17441 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITransformProvider.CanMove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITransformProvider.CanResize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06004423 RID: 17443 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITransformProvider.CanRotate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x00220424 File Offset: 0x0021F424
		void ITransformProvider.Move(double x, double y)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			if (double.IsInfinity(x) || double.IsNaN(x))
			{
				throw new ArgumentOutOfRangeException("x");
			}
			if (double.IsInfinity(y) || double.IsNaN(y))
			{
				throw new ArgumentOutOfRangeException("y");
			}
			((GridSplitter)base.Owner).KeyboardMoveSplitter(x, y);
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00220488 File Offset: 0x0021F488
		void ITransformProvider.Resize(double width, double height)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x00220488 File Offset: 0x0021F488
		void ITransformProvider.Rotate(double degrees)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}
	}
}
