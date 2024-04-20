using UnityEngine;

public class SM_TransRimShaderInOut : MonoBehaviour
{
	public float str = 1f;

	public float fadeIn = 1f;

	public float stay = 1f;

	public float fadeOut = 1f;

	private float timeGoes;

	private float currStr;

	private Renderer r;

	public void Start()
	{
		r = GetComponent<Renderer>();
		r.material.SetFloat("_AllPower", currStr);
	}

	public void Update()
	{
		timeGoes += Time.deltaTime;
		if (timeGoes < fadeIn)
		{
			currStr = timeGoes * str * (1f / fadeIn);
		}
		if (timeGoes > fadeIn && timeGoes < fadeIn + stay)
		{
			currStr = str;
		}
		if (timeGoes > fadeIn + stay)
		{
			currStr = str - (timeGoes - (fadeIn + stay)) * (1f / fadeOut);
		}
		r.material.SetFloat("_AllPower", currStr);
	}
}
