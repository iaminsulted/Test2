using System.Collections.Generic;

public class ResponseFavoriteMapAssets : Response
{
	public List<MapAsset> mapAssets;

	public ResponseFavoriteMapAssets(List<MapAsset> mapAssets)
	{
		type = 46;
		cmd = 36;
		this.mapAssets = mapAssets;
	}
}
