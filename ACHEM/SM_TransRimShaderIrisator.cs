using UnityEngine;

public class SM_TransRimShaderIrisator : MonoBehaviour
{
	public float topStr = 2f;

	public float botStr = 1f;

	public float minSpeed = 1f;

	public float maxSpeed = 1f;

	private float speed;

	private float timeGoes;

	private bool timeGoesUp = true;

	private Renderer r;

	public void RandomizeSpeed()
	{
		speed = Random.Range(minSpeed, maxSpeed);
	}

	public void Start()
	{
		r = GetComponent<Renderer>();
		timeGoes = botStr;
		speed = Random.Range(minSpeed, maxSpeed);
	}

	public void Update()
	{
		if (timeGoes > topStr)
		{
			timeGoesUp = false;
			RandomizeSpeed();
		}
		if (timeGoes < botStr)
		{
			timeGoesUp = true;
			RandomizeSpeed();
		}
		if (timeGoesUp)
		{
			timeGoes += Time.deltaTime * speed;
		}
		if (!timeGoesUp)
		{
			timeGoes -= Time.deltaTime * speed;
		}
		float value = timeGoes;
		r.material.SetFloat("_AllPower", value);
	}
}
