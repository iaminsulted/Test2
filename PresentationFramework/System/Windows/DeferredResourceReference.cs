using System;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003BC RID: 956
	internal class DeferredResourceReference : DeferredReference
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x00195105 File Offset: 0x00194105
		internal DeferredResourceReference(ResourceDictionary dictionary, object key)
		{
			this._dictionary = dictionary;
			this._keyOrValue = key;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x0019511C File Offset: 0x0019411C
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			if (this._dictionary != null)
			{
				bool flag;
				object value = this._dictionary.GetValue(this._keyOrValue, out flag);
				if (flag)
				{
					this._keyOrValue = value;
					this.RemoveFromDictionary();
				}
				if (valueSource == BaseValueSourceInternal.ThemeStyle || valueSource == BaseValueSourceInternal.ThemeStyleTrigger || valueSource == BaseValueSourceInternal.Style || valueSource == BaseValueSourceInternal.TemplateTrigger || valueSource == BaseValueSourceInternal.StyleTrigger || valueSource == BaseValueSourceInternal.ParentTemplate || valueSource == BaseValueSourceInternal.ParentTemplateTrigger)
				{
					StyleHelper.SealIfSealable(value);
				}
				this.OnInflated();
				return value;
			}
			return this._keyOrValue;
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x0019518C File Offset: 0x0019418C
		private void OnInflated()
		{
			if (this._inflatedList != null)
			{
				foreach (object obj in this._inflatedList)
				{
					((ResourceReferenceExpression)obj).OnDeferredResourceInflated(this);
				}
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x001951CC File Offset: 0x001941CC
		internal override Type GetValueType()
		{
			if (this._dictionary != null)
			{
				bool flag;
				return this._dictionary.GetValueType(this._keyOrValue, out flag);
			}
			if (this._keyOrValue == null)
			{
				return null;
			}
			return this._keyOrValue.GetType();
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x0019520A File Offset: 0x0019420A
		internal virtual void RemoveFromDictionary()
		{
			if (this._dictionary != null)
			{
				this._dictionary.DeferredResourceReferences.Remove(this);
				this._dictionary = null;
			}
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x0019522D File Offset: 0x0019422D
		internal virtual void AddInflatedListener(ResourceReferenceExpression listener)
		{
			if (this._inflatedList == null)
			{
				this._inflatedList = new WeakReferenceList(this);
			}
			this._inflatedList.Add(listener);
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x00195250 File Offset: 0x00194250
		internal virtual void RemoveInflatedListener(ResourceReferenceExpression listener)
		{
			if (this._inflatedList != null)
			{
				this._inflatedList.Remove(listener);
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06002861 RID: 10337 RVA: 0x00195267 File Offset: 0x00194267
		internal virtual object Key
		{
			get
			{
				return this._keyOrValue;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06002862 RID: 10338 RVA: 0x0019526F File Offset: 0x0019426F
		// (set) Token: 0x06002863 RID: 10339 RVA: 0x00195277 File Offset: 0x00194277
		internal ResourceDictionary Dictionary
		{
			get
			{
				return this._dictionary;
			}
			set
			{
				this._dictionary = value;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06002864 RID: 10340 RVA: 0x00195267 File Offset: 0x00194267
		// (set) Token: 0x06002865 RID: 10341 RVA: 0x00195280 File Offset: 0x00194280
		internal virtual object Value
		{
			get
			{
				return this._keyOrValue;
			}
			set
			{
				this._keyOrValue = value;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002866 RID: 10342 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsUnset
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x00195289 File Offset: 0x00194289
		internal bool IsInflated
		{
			get
			{
				return this._dictionary == null;
			}
		}

		// Token: 0x04001486 RID: 5254
		private ResourceDictionary _dictionary;

		// Token: 0x04001487 RID: 5255
		protected object _keyOrValue;

		// Token: 0x04001488 RID: 5256
		private WeakReferenceList _inflatedList;
	}
}
