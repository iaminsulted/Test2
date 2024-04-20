using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RandomUV : MonoBehaviour
{
	[SerializeField]
	private bool useTextureOffset;

	[SerializeField]
	private string texturePropertyName = "_MainTex";

	[SerializeField]
	private string offsetXPropertyName = "_XMove";

	[SerializeField]
	private string offsetYPropertyName = "_YMove";

	[SerializeField]
	private Vector2 rangeU = Vector2.zero;

	[SerializeField]
	private Vector2 rangeV = Vector2.zero;

	[SerializeField]
	private float timeToMove = 0.35f;

	[SerializeField]
	private float timePause = 0.75f;

	private Material mat;

	private Vector2 U;

	private Vector2 V;

	private float currentTime;

	private bool moving;

	private bool overrideName;

	private void Awake()
	{
		mat = GetComponent<Renderer>().material;
		if (mat == null)
		{
			Object.Destroy(this);
		}
		if (useTextureOffset)
		{
			if (!mat.HasProperty(texturePropertyName))
			{
				Object.Destroy(this);
				return;
			}
			Vector2 textureOffset = mat.GetTextureOffset(texturePropertyName);
			U.x = textureOffset.x;
			U.y = Random.Range(rangeU.x, rangeU.y);
			V.x = textureOffset.y;
			V.y = Random.Range(rangeV.x, rangeV.y);
			return;
		}
		if (mat.HasProperty(offsetXPropertyName))
		{
			U.x = mat.GetFloat(offsetXPropertyName);
		}
		else
		{
			U.x = 0f;
		}
		if (mat.HasProperty(offsetXPropertyName))
		{
			V.x = mat.GetFloat(offsetYPropertyName);
		}
		else
		{
			V.x = 0f;
		}
		U.y = Random.Range(rangeU.x, rangeU.y);
		V.y = Random.Range(rangeV.x, rangeV.y);
	}

	private void Update()
	{
		if (moving)
		{
			if (currentTime <= timeToMove)
			{
				currentTime += Time.deltaTime;
				float num = Mathf.Lerp(U.x, U.y, currentTime / timeToMove);
				float num2 = Mathf.Lerp(V.x, V.y, currentTime / timeToMove);
				if (useTextureOffset)
				{
					mat.SetTextureOffset(texturePropertyName, new Vector2(num, num2));
				}
				else
				{
					mat.SetFloat(offsetXPropertyName, num);
					mat.SetFloat(offsetYPropertyName, num2);
				}
			}
			else
			{
				currentTime = 0f;
				U.x = U.y;
				U.y = Random.Range(rangeU.x, rangeU.y);
				V.x = V.y;
				V.y = Random.Range(rangeV.x, rangeV.y);
				moving = false;
			}
		}
		if (!moving)
		{
			if (currentTime <= timePause)
			{
				currentTime += Time.deltaTime;
				return;
			}
			currentTime = 0f;
			moving = true;
		}
	}
}
