using System;
using System.Reflection;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000419 RID: 1049
	internal class WpfMemberInvoker : XamlMemberInvoker
	{
		// Token: 0x06003260 RID: 12896 RVA: 0x001D1797 File Offset: 0x001D0797
		public WpfMemberInvoker(WpfXamlMember member) : base(member)
		{
			this._member = member;
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x001D17A8 File Offset: 0x001D07A8
		public override void SetValue(object instance, object value)
		{
			DependencyObject dependencyObject = instance as DependencyObject;
			if (dependencyObject != null)
			{
				if (this._member.DependencyProperty != null)
				{
					dependencyObject.SetValue(this._member.DependencyProperty, value);
					return;
				}
				if (this._member.RoutedEvent != null)
				{
					Delegate @delegate = value as Delegate;
					if (@delegate != null)
					{
						UIElement.AddHandler(dependencyObject, this._member.RoutedEvent, @delegate);
						return;
					}
				}
			}
			base.SetValue(instance, value);
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x001D1814 File Offset: 0x001D0814
		public override object GetValue(object instance)
		{
			DependencyObject dependencyObject = instance as DependencyObject;
			if (dependencyObject != null && this._member.DependencyProperty != null)
			{
				object value = dependencyObject.GetValue(this._member.DependencyProperty);
				if (value != null)
				{
					return value;
				}
				if (!this._member.ApplyGetterFallback || this._member.UnderlyingMember == null)
				{
					return value;
				}
			}
			return base.GetValue(instance);
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x001D1878 File Offset: 0x001D0878
		public override ShouldSerializeResult ShouldSerializeValue(object instance)
		{
			if (!this._hasShouldSerializeMethodBeenLookedup)
			{
				Type declaringType = this._member.UnderlyingMember.DeclaringType;
				string name = "ShouldSerialize" + this._member.Name;
				BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				Type[] types = new Type[]
				{
					typeof(DependencyObject)
				};
				if (this._member.IsAttachable)
				{
					this._shouldSerializeMethod = declaringType.GetMethod(name, bindingFlags, null, types, null);
				}
				else
				{
					bindingFlags |= BindingFlags.Instance;
					this._shouldSerializeMethod = declaringType.GetMethod(name, bindingFlags, null, types, null);
				}
				this._hasShouldSerializeMethodBeenLookedup = true;
			}
			if (this._shouldSerializeMethod != null)
			{
				object[] parameters = new object[]
				{
					instance as DependencyObject
				};
				bool flag;
				if (this._member.IsAttachable)
				{
					flag = (bool)this._shouldSerializeMethod.Invoke(null, parameters);
				}
				else
				{
					flag = (bool)this._shouldSerializeMethod.Invoke(instance, parameters);
				}
				if (!flag)
				{
					return ShouldSerializeResult.False;
				}
				return ShouldSerializeResult.True;
			}
			else
			{
				DependencyObject dependencyObject = instance as DependencyObject;
				if (dependencyObject != null && this._member.DependencyProperty != null && !dependencyObject.ShouldSerializeProperty(this._member.DependencyProperty))
				{
					return ShouldSerializeResult.False;
				}
				return base.ShouldSerializeValue(instance);
			}
		}

		// Token: 0x04001BE1 RID: 7137
		private WpfXamlMember _member;

		// Token: 0x04001BE2 RID: 7138
		private bool _hasShouldSerializeMethodBeenLookedup;

		// Token: 0x04001BE3 RID: 7139
		private MethodInfo _shouldSerializeMethod;
	}
}
