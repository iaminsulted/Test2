using System;
using CinemaDirector;
using UnityEngine;

public class GeneralSkip : MonoBehaviour
{
	public static GeneralSkip Instance;

	public static Action Callback;

	public static Cutscene CutsceneObject;

	public static Action Skipped;

	private void Awake()
	{
		Instance = this;
	}

	public void Skip()
	{
		try
		{
			if (CutsceneObject != null)
			{
				CutsceneObject.Stop();
			}
		}
		catch
		{
		}
		if (Skipped != null)
		{
			Skipped();
		}
	}

	public static void Close()
	{
		if (Callback != null)
		{
			Callback();
		}
		if (Instance != null)
		{
			Instance.close();
		}
		Callback = null;
		Instance = null;
		CutsceneObject = null;
	}

	public void close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static void Show(Cutscene cutsceneobject = null, Action callback = null)
	{
		if (Instance == null)
		{
			CutsceneObject = cutsceneobject;
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GeneralSkip"), UIManager.Instance.transform);
			if (callback != null)
			{
				Callback = callback;
			}
		}
	}
}
