using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;
using MS.Internal.IO.Packaging;

namespace System.Windows.Documents
{
	// Token: 0x020006DF RID: 1759
	internal class WpfPayload
	{
		// Token: 0x06005C8D RID: 23693 RVA: 0x002874B4 File Offset: 0x002864B4
		private WpfPayload(Package package)
		{
			this._package = package;
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x002874C3 File Offset: 0x002864C3
		internal static string SaveRange(ITextRange range, ref Stream stream, bool useFlowDocumentAsRoot)
		{
			return WpfPayload.SaveRange(range, ref stream, useFlowDocumentAsRoot, false);
		}

		// Token: 0x06005C8F RID: 23695 RVA: 0x002874D0 File Offset: 0x002864D0
		internal static string SaveRange(ITextRange range, ref Stream stream, bool useFlowDocumentAsRoot, bool preserveTextElements)
		{
			if (range == null)
			{
				throw new ArgumentNullException("range");
			}
			WpfPayload wpfPayload = new WpfPayload(null);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			TextRangeSerialization.WriteXaml(new XmlTextWriter(stringWriter), range, useFlowDocumentAsRoot, wpfPayload, preserveTextElements);
			string text = stringWriter.ToString();
			if (stream != null || wpfPayload._images != null)
			{
				if (stream == null)
				{
					stream = new MemoryStream();
				}
				using (wpfPayload.CreatePackage(stream))
				{
					PackagePart packagePart = wpfPayload.CreateWpfEntryPart();
					Stream seekableStream = packagePart.GetSeekableStream();
					using (seekableStream)
					{
						StreamWriter streamWriter = new StreamWriter(seekableStream);
						using (streamWriter)
						{
							streamWriter.Write(text);
						}
					}
					wpfPayload.CreateComponentParts(packagePart);
				}
				Invariant.Assert(wpfPayload._images == null);
			}
			return text;
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x002875C0 File Offset: 0x002865C0
		internal static MemoryStream SaveImage(BitmapSource bitmapSource, string imageContentType)
		{
			MemoryStream memoryStream = new MemoryStream();
			WpfPayload wpfPayload = new WpfPayload(null);
			using (wpfPayload.CreatePackage(memoryStream))
			{
				int imageIndex = 0;
				string imageReference = WpfPayload.GetImageReference(WpfPayload.GetImageName(imageIndex, imageContentType));
				PackagePart packagePart = wpfPayload.CreateWpfEntryPart();
				Stream seekableStream = packagePart.GetSeekableStream();
				using (seekableStream)
				{
					StreamWriter streamWriter = new StreamWriter(seekableStream);
					using (streamWriter)
					{
						string value = string.Concat(new string[]
						{
							"<Span xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><InlineUIContainer><Image Width=\"",
							bitmapSource.Width.ToString(),
							"\" Height=\"",
							bitmapSource.Height.ToString(),
							"\" ><Image.Source><BitmapImage CacheOption=\"OnLoad\" UriSource=\"",
							imageReference,
							"\"/></Image.Source></Image></InlineUIContainer></Span>"
						});
						streamWriter.Write(value);
					}
				}
				wpfPayload.CreateImagePart(packagePart, bitmapSource, imageContentType, imageIndex);
			}
			return memoryStream;
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x002876D0 File Offset: 0x002866D0
		internal static object LoadElement(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			object result;
			try
			{
				WpfPayload wpfPayload = WpfPayload.OpenWpfPayload(stream);
				using (wpfPayload.Package)
				{
					PackagePart packagePart = wpfPayload.ValidatePayload();
					Uri uri = PackUriHelper.Create(new Uri("payload://wpf" + Interlocked.Increment(ref WpfPayload._wpfPayloadCount).ToString(), UriKind.Absolute), packagePart.Uri);
					Uri packageUri = PackUriHelper.GetPackageUri(uri);
					PackageStore.AddPackage(packageUri, wpfPayload.Package);
					ParserContext parserContext = new ParserContext();
					parserContext.BaseUri = uri;
					result = XamlReader.Load(packagePart.GetSeekableStream(), parserContext, true);
					PackageStore.RemovePackage(packageUri);
				}
			}
			catch (XamlParseException ex)
			{
				Invariant.Assert(ex != null);
				result = null;
			}
			catch (FileFormatException)
			{
				result = null;
			}
			catch (FileLoadException)
			{
				result = null;
			}
			catch (OutOfMemoryException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x002877D4 File Offset: 0x002867D4
		private PackagePart ValidatePayload()
		{
			PackagePart wpfEntryPart = this.GetWpfEntryPart();
			if (wpfEntryPart == null)
			{
				throw new XamlParseException(SR.Get("TextEditorCopyPaste_EntryPartIsMissingInXamlPackage"));
			}
			return wpfEntryPart;
		}

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x06005C93 RID: 23699 RVA: 0x002877EF File Offset: 0x002867EF
		public Package Package
		{
			get
			{
				return this._package;
			}
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x002877F8 File Offset: 0x002867F8
		private BitmapSource GetBitmapSourceFromImage(Image image)
		{
			if (image.Source is BitmapSource)
			{
				return (BitmapSource)image.Source;
			}
			Invariant.Assert(image.Source is DrawingImage);
			DpiScale dpi = image.GetDpi();
			DrawingImage drawingImage = (DrawingImage)image.Source;
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(drawingImage.Width * dpi.DpiScaleX), (int)(drawingImage.Height * dpi.DpiScaleY), 96.0, 96.0, PixelFormats.Default);
			renderTargetBitmap.Render(image);
			return renderTargetBitmap;
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x00287888 File Offset: 0x00286888
		private void CreateComponentParts(PackagePart sourcePart)
		{
			if (this._images != null)
			{
				for (int i = 0; i < this._images.Count; i++)
				{
					Image image = this._images[i];
					string imageContentType = WpfPayload.GetImageContentType(image.Source.ToString());
					this.CreateImagePart(sourcePart, this.GetBitmapSourceFromImage(image), imageContentType, i);
				}
				this._images = null;
			}
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x002878E8 File Offset: 0x002868E8
		private void CreateImagePart(PackagePart sourcePart, BitmapSource imageSource, string imageContentType, int imageIndex)
		{
			string imageName = WpfPayload.GetImageName(imageIndex, imageContentType);
			Uri uri = new Uri("/Xaml" + imageName, UriKind.Relative);
			PackagePart packPart = this._package.CreatePart(uri, imageContentType, CompressionOption.NotCompressed);
			sourcePart.CreateRelationship(uri, TargetMode.Internal, "http://schemas.microsoft.com/wpf/2005/10/xaml/component");
			BitmapEncoder bitmapEncoder = WpfPayload.GetBitmapEncoder(imageContentType);
			bitmapEncoder.Frames.Add(BitmapFrame.Create(imageSource));
			Stream seekableStream = packPart.GetSeekableStream();
			using (seekableStream)
			{
				bitmapEncoder.Save(seekableStream);
			}
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x00287974 File Offset: 0x00286974
		internal string AddImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (image.Source == null)
			{
				throw new ArgumentNullException("image.Source");
			}
			if (string.IsNullOrEmpty(image.Source.ToString()))
			{
				throw new ArgumentException(SR.Get("WpfPayload_InvalidImageSource"));
			}
			if (this._images == null)
			{
				this._images = new List<Image>();
			}
			string text = null;
			string imageContentType = WpfPayload.GetImageContentType(image.Source.ToString());
			for (int i = 0; i < this._images.Count; i++)
			{
				if (WpfPayload.ImagesAreIdentical(this.GetBitmapSourceFromImage(this._images[i]), this.GetBitmapSourceFromImage(image)))
				{
					Invariant.Assert(imageContentType == WpfPayload.GetImageContentType(this._images[i].Source.ToString()), "Image content types expected to be consistent: " + imageContentType + " vs. " + WpfPayload.GetImageContentType(this._images[i].Source.ToString()));
					text = WpfPayload.GetImageName(i, imageContentType);
				}
			}
			if (text == null)
			{
				text = WpfPayload.GetImageName(this._images.Count, imageContentType);
				this._images.Add(image);
			}
			return WpfPayload.GetImageReference(text);
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x00287AA4 File Offset: 0x00286AA4
		private static string GetImageContentType(string imageUriString)
		{
			string result;
			if (imageUriString.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/bmp";
			}
			else if (imageUriString.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/gif";
			}
			else if (imageUriString.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || imageUriString.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/jpeg";
			}
			else if (imageUriString.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/tiff";
			}
			else
			{
				result = "image/png";
			}
			return result;
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x00287B20 File Offset: 0x00286B20
		private static BitmapEncoder GetBitmapEncoder(string imageContentType)
		{
			BitmapEncoder result;
			if (!(imageContentType == "image/bmp"))
			{
				if (!(imageContentType == "image/gif"))
				{
					if (!(imageContentType == "image/jpeg"))
					{
						if (!(imageContentType == "image/tiff"))
						{
							if (!(imageContentType == "image/png"))
							{
								Invariant.Assert(false, "Unexpected image content type: " + imageContentType);
								result = null;
							}
							else
							{
								result = new PngBitmapEncoder();
							}
						}
						else
						{
							result = new TiffBitmapEncoder();
						}
					}
					else
					{
						result = new JpegBitmapEncoder();
					}
				}
				else
				{
					result = new GifBitmapEncoder();
				}
			}
			else
			{
				result = new BmpBitmapEncoder();
			}
			return result;
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x00287BAC File Offset: 0x00286BAC
		private static string GetImageFileExtension(string imageContentType)
		{
			string result;
			if (!(imageContentType == "image/bmp"))
			{
				if (!(imageContentType == "image/gif"))
				{
					if (!(imageContentType == "image/jpeg"))
					{
						if (!(imageContentType == "image/tiff"))
						{
							if (!(imageContentType == "image/png"))
							{
								Invariant.Assert(false, "Unexpected image content type: " + imageContentType);
								result = null;
							}
							else
							{
								result = ".png";
							}
						}
						else
						{
							result = ".tiff";
						}
					}
					else
					{
						result = ".jpeg";
					}
				}
				else
				{
					result = ".gif";
				}
			}
			else
			{
				result = ".bmp";
			}
			return result;
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x00287C38 File Offset: 0x00286C38
		private static bool ImagesAreIdentical(BitmapSource imageSource1, BitmapSource imageSource2)
		{
			BitmapFrameDecode bitmapFrameDecode = imageSource1 as BitmapFrameDecode;
			BitmapFrameDecode bitmapFrameDecode2 = imageSource2 as BitmapFrameDecode;
			if (bitmapFrameDecode != null && bitmapFrameDecode2 != null && bitmapFrameDecode.Decoder.Frames.Count == 1 && bitmapFrameDecode2.Decoder.Frames.Count == 1 && bitmapFrameDecode.Decoder.Frames[0] == bitmapFrameDecode2.Decoder.Frames[0])
			{
				return true;
			}
			if (imageSource1.Format.BitsPerPixel != imageSource2.Format.BitsPerPixel || imageSource1.PixelWidth != imageSource2.PixelWidth || imageSource1.PixelHeight != imageSource2.PixelHeight || imageSource1.DpiX != imageSource2.DpiX || imageSource1.DpiY != imageSource2.DpiY || imageSource1.Palette != imageSource2.Palette)
			{
				return false;
			}
			int num = (imageSource1.PixelWidth * imageSource1.Format.BitsPerPixel + 7) / 8;
			int num2 = num * (imageSource1.PixelHeight - 1) + num;
			byte[] array = new byte[num2];
			byte[] array2 = new byte[num2];
			imageSource1.CopyPixels(array, num, 0);
			imageSource2.CopyPixels(array2, num, 0);
			for (int i = 0; i < num2; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005C9C RID: 23708 RVA: 0x00287D78 File Offset: 0x00286D78
		internal Stream CreateXamlStream()
		{
			return this.CreateWpfEntryPart().GetSeekableStream();
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x00287D88 File Offset: 0x00286D88
		internal Stream CreateImageStream(int imageCount, string contentType, out string imagePartUriString)
		{
			imagePartUriString = WpfPayload.GetImageName(imageCount, contentType);
			Uri partUri = new Uri("/Xaml" + imagePartUriString, UriKind.Relative);
			PackagePart packPart = this._package.CreatePart(partUri, contentType, CompressionOption.NotCompressed);
			imagePartUriString = WpfPayload.GetImageReference(imagePartUriString);
			return packPart.GetSeekableStream();
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x00287DD0 File Offset: 0x00286DD0
		internal Stream GetImageStream(string imageSourceString)
		{
			Invariant.Assert(imageSourceString.StartsWith("./", StringComparison.OrdinalIgnoreCase));
			imageSourceString = imageSourceString.Substring(1);
			Uri partUri = new Uri("/Xaml" + imageSourceString, UriKind.Relative);
			return this._package.GetPart(partUri).GetSeekableStream();
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x00287E1A File Offset: 0x00286E1A
		private Package CreatePackage(Stream stream)
		{
			Invariant.Assert(this._package == null, "Package has been already created or open for this WpfPayload");
			this._package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
			return this._package;
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x00287E43 File Offset: 0x00286E43
		internal static WpfPayload CreateWpfPayload(Stream stream)
		{
			return new WpfPayload(Package.Open(stream, FileMode.Create, FileAccess.ReadWrite));
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x00287E52 File Offset: 0x00286E52
		internal static WpfPayload OpenWpfPayload(Stream stream)
		{
			return new WpfPayload(Package.Open(stream, FileMode.Open, FileAccess.Read));
		}

		// Token: 0x06005CA2 RID: 23714 RVA: 0x00287E64 File Offset: 0x00286E64
		private PackagePart CreateWpfEntryPart()
		{
			Uri uri = new Uri("/Xaml/Document.xaml", UriKind.Relative);
			PackagePart result = this._package.CreatePart(uri, "application/vnd.ms-wpf.xaml+xml", CompressionOption.Normal);
			this._package.CreateRelationship(uri, TargetMode.Internal, "http://schemas.microsoft.com/wpf/2005/10/xaml/entry");
			return result;
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x00287EA4 File Offset: 0x00286EA4
		private PackagePart GetWpfEntryPart()
		{
			PackagePart result = null;
			PackageRelationshipCollection relationshipsByType = this._package.GetRelationshipsByType("http://schemas.microsoft.com/wpf/2005/10/xaml/entry");
			PackageRelationship packageRelationship = null;
			using (IEnumerator<PackageRelationship> enumerator = relationshipsByType.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					packageRelationship = enumerator.Current;
				}
			}
			if (packageRelationship != null)
			{
				Uri targetUri = packageRelationship.TargetUri;
				result = this._package.GetPart(targetUri);
			}
			return result;
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x00287F14 File Offset: 0x00286F14
		private static string GetImageName(int imageIndex, string imageContentType)
		{
			string imageFileExtension = WpfPayload.GetImageFileExtension(imageContentType);
			return "/Image" + (imageIndex + 1).ToString() + imageFileExtension;
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x00287F3E File Offset: 0x00286F3E
		private static string GetImageReference(string imageName)
		{
			return "." + imageName;
		}

		// Token: 0x040030D7 RID: 12503
		private const string XamlContentType = "application/vnd.ms-wpf.xaml+xml";

		// Token: 0x040030D8 RID: 12504
		internal const string ImageBmpContentType = "image/bmp";

		// Token: 0x040030D9 RID: 12505
		private const string ImageGifContentType = "image/gif";

		// Token: 0x040030DA RID: 12506
		private const string ImageJpegContentType = "image/jpeg";

		// Token: 0x040030DB RID: 12507
		private const string ImageTiffContentType = "image/tiff";

		// Token: 0x040030DC RID: 12508
		private const string ImagePngContentType = "image/png";

		// Token: 0x040030DD RID: 12509
		private const string ImageBmpFileExtension = ".bmp";

		// Token: 0x040030DE RID: 12510
		private const string ImageGifFileExtension = ".gif";

		// Token: 0x040030DF RID: 12511
		private const string ImageJpegFileExtension = ".jpeg";

		// Token: 0x040030E0 RID: 12512
		private const string ImageJpgFileExtension = ".jpg";

		// Token: 0x040030E1 RID: 12513
		private const string ImageTiffFileExtension = ".tiff";

		// Token: 0x040030E2 RID: 12514
		private const string ImagePngFileExtension = ".png";

		// Token: 0x040030E3 RID: 12515
		private const string XamlRelationshipFromPackageToEntryPart = "http://schemas.microsoft.com/wpf/2005/10/xaml/entry";

		// Token: 0x040030E4 RID: 12516
		private const string XamlRelationshipFromXamlPartToComponentPart = "http://schemas.microsoft.com/wpf/2005/10/xaml/component";

		// Token: 0x040030E5 RID: 12517
		private const string XamlPayloadDirectory = "/Xaml";

		// Token: 0x040030E6 RID: 12518
		private const string XamlEntryName = "/Document.xaml";

		// Token: 0x040030E7 RID: 12519
		private const string XamlImageName = "/Image";

		// Token: 0x040030E8 RID: 12520
		private static int _wpfPayloadCount;

		// Token: 0x040030E9 RID: 12521
		private Package _package;

		// Token: 0x040030EA RID: 12522
		private List<Image> _images;
	}
}
