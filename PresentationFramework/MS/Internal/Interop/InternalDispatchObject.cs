using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200017D RID: 381
	internal abstract class InternalDispatchObject<IDispInterface> : IReflect
	{
		// Token: 0x06000C6C RID: 3180 RVA: 0x001301C8 File Offset: 0x0012F1C8
		protected InternalDispatchObject()
		{
			MethodInfo[] methods = typeof(IDispInterface).GetMethods();
			this._dispId2MethodMap = new Dictionary<int, MethodInfo>(methods.Length);
			foreach (MethodInfo methodInfo in methods)
			{
				int value = ((DispIdAttribute[])methodInfo.GetCustomAttributes(typeof(DispIdAttribute), false))[0].Value;
				this._dispId2MethodMap[value] = methodInfo;
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x001056E1 File Offset: 0x001046E1
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00109403 File Offset: 0x00108403
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x001056E1 File Offset: 0x001046E1
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x001056E1 File Offset: 0x001046E1
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x001056E1 File Offset: 0x001046E1
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x001056E1 File Offset: 0x001046E1
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00109403 File Offset: 0x00108403
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00109403 File Offset: 0x00108403
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return null;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x001056E1 File Offset: 0x001046E1
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x001056E1 File Offset: 0x001046E1
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0013023C File Offset: 0x0012F23C
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (name.StartsWith("[DISPID=", StringComparison.OrdinalIgnoreCase))
			{
				int key = int.Parse(name.Substring(8, name.Length - 9), CultureInfo.InvariantCulture);
				MethodInfo methodInfo;
				if (this._dispId2MethodMap.TryGetValue(key, out methodInfo))
				{
					return methodInfo.Invoke(this, invokeAttr, binder, args, culture);
				}
			}
			throw new MissingMethodException(base.GetType().Name, name);
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x001302A1 File Offset: 0x0012F2A1
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return typeof(IDispInterface);
			}
		}

		// Token: 0x0400098A RID: 2442
		private Dictionary<int, MethodInfo> _dispId2MethodMap;
	}
}
