using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows.Documents
{
	// Token: 0x020005D4 RID: 1492
	internal class HostedElements : IEnumerator<IInputElement>, IEnumerator, IDisposable
	{
		// Token: 0x06004800 RID: 18432 RVA: 0x0022AC4B File Offset: 0x00229C4B
		internal HostedElements(ReadOnlyCollection<TextSegment> textSegments)
		{
			this._textSegments = textSegments;
			this._currentPosition = null;
			this._currentTextSegment = 0;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0022AC68 File Offset: 0x00229C68
		void IDisposable.Dispose()
		{
			this._textSegments = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x0022AC78 File Offset: 0x00229C78
		public bool MoveNext()
		{
			if (this._textSegments == null)
			{
				throw new ObjectDisposedException("HostedElements");
			}
			if (this._textSegments.Count == 0)
			{
				return false;
			}
			if (this._currentPosition == null)
			{
				if (!(this._textSegments[0].Start is TextPointer))
				{
					this._currentPosition = null;
					return false;
				}
				this._currentPosition = new TextPointer(this._textSegments[0].Start as TextPointer);
			}
			else if (this._currentTextSegment < this._textSegments.Count)
			{
				this._currentPosition.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			while (this._currentTextSegment < this._textSegments.Count)
			{
				while (((ITextPointer)this._currentPosition).CompareTo(this._textSegments[this._currentTextSegment].End) < 0)
				{
					if (this._currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart || this._currentPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement)
					{
						return true;
					}
					this._currentPosition.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				this._currentTextSegment++;
				if (this._currentTextSegment < this._textSegments.Count)
				{
					if (!(this._textSegments[this._currentTextSegment].Start is TextPointer))
					{
						this._currentPosition = null;
						return false;
					}
					this._currentPosition = new TextPointer(this._textSegments[this._currentTextSegment].Start as TextPointer);
				}
			}
			return false;
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06004803 RID: 18435 RVA: 0x0022AE04 File Offset: 0x00229E04
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x001056E1 File Offset: 0x001046E1
		void IEnumerator.Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06004805 RID: 18437 RVA: 0x0022AE0C File Offset: 0x00229E0C
		public IInputElement Current
		{
			get
			{
				if (this._textSegments == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorCollectionDisposed"));
				}
				if (this._currentPosition == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
				}
				IInputElement result = null;
				TextPointerContext pointerContext = this._currentPosition.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext != TextPointerContext.EmbeddedElement)
				{
					if (pointerContext == TextPointerContext.ElementStart)
					{
						result = this._currentPosition.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					}
				}
				else
				{
					result = (IInputElement)this._currentPosition.GetAdjacentElement(LogicalDirection.Forward);
				}
				return result;
			}
		}

		// Token: 0x040025F2 RID: 9714
		private ReadOnlyCollection<TextSegment> _textSegments;

		// Token: 0x040025F3 RID: 9715
		private TextPointer _currentPosition;

		// Token: 0x040025F4 RID: 9716
		private int _currentTextSegment;
	}
}
