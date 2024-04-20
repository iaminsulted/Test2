using System;
using System.Diagnostics;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x0200032E RID: 814
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("COMMONDPS")]
	internal sealed class CommonDependencyPropertyAttribute : Attribute
	{
	}
}
