using System;

namespace System.Windows
{
	// Token: 0x02000378 RID: 888
	internal struct InheritablePropertyChangeInfo
	{
		// Token: 0x06002405 RID: 9221 RVA: 0x00181531 File Offset: 0x00180531
		internal InheritablePropertyChangeInfo(DependencyObject rootElement, DependencyProperty property, EffectiveValueEntry oldEntry, EffectiveValueEntry newEntry)
		{
			this._rootElement = rootElement;
			this._property = property;
			this._oldEntry = oldEntry;
			this._newEntry = newEntry;
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x00181550 File Offset: 0x00180550
		internal DependencyObject RootElement
		{
			get
			{
				return this._rootElement;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002407 RID: 9223 RVA: 0x00181558 File Offset: 0x00180558
		internal DependencyProperty Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002408 RID: 9224 RVA: 0x00181560 File Offset: 0x00180560
		internal EffectiveValueEntry OldEntry
		{
			get
			{
				return this._oldEntry;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x00181568 File Offset: 0x00180568
		internal EffectiveValueEntry NewEntry
		{
			get
			{
				return this._newEntry;
			}
		}

		// Token: 0x04001102 RID: 4354
		private DependencyObject _rootElement;

		// Token: 0x04001103 RID: 4355
		private DependencyProperty _property;

		// Token: 0x04001104 RID: 4356
		private EffectiveValueEntry _oldEntry;

		// Token: 0x04001105 RID: 4357
		private EffectiveValueEntry _newEntry;
	}
}
