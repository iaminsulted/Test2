using UnityEngine;

public class WebApiRequestCreateUserLoginAQ3D : WebApiRequest
{
	private const string PARAM_EMAIL = "Email";

	private const string PARAM_PASSWORD = "Password";

	private const string PARAM_BUILD_PLATFORM = "BuildPlatform";

	private const string PARAM_GUEST_TOKEN = "GuestToken";

	private const string PARAM_GUEST_ACTION = "GuestAction";

	private const string PARAM_LINK_SOURCE = "LinkSource";

	private const string PARAM_BIT_OPT_IN = "BitOptIn";

	private string _email;

	private string _password;

	private int _guestAction;

	private string _linkSource;

	private bool _optIn;

	public WebApiRequestCreateUserLoginAQ3D(string email, string password, int guestAction, bool optIn, string linkSource = "")
	{
		_email = email;
		_password = password;
		_guestAction = guestAction;
		_linkSource = linkSource;
		_optIn = optIn;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Email", Util.Base64Encode(_email));
		wWWForm.AddField("Password", Util.Base64Encode(_password));
		wWWForm.AddField("BuildPlatform", (int)BuildPlatform.Get);
		wWWForm.AddField("BitOptIn", _optIn.ToString());
		wWWForm.AddField("LinkSource", _linkSource);
		if (LoginManager.HasGuestGuid() && !LoginManager.HasRealAccount())
		{
			wWWForm.AddField("GuestAction", _guestAction);
			wWWForm.AddField("GuestToken", LoginManager.GetGuestGuid());
		}
		return wWWForm;
	}
}
