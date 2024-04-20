using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using Microsoft.Win32;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006EA RID: 1770
	public sealed class SerializerDescriptor
	{
		// Token: 0x06005CFA RID: 23802 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private SerializerDescriptor()
		{
		}

		// Token: 0x06005CFB RID: 23803 RVA: 0x0028BF29 File Offset: 0x0028AF29
		private static string GetNonEmptyRegistryString(RegistryKey key, string value)
		{
			string text = key.GetValue(value) as string;
			if (text == null)
			{
				throw new KeyNotFoundException();
			}
			return text;
		}

		// Token: 0x06005CFC RID: 23804 RVA: 0x0028BF40 File Offset: 0x0028AF40
		public static SerializerDescriptor CreateFromFactoryInstance(ISerializerFactory factoryInstance)
		{
			if (factoryInstance == null)
			{
				throw new ArgumentNullException("factoryInstance");
			}
			if (factoryInstance.DisplayName == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderDisplayNameNull"));
			}
			if (factoryInstance.ManufacturerName == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderManufacturerNameNull"));
			}
			if (factoryInstance.ManufacturerWebsite == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderManufacturerWebsiteNull"));
			}
			if (factoryInstance.DefaultFileExtension == null)
			{
				throw new ArgumentException(SR.Get("SerializerProviderDefaultFileExtensionNull"));
			}
			SerializerDescriptor serializerDescriptor = new SerializerDescriptor();
			serializerDescriptor._displayName = factoryInstance.DisplayName;
			serializerDescriptor._manufacturerName = factoryInstance.ManufacturerName;
			serializerDescriptor._manufacturerWebsite = factoryInstance.ManufacturerWebsite;
			serializerDescriptor._defaultFileExtension = factoryInstance.DefaultFileExtension;
			serializerDescriptor._isLoadable = true;
			Type type = factoryInstance.GetType();
			serializerDescriptor._assemblyName = type.Assembly.FullName;
			serializerDescriptor._assemblyPath = type.Assembly.Location;
			serializerDescriptor._assemblyVersion = type.Assembly.GetName().Version;
			serializerDescriptor._factoryInterfaceName = type.FullName;
			serializerDescriptor._winFXVersion = typeof(Button).Assembly.GetName().Version;
			return serializerDescriptor;
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x0028C067 File Offset: 0x0028B067
		internal ISerializerFactory CreateSerializerFactory()
		{
			return Assembly.LoadFrom(this.AssemblyPath).CreateInstance(this.FactoryInterfaceName) as ISerializerFactory;
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x0028C084 File Offset: 0x0028B084
		internal void WriteToRegistryKey(RegistryKey key)
		{
			key.SetValue("uiLanguage", CultureInfo.CurrentUICulture.Name);
			key.SetValue("displayName", this.DisplayName);
			key.SetValue("manufacturerName", this.ManufacturerName);
			key.SetValue("manufacturerWebsite", this.ManufacturerWebsite);
			key.SetValue("defaultFileExtension", this.DefaultFileExtension);
			key.SetValue("assemblyName", this.AssemblyName);
			key.SetValue("assemblyPath", this.AssemblyPath);
			key.SetValue("factoryInterfaceName", this.FactoryInterfaceName);
			key.SetValue("assemblyVersion", this.AssemblyVersion.ToString());
			key.SetValue("winFXVersion", this.WinFXVersion.ToString());
		}

		// Token: 0x06005CFF RID: 23807 RVA: 0x0028C14C File Offset: 0x0028B14C
		internal static SerializerDescriptor CreateFromRegistry(RegistryKey plugIns, string keyName)
		{
			SerializerDescriptor serializerDescriptor = new SerializerDescriptor();
			try
			{
				RegistryKey registryKey = plugIns.OpenSubKey(keyName);
				serializerDescriptor._displayName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "displayName");
				serializerDescriptor._manufacturerName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "manufacturerName");
				serializerDescriptor._manufacturerWebsite = new Uri(SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "manufacturerWebsite"));
				serializerDescriptor._defaultFileExtension = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "defaultFileExtension");
				serializerDescriptor._assemblyName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "assemblyName");
				serializerDescriptor._assemblyPath = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "assemblyPath");
				serializerDescriptor._factoryInterfaceName = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "factoryInterfaceName");
				serializerDescriptor._assemblyVersion = new Version(SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "assemblyVersion"));
				serializerDescriptor._winFXVersion = new Version(SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "winFXVersion"));
				string nonEmptyRegistryString = SerializerDescriptor.GetNonEmptyRegistryString(registryKey, "uiLanguage");
				registryKey.Close();
				if (!nonEmptyRegistryString.Equals(CultureInfo.CurrentUICulture.Name))
				{
					ISerializerFactory serializerFactory = serializerDescriptor.CreateSerializerFactory();
					serializerDescriptor._displayName = serializerFactory.DisplayName;
					serializerDescriptor._manufacturerName = serializerFactory.ManufacturerName;
					serializerDescriptor._manufacturerWebsite = serializerFactory.ManufacturerWebsite;
					serializerDescriptor._defaultFileExtension = serializerFactory.DefaultFileExtension;
					registryKey = plugIns.CreateSubKey(keyName);
					serializerDescriptor.WriteToRegistryKey(registryKey);
					registryKey.Close();
				}
			}
			catch (KeyNotFoundException)
			{
				serializerDescriptor = null;
			}
			if (serializerDescriptor != null)
			{
				Assembly assembly = Assembly.ReflectionOnlyLoadFrom(serializerDescriptor._assemblyPath);
				if (typeof(Button).Assembly.GetName().Version == serializerDescriptor._winFXVersion && assembly != null && assembly.GetName().Version == serializerDescriptor._assemblyVersion)
				{
					serializerDescriptor._isLoadable = true;
				}
			}
			return serializerDescriptor;
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x06005D00 RID: 23808 RVA: 0x0028C300 File Offset: 0x0028B300
		public string DisplayName
		{
			get
			{
				return this._displayName;
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x06005D01 RID: 23809 RVA: 0x0028C308 File Offset: 0x0028B308
		public string ManufacturerName
		{
			get
			{
				return this._manufacturerName;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x0028C310 File Offset: 0x0028B310
		public Uri ManufacturerWebsite
		{
			get
			{
				return this._manufacturerWebsite;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x06005D03 RID: 23811 RVA: 0x0028C318 File Offset: 0x0028B318
		public string DefaultFileExtension
		{
			get
			{
				return this._defaultFileExtension;
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x0028C320 File Offset: 0x0028B320
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x06005D05 RID: 23813 RVA: 0x0028C328 File Offset: 0x0028B328
		public string AssemblyPath
		{
			get
			{
				return this._assemblyPath;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x06005D06 RID: 23814 RVA: 0x0028C330 File Offset: 0x0028B330
		public string FactoryInterfaceName
		{
			get
			{
				return this._factoryInterfaceName;
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x06005D07 RID: 23815 RVA: 0x0028C338 File Offset: 0x0028B338
		public Version AssemblyVersion
		{
			get
			{
				return this._assemblyVersion;
			}
		}

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06005D08 RID: 23816 RVA: 0x0028C340 File Offset: 0x0028B340
		public Version WinFXVersion
		{
			get
			{
				return this._winFXVersion;
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06005D09 RID: 23817 RVA: 0x0028C348 File Offset: 0x0028B348
		public bool IsLoadable
		{
			get
			{
				return this._isLoadable;
			}
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x0028C350 File Offset: 0x0028B350
		public override bool Equals(object obj)
		{
			SerializerDescriptor serializerDescriptor = obj as SerializerDescriptor;
			return serializerDescriptor != null && (serializerDescriptor._displayName == this._displayName && serializerDescriptor._assemblyName == this._assemblyName && serializerDescriptor._assemblyPath == this._assemblyPath && serializerDescriptor._factoryInterfaceName == this._factoryInterfaceName && serializerDescriptor._defaultFileExtension == this._defaultFileExtension && serializerDescriptor._assemblyVersion == this._assemblyVersion) && serializerDescriptor._winFXVersion == this._winFXVersion;
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x0028C3F4 File Offset: 0x0028B3F4
		public override int GetHashCode()
		{
			string[] array = new string[11];
			array[0] = this._displayName;
			array[1] = "/";
			array[2] = this._assemblyName;
			array[3] = "/";
			array[4] = this._assemblyPath;
			array[5] = "/";
			array[6] = this._factoryInterfaceName;
			array[7] = "/";
			int num = 8;
			Version assemblyVersion = this._assemblyVersion;
			array[num] = ((assemblyVersion != null) ? assemblyVersion.ToString() : null);
			array[9] = "/";
			int num2 = 10;
			Version winFXVersion = this._winFXVersion;
			array[num2] = ((winFXVersion != null) ? winFXVersion.ToString() : null);
			return string.Concat(array).GetHashCode();
		}

		// Token: 0x04003143 RID: 12611
		private string _displayName;

		// Token: 0x04003144 RID: 12612
		private string _manufacturerName;

		// Token: 0x04003145 RID: 12613
		private Uri _manufacturerWebsite;

		// Token: 0x04003146 RID: 12614
		private string _defaultFileExtension;

		// Token: 0x04003147 RID: 12615
		private string _assemblyName;

		// Token: 0x04003148 RID: 12616
		private string _assemblyPath;

		// Token: 0x04003149 RID: 12617
		private string _factoryInterfaceName;

		// Token: 0x0400314A RID: 12618
		private Version _assemblyVersion;

		// Token: 0x0400314B RID: 12619
		private Version _winFXVersion;

		// Token: 0x0400314C RID: 12620
		private bool _isLoadable;
	}
}
