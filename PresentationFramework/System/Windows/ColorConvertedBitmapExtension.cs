using System;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Windows
{
	// Token: 0x02000347 RID: 839
	[MarkupExtensionReturnType(typeof(ColorConvertedBitmap))]
	public class ColorConvertedBitmapExtension : MarkupExtension
	{
		// Token: 0x06001FE4 RID: 8164 RVA: 0x00173A19 File Offset: 0x00172A19
		public ColorConvertedBitmapExtension()
		{
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00173A24 File Offset: 0x00172A24
		public ColorConvertedBitmapExtension(object image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			foreach (string text in ((string)image).Split(new char[]
			{
				' '
			}))
			{
				if (text.Length > 0)
				{
					if (this._image == null)
					{
						this._image = text;
					}
					else if (this._sourceProfile == null)
					{
						this._sourceProfile = text;
					}
					else
					{
						if (this._destinationProfile != null)
						{
							throw new InvalidOperationException(SR.Get("ColorConvertedBitmapExtensionSyntax"));
						}
						this._destinationProfile = text;
					}
				}
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00173ABC File Offset: 0x00172ABC
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this._image == null)
			{
				throw new InvalidOperationException(SR.Get("ColorConvertedBitmapExtensionNoSourceImage"));
			}
			if (this._sourceProfile == null)
			{
				throw new InvalidOperationException(SR.Get("ColorConvertedBitmapExtensionNoSourceProfile"));
			}
			IUriContext uriContext = serviceProvider.GetService(typeof(IUriContext)) as IUriContext;
			if (uriContext == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IUriContext"
				}));
			}
			this._baseUri = uriContext.BaseUri;
			Uri resolvedUri = this.GetResolvedUri(this._image);
			Uri resolvedUri2 = this.GetResolvedUri(this._sourceProfile);
			Uri resolvedUri3 = this.GetResolvedUri(this._destinationProfile);
			ColorContext sourceColorContext = new ColorContext(resolvedUri2);
			ColorContext destinationColorContext = (resolvedUri3 != null) ? new ColorContext(resolvedUri3) : new ColorContext(PixelFormats.Default);
			FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap(BitmapDecoder.Create(resolvedUri, BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None).Frames[0], PixelFormats.Bgra32, null, 0.0);
			object result = formatConvertedBitmap;
			try
			{
				result = new ColorConvertedBitmap(formatConvertedBitmap, sourceColorContext, destinationColorContext, PixelFormats.Bgra32);
			}
			catch (FileFormatException)
			{
			}
			return result;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x00173BE8 File Offset: 0x00172BE8
		private Uri GetResolvedUri(string uri)
		{
			if (uri == null)
			{
				return null;
			}
			return new Uri(this._baseUri, uri);
		}

		// Token: 0x04000FA5 RID: 4005
		private string _image;

		// Token: 0x04000FA6 RID: 4006
		private string _sourceProfile;

		// Token: 0x04000FA7 RID: 4007
		private Uri _baseUri;

		// Token: 0x04000FA8 RID: 4008
		private string _destinationProfile;
	}
}
