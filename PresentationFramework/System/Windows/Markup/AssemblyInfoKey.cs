using System;

namespace System.Windows.Markup
{
	// Token: 0x02000478 RID: 1144
	internal struct AssemblyInfoKey
	{
		// Token: 0x06003AE6 RID: 15078 RVA: 0x001F2918 File Offset: 0x001F1918
		public override bool Equals(object o)
		{
			if (!(o is AssemblyInfoKey))
			{
				return false;
			}
			AssemblyInfoKey assemblyInfoKey = (AssemblyInfoKey)o;
			if (assemblyInfoKey.AssemblyFullName == null)
			{
				return this.AssemblyFullName == null;
			}
			return assemblyInfoKey.AssemblyFullName.Equals(this.AssemblyFullName);
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001F2959 File Offset: 0x001F1959
		public static bool operator ==(AssemblyInfoKey key1, AssemblyInfoKey key2)
		{
			return key1.Equals(key2);
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001F296E File Offset: 0x001F196E
		public static bool operator !=(AssemblyInfoKey key1, AssemblyInfoKey key2)
		{
			return !key1.Equals(key2);
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001F2986 File Offset: 0x001F1986
		public override int GetHashCode()
		{
			if (this.AssemblyFullName == null)
			{
				return 0;
			}
			return this.AssemblyFullName.GetHashCode();
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x001F299D File Offset: 0x001F199D
		public override string ToString()
		{
			return this.AssemblyFullName;
		}

		// Token: 0x04001DDC RID: 7644
		internal string AssemblyFullName;
	}
}
