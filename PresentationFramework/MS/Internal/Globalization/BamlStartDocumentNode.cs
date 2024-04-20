using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x0200019F RID: 415
	internal sealed class BamlStartDocumentNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06000DC4 RID: 3524 RVA: 0x00136D59 File Offset: 0x00135D59
		internal BamlStartDocumentNode() : base(BamlNodeType.StartDocument)
		{
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00136D62 File Offset: 0x00135D62
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartDocument();
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00136D6A File Offset: 0x00135D6A
		internal override BamlTreeNode Copy()
		{
			return new BamlStartDocumentNode();
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00109403 File Offset: 0x00108403
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00136D71 File Offset: 0x00135D71
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public LocalizabilityAttribute InheritableAttribute
		{
			get
			{
				return new LocalizabilityAttribute(LocalizationCategory.None)
				{
					Readability = Readability.Readable,
					Modifiability = Modifiability.Modifiable
				};
			}
			set
			{
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public bool IsIgnored
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
