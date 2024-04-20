using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.IO.Packaging;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	// Token: 0x02000688 RID: 1672
	internal class Speller
	{
		// Token: 0x060052CE RID: 21198 RVA: 0x002594EC File Offset: 0x002584EC
		internal Speller(TextEditor textEditor)
		{
			this._textEditor = textEditor;
			this._textEditor.TextContainer.Change += this.OnTextContainerChange;
			if (this._textEditor.TextContainer.SymbolCount > 0)
			{
				this.ScheduleIdleCallback();
			}
			this._defaultCulture = ((InputLanguageManager.Current != null) ? InputLanguageManager.Current.CurrentInputLanguage : Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x00259560 File Offset: 0x00258560
		internal void Detach()
		{
			Invariant.Assert(this._textEditor != null);
			this._textEditor.TextContainer.Change -= this.OnTextContainerChange;
			if (this._pendingCaretMovedCallback)
			{
				this._textEditor.Selection.Changed -= this.OnCaretMoved;
				this._textEditor.UiScope.LostFocus -= this.OnLostFocus;
				this._pendingCaretMovedCallback = false;
			}
			if (this._highlightLayer != null)
			{
				this._textEditor.TextContainer.Highlights.RemoveLayer(this._highlightLayer);
				this._highlightLayer = null;
			}
			this._statusTable = null;
			if (this._spellerInterop != null)
			{
				this._spellerInterop.Dispose();
				this._spellerInterop = null;
			}
			this._textEditor = null;
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x00259630 File Offset: 0x00258630
		internal SpellingError GetError(ITextPointer position, LogicalDirection direction, bool forceEvaluation)
		{
			if (forceEvaluation && this.EnsureInitialized() && this._statusTable.IsRunType(position.CreateStaticPointer(), direction, SpellerStatusTable.RunType.Dirty))
			{
				this.ScanPosition(position, direction);
			}
			ITextPointer start;
			ITextPointer end;
			SpellingError result;
			if (this._statusTable != null && this._statusTable.GetError(position.CreateStaticPointer(), direction, out start, out end))
			{
				result = new SpellingError(this, start, end);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x00259694 File Offset: 0x00258694
		internal ITextPointer GetNextSpellingErrorPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.EnsureInitialized())
			{
				return null;
			}
			SpellerStatusTable.RunType runType;
			StaticTextPointer staticTextPointer;
			while (this._statusTable.GetRun(position.CreateStaticPointer(), direction, out runType, out staticTextPointer) && runType != SpellerStatusTable.RunType.Error)
			{
				if (runType == SpellerStatusTable.RunType.Dirty)
				{
					this.ScanPosition(position, direction);
					this._statusTable.GetRun(position.CreateStaticPointer(), direction, out runType, out staticTextPointer);
					Invariant.Assert(runType != SpellerStatusTable.RunType.Dirty);
					if (runType == SpellerStatusTable.RunType.Error)
					{
						break;
					}
				}
				position = staticTextPointer.CreateDynamicTextPointer(direction);
			}
			SpellingError error = this.GetError(position, direction, false);
			if (error != null)
			{
				return error.Start;
			}
			return null;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x0025971C File Offset: 0x0025871C
		internal IList GetSuggestionsForError(SpellingError error)
		{
			ArrayList arrayList = new ArrayList(1);
			XmlLanguage language;
			CultureInfo currentCultureAndLanguage = this.GetCurrentCultureAndLanguage(error.Start, out language);
			if (currentCultureAndLanguage != null && this._spellerInterop.CanSpellCheck(currentCultureAndLanguage))
			{
				ITextPointer contentStart;
				ITextPointer contextStart;
				this.ExpandToWordBreakAndContext(error.Start, LogicalDirection.Backward, language, out contentStart, out contextStart);
				ITextPointer contentEnd;
				ITextPointer contextEnd;
				this.ExpandToWordBreakAndContext(error.End, LogicalDirection.Forward, language, out contentEnd, out contextEnd);
				Speller.TextMap textMap = new Speller.TextMap(contextStart, contextEnd, contentStart, contentEnd);
				this.SetCulture(currentCultureAndLanguage);
				this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.SpellingErrorsWithSuggestions;
				this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanErrorTextSegment), new Speller.TextMapCallbackData(textMap, arrayList));
			}
			return arrayList;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x002597D0 File Offset: 0x002587D0
		internal void IgnoreAll(string word)
		{
			if (this._ignoredWordsList == null)
			{
				this._ignoredWordsList = new ArrayList(1);
			}
			int num = this._ignoredWordsList.BinarySearch(word, new CaseInsensitiveComparer(this._defaultCulture));
			if (num < 0)
			{
				this._ignoredWordsList.Insert(~num, word);
				if (this._statusTable != null)
				{
					StaticTextPointer textPosition = this._textEditor.TextContainer.CreateStaticPointerAtOffset(0);
					char[] array = null;
					while (!textPosition.IsNull)
					{
						ITextPointer textPointer;
						ITextPointer textPointer2;
						if (this._statusTable.GetError(textPosition, LogicalDirection.Forward, out textPointer, out textPointer2))
						{
							string textInternal = TextRangeBase.GetTextInternal(textPointer, textPointer2, ref array);
							if (string.Compare(word, textInternal, true, this._defaultCulture) == 0)
							{
								this._statusTable.MarkCleanRange(textPointer, textPointer2);
							}
						}
						textPosition = this._statusTable.GetNextErrorTransition(textPosition, LogicalDirection.Forward);
					}
				}
			}
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x00259890 File Offset: 0x00258890
		internal void SetSpellingReform(SpellingReform spellingReform)
		{
			if (this._spellingReform != spellingReform)
			{
				this._spellingReform = spellingReform;
				this.ResetErrors();
			}
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x002598A8 File Offset: 0x002588A8
		internal void SetCustomDictionaries(CustomDictionarySources dictionaryLocations, bool add)
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (add)
			{
				using (IEnumerator<Uri> enumerator = dictionaryLocations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Uri uri = enumerator.Current;
						this.OnDictionaryUriAdded(uri);
					}
					return;
				}
			}
			this.OnDictionaryUriCollectionCleared();
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x00259904 File Offset: 0x00258904
		internal void ResetErrors()
		{
			if (this._statusTable != null)
			{
				this._statusTable.MarkDirtyRange(this._textEditor.TextContainer.Start, this._textEditor.TextContainer.End);
				if (this._textEditor.TextContainer.SymbolCount > 0)
				{
					this.ScheduleIdleCallback();
				}
			}
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x0025995D File Offset: 0x0025895D
		internal static bool IsSpellerAffectingProperty(DependencyProperty property)
		{
			return property == FrameworkElement.LanguageProperty || property == SpellCheck.SpellingReformProperty;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x00259974 File Offset: 0x00258974
		internal void OnDictionaryUriAdded(Uri uri)
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (this.UriMap.ContainsKey(uri))
			{
				this.OnDictionaryUriRemoved(uri);
			}
			if (!uri.IsAbsoluteUri || uri.IsFile)
			{
				Uri uri2 = Speller.ResolvePathUri(uri);
				object lexicon = this._spellerInterop.LoadDictionary(uri2.LocalPath);
				this.UriMap.Add(uri, new Speller.DictionaryInfo(uri2, lexicon));
			}
			else
			{
				this.LoadDictionaryFromPackUri(uri);
			}
			this.ResetErrors();
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x002599EC File Offset: 0x002589EC
		internal void OnDictionaryUriRemoved(Uri uri)
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (!this.UriMap.ContainsKey(uri))
			{
				return;
			}
			Speller.DictionaryInfo dictionaryInfo = this.UriMap[uri];
			try
			{
				this._spellerInterop.UnloadDictionary(dictionaryInfo.Lexicon);
			}
			catch (Exception ex)
			{
				Trace.Write(string.Format(CultureInfo.InvariantCulture, "Unloading dictionary failed. Original Uri:{0}, file Uri:{1}, exception:{2}", uri.ToString(), dictionaryInfo.PathUri.ToString(), ex.ToString()));
				throw;
			}
			this.UriMap.Remove(uri);
			this.ResetErrors();
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x00259A84 File Offset: 0x00258A84
		internal void OnDictionaryUriCollectionCleared()
		{
			if (!this.EnsureInitialized())
			{
				return;
			}
			this._spellerInterop.ReleaseAllLexicons();
			this.UriMap.Clear();
			this.ResetErrors();
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x060052DB RID: 21211 RVA: 0x00259AAB File Offset: 0x00258AAB
		internal SpellerStatusTable StatusTable
		{
			get
			{
				return this._statusTable;
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x060052DC RID: 21212 RVA: 0x00259AB3 File Offset: 0x00258AB3
		private Dictionary<Uri, Speller.DictionaryInfo> UriMap
		{
			get
			{
				if (this._uriMap == null)
				{
					this._uriMap = new Dictionary<Uri, Speller.DictionaryInfo>();
				}
				return this._uriMap;
			}
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x00259AD0 File Offset: 0x00258AD0
		private bool EnsureInitialized()
		{
			if (this._spellerInterop != null)
			{
				return true;
			}
			if (this._failedToInit)
			{
				return false;
			}
			Invariant.Assert(this._highlightLayer == null);
			Invariant.Assert(this._statusTable == null);
			this._spellerInterop = SpellerInteropBase.CreateInstance();
			this._failedToInit = (this._spellerInterop == null);
			if (this._failedToInit)
			{
				return false;
			}
			this._highlightLayer = new SpellerHighlightLayer(this);
			this._statusTable = new SpellerStatusTable(this._textEditor.TextContainer.Start, this._highlightLayer);
			this._textEditor.TextContainer.Highlights.AddLayer(this._highlightLayer);
			this._spellingReform = (SpellingReform)this._textEditor.UiScope.GetValue(SpellCheck.SpellingReformProperty);
			return true;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x00259B9A File Offset: 0x00258B9A
		private void ScheduleIdleCallback()
		{
			if (!this._pendingIdleCallback)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new DispatcherOperationCallback(this.OnIdle), null);
				this._pendingIdleCallback = true;
			}
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x00259BC4 File Offset: 0x00258BC4
		private void ScheduleCaretMovedCallback()
		{
			if (!this._pendingCaretMovedCallback)
			{
				this._textEditor.Selection.Changed += this.OnCaretMoved;
				this._textEditor.UiScope.LostFocus += this.OnLostFocus;
				this._pendingCaretMovedCallback = true;
			}
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x00259C18 File Offset: 0x00258C18
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs e)
		{
			Invariant.Assert(sender == this._textEditor.TextContainer);
			if (e.Count == 0 || (e.TextChange == TextChangeType.PropertyModified && !Speller.IsSpellerAffectingProperty(e.Property)))
			{
				return;
			}
			if (this._failedToInit)
			{
				return;
			}
			if (this._statusTable != null)
			{
				this._statusTable.OnTextChange(e);
			}
			this.ScheduleIdleCallback();
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x00259C7C File Offset: 0x00258C7C
		private object OnIdle(object unused)
		{
			Invariant.Assert(this._pendingIdleCallback);
			this._pendingIdleCallback = false;
			if (this._textEditor != null && this.EnsureInitialized())
			{
				long timeLimit = DateTime.Now.Ticks + 200000L;
				ITextPointer textPointer = null;
				Speller.ScanStatus scanStatus = null;
				ITextPointer start;
				while (this.GetNextScanRange(textPointer, out start, out textPointer))
				{
					scanStatus = this.ScanRange(start, textPointer, timeLimit);
					if (scanStatus.HasExceededTimeLimit)
					{
						break;
					}
				}
				if (scanStatus != null && scanStatus.HasExceededTimeLimit)
				{
					this.ScheduleIdleCallback();
				}
			}
			return null;
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x00259CF6 File Offset: 0x00258CF6
		private void OnCaretMoved(object sender, EventArgs e)
		{
			this.OnCaretMovedWorker();
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x00259CF6 File Offset: 0x00258CF6
		private void OnLostFocus(object sender, RoutedEventArgs e)
		{
			this.OnCaretMovedWorker();
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x00259D00 File Offset: 0x00258D00
		private void OnCaretMovedWorker()
		{
			if (!this._pendingCaretMovedCallback || this._textEditor == null)
			{
				return;
			}
			this._textEditor.Selection.Changed -= this.OnCaretMoved;
			this._textEditor.UiScope.LostFocus -= this.OnLostFocus;
			this._pendingCaretMovedCallback = false;
			this.ScheduleIdleCallback();
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x00259D64 File Offset: 0x00258D64
		private bool GetNextScanRange(ITextPointer searchStart, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			if (searchStart == null)
			{
				searchStart = this._textEditor.TextContainer.Start;
			}
			ITextPointer textPointer;
			ITextPointer rawEnd;
			this.GetNextScanRangeRaw(searchStart, out textPointer, out rawEnd);
			if (textPointer != null)
			{
				this.AdjustScanRangeAroundComposition(textPointer, rawEnd, out start, out end);
			}
			return start != null;
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x00259DAC File Offset: 0x00258DAC
		private void GetNextScanRangeRaw(ITextPointer searchStart, out ITextPointer start, out ITextPointer end)
		{
			Invariant.Assert(searchStart != null);
			start = null;
			end = null;
			this._statusTable.GetFirstDirtyRange(searchStart, out start, out end);
			if (start != null)
			{
				Invariant.Assert(start.CompareTo(end) < 0);
				if (start.GetOffsetToPosition(end) > 64)
				{
					end = start.CreatePointer(64);
				}
				XmlLanguage currentLanguage = this.GetCurrentLanguage(start);
				end = this.GetNextLanguageTransition(start, LogicalDirection.Forward, currentLanguage, end);
				Invariant.Assert(start.CompareTo(end) < 0);
			}
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x00259E2C File Offset: 0x00258E2C
		private void AdjustScanRangeAroundComposition(ITextPointer rawStart, ITextPointer rawEnd, out ITextPointer start, out ITextPointer end)
		{
			start = rawStart;
			end = rawEnd;
			if (!this._textEditor.Selection.IsEmpty)
			{
				return;
			}
			if (!this._textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			ITextPointer start2 = this._textEditor.Selection.Start;
			this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.WordBreaking;
			XmlLanguage currentLanguage = this.GetCurrentLanguage(start2);
			ITextPointer textPointer = this.SearchForWordBreaks(start2, LogicalDirection.Backward, currentLanguage, 1, false);
			ITextPointer textPointer2 = this.SearchForWordBreaks(start2, LogicalDirection.Forward, currentLanguage, 1, false);
			Speller.TextMap textMap = new Speller.TextMap(textPointer, textPointer2, start2, start2);
			ArrayList arrayList = new ArrayList(2);
			this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, new SpellerInteropBase.EnumTextSegmentsCallback(this.ExpandToWordBreakCallback), arrayList);
			if (arrayList.Count != 0)
			{
				int offset;
				int offset2;
				this.FindPositionInSegmentList(textMap, LogicalDirection.Backward, arrayList, out offset, out offset2);
				textPointer = textMap.MapOffsetToPosition(offset);
				textPointer2 = textMap.MapOffsetToPosition(offset2);
			}
			if (textPointer.CompareTo(rawEnd) < 0 && textPointer2.CompareTo(rawStart) > 0)
			{
				if (textPointer.CompareTo(rawStart) > 0)
				{
					end = textPointer;
				}
				else if (textPointer2.CompareTo(rawEnd) < 0)
				{
					start = textPointer2;
				}
				else
				{
					this.GetNextScanRangeRaw(textPointer2, out start, out end);
				}
				this.ScheduleCaretMovedCallback();
			}
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x00259F4C File Offset: 0x00258F4C
		private Speller.ScanStatus ScanRange(ITextPointer start, ITextPointer end, long timeLimit)
		{
			Speller.ScanStatus scanStatus = new Speller.ScanStatus(timeLimit, start);
			XmlLanguage language;
			CultureInfo currentCultureAndLanguage = this.GetCurrentCultureAndLanguage(start, out language);
			if (currentCultureAndLanguage == null)
			{
				this._statusTable.MarkCleanRange(start, end);
			}
			else
			{
				this.SetCulture(currentCultureAndLanguage);
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.ExpandToWordBreakAndContext(start, LogicalDirection.Backward, language, out textPointer, out textPointer2);
				ITextPointer textPointer3;
				ITextPointer textPointer4;
				this.ExpandToWordBreakAndContext(end, LogicalDirection.Forward, language, out textPointer3, out textPointer4);
				Invariant.Assert(textPointer.CompareTo(textPointer3) < 0);
				Invariant.Assert(textPointer2.CompareTo(textPointer4) < 0);
				Invariant.Assert(textPointer.CompareTo(textPointer2) >= 0);
				Invariant.Assert(textPointer3.CompareTo(textPointer4) <= 0);
				this._statusTable.MarkCleanRange(textPointer, textPointer3);
				if (this._spellerInterop.CanSpellCheck(currentCultureAndLanguage))
				{
					this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.SpellingErrors;
					Speller.TextMap textMap = new Speller.TextMap(textPointer2, textPointer4, textPointer, textPointer3);
					this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, new SpellerInteropBase.EnumSentencesCallback(this.ScanRangeCheckTimeLimitCallback), new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanTextSegment), new Speller.TextMapCallbackData(textMap, scanStatus));
					if (scanStatus.TimeoutPosition != null)
					{
						if (scanStatus.TimeoutPosition.CompareTo(end) < 0)
						{
							this._statusTable.MarkDirtyRange(scanStatus.TimeoutPosition, end);
							if (scanStatus.TimeoutPosition.CompareTo(start) <= 0)
							{
								string[] array = new string[33];
								array[0] = "Speller is not advancing! \nCulture = ";
								int num = 1;
								CultureInfo cultureInfo = currentCultureAndLanguage;
								array[num] = ((cultureInfo != null) ? cultureInfo.ToString() : null);
								array[2] = "\nStart offset = ";
								array[3] = start.Offset.ToString();
								array[4] = " parent = ";
								array[5] = start.ParentType.Name;
								array[6] = "\nContextStart offset = ";
								array[7] = textPointer2.Offset.ToString();
								array[8] = " parent = ";
								array[9] = textPointer2.ParentType.Name;
								array[10] = "\nContentStart offset = ";
								array[11] = textPointer.Offset.ToString();
								array[12] = " parent = ";
								array[13] = textPointer.ParentType.Name;
								array[14] = "\nContentEnd offset = ";
								array[15] = textPointer3.Offset.ToString();
								array[16] = " parent = ";
								array[17] = textPointer3.ParentType.Name;
								array[18] = "\nContextEnd offset = ";
								array[19] = textPointer4.Offset.ToString();
								array[20] = " parent = ";
								array[21] = textPointer4.ParentType.Name;
								array[22] = "\nTimeout offset = ";
								array[23] = scanStatus.TimeoutPosition.Offset.ToString();
								array[24] = " parent = ";
								array[25] = scanStatus.TimeoutPosition.ParentType.Name;
								array[26] = "\ntextMap TextLength = ";
								array[27] = textMap.TextLength.ToString();
								array[28] = " text = ";
								array[29] = new string(textMap.Text);
								array[30] = "\nDocument = ";
								array[31] = start.TextContainer.Parent.GetType().Name;
								array[32] = "\n";
								string text = string.Concat(array);
								if (start is TextPointer)
								{
									text = text + "Xml = " + new TextRange((TextPointer)start.TextContainer.Start, (TextPointer)start.TextContainer.End).Xml;
								}
								Invariant.Assert(false, text);
							}
						}
						else
						{
							Invariant.Assert(scanStatus.TimeoutPosition.CompareTo(textPointer3) <= 0);
						}
					}
				}
			}
			return scanStatus;
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x0025A2C8 File Offset: 0x002592C8
		private bool ScanErrorTextSegment(SpellerInteropBase.ISpellerSegment textSegment, object o)
		{
			Speller.TextMapCallbackData textMapCallbackData = (Speller.TextMapCallbackData)o;
			SpellerInteropBase.ITextRange textRange = textSegment.TextRange;
			if (textRange.Start + textRange.Length <= textMapCallbackData.TextMap.ContentStartOffset)
			{
				return true;
			}
			if (textRange.Start >= textMapCallbackData.TextMap.ContentEndOffset)
			{
				return false;
			}
			if (textRange.Length > 1)
			{
				if (textSegment.SubSegments.Count == 0)
				{
					ArrayList arrayList = (ArrayList)textMapCallbackData.Data;
					if (textSegment.Suggestions.Count <= 0)
					{
						return false;
					}
					using (IEnumerator<string> enumerator = textSegment.Suggestions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string value = enumerator.Current;
							arrayList.Add(value);
						}
						return false;
					}
				}
				textSegment.EnumSubSegments(new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanErrorTextSegment), textMapCallbackData);
			}
			return false;
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x0025A39C File Offset: 0x0025939C
		private bool ScanTextSegment(SpellerInteropBase.ISpellerSegment textSegment, object o)
		{
			Speller.TextMapCallbackData textMapCallbackData = (Speller.TextMapCallbackData)o;
			SpellerInteropBase.ITextRange textRange = textSegment.TextRange;
			if (textRange.Start + textRange.Length <= textMapCallbackData.TextMap.ContentStartOffset)
			{
				return true;
			}
			if (textRange.Start >= textMapCallbackData.TextMap.ContentEndOffset)
			{
				return false;
			}
			if (textRange.Length > 1)
			{
				char[] array = new char[textRange.Length];
				Array.Copy(textMapCallbackData.TextMap.Text, textRange.Start, array, 0, textRange.Length);
				if (!this.IsIgnoredWord(array) && !textSegment.IsClean)
				{
					if (textSegment.SubSegments.Count == 0)
					{
						this.MarkErrorRange(textMapCallbackData.TextMap, textRange);
					}
					else
					{
						textSegment.EnumSubSegments(new SpellerInteropBase.EnumTextSegmentsCallback(this.ScanTextSegment), textMapCallbackData);
					}
				}
			}
			return true;
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x0025A45C File Offset: 0x0025945C
		private bool ScanRangeCheckTimeLimitCallback(SpellerInteropBase.ISpellerSentence sentence, object o)
		{
			Speller.TextMapCallbackData textMapCallbackData = (Speller.TextMapCallbackData)o;
			Speller.ScanStatus scanStatus = (Speller.ScanStatus)textMapCallbackData.Data;
			if (scanStatus.HasExceededTimeLimit)
			{
				Invariant.Assert(scanStatus.TimeoutPosition == null);
				int endOffset = sentence.EndOffset;
				if (endOffset >= 0)
				{
					int num = Math.Min(textMapCallbackData.TextMap.ContentEndOffset, endOffset);
					if (num > textMapCallbackData.TextMap.ContentStartOffset)
					{
						ITextPointer textPointer = textMapCallbackData.TextMap.MapOffsetToPosition(num);
						if (textPointer.CompareTo(scanStatus.StartPosition) > 0)
						{
							scanStatus.TimeoutPosition = textPointer;
						}
					}
				}
			}
			return scanStatus.TimeoutPosition == null;
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x0025A4EC File Offset: 0x002594EC
		private void MarkErrorRange(Speller.TextMap textMap, SpellerInteropBase.ITextRange sTextRange)
		{
			if (sTextRange.Start + sTextRange.Length > textMap.ContentEndOffset)
			{
				return;
			}
			ITextPointer start = textMap.MapOffsetToPosition(sTextRange.Start);
			ITextPointer end = textMap.MapOffsetToPosition(sTextRange.Start + sTextRange.Length);
			if (sTextRange.Start < textMap.ContentStartOffset)
			{
				Invariant.Assert(sTextRange.Start + sTextRange.Length > textMap.ContentStartOffset);
				this._statusTable.MarkCleanRange(start, end);
			}
			this._statusTable.MarkErrorRange(start, end);
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x0025A574 File Offset: 0x00259574
		private void ExpandToWordBreakAndContext(ITextPointer position, LogicalDirection direction, XmlLanguage language, out ITextPointer contentPosition, out ITextPointer contextPosition)
		{
			contentPosition = position;
			contextPosition = position;
			if (position.GetPointerContext(direction) == TextPointerContext.None)
			{
				return;
			}
			this._spellerInterop.Mode = SpellerInteropBase.SpellerMode.WordBreaking;
			ITextPointer textPointer = this.SearchForWordBreaks(position, direction, language, 4, true);
			LogicalDirection direction2 = (direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
			ITextPointer textPointer2 = this.SearchForWordBreaks(position, direction2, language, 1, false);
			ITextPointer contextStart;
			ITextPointer contextEnd;
			if (direction == LogicalDirection.Backward)
			{
				contextStart = textPointer;
				contextEnd = textPointer2;
			}
			else
			{
				contextStart = textPointer2;
				contextEnd = textPointer;
			}
			Speller.TextMap textMap = new Speller.TextMap(contextStart, contextEnd, position, position);
			ArrayList arrayList = new ArrayList(5);
			this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, new SpellerInteropBase.EnumTextSegmentsCallback(this.ExpandToWordBreakCallback), arrayList);
			if (arrayList.Count != 0)
			{
				int num2;
				int num3;
				int num = this.FindPositionInSegmentList(textMap, direction, arrayList, out num2, out num3);
				int num4;
				if (direction == LogicalDirection.Backward)
				{
					num4 = ((textMap.ContentStartOffset == num3) ? num3 : num2);
				}
				else
				{
					num4 = ((textMap.ContentStartOffset == num2) ? num2 : num3);
				}
				Speller.TextMapOffsetErrorLogger textMapOffsetErrorLogger = new Speller.TextMapOffsetErrorLogger(direction, textMap, arrayList, num, num2, num3, num4);
				textMapOffsetErrorLogger.LogDebugInfo();
				contentPosition = textMap.MapOffsetToPosition(num4);
				int num5;
				if (direction == LogicalDirection.Backward)
				{
					num -= 3;
					SpellerInteropBase.ITextRange textRange = (SpellerInteropBase.ITextRange)arrayList[Math.Max(num, 0)];
					num5 = Math.Min(textRange.Start, num4);
				}
				else
				{
					num += 4;
					SpellerInteropBase.ITextRange textRange = (SpellerInteropBase.ITextRange)arrayList[Math.Min(num, arrayList.Count - 1)];
					num5 = Math.Max(textRange.Start + textRange.Length, num4);
				}
				textMapOffsetErrorLogger.ContextOffset = num5;
				textMapOffsetErrorLogger.LogDebugInfo();
				contextPosition = textMap.MapOffsetToPosition(num5);
			}
			if (direction == LogicalDirection.Backward)
			{
				if (position.CompareTo(contentPosition) < 0)
				{
					contentPosition = position;
				}
				if (position.CompareTo(contextPosition) < 0)
				{
					contextPosition = position;
					return;
				}
			}
			else
			{
				if (position.CompareTo(contentPosition) > 0)
				{
					contentPosition = position;
				}
				if (position.CompareTo(contextPosition) > 0)
				{
					contextPosition = position;
				}
			}
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x0025A744 File Offset: 0x00259744
		private int FindPositionInSegmentList(Speller.TextMap textMap, LogicalDirection direction, ArrayList segments, out int leftWordBreak, out int rightWordBreak)
		{
			leftWordBreak = int.MaxValue;
			rightWordBreak = -1;
			SpellerInteropBase.ITextRange textRange = (SpellerInteropBase.ITextRange)segments[0];
			int i;
			if (textMap.ContentStartOffset < textRange.Start)
			{
				leftWordBreak = 0;
				rightWordBreak = textRange.Start;
				i = -1;
			}
			else
			{
				textRange = (SpellerInteropBase.ITextRange)segments[segments.Count - 1];
				if (textMap.ContentStartOffset > textRange.Start + textRange.Length)
				{
					leftWordBreak = textRange.Start + textRange.Length;
					rightWordBreak = textMap.TextLength;
					i = segments.Count;
				}
				else
				{
					for (i = 0; i < segments.Count; i++)
					{
						textRange = (SpellerInteropBase.ITextRange)segments[i];
						leftWordBreak = textRange.Start;
						rightWordBreak = textRange.Start + textRange.Length;
						if (leftWordBreak <= textMap.ContentStartOffset && rightWordBreak >= textMap.ContentStartOffset)
						{
							break;
						}
						if (i < segments.Count - 1 && rightWordBreak < textMap.ContentStartOffset)
						{
							textRange = (SpellerInteropBase.ITextRange)segments[i + 1];
							leftWordBreak = rightWordBreak;
							rightWordBreak = textRange.Start;
							if (rightWordBreak > textMap.ContentStartOffset)
							{
								if (direction == LogicalDirection.Backward)
								{
									i++;
									break;
								}
								break;
							}
						}
					}
				}
			}
			Invariant.Assert(leftWordBreak <= textMap.ContentStartOffset && textMap.ContentStartOffset <= rightWordBreak);
			return i;
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x0025A898 File Offset: 0x00259898
		private ITextPointer SearchForWordBreaks(ITextPointer position, LogicalDirection direction, XmlLanguage language, int minWordCount, bool stopOnError)
		{
			ITextPointer textPointer = position.CreatePointer();
			ITextPointer textPointer2 = null;
			if (stopOnError)
			{
				StaticTextPointer nextErrorTransition = this._statusTable.GetNextErrorTransition(position.CreateStaticPointer(), direction);
				if (!nextErrorTransition.IsNull)
				{
					textPointer2 = nextErrorTransition.CreateDynamicTextPointer(LogicalDirection.Forward);
				}
			}
			bool flag = false;
			int num;
			do
			{
				textPointer.MoveByOffset((direction == LogicalDirection.Backward) ? -32 : 32);
				if (textPointer2 != null && ((direction == LogicalDirection.Backward && textPointer2.CompareTo(textPointer) > 0) || (direction == LogicalDirection.Forward && textPointer2.CompareTo(textPointer) < 0)))
				{
					textPointer.MoveToPosition(textPointer2);
					flag = true;
				}
				ITextPointer nextLanguageTransition = this.GetNextLanguageTransition(position, direction, language, textPointer);
				if ((direction == LogicalDirection.Backward && nextLanguageTransition.CompareTo(textPointer) > 0) || (direction == LogicalDirection.Forward && nextLanguageTransition.CompareTo(textPointer) < 0))
				{
					textPointer.MoveToPosition(nextLanguageTransition);
					flag = true;
				}
				ITextPointer textPointer3;
				ITextPointer textPointer4;
				if (direction == LogicalDirection.Backward)
				{
					textPointer3 = textPointer;
					textPointer4 = position;
				}
				else
				{
					textPointer3 = position;
					textPointer4 = textPointer;
				}
				Speller.TextMap textMap = new Speller.TextMap(textPointer3, textPointer4, textPointer3, textPointer4);
				num = this._spellerInterop.EnumTextSegments(textMap.Text, textMap.TextLength, null, null, null);
			}
			while (!flag && num < minWordCount + 1 && textPointer.GetPointerContext(direction) != TextPointerContext.None);
			return textPointer;
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x0025A998 File Offset: 0x00259998
		private ITextPointer GetNextLanguageTransition(ITextPointer position, LogicalDirection direction, XmlLanguage language, ITextPointer haltPosition)
		{
			ITextPointer textPointer = position.CreatePointer();
			while (((direction == LogicalDirection.Forward && textPointer.CompareTo(haltPosition) < 0) || (direction == LogicalDirection.Backward && textPointer.CompareTo(haltPosition) > 0)) && this.GetCurrentLanguage(textPointer) == language)
			{
				textPointer.MoveToNextContextPosition(direction);
			}
			if ((direction == LogicalDirection.Forward && textPointer.CompareTo(haltPosition) > 0) || (direction == LogicalDirection.Backward && textPointer.CompareTo(haltPosition) < 0))
			{
				textPointer.MoveToPosition(haltPosition);
			}
			return textPointer;
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x0025AA03 File Offset: 0x00259A03
		private bool ExpandToWordBreakCallback(SpellerInteropBase.ISpellerSegment textSegment, object o)
		{
			((ArrayList)o).Add(textSegment.TextRange);
			return true;
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x0025AA18 File Offset: 0x00259A18
		private bool IsIgnoredWord(char[] word)
		{
			bool result = false;
			if (this._ignoredWordsList != null)
			{
				result = (this._ignoredWordsList.BinarySearch(new string(word), new CaseInsensitiveComparer(this._defaultCulture)) >= 0);
			}
			return result;
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x0025AA54 File Offset: 0x00259A54
		private static bool CanSpellCheck(CultureInfo culture)
		{
			string twoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			return twoLetterISOLanguageName == "en" || twoLetterISOLanguageName == "de" || twoLetterISOLanguageName == "fr" || twoLetterISOLanguageName == "es";
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x0025AAA3 File Offset: 0x00259AA3
		private void SetCulture(CultureInfo culture)
		{
			this._spellerInterop.SetLocale(culture);
			this._spellerInterop.SetReformMode(culture, this._spellingReform);
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x0025AAC4 File Offset: 0x00259AC4
		private void ScanPosition(ITextPointer position, LogicalDirection direction)
		{
			ITextPointer start;
			ITextPointer end;
			if (direction == LogicalDirection.Forward)
			{
				start = position;
				end = position.CreatePointer(1);
			}
			else
			{
				start = position.CreatePointer(-1);
				end = position;
			}
			this.ScanRange(start, end, long.MaxValue);
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x0025AB00 File Offset: 0x00259B00
		private XmlLanguage GetCurrentLanguage(ITextPointer position)
		{
			XmlLanguage result;
			this.GetCurrentCultureAndLanguage(position, out result);
			return result;
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x0025AB18 File Offset: 0x00259B18
		private CultureInfo GetCurrentCultureAndLanguage(ITextPointer position, out XmlLanguage language)
		{
			bool flag;
			CultureInfo cultureInfo;
			if (!this._textEditor.AcceptsRichContent && this._textEditor.UiScope.GetValueSource(FrameworkElement.LanguageProperty, null, out flag) == BaseValueSourceInternal.Default)
			{
				cultureInfo = this._defaultCulture;
				language = XmlLanguage.GetLanguage(cultureInfo.IetfLanguageTag);
			}
			else
			{
				language = (XmlLanguage)position.GetValue(FrameworkElement.LanguageProperty);
				if (language == null)
				{
					cultureInfo = null;
				}
				else
				{
					try
					{
						cultureInfo = language.GetSpecificCulture();
					}
					catch (InvalidOperationException)
					{
						cultureInfo = null;
					}
				}
			}
			return cultureInfo;
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x0025ABA0 File Offset: 0x00259BA0
		private static Uri ResolvePathUri(Uri uri)
		{
			Uri result;
			if (!uri.IsAbsoluteUri)
			{
				result = new Uri(new Uri(Directory.GetCurrentDirectory() + "/"), uri);
			}
			else
			{
				result = uri;
			}
			return result;
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x0025ABD8 File Offset: 0x00259BD8
		private void LoadDictionaryFromPackUri(Uri item)
		{
			Uri uri = Speller.LoadPackFile(item);
			string tempPath = Path.GetTempPath();
			try
			{
				object lexicon = this._spellerInterop.LoadDictionary(uri, tempPath);
				this.UriMap.Add(item, new Speller.DictionaryInfo(uri, lexicon));
			}
			finally
			{
				this.CleanupDictionaryTempFile(uri);
			}
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x0025AC30 File Offset: 0x00259C30
		private void CleanupDictionaryTempFile(Uri tempLocationUri)
		{
			if (tempLocationUri != null)
			{
				try
				{
					File.Delete(tempLocationUri.LocalPath);
				}
				catch (Exception ex)
				{
					Trace.Write(string.Format(CultureInfo.InvariantCulture, "Failure to delete temporary file with custom dictionary data. file Uri:{0},exception:{1}", tempLocationUri.ToString(), ex.ToString()));
					throw;
				}
			}
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0025AC88 File Offset: 0x00259C88
		private static Uri LoadPackFile(Uri uri)
		{
			Invariant.Assert(PackUriHelper.IsPackUri(uri));
			string uriString;
			using (Stream stream = WpfWebRequestHelper.CreateRequestAndGetResponseStream(BindUriHelper.GetResolvedUri(BaseUriHelper.PackAppBaseUri, uri)))
			{
				using (FileStream fileStream = FileHelper.CreateAndOpenTemporaryFile(out uriString, FileAccess.ReadWrite, FileOptions.None, null, "WPF"))
				{
					stream.CopyTo(fileStream);
				}
			}
			return new Uri(uriString);
		}

		// Token: 0x04002EBF RID: 11967
		private const int MaxIdleTimeSliceMs = 20;

		// Token: 0x04002EC0 RID: 11968
		private const long MaxIdleTimeSliceNs = 200000L;

		// Token: 0x04002EC1 RID: 11969
		private const int MaxScanBlockSize = 64;

		// Token: 0x04002EC2 RID: 11970
		private const int ContextBlockSize = 32;

		// Token: 0x04002EC3 RID: 11971
		private const int MinWordBreaksForContext = 4;

		// Token: 0x04002EC4 RID: 11972
		private TextEditor _textEditor;

		// Token: 0x04002EC5 RID: 11973
		private SpellerStatusTable _statusTable;

		// Token: 0x04002EC6 RID: 11974
		private SpellerHighlightLayer _highlightLayer;

		// Token: 0x04002EC7 RID: 11975
		private SpellerInteropBase _spellerInterop;

		// Token: 0x04002EC8 RID: 11976
		private SpellingReform _spellingReform;

		// Token: 0x04002EC9 RID: 11977
		private bool _pendingIdleCallback;

		// Token: 0x04002ECA RID: 11978
		private bool _pendingCaretMovedCallback;

		// Token: 0x04002ECB RID: 11979
		private ArrayList _ignoredWordsList;

		// Token: 0x04002ECC RID: 11980
		private readonly CultureInfo _defaultCulture;

		// Token: 0x04002ECD RID: 11981
		private bool _failedToInit;

		// Token: 0x04002ECE RID: 11982
		private Dictionary<Uri, Speller.DictionaryInfo> _uriMap;

		// Token: 0x02000B4E RID: 2894
		private class TextMap
		{
			// Token: 0x06008D89 RID: 36233 RVA: 0x0033E6C0 File Offset: 0x0033D6C0
			internal TextMap(ITextPointer contextStart, ITextPointer contextEnd, ITextPointer contentStart, ITextPointer contentEnd)
			{
				Invariant.Assert(contextStart.CompareTo(contentStart) <= 0);
				Invariant.Assert(contextEnd.CompareTo(contentEnd) >= 0);
				this._basePosition = contextStart.GetFrozenPointer(LogicalDirection.Backward);
				ITextPointer textPointer = contextStart.CreatePointer();
				int offsetToPosition = contextStart.GetOffsetToPosition(contextEnd);
				this._text = new char[offsetToPosition];
				this._positionMap = new int[offsetToPosition + 1];
				this._textLength = 0;
				int num = 0;
				this._contentStartOffset = 0;
				this._contentEndOffset = 0;
				while (textPointer.CompareTo(contextEnd) < 0)
				{
					if (textPointer.CompareTo(contentStart) == 0)
					{
						this._contentStartOffset = this._textLength;
					}
					if (textPointer.CompareTo(contentEnd) == 0)
					{
						this._contentEndOffset = this._textLength;
					}
					switch (textPointer.GetPointerContext(LogicalDirection.Forward))
					{
					case TextPointerContext.Text:
					{
						int num2 = textPointer.GetTextRunLength(LogicalDirection.Forward);
						num2 = Math.Min(num2, this._text.Length - this._textLength);
						num2 = Math.Min(num2, textPointer.GetOffsetToPosition(contextEnd));
						textPointer.GetTextInRun(LogicalDirection.Forward, this._text, this._textLength, num2);
						for (int i = this._textLength; i < this._textLength + num2; i++)
						{
							this._positionMap[i] = i + num;
						}
						int offsetToPosition2 = textPointer.GetOffsetToPosition(contentStart);
						if (offsetToPosition2 >= 0 && offsetToPosition2 <= num2)
						{
							this._contentStartOffset = this._textLength + textPointer.GetOffsetToPosition(contentStart);
						}
						offsetToPosition2 = textPointer.GetOffsetToPosition(contentEnd);
						if (offsetToPosition2 >= 0 && offsetToPosition2 <= num2)
						{
							this._contentEndOffset = this._textLength + textPointer.GetOffsetToPosition(contentEnd);
						}
						textPointer.MoveByOffset(num2);
						this._textLength += num2;
						break;
					}
					case TextPointerContext.EmbeddedElement:
						this._text[this._textLength] = '';
						this._positionMap[this._textLength] = this._textLength + num;
						this._textLength++;
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						break;
					case TextPointerContext.ElementStart:
					case TextPointerContext.ElementEnd:
						if (this.IsAdjacentToFormatElement(textPointer))
						{
							num++;
						}
						else
						{
							this._text[this._textLength] = ' ';
							this._positionMap[this._textLength] = this._textLength + num;
							this._textLength++;
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						break;
					}
				}
				if (textPointer.CompareTo(contentEnd) == 0)
				{
					this._contentEndOffset = this._textLength;
				}
				if (this._textLength > 0)
				{
					this._positionMap[this._textLength] = this._positionMap[this._textLength - 1] + 1;
				}
				else
				{
					this._positionMap[0] = 0;
				}
				Invariant.Assert(this._contentStartOffset <= this._contentEndOffset);
			}

			// Token: 0x06008D8A RID: 36234 RVA: 0x0033E964 File Offset: 0x0033D964
			internal ITextPointer MapOffsetToPosition(int offset)
			{
				Invariant.Assert(offset >= 0 && offset <= this._textLength);
				return this._basePosition.CreatePointer(this._positionMap[offset]);
			}

			// Token: 0x17001EDF RID: 7903
			// (get) Token: 0x06008D8B RID: 36235 RVA: 0x0033E991 File Offset: 0x0033D991
			internal int ContentStartOffset
			{
				get
				{
					return this._contentStartOffset;
				}
			}

			// Token: 0x17001EE0 RID: 7904
			// (get) Token: 0x06008D8C RID: 36236 RVA: 0x0033E999 File Offset: 0x0033D999
			internal int ContentEndOffset
			{
				get
				{
					return this._contentEndOffset;
				}
			}

			// Token: 0x17001EE1 RID: 7905
			// (get) Token: 0x06008D8D RID: 36237 RVA: 0x0033E9A1 File Offset: 0x0033D9A1
			internal char[] Text
			{
				get
				{
					return this._text;
				}
			}

			// Token: 0x17001EE2 RID: 7906
			// (get) Token: 0x06008D8E RID: 36238 RVA: 0x0033E9A9 File Offset: 0x0033D9A9
			internal int TextLength
			{
				get
				{
					return this._textLength;
				}
			}

			// Token: 0x06008D8F RID: 36239 RVA: 0x0033E9B4 File Offset: 0x0033D9B4
			private bool IsAdjacentToFormatElement(ITextPointer pointer)
			{
				bool result = false;
				TextPointerContext pointerContext = pointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart && TextSchema.IsFormattingType(pointer.GetElementType(LogicalDirection.Forward)))
				{
					result = true;
				}
				else if (pointerContext == TextPointerContext.ElementEnd && TextSchema.IsFormattingType(pointer.ParentType))
				{
					result = true;
				}
				return result;
			}

			// Token: 0x0400488A RID: 18570
			private readonly ITextPointer _basePosition;

			// Token: 0x0400488B RID: 18571
			private readonly char[] _text;

			// Token: 0x0400488C RID: 18572
			private readonly int[] _positionMap;

			// Token: 0x0400488D RID: 18573
			private readonly int _textLength;

			// Token: 0x0400488E RID: 18574
			private readonly int _contentStartOffset;

			// Token: 0x0400488F RID: 18575
			private readonly int _contentEndOffset;
		}

		// Token: 0x02000B4F RID: 2895
		private class ScanStatus
		{
			// Token: 0x06008D90 RID: 36240 RVA: 0x0033E9F5 File Offset: 0x0033D9F5
			internal ScanStatus(long timeLimit, ITextPointer startPosition)
			{
				this._timeLimit = timeLimit;
				this._startPosition = startPosition;
			}

			// Token: 0x17001EE3 RID: 7907
			// (get) Token: 0x06008D91 RID: 36241 RVA: 0x0033EA0C File Offset: 0x0033DA0C
			internal bool HasExceededTimeLimit
			{
				get
				{
					return DateTime.Now.Ticks >= this._timeLimit;
				}
			}

			// Token: 0x17001EE4 RID: 7908
			// (get) Token: 0x06008D92 RID: 36242 RVA: 0x0033EA31 File Offset: 0x0033DA31
			// (set) Token: 0x06008D93 RID: 36243 RVA: 0x0033EA39 File Offset: 0x0033DA39
			internal ITextPointer TimeoutPosition
			{
				get
				{
					return this._timeoutPosition;
				}
				set
				{
					this._timeoutPosition = value;
				}
			}

			// Token: 0x17001EE5 RID: 7909
			// (get) Token: 0x06008D94 RID: 36244 RVA: 0x0033EA42 File Offset: 0x0033DA42
			internal ITextPointer StartPosition
			{
				get
				{
					return this._startPosition;
				}
			}

			// Token: 0x04004890 RID: 18576
			private readonly long _timeLimit;

			// Token: 0x04004891 RID: 18577
			private readonly ITextPointer _startPosition;

			// Token: 0x04004892 RID: 18578
			private ITextPointer _timeoutPosition;
		}

		// Token: 0x02000B50 RID: 2896
		private class TextMapCallbackData
		{
			// Token: 0x06008D95 RID: 36245 RVA: 0x0033EA4A File Offset: 0x0033DA4A
			internal TextMapCallbackData(Speller.TextMap textmap, object data)
			{
				this._textmap = textmap;
				this._data = data;
			}

			// Token: 0x17001EE6 RID: 7910
			// (get) Token: 0x06008D96 RID: 36246 RVA: 0x0033EA60 File Offset: 0x0033DA60
			internal Speller.TextMap TextMap
			{
				get
				{
					return this._textmap;
				}
			}

			// Token: 0x17001EE7 RID: 7911
			// (get) Token: 0x06008D97 RID: 36247 RVA: 0x0033EA68 File Offset: 0x0033DA68
			internal object Data
			{
				get
				{
					return this._data;
				}
			}

			// Token: 0x04004893 RID: 18579
			private readonly Speller.TextMap _textmap;

			// Token: 0x04004894 RID: 18580
			private readonly object _data;
		}

		// Token: 0x02000B51 RID: 2897
		private class DictionaryInfo
		{
			// Token: 0x06008D98 RID: 36248 RVA: 0x0033EA70 File Offset: 0x0033DA70
			internal DictionaryInfo(Uri pathUri, object lexicon)
			{
				this._pathUri = pathUri;
				this._lexicon = lexicon;
			}

			// Token: 0x17001EE8 RID: 7912
			// (get) Token: 0x06008D99 RID: 36249 RVA: 0x0033EA86 File Offset: 0x0033DA86
			internal Uri PathUri
			{
				get
				{
					return this._pathUri;
				}
			}

			// Token: 0x17001EE9 RID: 7913
			// (get) Token: 0x06008D9A RID: 36250 RVA: 0x0033EA8E File Offset: 0x0033DA8E
			internal object Lexicon
			{
				get
				{
					return this._lexicon;
				}
			}

			// Token: 0x04004895 RID: 18581
			private readonly object _lexicon;

			// Token: 0x04004896 RID: 18582
			private readonly Uri _pathUri;
		}

		// Token: 0x02000B52 RID: 2898
		private class TextMapOffsetErrorLogger
		{
			// Token: 0x06008D9B RID: 36251 RVA: 0x0033EA98 File Offset: 0x0033DA98
			public TextMapOffsetErrorLogger(LogicalDirection direction, Speller.TextMap textMap, ArrayList segments, int positionInSegmentList, int leftWordBreak, int rightWordBreak, int contentOffset)
			{
				this._debugInfo = new Speller.TextMapOffsetErrorLogger.DebugInfo
				{
					Direction = direction.ToString(),
					SegmentCount = segments.Count,
					SegmentStartsAndLengths = new Speller.TextMapOffsetErrorLogger.SegmentInfo[segments.Count],
					PositionInSegmentList = positionInSegmentList,
					LeftWordBreak = leftWordBreak,
					RightWordBreak = rightWordBreak,
					ContentOffSet = contentOffset,
					ContextOffset = Speller.TextMapOffsetErrorLogger.UnsetValue,
					CalculationMode = Speller.TextMapOffsetErrorLogger.CalculationModes.ContentPosition,
					TextMapText = string.Join<char>(string.Empty, textMap.Text),
					TextMapTextLength = textMap.TextLength,
					TextMapContentStartOffset = textMap.ContentStartOffset,
					TextMapContentEndOffset = textMap.ContentEndOffset
				};
				for (int i = 0; i < segments.Count; i++)
				{
					SpellerInteropBase.ITextRange textRange = segments[i] as SpellerInteropBase.ITextRange;
					if (textRange != null)
					{
						this._debugInfo.SegmentStartsAndLengths[i] = new Speller.TextMapOffsetErrorLogger.SegmentInfo
						{
							Start = textRange.Start,
							Length = textRange.Length
						};
					}
				}
			}

			// Token: 0x17001EEA RID: 7914
			// (set) Token: 0x06008D9C RID: 36252 RVA: 0x0033EBB8 File Offset: 0x0033DBB8
			public int ContextOffset
			{
				set
				{
					this._debugInfo.ContextOffset = value;
					this._debugInfo.CalculationMode = Speller.TextMapOffsetErrorLogger.CalculationModes.ContextPosition;
				}
			}

			// Token: 0x06008D9D RID: 36253 RVA: 0x0033EBD4 File Offset: 0x0033DBD4
			public void LogDebugInfo()
			{
				int num = (this._debugInfo.CalculationMode == Speller.TextMapOffsetErrorLogger.CalculationModes.ContentPosition) ? this._debugInfo.ContentOffSet : this._debugInfo.ContextOffset;
				if (num < 0 || num > this._debugInfo.TextMapTextLength)
				{
					EventSource provider = TraceLoggingProvider.GetProvider();
					EventSourceOptions options = new EventSourceOptions
					{
						Keywords = (EventKeywords)70368744177664L,
						Tags = (EventTags)33554432
					};
					if (provider == null)
					{
						return;
					}
					provider.Write<Speller.TextMapOffsetErrorLogger.DebugInfo>(Speller.TextMapOffsetErrorLogger.TextMapOffsetError, options, this._debugInfo);
				}
			}

			// Token: 0x04004897 RID: 18583
			private static readonly string TextMapOffsetError = "TextMapOffsetError";

			// Token: 0x04004898 RID: 18584
			private Speller.TextMapOffsetErrorLogger.DebugInfo _debugInfo;

			// Token: 0x04004899 RID: 18585
			private static readonly int UnsetValue = -2;

			// Token: 0x02000C8D RID: 3213
			public enum CalculationModes
			{
				// Token: 0x04004FB7 RID: 20407
				ContentPosition,
				// Token: 0x04004FB8 RID: 20408
				ContextPosition
			}

			// Token: 0x02000C8E RID: 3214
			[EventData]
			private struct DebugInfo
			{
				// Token: 0x17001FFD RID: 8189
				// (get) Token: 0x06009557 RID: 38231 RVA: 0x0034DEAA File Offset: 0x0034CEAA
				// (set) Token: 0x06009558 RID: 38232 RVA: 0x0034DEB2 File Offset: 0x0034CEB2
				public string Direction { readonly get; set; }

				// Token: 0x17001FFE RID: 8190
				// (get) Token: 0x06009559 RID: 38233 RVA: 0x0034DEBB File Offset: 0x0034CEBB
				// (set) Token: 0x0600955A RID: 38234 RVA: 0x0034DEC3 File Offset: 0x0034CEC3
				public int SegmentCount { readonly get; set; }

				// Token: 0x17001FFF RID: 8191
				// (get) Token: 0x0600955B RID: 38235 RVA: 0x0034DECC File Offset: 0x0034CECC
				// (set) Token: 0x0600955C RID: 38236 RVA: 0x0034DED4 File Offset: 0x0034CED4
				public Speller.TextMapOffsetErrorLogger.SegmentInfo[] SegmentStartsAndLengths { readonly get; set; }

				// Token: 0x17002000 RID: 8192
				// (get) Token: 0x0600955D RID: 38237 RVA: 0x0034DEDD File Offset: 0x0034CEDD
				// (set) Token: 0x0600955E RID: 38238 RVA: 0x0034DEE5 File Offset: 0x0034CEE5
				public int PositionInSegmentList { readonly get; set; }

				// Token: 0x17002001 RID: 8193
				// (get) Token: 0x0600955F RID: 38239 RVA: 0x0034DEEE File Offset: 0x0034CEEE
				// (set) Token: 0x06009560 RID: 38240 RVA: 0x0034DEF6 File Offset: 0x0034CEF6
				public int LeftWordBreak { readonly get; set; }

				// Token: 0x17002002 RID: 8194
				// (get) Token: 0x06009561 RID: 38241 RVA: 0x0034DEFF File Offset: 0x0034CEFF
				// (set) Token: 0x06009562 RID: 38242 RVA: 0x0034DF07 File Offset: 0x0034CF07
				public int RightWordBreak { readonly get; set; }

				// Token: 0x17002003 RID: 8195
				// (get) Token: 0x06009563 RID: 38243 RVA: 0x0034DF10 File Offset: 0x0034CF10
				// (set) Token: 0x06009564 RID: 38244 RVA: 0x0034DF18 File Offset: 0x0034CF18
				public int ContentOffSet { readonly get; set; }

				// Token: 0x17002004 RID: 8196
				// (get) Token: 0x06009565 RID: 38245 RVA: 0x0034DF21 File Offset: 0x0034CF21
				// (set) Token: 0x06009566 RID: 38246 RVA: 0x0034DF29 File Offset: 0x0034CF29
				public int ContextOffset { readonly get; set; }

				// Token: 0x17002005 RID: 8197
				// (get) Token: 0x06009567 RID: 38247 RVA: 0x0034DF32 File Offset: 0x0034CF32
				// (set) Token: 0x06009568 RID: 38248 RVA: 0x0034DF3A File Offset: 0x0034CF3A
				public Speller.TextMapOffsetErrorLogger.CalculationModes CalculationMode { readonly get; set; }

				// Token: 0x17002006 RID: 8198
				// (get) Token: 0x06009569 RID: 38249 RVA: 0x0034DF43 File Offset: 0x0034CF43
				// (set) Token: 0x0600956A RID: 38250 RVA: 0x0034DF4B File Offset: 0x0034CF4B
				public string TextMapText { readonly get; set; }

				// Token: 0x17002007 RID: 8199
				// (get) Token: 0x0600956B RID: 38251 RVA: 0x0034DF54 File Offset: 0x0034CF54
				// (set) Token: 0x0600956C RID: 38252 RVA: 0x0034DF5C File Offset: 0x0034CF5C
				public int TextMapTextLength { readonly get; set; }

				// Token: 0x17002008 RID: 8200
				// (get) Token: 0x0600956D RID: 38253 RVA: 0x0034DF65 File Offset: 0x0034CF65
				// (set) Token: 0x0600956E RID: 38254 RVA: 0x0034DF6D File Offset: 0x0034CF6D
				public int TextMapContentStartOffset { readonly get; set; }

				// Token: 0x17002009 RID: 8201
				// (get) Token: 0x0600956F RID: 38255 RVA: 0x0034DF76 File Offset: 0x0034CF76
				// (set) Token: 0x06009570 RID: 38256 RVA: 0x0034DF7E File Offset: 0x0034CF7E
				public int TextMapContentEndOffset { readonly get; set; }
			}

			// Token: 0x02000C8F RID: 3215
			[EventData]
			private struct SegmentInfo
			{
				// Token: 0x1700200A RID: 8202
				// (get) Token: 0x06009571 RID: 38257 RVA: 0x0034DF87 File Offset: 0x0034CF87
				// (set) Token: 0x06009572 RID: 38258 RVA: 0x0034DF8F File Offset: 0x0034CF8F
				public int Start { readonly get; set; }

				// Token: 0x1700200B RID: 8203
				// (get) Token: 0x06009573 RID: 38259 RVA: 0x0034DF98 File Offset: 0x0034CF98
				// (set) Token: 0x06009574 RID: 38260 RVA: 0x0034DFA0 File Offset: 0x0034CFA0
				public int Length { readonly get; set; }
			}
		}
	}
}
