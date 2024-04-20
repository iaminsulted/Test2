using System;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000298 RID: 664
	[Serializable]
	internal class ReturnEventSaver
	{
		// Token: 0x0600190B RID: 6411 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal ReturnEventSaver()
		{
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00162700 File Offset: 0x00161700
		internal void _Detach(PageFunctionBase pf)
		{
			if (pf._Return != null && pf._Saver == null)
			{
				Delegate[] invocationList = pf._Return.GetInvocationList();
				ReturnEventSaverInfo[] array = this._returnList = new ReturnEventSaverInfo[invocationList.Length];
				for (int i = 0; i < invocationList.Length; i++)
				{
					Delegate @delegate = invocationList[i];
					bool fSamePf = false;
					if (@delegate.Target == pf)
					{
						fSamePf = true;
					}
					MethodInfo method = @delegate.Method;
					ReturnEventSaverInfo returnEventSaverInfo = new ReturnEventSaverInfo(@delegate.GetType().AssemblyQualifiedName, @delegate.Target.GetType().AssemblyQualifiedName, method.Name, fSamePf);
					array[i] = returnEventSaverInfo;
				}
				pf._Saver = this;
			}
			pf._DetachEvents();
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x001627B4 File Offset: 0x001617B4
		internal void _Attach(object caller, PageFunctionBase child)
		{
			ReturnEventSaverInfo[] array = null;
			array = this._returnList;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Compare(this._returnList[i]._targetTypeName, caller.GetType().AssemblyQualifiedName, StringComparison.Ordinal) != 0)
					{
						throw new NotSupportedException(SR.Get("ReturnEventHandlerMustBeOnParentPage"));
					}
					Delegate d;
					try
					{
						d = Delegate.CreateDelegate(Type.GetType(this._returnList[i]._delegateTypeName), caller, this._returnList[i]._delegateMethodName);
					}
					catch (Exception innerException)
					{
						throw new NotSupportedException(SR.Get("ReturnEventHandlerMustBeOnParentPage"), innerException);
					}
					child._AddEventHandler(d);
				}
			}
		}

		// Token: 0x04000D81 RID: 3457
		private ReturnEventSaverInfo[] _returnList;
	}
}
