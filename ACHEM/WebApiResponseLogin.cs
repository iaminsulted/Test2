public class WebApiResponseLogin : WebApiResponse
{
	public static int SharedError_InvalidClientVersion = 100;

	public Account Account;

	public override bool Success
	{
		get
		{
			if (Error == 0)
			{
				return Account != null;
			}
			return false;
		}
	}

	public bool HasStandardErrorActions()
	{
		if (HasError(WebApiResponse.Error_Basic))
		{
			BusyDialog.Close();
			Loader.close();
			MessageBox.Show("Unable to Login", Message);
			return true;
		}
		return false;
	}
}
