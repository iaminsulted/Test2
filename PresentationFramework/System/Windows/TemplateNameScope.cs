using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003C9 RID: 969
	internal class TemplateNameScope : INameScope
	{
		// Token: 0x060028CF RID: 10447 RVA: 0x00197502 File Offset: 0x00196502
		internal TemplateNameScope(DependencyObject templatedParent) : this(templatedParent, null, null)
		{
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x0019750D File Offset: 0x0019650D
		internal TemplateNameScope(DependencyObject templatedParent, List<DependencyObject> affectedChildren, FrameworkTemplate frameworkTemplate)
		{
			this._affectedChildren = affectedChildren;
			this._frameworkTemplate = frameworkTemplate;
			this._templatedParent = templatedParent;
			this._isTemplatedParentAnFE = true;
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x00197531 File Offset: 0x00196531
		void INameScope.RegisterName(string name, object scopedElement)
		{
			if (!(scopedElement is FrameworkContentElement) && !(scopedElement is FrameworkElement))
			{
				this.RegisterNameInternal(name, scopedElement);
			}
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x0019754C File Offset: 0x0019654C
		internal void RegisterNameInternal(string name, object scopedElement)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(scopedElement as DependencyObject, out frameworkElement, out frameworkContentElement, false);
			if (this._templatedParent == null)
			{
				if (this._nameMap == null)
				{
					this._nameMap = new HybridDictionary();
				}
				this._nameMap[name] = scopedElement;
				if (frameworkElement != null || frameworkContentElement != null)
				{
					this.SetTemplateParentValues(name, scopedElement);
					return;
				}
			}
			else
			{
				if (frameworkElement == null && frameworkContentElement == null)
				{
					Hashtable hashtable = TemplateNameScope._templatedNonFeChildrenField.GetValue(this._templatedParent);
					if (hashtable == null)
					{
						hashtable = new Hashtable(1);
						TemplateNameScope._templatedNonFeChildrenField.SetValue(this._templatedParent, hashtable);
					}
					hashtable[name] = scopedElement;
					return;
				}
				this._affectedChildren.Add(scopedElement as DependencyObject);
				int num;
				if (frameworkElement != null)
				{
					frameworkElement._templatedParent = this._templatedParent;
					frameworkElement.IsTemplatedParentAnFE = this._isTemplatedParentAnFE;
					num = (frameworkElement.TemplateChildIndex = (int)this._frameworkTemplate.ChildIndexFromChildName[name]);
				}
				else
				{
					frameworkContentElement._templatedParent = this._templatedParent;
					frameworkContentElement.IsTemplatedParentAnFE = this._isTemplatedParentAnFE;
					num = (frameworkContentElement.TemplateChildIndex = (int)this._frameworkTemplate.ChildIndexFromChildName[name]);
				}
				FrameworkTemplate.TemplateChildLoadedFlags templateChildLoadedFlags = this._frameworkTemplate._TemplateChildLoadedDictionary[num] as FrameworkTemplate.TemplateChildLoadedFlags;
				if (templateChildLoadedFlags != null && (templateChildLoadedFlags.HasLoadedChangedHandler || templateChildLoadedFlags.HasUnloadedChangedHandler))
				{
					BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry((frameworkElement != null) ? frameworkElement : frameworkContentElement);
				}
				StyleHelper.CreateInstanceDataForChild(StyleHelper.TemplateDataField, this._templatedParent, (frameworkElement != null) ? frameworkElement : frameworkContentElement, num, this._frameworkTemplate.HasInstanceValues, ref this._frameworkTemplate.ChildRecordFromChildIndex);
			}
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void INameScope.UnregisterName(string name)
		{
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x001976D8 File Offset: 0x001966D8
		object INameScope.FindName(string name)
		{
			if (this._templatedParent != null)
			{
				FrameworkObject frameworkObject = new FrameworkObject(this._templatedParent);
				if (frameworkObject.IsFE)
				{
					return StyleHelper.FindNameInTemplateContent(frameworkObject.FE, name, frameworkObject.FE.TemplateInternal);
				}
				return null;
			}
			else
			{
				if (this._nameMap == null || name == null || name == string.Empty)
				{
					return null;
				}
				return this._nameMap[name];
			}
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x00197745 File Offset: 0x00196745
		private void SetTemplateParentValues(string name, object element)
		{
			FrameworkTemplate.SetTemplateParentValues(name, element, this._frameworkTemplate, ref this._provideValueServiceProvider);
		}

		// Token: 0x040014A2 RID: 5282
		private List<DependencyObject> _affectedChildren;

		// Token: 0x040014A3 RID: 5283
		private static UncommonField<Hashtable> _templatedNonFeChildrenField = StyleHelper.TemplatedNonFeChildrenField;

		// Token: 0x040014A4 RID: 5284
		private DependencyObject _templatedParent;

		// Token: 0x040014A5 RID: 5285
		private FrameworkTemplate _frameworkTemplate;

		// Token: 0x040014A6 RID: 5286
		private bool _isTemplatedParentAnFE;

		// Token: 0x040014A7 RID: 5287
		private ProvideValueServiceProvider _provideValueServiceProvider;

		// Token: 0x040014A8 RID: 5288
		private HybridDictionary _nameMap;
	}
}
