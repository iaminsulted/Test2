using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x02000181 RID: 385
	internal class DrawingAttributesDefaultValueFactory : DefaultValueFactory
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00130842 File Offset: 0x0012F842
		internal DrawingAttributesDefaultValueFactory()
		{
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0013084A File Offset: 0x0012F84A
		internal override object DefaultValue
		{
			get
			{
				return new DrawingAttributes();
			}
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00130854 File Offset: 0x0012F854
		internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
		{
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			DrawingAttributesDefaultValueFactory.DrawingAttributesDefaultPromoter @object = new DrawingAttributesDefaultValueFactory.DrawingAttributesDefaultPromoter((InkCanvas)owner);
			drawingAttributes.AttributeChanged += @object.OnDrawingAttributesChanged;
			drawingAttributes.PropertyDataChanged += @object.OnDrawingAttributesChanged;
			return drawingAttributes;
		}

		// Token: 0x020009C4 RID: 2500
		private class DrawingAttributesDefaultPromoter
		{
			// Token: 0x060083D9 RID: 33753 RVA: 0x00324653 File Offset: 0x00323653
			internal DrawingAttributesDefaultPromoter(InkCanvas owner)
			{
				this._owner = owner;
			}

			// Token: 0x060083DA RID: 33754 RVA: 0x00324664 File Offset: 0x00323664
			internal void OnDrawingAttributesChanged(object sender, PropertyDataChangedEventArgs e)
			{
				DrawingAttributes drawingAttributes = (DrawingAttributes)sender;
				drawingAttributes.AttributeChanged -= this.OnDrawingAttributesChanged;
				drawingAttributes.PropertyDataChanged -= this.OnDrawingAttributesChanged;
				if (this._owner.ReadLocalValue(InkCanvas.DefaultDrawingAttributesProperty) == DependencyProperty.UnsetValue)
				{
					this._owner.SetValue(InkCanvas.DefaultDrawingAttributesProperty, drawingAttributes);
				}
				InkCanvas.DefaultDrawingAttributesProperty.GetMetadata(this._owner.DependencyObjectType).ClearCachedDefaultValue(this._owner, InkCanvas.DefaultDrawingAttributesProperty);
			}

			// Token: 0x04003F8D RID: 16269
			private readonly InkCanvas _owner;
		}
	}
}
