using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows.Input
{
	// Token: 0x02000427 RID: 1063
	internal class CommandValueSerializer : ValueSerializer
	{
		// Token: 0x06003366 RID: 13158 RVA: 0x001D69CC File Offset: 0x001D59CC
		public override bool CanConvertToString(object value, IValueSerializerContext context)
		{
			if (context == null || context.GetValueSerializerFor(typeof(Type)) == null)
			{
				return false;
			}
			RoutedCommand routedCommand = value as RoutedCommand;
			if (routedCommand == null || routedCommand.OwnerType == null)
			{
				return false;
			}
			if (CommandConverter.IsKnownType(routedCommand.OwnerType))
			{
				return true;
			}
			string name = routedCommand.Name + "Command";
			Type ownerType = routedCommand.OwnerType;
			string name2 = ownerType.Name;
			return ownerType.GetProperty(name, BindingFlags.Static | BindingFlags.Public) != null || ownerType.GetField(name, BindingFlags.Static | BindingFlags.Public) != null;
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanConvertFromString(string value, IValueSerializerContext context)
		{
			return true;
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x001D6A60 File Offset: 0x001D5A60
		public override string ConvertToString(object value, IValueSerializerContext context)
		{
			if (value == null)
			{
				return string.Empty;
			}
			RoutedCommand routedCommand = value as RoutedCommand;
			if (routedCommand == null || !(null != routedCommand.OwnerType))
			{
				throw base.GetConvertToException(value, typeof(string));
			}
			if (CommandConverter.IsKnownType(routedCommand.OwnerType))
			{
				return routedCommand.Name;
			}
			if (context == null)
			{
				throw new InvalidOperationException(SR.Get("ValueSerializerContextUnavailable", new object[]
				{
					base.GetType().Name
				}));
			}
			ValueSerializer valueSerializerFor = context.GetValueSerializerFor(typeof(Type));
			if (valueSerializerFor == null)
			{
				throw new InvalidOperationException(SR.Get("TypeValueSerializerUnavailable", new object[]
				{
					base.GetType().Name
				}));
			}
			return valueSerializerFor.ConvertToString(routedCommand.OwnerType, context) + "." + routedCommand.Name + "Command";
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x001D6B40 File Offset: 0x001D5B40
		public override IEnumerable<Type> TypeReferences(object value, IValueSerializerContext context)
		{
			if (value != null)
			{
				RoutedCommand routedCommand = value as RoutedCommand;
				if (routedCommand != null && routedCommand.OwnerType != null && !CommandConverter.IsKnownType(routedCommand.OwnerType))
				{
					return new Type[]
					{
						routedCommand.OwnerType
					};
				}
			}
			return base.TypeReferences(value, context);
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x001D6B90 File Offset: 0x001D5B90
		public override object ConvertFromString(string value, IValueSerializerContext context)
		{
			if (value != null)
			{
				if (!(value != string.Empty))
				{
					return null;
				}
				Type ownerType = null;
				int num = value.IndexOf('.');
				string localName;
				if (num >= 0)
				{
					string value2 = value.Substring(0, num);
					if (context == null)
					{
						throw new InvalidOperationException(SR.Get("ValueSerializerContextUnavailable", new object[]
						{
							base.GetType().Name
						}));
					}
					ValueSerializer valueSerializerFor = context.GetValueSerializerFor(typeof(Type));
					if (valueSerializerFor == null)
					{
						throw new InvalidOperationException(SR.Get("TypeValueSerializerUnavailable", new object[]
						{
							base.GetType().Name
						}));
					}
					ownerType = (valueSerializerFor.ConvertFromString(value2, context) as Type);
					localName = value.Substring(num + 1).Trim();
				}
				else
				{
					localName = value.Trim();
				}
				ICommand command = CommandConverter.ConvertFromHelper(ownerType, localName);
				if (command != null)
				{
					return command;
				}
			}
			return base.ConvertFromString(value, context);
		}
	}
}
