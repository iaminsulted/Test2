using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float velocity;

	public float duration;

	public void Start()
	{
		Object.Destroy(base.gameObject, duration);
	}

	public void Update()
	{
		base.transform.position += base.transform.forward * velocity * Time.deltaTime;
	}
}
