using System;
using System.Collections.Generic;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020003D5 RID: 981
	internal struct TreeChangeInfo
	{
		// Token: 0x0600290D RID: 10509 RVA: 0x001981EB File Offset: 0x001971EB
		public TreeChangeInfo(DependencyObject root, DependencyObject parent, bool isAddOperation)
		{
			this._rootOfChange = root;
			this._isAddOperation = isAddOperation;
			this._topmostCollapsedParentNode = null;
			this._rootInheritableValues = null;
			this._inheritablePropertiesStack = null;
			this._valueIndexer = 0;
			this.InheritablePropertiesStack.Push(this.CreateParentInheritableProperties(root, parent, isAddOperation));
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x0019822C File Offset: 0x0019722C
		internal FrugalObjectList<DependencyProperty> CreateParentInheritableProperties(DependencyObject d, DependencyObject parent, bool isAddOperation)
		{
			if (parent == null)
			{
				return new FrugalObjectList<DependencyProperty>(0);
			}
			DependencyObjectType dependencyObjectType = d.DependencyObjectType;
			EffectiveValueEntry[] array = null;
			uint num = 0U;
			uint num2 = 0U;
			if (!parent.IsSelfInheritanceParent)
			{
				DependencyObject inheritanceParent = parent.InheritanceParent;
				if (inheritanceParent != null)
				{
					array = inheritanceParent.EffectiveValues;
					num = inheritanceParent.EffectiveValuesCount;
					num2 = inheritanceParent.InheritableEffectiveValuesCount;
				}
			}
			else
			{
				array = parent.EffectiveValues;
				num = parent.EffectiveValuesCount;
				num2 = parent.InheritableEffectiveValuesCount;
			}
			FrugalObjectList<DependencyProperty> frugalObjectList = new FrugalObjectList<DependencyProperty>((int)num2);
			if (num2 == 0U)
			{
				return frugalObjectList;
			}
			this._rootInheritableValues = new InheritablePropertyChangeInfo[num2];
			int num3 = 0;
			FrameworkObject frameworkObject = new FrameworkObject(parent);
			for (uint num4 = 0U; num4 < num; num4 += 1U)
			{
				EffectiveValueEntry effectiveValueEntry = array[(int)num4];
				DependencyProperty dependencyProperty = DependencyProperty.RegisteredPropertyList.List[effectiveValueEntry.PropertyIndex];
				if (dependencyProperty != null && dependencyProperty.IsPotentiallyInherited)
				{
					PropertyMetadata metadata = dependencyProperty.GetMetadata(parent.DependencyObjectType);
					if (metadata != null && metadata.IsInherited)
					{
						FrameworkPropertyMetadata frameworkPropertyMetadata = (FrameworkPropertyMetadata)metadata;
						if (!TreeWalkHelper.SkipNow(frameworkObject.InheritanceBehavior) || frameworkPropertyMetadata.OverridesInheritanceBehavior)
						{
							frugalObjectList.Add(dependencyProperty);
							EffectiveValueEntry valueEntry = d.GetValueEntry(d.LookupEntry(dependencyProperty.GlobalIndex), dependencyProperty, dependencyProperty.GetMetadata(dependencyObjectType), RequestFlags.DeferredReferences);
							EffectiveValueEntry newEntry;
							if (isAddOperation)
							{
								newEntry = effectiveValueEntry;
								if (newEntry.BaseValueSourceInternal != BaseValueSourceInternal.Default || newEntry.HasModifiers)
								{
									newEntry = newEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
									newEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
								}
							}
							else
							{
								newEntry = default(EffectiveValueEntry);
							}
							this._rootInheritableValues[num3++] = new InheritablePropertyChangeInfo(d, dependencyProperty, valueEntry, newEntry);
							if ((ulong)num2 == (ulong)((long)num3))
							{
								break;
							}
						}
					}
				}
			}
			return frugalObjectList;
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x001983CB File Offset: 0x001973CB
		internal void ResetInheritableValueIndexer()
		{
			this._valueIndexer = 0;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x001983D4 File Offset: 0x001973D4
		internal InheritablePropertyChangeInfo GetRootInheritableValue(DependencyProperty dp)
		{
			InheritablePropertyChangeInfo result;
			do
			{
				InheritablePropertyChangeInfo[] rootInheritableValues = this._rootInheritableValues;
				int valueIndexer = this._valueIndexer;
				this._valueIndexer = valueIndexer + 1;
				result = rootInheritableValues[valueIndexer];
			}
			while (result.Property != dp);
			return result;
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002911 RID: 10513 RVA: 0x00198409 File Offset: 0x00197409
		internal Stack<FrugalObjectList<DependencyProperty>> InheritablePropertiesStack
		{
			get
			{
				if (this._inheritablePropertiesStack == null)
				{
					this._inheritablePropertiesStack = new Stack<FrugalObjectList<DependencyProperty>>(1);
				}
				return this._inheritablePropertiesStack;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06002912 RID: 10514 RVA: 0x00198425 File Offset: 0x00197425
		// (set) Token: 0x06002913 RID: 10515 RVA: 0x0019842D File Offset: 0x0019742D
		internal object TopmostCollapsedParentNode
		{
			get
			{
				return this._topmostCollapsedParentNode;
			}
			set
			{
				this._topmostCollapsedParentNode = value;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06002914 RID: 10516 RVA: 0x00198436 File Offset: 0x00197436
		internal bool IsAddOperation
		{
			get
			{
				return this._isAddOperation;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06002915 RID: 10517 RVA: 0x0019843E File Offset: 0x0019743E
		internal DependencyObject Root
		{
			get
			{
				return this._rootOfChange;
			}
		}

		// Token: 0x040014D4 RID: 5332
		private Stack<FrugalObjectList<DependencyProperty>> _inheritablePropertiesStack;

		// Token: 0x040014D5 RID: 5333
		private object _topmostCollapsedParentNode;

		// Token: 0x040014D6 RID: 5334
		private bool _isAddOperation;

		// Token: 0x040014D7 RID: 5335
		private DependencyObject _rootOfChange;

		// Token: 0x040014D8 RID: 5336
		private InheritablePropertyChangeInfo[] _rootInheritableValues;

		// Token: 0x040014D9 RID: 5337
		private int _valueIndexer;
	}
}
