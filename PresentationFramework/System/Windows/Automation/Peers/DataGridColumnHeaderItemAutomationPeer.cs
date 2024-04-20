using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000551 RID: 1361
	public class DataGridColumnHeaderItemAutomationPeer : ItemAutomationPeer, IInvokeProvider, IScrollItemProvider, ITransformProvider, IVirtualizedItemProvider
	{
		// Token: 0x06004344 RID: 17220 RVA: 0x0021DD81 File Offset: 0x0021CD81
		public DataGridColumnHeaderItemAutomationPeer(object item, DataGridColumn column, DataGridColumnHeadersPresenterAutomationPeer peer) : base(item, peer)
		{
			this._column = column;
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x001FC275 File Offset: 0x001FB275
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x0021DD94 File Offset: 0x0021CD94
		protected override string GetClassNameCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClassName();
			}
			base.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x0021DDC0 File Offset: 0x0021CDC0
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface == PatternInterface.ScrollItem)
					{
						if (this.Column != null)
						{
							return this;
						}
					}
				}
				else if (this.Column != null && this.Column.CanUserSort)
				{
					return this;
				}
			}
			else if (patternInterface != PatternInterface.Transform)
			{
				if (patternInterface == PatternInterface.VirtualizedItem)
				{
					if (this.Column != null)
					{
						return this;
					}
				}
			}
			else if (this.Column != null && this.Column.CanUserResize)
			{
				return this;
			}
			return null;
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x0021DE2C File Offset: 0x0021CE2C
		void IInvokeProvider.Invoke()
		{
			UIElementAutomationPeer uielementAutomationPeer = this.GetWrapperPeer() as UIElementAutomationPeer;
			if (uielementAutomationPeer != null)
			{
				((DataGridColumnHeader)uielementAutomationPeer.Owner).Invoke();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x0021DE5F File Offset: 0x0021CE5F
		void IScrollItemProvider.ScrollIntoView()
		{
			if (this.Column != null && this.OwningDataGrid != null)
			{
				this.OwningDataGrid.ScrollIntoView(null, this.Column);
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x0600434B RID: 17227 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITransformProvider.CanMove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x0600434C RID: 17228 RVA: 0x0021DE83 File Offset: 0x0021CE83
		bool ITransformProvider.CanResize
		{
			get
			{
				return this.Column != null && this.Column.CanUserResize;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x0600434D RID: 17229 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITransformProvider.CanRotate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x0021DE9A File Offset: 0x0021CE9A
		void ITransformProvider.Move(double x, double y)
		{
			throw new InvalidOperationException(SR.Get("DataGridColumnHeaderItemAutomationPeer_Unsupported"));
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x0021DEAB File Offset: 0x0021CEAB
		void ITransformProvider.Resize(double width, double height)
		{
			if (this.OwningDataGrid != null && this.Column.CanUserResize)
			{
				this.Column.Width = new DataGridLength(width);
				return;
			}
			throw new InvalidOperationException(SR.Get("DataGridColumnHeaderItemAutomationPeer_Unresizable"));
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x0021DE9A File Offset: 0x0021CE9A
		void ITransformProvider.Rotate(double degrees)
		{
			throw new InvalidOperationException(SR.Get("DataGridColumnHeaderItemAutomationPeer_Unsupported"));
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x0021DEE3 File Offset: 0x0021CEE3
		void IVirtualizedItemProvider.Realize()
		{
			if (this.OwningDataGrid != null)
			{
				this.OwningDataGrid.ScrollIntoView(null, this.Column);
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06004352 RID: 17234 RVA: 0x0021DEFF File Offset: 0x0021CEFF
		// (set) Token: 0x06004353 RID: 17235 RVA: 0x0021DF08 File Offset: 0x0021CF08
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer owningColumnHeaderPeer = this.OwningColumnHeaderPeer;
				if (owningColumnHeaderPeer != null)
				{
					owningColumnHeaderPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06004354 RID: 17236 RVA: 0x0021DF31 File Offset: 0x0021CF31
		internal DataGridColumnHeader OwningHeader
		{
			get
			{
				return base.GetWrapper() as DataGridColumnHeader;
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06004355 RID: 17237 RVA: 0x0021DF3E File Offset: 0x0021CF3E
		internal DataGrid OwningDataGrid
		{
			get
			{
				return this.Column.DataGridOwner;
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06004356 RID: 17238 RVA: 0x0021DF4B File Offset: 0x0021CF4B
		internal DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06004357 RID: 17239 RVA: 0x0021DF53 File Offset: 0x0021CF53
		internal DataGridColumnHeaderAutomationPeer OwningColumnHeaderPeer
		{
			get
			{
				return this.GetWrapperPeer() as DataGridColumnHeaderAutomationPeer;
			}
		}

		// Token: 0x04002517 RID: 9495
		private DataGridColumn _column;
	}
}
