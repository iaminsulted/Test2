using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MS.Internal.AppModel;
using MS.Win32;

namespace MS.Internal.Ink
{
	// Token: 0x0200018D RID: 397
	internal static class PenCursorManager
	{
		// Token: 0x06000D3C RID: 3388 RVA: 0x00133DE4 File Offset: 0x00132DE4
		internal static Cursor GetPenCursor(DrawingAttributes drawingAttributes, bool isHollow, bool isRightToLeft, double dpiScaleX, double dpiScaleY)
		{
			return PenCursorManager.CreateCursorFromDrawing(PenCursorManager.CreatePenDrawing(drawingAttributes, isHollow, isRightToLeft, dpiScaleX, dpiScaleY), new Point(0.0, 0.0));
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00133E10 File Offset: 0x00132E10
		internal static Cursor GetPointEraserCursor(StylusShape stylusShape, Matrix tranform, double dpiScaleX, double dpiScaleY)
		{
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			if (stylusShape.GetType() == typeof(RectangleStylusShape))
			{
				drawingAttributes.StylusTip = StylusTip.Rectangle;
			}
			else
			{
				drawingAttributes.StylusTip = StylusTip.Ellipse;
			}
			drawingAttributes.Height = stylusShape.Height;
			drawingAttributes.Width = stylusShape.Width;
			drawingAttributes.Color = Colors.Black;
			if (!tranform.IsIdentity)
			{
				drawingAttributes.StylusTipTransform *= tranform;
			}
			if (!DoubleUtil.IsZero(stylusShape.Rotation))
			{
				Matrix identity = Matrix.Identity;
				identity.Rotate(stylusShape.Rotation);
				drawingAttributes.StylusTipTransform *= identity;
			}
			return PenCursorManager.GetPenCursor(drawingAttributes, true, false, dpiScaleX, dpiScaleY);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00133EC4 File Offset: 0x00132EC4
		internal static Cursor GetStrokeEraserCursor()
		{
			if (PenCursorManager.s_StrokeEraserCursor == null)
			{
				PenCursorManager.s_StrokeEraserCursor = PenCursorManager.CreateCursorFromDrawing(PenCursorManager.CreateStrokeEraserDrawing(), new Point(5.0, 5.0));
			}
			return PenCursorManager.s_StrokeEraserCursor;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00133EF8 File Offset: 0x00132EF8
		internal static Cursor GetSelectionCursor(InkCanvasSelectionHitResult hitResult, bool isRightToLeft)
		{
			Cursor result;
			switch (hitResult)
			{
			case InkCanvasSelectionHitResult.TopLeft:
			case InkCanvasSelectionHitResult.BottomRight:
				if (isRightToLeft)
				{
					result = Cursors.SizeNESW;
				}
				else
				{
					result = Cursors.SizeNWSE;
				}
				break;
			case InkCanvasSelectionHitResult.Top:
			case InkCanvasSelectionHitResult.Bottom:
				result = Cursors.SizeNS;
				break;
			case InkCanvasSelectionHitResult.TopRight:
			case InkCanvasSelectionHitResult.BottomLeft:
				if (isRightToLeft)
				{
					result = Cursors.SizeNWSE;
				}
				else
				{
					result = Cursors.SizeNESW;
				}
				break;
			case InkCanvasSelectionHitResult.Right:
			case InkCanvasSelectionHitResult.Left:
				result = Cursors.SizeWE;
				break;
			case InkCanvasSelectionHitResult.Selection:
				result = Cursors.SizeAll;
				break;
			default:
				result = Cursors.Cross;
				break;
			}
			return result;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00133F78 File Offset: 0x00132F78
		private static Cursor CreateCursorFromDrawing(Drawing drawing, Point hotspot)
		{
			Cursor arrow = Cursors.Arrow;
			Rect bounds = drawing.Bounds;
			double width = bounds.Width;
			double height = bounds.Height;
			int num = IconHelper.AlignToBytes(bounds.Width, 1);
			int num2 = IconHelper.AlignToBytes(bounds.Height, 1);
			bounds.Inflate(((double)num - width) / 2.0, ((double)num2 - height) / 2.0);
			int xHotspot = (int)Math.Round(hotspot.X - bounds.Left);
			int yHotspot = (int)Math.Round(hotspot.Y - bounds.Top);
			NativeMethods.IconHandle iconHandle = IconHelper.CreateIconCursor(PenCursorManager.GetPixels(PenCursorManager.RenderVisualToBitmap(PenCursorManager.CreateCursorDrawingVisual(drawing, num, num2), num, num2), num, num2), num, num2, xHotspot, yHotspot, false);
			if (iconHandle.IsInvalid)
			{
				return Cursors.Arrow;
			}
			return CursorInteropHelper.CriticalCreate(iconHandle);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00134050 File Offset: 0x00133050
		private static DrawingVisual CreateCursorDrawingVisual(Drawing drawing, int width, int height)
		{
			DrawingBrush drawingBrush = new DrawingBrush(drawing);
			drawingBrush.Stretch = Stretch.None;
			drawingBrush.AlignmentX = AlignmentX.Center;
			drawingBrush.AlignmentY = AlignmentY.Center;
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawRectangle(drawingBrush, null, new Rect(0.0, 0.0, (double)width, (double)height));
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			return drawingVisual;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x001340C8 File Offset: 0x001330C8
		private static RenderTargetBitmap RenderVisualToBitmap(Visual visual, int width, int height)
		{
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96.0, 96.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(visual);
			return renderTargetBitmap;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x001340F0 File Offset: 0x001330F0
		private static byte[] GetPixels(RenderTargetBitmap rtb, int width, int height)
		{
			int num = width * 4;
			FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap();
			formatConvertedBitmap.BeginInit();
			formatConvertedBitmap.Source = rtb;
			formatConvertedBitmap.DestinationFormat = PixelFormats.Bgra32;
			formatConvertedBitmap.EndInit();
			byte[] array = new byte[num * height];
			formatConvertedBitmap.CriticalCopyPixels(Int32Rect.Empty, array, num, 0);
			return array;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0013413C File Offset: 0x0013313C
		private static Drawing CreatePenDrawing(DrawingAttributes drawingAttributes, bool isHollow, bool isRightToLeft, double dpiScaleX, double dpiScaleY)
		{
			Stroke stroke = new Stroke(new StylusPointCollection
			{
				new StylusPoint(0.0, 0.0)
			}, new DrawingAttributes
			{
				Color = drawingAttributes.Color,
				Width = drawingAttributes.Width,
				Height = drawingAttributes.Height,
				StylusTipTransform = drawingAttributes.StylusTipTransform,
				IsHighlighter = drawingAttributes.IsHighlighter,
				StylusTip = drawingAttributes.StylusTip
			});
			stroke.DrawingAttributes.Width = PenCursorManager.ConvertToPixel(stroke.DrawingAttributes.Width, dpiScaleX);
			stroke.DrawingAttributes.Height = PenCursorManager.ConvertToPixel(stroke.DrawingAttributes.Height, dpiScaleY);
			double num = Math.Min(SystemParameters.PrimaryScreenWidth / 2.0, SystemParameters.PrimaryScreenHeight / 2.0);
			Rect bounds = stroke.GetBounds();
			bool flag = false;
			if (DoubleUtil.LessThan(bounds.Width, 1.0))
			{
				stroke.DrawingAttributes.Width = 1.0;
				flag = true;
			}
			else if (DoubleUtil.GreaterThan(bounds.Width, num))
			{
				stroke.DrawingAttributes.Width = num;
				flag = true;
			}
			if (DoubleUtil.LessThan(bounds.Height, 1.0))
			{
				stroke.DrawingAttributes.Height = 1.0;
				flag = true;
			}
			else if (DoubleUtil.GreaterThan(bounds.Height, num))
			{
				stroke.DrawingAttributes.Height = num;
				flag = true;
			}
			if (flag)
			{
				stroke.DrawingAttributes.StylusTipTransform = Matrix.Identity;
			}
			if (isRightToLeft)
			{
				Matrix stylusTipTransform = stroke.DrawingAttributes.StylusTipTransform;
				stylusTipTransform.Scale(-1.0, 1.0);
				if (stylusTipTransform.HasInverse)
				{
					stroke.DrawingAttributes.StylusTipTransform = stylusTipTransform;
				}
			}
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingGroup.Open();
				if (isHollow)
				{
					stroke.DrawInternal(drawingContext, stroke.DrawingAttributes, isHollow);
				}
				else
				{
					stroke.Draw(drawingContext, stroke.DrawingAttributes);
				}
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			return drawingGroup;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0013436C File Offset: 0x0013336C
		private static Drawing CreateStrokeEraserDrawing()
		{
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingGroup.Open();
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(Color.FromRgb(240, 242, byte.MaxValue), Color.FromRgb(180, 207, 248), 45.0);
				linearGradientBrush.Freeze();
				SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(180, 207, 248));
				solidColorBrush.Freeze();
				Pen pen = new Pen(Brushes.Gray, 0.7);
				pen.Freeze();
				PathGeometry pathGeometry = new PathGeometry();
				PathFigure pathFigure = new PathFigure();
				pathFigure.StartPoint = new Point(5.0, 5.0);
				LineSegment lineSegment = new LineSegment(new Point(16.0, 5.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(26.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(15.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(5.0, 5.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				pathFigure.IsClosed = true;
				pathFigure.Freeze();
				pathGeometry.Figures.Add(pathFigure);
				pathFigure = new PathFigure();
				pathFigure.StartPoint = new Point(5.0, 5.0);
				lineSegment = new LineSegment(new Point(5.0, 10.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(15.0, 19.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(15.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(5.0, 5.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				pathFigure.IsClosed = true;
				pathFigure.Freeze();
				pathGeometry.Figures.Add(pathFigure);
				pathGeometry.Freeze();
				PathGeometry pathGeometry2 = new PathGeometry();
				pathFigure = new PathFigure();
				pathFigure.StartPoint = new Point(15.0, 15.0);
				lineSegment = new LineSegment(new Point(15.0, 19.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(26.0, 19.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(26.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment.Freeze();
				lineSegment = new LineSegment(new Point(15.0, 15.0), true);
				pathFigure.Segments.Add(lineSegment);
				pathFigure.IsClosed = true;
				pathFigure.Freeze();
				pathGeometry2.Figures.Add(pathFigure);
				pathGeometry2.Freeze();
				drawingContext.DrawGeometry(linearGradientBrush, pen, pathGeometry);
				drawingContext.DrawGeometry(solidColorBrush, pen, pathGeometry2);
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(5.0, 0.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(0.0, 5.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(2.0, 2.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(8.0, 2.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(2.0, 8.0));
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			return drawingGroup;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x001348A0 File Offset: 0x001338A0
		private static double ConvertToPixel(double value, double dpiScale)
		{
			if (dpiScale != 0.0)
			{
				return value * dpiScale;
			}
			return value;
		}

		// Token: 0x040009D6 RID: 2518
		private static Cursor s_StrokeEraserCursor;
	}
}
