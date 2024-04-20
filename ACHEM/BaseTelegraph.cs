using UnityEngine;

public abstract class BaseTelegraph
{
	private static AssetBundle assetBundle;

	public Vector3 OffsetPosition;

	public float OffsetRotation;

	protected string MainTextureName;

	private static AssetBundle getAssetBundle
	{
		get
		{
			if (assetBundle == null)
			{
				assetBundle = AssetBundleManager.GetBundle(Main.AssetBundleURL + Session.Account.Characters_Bundle.FileName);
			}
			return assetBundle;
		}
	}

	protected Texture MainTexture => getAssetBundle.LoadAsset<Texture>(MainTextureName);

	protected BaseTelegraph(Vector3 offsetPosition, float offsetRotation, string mainTextureName)
	{
		OffsetPosition = offsetPosition;
		OffsetRotation = offsetRotation;
		MainTextureName = mainTextureName;
	}

	public virtual void Draw(Projector projector, Color color)
	{
	}
}
