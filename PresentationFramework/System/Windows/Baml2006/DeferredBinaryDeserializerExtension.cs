using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;

namespace System.Windows.Baml2006
{
	// Token: 0x02000410 RID: 1040
	internal class DeferredBinaryDeserializerExtension : MarkupExtension
	{
		// Token: 0x06002D33 RID: 11571 RVA: 0x001AB554 File Offset: 0x001AA554
		public DeferredBinaryDeserializerExtension(IFreezeFreezables freezer, BinaryReader reader, int converterId, int dataByteSize)
		{
			this._freezer = freezer;
			this._canFreeze = freezer.FreezeFreezables;
			byte[] buffer = reader.ReadBytes(dataByteSize);
			this._stream = new MemoryStream(buffer);
			this._reader = new BinaryReader(this._stream);
			this._converterId = converterId;
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x001AB5A8 File Offset: 0x001AA5A8
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			this._stream.Position = 0L;
			switch (this._converterId)
			{
			case 744:
				return SolidColorBrush.DeserializeFrom(this._reader, new DeferredBinaryDeserializerExtension.DeferredBinaryDeserializerExtensionContext(serviceProvider, this._freezer, this._canFreeze));
			case 746:
				return Parsers.DeserializeStreamGeometry(this._reader);
			case 747:
				return Point3DCollection.DeserializeFrom(this._reader);
			case 748:
				return PointCollection.DeserializeFrom(this._reader);
			case 752:
				return Vector3DCollection.DeserializeFrom(this._reader);
			}
			throw new NotImplementedException();
		}

		// Token: 0x04001BAA RID: 7082
		private IFreezeFreezables _freezer;

		// Token: 0x04001BAB RID: 7083
		private bool _canFreeze;

		// Token: 0x04001BAC RID: 7084
		private readonly BinaryReader _reader;

		// Token: 0x04001BAD RID: 7085
		private readonly Stream _stream;

		// Token: 0x04001BAE RID: 7086
		private readonly int _converterId;

		// Token: 0x02000AB0 RID: 2736
		private class DeferredBinaryDeserializerExtensionContext : ITypeDescriptorContext, IServiceProvider, IFreezeFreezables
		{
			// Token: 0x0600874C RID: 34636 RVA: 0x003360C3 File Offset: 0x003350C3
			public DeferredBinaryDeserializerExtensionContext(IServiceProvider serviceProvider, IFreezeFreezables freezer, bool canFreeze)
			{
				this._freezer = freezer;
				this._canFreeze = canFreeze;
				this._serviceProvider = serviceProvider;
			}

			// Token: 0x0600874D RID: 34637 RVA: 0x003360E0 File Offset: 0x003350E0
			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IFreezeFreezables))
				{
					return this;
				}
				return this._serviceProvider.GetService(serviceType);
			}

			// Token: 0x0600874E RID: 34638 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			// Token: 0x0600874F RID: 34639 RVA: 0x00105F35 File Offset: 0x00104F35
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			// Token: 0x17001E58 RID: 7768
			// (get) Token: 0x06008750 RID: 34640 RVA: 0x00109403 File Offset: 0x00108403
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001E59 RID: 7769
			// (get) Token: 0x06008751 RID: 34641 RVA: 0x00109403 File Offset: 0x00108403
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001E5A RID: 7770
			// (get) Token: 0x06008752 RID: 34642 RVA: 0x00109403 File Offset: 0x00108403
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001E5B RID: 7771
			// (get) Token: 0x06008753 RID: 34643 RVA: 0x00336102 File Offset: 0x00335102
			bool IFreezeFreezables.FreezeFreezables
			{
				get
				{
					return this._canFreeze;
				}
			}

			// Token: 0x06008754 RID: 34644 RVA: 0x0033610A File Offset: 0x0033510A
			bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
			{
				return this._freezer.TryFreeze(value, freezable);
			}

			// Token: 0x06008755 RID: 34645 RVA: 0x00336119 File Offset: 0x00335119
			Freezable IFreezeFreezables.TryGetFreezable(string value)
			{
				return this._freezer.TryGetFreezable(value);
			}

			// Token: 0x040042C8 RID: 17096
			private IServiceProvider _serviceProvider;

			// Token: 0x040042C9 RID: 17097
			private IFreezeFreezables _freezer;

			// Token: 0x040042CA RID: 17098
			private bool _canFreeze;
		}
	}
}
