using System;
using System.Collections.Generic;
using MS.Internal.WindowsBase;

namespace System.Windows.Markup
{
	// Token: 0x020004D3 RID: 1235
	internal class RoutedEventValueSerializer : ValueSerializer
	{
		// Token: 0x06003F44 RID: 16196 RVA: 0x00210C2B File Offset: 0x0020FC2B
		public override bool CanConvertToString(object value, IValueSerializerContext context)
		{
			return ValueSerializer.GetSerializerFor(typeof(Type), context) != null;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x00210C2B File Offset: 0x0020FC2B
		public override bool CanConvertFromString(string value, IValueSerializerContext context)
		{
			return ValueSerializer.GetSerializerFor(typeof(Type), context) != null;
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x00210C40 File Offset: 0x0020FC40
		public override string ConvertToString(object value, IValueSerializerContext context)
		{
			RoutedEvent routedEvent = value as RoutedEvent;
			if (routedEvent != null)
			{
				ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(typeof(Type), context);
				if (serializerFor != null)
				{
					return serializerFor.ConvertToString(routedEvent.OwnerType, context) + "." + routedEvent.Name;
				}
			}
			return base.ConvertToString(value, context);
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x00210C91 File Offset: 0x0020FC91
		private static void ForceTypeConstructors(Type currentType)
		{
			while (currentType != null && !RoutedEventValueSerializer.initializedTypes.ContainsKey(currentType))
			{
				SecurityHelper.RunClassConstructor(currentType);
				RoutedEventValueSerializer.initializedTypes[currentType] = currentType;
				currentType = currentType.BaseType;
			}
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x00210CC8 File Offset: 0x0020FCC8
		public override object ConvertFromString(string value, IValueSerializerContext context)
		{
			ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(typeof(Type), context);
			if (serializerFor != null)
			{
				int num = value.IndexOf('.');
				if (num > 0)
				{
					Type type = serializerFor.ConvertFromString(value.Substring(0, num), context) as Type;
					string name = value.Substring(num + 1).Trim();
					RoutedEventValueSerializer.ForceTypeConstructors(type);
					return EventManager.GetRoutedEventFromName(name, type);
				}
			}
			return base.ConvertFromString(value, context);
		}

		// Token: 0x04002365 RID: 9061
		private static Dictionary<Type, Type> initializedTypes = new Dictionary<Type, Type>();
	}
}
