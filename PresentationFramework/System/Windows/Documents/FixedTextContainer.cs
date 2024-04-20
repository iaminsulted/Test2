using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000617 RID: 1559
	internal sealed class FixedTextContainer : ITextContainer
	{
		// Token: 0x06004BEE RID: 19438 RVA: 0x0023AA55 File Offset: 0x00239A55
		internal FixedTextContainer(DependencyObject parent)
		{
			this._parent = parent;
			this._CreateEmptyContainer();
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextContainer.BeginChange()
		{
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x0022F68B File Offset: 0x0022E68B
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x0022F693 File Offset: 0x0022E693
		void ITextContainer.EndChange()
		{
			((ITextContainer)this).EndChange(false);
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextContainer.EndChange(bool skipEvents)
		{
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x0022F700 File Offset: 0x0022E700
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return ((ITextContainer)this).Start.CreatePointer(offset, direction);
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x001056E1 File Offset: 0x001046E1
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x0022F70F File Offset: 0x0022E70F
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x0022F723 File Offset: 0x0022E723
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x0022F733 File Offset: 0x0022E733
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x0022F747 File Offset: 0x0022E747
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x0022F766 File Offset: 0x0022E766
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x0022F77F File Offset: 0x0022E77F
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00109403 File Offset: 0x00108403
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x0022F793 File Offset: 0x0022E793
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x0022F7AD File Offset: 0x0022E7AD
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x0022F7C7 File Offset: 0x0022E7C7
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x0022F7E6 File Offset: 0x0022E7E6
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x0022F7FA File Offset: 0x0022E7FA
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x06004C01 RID: 19457 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x0023AA6A File Offset: 0x00239A6A
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x0023AA72 File Offset: 0x00239A72
		ITextPointer ITextContainer.End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x00105F35 File Offset: 0x00104F35
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x0023AA7A File Offset: 0x00239A7A
		Highlights ITextContainer.Highlights
		{
			get
			{
				return this.Highlights;
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0023AA82 File Offset: 0x00239A82
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x0023AA8A File Offset: 0x00239A8A
		// (set) Token: 0x06004C08 RID: 19464 RVA: 0x0023AA92 File Offset: 0x00239A92
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this.TextSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x00109403 File Offset: 0x00108403
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x0023AA9B File Offset: 0x00239A9B
		// (set) Token: 0x06004C0B RID: 19467 RVA: 0x0023AAA3 File Offset: 0x00239AA3
		ITextView ITextContainer.TextView
		{
			get
			{
				return this._textview;
			}
			set
			{
				this._textview = value;
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06004C0C RID: 19468 RVA: 0x0022F850 File Offset: 0x0022E850
		int ITextContainer.SymbolCount
		{
			get
			{
				return ((ITextContainer)this).Start.GetOffsetToPosition(((ITextContainer)this).End);
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x001056E1 File Offset: 0x001046E1
		int ITextContainer.IMECharCount
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06004C0E RID: 19470 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		// (remove) Token: 0x06004C0F RID: 19471 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public event EventHandler Changing
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06004C10 RID: 19472 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		// (remove) Token: 0x06004C11 RID: 19473 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public event TextContainerChangeEventHandler Change
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06004C12 RID: 19474 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		// (remove) Token: 0x06004C13 RID: 19475 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public event TextContainerChangedEventHandler Changed
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x0023AAAC File Offset: 0x00239AAC
		internal FixedTextPointer VerifyPosition(ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (position.TextContainer != this)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedContainer", new object[]
				{
					"position"
				}));
			}
			FixedTextPointer fixedTextPointer = position as FixedTextPointer;
			if (fixedTextPointer == null)
			{
				throw new ArgumentException(SR.Get("BadFixedTextPosition", new object[]
				{
					"position"
				}));
			}
			return fixedTextPointer;
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x0023AB18 File Offset: 0x00239B18
		internal int GetPageNumber(ITextPointer textPointer)
		{
			FixedTextPointer fixedTextPointer = textPointer as FixedTextPointer;
			int result = int.MaxValue;
			if (fixedTextPointer != null)
			{
				if (fixedTextPointer.CompareTo(((ITextContainer)this).Start) == 0)
				{
					result = 0;
				}
				else if (fixedTextPointer.CompareTo(((ITextContainer)this).End) == 0)
				{
					result = this.FixedDocument.PageCount - 1;
				}
				else
				{
					FlowNode flowNode;
					int num;
					fixedTextPointer.FlowPosition.GetFlowNode(fixedTextPointer.LogicalDirection, out flowNode, out num);
					FixedElement fixedElement = flowNode.Cookie as FixedElement;
					FixedPosition fixedPosition;
					if (flowNode.Type == FlowNodeType.Boundary)
					{
						if (flowNode.Fp > 0)
						{
							result = this.FixedDocument.PageCount - 1;
						}
						else
						{
							result = 0;
						}
					}
					else if (flowNode.Type == FlowNodeType.Virtual || flowNode.Type == FlowNodeType.Noop)
					{
						result = (int)flowNode.Cookie;
					}
					else if (fixedElement != null)
					{
						result = fixedElement.PageIndex;
					}
					else if (this.FixedTextBuilder.GetFixedPosition(fixedTextPointer.FlowPosition, fixedTextPointer.LogicalDirection, out fixedPosition))
					{
						result = fixedPosition.Page;
					}
				}
			}
			return result;
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x0023AC0C File Offset: 0x00239C0C
		internal void GetMultiHighlights(FixedTextPointer start, FixedTextPointer end, Dictionary<FixedPage, ArrayList> highlights, FixedHighlightType t, Brush foregroundBrush, Brush backgroundBrush)
		{
			if (start.CompareTo(end) > 0)
			{
				FixedTextPointer fixedTextPointer = start;
				start = end;
				end = fixedTextPointer;
			}
			int num = 0;
			int num2 = 0;
			FixedSOMElement[] array;
			if (this._GetFixedNodesForFlowRange(start, end, out array, out num, out num2))
			{
				for (int i = 0; i < array.Length; i++)
				{
					FixedSOMElement fixedSOMElement = array[i];
					FixedNode fixedNode = fixedSOMElement.FixedNode;
					FixedPage fixedPage = this.FixedDocument.SyncGetPageWithCheck(fixedNode.Page);
					if (fixedPage != null)
					{
						DependencyObject element = fixedPage.GetElement(fixedNode);
						if (element != null)
						{
							int num3 = 0;
							UIElement element2;
							int num4;
							if (element is Image || element is Path)
							{
								element2 = (UIElement)element;
								num4 = 1;
							}
							else
							{
								if (!(element is Glyphs))
								{
									goto IL_13A;
								}
								element2 = (UIElement)element;
								num3 = fixedSOMElement.StartIndex;
								num4 = fixedSOMElement.EndIndex;
							}
							if (i == 0)
							{
								num3 = num;
							}
							if (i == array.Length - 1)
							{
								num4 = num2;
							}
							ArrayList arrayList;
							if (highlights.ContainsKey(fixedPage))
							{
								arrayList = highlights[fixedPage];
							}
							else
							{
								arrayList = new ArrayList();
								highlights.Add(fixedPage, arrayList);
							}
							FixedSOMTextRun fixedSOMTextRun = fixedSOMElement as FixedSOMTextRun;
							if (fixedSOMTextRun != null && fixedSOMTextRun.IsReversed)
							{
								int num5 = num3;
								num3 = fixedSOMElement.EndIndex - num4;
								num4 = fixedSOMElement.EndIndex - num5;
							}
							FixedHighlight value = new FixedHighlight(element2, num3, num4, t, foregroundBrush, backgroundBrush);
							arrayList.Add(value);
						}
					}
					IL_13A:;
				}
			}
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x06004C17 RID: 19479 RVA: 0x0023AD60 File Offset: 0x00239D60
		internal FixedDocument FixedDocument
		{
			get
			{
				if (this._fixedPanel == null && this._parent is FixedDocument)
				{
					this._fixedPanel = (FixedDocument)this._parent;
				}
				return this._fixedPanel;
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x06004C18 RID: 19480 RVA: 0x0023AD8E File Offset: 0x00239D8E
		internal FixedTextBuilder FixedTextBuilder
		{
			get
			{
				return this._fixedTextBuilder;
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x0023AD96 File Offset: 0x00239D96
		internal FixedElement ContainerElement
		{
			get
			{
				return this._containerElement;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x0023AD9E File Offset: 0x00239D9E
		internal Highlights Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new Highlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x0023ADBA File Offset: 0x00239DBA
		internal ITextSelection TextSelection
		{
			get
			{
				return this._textSelection;
			}
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x0023ADC4 File Offset: 0x00239DC4
		private void _CreateEmptyContainer()
		{
			this._fixedTextBuilder = new FixedTextBuilder(this);
			this._start = new FixedTextPointer(false, LogicalDirection.Backward, new FlowPosition(this, this.FixedTextBuilder.FixedFlowMap.FlowStartEdge, 1));
			this._end = new FixedTextPointer(false, LogicalDirection.Forward, new FlowPosition(this, this.FixedTextBuilder.FixedFlowMap.FlowEndEdge, 0));
			this._containerElement = new FixedElement(FixedElement.ElementType.Container, this._start, this._end, int.MaxValue);
			this._start.FlowPosition.AttachElement(this._containerElement);
			this._end.FlowPosition.AttachElement(this._containerElement);
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x0023AE70 File Offset: 0x00239E70
		internal void OnNewFlowElement(FixedElement parentElement, FixedElement.ElementType elementType, FlowPosition pStart, FlowPosition pEnd, object source, int pageIndex)
		{
			FixedTextPointer start = new FixedTextPointer(false, LogicalDirection.Backward, pStart);
			FixedTextPointer end = new FixedTextPointer(false, LogicalDirection.Forward, pEnd);
			FixedElement fixedElement = new FixedElement(elementType, start, end, pageIndex);
			if (source != null)
			{
				fixedElement.Object = source;
			}
			parentElement.Append(fixedElement);
			pStart.AttachElement(fixedElement);
			pEnd.AttachElement(fixedElement);
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x0023AEC0 File Offset: 0x00239EC0
		private bool _GetFixedNodesForFlowRange(ITextPointer start, ITextPointer end, out FixedSOMElement[] elements, out int startIndex, out int endIndex)
		{
			elements = null;
			startIndex = 0;
			endIndex = 0;
			if (start.CompareTo(end) == 0)
			{
				return false;
			}
			FixedTextPointer fixedTextPointer = (FixedTextPointer)start;
			FixedTextPointer fixedTextPointer2 = (FixedTextPointer)end;
			return this.FixedTextBuilder.GetFixedNodesForFlowRange(fixedTextPointer.FlowPosition, fixedTextPointer2.FlowPosition, out elements, out startIndex, out endIndex);
		}

		// Token: 0x040027B8 RID: 10168
		private FixedDocument _fixedPanel;

		// Token: 0x040027B9 RID: 10169
		private FixedTextBuilder _fixedTextBuilder;

		// Token: 0x040027BA RID: 10170
		private DependencyObject _parent;

		// Token: 0x040027BB RID: 10171
		private FixedElement _containerElement;

		// Token: 0x040027BC RID: 10172
		private FixedTextPointer _start;

		// Token: 0x040027BD RID: 10173
		private FixedTextPointer _end;

		// Token: 0x040027BE RID: 10174
		private Highlights _highlights;

		// Token: 0x040027BF RID: 10175
		private ITextSelection _textSelection;

		// Token: 0x040027C0 RID: 10176
		private ITextView _textview;
	}
}
