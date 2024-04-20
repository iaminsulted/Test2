using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006B1 RID: 1713
	internal class TextElementEnumerator<TextElementType> : IEnumerator<!0>, IEnumerator, IDisposable where TextElementType : TextElement
	{
		// Token: 0x0600571C RID: 22300 RVA: 0x0026CD38 File Offset: 0x0026BD38
		internal TextElementEnumerator(TextPointer start, TextPointer end)
		{
			Invariant.Assert((start != null && end != null) || (start == null && end == null), "If start is null end should be null!");
			this._start = start;
			this._end = end;
			if (this._start != null)
			{
				this._generation = this._start.TextContainer.Generation;
			}
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x0026CD94 File Offset: 0x0026BD94
		public void Dispose()
		{
			this._current = default(TextElementType);
			this._navigator = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x0600571E RID: 22302 RVA: 0x0026CDAF File Offset: 0x0026BDAF
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x0026CDBC File Offset: 0x0026BDBC
		public TextElementType Current
		{
			get
			{
				if (this._navigator == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
				}
				if (this._current == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
				return this._current;
			}
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x0026CDFC File Offset: 0x0026BDFC
		public bool MoveNext()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation)
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
				this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			else
			{
				Invariant.Assert(this._navigator.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart, "Unexpected run type in TextElementEnumerator");
				this._navigator.MoveToElementEdge(ElementEdge.AfterEnd);
				this._navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			if (this._navigator.CompareTo(this._end) < 0)
			{
				this._current = (TextElementType)((object)this._navigator.Parent);
			}
			else
			{
				this._current = default(TextElementType);
			}
			return this._current != null;
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x0026CF14 File Offset: 0x0026BF14
		public void Reset()
		{
			if (this._start != null && this._generation != this._start.TextContainer.Generation)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
			this._navigator = null;
			this._current = default(TextElementType);
		}

		// Token: 0x04002FB4 RID: 12212
		private readonly TextPointer _start;

		// Token: 0x04002FB5 RID: 12213
		private readonly TextPointer _end;

		// Token: 0x04002FB6 RID: 12214
		private readonly uint _generation;

		// Token: 0x04002FB7 RID: 12215
		private TextPointer _navigator;

		// Token: 0x04002FB8 RID: 12216
		private TextElementType _current;
	}
}
