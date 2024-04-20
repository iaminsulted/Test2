public static class SteamStore
{
	public const string WEBAPIPWD = "Art789369!";

	public const string STARTNEWTXN = "StartNewTransaction?";

	public const string AUTHORIZETXN = "OnAuthorized?";

	public static InitTxnResponse NewTxnResponse;

	public static FinalizeTxnResponse FinalTxnResponse;

	public static string GetWEBAPIURL => Main.WebServiceURL + "/steam/";

	static SteamStore()
	{
		NewTxnResponse = new InitTxnResponse();
		FinalTxnResponse = new FinalizeTxnResponse();
	}
}
