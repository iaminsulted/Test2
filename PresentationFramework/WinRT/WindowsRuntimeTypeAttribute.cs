using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x020000A0 RID: 160
	[EditorBrowsable(EditorBrowsableState.Never)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
	internal sealed class WindowsRuntimeTypeAttribute : Attribute
	{
	}
}
