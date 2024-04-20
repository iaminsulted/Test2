using UnityEngine;

public class WebApiRequestLoginGuest : WebApiRequestLogin
{
	private const string PARAM_GUEST_TOKEN = "GuestToken";

	private const string PARAM_LINK_SOURCE = "LinkSource";

	private const string PARAM_ACCEPTED_TERMS = "AcceptedTerms";

	private string _guestToken;

	private string _linkSource;

	private int _hasAcceptedTerms;

	public WebApiRequestLoginGuest(string guestToken, string linkSource, int acceptedTerms = 0)
	{
		_guestToken = guestToken;
		_linkSource = linkSource;
		_hasAcceptedTerms = acceptedTerms;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWFormWithStandardParams = GetWWWFormWithStandardParams();
		wWWFormWithStandardParams.AddField("GuestToken", _guestToken);
		wWWFormWithStandardParams.AddField("LinkSource", _linkSource);
		wWWFormWithStandardParams.AddField("AcceptedTerms", _hasAcceptedTerms);
		return wWWFormWithStandardParams;
	}
}
