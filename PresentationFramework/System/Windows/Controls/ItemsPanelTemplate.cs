using System;
using System.Xaml;

namespace System.Windows.Controls
{
	// Token: 0x020007A3 RID: 1955
	public class ItemsPanelTemplate : FrameworkTemplate
	{
		// Token: 0x06006E5D RID: 28253 RVA: 0x00174A65 File Offset: 0x00173A65
		public ItemsPanelTemplate()
		{
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x002D1795 File Offset: 0x002D0795
		public ItemsPanelTemplate(FrameworkElementFactory root)
		{
			base.VisualTree = root;
		}

		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x06006E5F RID: 28255 RVA: 0x002D17A4 File Offset: 0x002D07A4
		internal override Type TargetTypeInternal
		{
			get
			{
				return ItemsPanelTemplate.DefaultTargetType;
			}
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x00174B1B File Offset: 0x00173B1B
		internal override void SetTargetTypeInternal(Type targetType)
		{
			throw new InvalidOperationException(SR.Get("TemplateNotTargetType"));
		}

		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x06006E61 RID: 28257 RVA: 0x002D17AB File Offset: 0x002D07AB
		internal static Type DefaultTargetType
		{
			get
			{
				return typeof(ItemsPresenter);
			}
		}

		// Token: 0x06006E62 RID: 28258 RVA: 0x002D17B8 File Offset: 0x002D07B8
		internal override void ProcessTemplateBeforeSeal()
		{
			FrameworkElementFactory visualTree;
			if (base.HasContent)
			{
				TemplateContent template = base.Template;
				XamlType xamlType = template.SchemaContext.GetXamlType(typeof(Panel));
				if (template.RootType == null || !template.RootType.CanAssignTo(xamlType))
				{
					throw new InvalidOperationException(SR.Get("ItemsPanelNotAPanel", new object[]
					{
						template.RootType
					}));
				}
			}
			else if ((visualTree = base.VisualTree) != null)
			{
				if (!typeof(Panel).IsAssignableFrom(visualTree.Type))
				{
					throw new InvalidOperationException(SR.Get("ItemsPanelNotAPanel", new object[]
					{
						visualTree.Type
					}));
				}
				visualTree.SetValue(Panel.IsItemsHostProperty, true);
			}
		}

		// Token: 0x06006E63 RID: 28259 RVA: 0x002D1878 File Offset: 0x002D0878
		protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (!(templatedParent is ItemsPresenter))
			{
				throw new ArgumentException(SR.Get("TemplateTargetTypeMismatch", new object[]
				{
					"ItemsPresenter",
					templatedParent.GetType().Name
				}));
			}
		}
	}
}
