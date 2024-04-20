using System;
using System.Collections.Generic;

namespace System.Windows
{
	// Token: 0x02000394 RID: 916
	internal struct ResourcesChangeInfo
	{
		// Token: 0x06002516 RID: 9494 RVA: 0x00185983 File Offset: 0x00184983
		internal ResourcesChangeInfo(object key)
		{
			this._oldDictionaries = null;
			this._newDictionaries = null;
			this._key = key;
			this._container = null;
			this._flags = (ResourcesChangeInfo.PrivateFlags)0;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x001859A8 File Offset: 0x001849A8
		internal ResourcesChangeInfo(ResourceDictionary oldDictionary, ResourceDictionary newDictionary)
		{
			this._oldDictionaries = null;
			if (oldDictionary != null)
			{
				this._oldDictionaries = new List<ResourceDictionary>(1);
				this._oldDictionaries.Add(oldDictionary);
			}
			this._newDictionaries = null;
			if (newDictionary != null)
			{
				this._newDictionaries = new List<ResourceDictionary>(1);
				this._newDictionaries.Add(newDictionary);
			}
			this._key = null;
			this._container = null;
			this._flags = (ResourcesChangeInfo.PrivateFlags)0;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x00185A0E File Offset: 0x00184A0E
		internal ResourcesChangeInfo(List<ResourceDictionary> oldDictionaries, List<ResourceDictionary> newDictionaries, bool isStyleResourcesChange, bool isTemplateResourcesChange, DependencyObject container)
		{
			this._oldDictionaries = oldDictionaries;
			this._newDictionaries = newDictionaries;
			this._key = null;
			this._container = container;
			this._flags = (ResourcesChangeInfo.PrivateFlags)0;
			this.IsStyleResourcesChange = isStyleResourcesChange;
			this.IsTemplateResourcesChange = isTemplateResourcesChange;
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x00185A44 File Offset: 0x00184A44
		internal static ResourcesChangeInfo ThemeChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsThemeChange = true
				};
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x00185A64 File Offset: 0x00184A64
		internal static ResourcesChangeInfo TreeChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsTreeChange = true
				};
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x00185A84 File Offset: 0x00184A84
		internal static ResourcesChangeInfo SysColorsOrSettingsChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsSysColorsOrSettingsChange = true
				};
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x00185AA4 File Offset: 0x00184AA4
		internal static ResourcesChangeInfo CatastrophicDictionaryChangeInfo
		{
			get
			{
				return new ResourcesChangeInfo
				{
					IsCatastrophicDictionaryChange = true
				};
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x00185AC2 File Offset: 0x00184AC2
		// (set) Token: 0x0600251E RID: 9502 RVA: 0x00185ACB File Offset: 0x00184ACB
		internal bool IsThemeChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsThemeChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsThemeChange, value);
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x00185AD5 File Offset: 0x00184AD5
		// (set) Token: 0x06002520 RID: 9504 RVA: 0x00185ADE File Offset: 0x00184ADE
		internal bool IsTreeChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTreeChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTreeChange, value);
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x00185AE8 File Offset: 0x00184AE8
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x00185AF1 File Offset: 0x00184AF1
		internal bool IsStyleResourcesChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsStyleResourceChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsStyleResourceChange, value);
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x00185AFB File Offset: 0x00184AFB
		// (set) Token: 0x06002524 RID: 9508 RVA: 0x00185B04 File Offset: 0x00184B04
		internal bool IsTemplateResourcesChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTemplateResourceChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsTemplateResourceChange, value);
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x00185B0E File Offset: 0x00184B0E
		// (set) Token: 0x06002526 RID: 9510 RVA: 0x00185B18 File Offset: 0x00184B18
		internal bool IsSysColorsOrSettingsChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsSysColorsOrSettingsChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsSysColorsOrSettingsChange, value);
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x00185B23 File Offset: 0x00184B23
		// (set) Token: 0x06002528 RID: 9512 RVA: 0x00185B2D File Offset: 0x00184B2D
		internal bool IsCatastrophicDictionaryChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsCatastrophicDictionaryChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsCatastrophicDictionaryChange, value);
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x00185B38 File Offset: 0x00184B38
		// (set) Token: 0x0600252A RID: 9514 RVA: 0x00185B42 File Offset: 0x00184B42
		internal bool IsImplicitDataTemplateChange
		{
			get
			{
				return this.ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags.IsImplicitDataTemplateChange);
			}
			set
			{
				this.WritePrivateFlag(ResourcesChangeInfo.PrivateFlags.IsImplicitDataTemplateChange, value);
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x00185B4D File Offset: 0x00184B4D
		internal bool IsResourceAddOperation
		{
			get
			{
				return this._key != null || (this._newDictionaries != null && this._newDictionaries.Count > 0);
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x0600252C RID: 9516 RVA: 0x00185B71 File Offset: 0x00184B71
		internal DependencyObject Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x00185B7C File Offset: 0x00184B7C
		internal bool Contains(object key, bool isImplicitStyleKey)
		{
			if (this.IsTreeChange || this.IsCatastrophicDictionaryChange)
			{
				return true;
			}
			if (this.IsThemeChange || this.IsSysColorsOrSettingsChange)
			{
				return !isImplicitStyleKey;
			}
			if (this._key != null && object.Equals(this._key, key))
			{
				return true;
			}
			if (this._oldDictionaries != null)
			{
				for (int i = 0; i < this._oldDictionaries.Count; i++)
				{
					if (this._oldDictionaries[i].Contains(key))
					{
						return true;
					}
				}
			}
			if (this._newDictionaries != null)
			{
				for (int j = 0; j < this._newDictionaries.Count; j++)
				{
					if (this._newDictionaries[j].Contains(key))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00185C34 File Offset: 0x00184C34
		internal void SetIsImplicitDataTemplateChange()
		{
			bool flag = this.IsCatastrophicDictionaryChange || this._key is DataTemplateKey;
			if (!flag && this._oldDictionaries != null)
			{
				using (List<ResourceDictionary>.Enumerator enumerator = this._oldDictionaries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasImplicitDataTemplates)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag && this._newDictionaries != null)
			{
				using (List<ResourceDictionary>.Enumerator enumerator = this._newDictionaries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasImplicitDataTemplates)
						{
							flag = true;
							break;
						}
					}
				}
			}
			this.IsImplicitDataTemplateChange = flag;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x00185D08 File Offset: 0x00184D08
		private void WritePrivateFlag(ResourcesChangeInfo.PrivateFlags bit, bool value)
		{
			if (value)
			{
				this._flags |= bit;
				return;
			}
			this._flags &= ~bit;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x00185D2C File Offset: 0x00184D2C
		private bool ReadPrivateFlag(ResourcesChangeInfo.PrivateFlags bit)
		{
			return (this._flags & bit) > (ResourcesChangeInfo.PrivateFlags)0;
		}

		// Token: 0x04001175 RID: 4469
		private List<ResourceDictionary> _oldDictionaries;

		// Token: 0x04001176 RID: 4470
		private List<ResourceDictionary> _newDictionaries;

		// Token: 0x04001177 RID: 4471
		private object _key;

		// Token: 0x04001178 RID: 4472
		private DependencyObject _container;

		// Token: 0x04001179 RID: 4473
		private ResourcesChangeInfo.PrivateFlags _flags;

		// Token: 0x02000A8D RID: 2701
		private enum PrivateFlags : byte
		{
			// Token: 0x040041B8 RID: 16824
			IsThemeChange = 1,
			// Token: 0x040041B9 RID: 16825
			IsTreeChange,
			// Token: 0x040041BA RID: 16826
			IsStyleResourceChange = 4,
			// Token: 0x040041BB RID: 16827
			IsTemplateResourceChange = 8,
			// Token: 0x040041BC RID: 16828
			IsSysColorsOrSettingsChange = 16,
			// Token: 0x040041BD RID: 16829
			IsCatastrophicDictionaryChange = 32,
			// Token: 0x040041BE RID: 16830
			IsImplicitDataTemplateChange = 64
		}
	}
}
