using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x02000607 RID: 1543
	internal abstract class FixedSOMElement : FixedSOMSemanticBox
	{
		// Token: 0x06004B30 RID: 19248 RVA: 0x0023638C File Offset: 0x0023538C
		protected FixedSOMElement(FixedNode fixedNode, int startIndex, int endIndex, GeneralTransform transform)
		{
			this._fixedNode = fixedNode;
			this._startIndex = startIndex;
			this._endIndex = endIndex;
			Transform affineTransform = transform.AffineTransform;
			if (affineTransform != null)
			{
				this._mat = affineTransform.Value;
				return;
			}
			this._mat = Transform.Identity.Value;
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x002363DC File Offset: 0x002353DC
		protected FixedSOMElement(FixedNode fixedNode, GeneralTransform transform)
		{
			this._fixedNode = fixedNode;
			Transform affineTransform = transform.AffineTransform;
			if (affineTransform != null)
			{
				this._mat = affineTransform.Value;
				return;
			}
			this._mat = Transform.Identity.Value;
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x00236420 File Offset: 0x00235420
		public static FixedSOMElement CreateFixedSOMElement(FixedPage page, UIElement uiElement, FixedNode fixedNode, int startIndex, int endIndex)
		{
			FixedSOMElement result = null;
			if (uiElement is Glyphs)
			{
				Glyphs glyphs = uiElement as Glyphs;
				if (glyphs.UnicodeString.Length > 0)
				{
					GlyphRun glyphRun = glyphs.ToGlyphRun();
					Rect boundingRect = glyphRun.ComputeAlignmentBox();
					boundingRect.Offset(glyphs.OriginX, glyphs.OriginY);
					GeneralTransform transform = glyphs.TransformToAncestor(page);
					if (startIndex < 0)
					{
						startIndex = 0;
					}
					if (endIndex < 0)
					{
						endIndex = ((glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count);
					}
					result = FixedSOMTextRun.Create(boundingRect, transform, glyphs, fixedNode, startIndex, endIndex, false);
				}
			}
			else if (uiElement is Image)
			{
				result = FixedSOMImage.Create(page, uiElement as Image, fixedNode);
			}
			else if (uiElement is Path)
			{
				result = FixedSOMImage.Create(page, uiElement as Path, fixedNode);
			}
			return result;
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x002364DD File Offset: 0x002354DD
		public FixedNode FixedNode
		{
			get
			{
				return this._fixedNode;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06004B34 RID: 19252 RVA: 0x002364E5 File Offset: 0x002354E5
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x002364ED File Offset: 0x002354ED
		public int EndIndex
		{
			get
			{
				return this._endIndex;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06004B36 RID: 19254 RVA: 0x002364F5 File Offset: 0x002354F5
		// (set) Token: 0x06004B37 RID: 19255 RVA: 0x002364FD File Offset: 0x002354FD
		internal FlowNode FlowNode
		{
			get
			{
				return this._flowNode;
			}
			set
			{
				this._flowNode = value;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x00236506 File Offset: 0x00235506
		// (set) Token: 0x06004B39 RID: 19257 RVA: 0x0023650E File Offset: 0x0023550E
		internal int OffsetInFlowNode
		{
			get
			{
				return this._offsetInFlowNode;
			}
			set
			{
				this._offsetInFlowNode = value;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x06004B3A RID: 19258 RVA: 0x00236517 File Offset: 0x00235517
		internal Matrix Matrix
		{
			get
			{
				return this._mat;
			}
		}

		// Token: 0x0400276C RID: 10092
		protected FixedNode _fixedNode;

		// Token: 0x0400276D RID: 10093
		protected int _startIndex;

		// Token: 0x0400276E RID: 10094
		protected int _endIndex;

		// Token: 0x0400276F RID: 10095
		protected Matrix _mat;

		// Token: 0x04002770 RID: 10096
		private FlowNode _flowNode;

		// Token: 0x04002771 RID: 10097
		private int _offsetInFlowNode;
	}
}
