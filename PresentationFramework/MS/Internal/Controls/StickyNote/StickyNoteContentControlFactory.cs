using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Xml;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000267 RID: 615
	internal static class StickyNoteContentControlFactory
	{
		// Token: 0x060017E2 RID: 6114 RVA: 0x0015F984 File Offset: 0x0015E984
		public static StickyNoteContentControl CreateContentControl(StickyNoteType type, UIElement content)
		{
			StickyNoteContentControl result = null;
			if (type != StickyNoteType.Text)
			{
				if (type == StickyNoteType.Ink)
				{
					InkCanvas inkCanvas = content as InkCanvas;
					if (inkCanvas == null)
					{
						throw new InvalidOperationException(SR.Get("InvalidStickyNoteTemplate", new object[]
						{
							type,
							typeof(InkCanvas),
							"PART_ContentControl"
						}));
					}
					result = new StickyNoteContentControlFactory.StickyNoteInkCanvas(inkCanvas);
				}
			}
			else
			{
				RichTextBox richTextBox = content as RichTextBox;
				if (richTextBox == null)
				{
					throw new InvalidOperationException(SR.Get("InvalidStickyNoteTemplate", new object[]
					{
						type,
						typeof(RichTextBox),
						"PART_ContentControl"
					}));
				}
				result = new StickyNoteContentControlFactory.StickyNoteRichTextBox(richTextBox);
			}
			return result;
		}

		// Token: 0x02000A0B RID: 2571
		private class StickyNoteRichTextBox : StickyNoteContentControl
		{
			// Token: 0x060084B4 RID: 33972 RVA: 0x00326AC2 File Offset: 0x00325AC2
			public StickyNoteRichTextBox(RichTextBox rtb) : base(rtb)
			{
				DataObject.AddPastingHandler(rtb, new DataObjectPastingEventHandler(this.OnPastingDataObject));
			}

			// Token: 0x060084B5 RID: 33973 RVA: 0x00326ADD File Offset: 0x00325ADD
			public override void Clear()
			{
				((RichTextBox)base.InnerControl).Document = new FlowDocument(new Paragraph(new Run()));
			}

			// Token: 0x060084B6 RID: 33974 RVA: 0x00326B00 File Offset: 0x00325B00
			public override void Save(XmlNode node)
			{
				RichTextBox richTextBox = (RichTextBox)base.InnerControl;
				TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
				if (!textRange.IsEmpty)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						textRange.Save(memoryStream, DataFormats.Xaml);
						if (memoryStream.Length.CompareTo(1610612733L) > 0)
						{
							throw new InvalidOperationException(SR.Get("MaximumNoteSizeExceeded"));
						}
						node.InnerText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					}
				}
			}

			// Token: 0x060084B7 RID: 33975 RVA: 0x00326BAC File Offset: 0x00325BAC
			public override void Load(XmlNode node)
			{
				RichTextBox richTextBox = (RichTextBox)base.InnerControl;
				FlowDocument flowDocument = new FlowDocument();
				TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd, true);
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(node.InnerText)))
				{
					textRange.Load(memoryStream, DataFormats.Xaml);
				}
				richTextBox.Document = flowDocument;
			}

			// Token: 0x17001DD3 RID: 7635
			// (get) Token: 0x060084B8 RID: 33976 RVA: 0x00326C20 File Offset: 0x00325C20
			public override bool IsEmpty
			{
				get
				{
					RichTextBox richTextBox = (RichTextBox)base.InnerControl;
					return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).IsEmpty;
				}
			}

			// Token: 0x17001DD4 RID: 7636
			// (get) Token: 0x060084B9 RID: 33977 RVA: 0x00105F35 File Offset: 0x00104F35
			public override StickyNoteType Type
			{
				get
				{
					return StickyNoteType.Text;
				}
			}

			// Token: 0x060084BA RID: 33978 RVA: 0x00326C5C File Offset: 0x00325C5C
			private void OnPastingDataObject(object sender, DataObjectPastingEventArgs e)
			{
				if (e.FormatToApply == DataFormats.Rtf)
				{
					UTF8Encoding utf8Encoding = new UTF8Encoding();
					string s = e.DataObject.GetData(DataFormats.Rtf) as string;
					MemoryStream stream = new MemoryStream(utf8Encoding.GetBytes(s));
					FlowDocument flowDocument = new FlowDocument();
					TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
					textRange.Load(stream, DataFormats.Rtf);
					MemoryStream memoryStream = new MemoryStream();
					textRange.Save(memoryStream, DataFormats.Xaml);
					DataObject dataObject = new DataObject();
					dataObject.SetData(DataFormats.Xaml, utf8Encoding.GetString(memoryStream.GetBuffer()));
					e.DataObject = dataObject;
					e.FormatToApply = DataFormats.Xaml;
					return;
				}
				if (e.FormatToApply == DataFormats.Bitmap || e.FormatToApply == DataFormats.EnhancedMetafile || e.FormatToApply == DataFormats.MetafilePicture || e.FormatToApply == DataFormats.Tiff)
				{
					e.CancelCommand();
					return;
				}
				if (e.FormatToApply == DataFormats.XamlPackage)
				{
					e.FormatToApply = DataFormats.Xaml;
				}
			}
		}

		// Token: 0x02000A0C RID: 2572
		private class StickyNoteInkCanvas : StickyNoteContentControl
		{
			// Token: 0x060084BB RID: 33979 RVA: 0x00326D7D File Offset: 0x00325D7D
			public StickyNoteInkCanvas(InkCanvas canvas) : base(canvas)
			{
			}

			// Token: 0x060084BC RID: 33980 RVA: 0x00326D86 File Offset: 0x00325D86
			public override void Clear()
			{
				((InkCanvas)base.InnerControl).Strokes.Clear();
			}

			// Token: 0x060084BD RID: 33981 RVA: 0x00326DA0 File Offset: 0x00325DA0
			public override void Save(XmlNode node)
			{
				StrokeCollection strokes = ((InkCanvas)base.InnerControl).Strokes;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					strokes.Save(memoryStream);
					if (memoryStream.Length.CompareTo(1610612733L) > 0)
					{
						throw new InvalidOperationException(SR.Get("MaximumNoteSizeExceeded"));
					}
					node.InnerText = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				}
			}

			// Token: 0x060084BE RID: 33982 RVA: 0x00326E28 File Offset: 0x00325E28
			public override void Load(XmlNode node)
			{
				StrokeCollection strokes = null;
				if (string.IsNullOrEmpty(node.InnerText))
				{
					strokes = new StrokeCollection();
				}
				else
				{
					using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(node.InnerText)))
					{
						strokes = new StrokeCollection(memoryStream);
					}
				}
				((InkCanvas)base.InnerControl).Strokes = strokes;
			}

			// Token: 0x17001DD5 RID: 7637
			// (get) Token: 0x060084BF RID: 33983 RVA: 0x00326E94 File Offset: 0x00325E94
			public override bool IsEmpty
			{
				get
				{
					return ((InkCanvas)base.InnerControl).Strokes.Count == 0;
				}
			}

			// Token: 0x17001DD6 RID: 7638
			// (get) Token: 0x060084C0 RID: 33984 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public override StickyNoteType Type
			{
				get
				{
					return StickyNoteType.Ink;
				}
			}
		}
	}
}
