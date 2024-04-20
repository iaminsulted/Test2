using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WebSocketSharp.Net
{
	// Token: 0x0200003D RID: 61
	public class ClientSslConfiguration
	{
		// Token: 0x0600040E RID: 1038 RVA: 0x000180DC File Offset: 0x000162DC
		public ClientSslConfiguration()
		{
			this._enabledSslProtocols = SslProtocols.Default;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000180F1 File Offset: 0x000162F1
		public ClientSslConfiguration(string targetHost)
		{
			this._targetHost = targetHost;
			this._enabledSslProtocols = SslProtocols.Default;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00018110 File Offset: 0x00016310
		public ClientSslConfiguration(ClientSslConfiguration configuration)
		{
			bool flag = configuration == null;
			if (flag)
			{
				throw new ArgumentNullException("configuration");
			}
			this._checkCertRevocation = configuration._checkCertRevocation;
			this._clientCertSelectionCallback = configuration._clientCertSelectionCallback;
			this._clientCerts = configuration._clientCerts;
			this._enabledSslProtocols = configuration._enabledSslProtocols;
			this._serverCertValidationCallback = configuration._serverCertValidationCallback;
			this._targetHost = configuration._targetHost;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00018180 File Offset: 0x00016380
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00018198 File Offset: 0x00016398
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x000181A4 File Offset: 0x000163A4
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x000181BC File Offset: 0x000163BC
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return this._clientCerts;
			}
			set
			{
				this._clientCerts = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x000181C8 File Offset: 0x000163C8
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x000181FF File Offset: 0x000163FF
		public LocalCertificateSelectionCallback ClientCertificateSelectionCallback
		{
			get
			{
				bool flag = this._clientCertSelectionCallback == null;
				if (flag)
				{
					this._clientCertSelectionCallback = new LocalCertificateSelectionCallback(ClientSslConfiguration.defaultSelectClientCertificate);
				}
				return this._clientCertSelectionCallback;
			}
			set
			{
				this._clientCertSelectionCallback = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001820C File Offset: 0x0001640C
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00018224 File Offset: 0x00016424
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00018230 File Offset: 0x00016430
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00018267 File Offset: 0x00016467
		public RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				bool flag = this._serverCertValidationCallback == null;
				if (flag)
				{
					this._serverCertValidationCallback = new RemoteCertificateValidationCallback(ClientSslConfiguration.defaultValidateServerCertificate);
				}
				return this._serverCertValidationCallback;
			}
			set
			{
				this._serverCertValidationCallback = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00018274 File Offset: 0x00016474
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0001828C File Offset: 0x0001648C
		public string TargetHost
		{
			get
			{
				return this._targetHost;
			}
			set
			{
				this._targetHost = value;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00018298 File Offset: 0x00016498
		private static X509Certificate defaultSelectClientCertificate(object sender, string targetHost, X509CertificateCollection clientCertificates, X509Certificate serverCertificate, string[] acceptableIssuers)
		{
			return null;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000182AC File Offset: 0x000164AC
		private static bool defaultValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		// Token: 0x040001A3 RID: 419
		private bool _checkCertRevocation;

		// Token: 0x040001A4 RID: 420
		private LocalCertificateSelectionCallback _clientCertSelectionCallback;

		// Token: 0x040001A5 RID: 421
		private X509CertificateCollection _clientCerts;

		// Token: 0x040001A6 RID: 422
		private SslProtocols _enabledSslProtocols;

		// Token: 0x040001A7 RID: 423
		private RemoteCertificateValidationCallback _serverCertValidationCallback;

		// Token: 0x040001A8 RID: 424
		private string _targetHost;
	}
}
