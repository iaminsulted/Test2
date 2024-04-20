using System;
using System.ComponentModel;

namespace System.Windows.Data
{
	// Token: 0x0200046D RID: 1133
	public class XmlNamespaceMapping : ISupportInitialize
	{
		// Token: 0x06003A59 RID: 14937 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		public XmlNamespaceMapping()
		{
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x001F064D File Offset: 0x001EF64D
		public XmlNamespaceMapping(string prefix, Uri uri)
		{
			this._prefix = prefix;
			this._uri = uri;
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x001F0663 File Offset: 0x001EF663
		// (set) Token: 0x06003A5C RID: 14940 RVA: 0x001F066C File Offset: 0x001EF66C
		public string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				if (!this._initializing)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsInitializeOnly", new object[]
					{
						"Prefix",
						base.GetType().Name
					}));
				}
				if (this._prefix != null && this._prefix != value)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
					{
						"Prefix",
						base.GetType().Name
					}));
				}
				this._prefix = value;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x001F06F6 File Offset: 0x001EF6F6
		// (set) Token: 0x06003A5E RID: 14942 RVA: 0x001F0700 File Offset: 0x001EF700
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
			set
			{
				if (!this._initializing)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsInitializeOnly", new object[]
					{
						"Uri",
						base.GetType().Name
					}));
				}
				if (this._uri != null && this._uri != value)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
					{
						"Uri",
						base.GetType().Name
					}));
				}
				this._uri = value;
			}
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x001F0790 File Offset: 0x001EF790
		public override bool Equals(object obj)
		{
			return this == obj as XmlNamespaceMapping;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x001F079E File Offset: 0x001EF79E
		public static bool operator ==(XmlNamespaceMapping mappingA, XmlNamespaceMapping mappingB)
		{
			if (mappingA == null)
			{
				return mappingB == null;
			}
			return mappingB != null && mappingA.Prefix == mappingB.Prefix && mappingA.Uri == mappingB.Uri;
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x001F07D3 File Offset: 0x001EF7D3
		public static bool operator !=(XmlNamespaceMapping mappingA, XmlNamespaceMapping mappingB)
		{
			return !(mappingA == mappingB);
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x001F07E0 File Offset: 0x001EF7E0
		public override int GetHashCode()
		{
			int num = 0;
			if (this._prefix != null)
			{
				num = this._prefix.GetHashCode();
			}
			if (this._uri != null)
			{
				return num + this._uri.GetHashCode();
			}
			return num;
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x001F0820 File Offset: 0x001EF820
		void ISupportInitialize.BeginInit()
		{
			this._initializing = true;
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x001F082C File Offset: 0x001EF82C
		void ISupportInitialize.EndInit()
		{
			if (this._prefix == null)
			{
				throw new InvalidOperationException(SR.Get("PropertyMustHaveValue", new object[]
				{
					"Prefix",
					base.GetType().Name
				}));
			}
			if (this._uri == null)
			{
				throw new InvalidOperationException(SR.Get("PropertyMustHaveValue", new object[]
				{
					"Uri",
					base.GetType().Name
				}));
			}
			this._initializing = false;
		}

		// Token: 0x04001DA9 RID: 7593
		private string _prefix;

		// Token: 0x04001DAA RID: 7594
		private Uri _uri;

		// Token: 0x04001DAB RID: 7595
		private bool _initializing;
	}
}
