using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UITutorialPopup : MonoBehaviour
{
	private static List<GameObject> instances = new List<GameObject>();

	[FormerlySerializedAs("tutorial")]
	public Tutorial Tutorial;

	private Action callback;

	public static bool Show(Tutorial tutorial, Action callback = null)
	{
		if (UIGame.Instance == null)
		{
			return false;
		}
		string text = ((UIGame.ControlScheme == ControlScheme.PC) ? "PC" : "Mobile");
		int num = (int)tutorial;
		GameObject gameObject = Resources.Load<GameObject>("UIElements/Tutorial" + num + text);
		if (gameObject == null)
		{
			return false;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, UIGame.Instance.Container);
		instances.Add(gameObject2);
		gameObject2.GetComponent<UITutorialPopup>().callback = callback;
		return true;
	}

	public static bool IsPopupShown(Tutorial tutorial)
	{
		foreach (GameObject instance in instances)
		{
			if (instance.GetComponent<UITutorialPopup>().Tutorial == tutorial)
			{
				return true;
			}
		}
		return false;
	}

	public static void Clear()
	{
		foreach (GameObject instance in instances)
		{
			UnityEngine.Object.Destroy(instance);
		}
		instances.Clear();
	}

	private void OnEnable()
	{
		Session.MyPlayerData.QuestObjectiveUpdated += OnQuestObjectiveUpdated;
		if (Tutorial == Tutorial.Look)
		{
			Game.Instance.camController.CameraMoved += OnCameraMoved;
			InputManager.DisableInput();
		}
		else if (Tutorial == Tutorial.SelectTarget)
		{
			Entities.Instance.me.TargetSelected += OnTargetSelected;
		}
		else if (Tutorial == Tutorial.QuestTracker)
		{
			UIGame.Instance.QuestTracker.Compass.IsFlashing = true;
			UIGame.Instance.QuestTracker.IsPulsing = true;
		}
		else if (Tutorial != Tutorial.Combat && Tutorial == Tutorial.Jump)
		{
			Entities.Instance.me.moveController.Jumped += OnJumped;
		}
	}

	private void OnDisable()
	{
		Session.MyPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
		if (Tutorial == Tutorial.Look)
		{
			Game.Instance.camController.CameraMoved -= OnCameraMoved;
			Game.Instance.EnableControls();
		}
		else if (Tutorial == Tutorial.SelectTarget)
		{
			Entities.Instance.me.TargetSelected -= OnTargetSelected;
		}
		else if (Tutorial == Tutorial.QuestTracker)
		{
			UIGame.Instance.QuestTracker.Compass.IsFlashing = false;
			UIGame.Instance.QuestTracker.IsPulsing = false;
		}
		else if (Tutorial == Tutorial.Jump)
		{
			Entities.Instance.me.moveController.Jumped -= OnJumped;
		}
	}

	private void OnJumped()
	{
		if (Tutorial == Tutorial.Jump)
		{
			Close();
		}
	}

	private void OnTargetSelected(Entity entity)
	{
		if (Tutorial == Tutorial.SelectTarget)
		{
			Close();
		}
	}

	private void OnCameraMoved()
	{
		if (Tutorial == Tutorial.Look)
		{
			Close();
		}
	}

	private void OnQuestObjectiveUpdated(int index, int value)
	{
		if (Tutorial != Tutorial.Look)
		{
			Close();
		}
	}

	private void Close()
	{
		instances.Remove(base.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
		if (callback != null)
		{
			callback();
		}
	}
}
