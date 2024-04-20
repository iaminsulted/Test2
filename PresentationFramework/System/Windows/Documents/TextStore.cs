using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C7 RID: 1735
	internal class TextStore : UnsafeNativeMethods.ITextStoreACP, UnsafeNativeMethods.ITfThreadFocusSink, UnsafeNativeMethods.ITfContextOwnerCompositionSink, UnsafeNativeMethods.ITfTextEditSink, UnsafeNativeMethods.ITfTransitoryExtensionSink, UnsafeNativeMethods.ITfMouseTrackerACP
	{
		// Token: 0x06005A07 RID: 23047 RVA: 0x0027F24C File Offset: 0x0027E24C
		internal TextStore(TextEditor textEditor)
		{
			this._weakTextEditor = new TextStore.ScopeWeakReference(textEditor);
			this._threadFocusCookie = -1;
			this._editSinkCookie = -1;
			this._editCookie = -1;
			this._transitoryExtensionSinkCookie = -1;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x0027F29C File Offset: 0x0027E29C
		public void AdviseSink(ref Guid riid, object obj, UnsafeNativeMethods.AdviseFlags flags)
		{
			if (riid != UnsafeNativeMethods.IID_ITextStoreACPSink)
			{
				throw new COMException(SR.Get("TextStore_CONNECT_E_CANNOTCONNECT"), -2147220990);
			}
			UnsafeNativeMethods.ITextStoreACPSink textStoreACPSink = obj as UnsafeNativeMethods.ITextStoreACPSink;
			if (textStoreACPSink == null)
			{
				throw new COMException(SR.Get("TextStore_E_NOINTERFACE"), -2147467262);
			}
			if (this.HasSink)
			{
				Marshal.ReleaseComObject(this._sink);
			}
			else
			{
				this._textservicesHost.RegisterWinEventSink(this);
			}
			this._sink = textStoreACPSink;
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x0027F318 File Offset: 0x0027E318
		public void UnadviseSink(object obj)
		{
			if (obj != this._sink)
			{
				throw new COMException(SR.Get("TextStore_CONNECT_E_NOCONNECTION"), -2147220992);
			}
			Marshal.ReleaseComObject(this._sink);
			this._sink = null;
			this._textservicesHost.UnregisterWinEventSink(this);
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x0027F358 File Offset: 0x0027E358
		public void RequestLock(UnsafeNativeMethods.LockFlags flags, out int hrSession)
		{
			if (!this.HasSink)
			{
				throw new COMException(SR.Get("TextStore_NoSink"));
			}
			if (flags == (UnsafeNativeMethods.LockFlags)0)
			{
				throw new COMException(SR.Get("TextStore_BadLockFlags"));
			}
			if (this._lockFlags != (UnsafeNativeMethods.LockFlags)0)
			{
				if ((this._lockFlags & UnsafeNativeMethods.LockFlags.TS_LF_WRITE) == UnsafeNativeMethods.LockFlags.TS_LF_WRITE || (flags & UnsafeNativeMethods.LockFlags.TS_LF_WRITE) == (UnsafeNativeMethods.LockFlags)0 || (flags & UnsafeNativeMethods.LockFlags.TS_LF_SYNC) == UnsafeNativeMethods.LockFlags.TS_LF_SYNC)
				{
					throw new COMException(SR.Get("TextStore_ReentrantRequestLock"));
				}
				this._pendingWriteReq = true;
				hrSession = 262912;
				return;
			}
			else
			{
				if (this._textChangeReentrencyCount == 0)
				{
					hrSession = this.GrantLockWorker(flags);
					return;
				}
				if ((flags & UnsafeNativeMethods.LockFlags.TS_LF_SYNC) == (UnsafeNativeMethods.LockFlags)0)
				{
					if (this._pendingAsyncLockFlags == (UnsafeNativeMethods.LockFlags)0)
					{
						this._pendingAsyncLockFlags = flags;
						Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.GrantLockHandler), null);
					}
					else if ((flags & UnsafeNativeMethods.LockFlags.TS_LF_READWRITE & this._pendingAsyncLockFlags) != (flags & UnsafeNativeMethods.LockFlags.TS_LF_READWRITE))
					{
						this._pendingAsyncLockFlags = flags;
					}
					hrSession = 262912;
					return;
				}
				hrSession = -2147220984;
				return;
			}
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x0027F434 File Offset: 0x0027E434
		public void GetStatus(out UnsafeNativeMethods.TS_STATUS status)
		{
			if (this.IsTextEditorValid && this.IsReadOnly)
			{
				status.dynamicFlags = UnsafeNativeMethods.DynamicStatusFlags.TS_SD_READONLY;
			}
			else
			{
				status.dynamicFlags = (UnsafeNativeMethods.DynamicStatusFlags)0;
			}
			status.staticFlags = UnsafeNativeMethods.StaticStatusFlags.TS_SS_REGIONS;
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x0027F45D File Offset: 0x0027E45D
		public void QueryInsert(int startIndex, int endIndex, int cch, out int startResultIndex, out int endResultIndex)
		{
			startResultIndex = startIndex;
			endResultIndex = endIndex;
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x0027F468 File Offset: 0x0027E468
		public void GetSelection(int index, int count, UnsafeNativeMethods.TS_SELECTION_ACP[] selection, out int fetched)
		{
			fetched = 0;
			if (count > 0 && (index == 0 || index == -1))
			{
				selection[0].start = this.TextSelection.Start.CharOffset;
				selection[0].end = this.TextSelection.End.CharOffset;
				selection[0].style.ase = ((this.TextSelection.MovingPosition.CompareTo(this.TextSelection.Start) == 0) ? UnsafeNativeMethods.TsActiveSelEnd.TS_AE_START : UnsafeNativeMethods.TsActiveSelEnd.TS_AE_END);
				selection[0].style.interimChar = this._interimSelection;
				fetched = 1;
			}
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x0027F510 File Offset: 0x0027E510
		public void SetSelection(int count, UnsafeNativeMethods.TS_SELECTION_ACP[] selection)
		{
			if (count == 1)
			{
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.GetNormalizedRange(selection[0].start, selection[0].end, out textPointer, out textPointer2);
				if (selection[0].start == selection[0].end)
				{
					this.TextSelection.SetCaretToPosition(textPointer, LogicalDirection.Backward, true, true);
				}
				else if (selection[0].style.ase == UnsafeNativeMethods.TsActiveSelEnd.TS_AE_START)
				{
					this.TextSelection.Select(textPointer2, textPointer);
				}
				else
				{
					this.TextSelection.Select(textPointer, textPointer2);
				}
				bool interimSelection = this._interimSelection;
				this._interimSelection = selection[0].style.interimChar;
				if (interimSelection != this._interimSelection)
				{
					this.TextSelection.OnInterimSelectionChanged(this._interimSelection);
				}
			}
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x0027F5D8 File Offset: 0x0027E5D8
		public void GetText(int startIndex, int endIndex, char[] text, int cchReq, out int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, out int cRunInfoRcv, out int nextIndex)
		{
			charsCopied = 0;
			cRunInfoRcv = 0;
			nextIndex = startIndex;
			if (cchReq == 0 && cRunInfoReq == 0)
			{
				return;
			}
			if (startIndex == endIndex)
			{
				return;
			}
			ITextPointer textPointer = this.CreatePointerAtCharOffset(startIndex, LogicalDirection.Forward);
			ITextPointer textPointer2 = (endIndex >= 0) ? this.CreatePointerAtCharOffset(endIndex, LogicalDirection.Forward) : null;
			bool flag = false;
			while (!flag && (cchReq == 0 || cchReq > charsCopied) && (cRunInfoReq == 0 || cRunInfoReq > cRunInfoRcv))
			{
				switch (textPointer.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.None:
					flag = true;
					break;
				case TextPointerContext.Text:
					flag = TextStore.WalkTextRun(textPointer, textPointer2, text, cchReq, ref charsCopied, runInfo, cRunInfoReq, ref cRunInfoRcv);
					break;
				case TextPointerContext.EmbeddedElement:
					flag = TextStore.WalkObjectRun(textPointer, textPointer2, text, cchReq, ref charsCopied, runInfo, cRunInfoReq, ref cRunInfoRcv);
					break;
				case TextPointerContext.ElementStart:
				{
					Invariant.Assert(textPointer is TextPointer);
					TextElement textElement = (TextElement)((TextPointer)textPointer).GetAdjacentElement(LogicalDirection.Forward);
					if (textElement.IMELeftEdgeCharCount > 0)
					{
						Invariant.Assert(textElement.IMELeftEdgeCharCount == 1);
						flag = TextStore.WalkRegionBoundary(textPointer, textPointer2, text, cchReq, ref charsCopied, runInfo, cRunInfoReq, ref cRunInfoRcv);
					}
					else
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						flag = (textPointer2 != null && textPointer.CompareTo(textPointer2) >= 0);
					}
					break;
				}
				case TextPointerContext.ElementEnd:
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					flag = (textPointer2 != null && textPointer.CompareTo(textPointer2) >= 0);
					break;
				default:
					Invariant.Assert(false, "Bogus TextPointerContext!");
					break;
				}
			}
			nextIndex = textPointer.CharOffset;
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x0027F738 File Offset: 0x0027E738
		public void SetText(UnsafeNativeMethods.SetTextFlags flags, int startIndex, int endIndex, char[] text, int cch, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			ITextPointer nextInsertionPosition;
			ITextPointer textPointer;
			this.GetNormalizedRange(startIndex, endIndex, out nextInsertionPosition, out textPointer);
			while (nextInsertionPosition != null && TextPointerBase.IsBeforeFirstTable(nextInsertionPosition))
			{
				nextInsertionPosition = nextInsertionPosition.GetNextInsertionPosition(LogicalDirection.Forward);
			}
			if (nextInsertionPosition == null)
			{
				throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
			}
			if (nextInsertionPosition.CompareTo(textPointer) > 0)
			{
				textPointer = nextInsertionPosition;
			}
			string text2 = this.FilterCompositionString(new string(text), nextInsertionPosition.GetOffsetToPosition(textPointer));
			if (text2 == null)
			{
				throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
			}
			TextStore.CompositionParentUndoUnit textParentUndoUnit = this.OpenCompositionUndoUnit();
			UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
			try
			{
				ITextRange range = new TextRange(nextInsertionPosition, textPointer, true);
				this.TextEditor.SetText(range, text2, InputLanguageManager.Current.CurrentInputLanguage);
				change.start = startIndex;
				change.oldEnd = endIndex;
				change.newEnd = endIndex + text.Length - (endIndex - startIndex);
				this.ValidateChange(change);
				this.VerifyTextStoreConsistency();
				undoCloseAction = UndoCloseAction.Commit;
			}
			finally
			{
				this.CloseTextParentUndoUnit(textParentUndoUnit, undoCloseAction);
			}
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x0027F854 File Offset: 0x0027E854
		public void GetFormattedText(int startIndex, int endIndex, out object obj)
		{
			obj = null;
			throw new COMException(SR.Get("TextStore_E_NOTIMPL"), -2147467263);
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x0027F86D File Offset: 0x0027E86D
		public void GetEmbedded(int index, ref Guid guidService, ref Guid riid, out object obj)
		{
			obj = null;
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x0027F873 File Offset: 0x0027E873
		public void QueryInsertEmbedded(ref Guid guidService, int formatEtc, out bool insertable)
		{
			insertable = false;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x0027F878 File Offset: 0x0027E878
		public void InsertEmbedded(UnsafeNativeMethods.InsertEmbeddedFlags flags, int startIndex, int endIndex, object obj, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			throw new COMException(SR.Get("TextStore_TS_E_FORMAT"), -2147220982);
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x0027F8AC File Offset: 0x0027E8AC
		public void InsertTextAtSelection(UnsafeNativeMethods.InsertAtSelectionFlags flags, char[] text, int cch, out int startIndex, out int endIndex, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			startIndex = -1;
			endIndex = -1;
			change.start = 0;
			change.oldEnd = 0;
			change.newEnd = 0;
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			ITextRange textRange = new TextRange(this.TextSelection.AnchorPosition, this.TextSelection.MovingPosition);
			textRange.ApplyTypingHeuristics(false);
			ITextPointer textPointer;
			ITextPointer textPointer2;
			TextStore.GetAdjustedSelection(textRange.Start, textRange.End, out textPointer, out textPointer2);
			ITextPointer textPointer3 = textPointer.CreatePointer();
			textPointer3.SetLogicalDirection(LogicalDirection.Backward);
			ITextPointer textPointer4 = textPointer2.CreatePointer();
			textPointer4.SetLogicalDirection(LogicalDirection.Forward);
			int charOffset = textPointer3.CharOffset;
			int charOffset2 = textPointer4.CharOffset;
			if ((flags & UnsafeNativeMethods.InsertAtSelectionFlags.TS_IAS_QUERYONLY) == (UnsafeNativeMethods.InsertAtSelectionFlags)0)
			{
				TextStore.CompositionParentUndoUnit textParentUndoUnit = this.OpenCompositionUndoUnit();
				UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
				try
				{
					this.VerifyTextStoreConsistency();
					change.oldEnd = charOffset2;
					string text2 = this.FilterCompositionString(new string(text), textRange.Start.GetOffsetToPosition(textRange.End));
					if (text2 == null)
					{
						throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
					}
					this.TextSelection.ApplyTypingHeuristics(false);
					if (textPointer.CompareTo(this.TextSelection.Start) != 0 || textPointer2.CompareTo(this.TextSelection.End) != 0)
					{
						this.TextSelection.Select(textPointer, textPointer2);
					}
					if (!this._isComposing && this._previousCompositionStartOffset == -1)
					{
						this._previousCompositionStartOffset = this.TextSelection.Start.Offset;
						this._previousCompositionEndOffset = this.TextSelection.End.Offset;
					}
					this.TextEditor.SetSelectedText(text2, InputLanguageManager.Current.CurrentInputLanguage);
					change.start = textPointer3.CharOffset;
					change.newEnd = textPointer4.CharOffset;
					this.ValidateChange(change);
					this.VerifyTextStoreConsistency();
					undoCloseAction = UndoCloseAction.Commit;
				}
				finally
				{
					this.CloseTextParentUndoUnit(textParentUndoUnit, undoCloseAction);
				}
			}
			if ((flags & UnsafeNativeMethods.InsertAtSelectionFlags.TS_IAS_NOQUERY) == (UnsafeNativeMethods.InsertAtSelectionFlags)0)
			{
				startIndex = charOffset;
				endIndex = textPointer4.CharOffset;
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x0027FABC File Offset: 0x0027EABC
		public void InsertEmbeddedAtSelection(UnsafeNativeMethods.InsertAtSelectionFlags flags, object obj, out int startIndex, out int endIndex, out UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			startIndex = -1;
			endIndex = -1;
			change.start = 0;
			change.oldEnd = 0;
			change.newEnd = 0;
			if (this.IsReadOnly)
			{
				throw new COMException(SR.Get("TextStore_TS_E_READONLY"), -2147220983);
			}
			throw new COMException(SR.Get("TextStore_TS_E_FORMAT"), -2147220982);
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x0027FB1C File Offset: 0x0027EB1C
		public int RequestSupportedAttrs(UnsafeNativeMethods.AttributeFlags flags, int count, Guid[] filterAttributes)
		{
			this.PrepareAttributes((InputScope)this.UiScope.GetValue(InputMethod.InputScopeProperty), (double)this.UiScope.GetValue(TextElement.FontSizeProperty), (FontFamily)this.UiScope.GetValue(TextElement.FontFamilyProperty), (XmlLanguage)this.UiScope.GetValue(FrameworkContentElement.LanguageProperty), this.UiScope, count, filterAttributes);
			if (this._preparedattributes.Count == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x0027FB9C File Offset: 0x0027EB9C
		public int RequestAttrsAtPosition(int index, int count, Guid[] filterAttributes, UnsafeNativeMethods.AttributeFlags flags)
		{
			ITextPointer textPointer = this.CreatePointerAtCharOffset(index, LogicalDirection.Forward);
			this.PrepareAttributes((InputScope)textPointer.GetValue(InputMethod.InputScopeProperty), (double)textPointer.GetValue(TextElement.FontSizeProperty), (FontFamily)textPointer.GetValue(TextElement.FontFamilyProperty), (XmlLanguage)textPointer.GetValue(FrameworkContentElement.LanguageProperty), null, count, filterAttributes);
			if (this._preparedattributes.Count == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x0027FC0B File Offset: 0x0027EC0B
		public void RequestAttrsTransitioningAtPosition(int position, int count, Guid[] filterAttributes, UnsafeNativeMethods.AttributeFlags flags)
		{
			throw new COMException(SR.Get("TextStore_E_NOTIMPL"), -2147467263);
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x0027FC21 File Offset: 0x0027EC21
		public void FindNextAttrTransition(int startIndex, int haltIndex, int count, Guid[] filterAttributes, UnsafeNativeMethods.AttributeFlags flags, out int acpNext, out bool found, out int foundOffset)
		{
			acpNext = 0;
			found = false;
			foundOffset = 0;
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x0027FC30 File Offset: 0x0027EC30
		public void RetrieveRequestedAttrs(int count, UnsafeNativeMethods.TS_ATTRVAL[] attributeVals, out int fetched)
		{
			fetched = 0;
			int num = 0;
			while (num < count && num < this._preparedattributes.Count)
			{
				attributeVals[num] = (UnsafeNativeMethods.TS_ATTRVAL)this._preparedattributes[num];
				fetched++;
				num++;
			}
			this._preparedattributes.Clear();
			this._preparedattributes = null;
		}

		// Token: 0x06005A1C RID: 23068 RVA: 0x0027FC8A File Offset: 0x0027EC8A
		public void GetEnd(out int end)
		{
			end = this.TextContainer.IMECharCount;
		}

		// Token: 0x06005A1D RID: 23069 RVA: 0x0011EFC2 File Offset: 0x0011DFC2
		public void GetActiveView(out int viewCookie)
		{
			viewCookie = 0;
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x0027FC9C File Offset: 0x0027EC9C
		public void GetACPFromPoint(int viewCookie, ref UnsafeNativeMethods.POINT tsfPoint, UnsafeNativeMethods.GetPositionFromPointFlags flags, out int positionCP)
		{
			NativeMethods.POINT point = new NativeMethods.POINT(tsfPoint.x, tsfPoint.y);
			PresentationSource presentationSource;
			IWin32Window win32Window;
			ITextView textView;
			this.GetVisualInfo(out presentationSource, out win32Window, out textView);
			CompositionTarget compositionTarget = presentationSource.CompositionTarget;
			SafeNativeMethods.ScreenToClient(new HandleRef(null, win32Window.Handle), point);
			Point point2 = new Point((double)point.x, (double)point.y);
			point2 = compositionTarget.TransformFromDevice.Transform(point2);
			GeneralTransform generalTransform = compositionTarget.RootVisual.TransformToDescendant(this.RenderScope);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point2, out point2);
			}
			if (!textView.Validate(point2))
			{
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(point2, (flags & UnsafeNativeMethods.GetPositionFromPointFlags.GXFPF_NEAREST) > (UnsafeNativeMethods.GetPositionFromPointFlags)0);
			if (textPositionFromPoint == null)
			{
				throw new COMException(SR.Get("TextStore_TS_E_INVALIDPOINT"), -2147220985);
			}
			positionCP = textPositionFromPoint.CharOffset;
			if ((flags & UnsafeNativeMethods.GetPositionFromPointFlags.GXFPF_ROUND_NEAREST) == (UnsafeNativeMethods.GetPositionFromPointFlags)0)
			{
				ITextPointer position = textPositionFromPoint.CreatePointer(LogicalDirection.Backward);
				ITextPointer textPointer = textPositionFromPoint.CreatePointer(LogicalDirection.Forward);
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
				Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(position);
				Rect rectangleFromTextPosition2 = textView.GetRectangleFromTextPosition(textPointer);
				Point point3 = new Point(Math.Min(rectangleFromTextPosition2.Left, rectangleFromTextPosition.Left), Math.Min(rectangleFromTextPosition2.Top, rectangleFromTextPosition.Top));
				Point point4 = new Point(Math.Max(rectangleFromTextPosition2.Left, rectangleFromTextPosition.Left), Math.Max(rectangleFromTextPosition2.Bottom, rectangleFromTextPosition.Bottom));
				Rect rect = new Rect(point3, point4);
				if (rect.Contains(point2))
				{
					positionCP--;
				}
			}
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x0027FE2C File Offset: 0x0027EE2C
		void UnsafeNativeMethods.ITextStoreACP.GetTextExt(int viewCookie, int startIndex, int endIndex, out UnsafeNativeMethods.RECT rect, out bool clipped)
		{
			this._isInUpdateLayout = true;
			this.UiScope.UpdateLayout();
			this._isInUpdateLayout = false;
			if (this._hasTextChangedInUpdateLayout)
			{
				this._netCharCount = this.TextContainer.IMECharCount;
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			rect = default(UnsafeNativeMethods.RECT);
			clipped = false;
			PresentationSource presentationSource;
			IWin32Window win32Window;
			ITextView textView;
			this.GetVisualInfo(out presentationSource, out win32Window, out textView);
			CompositionTarget compositionTarget = presentationSource.CompositionTarget;
			ITextPointer textPointer = this.CreatePointerAtCharOffset(startIndex, LogicalDirection.Forward);
			textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
			if (!this.TextView.IsValid)
			{
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			Point topLeft;
			Point bottomRight;
			if (startIndex == endIndex)
			{
				Rect characterRect = textPointer.GetCharacterRect(LogicalDirection.Forward);
				topLeft = characterRect.TopLeft;
				bottomRight = characterRect.BottomRight;
			}
			else
			{
				Rect rect2 = new Rect(Size.Empty);
				ITextPointer textPointer2 = textPointer.CreatePointer();
				ITextPointer textPointer3 = this.CreatePointerAtCharOffset(endIndex, LogicalDirection.Backward);
				textPointer3.MoveToInsertionPosition(LogicalDirection.Backward);
				ITextPointer textPointer4;
				bool flag;
				do
				{
					TextSegment lineRange = this.TextView.GetLineRange(textPointer2);
					Rect rect3;
					if (!lineRange.IsNull)
					{
						ITextPointer start = (lineRange.Start.CompareTo(textPointer) <= 0) ? textPointer : lineRange.Start;
						textPointer4 = ((lineRange.End.CompareTo(textPointer3) >= 0) ? textPointer3 : lineRange.End);
						rect3 = TextStore.GetLineBounds(start, textPointer4);
						flag = (textPointer2.MoveToLineBoundary(1) != 0);
					}
					else
					{
						rect3 = textPointer2.GetCharacterRect(LogicalDirection.Forward);
						flag = textPointer2.MoveToNextInsertionPosition(LogicalDirection.Forward);
						textPointer4 = textPointer2;
					}
					if (!rect3.IsEmpty)
					{
						rect2.Union(rect3);
					}
				}
				while (textPointer4.CompareTo(textPointer3) != 0 && flag);
				topLeft = rect2.TopLeft;
				bottomRight = rect2.BottomRight;
			}
			GeneralTransform generalTransform = this.UiScope.TransformToAncestor(compositionTarget.RootVisual);
			generalTransform.TryTransform(topLeft, out topLeft);
			generalTransform.TryTransform(bottomRight, out bottomRight);
			rect = TextStore.TransformRootRectToScreenCoordinates(topLeft, bottomRight, win32Window, presentationSource);
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x00280014 File Offset: 0x0027F014
		public void GetScreenExt(int viewCookie, out UnsafeNativeMethods.RECT rect)
		{
			Rect visualContentBounds = this.UiScope.VisualContentBounds;
			Rect visualDescendantBounds = this.UiScope.VisualDescendantBounds;
			visualContentBounds.Union(visualDescendantBounds);
			PresentationSource presentationSource;
			IWin32Window win32Window;
			ITextView textView;
			this.GetVisualInfo(out presentationSource, out win32Window, out textView);
			CompositionTarget compositionTarget = presentationSource.CompositionTarget;
			Point point = new Point(visualContentBounds.Left, visualContentBounds.Top);
			Point point2 = new Point(visualContentBounds.Right, visualContentBounds.Bottom);
			GeneralTransform generalTransform = this.UiScope.TransformToAncestor(compositionTarget.RootVisual);
			generalTransform.TryTransform(point, out point);
			generalTransform.TryTransform(point2, out point2);
			rect = TextStore.TransformRootRectToScreenCoordinates(point, point2, win32Window, presentationSource);
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x002800B8 File Offset: 0x0027F0B8
		void UnsafeNativeMethods.ITextStoreACP.GetWnd(int viewCookie, out IntPtr hwnd)
		{
			hwnd = IntPtr.Zero;
			hwnd = this.CriticalSourceWnd;
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x002800C9 File Offset: 0x0027F0C9
		void UnsafeNativeMethods.ITfThreadFocusSink.OnSetThreadFocus()
		{
			if (!this.IsTextEditorValid)
			{
				return;
			}
			if (Keyboard.FocusedElement == this.UiScope)
			{
				this.OnGotFocus();
			}
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public void OnKillThreadFocus()
		{
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x002800E8 File Offset: 0x0027F0E8
		public void OnStartComposition(UnsafeNativeMethods.ITfCompositionView view, out bool ok)
		{
			if (this._isComposing)
			{
				ok = false;
				return;
			}
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.GetCompositionPositions(view, out textPointer, out textPointer2);
			int startOffsetBefore = textPointer.Offset;
			int endOffsetBefore = textPointer2.Offset;
			this._lastCompositionText = TextRangeBase.GetTextInternal(textPointer, textPointer2);
			if (this._previousCompositionStartOffset != -1)
			{
				startOffsetBefore = this._previousCompositionStartOffset;
				endOffsetBefore = this._previousCompositionEndOffset;
			}
			else if (this.TextEditor.AcceptsRichContent && textPointer.CompareTo(textPointer2) != 0)
			{
				TextElement element = (TextElement)((TextPointer)textPointer).Parent;
				TextElement element2 = (TextElement)((TextPointer)textPointer2).Parent;
				TextElement commonAncestor = TextElement.GetCommonAncestor(element, element2);
				int imecharCount = this.TextContainer.IMECharCount;
				TextRange textRange = new TextRange(textPointer, textPointer2);
				string text = textRange.Text;
				if (commonAncestor is Run)
				{
					this.TextEditor.MarkCultureProperty(textRange, InputLanguageManager.Current.CurrentInputLanguage);
				}
				else if (commonAncestor is Paragraph || commonAncestor is Span)
				{
					this.TextEditor.SetText(textRange, text, InputLanguageManager.Current.CurrentInputLanguage);
				}
				Invariant.Assert(textRange.Text == text);
				Invariant.Assert(imecharCount == this.TextContainer.IMECharCount);
			}
			this.CompositionEventList.Add(new TextStore.CompositionEventRecord(TextStore.CompositionStage.StartComposition, startOffsetBefore, endOffsetBefore, this._lastCompositionText));
			this._previousCompositionStartOffset = textPointer.Offset;
			this._previousCompositionEndOffset = textPointer2.Offset;
			this._isComposing = true;
			this.BreakTypingSequence(textPointer2);
			ok = true;
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x0028025C File Offset: 0x0027F25C
		public void OnUpdateComposition(UnsafeNativeMethods.ITfCompositionView view, UnsafeNativeMethods.ITfRange rangeNew)
		{
			this.TextEditor.CloseToolTip();
			Invariant.Assert(this._isComposing);
			Invariant.Assert(this._previousCompositionStartOffset != -1);
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.GetCompositionPositions(view, out textPointer, out textPointer2);
			ITextPointer textPointer3 = null;
			ITextPointer textPointer4 = null;
			bool flag = false;
			if (rangeNew != null)
			{
				this.TextPositionsFromITfRange(rangeNew, out textPointer3, out textPointer4);
				flag = (textPointer3.Offset != textPointer.Offset || textPointer4.Offset != textPointer2.Offset);
			}
			string textInternal = TextRangeBase.GetTextInternal(textPointer, textPointer2);
			if (flag)
			{
				TextStore.CompositionEventRecord item = new TextStore.CompositionEventRecord(TextStore.CompositionStage.UpdateComposition, this._previousCompositionStartOffset, this._previousCompositionEndOffset, textInternal, true);
				this.CompositionEventList.Add(item);
				this._previousCompositionStartOffset = textPointer3.Offset;
				this._previousCompositionEndOffset = textPointer4.Offset;
				this._lastCompositionText = null;
			}
			else
			{
				TextStore.CompositionEventRecord item2 = new TextStore.CompositionEventRecord(TextStore.CompositionStage.UpdateComposition, this._previousCompositionStartOffset, this._previousCompositionEndOffset, textInternal);
				if (this.CompositionEventList.Count != 0)
				{
					TextStore.CompositionEventRecord compositionEventRecord = this.CompositionEventList[this.CompositionEventList.Count - 1];
				}
				if (this._lastCompositionText == null || string.CompareOrdinal(textInternal, this._lastCompositionText) != 0)
				{
					this.CompositionEventList.Add(item2);
				}
				this._previousCompositionStartOffset = textPointer.Offset;
				this._previousCompositionEndOffset = textPointer2.Offset;
				this._lastCompositionText = textInternal;
			}
			this.BreakTypingSequence(textPointer2);
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x002803B0 File Offset: 0x0027F3B0
		public void OnEndComposition(UnsafeNativeMethods.ITfCompositionView view)
		{
			Invariant.Assert(this._isComposing);
			Invariant.Assert(this._previousCompositionStartOffset != -1);
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.GetCompositionPositions(view, out textPointer, out textPointer2);
			if (this._compositionEventState == TextStore.CompositionEventState.NotRaisingEvents)
			{
				this.CompositionEventList.Add(new TextStore.CompositionEventRecord(TextStore.CompositionStage.EndComposition, textPointer.Offset, textPointer2.Offset, TextRangeBase.GetTextInternal(textPointer, textPointer2)));
				TextStore.CompositionParentUndoUnit compositionParentUndoUnit = this.PeekCompositionParentUndoUnit();
				if (compositionParentUndoUnit != null)
				{
					compositionParentUndoUnit.IsLastCompositionUnit = true;
				}
			}
			this._nextUndoUnitIsFirstCompositionUnit = true;
			this._isComposing = false;
			this._previousCompositionStartOffset = -1;
			this._previousCompositionEndOffset = -1;
			if (this._interimSelection)
			{
				this._interimSelection = false;
				this.TextSelection.OnInterimSelectionChanged(this._interimSelection);
			}
			this.BreakTypingSequence(textPointer2);
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x00280464 File Offset: 0x0027F464
		void UnsafeNativeMethods.ITfTextEditSink.OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			this._textservicesproperty.OnEndEdit(context, ecReadOnly, editRecord);
			Marshal.ReleaseComObject(editRecord);
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x0028047C File Offset: 0x0027F47C
		public void OnTransitoryExtensionUpdated(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfRange rangeResult, UnsafeNativeMethods.ITfRange rangeComposition, out bool fDeleteResultRange)
		{
			fDeleteResultRange = true;
			if (rangeResult != null)
			{
				string text = TextStore.StringFromITfRange(rangeResult, ecReadOnly);
				if (text.Length > 0)
				{
					if (this.TextEditor.AllowOvertype && this.TextEditor._OvertypeMode && this.TextSelection.IsEmpty)
					{
						ITextPointer textPointer = this.TextSelection.End.CreatePointer();
						textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
						if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
						{
							char[] array = new char[2];
							textPointer.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
							if (array[0] != Environment.NewLine[0] || array[1] != Environment.NewLine[1])
							{
								int length = text.Length;
								while (length-- > 0)
								{
									this.TextSelection.ExtendToNextInsertionPosition(LogicalDirection.Forward);
								}
							}
						}
					}
					string text2 = this.FilterCompositionString(text, this.TextSelection.Start.GetOffsetToPosition(this.TextSelection.End));
					if (text2 == null)
					{
						throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
					}
					this.TextEditor.SetText(this.TextSelection, text2, InputLanguageManager.Current.CurrentInputLanguage);
					this.TextSelection.Select(this.TextSelection.End, this.TextSelection.End);
				}
			}
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x002805C4 File Offset: 0x0027F5C4
		public int AdviceMouseSink(UnsafeNativeMethods.ITfRangeACP range, UnsafeNativeMethods.ITfMouseSink sink, out int dwCookie)
		{
			if (this._mouseSinks == null)
			{
				this._mouseSinks = new ArrayList(1);
			}
			this._mouseSinks.Sort();
			dwCookie = 0;
			while (dwCookie < this._mouseSinks.Count && ((TextStore.MouseSink)this._mouseSinks[dwCookie]).Cookie == dwCookie)
			{
				dwCookie++;
			}
			Invariant.Assert(dwCookie != -1);
			this._mouseSinks.Add(new TextStore.MouseSink(range, sink, dwCookie));
			if (this._mouseSinks.Count == 1)
			{
				this.UiScope.PreviewMouseLeftButtonDown += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseLeftButtonUp += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseRightButtonDown += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseRightButtonUp += this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseMove += this.OnMouseEvent;
			}
			return 0;
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x002806CC File Offset: 0x0027F6CC
		public int UnadviceMouseSink(int dwCookie)
		{
			int result = -2147024809;
			for (int i = 0; i < this._mouseSinks.Count; i++)
			{
				TextStore.MouseSink mouseSink = (TextStore.MouseSink)this._mouseSinks[i];
				if (mouseSink.Cookie == dwCookie)
				{
					this._mouseSinks.RemoveAt(i);
					if (this._mouseSinks.Count == 0)
					{
						this.UiScope.PreviewMouseLeftButtonDown -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseLeftButtonUp -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseRightButtonDown -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseRightButtonUp -= this.OnMouseButtonEvent;
						this.UiScope.PreviewMouseMove -= this.OnMouseEvent;
					}
					if (mouseSink.Locked)
					{
						mouseSink.PendingDispose = true;
					}
					else
					{
						mouseSink.Dispose();
					}
					result = 0;
					break;
				}
			}
			return result;
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x002807C4 File Offset: 0x0027F7C4
		internal void OnAttach()
		{
			this._netCharCount = this.TextContainer.IMECharCount;
			this._textservicesHost = TextServicesHost.Current;
			this._textservicesHost.RegisterTextStore(this);
			this.TextContainer.Change += this.OnTextContainerChange;
			this._textservicesproperty = new TextServicesProperty(this);
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x0028081C File Offset: 0x0027F81C
		internal void OnDetach(bool finalizer)
		{
			if (this.IsTextEditorValid)
			{
				this.TextContainer.Change -= this.OnTextContainerChange;
			}
			this._textservicesHost.UnregisterTextStore(this, finalizer);
			this._textservicesproperty = null;
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x00280854 File Offset: 0x0027F854
		internal void OnGotFocus()
		{
			if ((bool)this.UiScope.GetValue(InputMethod.IsInputMethodEnabledProperty))
			{
				this._textservicesHost.ThreadManager.SetFocus(this.DocumentManager);
			}
			if (this._makeLayoutChangeOnGotFocus)
			{
				this.OnLayoutUpdated();
				this._makeLayoutChangeOnGotFocus = false;
			}
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x002808A3 File Offset: 0x0027F8A3
		internal void OnLostFocus()
		{
			this.CompleteComposition();
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x002808AB File Offset: 0x0027F8AB
		internal void OnLayoutUpdated()
		{
			if (this.HasSink)
			{
				this._sink.OnLayoutChange(UnsafeNativeMethods.TsLayoutCode.TS_LC_CHANGE, 0);
			}
			if (this._textservicesproperty != null)
			{
				this._textservicesproperty.OnLayoutUpdated();
			}
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x002808D5 File Offset: 0x0027F8D5
		internal void OnSelectionChange()
		{
			this._compositionModifiedByEventListener = true;
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x002808DE File Offset: 0x0027F8DE
		internal void OnSelectionChanged()
		{
			if (this._compositionEventState == TextStore.CompositionEventState.RaisingEvents)
			{
				return;
			}
			if (this._ignoreNextSelectionChange)
			{
				this._ignoreNextSelectionChange = false;
				return;
			}
			if (this.HasSink)
			{
				this._sink.OnSelectionChange();
			}
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x00280910 File Offset: 0x0027F910
		internal bool QueryRangeOrReconvertSelection(bool fDoReconvert)
		{
			if (this._isComposing && !fDoReconvert)
			{
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.GetCompositionPositions(out textPointer, out textPointer2);
				if (textPointer != null && textPointer2 != null && textPointer.CompareTo(this.TextSelection.Start) <= 0 && textPointer.CompareTo(this.TextSelection.End) <= 0 && textPointer2.CompareTo(this.TextSelection.Start) >= 0 && textPointer2.CompareTo(this.TextSelection.End) >= 0)
				{
					return true;
				}
			}
			UnsafeNativeMethods.ITfFnReconversion tfFnReconversion;
			UnsafeNativeMethods.ITfRange tfRange;
			bool fnReconv = this.GetFnReconv(this.TextSelection.Start, this.TextSelection.End, out tfFnReconversion, out tfRange);
			if (tfFnReconversion != null)
			{
				if (fDoReconvert)
				{
					tfFnReconversion.Reconvert(tfRange);
				}
				Marshal.ReleaseComObject(tfFnReconversion);
			}
			if (tfRange != null)
			{
				Marshal.ReleaseComObject(tfRange);
			}
			return fnReconv;
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x002809CC File Offset: 0x0027F9CC
		internal UnsafeNativeMethods.ITfCandidateList GetReconversionCandidateList()
		{
			UnsafeNativeMethods.ITfCandidateList result = null;
			UnsafeNativeMethods.ITfFnReconversion tfFnReconversion;
			UnsafeNativeMethods.ITfRange tfRange;
			this.GetFnReconv(this.TextSelection.Start, this.TextSelection.End, out tfFnReconversion, out tfRange);
			if (tfFnReconversion != null)
			{
				tfFnReconversion.GetReconversion(tfRange, out result);
				Marshal.ReleaseComObject(tfFnReconversion);
			}
			if (tfRange != null)
			{
				Marshal.ReleaseComObject(tfRange);
			}
			return result;
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x00280A1C File Offset: 0x0027FA1C
		private bool GetFnReconv(ITextPointer textStart, ITextPointer textEnd, out UnsafeNativeMethods.ITfFnReconversion funcReconv, out UnsafeNativeMethods.ITfRange rangeNew)
		{
			bool flag = false;
			funcReconv = null;
			rangeNew = null;
			UnsafeNativeMethods.ITfContext tfContext;
			this.DocumentManager.GetBase(out tfContext);
			UnsafeNativeMethods.ITfRange tfRange;
			tfContext.GetStart(this.EditCookie, out tfRange);
			UnsafeNativeMethods.ITfRangeACP tfRangeACP = tfRange as UnsafeNativeMethods.ITfRangeACP;
			int charOffset = textStart.CharOffset;
			int charOffset2 = textEnd.CharOffset;
			tfRangeACP.SetExtent(charOffset, charOffset2 - charOffset);
			Guid guid_SYSTEM_FUNCTIONPROVIDER = UnsafeNativeMethods.GUID_SYSTEM_FUNCTIONPROVIDER;
			Guid guid_Null = UnsafeNativeMethods.Guid_Null;
			Guid iid_ITfFnReconversion = UnsafeNativeMethods.IID_ITfFnReconversion;
			UnsafeNativeMethods.ITfFunctionProvider tfFunctionProvider;
			this._textservicesHost.ThreadManager.GetFunctionProvider(ref guid_SYSTEM_FUNCTIONPROVIDER, out tfFunctionProvider);
			object obj;
			tfFunctionProvider.GetFunction(ref guid_Null, ref iid_ITfFnReconversion, out obj);
			funcReconv = (obj as UnsafeNativeMethods.ITfFnReconversion);
			funcReconv.QueryRange(tfRange, out rangeNew, out flag);
			Marshal.ReleaseComObject(tfFunctionProvider);
			if (!flag)
			{
				Marshal.ReleaseComObject(funcReconv);
				funcReconv = null;
			}
			Marshal.ReleaseComObject(tfRange);
			Marshal.ReleaseComObject(tfContext);
			return flag;
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x00280ADF File Offset: 0x0027FADF
		internal void CompleteCompositionAsync()
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.CompleteCompositionHandler), null);
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x00280AFB File Offset: 0x0027FAFB
		internal void CompleteComposition()
		{
			if (this._isComposing)
			{
				FrameworkTextComposition.CompleteCurrentComposition(this.DocumentManager);
			}
			this._previousCompositionStartOffset = -1;
			this._previousCompositionEndOffset = -1;
			this._previousCompositionStart = null;
			this._previousCompositionEnd = null;
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x00280B2C File Offset: 0x0027FB2C
		internal ITextPointer CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			this.ValidateCharOffset(charOffset);
			ITextPointer textPointer = this.TextContainer.CreatePointerAtCharOffset(charOffset, direction);
			if (textPointer == null)
			{
				textPointer = this.TextSelection.Start.CreatePointer(direction);
			}
			return textPointer;
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x00280B64 File Offset: 0x0027FB64
		internal void MakeLayoutChangeOnGotFocus()
		{
			if (this._isComposing)
			{
				this._makeLayoutChangeOnGotFocus = true;
			}
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x00280B78 File Offset: 0x0027FB78
		internal void UpdateCompositionText(FrameworkTextComposition composition)
		{
			if (this._compositionModifiedByEventListener)
			{
				return;
			}
			this._handledByTextStoreListener = true;
			bool compositionModifiedByEventListener = false;
			ITextRange textRange;
			string text;
			if (composition._ResultStart != null)
			{
				textRange = new TextRange(composition._ResultStart, composition._ResultEnd, true);
				text = this.TextEditor._FilterText(composition.Text, textRange);
				if (text.Length != composition.Text.Length)
				{
					compositionModifiedByEventListener = true;
				}
			}
			else
			{
				textRange = new TextRange(composition._CompositionStart, composition._CompositionEnd, true);
				text = this.TextEditor._FilterText(composition.CompositionText, textRange, false);
				Invariant.Assert(text.Length == composition.CompositionText.Length);
			}
			this._nextUndoUnitIsFirstCompositionUnit = false;
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit = this.PeekCompositionParentUndoUnit();
			if (compositionParentUndoUnit != null)
			{
				compositionParentUndoUnit.IsLastCompositionUnit = false;
			}
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit2 = this.OpenCompositionUndoUnit(textRange.Start, textRange.End);
			UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
			if (composition._ResultStart != null)
			{
				this._nextUndoUnitIsFirstCompositionUnit = true;
				compositionParentUndoUnit2.IsLastCompositionUnit = true;
			}
			this.TextSelection.BeginChange();
			try
			{
				this.TextEditor.SetText(textRange, text, InputLanguageManager.Current.CurrentInputLanguage);
				if (this._interimSelection)
				{
					this.TextSelection.Select(textRange.Start, textRange.End);
				}
				else
				{
					this.TextSelection.SetCaretToPosition(textRange.End, LogicalDirection.Backward, true, true);
				}
				compositionParentUndoUnit2.RecordRedoSelectionState(textRange.End, textRange.End);
				undoCloseAction = UndoCloseAction.Commit;
			}
			finally
			{
				this._compositionModifiedByEventListener = compositionModifiedByEventListener;
				this.TextSelection.EndChange();
				this.CloseTextParentUndoUnit(compositionParentUndoUnit2, undoCloseAction);
			}
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x00280D00 File Offset: 0x0027FD00
		internal static FrameworkTextComposition CreateComposition(TextEditor editor, object owner)
		{
			FrameworkTextComposition result;
			if (editor.AcceptsRichContent)
			{
				result = new FrameworkRichTextComposition(InputManager.UnsecureCurrent, editor.UiScope, owner);
			}
			else
			{
				result = new FrameworkTextComposition(InputManager.Current, editor.UiScope, owner);
			}
			return result;
		}

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x06005A3B RID: 23099 RVA: 0x00280D3C File Offset: 0x0027FD3C
		internal UIElement RenderScope
		{
			get
			{
				if (this.TextEditor == null)
				{
					return null;
				}
				if (this.TextEditor.TextView == null)
				{
					return null;
				}
				return this.TextEditor.TextView.RenderScope;
			}
		}

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x06005A3C RID: 23100 RVA: 0x00280D67 File Offset: 0x0027FD67
		internal FrameworkElement UiScope
		{
			get
			{
				return this.TextEditor.UiScope;
			}
		}

		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x06005A3D RID: 23101 RVA: 0x00280D74 File Offset: 0x0027FD74
		internal ITextContainer TextContainer
		{
			get
			{
				return this.TextEditor.TextContainer;
			}
		}

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x06005A3E RID: 23102 RVA: 0x00280D81 File Offset: 0x0027FD81
		internal ITextView TextView
		{
			get
			{
				return this.TextEditor.TextView;
			}
		}

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x06005A3F RID: 23103 RVA: 0x00280D8E File Offset: 0x0027FD8E
		// (set) Token: 0x06005A40 RID: 23104 RVA: 0x00280DA5 File Offset: 0x0027FDA5
		internal UnsafeNativeMethods.ITfDocumentMgr DocumentManager
		{
			get
			{
				if (this._documentmanager == null)
				{
					return null;
				}
				return this._documentmanager.Value;
			}
			set
			{
				this._documentmanager = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfDocumentMgr>(value);
			}
		}

		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x06005A41 RID: 23105 RVA: 0x00280DB3 File Offset: 0x0027FDB3
		// (set) Token: 0x06005A42 RID: 23106 RVA: 0x00280DBB File Offset: 0x0027FDBB
		internal int ThreadFocusCookie
		{
			get
			{
				return this._threadFocusCookie;
			}
			set
			{
				this._threadFocusCookie = value;
			}
		}

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x06005A43 RID: 23107 RVA: 0x00280DC4 File Offset: 0x0027FDC4
		// (set) Token: 0x06005A44 RID: 23108 RVA: 0x00280DCC File Offset: 0x0027FDCC
		internal int EditSinkCookie
		{
			get
			{
				return this._editSinkCookie;
			}
			set
			{
				this._editSinkCookie = value;
			}
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x06005A45 RID: 23109 RVA: 0x00280DD5 File Offset: 0x0027FDD5
		// (set) Token: 0x06005A46 RID: 23110 RVA: 0x00280DDD File Offset: 0x0027FDDD
		internal int EditCookie
		{
			get
			{
				return this._editCookie;
			}
			set
			{
				this._editCookie = value;
			}
		}

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x06005A47 RID: 23111 RVA: 0x00280DE6 File Offset: 0x0027FDE6
		internal bool IsInterimSelection
		{
			get
			{
				return this._interimSelection;
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x06005A48 RID: 23112 RVA: 0x00280DEE File Offset: 0x0027FDEE
		internal bool IsComposing
		{
			get
			{
				return this._isComposing;
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x06005A49 RID: 23113 RVA: 0x00280DF6 File Offset: 0x0027FDF6
		internal bool IsEffectivelyComposing
		{
			get
			{
				return this._isComposing && !this._isEffectivelyNotComposing;
			}
		}

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x06005A4A RID: 23114 RVA: 0x00280E0B File Offset: 0x0027FE0B
		// (set) Token: 0x06005A4B RID: 23115 RVA: 0x00280E13 File Offset: 0x0027FE13
		internal int TransitoryExtensionSinkCookie
		{
			get
			{
				return this._transitoryExtensionSinkCookie;
			}
			set
			{
				this._transitoryExtensionSinkCookie = value;
			}
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x06005A4C RID: 23116 RVA: 0x00280E1C File Offset: 0x0027FE1C
		internal IntPtr CriticalSourceWnd
		{
			get
			{
				bool callerIsTrusted = true;
				return this.GetSourceWnd(callerIsTrusted);
			}
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x00280E34 File Offset: 0x0027FE34
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			if (args.IMECharCount > 0 && (args.TextChange == TextChangeType.ContentAdded || args.TextChange == TextChangeType.ContentRemoved))
			{
				this._compositionModifiedByEventListener = true;
			}
			if (this._compositionEventState == TextStore.CompositionEventState.RaisingEvents)
			{
				return;
			}
			Invariant.Assert(sender == this.TextContainer);
			if (this._lockFlags == (UnsafeNativeMethods.LockFlags)0 && this.HasSink)
			{
				int num = 0;
				int num2 = 0;
				if (args.TextChange == TextChangeType.ContentAdded)
				{
					num = args.IMECharCount;
				}
				else if (args.TextChange == TextChangeType.ContentRemoved)
				{
					num2 = args.IMECharCount;
				}
				if (num <= 0 && num2 <= 0)
				{
					return;
				}
				UnsafeNativeMethods.TS_TEXTCHANGE ts_TEXTCHANGE;
				ts_TEXTCHANGE.start = args.ITextPosition.CharOffset;
				ts_TEXTCHANGE.oldEnd = ts_TEXTCHANGE.start + num2;
				ts_TEXTCHANGE.newEnd = ts_TEXTCHANGE.start + num;
				this.ValidateChange(ts_TEXTCHANGE);
				try
				{
					this._textChangeReentrencyCount++;
					this._sink.OnTextChange((UnsafeNativeMethods.OnTextChangeFlags)0, ref ts_TEXTCHANGE);
					return;
				}
				finally
				{
					this._textChangeReentrencyCount--;
				}
			}
			if (this._isInUpdateLayout)
			{
				this._hasTextChangedInUpdateLayout = true;
			}
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x00280F40 File Offset: 0x0027FF40
		private object GrantLockHandler(object o)
		{
			if (this._textservicesHost != null && this.HasSink)
			{
				this.GrantLockWorker(this._pendingAsyncLockFlags);
			}
			this._pendingAsyncLockFlags = (UnsafeNativeMethods.LockFlags)0;
			return null;
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x06005A4F RID: 23119 RVA: 0x00280F67 File Offset: 0x0027FF67
		private bool HasSink
		{
			get
			{
				return this._sink != null;
			}
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x00280F74 File Offset: 0x0027FF74
		private int GrantLockWorker(UnsafeNativeMethods.LockFlags flags)
		{
			TextEditor textEditor = this.TextEditor;
			int result;
			if (textEditor == null)
			{
				result = -2147467259;
			}
			else
			{
				this._lockFlags = flags;
				this._hasTextChangedInUpdateLayout = false;
				UndoManager undoManager = UndoManager.GetUndoManager(textEditor.TextContainer.Parent);
				int previousUndoCount = 0;
				bool isImeSupportModeEnabled = false;
				if (undoManager != null)
				{
					previousUndoCount = undoManager.UndoCount;
					isImeSupportModeEnabled = undoManager.IsImeSupportModeEnabled;
					undoManager.IsImeSupportModeEnabled = true;
				}
				this._previousCompositionStartOffset = ((this._previousCompositionStart == null) ? -1 : this._previousCompositionStart.Offset);
				this._previousCompositionEndOffset = ((this._previousCompositionEnd == null) ? -1 : this._previousCompositionEnd.Offset);
				try
				{
					textEditor.Selection.BeginChangeNoUndo();
					try
					{
						result = this.GrantLock();
						if (this._pendingWriteReq)
						{
							this._lockFlags = UnsafeNativeMethods.LockFlags.TS_LF_READWRITE;
							this.GrantLock();
						}
					}
					finally
					{
						this._pendingWriteReq = false;
						this._lockFlags = (UnsafeNativeMethods.LockFlags)0;
						this._ignoreNextSelectionChange = textEditor.Selection._IsChanged;
						try
						{
							textEditor.Selection.EndChange(false, true);
						}
						finally
						{
							this._ignoreNextSelectionChange = false;
							this._previousCompositionStart = ((this._previousCompositionStartOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionStartOffset, LogicalDirection.Backward));
							this._previousCompositionEnd = ((this._previousCompositionEndOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionEndOffset, LogicalDirection.Forward));
						}
					}
					if (undoManager != null)
					{
						this.HandleCompositionEvents(previousUndoCount);
					}
				}
				finally
				{
					if (undoManager != null)
					{
						undoManager.IsImeSupportModeEnabled = isImeSupportModeEnabled;
					}
					this._previousCompositionStart = ((this._previousCompositionStartOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionStartOffset, LogicalDirection.Backward));
					this._previousCompositionEnd = ((this._previousCompositionEndOffset == -1) ? null : textEditor.TextContainer.CreatePointerAtOffset(this._previousCompositionEndOffset, LogicalDirection.Forward));
				}
			}
			return result;
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x00281140 File Offset: 0x00280140
		private int GrantLock()
		{
			Invariant.Assert(Thread.CurrentThread == this._textservicesHost.Dispatcher.Thread, "GrantLock called on bad thread!");
			this.VerifyTextStoreConsistency();
			int result = this._sink.OnLockGranted(this._lockFlags);
			this.VerifyTextStoreConsistency();
			return result;
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x00281180 File Offset: 0x00280180
		private static bool WalkTextRun(ITextPointer navigator, ITextPointer limit, char[] text, int cchReq, ref int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, ref int cRunInfoRcv)
		{
			Invariant.Assert(navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
			Invariant.Assert(limit == null || navigator.CompareTo(limit) <= 0);
			bool result = false;
			int num;
			if (cchReq > 0)
			{
				num = TextPointerBase.GetTextWithLimit(navigator, LogicalDirection.Forward, text, charsCopied, Math.Min(cchReq, text.Length - charsCopied), limit);
				navigator.MoveByOffset(num);
				charsCopied += num;
				result = (text.Length == charsCopied || (limit != null && navigator.CompareTo(limit) == 0));
			}
			else
			{
				num = navigator.GetTextRunLength(LogicalDirection.Forward);
				navigator.MoveToNextContextPosition(LogicalDirection.Forward);
				if (limit != null && navigator.CompareTo(limit) >= 0)
				{
					int offsetToPosition = limit.GetOffsetToPosition(navigator);
					Invariant.Assert(offsetToPosition >= 0 && offsetToPosition <= num, "Bogus offset -- extends past run!");
					num -= offsetToPosition;
					navigator.MoveToPosition(limit);
					result = true;
				}
			}
			if (cRunInfoReq > 0 && num > 0)
			{
				if (cRunInfoRcv > 0 && runInfo[cRunInfoRcv - 1].type == UnsafeNativeMethods.TsRunType.TS_RT_PLAIN)
				{
					int num2 = cRunInfoRcv - 1;
					runInfo[num2].count = runInfo[num2].count + num;
				}
				else
				{
					runInfo[cRunInfoRcv].count = num;
					runInfo[cRunInfoRcv].type = UnsafeNativeMethods.TsRunType.TS_RT_PLAIN;
					cRunInfoRcv++;
				}
			}
			return result;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x002812B0 File Offset: 0x002802B0
		private static bool WalkObjectRun(ITextPointer navigator, ITextPointer limit, char[] text, int cchReq, ref int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, ref int cRunInfoRcv)
		{
			Invariant.Assert(navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement);
			Invariant.Assert(limit == null || navigator.CompareTo(limit) <= 0);
			if (limit != null && navigator.CompareTo(limit) == 0)
			{
				return true;
			}
			int result = 0;
			navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			if (cchReq >= 1)
			{
				text[charsCopied] = '￼';
				charsCopied++;
			}
			if (cRunInfoReq > 0)
			{
				if (cRunInfoRcv > 0 && runInfo[cRunInfoRcv - 1].type == UnsafeNativeMethods.TsRunType.TS_RT_PLAIN)
				{
					int num = cRunInfoRcv - 1;
					runInfo[num].count = runInfo[num].count + 1;
					return result != 0;
				}
				runInfo[cRunInfoRcv].count = 1;
				runInfo[cRunInfoRcv].type = UnsafeNativeMethods.TsRunType.TS_RT_PLAIN;
				cRunInfoRcv++;
			}
			return result != 0;
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x00281370 File Offset: 0x00280370
		private static bool WalkRegionBoundary(ITextPointer navigator, ITextPointer limit, char[] text, int cchReq, ref int charsCopied, UnsafeNativeMethods.TS_RUNINFO[] runInfo, int cRunInfoReq, ref int cRunInfoRcv)
		{
			Invariant.Assert(navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart || navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd);
			Invariant.Assert(limit == null || navigator.CompareTo(limit) <= 0);
			if (limit != null && navigator.CompareTo(limit) >= 0)
			{
				return true;
			}
			bool result = false;
			if (cchReq > 0)
			{
				char c = (navigator.GetAdjacentElement(LogicalDirection.Forward) is TableCell) ? '\0' : '\n';
				text[charsCopied] = c;
				navigator.MoveByOffset(1);
				charsCopied++;
				result = (text.Length == charsCopied || (limit != null && navigator.CompareTo(limit) == 0));
			}
			else
			{
				navigator.MoveByOffset(1);
			}
			if (cRunInfoReq > 0)
			{
				if (cRunInfoRcv > 0 && runInfo[cRunInfoRcv - 1].type == UnsafeNativeMethods.TsRunType.TS_RT_PLAIN)
				{
					int num = cRunInfoRcv - 1;
					runInfo[num].count = runInfo[num].count + 1;
				}
				else
				{
					runInfo[cRunInfoRcv].count = 1;
					runInfo[cRunInfoRcv].type = UnsafeNativeMethods.TsRunType.TS_RT_PLAIN;
					cRunInfoRcv++;
				}
			}
			return result;
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x00281475 File Offset: 0x00280475
		private void GetVisualInfo(out PresentationSource source, out IWin32Window win32Window, out ITextView view)
		{
			source = PresentationSource.CriticalFromVisual(this.RenderScope);
			win32Window = (source as IWin32Window);
			if (win32Window == null)
			{
				throw new COMException(SR.Get("TextStore_TS_E_NOLAYOUT"), -2147220986);
			}
			view = this.TextView;
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x002814B0 File Offset: 0x002804B0
		private static UnsafeNativeMethods.RECT TransformRootRectToScreenCoordinates(Point milPointTopLeft, Point milPointBottomRight, IWin32Window win32Window, PresentationSource source)
		{
			UnsafeNativeMethods.RECT result = default(UnsafeNativeMethods.RECT);
			CompositionTarget compositionTarget = source.CompositionTarget;
			milPointTopLeft = compositionTarget.TransformToDevice.Transform(milPointTopLeft);
			milPointBottomRight = compositionTarget.TransformToDevice.Transform(milPointBottomRight);
			IntPtr handle = IntPtr.Zero;
			handle = win32Window.Handle;
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.ClientToScreen(new HandleRef(null, handle), point);
			result.left = (int)((double)point.x + milPointTopLeft.X);
			result.right = (int)((double)point.x + milPointBottomRight.X);
			result.top = (int)((double)point.y + milPointTopLeft.Y);
			result.bottom = (int)((double)point.y + milPointBottomRight.Y);
			return result;
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x0028156C File Offset: 0x0028056C
		private static string GetFontFamilyName(FontFamily fontFamily, XmlLanguage language)
		{
			if (fontFamily == null)
			{
				return null;
			}
			if (fontFamily.Source != null)
			{
				return fontFamily.Source;
			}
			LanguageSpecificStringDictionary familyNames = fontFamily.FamilyNames;
			if (familyNames == null)
			{
				return null;
			}
			foreach (XmlLanguage key in language.MatchingLanguages)
			{
				string text = familyNames[key];
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x002815F4 File Offset: 0x002805F4
		private void PrepareAttributes(InputScope inputScope, double fontSize, FontFamily fontFamily, XmlLanguage language, Visual visual, int count, Guid[] filterAttributes)
		{
			if (this._preparedattributes == null)
			{
				this._preparedattributes = new ArrayList(count);
			}
			else
			{
				this._preparedattributes.Clear();
			}
			int i = 0;
			while (i < TextStore._supportingattributes.Length)
			{
				if (count == 0)
				{
					goto IL_67;
				}
				bool flag = false;
				for (int j = 0; j < count; j++)
				{
					if (TextStore._supportingattributes[i].Guid.Equals(filterAttributes[j]))
					{
						flag = true;
					}
				}
				if (flag)
				{
					goto IL_67;
				}
				IL_37A:
				i++;
				continue;
				IL_67:
				UnsafeNativeMethods.TS_ATTRVAL ts_ATTRVAL = new UnsafeNativeMethods.TS_ATTRVAL
				{
					attributeId = TextStore._supportingattributes[i].Guid,
					overlappedId = (int)TextStore._supportingattributes[i].Style,
					val = new NativeMethods.VARIANT()
				};
				ts_ATTRVAL.val.SuppressFinalize();
				switch (TextStore._supportingattributes[i].Style)
				{
				case TextStore.AttributeStyle.InputScope:
				{
					object o = new InputScopeAttribute(inputScope);
					ts_ATTRVAL.val.vt = 13;
					ts_ATTRVAL.val.data1.Value = Marshal.GetIUnknownForObject(o);
					break;
				}
				case TextStore.AttributeStyle.Font_Style_Height:
					ts_ATTRVAL.val.vt = 3;
					ts_ATTRVAL.val.data1.Value = (IntPtr)((int)fontSize);
					break;
				case TextStore.AttributeStyle.Font_FaceName:
				{
					string fontFamilyName = TextStore.GetFontFamilyName(fontFamily, language);
					if (fontFamilyName != null)
					{
						ts_ATTRVAL.val.vt = 8;
						ts_ATTRVAL.val.data1.Value = Marshal.StringToBSTR(fontFamilyName);
					}
					break;
				}
				case TextStore.AttributeStyle.Font_SizePts:
					ts_ATTRVAL.val.vt = 3;
					ts_ATTRVAL.val.data1.Value = (IntPtr)((int)(fontSize / 96.0 * 72.0));
					break;
				case TextStore.AttributeStyle.Text_ReadOnly:
					ts_ATTRVAL.val.vt = 11;
					ts_ATTRVAL.val.data1.Value = (this.IsReadOnly ? ((IntPtr)1) : ((IntPtr)0));
					break;
				case TextStore.AttributeStyle.Text_Orientation:
				{
					ts_ATTRVAL.val.vt = 3;
					ts_ATTRVAL.val.data1.Value = (IntPtr)0;
					PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this.RenderScope);
					if (presentationSource != null)
					{
						Visual rootVisual = presentationSource.RootVisual;
						if (rootVisual != null && visual != null)
						{
							Transform affineTransform = visual.TransformToAncestor(rootVisual).AffineTransform;
							if (affineTransform != null)
							{
								Matrix value = affineTransform.Value;
								if (value.M11 != 0.0 || value.M12 != 0.0)
								{
									double num = Math.Asin(value.M12 / Math.Sqrt(value.M11 * value.M11 + value.M12 * value.M12));
									double num2 = Math.Round(Math.Acos(value.M11 / Math.Sqrt(value.M11 * value.M11 + value.M12 * value.M12)) * 180.0 / 3.141592653589793, 0);
									double num3;
									if (num <= 0.0)
									{
										num3 = num2;
									}
									else
									{
										num3 = 360.0 - num2;
									}
									ts_ATTRVAL.val.data1.Value = (IntPtr)((int)num3 * 10);
								}
							}
						}
					}
					break;
				}
				case TextStore.AttributeStyle.Text_VerticalWriting:
					ts_ATTRVAL.val.vt = 11;
					ts_ATTRVAL.val.data1.Value = (IntPtr)0;
					break;
				}
				this._preparedattributes.Add(ts_ATTRVAL);
				goto IL_37A;
			}
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x0028198C File Offset: 0x0028098C
		private void TextPositionsFromITfRange(UnsafeNativeMethods.ITfRange range, out ITextPointer start, out ITextPointer end)
		{
			int num;
			int num2;
			(range as UnsafeNativeMethods.ITfRangeACP).GetExtent(out num, out num2);
			start = this.CreatePointerAtCharOffset(num, LogicalDirection.Backward);
			end = this.CreatePointerAtCharOffset(num + num2, LogicalDirection.Forward);
			while (start.CompareTo(end) < 0 && start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
			{
				start.MoveToNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x002819E0 File Offset: 0x002809E0
		private void GetCompositionPositions(out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			if (this._isComposing)
			{
				UnsafeNativeMethods.ITfCompositionView currentCompositionView = FrameworkTextComposition.GetCurrentCompositionView(this.DocumentManager);
				if (currentCompositionView != null)
				{
					this.GetCompositionPositions(currentCompositionView, out start, out end);
				}
			}
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x00281A14 File Offset: 0x00280A14
		private void GetCompositionPositions(UnsafeNativeMethods.ITfCompositionView view, out ITextPointer start, out ITextPointer end)
		{
			UnsafeNativeMethods.ITfRange tfRange;
			view.GetRange(out tfRange);
			this.TextPositionsFromITfRange(tfRange, out start, out end);
			Marshal.ReleaseComObject(tfRange);
			Marshal.ReleaseComObject(view);
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x00281A40 File Offset: 0x00280A40
		private static string StringFromITfRange(UnsafeNativeMethods.ITfRange range, int ecReadOnly)
		{
			UnsafeNativeMethods.ITfRangeACP tfRangeACP = (UnsafeNativeMethods.ITfRangeACP)range;
			int num;
			int num2;
			tfRangeACP.GetExtent(out num, out num2);
			char[] array = new char[num2];
			int num3;
			tfRangeACP.GetText(ecReadOnly, 0, array, num2, out num3);
			return new string(array);
		}

		// Token: 0x06005A5D RID: 23133 RVA: 0x00281A75 File Offset: 0x00280A75
		private void OnMouseButtonEvent(object sender, MouseButtonEventArgs e)
		{
			e.Handled = this.InternalMouseEventHandler();
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x00281A75 File Offset: 0x00280A75
		private void OnMouseEvent(object sender, MouseEventArgs e)
		{
			e.Handled = this.InternalMouseEventHandler();
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x00281A84 File Offset: 0x00280A84
		private bool InternalMouseEventHandler()
		{
			int num = 0;
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				num |= 1;
			}
			if (Mouse.RightButton == MouseButtonState.Pressed)
			{
				num |= 2;
			}
			if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
			{
				num |= 4;
			}
			if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
			{
				num |= 8;
			}
			Point position = Mouse.GetPosition(this.RenderScope);
			ITextView textView = this.TextView;
			if (textView == null)
			{
				return false;
			}
			if (!textView.Validate(position))
			{
				return false;
			}
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(position, false);
			if (textPositionFromPoint == null)
			{
				return false;
			}
			Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(textPositionFromPoint);
			ITextPointer textPointer = textPositionFromPoint.CreatePointer();
			if (textPointer == null)
			{
				return false;
			}
			if (position.X - rectangleFromTextPosition.Left >= 0.0)
			{
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
			}
			else
			{
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
			}
			Rect rectangleFromTextPosition2 = textView.GetRectangleFromTextPosition(textPointer);
			int charOffset = textPositionFromPoint.CharOffset;
			int num2;
			if (position.X - rectangleFromTextPosition.Left >= 0.0)
			{
				if ((position.X - rectangleFromTextPosition.Left) * 4.0 / (rectangleFromTextPosition2.Left - rectangleFromTextPosition.Left) <= 1.0)
				{
					num2 = 2;
				}
				else
				{
					num2 = 3;
				}
			}
			else if ((position.X - rectangleFromTextPosition2.Left) * 4.0 / (rectangleFromTextPosition.Left - rectangleFromTextPosition2.Left) <= 3.0)
			{
				num2 = 0;
			}
			else
			{
				num2 = 1;
			}
			bool flag = false;
			int num3 = 0;
			while (num3 < this._mouseSinks.Count && !flag)
			{
				TextStore.MouseSink mouseSink = (TextStore.MouseSink)this._mouseSinks[num3];
				int num4;
				int num5;
				mouseSink.Range.GetExtent(out num4, out num5);
				if (charOffset >= num4 && charOffset <= num4 + num5 && (charOffset != num4 || num2 > 1) && (charOffset != num4 + num5 || num2 < 2))
				{
					mouseSink.Locked = true;
					try
					{
						mouseSink.Sink.OnMouseEvent(charOffset - num4, num2, num, out flag);
					}
					finally
					{
						mouseSink.Locked = false;
					}
				}
				num3++;
			}
			return flag;
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x00281C90 File Offset: 0x00280C90
		private TextStore.CompositionParentUndoUnit OpenCompositionUndoUnit()
		{
			return this.OpenCompositionUndoUnit(null, null);
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x00281C9C File Offset: 0x00280C9C
		private TextStore.CompositionParentUndoUnit OpenCompositionUndoUnit(ITextPointer compositionStart, ITextPointer compositionEnd)
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			if (undoManager == null || !undoManager.IsEnabled)
			{
				return null;
			}
			if (compositionStart == null)
			{
				Invariant.Assert(compositionEnd == null);
				this.GetCompositionPositions(out compositionStart, out compositionEnd);
			}
			ITextPointer textPointer;
			if (compositionStart != null && compositionStart.CompareTo(this.TextSelection.Start) > 0)
			{
				textPointer = compositionStart;
			}
			else
			{
				textPointer = this.TextSelection.Start;
			}
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit = new TextStore.CompositionParentUndoUnit(this.TextSelection, textPointer, textPointer, this._nextUndoUnitIsFirstCompositionUnit);
			this._nextUndoUnitIsFirstCompositionUnit = false;
			undoManager.Open(compositionParentUndoUnit);
			return compositionParentUndoUnit;
		}

		// Token: 0x06005A62 RID: 23138 RVA: 0x00281D28 File Offset: 0x00280D28
		private static Rect GetLineBounds(ITextPointer start, ITextPointer end)
		{
			if (!start.HasValidLayout || !end.HasValidLayout)
			{
				return Rect.Empty;
			}
			Rect characterRect = start.GetCharacterRect(LogicalDirection.Forward);
			characterRect.Union(end.GetCharacterRect(LogicalDirection.Backward));
			ITextPointer textPointer = start.CreatePointer(LogicalDirection.Forward);
			while (textPointer.MoveToNextContextPosition(LogicalDirection.Forward) && textPointer.CompareTo(end) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
				switch (pointerContext)
				{
				case TextPointerContext.Text:
					break;
				case TextPointerContext.EmbeddedElement:
				case TextPointerContext.ElementEnd:
					characterRect.Union(textPointer.GetCharacterRect(LogicalDirection.Backward));
					break;
				case TextPointerContext.ElementStart:
					characterRect.Union(textPointer.GetCharacterRect(LogicalDirection.Backward));
					textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
					break;
				default:
					Invariant.Assert(pointerContext > TextPointerContext.None);
					break;
				}
			}
			return characterRect;
		}

		// Token: 0x06005A63 RID: 23139 RVA: 0x00281DD4 File Offset: 0x00280DD4
		private string FilterCompositionString(string text, int charsToReplaceCount)
		{
			string text2 = this.TextEditor._FilterText(text, charsToReplaceCount, false);
			if (text2.Length != text.Length)
			{
				this.CompleteCompositionAsync();
				return null;
			}
			return text2;
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x00281E07 File Offset: 0x00280E07
		private object CompleteCompositionHandler(object o)
		{
			this.CompleteComposition();
			return null;
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x00281E10 File Offset: 0x00280E10
		private IntPtr GetSourceWnd(bool callerIsTrusted)
		{
			IntPtr result = IntPtr.Zero;
			if (this.RenderScope != null)
			{
				IWin32Window win32Window;
				if (callerIsTrusted)
				{
					win32Window = (PresentationSource.CriticalFromVisual(this.RenderScope) as IWin32Window);
				}
				else
				{
					win32Window = (PresentationSource.FromVisual(this.RenderScope) as IWin32Window);
				}
				if (win32Window != null)
				{
					result = win32Window.Handle;
				}
			}
			return result;
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x00281E60 File Offset: 0x00280E60
		private void ValidateChange(UnsafeNativeMethods.TS_TEXTCHANGE change)
		{
			Invariant.Assert(change.start >= 0, "Bad StartIndex");
			Invariant.Assert(change.start <= change.oldEnd, "Bad oldEnd index");
			Invariant.Assert(change.start <= change.newEnd, "Bad newEnd index");
			this._netCharCount += change.newEnd - change.oldEnd;
			Invariant.Assert(this._netCharCount >= 0, "Negative _netCharCount!");
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x00281EE9 File Offset: 0x00280EE9
		private void VerifyTextStoreConsistency()
		{
			if (this._netCharCount != this.TextContainer.IMECharCount)
			{
				Invariant.Assert(false, "TextContainer/TextStore have inconsistent char counts!");
			}
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x00281F0C File Offset: 0x00280F0C
		private void ValidateCharOffset(int offset)
		{
			if (offset < 0 || offset > this.TextContainer.IMECharCount)
			{
				throw new ArgumentException(SR.Get("TextStore_BadIMECharOffset", new object[]
				{
					offset,
					this.TextContainer.IMECharCount
				}));
			}
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x00281F60 File Offset: 0x00280F60
		private void BreakTypingSequence(ITextPointer caretPosition)
		{
			TextStore.CompositionParentUndoUnit compositionParentUndoUnit = this.PeekCompositionParentUndoUnit();
			if (compositionParentUndoUnit != null)
			{
				compositionParentUndoUnit.RecordRedoSelectionState(caretPosition, caretPosition);
			}
		}

		// Token: 0x06005A6A RID: 23146 RVA: 0x00281F80 File Offset: 0x00280F80
		private static void GetAdjustedSelection(ITextPointer startIn, ITextPointer endIn, out ITextPointer startOut, out ITextPointer endOut)
		{
			startOut = startIn;
			endOut = endIn;
			TextPointer textPointer = startOut as TextPointer;
			if (textPointer == null)
			{
				return;
			}
			TextPointer position = (TextPointer)endOut;
			if (startIn.CompareTo(endIn) != 0)
			{
				bool flag = TextPointerBase.IsInBlockUIContainer(textPointer) || TextPointerBase.IsInBlockUIContainer(position);
				TableCell tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition(textPointer);
				TableCell tableCellFromPosition2 = TextRangeEditTables.GetTableCellFromPosition(position);
				bool flag2 = tableCellFromPosition != null && tableCellFromPosition == tableCellFromPosition2;
				bool flag3 = TextRangeEditTables.GetTableFromPosition(textPointer) != null || TextRangeEditTables.GetTableFromPosition(position) != null;
				if (!flag && (flag2 || !flag3))
				{
					return;
				}
			}
			if (textPointer.IsAtRowEnd)
			{
				TextPointer nextInsertionPosition = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
				textPointer = TextRangeEditTables.GetAdjustedRowEndPosition(TextRangeEditTables.GetTableFromPosition(textPointer), textPointer);
				if (!textPointer.IsAtInsertionPosition)
				{
					textPointer = nextInsertionPosition;
				}
			}
			else if (TextPointerBase.IsInBlockUIContainer(textPointer))
			{
				if (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
				{
					textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
				}
				else
				{
					textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
				}
			}
			while (textPointer != null && TextPointerBase.IsBeforeFirstTable(textPointer))
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
			}
			if (textPointer == null || textPointer.IsAtRowEnd || TextPointerBase.IsInBlockUIContainer(textPointer))
			{
				throw new COMException(SR.Get("TextStore_CompositionRejected"), -2147467259);
			}
			startOut = textPointer;
			endOut = textPointer;
		}

		// Token: 0x06005A6B RID: 23147 RVA: 0x00282090 File Offset: 0x00281090
		private void GetNormalizedRange(int startCharOffset, int endCharOffset, out ITextPointer start, out ITextPointer end)
		{
			start = this.CreatePointerAtCharOffset(startCharOffset, LogicalDirection.Forward);
			end = ((startCharOffset == endCharOffset) ? start : this.CreatePointerAtCharOffset(endCharOffset, LogicalDirection.Backward));
			while (start.CompareTo(end) < 0)
			{
				TextPointerContext pointerContext = start.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart)
				{
					TextElement textElement = start.GetAdjacentElement(LogicalDirection.Forward) as TextElement;
					if (textElement == null)
					{
						break;
					}
					if (textElement.IMELeftEdgeCharCount != 0)
					{
						break;
					}
				}
				else if (pointerContext != TextPointerContext.ElementEnd)
				{
					break;
				}
				start.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			if (start.CompareTo(end) == 0)
			{
				start = start.GetFormatNormalizedPosition(LogicalDirection.Backward);
				end = start;
				return;
			}
			start = start.GetFormatNormalizedPosition(LogicalDirection.Backward);
			end = end.GetFormatNormalizedPosition(LogicalDirection.Backward);
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x00282130 File Offset: 0x00281130
		private void HandleCompositionEvents(int previousUndoCount)
		{
			if (this.CompositionEventList.Count == 0 || this._compositionEventState != TextStore.CompositionEventState.NotRaisingEvents)
			{
				return;
			}
			this._compositionEventState = TextStore.CompositionEventState.RaisingEvents;
			try
			{
				int offset = this.TextSelection.AnchorPosition.Offset;
				int offset2 = this.TextSelection.MovingPosition.Offset;
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				undoManager.SetRedoStack(null);
				this.UndoQuietly(undoManager.UndoCount - previousUndoCount);
				Stack imeChangeStack = undoManager.SetRedoStack(null);
				int undoCount = undoManager.UndoCount;
				int appSelectionAnchorOffset;
				int appSelectionMovingOffset;
				this.RaiseCompositionEvents(out appSelectionAnchorOffset, out appSelectionMovingOffset);
				this.SetFinalDocumentState(undoManager, imeChangeStack, undoManager.UndoCount - undoCount, offset, offset2, appSelectionAnchorOffset, appSelectionMovingOffset);
			}
			finally
			{
				this.CompositionEventList.Clear();
				this._compositionEventState = TextStore.CompositionEventState.NotRaisingEvents;
			}
		}

		// Token: 0x06005A6D RID: 23149 RVA: 0x002821FC File Offset: 0x002811FC
		private TextParentUndoUnit OpenTextParentUndoUnit()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			TextParentUndoUnit textParentUndoUnit = new TextParentUndoUnit(this.TextSelection, this.TextSelection.Start, this.TextSelection.Start);
			undoManager.Open(textParentUndoUnit);
			return textParentUndoUnit;
		}

		// Token: 0x06005A6E RID: 23150 RVA: 0x00282242 File Offset: 0x00281242
		private void CloseTextParentUndoUnit(TextParentUndoUnit textParentUndoUnit, UndoCloseAction undoCloseAction)
		{
			if (textParentUndoUnit != null)
			{
				UndoManager.GetUndoManager(this.TextContainer.Parent).Close(textParentUndoUnit, undoCloseAction);
			}
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x00282260 File Offset: 0x00281260
		private void RaiseCompositionEvents(out int appSelectionAnchorOffset, out int appSelectionMovingOffset)
		{
			appSelectionAnchorOffset = -1;
			appSelectionMovingOffset = -1;
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			int i = 0;
			while (i < this.CompositionEventList.Count)
			{
				TextStore.CompositionEventRecord compositionEventRecord = this.CompositionEventList[i];
				FrameworkTextComposition frameworkTextComposition = TextStore.CreateComposition(this.TextEditor, this);
				ITextPointer start = this.TextContainer.CreatePointerAtOffset(compositionEventRecord.StartOffsetBefore, LogicalDirection.Backward);
				ITextPointer textPointer = this.TextContainer.CreatePointerAtOffset(compositionEventRecord.EndOffsetBefore, LogicalDirection.Forward);
				bool flag = false;
				this._handledByTextStoreListener = false;
				this._compositionModifiedByEventListener = false;
				switch (compositionEventRecord.Stage)
				{
				case TextStore.CompositionStage.StartComposition:
					frameworkTextComposition.Stage = TextCompositionStage.None;
					frameworkTextComposition.SetCompositionPositions(start, textPointer, compositionEventRecord.Text);
					undoManager.MinUndoStackCount = undoManager.UndoCount;
					try
					{
						flag = TextCompositionManager.StartComposition(frameworkTextComposition);
						break;
					}
					finally
					{
						undoManager.MinUndoStackCount = 0;
					}
					goto IL_CB;
				case TextStore.CompositionStage.UpdateComposition:
					goto IL_CB;
				case TextStore.CompositionStage.EndComposition:
					goto IL_14D;
				default:
					goto IL_190;
				}
				IL_19B:
				if ((compositionEventRecord.Stage == TextStore.CompositionStage.EndComposition && !this._handledByTextStoreListener) || (compositionEventRecord.Stage != TextStore.CompositionStage.EndComposition && flag) || frameworkTextComposition.PendingComplete)
				{
					this._compositionModifiedByEventListener = true;
				}
				if (this._compositionModifiedByEventListener)
				{
					appSelectionAnchorOffset = this.TextSelection.AnchorPosition.Offset;
					appSelectionMovingOffset = this.TextSelection.MovingPosition.Offset;
					return;
				}
				if (compositionEventRecord.Stage != TextStore.CompositionStage.EndComposition && !compositionEventRecord.IsShiftUpdate)
				{
					this.UpdateCompositionText(frameworkTextComposition);
				}
				if (compositionEventRecord.Stage == TextStore.CompositionStage.EndComposition)
				{
					start = textPointer.GetFrozenPointer(LogicalDirection.Backward);
				}
				if (this._compositionModifiedByEventListener)
				{
					appSelectionAnchorOffset = this.TextSelection.AnchorPosition.Offset;
					appSelectionMovingOffset = this.TextSelection.MovingPosition.Offset;
					return;
				}
				i++;
				continue;
				IL_CB:
				frameworkTextComposition.Stage = TextCompositionStage.Started;
				frameworkTextComposition.SetCompositionPositions(start, textPointer, compositionEventRecord.Text);
				undoManager.MinUndoStackCount = undoManager.UndoCount;
				try
				{
					if (this.IsCompositionRecordShifted(compositionEventRecord) && this.IsMaxLengthExceeded(frameworkTextComposition.CompositionText, compositionEventRecord.EndOffsetBefore - compositionEventRecord.StartOffsetBefore))
					{
						frameworkTextComposition.SetResultPositions(start, textPointer, compositionEventRecord.Text);
						flag = TextCompositionManager.CompleteComposition(frameworkTextComposition);
						this._compositionModifiedByEventListener = true;
						goto IL_19B;
					}
					if (!compositionEventRecord.IsShiftUpdate)
					{
						flag = TextCompositionManager.UpdateComposition(frameworkTextComposition);
					}
					goto IL_19B;
				}
				finally
				{
					undoManager.MinUndoStackCount = 0;
				}
				IL_14D:
				frameworkTextComposition.Stage = TextCompositionStage.Started;
				frameworkTextComposition.SetResultPositions(start, textPointer, compositionEventRecord.Text);
				undoManager.MinUndoStackCount = undoManager.UndoCount;
				try
				{
					this._isEffectivelyNotComposing = true;
					flag = TextCompositionManager.CompleteComposition(frameworkTextComposition);
					goto IL_19B;
				}
				finally
				{
					undoManager.MinUndoStackCount = 0;
					this._isEffectivelyNotComposing = false;
				}
				IL_190:
				Invariant.Assert(false, "Invalid composition stage!");
				goto IL_19B;
			}
		}

		// Token: 0x06005A70 RID: 23152 RVA: 0x002824FC File Offset: 0x002814FC
		private bool IsMaxLengthExceeded(string textData, int charsToReplaceCount)
		{
			if (!this.TextEditor.AcceptsRichContent && this.TextEditor.MaxLength > 0)
			{
				int num = this.TextContainer.SymbolCount - charsToReplaceCount;
				int num2 = Math.Max(0, this.TextEditor.MaxLength - num);
				if (textData.Length > num2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x00282552 File Offset: 0x00281552
		private bool IsCompositionRecordShifted(TextStore.CompositionEventRecord record)
		{
			return (0 <= record.StartOffsetBefore && 0 <= this._previousCompositionStartOffset && record.StartOffsetBefore < this._previousCompositionStartOffset) || record.IsShiftUpdate;
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x00282580 File Offset: 0x00281580
		private void SetFinalDocumentState(UndoManager undoManager, Stack imeChangeStack, int appChangeCount, int imeSelectionAnchorOffset, int imeSelectionMovingOffset, int appSelectionAnchorOffset, int appSelectionMovingOffset)
		{
			this.TextSelection.BeginChangeNoUndo();
			try
			{
				bool compositionModifiedByEventListener = this._compositionModifiedByEventListener;
				this.UndoQuietly(appChangeCount);
				Stack redoStack = undoManager.SetRedoStack(imeChangeStack);
				int count = imeChangeStack.Count;
				this.RedoQuietly(count);
				Invariant.Assert(this._netCharCount == this.TextContainer.IMECharCount);
				if (compositionModifiedByEventListener)
				{
					int num = undoManager.UndoCount;
					if (this._isComposing)
					{
						TextParentUndoUnit textParentUndoUnit = this.OpenTextParentUndoUnit();
						try
						{
							this.CompleteComposition();
						}
						finally
						{
							this.CloseTextParentUndoUnit(textParentUndoUnit, (textParentUndoUnit.LastUnit != null) ? UndoCloseAction.Commit : UndoCloseAction.Discard);
						}
					}
					num = undoManager.UndoCount - num;
					this._compositionEventState = TextStore.CompositionEventState.ApplyingApplicationChange;
					try
					{
						this.UndoQuietly(num);
						this.UndoQuietly(count);
						undoManager.SetRedoStack(redoStack);
						this.RedoQuietly(appChangeCount);
						Invariant.Assert(this._netCharCount == this.TextContainer.IMECharCount);
						ITextPointer position = this.TextContainer.CreatePointerAtOffset(appSelectionAnchorOffset, LogicalDirection.Forward);
						ITextPointer position2 = this.TextContainer.CreatePointerAtOffset(appSelectionMovingOffset, LogicalDirection.Forward);
						this.TextSelection.Select(position, position2);
						this.MergeCompositionUndoUnits();
						return;
					}
					finally
					{
						this._compositionEventState = TextStore.CompositionEventState.RaisingEvents;
					}
				}
				ITextPointer position3 = this.TextContainer.CreatePointerAtOffset(imeSelectionAnchorOffset, LogicalDirection.Backward);
				ITextPointer position4 = this.TextContainer.CreatePointerAtOffset(imeSelectionMovingOffset, LogicalDirection.Backward);
				this.TextSelection.Select(position3, position4);
				this.MergeCompositionUndoUnits();
			}
			finally
			{
				this.TextSelection.EndChange(false, true);
			}
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x00282720 File Offset: 0x00281720
		private void UndoQuietly(int count)
		{
			if (count > 0)
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				this.TextSelection.BeginChangeNoUndo();
				try
				{
					undoManager.Undo(count);
				}
				finally
				{
					this.TextSelection.EndChange(false, true);
				}
			}
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x00282774 File Offset: 0x00281774
		private void RedoQuietly(int count)
		{
			if (count > 0)
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
				this.TextSelection.BeginChangeNoUndo();
				try
				{
					undoManager.Redo(count);
				}
				finally
				{
					this.TextSelection.EndChange(false, true);
				}
			}
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x002827C8 File Offset: 0x002817C8
		private void MergeCompositionUndoUnits()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			if (undoManager == null || !undoManager.IsEnabled)
			{
				return;
			}
			int i = undoManager.UndoCount - 1;
			int num = undoManager.UndoCount - 1;
			while (i >= 0)
			{
				TextStore.CompositionParentUndoUnit compositionParentUndoUnit = undoManager.GetUndoUnit(i) as TextStore.CompositionParentUndoUnit;
				if (compositionParentUndoUnit == null || (compositionParentUndoUnit.IsFirstCompositionUnit && compositionParentUndoUnit.IsLastCompositionUnit))
				{
					break;
				}
				if (!compositionParentUndoUnit.IsFirstCompositionUnit)
				{
					i--;
				}
				else
				{
					int num2 = num - i;
					for (int j = i + 1; j <= i + num2; j++)
					{
						TextStore.CompositionParentUndoUnit unit = (TextStore.CompositionParentUndoUnit)undoManager.GetUndoUnit(j);
						compositionParentUndoUnit.MergeCompositionUnit(unit);
					}
					undoManager.RemoveUndoRange(i + 1, num2);
					i--;
					num = i;
				}
			}
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x0028287C File Offset: 0x0028187C
		private TextStore.CompositionParentUndoUnit PeekCompositionParentUndoUnit()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this.TextContainer.Parent);
			if (undoManager == null || !undoManager.IsEnabled)
			{
				return null;
			}
			return undoManager.PeekUndoStack() as TextStore.CompositionParentUndoUnit;
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06005A77 RID: 23159 RVA: 0x002828B2 File Offset: 0x002818B2
		private bool IsTextEditorValid
		{
			get
			{
				return this._weakTextEditor.IsValid;
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06005A78 RID: 23160 RVA: 0x002828BF File Offset: 0x002818BF
		private TextEditor TextEditor
		{
			get
			{
				return this._weakTextEditor.TextEditor;
			}
		}

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x06005A79 RID: 23161 RVA: 0x002828CC File Offset: 0x002818CC
		private ITextSelection TextSelection
		{
			get
			{
				return this.TextEditor.Selection;
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x06005A7A RID: 23162 RVA: 0x002828D9 File Offset: 0x002818D9
		private bool IsReadOnly
		{
			get
			{
				return (bool)this.UiScope.GetValue(TextEditor.IsReadOnlyProperty) || this.TextEditor.IsReadOnly;
			}
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x06005A7B RID: 23163 RVA: 0x002828FF File Offset: 0x002818FF
		private List<TextStore.CompositionEventRecord> CompositionEventList
		{
			get
			{
				if (this._compositionEventList == null)
				{
					this._compositionEventList = new List<TextStore.CompositionEventRecord>();
				}
				return this._compositionEventList;
			}
		}

		// Token: 0x0400302F RID: 12335
		private readonly TextStore.ScopeWeakReference _weakTextEditor;

		// Token: 0x04003030 RID: 12336
		private TextServicesHost _textservicesHost;

		// Token: 0x04003031 RID: 12337
		private UnsafeNativeMethods.ITextStoreACPSink _sink;

		// Token: 0x04003032 RID: 12338
		private bool _pendingWriteReq;

		// Token: 0x04003033 RID: 12339
		private UnsafeNativeMethods.LockFlags _lockFlags;

		// Token: 0x04003034 RID: 12340
		private UnsafeNativeMethods.LockFlags _pendingAsyncLockFlags;

		// Token: 0x04003035 RID: 12341
		private int _textChangeReentrencyCount;

		// Token: 0x04003036 RID: 12342
		private bool _isComposing;

		// Token: 0x04003037 RID: 12343
		private bool _isEffectivelyNotComposing;

		// Token: 0x04003038 RID: 12344
		private int _previousCompositionStartOffset = -1;

		// Token: 0x04003039 RID: 12345
		private int _previousCompositionEndOffset = -1;

		// Token: 0x0400303A RID: 12346
		private ITextPointer _previousCompositionStart;

		// Token: 0x0400303B RID: 12347
		private ITextPointer _previousCompositionEnd;

		// Token: 0x0400303C RID: 12348
		private TextServicesProperty _textservicesproperty;

		// Token: 0x0400303D RID: 12349
		private const int _viewCookie = 0;

		// Token: 0x0400303E RID: 12350
		private SecurityCriticalDataClass<UnsafeNativeMethods.ITfDocumentMgr> _documentmanager;

		// Token: 0x0400303F RID: 12351
		private int _threadFocusCookie;

		// Token: 0x04003040 RID: 12352
		private int _editSinkCookie;

		// Token: 0x04003041 RID: 12353
		private int _editCookie;

		// Token: 0x04003042 RID: 12354
		private int _transitoryExtensionSinkCookie;

		// Token: 0x04003043 RID: 12355
		private ArrayList _preparedattributes;

		// Token: 0x04003044 RID: 12356
		private ArrayList _mouseSinks;

		// Token: 0x04003045 RID: 12357
		private static readonly TextStore.TextServicesAttribute[] _supportingattributes = new TextStore.TextServicesAttribute[]
		{
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.GUID_PROP_INPUTSCOPE, TextStore.AttributeStyle.InputScope),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Font_Style_Height, TextStore.AttributeStyle.Font_Style_Height),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Font_FaceName, TextStore.AttributeStyle.Font_FaceName),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Font_SizePts, TextStore.AttributeStyle.Font_SizePts),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Text_ReadOnly, TextStore.AttributeStyle.Text_ReadOnly),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Text_Orientation, TextStore.AttributeStyle.Text_Orientation),
			new TextStore.TextServicesAttribute(UnsafeNativeMethods.TSATTRID_Text_VerticalWriting, TextStore.AttributeStyle.Text_VerticalWriting)
		};

		// Token: 0x04003046 RID: 12358
		private bool _interimSelection;

		// Token: 0x04003047 RID: 12359
		private bool _ignoreNextSelectionChange;

		// Token: 0x04003048 RID: 12360
		private int _netCharCount;

		// Token: 0x04003049 RID: 12361
		private bool _makeLayoutChangeOnGotFocus;

		// Token: 0x0400304A RID: 12362
		private TextStore.CompositionEventState _compositionEventState;

		// Token: 0x0400304B RID: 12363
		private bool _compositionModifiedByEventListener;

		// Token: 0x0400304C RID: 12364
		private List<TextStore.CompositionEventRecord> _compositionEventList;

		// Token: 0x0400304D RID: 12365
		private bool _nextUndoUnitIsFirstCompositionUnit = true;

		// Token: 0x0400304E RID: 12366
		private string _lastCompositionText;

		// Token: 0x0400304F RID: 12367
		private bool _handledByTextStoreListener;

		// Token: 0x04003050 RID: 12368
		private bool _isInUpdateLayout;

		// Token: 0x04003051 RID: 12369
		private bool _hasTextChangedInUpdateLayout;

		// Token: 0x02000B73 RID: 2931
		private enum AttributeStyle
		{
			// Token: 0x040048F0 RID: 18672
			InputScope,
			// Token: 0x040048F1 RID: 18673
			Font_Style_Height,
			// Token: 0x040048F2 RID: 18674
			Font_FaceName,
			// Token: 0x040048F3 RID: 18675
			Font_SizePts,
			// Token: 0x040048F4 RID: 18676
			Text_ReadOnly,
			// Token: 0x040048F5 RID: 18677
			Text_Orientation,
			// Token: 0x040048F6 RID: 18678
			Text_VerticalWriting
		}

		// Token: 0x02000B74 RID: 2932
		private struct TextServicesAttribute
		{
			// Token: 0x06008E04 RID: 36356 RVA: 0x0034031E File Offset: 0x0033F31E
			internal TextServicesAttribute(Guid guid, TextStore.AttributeStyle style)
			{
				this._guid = guid;
				this._style = style;
			}

			// Token: 0x17001F09 RID: 7945
			// (get) Token: 0x06008E05 RID: 36357 RVA: 0x0034032E File Offset: 0x0033F32E
			internal Guid Guid
			{
				get
				{
					return this._guid;
				}
			}

			// Token: 0x17001F0A RID: 7946
			// (get) Token: 0x06008E06 RID: 36358 RVA: 0x00340336 File Offset: 0x0033F336
			internal TextStore.AttributeStyle Style
			{
				get
				{
					return this._style;
				}
			}

			// Token: 0x040048F7 RID: 18679
			private Guid _guid;

			// Token: 0x040048F8 RID: 18680
			private TextStore.AttributeStyle _style;
		}

		// Token: 0x02000B75 RID: 2933
		private class ScopeWeakReference : WeakReference
		{
			// Token: 0x06008E07 RID: 36359 RVA: 0x0032A9B4 File Offset: 0x003299B4
			internal ScopeWeakReference(object obj) : base(obj)
			{
			}

			// Token: 0x17001F0B RID: 7947
			// (get) Token: 0x06008E08 RID: 36360 RVA: 0x00340340 File Offset: 0x0033F340
			internal bool IsValid
			{
				get
				{
					bool result;
					try
					{
						result = this.IsAlive;
					}
					catch (InvalidOperationException)
					{
						result = false;
					}
					return result;
				}
			}

			// Token: 0x17001F0C RID: 7948
			// (get) Token: 0x06008E09 RID: 36361 RVA: 0x0034036C File Offset: 0x0033F36C
			internal TextEditor TextEditor
			{
				get
				{
					TextEditor result;
					try
					{
						result = (TextEditor)this.Target;
					}
					catch (InvalidOperationException)
					{
						result = null;
					}
					return result;
				}
			}
		}

		// Token: 0x02000B76 RID: 2934
		private class MouseSink : IDisposable, IComparer
		{
			// Token: 0x06008E0A RID: 36362 RVA: 0x003403A0 File Offset: 0x0033F3A0
			internal MouseSink(UnsafeNativeMethods.ITfRangeACP range, UnsafeNativeMethods.ITfMouseSink sink, int cookie)
			{
				this._range = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfRangeACP>(range);
				this._sink = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfMouseSink>(sink);
				this._cookie = cookie;
			}

			// Token: 0x06008E0B RID: 36363 RVA: 0x003403C8 File Offset: 0x0033F3C8
			public void Dispose()
			{
				Invariant.Assert(!this._locked);
				if (this._range != null)
				{
					Marshal.ReleaseComObject(this._range.Value);
					this._range = null;
				}
				if (this._sink != null)
				{
					Marshal.ReleaseComObject(this._sink.Value);
					this._sink = null;
				}
				this._cookie = -1;
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008E0C RID: 36364 RVA: 0x00340430 File Offset: 0x0033F430
			public int Compare(object x, object y)
			{
				return ((TextStore.MouseSink)x)._cookie - ((TextStore.MouseSink)y)._cookie;
			}

			// Token: 0x17001F0D RID: 7949
			// (get) Token: 0x06008E0D RID: 36365 RVA: 0x00340449 File Offset: 0x0033F449
			// (set) Token: 0x06008E0E RID: 36366 RVA: 0x00340451 File Offset: 0x0033F451
			internal bool Locked
			{
				get
				{
					return this._locked;
				}
				set
				{
					this._locked = value;
					if (!this._locked && this._pendingDispose)
					{
						this.Dispose();
					}
				}
			}

			// Token: 0x17001F0E RID: 7950
			// (set) Token: 0x06008E0F RID: 36367 RVA: 0x00340470 File Offset: 0x0033F470
			internal bool PendingDispose
			{
				set
				{
					this._pendingDispose = value;
				}
			}

			// Token: 0x17001F0F RID: 7951
			// (get) Token: 0x06008E10 RID: 36368 RVA: 0x00340479 File Offset: 0x0033F479
			internal UnsafeNativeMethods.ITfRangeACP Range
			{
				get
				{
					return this._range.Value;
				}
			}

			// Token: 0x17001F10 RID: 7952
			// (get) Token: 0x06008E11 RID: 36369 RVA: 0x00340486 File Offset: 0x0033F486
			internal UnsafeNativeMethods.ITfMouseSink Sink
			{
				get
				{
					return this._sink.Value;
				}
			}

			// Token: 0x17001F11 RID: 7953
			// (get) Token: 0x06008E12 RID: 36370 RVA: 0x00340493 File Offset: 0x0033F493
			internal int Cookie
			{
				get
				{
					return this._cookie;
				}
			}

			// Token: 0x040048F9 RID: 18681
			private SecurityCriticalDataClass<UnsafeNativeMethods.ITfRangeACP> _range;

			// Token: 0x040048FA RID: 18682
			private SecurityCriticalDataClass<UnsafeNativeMethods.ITfMouseSink> _sink;

			// Token: 0x040048FB RID: 18683
			private int _cookie;

			// Token: 0x040048FC RID: 18684
			private bool _locked;

			// Token: 0x040048FD RID: 18685
			private bool _pendingDispose;
		}

		// Token: 0x02000B77 RID: 2935
		private class CompositionParentUndoUnit : TextParentUndoUnit
		{
			// Token: 0x06008E13 RID: 36371 RVA: 0x0034049B File Offset: 0x0033F49B
			internal CompositionParentUndoUnit(ITextSelection selection, ITextPointer anchorPosition, ITextPointer movingPosition, bool isFirstCompositionUnit) : base(selection, anchorPosition, movingPosition)
			{
				this._isFirstCompositionUnit = isFirstCompositionUnit;
			}

			// Token: 0x06008E14 RID: 36372 RVA: 0x003404AE File Offset: 0x0033F4AE
			private CompositionParentUndoUnit(TextStore.CompositionParentUndoUnit undoUnit) : base(undoUnit)
			{
				this._isFirstCompositionUnit = undoUnit._isFirstCompositionUnit;
				this._isLastCompositionUnit = undoUnit._isLastCompositionUnit;
			}

			// Token: 0x06008E15 RID: 36373 RVA: 0x003404CF File Offset: 0x0033F4CF
			protected override TextParentUndoUnit CreateRedoUnit()
			{
				return new TextStore.CompositionParentUndoUnit(this);
			}

			// Token: 0x06008E16 RID: 36374 RVA: 0x003404D8 File Offset: 0x0033F4D8
			internal void MergeCompositionUnit(TextStore.CompositionParentUndoUnit unit)
			{
				object[] array = unit.CopyUnits();
				Invariant.Assert(this.Locked);
				this.Locked = false;
				for (int i = array.Length - 1; i >= 0; i--)
				{
					this.Add((IUndoUnit)array[i]);
				}
				this.Locked = true;
				base.MergeRedoSelectionState(unit);
				this._isLastCompositionUnit |= unit.IsLastCompositionUnit;
			}

			// Token: 0x17001F12 RID: 7954
			// (get) Token: 0x06008E17 RID: 36375 RVA: 0x0034053D File Offset: 0x0033F53D
			internal bool IsFirstCompositionUnit
			{
				get
				{
					return this._isFirstCompositionUnit;
				}
			}

			// Token: 0x17001F13 RID: 7955
			// (get) Token: 0x06008E18 RID: 36376 RVA: 0x00340545 File Offset: 0x0033F545
			// (set) Token: 0x06008E19 RID: 36377 RVA: 0x0034054D File Offset: 0x0033F54D
			internal bool IsLastCompositionUnit
			{
				get
				{
					return this._isLastCompositionUnit;
				}
				set
				{
					this._isLastCompositionUnit = value;
				}
			}

			// Token: 0x06008E1A RID: 36378 RVA: 0x00340556 File Offset: 0x0033F556
			private object[] CopyUnits()
			{
				return base.Units.ToArray();
			}

			// Token: 0x040048FE RID: 18686
			private readonly bool _isFirstCompositionUnit;

			// Token: 0x040048FF RID: 18687
			private bool _isLastCompositionUnit;
		}

		// Token: 0x02000B78 RID: 2936
		private enum CompositionEventState
		{
			// Token: 0x04004901 RID: 18689
			NotRaisingEvents,
			// Token: 0x04004902 RID: 18690
			RaisingEvents,
			// Token: 0x04004903 RID: 18691
			ApplyingApplicationChange
		}

		// Token: 0x02000B79 RID: 2937
		private enum CompositionStage
		{
			// Token: 0x04004905 RID: 18693
			StartComposition = 1,
			// Token: 0x04004906 RID: 18694
			UpdateComposition,
			// Token: 0x04004907 RID: 18695
			EndComposition
		}

		// Token: 0x02000B7A RID: 2938
		private class CompositionEventRecord
		{
			// Token: 0x06008E1B RID: 36379 RVA: 0x00340563 File Offset: 0x0033F563
			internal CompositionEventRecord(TextStore.CompositionStage stage, int startOffsetBefore, int endOffsetBefore, string text) : this(stage, startOffsetBefore, endOffsetBefore, text, false)
			{
			}

			// Token: 0x06008E1C RID: 36380 RVA: 0x00340571 File Offset: 0x0033F571
			internal CompositionEventRecord(TextStore.CompositionStage stage, int startOffsetBefore, int endOffsetBefore, string text, bool isShiftUpdate)
			{
				this._stage = stage;
				this._startOffsetBefore = startOffsetBefore;
				this._endOffsetBefore = endOffsetBefore;
				this._text = text;
				this._isShiftUpdate = isShiftUpdate;
			}

			// Token: 0x17001F14 RID: 7956
			// (get) Token: 0x06008E1D RID: 36381 RVA: 0x0034059E File Offset: 0x0033F59E
			internal TextStore.CompositionStage Stage
			{
				get
				{
					return this._stage;
				}
			}

			// Token: 0x17001F15 RID: 7957
			// (get) Token: 0x06008E1E RID: 36382 RVA: 0x003405A6 File Offset: 0x0033F5A6
			internal int StartOffsetBefore
			{
				get
				{
					return this._startOffsetBefore;
				}
			}

			// Token: 0x17001F16 RID: 7958
			// (get) Token: 0x06008E1F RID: 36383 RVA: 0x003405AE File Offset: 0x0033F5AE
			internal int EndOffsetBefore
			{
				get
				{
					return this._endOffsetBefore;
				}
			}

			// Token: 0x17001F17 RID: 7959
			// (get) Token: 0x06008E20 RID: 36384 RVA: 0x003405B6 File Offset: 0x0033F5B6
			internal string Text
			{
				get
				{
					return this._text;
				}
			}

			// Token: 0x17001F18 RID: 7960
			// (get) Token: 0x06008E21 RID: 36385 RVA: 0x003405BE File Offset: 0x0033F5BE
			internal bool IsShiftUpdate
			{
				get
				{
					return this._isShiftUpdate;
				}
			}

			// Token: 0x04004908 RID: 18696
			private readonly TextStore.CompositionStage _stage;

			// Token: 0x04004909 RID: 18697
			private readonly int _startOffsetBefore;

			// Token: 0x0400490A RID: 18698
			private readonly int _endOffsetBefore;

			// Token: 0x0400490B RID: 18699
			private readonly string _text;

			// Token: 0x0400490C RID: 18700
			private readonly bool _isShiftUpdate;
		}
	}
}
