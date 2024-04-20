using System;

namespace System.Windows
{
	// Token: 0x020003BE RID: 958
	internal class DeferredThemeResourceReference : DeferredResourceReference
	{
		// Token: 0x0600286B RID: 10347 RVA: 0x00195338 File Offset: 0x00194338
		internal DeferredThemeResourceReference(ResourceDictionary dictionary, object resourceKey, bool canCacheAsThemeResource) : base(dictionary, resourceKey)
		{
			this._canCacheAsThemeResource = canCacheAsThemeResource;
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x0019534C File Offset: 0x0019434C
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			object result;
			lock (themeDictionaryLock)
			{
				if (base.Dictionary != null)
				{
					object key = this.Key;
					SystemResources.IsSystemResourcesParsing = true;
					bool flag2;
					object value;
					try
					{
						value = base.Dictionary.GetValue(key, out flag2);
						if (flag2)
						{
							this.Value = value;
							base.Dictionary = null;
						}
					}
					finally
					{
						SystemResources.IsSystemResourcesParsing = false;
					}
					if ((key is Type || key is ResourceKey) && this._canCacheAsThemeResource && flag2)
					{
						SystemResources.CacheResource(key, value, false);
					}
					result = value;
				}
				else
				{
					result = this.Value;
				}
			}
			return result;
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x00195408 File Offset: 0x00194408
		internal override Type GetValueType()
		{
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			Type valueType;
			lock (themeDictionaryLock)
			{
				valueType = base.GetValueType();
			}
			return valueType;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void RemoveFromDictionary()
		{
		}

		// Token: 0x04001489 RID: 5257
		private bool _canCacheAsThemeResource;
	}
}
