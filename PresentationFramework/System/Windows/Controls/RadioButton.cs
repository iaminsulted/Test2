using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007C6 RID: 1990
	[Localizability(LocalizationCategory.RadioButton)]
	public class RadioButton : ToggleButton
	{
		// Token: 0x060071F2 RID: 29170 RVA: 0x002DC580 File Offset: 0x002DB580
		static RadioButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(typeof(RadioButton)));
			RadioButton._dType = DependencyObjectType.FromSystemTypeInternal(typeof(RadioButton));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.RadioButton);
		}

		// Token: 0x060071F4 RID: 29172 RVA: 0x002DC630 File Offset: 0x002DB630
		private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RadioButton radioButton = (RadioButton)d;
			string text = e.NewValue as string;
			string value = RadioButton._currentlyRegisteredGroupName.GetValue(radioButton);
			if (text != value)
			{
				if (!string.IsNullOrEmpty(value))
				{
					RadioButton.Unregister(value, radioButton);
				}
				if (!string.IsNullOrEmpty(text))
				{
					RadioButton.Register(text, radioButton);
				}
			}
		}

		// Token: 0x060071F5 RID: 29173 RVA: 0x002DC684 File Offset: 0x002DB684
		private static void Register(string groupName, RadioButton radioButton)
		{
			if (RadioButton._groupNameToElements == null)
			{
				RadioButton._groupNameToElements = new Hashtable(1);
			}
			Hashtable groupNameToElements = RadioButton._groupNameToElements;
			lock (groupNameToElements)
			{
				ArrayList arrayList = (ArrayList)RadioButton._groupNameToElements[groupName];
				if (arrayList == null)
				{
					arrayList = new ArrayList(1);
					RadioButton._groupNameToElements[groupName] = arrayList;
				}
				else
				{
					RadioButton.PurgeDead(arrayList, null);
				}
				arrayList.Add(new WeakReference(radioButton));
			}
			RadioButton._currentlyRegisteredGroupName.SetValue(radioButton, groupName);
		}

		// Token: 0x060071F6 RID: 29174 RVA: 0x002DC718 File Offset: 0x002DB718
		private static void Unregister(string groupName, RadioButton radioButton)
		{
			if (RadioButton._groupNameToElements == null)
			{
				return;
			}
			Hashtable groupNameToElements = RadioButton._groupNameToElements;
			lock (groupNameToElements)
			{
				ArrayList arrayList = (ArrayList)RadioButton._groupNameToElements[groupName];
				if (arrayList != null)
				{
					RadioButton.PurgeDead(arrayList, radioButton);
					if (arrayList.Count == 0)
					{
						RadioButton._groupNameToElements.Remove(groupName);
					}
				}
			}
			RadioButton._currentlyRegisteredGroupName.SetValue(radioButton, null);
		}

		// Token: 0x060071F7 RID: 29175 RVA: 0x002DC794 File Offset: 0x002DB794
		private static void PurgeDead(ArrayList elements, object elementToRemove)
		{
			int i = 0;
			while (i < elements.Count)
			{
				object target = ((WeakReference)elements[i]).Target;
				if (target == null || target == elementToRemove)
				{
					elements.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060071F8 RID: 29176 RVA: 0x002DC7D4 File Offset: 0x002DB7D4
		private void UpdateRadioButtonGroup()
		{
			string groupName = this.GroupName;
			if (!string.IsNullOrEmpty(groupName))
			{
				Visual visualRoot = KeyboardNavigation.GetVisualRoot(this);
				if (RadioButton._groupNameToElements == null)
				{
					RadioButton._groupNameToElements = new Hashtable(1);
				}
				Hashtable groupNameToElements = RadioButton._groupNameToElements;
				lock (groupNameToElements)
				{
					ArrayList arrayList = (ArrayList)RadioButton._groupNameToElements[groupName];
					int i = 0;
					while (i < arrayList.Count)
					{
						RadioButton radioButton = ((WeakReference)arrayList[i]).Target as RadioButton;
						if (radioButton == null)
						{
							arrayList.RemoveAt(i);
						}
						else
						{
							if (radioButton != this)
							{
								bool? isChecked = radioButton.IsChecked;
								bool flag2 = true;
								if ((isChecked.GetValueOrDefault() == flag2 & isChecked != null) && visualRoot == KeyboardNavigation.GetVisualRoot(radioButton))
								{
									radioButton.UncheckRadioButton();
								}
							}
							i++;
						}
					}
					return;
				}
			}
			DependencyObject parent = base.Parent;
			if (parent != null)
			{
				foreach (object obj in LogicalTreeHelper.GetChildren(parent))
				{
					RadioButton radioButton2 = obj as RadioButton;
					if (radioButton2 != null && radioButton2 != this && string.IsNullOrEmpty(radioButton2.GroupName))
					{
						bool? isChecked = radioButton2.IsChecked;
						bool flag = true;
						if (isChecked.GetValueOrDefault() == flag & isChecked != null)
						{
							radioButton2.UncheckRadioButton();
						}
					}
				}
			}
		}

		// Token: 0x060071F9 RID: 29177 RVA: 0x002DC930 File Offset: 0x002DB930
		private void UncheckRadioButton()
		{
			base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x060071FA RID: 29178 RVA: 0x002DC942 File Offset: 0x002DB942
		// (set) Token: 0x060071FB RID: 29179 RVA: 0x002DC954 File Offset: 0x002DB954
		[DefaultValue("")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string GroupName
		{
			get
			{
				return (string)base.GetValue(RadioButton.GroupNameProperty);
			}
			set
			{
				base.SetValue(RadioButton.GroupNameProperty, value);
			}
		}

		// Token: 0x060071FC RID: 29180 RVA: 0x002DC962 File Offset: 0x002DB962
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RadioButtonAutomationPeer(this);
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x002DC96A File Offset: 0x002DB96A
		protected override void OnChecked(RoutedEventArgs e)
		{
			this.UpdateRadioButtonGroup();
			base.OnChecked(e);
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x002DC979 File Offset: 0x002DB979
		protected internal override void OnToggle()
		{
			base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.TrueBox);
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x0029A353 File Offset: 0x00299353
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (!base.IsKeyboardFocused)
			{
				base.Focus();
			}
			base.OnAccessKey(e);
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x06007200 RID: 29184 RVA: 0x002DC98B File Offset: 0x002DB98B
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return RadioButton._dType;
			}
		}

		// Token: 0x0400374A RID: 14154
		public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(RadioButton), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(RadioButton.OnGroupNameChanged)));

		// Token: 0x0400374B RID: 14155
		private static DependencyObjectType _dType;

		// Token: 0x0400374C RID: 14156
		[ThreadStatic]
		private static Hashtable _groupNameToElements;

		// Token: 0x0400374D RID: 14157
		private static readonly UncommonField<string> _currentlyRegisteredGroupName = new UncommonField<string>();
	}
}
