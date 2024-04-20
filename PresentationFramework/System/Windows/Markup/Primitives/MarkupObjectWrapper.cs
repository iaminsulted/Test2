using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000531 RID: 1329
	internal class MarkupObjectWrapper : MarkupObject
	{
		// Token: 0x060041DA RID: 16858 RVA: 0x00219604 File Offset: 0x00218604
		public MarkupObjectWrapper(MarkupObject baseObject)
		{
			this._baseObject = baseObject;
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x00219613 File Offset: 0x00218613
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._baseObject.AssignRootContext(context);
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060041DC RID: 16860 RVA: 0x00219621 File Offset: 0x00218621
		public override AttributeCollection Attributes
		{
			get
			{
				return this._baseObject.Attributes;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x0021962E File Offset: 0x0021862E
		public override Type ObjectType
		{
			get
			{
				return this._baseObject.ObjectType;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060041DE RID: 16862 RVA: 0x0021963B File Offset: 0x0021863B
		public override object Instance
		{
			get
			{
				return this._baseObject.Instance;
			}
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x00219648 File Offset: 0x00218648
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			return this._baseObject.GetProperties(mapToConstructorArgs);
		}

		// Token: 0x040024DE RID: 9438
		private MarkupObject _baseObject;
	}
}
