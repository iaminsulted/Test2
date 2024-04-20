using System;
using System.Collections;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020007DA RID: 2010
	public sealed class SpellCheck
	{
		// Token: 0x060073AD RID: 29613 RVA: 0x002E3F14 File Offset: 0x002E2F14
		internal SpellCheck(TextBoxBase owner)
		{
			this._owner = owner;
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x060073AE RID: 29614 RVA: 0x002E3F23 File Offset: 0x002E2F23
		// (set) Token: 0x060073AF RID: 29615 RVA: 0x002E3F3A File Offset: 0x002E2F3A
		public bool IsEnabled
		{
			get
			{
				return (bool)this._owner.GetValue(SpellCheck.IsEnabledProperty);
			}
			set
			{
				this._owner.SetValue(SpellCheck.IsEnabledProperty, value);
			}
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x002E3F4D File Offset: 0x002E2F4D
		public static void SetIsEnabled(TextBoxBase textBoxBase, bool value)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			textBoxBase.SetValue(SpellCheck.IsEnabledProperty, value);
		}

		// Token: 0x060073B1 RID: 29617 RVA: 0x002E3F69 File Offset: 0x002E2F69
		public static bool GetIsEnabled(TextBoxBase textBoxBase)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			return (bool)textBoxBase.GetValue(SpellCheck.IsEnabledProperty);
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x060073B2 RID: 29618 RVA: 0x002E3F89 File Offset: 0x002E2F89
		// (set) Token: 0x060073B3 RID: 29619 RVA: 0x002E3FA0 File Offset: 0x002E2FA0
		public SpellingReform SpellingReform
		{
			get
			{
				return (SpellingReform)this._owner.GetValue(SpellCheck.SpellingReformProperty);
			}
			set
			{
				this._owner.SetValue(SpellCheck.SpellingReformProperty, value);
			}
		}

		// Token: 0x060073B4 RID: 29620 RVA: 0x002E3FB8 File Offset: 0x002E2FB8
		public static void SetSpellingReform(TextBoxBase textBoxBase, SpellingReform value)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			textBoxBase.SetValue(SpellCheck.SpellingReformProperty, value);
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x060073B5 RID: 29621 RVA: 0x002E3FD9 File Offset: 0x002E2FD9
		public IList CustomDictionaries
		{
			get
			{
				return (IList)this._owner.GetValue(SpellCheck.CustomDictionariesProperty);
			}
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x002E3FF0 File Offset: 0x002E2FF0
		public static IList GetCustomDictionaries(TextBoxBase textBoxBase)
		{
			if (textBoxBase == null)
			{
				throw new ArgumentNullException("textBoxBase");
			}
			return (IList)textBoxBase.GetValue(SpellCheck.CustomDictionariesProperty);
		}

		// Token: 0x060073B7 RID: 29623 RVA: 0x002E4010 File Offset: 0x002E3010
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(textBoxBase);
				if (textEditor != null)
				{
					textEditor.SetSpellCheckEnabled((bool)e.NewValue);
					if ((bool)e.NewValue != (bool)e.OldValue)
					{
						textEditor.SetCustomDictionaries((bool)e.NewValue);
					}
				}
			}
		}

		// Token: 0x060073B8 RID: 29624 RVA: 0x002E4070 File Offset: 0x002E3070
		private static void OnSpellingReformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null)
			{
				TextEditor textEditor = TextEditor._GetTextEditor(textBoxBase);
				if (textEditor != null)
				{
					textEditor.SetSpellingReform((SpellingReform)e.NewValue);
				}
			}
		}

		// Token: 0x040037E2 RID: 14306
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SpellCheck), new FrameworkPropertyMetadata(new PropertyChangedCallback(SpellCheck.OnIsEnabledChanged)));

		// Token: 0x040037E3 RID: 14307
		public static readonly DependencyProperty SpellingReformProperty = DependencyProperty.RegisterAttached("SpellingReform", typeof(SpellingReform), typeof(SpellCheck), new FrameworkPropertyMetadata((Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "de") ? SpellingReform.Postreform : SpellingReform.PreAndPostreform, new PropertyChangedCallback(SpellCheck.OnSpellingReformChanged)));

		// Token: 0x040037E4 RID: 14308
		private static readonly DependencyPropertyKey CustomDictionariesPropertyKey = DependencyProperty.RegisterAttachedReadOnly("CustomDictionaries", typeof(IList), typeof(SpellCheck), new FrameworkPropertyMetadata(new SpellCheck.DictionaryCollectionFactory()));

		// Token: 0x040037E5 RID: 14309
		public static readonly DependencyProperty CustomDictionariesProperty = SpellCheck.CustomDictionariesPropertyKey.DependencyProperty;

		// Token: 0x040037E6 RID: 14310
		private readonly TextBoxBase _owner;

		// Token: 0x02000C20 RID: 3104
		internal class DictionaryCollectionFactory : DefaultValueFactory
		{
			// Token: 0x06009091 RID: 37009 RVA: 0x00130842 File Offset: 0x0012F842
			internal DictionaryCollectionFactory()
			{
			}

			// Token: 0x17001F97 RID: 8087
			// (get) Token: 0x06009092 RID: 37010 RVA: 0x00109403 File Offset: 0x00108403
			internal override object DefaultValue
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06009093 RID: 37011 RVA: 0x00346CE9 File Offset: 0x00345CE9
			internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
			{
				return new CustomDictionarySources(owner as TextBoxBase);
			}
		}
	}
}
