using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000221 RID: 545
	internal sealed class DisplayMemberTemplateSelector : DataTemplateSelector
	{
		// Token: 0x06001479 RID: 5241 RVA: 0x00152304 File Offset: 0x00151304
		public DisplayMemberTemplateSelector(string displayMemberPath, string stringFormat)
		{
			this._displayMemberPath = displayMemberPath;
			this._stringFormat = stringFormat;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0015231C File Offset: 0x0015131C
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (SystemXmlHelper.IsXmlNode(item))
			{
				if (this._xmlNodeContentTemplate == null)
				{
					this._xmlNodeContentTemplate = new DataTemplate();
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					Binding binding = new Binding();
					binding.XPath = this._displayMemberPath;
					binding.StringFormat = this._stringFormat;
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					this._xmlNodeContentTemplate.VisualTree = frameworkElementFactory;
					this._xmlNodeContentTemplate.Seal();
				}
				return this._xmlNodeContentTemplate;
			}
			if (this._clrNodeContentTemplate == null)
			{
				this._clrNodeContentTemplate = new DataTemplate();
				FrameworkElementFactory frameworkElementFactory2 = ContentPresenter.CreateTextBlockFactory();
				Binding binding2 = new Binding();
				binding2.Path = new PropertyPath(this._displayMemberPath, Array.Empty<object>());
				binding2.StringFormat = this._stringFormat;
				frameworkElementFactory2.SetBinding(TextBlock.TextProperty, binding2);
				this._clrNodeContentTemplate.VisualTree = frameworkElementFactory2;
				this._clrNodeContentTemplate.Seal();
			}
			return this._clrNodeContentTemplate;
		}

		// Token: 0x04000BCF RID: 3023
		private string _displayMemberPath;

		// Token: 0x04000BD0 RID: 3024
		private string _stringFormat;

		// Token: 0x04000BD1 RID: 3025
		private DataTemplate _xmlNodeContentTemplate;

		// Token: 0x04000BD2 RID: 3026
		private DataTemplate _clrNodeContentTemplate;
	}
}
