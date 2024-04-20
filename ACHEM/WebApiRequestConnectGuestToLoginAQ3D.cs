using UnityEngine;

public class WebApiRequestConnectGuestToLoginAQ3D : WebApiRequest
{
	private const string PARAM_EMAIL = "Email";

	private const string PARAM_PASSWORD = "Password";

	private const string PARAM_GUEST_TOKEN = "GuestToken";

	private const string PARAM_BIT_OPT_IN = "BitOptIn";

	private string _email;

	private string _password;

	private string _guestToken;

	private bool _optIn;

	public WebApiRequestConnectGuestToLoginAQ3D(string email, string password, bool optIn, string guestToken)
	{
		_email = email;
		_password = password;
		_optIn = optIn;
		_guestToken = guestToken;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Email", Util.Base64Encode(_email));
		wWWForm.AddField("Password", Util.Base64Encode(_password));
		wWWForm.AddField("BitOptIn", _optIn.ToString());
		wWWForm.AddField("GuestToken", _guestToken);
		return wWWForm;
	}
}
