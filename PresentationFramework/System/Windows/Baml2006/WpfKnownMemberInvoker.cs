using System;
using System.Reflection;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000416 RID: 1046
	internal class WpfKnownMemberInvoker : XamlMemberInvoker
	{
		// Token: 0x06003229 RID: 12841 RVA: 0x001D0F88 File Offset: 0x001CFF88
		public WpfKnownMemberInvoker(WpfKnownMember member) : base(member)
		{
			this._member = member;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x001D0F98 File Offset: 0x001CFF98
		public override object GetValue(object instance)
		{
			if (this._member.DependencyProperty != null)
			{
				return ((DependencyObject)instance).GetValue(this._member.DependencyProperty);
			}
			return this._member.GetDelegate(instance);
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x001D0FCF File Offset: 0x001CFFCF
		public override void SetValue(object instance, object value)
		{
			if (this._member.DependencyProperty != null)
			{
				((DependencyObject)instance).SetValue(this._member.DependencyProperty, value);
				return;
			}
			this._member.SetDelegate(instance, value);
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x001D1008 File Offset: 0x001D0008
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

		// Token: 0x04001BCF RID: 7119
		private WpfKnownMember _member;

		// Token: 0x04001BD0 RID: 7120
		private bool _hasShouldSerializeMethodBeenLookedup;

		// Token: 0x04001BD1 RID: 7121
		private MethodInfo _shouldSerializeMethod;
	}
}
