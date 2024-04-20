using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WebSocketSharp.Net
{
	// Token: 0x0200003E RID: 62
	public class ServerSslConfiguration
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x000182BF File Offset: 0x000164BF
		public ServerSslConfiguration()
		{
			this._enabledSslProtocols = SslProtocols.Default;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000182D4 File Offset: 0x000164D4
		public ServerSslConfiguration(X509Certificate2 serverCertificate)
		{
			this._serverCert = serverCertificate;
			this._enabledSslProtocols = SslProtocols.Default;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000182F0 File Offset: 0x000164F0
		public ServerSslConfiguration(ServerSslConfiguration configuration)
		{
			bool flag = configuration == null;
			if (flag)
			{
				throw new ArgumentNullException("configuration");
			}
			this._checkCertRevocation = configuration._checkCertRevocation;
			this._clientCertRequired = configuration._clientCertRequired;
			this._clientCertValidationCallback = configuration._clientCertValidationCallback;
			this._enabledSslProtocols = configuration._enabledSslProtocols;
			this._serverCert = configuration._serverCert;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00018354 File Offset: 0x00016554
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0001836C File Offset: 0x0001656C
		public bool CheckCertificateRevocation
		{
			get
			{
				return this._checkCertRevocation;
			}
			set
			{
				this._checkCertRevocation = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00018378 File Offset: 0x00016578
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00018390 File Offset: 0x00016590
		public bool ClientCertificateRequired
		{
			get
			{
				return this._clientCertRequired;
			}
			set
			{
				this._clientCertRequired = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0001839C File Offset: 0x0001659C
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x000183D3 File Offset: 0x000165D3
		public RemoteCertificateValidationCallback ClientCertificateValidationCallback
		{
			get
			{
				bool flag = this._clientCertValidationCallback == null;
				if (flag)
				{
					this._clientCertValidationCallback = new RemoteCertificateValidationCallback(ServerSslConfiguration.defaultValidateClientCertificate);
				}
				return this._clientCertValidationCallback;
			}
			set
			{
				this._clientCertValidationCallback = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x000183E0 File Offset: 0x000165E0
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x000183F8 File Offset: 0x000165F8
		public SslProtocols EnabledSslProtocols
		{
			get
			{
				return this._enabledSslProtocols;
			}
			set
			{
				this._enabledSslProtocols = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00018404 File Offset: 0x00016604
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0001841C File Offset: 0x0001661C
		public X509Certificate2 ServerCertificate
		{
			get
			{
				return this._serverCert;
			}
			set
			{
				this._serverCert = value;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00018428 File Offset: 0x00016628
		private static bool defaultValidateClientCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		// Token: 0x040001A9 RID: 425
		private bool _checkCertRevocation;

		// Token: 0x040001AA RID: 426
		private bool _clientCertRequired;

		// Token: 0x040001AB RID: 427
		private RemoteCertificateValidationCallback _clientCertValidationCallback;

		// Token: 0x040001AC RID: 428
		private SslProtocols _enabledSslProtocols;

		// Token: 0x040001AD RID: 429
		private X509Certificate2 _serverCert;
	}
}
