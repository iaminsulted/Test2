using System;
using System.Collections;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x0200053E RID: 1342
	public sealed class BamlLocalizationDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06004262 RID: 16994 RVA: 0x0021B5F1 File Offset: 0x0021A5F1
		internal BamlLocalizationDictionaryEnumerator(IEnumerator enumerator)
		{
			this._enumerator = enumerator;
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x0021B600 File Offset: 0x0021A600
		public bool MoveNext()
		{
			return this._enumerator.MoveNext();
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0021B60D File Offset: 0x0021A60D
		public void Reset()
		{
			this._enumerator.Reset();
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06004265 RID: 16997 RVA: 0x0021B61A File Offset: 0x0021A61A
		public DictionaryEntry Entry
		{
			get
			{
				return (DictionaryEntry)this._enumerator.Current;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x0021B62C File Offset: 0x0021A62C
		public BamlLocalizableResourceKey Key
		{
			get
			{
				return (BamlLocalizableResourceKey)this.Entry.Key;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x0021B64C File Offset: 0x0021A64C
		public BamlLocalizableResource Value
		{
			get
			{
				return (BamlLocalizableResource)this.Entry.Value;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x0021B66C File Offset: 0x0021A66C
		public DictionaryEntry Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x0021B674 File Offset: 0x0021A674
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x0021B681 File Offset: 0x0021A681
		object IDictionaryEnumerator.Key
		{
			get
			{
				return this.Key;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x0600426B RID: 17003 RVA: 0x0021B689 File Offset: 0x0021A689
		object IDictionaryEnumerator.Value
		{
			get
			{
				return this.Value;
			}
		}

		// Token: 0x040024FD RID: 9469
		private IEnumerator _enumerator;
	}
}
