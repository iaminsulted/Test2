using UnityEngine;

public class WebApiRequestStopReconnect : WebApiRequest
{
	private const string PARAM_CHARID = "CharID";

	private const string PARAM_TOKEN = "Token";

	public int charID;

	public string token;

	public WebApiRequestStopReconnect(int charID, string token)
	{
		this.charID = charID;
		this.token = token;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("CharID", charID);
		wWWForm.AddField("Token", token);
		return wWWForm;
	}
}
