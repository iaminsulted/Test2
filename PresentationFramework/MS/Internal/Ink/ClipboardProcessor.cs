using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace MS.Internal.Ink
{
	// Token: 0x02000180 RID: 384
	internal class ClipboardProcessor
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x001302D3 File Offset: 0x0012F2D3
		internal ClipboardProcessor(InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._preferredClipboardData = new Dictionary<InkCanvasClipboardFormat, ClipboardData>();
			this._preferredClipboardData.Add(InkCanvasClipboardFormat.InkSerializedFormat, new ISFClipboardData());
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0013030C File Offset: 0x0012F30C
		internal bool CheckDataFormats(IDataObject dataObject)
		{
			foreach (KeyValuePair<InkCanvasClipboardFormat, ClipboardData> keyValuePair in this._preferredClipboardData)
			{
				if (keyValuePair.Value.CanPaste(dataObject))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00130370 File Offset: 0x0012F370
		internal InkCanvasClipboardDataFormats CopySelectedData(IDataObject dataObject)
		{
			InkCanvasClipboardDataFormats inkCanvasClipboardDataFormats = InkCanvasClipboardDataFormats.None;
			InkCanvasSelection inkCanvasSelection = this.InkCanvas.InkCanvasSelection;
			StrokeCollection strokeCollection = inkCanvasSelection.SelectedStrokes;
			if (strokeCollection.Count > 1)
			{
				StrokeCollection strokeCollection2 = new StrokeCollection();
				StrokeCollection strokes = this.InkCanvas.Strokes;
				int num = 0;
				while (num < strokes.Count && strokeCollection.Count != strokeCollection2.Count)
				{
					for (int i = 0; i < strokeCollection.Count; i++)
					{
						if (strokes[num] == strokeCollection[i])
						{
							strokeCollection2.Add(strokeCollection[i]);
							break;
						}
					}
					num++;
				}
				strokeCollection = strokeCollection2.Clone();
			}
			else
			{
				strokeCollection = strokeCollection.Clone();
			}
			List<UIElement> list = new List<UIElement>(inkCanvasSelection.SelectedElements);
			Rect selectionBounds = inkCanvasSelection.SelectionBounds;
			if (strokeCollection.Count != 0 || list.Count != 0)
			{
				Matrix identity = Matrix.Identity;
				identity.OffsetX = -selectionBounds.Left;
				identity.OffsetY = -selectionBounds.Top;
				if (strokeCollection.Count != 0)
				{
					inkCanvasSelection.TransformStrokes(strokeCollection, identity);
					new ISFClipboardData(strokeCollection).CopyToDataObject(dataObject);
					inkCanvasClipboardDataFormats |= InkCanvasClipboardDataFormats.ISF;
				}
				if (this.CopySelectionInXAML(dataObject, strokeCollection, list, identity, selectionBounds.Size))
				{
					inkCanvasClipboardDataFormats |= InkCanvasClipboardDataFormats.XAML;
				}
			}
			return inkCanvasClipboardDataFormats;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x001304A4 File Offset: 0x0012F4A4
		internal bool PasteData(IDataObject dataObject, ref StrokeCollection newStrokes, ref List<UIElement> newElements)
		{
			foreach (KeyValuePair<InkCanvasClipboardFormat, ClipboardData> keyValuePair in this._preferredClipboardData)
			{
				InkCanvasClipboardFormat key = keyValuePair.Key;
				ClipboardData value = keyValuePair.Value;
				if (value.CanPaste(dataObject))
				{
					switch (key)
					{
					case InkCanvasClipboardFormat.InkSerializedFormat:
					{
						ISFClipboardData isfclipboardData = (ISFClipboardData)value;
						isfclipboardData.PasteFromDataObject(dataObject);
						newStrokes = isfclipboardData.Strokes;
						break;
					}
					case InkCanvasClipboardFormat.Text:
					{
						TextClipboardData textClipboardData = (TextClipboardData)value;
						textClipboardData.PasteFromDataObject(dataObject);
						newElements = textClipboardData.Elements;
						break;
					}
					case InkCanvasClipboardFormat.Xaml:
					{
						XamlClipboardData xamlClipboardData = (XamlClipboardData)value;
						xamlClipboardData.PasteFromDataObject(dataObject);
						List<UIElement> elements = xamlClipboardData.Elements;
						if (elements != null && elements.Count != 0)
						{
							if (elements.Count == 1 && ClipboardProcessor.InkCanvasDType.IsInstanceOfType(elements[0]))
							{
								this.TearDownInkCanvasContainer((InkCanvas)elements[0], ref newStrokes, ref newElements);
							}
							else
							{
								newElements = elements;
							}
						}
						break;
					}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x001305C4 File Offset: 0x0012F5C4
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x001305D4 File Offset: 0x0012F5D4
		internal IEnumerable<InkCanvasClipboardFormat> PreferredFormats
		{
			get
			{
				foreach (KeyValuePair<InkCanvasClipboardFormat, ClipboardData> keyValuePair in this._preferredClipboardData)
				{
					yield return keyValuePair.Key;
				}
				Dictionary<InkCanvasClipboardFormat, ClipboardData>.Enumerator enumerator = default(Dictionary<InkCanvasClipboardFormat, ClipboardData>.Enumerator);
				yield break;
				yield break;
			}
			set
			{
				Dictionary<InkCanvasClipboardFormat, ClipboardData> dictionary = new Dictionary<InkCanvasClipboardFormat, ClipboardData>();
				foreach (InkCanvasClipboardFormat key in value)
				{
					if (!dictionary.ContainsKey(key))
					{
						ClipboardData value2;
						switch (key)
						{
						case InkCanvasClipboardFormat.InkSerializedFormat:
							value2 = new ISFClipboardData();
							break;
						case InkCanvasClipboardFormat.Text:
							value2 = new TextClipboardData();
							break;
						case InkCanvasClipboardFormat.Xaml:
							value2 = new XamlClipboardData();
							break;
						default:
							throw new ArgumentException(SR.Get("InvalidClipboardFormat"), "value");
						}
						dictionary.Add(key, value2);
					}
				}
				this._preferredClipboardData = dictionary;
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00130678 File Offset: 0x0012F678
		private bool CopySelectionInXAML(IDataObject dataObject, StrokeCollection strokes, List<UIElement> elements, Matrix transform, Size size)
		{
			InkCanvas inkCanvas = new InkCanvas();
			if (strokes.Count != 0)
			{
				inkCanvas.Strokes = strokes;
			}
			int count = elements.Count;
			if (count != 0)
			{
				InkCanvasSelection inkCanvasSelection = this.InkCanvas.InkCanvasSelection;
				for (int i = 0; i < count; i++)
				{
					UIElement uielement = XamlReader.Load(new XmlTextReader(new StringReader(XamlWriter.Save(elements[i])))) as UIElement;
					((IAddChild)inkCanvas).AddChild(uielement);
					inkCanvasSelection.UpdateElementBounds(elements[i], uielement, transform);
				}
			}
			if (inkCanvas != null)
			{
				inkCanvas.Width = size.Width;
				inkCanvas.Height = size.Height;
				ClipboardData clipboardData = new XamlClipboardData(new UIElement[]
				{
					inkCanvas
				});
				try
				{
					clipboardData.CopyToDataObject(dataObject);
				}
				catch (SecurityException)
				{
					inkCanvas = null;
				}
			}
			return inkCanvas != null;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0013074C File Offset: 0x0012F74C
		private void TearDownInkCanvasContainer(InkCanvas rootInkCanvas, ref StrokeCollection newStrokes, ref List<UIElement> newElements)
		{
			newStrokes = rootInkCanvas.Strokes;
			if (rootInkCanvas.Children.Count != 0)
			{
				List<UIElement> list = new List<UIElement>(rootInkCanvas.Children.Count);
				foreach (object obj in rootInkCanvas.Children)
				{
					UIElement item = (UIElement)obj;
					list.Add(item);
				}
				foreach (UIElement element in list)
				{
					rootInkCanvas.Children.Remove(element);
				}
				newElements = list;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00130818 File Offset: 0x0012F818
		private InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00130820 File Offset: 0x0012F820
		private static DependencyObjectType InkCanvasDType
		{
			get
			{
				if (ClipboardProcessor.s_InkCanvasDType == null)
				{
					ClipboardProcessor.s_InkCanvasDType = DependencyObjectType.FromSystemTypeInternal(typeof(InkCanvas));
				}
				return ClipboardProcessor.s_InkCanvasDType;
			}
		}

		// Token: 0x0400098F RID: 2447
		private InkCanvas _inkCanvas;

		// Token: 0x04000990 RID: 2448
		private static DependencyObjectType s_InkCanvasDType;

		// Token: 0x04000991 RID: 2449
		private Dictionary<InkCanvasClipboardFormat, ClipboardData> _preferredClipboardData;
	}
}
