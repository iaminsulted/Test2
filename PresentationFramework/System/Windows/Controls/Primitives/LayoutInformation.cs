using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000844 RID: 2116
	public static class LayoutInformation
	{
		// Token: 0x06007BD3 RID: 31699 RVA: 0x0030C85D File Offset: 0x0030B85D
		private static void CheckArgument(FrameworkElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
		}

		// Token: 0x06007BD4 RID: 31700 RVA: 0x0030C86D File Offset: 0x0030B86D
		public static Rect GetLayoutSlot(FrameworkElement element)
		{
			LayoutInformation.CheckArgument(element);
			return element.PreviousArrangeRect;
		}

		// Token: 0x06007BD5 RID: 31701 RVA: 0x0030C87B File Offset: 0x0030B87B
		public static Geometry GetLayoutClip(FrameworkElement element)
		{
			LayoutInformation.CheckArgument(element);
			return element.GetLayoutClipInternal();
		}

		// Token: 0x06007BD6 RID: 31702 RVA: 0x0030C88C File Offset: 0x0030B88C
		public static UIElement GetLayoutExceptionElement(Dispatcher dispatcher)
		{
			if (dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}
			UIElement result = null;
			ContextLayoutManager contextLayoutManager = ContextLayoutManager.From(dispatcher);
			if (contextLayoutManager != null)
			{
				result = contextLayoutManager.GetLastExceptionElement();
			}
			return result;
		}
	}
}
