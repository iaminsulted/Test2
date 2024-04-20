using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MS.Internal.Data
{
	// Token: 0x0200020F RID: 527
	internal class CommitManager
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x0014EFBD File Offset: 0x0014DFBD
		internal bool IsEmpty
		{
			get
			{
				return this._bindings.Count == 0 && this._bindingGroups.Count == 0;
			}
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0014EFDC File Offset: 0x0014DFDC
		internal void AddBindingGroup(BindingGroup bindingGroup)
		{
			this._bindingGroups.Add(bindingGroup);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0014EFEA File Offset: 0x0014DFEA
		internal void RemoveBindingGroup(BindingGroup bindingGroup)
		{
			this._bindingGroups.Remove(bindingGroup);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0014EFF9 File Offset: 0x0014DFF9
		internal void AddBinding(BindingExpressionBase binding)
		{
			this._bindings.Add(binding);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0014F007 File Offset: 0x0014E007
		internal void RemoveBinding(BindingExpressionBase binding)
		{
			this._bindings.Remove(binding);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0014F018 File Offset: 0x0014E018
		internal List<BindingGroup> GetBindingGroupsInScope(DependencyObject element)
		{
			List<BindingGroup> list = this._bindingGroups.ToList();
			List<BindingGroup> list2 = CommitManager.EmptyBindingGroupList;
			foreach (BindingGroup bindingGroup in list)
			{
				DependencyObject owner = bindingGroup.Owner;
				if (owner != null && this.IsInScope(element, owner))
				{
					if (list2 == CommitManager.EmptyBindingGroupList)
					{
						list2 = new List<BindingGroup>();
					}
					list2.Add(bindingGroup);
				}
			}
			return list2;
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0014F09C File Offset: 0x0014E09C
		internal List<BindingExpressionBase> GetBindingsInScope(DependencyObject element)
		{
			List<BindingExpressionBase> list = this._bindings.ToList();
			List<BindingExpressionBase> list2 = CommitManager.EmptyBindingList;
			foreach (BindingExpressionBase bindingExpressionBase in list)
			{
				DependencyObject targetElement = bindingExpressionBase.TargetElement;
				if (targetElement != null && bindingExpressionBase.IsEligibleForCommit && this.IsInScope(element, targetElement))
				{
					if (list2 == CommitManager.EmptyBindingList)
					{
						list2 = new List<BindingExpressionBase>();
					}
					list2.Add(bindingExpressionBase);
				}
			}
			return list2;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0014F128 File Offset: 0x0014E128
		internal bool Purge()
		{
			bool flag = false;
			int count = this._bindings.Count;
			if (count > 0)
			{
				foreach (BindingExpressionBase bindingExpressionBase in this._bindings.ToList())
				{
					DependencyObject targetElement = bindingExpressionBase.TargetElement;
				}
			}
			flag = (flag || this._bindings.Count < count);
			count = this._bindingGroups.Count;
			if (count > 0)
			{
				foreach (BindingGroup bindingGroup in this._bindingGroups.ToList())
				{
					DependencyObject owner = bindingGroup.Owner;
				}
			}
			flag = (flag || this._bindingGroups.Count < count);
			return flag;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0014F210 File Offset: 0x0014E210
		private bool IsInScope(DependencyObject ancestor, DependencyObject element)
		{
			return ancestor == null || VisualTreeHelper.IsAncestorOf(ancestor, element);
		}

		// Token: 0x04000B92 RID: 2962
		private CommitManager.Set<BindingGroup> _bindingGroups = new CommitManager.Set<BindingGroup>();

		// Token: 0x04000B93 RID: 2963
		private CommitManager.Set<BindingExpressionBase> _bindings = new CommitManager.Set<BindingExpressionBase>();

		// Token: 0x04000B94 RID: 2964
		private static readonly List<BindingGroup> EmptyBindingGroupList = new List<BindingGroup>();

		// Token: 0x04000B95 RID: 2965
		private static readonly List<BindingExpressionBase> EmptyBindingList = new List<BindingExpressionBase>();

		// Token: 0x020009EB RID: 2539
		private class Set<T> : Dictionary<T, object>, IEnumerable<T>, IEnumerable
		{
			// Token: 0x06008441 RID: 33857 RVA: 0x003254BD File Offset: 0x003244BD
			public Set()
			{
			}

			// Token: 0x06008442 RID: 33858 RVA: 0x003254C5 File Offset: 0x003244C5
			public Set(IDictionary<T, object> other) : base(other)
			{
			}

			// Token: 0x06008443 RID: 33859 RVA: 0x003254CE File Offset: 0x003244CE
			public Set(IEqualityComparer<T> comparer) : base(comparer)
			{
			}

			// Token: 0x06008444 RID: 33860 RVA: 0x003254D7 File Offset: 0x003244D7
			public void Add(T item)
			{
				if (!base.ContainsKey(item))
				{
					base.Add(item, null);
				}
			}

			// Token: 0x06008445 RID: 33861 RVA: 0x003254EA File Offset: 0x003244EA
			IEnumerator<T> IEnumerable<!0>.GetEnumerator()
			{
				return base.Keys.GetEnumerator();
			}

			// Token: 0x06008446 RID: 33862 RVA: 0x003254FC File Offset: 0x003244FC
			public List<T> ToList()
			{
				return new List<T>(base.Keys);
			}
		}
	}
}
