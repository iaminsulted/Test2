using System;
using System.Reflection;

namespace MS.Internal.WindowsRuntime
{
	// Token: 0x020002E3 RID: 739
	internal static class ReflectionHelper
	{
		// Token: 0x06001BE1 RID: 7137 RVA: 0x0016A970 File Offset: 0x00169970
		public static TResult ReflectionStaticCall<TResult>(this Type type, string methodName)
		{
			MethodInfo method = type.GetMethod(methodName, Type.EmptyTypes);
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return (TResult)((object)method.Invoke(null, null));
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0016A99C File Offset: 0x0016999C
		public static TResult ReflectionStaticCall<TResult, TArg>(this Type type, string methodName, TArg arg)
		{
			MethodInfo method = type.GetMethod(methodName, new Type[]
			{
				typeof(TArg)
			});
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return (TResult)((object)method.Invoke(null, new object[]
			{
				arg
			}));
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0016A9ED File Offset: 0x001699ED
		public static TResult ReflectionCall<TResult>(this object obj, string methodName)
		{
			return (TResult)((object)obj.ReflectionCall(methodName));
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0016A9FB File Offset: 0x001699FB
		public static object ReflectionCall(this object obj, string methodName)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, Type.EmptyTypes);
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return method.Invoke(obj, null);
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0016AA28 File Offset: 0x00169A28
		public static object ReflectionCall<TArg1>(this object obj, string methodName, TArg1 arg1)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, new Type[]
			{
				typeof(TArg1)
			});
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return method.Invoke(obj, new object[]
			{
				arg1
			});
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0016AA79 File Offset: 0x00169A79
		public static TResult ReflectionCall<TResult, TArg1>(this object obj, string methodName, TArg1 arg1)
		{
			return (TResult)((object)obj.ReflectionCall(methodName, arg1));
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0016AA88 File Offset: 0x00169A88
		public static object ReflectionCall<TArg1, TArg2>(this object obj, string methodName, TArg1 arg1, TArg2 arg2)
		{
			MethodInfo method = obj.GetType().GetMethod(methodName, new Type[]
			{
				typeof(TArg1),
				typeof(TArg2)
			});
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}
			return method.Invoke(obj, new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x0016AAEF File Offset: 0x00169AEF
		public static TResult ReflectionCall<TResult, TArg1, TArg2>(this object obj, string methodName, TArg1 arg1, TArg2 arg2)
		{
			return (TResult)((object)obj.ReflectionCall(methodName, arg1, arg2));
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0016AAFF File Offset: 0x00169AFF
		public static TResult ReflectionGetField<TResult>(this object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().GetField(fieldName);
			if (field == null)
			{
				throw new MissingFieldException(fieldName);
			}
			return (TResult)((object)field.GetValue(obj));
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0016AB28 File Offset: 0x00169B28
		public static object ReflectionNew(this Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				throw new MissingMethodException(type.FullName + "." + type.Name + "()");
			}
			return constructor.Invoke(null);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0016AB68 File Offset: 0x00169B68
		public static object ReflectionNew<TArg1>(this Type type, TArg1 arg1)
		{
			ConstructorInfo constructor = type.GetConstructor(new Type[]
			{
				typeof(TArg1)
			});
			if (constructor == null)
			{
				throw new MissingMethodException(string.Format("{0}.{1}({2})", type.FullName, type.Name, typeof(TArg1).Name));
			}
			return constructor.Invoke(new object[]
			{
				arg1
			});
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0016ABD8 File Offset: 0x00169BD8
		public static object ReflectionNew<TArg1, TArg2>(this Type type, TArg1 arg1, TArg2 arg2)
		{
			ConstructorInfo constructor = type.GetConstructor(new Type[]
			{
				typeof(TArg1),
				typeof(TArg2)
			});
			if (constructor == null)
			{
				throw new MissingMethodException(string.Format("{0}.{1}({2},{3})", new object[]
				{
					type.FullName,
					type.Name,
					typeof(TArg1).Name,
					typeof(TArg2).Name
				}));
			}
			return constructor.Invoke(new object[]
			{
				arg1,
				arg2
			});
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0016AC7F File Offset: 0x00169C7F
		public static TResult ReflectionGetProperty<TResult>(this object obj, string propertyName)
		{
			PropertyInfo property = obj.GetType().GetProperty(propertyName);
			if (property == null)
			{
				throw new MissingMemberException(propertyName);
			}
			return (TResult)((object)property.GetValue(obj));
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x0016ACA8 File Offset: 0x00169CA8
		public static object ReflectionGetProperty(this object obj, string propertyName)
		{
			return obj.ReflectionGetProperty(propertyName);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0016ACB1 File Offset: 0x00169CB1
		public static TResult ReflectionStaticGetProperty<TResult>(this Type type, string propertyName)
		{
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static);
			if (property == null)
			{
				throw new MissingMemberException(propertyName);
			}
			return (TResult)((object)property.GetValue(null));
		}
	}
}
