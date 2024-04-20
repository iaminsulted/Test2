using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WinRT
{
	// Token: 0x020000B0 RID: 176
	internal struct MarshalInterface<T>
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x000FC11C File Offset: 0x000FB11C
		public static T FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return (T)((object)null);
			}
			object obj = MarshalInspectable.FromAbi(ptr);
			if (obj is T)
			{
				return (T)((object)obj);
			}
			if (MarshalInterface<T>._FromAbi == null)
			{
				MarshalInterface<T>._FromAbi = MarshalInterface<T>.BindFromAbi();
			}
			return MarshalInterface<T>._FromAbi(ptr);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000FC174 File Offset: 0x000FB174
		public static IObjectReference CreateMarshaler(T value)
		{
			if (value == null)
			{
				return null;
			}
			if (value.GetType() == MarshalInterface<T>.HelperType)
			{
				if (MarshalInterface<T>._ToAbi == null)
				{
					MarshalInterface<T>._ToAbi = MarshalInterface<T>.BindToAbi();
				}
				return MarshalInterface<T>._ToAbi(value);
			}
			if (MarshalInterface<T>._As == null)
			{
				MarshalInterface<T>._As = MarshalInterface<T>.BindAs();
			}
			IObjectReference arg = MarshalInspectable.CreateMarshaler(value, true);
			return MarshalInterface<T>._As(arg);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000FC1EA File Offset: 0x000FB1EA
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<T>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000FC1FB File Offset: 0x000FB1FB
		public static void DisposeAbi(IntPtr thisPtr)
		{
			MarshalInterfaceHelper<T>.DisposeAbi(thisPtr);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000FC203 File Offset: 0x000FB203
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<T>.DisposeMarshaler(value);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000FC20B File Offset: 0x000FB20B
		public static IntPtr FromManaged(T value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return MarshalInterface<T>.CreateMarshaler(value).GetRef();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000FC226 File Offset: 0x000FB226
		public unsafe static void CopyManaged(T value, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = ((value == null) ? IntPtr.Zero : MarshalInterface<T>.CreateMarshaler(value).GetRef());
		}

		// Token: 0x060002BF RID: 703 RVA: 0x000FC24A File Offset: 0x000FB24A
		public static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray(T[] array)
		{
			return MarshalInterfaceHelper<T>.CreateMarshalerArray(array, (T o) => MarshalInterface<T>.CreateMarshaler(o));
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000FC271 File Offset: 0x000FB271
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<T>.GetAbiArray(box);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000FC279 File Offset: 0x000FB279
		public static T[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<T>.FromAbiArray(box, new Func<IntPtr, T>(MarshalInterface<T>.FromAbi));
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000FC28D File Offset: 0x000FB28D
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(T[] array)
		{
			return MarshalInterfaceHelper<T>.FromManagedArray(array, (T o) => MarshalInterface<T>.FromManaged(o));
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000FC2B4 File Offset: 0x000FB2B4
		public static void CopyManagedArray(T[] array, IntPtr data)
		{
			MarshalInterfaceHelper<T>.CopyManagedArray(array, data, delegate(T o, IntPtr dest)
			{
				MarshalInterface<T>.CopyManaged(o, dest);
			});
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000FC2DC File Offset: 0x000FB2DC
		public static void DisposeMarshalerArray(object box)
		{
			MarshalInterfaceHelper<T>.DisposeMarshalerArray(box);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000FC2E4 File Offset: 0x000FB2E4
		public static void DisposeAbiArray(object box)
		{
			MarshalInterfaceHelper<T>.DisposeAbiArray(box);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000FC2EC File Offset: 0x000FB2EC
		private static Func<IntPtr, T> BindFromAbi()
		{
			MethodInfo method = MarshalInterface<T>.HelperType.GetMethod("FromAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			ConstructorInfo constructor = MarshalInterface<T>.HelperType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new Type[]
			{
				method.ReturnType
			}, null);
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(IntPtr), "arg")
			};
			return Expression.Lambda<Func<IntPtr, T>>(Expression.New(constructor, new Expression[]
			{
				Expression.Call(method, array[0])
			}), array).Compile();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000FC370 File Offset: 0x000FB370
		private static Func<T, IObjectReference> BindToAbi()
		{
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(T), "arg")
			};
			return Expression.Lambda<Func<T, IObjectReference>>(Expression.MakeMemberAccess(Expression.Convert(array[0], MarshalInterface<T>.HelperType), MarshalInterface<T>.HelperType.GetField("_obj", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)), array).Compile();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000FC3CC File Offset: 0x000FB3CC
		private static Func<IObjectReference, IObjectReference> BindAs()
		{
			Type helperType = typeof(T).GetHelperType();
			ParameterExpression[] array = new ParameterExpression[]
			{
				Expression.Parameter(typeof(IObjectReference), "arg")
			};
			return Expression.Lambda<Func<IObjectReference, IObjectReference>>(Expression.Call(array[0], typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				helperType.FindVftblType()
			})), array).Compile();
		}

		// Token: 0x040005C8 RID: 1480
		private static readonly Type HelperType = typeof(T).GetHelperType();

		// Token: 0x040005C9 RID: 1481
		private static Func<T, IObjectReference> _ToAbi;

		// Token: 0x040005CA RID: 1482
		private static Func<IntPtr, T> _FromAbi;

		// Token: 0x040005CB RID: 1483
		private static Func<IObjectReference, IObjectReference> _As;
	}
}
