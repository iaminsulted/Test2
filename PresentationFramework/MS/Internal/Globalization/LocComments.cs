using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x020001B4 RID: 436
	internal static class LocComments
	{
		// Token: 0x06000E32 RID: 3634 RVA: 0x001383FD File Offset: 0x001373FD
		internal static bool IsLocLocalizabilityProperty(string type, string property)
		{
			return "Attributes" == property && "System.Windows.Localization" == type;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00138419 File Offset: 0x00137419
		internal static bool IsLocCommentsProperty(string type, string property)
		{
			return "Comments" == property && "System.Windows.Localization" == type;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00138438 File Offset: 0x00137438
		internal static PropertyComment[] ParsePropertyLocalizabilityAttributes(string input)
		{
			PropertyComment[] array = LocComments.ParsePropertyComments(input);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Value = LocComments.LookupAndSetLocalizabilityAttribute((string)array[i].Value);
				}
			}
			return array;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00138478 File Offset: 0x00137478
		internal static PropertyComment[] ParsePropertyComments(string input)
		{
			if (input == null)
			{
				return null;
			}
			List<PropertyComment> list = new List<PropertyComment>(8);
			StringBuilder stringBuilder = new StringBuilder();
			PropertyComment propertyComment = new PropertyComment();
			bool flag = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (propertyComment.PropertyName == null)
				{
					if (char.IsWhiteSpace(input[i]) && !flag)
					{
						if (stringBuilder.Length > 0)
						{
							propertyComment.PropertyName = stringBuilder.ToString();
							stringBuilder = new StringBuilder();
						}
					}
					else if (input[i] == '(' && !flag)
					{
						if (i <= 0)
						{
							throw new FormatException(SR.Get("InvalidLocCommentTarget", new object[]
							{
								input
							}));
						}
						propertyComment.PropertyName = stringBuilder.ToString();
						stringBuilder = new StringBuilder();
						i--;
					}
					else if (input[i] == '\\' && !flag)
					{
						flag = true;
					}
					else
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
				}
				else if (stringBuilder.Length == 0)
				{
					if (input[i] == '(' && !flag)
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
					else if (!char.IsWhiteSpace(input[i]))
					{
						throw new FormatException(SR.Get("InvalidLocCommentValue", new object[]
						{
							propertyComment.PropertyName,
							input
						}));
					}
				}
				else if (input[i] == ')')
				{
					if (!flag)
					{
						propertyComment.Value = stringBuilder.ToString().Substring(1);
						list.Add(propertyComment);
						stringBuilder = new StringBuilder();
						propertyComment = new PropertyComment();
					}
					else
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
				}
				else
				{
					if (input[i] == '(' && !flag)
					{
						throw new FormatException(SR.Get("InvalidLocCommentValue", new object[]
						{
							propertyComment.PropertyName,
							input
						}));
					}
					if (input[i] == '\\' && !flag)
					{
						flag = true;
					}
					else
					{
						stringBuilder.Append(input[i]);
						flag = false;
					}
				}
			}
			if (propertyComment.PropertyName != null || stringBuilder.Length != 0)
			{
				throw new FormatException(SR.Get("UnmatchedLocComment", new object[]
				{
					input
				}));
			}
			return list.ToArray();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x001386A4 File Offset: 0x001376A4
		private static LocalizabilityGroup LookupAndSetLocalizabilityAttribute(string input)
		{
			LocalizabilityGroup localizabilityGroup = new LocalizabilityGroup();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsWhiteSpace(input[i]))
				{
					if (stringBuilder.Length > 0)
					{
						LocComments.ParseLocalizabilityString(stringBuilder.ToString(), localizabilityGroup);
						stringBuilder = new StringBuilder();
					}
				}
				else
				{
					stringBuilder.Append(input[i]);
				}
			}
			if (stringBuilder.Length > 0)
			{
				LocComments.ParseLocalizabilityString(stringBuilder.ToString(), localizabilityGroup);
			}
			return localizabilityGroup;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00138720 File Offset: 0x00137720
		private static void ParseLocalizabilityString(string value, LocalizabilityGroup attributeGroup)
		{
			int num;
			if (LocComments.ReadabilityIndexTable.TryGet(value, out num))
			{
				attributeGroup.Readability = (Readability)num;
				return;
			}
			if (LocComments.ModifiabilityIndexTable.TryGet(value, out num))
			{
				attributeGroup.Modifiability = (Modifiability)num;
				return;
			}
			if (LocComments.LocalizationCategoryIndexTable.TryGet(value, out num))
			{
				attributeGroup.Category = (LocalizationCategory)num;
				return;
			}
			throw new FormatException(SR.Get("InvalidLocalizabilityValue", new object[]
			{
				value
			}));
		}

		// Token: 0x04000A33 RID: 2611
		private const char CommentStart = '(';

		// Token: 0x04000A34 RID: 2612
		private const char CommentEnd = ')';

		// Token: 0x04000A35 RID: 2613
		private const char EscapeChar = '\\';

		// Token: 0x04000A36 RID: 2614
		internal const string LocDocumentRoot = "LocalizableAssembly";

		// Token: 0x04000A37 RID: 2615
		internal const string LocResourcesElement = "LocalizableFile";

		// Token: 0x04000A38 RID: 2616
		internal const string LocCommentsElement = "LocalizationDirectives";

		// Token: 0x04000A39 RID: 2617
		internal const string LocFileNameAttribute = "Name";

		// Token: 0x04000A3A RID: 2618
		internal const string LocCommentIDAttribute = "Uid";

		// Token: 0x04000A3B RID: 2619
		internal const string LocCommentsAttribute = "Comments";

		// Token: 0x04000A3C RID: 2620
		internal const string LocLocalizabilityAttribute = "Attributes";

		// Token: 0x04000A3D RID: 2621
		private static LocComments.EnumNameIndexTable ReadabilityIndexTable = new LocComments.EnumNameIndexTable("Readability.", new string[]
		{
			"Unreadable",
			"Readable",
			"Inherit"
		});

		// Token: 0x04000A3E RID: 2622
		private static LocComments.EnumNameIndexTable ModifiabilityIndexTable = new LocComments.EnumNameIndexTable("Modifiability.", new string[]
		{
			"Unmodifiable",
			"Modifiable",
			"Inherit"
		});

		// Token: 0x04000A3F RID: 2623
		private static LocComments.EnumNameIndexTable LocalizationCategoryIndexTable = new LocComments.EnumNameIndexTable("LocalizationCategory.", new string[]
		{
			"None",
			"Text",
			"Title",
			"Label",
			"Button",
			"CheckBox",
			"ComboBox",
			"ListBox",
			"Menu",
			"RadioButton",
			"ToolTip",
			"Hyperlink",
			"TextFlow",
			"XmlData",
			"Font",
			"Inherit",
			"Ignore",
			"NeverLocalize"
		});

		// Token: 0x020009CE RID: 2510
		private class EnumNameIndexTable
		{
			// Token: 0x060083F3 RID: 33779 RVA: 0x00324BAD File Offset: 0x00323BAD
			internal EnumNameIndexTable(string enumPrefix, string[] enumNames)
			{
				this._enumPrefix = enumPrefix;
				this._enumNames = enumNames;
			}

			// Token: 0x060083F4 RID: 33780 RVA: 0x00324BC4 File Offset: 0x00323BC4
			internal bool TryGet(string enumName, out int enumIndex)
			{
				enumIndex = 0;
				if (enumName.StartsWith(this._enumPrefix, StringComparison.Ordinal))
				{
					enumName = enumName.Substring(this._enumPrefix.Length);
				}
				for (int i = 0; i < this._enumNames.Length; i++)
				{
					if (string.Compare(enumName, this._enumNames[i], StringComparison.Ordinal) == 0)
					{
						enumIndex = i;
						return true;
					}
				}
				return false;
			}

			// Token: 0x04003FB6 RID: 16310
			private string _enumPrefix;

			// Token: 0x04003FB7 RID: 16311
			private string[] _enumNames;
		}
	}
}
