using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Annotations;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D0 RID: 720
	internal sealed class DataIdProcessor : SubTreeProcessor
	{
		// Token: 0x06001B1D RID: 6941 RVA: 0x00166C70 File Offset: 0x00165C70
		public DataIdProcessor(LocatorManager manager) : base(manager)
		{
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00166C7C File Offset: 0x00165C7C
		public override IList<IAttachedAnnotation> PreProcessNode(DependencyObject node, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			object obj = node.ReadLocalValue(DataIdProcessor.DataIdProperty);
			if ((bool)node.GetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty) && obj != DependencyProperty.UnsetValue)
			{
				calledProcessAnnotations = true;
				return base.Manager.ProcessAnnotations(node);
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x00166CD4 File Offset: 0x00165CD4
		public override IList<IAttachedAnnotation> PostProcessNode(DependencyObject node, bool childrenCalledProcessAnnotations, out bool calledProcessAnnotations)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			object obj = node.ReadLocalValue(DataIdProcessor.DataIdProperty);
			if (!(bool)node.GetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty) && !childrenCalledProcessAnnotations && obj != DependencyProperty.UnsetValue)
			{
				FrameworkElement frameworkElement = null;
				FrameworkElement frameworkElement2 = node as FrameworkElement;
				if (frameworkElement2 != null)
				{
					frameworkElement = (frameworkElement2.Parent as FrameworkElement);
				}
				AnnotationService service = AnnotationService.GetService(node);
				if (service != null && (service.Root == node || (frameworkElement != null && service.Root == frameworkElement.TemplatedParent)))
				{
					calledProcessAnnotations = true;
					return base.Manager.ProcessAnnotations(node);
				}
			}
			calledProcessAnnotations = false;
			return null;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00166D68 File Offset: 0x00165D68
		public override ContentLocator GenerateLocator(PathNode node, out bool continueGenerating)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			continueGenerating = true;
			ContentLocator contentLocator = null;
			ContentLocatorPart contentLocatorPart = this.CreateLocatorPart(node.Node);
			if (contentLocatorPart != null)
			{
				contentLocator = new ContentLocator();
				contentLocator.Parts.Add(contentLocatorPart);
			}
			return contentLocator;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00166DAC File Offset: 0x00165DAC
		public override DependencyObject ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out bool continueResolving)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (DataIdProcessor.DataIdElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			continueResolving = true;
			string text = locatorPart.NameValuePairs["Value"];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			string nodeId = this.GetNodeId(startNode);
			if (nodeId != null)
			{
				if (nodeId.Equals(text))
				{
					return startNode;
				}
				continueResolving = false;
			}
			return null;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00166E94 File Offset: 0x00165E94
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])DataIdProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00166EA5 File Offset: 0x00165EA5
		public static void SetDataId(DependencyObject d, string id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			d.SetValue(DataIdProcessor.DataIdProperty, id);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00166EC1 File Offset: 0x00165EC1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static string GetDataId(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return d.GetValue(DataIdProcessor.DataIdProperty) as string;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00166EE1 File Offset: 0x00165EE1
		public static void SetFetchAnnotationsAsBatch(DependencyObject d, bool id)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			d.SetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty, id);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00166EFD File Offset: 0x00165EFD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static bool GetFetchAnnotationsAsBatch(DependencyObject d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return (bool)d.GetValue(DataIdProcessor.FetchAnnotationsAsBatchProperty);
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00166F20 File Offset: 0x00165F20
		private static void OnDataIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			string a = (string)e.OldValue;
			string b = (string)e.NewValue;
			if (!string.Equals(a, b))
			{
				AnnotationService service = AnnotationService.GetService(d);
				if (service != null && service.IsEnabled)
				{
					service.UnloadAnnotations(d);
					service.LoadAnnotations(d);
				}
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x00166F70 File Offset: 0x00165F70
		private static object CoerceDataId(DependencyObject d, object value)
		{
			string text = (string)value;
			if (text == null || text.Length != 0)
			{
				return value;
			}
			return null;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x00166F94 File Offset: 0x00165F94
		private ContentLocatorPart CreateLocatorPart(DependencyObject node)
		{
			string nodeId = this.GetNodeId(node);
			if (nodeId == null || nodeId.Length == 0)
			{
				return null;
			}
			return new ContentLocatorPart(DataIdProcessor.DataIdElementName)
			{
				NameValuePairs = 
				{
					{
						"Value",
						nodeId
					}
				}
			};
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00166FD4 File Offset: 0x00165FD4
		internal string GetNodeId(DependencyObject d)
		{
			string text = d.GetValue(DataIdProcessor.DataIdProperty) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = null;
			}
			return text;
		}

		// Token: 0x04000DF8 RID: 3576
		public const string Id = "Id";

		// Token: 0x04000DF9 RID: 3577
		public static readonly DependencyProperty DataIdProperty = DependencyProperty.RegisterAttached("DataId", typeof(string), typeof(DataIdProcessor), new PropertyMetadata(null, new PropertyChangedCallback(DataIdProcessor.OnDataIdPropertyChanged), new CoerceValueCallback(DataIdProcessor.CoerceDataId)));

		// Token: 0x04000DFA RID: 3578
		public static readonly DependencyProperty FetchAnnotationsAsBatchProperty = DependencyProperty.RegisterAttached("FetchAnnotationsAsBatch", typeof(bool), typeof(DataIdProcessor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04000DFB RID: 3579
		private static readonly XmlQualifiedName DataIdElementName = new XmlQualifiedName("DataId", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04000DFC RID: 3580
		private const string ValueAttributeName = "Value";

		// Token: 0x04000DFD RID: 3581
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			DataIdProcessor.DataIdElementName
		};
	}
}
