using System;
using System.Collections.Generic;
using MS.Internal.Xaml.Context;

namespace System.Windows.Baml2006
{
	// Token: 0x02000407 RID: 1031
	internal class Baml2006ReaderContext
	{
		// Token: 0x06002CF3 RID: 11507 RVA: 0x001AAFFC File Offset: 0x001A9FFC
		public Baml2006ReaderContext(Baml2006SchemaContext schemaContext)
		{
			if (schemaContext == null)
			{
				throw new ArgumentNullException("schemaContext");
			}
			this._schemaContext = schemaContext;
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x001AB04E File Offset: 0x001AA04E
		public Baml2006SchemaContext SchemaContext
		{
			get
			{
				return this._schemaContext;
			}
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x001AB056 File Offset: 0x001AA056
		public void PushScope()
		{
			this._stack.PushScope();
			this.CurrentFrame.FreezeFreezables = this.PreviousFrame.FreezeFreezables;
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x001AB079 File Offset: 0x001AA079
		public void PopScope()
		{
			this._stack.PopScope();
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x001AB086 File Offset: 0x001AA086
		public Baml2006ReaderFrame CurrentFrame
		{
			get
			{
				return this._stack.CurrentFrame;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x001AB093 File Offset: 0x001AA093
		public Baml2006ReaderFrame PreviousFrame
		{
			get
			{
				return this._stack.PreviousFrame;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x001AB0A0 File Offset: 0x001AA0A0
		// (set) Token: 0x06002CFA RID: 11514 RVA: 0x001AB0A8 File Offset: 0x001AA0A8
		public List<KeyRecord> KeyList { get; set; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06002CFB RID: 11515 RVA: 0x001AB0B1 File Offset: 0x001AA0B1
		// (set) Token: 0x06002CFC RID: 11516 RVA: 0x001AB0B9 File Offset: 0x001AA0B9
		public int CurrentKey { get; set; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x001AB0C2 File Offset: 0x001AA0C2
		public KeyRecord LastKey
		{
			get
			{
				if (this.KeyList != null && this.KeyList.Count > 0)
				{
					return this.KeyList[this.KeyList.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x001AB0F4 File Offset: 0x001AA0F4
		// (set) Token: 0x06002CFF RID: 11519 RVA: 0x001AB0FC File Offset: 0x001AA0FC
		public bool InsideKeyRecord { get; set; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06002D00 RID: 11520 RVA: 0x001AB105 File Offset: 0x001AA105
		// (set) Token: 0x06002D01 RID: 11521 RVA: 0x001AB10D File Offset: 0x001AA10D
		public bool InsideStaticResource { get; set; }

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06002D02 RID: 11522 RVA: 0x001AB116 File Offset: 0x001AA116
		// (set) Token: 0x06002D03 RID: 11523 RVA: 0x001AB11E File Offset: 0x001AA11E
		public int TemplateStartDepth { get; set; }

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x001AB127 File Offset: 0x001AA127
		// (set) Token: 0x06002D05 RID: 11525 RVA: 0x001AB12F File Offset: 0x001AA12F
		public int LineNumber { get; set; }

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06002D06 RID: 11526 RVA: 0x001AB138 File Offset: 0x001AA138
		// (set) Token: 0x06002D07 RID: 11527 RVA: 0x001AB140 File Offset: 0x001AA140
		public int LineOffset { get; set; }

		// Token: 0x04001B5A RID: 7002
		private Baml2006SchemaContext _schemaContext;

		// Token: 0x04001B5B RID: 7003
		private XamlContextStack<Baml2006ReaderFrame> _stack = new XamlContextStack<Baml2006ReaderFrame>(() => new Baml2006ReaderFrame());
	}
}
