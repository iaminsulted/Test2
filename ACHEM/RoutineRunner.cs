using System;
using System.Collections;
using UnityEngine;

public class RoutineRunner : MonoBehaviour
{
	public static RoutineRunner Instance;

	private void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void RunCoroutine(float delay, Action action)
	{
		StartCoroutine(RunCoroutine_R(delay, action));
	}

	private IEnumerator RunCoroutine_R(float delay, Action action)
	{
		yield return new WaitForSeconds(delay);
		action();
	}
}
