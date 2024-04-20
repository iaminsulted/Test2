public class WebApiResponseLoginFacebook : WebApiResponseLogin
{
	public static int FacebookError_InvalidAccessToken = 2;

	public static int FacebookError_GraphDidNotReturnValidEmail = 3;

	public static int FacebookError_AskUserIntentionForAQ3DAccount = 4;

	public static int FacebookError_HasNotAcceptedTOS = 5;

	public string CharacterName => Message;
}
