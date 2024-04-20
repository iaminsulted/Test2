using System;

namespace System.Windows.Markup
{
	// Token: 0x020004D0 RID: 1232
	internal class ReaderContextStackData
	{
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06003F1C RID: 16156 RVA: 0x0021053A File Offset: 0x0020F53A
		internal ReaderFlags ContextType
		{
			get
			{
				return this._contextFlags & ReaderFlags.ContextTypeMask;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06003F1D RID: 16157 RVA: 0x00210548 File Offset: 0x0020F548
		// (set) Token: 0x06003F1E RID: 16158 RVA: 0x00210550 File Offset: 0x0020F550
		internal object ObjectData
		{
			get
			{
				return this._contextData;
			}
			set
			{
				this._contextData = value;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06003F1F RID: 16159 RVA: 0x00210559 File Offset: 0x0020F559
		// (set) Token: 0x06003F20 RID: 16160 RVA: 0x00210561 File Offset: 0x0020F561
		internal object Key
		{
			get
			{
				return this._contextKey;
			}
			set
			{
				this._contextKey = value;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x0021056A File Offset: 0x0020F56A
		// (set) Token: 0x06003F22 RID: 16162 RVA: 0x00210572 File Offset: 0x0020F572
		internal string Uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				this._uid = value;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x0021057B File Offset: 0x0020F57B
		// (set) Token: 0x06003F24 RID: 16164 RVA: 0x00210583 File Offset: 0x0020F583
		internal string ElementNameOrPropertyName
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06003F25 RID: 16165 RVA: 0x0021058C File Offset: 0x0020F58C
		// (set) Token: 0x06003F26 RID: 16166 RVA: 0x00210594 File Offset: 0x0020F594
		internal object ContentProperty
		{
			get
			{
				return this._contentProperty;
			}
			set
			{
				this._contentProperty = value;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x0021059D File Offset: 0x0020F59D
		// (set) Token: 0x06003F28 RID: 16168 RVA: 0x002105A5 File Offset: 0x0020F5A5
		internal Type ExpectedType
		{
			get
			{
				return this._expectedType;
			}
			set
			{
				this._expectedType = value;
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06003F29 RID: 16169 RVA: 0x002105AE File Offset: 0x0020F5AE
		// (set) Token: 0x06003F2A RID: 16170 RVA: 0x002105B6 File Offset: 0x0020F5B6
		internal short ExpectedTypeId
		{
			get
			{
				return this._expectedTypeId;
			}
			set
			{
				this._expectedTypeId = value;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06003F2B RID: 16171 RVA: 0x002105BF File Offset: 0x0020F5BF
		// (set) Token: 0x06003F2C RID: 16172 RVA: 0x002105C7 File Offset: 0x0020F5C7
		internal bool CreateUsingTypeConverter
		{
			get
			{
				return this._createUsingTypeConverter;
			}
			set
			{
				this._createUsingTypeConverter = value;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06003F2D RID: 16173 RVA: 0x002105D0 File Offset: 0x0020F5D0
		// (set) Token: 0x06003F2E RID: 16174 RVA: 0x002105D8 File Offset: 0x0020F5D8
		internal ReaderFlags ContextFlags
		{
			get
			{
				return this._contextFlags;
			}
			set
			{
				this._contextFlags = value;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06003F2F RID: 16175 RVA: 0x002105E1 File Offset: 0x0020F5E1
		internal bool NeedToAddToTree
		{
			get
			{
				return this.CheckFlag(ReaderFlags.NeedToAddToTree);
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x002105EA File Offset: 0x0020F5EA
		internal void MarkAddedToTree()
		{
			this.ContextFlags = ((this.ContextFlags | ReaderFlags.AddedToTree) & (ReaderFlags)65534);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00210600 File Offset: 0x0020F600
		internal bool CheckFlag(ReaderFlags flag)
		{
			return (this.ContextFlags & flag) == flag;
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0021060D File Offset: 0x0020F60D
		internal void SetFlag(ReaderFlags flag)
		{
			this.ContextFlags |= flag;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0021061D File Offset: 0x0020F61D
		internal void ClearFlag(ReaderFlags flag)
		{
			this.ContextFlags &= ~flag;
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06003F34 RID: 16180 RVA: 0x0021062F File Offset: 0x0020F62F
		internal bool IsObjectElement
		{
			get
			{
				return this.ContextType == ReaderFlags.DependencyObject || this.ContextType == ReaderFlags.ClrObject;
			}
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x00210650 File Offset: 0x0020F650
		internal void ClearData()
		{
			this._contextFlags = ReaderFlags.Unknown;
			this._contextData = null;
			this._contextKey = null;
			this._contentProperty = null;
			this._expectedType = null;
			this._expectedTypeId = 0;
			this._createUsingTypeConverter = false;
			this._uid = null;
			this._name = null;
		}

		// Token: 0x04002358 RID: 9048
		private ReaderFlags _contextFlags;

		// Token: 0x04002359 RID: 9049
		private object _contextData;

		// Token: 0x0400235A RID: 9050
		private object _contextKey;

		// Token: 0x0400235B RID: 9051
		private string _uid;

		// Token: 0x0400235C RID: 9052
		private string _name;

		// Token: 0x0400235D RID: 9053
		private object _contentProperty;

		// Token: 0x0400235E RID: 9054
		private Type _expectedType;

		// Token: 0x0400235F RID: 9055
		private short _expectedTypeId;

		// Token: 0x04002360 RID: 9056
		private bool _createUsingTypeConverter;
	}
}
