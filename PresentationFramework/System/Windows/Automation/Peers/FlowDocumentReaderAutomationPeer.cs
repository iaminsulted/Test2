using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000560 RID: 1376
	public class FlowDocumentReaderAutomationPeer : FrameworkElementAutomationPeer, IMultipleViewProvider
	{
		// Token: 0x060043FF RID: 17407 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public FlowDocumentReaderAutomationPeer(FlowDocumentReader owner) : base(owner)
		{
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0021FDE0 File Offset: 0x0021EDE0
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result;
			if (patternInterface == PatternInterface.MultipleView)
			{
				result = this;
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0021FE04 File Offset: 0x0021EE04
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			FlowDocument document = ((FlowDocumentReader)base.Owner).Document;
			if (document != null)
			{
				AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement(document);
				if (this._documentPeer != automationPeer)
				{
					if (this._documentPeer != null)
					{
						this._documentPeer.OnDisconnected();
					}
					this._documentPeer = (automationPeer as DocumentAutomationPeer);
				}
				if (automationPeer != null)
				{
					if (list == null)
					{
						list = new List<AutomationPeer>();
					}
					list.Add(automationPeer);
				}
			}
			return list;
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0021FE6F File Offset: 0x0021EE6F
		protected override string GetClassNameCore()
		{
			return "FlowDocumentReader";
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0021FE76 File Offset: 0x0021EE76
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseCurrentViewChangedEvent(FlowDocumentReaderViewingMode newMode, FlowDocumentReaderViewingMode oldMode)
		{
			if (newMode != oldMode)
			{
				base.RaisePropertyChangedEvent(MultipleViewPatternIdentifiers.CurrentViewProperty, this.ConvertModeToViewId(newMode), this.ConvertModeToViewId(oldMode));
			}
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0021FEA0 File Offset: 0x0021EEA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseSupportedViewsChangedEvent(DependencyPropertyChangedEventArgs e)
		{
			bool flag;
			bool flag2;
			bool flag4;
			bool flag3;
			bool flag6;
			bool flag5;
			if (e.Property == FlowDocumentReader.IsPageViewEnabledProperty)
			{
				flag = (bool)e.NewValue;
				flag2 = (bool)e.OldValue;
				flag3 = (flag4 = this.FlowDocumentReader.IsTwoPageViewEnabled);
				flag5 = (flag6 = this.FlowDocumentReader.IsScrollViewEnabled);
			}
			else if (e.Property == FlowDocumentReader.IsTwoPageViewEnabledProperty)
			{
				flag2 = (flag = this.FlowDocumentReader.IsPageViewEnabled);
				flag4 = (bool)e.NewValue;
				flag3 = (bool)e.OldValue;
				flag5 = (flag6 = this.FlowDocumentReader.IsScrollViewEnabled);
			}
			else
			{
				flag2 = (flag = this.FlowDocumentReader.IsPageViewEnabled);
				flag3 = (flag4 = this.FlowDocumentReader.IsTwoPageViewEnabled);
				flag6 = (bool)e.NewValue;
				flag5 = (bool)e.OldValue;
			}
			if (flag != flag2 || flag4 != flag3 || flag6 != flag5)
			{
				int[] supportedViews = this.GetSupportedViews(flag, flag4, flag6);
				int[] supportedViews2 = this.GetSupportedViews(flag2, flag3, flag5);
				base.RaisePropertyChangedEvent(MultipleViewPatternIdentifiers.SupportedViewsProperty, supportedViews, supportedViews2);
			}
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0021FFB0 File Offset: 0x0021EFB0
		private int[] GetSupportedViews(bool single, bool facing, bool scroll)
		{
			int num = 0;
			if (single)
			{
				num++;
			}
			if (facing)
			{
				num++;
			}
			if (scroll)
			{
				num++;
			}
			int[] array = (num > 0) ? new int[num] : null;
			num = 0;
			if (single)
			{
				array[num++] = this.ConvertModeToViewId(FlowDocumentReaderViewingMode.Page);
			}
			if (facing)
			{
				array[num++] = this.ConvertModeToViewId(FlowDocumentReaderViewingMode.TwoPage);
			}
			if (scroll)
			{
				array[num++] = this.ConvertModeToViewId(FlowDocumentReaderViewingMode.Scroll);
			}
			return array;
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x001136C4 File Offset: 0x001126C4
		private int ConvertModeToViewId(FlowDocumentReaderViewingMode mode)
		{
			return (int)mode;
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x00220018 File Offset: 0x0021F018
		private FlowDocumentReaderViewingMode ConvertViewIdToMode(int viewId)
		{
			Invariant.Assert(viewId >= 0 && viewId <= 2);
			return (FlowDocumentReaderViewingMode)viewId;
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06004408 RID: 17416 RVA: 0x0022002E File Offset: 0x0021F02E
		private FlowDocumentReader FlowDocumentReader
		{
			get
			{
				return (FlowDocumentReader)base.Owner;
			}
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x0022003C File Offset: 0x0021F03C
		string IMultipleViewProvider.GetViewName(int viewId)
		{
			string result = string.Empty;
			if (viewId >= 0 && viewId <= 2)
			{
				FlowDocumentReaderViewingMode flowDocumentReaderViewingMode = this.ConvertViewIdToMode(viewId);
				if (flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Page)
				{
					result = SR.Get("FlowDocumentReader_MultipleViewProvider_PageViewName");
				}
				else if (flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.TwoPage)
				{
					result = SR.Get("FlowDocumentReader_MultipleViewProvider_TwoPageViewName");
				}
				else if (flowDocumentReaderViewingMode == FlowDocumentReaderViewingMode.Scroll)
				{
					result = SR.Get("FlowDocumentReader_MultipleViewProvider_ScrollViewName");
				}
			}
			return result;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x00220090 File Offset: 0x0021F090
		void IMultipleViewProvider.SetCurrentView(int viewId)
		{
			if (viewId >= 0 && viewId <= 2)
			{
				this.FlowDocumentReader.ViewingMode = this.ConvertViewIdToMode(viewId);
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x0600440B RID: 17419 RVA: 0x002200AC File Offset: 0x0021F0AC
		int IMultipleViewProvider.CurrentView
		{
			get
			{
				return this.ConvertModeToViewId(this.FlowDocumentReader.ViewingMode);
			}
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x002200BF File Offset: 0x0021F0BF
		int[] IMultipleViewProvider.GetSupportedViews()
		{
			return this.GetSupportedViews(this.FlowDocumentReader.IsPageViewEnabled, this.FlowDocumentReader.IsTwoPageViewEnabled, this.FlowDocumentReader.IsScrollViewEnabled);
		}

		// Token: 0x04002523 RID: 9507
		private DocumentAutomationPeer _documentPeer;
	}
}
