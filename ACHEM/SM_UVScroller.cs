using UnityEngine;

public class SM_UVScroller : MonoBehaviour
{
	public int targetMaterialSlot;

	public float speedY = 0.5f;

	public float speedX;

	private float timeWentX;

	private float timeWentY;

	private Renderer r;

	public void Start()
	{
		r = GetComponent<Renderer>();
	}

	public void Update()
	{
		timeWentY += Time.deltaTime * speedY;
		timeWentX += Time.deltaTime * speedX;
		r.materials[targetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(timeWentX, timeWentY));
	}
}