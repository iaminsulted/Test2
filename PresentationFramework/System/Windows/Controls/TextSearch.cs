using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Win32;

namespace System.Windows.Controls
{
	// Token: 0x020007EA RID: 2026
	public sealed class TextSearch : DependencyObject
	{
		// Token: 0x06007542 RID: 30018 RVA: 0x002EB0B7 File Offset: 0x002EA0B7
		private TextSearch(ItemsControl itemsControl)
		{
			if (itemsControl == null)
			{
				throw new ArgumentNullException("itemsControl");
			}
			this._attachedTo = itemsControl;
			this.ResetState();
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x002EB0DC File Offset: 0x002EA0DC
		internal static TextSearch EnsureInstance(ItemsControl itemsControl)
		{
			TextSearch textSearch = (TextSearch)itemsControl.GetValue(TextSearch.TextSearchInstanceProperty);
			if (textSearch == null)
			{
				textSearch = new TextSearch(itemsControl);
				itemsControl.SetValue(TextSearch.TextSearchInstancePropertyKey, textSearch);
			}
			return textSearch;
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x002EB111 File Offset: 0x002EA111
		public static void SetTextPath(DependencyObject element, string path)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextSearch.TextPathProperty, path);
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x002EB12D File Offset: 0x002EA12D
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetTextPath(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (string)element.GetValue(TextSearch.TextPathProperty);
		}

		// Token: 0x06007546 RID: 30022 RVA: 0x002EB14D File Offset: 0x002EA14D
		public static void SetText(DependencyObject element, string text)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextSearch.TextProperty, text);
		}

		// Token: 0x06007547 RID: 30023 RVA: 0x002EB169 File Offset: 0x002EA169
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetText(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (string)element.GetValue(TextSearch.TextProperty);
		}

