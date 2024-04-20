using System;
using System.Globalization;
using System.Reflection;

namespace MS.Internal.Data
{
	// Token: 0x02000229 RID: 553
	internal class IndexerPropertyInfo : PropertyInfo
	{
		// Token: 0x060014D7 RID: 5335 RVA: 0x001536D7 File Offset: 0x001526D7
		private IndexerPropertyInfo()
		{
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x001536DF File Offset: 0x001526DF
		internal static IndexerPropertyInfo Instance
		{
			get
			{
				return IndexerPropertyInfo._instance;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x001056E1 File Offset: 0x001046E1
		public override PropertyAttributes Attributes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x001056E1 File Offset: 0x001046E1
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00109403 File Offset: 0x00108403
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return null;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x001536E6 File Offset: 0x001526E6
		public override ParameterInfo[] GetIndexParameters()
		{
			return Array.Empty<ParameterInfo>();
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00109403 File Offset: 0x00108403
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return null;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x001136C4 File Offset: 0x001126C4
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return obj;
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x00152422 File Offset: 0x00151422
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x001056E1 File Offset: 0x001046E1
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x001056E1 File Offset: 0x001046E1
		public override Type DeclaringType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x001056E1 File Offset: 0x001046E1
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x001056E1 File Offset: 0x001046E1
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x001056E1 File Offset: 0x001046E1
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060014E7 RID: 5351 RVA: 0x001536ED File Offset: 0x001526ED
		public override string Name
		{
			get
			{
				return "IndexerProperty";
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x001056E1 File Offset: 0x001046E1
		public override Type ReflectedType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x04000BF0 RID: 3056
		private static readonly IndexerPropertyInfo _instance = new IndexerPropertyInfo();
	}
}
