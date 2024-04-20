using System;

namespace System.Windows.Documents
{
	// Token: 0x02000642 RID: 1602
	public class ListItemCollection : TextElementCollection<ListItem>
	{
		// Token: 0x06004F47 RID: 20295 RVA: 0x00243C0E File Offset: 0x00242C0E
		internal ListItemCollection(DependencyObject owner, bool isOwnerParent) : base(owner, isOwnerParent)
		{
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06004F48 RID: 20296 RVA: 0x00243C18 File Offset: 0x00242C18
		public ListItem FirstListItem
		{
			get
			{
				return base.FirstChild;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06004F49 RID: 20297 RVA: 0x00243C20 File Offset: 0x00242C20
		public ListItem LastListItem
		{
			get
			{
				return base.LastChild;
			}
		}
	}
}
