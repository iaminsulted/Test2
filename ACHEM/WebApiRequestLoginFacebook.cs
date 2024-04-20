using UnityEngine;

public class WebApiRequestLoginFacebook : WebApiRequestLogin
{
	private const string PARAM_GUEST_TOKEN = "GuestToken";

	private const string PARAM_FB_ACCESS_TOKEN = "FBAccessToken";

	private const string PARAM_AQ3D_USER_ID = "AQ3DUserID";

	private const string PARAM_INTENTION_FOR_AQ3D_ACCOUNT = "IntentionForAQ3DAccount";

	private const string PARAM_VERIFIED_AQ3D_EMAIL = "VerifiedAQ3DEmail";

	private const string PARAM_VERIFIED_AQ3D_PASSWORD = "VerifiedAQ3DPassword";

	private const string PARAM_LINK_SOURCE = "LinkSource";

	private const string PARAM_ACCEPTED_TERMS = "AcceptedTerms";

	private int _hasAcceptedTerms;

	private string _guestToken;

	private string _fbAccessToken;

	private int _aq3dUserID;

	private int _intentionForAQ3DAccount;

	private string _verifiedAQ3DEmail;

	private string _verifiedAQ3DPassword;

	private string _linkSource;

	public WebApiRequestLoginFacebook(string guestToken, string fbAccessToken, int aq3dUserID, int intentionForAQ3DAccount = 0, string verifiedAQ3DEmail = "", string verifiedAQ3DPassword = "", string linkSource = "", int acceptedTerms = 0)
	{
		_guestToken = guestToken;
		_fbAccessToken = fbAccessToken;
		_aq3dUserID = aq3dUserID;
		_intentionForAQ3DAccount = intentionForAQ3DAccount;
		_verifiedAQ3DEmail = verifiedAQ3DEmail;
		_verifiedAQ3DPassword = verifiedAQ3DPassword;
		_linkSource = linkSource;
		_hasAcceptedTerms = acceptedTerms;
	}

	public override WWWForm GetWWWForm()
	{
		WWWForm wWWFormWithStandardParams = GetWWWFormWithStandardParams();
		wWWFormWithStandardParams.AddField("GuestToken", _guestToken);
		wWWFormWithStandardParams.AddField("FBAccessToken", _fbAccessToken);
		wWWFormWithStandardParams.AddField("AQ3DUserID", _aq3dUserID);
		wWWFormWithStandardParams.AddField("IntentionForAQ3DAccount", _intentionForAQ3DAccount);
		wWWFormWithStandardParams.AddField("VerifiedAQ3DEmail", _verifiedAQ3DEmail);
		wWWFormWithStandardParams.AddField("VerifiedAQ3DPassword", _verifiedAQ3DPassword);
		wWWFormWithStandardParams.AddField("LinkSource", _linkSource);
		wWWFormWithStandardParams.AddField("AcceptedTerms", _hasAcceptedTerms);
		return wWWFormWithStandardParams;
	}
}
