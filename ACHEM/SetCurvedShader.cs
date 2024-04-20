using UnityEngine;

public class SetCurvedShader : MonoBehaviour
{
	[Range(-0.005f, 0.005f)]
	public float attenuateXfloor = -0.005f;

	[Range(-0.005f, 0.005f)]
	public float attenuateXceiling = 0.005f;

	[Range(-0.005f, 0.005f)]
	public float attenuateYfloor = -0.005f;

	[Range(-0.005f, 0.005f)]
	public float attenuateYceiling = 0.005f;

	public bool randomize = true;

	[Range(-0.005f, 0.005f)]
	public float attenuatex;

	[Range(-0.005f, 0.005f)]
	public float attenuatey;

	public float horizonOffset;

	public float spread = 1f;

	public bool moveCurve;

	private float SecondsTilNextChange;

	private float SecondsNow;

	private float newattenuatex;

	private float newattenuatey;

	private float lastattenuatex;

	private float lastattenuatey;

	public float smooth = 0.5f;

	private float currentTime;

	public float timeToMove = 0.35f;

	public float timePause = 0.75f;

	private void Start()
	{
	}

	private void Update()
	{
		Shader.SetGlobalFloat("_SPREAD", spread);
		Shader.SetGlobalFloat("_ATTENUATEX", attenuatex);
		Shader.SetGlobalFloat("_ATTENUATEY", attenuatey);
		Shader.SetGlobalFloat("_HORIZON", horizonOffset);
		if (moveCurve)
		{
			if (currentTime <= timeToMove)
			{
				currentTime += Time.deltaTime;
				attenuatex = Mathf.Lerp(lastattenuatex, newattenuatex, currentTime / timeToMove);
				attenuatey = Mathf.Lerp(lastattenuatey, newattenuatey, currentTime / timeToMove);
			}
			else
			{
				lastattenuatex = newattenuatex;
				lastattenuatey = newattenuatey;
				currentTime = 0f;
				newattenuatex = Random.Range(attenuateXfloor, attenuateXceiling);
				newattenuatey = Random.Range(attenuateYfloor, attenuateYceiling);
				timeToMove = Random.Range(0.5f, 10f);
				moveCurve = false;
			}
		}
		if (!moveCurve)
		{
			if (currentTime <= timePause)
			{
				currentTime += Time.deltaTime;
				return;
			}
			timePause = Random.Range(0.5f, 10f);
			currentTime = 0f;
			moveCurve = true;
		}
	}

	private void selectattenuations()
	{
		if (SecondsNow > SecondsTilNextChange)
		{
			SecondsNow = 0f;
			SecondsTilNextChange = Random.Range(10f, 30f);
			Debug.Log("Seconds " + SecondsNow + ":" + SecondsTilNextChange);
			lastattenuatex = newattenuatex;
			lastattenuatey = newattenuatey;
			newattenuatex = Random.Range(attenuateXfloor, attenuateXceiling);
			newattenuatey = Random.Range(attenuateYfloor, attenuateYceiling);
			Debug.Log("attX - " + newattenuatex + ": attY -" + newattenuatey);
		}
		else
		{
			SecondsNow += Time.deltaTime;
		}
	}
}
