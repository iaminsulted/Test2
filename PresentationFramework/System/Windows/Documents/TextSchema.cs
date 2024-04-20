using System;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006BE RID: 1726
	internal static class TextSchema
	{
		// Token: 0x06005951 RID: 22865 RVA: 0x0027AD60 File Offset: 0x00279D60
		static TextSchema()
		{
			DependencyProperty[] array = new DependencyProperty[]
			{
				FrameworkElement.LanguageProperty,
				FrameworkElement.FlowDirectionProperty,
				NumberSubstitution.CultureSourceProperty,
				NumberSubstitution.SubstitutionProperty,
				NumberSubstitution.CultureOverrideProperty,
				TextElement.FontFamilyProperty,
				TextElement.FontStyleProperty,
				TextElement.FontWeightProperty,
				TextElement.FontStretchProperty,
				TextElement.FontSizeProperty,
				TextElement.ForegroundProperty
			};
			TextSchema._inheritableTextElementProperties = new DependencyProperty[array.Length + Typography.TypographyPropertiesList.Length];
			Array.Copy(array, 0, TextSchema._inheritableTextElementProperties, 0, array.Length);
			Array.Copy(Typography.TypographyPropertiesList, 0, TextSchema._inheritableTextElementProperties, array.Length, Typography.TypographyPropertiesList.Length);
			DependencyProperty[] array2 = new DependencyProperty[]
			{
				Block.TextAlignmentProperty,
				Block.LineHeightProperty,
				Block.IsHyphenationEnabledProperty
			};
			TextSchema._inheritableBlockProperties = new DependencyProperty[array2.Length + TextSchema._inheritableTextElementProperties.Length];
			Array.Copy(array2, 0, TextSchema._inheritableBlockProperties, 0, array2.Length);
			Array.Copy(TextSchema._inheritableTextElementProperties, 0, TextSchema._inheritableBlockProperties, array2.Length, TextSchema._inheritableTextElementProperties.Length);
			DependencyProperty[] array3 = new DependencyProperty[]
			{
				Block.TextAlignmentProperty
			};
			TextSchema._inheritableTableCellProperties = new DependencyProperty[array3.Length + TextSchema._inheritableTextElementProperties.Length];
			Array.Copy(array3, TextSchema._inheritableTableCellProperties, array3.Length);
			Array.Copy(TextSchema._inheritableTextElementProperties, 0, TextSchema._inheritableTableCellProperties, array3.Length, TextSchema._inheritableTextElementProperties.Length);
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x0027B33F File Offset: 0x0027A33F
		internal static bool IsInTextContent(ITextPointer position)
		{
			return TextSchema.IsValidChild(position, typeof(string));
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x0027B354 File Offset: 0x0027A354
		internal static bool ValidateChild(TextElement parent, TextElement child, bool throwIfIllegalChild, bool throwIfIllegalHyperlinkDescendent)
		{
			if (TextSchema.HasHyperlinkAncestor(parent) && TextSchema.HasIllegalHyperlinkDescendant(child, throwIfIllegalHyperlinkDescendent))
			{
				return false;
			}
			bool flag = TextSchema.IsValidChild(parent.GetType(), child.GetType());
			if (!flag && throwIfIllegalChild)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					parent.GetType().Name,
					child.GetType().Name
				}));
			}
			return flag;
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x0027B3C1 File Offset: 0x0027A3C1
		internal static bool IsValidChild(TextElement parent, Type childType)
		{
			return TextSchema.ValidateChild(parent, childType, false, false);
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x0027B3CC File Offset: 0x0027A3CC
		internal static bool ValidateChild(TextElement parent, Type childType, bool throwIfIllegalChild, bool throwIfIllegalHyperlinkDescendent)
		{
			if (TextSchema.HasHyperlinkAncestor(parent) && (typeof(Hyperlink).IsAssignableFrom(childType) || typeof(AnchoredBlock).IsAssignableFrom(childType)))
			{
				if (throwIfIllegalHyperlinkDescendent)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_IllegalHyperlinkChild", new object[]
					{
						childType
					}));
				}
				return false;
			}
			else
			{
				bool flag = TextSchema.IsValidChild(parent.GetType(), childType);
				if (!flag && throwIfIllegalChild)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
					{
						parent.GetType().Name,
						childType.Name
					}));
				}
				return flag;
			}
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x0027B467 File Offset: 0x0027A467
		internal static bool IsValidChild(TextPointer position, Type childType)
		{
			return TextSchema.ValidateChild(position, childType, false, false);
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x0027B474 File Offset: 0x0027A474
		internal static bool ValidateChild(TextPointer position, Type childType, bool throwIfIllegalChild, bool throwIfIllegalHyperlinkDescendent)
		{
			DependencyObject parent = position.Parent;
			if (parent == null)
			{
				TextElement adjacentElementFromOuterPosition = position.GetAdjacentElementFromOuterPosition(LogicalDirection.Backward);
				TextElement adjacentElementFromOuterPosition2 = position.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				return (adjacentElementFromOuterPosition == null || TextSchema.IsValidSibling(adjacentElementFromOuterPosition.GetType(), childType)) && (adjacentElementFromOuterPosition2 == null || TextSchema.IsValidSibling(adjacentElementFromOuterPosition2.GetType(), childType));
			}
			if (parent is TextElement)
			{
				return TextSchema.ValidateChild((TextElement)parent, childType, throwIfIllegalChild, throwIfIllegalHyperlinkDescendent);
			}
			bool flag = TextSchema.IsValidChild(parent.GetType(), childType);
			if (!flag && throwIfIllegalChild)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					parent.GetType().Name,
					childType.Name
				}));
			}
			return flag;
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x0027B51C File Offset: 0x0027A51C
		internal static bool IsValidSibling(Type siblingType, Type newType)
		{
			if (typeof(Inline).IsAssignableFrom(newType))
			{
				return typeof(Inline).IsAssignableFrom(siblingType);
			}
			if (typeof(Block).IsAssignableFrom(newType))
			{
				return typeof(Block).IsAssignableFrom(siblingType);
			}
			if (typeof(TableRowGroup).IsAssignableFrom(newType))
			{
				return typeof(TableRowGroup).IsAssignableFrom(siblingType);
			}
			if (typeof(TableRow).IsAssignableFrom(newType))
			{
				return typeof(TableRow).IsAssignableFrom(siblingType);
			}
			if (typeof(TableCell).IsAssignableFrom(newType))
			{
				return typeof(TableCell).IsAssignableFrom(siblingType);
			}
			if (typeof(ListItem).IsAssignableFrom(newType))
			{
				return typeof(ListItem).IsAssignableFrom(siblingType);
			}
			Invariant.Assert(false, "unexpected value for newType");
			return false;
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x0027B608 File Offset: 0x0027A608
		internal static bool IsValidChild(ITextPointer position, Type childType)
		{
			return (!typeof(TextElement).IsAssignableFrom(position.ParentType) || !TextPointerBase.IsInHyperlinkScope(position) || (!typeof(Hyperlink).IsAssignableFrom(childType) && !typeof(AnchoredBlock).IsAssignableFrom(childType))) && TextSchema.IsValidChild(position.ParentType, childType);
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x0027B666 File Offset: 0x0027A666
		internal static bool IsValidChildOfContainer(Type parentType, Type childType)
		{
			Invariant.Assert(!typeof(TextElement).IsAssignableFrom(parentType));
			return TextSchema.IsValidChild(parentType, childType);
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x0027B688 File Offset: 0x0027A688
		internal static bool HasHyperlinkAncestor(TextElement element)
		{
			Inline inline = element as Inline;
			while (inline != null && !(inline is Hyperlink))
			{
				inline = (inline.Parent as Inline);
			}
			return inline != null;
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x0027B6B9 File Offset: 0x0027A6B9
		internal static bool IsFormattingType(Type elementType)
		{
			return typeof(Run).IsAssignableFrom(elementType) || typeof(Span).IsAssignableFrom(elementType);
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x0027B6DF File Offset: 0x0027A6DF
		internal static bool IsKnownType(Type elementType)
		{
			return elementType.Module == typeof(TextElement).Module || elementType.Module == typeof(UIElement).Module;
		}

		// Token: 0x0600595E RID: 22878 RVA: 0x0027B719 File Offset: 0x0027A719
		internal static bool IsNonFormattingInline(Type elementType)
		{
			return typeof(Inline).IsAssignableFrom(elementType) && !TextSchema.IsFormattingType(elementType);
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x0027B738 File Offset: 0x0027A738
		internal static bool IsMergeableInline(Type elementType)
		{
			return TextSchema.IsFormattingType(elementType) && !TextSchema.IsNonMergeableInline(elementType);
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x0027B750 File Offset: 0x0027A750
		internal static bool IsNonMergeableInline(Type elementType)
		{
			TextElementEditingBehaviorAttribute textElementEditingBehaviorAttribute = (TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(elementType, typeof(TextElementEditingBehaviorAttribute));
			return textElementEditingBehaviorAttribute != null && !textElementEditingBehaviorAttribute.IsMergeable;
		}

		// Token: 0x06005961 RID: 22881 RVA: 0x0027B784 File Offset: 0x0027A784
		internal static bool AllowsParagraphMerging(Type elementType)
		{
			return typeof(Paragraph).IsAssignableFrom(elementType) || typeof(ListItem).IsAssignableFrom(elementType) || typeof(List).IsAssignableFrom(elementType) || typeof(Section).IsAssignableFrom(elementType);
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x0027B7D9 File Offset: 0x0027A7D9
		internal static bool IsParagraphOrBlockUIContainer(Type elementType)
		{
			return typeof(Paragraph).IsAssignableFrom(elementType) || typeof(BlockUIContainer).IsAssignableFrom(elementType);
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x0027B7FF File Offset: 0x0027A7FF
		internal static bool IsBlock(Type type)
		{
			return typeof(Block).IsAssignableFrom(type);
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x0027B811 File Offset: 0x0027A811
		internal static bool IsBreak(Type type)
		{
			return typeof(LineBreak).IsAssignableFrom(type);
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x0027B823 File Offset: 0x0027A823
		internal static bool HasTextDecorations(object value)
		{
			return value is TextDecorationCollection && ((TextDecorationCollection)value).Count > 0;
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x0027B840 File Offset: 0x0027A840
		internal static Type GetStandardElementType(Type type, bool reduceElement)
		{
			if (typeof(Run).IsAssignableFrom(type))
			{
				return typeof(Run);
			}
			if (typeof(Hyperlink).IsAssignableFrom(type))
			{
				return typeof(Hyperlink);
			}
			if (typeof(Span).IsAssignableFrom(type))
			{
				return typeof(Span);
			}
			if (typeof(InlineUIContainer).IsAssignableFrom(type))
			{
				if (!reduceElement)
				{
					return typeof(InlineUIContainer);
				}
				return typeof(Run);
			}
			else
			{
				if (typeof(LineBreak).IsAssignableFrom(type))
				{
					return typeof(LineBreak);
				}
				if (typeof(Floater).IsAssignableFrom(type))
				{
					return typeof(Floater);
				}
				if (typeof(Figure).IsAssignableFrom(type))
				{
					return typeof(Figure);
				}
				if (typeof(Paragraph).IsAssignableFrom(type))
				{
					return typeof(Paragraph);
				}
				if (typeof(Section).IsAssignableFrom(type))
				{
					return typeof(Section);
				}
				if (typeof(List).IsAssignableFrom(type))
				{
					return typeof(List);
				}
				if (typeof(Table).IsAssignableFrom(type))
				{
					return typeof(Table);
				}
				if (typeof(BlockUIContainer).IsAssignableFrom(type))
				{
					if (!reduceElement)
					{
						return typeof(BlockUIContainer);
					}
					return typeof(Paragraph);
				}
				else
				{
					if (typeof(ListItem).IsAssignableFrom(type))
					{
						return typeof(ListItem);
					}
					if (typeof(TableColumn).IsAssignableFrom(type))
					{
						return typeof(TableColumn);
					}
					if (typeof(TableRowGroup).IsAssignableFrom(type))
					{
						return typeof(TableRowGroup);
					}
					if (typeof(TableRow).IsAssignableFrom(type))
					{
						return typeof(TableRow);
					}
					if (typeof(TableCell).IsAssignableFrom(type))
					{
						return typeof(TableCell);
					}
					Invariant.Assert(false, "We do not expect any unknown elements derived directly from TextElement, Block or Inline. Schema must have been checking for that");
					return null;
				}
			}
		}

		// Token: 0x06005967 RID: 22887 RVA: 0x0027BA64 File Offset: 0x0027AA64
		internal static DependencyProperty[] GetInheritableProperties(Type type)
		{
			if (typeof(TableCell).IsAssignableFrom(type))
			{
				return TextSchema._inheritableTableCellProperties;
			}
			if (typeof(Block).IsAssignableFrom(type) || typeof(FlowDocument).IsAssignableFrom(type))
			{
				return TextSchema._inheritableBlockProperties;
			}
			Invariant.Assert(typeof(TextElement).IsAssignableFrom(type) || typeof(TableColumn).IsAssignableFrom(type), "type must be one of TextElement, FlowDocument or TableColumn");
			return TextSchema._inheritableTextElementProperties;
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x0027BAE8 File Offset: 0x0027AAE8
		internal static DependencyProperty[] GetNoninheritableProperties(Type type)
		{
			if (typeof(Run).IsAssignableFrom(type))
			{
				return TextSchema._inlineProperties;
			}
			if (typeof(Hyperlink).IsAssignableFrom(type))
			{
				return TextSchema._hyperlinkProperties;
			}
			if (typeof(Span).IsAssignableFrom(type))
			{
				return TextSchema._inlineProperties;
			}
			if (typeof(InlineUIContainer).IsAssignableFrom(type))
			{
				return TextSchema._inlineProperties;
			}
			if (typeof(LineBreak).IsAssignableFrom(type))
			{
				return TextSchema._emptyPropertyList;
			}
			if (typeof(Floater).IsAssignableFrom(type))
			{
				return TextSchema._floaterProperties;
			}
			if (typeof(Figure).IsAssignableFrom(type))
			{
				return TextSchema._figureProperties;
			}
			if (typeof(Paragraph).IsAssignableFrom(type))
			{
				return TextSchema._paragraphProperties;
			}
			if (typeof(Section).IsAssignableFrom(type))
			{
				return TextSchema._blockProperties;
			}
			if (typeof(List).IsAssignableFrom(type))
			{
				return TextSchema._listProperties;
			}
			if (typeof(Table).IsAssignableFrom(type))
			{
				return TextSchema._tableProperties;
			}
			if (typeof(BlockUIContainer).IsAssignableFrom(type))
			{
				return TextSchema._blockProperties;
			}
			if (typeof(ListItem).IsAssignableFrom(type))
			{
				return TextSchema._listItemProperties;
			}
			if (typeof(TableColumn).IsAssignableFrom(type))
			{
				return TextSchema._tableColumnProperties;
			}
			if (typeof(TableRowGroup).IsAssignableFrom(type))
			{
				return TextSchema._tableRowGroupProperties;
			}
			if (typeof(TableRow).IsAssignableFrom(type))
			{
				return TextSchema._tableRowProperties;
			}
			if (typeof(TableCell).IsAssignableFrom(type))
			{
				return TextSchema._tableCellProperties;
			}
			Invariant.Assert(false, "We do not expect any unknown elements derived directly from TextElement. Schema must have been checking for that");
			return TextSchema._emptyPropertyList;
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x0027BCA0 File Offset: 0x0027ACA0
		internal static bool ValuesAreEqual(object value1, object value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			if (value1 == null)
			{
				if (value2 is TextDecorationCollection)
				{
					return ((TextDecorationCollection)value2).Count == 0;
				}
				return value2 is TextEffectCollection && ((TextEffectCollection)value2).Count == 0;
			}
			else if (value2 == null)
			{
				if (value1 is TextDecorationCollection)
				{
					return ((TextDecorationCollection)value1).Count == 0;
				}
				return value1 is TextEffectCollection && ((TextEffectCollection)value1).Count == 0;
			}
			else
			{
				if (value1.GetType() != value2.GetType())
				{
					return false;
				}
				if (value1 is TextDecorationCollection)
				{
					TextDecorationCollection textDecorationCollection = (TextDecorationCollection)value1;
					TextDecorationCollection textDecorations = (TextDecorationCollection)value2;
					return textDecorationCollection.ValueEquals(textDecorations);
				}
				if (value1 is FontFamily)
				{
					object obj = (FontFamily)value1;
					FontFamily obj2 = (FontFamily)value2;
					return obj.Equals(obj2);
				}
				if (value1 is Brush)
				{
					return TextSchema.AreBrushesEqual((Brush)value1, (Brush)value2);
				}
				string a = value1.ToString();
				string b = value2.ToString();
				return a == b;
			}
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x0027BD94 File Offset: 0x0027AD94
		internal static bool IsParagraphProperty(DependencyProperty formattingProperty)
		{
			for (int i = 0; i < TextSchema._inheritableBlockProperties.Length; i++)
			{
				if (formattingProperty == TextSchema._inheritableBlockProperties[i])
				{
					return true;
				}
			}
			for (int j = 0; j < TextSchema._paragraphProperties.Length; j++)
			{
				if (formattingProperty == TextSchema._paragraphProperties[j])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x0027BDE0 File Offset: 0x0027ADE0
		internal static bool IsCharacterProperty(DependencyProperty formattingProperty)
		{
			for (int i = 0; i < TextSchema._inheritableTextElementProperties.Length; i++)
			{
				if (formattingProperty == TextSchema._inheritableTextElementProperties[i])
				{
					return true;
				}
			}
			for (int j = 0; j < TextSchema._inlineProperties.Length; j++)
			{
				if (formattingProperty == TextSchema._inlineProperties[j])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x0027BE2C File Offset: 0x0027AE2C
		internal static bool IsNonFormattingCharacterProperty(DependencyProperty property)
		{
			for (int i = 0; i < TextSchema._nonFormattingCharacterProperties.Length; i++)
			{
				if (property == TextSchema._nonFormattingCharacterProperties[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x0027BE58 File Offset: 0x0027AE58
		internal static DependencyProperty[] GetNonFormattingCharacterProperties()
		{
			return TextSchema._nonFormattingCharacterProperties;
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x0027BE60 File Offset: 0x0027AE60
		internal static bool IsStructuralCharacterProperty(DependencyProperty formattingProperty)
		{
			int num = 0;
			while (num < TextSchema._structuralCharacterProperties.Length && formattingProperty != TextSchema._structuralCharacterProperties[num])
			{
				num++;
			}
			return num < TextSchema._structuralCharacterProperties.Length;
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x0027BE94 File Offset: 0x0027AE94
		internal static bool IsPropertyIncremental(DependencyProperty property)
		{
			if (property == null)
			{
				return false;
			}
			Type propertyType = property.PropertyType;
			return typeof(double).IsAssignableFrom(propertyType) || typeof(long).IsAssignableFrom(propertyType) || typeof(int).IsAssignableFrom(propertyType) || typeof(Thickness).IsAssignableFrom(propertyType);
		}

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x06005970 RID: 22896 RVA: 0x0027BEF5 File Offset: 0x0027AEF5
		internal static DependencyProperty[] BehavioralProperties
		{
			get
			{
				return TextSchema._behavioralPropertyList;
			}
		}

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x06005971 RID: 22897 RVA: 0x0027BEFC File Offset: 0x0027AEFC
		internal static DependencyProperty[] ImageProperties
		{
			get
			{
				return TextSchema._imagePropertyList;
			}
		}

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x06005972 RID: 22898 RVA: 0x0027BF03 File Offset: 0x0027AF03
		internal static DependencyProperty[] StructuralCharacterProperties
		{
			get
			{
				return TextSchema._structuralCharacterProperties;
			}
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x0027BF0C File Offset: 0x0027AF0C
		private static bool IsValidChild(Type parentType, Type childType)
		{
			if (parentType == null || typeof(Run).IsAssignableFrom(parentType) || typeof(TextBox).IsAssignableFrom(parentType) || typeof(PasswordBox).IsAssignableFrom(parentType))
			{
				return childType == typeof(string);
			}
			if (typeof(TextBlock).IsAssignableFrom(parentType))
			{
				return typeof(Inline).IsAssignableFrom(childType) && !typeof(AnchoredBlock).IsAssignableFrom(childType);
			}
			if (typeof(Hyperlink).IsAssignableFrom(parentType))
			{
				return typeof(Inline).IsAssignableFrom(childType) && !typeof(Hyperlink).IsAssignableFrom(childType) && !typeof(AnchoredBlock).IsAssignableFrom(childType);
			}
			if (typeof(Span).IsAssignableFrom(parentType) || typeof(Paragraph).IsAssignableFrom(parentType) || typeof(AccessText).IsAssignableFrom(parentType))
			{
				return typeof(Inline).IsAssignableFrom(childType);
			}
			if (typeof(InlineUIContainer).IsAssignableFrom(parentType))
			{
				return typeof(UIElement).IsAssignableFrom(childType);
			}
			if (typeof(List).IsAssignableFrom(parentType))
			{
				return typeof(ListItem).IsAssignableFrom(childType);
			}
			if (typeof(Table).IsAssignableFrom(parentType))
			{
				return typeof(TableRowGroup).IsAssignableFrom(childType);
			}
			if (typeof(TableRowGroup).IsAssignableFrom(parentType))
			{
				return typeof(TableRow).IsAssignableFrom(childType);
			}
			if (typeof(TableRow).IsAssignableFrom(parentType))
			{
				return typeof(TableCell).IsAssignableFrom(childType);
			}
			if (typeof(Section).IsAssignableFrom(parentType) || typeof(ListItem).IsAssignableFrom(parentType) || typeof(TableCell).IsAssignableFrom(parentType) || typeof(Floater).IsAssignableFrom(parentType) || typeof(Figure).IsAssignableFrom(parentType) || typeof(FlowDocument).IsAssignableFrom(parentType))
			{
				return typeof(Block).IsAssignableFrom(childType);
			}
			return typeof(BlockUIContainer).IsAssignableFrom(parentType) && typeof(UIElement).IsAssignableFrom(childType);
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x0027C188 File Offset: 0x0027B188
		private static bool HasIllegalHyperlinkDescendant(TextElement element, bool throwIfIllegalDescendent)
		{
			TextPointer textPointer = element.ElementStart;
			TextPointer elementEnd = element.ElementEnd;
			while (textPointer.CompareTo(elementEnd) < 0)
			{
				if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					TextElement textElement = (TextElement)textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if (textElement is Hyperlink || textElement is AnchoredBlock)
					{
						if (throwIfIllegalDescendent)
						{
							throw new InvalidOperationException(SR.Get("TextSchema_IllegalHyperlinkChild", new object[]
							{
								textElement.GetType()
							}));
						}
						return true;
					}
				}
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return false;
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x0027C204 File Offset: 0x0027B204
		private static bool AreBrushesEqual(Brush brush1, Brush brush2)
		{
			SolidColorBrush solidColorBrush = brush1 as SolidColorBrush;
			if (solidColorBrush != null)
			{
				return solidColorBrush.Color.Equals(((SolidColorBrush)brush2).Color);
			}
			string stringValue = DPTypeDescriptorContext.GetStringValue(TextElement.BackgroundProperty, brush1);
			string stringValue2 = DPTypeDescriptorContext.GetStringValue(TextElement.BackgroundProperty, brush2);
			return stringValue != null && stringValue2 != null && stringValue == stringValue2;
		}

		// Token: 0x04002FEC RID: 12268
		private static readonly DependencyProperty[] _inheritableTextElementProperties;

		// Token: 0x04002FED RID: 12269
		private static readonly DependencyProperty[] _inheritableBlockProperties;

		// Token: 0x04002FEE RID: 12270
		private static readonly DependencyProperty[] _inheritableTableCellProperties;

		// Token: 0x04002FEF RID: 12271
		private static readonly DependencyProperty[] _hyperlinkProperties = new DependencyProperty[]
		{
			Hyperlink.NavigateUriProperty,
			Hyperlink.TargetNameProperty,
			Hyperlink.CommandProperty,
			Hyperlink.CommandParameterProperty,
			Hyperlink.CommandTargetProperty,
			Inline.BaselineAlignmentProperty,
			Inline.TextDecorationsProperty,
			TextElement.BackgroundProperty,
			FrameworkContentElement.ToolTipProperty
		};

		// Token: 0x04002FF0 RID: 12272
		private static readonly DependencyProperty[] _inlineProperties = new DependencyProperty[]
		{
			Inline.BaselineAlignmentProperty,
			Inline.TextDecorationsProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF1 RID: 12273
		private static readonly DependencyProperty[] _paragraphProperties = new DependencyProperty[]
		{
			Paragraph.MinWidowLinesProperty,
			Paragraph.MinOrphanLinesProperty,
			Paragraph.TextIndentProperty,
			Paragraph.KeepWithNextProperty,
			Paragraph.KeepTogetherProperty,
			Paragraph.TextDecorationsProperty,
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF2 RID: 12274
		private static readonly DependencyProperty[] _listProperties = new DependencyProperty[]
		{
			List.MarkerStyleProperty,
			List.MarkerOffsetProperty,
			List.StartIndexProperty,
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF3 RID: 12275
		private static readonly DependencyProperty[] _listItemProperties = new DependencyProperty[]
		{
			ListItem.MarginProperty,
			ListItem.PaddingProperty,
			ListItem.BorderThicknessProperty,
			ListItem.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF4 RID: 12276
		private static readonly DependencyProperty[] _tableProperties = new DependencyProperty[]
		{
			Table.CellSpacingProperty,
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF5 RID: 12277
		private static readonly DependencyProperty[] _tableColumnProperties = new DependencyProperty[]
		{
			TableColumn.WidthProperty,
			TableColumn.BackgroundProperty
		};

		// Token: 0x04002FF6 RID: 12278
		private static readonly DependencyProperty[] _tableRowGroupProperties = new DependencyProperty[]
		{
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF7 RID: 12279
		private static readonly DependencyProperty[] _tableRowProperties = new DependencyProperty[]
		{
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF8 RID: 12280
		private static readonly DependencyProperty[] _tableCellProperties = new DependencyProperty[]
		{
			TableCell.ColumnSpanProperty,
			TableCell.RowSpanProperty,
			TableCell.PaddingProperty,
			TableCell.BorderThicknessProperty,
			TableCell.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FF9 RID: 12281
		private static readonly DependencyProperty[] _floaterProperties = new DependencyProperty[]
		{
			Floater.HorizontalAlignmentProperty,
			Floater.WidthProperty,
			AnchoredBlock.MarginProperty,
			AnchoredBlock.PaddingProperty,
			AnchoredBlock.BorderThicknessProperty,
			AnchoredBlock.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FFA RID: 12282
		private static readonly DependencyProperty[] _figureProperties = new DependencyProperty[]
		{
			Figure.HorizontalAnchorProperty,
			Figure.VerticalAnchorProperty,
			Figure.HorizontalOffsetProperty,
			Figure.VerticalOffsetProperty,
			Figure.CanDelayPlacementProperty,
			Figure.WrapDirectionProperty,
			Figure.WidthProperty,
			Figure.HeightProperty,
			AnchoredBlock.MarginProperty,
			AnchoredBlock.PaddingProperty,
			AnchoredBlock.BorderThicknessProperty,
			AnchoredBlock.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FFB RID: 12283
		private static readonly DependencyProperty[] _blockProperties = new DependencyProperty[]
		{
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			Block.BreakPageBeforeProperty,
			Block.BreakColumnBeforeProperty,
			Block.ClearFloatersProperty,
			Block.IsHyphenationEnabledProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FFC RID: 12284
		private static readonly DependencyProperty[] _textElementPropertyList = new DependencyProperty[]
		{
			TextElement.BackgroundProperty
		};

		// Token: 0x04002FFD RID: 12285
		private static readonly DependencyProperty[] _imagePropertyList = new DependencyProperty[]
		{
			Image.SourceProperty,
			Image.StretchProperty,
			Image.StretchDirectionProperty,
			FrameworkElement.LanguageProperty,
			FrameworkElement.LayoutTransformProperty,
			FrameworkElement.WidthProperty,
			FrameworkElement.MinWidthProperty,
			FrameworkElement.MaxWidthProperty,
			FrameworkElement.HeightProperty,
			FrameworkElement.MinHeightProperty,
			FrameworkElement.MaxHeightProperty,
			FrameworkElement.MarginProperty,
			FrameworkElement.HorizontalAlignmentProperty,
			FrameworkElement.VerticalAlignmentProperty,
			FrameworkElement.CursorProperty,
			FrameworkElement.ForceCursorProperty,
			FrameworkElement.ToolTipProperty,
			UIElement.RenderTransformProperty,
			UIElement.RenderTransformOriginProperty,
			UIElement.OpacityProperty,
			UIElement.OpacityMaskProperty,
			UIElement.BitmapEffectProperty,
			UIElement.BitmapEffectInputProperty,
			UIElement.VisibilityProperty,
			UIElement.ClipToBoundsProperty,
			UIElement.ClipProperty,
			UIElement.SnapsToDevicePixelsProperty,
			TextBlock.BaselineOffsetProperty
		};

		// Token: 0x04002FFE RID: 12286
		private static readonly DependencyProperty[] _behavioralPropertyList = new DependencyProperty[]
		{
			UIElement.AllowDropProperty
		};

		// Token: 0x04002FFF RID: 12287
		private static readonly DependencyProperty[] _emptyPropertyList = new DependencyProperty[0];

		// Token: 0x04003000 RID: 12288
		private static readonly DependencyProperty[] _structuralCharacterProperties = new DependencyProperty[]
		{
			Inline.FlowDirectionProperty
		};

		// Token: 0x04003001 RID: 12289
		private static readonly DependencyProperty[] _nonFormattingCharacterProperties = new DependencyProperty[]
		{
			FrameworkElement.FlowDirectionProperty,
			FrameworkElement.LanguageProperty,
			Run.TextProperty
		};
	}
}
