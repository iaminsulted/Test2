using System;

namespace MS.Internal.Documents
{
	// Token: 0x020001D1 RID: 465
	internal interface IParentUndoUnit : IUndoUnit
	{
		// Token: 0x0600105C RID: 4188
		void Clear();

		// Token: 0x0600105D RID: 4189
		void Open(IParentUndoUnit newUnit);

		// Token: 0x0600105E RID: 4190
		void Close(UndoCloseAction closeAction);

		// Token: 0x0600105F RID: 4191
		void Close(IParentUndoUnit closingUnit, UndoCloseAction closeAction);

		// Token: 0x06001060 RID: 4192
		void Add(IUndoUnit newUnit);

		// Token: 0x06001061 RID: 4193
		void OnNextAdd();

		// Token: 0x06001062 RID: 4194
		void OnNextDiscard();

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001063 RID: 4195
		IUndoUnit LastUnit { get; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001064 RID: 4196
		IParentUndoUnit OpenedUnit { get; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001065 RID: 4197
		// (set) Token: 0x06001066 RID: 4198
		string Description { get; set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001067 RID: 4199
		bool Locked { get; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001068 RID: 4200
		// (set) Token: 0x06001069 RID: 4201
		object Container { get; set; }
	}
}
