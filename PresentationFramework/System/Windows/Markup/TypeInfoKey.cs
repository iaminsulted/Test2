using System;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x02000479 RID: 1145
	internal struct TypeInfoKey
	{
		// Token: 0x06003AEB RID: 15083 RVA: 0x001F29A8 File Offset: 0x001F19A8
		public override bool Equals(object o)
		{
			if (!(o is TypeInfoKey))
			{
				return false;
			}
			TypeInfoKey typeInfoKey = (TypeInfoKey)o;
			if (!((typeInfoKey.DeclaringAssembly != null) ? typeInfoKey.DeclaringAssembly.Equals(this.DeclaringAssembly) : (this.DeclaringAssembly == null)))
			{
				return false;
			}
			if (typeInfoKey.TypeFullName == null)
			{
				return this.TypeFullName == null;
			}
			return typeInfoKey.TypeFullName.Equals(this.TypeFullName);
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x001F2A11 File Offset: 0x001F1A11
		public static bool operator ==(TypeInfoKey key1, TypeInfoKey key2)
		{
			return key1.Equals(key2);
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x001F2A26 File Offset: 0x001F1A26
		public static bool operator !=(TypeInfoKey key1, TypeInfoKey key2)
		{
			return !key1.Equals(key2);
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x001F2A3E File Offset: 0x001F1A3E
		public override int GetHashCode()
		{
			return ((this.DeclaringAssembly != null) ? this.DeclaringAssembly.GetHashCode() : 0) ^ ((this.TypeFullName != null) ? this.TypeFullName.GetHashCode() : 0);
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x001F2A70 File Offset: 0x001F1A70
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("TypeInfoKey: Assembly=");
			stringBuilder.Append((this.DeclaringAssembly != null) ? this.DeclaringAssembly : "null");
			stringBuilder.Append(" Type=");
			stringBuilder.Append((this.TypeFullName != null) ? this.TypeFullName : "null");
			return stringBuilder.ToString();
		}

		// Token: 0x04001DDD RID: 7645
		internal string DeclaringAssembly;

		// Token: 0x04001DDE RID: 7646
		internal string TypeFullName;
	}
}
