using UnityEngine.Networking;

public class UnityWebRequestHelper
{
	public static string AppendCloudFlareRay(UnityWebRequest w, string customContext)
	{
		return customContext + " CF-Ray: " + w.GetResponseHeader("CF-Ray");
	}

	public static string GetCloudFlareRay(UnityWebRequest w)
	{
		return "CF-Ray: " + w.GetResponseHeader("CF-Ray");
	}
}
