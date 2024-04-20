using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000271 RID: 625
	internal class ContentFilePart : PackagePart
	{
		// Token: 0x06001820 RID: 6176 RVA: 0x0016032F File Offset: 0x0015F32F
		internal ContentFilePart(Package container, Uri uri) : base(container, uri)
		{
			Invariant.Assert(Application.ResourceAssembly != null, "If the entry assembly is null no ContentFileParts should be created");
			this._fullPath = null;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00160358 File Offset: 0x0015F358
		protected override Stream GetStreamCore(FileMode mode, FileAccess access)
		{
			if (this._fullPath == null)
			{
				Uri entryAssemblyLocation = this.GetEntryAssemblyLocation();
				string relativeUri;
				string text;
				string text2;
				string text3;
				BaseUriHelper.GetAssemblyNameAndPart(base.Uri, out relativeUri, out text, out text2, out text3);
				Uri uri = new Uri(entryAssemblyLocation, relativeUri);
				this._fullPath = uri.LocalPath;
			}
			Stream stream = this.CriticalOpenFile(this._fullPath);
			if (stream == null)
			{
				throw new IOException(SR.Get("UnableToLocateResource", new object[]
				{
					base.Uri.ToString()
				}));
			}
			return stream;
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x001603D3 File Offset: 0x0015F3D3
		protected override string GetContentTypeCore()
		{
			return MimeTypeMapper.GetMimeTypeFromUri(new Uri(base.Uri.ToString(), UriKind.RelativeOrAbsolute)).ToString();
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x001603F0 File Offset: 0x0015F3F0
		private Uri GetEntryAssemblyLocation()
		{
			Uri result = null;
			try
			{
				result = new Uri(Application.ResourceAssembly.CodeBase);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00160430 File Offset: 0x0015F430
		private Stream CriticalOpenFile(string filename)
		{
			return File.Open(filename, FileMode.Open, FileAccess.Read, ResourceContainer.FileShare);
		}

		// Token: 0x04000D08 RID: 3336
		private string _fullPath;
	}
}
