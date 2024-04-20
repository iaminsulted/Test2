using System;
using System.Runtime.Serialization;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000285 RID: 645
	[Serializable]
	internal abstract class JournalEntryPageFunction : JournalEntry, ISerializable
	{
		// Token: 0x06001871 RID: 6257 RVA: 0x00160BE3 File Offset: 0x0015FBE3
		internal JournalEntryPageFunction(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, null)
		{
			this.PageFunctionId = pageFunction.PageFunctionId;
			this.ParentPageFunctionId = pageFunction.ParentPageFunctionId;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00160C08 File Offset: 0x0015FC08
		protected JournalEntryPageFunction(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._pageFunctionId = (Guid)info.GetValue("_pageFunctionId", typeof(Guid));
			this._parentPageFunctionId = (Guid)info.GetValue("_parentPageFunctionId", typeof(Guid));
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00160C5D File Offset: 0x0015FC5D
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_pageFunctionId", this._pageFunctionId);
			info.AddValue("_parentPageFunctionId", this._parentPageFunctionId);
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x00160C93 File Offset: 0x0015FC93
		// (set) Token: 0x06001875 RID: 6261 RVA: 0x00160C9B File Offset: 0x0015FC9B
		internal Guid PageFunctionId
		{
			get
			{
				return this._pageFunctionId;
			}
			set
			{
				this._pageFunctionId = value;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001876 RID: 6262 RVA: 0x00160CA4 File Offset: 0x0015FCA4
		// (set) Token: 0x06001877 RID: 6263 RVA: 0x00160CAC File Offset: 0x0015FCAC
		internal Guid ParentPageFunctionId
		{
			get
			{
				return this._parentPageFunctionId;
			}
			set
			{
				this._parentPageFunctionId = value;
			}
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsPageFunction()
		{
			return true;
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00105F35 File Offset: 0x00104F35
		internal override bool IsAlive()
		{
			return false;
		}

		// Token: 0x0600187A RID: 6266
		internal abstract PageFunctionBase ResumePageFunction();

		// Token: 0x0600187B RID: 6267 RVA: 0x00160CB8 File Offset: 0x0015FCB8
		internal static int GetParentPageJournalIndex(NavigationService NavigationService, Journal journal, PageFunctionBase endingPF)
		{
			for (int i = journal.CurrentIndex - 1; i >= 0; i--)
			{
				JournalEntry journalEntry = journal[i];
				if (!(journalEntry.NavigationServiceId != NavigationService.GuidId))
				{
					JournalEntryPageFunction journalEntryPageFunction = journalEntry as JournalEntryPageFunction;
					if (endingPF.ParentPageFunctionId == Guid.Empty)
					{
						if (journalEntryPageFunction == null)
						{
							return i;
						}
					}
					else if (journalEntryPageFunction != null && journalEntryPageFunction.PageFunctionId == endingPF.ParentPageFunctionId)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x04000D51 RID: 3409
		private Guid _pageFunctionId;

		// Token: 0x04000D52 RID: 3410
		private Guid _parentPageFunctionId;

		// Token: 0x04000D53 RID: 3411
		internal const int _NoParentPage = -1;
	}
}
