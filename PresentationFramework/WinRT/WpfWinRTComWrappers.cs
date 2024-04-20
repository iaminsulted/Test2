using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.Windows.Data.Text;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000A2 RID: 162
	internal class WpfWinRTComWrappers : ComWrappers
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000256 RID: 598 RVA: 0x000F9E22 File Offset: 0x000F8E22
		public static IUnknownVftbl IUnknownVftbl { get; }

		// Token: 0x06000257 RID: 599 RVA: 0x000F9E2C File Offset: 0x000F8E2C
		static WpfWinRTComWrappers()
		{
			IntPtr ptr;
			IntPtr ptr2;
			IntPtr ptr3;
			ComWrappers.GetIUnknownImpl(out ptr, out ptr2, out ptr3);
			WpfWinRTComWrappers.IUnknownVftbl = new IUnknownVftbl
			{
				QueryInterface = Marshal.GetDelegateForFunctionPointer<IUnknownVftbl._QueryInterface>(ptr),
				AddRef = Marshal.GetDelegateForFunctionPointer<IUnknownVftbl._AddRef>(ptr2),
				Release = Marshal.GetDelegateForFunctionPointer<IUnknownVftbl._Release>(ptr3)
			};
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000F9E84 File Offset: 0x000F8E84
		protected unsafe override ComWrappers.ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count)
		{
			List<ComWrappers.ComInterfaceEntry> interfaceTableEntries = ComWrappersSupport.GetInterfaceTableEntries(obj);
			if (flags.HasFlag(CreateComInterfaceFlags.CallerDefinedIUnknown))
			{
				interfaceTableEntries.Add(new ComWrappers.ComInterfaceEntry
				{
					IID = typeof(IUnknownVftbl).GUID,
					Vtable = IUnknownVftbl.AbiToProjectionVftblPtr
				});
			}
			interfaceTableEntries.Add(new ComWrappers.ComInterfaceEntry
			{
				IID = typeof(IInspectable).GUID,
				Vtable = IInspectable.Vftbl.AbiToProjectionVftablePtr
			});
			count = interfaceTableEntries.Count;
			ComWrappers.ComInterfaceEntry* ptr = (ComWrappers.ComInterfaceEntry*)((void*)Marshal.AllocCoTaskMem(sizeof(ComWrappers.ComInterfaceEntry) * count));
			for (int i = 0; i < count; i++)
			{
				ptr[i] = interfaceTableEntries[i];
			}
			WpfWinRTComWrappers.ComInterfaceEntryCleanupTable.Add(obj, new WpfWinRTComWrappers.VtableEntriesCleanupScout(ptr));
			return ptr;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000F9F60 File Offset: 0x000F8F60
		protected override object CreateObject(IntPtr externalComObject, CreateObjectFlags flags)
		{
			IObjectReference objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface(externalComObject);
			ObjectReference<IInspectable.Vftbl> obj;
			if (objectReferenceForInterface.TryAs<IInspectable.Vftbl>(out obj) == 0)
			{
				IInspectable inspectable = new IInspectable(obj);
				object result;
				if (inspectable.GetRuntimeClassName(true) == "Windows.Data.Text.WordSegment")
				{
					result = new MS.Internal.WindowsRuntime.Windows.Data.Text.WordSegment(new MS.Internal.WindowsRuntime.ABI.Windows.Data.Text.IWordSegment(objectReferenceForInterface));
				}
				else
				{
					result = inspectable;
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000F9FAC File Offset: 0x000F8FAC
		protected override void ReleaseObjects(IEnumerable objects)
		{
			using (IEnumerator enumerator = objects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IObjectReference objectReference;
					if (!ComWrappersSupport.TryUnwrapObject(enumerator.Current, out objectReference))
					{
						throw new InvalidOperationException("Cannot release objects that are not runtime wrappers of native WinRT objects.");
					}
					objectReference.Dispose();
				}
			}
		}

		// Token: 0x04000591 RID: 1425
		private static ConditionalWeakTable<object, WpfWinRTComWrappers.VtableEntriesCleanupScout> ComInterfaceEntryCleanupTable = new ConditionalWeakTable<object, WpfWinRTComWrappers.VtableEntriesCleanupScout>();

		// Token: 0x02000887 RID: 2183
		private class VtableEntriesCleanupScout
		{
			// Token: 0x06008020 RID: 32800 RVA: 0x00321D28 File Offset: 0x00320D28
			public unsafe VtableEntriesCleanupScout(ComWrappers.ComInterfaceEntry* data)
			{
				this._data = data;
			}

			// Token: 0x06008021 RID: 32801 RVA: 0x00321D38 File Offset: 0x00320D38
			unsafe ~VtableEntriesCleanupScout()
			{
				Marshal.FreeCoTaskMem((IntPtr)((void*)this._data));
			}

			// Token: 0x04003BD5 RID: 15317
			private unsafe readonly ComWrappers.ComInterfaceEntry* _data;
		}
	}
}
