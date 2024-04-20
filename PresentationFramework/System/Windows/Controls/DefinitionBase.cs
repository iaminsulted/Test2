using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000775 RID: 1909
	[Localizability(LocalizationCategory.Ignore)]
	public abstract class DefinitionBase : FrameworkContentElement
	{
		// Token: 0x060067B7 RID: 26551 RVA: 0x002B5F12 File Offset: 0x002B4F12
		internal DefinitionBase(bool isColumnDefinition)
		{
			this._isColumnDefinition = isColumnDefinition;
			this._parentIndex = -1;
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x060067B8 RID: 26552 RVA: 0x002B5F28 File Offset: 0x002B4F28
		// (set) Token: 0x060067B9 RID: 26553 RVA: 0x002B5F3A File Offset: 0x002B4F3A
		public string SharedSizeGroup
		{
			get
			{
				return (string)base.GetValue(DefinitionBase.SharedSizeGroupProperty);
			}
			set
			{
				base.SetValue(DefinitionBase.SharedSizeGroupProperty, value);
			}
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x002B5F48 File Offset: 0x002B4F48
		internal void OnEnterParentTree()
		{
			if (this._sharedState == null)
			{
				string sharedSizeGroup = this.SharedSizeGroup;
				if (sharedSizeGroup != null)
				{
					DefinitionBase.SharedSizeScope privateSharedSizeScope = this.PrivateSharedSizeScope;
					if (privateSharedSizeScope != null)
					{
						this._sharedState = privateSharedSizeScope.EnsureSharedState(sharedSizeGroup);
						this._sharedState.AddMember(this);
					}
				}
			}
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x002B5F8A File Offset: 0x002B4F8A
		internal void OnExitParentTree()
		{
			this._offset = 0.0;
			if (this._sharedState != null)
			{
				this._sharedState.RemoveMember(this);
				this._sharedState = null;
			}
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x002B5FB6 File Offset: 0x002B4FB6
		internal void OnBeforeLayout(Grid grid)
		{
			this._minSize = 0.0;
			this.LayoutWasUpdated = true;
			if (this._sharedState != null)
			{
				this._sharedState.EnsureDeferredValidation(grid);
			}
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x002B5FE2 File Offset: 0x002B4FE2
		internal void UpdateMinSize(double minSize)
		{
			this._minSize = Math.Max(this._minSize, minSize);
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x002B5FF6 File Offset: 0x002B4FF6
		internal void SetMinSize(double minSize)
		{
			this._minSize = minSize;
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x002B6000 File Offset: 0x002B5000
		internal static void OnUserSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.Invalidate();
					return;
				}
				Grid grid = (Grid)definitionBase.Parent;
				if (((GridLength)e.OldValue).GridUnitType != ((GridLength)e.NewValue).GridUnitType)
				{
					grid.Invalidate();
					return;
				}
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x002B6074 File Offset: 0x002B5074
		internal static bool IsUserSizePropertyValueValid(object value)
		{
			return ((GridLength)value).Value >= 0.0;
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x002B60A0 File Offset: 0x002B50A0
		internal static void OnUserMinSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				((Grid)definitionBase.Parent).InvalidateMeasure();
			}
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0017B610 File Offset: 0x0017A610
		internal static bool IsUserMinSizePropertyValueValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0 && !double.IsPositiveInfinity(num);
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x002B60A0 File Offset: 0x002B50A0
		internal static void OnUserMaxSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				((Grid)definitionBase.Parent).InvalidateMeasure();
			}
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x0017B644 File Offset: 0x0017A644
		internal static bool IsUserMaxSizePropertyValueValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x002B60CC File Offset: 0x002B50CC
		internal static void OnIsSharedSizeScopePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				DefinitionBase.SharedSizeScope value = new DefinitionBase.SharedSizeScope();
				d.SetValue(DefinitionBase.PrivateSharedSizeScopeProperty, value);
				return;
			}
			d.ClearValue(DefinitionBase.PrivateSharedSizeScopeProperty);
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x060067C6 RID: 26566 RVA: 0x002B6105 File Offset: 0x002B5105
		internal bool IsShared
		{
			get
			{
				return this._sharedState != null;
			}
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x060067C7 RID: 26567 RVA: 0x002B6110 File Offset: 0x002B5110
		internal GridLength UserSize
		{
			get
			{
				if (this._sharedState == null)
				{
					return this.UserSizeValueCache;
				}
				return this._sharedState.UserSize;
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x060067C8 RID: 26568 RVA: 0x002B612C File Offset: 0x002B512C
		internal double UserMinSize
		{
			get
			{
				return this.UserMinSizeValueCache;
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x060067C9 RID: 26569 RVA: 0x002B6134 File Offset: 0x002B5134
		internal double UserMaxSize
		{
			get
			{
				return this.UserMaxSizeValueCache;
			}
		}

		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x060067CA RID: 26570 RVA: 0x002B613C File Offset: 0x002B513C
		// (set) Token: 0x060067CB RID: 26571 RVA: 0x002B6144 File Offset: 0x002B5144
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x060067CC RID: 26572 RVA: 0x002B614D File Offset: 0x002B514D
		// (set) Token: 0x060067CD RID: 26573 RVA: 0x002B6155 File Offset: 0x002B5155
		internal Grid.LayoutTimeSizeType SizeType
		{
			get
			{
				return this._sizeType;
			}
			set
			{
				this._sizeType = value;
			}
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x060067CE RID: 26574 RVA: 0x002B615E File Offset: 0x002B515E
		// (set) Token: 0x060067CF RID: 26575 RVA: 0x002B6166 File Offset: 0x002B5166
		internal double MeasureSize
		{
			get
			{
				return this._measureSize;
			}
			set
			{
				this._measureSize = value;
			}
		}

		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x060067D0 RID: 26576 RVA: 0x002B6170 File Offset: 0x002B5170
		internal double PreferredSize
		{
			get
			{
				double num = this.MinSize;
				if (this._sizeType != Grid.LayoutTimeSizeType.Auto && num < this._measureSize)
				{
					num = this._measureSize;
				}
				return num;
			}
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x060067D1 RID: 26577 RVA: 0x002B619E File Offset: 0x002B519E
		// (set) Token: 0x060067D2 RID: 26578 RVA: 0x002B61A6 File Offset: 0x002B51A6
		internal double SizeCache
		{
			get
			{
				return this._sizeCache;
			}
			set
			{
				this._sizeCache = value;
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x060067D3 RID: 26579 RVA: 0x002B61B0 File Offset: 0x002B51B0
		internal double MinSize
		{
			get
			{
				double minSize = this._minSize;
				if (this.UseSharedMinimum && this._sharedState != null && minSize < this._sharedState.MinSize)
				{
					minSize = this._sharedState.MinSize;
				}
				return minSize;
			}
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x060067D4 RID: 26580 RVA: 0x002B61F0 File Offset: 0x002B51F0
		internal double MinSizeForArrange
		{
			get
			{
				double minSize = this._minSize;
				if (this._sharedState != null && (this.UseSharedMinimum || !this.LayoutWasUpdated) && minSize < this._sharedState.MinSize)
				{
					minSize = this._sharedState.MinSize;
				}
				return minSize;
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x060067D5 RID: 26581 RVA: 0x002B6237 File Offset: 0x002B5237
		internal double RawMinSize
		{
			get
			{
				return this._minSize;
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x060067D6 RID: 26582 RVA: 0x002B623F File Offset: 0x002B523F
		// (set) Token: 0x060067D7 RID: 26583 RVA: 0x002B6247 File Offset: 0x002B5247
		internal double FinalOffset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x060067D8 RID: 26584 RVA: 0x002B6250 File Offset: 0x002B5250
		internal GridLength UserSizeValueCache
		{
			get
			{
				return (GridLength)base.GetValue(this._isColumnDefinition ? ColumnDefinition.WidthProperty : RowDefinition.HeightProperty);
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x060067D9 RID: 26585 RVA: 0x002B6271 File Offset: 0x002B5271
		internal double UserMinSizeValueCache
		{
			get
			{
				return (double)base.GetValue(this._isColumnDefinition ? ColumnDefinition.MinWidthProperty : RowDefinition.MinHeightProperty);
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x060067DA RID: 26586 RVA: 0x002B6292 File Offset: 0x002B5292
		internal double UserMaxSizeValueCache
		{
			get
			{
				return (double)base.GetValue(this._isColumnDefinition ? ColumnDefinition.MaxWidthProperty : RowDefinition.MaxHeightProperty);
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x060067DB RID: 26587 RVA: 0x002B62B3 File Offset: 0x002B52B3
		internal bool InParentLogicalTree
		{
			get
			{
				return this._parentIndex != -1;
			}
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x002B62C1 File Offset: 0x002B52C1
		private void SetFlags(bool value, DefinitionBase.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x002B62E0 File Offset: 0x002B52E0
		private bool CheckFlagsAnd(DefinitionBase.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x002B62F0 File Offset: 0x002B52F0
		private static void OnSharedSizeGroupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				string text = (string)e.NewValue;
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.RemoveMember(definitionBase);
					definitionBase._sharedState = null;
				}
				if (definitionBase._sharedState == null && text != null)
				{
					DefinitionBase.SharedSizeScope privateSharedSizeScope = definitionBase.PrivateSharedSizeScope;
					if (privateSharedSizeScope != null)
					{
						definitionBase._sharedState = privateSharedSizeScope.EnsureSharedState(text);
						definitionBase._sharedState.AddMember(definitionBase);
					}
				}
			}
		}

		// Token: 0x060067DF RID: 26591 RVA: 0x002B6364 File Offset: 0x002B5364
		private static bool SharedSizeGroupPropertyValueValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			string text = (string)value;
			if (text != string.Empty)
			{
				int num = -1;
				while (++num < text.Length)
				{
					bool flag = char.IsDigit(text[num]);
					if ((num == 0 && flag) || (!flag && !char.IsLetter(text[num]) && '_' != text[num]))
					{
						break;
					}
				}
				if (num == text.Length)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x002B63D8 File Offset: 0x002B53D8
		private static void OnPrivateSharedSizeScopePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				DefinitionBase.SharedSizeScope sharedSizeScope = (DefinitionBase.SharedSizeScope)e.NewValue;
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.RemoveMember(definitionBase);
					definitionBase._sharedState = null;
				}
				if (definitionBase._sharedState == null && sharedSizeScope != null && definitionBase.SharedSizeGroup != null)
				{
					definitionBase._sharedState = sharedSizeScope.EnsureSharedState(definitionBase.SharedSizeGroup);
					definitionBase._sharedState.AddMember(definitionBase);
				}
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x060067E1 RID: 26593 RVA: 0x002B644D File Offset: 0x002B544D
		private DefinitionBase.SharedSizeScope PrivateSharedSizeScope
		{
			get
			{
				return (DefinitionBase.SharedSizeScope)base.GetValue(DefinitionBase.PrivateSharedSizeScopeProperty);
			}
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x060067E2 RID: 26594 RVA: 0x002B645F File Offset: 0x002B545F
		// (set) Token: 0x060067E3 RID: 26595 RVA: 0x002B6469 File Offset: 0x002B5469
		private bool UseSharedMinimum
		{
			get
			{
				return this.CheckFlagsAnd(DefinitionBase.Flags.UseSharedMinimum);
			}
			set
			{
				this.SetFlags(value, DefinitionBase.Flags.UseSharedMinimum);
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x060067E4 RID: 26596 RVA: 0x002B6474 File Offset: 0x002B5474
		// (set) Token: 0x060067E5 RID: 26597 RVA: 0x002B647E File Offset: 0x002B547E
		private bool LayoutWasUpdated
		{
			get
			{
				return this.CheckFlagsAnd(DefinitionBase.Flags.LayoutWasUpdated);
			}
			set
			{
				this.SetFlags(value, DefinitionBase.Flags.LayoutWasUpdated);
			}
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x002B648C File Offset: 0x002B548C
		static DefinitionBase()
		{
			DefinitionBase.PrivateSharedSizeScopeProperty.OverrideMetadata(typeof(DefinitionBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(DefinitionBase.OnPrivateSharedSizeScopePropertyChanged)));
		}

		// Token: 0x04003453 RID: 13395
		private readonly bool _isColumnDefinition;

		// Token: 0x04003454 RID: 13396
		private DefinitionBase.Flags _flags;

		// Token: 0x04003455 RID: 13397
		private int _parentIndex;

		// Token: 0x04003456 RID: 13398
		private Grid.LayoutTimeSizeType _sizeType;

		// Token: 0x04003457 RID: 13399
		private double _minSize;

		// Token: 0x04003458 RID: 13400
		private double _measureSize;

		// Token: 0x04003459 RID: 13401
		private double _sizeCache;

		// Token: 0x0400345A RID: 13402
		private double _offset;

		// Token: 0x0400345B RID: 13403
		private DefinitionBase.SharedSizeState _sharedState;

		// Token: 0x0400345C RID: 13404
		internal const bool ThisIsColumnDefinition = true;

		// Token: 0x0400345D RID: 13405
		internal const bool ThisIsRowDefinition = false;

		// Token: 0x0400345E RID: 13406
		internal static readonly DependencyProperty PrivateSharedSizeScopeProperty = DependencyProperty.RegisterAttached("PrivateSharedSizeScope", typeof(DefinitionBase.SharedSizeScope), typeof(DefinitionBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400345F RID: 13407
		public static readonly DependencyProperty SharedSizeGroupProperty = DependencyProperty.Register("SharedSizeGroup", typeof(string), typeof(DefinitionBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(DefinitionBase.OnSharedSizeGroupPropertyChanged)), new ValidateValueCallback(DefinitionBase.SharedSizeGroupPropertyValueValid));

		// Token: 0x02000BD1 RID: 3025
		[Flags]
		private enum Flags : byte
		{
			// Token: 0x04004A04 RID: 18948
			UseSharedMinimum = 32,
			// Token: 0x04004A05 RID: 18949
			LayoutWasUpdated = 64
		}

		// Token: 0x02000BD2 RID: 3026
		private class SharedSizeScope
		{
			// Token: 0x06008F85 RID: 36741 RVA: 0x00344608 File Offset: 0x00343608
			internal DefinitionBase.SharedSizeState EnsureSharedState(string sharedSizeGroup)
			{
				DefinitionBase.SharedSizeState sharedSizeState = this._registry[sharedSizeGroup] as DefinitionBase.SharedSizeState;
				if (sharedSizeState == null)
				{
					sharedSizeState = new DefinitionBase.SharedSizeState(this, sharedSizeGroup);
					this._registry[sharedSizeGroup] = sharedSizeState;
				}
				return sharedSizeState;
			}

			// Token: 0x06008F86 RID: 36742 RVA: 0x00344640 File Offset: 0x00343640
			internal void Remove(object key)
			{
				this._registry.Remove(key);
			}

			// Token: 0x04004A06 RID: 18950
			private Hashtable _registry = new Hashtable();
		}

		// Token: 0x02000BD3 RID: 3027
		private class SharedSizeState
		{
			// Token: 0x06008F88 RID: 36744 RVA: 0x00344661 File Offset: 0x00343661
			internal SharedSizeState(DefinitionBase.SharedSizeScope sharedSizeScope, string sharedSizeGroupId)
			{
				this._sharedSizeScope = sharedSizeScope;
				this._sharedSizeGroupId = sharedSizeGroupId;
				this._registry = new List<DefinitionBase>();
				this._layoutUpdated = new EventHandler(this.OnLayoutUpdated);
				this._broadcastInvalidation = true;
			}

			// Token: 0x06008F89 RID: 36745 RVA: 0x0034469B File Offset: 0x0034369B
			internal void AddMember(DefinitionBase member)
			{
				this._registry.Add(member);
				this.Invalidate();
			}

			// Token: 0x06008F8A RID: 36746 RVA: 0x003446AF File Offset: 0x003436AF
			internal void RemoveMember(DefinitionBase member)
			{
				this.Invalidate();
				this._registry.Remove(member);
				if (this._registry.Count == 0)
				{
					this._sharedSizeScope.Remove(this._sharedSizeGroupId);
				}
			}

			// Token: 0x06008F8B RID: 36747 RVA: 0x003446E4 File Offset: 0x003436E4
			internal void Invalidate()
			{
				this._userSizeValid = false;
				if (this._broadcastInvalidation)
				{
					int i = 0;
					int count = this._registry.Count;
					while (i < count)
					{
						((Grid)this._registry[i].Parent).Invalidate();
						i++;
					}
					this._broadcastInvalidation = false;
				}
			}

			// Token: 0x06008F8C RID: 36748 RVA: 0x0034473A File Offset: 0x0034373A
			internal void EnsureDeferredValidation(UIElement layoutUpdatedHost)
			{
				if (this._layoutUpdatedHost == null)
				{
					this._layoutUpdatedHost = layoutUpdatedHost;
					this._layoutUpdatedHost.LayoutUpdated += this._layoutUpdated;
				}
			}

			// Token: 0x17001F63 RID: 8035
			// (get) Token: 0x06008F8D RID: 36749 RVA: 0x0034475C File Offset: 0x0034375C
			internal double MinSize
			{
				get
				{
					if (!this._userSizeValid)
					{
						this.EnsureUserSizeValid();
					}
					return this._minSize;
				}
			}

			// Token: 0x17001F64 RID: 8036
			// (get) Token: 0x06008F8E RID: 36750 RVA: 0x00344772 File Offset: 0x00343772
			internal GridLength UserSize
			{
				get
				{
					if (!this._userSizeValid)
					{
						this.EnsureUserSizeValid();
					}
					return this._userSize;
				}
			}

			// Token: 0x06008F8F RID: 36751 RVA: 0x00344788 File Offset: 0x00343788
			private void EnsureUserSizeValid()
			{
				this._userSize = new GridLength(1.0, GridUnitType.Auto);
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					GridLength userSizeValueCache = this._registry[i].UserSizeValueCache;
					if (userSizeValueCache.GridUnitType == GridUnitType.Pixel)
					{
						if (this._userSize.GridUnitType == GridUnitType.Auto)
						{
							this._userSize = userSizeValueCache;
						}
						else if (this._userSize.Value < userSizeValueCache.Value)
						{
							this._userSize = userSizeValueCache;
						}
					}
					i++;
				}
				this._minSize = (this._userSize.IsAbsolute ? this._userSize.Value : 0.0);
				this._userSizeValid = true;
			}

			// Token: 0x06008F90 RID: 36752 RVA: 0x00344840 File Offset: 0x00343840
			private void OnLayoutUpdated(object sender, EventArgs e)
			{
				double num = 0.0;
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					num = Math.Max(num, this._registry[i]._minSize);
					i++;
				}
				bool flag = !DoubleUtil.AreClose(this._minSize, num);
				int j = 0;
				int count2 = this._registry.Count;
				while (j < count2)
				{
					DefinitionBase definitionBase = this._registry[j];
					bool flag2 = !DoubleUtil.AreClose(definitionBase._minSize, num);
					bool flag3;
					if (!definitionBase.UseSharedMinimum)
					{
						flag3 = !flag2;
					}
					else if (flag2)
					{
						flag3 = !flag;
					}
					else
					{
						flag3 = (definitionBase.LayoutWasUpdated && DoubleUtil.GreaterThanOrClose(definitionBase._minSize, this.MinSize));
					}
					if (!flag3)
					{
						((Grid)definitionBase.Parent).InvalidateMeasure();
					}
					else if (!DoubleUtil.AreClose(num, definitionBase.SizeCache))
					{
						((Grid)definitionBase.Parent).InvalidateArrange();
					}
					definitionBase.UseSharedMinimum = flag2;
					definitionBase.LayoutWasUpdated = false;
					j++;
				}
				this._minSize = num;
				this._layoutUpdatedHost.LayoutUpdated -= this._layoutUpdated;
				this._layoutUpdatedHost = null;
				this._broadcastInvalidation = true;
			}

			// Token: 0x04004A07 RID: 18951
			private readonly DefinitionBase.SharedSizeScope _sharedSizeScope;

			// Token: 0x04004A08 RID: 18952
			private readonly string _sharedSizeGroupId;

			// Token: 0x04004A09 RID: 18953
			private readonly List<DefinitionBase> _registry;

			// Token: 0x04004A0A RID: 18954
			private readonly EventHandler _layoutUpdated;

			// Token: 0x04004A0B RID: 18955
			private UIElement _layoutUpdatedHost;

			// Token: 0x04004A0C RID: 18956
			private bool _broadcastInvalidation;

			// Token: 0x04004A0D RID: 18957
			private bool _userSizeValid;

			// Token: 0x04004A0E RID: 18958
			private GridLength _userSize;

			// Token: 0x04004A0F RID: 18959
			private double _minSize;
		}
	}
}
