using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200061F RID: 1567
	public class FrameworkTextComposition : TextComposition
	{
		// Token: 0x06004D26 RID: 19750 RVA: 0x0023EA60 File Offset: 0x0023DA60
		internal FrameworkTextComposition(InputManager inputManager, IInputElement source, object owner) : base(inputManager, source, string.Empty, TextCompositionAutoComplete.Off)
		{
			this._owner = owner;
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0023EA77 File Offset: 0x0023DA77
		public override void Complete()
		{
			this._pendingComplete = true;
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06004D28 RID: 19752 RVA: 0x0023EA80 File Offset: 0x0023DA80
		public int ResultOffset
		{
			get
			{
				if (this._ResultStart != null)
				{
					return this._offset;
				}
				return -1;
			}
		}

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0023EA92 File Offset: 0x0023DA92
		public int ResultLength
		{
			get
			{
				if (this._ResultStart != null)
				{
					return this._length;
				}
				return -1;
			}
		}

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x06004D2A RID: 19754 RVA: 0x0023EAA4 File Offset: 0x0023DAA4
		public int CompositionOffset
		{
			get
			{
				if (this._CompositionStart != null)
				{
					return this._offset;
				}
				return -1;
			}
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x06004D2B RID: 19755 RVA: 0x0023EAB6 File Offset: 0x0023DAB6
		public int CompositionLength
		{
			get
			{
				if (this._CompositionStart != null)
				{
					return this._length;
				}
				return -1;
			}
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x0023EAC8 File Offset: 0x0023DAC8
		internal static void CompleteCurrentComposition(UnsafeNativeMethods.ITfDocumentMgr documentMgr)
		{
			UnsafeNativeMethods.ITfContext tfContext;
			documentMgr.GetBase(out tfContext);
			UnsafeNativeMethods.ITfCompositionView composition = FrameworkTextComposition.GetComposition(tfContext);
			if (composition != null)
			{
				(tfContext as UnsafeNativeMethods.ITfContextOwnerCompositionServices).TerminateComposition(composition);
				Marshal.ReleaseComObject(composition);
			}
			Marshal.ReleaseComObject(tfContext);
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x0023EB04 File Offset: 0x0023DB04
		internal static UnsafeNativeMethods.ITfCompositionView GetCurrentCompositionView(UnsafeNativeMethods.ITfDocumentMgr documentMgr)
		{
			UnsafeNativeMethods.ITfContext tfContext;
			documentMgr.GetBase(out tfContext);
			UnsafeNativeMethods.ITfCompositionView composition = FrameworkTextComposition.GetComposition(tfContext);
			Marshal.ReleaseComObject(tfContext);
			return composition;
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x0023EB28 File Offset: 0x0023DB28
		internal void SetResultPositions(ITextPointer start, ITextPointer end, string text)
		{
			Invariant.Assert(start != null);
			Invariant.Assert(end != null);
			Invariant.Assert(text != null);
			this._compositionStart = null;
			this._compositionEnd = null;
			this._resultStart = start.GetFrozenPointer(LogicalDirection.Backward);
			this._resultEnd = end.GetFrozenPointer(LogicalDirection.Forward);
			base.Text = text;
			base.CompositionText = string.Empty;
			this._offset = ((this._resultStart == null) ? -1 : this._resultStart.Offset);
			this._length = ((this._resultStart == null) ? -1 : this._resultStart.GetOffsetToPosition(this._resultEnd));
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x0023EBC8 File Offset: 0x0023DBC8
		internal void SetCompositionPositions(ITextPointer start, ITextPointer end, string text)
		{
			Invariant.Assert(start != null);
			Invariant.Assert(end != null);
			Invariant.Assert(text != null);
			this._compositionStart = start.GetFrozenPointer(LogicalDirection.Backward);
			this._compositionEnd = end.GetFrozenPointer(LogicalDirection.Forward);
			this._resultStart = null;
			this._resultEnd = null;
			base.Text = string.Empty;
			base.CompositionText = text;
			this._offset = ((this._compositionStart == null) ? -1 : this._compositionStart.Offset);
			this._length = ((this._compositionStart == null) ? -1 : this._compositionStart.GetOffsetToPosition(this._compositionEnd));
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x0023EC68 File Offset: 0x0023DC68
		internal ITextPointer _ResultStart
		{
			get
			{
				return this._resultStart;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0023EC70 File Offset: 0x0023DC70
		internal ITextPointer _ResultEnd
		{
			get
			{
				return this._resultEnd;
			}
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x0023EC78 File Offset: 0x0023DC78
		internal ITextPointer _CompositionStart
		{
			get
			{
				return this._compositionStart;
			}
		}

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06004D33 RID: 19763 RVA: 0x0023EC80 File Offset: 0x0023DC80
		internal ITextPointer _CompositionEnd
		{
			get
			{
				return this._compositionEnd;
			}
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06004D34 RID: 19764 RVA: 0x0023EC88 File Offset: 0x0023DC88
		internal bool PendingComplete
		{
			get
			{
				return this._pendingComplete;
			}
		}

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x06004D35 RID: 19765 RVA: 0x0023EC90 File Offset: 0x0023DC90
		internal object Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0023EC98 File Offset: 0x0023DC98
		private static UnsafeNativeMethods.ITfCompositionView GetComposition(UnsafeNativeMethods.ITfContext context)
		{
			UnsafeNativeMethods.ITfCompositionView[] array = new UnsafeNativeMethods.ITfCompositionView[1];
			UnsafeNativeMethods.IEnumITfCompositionView enumITfCompositionView;
			((UnsafeNativeMethods.ITfContextComposition)context).EnumCompositions(out enumITfCompositionView);
			int num;
			enumITfCompositionView.Next(1, array, out num);
			Marshal.ReleaseComObject(enumITfCompositionView);
			return array[0];
		}

		// Token: 0x040027F4 RID: 10228
		private ITextPointer _resultStart;

		// Token: 0x040027F5 RID: 10229
		private ITextPointer _resultEnd;

		// Token: 0x040027F6 RID: 10230
		private ITextPointer _compositionStart;

		// Token: 0x040027F7 RID: 10231
		private ITextPointer _compositionEnd;

		// Token: 0x040027F8 RID: 10232
		private int _offset;

		// Token: 0x040027F9 RID: 10233
		private int _length;

		// Token: 0x040027FA RID: 10234
		private readonly object _owner;

		// Token: 0x040027FB RID: 10235
		private bool _pendingComplete;
	}
}
