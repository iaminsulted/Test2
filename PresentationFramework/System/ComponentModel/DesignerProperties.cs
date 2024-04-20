using System;
using System.Windows;
using MS.Internal.KnownBoxes;

namespace System.ComponentModel
{
	// Token: 0x0200033A RID: 826
	public static class DesignerProperties
	{
		// Token: 0x06001F27 RID: 7975 RVA: 0x0017119D File Offset: 0x0017019D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static bool GetIsInDesignMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(DesignerProperties.IsInDesignModeProperty);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x001711BD File Offset: 0x001701BD
		public static void SetIsInDesignMode(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(DesignerProperties.IsInDesignModeProperty, value);
		}

		// Token: 0x04000F6F RID: 3951
		public static readonly DependencyProperty IsInDesignModeProperty = DependencyProperty.RegisterAttached("IsInDesignMode", typeof(bool), typeof(DesignerProperties), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));
	}
}
