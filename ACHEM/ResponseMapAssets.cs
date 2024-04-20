using System.Collections.Generic;

public class ResponseMapAssets : Response
{
	public List<MapAsset> mapAssets;

	public ResponseMapAssets(List<MapAsset> mapAssets)
	{
		type = 46;
		cmd = 34;
		this.mapAssets = mapAssets;
	}
}
