using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200084F RID: 2127
	public class ResizeGrip : Control
	{
		// Token: 0x06007CDC RID: 31964 RVA: 0x00311068 File Offset: 0x00310068
		static ResizeGrip()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(typeof(ResizeGrip)));
			ResizeGrip._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ResizeGrip));
			Window.IWindowServiceProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(new PropertyChangedCallback(ResizeGrip._OnWindowServiceChanged)));
		}

		// Token: 0x06007CDE RID: 31966 RVA: 0x003110D1 File Offset: 0x003100D1
		private static void _OnWindowServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as ResizeGrip).OnWindowServiceChanged(e.OldValue as Window, e.NewValue as Window);
		}

		// Token: 0x06007CDF RID: 31967 RVA: 0x003110F6 File Offset: 0x003100F6
		private void OnWindowServiceChanged(Window oldWindow, Window newWindow)
		{
			if (oldWindow != null && oldWindow != newWindow)
			{
				oldWindow.ClearResizeGripControl(this);
			}
			if (newWindow != null)
			{
				newWindow.SetResizeGripControl(this);
			}
		}

		// Token: 0x17001CD5 RID: 7381
		// (get) Token: 0x06007CE0 RID: 31968 RVA: 0x00311110 File Offset: 0x00310110
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ResizeGrip._dType;
			}
		}

		// Token: 0x17001CD6 RID: 7382
		// (get) Token: 0x06007CE1 RID: 31969 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x04003AAA RID: 15018
		private static DependencyObjectType _dType;
	}
}
