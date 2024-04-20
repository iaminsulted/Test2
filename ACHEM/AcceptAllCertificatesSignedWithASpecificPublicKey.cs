using UnityEngine.Networking;

public class AcceptAllCertificatesSignedWithASpecificPublicKey : CertificateHandler
{
	protected override bool ValidateCertificate(byte[] certificateData)
	{
		return true;
	}
}
