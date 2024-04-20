using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Automation;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000559 RID: 1369
	public class DocumentAutomationPeer : ContentTextAutomationPeer
	{
		// Token: 0x060043D7 RID: 17367 RVA: 0x0021F6A8 File Offset: 0x0021E6A8
		public DocumentAutomationPeer(FrameworkContentElement owner) : base(owner)
		{
			if (owner is IServiceProvider)
			{
				this._textContainer = (((IServiceProvider)owner).GetService(typeof(ITextContainer)) as ITextContainer);
				if (this._textContainer != null)
				{
					this._textPattern = new TextAdaptor(this, this._textContainer);
				}
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0021F6FE File Offset: 0x0021E6FE
		internal void OnDisconnected()
		{
			if (this._textPattern != null)
			{
				this._textPattern.Dispose();
				this._textPattern = null;
			}
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x0021F71C File Offset: 0x0021E71C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this._childrenStart != null && this._childrenEnd != null)
			{
				ITextContainer textContainer = ((IServiceProvider)base.Owner).GetService(typeof(ITextContainer)) as ITextContainer;
				return TextContainerHelper.GetAutomationPeersFromRange(this._childrenStart, this._childrenEnd, textContainer.Start);
			}
			return null;
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0021F774 File Offset: 0x0021E774
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result;
			if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null && base.Owner is IServiceProvider)
				{
					ITextContainer textContainer = ((IServiceProvider)base.Owner).GetService(typeof(ITextContainer)) as ITextContainer;
					if (textContainer != null)
					{
						this._textPattern = new TextAdaptor(this, textContainer);
					}
				}
				result = this._textPattern;
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x001FD726 File Offset: 0x001FC726
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x0021F7DF File Offset: 0x0021E7DF
		protected override string GetClassNameCore()
		{
			return "Document";
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0021F7E6 File Offset: 0x0021E7E6
		protected override bool IsControlElementCore()
		{
			if (base.IncludeInvisibleElementsInControlView)
			{
				return true;
			}
			ITextContainer textContainer = this._textContainer;
			ITextView textView = (textContainer != null) ? textContainer.TextView : null;
			UIElement uielement = (textView != null) ? textView.RenderScope : null;
			return uielement != null && uielement.IsVisible;
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0021F81C File Offset: 0x0021E81C
		protected override Rect GetBoundingRectangleCore()
		{
			UIElement uielement;
			Rect rect = this.CalculateBoundingRect(false, out uielement);
			if (rect != Rect.Empty && uielement != null)
			{
				HwndSource hwndSource = PresentationSource.CriticalFromVisual(uielement) as HwndSource;
				if (hwndSource != null)
				{
					rect = PointUtil.ElementToRoot(rect, uielement, hwndSource);
					rect = PointUtil.RootToClient(rect, hwndSource);
					rect = PointUtil.ClientToScreen(rect, hwndSource);
				}
			}
			return rect;
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0021F86C File Offset: 0x0021E86C
		protected override Point GetClickablePointCore()
		{
			Point result = default(Point);
			UIElement uielement;
			Rect rect = this.CalculateBoundingRect(true, out uielement);
			if (rect != Rect.Empty && uielement != null)
			{
				HwndSource hwndSource = PresentationSource.CriticalFromVisual(uielement) as HwndSource;
				if (hwndSource != null)
				{
					rect = PointUtil.ElementToRoot(rect, uielement, hwndSource);
					rect = PointUtil.RootToClient(rect, hwndSource);
					rect = PointUtil.ClientToScreen(rect, hwndSource);
					result = new Point(rect.Left + rect.Width * 0.1, rect.Top + rect.Height * 0.1);
				}
			}
			return result;
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0021F900 File Offset: 0x0021E900
		protected override bool IsOffscreenCore()
		{
			IsOffscreenBehavior isOffscreenBehavior = AutomationProperties.GetIsOffscreenBehavior(base.Owner);
			UIElement uielement;
			return isOffscreenBehavior != IsOffscreenBehavior.Onscreen && (isOffscreenBehavior == IsOffscreenBehavior.Offscreen || DoubleUtil.AreClose(this.CalculateBoundingRect(true, out uielement), Rect.Empty) || uielement == null);
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0021F942 File Offset: 0x0021E942
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			this._childrenStart = start.CreatePointer();
			this._childrenEnd = end.CreatePointer();
			base.ResetChildrenCache();
			return base.GetChildren();
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0021F968 File Offset: 0x0021E968
		private Rect CalculateBoundingRect(bool clipToVisible, out UIElement uiScope)
		{
			uiScope = null;
			Rect empty = Rect.Empty;
			if (base.Owner is IServiceProvider)
			{
				ITextContainer textContainer = ((IServiceProvider)base.Owner).GetService(typeof(ITextContainer)) as ITextContainer;
				ITextView textView = (textContainer != null) ? textContainer.TextView : null;
				if (textView != null)
				{
					if (!textView.IsValid)
					{
						if (!textView.Validate())
						{
							textView = null;
						}
						if (textView != null && !textView.IsValid)
						{
							textView = null;
						}
					}
					if (textView != null)
					{
						empty = new Rect(textView.RenderScope.RenderSize);
						uiScope = textView.RenderScope;
						if (clipToVisible)
						{
							Visual visual = textView.RenderScope;
							while (visual != null && empty != Rect.Empty)
							{
								if (VisualTreeHelper.GetClip(visual) != null)
								{
									GeneralTransform inverse = textView.RenderScope.TransformToAncestor(visual).Inverse;
									if (inverse != null)
									{
										Rect rect = VisualTreeHelper.GetClip(visual).Bounds;
										rect = inverse.TransformBounds(rect);
										empty.Intersect(rect);
									}
									else
									{
										empty = Rect.Empty;
									}
								}
								visual = (VisualTreeHelper.GetParent(visual) as Visual);
							}
						}
					}
				}
			}
			return empty;
		}

		// Token: 0x0400251E RID: 9502
		private ITextPointer _childrenStart;

		// Token: 0x0400251F RID: 9503
		private ITextPointer _childrenEnd;

		// Token: 0x04002520 RID: 9504
		private TextAdaptor _textPattern;

		// Token: 0x04002521 RID: 9505
		private ITextContainer _textContainer;
	}
}
