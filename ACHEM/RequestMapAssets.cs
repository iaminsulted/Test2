public class RequestMapAssets : Request
{
	public int assetBundleID;

	public RequestMapAssets(int assetBundleID)
	{
		type = 46;
		cmd = 33;
		this.assetBundleID = assetBundleID;
	}
}
