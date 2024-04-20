using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007ED RID: 2029
	[ContentProperty("ToolBars")]
	public class ToolBarTray : FrameworkElement, IAddChild
	{
		// Token: 0x06007599 RID: 30105 RVA: 0x002EC484 File Offset: 0x002EB484
		static ToolBarTray()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBarTray), new FrameworkPropertyMetadata(typeof(ToolBarTray)));
			ToolBarTray._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolBarTray));
			EventManager.RegisterClassHandler(typeof(ToolBarTray), Thumb.DragDeltaEvent, new DragDeltaEventHandler(ToolBarTray.OnThumbDragDelta));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ToolBarTray), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBarTray);
		}

		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x0600759B RID: 30107 RVA: 0x002EC5C7 File Offset: 0x002EB5C7
		// (set) Token: 0x0600759C RID: 30108 RVA: 0x002EC5D9 File Offset: 0x002EB5D9
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(ToolBarTray.BackgroundProperty);
			}
			set
			{
				base.SetValue(ToolBarTray.BackgroundProperty, value);
			}
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x002EC5E8 File Offset: 0x002EB5E8
		private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Collection<ToolBar> toolBars = ((ToolBarTray)d).ToolBars;
			for (int i = 0; i < toolBars.Count; i++)
			{
				toolBars[i].CoerceValue(ToolBar.OrientationProperty);
			}
		}

		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x0600759E RID: 30110 RVA: 0x002EC623 File Offset: 0x002EB623
		// (set) Token: 0x0600759F RID: 30111 RVA: 0x002EC635 File Offset: 0x002EB635
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ToolBarTray.OrientationProperty);
			}
			set
			{
				base.SetValue(ToolBarTray.OrientationProperty, value);
			}
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x060075A0 RID: 30112 RVA: 0x002EC648 File Offset: 0x002EB648
		// (set) Token: 0x060075A1 RID: 30113 RVA: 0x002EC65A File Offset: 0x002EB65A
		public bool IsLocked
		{
			get
			{
				return (bool)base.GetValue(ToolBarTray.IsLockedProperty);
			}
			set
			{
				base.SetValue(ToolBarTray.IsLockedProperty, value);
			}
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x002EC668 File Offset: 0x002EB668
		public static void SetIsLocked(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolBarTray.IsLockedProperty, value);
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x002EC684 File Offset: 0x002EB684
		public static bool GetIsLocked(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolBarTray.IsLockedProperty);
		}

		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x060075A4 RID: 30116 RVA: 0x002EC6A4 File Offset: 0x002EB6A4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<ToolBar> ToolBars
		{
			get
			{
				if (this._toolBarsCollection == null)
				{
					this._toolBarsCollection = new ToolBarTray.ToolBarCollection(this);
				}
				return this._toolBarsCollection;
			}
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x002EC6C0 File Offset: 0x002EB6C0
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ToolBar toolBar = value as ToolBar;
			if (toolBar == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(ToolBar)
				}), "value");
			}
			this.ToolBars.Add(toolBar);
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x060075A7 RID: 30119 RVA: 0x002EC722 File Offset: 0x002EB722
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.VisualChildrenCount == 0)
				{
					return EmptyEnumerator.Instance;
				}
				return this.ToolBars.GetEnumerator();
			}
		}

		// Token: 0x060075A8 RID: 30120 RVA: 0x002EC740 File Offset: 0x002EB740
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x002EC794 File Offset: 0x002EB794
		protected override Size MeasureOverride(Size constraint)
		{
			this.GenerateBands();
			Size result = default(Size);
			bool flag = this.Orientation == Orientation.Horizontal;
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			for (int i = 0; i < this._bands.Count; i++)
			{
				double num = flag ? constraint.Width : constraint.Height;
				List<ToolBar> band = this._bands[i].Band;
				double num2 = 0.0;
				double num3 = 0.0;
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar = band[j];
					num -= toolBar.MinLength;
					if (DoubleUtil.LessThan(num, 0.0))
					{
						num = 0.0;
						break;
					}
				}
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar2 = band[j];
					num += toolBar2.MinLength;
					if (flag)
					{
						availableSize.Width = num;
					}
					else
					{
						availableSize.Height = num;
					}
					toolBar2.Measure(availableSize);
					num2 = Math.Max(num2, flag ? toolBar2.DesiredSize.Height : toolBar2.DesiredSize.Width);
					num3 += (flag ? toolBar2.DesiredSize.Width : toolBar2.DesiredSize.Height);
					num -= (flag ? toolBar2.DesiredSize.Width : toolBar2.DesiredSize.Height);
					if (DoubleUtil.LessThan(num, 0.0))
					{
						num = 0.0;
					}
				}
				this._bands[i].Thickness = num2;
				if (flag)
				{
					result.Height += num2;
					result.Width = Math.Max(result.Width, num3);
				}
				else
				{
					result.Width += num2;
					result.Height = Math.Max(result.Height, num3);
				}
			}
			return result;
		}

		// Token: 0x060075AA RID: 30122 RVA: 0x002EC9C4 File Offset: 0x002EB9C4
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			Rect finalRect = default(Rect);
			for (int i = 0; i < this._bands.Count; i++)
			{
				List<ToolBar> band = this._bands[i].Band;
				double thickness = this._bands[i].Thickness;
				if (flag)
				{
					finalRect.X = 0.0;
				}
				else
				{
					finalRect.Y = 0.0;
				}
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar = band[j];
					Size size = new Size(flag ? toolBar.DesiredSize.Width : thickness, flag ? thickness : toolBar.DesiredSize.Height);
					finalRect.Size = size;
					toolBar.Arrange(finalRect);
					if (flag)
					{
						finalRect.X += size.Width;
					}
					else
					{
						finalRect.Y += size.Height;
					}
				}
				if (flag)
				{
					finalRect.Y += thickness;
				}
				else
				{
					finalRect.X += thickness;
				}
			}
			return arrangeSize;
		}

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x060075AB RID: 30123 RVA: 0x002ECB07 File Offset: 0x002EBB07
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._toolBarsCollection == null)
				{
					return 0;
				}
				return this._toolBarsCollection.Count;
			}
		}

		// Token: 0x060075AC RID: 30124 RVA: 0x002ECB1E File Offset: 0x002EBB1E
		protected override Visual GetVisualChild(int index)
		{
			if (this._toolBarsCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._toolBarsCollection[index];
		}

		// Token: 0x060075AD RID: 30125 RVA: 0x002ECB50 File Offset: 0x002EBB50
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			ToolBarTray toolBarTray = (ToolBarTray)sender;
			if (toolBarTray.IsLocked)
			{
				return;
			}
			toolBarTray.ProcessThumbDragDelta(e);
		}

		// Token: 0x060075AE RID: 30126 RVA: 0x002ECB74 File Offset: 0x002EBB74
		private void ProcessThumbDragDelta(DragDeltaEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (thumb != null)
			{
				ToolBar toolBar = thumb.TemplatedParent as ToolBar;
				if (toolBar != null && toolBar.Parent == this)
				{
					if (this._bandsDirty)
					{
						this.GenerateBands();
					}
					bool flag = this.Orientation == Orientation.Horizontal;
					int band = toolBar.Band;
					Point position = Mouse.PrimaryDevice.GetPosition(this);
					Point point = this.TransformPointToToolBar(toolBar, position);
					int bandFromOffset = this.GetBandFromOffset(flag ? position.Y : position.X);
					double num = flag ? e.HorizontalChange : e.VerticalChange;
					double num2;
					if (flag)
					{
						num2 = position.X - point.X;
					}
					else
					{
						num2 = position.Y - point.Y;
					}
					double num3 = num2 + num;
					if (bandFromOffset == band)
					{
						List<ToolBar> band2 = this._bands[band].Band;
						int bandIndex = toolBar.BandIndex;
						if (DoubleUtil.LessThan(num, 0.0))
						{
							double num4 = this.ToolBarsTotalMinimum(band2, 0, bandIndex - 1);
							if (DoubleUtil.LessThanOrClose(num4, num3))
							{
								this.ShrinkToolBars(band2, 0, bandIndex - 1, -num);
							}
							else if (bandIndex > 0)
							{
								ToolBar toolBar2 = band2[bandIndex - 1];
								Point point2 = this.TransformPointToToolBar(toolBar2, position);
								if (DoubleUtil.LessThan(flag ? point2.X : point2.Y, 0.0))
								{
									toolBar2.BandIndex = bandIndex;
									band2[bandIndex] = toolBar2;
									toolBar.BandIndex = bandIndex - 1;
									band2[bandIndex - 1] = toolBar;
									if (bandIndex + 1 == band2.Count)
									{
										toolBar2.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
									}
								}
								else if (flag)
								{
									if (DoubleUtil.LessThan(num4, position.X - point.X))
									{
										this.ShrinkToolBars(band2, 0, bandIndex - 1, position.X - point.X - num4);
									}
								}
								else if (DoubleUtil.LessThan(num4, position.Y - point.Y))
								{
									this.ShrinkToolBars(band2, 0, bandIndex - 1, position.Y - point.Y - num4);
								}
							}
						}
						else if (DoubleUtil.GreaterThan(this.ToolBarsTotalMaximum(band2, 0, bandIndex - 1), num3))
						{
							this.ExpandToolBars(band2, 0, bandIndex - 1, num);
						}
						else if (bandIndex < band2.Count - 1)
						{
							ToolBar toolBar3 = band2[bandIndex + 1];
							Point point3 = this.TransformPointToToolBar(toolBar3, position);
							if (DoubleUtil.GreaterThanOrClose(flag ? point3.X : point3.Y, 0.0))
							{
								toolBar3.BandIndex = bandIndex;
								band2[bandIndex] = toolBar3;
								toolBar.BandIndex = bandIndex + 1;
								band2[bandIndex + 1] = toolBar;
								if (bandIndex + 2 == band2.Count)
								{
									toolBar.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
								}
							}
							else
							{
								this.ExpandToolBars(band2, 0, bandIndex - 1, num);
							}
						}
						else
						{
							this.ExpandToolBars(band2, 0, bandIndex - 1, num);
						}
					}
					else
					{
						this._bandsDirty = true;
						toolBar.Band = bandFromOffset;
						toolBar.ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
						if (bandFromOffset >= 0 && bandFromOffset < this._bands.Count)
						{
							this.MoveToolBar(toolBar, bandFromOffset, num3);
						}
						List<ToolBar> band3 = this._bands[band].Band;
						for (int i = 0; i < band3.Count; i++)
						{
							band3[i].ClearValue(flag ? FrameworkElement.WidthProperty : FrameworkElement.HeightProperty);
						}
					}
					e.Handled = true;
				}
			}
		}

		// Token: 0x060075AF RID: 30127 RVA: 0x002ECF48 File Offset: 0x002EBF48
		private Point TransformPointToToolBar(ToolBar toolBar, Point point)
		{
			Point result = point;
			GeneralTransform generalTransform = base.TransformToDescendant(toolBar);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point, out result);
			}
			return result;
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x002ECF70 File Offset: 0x002EBF70
		private void ShrinkToolBars(List<ToolBar> band, int startIndex, int endIndex, double shrinkAmount)
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					ToolBar toolBar = band[i];
					if (DoubleUtil.GreaterThanOrClose(toolBar.RenderSize.Width - shrinkAmount, toolBar.MinLength))
					{
						toolBar.Width = toolBar.RenderSize.Width - shrinkAmount;
						return;
					}
					toolBar.Width = toolBar.MinLength;
					shrinkAmount -= toolBar.RenderSize.Width - toolBar.MinLength;
				}
				return;
			}
			for (int j = endIndex; j >= startIndex; j--)
			{
				ToolBar toolBar2 = band[j];
				if (DoubleUtil.GreaterThanOrClose(toolBar2.RenderSize.Height - shrinkAmount, toolBar2.MinLength))
				{
					toolBar2.Height = toolBar2.RenderSize.Height - shrinkAmount;
					return;
				}
				toolBar2.Height = toolBar2.MinLength;
				shrinkAmount -= toolBar2.RenderSize.Height - toolBar2.MinLength;
			}
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x002ED070 File Offset: 0x002EC070
		private double ToolBarsTotalMinimum(List<ToolBar> band, int startIndex, int endIndex)
		{
			double num = 0.0;
			for (int i = startIndex; i <= endIndex; i++)
			{
				num += band[i].MinLength;
			}
			return num;
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x002ED0A4 File Offset: 0x002EC0A4
		private void ExpandToolBars(List<ToolBar> band, int startIndex, int endIndex, double expandAmount)
		{
			if (this.Orientation == Orientation.Horizontal)
			{
				for (int i = endIndex; i >= startIndex; i--)
				{
					ToolBar toolBar = band[i];
					if (DoubleUtil.LessThanOrClose(toolBar.RenderSize.Width + expandAmount, toolBar.MaxLength))
					{
						toolBar.Width = toolBar.RenderSize.Width + expandAmount;
						return;
					}
					toolBar.Width = toolBar.MaxLength;
					expandAmount -= toolBar.MaxLength - toolBar.RenderSize.Width;
				}
				return;
			}
			for (int j = endIndex; j >= startIndex; j--)
			{
				ToolBar toolBar2 = band[j];
				if (DoubleUtil.LessThanOrClose(toolBar2.RenderSize.Height + expandAmount, toolBar2.MaxLength))
				{
					toolBar2.Height = toolBar2.RenderSize.Height + expandAmount;
					return;
				}
				toolBar2.Height = toolBar2.MaxLength;
				expandAmount -= toolBar2.MaxLength - toolBar2.RenderSize.Height;
			}
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x002ED1A4 File Offset: 0x002EC1A4
		private double ToolBarsTotalMaximum(List<ToolBar> band, int startIndex, int endIndex)
		{
			double num = 0.0;
			for (int i = startIndex; i <= endIndex; i++)
			{
				num += band[i].MaxLength;
			}
			return num;
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x002ED1D8 File Offset: 0x002EC1D8
		private void MoveToolBar(ToolBar toolBar, int newBandNumber, double position)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			List<ToolBar> band = this._bands[newBandNumber].Band;
			if (DoubleUtil.LessThanOrClose(position, 0.0))
			{
				toolBar.BandIndex = -1;
				return;
			}
			double num = 0.0;
			int num2 = -1;
			int i;
			for (i = 0; i < band.Count; i++)
			{
				ToolBar toolBar2 = band[i];
				if (num2 == -1)
				{
					num += (flag ? toolBar2.RenderSize.Width : toolBar2.RenderSize.Height);
					if (DoubleUtil.GreaterThan(num, position))
					{
						num2 = i + 1;
						toolBar.BandIndex = num2;
						if (flag)
						{
							toolBar2.Width = Math.Max(toolBar2.MinLength, toolBar2.RenderSize.Width - num + position);
						}
						else
						{
							toolBar2.Height = Math.Max(toolBar2.MinLength, toolBar2.RenderSize.Height - num + position);
						}
					}
				}
				else
				{
					toolBar2.BandIndex = i + 1;
				}
			}
			if (num2 == -1)
			{
				toolBar.BandIndex = i;
			}
		}

		// Token: 0x060075B5 RID: 30133 RVA: 0x002ED2F8 File Offset: 0x002EC2F8
		private int GetBandFromOffset(double toolBarOffset)
		{
			if (DoubleUtil.LessThan(toolBarOffset, 0.0))
			{
				return -1;
			}
			double num = 0.0;
			for (int i = 0; i < this._bands.Count; i++)
			{
				num += this._bands[i].Thickness;
				if (DoubleUtil.GreaterThan(num, toolBarOffset))
				{
					return i;
				}
			}
			return this._bands.Count;
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x002ED364 File Offset: 0x002EC364
		private void GenerateBands()
		{
			if (!this.IsBandsDirty())
			{
				return;
			}
			Collection<ToolBar> toolBars = this.ToolBars;
			this._bands.Clear();
			for (int i = 0; i < toolBars.Count; i++)
			{
				this.InsertBand(toolBars[i], i);
			}
			for (int j = 0; j < this._bands.Count; j++)
			{
				List<ToolBar> band = this._bands[j].Band;
				for (int k = 0; k < band.Count; k++)
				{
					ToolBar toolBar = band[k];
					toolBar.Band = j;
					toolBar.BandIndex = k;
				}
			}
			this._bandsDirty = false;
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x002ED408 File Offset: 0x002EC408
		private bool IsBandsDirty()
		{
			if (this._bandsDirty)
			{
				return true;
			}
			int num = 0;
			Collection<ToolBar> toolBars = this.ToolBars;
			for (int i = 0; i < this._bands.Count; i++)
			{
				List<ToolBar> band = this._bands[i].Band;
				for (int j = 0; j < band.Count; j++)
				{
					ToolBar toolBar = band[j];
					if (toolBar.Band != i || toolBar.BandIndex != j || !toolBars.Contains(toolBar))
					{
						return true;
					}
				}
				num += band.Count;
			}
			return num != toolBars.Count;
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x002ED4A8 File Offset: 0x002EC4A8
		private void InsertBand(ToolBar toolBar, int toolBarIndex)
		{
			int band = toolBar.Band;
			for (int i = 0; i < this._bands.Count; i++)
			{
				int band2 = this._bands[i].Band[0].Band;
				if (band == band2)
				{
					return;
				}
				if (band < band2)
				{
					this._bands.Insert(i, this.CreateBand(toolBarIndex));
					return;
				}
			}
			this._bands.Add(this.CreateBand(toolBarIndex));
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x002ED520 File Offset: 0x002EC520
		private ToolBarTray.BandInfo CreateBand(int startIndex)
		{
			Collection<ToolBar> toolBars = this.ToolBars;
			ToolBarTray.BandInfo bandInfo = new ToolBarTray.BandInfo();
			ToolBar toolBar = toolBars[startIndex];
			bandInfo.Band.Add(toolBar);
			int band = toolBar.Band;
			for (int i = startIndex + 1; i < toolBars.Count; i++)
			{
				toolBar = toolBars[i];
				if (band == toolBar.Band)
				{
					this.InsertToolBar(toolBar, bandInfo.Band);
				}
			}
			return bandInfo;
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x002ED58C File Offset: 0x002EC58C
		private void InsertToolBar(ToolBar toolBar, List<ToolBar> band)
		{
			for (int i = 0; i < band.Count; i++)
			{
				if (toolBar.BandIndex < band[i].BandIndex)
				{
					band.Insert(i, toolBar);
					return;
				}
			}
			band.Add(toolBar);
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x060075BB RID: 30139 RVA: 0x002ED5CE File Offset: 0x002EC5CE
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToolBarTray._dType;
			}
		}

		// Token: 0x0400385F RID: 14431
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(ToolBarTray), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003860 RID: 14432
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ToolBarTray), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsParentMeasure, new PropertyChangedCallback(ToolBarTray.OnOrientationPropertyChanged)), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		// Token: 0x04003861 RID: 14433
		public static readonly DependencyProperty IsLockedProperty = DependencyProperty.RegisterAttached("IsLocked", typeof(bool), typeof(ToolBarTray), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04003862 RID: 14434
		private List<ToolBarTray.BandInfo> _bands = new List<ToolBarTray.BandInfo>(0);

		// Token: 0x04003863 RID: 14435
		private bool _bandsDirty = true;

		// Token: 0x04003864 RID: 14436
		private ToolBarTray.ToolBarCollection _toolBarsCollection;

		// Token: 0x04003865 RID: 14437
		private static DependencyObjectType _dType;

		// Token: 0x02000C26 RID: 3110
		private class ToolBarCollection : Collection<ToolBar>
		{
			// Token: 0x060090A6 RID: 37030 RVA: 0x00346EE5 File Offset: 0x00345EE5
			public ToolBarCollection(ToolBarTray parent)
			{
				this._parent = parent;
			}

			// Token: 0x060090A7 RID: 37031 RVA: 0x00346EF4 File Offset: 0x00345EF4
			protected override void InsertItem(int index, ToolBar toolBar)
			{
				base.InsertItem(index, toolBar);
				this._parent.AddLogicalChild(toolBar);
				this._parent.AddVisualChild(toolBar);
				this._parent.InvalidateMeasure();
			}

			// Token: 0x060090A8 RID: 37032 RVA: 0x00346F24 File Offset: 0x00345F24
			protected override void SetItem(int index, ToolBar toolBar)
			{
				ToolBar toolBar2 = base.Items[index];
				if (toolBar != toolBar2)
				{
					base.SetItem(index, toolBar);
					this._parent.RemoveVisualChild(toolBar2);
					this._parent.RemoveLogicalChild(toolBar2);
					this._parent.AddLogicalChild(toolBar);
					this._parent.AddVisualChild(toolBar);
					this._parent.InvalidateMeasure();
				}
			}

			// Token: 0x060090A9 RID: 37033 RVA: 0x00346F88 File Offset: 0x00345F88
			protected override void RemoveItem(int index)
			{
				ToolBar child = base[index];
				base.RemoveItem(index);
				this._parent.RemoveVisualChild(child);
				this._parent.RemoveLogicalChild(child);
				this._parent.InvalidateMeasure();
			}

			// Token: 0x060090AA RID: 37034 RVA: 0x00346FC8 File Offset: 0x00345FC8
			protected override void ClearItems()
			{
				int count = base.Count;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						ToolBar child = base[i];
						this._parent.RemoveVisualChild(child);
						this._parent.RemoveLogicalChild(child);
					}
					this._parent.InvalidateMeasure();
				}
				base.ClearItems();
			}

			// Token: 0x04004B4D RID: 19277
			private readonly ToolBarTray _parent;
		}

		// Token: 0x02000C27 RID: 3111
		private class BandInfo
		{
			// Token: 0x17001F9E RID: 8094
			// (get) Token: 0x060090AC RID: 37036 RVA: 0x00347030 File Offset: 0x00346030
			public List<ToolBar> Band
			{
				get
				{
					return this._band;
				}
			}

			// Token: 0x17001F9F RID: 8095
			// (get) Token: 0x060090AD RID: 37037 RVA: 0x00347038 File Offset: 0x00346038
			// (set) Token: 0x060090AE RID: 37038 RVA: 0x00347040 File Offset: 0x00346040
			public double Thickness
			{
				get
				{
					return this._thickness;
				}
				set
				{
					this._thickness = value;
				}
			}

			// Token: 0x04004B4E RID: 19278
			private List<ToolBar> _band = new List<ToolBar>();

			// Token: 0x04004B4F RID: 19279
			private double _thickness;
		}
	}
}