		// Token: 0x06007548 RID: 30024 RVA: 0x002EB18C File Offset: 0x002EA18C
		internal bool DoSearch(string nextChar)
		{
			bool lookForFallbackMatchToo = false;
			int num = 0;
			ItemCollection items = this._attachedTo.Items;
			if (this.IsActive)
			{
				num = this.MatchedItemIndex;
			}
			if (this._charsEntered.Count > 0 && string.Compare(this._charsEntered[this._charsEntered.Count - 1], nextChar, true, TextSearch.GetCulture(this._attachedTo)) == 0)
			{
				lookForFallbackMatchToo = true;
			}
			string primaryTextPath = TextSearch.GetPrimaryTextPath(this._attachedTo);
			bool flag = false;
			int num2 = TextSearch.FindMatchingPrefix(this._attachedTo, primaryTextPath, this.Prefix, nextChar, num, lookForFallbackMatchToo, ref flag);
			if (num2 != -1)
			{
				if (!this.IsActive || num2 != num)
				{
					object item = items[num2];
					this._attachedTo.NavigateToItem(item, num2, new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, ModifierKeys.None));
					this.MatchedItemIndex = num2;
				}
				if (flag)
				{
					this.AddCharToPrefix(nextChar);
				}
				if (!this.IsActive)
				{
					this.IsActive = true;
				}
			}
			if (this.IsActive)
			{
				this.ResetTimeout();
			}
			return num2 != -1;
		}

		// Token: 0x06007549 RID: 30025 RVA: 0x002EB28C File Offset: 0x002EA28C
		internal bool DeleteLastCharacter()
		{
			if (this.IsActive && this._charsEntered.Count > 0)
			{
				string text = this._charsEntered[this._charsEntered.Count - 1];
				string prefix = this.Prefix;
				this._charsEntered.RemoveAt(this._charsEntered.Count - 1);
				this.Prefix = prefix.Substring(0, prefix.Length - text.Length);
				this.ResetTimeout();
				return true;
			}
			return false;
		}

		// Token: 0x0600754A RID: 30026 RVA: 0x002EB30C File Offset: 0x002EA30C
		private static void GetMatchingPrefixAndRemainingTextLength(string matchedText, string newText, CultureInfo cultureInfo, bool ignoreCase, out int matchedPrefixLength, out int textExcludingPrefixLength)
		{
			matchedPrefixLength = 0;
			textExcludingPrefixLength = 0;
			if (matchedText.Length < newText.Length)
			{
				matchedPrefixLength = matchedText.Length;
				textExcludingPrefixLength = 0;
				return;
			}
			int num = newText.Length;
			int num2 = num + 1;
			for (;;)
			{
				if (num >= 1)
				{
					string strB = matchedText.Substring(0, num);
					if (string.Compare(newText, strB, ignoreCase, cultureInfo) == 0)
					{
						break;
					}
				}
				if (num2 <= matchedText.Length)
				{
					string strB = matchedText.Substring(0, num2);
					if (string.Compare(newText, strB, ignoreCase, cultureInfo) == 0)
					{
						goto Block_5;
					}
				}
				num--;
				num2++;
				if (num < 1 && num2 > matchedText.Length)
				{
					return;
				}
			}
			matchedPrefixLength = num;
			textExcludingPrefixLength = matchedText.Length - num;
			return;
			Block_5:
			matchedPrefixLength = num2;
			textExcludingPrefixLength = matchedText.Length - num2;
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x002EB3B4 File Offset: 0x002EA3B4
		private static int FindMatchingPrefix(ItemsControl itemsControl, string primaryTextPath, string prefix, string newChar, int startItemIndex, bool lookForFallbackMatchToo, ref bool wasNewCharUsed)
		{
			ItemCollection items = itemsControl.Items;
			int num = -1;
			int num2 = -1;
			int count = items.Count;
			if (count == 0)
			{
				return -1;
			}
			string value = prefix + newChar;
			if (string.IsNullOrEmpty(value))
			{
				return -1;
			}
			BindingExpression bindingExpression = null;
			object item = itemsControl.Items[0];
			if (SystemXmlHelper.IsXmlNode(item) || !string.IsNullOrEmpty(primaryTextPath))
			{
				bindingExpression = TextSearch.CreateBindingExpression(itemsControl, item, primaryTextPath);
				TextSearch.TextValueBindingExpression.SetValue(itemsControl, bindingExpression);
			}
			bool flag = true;
			wasNewCharUsed = false;
			CultureInfo culture = TextSearch.GetCulture(itemsControl);
			int i = startItemIndex;
			while (i < count)
			{
				object obj = items[i];
				if (obj != null)
				{
					string primaryText = TextSearch.GetPrimaryText(obj, bindingExpression, itemsControl);
					bool isTextSearchCaseSensitive = itemsControl.IsTextSearchCaseSensitive;
					if (primaryText != null && primaryText.StartsWith(value, !isTextSearchCaseSensitive, culture))
					{
						wasNewCharUsed = true;
						num = i;
						break;
					}
					if (lookForFallbackMatchToo)
					{
						if (!flag && prefix != string.Empty)
						{
							if (primaryText != null && num2 == -1 && primaryText.StartsWith(prefix, !isTextSearchCaseSensitive, culture))
							{
								num2 = i;
							}
						}
						else
						{
							flag = false;
						}
					}
				}
				i++;
				if (i >= count)
				{
					i = 0;
				}
				if (i == startItemIndex)
				{
					break;
				}
			}
			if (bindingExpression != null)
			{
				TextSearch.TextValueBindingExpression.ClearValue(itemsControl);
			}
			if (num == -1 && num2 != -1)
			{
				num = num2;
			}
			return num;
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x002EB4EC File Offset: 0x002EA4EC
		internal static MatchedTextInfo FindMatchingPrefix(ItemsControl itemsControl, string prefix)
		{
			bool flag = false;
			int num = TextSearch.FindMatchingPrefix(itemsControl, TextSearch.GetPrimaryTextPath(itemsControl), prefix, string.Empty, 0, false, ref flag);
			MatchedTextInfo result;
			if (num >= 0)
			{
				CultureInfo culture = TextSearch.GetCulture(itemsControl);
				bool isTextSearchCaseSensitive = itemsControl.IsTextSearchCaseSensitive;
				string primaryTextFromItem = TextSearch.GetPrimaryTextFromItem(itemsControl, itemsControl.Items[num]);
				int matchedPrefixLength;
				int textExcludingPrefixLength;
				TextSearch.GetMatchingPrefixAndRemainingTextLength(primaryTextFromItem, prefix, culture, !isTextSearchCaseSensitive, out matchedPrefixLength, out textExcludingPrefixLength);
				result = new MatchedTextInfo(num, primaryTextFromItem, matchedPrefixLength, textExcludingPrefixLength);
			}
			else
			{
				result = MatchedTextInfo.NoMatch;
			}
			return result;
		}

		// Token: 0x0600754D RID: 30029 RVA: 0x002EB564 File Offset: 0x002EA564
		private void ResetTimeout()
		{
			if (this._timeoutTimer == null)
			{
				this._timeoutTimer = new DispatcherTimer(DispatcherPriority.Normal);
				this._timeoutTimer.Tick += this.OnTimeout;
			}
			else
			{
				this._timeoutTimer.Stop();
			}
			this._timeoutTimer.Interval = this.TimeOut;
			this._timeoutTimer.Start();
		}

		// Token: 0x0600754E RID: 30030 RVA: 0x002EB5C6 File Offset: 0x002EA5C6
		private void AddCharToPrefix(string newChar)
		{
			this.Prefix += newChar;
			this._charsEntered.Add(newChar);
		}

		// Token: 0x0600754F RID: 30031 RVA: 0x002EB5E8 File Offset: 0x002EA5E8
		private static string GetPrimaryTextPath(ItemsControl itemsControl)
		{
			string text = (string)itemsControl.GetValue(TextSearch.TextPathProperty);
			if (string.IsNullOrEmpty(text))
			{
				text = itemsControl.DisplayMemberPath;
			}
			return text;
		}

		// Token: 0x06007550 RID: 30032 RVA: 0x002EB618 File Offset: 0x002EA618
		private static string GetPrimaryText(object item, BindingExpression primaryTextBinding, DependencyObject primaryTextBindingHome)
		{
			DependencyObject dependencyObject = item as DependencyObject;
			if (dependencyObject != null)
			{
				string text = (string)dependencyObject.GetValue(TextSearch.TextProperty);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			if (primaryTextBinding != null && primaryTextBindingHome != null)
			{
				primaryTextBinding.Activate(item);
				return TextSearch.ConvertToPlainText(primaryTextBinding.Value);
			}
			return TextSearch.ConvertToPlainText(item);
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x002EB66C File Offset: 0x002EA66C
		private static string ConvertToPlainText(object o)
		{
			FrameworkElement frameworkElement = o as FrameworkElement;
			if (frameworkElement != null)
			{
				string plainText = frameworkElement.GetPlainText();
				if (plainText != null)
				{
					return plainText;
				}
			}
			if (o == null)
			{
				return string.Empty;
			}
			return o.ToString();
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x002EB6A0 File Offset: 0x002EA6A0
		internal static string GetPrimaryTextFromItem(ItemsControl itemsControl, object item)
		{
			if (item == null)
			{
				return string.Empty;
			}
			BindingExpression bindingExpression = TextSearch.CreateBindingExpression(itemsControl, item, TextSearch.GetPrimaryTextPath(itemsControl));
			TextSearch.TextValueBindingExpression.SetValue(itemsControl, bindingExpression);
			string primaryText = TextSearch.GetPrimaryText(item, bindingExpression, itemsControl);
			TextSearch.TextValueBindingExpression.ClearValue(itemsControl);
			return primaryText;
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x002EB6E4 File Offset: 0x002EA6E4
		private static BindingExpression CreateBindingExpression(ItemsControl itemsControl, object item, string primaryTextPath)
		{
			Binding binding = new Binding();
			if (SystemXmlHelper.IsXmlNode(item))
			{
				binding.XPath = primaryTextPath;
				binding.Path = new PropertyPath("/InnerText", Array.Empty<object>());
			}
			else
			{
				binding.Path = new PropertyPath(primaryTextPath, Array.Empty<object>());
			}
			binding.Mode = BindingMode.OneWay;
			binding.Source = null;
			return (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(itemsControl, binding);
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x002EB748 File Offset: 0x002EA748
		private void OnTimeout(object sender, EventArgs e)
		{
			this.ResetState();
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x002EB750 File Offset: 0x002EA750
		private void ResetState()
		{
			this.IsActive = false;
			this.Prefix = string.Empty;
			this.MatchedItemIndex = -1;
			if (this._charsEntered == null)
			{
				this._charsEntered = new List<string>(10);
			}
			else
			{
				this._charsEntered.Clear();
			}
			if (this._timeoutTimer != null)
			{
				this._timeoutTimer.Stop();
			}
			this._timeoutTimer = null;
		}

		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x06007556 RID: 30038 RVA: 0x002EB7B2 File Offset: 0x002EA7B2
		private TimeSpan TimeOut
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)(SafeNativeMethods.GetDoubleClickTime() * 2));
			}
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x002EB7C1 File Offset: 0x002EA7C1
		private static TextSearch GetInstance(DependencyObject d)
		{
			return TextSearch.EnsureInstance(d as ItemsControl);
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x002EB7CE File Offset: 0x002EA7CE
		private void TypeAKey(string c)
		{
			this.DoSearch(c);
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x002EB7D8 File Offset: 0x002EA7D8
		private void CauseTimeOut()
		{
			if (this._timeoutTimer != null)
			{
				this._timeoutTimer.Stop();
				this.OnTimeout(this._timeoutTimer, EventArgs.Empty);
			}
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x002EB7FE File Offset: 0x002EA7FE
		internal string GetCurrentPrefix()
		{
			return this.Prefix;
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x002EB808 File Offset: 0x002EA808
		internal static string GetPrimaryText(FrameworkElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			string text = (string)element.GetValue(TextSearch.TextProperty);
			if (text != null && text != string.Empty)
			{
				return text;
			}
			return element.GetPlainText();
		}

		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x0600755C RID: 30044 RVA: 0x002EB84C File Offset: 0x002EA84C
		// (set) Token: 0x0600755D RID: 30045 RVA: 0x002EB854 File Offset: 0x002EA854
		private string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x0600755E RID: 30046 RVA: 0x002EB85D File Offset: 0x002EA85D
		// (set) Token: 0x0600755F RID: 30047 RVA: 0x002EB865 File Offset: 0x002EA865
		private bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				this._isActive = value;
			}
		}

		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x06007560 RID: 30048 RVA: 0x002EB86E File Offset: 0x002EA86E
		// (set) Token: 0x06007561 RID: 30049 RVA: 0x002EB876 File Offset: 0x002EA876
		private int MatchedItemIndex
		{
			get
			{
				return this._matchedItemIndex;
			}
			set
			{
				this._matchedItemIndex = value;
			}
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x002EB880 File Offset: 0x002EA880
		private static CultureInfo GetCulture(DependencyObject element)
		{
			object value = element.GetValue(FrameworkElement.LanguageProperty);
			CultureInfo result = null;
			if (value != null)
			{
				XmlLanguage xmlLanguage = (XmlLanguage)value;
				try
				{
					result = xmlLanguage.GetSpecificCulture();
				}
				catch (InvalidOperationException)
				{
				}
			}
			return result;
		}

		// Token: 0x0400383D RID: 14397
		public static readonly DependencyProperty TextPathProperty = DependencyProperty.RegisterAttached("TextPath", typeof(string), typeof(TextSearch), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x0400383E RID: 14398
		public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(TextSearch), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		// Token: 0x0400383F RID: 14399
		private static readonly DependencyProperty CurrentPrefixProperty = DependencyProperty.RegisterAttached("CurrentPrefix", typeof(string), typeof(TextSearch), new FrameworkPropertyMetadata(null));

		// Token: 0x04003840 RID: 14400
		private static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(TextSearch), new FrameworkPropertyMetadata(false));

		// Token: 0x04003841 RID: 14401
		private static readonly DependencyPropertyKey TextSearchInstancePropertyKey = DependencyProperty.RegisterAttachedReadOnly("TextSearchInstance", typeof(TextSearch), typeof(TextSearch), new FrameworkPropertyMetadata(null));

		// Token: 0x04003842 RID: 14402
		private static readonly DependencyProperty TextSearchInstanceProperty = TextSearch.TextSearchInstancePropertyKey.DependencyProperty;

		// Token: 0x04003843 RID: 14403
		private static readonly BindingExpressionUncommonField TextValueBindingExpression = new BindingExpressionUncommonField();

		// Token: 0x04003844 RID: 14404
		private ItemsControl _attachedTo;

		// Token: 0x04003845 RID: 14405
		private string _prefix;

		// Token: 0x04003846 RID: 14406
		private List<string> _charsEntered;

		// Token: 0x04003847 RID: 14407
		private bool _isActive;

		// Token: 0x04003848 RID: 14408
		private int _matchedItemIndex;

		// Token: 0x04003849 RID: 14409
		private DispatcherTimer _timeoutTimer;
	}
}
