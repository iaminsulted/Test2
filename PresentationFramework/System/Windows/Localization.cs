using System;
using System.Runtime.CompilerServices;
using MS.Internal.Globalization;

namespace System.Windows
{
	// Token: 0x0200037C RID: 892
	public static class Localization
	{
		// Token: 0x06002419 RID: 9241 RVA: 0x0018178E File Offset: 0x0018078E
		[AttachedPropertyBrowsableForType(typeof(object))]
		public static string GetComments(object element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return Localization.GetValue(element, Localization.CommentsProperty);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x001817A9 File Offset: 0x001807A9
		public static void SetComments(object element, string comments)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			LocComments.ParsePropertyComments(comments);
			Localization.SetValue(element, Localization.CommentsProperty, comments);
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x001817CC File Offset: 0x001807CC
		[AttachedPropertyBrowsableForType(typeof(object))]
		public static string GetAttributes(object element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return Localization.GetValue(element, Localization.AttributesProperty);
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x001817E7 File Offset: 0x001807E7
		public static void SetAttributes(object element, string attributes)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			LocComments.ParsePropertyLocalizabilityAttributes(attributes);
			Localization.SetValue(element, Localization.AttributesProperty, attributes);
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0018180C File Offset: 0x0018080C
		private static string GetValue(object element, DependencyProperty property)
		{
			DependencyObject dependencyObject = element as DependencyObject;
			if (dependencyObject != null)
			{
				return (string)dependencyObject.GetValue(property);
			}
			string result;
			if (property == Localization.CommentsProperty)
			{
				Localization._commentsOnObjects.TryGetValue(element, out result);
			}
			else
			{
				Localization._attributesOnObjects.TryGetValue(element, out result);
			}
			return result;
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x00181858 File Offset: 0x00180858
		private static void SetValue(object element, DependencyProperty property, string value)
		{
			DependencyObject dependencyObject = element as DependencyObject;
			if (dependencyObject != null)
			{
				dependencyObject.SetValue(property, value);
				return;
			}
			if (property == Localization.CommentsProperty)
			{
				Localization._commentsOnObjects.Remove(element);
				Localization._commentsOnObjects.Add(element, value);
				return;
			}
			Localization._attributesOnObjects.Remove(element);
			Localization._attributesOnObjects.Add(element, value);
		}

		// Token: 0x04001110 RID: 4368
		public static readonly DependencyProperty CommentsProperty = DependencyProperty.RegisterAttached("Comments", typeof(string), typeof(Localization));

		// Token: 0x04001111 RID: 4369
		public static readonly DependencyProperty AttributesProperty = DependencyProperty.RegisterAttached("Attributes", typeof(string), typeof(Localization));

		// Token: 0x04001112 RID: 4370
		private static ConditionalWeakTable<object, string> _commentsOnObjects = new ConditionalWeakTable<object, string>();

		// Token: 0x04001113 RID: 4371
		private static ConditionalWeakTable<object, string> _attributesOnObjects = new ConditionalWeakTable<object, string>();
	}
}
