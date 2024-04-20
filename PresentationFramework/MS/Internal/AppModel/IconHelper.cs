using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x02000277 RID: 631
	internal static class IconHelper
	{
		// Token: 0x06001825 RID: 6181 RVA: 0x00160440 File Offset: 0x0015F440
		private static void EnsureSystemMetrics()
		{
			if (IconHelper.s_systemBitDepth == 0)
			{
				HandleRef hDC = new HandleRef(null, UnsafeNativeMethods.GetDC(default(HandleRef)));
				try
				{
					int num = UnsafeNativeMethods.GetDeviceCaps(hDC, 12);
					num *= UnsafeNativeMethods.GetDeviceCaps(hDC, 14);
					if (num == 8)
					{
						num = 4;
					}
					int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(SM.CXSMICON);
					int systemMetrics2 = UnsafeNativeMethods.GetSystemMetrics(SM.CYSMICON);
					int systemMetrics3 = UnsafeNativeMethods.GetSystemMetrics(SM.CXICON);
					int systemMetrics4 = UnsafeNativeMethods.GetSystemMetrics(SM.CYICON);
					IconHelper.s_smallIconSize = new Size((double)systemMetrics, (double)systemMetrics2);
					IconHelper.s_iconSize = new Size((double)systemMetrics3, (double)systemMetrics4);
					IconHelper.s_systemBitDepth = num;
				}
				finally
				{
					UnsafeNativeMethods.ReleaseDC(default(HandleRef), hDC);
				}
			}
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x001604F4 File Offset: 0x0015F4F4
		public static void GetDefaultIconHandles(out NativeMethods.IconHandle largeIconHandle, out NativeMethods.IconHandle smallIconHandle)
		{
			largeIconHandle = null;
			smallIconHandle = null;
			UnsafeNativeMethods.ExtractIconEx(UnsafeNativeMethods.GetModuleFileName(default(HandleRef)), 0, out largeIconHandle, out smallIconHandle, 1);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0016051F File Offset: 0x0015F51F
		public static void GetIconHandlesFromImageSource(ImageSource image, out NativeMethods.IconHandle largeIconHandle, out NativeMethods.IconHandle smallIconHandle)
		{
			IconHelper.EnsureSystemMetrics();
			largeIconHandle = IconHelper.CreateIconHandleFromImageSource(image, IconHelper.s_iconSize);
			smallIconHandle = IconHelper.CreateIconHandleFromImageSource(image, IconHelper.s_smallIconSize);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00160540 File Offset: 0x0015F540
		public static NativeMethods.IconHandle CreateIconHandleFromImageSource(ImageSource image, Size size)
		{
			IconHelper.EnsureSystemMetrics();
			bool flag = false;
			BitmapFrame bitmapFrame = image as BitmapFrame;
			bool flag2;
			if (bitmapFrame == null)
			{
				flag2 = (null != null);
			}
			else
			{
				BitmapDecoder decoder = bitmapFrame.Decoder;
				flag2 = (((decoder != null) ? decoder.Frames : null) != null);
			}
			if (flag2)
			{
				bitmapFrame = IconHelper.GetBestMatch(bitmapFrame.Decoder.Frames, size);
				flag = (bitmapFrame.Decoder is IconBitmapDecoder || ((double)bitmapFrame.PixelWidth == size.Width && (double)bitmapFrame.PixelHeight == size.Height));
				image = bitmapFrame;
			}
			if (!flag)
			{
				bitmapFrame = BitmapFrame.Create(IconHelper.GenerateBitmapSource(image, size));
			}
			return IconHelper.CreateIconHandleFromBitmapFrame(bitmapFrame);
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x001605D4 File Offset: 0x0015F5D4
		private static BitmapSource GenerateBitmapSource(ImageSource img, Size renderSize)
		{
			Rect rectangle = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
			double num = renderSize.Width / renderSize.Height;
			double num2 = img.Width / img.Height;
			if (img.Width <= renderSize.Width && img.Height <= renderSize.Height)
			{
				rectangle = new Rect((renderSize.Width - img.Width) / 2.0, (renderSize.Height - img.Height) / 2.0, img.Width, img.Height);
			}
			else if (num > num2)
			{
				double num3 = img.Width / img.Height * renderSize.Width;
				rectangle = new Rect((renderSize.Width - num3) / 2.0, 0.0, num3, renderSize.Height);
			}
			else if (num < num2)
			{
				double num4 = img.Height / img.Width * renderSize.Height;
				rectangle = new Rect(0.0, (renderSize.Height - num4) / 2.0, renderSize.Width, num4);
			}
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawImage(img, rectangle);
			drawingContext.Close();
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)renderSize.Width, (int)renderSize.Height, 96.0, 96.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0016076C File Offset: 0x0015F76C
		private static NativeMethods.IconHandle CreateIconHandleFromBitmapFrame(BitmapFrame sourceBitmapFrame)
		{
			Invariant.Assert(sourceBitmapFrame != null, "sourceBitmapFrame cannot be null here");
			BitmapSource bitmapSource = sourceBitmapFrame;
			if (bitmapSource.Format != PixelFormats.Bgra32 && bitmapSource.Format != PixelFormats.Pbgra32)
			{
				bitmapSource = new FormatConvertedBitmap(bitmapSource, PixelFormats.Bgra32, null, 0.0);
			}
			int pixelWidth = bitmapSource.PixelWidth;
			int pixelHeight = bitmapSource.PixelHeight;
			int num = (bitmapSource.Format.BitsPerPixel * pixelWidth + 31) / 32 * 4;
			byte[] array = new byte[num * pixelHeight];
			bitmapSource.CopyPixels(array, num, 0);
			return IconHelper.CreateIconCursor(array, pixelWidth, pixelHeight, 0, 0, true);
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0016080C File Offset: 0x0015F80C
		internal static NativeMethods.IconHandle CreateIconCursor(byte[] colorArray, int width, int height, int xHotspot, int yHotspot, bool isIcon)
		{
			NativeMethods.BitmapHandle bitmapHandle = null;
			NativeMethods.BitmapHandle bitmapHandle2 = null;
			NativeMethods.IconHandle result;
			try
			{
				NativeMethods.BITMAPINFO bitmapinfo = new NativeMethods.BITMAPINFO(width, -height, 32);
				bitmapinfo.bmiHeader_biCompression = 0;
				IntPtr zero = IntPtr.Zero;
				bitmapHandle = UnsafeNativeMethods.CreateDIBSection(new HandleRef(null, IntPtr.Zero), ref bitmapinfo, 0, ref zero, null, 0);
				if (bitmapHandle.IsInvalid || zero == IntPtr.Zero)
				{
					result = NativeMethods.IconHandle.GetInvalidIcon();
				}
				else
				{
					Marshal.Copy(colorArray, 0, zero, colorArray.Length);
					byte[] array = IconHelper.GenerateMaskArray(width, height, colorArray);
					Invariant.Assert(array != null);
					bitmapHandle2 = UnsafeNativeMethods.CreateBitmap(width, height, 1, 1, array);
					if (bitmapHandle2.IsInvalid)
					{
						result = NativeMethods.IconHandle.GetInvalidIcon();
					}
					else
					{
						result = UnsafeNativeMethods.CreateIconIndirect(new NativeMethods.ICONINFO
						{
							fIcon = isIcon,
							xHotspot = xHotspot,
							yHotspot = yHotspot,
							hbmMask = bitmapHandle2,
							hbmColor = bitmapHandle
						});
					}
				}
			}
			finally
			{
				if (bitmapHandle != null)
				{
					bitmapHandle.Dispose();
					bitmapHandle = null;
				}
				if (bitmapHandle2 != null)
				{
					bitmapHandle2.Dispose();
					bitmapHandle2 = null;
				}
			}
			return result;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00160908 File Offset: 0x0015F908
		private static byte[] GenerateMaskArray(int width, int height, byte[] colorArray)
		{
			int num = width * height;
			int num2 = IconHelper.AlignToBytes((double)width, 2) / 8;
			byte[] array = new byte[num2 * height];
			for (int i = 0; i < num; i++)
			{
				int num3 = i % width;
				int num4 = i / width;
				int num5 = num3 / 8;
				byte b = (byte)(128 >> num3 % 8);
				if (colorArray[i * 4 + 3] == 0)
				{
					byte[] array2 = array;
					int num6 = num5 + num2 * num4;
					array2[num6] |= b;
				}
				else
				{
					byte[] array3 = array;
					int num7 = num5 + num2 * num4;
					array3[num7] &= ~b;
				}
				if (num3 == width - 1 && width == 8)
				{
					array[1 + num2 * num4] = byte.MaxValue;
				}
			}
			return array;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x001609A8 File Offset: 0x0015F9A8
		internal static int AlignToBytes(double original, int nBytesCount)
		{
			int num = 8 << nBytesCount - 1;
			return ((int)Math.Ceiling(original) + (num - 1)) / num * num;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x001609CD File Offset: 0x0015F9CD
		private static int MatchImage(BitmapFrame frame, Size size, int bpp)
		{
			return 2 * IconHelper.MyAbs(bpp, IconHelper.s_systemBitDepth, false) + IconHelper.MyAbs(frame.PixelWidth, (int)size.Width, true) + IconHelper.MyAbs(frame.PixelHeight, (int)size.Height, true);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000F882C File Offset: 0x000F782C
		private static int MyAbs(int valueHave, int valueWant, bool fPunish)
		{
			int num = valueHave - valueWant;
			if (num < 0)
			{
				num = (fPunish ? -2 : -1) * num;
			}
			return num;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00160A08 File Offset: 0x0015FA08
		private static BitmapFrame GetBestMatch(ReadOnlyCollection<BitmapFrame> frames, Size size)
		{
			Invariant.Assert(size.Width != 0.0, "input param width should not be zero");
			Invariant.Assert(size.Height != 0.0, "input param height should not be zero");
			int num = int.MaxValue;
			int num2 = 0;
			int index = 0;
			bool flag = frames[0].Decoder is IconBitmapDecoder;
			int num3 = 0;
			while (num3 < frames.Count && num != 0)
			{
				int num4 = flag ? frames[num3].Thumbnail.Format.BitsPerPixel : frames[num3].Format.BitsPerPixel;
				if (num4 == 0)
				{
					num4 = 8;
				}
				int num5 = IconHelper.MatchImage(frames[num3], size, num4);
				if (num5 < num)
				{
					index = num3;
					num2 = num4;
					num = num5;
				}
				else if (num5 == num && num2 < num4)
				{
					index = num3;
					num2 = num4;
				}
				num3++;
			}
			return frames[index];
		}

		// Token: 0x04000D3C RID: 3388
		private static Size s_smallIconSize;

		// Token: 0x04000D3D RID: 3389
		private static Size s_iconSize;

		// Token: 0x04000D3E RID: 3390
		private static int s_systemBitDepth;
	}
}
