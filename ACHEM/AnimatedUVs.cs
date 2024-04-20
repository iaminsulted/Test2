using UnityEngine;

public class AnimatedUVs : MonoBehaviour
{
	public int materialIndex;

	public Vector2 uvAnimationRate = new Vector2(1f, 0f);

	public string textureName1 = "_DetailMap";

	private Vector2 uvOffset = Vector2.zero;

	private void LateUpdate()
	{
		uvOffset += uvAnimationRate * Time.deltaTime;
		if (GetComponent<Renderer>().enabled)
		{
			GetComponent<Renderer>().materials[materialIndex].SetTextureOffset(textureName1, uvOffset);
		}
	}
}
