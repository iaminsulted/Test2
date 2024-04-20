using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Xps.Serialization;
using Microsoft.Win32;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006EB RID: 1771
	public sealed class SerializerProvider
	{
		// Token: 0x06005D0C RID: 23820 RVA: 0x0028C48C File Offset: 0x0028B48C
		public SerializerProvider()
		{
			List<SerializerDescriptor> list = new List<SerializerDescriptor>();
			SerializerDescriptor serializerDescriptor = this.CreateSystemSerializerDescriptor();
			if (serializerDescriptor != null)
			{
				list.Add(serializerDescriptor);
			}
			RegistryKey registryKey = SerializerProvider._rootKey.CreateSubKey("SOFTWARE\\Microsoft\\WinFX Serializers");
			if (registryKey != null)
			{
				foreach (string keyName in registryKey.GetSubKeyNames())
				{
					serializerDescriptor = SerializerDescriptor.CreateFromRegistry(registryKey, keyName);
					if (serializerDescriptor != null)
					{
						list.Add(serializerDescriptor);
					}
				}
				registryKey.Close();
			}
			this._installedSerializers = list.AsReadOnly();
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x0028C510 File Offset: 0x0028B510
		public static void RegisterSerializer(SerializerDescriptor serializerDescriptor, bool overwrite)
		{
			if (serializerDescriptor == null)
			{
				throw new ArgumentNullException("serializerDescriptor");
			}
			RegistryKey registryKey = SerializerProvider._rootKey.CreateSubKey("SOFTWARE\\Microsoft\\WinFX Serializers");
			string[] array = new string[7];
			array[0] = serializerDescriptor.DisplayName;
			array[1] = "/";
			array[2] = serializerDescriptor.AssemblyName;
			array[3] = "/";
			int num = 4;
			Version assemblyVersion = serializerDescriptor.AssemblyVersion;
			array[num] = ((assemblyVersion != null) ? assemblyVersion.ToString() : null);
			array[5] = "/";
			int num2 = 6;
			Version winFXVersion = serializerDescriptor.WinFXVersion;
			array[num2] = ((winFXVersion != null) ? winFXVersion.ToString() : null);
			string text = string.Concat(array);
			if (!overwrite && registryKey.OpenSubKey(text) != null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderAlreadyRegistered"), text);
			}
			RegistryKey registryKey2 = registryKey.CreateSubKey(text);
			serializerDescriptor.WriteToRegistryKey(registryKey2);
			registryKey2.Close();
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x0028C5D0 File Offset: 0x0028B5D0
		public static void UnregisterSerializer(SerializerDescriptor serializerDescriptor)
		{
			if (serializerDescriptor == null)
			{
				throw new ArgumentNullException("serializerDescriptor");
			}
			RegistryKey registryKey = SerializerProvider._rootKey.CreateSubKey("SOFTWARE\\Microsoft\\WinFX Serializers");
			string[] array = new string[7];
			array[0] = serializerDescriptor.DisplayName;
			array[1] = "/";
			array[2] = serializerDescriptor.AssemblyName;
			array[3] = "/";
			int num = 4;
			Version assemblyVersion = serializerDescriptor.AssemblyVersion;
			array[num] = ((assemblyVersion != null) ? assemblyVersion.ToString() : null);
			array[5] = "/";
			int num2 = 6;
			Version winFXVersion = serializerDescriptor.WinFXVersion;
			array[num2] = ((winFXVersion != null) ? winFXVersion.ToString() : null);
			string text = string.Concat(array);
			if (registryKey.OpenSubKey(text) == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderNotRegistered"), text);
			}
			registryKey.DeleteSubKeyTree(text);
		}

		// Token: 0x06005D0F RID: 23823 RVA: 0x0028C67C File Offset: 0x0028B67C
		public SerializerWriter CreateSerializerWriter(SerializerDescriptor serializerDescriptor, Stream stream)
		{
			SerializerWriter result = null;
			if (serializerDescriptor == null)
			{
				throw new ArgumentNullException("serializerDescriptor");
			}
			string[] array = new string[7];
			array[0] = serializerDescriptor.DisplayName;
			array[1] = "/";
			array[2] = serializerDescriptor.AssemblyName;
			array[3] = "/";
			int num = 4;
			Version assemblyVersion = serializerDescriptor.AssemblyVersion;
			array[num] = ((assemblyVersion != null) ? assemblyVersion.ToString() : null);
			array[5] = "/";
			int num2 = 6;
			Version winFXVersion = serializerDescriptor.WinFXVersion;
			array[num2] = ((winFXVersion != null) ? winFXVersion.ToString() : null);
			string paramName = string.Concat(array);
			if (!serializerDescriptor.IsLoadable)
			{
				throw new ArgumentException(SR.Get("SerializerProviderWrongVersion"), paramName);
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool flag = false;
			using (IEnumerator<SerializerDescriptor> enumerator = this.InstalledSerializers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Equals(serializerDescriptor))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				throw new ArgumentException(SR.Get("SerializerProviderUnknownSerializer"), paramName);
			}
			try
			{
				result = serializerDescriptor.CreateSerializerFactory().CreateSerializerWriter(stream);
			}
			catch (FileNotFoundException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			catch (FileLoadException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			catch (BadImageFormatException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			catch (MissingMethodException)
			{
				throw new ArgumentException(SR.Get("SerializerProviderCannotLoad"), serializerDescriptor.DisplayName);
			}
			return result;
		}

		// Token: 0x06005D10 RID: 23824 RVA: 0x0028C818 File Offset: 0x0028B818
		private SerializerDescriptor CreateSystemSerializerDescriptor()
		{
			return SerializerDescriptor.CreateFromFactoryInstance(new XpsSerializerFactory());
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x06005D11 RID: 23825 RVA: 0x0028C824 File Offset: 0x0028B824
		public ReadOnlyCollection<SerializerDescriptor> InstalledSerializers
		{
			get
			{
				return this._installedSerializers;
			}
		}

		// Token: 0x0400314D RID: 12621
		private const string _registryPath = "SOFTWARE\\Microsoft\\WinFX Serializers";

		// Token: 0x0400314E RID: 12622
		private static readonly RegistryKey _rootKey = Registry.LocalMachine;

		// Token: 0x0400314F RID: 12623
		private ReadOnlyCollection<SerializerDescriptor> _installedSerializers;
	}
}
