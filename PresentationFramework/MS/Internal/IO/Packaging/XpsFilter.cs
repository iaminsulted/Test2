using System;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200016D RID: 365
	[ComVisible(true)]
	[Guid("0B8732A6-AF74-498c-A251-9DC86B0538B0")]
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class XpsFilter : IFilter, IPersistFile, IPersistStream
	{
		// Token: 0x06000C38 RID: 3128 RVA: 0x0012FC04 File Offset: 0x0012EC04
		IFILTER_FLAGS IFilter.Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes)
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147467259);
			}
			if (cAttributes > 0U && aAttributes == null)
			{
				throw new COMException(SR.Get("FilterInitInvalidAttributes"), -2147024809);
			}
			return this._filter.Init(grfFlags, cAttributes, aAttributes);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0012FC58 File Offset: 0x0012EC58
		STAT_CHUNK IFilter.GetChunk()
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			STAT_CHUNK chunk;
			try
			{
				chunk = this._filter.GetChunk();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147215616)
				{
					this.ReleaseResources();
				}
				throw;
			}
			return chunk;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0012FCB8 File Offset: 0x0012ECB8
		void IFilter.GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			if (pBuffer == IntPtr.Zero)
			{
				throw new NullReferenceException(SR.Get("FilterNullGetTextBufferPointer"));
			}
			if (bufCharacterCount == 0U)
			{
				return;
			}
			if (bufCharacterCount == 1U)
			{
				Marshal.WriteInt16(pBuffer, 0);
				return;
			}
			int num = (int)bufCharacterCount;
			if (bufCharacterCount > 4096U)
			{
				bufCharacterCount = 4096U;
			}
			uint num2 = bufCharacterCount - 1U;
			bufCharacterCount = num2;
			uint num3 = num2;
			this._filter.GetText(ref bufCharacterCount, pBuffer);
			if (bufCharacterCount > num3)
			{
				throw new COMException(SR.Get("AuxiliaryFilterReturnedAnomalousCountOfCharacters"), -2147215613);
			}
			if (num == 2 && Marshal.ReadInt16(pBuffer) == 0)
			{
				bufCharacterCount = 2U;
				this._filter.GetText(ref bufCharacterCount, pBuffer);
				if (bufCharacterCount > 2U)
				{
					throw new COMException(SR.Get("AuxiliaryFilterReturnedAnomalousCountOfCharacters"), -2147215613);
				}
				if (bufCharacterCount == 2U)
				{
					Invariant.Assert(Marshal.ReadInt16(pBuffer, 2) == 0);
					bufCharacterCount = 1U;
				}
			}
			Marshal.WriteInt16(pBuffer, (int)(bufCharacterCount * 2U), 0);
			bufCharacterCount += 1U;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0012FDB1 File Offset: 0x0012EDB1
		IntPtr IFilter.GetValue()
		{
			if (this._filter == null)
			{
				throw new COMException(SR.Get("FileToFilterNotLoaded"), -2147215613);
			}
			return this._filter.GetValue();
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0012D27E File Offset: 0x0012C27E
		IntPtr IFilter.BindRegion([In] FILTERREGION origPos, [In] ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0012FDDB File Offset: 0x0012EDDB
		void IPersistFile.GetClassID(out Guid pClassID)
		{
			pClassID = XpsFilter._filterClsid;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0012FDE8 File Offset: 0x0012EDE8
		[PreserveSig]
		int IPersistFile.GetCurFile(out string ppszFileName)
		{
			ppszFileName = null;
			if (this._filter == null || this._xpsFileName == null)
			{
				ppszFileName = "*.xps";
				return 1;
			}
			ppszFileName = this._xpsFileName;
			return 0;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		[PreserveSig]
		int IPersistFile.IsDirty()
		{
			return 1;
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0012FE10 File Offset: 0x0012EE10
		void IPersistFile.Load(string pszFileName, int dwMode)
		{
			if (pszFileName == null || pszFileName == string.Empty)
			{
				throw new ArgumentException(SR.Get("FileNameNullOrEmpty"), "pszFileName");
			}
			if ((dwMode & 4096) == 4096)
			{
				throw new ArgumentException(SR.Get("FilterLoadInvalidModeFlag"), "dwMode");
			}
			FileMode fileMode = FileMode.Open;
			STGM_FLAGS stgm_FLAGS = (STGM_FLAGS)(dwMode & 3);
			if (stgm_FLAGS == STGM_FLAGS.READ || stgm_FLAGS == STGM_FLAGS.READWRITE)
			{
				FileAccess fileAccess = FileAccess.Read;
				FileShare fileSharing = FileShare.ReadWrite;
				Invariant.Assert(this._package == null || this._encryptedPackage == null);
				this.ReleaseResources();
				this._filter = null;
				this._xpsFileName = null;
				bool flag = EncryptedPackageEnvelope.IsEncryptedPackageEnvelope(pszFileName);
				try
				{
					this._packageStream = XpsFilter.FileToStream(pszFileName, fileMode, fileAccess, fileSharing, 1048576L);
					if (flag)
					{
						this._encryptedPackage = EncryptedPackageEnvelope.Open(this._packageStream);
						this._filter = new EncryptedPackageFilter(this._encryptedPackage);
					}
					else
					{
						this._package = Package.Open(this._packageStream);
						this._filter = new PackageFilter(this._package);
					}
				}
				catch (IOException ex)
				{
					throw new COMException(ex.Message, -2147215613);
				}
				catch (FileFormatException ex2)
				{
					throw new COMException(ex2.Message, -2147215604);
				}
				finally
				{
					if (this._filter == null)
					{
						this.ReleaseResources();
					}
				}
				this._xpsFileName = pszFileName;
				return;
			}
			throw new ArgumentException(SR.Get("FilterLoadInvalidModeFlag"), "dwMode");
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0012FF84 File Offset: 0x0012EF84
		void IPersistFile.Save(string pszFileName, bool fRemember)
		{
			throw new COMException(SR.Get("FilterIPersistFileIsReadOnly"), -2147286781);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IPersistFile.SaveCompleted(string pszFileName)
		{
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0012FDDB File Offset: 0x0012EDDB
		void IPersistStream.GetClassID(out Guid pClassID)
		{
			pClassID = XpsFilter._filterClsid;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		[PreserveSig]
		int IPersistStream.IsDirty()
		{
			return 1;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0012FF9C File Offset: 0x0012EF9C
		void IPersistStream.Load(IStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			Invariant.Assert(this._package == null || this._encryptedPackage == null);
			this.ReleaseResources();
			this._filter = null;
			this._xpsFileName = null;
			try
			{
				this._packageStream = new UnsafeIndexingFilterStream(stream);
				if (EncryptedPackageEnvelope.IsEncryptedPackageEnvelope(this._packageStream))
				{
					this._encryptedPackage = EncryptedPackageEnvelope.Open(this._packageStream);
					this._filter = new EncryptedPackageFilter(this._encryptedPackage);
				}
				else
				{
					this._package = Package.Open(this._packageStream);
					this._filter = new PackageFilter(this._package);
				}
			}
			catch (IOException ex)
			{
				throw new COMException(ex.Message, -2147215613);
			}
			catch (Exception ex2)
			{
				throw new COMException(ex2.Message, -2147215604);
			}
			finally
			{
				if (this._filter == null)
				{
					this.ReleaseResources();
				}
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0013009C File Offset: 0x0012F09C
		void IPersistStream.Save(IStream stream, bool fClearDirty)
		{
			throw new COMException(SR.Get("FilterIPersistStreamIsReadOnly"), -2147286781);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x001300B2 File Offset: 0x0012F0B2
		void IPersistStream.GetSizeMax(out long pcbSize)
		{
			throw new NotSupportedException(SR.Get("FilterIPersistFileIsReadOnly"));
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x001300C4 File Offset: 0x0012F0C4
		private void ReleaseResources()
		{
			if (this._encryptedPackage != null)
			{
				this._encryptedPackage.Close();
				this._encryptedPackage = null;
			}
			else if (this._package != null)
			{
				this._package.Close();
				this._package = null;
			}
			if (this._packageStream != null)
			{
				this._packageStream.Close();
				this._packageStream = null;
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00130124 File Offset: 0x0012F124
		private static Stream FileToStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileSharing, long maxMemoryStream)
		{
			long length = new FileInfo(filePath).Length;
			Stream stream = new FileStream(filePath, fileMode, fileAccess, fileSharing);
			if (length < maxMemoryStream)
			{
				MemoryStream memoryStream = new MemoryStream((int)length);
				using (stream)
				{
					PackagingUtilities.CopyStream(stream, memoryStream, length, 4096);
				}
				stream = memoryStream;
			}
			return stream;
		}

		// Token: 0x0400092E RID: 2350
		[ComVisible(false)]
		private static readonly Guid _filterClsid = new Guid(193409702U, 44916, 18828, 162, 81, 157, 200, 107, 5, 56, 176);

		// Token: 0x0400092F RID: 2351
		[ComVisible(false)]
		private IFilter _filter;

		// Token: 0x04000930 RID: 2352
		[ComVisible(false)]
		private Package _package;

		// Token: 0x04000931 RID: 2353
		[ComVisible(false)]
		private EncryptedPackageEnvelope _encryptedPackage;

		// Token: 0x04000932 RID: 2354
		[ComVisible(false)]
		private string _xpsFileName;

		// Token: 0x04000933 RID: 2355
		[ComVisible(false)]
		private Stream _packageStream;

		// Token: 0x04000934 RID: 2356
		[ComVisible(false)]
		private const int _int16Size = 2;

		// Token: 0x04000935 RID: 2357
		[ComVisible(false)]
		private const uint _maxTextBufferSizeInCharacters = 4096U;

		// Token: 0x04000936 RID: 2358
		[ComVisible(false)]
		private const int _maxMemoryStreamBuffer = 1048576;
	}
}
