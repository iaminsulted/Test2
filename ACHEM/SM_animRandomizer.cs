using UnityEngine;

public class SM_animRandomizer : MonoBehaviour
{
	public AnimationClip[] animList;

	public AnimationClip actualAnim;

	public float minSpeed = 0.7f;

	public float maxSpeed = 1.5f;

	public void Start()
	{
		int num = (int)Mathf.Floor(Random.Range(0, animList.Length));
		actualAnim = animList[num];
		Animation component = GetComponent<Animation>();
		if (!(component != null))
		{
			return;
		}
		component.Play(actualAnim.name);
		foreach (AnimationState item in component)
		{
			if (item.name == actualAnim.name)
			{
				item.speed = Random.Range(minSpeed, maxSpeed);
			}
		}
	}
}
