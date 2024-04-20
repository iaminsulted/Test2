using UnityEngine;

public class PulseColor : MonoBehaviour
{
	public UISprite Sprite;

	public Color Color1;

	public Color Color2;

	private Color origColor;

	private float startTime;

	private void OnEnable()
	{
		origColor = Sprite.color;
		startTime = Time.time;
		Sprite.color = Color1;
	}

	private void OnDisable()
	{
		Sprite.color = origColor;
	}

	private void Start()
	{
	}

	private void Update()
	{
		Sprite.color = Color.Lerp(Color1, Color2, Mathf.PingPong((Time.time - startTime) * 2f, 1f));
	}
}
