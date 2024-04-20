using System;
using System.IO;
using System.IO.Packaging;
using System.Resources;
using System.Windows;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x02000296 RID: 662
	internal class ResourcePart : PackagePart
	{
		// Token: 0x06001906 RID: 6406 RVA: 0x0016251C File Offset: 0x0016151C
		public ResourcePart(Package container, Uri uri, string name, ResourceManagerWrapper rmWrapper) : base(container, uri)
		{
			if (rmWrapper == null)
			{
				throw new ArgumentNullException("rmWrapper");
			}
			this._rmWrapper.Value = rmWrapper;
			this._name = name;
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x00162554 File Offset: 0x00161554
		protected override Stream GetStreamCore(FileMode mode, FileAccess access)
		{
			Stream stream = this.EnsureResourceLocationSet();
			if (stream == null)
			{
				stream = this._rmWrapper.Value.GetStream(this._name);
				if (stream == null)
				{
					throw new IOException(SR.Get("UnableToLocateResource", new object[]
					{
						this._name
					}));
				}
			}
			ContentType contentType = new ContentType(base.ContentType);
			if (MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
			{
				stream = new BamlStream(stream, this._rmWrapper.Value.Assembly);
			}
			return stream;
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x001625D7 File Offset: 0x001615D7
		protected override string GetContentTypeCore()
		{
			this.EnsureResourceLocationSet();
			return MimeTypeMapper.GetMimeTypeFromUri(new Uri(this._name, UriKind.RelativeOrAbsolute)).ToString();
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x001625F8 File Offset: 0x001615F8
		private Stream EnsureResourceLocationSet()
		{
			object globalLock = this._globalLock;
			lock (globalLock)
			{
				if (this._ensureResourceIsCalled)
				{
					return null;
				}
				this._ensureResourceIsCalled = true;
				try
				{
					if (string.Compare(Path.GetExtension(this._name), ".baml", StringComparison.OrdinalIgnoreCase) == 0)
					{
						throw new IOException(SR.Get("UnableToLocateResource", new object[]
						{
							this._name
						}));
					}
					if (string.Compare(Path.GetExtension(this._name), ".xaml", StringComparison.OrdinalIgnoreCase) == 0)
					{
						string name = Path.ChangeExtension(this._name, ".baml");
						Stream stream = this._rmWrapper.Value.GetStream(name);
						if (stream != null)
						{
							this._name = name;
							return stream;
						}
					}
				}
				catch (MissingManifestResourceException)
				{
				}
			}
			return null;
		}

		// Token: 0x04000D79 RID: 3449
		private SecurityCriticalDataForSet<ResourceManagerWrapper> _rmWrapper;

		// Token: 0x04000D7A RID: 3450
		private bool _ensureResourceIsCalled;

		// Token: 0x04000D7B RID: 3451
		private string _name;

		// Token: 0x04000D7C RID: 3452
		private object _globalLock = new object();
	}
}
