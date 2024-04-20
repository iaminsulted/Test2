using System;
using System.Collections.Generic;
using System.Windows;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200010B RID: 267
	internal abstract class BaseParaClient : UnmanagedHandle
	{
		// Token: 0x060006A4 RID: 1700 RVA: 0x00109339 File Offset: 0x00108339
		protected BaseParaClient(BaseParagraph paragraph) : base(paragraph.PtsContext)
		{
			this._paraHandle = new SecurityCriticalDataForSet<IntPtr>(IntPtr.Zero);
			this._paragraph = paragraph;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00109360 File Offset: 0x00108360
		internal void Arrange(IntPtr pfspara, PTS.FSRECT rcPara, int dvrTopSpace, uint fswdirParent)
		{
			this._paraHandle.Value = pfspara;
			this._rect = rcPara;
			this._dvrTopSpace = dvrTopSpace;
			this._pageContext = this.Paragraph.StructuralCache.CurrentArrangeContext.PageContext;
			this._flowDirectionParent = PTS.FswdirToFlowDirection(fswdirParent);
			this._flowDirection = (FlowDirection)this.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty);
			this.OnArrange();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x001093D5 File Offset: 0x001083D5
		internal virtual int GetFirstTextLineBaseline()
		{
			return this._rect.v + this._rect.dv;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x001093EE File Offset: 0x001083EE
		internal void TransferDisplayInfo(BaseParaClient oldParaClient)
		{
			this._visual = oldParaClient._visual;
			oldParaClient._visual = null;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			return null;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00109406 File Offset: 0x00108406
		internal virtual List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			return new List<Rect>();
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00109410 File Offset: 0x00108410
		internal virtual void GetRectanglesForParagraphElement(out List<Rect> rectangles)
		{
			rectangles = new List<Rect>();
			Rect item = TextDpi.FromTextRect(this._rect);
			rectangles.Add(item);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void UpdateViewport(ref PTS.FSRECT viewport)
		{
		}

		// Token: 0x060006AD RID: 1709
		internal abstract ParagraphResult CreateParagraphResult();

		// Token: 0x060006AE RID: 1710
		internal abstract TextContentRange GetTextContentRange();

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00109438 File Offset: 0x00108438
		internal virtual ParagraphVisual Visual
		{
			get
			{
				if (this._visual == null)
				{
					this._visual = new ParagraphVisual();
				}
				return this._visual;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool IsFirstChunk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool IsLastChunk
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00109453 File Offset: 0x00108453
		internal BaseParagraph Paragraph
		{
			get
			{
				return this._paragraph;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0010945B File Offset: 0x0010845B
		internal PTS.FSRECT Rect
		{
			get
			{
				return this._rect;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00109463 File Offset: 0x00108463
		internal FlowDirection ThisFlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0010946B File Offset: 0x0010846B
		internal FlowDirection ParentFlowDirection
		{
			get
			{
				return this._flowDirectionParent;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00109473 File Offset: 0x00108473
		internal FlowDirection PageFlowDirection
		{
			get
			{
				return this.Paragraph.StructuralCache.PageFlowDirection;
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00109485 File Offset: 0x00108485
		protected virtual void OnArrange()
		{
			this.Paragraph.UpdateLastFormatPositions();
		}

		// Token: 0x0400071C RID: 1820
		protected readonly BaseParagraph _paragraph;

		// Token: 0x0400071D RID: 1821
		protected SecurityCriticalDataForSet<IntPtr> _paraHandle;

		// Token: 0x0400071E RID: 1822
		protected PTS.FSRECT _rect;

		// Token: 0x0400071F RID: 1823
		protected int _dvrTopSpace;

		// Token: 0x04000720 RID: 1824
		protected ParagraphVisual _visual;

		// Token: 0x04000721 RID: 1825
		protected PageContext _pageContext;

		// Token: 0x04000722 RID: 1826
		protected FlowDirection _flowDirectionParent;

		// Token: 0x04000723 RID: 1827
		protected FlowDirection _flowDirection;
	}
}
