using System;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000533 RID: 1331
	internal class ExtensionSimplifierMarkupObject : MarkupObjectWrapper
	{
		// Token: 0x060041F1 RID: 16881 RVA: 0x00219735 File Offset: 0x00218735
		public ExtensionSimplifierMarkupObject(MarkupObject baseObject, IValueSerializerContext context) : base(baseObject)
		{
			this._context = context;
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x00219745 File Offset: 0x00218745
		private IEnumerable<MarkupProperty> GetBaseProperties(bool mapToConstructorArgs)
		{
			return base.GetProperties(mapToConstructorArgs);
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x0021974E File Offset: 0x0021874E
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			foreach (MarkupProperty baseProperty in this.GetBaseProperties(mapToConstructorArgs))
			{
				yield return new ExtensionSimplifierProperty(baseProperty, this._context);
			}
			IEnumerator<MarkupProperty> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x00219765 File Offset: 0x00218765
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = context;
			base.AssignRootContext(context);
		}

		// Token: 0x040024E0 RID: 9440
		private IValueSerializerContext _context;
	}
}
