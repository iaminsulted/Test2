using System;
using System.Windows;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x02000190 RID: 400
	internal class StrokeCollectionDefaultValueFactory : DefaultValueFactory
	{
		// Token: 0x06000D62 RID: 3426 RVA: 0x00130842 File Offset: 0x0012F842
		internal StrokeCollectionDefaultValueFactory()
		{
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00135225 File Offset: 0x00134225
		internal override object DefaultValue
		{
			get
			{
				return new StrokeCollection();
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0013522C File Offset: 0x0013422C
		internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			StrokeCollectionDefaultValueFactory.StrokeCollectionDefaultPromoter @object = new StrokeCollectionDefaultValueFactory.StrokeCollectionDefaultPromoter(owner, property);
			strokeCollection.StrokesChanged += @object.OnStrokeCollectionChanged<StrokeCollectionChangedEventArgs>;
			strokeCollection.PropertyDataChanged += @object.OnStrokeCollectionChanged<PropertyDataChangedEventArgs>;
			return strokeCollection;
		}

		// Token: 0x020009C9 RID: 2505
		private class StrokeCollectionDefaultPromoter
		{
			// Token: 0x060083E8 RID: 33768 RVA: 0x003248C3 File Offset: 0x003238C3
			internal StrokeCollectionDefaultPromoter(DependencyObject owner, DependencyProperty property)
			{
				this._owner = owner;
				this._dependencyProperty = property;
			}

			// Token: 0x060083E9 RID: 33769 RVA: 0x003248DC File Offset: 0x003238DC
			internal void OnStrokeCollectionChanged<TEventArgs>(object sender, TEventArgs e)
			{
				StrokeCollection strokeCollection = (StrokeCollection)sender;
				strokeCollection.StrokesChanged -= this.OnStrokeCollectionChanged<StrokeCollectionChangedEventArgs>;
				strokeCollection.PropertyDataChanged -= this.OnStrokeCollectionChanged<PropertyDataChangedEventArgs>;
				if (this._owner.ReadLocalValue(this._dependencyProperty) == DependencyProperty.UnsetValue)
				{
					this._owner.SetValue(this._dependencyProperty, strokeCollection);
				}
				this._dependencyProperty.GetMetadata(this._owner.DependencyObjectType).ClearCachedDefaultValue(this._owner, this._dependencyProperty);
			}

			// Token: 0x04003FA4 RID: 16292
			private readonly DependencyObject _owner;

			// Token: 0x04003FA5 RID: 16293
			private readonly DependencyProperty _dependencyProperty;
		}
	}
}
