using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace MS.Internal.Markup
{
	// Token: 0x02000158 RID: 344
	internal class StaticExtension : StaticExtension
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x0012C6BE File Offset: 0x0012B6BE
		public StaticExtension()
		{
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0012C6C6 File Offset: 0x0012B6C6
		public StaticExtension(string member) : base(member)
		{
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0012C6D0 File Offset: 0x0012B6D0
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (base.Member == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionStaticMember"));
			}
			object obj;
			if (base.MemberType != null)
			{
				obj = SystemResourceKey.GetSystemResourceKey(base.MemberType.Name + "." + base.Member);
				if (obj != null)
				{
					return obj;
				}
			}
			else
			{
				obj = SystemResourceKey.GetSystemResourceKey(base.Member);
				if (obj != null)
				{
					return obj;
				}
				int num = base.Member.IndexOf('.');
				if (num < 0)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionBadStatic", new object[]
					{
						base.Member
					}));
				}
				string text = base.Member.Substring(0, num);
				if (text == string.Empty)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionBadStatic", new object[]
					{
						base.Member
					}));
				}
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}
				IXamlTypeResolver xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
				if (xamlTypeResolver == null)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionNoContext", new object[]
					{
						base.GetType().Name,
						"IXamlTypeResolver"
					}));
				}
				base.MemberType = xamlTypeResolver.Resolve(text);
				base.Member = base.Member.Substring(num + 1, base.Member.Length - num - 1);
			}
			obj = CommandConverter.GetKnownControlCommand(base.MemberType, base.Member);
			if (obj != null)
			{
				return obj;
			}
			return base.ProvideValue(serviceProvider);
		}
	}
}
