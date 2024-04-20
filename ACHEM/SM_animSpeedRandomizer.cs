using UnityEngine;

public class SM_animSpeedRandomizer : MonoBehaviour
{
	public float minSpeed = 0.7f;

	public float maxSpeed = 1.5f;

	public void Start()
	{
		Animation component = GetComponent<Animation>();
		if (!(component != null))
		{
			return;
		}
		foreach (AnimationState item in component)
		{
			item.speed = Random.Range(minSpeed, maxSpeed);
		}
	}
}
