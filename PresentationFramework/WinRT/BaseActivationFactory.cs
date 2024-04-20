using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200009C RID: 156
	internal class BaseActivationFactory
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000F9848 File Offset: 0x000F8848
		public BaseActivationFactory(string typeNamespace, string typeFullName)
		{
			string runtimeClassId = TypeExtensions.RemoveNamespacePrefix(typeFullName);
			ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> activationFactory = WinrtModule.GetActivationFactory(runtimeClassId);
			this._IActivationFactory = activationFactory.Item1;
			int item = activationFactory.Item2;
			if (this._IActivationFactory != null)
			{
				return;
			}
			string text = typeNamespace;
			for (;;)
			{
				try
				{
					activationFactory = DllModule.Load(text + ".dll").GetActivationFactory(runtimeClassId);
					this._IActivationFactory = activationFactory.Item1;
					if (this._IActivationFactory != null)
					{
						break;
					}
				}
				catch (Exception)
				{
				}
				int num = text.LastIndexOf(".");
				if (num <= 0)
				{
					Marshal.ThrowExceptionForHR(item);
				}
				text = text.Remove(num);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000F98F0 File Offset: 0x000F88F0
		public ObjectReference<I> _ActivateInstance<I>()
		{
			IntPtr intPtr;
			Marshal.ThrowExceptionForHR(this._IActivationFactory.Vftbl.ActivateInstance(this._IActivationFactory.ThisPtr, out intPtr));
			ObjectReference<I> result;
			try
			{
				result = ComWrappersSupport.GetObjectReferenceForInterface(intPtr).As<I>();
			}
			finally
			{
				MarshalInspectable.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000F994C File Offset: 0x000F894C
		public ObjectReference<I> _As<I>()
		{
			return this._IActivationFactory.As<I>();
		}

		// Token: 0x04000588 RID: 1416
		private ObjectReference<IActivationFactoryVftbl> _IActivationFactory;
	}
}
