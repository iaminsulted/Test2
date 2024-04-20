using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Xaml;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x020004D1 RID: 1233
	internal class RestrictiveXamlXmlReader : XamlXmlReader
	{
		// Token: 0x06003F37 RID: 16183 RVA: 0x0021069C File Offset: 0x0020F69C
		static RestrictiveXamlXmlReader()
		{
			RestrictiveXamlXmlReader._unloadedTypes = new ConcurrentDictionary<string, List<RestrictiveXamlXmlReader.RestrictedType>>();
			foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in RestrictiveXamlXmlReader._restrictedTypes)
			{
				if (!string.IsNullOrEmpty(restrictedType.AssemblyName))
				{
					if (!RestrictiveXamlXmlReader._unloadedTypes.ContainsKey(restrictedType.AssemblyName))
					{
						RestrictiveXamlXmlReader._unloadedTypes[restrictedType.AssemblyName] = new List<RestrictiveXamlXmlReader.RestrictedType>();
					}
					RestrictiveXamlXmlReader._unloadedTypes[restrictedType.AssemblyName].Add(restrictedType);
				}
				else
				{
					Type type = System.Type.GetType(restrictedType.TypeName, false);
					if (type != null)
					{
						restrictedType.TypeReference = type;
					}
				}
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x002107D4 File Offset: 0x0020F7D4
		public RestrictiveXamlXmlReader(XmlReader xmlReader, XamlSchemaContext schemaContext, XamlXmlReaderSettings settings) : base(xmlReader, schemaContext, settings)
		{
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x002107EC File Offset: 0x0020F7EC
		internal RestrictiveXamlXmlReader(XmlReader xmlReader, XamlSchemaContext schemaContext, XamlXmlReaderSettings settings, List<Type> safeTypes) : base(xmlReader, schemaContext, settings)
		{
			if (safeTypes != null)
			{
				foreach (Type item in safeTypes)
				{
					this._safeTypesSet.Add(item);
				}
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0021085C File Offset: 0x0020F85C
		public override bool Read()
		{
			int num = 0;
			bool result;
			while (result = base.Read())
			{
				if (num <= 0)
				{
					if (this.NodeType != XamlNodeType.StartObject || !this.IsRestrictedType(this.Type.UnderlyingType))
					{
						break;
					}
					num = 1;
				}
				else if (this.NodeType == XamlNodeType.StartObject || this.NodeType == XamlNodeType.GetObject)
				{
					num++;
				}
				else if (this.NodeType == XamlNodeType.EndObject)
				{
					num--;
				}
			}
			return result;
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x002108C4 File Offset: 0x0020F8C4
		private bool IsRestrictedType(Type type)
		{
			if (type != null)
			{
				if (this._safeTypesSet.Contains(type))
				{
					return false;
				}
				RestrictiveXamlXmlReader.EnsureLatestAssemblyLoadInformation();
				foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in RestrictiveXamlXmlReader._restrictedTypes)
				{
					Type typeReference = restrictedType.TypeReference;
					if (typeReference != null && typeReference.IsAssignableFrom(type))
					{
						return true;
					}
				}
				this._safeTypesSet.Add(type);
				return false;
			}
			return false;
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x00210954 File Offset: 0x0020F954
		private static void EnsureLatestAssemblyLoadInformation()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (assemblies.Length != RestrictiveXamlXmlReader._loadedAssembliesCount)
			{
				Assembly[] array = assemblies;
				for (int i = 0; i < array.Length; i++)
				{
					RestrictiveXamlXmlReader.RegisterAssembly(array[i]);
				}
				RestrictiveXamlXmlReader._loadedAssembliesCount = assemblies.Length;
			}
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x00210998 File Offset: 0x0020F998
		private static void RegisterAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
				string fullName = assembly.FullName;
				List<RestrictiveXamlXmlReader.RestrictedType> list = null;
				if (RestrictiveXamlXmlReader._unloadedTypes.TryGetValue(fullName, out list))
				{
					if (list != null)
					{
						foreach (RestrictiveXamlXmlReader.RestrictedType restrictedType in list)
						{
							Type type = assembly.GetType(restrictedType.TypeName, false);
							restrictedType.TypeReference = type;
						}
					}
					RestrictiveXamlXmlReader._unloadedTypes.TryRemove(fullName, out list);
				}
			}
		}

		// Token: 0x04002361 RID: 9057
		private static List<RestrictiveXamlXmlReader.RestrictedType> _restrictedTypes = new List<RestrictiveXamlXmlReader.RestrictedType>
		{
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.Data.ObjectDataProvider", ""),
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.ResourceDictionary", ""),
			new RestrictiveXamlXmlReader.RestrictedType("System.Configuration.Install.AssemblyInstaller", "System.Configuration.Install, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
			new RestrictiveXamlXmlReader.RestrictedType("System.Activities.Presentation.WorkflowDesigner", "System.Activities.Presentation, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 31bf3856ad364e35"),
			new RestrictiveXamlXmlReader.RestrictedType("System.Windows.Forms.BindingSource", "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
		};

		// Token: 0x04002362 RID: 9058
		private static ConcurrentDictionary<string, List<RestrictiveXamlXmlReader.RestrictedType>> _unloadedTypes = null;

		// Token: 0x04002363 RID: 9059
		private HashSet<Type> _safeTypesSet = new HashSet<Type>();

		// Token: 0x04002364 RID: 9060
		[ThreadStatic]
		private static int _loadedAssembliesCount;

		// Token: 0x02000AF8 RID: 2808
		private class RestrictedType
		{
			// Token: 0x06008B92 RID: 35730 RVA: 0x00339FA3 File Offset: 0x00338FA3
			public RestrictedType(string typeName, string assemblyName)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
			}

			// Token: 0x17001E99 RID: 7833
			// (get) Token: 0x06008B93 RID: 35731 RVA: 0x00339FB9 File Offset: 0x00338FB9
			// (set) Token: 0x06008B94 RID: 35732 RVA: 0x00339FC1 File Offset: 0x00338FC1
			public string TypeName { get; set; }

			// Token: 0x17001E9A RID: 7834
			// (get) Token: 0x06008B95 RID: 35733 RVA: 0x00339FCA File Offset: 0x00338FCA
			// (set) Token: 0x06008B96 RID: 35734 RVA: 0x00339FD2 File Offset: 0x00338FD2
			public string AssemblyName { get; set; }

			// Token: 0x17001E9B RID: 7835
			// (get) Token: 0x06008B97 RID: 35735 RVA: 0x00339FDB File Offset: 0x00338FDB
			// (set) Token: 0x06008B98 RID: 35736 RVA: 0x00339FE3 File Offset: 0x00338FE3
			public Type TypeReference { get; set; }
		}
	}
}
