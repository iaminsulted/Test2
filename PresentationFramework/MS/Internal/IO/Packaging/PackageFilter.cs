using System;
using System.Collections;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;
using MS.Internal.Interop;
using MS.Internal.IO.Packaging.Extensions;
using MS.Internal.Utility;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000169 RID: 361
	internal class PackageFilter : IFilter
	{
		// Token: 0x06000C0D RID: 3085 RVA: 0x0012F168 File Offset: 0x0012E168
		internal PackageFilter(Package package)
		{
			if (package == null)
			{
				throw new ArgumentNullException("package");
			}
			this._package = package;
			this._partIterator = this._package.GetParts().GetEnumerator();
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0012F202 File Offset: 0x0012E202
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, uint cAttributes, FULLPROPSPEC[] aAttributes)
		{
			this._grfFlags = grfFlags;
			this._cAttributes = cAttributes;
			this._aAttributes = aAttributes;
			this._partIterator.Reset();
			this._progress = PackageFilter.Progress.FilteringNotStarted;
			return IFILTER_FLAGS.IFILTER_FLAGS_NONE;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0012F22C File Offset: 0x0012E22C
		public STAT_CHUNK GetChunk()
		{
			if (this._progress == PackageFilter.Progress.FilteringNotStarted)
			{
				this.MoveToNextFilter();
			}
			if (this._progress == PackageFilter.Progress.FilteringCompleted)
			{
				throw new COMException(SR.Get("FilterEndOfChunks"), -2147215616);
			}
			do
			{
				try
				{
					STAT_CHUNK chunk = this._currentFilter.GetChunk();
					if ((!this._isInternalFilter || chunk.idChunk != 0U) && (this._progress == PackageFilter.Progress.FilteringCoreProperties || (chunk.flags & CHUNKSTATE.CHUNK_VALUE) != CHUNKSTATE.CHUNK_VALUE))
					{
						chunk.idChunk = this.AllocateChunkID();
						chunk.idChunkSource = chunk.idChunk;
						if (this._firstChunkFromFilter)
						{
							chunk.breakType = CHUNK_BREAKTYPE.CHUNK_EOP;
							this._firstChunkFromFilter = false;
						}
						return chunk;
					}
				}
				catch (COMException)
				{
				}
				catch (IOException)
				{
					if (this._isInternalFilter)
					{
						throw;
					}
				}
				this.MoveToNextFilter();
			}
			while (this._progress != PackageFilter.Progress.FilteringCompleted);
			throw new COMException(SR.Get("FilterEndOfChunks"), -2147215616);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0012F324 File Offset: 0x0012E324
		public void GetText(ref uint bufferCharacterCount, IntPtr pBuffer)
		{
			if (this._progress != PackageFilter.Progress.FilteringContent)
			{
				throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
			}
			this._currentFilter.GetText(ref bufferCharacterCount, pBuffer);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0012F351 File Offset: 0x0012E351
		public IntPtr GetValue()
		{
			if (this._progress != PackageFilter.Progress.FilteringCoreProperties)
			{
				throw new COMException(SR.Get("FilterGetValueNotSupported"), -2147215610);
			}
			return this._currentFilter.GetValue();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0012D27E File Offset: 0x0012C27E
		public IntPtr BindRegion(FILTERREGION origPos, ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0012F37C File Offset: 0x0012E37C
		private IFilter GetFilterFromClsid(Guid clsid)
		{
			Type typeFromCLSID = Type.GetTypeFromCLSID(clsid);
			IFilter result;
			try
			{
				result = (IFilter)Activator.CreateInstance(typeFromCLSID);
			}
			catch (InvalidCastException)
			{
				return null;
			}
			catch (COMException)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0012F3C8 File Offset: 0x0012E3C8
		private void MoveToNextFilter()
		{
			this._isInternalFilter = false;
			switch (this._progress)
			{
			case PackageFilter.Progress.FilteringNotStarted:
				this._currentFilter = new IndexingFilterMarshaler(new CorePropertiesFilter(this._package.PackageProperties))
				{
					ThrowOnEndOfChunks = false
				};
				this._currentFilter.Init(this._grfFlags, this._cAttributes, this._aAttributes);
				this._isInternalFilter = true;
				this._progress = PackageFilter.Progress.FilteringCoreProperties;
				return;
			case PackageFilter.Progress.FilteringCoreProperties:
			case PackageFilter.Progress.FilteringContent:
				if (this._currentStream != null)
				{
					this._currentStream.Close();
					this._currentStream = null;
				}
				this._currentFilter = null;
				while (this._partIterator.MoveNext())
				{
					PackagePart packagePart = (PackagePart)this._partIterator.Current;
					ContentType contentType = packagePart.ValidatedContentType();
					string filterClsid = this.GetFilterClsid(contentType, packagePart.Uri);
					if (filterClsid != null)
					{
						this._currentFilter = this.GetFilterFromClsid(new Guid(filterClsid));
						if (this._currentFilter != null)
						{
							this._currentStream = packagePart.GetSeekableStream();
							ManagedIStream pstm = new ManagedIStream(this._currentStream);
							try
							{
								((IPersistStreamWithArrays)this._currentFilter).Load(pstm);
								this._currentFilter.Init(this._grfFlags, this._cAttributes, this._aAttributes);
								break;
							}
							catch (InvalidCastException)
							{
							}
							catch (COMException)
							{
							}
							catch (IOException)
							{
							}
						}
					}
					if (BindUriHelper.IsXamlMimeType(contentType))
					{
						if (this._currentStream == null)
						{
							this._currentStream = packagePart.GetSeekableStream();
						}
						this._currentFilter = new IndexingFilterMarshaler(new XamlFilter(this._currentStream))
						{
							ThrowOnEndOfChunks = false
						};
						this._currentFilter.Init(this._grfFlags, this._cAttributes, this._aAttributes);
						this._isInternalFilter = true;
						break;
					}
					if (this._currentStream != null)
					{
						this._currentStream.Close();
						this._currentStream = null;
					}
					this._currentFilter = null;
				}
				if (this._currentFilter == null)
				{
					this._progress = PackageFilter.Progress.FilteringCompleted;
					return;
				}
				this._firstChunkFromFilter = true;
				this._progress = PackageFilter.Progress.FilteringContent;
				break;
			case PackageFilter.Progress.FilteringCompleted:
				break;
			default:
				return;
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0012F5EC File Offset: 0x0012E5EC
		private uint AllocateChunkID()
		{
			Invariant.Assert(this._currentChunkID <= uint.MaxValue);
			this._currentChunkID += 1U;
			return this._currentChunkID;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0012F614 File Offset: 0x0012E614
		private string GetFilterClsid(ContentType contentType, Uri partUri)
		{
			string text = null;
			if (contentType != null && !ContentType.Empty.AreTypeAndSubTypeEqual(contentType))
			{
				text = this.FileTypeGuidFromMimeType(contentType);
			}
			else
			{
				string partExtension = this.GetPartExtension(partUri);
				if (partExtension != null)
				{
					text = this.FileTypeGuidFromFileExtension(partExtension);
				}
			}
			if (text == null)
			{
				return null;
			}
			RegistryKey registryKey = PackageFilter.FindSubkey(Registry.ClassesRoot, PackageFilter.MakeRegistryPath(this._IFilterAddinPath, new string[]
			{
				text
			}));
			if (registryKey == null)
			{
				return null;
			}
			return (string)registryKey.GetValue(null);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0012F688 File Offset: 0x0012E688
		private static RegistryKey FindSubkey(RegistryKey containingKey, string[] keyPath)
		{
			RegistryKey registryKey = containingKey;
			for (int i = 0; i < keyPath.Length; i++)
			{
				if (registryKey == null)
				{
					return null;
				}
				registryKey = registryKey.OpenSubKey(keyPath[i]);
			}
			return registryKey;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0012F6B8 File Offset: 0x0012E6B8
		private string FileTypeGuidFromMimeType(ContentType contentType)
		{
			RegistryKey registryKey = PackageFilter.FindSubkey(Registry.ClassesRoot, this._mimeContentTypeKey);
			RegistryKey registryKey2 = (registryKey == null) ? null : registryKey.OpenSubKey(contentType.ToString());
			if (registryKey2 == null)
			{
				return null;
			}
			string text = (string)registryKey2.GetValue("Extension");
			if (text == null)
			{
				return null;
			}
			return this.FileTypeGuidFromFileExtension(text);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0012F70C File Offset: 0x0012E70C
		private string FileTypeGuidFromFileExtension(string dottedExtensionName)
		{
			RegistryKey registryKey = PackageFilter.FindSubkey(Registry.ClassesRoot, PackageFilter.MakeRegistryPath(this._persistentHandlerKey, new string[]
			{
				dottedExtensionName
			}));
			if (registryKey != null)
			{
				return (string)registryKey.GetValue(null);
			}
			return null;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0012F74C File Offset: 0x0012E74C
		private string GetPartExtension(Uri partUri)
		{
			Invariant.Assert(partUri != null);
			string extension = Path.GetExtension(PackUriHelper.GetStringForPartUri(partUri));
			if (extension == string.Empty)
			{
				return null;
			}
			return extension;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0012F784 File Offset: 0x0012E784
		private static string[] MakeRegistryPath(string[] pathWithGaps, params string[] stopGaps)
		{
			string[] array = (string[])pathWithGaps.Clone();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					array[i] = stopGaps[num];
					num++;
				}
			}
			return array;
		}

		// Token: 0x04000911 RID: 2321
		private readonly string[] _IFilterAddinPath = new string[]
		{
			"CLSID",
			null,
			"PersistentAddinsRegistered",
			"{89BCB740-6119-101A-BCB7-00DD010655AF}"
		};

		// Token: 0x04000912 RID: 2322
		private readonly string[] _mimeContentTypeKey = new string[]
		{
			"MIME",
			"Database",
			"Content Type"
		};

		// Token: 0x04000913 RID: 2323
		private readonly string[] _persistentHandlerKey = new string[]
		{
			null,
			"PersistentHandler"
		};

		// Token: 0x04000914 RID: 2324
		private Package _package;

		// Token: 0x04000915 RID: 2325
		private uint _currentChunkID;

		// Token: 0x04000916 RID: 2326
		private IEnumerator _partIterator;

		// Token: 0x04000917 RID: 2327
		private IFilter _currentFilter;

		// Token: 0x04000918 RID: 2328
		private Stream _currentStream;

		// Token: 0x04000919 RID: 2329
		private bool _firstChunkFromFilter;

		// Token: 0x0400091A RID: 2330
		private PackageFilter.Progress _progress;

		// Token: 0x0400091B RID: 2331
		private bool _isInternalFilter;

		// Token: 0x0400091C RID: 2332
		private IFILTER_INIT _grfFlags;

		// Token: 0x0400091D RID: 2333
		private uint _cAttributes;

		// Token: 0x0400091E RID: 2334
		private FULLPROPSPEC[] _aAttributes;

		// Token: 0x0400091F RID: 2335
		private const string _extension = "Extension";

		// Token: 0x020009C2 RID: 2498
		private enum Progress
		{
			// Token: 0x04003F84 RID: 16260
			FilteringNotStarted,
			// Token: 0x04003F85 RID: 16261
			FilteringCoreProperties,
			// Token: 0x04003F86 RID: 16262
			FilteringContent,
			// Token: 0x04003F87 RID: 16263
			FilteringCompleted
		}
	}
}
