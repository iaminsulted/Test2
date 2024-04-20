using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000348 RID: 840
	[TypeConverter(typeof(ComponentResourceKeyConverter))]
	public class ComponentResourceKey : ResourceKey
	{
		// Token: 0x06001FE8 RID: 8168 RVA: 0x00173BFB File Offset: 0x00172BFB
		public ComponentResourceKey()
		{
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00173C04 File Offset: 0x00172C04
		public ComponentResourceKey(Type typeInTargetAssembly, object resourceId)
		{
			if (typeInTargetAssembly == null)
			{
				throw new ArgumentNullException("typeInTargetAssembly");
			}
			if (resourceId == null)
			{
				throw new ArgumentNullException("resourceId");
			}
			this._typeInTargetAssembly = typeInTargetAssembly;
			this._typeInTargetAssemblyInitialized = true;
			this._resourceId = resourceId;
			this._resourceIdInitialized = true;
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001FEA RID: 8170 RVA: 0x00173C55 File Offset: 0x00172C55
		// (set) Token: 0x06001FEB RID: 8171 RVA: 0x00173C5D File Offset: 0x00172C5D
		public Type TypeInTargetAssembly
		{
			get
			{
				return this._typeInTargetAssembly;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._typeInTargetAssemblyInitialized)
				{
					throw new InvalidOperationException(SR.Get("ChangingTypeNotAllowed"));
				}
				this._typeInTargetAssembly = value;
				this._typeInTargetAssemblyInitialized = true;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001FEC RID: 8172 RVA: 0x00173C99 File Offset: 0x00172C99
		public override Assembly Assembly
		{
			get
			{
				if (!(this._typeInTargetAssembly != null))
				{
					return null;
				}
				return this._typeInTargetAssembly.Assembly;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x00173CB6 File Offset: 0x00172CB6
		// (set) Token: 0x06001FEE RID: 8174 RVA: 0x00173CBE File Offset: 0x00172CBE
		public object ResourceId
		{
			get
			{
				return this._resourceId;
			}
			set
			{
				if (this._resourceIdInitialized)
				{
					throw new InvalidOperationException(SR.Get("ChangingIdNotAllowed"));
				}
				this._resourceId = value;
				this._resourceIdInitialized = true;
			}
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00173CE8 File Offset: 0x00172CE8
		public override bool Equals(object o)
		{
			ComponentResourceKey componentResourceKey = o as ComponentResourceKey;
			if (componentResourceKey == null)
			{
				return false;
			}
			if (!((componentResourceKey._typeInTargetAssembly != null) ? componentResourceKey._typeInTargetAssembly.Equals(this._typeInTargetAssembly) : (this._typeInTargetAssembly == null)))
			{
				return false;
			}
			if (componentResourceKey._resourceId == null)
			{
				return this._resourceId == null;
			}
			return componentResourceKey._resourceId.Equals(this._resourceId);
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00173D55 File Offset: 0x00172D55
		public override int GetHashCode()
		{
			return ((this._typeInTargetAssembly != null) ? this._typeInTargetAssembly.GetHashCode() : 0) ^ ((this._resourceId != null) ? this._resourceId.GetHashCode() : 0);
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00173D8C File Offset: 0x00172D8C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append("TargetType=");
			stringBuilder.Append((this._typeInTargetAssembly != null) ? this._typeInTargetAssembly.FullName : "null");
			stringBuilder.Append(" ID=");
			stringBuilder.Append((this._resourceId != null) ? this._resourceId.ToString() : "null");
			return stringBuilder.ToString();
		}

		// Token: 0x04000FA9 RID: 4009
		private Type _typeInTargetAssembly;

		// Token: 0x04000FAA RID: 4010
		private bool _typeInTargetAssemblyInitialized;

		// Token: 0x04000FAB RID: 4011
		private object _resourceId;

		// Token: 0x04000FAC RID: 4012
		private bool _resourceIdInitialized;
	}
}
