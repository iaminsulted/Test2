using System;
using System.Reflection;
using MS.Internal.WindowsBase;

namespace System.Windows.Baml2006
{
	// Token: 0x0200040D RID: 1037
	internal class Baml6Assembly
	{
		// Token: 0x06002D2B RID: 11563 RVA: 0x001AB408 File Offset: 0x001AA408
		public Baml6Assembly(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this._assembly = null;
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x001AB42C File Offset: 0x001AA42C
		public Baml6Assembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.Name = null;
			this._assembly = assembly;
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x001AB458 File Offset: 0x001AA458
		public Assembly Assembly
		{
			get
			{
				if (this._assembly != null)
				{
					return this._assembly;
				}
				AssemblyName assemblyName = new AssemblyName(this.Name);
				this._assembly = SafeSecurityHelper.GetLoadedAssembly(assemblyName);
				if (this._assembly == null)
				{
					byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
					if (assemblyName.Version != null || assemblyName.CultureInfo != null || publicKeyToken != null)
					{
						try
						{
							this._assembly = Assembly.Load(assemblyName.FullName);
							goto IL_A5;
						}
						catch
						{
							AssemblyName assemblyName2 = new AssemblyName(assemblyName.Name);
							if (publicKeyToken != null)
							{
								assemblyName2.SetPublicKeyToken(publicKeyToken);
							}
							this._assembly = Assembly.Load(assemblyName2);
							goto IL_A5;
						}
					}
					this._assembly = Assembly.LoadWithPartialName(assemblyName.Name);
				}
				IL_A5:
				return this._assembly;
			}
		}

		// Token: 0x04001BA6 RID: 7078
		public readonly string Name;

		// Token: 0x04001BA7 RID: 7079
		private Assembly _assembly;
	}
}
