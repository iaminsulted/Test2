using UnityEngine;

public class WebApiRequestConnectGuestToLoginFB : WebApiRequest
{
	private const string PARAM_FB_ACCESS_TOKEN = "FBAccessToken";

	private const string PARAM_GUEST_TOKEN = "GuestToken";

	private string _fbAccessToken;

	private string _guestToken;

	public WebApiRequestConnectGuestToLoginFB(string fbAccessToken, string guestToken)
	{
		_fbAccessToken = fbAccessToken;
		_guestToken = guestToken;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("FBAccessToken", _fbAccessToken);
		wWWForm.AddField("GuestToken", _guestToken);
		return wWWForm;
	}
}
