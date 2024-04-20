using UnityEngine;

public class WebApiRequestUserDelete : WebApiRequest
{
	private const string PARAM_UID = "uid";

	private const string PARAM_TOKEN = "strToken";

	private int uid;

	private string token;

	public WebApiRequestUserDelete(int uid, string token)
	{
		this.uid = uid;
		this.token = token;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("strToken", token);
		return wWWForm;
	}
}
