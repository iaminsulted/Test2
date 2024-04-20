using System;
using System.Reflection;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000B3 RID: 179
	internal abstract class IObjectReference : IDisposable
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000FCC83 File Offset: 0x000FBC83
		public IntPtr ThisPtr
		{
			get
			{
				this.ThrowIfDisposed();
				return this._thisPtr;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060002DB RID: 731 RVA: 0x000FCC91 File Offset: 0x000FBC91
		protected IUnknownVftbl VftblIUnknown
		{
			get
			{
				this.ThrowIfDisposed();
				return this.VftblIUnknownUnsafe;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060002DC RID: 732 RVA: 0x000FCC9F File Offset: 0x000FBC9F
		protected virtual IUnknownVftbl VftblIUnknownUnsafe { get; }

		// Token: 0x060002DD RID: 733 RVA: 0x000FCCA7 File Offset: 0x000FBCA7
		protected IObjectReference(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				throw new ArgumentNullException("thisPtr");
			}
			this._thisPtr = thisPtr;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000FCCD0 File Offset: 0x000FBCD0
		~IObjectReference()
		{
			this.Dispose(false);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000FCD00 File Offset: 0x000FBD00
		public ObjectReference<T> As<T>()
		{
			return this.As<T>(GuidGenerator.GetIID(typeof(T)));
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000FCD18 File Offset: 0x000FBD18
		public ObjectReference<T> As<T>(Guid iid)
		{
			this.ThrowIfDisposed();
			IntPtr intPtr;
			Marshal.ThrowExceptionForHR(this.VftblIUnknown.QueryInterface(this.ThisPtr, ref iid, out intPtr));
			return ObjectReference<T>.Attach(ref intPtr);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000FCD51 File Offset: 0x000FBD51
		public int TryAs<T>(out ObjectReference<T> objRef)
		{
			return this.TryAs<T>(GuidGenerator.GetIID(typeof(T)), out objRef);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000FCD6C File Offset: 0x000FBD6C
		public virtual int TryAs<T>(Guid iid, out ObjectReference<T> objRef)
		{
			objRef = null;
			this.ThrowIfDisposed();
			IntPtr intPtr;
			int num = this.VftblIUnknown.QueryInterface(this.ThisPtr, ref iid, out intPtr);
			if (num >= 0)
			{
				objRef = ObjectReference<T>.Attach(ref intPtr);
			}
			return num;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000FCDA9 File Offset: 0x000FBDA9
		public IObjectReference As(Guid iid)
		{
			return this.As<IUnknownVftbl>(iid);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000FCDB4 File Offset: 0x000FBDB4
		public T AsType<T>()
		{
			this.ThrowIfDisposed();
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[]
			{
				typeof(IObjectReference)
			});
			if (constructor != null)
			{
				ConstructorInfo constructorInfo = constructor;
				object[] parameters = new IObjectReference[]
				{
					this
				};
				return (T)((object)constructorInfo.Invoke(parameters));
			}
			throw new InvalidOperationException("Target type is not a projected interface.");
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000FCE15 File Offset: 0x000FBE15
		public IntPtr GetRef()
		{
			this.ThrowIfDisposed();
			this.AddRef();
			return this.ThisPtr;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000FCE29 File Offset: 0x000FBE29
		protected void ThrowIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("ObjectReference");
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000FCE3E File Offset: 0x000FBE3E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000FCE4D File Offset: 0x000FBE4D
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.Release();
			this.disposed = true;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000FCE65 File Offset: 0x000FBE65
		protected virtual void AddRef()
		{
			this.VftblIUnknown.AddRef(this.ThisPtr);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000FCE80 File Offset: 0x000FBE80
		protected virtual void Release()
		{
			IUnknownVftbl._Release release = this.VftblIUnknown.Release;
			if (release == null)
			{
				release = Marshal.PtrToStructure<IUnknownVftbl>(Marshal.PtrToStructure<VftblPtr>(this.ThisPtr).Vftbl).Release;
			}
			release(this.ThisPtr);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000FCEC4 File Offset: 0x000FBEC4
		public bool IsReferenceToManagedObject
		{
			get
			{
				bool result;
				using (ObjectReference<IUnknownVftbl> objectReference = this.As<IUnknownVftbl>())
				{
					result = objectReference.VftblIUnknown.Equals(IUnknownVftbl.AbiToProjectionVftbl);
				}
				return result;
			}
		}

		// Token: 0x040005DD RID: 1501
		protected bool disposed;

		// Token: 0x040005DE RID: 1502
		private readonly IntPtr _thisPtr;
	}
}
