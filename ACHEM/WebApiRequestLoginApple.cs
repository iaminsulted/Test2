using UnityEngine;

public class WebApiRequestLoginApple : WebApiRequestLogin
{
	private const string PARAM_ID_TOKEN = "IdToken";

	private const string PARAM_LINK_SOURCE = "LinkSource";

	private const string PARAM_ACCEPTED_TERMS = "AcceptedTerms";

	private string _idToken;

	private string _linkSource;

	private int _hasAcceptedTerms;

	public WebApiRequestLoginApple(string idToken, string linkSource, int acceptedTerms = 0)
	{
		_idToken = idToken;
		_linkSource = linkSource;
		_hasAcceptedTerms = acceptedTerms;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWFormWithStandardParams = GetWWWFormWithStandardParams();
		wWWFormWithStandardParams.AddField("IdToken", _idToken);
		wWWFormWithStandardParams.AddField("LinkSource", _linkSource);
		wWWFormWithStandardParams.AddField("AcceptedTerms", _hasAcceptedTerms);
		return wWWFormWithStandardParams;
	}
}
