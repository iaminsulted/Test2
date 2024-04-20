using UnityEngine;

public class UIButtonGlowParticle : MonoBehaviour
{
	private float GlowScaleX;

	private float GlowScaleY;

	public UISprite Sprite;

	private void Start()
	{
		if (Sprite == null)
		{
			UISprite component = base.transform.parent.GetComponent<UISprite>();
			GlowScaleX = component.width / 50;
			GlowScaleY = component.height / 50;
		}
		else
		{
			GlowScaleX = Sprite.width / 50;
			GlowScaleY = Sprite.height / 50;
		}
		base.transform.localScale = new Vector3(GlowScaleX, GlowScaleY, 1f);
	}

	private void Update()
	{
	}
}
