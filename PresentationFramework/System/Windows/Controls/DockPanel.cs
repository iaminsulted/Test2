using System;
using System.Windows.Media;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000777 RID: 1911
	public class DockPanel : Panel
	{
		// Token: 0x060067E7 RID: 26599 RVA: 0x002B652C File Offset: 0x002B552C
		static DockPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.DockPanel);
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x002B65B9 File Offset: 0x002B55B9
		[AttachedPropertyBrowsableForChildren]
		public static Dock GetDock(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Dock)element.GetValue(DockPanel.DockProperty);
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x002B65D9 File Offset: 0x002B55D9
		public static void SetDock(UIElement element, Dock dock)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(DockPanel.DockProperty, dock);
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x002B65FC File Offset: 0x002B55FC
		private static void OnDockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				DockPanel dockPanel = VisualTreeHelper.GetParent(uielement) as DockPanel;
				if (dockPanel != null)
				{
					dockPanel.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x060067EC RID: 26604 RVA: 0x002B6628 File Offset: 0x002B5628
		// (set) Token: 0x060067ED RID: 26605 RVA: 0x002B663A File Offset: 0x002B563A
		public bool LastChildFill
		{
			get
			{
				return (bool)base.GetValue(DockPanel.LastChildFillProperty);
			}
			set
			{
				base.SetValue(DockPanel.LastChildFillProperty, value);
			}
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x002B6648 File Offset: 0x002B5648
		protected override Size MeasureOverride(Size constraint)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					Size availableSize = new Size(Math.Max(0.0, constraint.Width - num3), Math.Max(0.0, constraint.Height - num4));
					uielement.Measure(availableSize);
					Size desiredSize = uielement.DesiredSize;
					switch (DockPanel.GetDock(uielement))
					{
					case Dock.Left:
					case Dock.Right:
						num2 = Math.Max(num2, num4 + desiredSize.Height);
						num3 += desiredSize.Width;
						break;
					case Dock.Top:
					case Dock.Bottom:
						num = Math.Max(num, num3 + desiredSize.Width);
						num4 += desiredSize.Height;
						break;
					}
				}
				i++;
			}
			num = Math.Max(num, num3);
			num2 = Math.Max(num2, num4);
			return new Size(num, num2);
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x002B6770 File Offset: 0x002B5770
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			int count = internalChildren.Count;
			int num = count - (this.LastChildFill ? 1 : 0);
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					Size desiredSize = uielement.DesiredSize;
					Rect finalRect = new Rect(num2, num3, Math.Max(0.0, arrangeSize.Width - (num2 + num4)), Math.Max(0.0, arrangeSize.Height - (num3 + num5)));
					if (i < num)
					{
						switch (DockPanel.GetDock(uielement))
						{
						case Dock.Left:
							num2 += desiredSize.Width;
							finalRect.Width = desiredSize.Width;
							break;
						case Dock.Top:
							num3 += desiredSize.Height;
							finalRect.Height = desiredSize.Height;
							break;
						case Dock.Right:
							num4 += desiredSize.Width;
							finalRect.X = Math.Max(0.0, arrangeSize.Width - num4);
							finalRect.Width = desiredSize.Width;
							break;
						case Dock.Bottom:
							num5 += desiredSize.Height;
							finalRect.Y = Math.Max(0.0, arrangeSize.Height - num5);
							finalRect.Height = desiredSize.Height;
							break;
						}
					}
					uielement.Arrange(finalRect);
				}
			}
			return arrangeSize;
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x002B6918 File Offset: 0x002B5918
		internal static bool IsValidDock(object o)
		{
			Dock dock = (Dock)o;
			return dock == Dock.Left || dock == Dock.Top || dock == Dock.Right || dock == Dock.Bottom;
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x060067F1 RID: 26609 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x04003465 RID: 13413
		[CommonDependencyProperty]
		public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register("LastChildFill", typeof(bool), typeof(DockPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));

		// Token: 0x04003466 RID: 13414
		[CommonDependencyProperty]
		public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached("Dock", typeof(Dock), typeof(DockPanel), new FrameworkPropertyMetadata(Dock.Left, new PropertyChangedCallback(DockPanel.OnDockChanged)), new ValidateValueCallback(DockPanel.IsValidDock));
	}
}
