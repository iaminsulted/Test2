using System;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000418 RID: 1048
	internal class WpfKnownTypeInvoker : XamlTypeInvoker
	{
		// Token: 0x0600325E RID: 12894 RVA: 0x001D170A File Offset: 0x001D070A
		public WpfKnownTypeInvoker(WpfKnownType type) : base(type)
		{
			this._type = type;
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x001D171C File Offset: 0x001D071C
		public override object CreateInstance(object[] arguments)
		{
			if ((arguments == null || arguments.Length == 0) && this._type.DefaultConstructor != null)
			{
				return this._type.DefaultConstructor();
			}
			if (!this._type.IsMarkupExtension)
			{
				return base.CreateInstance(arguments);
			}
			Baml6ConstructorInfo baml6ConstructorInfo;
			if (!this._type.Constructors.TryGetValue(arguments.Length, out baml6ConstructorInfo))
			{
				throw new InvalidOperationException(SR.Get("PositionalArgumentsWrongLength"));
			}
			return baml6ConstructorInfo.Constructor(arguments);
		}

		// Token: 0x04001BE0 RID: 7136
		private WpfKnownType _type;
	}
}
