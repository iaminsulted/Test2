using System;
using System.Runtime.Serialization;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000283 RID: 643
	[Serializable]
	internal class JournalEntryUri : JournalEntry, ISerializable
	{
		// Token: 0x06001869 RID: 6249 RVA: 0x00160B5F File Offset: 0x0015FB5F
		internal JournalEntryUri(JournalEntryGroupState jeGroupState, Uri uri) : base(jeGroupState, uri)
		{
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00160B69 File Offset: 0x0015FB69
		protected JournalEntryUri(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00160B73 File Offset: 0x0015FB73
		internal override void SaveState(object contentObject)
		{
			Invariant.Assert(base.Source != null, "Can't journal by Uri without a Uri.");
			base.SaveState(contentObject);
		}
	}
}
