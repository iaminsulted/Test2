using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WinRT
{
	// Token: 0x020000AD RID: 173
	internal class MarshalGeneric<T>
	{
		// Token: 0x06000299 RID: 665 RVA: 0x000FB48C File Offset: 0x000FA48C
		static MarshalGeneric()
		{
			MarshalGeneric<T>.CopyAbi = MarshalGeneric<T>.BindCopyAbi();
			MarshalGeneric<T>.FromManaged = MarshalGeneric<T>.BindFromManaged();
			MarshalGeneric<T>.CopyManaged = MarshalGeneric<T>.BindCopyManaged();
			MarshalGeneric<T>.DisposeMarshaler = MarshalGeneric<T>.BindDisposeMarshaler();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000FB51C File Offset: 0x000FA51C
		private static Func<T, object> BindCreateMarshaler()
		{
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(T), "arg")
			};
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("CreateMarshaler", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Expression[] arguments = array;
			return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Call(method, arguments), typeof(object)), array).Compile();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000FB57C File Offset: 0x000FA57C
		private static Func<object, object> BindGetAbi()
		{
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(object), "arg")
			};
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("GetAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Expression[] arguments = new UnaryExpression[]
			{
				Expression.Convert(array[0], MarshalGeneric<T>.MarshalerType)
			};
			return Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Call(method, arguments), typeof(object)), array).Compile();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000FB5F0 File Offset: 0x000FA5F0
		private static Action<object, IntPtr> BindCopyAbi()
		{
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("CopyAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				return null;
			}
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(object), "arg"),
				Expression.Parameter(typeof(IntPtr), "dest")
			};
			return Expression.Lambda<Action<object, IntPtr>>(Expression.Call(method, new Expression[]
			{
				Expression.Convert(array[0], MarshalGeneric<T>.MarshalerType),
				array[1]
			}), array).Compile();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000FB67C File Offset: 0x000FA67C
		private static Func<object, T> BindFromAbi()
		{
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(object), "arg")
			};
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("FromAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Expression[] arguments = new UnaryExpression[]
			{
				Expression.Convert(array[0], MarshalGeneric<T>.AbiType)
			};
			return Expression.Lambda<Func<object, T>>(Expression.Call(method, arguments), array).Compile();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000FB6E0 File Offset: 0x000FA6E0
		private static Func<T, object> BindFromManaged()
		{
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(T), "arg")
			};
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("FromManaged", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Expression[] arguments = array;
			return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Call(method, arguments), typeof(object)), array).Compile();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000FB740 File Offset: 0x000FA740
		private static Action<T, IntPtr> BindCopyManaged()
		{
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("CopyManaged", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				return null;
			}
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(T), "arg"),
				Expression.Parameter(typeof(IntPtr), "dest")
			};
			MethodInfo method2 = method;
			Expression[] arguments = array;
			return Expression.Lambda<Action<T, IntPtr>>(Expression.Call(method2, arguments), array).Compile();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000FB7B4 File Offset: 0x000FA7B4
		private static Action<object> BindDisposeMarshaler()
		{
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(object), "arg")
			};
			MethodInfo method = MarshalGeneric<T>.HelperType.GetMethod("DisposeMarshaler", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Expression[] arguments = new UnaryExpression[]
			{
				Expression.Convert(array[0], MarshalGeneric<T>.MarshalerType)
			};
			return Expression.Lambda<Action<object>>(Expression.Call(method, arguments), array).Compile();
		}

		// Token: 0x040005BE RID: 1470
		protected static readonly Type HelperType = typeof(T).GetHelperType();

		// Token: 0x040005BF RID: 1471
		protected static readonly Type AbiType = typeof(T).GetAbiType();

		// Token: 0x040005C0 RID: 1472
		protected static readonly Type MarshalerType = typeof(T).GetMarshalerType();

		// Token: 0x040005C1 RID: 1473
		public static readonly Func<T, object> CreateMarshaler = MarshalGeneric<T>.BindCreateMarshaler();

		// Token: 0x040005C2 RID: 1474
		public static readonly Func<object, object> GetAbi = MarshalGeneric<T>.BindGetAbi();

		// Token: 0x040005C3 RID: 1475
		public static readonly Action<object, IntPtr> CopyAbi;

		// Token: 0x040005C4 RID: 1476
		public static readonly Func<object, T> FromAbi = MarshalGeneric<T>.BindFromAbi();

		// Token: 0x040005C5 RID: 1477
		public static readonly Func<T, object> FromManaged;

		// Token: 0x040005C6 RID: 1478
		public static readonly Action<T, IntPtr> CopyManaged;

		// Token: 0x040005C7 RID: 1479
		public static readonly Action<object> DisposeMarshaler;
	}
}
