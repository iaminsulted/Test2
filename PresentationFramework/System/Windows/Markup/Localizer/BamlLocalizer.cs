using System;
using System.IO;
using MS.Internal.Globalization;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x02000540 RID: 1344
	public class BamlLocalizer
	{
		// Token: 0x06004289 RID: 17033 RVA: 0x0021BA09 File Offset: 0x0021AA09
		public BamlLocalizer(Stream source) : this(source, null)
		{
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x0021BA13 File Offset: 0x0021AA13
		public BamlLocalizer(Stream source, BamlLocalizabilityResolver resolver) : this(source, resolver, null)
		{
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x0021BA1E File Offset: 0x0021AA1E
		public BamlLocalizer(Stream source, BamlLocalizabilityResolver resolver, TextReader comments)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this._tree = BamlResourceDeserializer.LoadBaml(source);
			this._bamlTreeMap = new BamlTreeMap(this, this._tree, resolver, comments);
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x0021BA54 File Offset: 0x0021AA54
		public BamlLocalizationDictionary ExtractResources()
		{
			return this._bamlTreeMap.LocalizationDictionary.Copy();
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x0021BA68 File Offset: 0x0021AA68
		public void UpdateBaml(Stream target, BamlLocalizationDictionary updates)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (updates == null)
			{
				throw new ArgumentNullException("updates");
			}
			BamlTree tree = this._tree.Copy();
			this._bamlTreeMap.EnsureMap();
			BamlTreeUpdater.UpdateTree(tree, this._bamlTreeMap, updates);
			BamlResourceSerializer.Serialize(this, tree, target);
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x0600428E RID: 17038 RVA: 0x0021BAC0 File Offset: 0x0021AAC0
		// (remove) Token: 0x0600428F RID: 17039 RVA: 0x0021BAF8 File Offset: 0x0021AAF8
		public event BamlLocalizerErrorNotifyEventHandler ErrorNotify;

		// Token: 0x06004290 RID: 17040 RVA: 0x0021BB30 File Offset: 0x0021AB30
		protected virtual void OnErrorNotify(BamlLocalizerErrorNotifyEventArgs e)
		{
			BamlLocalizerErrorNotifyEventHandler errorNotify = this.ErrorNotify;
			if (errorNotify != null)
			{
				errorNotify(this, e);
			}
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0021BB4F File Offset: 0x0021AB4F
		internal void RaiseErrorNotifyEvent(BamlLocalizerErrorNotifyEventArgs e)
		{
			this.OnErrorNotify(e);
		}

		// Token: 0x04002501 RID: 9473
		private BamlTreeMap _bamlTreeMap;

		// Token: 0x04002502 RID: 9474
		private BamlTree _tree;
	}
}
