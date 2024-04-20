using UnityEngine;

public class WebApiRequestAddLoginAQ3D : WebApiRequest
{
	private const string PARAM_EMAIL = "Email";

	private const string PARAM_PASSWORD = "Password";

	private const string PARAM_APPLE_JWT = "AppleJWT";

	private string _email;

	private string _password;

	private string _appleJWT;

	public WebApiRequestAddLoginAQ3D(string email, string password, string appleJWT)
	{
		_email = email;
		_password = password;
		_appleJWT = appleJWT;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Email", Util.Base64Encode(_email));
		wWWForm.AddField("Password", Util.Base64Encode(_password));
		wWWForm.AddField("AppleJWT", _appleJWT);
		return wWWForm;
	}
}
