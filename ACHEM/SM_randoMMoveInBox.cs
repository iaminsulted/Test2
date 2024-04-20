using UnityEngine;

public class SM_randoMMoveInBox : MonoBehaviour
{
	public float xspeed = 1f;

	public float yspeed = 1.5f;

	public float zspeed = 2f;

	public float speedDeviation;

	public float xDim = 0.3f;

	public float yDim = 0.3f;

	public float zDim = 0.3f;

	public void Start()
	{
		base.transform.localPosition = new Vector3(0f, 0f, 0f);
		xspeed += (float)Random.Range(-1, 1) * speedDeviation;
		yspeed += (float)Random.Range(-1, 1) * speedDeviation;
		zspeed += (float)Random.Range(-1, 1) * speedDeviation;
	}

	public void Update()
	{
		base.transform.Translate(new Vector3(xspeed, yspeed, zspeed) * Time.deltaTime);
		if (base.transform.localPosition.x > xDim)
		{
			xspeed = 0f - Mathf.Abs(xspeed);
		}
		if (base.transform.localPosition.x < 0f - xDim)
		{
			xspeed = Mathf.Abs(xspeed);
		}
		if (base.transform.localPosition.y > yDim)
		{
			yspeed = 0f - Mathf.Abs(yspeed);
		}
		if (base.transform.localPosition.y < 0f - yDim)
		{
			yspeed = Mathf.Abs(yspeed);
		}
		if (base.transform.localPosition.z > zDim)
		{
			zspeed = 0f - Mathf.Abs(zspeed);
		}
		if (base.transform.localPosition.z < 0f - zDim)
		{
			zspeed = Mathf.Abs(zspeed);
		}
	}
}
