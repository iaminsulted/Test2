using UnityEngine;

public class WebApiRequestLoginAQ3D : WebApiRequestLogin
{
	private const string PARAM_EMAIL = "Email";

	private const string PARAM_PASSWORD = "Password";

	private string _email;

	private string _password;

	public WebApiRequestLoginAQ3D(string email, string password)
	{
		_email = email;
		_password = password;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWFormWithStandardParams = GetWWWFormWithStandardParams();
		wWWFormWithStandardParams.AddField("Email", Util.Base64Encode(_email));
		wWWFormWithStandardParams.AddField("Password", Util.Base64Encode(_password));
		return wWWFormWithStandardParams;
	}
}
