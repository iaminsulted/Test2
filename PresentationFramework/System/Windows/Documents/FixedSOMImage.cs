using System;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x0200060A RID: 1546
	internal sealed class FixedSOMImage : FixedSOMElement
	{
		// Token: 0x06004B4C RID: 19276 RVA: 0x002369D0 File Offset: 0x002359D0
		private FixedSOMImage(Rect imageRect, GeneralTransform trans, Uri sourceUri, FixedNode node, DependencyObject o) : base(node, trans)
		{
			this._boundingRect = trans.TransformBounds(imageRect);
			this._source = sourceUri;
			this._startIndex = 0;
			this._endIndex = 1;
			this._name = AutomationProperties.GetName(o);
			this._helpText = AutomationProperties.GetHelpText(o);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00236A24 File Offset: 0x00235A24
		public static FixedSOMImage Create(FixedPage page, Image image, FixedNode fixedNode)
		{
			Uri sourceUri = null;
			if (image.Source is BitmapImage)
			{
				sourceUri = (image.Source as BitmapImage).UriSource;
			}
			else if (image.Source is BitmapFrame)
			{
				sourceUri = new Uri((image.Source as BitmapFrame).ToString(), UriKind.RelativeOrAbsolute);
			}
			Rect imageRect = new Rect(image.RenderSize);
			GeneralTransform trans = image.TransformToAncestor(page);
			return new FixedSOMImage(imageRect, trans, sourceUri, fixedNode, image);
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x00236A94 File Offset: 0x00235A94
		public static FixedSOMImage Create(FixedPage page, Path path, FixedNode fixedNode)
		{
			ImageSource imageSource = ((ImageBrush)path.Fill).ImageSource;
			Uri sourceUri = null;
			if (imageSource is BitmapImage)
			{
				sourceUri = (imageSource as BitmapImage).UriSource;
			}
			else if (imageSource is BitmapFrame)
			{
				sourceUri = new Uri((imageSource as BitmapFrame).ToString(), UriKind.RelativeOrAbsolute);
			}
			Rect bounds = path.Data.Bounds;
			GeneralTransform trans = path.TransformToAncestor(page);
			return new FixedSOMImage(bounds, trans, sourceUri, fixedNode, path);
		}

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06004B4F RID: 19279 RVA: 0x00236B00 File Offset: 0x00235B00
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x06004B50 RID: 19280 RVA: 0x00236B08 File Offset: 0x00235B08
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x00236B10 File Offset: 0x00235B10
		internal string HelpText
		{
			get
			{
				return this._helpText;
			}
		}

		// Token: 0x04002777 RID: 10103
		private Uri _source;

		// Token: 0x04002778 RID: 10104
		private string _name;

		// Token: 0x04002779 RID: 10105
		private string _helpText;
	}
}
