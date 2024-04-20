using UnityEngine;

public abstract class WebApiRequestLogin : WebApiRequest
{
	private const string PARAM_CLIENT_ID = "ClientID";

	private const string PARAM_DEVICE_UNIQUE_IDENTIFIER = "DeviceID";

	private const string PARAM_BUILD_PLATFORM = "BuildPlatform";

	protected WWWForm GetWWWFormWithStandardParams()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("ClientID", Main.ClientID);
		wWWForm.AddField("DeviceID", SystemInfo.deviceUniqueIdentifier);
		wWWForm.AddField("BuildPlatform", (int)BuildPlatform.Get);
		return wWWForm;
	}
}
