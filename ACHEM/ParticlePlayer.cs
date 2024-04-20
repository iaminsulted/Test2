using System.Collections;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
	private ParticleSystem[] childParticleSystem;

	private void OnEnable()
	{
		childParticleSystem = GetComponentsInChildren<ParticleSystem>();
		StartCoroutine(Increase());
	}

	private IEnumerator Increase()
	{
		float speed = 1f;
		while (speed <= 2f)
		{
			for (int i = 0; i < childParticleSystem.Length; i++)
			{
				speed = Mathf.Lerp(speed, 2f, Time.deltaTime);
				ParticleSystem.MainModule main = childParticleSystem[i].main;
				main.simulationSpeed = speed;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private void OnDisable()
	{
		StopCoroutine(Increase());
	}
}
