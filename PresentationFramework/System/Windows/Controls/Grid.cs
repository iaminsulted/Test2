using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000780 RID: 1920
	public class Grid : Panel, IAddChild
	{
		// Token: 0x060069A7 RID: 27047 RVA: 0x002BD598 File Offset: 0x002BC598
		static Grid()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.Grid);
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x002BD78C File Offset: 0x002BC78C
		public Grid()
		{
			this.SetFlags((bool)Grid.ShowGridLinesProperty.GetDefaultValue(base.DependencyObjectType), Grid.Flags.ShowGridLinesPropertyValue);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x002BD7B4 File Offset: 0x002BC7B4
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			UIElement uielement = value as UIElement;
			if (uielement != null)
			{
				base.Children.Add(uielement);
				return;
			}
			throw new ArgumentException(SR.Get("Grid_UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(UIElement)
			}), "value");
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x060069AB RID: 27051 RVA: 0x002BD818 File Offset: 0x002BC818
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				bool flag = base.VisualChildrenCount == 0 || base.IsItemsHost;
				if (flag)
				{
					Grid.ExtendedData extData = this.ExtData;
					if (extData == null || ((extData.ColumnDefinitions == null || extData.ColumnDefinitions.Count == 0) && (extData.RowDefinitions == null || extData.RowDefinitions.Count == 0)))
					{
						return EmptyEnumerator.Instance;
					}
				}
				return new Grid.GridChildrenCollectionEnumeratorSimple(this, !flag);
			}
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x002BD87E File Offset: 0x002BC87E
		public static void SetColumn(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.ColumnProperty, value);
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x002BD89F File Offset: 0x002BC89F
		[AttachedPropertyBrowsableForChildren]
		public static int GetColumn(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.ColumnProperty);
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x002BD8BF File Offset: 0x002BC8BF
		public static void SetRow(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.RowProperty, value);
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x002BD8E0 File Offset: 0x002BC8E0
		[AttachedPropertyBrowsableForChildren]
		public static int GetRow(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.RowProperty);
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x002BD900 File Offset: 0x002BC900
		public static void SetColumnSpan(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.ColumnSpanProperty, value);
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x002BD921 File Offset: 0x002BC921
		[AttachedPropertyBrowsableForChildren]
		public static int GetColumnSpan(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.ColumnSpanProperty);
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x002BD941 File Offset: 0x002BC941
		public static void SetRowSpan(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.RowSpanProperty, value);
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x002BD962 File Offset: 0x002BC962
		[AttachedPropertyBrowsableForChildren]
		public static int GetRowSpan(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Grid.RowSpanProperty);
		}

		// Token: 0x060069B4 RID: 27060 RVA: 0x002BD982 File Offset: 0x002BC982
		public static void SetIsSharedSizeScope(UIElement element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Grid.IsSharedSizeScopeProperty, value);
		}

		// Token: 0x060069B5 RID: 27061 RVA: 0x002BD99E File Offset: 0x002BC99E
		public static bool GetIsSharedSizeScope(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Grid.IsSharedSizeScopeProperty);
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x060069B6 RID: 27062 RVA: 0x002BD9BE File Offset: 0x002BC9BE
		// (set) Token: 0x060069B7 RID: 27063 RVA: 0x002BD9CB File Offset: 0x002BC9CB
		public bool ShowGridLines
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.ShowGridLinesPropertyValue);
			}
			set
			{
				base.SetValue(Grid.ShowGridLinesProperty, value);
			}
		}

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x060069B8 RID: 27064 RVA: 0x002BD9D9 File Offset: 0x002BC9D9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColumnDefinitionCollection ColumnDefinitions
		{
			get
			{
				if (this._data == null)
				{
					this._data = new Grid.ExtendedData();
				}
				if (this._data.ColumnDefinitions == null)
				{
					this._data.ColumnDefinitions = new ColumnDefinitionCollection(this);
				}
				return this._data.ColumnDefinitions;
			}
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x060069B9 RID: 27065 RVA: 0x002BDA17 File Offset: 0x002BCA17
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public RowDefinitionCollection RowDefinitions
		{
			get
			{
				if (this._data == null)
				{
					this._data = new Grid.ExtendedData();
				}
				if (this._data.RowDefinitions == null)
				{
					this._data.RowDefinitions = new RowDefinitionCollection(this);
				}
				return this._data.RowDefinitions;
			}
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x002BDA55 File Offset: 0x002BCA55
		protected override Visual GetVisualChild(int index)
		{
			if (index != base.VisualChildrenCount)
			{
				return base.GetVisualChild(index);
			}
			if (this._gridLinesRenderer == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._gridLinesRenderer;
		}

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x060069BB RID: 27067 RVA: 0x002BDA91 File Offset: 0x002BCA91
		protected override int VisualChildrenCount
		{
			get
			{
				return base.VisualChildrenCount + ((this._gridLinesRenderer != null) ? 1 : 0);
			}
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x002BDAA8 File Offset: 0x002BCAA8
		protected override Size MeasureOverride(Size constraint)
		{
			Grid.ExtendedData extData = this.ExtData;
			Size result;
			try
			{
				this.ListenToNotifications = true;
				this.MeasureOverrideInProgress = true;
				if (extData == null)
				{
					result = default(Size);
					UIElementCollection internalChildren = base.InternalChildren;
					int i = 0;
					int count = internalChildren.Count;
					while (i < count)
					{
						UIElement uielement = internalChildren[i];
						if (uielement != null)
						{
							uielement.Measure(constraint);
							result.Width = Math.Max(result.Width, uielement.DesiredSize.Width);
							result.Height = Math.Max(result.Height, uielement.DesiredSize.Height);
						}
						i++;
					}
				}
				else
				{
					bool flag = double.IsPositiveInfinity(constraint.Width);
					bool flag2 = double.IsPositiveInfinity(constraint.Height);
					if (this.RowDefinitionCollectionDirty || this.ColumnDefinitionCollectionDirty)
					{
						if (this._definitionIndices != null)
						{
							Array.Clear(this._definitionIndices, 0, this._definitionIndices.Length);
							this._definitionIndices = null;
						}
						if (base.UseLayoutRounding && this._roundingErrors != null)
						{
							Array.Clear(this._roundingErrors, 0, this._roundingErrors.Length);
							this._roundingErrors = null;
						}
					}
					this.ValidateDefinitionsUStructure();
					this.ValidateDefinitionsLayout(this.DefinitionsU, flag);
					this.ValidateDefinitionsVStructure();
					this.ValidateDefinitionsLayout(this.DefinitionsV, flag2);
					this.CellsStructureDirty |= (this.SizeToContentU != flag || this.SizeToContentV != flag2);
					this.SizeToContentU = flag;
					this.SizeToContentV = flag2;
					this.ValidateCells();
					this.MeasureCellsGroup(extData.CellGroup1, constraint, false, false);
					if (!this.HasGroup3CellsInAutoRows)
					{
						if (this.HasStarCellsV)
						{
							this.ResolveStar(this.DefinitionsV, constraint.Height);
						}
						this.MeasureCellsGroup(extData.CellGroup2, constraint, false, false);
						if (this.HasStarCellsU)
						{
							this.ResolveStar(this.DefinitionsU, constraint.Width);
						}
						this.MeasureCellsGroup(extData.CellGroup3, constraint, false, false);
					}
					else if (extData.CellGroup2 > this.PrivateCells.Length)
					{
						if (this.HasStarCellsU)
						{
							this.ResolveStar(this.DefinitionsU, constraint.Width);
						}
						this.MeasureCellsGroup(extData.CellGroup3, constraint, false, false);
						if (this.HasStarCellsV)
						{
							this.ResolveStar(this.DefinitionsV, constraint.Height);
						}
					}
					else
					{
						bool flag3 = false;
						int num = 0;
						double[] minSizes = this.CacheMinSizes(extData.CellGroup2, false);
						double[] minSizes2 = this.CacheMinSizes(extData.CellGroup3, true);
						this.MeasureCellsGroup(extData.CellGroup2, constraint, false, true);
						do
						{
							if (flag3)
							{
								this.ApplyCachedMinSizes(minSizes2, true);
							}
							if (this.HasStarCellsU)
							{
								this.ResolveStar(this.DefinitionsU, constraint.Width);
							}
							this.MeasureCellsGroup(extData.CellGroup3, constraint, false, false);
							this.ApplyCachedMinSizes(minSizes, false);
							if (this.HasStarCellsV)
							{
								this.ResolveStar(this.DefinitionsV, constraint.Height);
							}
							this.MeasureCellsGroup(extData.CellGroup2, constraint, num == 5, false, out flag3);
						}
						while (flag3 && ++num <= 5);
					}
					this.MeasureCellsGroup(extData.CellGroup4, constraint, false, false);
					result = new Size(this.CalculateDesiredSize(this.DefinitionsU), this.CalculateDesiredSize(this.DefinitionsV));
				}
			}
			finally
			{
				this.MeasureOverrideInProgress = false;
			}
			return result;
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x002BDE0C File Offset: 0x002BCE0C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			try
			{
				this.ArrangeOverrideInProgress = true;
				if (this._data == null)
				{
					UIElementCollection internalChildren = base.InternalChildren;
					int i = 0;
					int count = internalChildren.Count;
					while (i < count)
					{
						UIElement uielement = internalChildren[i];
						if (uielement != null)
						{
							uielement.Arrange(new Rect(arrangeSize));
						}
						i++;
					}
				}
				else
				{
					this.SetFinalSize(this.DefinitionsU, arrangeSize.Width, true);
					this.SetFinalSize(this.DefinitionsV, arrangeSize.Height, false);
					UIElementCollection internalChildren2 = base.InternalChildren;
					for (int j = 0; j < this.PrivateCells.Length; j++)
					{
						UIElement uielement2 = internalChildren2[j];
						if (uielement2 != null)
						{
							int columnIndex = this.PrivateCells[j].ColumnIndex;
							int rowIndex = this.PrivateCells[j].RowIndex;
							int columnSpan = this.PrivateCells[j].ColumnSpan;
							int rowSpan = this.PrivateCells[j].RowSpan;
							Rect finalRect = new Rect((columnIndex == 0) ? 0.0 : this.DefinitionsU[columnIndex].FinalOffset, (rowIndex == 0) ? 0.0 : this.DefinitionsV[rowIndex].FinalOffset, this.GetFinalSizeForRange(this.DefinitionsU, columnIndex, columnSpan), this.GetFinalSizeForRange(this.DefinitionsV, rowIndex, rowSpan));
							uielement2.Arrange(finalRect);
						}
					}
					Grid.GridLinesRenderer gridLinesRenderer = this.EnsureGridLinesRenderer();
					if (gridLinesRenderer != null)
					{
						gridLinesRenderer.UpdateRenderBounds(arrangeSize);
					}
				}
			}
			finally
			{
				this.SetValid();
				this.ArrangeOverrideInProgress = false;
			}
			return arrangeSize;
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x002BDFB8 File Offset: 0x002BCFB8
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			this.CellsStructureDirty = true;
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x002BDFC9 File Offset: 0x002BCFC9
		internal void Invalidate()
		{
			this.CellsStructureDirty = true;
			base.InvalidateMeasure();
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x002BDFD8 File Offset: 0x002BCFD8
		internal double GetFinalColumnDefinitionWidth(int columnIndex)
		{
			double num = 0.0;
			Invariant.Assert(this._data != null);
			if (!this.ColumnDefinitionCollectionDirty)
			{
				DefinitionBase[] definitionsU = this.DefinitionsU;
				num = definitionsU[(columnIndex + 1) % definitionsU.Length].FinalOffset;
				if (columnIndex != 0)
				{
					num -= definitionsU[columnIndex].FinalOffset;
				}
			}
			return num;
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x002BE02C File Offset: 0x002BD02C
		internal double GetFinalRowDefinitionHeight(int rowIndex)
		{
			double num = 0.0;
			Invariant.Assert(this._data != null);
			if (!this.RowDefinitionCollectionDirty)
			{
				DefinitionBase[] definitionsV = this.DefinitionsV;
				num = definitionsV[(rowIndex + 1) % definitionsV.Length].FinalOffset;
				if (rowIndex != 0)
				{
					num -= definitionsV[rowIndex].FinalOffset;
				}
			}
			return num;
		}

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x060069C2 RID: 27074 RVA: 0x002BE07E File Offset: 0x002BD07E
		// (set) Token: 0x060069C3 RID: 27075 RVA: 0x002BE08B File Offset: 0x002BD08B
		internal bool MeasureOverrideInProgress
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.MeasureOverrideInProgress);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.MeasureOverrideInProgress);
			}
		}

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x060069C4 RID: 27076 RVA: 0x002BE099 File Offset: 0x002BD099
		// (set) Token: 0x060069C5 RID: 27077 RVA: 0x002BE0A6 File Offset: 0x002BD0A6
		internal bool ArrangeOverrideInProgress
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.ArrangeOverrideInProgress);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.ArrangeOverrideInProgress);
			}
		}

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x060069C6 RID: 27078 RVA: 0x002BE0B4 File Offset: 0x002BD0B4
		// (set) Token: 0x060069C7 RID: 27079 RVA: 0x002BE0C0 File Offset: 0x002BD0C0
		internal bool ColumnDefinitionCollectionDirty
		{
			get
			{
				return !this.CheckFlagsAnd(Grid.Flags.ValidDefinitionsUStructure);
			}
			set
			{
				this.SetFlags(!value, Grid.Flags.ValidDefinitionsUStructure);
			}
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x060069C8 RID: 27080 RVA: 0x002BE0CD File Offset: 0x002BD0CD
		// (set) Token: 0x060069C9 RID: 27081 RVA: 0x002BE0D9 File Offset: 0x002BD0D9
		internal bool RowDefinitionCollectionDirty
		{
			get
			{
				return !this.CheckFlagsAnd(Grid.Flags.ValidDefinitionsVStructure);
			}
			set
			{
				this.SetFlags(!value, Grid.Flags.ValidDefinitionsVStructure);
			}
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x002BE0E6 File Offset: 0x002BD0E6
		private void ValidateCells()
		{
			if (this.CellsStructureDirty)
			{
				this.ValidateCellsCore();
				this.CellsStructureDirty = false;
			}
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x002BE100 File Offset: 0x002BD100
		private void ValidateCellsCore()
		{
			UIElementCollection internalChildren = base.InternalChildren;
			Grid.ExtendedData extData = this.ExtData;
			extData.CellCachesCollection = new Grid.CellCache[internalChildren.Count];
			extData.CellGroup1 = int.MaxValue;
			extData.CellGroup2 = int.MaxValue;
			extData.CellGroup3 = int.MaxValue;
			extData.CellGroup4 = int.MaxValue;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = this.PrivateCells.Length - 1; i >= 0; i--)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					Grid.CellCache cellCache = default(Grid.CellCache);
					cellCache.ColumnIndex = Math.Min(Grid.GetColumn(uielement), this.DefinitionsU.Length - 1);
					cellCache.RowIndex = Math.Min(Grid.GetRow(uielement), this.DefinitionsV.Length - 1);
					cellCache.ColumnSpan = Math.Min(Grid.GetColumnSpan(uielement), this.DefinitionsU.Length - cellCache.ColumnIndex);
					cellCache.RowSpan = Math.Min(Grid.GetRowSpan(uielement), this.DefinitionsV.Length - cellCache.RowIndex);
					cellCache.SizeTypeU = this.GetLengthTypeForRange(this.DefinitionsU, cellCache.ColumnIndex, cellCache.ColumnSpan);
					cellCache.SizeTypeV = this.GetLengthTypeForRange(this.DefinitionsV, cellCache.RowIndex, cellCache.RowSpan);
					flag |= cellCache.IsStarU;
					flag2 |= cellCache.IsStarV;
					if (!cellCache.IsStarV)
					{
						if (!cellCache.IsStarU)
						{
							cellCache.Next = extData.CellGroup1;
							extData.CellGroup1 = i;
						}
						else
						{
							cellCache.Next = extData.CellGroup3;
							extData.CellGroup3 = i;
							flag3 |= cellCache.IsAutoV;
						}
					}
					else if (cellCache.IsAutoU && !cellCache.IsStarU)
					{
						cellCache.Next = extData.CellGroup2;
						extData.CellGroup2 = i;
					}
					else
					{
						cellCache.Next = extData.CellGroup4;
						extData.CellGroup4 = i;
					}
					this.PrivateCells[i] = cellCache;
				}
			}
			this.HasStarCellsU = flag;
			this.HasStarCellsV = flag2;
			this.HasGroup3CellsInAutoRows = flag3;
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x002BE31C File Offset: 0x002BD31C
		private void ValidateDefinitionsUStructure()
		{
			if (this.ColumnDefinitionCollectionDirty)
			{
				Grid.ExtendedData extData = this.ExtData;
				if (extData.ColumnDefinitions == null)
				{
					if (extData.DefinitionsU == null)
					{
						extData.DefinitionsU = new DefinitionBase[]
						{
							new ColumnDefinition()
						};
					}
				}
				else
				{
					extData.ColumnDefinitions.InternalTrimToSize();
					if (extData.ColumnDefinitions.InternalCount == 0)
					{
						extData.DefinitionsU = new DefinitionBase[]
						{
							new ColumnDefinition()
						};
					}
					else
					{
						extData.DefinitionsU = extData.ColumnDefinitions.InternalItems;
					}
				}
				this.ColumnDefinitionCollectionDirty = false;
			}
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x002BE3A4 File Offset: 0x002BD3A4
		private void ValidateDefinitionsVStructure()
		{
			if (this.RowDefinitionCollectionDirty)
			{
				Grid.ExtendedData extData = this.ExtData;
				if (extData.RowDefinitions == null)
				{
					if (extData.DefinitionsV == null)
					{
						extData.DefinitionsV = new DefinitionBase[]
						{
							new RowDefinition()
						};
					}
				}
				else
				{
					extData.RowDefinitions.InternalTrimToSize();
					if (extData.RowDefinitions.InternalCount == 0)
					{
						extData.DefinitionsV = new DefinitionBase[]
						{
							new RowDefinition()
						};
					}
					else
					{
						extData.DefinitionsV = extData.RowDefinitions.InternalItems;
					}
				}
				this.RowDefinitionCollectionDirty = false;
			}
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x002BE42C File Offset: 0x002BD42C
		private void ValidateDefinitionsLayout(DefinitionBase[] definitions, bool treatStarAsAuto)
		{
			for (int i = 0; i < definitions.Length; i++)
			{
				definitions[i].OnBeforeLayout(this);
				double num = definitions[i].UserMinSize;
				double userMaxSize = definitions[i].UserMaxSize;
				double val = 0.0;
				switch (definitions[i].UserSize.GridUnitType)
				{
				case GridUnitType.Auto:
					definitions[i].SizeType = Grid.LayoutTimeSizeType.Auto;
					val = double.PositiveInfinity;
					break;
				case GridUnitType.Pixel:
					definitions[i].SizeType = Grid.LayoutTimeSizeType.Pixel;
					val = definitions[i].UserSize.Value;
					num = Math.Max(num, Math.Min(val, userMaxSize));
					break;
				case GridUnitType.Star:
					if (treatStarAsAuto)
					{
						definitions[i].SizeType = Grid.LayoutTimeSizeType.Auto;
						val = double.PositiveInfinity;
					}
					else
					{
						definitions[i].SizeType = Grid.LayoutTimeSizeType.Star;
						val = double.PositiveInfinity;
					}
					break;
				}
				definitions[i].UpdateMinSize(num);
				definitions[i].MeasureSize = Math.Max(num, Math.Min(val, userMaxSize));
			}
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x002BE524 File Offset: 0x002BD524
		private double[] CacheMinSizes(int cellsHead, bool isRows)
		{
			double[] array = isRows ? new double[this.DefinitionsV.Length] : new double[this.DefinitionsU.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1.0;
			}
			int num = cellsHead;
			do
			{
				if (isRows)
				{
					array[this.PrivateCells[num].RowIndex] = this.DefinitionsV[this.PrivateCells[num].RowIndex].RawMinSize;
				}
				else
				{
					array[this.PrivateCells[num].ColumnIndex] = this.DefinitionsU[this.PrivateCells[num].ColumnIndex].RawMinSize;
				}
				num = this.PrivateCells[num].Next;
			}
			while (num < this.PrivateCells.Length);
			return array;
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x002BE5F4 File Offset: 0x002BD5F4
		private void ApplyCachedMinSizes(double[] minSizes, bool isRows)
		{
			for (int i = 0; i < minSizes.Length; i++)
			{
				if (DoubleUtil.GreaterThanOrClose(minSizes[i], 0.0))
				{
					if (isRows)
					{
						this.DefinitionsV[i].SetMinSize(minSizes[i]);
					}
					else
					{
						this.DefinitionsU[i].SetMinSize(minSizes[i]);
					}
				}
			}
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x002BE648 File Offset: 0x002BD648
		private void MeasureCellsGroup(int cellsHead, Size referenceSize, bool ignoreDesiredSizeU, bool forceInfinityV)
		{
			bool flag;
			this.MeasureCellsGroup(cellsHead, referenceSize, ignoreDesiredSizeU, forceInfinityV, out flag);
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x002BE664 File Offset: 0x002BD664
		private void MeasureCellsGroup(int cellsHead, Size referenceSize, bool ignoreDesiredSizeU, bool forceInfinityV, out bool hasDesiredSizeUChanged)
		{
			hasDesiredSizeUChanged = false;
			if (cellsHead >= this.PrivateCells.Length)
			{
				return;
			}
			UIElementCollection internalChildren = base.InternalChildren;
			Hashtable hashtable = null;
			int num = cellsHead;
			do
			{
				double width = internalChildren[num].DesiredSize.Width;
				this.MeasureCell(num, forceInfinityV);
				hasDesiredSizeUChanged |= !DoubleUtil.AreClose(width, internalChildren[num].DesiredSize.Width);
				if (!ignoreDesiredSizeU)
				{
					if (this.PrivateCells[num].ColumnSpan == 1)
					{
						this.DefinitionsU[this.PrivateCells[num].ColumnIndex].UpdateMinSize(Math.Min(internalChildren[num].DesiredSize.Width, this.DefinitionsU[this.PrivateCells[num].ColumnIndex].UserMaxSize));
					}
					else
					{
						Grid.RegisterSpan(ref hashtable, this.PrivateCells[num].ColumnIndex, this.PrivateCells[num].ColumnSpan, true, internalChildren[num].DesiredSize.Width);
					}
				}
				if (!forceInfinityV)
				{
					if (this.PrivateCells[num].RowSpan == 1)
					{
						this.DefinitionsV[this.PrivateCells[num].RowIndex].UpdateMinSize(Math.Min(internalChildren[num].DesiredSize.Height, this.DefinitionsV[this.PrivateCells[num].RowIndex].UserMaxSize));
					}
					else
					{
						Grid.RegisterSpan(ref hashtable, this.PrivateCells[num].RowIndex, this.PrivateCells[num].RowSpan, false, internalChildren[num].DesiredSize.Height);
					}
				}
				num = this.PrivateCells[num].Next;
			}
			while (num < this.PrivateCells.Length);
			if (hashtable != null)
			{
				foreach (object obj in hashtable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					Grid.SpanKey spanKey = (Grid.SpanKey)dictionaryEntry.Key;
					double requestedSize = (double)dictionaryEntry.Value;
					this.EnsureMinSizeInDefinitionRange(spanKey.U ? this.DefinitionsU : this.DefinitionsV, spanKey.Start, spanKey.Count, requestedSize, spanKey.U ? referenceSize.Width : referenceSize.Height);
				}
			}
		}

		// Token: 0x060069D3 RID: 27091 RVA: 0x002BE904 File Offset: 0x002BD904
		private static void RegisterSpan(ref Hashtable store, int start, int count, bool u, double value)
		{
			if (store == null)
			{
				store = new Hashtable();
			}
			Grid.SpanKey key = new Grid.SpanKey(start, count, u);
			object obj = store[key];
			if (obj == null || value > (double)obj)
			{
				store[key] = value;
			}
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x002BE94C File Offset: 0x002BD94C
		private void MeasureCell(int cell, bool forceInfinityV)
		{
			double width;
			if (this.PrivateCells[cell].IsAutoU && !this.PrivateCells[cell].IsStarU)
			{
				width = double.PositiveInfinity;
			}
			else
			{
				width = this.GetMeasureSizeForRange(this.DefinitionsU, this.PrivateCells[cell].ColumnIndex, this.PrivateCells[cell].ColumnSpan);
			}
			double height;
			if (forceInfinityV)
			{
				height = double.PositiveInfinity;
			}
			else if (this.PrivateCells[cell].IsAutoV && !this.PrivateCells[cell].IsStarV)
			{
				height = double.PositiveInfinity;
			}
			else
			{
				height = this.GetMeasureSizeForRange(this.DefinitionsV, this.PrivateCells[cell].RowIndex, this.PrivateCells[cell].RowSpan);
			}
			UIElement uielement = base.InternalChildren[cell];
			if (uielement != null)
			{
				Size availableSize = new Size(width, height);
				uielement.Measure(availableSize);
			}
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x002BEA4C File Offset: 0x002BDA4C
		private double GetMeasureSizeForRange(DefinitionBase[] definitions, int start, int count)
		{
			double num = 0.0;
			int num2 = start + count - 1;
			do
			{
				num += ((definitions[num2].SizeType == Grid.LayoutTimeSizeType.Auto) ? definitions[num2].MinSize : definitions[num2].MeasureSize);
			}
			while (--num2 >= start);
			return num;
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x002BEA94 File Offset: 0x002BDA94
		private Grid.LayoutTimeSizeType GetLengthTypeForRange(DefinitionBase[] definitions, int start, int count)
		{
			Grid.LayoutTimeSizeType layoutTimeSizeType = Grid.LayoutTimeSizeType.None;
			int num = start + count - 1;
			do
			{
				layoutTimeSizeType |= definitions[num].SizeType;
			}
			while (--num >= start);
			return layoutTimeSizeType;
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x002BEAC0 File Offset: 0x002BDAC0
		private void EnsureMinSizeInDefinitionRange(DefinitionBase[] definitions, int start, int count, double requestedSize, double percentReferenceSize)
		{
			if (!Grid._IsZero(requestedSize))
			{
				DefinitionBase[] tempDefinitions = this.TempDefinitions;
				int num = start + count;
				int num2 = 0;
				double num3 = 0.0;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				for (int i = start; i < num; i++)
				{
					double minSize = definitions[i].MinSize;
					double preferredSize = definitions[i].PreferredSize;
					double num7 = Math.Max(definitions[i].UserMaxSize, minSize);
					num3 += minSize;
					num4 += preferredSize;
					num5 += num7;
					definitions[i].SizeCache = num7;
					if (num6 < num7)
					{
						num6 = num7;
					}
					if (definitions[i].UserSize.IsAuto)
					{
						num2++;
					}
					tempDefinitions[i - start] = definitions[i];
				}
				if (requestedSize > num3)
				{
					if (requestedSize <= num4)
					{
						Array.Sort(tempDefinitions, 0, count, Grid.s_spanPreferredDistributionOrderComparer);
						int j = 0;
						double num8 = requestedSize;
						while (j < num2)
						{
							num8 -= tempDefinitions[j].MinSize;
							j++;
						}
						while (j < count)
						{
							double num9 = Math.Min(num8 / (double)(count - j), tempDefinitions[j].PreferredSize);
							if (num9 > tempDefinitions[j].MinSize)
							{
								tempDefinitions[j].UpdateMinSize(num9);
							}
							num8 -= num9;
							j++;
						}
						return;
					}
					if (requestedSize <= num5)
					{
						Array.Sort(tempDefinitions, 0, count, Grid.s_spanMaxDistributionOrderComparer);
						int k = 0;
						double num10 = requestedSize - num4;
						while (k < count - num2)
						{
							double preferredSize2 = tempDefinitions[k].PreferredSize;
							double val = preferredSize2 + num10 / (double)(count - num2 - k);
							tempDefinitions[k].UpdateMinSize(Math.Min(val, tempDefinitions[k].SizeCache));
							num10 -= tempDefinitions[k].MinSize - preferredSize2;
							k++;
						}
						while (k < count)
						{
							double minSize2 = tempDefinitions[k].MinSize;
							double val2 = minSize2 + num10 / (double)(count - k);
							tempDefinitions[k].UpdateMinSize(Math.Min(val2, tempDefinitions[k].SizeCache));
							num10 -= tempDefinitions[k].MinSize - minSize2;
							k++;
						}
						return;
					}
					double num11 = requestedSize / (double)count;
					if (num11 < num6 && !Grid._AreClose(num11, num6))
					{
						double num12 = num6 * (double)count - num5;
						double num13 = requestedSize - num5;
						for (int l = 0; l < count; l++)
						{
							double num14 = (num6 - tempDefinitions[l].SizeCache) * num13 / num12;
							tempDefinitions[l].UpdateMinSize(tempDefinitions[l].SizeCache + num14);
						}
						return;
					}
					for (int m = 0; m < count; m++)
					{
						tempDefinitions[m].UpdateMinSize(num11);
					}
				}
			}
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x002BED56 File Offset: 0x002BDD56
		private void ResolveStar(DefinitionBase[] definitions, double availableSize)
		{
			if (FrameworkAppContextSwitches.GridStarDefinitionsCanExceedAvailableSpace)
			{
				this.ResolveStarLegacy(definitions, availableSize);
				return;
			}
			this.ResolveStarMaxDiscrepancy(definitions, availableSize);
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x002BED70 File Offset: 0x002BDD70
		private void ResolveStarLegacy(DefinitionBase[] definitions, double availableSize)
		{
			DefinitionBase[] tempDefinitions = this.TempDefinitions;
			int num = 0;
			double num2 = 0.0;
			for (int i = 0; i < definitions.Length; i++)
			{
				switch (definitions[i].SizeType)
				{
				case Grid.LayoutTimeSizeType.Pixel:
					num2 += definitions[i].MeasureSize;
					break;
				case Grid.LayoutTimeSizeType.Auto:
					num2 += definitions[i].MinSize;
					break;
				case Grid.LayoutTimeSizeType.Star:
				{
					tempDefinitions[num++] = definitions[i];
					double num3 = definitions[i].UserSize.Value;
					if (Grid._IsZero(num3))
					{
						definitions[i].MeasureSize = 0.0;
						definitions[i].SizeCache = 0.0;
					}
					else
					{
						num3 = Math.Min(num3, 1E+298);
						definitions[i].MeasureSize = num3;
						double num4 = Math.Max(definitions[i].MinSize, definitions[i].UserMaxSize);
						num4 = Math.Min(num4, 1E+298);
						definitions[i].SizeCache = num4 / num3;
					}
					break;
				}
				}
			}
			if (num > 0)
			{
				Array.Sort(tempDefinitions, 0, num, Grid.s_starDistributionOrderComparer);
				double num5 = 0.0;
				int num6 = num - 1;
				do
				{
					num5 += tempDefinitions[num6].MeasureSize;
					tempDefinitions[num6].SizeCache = num5;
				}
				while (--num6 >= 0);
				num6 = 0;
				do
				{
					double measureSize = tempDefinitions[num6].MeasureSize;
					double num7;
					if (Grid._IsZero(measureSize))
					{
						num7 = tempDefinitions[num6].MinSize;
					}
					else
					{
						num7 = Math.Min(Math.Max(availableSize - num2, 0.0) * (measureSize / tempDefinitions[num6].SizeCache), tempDefinitions[num6].UserMaxSize);
						num7 = Math.Max(tempDefinitions[num6].MinSize, num7);
					}
					tempDefinitions[num6].MeasureSize = num7;
					num2 += num7;
				}
				while (++num6 < num);
			}
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x002BEF50 File Offset: 0x002BDF50
		private void ResolveStarMaxDiscrepancy(DefinitionBase[] definitions, double availableSize)
		{
			int num = definitions.Length;
			DefinitionBase[] tempDefinitions = this.TempDefinitions;
			double num2 = 0.0;
			int num3 = 0;
			double scale = 1.0;
			double num4 = 0.0;
			for (int i = 0; i < num; i++)
			{
				DefinitionBase definitionBase = definitions[i];
				if (definitionBase.SizeType == Grid.LayoutTimeSizeType.Star)
				{
					num3++;
					definitionBase.MeasureSize = 1.0;
					if (definitionBase.UserSize.Value > num4)
					{
						num4 = definitionBase.UserSize.Value;
					}
				}
			}
			if (double.IsPositiveInfinity(num4))
			{
				scale = -1.0;
			}
			else if (num3 > 0)
			{
				double num5 = Math.Floor(Math.Log(double.MaxValue / num4 / (double)num3, 2.0));
				if (num5 < 0.0)
				{
					scale = Math.Pow(2.0, num5 - 4.0);
				}
			}
			bool flag = true;
			while (flag)
			{
				double num6 = 0.0;
				num2 = 0.0;
				int j;
				int num7 = j = 0;
				for (int k = 0; k < num; k++)
				{
					DefinitionBase definitionBase2 = definitions[k];
					switch (definitionBase2.SizeType)
					{
					case Grid.LayoutTimeSizeType.Pixel:
						num2 += definitionBase2.MeasureSize;
						break;
					case Grid.LayoutTimeSizeType.Auto:
						num2 += definitions[k].MinSize;
						break;
					case Grid.LayoutTimeSizeType.Star:
						if (definitionBase2.MeasureSize < 0.0)
						{
							num2 += -definitionBase2.MeasureSize;
						}
						else
						{
							double num8 = Grid.StarWeight(definitionBase2, scale);
							num6 += num8;
							if (definitionBase2.MinSize > 0.0)
							{
								tempDefinitions[j++] = definitionBase2;
								definitionBase2.MeasureSize = num8 / definitionBase2.MinSize;
							}
							double num9 = Math.Max(definitionBase2.MinSize, definitionBase2.UserMaxSize);
							if (!double.IsPositiveInfinity(num9))
							{
								tempDefinitions[num + num7++] = definitionBase2;
								definitionBase2.SizeCache = num8 / num9;
							}
						}
						break;
					}
				}
				int num10 = j;
				int num11 = num7;
				double num12 = 0.0;
				double num13 = availableSize - num2;
				double num14 = num6 - num12;
				Array.Sort(tempDefinitions, 0, j, Grid.s_minRatioComparer);
				Array.Sort(tempDefinitions, num, num7, Grid.s_maxRatioComparer);
				while (j + num7 > 0 && num13 > 0.0)
				{
					if (num14 < num6 * 0.00390625)
					{
						num12 = 0.0;
						num6 = 0.0;
						for (int l = 0; l < num; l++)
						{
							DefinitionBase definitionBase3 = definitions[l];
							if (definitionBase3.SizeType == Grid.LayoutTimeSizeType.Star && definitionBase3.MeasureSize > 0.0)
							{
								num6 += Grid.StarWeight(definitionBase3, scale);
							}
						}
						num14 = num6 - num12;
					}
					double minRatio = (j > 0) ? tempDefinitions[j - 1].MeasureSize : double.PositiveInfinity;
					double maxRatio = (num7 > 0) ? tempDefinitions[num + num7 - 1].SizeCache : -1.0;
					double proportion = num14 / num13;
					bool? flag2 = Grid.Choose(minRatio, maxRatio, proportion);
					if (flag2 == null)
					{
						break;
					}
					bool? flag3 = flag2;
					bool flag4 = true;
					DefinitionBase definitionBase4;
					double num15;
					if (flag3.GetValueOrDefault() == flag4 & flag3 != null)
					{
						definitionBase4 = tempDefinitions[j - 1];
						num15 = definitionBase4.MinSize;
						j--;
					}
					else
					{
						definitionBase4 = tempDefinitions[num + num7 - 1];
						num15 = Math.Max(definitionBase4.MinSize, definitionBase4.UserMaxSize);
						num7--;
					}
					num2 += num15;
					definitionBase4.MeasureSize = -num15;
					num12 += Grid.StarWeight(definitionBase4, scale);
					num3--;
					num13 = availableSize - num2;
					num14 = num6 - num12;
					while (j > 0)
					{
						if (tempDefinitions[j - 1].MeasureSize >= 0.0)
						{
							break;
						}
						j--;
						tempDefinitions[j] = null;
					}
					while (num7 > 0 && tempDefinitions[num + num7 - 1].MeasureSize < 0.0)
					{
						num7--;
						tempDefinitions[num + num7] = null;
					}
				}
				flag = false;
				if (num3 == 0 && num2 < availableSize)
				{
					for (int m = j; m < num10; m++)
					{
						DefinitionBase definitionBase5 = tempDefinitions[m];
						if (definitionBase5 != null)
						{
							definitionBase5.MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
				if (num2 > availableSize)
				{
					for (int n = num7; n < num11; n++)
					{
						DefinitionBase definitionBase6 = tempDefinitions[num + n];
						if (definitionBase6 != null)
						{
							definitionBase6.MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
			}
			num3 = 0;
			for (int num16 = 0; num16 < num; num16++)
			{
				DefinitionBase definitionBase7 = definitions[num16];
				if (definitionBase7.SizeType == Grid.LayoutTimeSizeType.Star)
				{
					if (definitionBase7.MeasureSize < 0.0)
					{
						definitionBase7.MeasureSize = -definitionBase7.MeasureSize;
					}
					else
					{
						tempDefinitions[num3++] = definitionBase7;
						definitionBase7.MeasureSize = Grid.StarWeight(definitionBase7, scale);
					}
				}
			}
			if (num3 > 0)
			{
				Array.Sort(tempDefinitions, 0, num3, Grid.s_starWeightComparer);
				double num6 = 0.0;
				for (int num17 = 0; num17 < num3; num17++)
				{
					DefinitionBase definitionBase8 = tempDefinitions[num17];
					num6 += definitionBase8.MeasureSize;
					definitionBase8.SizeCache = num6;
				}
				for (int num18 = num3 - 1; num18 >= 0; num18--)
				{
					DefinitionBase definitionBase9 = tempDefinitions[num18];
					double num19 = (definitionBase9.MeasureSize > 0.0) ? (Math.Max(availableSize - num2, 0.0) * (definitionBase9.MeasureSize / definitionBase9.SizeCache)) : 0.0;
					num19 = Math.Min(num19, definitionBase9.UserMaxSize);
					num19 = Math.Max(definitionBase9.MinSize, num19);
					definitionBase9.MeasureSize = num19;
					num2 += num19;
				}
			}
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x002BF520 File Offset: 0x002BE520
		private double CalculateDesiredSize(DefinitionBase[] definitions)
		{
			double num = 0.0;
			for (int i = 0; i < definitions.Length; i++)
			{
				num += definitions[i].MinSize;
			}
			return num;
		}

		// Token: 0x060069DC RID: 27100 RVA: 0x002BF551 File Offset: 0x002BE551
		private void SetFinalSize(DefinitionBase[] definitions, double finalSize, bool columns)
		{
			if (FrameworkAppContextSwitches.GridStarDefinitionsCanExceedAvailableSpace)
			{
				this.SetFinalSizeLegacy(definitions, finalSize, columns);
				return;
			}
			this.SetFinalSizeMaxDiscrepancy(definitions, finalSize, columns);
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x002BF570 File Offset: 0x002BE570
		private void SetFinalSizeLegacy(DefinitionBase[] definitions, double finalSize, bool columns)
		{
			int num = 0;
			int num2 = definitions.Length;
			double num3 = 0.0;
			bool useLayoutRounding = base.UseLayoutRounding;
			int[] definitionIndices = this.DefinitionIndices;
			double[] array = null;
			double dpiScale = 1.0;
			if (useLayoutRounding)
			{
				DpiScale dpi = base.GetDpi();
				dpiScale = (columns ? dpi.DpiScaleX : dpi.DpiScaleY);
				array = this.RoundingErrors;
			}
			for (int i = 0; i < definitions.Length; i++)
			{
				if (definitions[i].UserSize.IsStar)
				{
					double num4 = definitions[i].UserSize.Value;
					if (Grid._IsZero(num4))
					{
						definitions[i].MeasureSize = 0.0;
						definitions[i].SizeCache = 0.0;
					}
					else
					{
						num4 = Math.Min(num4, 1E+298);
						definitions[i].MeasureSize = num4;
						double num5 = Math.Max(definitions[i].MinSizeForArrange, definitions[i].UserMaxSize);
						num5 = Math.Min(num5, 1E+298);
						definitions[i].SizeCache = num5 / num4;
						if (useLayoutRounding)
						{
							array[i] = definitions[i].SizeCache;
							definitions[i].SizeCache = UIElement.RoundLayoutValue(definitions[i].SizeCache, dpiScale);
						}
					}
					definitionIndices[num++] = i;
				}
				else
				{
					double num6 = 0.0;
					GridUnitType gridUnitType = definitions[i].UserSize.GridUnitType;
					if (gridUnitType != GridUnitType.Auto)
					{
						if (gridUnitType == GridUnitType.Pixel)
						{
							num6 = definitions[i].UserSize.Value;
						}
					}
					else
					{
						num6 = definitions[i].MinSizeForArrange;
					}
					double val;
					if (definitions[i].IsShared)
					{
						val = num6;
					}
					else
					{
						val = definitions[i].UserMaxSize;
					}
					definitions[i].SizeCache = Math.Max(definitions[i].MinSizeForArrange, Math.Min(num6, val));
					if (useLayoutRounding)
					{
						array[i] = definitions[i].SizeCache;
						definitions[i].SizeCache = UIElement.RoundLayoutValue(definitions[i].SizeCache, dpiScale);
					}
					num3 += definitions[i].SizeCache;
					definitionIndices[--num2] = i;
				}
			}
			if (num > 0)
			{
				Grid.StarDistributionOrderIndexComparer comparer = new Grid.StarDistributionOrderIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, num, comparer);
				double num7 = 0.0;
				int num8 = num - 1;
				do
				{
					num7 += definitions[definitionIndices[num8]].MeasureSize;
					definitions[definitionIndices[num8]].SizeCache = num7;
				}
				while (--num8 >= 0);
				num8 = 0;
				do
				{
					double measureSize = definitions[definitionIndices[num8]].MeasureSize;
					double num9;
					if (Grid._IsZero(measureSize))
					{
						num9 = definitions[definitionIndices[num8]].MinSizeForArrange;
					}
					else
					{
						num9 = Math.Min(Math.Max(finalSize - num3, 0.0) * (measureSize / definitions[definitionIndices[num8]].SizeCache), definitions[definitionIndices[num8]].UserMaxSize);
						num9 = Math.Max(definitions[definitionIndices[num8]].MinSizeForArrange, num9);
					}
					definitions[definitionIndices[num8]].SizeCache = num9;
					if (useLayoutRounding)
					{
						array[definitionIndices[num8]] = definitions[definitionIndices[num8]].SizeCache;
						definitions[definitionIndices[num8]].SizeCache = UIElement.RoundLayoutValue(definitions[definitionIndices[num8]].SizeCache, dpiScale);
					}
					num3 += definitions[definitionIndices[num8]].SizeCache;
				}
				while (++num8 < num);
			}
			if (num3 > finalSize && !Grid._AreClose(num3, finalSize))
			{
				Grid.DistributionOrderIndexComparer comparer2 = new Grid.DistributionOrderIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, definitions.Length, comparer2);
				double num10 = finalSize - num3;
				for (int j = 0; j < definitions.Length; j++)
				{
					int num11 = definitionIndices[j];
					double num12 = definitions[num11].SizeCache + num10 / (double)(definitions.Length - j);
					double value = num12;
					num12 = Math.Max(num12, definitions[num11].MinSizeForArrange);
					num12 = Math.Min(num12, definitions[num11].SizeCache);
					if (useLayoutRounding)
					{
						array[num11] = num12;
						num12 = UIElement.RoundLayoutValue(value, dpiScale);
						num12 = Math.Max(num12, definitions[num11].MinSizeForArrange);
						num12 = Math.Min(num12, definitions[num11].SizeCache);
					}
					num10 -= num12 - definitions[num11].SizeCache;
					definitions[num11].SizeCache = num12;
				}
				num3 = finalSize - num10;
			}
			if (useLayoutRounding && !Grid._AreClose(num3, finalSize))
			{
				for (int k = 0; k < definitions.Length; k++)
				{
					array[k] -= definitions[k].SizeCache;
					definitionIndices[k] = k;
				}
				Grid.RoundingErrorIndexComparer comparer3 = new Grid.RoundingErrorIndexComparer(array);
				Array.Sort(definitionIndices, 0, definitions.Length, comparer3);
				double num13 = num3;
				double num14 = UIElement.RoundLayoutValue(1.0, dpiScale);
				if (num3 > finalSize)
				{
					int num15 = definitions.Length - 1;
					while (num13 > finalSize && !Grid._AreClose(num13, finalSize))
					{
						if (num15 < 0)
						{
							break;
						}
						DefinitionBase definitionBase = definitions[definitionIndices[num15]];
						double num16 = definitionBase.SizeCache - num14;
						num16 = Math.Max(num16, definitionBase.MinSizeForArrange);
						if (num16 < definitionBase.SizeCache)
						{
							num13 -= num14;
						}
						definitionBase.SizeCache = num16;
						num15--;
					}
				}
				else if (num3 < finalSize)
				{
					int num17 = 0;
					while (num13 < finalSize && !Grid._AreClose(num13, finalSize) && num17 < definitions.Length)
					{
						DefinitionBase definitionBase2 = definitions[definitionIndices[num17]];
						double num18 = definitionBase2.SizeCache + num14;
						num18 = Math.Max(num18, definitionBase2.MinSizeForArrange);
						if (num18 > definitionBase2.SizeCache)
						{
							num13 += num14;
						}
						definitionBase2.SizeCache = num18;
						num17++;
					}
				}
			}
			definitions[0].FinalOffset = 0.0;
			for (int l = 0; l < definitions.Length; l++)
			{
				definitions[(l + 1) % definitions.Length].FinalOffset = definitions[l].FinalOffset + definitions[l].SizeCache;
			}
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x002BFB24 File Offset: 0x002BEB24
		private void SetFinalSizeMaxDiscrepancy(DefinitionBase[] definitions, double finalSize, bool columns)
		{
			int num = definitions.Length;
			int[] definitionIndices = this.DefinitionIndices;
			double num2 = 0.0;
			int num3 = 0;
			double scale = 1.0;
			double num4 = 0.0;
			for (int i = 0; i < num; i++)
			{
				DefinitionBase definitionBase = definitions[i];
				if (definitionBase.UserSize.IsStar)
				{
					num3++;
					definitionBase.MeasureSize = 1.0;
					if (definitionBase.UserSize.Value > num4)
					{
						num4 = definitionBase.UserSize.Value;
					}
				}
			}
			if (double.IsPositiveInfinity(num4))
			{
				scale = -1.0;
			}
			else if (num3 > 0)
			{
				double num5 = Math.Floor(Math.Log(double.MaxValue / num4 / (double)num3, 2.0));
				if (num5 < 0.0)
				{
					scale = Math.Pow(2.0, num5 - 4.0);
				}
			}
			bool flag = true;
			while (flag)
			{
				double num6 = 0.0;
				num2 = 0.0;
				int j;
				int num7 = j = 0;
				for (int k = 0; k < num; k++)
				{
					DefinitionBase definitionBase2 = definitions[k];
					if (definitionBase2.UserSize.IsStar)
					{
						if (definitionBase2.MeasureSize < 0.0)
						{
							num2 += -definitionBase2.MeasureSize;
						}
						else
						{
							double num8 = Grid.StarWeight(definitionBase2, scale);
							num6 += num8;
							if (definitionBase2.MinSizeForArrange > 0.0)
							{
								definitionIndices[j++] = k;
								definitionBase2.MeasureSize = num8 / definitionBase2.MinSizeForArrange;
							}
							double num9 = Math.Max(definitionBase2.MinSizeForArrange, definitionBase2.UserMaxSize);
							if (!double.IsPositiveInfinity(num9))
							{
								definitionIndices[num + num7++] = k;
								definitionBase2.SizeCache = num8 / num9;
							}
						}
					}
					else
					{
						double num10 = 0.0;
						GridUnitType gridUnitType = definitionBase2.UserSize.GridUnitType;
						if (gridUnitType != GridUnitType.Auto)
						{
							if (gridUnitType == GridUnitType.Pixel)
							{
								num10 = definitionBase2.UserSize.Value;
							}
						}
						else
						{
							num10 = definitionBase2.MinSizeForArrange;
						}
						double val;
						if (definitionBase2.IsShared)
						{
							val = num10;
						}
						else
						{
							val = definitionBase2.UserMaxSize;
						}
						definitionBase2.SizeCache = Math.Max(definitionBase2.MinSizeForArrange, Math.Min(num10, val));
						num2 += definitionBase2.SizeCache;
					}
				}
				int num11 = j;
				int num12 = num7;
				double num13 = 0.0;
				double num14 = finalSize - num2;
				double num15 = num6 - num13;
				Grid.MinRatioIndexComparer comparer = new Grid.MinRatioIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, j, comparer);
				Grid.MaxRatioIndexComparer comparer2 = new Grid.MaxRatioIndexComparer(definitions);
				Array.Sort(definitionIndices, num, num7, comparer2);
				while (j + num7 > 0 && num14 > 0.0)
				{
					if (num15 < num6 * 0.00390625)
					{
						num13 = 0.0;
						num6 = 0.0;
						for (int l = 0; l < num; l++)
						{
							DefinitionBase definitionBase3 = definitions[l];
							if (definitionBase3.UserSize.IsStar && definitionBase3.MeasureSize > 0.0)
							{
								num6 += Grid.StarWeight(definitionBase3, scale);
							}
						}
						num15 = num6 - num13;
					}
					double minRatio = (j > 0) ? definitions[definitionIndices[j - 1]].MeasureSize : double.PositiveInfinity;
					double maxRatio = (num7 > 0) ? definitions[definitionIndices[num + num7 - 1]].SizeCache : -1.0;
					double proportion = num15 / num14;
					bool? flag2 = Grid.Choose(minRatio, maxRatio, proportion);
					if (flag2 == null)
					{
						break;
					}
					bool? flag3 = flag2;
					bool flag4 = true;
					DefinitionBase definitionBase4;
					double num17;
					if (flag3.GetValueOrDefault() == flag4 & flag3 != null)
					{
						int num16 = definitionIndices[j - 1];
						definitionBase4 = definitions[num16];
						num17 = definitionBase4.MinSizeForArrange;
						j--;
					}
					else
					{
						int num16 = definitionIndices[num + num7 - 1];
						definitionBase4 = definitions[num16];
						num17 = Math.Max(definitionBase4.MinSizeForArrange, definitionBase4.UserMaxSize);
						num7--;
					}
					num2 += num17;
					definitionBase4.MeasureSize = -num17;
					num13 += Grid.StarWeight(definitionBase4, scale);
					num3--;
					num14 = finalSize - num2;
					num15 = num6 - num13;
					while (j > 0)
					{
						if (definitions[definitionIndices[j - 1]].MeasureSize >= 0.0)
						{
							break;
						}
						j--;
						definitionIndices[j] = -1;
					}
					while (num7 > 0 && definitions[definitionIndices[num + num7 - 1]].MeasureSize < 0.0)
					{
						num7--;
						definitionIndices[num + num7] = -1;
					}
				}
				flag = false;
				if (num3 == 0 && num2 < finalSize)
				{
					for (int m = j; m < num11; m++)
					{
						if (definitionIndices[m] >= 0)
						{
							definitions[definitionIndices[m]].MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
				if (num2 > finalSize)
				{
					for (int n = num7; n < num12; n++)
					{
						if (definitionIndices[num + n] >= 0)
						{
							definitions[definitionIndices[num + n]].MeasureSize = 1.0;
							num3++;
							flag = true;
						}
					}
				}
			}
			num3 = 0;
			for (int num18 = 0; num18 < num; num18++)
			{
				DefinitionBase definitionBase5 = definitions[num18];
				if (definitionBase5.UserSize.IsStar)
				{
					if (definitionBase5.MeasureSize < 0.0)
					{
						definitionBase5.SizeCache = -definitionBase5.MeasureSize;
					}
					else
					{
						definitionIndices[num3++] = num18;
						definitionBase5.MeasureSize = Grid.StarWeight(definitionBase5, scale);
					}
				}
			}
			if (num3 > 0)
			{
				Grid.StarWeightIndexComparer comparer3 = new Grid.StarWeightIndexComparer(definitions);
				Array.Sort(definitionIndices, 0, num3, comparer3);
				double num6 = 0.0;
				for (int num19 = 0; num19 < num3; num19++)
				{
					DefinitionBase definitionBase6 = definitions[definitionIndices[num19]];
					num6 += definitionBase6.MeasureSize;
					definitionBase6.SizeCache = num6;
				}
				for (int num20 = num3 - 1; num20 >= 0; num20--)
				{
					DefinitionBase definitionBase7 = definitions[definitionIndices[num20]];
					double num21 = (definitionBase7.MeasureSize > 0.0) ? (Math.Max(finalSize - num2, 0.0) * (definitionBase7.MeasureSize / definitionBase7.SizeCache)) : 0.0;
					num21 = Math.Min(num21, definitionBase7.UserMaxSize);
					num21 = Math.Max(definitionBase7.MinSizeForArrange, num21);
					num2 += num21;
					definitionBase7.SizeCache = num21;
				}
			}
			if (base.UseLayoutRounding)
			{
				DpiScale dpi = base.GetDpi();
				double num22 = columns ? dpi.DpiScaleX : dpi.DpiScaleY;
				double[] roundingErrors = this.RoundingErrors;
				double num23 = 0.0;
				for (int num24 = 0; num24 < definitions.Length; num24++)
				{
					DefinitionBase definitionBase8 = definitions[num24];
					double num25 = UIElement.RoundLayoutValue(definitionBase8.SizeCache, num22);
					roundingErrors[num24] = num25 - definitionBase8.SizeCache;
					definitionBase8.SizeCache = num25;
					num23 += num25;
				}
				if (!Grid._AreClose(num23, finalSize))
				{
					for (int num26 = 0; num26 < definitions.Length; num26++)
					{
						definitionIndices[num26] = num26;
					}
					Grid.RoundingErrorIndexComparer comparer4 = new Grid.RoundingErrorIndexComparer(roundingErrors);
					Array.Sort(definitionIndices, 0, definitions.Length, comparer4);
					double num27 = num23;
					double num28 = 1.0 / num22;
					if (num23 > finalSize)
					{
						int num29 = definitions.Length - 1;
						while (num27 > finalSize && !Grid._AreClose(num27, finalSize))
						{
							if (num29 < 0)
							{
								break;
							}
							DefinitionBase definitionBase9 = definitions[definitionIndices[num29]];
							double num30 = definitionBase9.SizeCache - num28;
							num30 = Math.Max(num30, definitionBase9.MinSizeForArrange);
							if (num30 < definitionBase9.SizeCache)
							{
								num27 -= num28;
							}
							definitionBase9.SizeCache = num30;
							num29--;
						}
					}
					else if (num23 < finalSize)
					{
						int num31 = 0;
						while (num27 < finalSize && !Grid._AreClose(num27, finalSize) && num31 < definitions.Length)
						{
							DefinitionBase definitionBase10 = definitions[definitionIndices[num31]];
							double num32 = definitionBase10.SizeCache + num28;
							num32 = Math.Max(num32, definitionBase10.MinSizeForArrange);
							if (num32 > definitionBase10.SizeCache)
							{
								num27 += num28;
							}
							definitionBase10.SizeCache = num32;
							num31++;
						}
					}
				}
			}
			definitions[0].FinalOffset = 0.0;
			for (int num33 = 0; num33 < definitions.Length; num33++)
			{
				definitions[(num33 + 1) % definitions.Length].FinalOffset = definitions[num33].FinalOffset + definitions[num33].SizeCache;
			}
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x002C0370 File Offset: 0x002BF370
		private static bool? Choose(double minRatio, double maxRatio, double proportion)
		{
			if (minRatio < proportion)
			{
				if (maxRatio <= proportion)
				{
					return new bool?(true);
				}
				double num = Math.Floor(Math.Log(minRatio, 2.0));
				double num2 = Math.Floor(Math.Log(maxRatio, 2.0));
				double num3 = Math.Pow(2.0, Math.Floor((num + num2) / 2.0));
				if (proportion / num3 * (proportion / num3) > minRatio / num3 * (maxRatio / num3))
				{
					return new bool?(true);
				}
				return new bool?(false);
			}
			else
			{
				if (maxRatio > proportion)
				{
					return new bool?(false);
				}
				return null;
			}
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x002C0409 File Offset: 0x002BF409
		private static int CompareRoundingErrors(KeyValuePair<int, double> x, KeyValuePair<int, double> y)
		{
			if (x.Value < y.Value)
			{
				return -1;
			}
			if (x.Value > y.Value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060069E1 RID: 27105 RVA: 0x002C0430 File Offset: 0x002BF430
		private double GetFinalSizeForRange(DefinitionBase[] definitions, int start, int count)
		{
			double num = 0.0;
			int num2 = start + count - 1;
			do
			{
				num += definitions[num2].SizeCache;
			}
			while (--num2 >= start);
			return num;
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x002C0464 File Offset: 0x002BF464
		private void SetValid()
		{
			Grid.ExtendedData extData = this.ExtData;
			if (extData != null && extData.TempDefinitions != null)
			{
				Array.Clear(extData.TempDefinitions, 0, Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length));
				extData.TempDefinitions = null;
			}
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x002C04AC File Offset: 0x002BF4AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColumnDefinitions()
		{
			Grid.ExtendedData extData = this.ExtData;
			return extData != null && extData.ColumnDefinitions != null && extData.ColumnDefinitions.Count > 0;
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x002C04DC File Offset: 0x002BF4DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeRowDefinitions()
		{
			Grid.ExtendedData extData = this.ExtData;
			return extData != null && extData.RowDefinitions != null && extData.RowDefinitions.Count > 0;
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x002C050C File Offset: 0x002BF50C
		private Grid.GridLinesRenderer EnsureGridLinesRenderer()
		{
			if (this.ShowGridLines && this._gridLinesRenderer == null)
			{
				this._gridLinesRenderer = new Grid.GridLinesRenderer();
				base.AddVisualChild(this._gridLinesRenderer);
			}
			if (!this.ShowGridLines && this._gridLinesRenderer != null)
			{
				base.RemoveVisualChild(this._gridLinesRenderer);
				this._gridLinesRenderer = null;
			}
			return this._gridLinesRenderer;
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x002C0569 File Offset: 0x002BF569
		private void SetFlags(bool value, Grid.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x002C0587 File Offset: 0x002BF587
		private bool CheckFlagsAnd(Grid.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x002C0594 File Offset: 0x002BF594
		private bool CheckFlagsOr(Grid.Flags flags)
		{
			return flags == (Grid.Flags)0 || (this._flags & flags) > (Grid.Flags)0;
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x002C05A8 File Offset: 0x002BF5A8
		private static void OnShowGridLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Grid grid = (Grid)d;
			if (grid.ExtData != null && grid.ListenToNotifications)
			{
				grid.InvalidateVisual();
			}
			grid.SetFlags((bool)e.NewValue, Grid.Flags.ShowGridLinesPropertyValue);
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x002C05EC File Offset: 0x002BF5EC
		private static void OnCellAttachedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Visual visual = d as Visual;
			if (visual != null)
			{
				Grid grid = VisualTreeHelper.GetParent(visual) as Grid;
				if (grid != null && grid.ExtData != null && grid.ListenToNotifications)
				{
					grid.CellsStructureDirty = true;
					grid.InvalidateMeasure();
				}
			}
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool IsIntValueNotNegative(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x0024392A File Offset: 0x0024292A
		private static bool IsIntValueGreaterThanZero(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x002C062F File Offset: 0x002BF62F
		private static bool CompareNullRefs(object x, object y, out int result)
		{
			result = 2;
			if (x == null)
			{
				if (y == null)
				{
					result = 0;
				}
				else
				{
					result = -1;
				}
			}
			else if (y == null)
			{
				result = 1;
			}
			return result != 2;
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x060069EE RID: 27118 RVA: 0x002C0652 File Offset: 0x002BF652
		private DefinitionBase[] DefinitionsU
		{
			get
			{
				return this.ExtData.DefinitionsU;
			}
		}

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x060069EF RID: 27119 RVA: 0x002C065F File Offset: 0x002BF65F
		private DefinitionBase[] DefinitionsV
		{
			get
			{
				return this.ExtData.DefinitionsV;
			}
		}

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x060069F0 RID: 27120 RVA: 0x002C066C File Offset: 0x002BF66C
		private DefinitionBase[] TempDefinitions
		{
			get
			{
				Grid.ExtendedData extData = this.ExtData;
				int num = Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length) * 2;
				if (extData.TempDefinitions == null || extData.TempDefinitions.Length < num)
				{
					WeakReference weakReference = (WeakReference)Thread.GetData(Grid.s_tempDefinitionsDataSlot);
					if (weakReference == null)
					{
						extData.TempDefinitions = new DefinitionBase[num];
						Thread.SetData(Grid.s_tempDefinitionsDataSlot, new WeakReference(extData.TempDefinitions));
					}
					else
					{
						extData.TempDefinitions = (DefinitionBase[])weakReference.Target;
						if (extData.TempDefinitions == null || extData.TempDefinitions.Length < num)
						{
							extData.TempDefinitions = new DefinitionBase[num];
							weakReference.Target = extData.TempDefinitions;
						}
					}
				}
				return extData.TempDefinitions;
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x060069F1 RID: 27121 RVA: 0x002C0724 File Offset: 0x002BF724
		private int[] DefinitionIndices
		{
			get
			{
				int num = Math.Max(Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length), 1) * 2;
				if (this._definitionIndices == null || this._definitionIndices.Length < num)
				{
					this._definitionIndices = new int[num];
				}
				return this._definitionIndices;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x060069F2 RID: 27122 RVA: 0x002C0774 File Offset: 0x002BF774
		private double[] RoundingErrors
		{
			get
			{
				int num = Math.Max(this.DefinitionsU.Length, this.DefinitionsV.Length);
				if (this._roundingErrors == null && num == 0)
				{
					this._roundingErrors = new double[1];
				}
				else if (this._roundingErrors == null || this._roundingErrors.Length < num)
				{
					this._roundingErrors = new double[num];
				}
				return this._roundingErrors;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x060069F3 RID: 27123 RVA: 0x002C07D5 File Offset: 0x002BF7D5
		private Grid.CellCache[] PrivateCells
		{
			get
			{
				return this.ExtData.CellCachesCollection;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x060069F4 RID: 27124 RVA: 0x002C07E2 File Offset: 0x002BF7E2
		// (set) Token: 0x060069F5 RID: 27125 RVA: 0x002C07EE File Offset: 0x002BF7EE
		private bool CellsStructureDirty
		{
			get
			{
				return !this.CheckFlagsAnd(Grid.Flags.ValidCellsStructure);
			}
			set
			{
				this.SetFlags(!value, Grid.Flags.ValidCellsStructure);
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x060069F6 RID: 27126 RVA: 0x002C07FB File Offset: 0x002BF7FB
		// (set) Token: 0x060069F7 RID: 27127 RVA: 0x002C0808 File Offset: 0x002BF808
		private bool ListenToNotifications
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.ListenToNotifications);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.ListenToNotifications);
			}
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x060069F8 RID: 27128 RVA: 0x002C0816 File Offset: 0x002BF816
		// (set) Token: 0x060069F9 RID: 27129 RVA: 0x002C0823 File Offset: 0x002BF823
		private bool SizeToContentU
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.SizeToContentU);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.SizeToContentU);
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x060069FA RID: 27130 RVA: 0x002C0831 File Offset: 0x002BF831
		// (set) Token: 0x060069FB RID: 27131 RVA: 0x002C083E File Offset: 0x002BF83E
		private bool SizeToContentV
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.SizeToContentV);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.SizeToContentV);
			}
		}

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x060069FC RID: 27132 RVA: 0x002C084C File Offset: 0x002BF84C
		// (set) Token: 0x060069FD RID: 27133 RVA: 0x002C0859 File Offset: 0x002BF859
		private bool HasStarCellsU
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.HasStarCellsU);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.HasStarCellsU);
			}
		}

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x060069FE RID: 27134 RVA: 0x002C0867 File Offset: 0x002BF867
		// (set) Token: 0x060069FF RID: 27135 RVA: 0x002C0874 File Offset: 0x002BF874
		private bool HasStarCellsV
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.HasStarCellsV);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.HasStarCellsV);
			}
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x06006A00 RID: 27136 RVA: 0x002C0882 File Offset: 0x002BF882
		// (set) Token: 0x06006A01 RID: 27137 RVA: 0x002C088F File Offset: 0x002BF88F
		private bool HasGroup3CellsInAutoRows
		{
			get
			{
				return this.CheckFlagsAnd(Grid.Flags.HasGroup3CellsInAutoRows);
			}
			set
			{
				this.SetFlags(value, Grid.Flags.HasGroup3CellsInAutoRows);
			}
		}

		// Token: 0x06006A02 RID: 27138 RVA: 0x002C089D File Offset: 0x002BF89D
		private static bool _IsZero(double d)
		{
			return Math.Abs(d) < 1E-05;
		}

		// Token: 0x06006A03 RID: 27139 RVA: 0x002C08B0 File Offset: 0x002BF8B0
		private static bool _AreClose(double d1, double d2)
		{
			return Math.Abs(d1 - d2) < 1E-05;
		}

		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x002C08C5 File Offset: 0x002BF8C5
		private Grid.ExtendedData ExtData
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x002C08D0 File Offset: 0x002BF8D0
		private static double StarWeight(DefinitionBase def, double scale)
		{
			if (scale >= 0.0)
			{
				return def.UserSize.Value * scale;
			}
			if (!double.IsPositiveInfinity(def.UserSize.Value))
			{
				return 0.0;
			}
			return 1.0;
		}

		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x06006A06 RID: 27142 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x06006A07 RID: 27143 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("GRIDPARANOIA")]
		internal void EnterCounterScope(Grid.Counters scopeCounter)
		{
		}

		// Token: 0x06006A08 RID: 27144 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("GRIDPARANOIA")]
		internal void ExitCounterScope(Grid.Counters scopeCounter)
		{
		}

		// Token: 0x06006A09 RID: 27145 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("GRIDPARANOIA")]
		internal void EnterCounter(Grid.Counters counter)
		{
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("GRIDPARANOIA")]
		internal void ExitCounter(Grid.Counters counter)
		{
		}

		// Token: 0x04003514 RID: 13588
		private Grid.ExtendedData _data;

		// Token: 0x04003515 RID: 13589
		private Grid.Flags _flags;

		// Token: 0x04003516 RID: 13590
		private Grid.GridLinesRenderer _gridLinesRenderer;

		// Token: 0x04003517 RID: 13591
		private int[] _definitionIndices;

		// Token: 0x04003518 RID: 13592
		private double[] _roundingErrors;

		// Token: 0x04003519 RID: 13593
		private const double c_epsilon = 1E-05;

		// Token: 0x0400351A RID: 13594
		private const double c_starClip = 1E+298;

		// Token: 0x0400351B RID: 13595
		private const int c_layoutLoopMaxCount = 5;

		// Token: 0x0400351C RID: 13596
		private static readonly LocalDataStoreSlot s_tempDefinitionsDataSlot = Thread.AllocateDataSlot();

		// Token: 0x0400351D RID: 13597
		private static readonly IComparer s_spanPreferredDistributionOrderComparer = new Grid.SpanPreferredDistributionOrderComparer();

		// Token: 0x0400351E RID: 13598
		private static readonly IComparer s_spanMaxDistributionOrderComparer = new Grid.SpanMaxDistributionOrderComparer();

		// Token: 0x0400351F RID: 13599
		private static readonly IComparer s_starDistributionOrderComparer = new Grid.StarDistributionOrderComparer();

		// Token: 0x04003520 RID: 13600
		private static readonly IComparer s_distributionOrderComparer = new Grid.DistributionOrderComparer();

		// Token: 0x04003521 RID: 13601
		private static readonly IComparer s_minRatioComparer = new Grid.MinRatioComparer();

		// Token: 0x04003522 RID: 13602
		private static readonly IComparer s_maxRatioComparer = new Grid.MaxRatioComparer();

		// Token: 0x04003523 RID: 13603
		private static readonly IComparer s_starWeightComparer = new Grid.StarWeightComparer();

		// Token: 0x04003524 RID: 13604
		public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(Grid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Grid.OnShowGridLinesPropertyChanged)));

		// Token: 0x04003525 RID: 13605
		[CommonDependencyProperty]
		public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueNotNegative));

		// Token: 0x04003526 RID: 13606
		[CommonDependencyProperty]
		public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueNotNegative));

		// Token: 0x04003527 RID: 13607
		[CommonDependencyProperty]
		public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueGreaterThanZero));

		// Token: 0x04003528 RID: 13608
		[CommonDependencyProperty]
		public static readonly DependencyProperty RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan", typeof(int), typeof(Grid), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(Grid.OnCellAttachedPropertyChanged)), new ValidateValueCallback(Grid.IsIntValueGreaterThanZero));

		// Token: 0x04003529 RID: 13609
		public static readonly DependencyProperty IsSharedSizeScopeProperty = DependencyProperty.RegisterAttached("IsSharedSizeScope", typeof(bool), typeof(Grid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DefinitionBase.OnIsSharedSizeScopePropertyChanged)));

		// Token: 0x02000BD7 RID: 3031
		private class ExtendedData
		{
			// Token: 0x04004A1B RID: 18971
			internal ColumnDefinitionCollection ColumnDefinitions;

			// Token: 0x04004A1C RID: 18972
			internal RowDefinitionCollection RowDefinitions;

			// Token: 0x04004A1D RID: 18973
			internal DefinitionBase[] DefinitionsU;

			// Token: 0x04004A1E RID: 18974
			internal DefinitionBase[] DefinitionsV;

			// Token: 0x04004A1F RID: 18975
			internal Grid.CellCache[] CellCachesCollection;

			// Token: 0x04004A20 RID: 18976
			internal int CellGroup1;

			// Token: 0x04004A21 RID: 18977
			internal int CellGroup2;

			// Token: 0x04004A22 RID: 18978
			internal int CellGroup3;

			// Token: 0x04004A23 RID: 18979
			internal int CellGroup4;

			// Token: 0x04004A24 RID: 18980
			internal DefinitionBase[] TempDefinitions;
		}

		// Token: 0x02000BD8 RID: 3032
		[Flags]
		private enum Flags
		{
			// Token: 0x04004A26 RID: 18982
			ValidDefinitionsUStructure = 1,
			// Token: 0x04004A27 RID: 18983
			ValidDefinitionsVStructure = 2,
			// Token: 0x04004A28 RID: 18984
			ValidCellsStructure = 4,
			// Token: 0x04004A29 RID: 18985
			ShowGridLinesPropertyValue = 256,
			// Token: 0x04004A2A RID: 18986
			ListenToNotifications = 4096,
			// Token: 0x04004A2B RID: 18987
			SizeToContentU = 8192,
			// Token: 0x04004A2C RID: 18988
			SizeToContentV = 16384,
			// Token: 0x04004A2D RID: 18989
			HasStarCellsU = 32768,
			// Token: 0x04004A2E RID: 18990
			HasStarCellsV = 65536,
			// Token: 0x04004A2F RID: 18991
			HasGroup3CellsInAutoRows = 131072,
			// Token: 0x04004A30 RID: 18992
			MeasureOverrideInProgress = 262144,
			// Token: 0x04004A31 RID: 18993
			ArrangeOverrideInProgress = 524288
		}

		// Token: 0x02000BD9 RID: 3033
		[Flags]
		internal enum LayoutTimeSizeType : byte
		{
			// Token: 0x04004A33 RID: 18995
			None = 0,
			// Token: 0x04004A34 RID: 18996
			Pixel = 1,
			// Token: 0x04004A35 RID: 18997
			Auto = 2,
			// Token: 0x04004A36 RID: 18998
			Star = 4
		}

		// Token: 0x02000BDA RID: 3034
		private struct CellCache
		{
			// Token: 0x17001F65 RID: 8037
			// (get) Token: 0x06008F96 RID: 36758 RVA: 0x00344A03 File Offset: 0x00343A03
			internal bool IsStarU
			{
				get
				{
					return (this.SizeTypeU & Grid.LayoutTimeSizeType.Star) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x17001F66 RID: 8038
			// (get) Token: 0x06008F97 RID: 36759 RVA: 0x00344A10 File Offset: 0x00343A10
			internal bool IsAutoU
			{
				get
				{
					return (this.SizeTypeU & Grid.LayoutTimeSizeType.Auto) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x17001F67 RID: 8039
			// (get) Token: 0x06008F98 RID: 36760 RVA: 0x00344A1D File Offset: 0x00343A1D
			internal bool IsStarV
			{
				get
				{
					return (this.SizeTypeV & Grid.LayoutTimeSizeType.Star) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x17001F68 RID: 8040
			// (get) Token: 0x06008F99 RID: 36761 RVA: 0x00344A2A File Offset: 0x00343A2A
			internal bool IsAutoV
			{
				get
				{
					return (this.SizeTypeV & Grid.LayoutTimeSizeType.Auto) > Grid.LayoutTimeSizeType.None;
				}
			}

			// Token: 0x04004A37 RID: 18999
			internal int ColumnIndex;

			// Token: 0x04004A38 RID: 19000
			internal int RowIndex;

			// Token: 0x04004A39 RID: 19001
			internal int ColumnSpan;

			// Token: 0x04004A3A RID: 19002
			internal int RowSpan;

			// Token: 0x04004A3B RID: 19003
			internal Grid.LayoutTimeSizeType SizeTypeU;

			// Token: 0x04004A3C RID: 19004
			internal Grid.LayoutTimeSizeType SizeTypeV;

			// Token: 0x04004A3D RID: 19005
			internal int Next;
		}

		// Token: 0x02000BDB RID: 3035
		private class SpanKey
		{
			// Token: 0x06008F9A RID: 36762 RVA: 0x00344A37 File Offset: 0x00343A37
			internal SpanKey(int start, int count, bool u)
			{
				this._start = start;
				this._count = count;
				this._u = u;
			}

			// Token: 0x06008F9B RID: 36763 RVA: 0x00344A54 File Offset: 0x00343A54
			public override int GetHashCode()
			{
				int num = this._start ^ this._count << 2;
				if (this._u)
				{
					num &= 134217727;
				}
				else
				{
					num |= 134217728;
				}
				return num;
			}

			// Token: 0x06008F9C RID: 36764 RVA: 0x00344A8C File Offset: 0x00343A8C
			public override bool Equals(object obj)
			{
				Grid.SpanKey spanKey = obj as Grid.SpanKey;
				return spanKey != null && spanKey._start == this._start && spanKey._count == this._count && spanKey._u == this._u;
			}

			// Token: 0x17001F69 RID: 8041
			// (get) Token: 0x06008F9D RID: 36765 RVA: 0x00344ACF File Offset: 0x00343ACF
			internal int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001F6A RID: 8042
			// (get) Token: 0x06008F9E RID: 36766 RVA: 0x00344AD7 File Offset: 0x00343AD7
			internal int Count
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x17001F6B RID: 8043
			// (get) Token: 0x06008F9F RID: 36767 RVA: 0x00344ADF File Offset: 0x00343ADF
			internal bool U
			{
				get
				{
					return this._u;
				}
			}

			// Token: 0x04004A3E RID: 19006
			private int _start;

			// Token: 0x04004A3F RID: 19007
			private int _count;

			// Token: 0x04004A40 RID: 19008
			private bool _u;
		}

		// Token: 0x02000BDC RID: 3036
		private class SpanPreferredDistributionOrderComparer : IComparer
		{
			// Token: 0x06008FA0 RID: 36768 RVA: 0x00344AE8 File Offset: 0x00343AE8
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					if (definitionBase.UserSize.IsAuto)
					{
						if (definitionBase2.UserSize.IsAuto)
						{
							result = definitionBase.MinSize.CompareTo(definitionBase2.MinSize);
						}
						else
						{
							result = -1;
						}
					}
					else if (definitionBase2.UserSize.IsAuto)
					{
						result = 1;
					}
					else
					{
						result = definitionBase.PreferredSize.CompareTo(definitionBase2.PreferredSize);
					}
				}
				return result;
			}
		}

		// Token: 0x02000BDD RID: 3037
		private class SpanMaxDistributionOrderComparer : IComparer
		{
			// Token: 0x06008FA2 RID: 36770 RVA: 0x00344B78 File Offset: 0x00343B78
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					if (definitionBase.UserSize.IsAuto)
					{
						if (definitionBase2.UserSize.IsAuto)
						{
							result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
						}
						else
						{
							result = 1;
						}
					}
					else if (definitionBase2.UserSize.IsAuto)
					{
						result = -1;
					}
					else
					{
						result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
					}
				}
				return result;
			}
		}

		// Token: 0x02000BDE RID: 3038
		private class StarDistributionOrderComparer : IComparer
		{
			// Token: 0x06008FA4 RID: 36772 RVA: 0x00344C08 File Offset: 0x00343C08
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}
		}

		// Token: 0x02000BDF RID: 3039
		private class DistributionOrderComparer : IComparer
		{
			// Token: 0x06008FA6 RID: 36774 RVA: 0x00344C44 File Offset: 0x00343C44
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					double num = definitionBase.SizeCache - definitionBase.MinSizeForArrange;
					double value = definitionBase2.SizeCache - definitionBase2.MinSizeForArrange;
					result = num.CompareTo(value);
				}
				return result;
			}
		}

		// Token: 0x02000BE0 RID: 3040
		private class StarDistributionOrderIndexComparer : IComparer
		{
			// Token: 0x06008FA8 RID: 36776 RVA: 0x00344C92 File Offset: 0x00343C92
			internal StarDistributionOrderIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008FA9 RID: 36777 RVA: 0x00344CAC File Offset: 0x00343CAC
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}

			// Token: 0x04004A41 RID: 19009
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000BE1 RID: 3041
		private class DistributionOrderIndexComparer : IComparer
		{
			// Token: 0x06008FAA RID: 36778 RVA: 0x00344D29 File Offset: 0x00343D29
			internal DistributionOrderIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008FAB RID: 36779 RVA: 0x00344D44 File Offset: 0x00343D44
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					double num3 = definitionBase.SizeCache - definitionBase.MinSizeForArrange;
					double value = definitionBase2.SizeCache - definitionBase2.MinSizeForArrange;
					result = num3.CompareTo(value);
				}
				return result;
			}

			// Token: 0x04004A42 RID: 19010
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000BE2 RID: 3042
		private class RoundingErrorIndexComparer : IComparer
		{
			// Token: 0x06008FAC RID: 36780 RVA: 0x00344DD3 File Offset: 0x00343DD3
			internal RoundingErrorIndexComparer(double[] errors)
			{
				Invariant.Assert(errors != null);
				this.errors = errors;
			}

			// Token: 0x06008FAD RID: 36781 RVA: 0x00344DEC File Offset: 0x00343DEC
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				int result;
				if (!Grid.CompareNullRefs(num, num2, out result))
				{
					double num3 = this.errors[num.Value];
					double value = this.errors[num2.Value];
					result = num3.CompareTo(value);
				}
				return result;
			}

			// Token: 0x04004A43 RID: 19011
			private readonly double[] errors;
		}

		// Token: 0x02000BE3 RID: 3043
		private class MinRatioComparer : IComparer
		{
			// Token: 0x06008FAE RID: 36782 RVA: 0x00344E50 File Offset: 0x00343E50
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase2, definitionBase, out result))
				{
					result = definitionBase2.MeasureSize.CompareTo(definitionBase.MeasureSize);
				}
				return result;
			}
		}

		// Token: 0x02000BE4 RID: 3044
		private class MaxRatioComparer : IComparer
		{
			// Token: 0x06008FB0 RID: 36784 RVA: 0x00344C08 File Offset: 0x00343C08
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}
		}

		// Token: 0x02000BE5 RID: 3045
		private class StarWeightComparer : IComparer
		{
			// Token: 0x06008FB2 RID: 36786 RVA: 0x00344E8C File Offset: 0x00343E8C
			public int Compare(object x, object y)
			{
				DefinitionBase definitionBase = x as DefinitionBase;
				DefinitionBase definitionBase2 = y as DefinitionBase;
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.MeasureSize.CompareTo(definitionBase2.MeasureSize);
				}
				return result;
			}
		}

		// Token: 0x02000BE6 RID: 3046
		private class MinRatioIndexComparer : IComparer
		{
			// Token: 0x06008FB4 RID: 36788 RVA: 0x00344EC8 File Offset: 0x00343EC8
			internal MinRatioIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008FB5 RID: 36789 RVA: 0x00344EE0 File Offset: 0x00343EE0
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase2, definitionBase, out result))
				{
					result = definitionBase2.MeasureSize.CompareTo(definitionBase.MeasureSize);
				}
				return result;
			}

			// Token: 0x04004A44 RID: 19012
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000BE7 RID: 3047
		private class MaxRatioIndexComparer : IComparer
		{
			// Token: 0x06008FB6 RID: 36790 RVA: 0x00344F5D File Offset: 0x00343F5D
			internal MaxRatioIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008FB7 RID: 36791 RVA: 0x00344F78 File Offset: 0x00343F78
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.SizeCache.CompareTo(definitionBase2.SizeCache);
				}
				return result;
			}

			// Token: 0x04004A45 RID: 19013
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000BE8 RID: 3048
		private class StarWeightIndexComparer : IComparer
		{
			// Token: 0x06008FB8 RID: 36792 RVA: 0x00344FF5 File Offset: 0x00343FF5
			internal StarWeightIndexComparer(DefinitionBase[] definitions)
			{
				Invariant.Assert(definitions != null);
				this.definitions = definitions;
			}

			// Token: 0x06008FB9 RID: 36793 RVA: 0x00345010 File Offset: 0x00344010
			public int Compare(object x, object y)
			{
				int? num = x as int?;
				int? num2 = y as int?;
				DefinitionBase definitionBase = null;
				DefinitionBase definitionBase2 = null;
				if (num != null)
				{
					definitionBase = this.definitions[num.Value];
				}
				if (num2 != null)
				{
					definitionBase2 = this.definitions[num2.Value];
				}
				int result;
				if (!Grid.CompareNullRefs(definitionBase, definitionBase2, out result))
				{
					result = definitionBase.MeasureSize.CompareTo(definitionBase2.MeasureSize);
				}
				return result;
			}

			// Token: 0x04004A46 RID: 19014
			private readonly DefinitionBase[] definitions;
		}

		// Token: 0x02000BE9 RID: 3049
		private class GridChildrenCollectionEnumeratorSimple : IEnumerator
		{
			// Token: 0x06008FBA RID: 36794 RVA: 0x00345090 File Offset: 0x00344090
			internal GridChildrenCollectionEnumeratorSimple(Grid grid, bool includeChildren)
			{
				this._currentEnumerator = -1;
				this._enumerator0 = new ColumnDefinitionCollection.Enumerator((grid.ExtData != null) ? grid.ExtData.ColumnDefinitions : null);
				this._enumerator1 = new RowDefinitionCollection.Enumerator((grid.ExtData != null) ? grid.ExtData.RowDefinitions : null);
				this._enumerator2Index = 0;
				if (includeChildren)
				{
					this._enumerator2Collection = grid.Children;
					this._enumerator2Count = this._enumerator2Collection.Count;
					return;
				}
				this._enumerator2Collection = null;
				this._enumerator2Count = 0;
			}

			// Token: 0x06008FBB RID: 36795 RVA: 0x00345124 File Offset: 0x00344124
			public bool MoveNext()
			{
				while (this._currentEnumerator < 3)
				{
					if (this._currentEnumerator >= 0)
					{
						switch (this._currentEnumerator)
						{
						case 0:
							if (this._enumerator0.MoveNext())
							{
								this._currentChild = this._enumerator0.Current;
								return true;
							}
							break;
						case 1:
							if (this._enumerator1.MoveNext())
							{
								this._currentChild = this._enumerator1.Current;
								return true;
							}
							break;
						case 2:
							if (this._enumerator2Index < this._enumerator2Count)
							{
								this._currentChild = this._enumerator2Collection[this._enumerator2Index];
								this._enumerator2Index++;
								return true;
							}
							break;
						}
					}
					this._currentEnumerator++;
				}
				return false;
			}

			// Token: 0x17001F6C RID: 8044
			// (get) Token: 0x06008FBC RID: 36796 RVA: 0x003451ED File Offset: 0x003441ED
			public object Current
			{
				get
				{
					if (this._currentEnumerator == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					if (this._currentEnumerator >= 3)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
					}
					return this._currentChild;
				}
			}

			// Token: 0x06008FBD RID: 36797 RVA: 0x00345227 File Offset: 0x00344227
			public void Reset()
			{
				this._currentEnumerator = -1;
				this._currentChild = null;
				this._enumerator0.Reset();
				this._enumerator1.Reset();
				this._enumerator2Index = 0;
			}

			// Token: 0x04004A47 RID: 19015
			private int _currentEnumerator;

			// Token: 0x04004A48 RID: 19016
			private object _currentChild;

			// Token: 0x04004A49 RID: 19017
			private ColumnDefinitionCollection.Enumerator _enumerator0;

			// Token: 0x04004A4A RID: 19018
			private RowDefinitionCollection.Enumerator _enumerator1;

			// Token: 0x04004A4B RID: 19019
			private UIElementCollection _enumerator2Collection;

			// Token: 0x04004A4C RID: 19020
			private int _enumerator2Index;

			// Token: 0x04004A4D RID: 19021
			private int _enumerator2Count;
		}

		// Token: 0x02000BEA RID: 3050
		internal class GridLinesRenderer : DrawingVisual
		{
			// Token: 0x06008FBE RID: 36798 RVA: 0x00345254 File Offset: 0x00344254
			static GridLinesRenderer()
			{
				Grid.GridLinesRenderer.s_oddDashPen = new Pen(Brushes.Blue, 1.0);
				DoubleCollection doubleCollection = new DoubleCollection();
				doubleCollection.Add(4.0);
				doubleCollection.Add(4.0);
				Grid.GridLinesRenderer.s_oddDashPen.DashStyle = new DashStyle(doubleCollection, 0.0);
				Grid.GridLinesRenderer.s_oddDashPen.DashCap = PenLineCap.Flat;
				Grid.GridLinesRenderer.s_oddDashPen.Freeze();
				Grid.GridLinesRenderer.s_evenDashPen = new Pen(Brushes.Yellow, 1.0);
				DoubleCollection doubleCollection2 = new DoubleCollection();
				doubleCollection2.Add(4.0);
				doubleCollection2.Add(4.0);
				Grid.GridLinesRenderer.s_evenDashPen.DashStyle = new DashStyle(doubleCollection2, 4.0);
				Grid.GridLinesRenderer.s_evenDashPen.DashCap = PenLineCap.Flat;
				Grid.GridLinesRenderer.s_evenDashPen.Freeze();
			}

			// Token: 0x06008FBF RID: 36799 RVA: 0x00345354 File Offset: 0x00344354
			internal void UpdateRenderBounds(Size boundsSize)
			{
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					Grid grid = VisualTreeHelper.GetParent(this) as Grid;
					if (grid != null && grid.ShowGridLines)
					{
						for (int i = 1; i < grid.DefinitionsU.Length; i++)
						{
							Grid.GridLinesRenderer.DrawGridLine(drawingContext, grid.DefinitionsU[i].FinalOffset, 0.0, grid.DefinitionsU[i].FinalOffset, boundsSize.Height);
						}
						for (int j = 1; j < grid.DefinitionsV.Length; j++)
						{
							Grid.GridLinesRenderer.DrawGridLine(drawingContext, 0.0, grid.DefinitionsV[j].FinalOffset, boundsSize.Width, grid.DefinitionsV[j].FinalOffset);
						}
					}
				}
			}

			// Token: 0x06008FC0 RID: 36800 RVA: 0x00345428 File Offset: 0x00344428
			private static void DrawGridLine(DrawingContext drawingContext, double startX, double startY, double endX, double endY)
			{
				Point point = new Point(startX, startY);
				Point point2 = new Point(endX, endY);
				drawingContext.DrawLine(Grid.GridLinesRenderer.s_oddDashPen, point, point2);
				drawingContext.DrawLine(Grid.GridLinesRenderer.s_evenDashPen, point, point2);
			}

			// Token: 0x04004A4E RID: 19022
			private const double c_dashLength = 4.0;

			// Token: 0x04004A4F RID: 19023
			private const double c_penWidth = 1.0;

			// Token: 0x04004A50 RID: 19024
			private static readonly Pen s_oddDashPen;

			// Token: 0x04004A51 RID: 19025
			private static readonly Pen s_evenDashPen;

			// Token: 0x04004A52 RID: 19026
			private static readonly Point c_zeroPoint = new Point(0.0, 0.0);
		}

		// Token: 0x02000BEB RID: 3051
		internal enum Counters
		{
			// Token: 0x04004A54 RID: 19028
			Default = -1,
			// Token: 0x04004A55 RID: 19029
			MeasureOverride,
			// Token: 0x04004A56 RID: 19030
			_ValidateColsStructure,
			// Token: 0x04004A57 RID: 19031
			_ValidateRowsStructure,
			// Token: 0x04004A58 RID: 19032
			_ValidateCells,
			// Token: 0x04004A59 RID: 19033
			_MeasureCell,
			// Token: 0x04004A5A RID: 19034
			__MeasureChild,
			// Token: 0x04004A5B RID: 19035
			_CalculateDesiredSize,
			// Token: 0x04004A5C RID: 19036
			ArrangeOverride,
			// Token: 0x04004A5D RID: 19037
			_SetFinalSize,
			// Token: 0x04004A5E RID: 19038
			_ArrangeChildHelper2,
			// Token: 0x04004A5F RID: 19039
			_PositionCell,
			// Token: 0x04004A60 RID: 19040
			Count
		}
	}
}
