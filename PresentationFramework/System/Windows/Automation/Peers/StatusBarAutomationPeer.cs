using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200058F RID: 1423
	public class StatusBarAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004581 RID: 17793 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public StatusBarAutomationPeer(StatusBar owner) : base(owner)
		{
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00223F87 File Offset: 0x00222F87
		protected override string GetClassNameCore()
		{
			return "StatusBar";
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x001FD227 File Offset: 0x001FC227
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.StatusBar;
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x00223F90 File Offset: 0x00222F90
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = new List<AutomationPeer>();
			ItemsControl itemsControl = base.Owner as ItemsControl;
			if (itemsControl != null)
			{
				foreach (object obj in ((IEnumerable)itemsControl.Items))
				{
					if (obj is Separator)
					{
						Separator element = obj as Separator;
						list.Add(UIElementAutomationPeer.CreatePeerForElement(element));
					}
					else
					{
						StatusBarItem statusBarItem = itemsControl.ItemContainerGenerator.ContainerFromItem(obj) as StatusBarItem;
						if (statusBarItem != null)
						{
							if (obj is string || obj is TextBlock || (obj is StatusBarItem && ((StatusBarItem)obj).Content is string))
							{
								list.Add(UIElementAutomationPeer.CreatePeerForElement(statusBarItem));
							}
							else
							{
								List<AutomationPeer> childrenAutomationPeer = this.GetChildrenAutomationPeer(statusBarItem);
								if (childrenAutomationPeer != null)
								{
									foreach (AutomationPeer item in childrenAutomationPeer)
									{
										list.Add(item);
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x002240C0 File Offset: 0x002230C0
		private List<AutomationPeer> GetChildrenAutomationPeer(Visual parent)
		{
			Invariant.Assert(parent != null);
			List<AutomationPeer> children = null;
			StatusBarAutomationPeer.iterate(parent, delegate(AutomationPeer peer)
			{
				if (children == null)
				{
					children = new List<AutomationPeer>();
				}
				children.Add(peer);
				return false;
			});
			return children;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x002240FC File Offset: 0x002230FC
		private static bool iterate(Visual parent, StatusBarAutomationPeer.IteratorCallback callback)
		{
			bool flag = false;
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			int num = 0;
			while (num < internalVisualChildrenCount && !flag)
			{
				Visual visual = parent.InternalGetVisualChild(num);
				AutomationPeer peer;
				if (visual != null && visual.CheckFlagsAnd(VisualFlags.IsUIElement) && (peer = UIElementAutomationPeer.CreatePeerForElement((UIElement)visual)) != null)
				{
					flag = callback(peer);
				}
				else
				{
					flag = StatusBarAutomationPeer.iterate(visual, callback);
				}
				num++;
			}
			return flag;
		}

		// Token: 0x02000B22 RID: 2850
		// (Invoke) Token: 0x06008C63 RID: 35939
		private delegate bool IteratorCallback(AutomationPeer peer);
	}
}
