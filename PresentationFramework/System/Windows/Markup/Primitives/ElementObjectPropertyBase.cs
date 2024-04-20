using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000528 RID: 1320
	internal abstract class ElementObjectPropertyBase : ElementPropertyBase
	{
		// Token: 0x060041A6 RID: 16806 RVA: 0x0021911D File Offset: 0x0021811D
		protected ElementObjectPropertyBase(ElementMarkupObject obj) : base(obj.Manager)
		{
			this._object = obj;
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x00219132 File Offset: 0x00218132
		protected override IValueSerializerContext GetItemContext()
		{
			return this._object.Context;
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0021913F File Offset: 0x0021813F
		protected override Type GetObjectType()
		{
			return this._object.ObjectType;
		}

		// Token: 0x040024D3 RID: 9427
		protected readonly ElementMarkupObject _object;
	}
}
