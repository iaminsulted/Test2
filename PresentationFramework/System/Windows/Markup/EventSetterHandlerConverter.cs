using System;
using System.ComponentModel;
using System.Globalization;
using System.Xaml;

namespace System.Windows.Markup
{
	// Token: 0x02000470 RID: 1136
	public sealed class EventSetterHandlerConverter : TypeConverter
	{
		// Token: 0x06003A7A RID: 14970 RVA: 0x001832F5 File Offset: 0x001822F5
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return false;
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x001F0C50 File Offset: 0x001EFC50
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (typeDescriptorContext == null)
			{
				throw new ArgumentNullException("typeDescriptorContext");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (EventSetterHandlerConverter.s_ServiceProviderContextType == null)
			{
				EventSetterHandlerConverter.s_ServiceProviderContextType = typeof(IRootObjectProvider).Assembly.GetType("MS.Internal.Xaml.ServiceProviderContext");
			}
			if (typeDescriptorContext.GetType() != EventSetterHandlerConverter.s_ServiceProviderContextType)
			{
				throw new ArgumentException(SR.Get("TextRange_InvalidParameterValue"), "typeDescriptorContext");
			}
			IRootObjectProvider rootObjectProvider = typeDescriptorContext.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootObjectProvider != null && source is string)
			{
				IProvideValueTarget provideValueTarget = typeDescriptorContext.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
				if (provideValueTarget != null)
				{
					EventSetter eventSetter = provideValueTarget.TargetObject as EventSetter;
					string text;
					if (eventSetter != null && (text = (source as string)) != null)
					{
						text = text.Trim();
						return Delegate.CreateDelegate(eventSetter.Event.HandlerType, rootObjectProvider.RootObject, text);
					}
				}
			}
			throw base.GetConvertFromException(source);
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x001F0D44 File Offset: 0x001EFD44
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x04001DAC RID: 7596
		private static Type s_ServiceProviderContextType;
	}
}
