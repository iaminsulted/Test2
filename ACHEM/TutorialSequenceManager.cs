using System;
using System.Collections;
using UnityEngine;

public class TutorialSequenceManager : MonoBehaviour
{
	private static TutorialSequenceManager instance;

	public Tutorial Tutorial;

	private Action callback;

	private TutorialEnumerator tutorialEnumerator;

	private Coroutine coroutine;

	private Coroutine subcoroutine;

	private bool isPlaying;

	private void Awake()
	{
		instance = this;
	}

	public static void SkipToStep(int stepNumber)
	{
		instance.tutorialEnumerator.Current.Completed -= instance.DoNextStep;
		int order = instance.tutorialEnumerator.Current.Order;
		int num = stepNumber - order - 1;
		for (int i = 0; i < num; i++)
		{
			instance.tutorialEnumerator.MoveNext();
		}
		instance.DoNextStep();
	}

	public static bool Show(Tutorial tutorial, Action callback = null)
	{
		if (instance == null)
		{
			GameObject obj = new GameObject("Tutorial Sequence Manager");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			instance = obj.AddComponent<TutorialSequenceManager>();
		}
		TutorialStep[] array = Resources.LoadAll<TutorialStep>("TutorialSequences/" + tutorial);
		if (array == null || array.Length == 0)
		{
			return false;
		}
		if (instance.isPlaying && instance.Tutorial == tutorial)
		{
			return false;
		}
		instance.Initialize(tutorial, array, callback);
		return true;
	}

	private void Initialize(Tutorial tutorial, TutorialStep[] tutorialSteps, Action callback)
	{
		if (tutorialEnumerator != null && tutorialEnumerator.Current != null)
		{
			tutorialEnumerator.Current.Completed -= DoNextStep;
			tutorialEnumerator.Dispose();
			tutorialEnumerator = null;
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}
			if (subcoroutine != null)
			{
				StopCoroutine(subcoroutine);
			}
		}
		isPlaying = true;
		Tutorial = tutorial;
		this.callback = callback;
		tutorialEnumerator = new TutorialEnumerator(tutorialSteps);
		DoNextStep();
	}

	private void DoNextStep()
	{
		coroutine = StartCoroutine(DoNextStepCoroutine());
	}

	private IEnumerator DoNextStepCoroutine()
	{
		if (tutorialEnumerator.Current != null)
		{
			tutorialEnumerator.Current.Completed -= DoNextStep;
		}
		if (tutorialEnumerator.MoveNext())
		{
			tutorialEnumerator.Current.Completed += DoNextStep;
			subcoroutine = StartCoroutine(tutorialEnumerator.Current.Start());
			while (!tutorialEnumerator.Current.IsStarted)
			{
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}
		tutorialEnumerator.Dispose();
		tutorialEnumerator = null;
		if (callback != null)
		{
			callback();
		}
		isPlaying = false;
	}
}
