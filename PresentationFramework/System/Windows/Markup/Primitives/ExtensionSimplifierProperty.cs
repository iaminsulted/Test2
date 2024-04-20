using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000534 RID: 1332
	internal class ExtensionSimplifierProperty : MarkupPropertyWrapper
	{
		// Token: 0x060041F5 RID: 16885 RVA: 0x00219775 File Offset: 0x00218775
		public ExtensionSimplifierProperty(MarkupProperty baseProperty, IValueSerializerContext context) : base(baseProperty)
		{
			this._context = context;
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x00219788 File Offset: 0x00218788
		public override bool IsComposite
		{
			get
			{
				if (!base.IsComposite)
				{
					return false;
				}
				if (base.IsCollectionProperty)
				{
					return true;
				}
				bool flag = true;
				foreach (MarkupObject markupObject in this.Items)
				{
					if (!flag || !typeof(MarkupExtension).IsAssignableFrom(markupObject.ObjectType))
					{
						return true;
					}
					flag = false;
					markupObject.AssignRootContext(this._context);
					using (IEnumerator<MarkupProperty> enumerator2 = markupObject.Properties.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.IsComposite)
							{
								return true;
							}
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00219858 File Offset: 0x00218858
		private IEnumerable<MarkupObject> GetBaseItems()
		{
			return base.Items;
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x00219860 File Offset: 0x00218860
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				foreach (MarkupObject baseObject in this.GetBaseItems())
				{
					ExtensionSimplifierMarkupObject extensionSimplifierMarkupObject = new ExtensionSimplifierMarkupObject(baseObject, this._context);
					extensionSimplifierMarkupObject.AssignRootContext(this._context);
					yield return extensionSimplifierMarkupObject;
				}
				IEnumerator<MarkupObject> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x00219870 File Offset: 0x00218870
		public override string StringValue
		{
			get
			{
				string text = null;
				if (!base.IsComposite)
				{
					text = MarkupExtensionParser.AddEscapeToLiteralString(base.StringValue);
				}
				else
				{
					using (IEnumerator<MarkupObject> enumerator = this.Items.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							MarkupObject item = enumerator.Current;
							text = this.ConvertMarkupItemToString(item);
						}
					}
					if (text == null)
					{
						text = "";
					}
				}
				return text;
			}
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x002198E4 File Offset: 0x002188E4
		private string ConvertMarkupItemToString(MarkupObject item)
		{
			ValueSerializer valueSerializerFor = this._context.GetValueSerializerFor(typeof(Type));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			string text = valueSerializerFor.ConvertToString(item.ObjectType, this._context);
			if (text.EndsWith("Extension", StringComparison.Ordinal))
			{
				stringBuilder.Append(text, 0, text.Length - 9);
			}
			else
			{
				stringBuilder.Append(text);
			}
			bool flag = true;
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				stringBuilder.Append(flag ? " " : ", ");
				flag = false;
				if (!markupProperty.IsConstructorArgument)
				{
					stringBuilder.Append(markupProperty.Name);
					stringBuilder.Append('=');
				}
				string text2 = markupProperty.StringValue;
				if (text2 != null && text2.Length > 0)
				{
					if (text2[0] == '{')
					{
						if (text2.Length <= 1 || text2[1] != '}')
						{
							stringBuilder.Append(text2);
							continue;
						}
						text2 = text2.Substring(2);
					}
					foreach (char c in text2)
					{
						if (c != ',')
						{
							if (c != '{')
							{
								if (c != '}')
								{
									stringBuilder.Append(c);
								}
								else
								{
									stringBuilder.Append("\\}");
								}
							}
							else
							{
								stringBuilder.Append("\\{");
							}
						}
						else
						{
							stringBuilder.Append("\\,");
						}
					}
				}
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00219AA4 File Offset: 0x00218AA4
		internal override void VerifyOnlySerializableTypes()
		{
			base.VerifyOnlySerializableTypes();
			if (base.IsComposite)
			{
				foreach (MarkupObject markupObject in this.Items)
				{
					MarkupWriter.VerifyTypeIsSerializable(markupObject.ObjectType);
					foreach (MarkupProperty markupProperty in markupObject.Properties)
					{
						markupProperty.VerifyOnlySerializableTypes();
					}
				}
			}
		}

		// Token: 0x040024E1 RID: 9441
		private IValueSerializerContext _context;

		// Token: 0x040024E2 RID: 9442
		private const int EXTENSIONLENGTH = 9;
	}
}
