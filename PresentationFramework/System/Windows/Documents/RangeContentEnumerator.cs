using System;
using System.Collections;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000652 RID: 1618
	internal class RangeContentEnumerator : IEnumerator
	{
		// Token: 0x06005015 RID: 20501 RVA: 0x002453E8 File Offset: 0x002443E8
		internal RangeContentEnumerator(TextPointer start, TextPointer end)
		{
			Invariant.Assert((start != null && end != null) || (start == null && end == null), "If start is null end should be null!");
			this._start = start;
			this._end = end;
			if (this._start != null)
			{
				this._generation = this._start.TextContainer.Generation;
			}
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x00245444 File Offset: 0x00244444
		public object Current
		{
			get
			{
				if (this._navigator == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
				}
				if (this._currentCache != null)
				{
					return this._currentCache;
				}
				if (this._navigator.CompareTo(this._end) >= 0)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
				if (this._generation != this._start.TextContainer.Generation && !this.IsLogicalChildrenIterationInProgress)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				switch (this._navigator.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.Text:
				{
					int num = 0;
					do
					{
						int textRunLength = this._navigator.GetTextRunLength(LogicalDirection.Forward);
						this.EnsureBufferCapacity(num + textRunLength);
						this._navigator.GetTextInRun(LogicalDirection.Forward, this._buffer, num, textRunLength);
						num += textRunLength;
						this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					while (this._navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
					this._currentCache = new string(this._buffer, 0, num);
					break;
				}
				case TextPointerContext.EmbeddedElement:
					this._currentCache = this._navigator.GetAdjacentElement(LogicalDirection.Forward);
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				case TextPointerContext.ElementStart:
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					this._currentCache = this._navigator.Parent;
					this._navigator.MoveToElementEdge(ElementEdge.AfterEnd);
					break;
				default:
					Invariant.Assert(false, "Unexpected run type!");
					this._currentCache = null;
					break;
				}
				return this._currentCache;
			}
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x002455BC File Offset: 0x002445BC
		public bool MoveNext()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation && !this.IsLogicalChildrenIterationInProgress)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
			if (this._start == null || this._start.CompareTo(this._end) == 0)
			{
				return false;
			}
			if (this._navigator != null && this._navigator.CompareTo(this._end) >= 0)
			{
				return false;
			}
			if (this._navigator == null)
			{
				this._navigator = new TextPointer(this._start);
			}
			else if (this._currentCache == null)
			{
				switch (this._navigator.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.Text:
					do
					{
						this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
						if (this._navigator.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
						{
							break;
						}
					}
					while (this._navigator.CompareTo(this._end) < 0);
					break;
				case TextPointerContext.EmbeddedElement:
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				case TextPointerContext.ElementStart:
					this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
					this._navigator.MoveToPosition(((TextElement)this._navigator.Parent).ElementEnd);
					break;
				default:
					Invariant.Assert(false, "Unexpected run type!");
					break;
				}
			}
			this._currentCache = null;
			return this._navigator.CompareTo(this._end) < 0;
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x0024571E File Offset: 0x0024471E
		public void Reset()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
			this._navigator = null;
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x00245758 File Offset: 0x00244758
		private void EnsureBufferCapacity(int size)
		{
			if (this._buffer == null)
			{
				this._buffer = new char[size];
				return;
			}
			if (this._buffer.Length < size)
			{
				char[] array = new char[Math.Max(2 * this._buffer.Length, size)];
				this._buffer.CopyTo(array, 0);
				this._buffer = array;
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x0600501A RID: 20506 RVA: 0x002457B0 File Offset: 0x002447B0
		private bool IsLogicalChildrenIterationInProgress
		{
			get
			{
				for (DependencyObject parent = this._start.Parent; parent != null; parent = LogicalTreeHelper.GetParent(parent))
				{
					FrameworkElement frameworkElement = parent as FrameworkElement;
					if (frameworkElement != null)
					{
						if (frameworkElement.IsLogicalChildrenIterationInProgress)
						{
							return true;
						}
					}
					else
					{
						FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
						if (frameworkContentElement != null && frameworkContentElement.IsLogicalChildrenIterationInProgress)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0400288D RID: 10381
		private readonly TextPointer _start;

		// Token: 0x0400288E RID: 10382
		private readonly TextPointer _end;

		// Token: 0x0400288F RID: 10383
		private readonly uint _generation;

		// Token: 0x04002890 RID: 10384
		private TextPointer _navigator;

		// Token: 0x04002891 RID: 10385
		private object _currentCache;

		// Token: 0x04002892 RID: 10386
		private char[] _buffer;
	}
}
