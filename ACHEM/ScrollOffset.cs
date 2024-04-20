using UnityEngine;

public class ScrollOffset : MonoBehaviour
{
	public Vector2 ScrollSpeed = new Vector2(0f, 0f);

	public string Tex = "_BlendTex";

	private Vector2 uvOffset = Vector2.zero;

	private Material mat;

	private void Awake()
	{
		mat = GetComponent<Renderer>().sharedMaterial;
	}

	private void LateUpdate()
	{
		uvOffset.x = (uvOffset.x + ScrollSpeed.x * Time.deltaTime) % 1f;
		uvOffset.y = (uvOffset.y + ScrollSpeed.y * Time.deltaTime) % 1f;
		mat.SetTextureOffset(Tex, uvOffset);
	}
}
