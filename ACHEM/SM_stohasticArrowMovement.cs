using UnityEngine;

public class SM_stohasticArrowMovement : MonoBehaviour
{
	public float rotSpeed = 3f;

	public float rotRandomPlus = 0.5f;

	public float rotTreshold = 50f;

	public float changeRotMin = 1f;

	public float changeRotMax = 2f;

	public float minSpeed = 0.5f;

	public float maxSpeed = 2f;

	public float changeSpeedMin = 0.5f;

	public float changeSpeedMax = 2f;

	private float speed;

	private float timeGoesX;

	private float timeGoesY;

	private float timeGoesSpeed;

	private float timeToChangeX = 0.1f;

	private float timeToChangeY = 0.1f;

	private float timeToChangeSpeed = 0.1f;

	private bool xDir = true;

	private bool yDir = true;

	private float curRotSpeedX;

	private float curRotSpeedY;

	private float lendX;

	private float lendY;

	public void RandomizeSpeed()
	{
		speed = Random.Range(minSpeed, maxSpeed);
	}

	public void RandomizeXRot()
	{
		float num = Random.value * rotRandomPlus;
		curRotSpeedX = rotSpeed * num;
	}

	public void RandomizeYRot()
	{
		float num = Random.value * rotRandomPlus;
		curRotSpeedY = rotSpeed * num;
	}

	public void Start()
	{
		RandomizeSpeed();
		if ((double)Random.value > 0.5)
		{
			xDir = !xDir;
		}
		if ((double)Random.value > 0.5)
		{
			yDir = !yDir;
		}
		timeToChangeY = Random.Range(changeRotMin, changeRotMax);
		timeToChangeX = Random.Range(changeRotMin, changeRotMax);
		timeToChangeSpeed = Random.Range(changeSpeedMin, changeSpeedMax);
		curRotSpeedX = Random.Range(rotSpeed, rotSpeed + rotRandomPlus);
		curRotSpeedY = Random.Range(rotSpeed, rotSpeed + rotRandomPlus);
	}

	public void Update()
	{
		if (xDir)
		{
			lendX += Time.deltaTime * curRotSpeedX;
		}
		if (!xDir)
		{
			lendX -= Time.deltaTime * curRotSpeedX;
		}
		if (yDir)
		{
			lendY += Time.deltaTime * curRotSpeedY;
		}
		if (!yDir)
		{
			lendY -= Time.deltaTime * curRotSpeedY;
		}
		if (lendX > rotTreshold)
		{
			lendX = rotTreshold;
			xDir = !xDir;
		}
		if (lendX > rotTreshold)
		{
			lendX = 0f - rotTreshold;
			xDir = !xDir;
		}
		if (lendY > rotTreshold)
		{
			lendY = rotTreshold;
			yDir = !yDir;
		}
		if (lendY > rotTreshold)
		{
			lendY = 0f - rotTreshold;
			yDir = !yDir;
		}
		base.transform.Rotate(lendX * Time.deltaTime, lendY * Time.deltaTime, 0f);
		base.transform.Translate(0f, speed * Time.deltaTime, 0f);
		timeGoesX += Time.deltaTime;
		timeGoesY += Time.deltaTime;
		timeGoesSpeed += Time.deltaTime;
		if (timeGoesX > timeToChangeX)
		{
			xDir = !xDir;
			timeGoesX -= timeGoesX;
			timeToChangeX = Random.Range(changeRotMin, changeRotMax);
			curRotSpeedX = Random.Range(rotSpeed, rotSpeed + rotRandomPlus);
		}
		if (timeGoesY > timeToChangeY)
		{
			yDir = !yDir;
			timeGoesY -= timeGoesY;
			timeToChangeY = Random.Range(changeRotMin, changeRotMax);
			curRotSpeedY = Random.Range(rotSpeed, rotSpeed + rotRandomPlus);
		}
		if (timeGoesSpeed > timeToChangeSpeed)
		{
			RandomizeSpeed();
			timeGoesSpeed -= timeGoesSpeed;
			timeToChangeSpeed = Random.Range(changeSpeedMin, changeSpeedMax);
			Debug.Log("hejj");
		}
	}
}
