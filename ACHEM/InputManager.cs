using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
	private static bool allowInput;

	private static readonly Dictionary<int, List<KeyCode>> map;

	public static List<KeyCode> KeyCodeBlackList;

	public static bool InputEnabled
	{
		get
		{
			if (!UICamera.inputHasFocus)
			{
				return allowInput;
			}
			return false;
		}
	}

	public static event Action<InputAction> ActionEvent;

	static InputManager()
	{
		allowInput = true;
		map = new Dictionary<int, List<KeyCode>>();
		KeyCodeBlackList = new List<KeyCode>
		{
			KeyCode.None,
			KeyCode.Backspace,
			KeyCode.LeftArrow,
			KeyCode.RightArrow,
			KeyCode.UpArrow,
			KeyCode.DownArrow,
			KeyCode.Mouse0,
			KeyCode.Mouse1
		};
		InitMap();
		SettingsManager.KeyMappingUpdated += OnKeysUpdated;
	}

	public static void EnableInput()
	{
		allowInput = true;
	}

	public static void DisableInput()
	{
		allowInput = false;
	}

	private static void InitMap()
	{
		foreach (Keybind value in SettingsManager.Keybinds.Values)
		{
			SetActionKey(value.action, value.key);
		}
		AddKeyToActionMap(InputAction.Forward, KeyCode.UpArrow);
		AddKeyToActionMap(InputAction.Backward, KeyCode.DownArrow);
		AddKeyToActionMap(InputAction.LeftRotate, KeyCode.LeftArrow);
		AddKeyToActionMap(InputAction.RightRotate, KeyCode.RightArrow);
		AddKeyToActionMap(InputAction.MouseLeft, KeyCode.Mouse0);
		AddKeyToActionMap(InputAction.MouseRight, KeyCode.Mouse1);
		AddKeyToActionMap(InputAction.MouseMiddle, KeyCode.Mouse2);
		AddKeyToActionMap(InputAction.Cancel, KeyCode.Escape);
	}

	public static void Update()
	{
		if (!Input.anyKey)
		{
			return;
		}
		foreach (int key in map.Keys)
		{
			if (GetActionKeyDown((InputAction)key))
			{
				OnActionEvent((InputAction)key);
			}
		}
	}

	public static void OnKeysUpdated()
	{
		InitMap();
	}

	private static void AddKeyToActionMap(InputAction action, KeyCode keyCode)
	{
		if (map.TryGetValue((int)action, out var _))
		{
			map[(int)action].Add(keyCode);
			return;
		}
		map[(int)action] = new List<KeyCode> { keyCode };
	}

	private static List<KeyCode> GetActionKeys(InputAction action)
	{
		if (map.TryGetValue((int)action, out var value))
		{
			return value;
		}
		return null;
	}

	public static void OnActionEvent(InputAction action)
	{
		if (!UICamera.inputHasFocus)
		{
			InputManager.ActionEvent?.Invoke(action);
		}
	}

	private static void SetActionKey(InputAction action, KeyCode keyCode)
	{
		map[(int)action] = new List<KeyCode> { keyCode };
	}

	public static bool GetActionKey(InputAction action)
	{
		if (!InputEnabled)
		{
			return false;
		}
		foreach (KeyCode actionKey in GetActionKeys(action))
		{
			if (Input.GetKey(actionKey))
			{
				return true;
			}
		}
		return false;
	}

	public static bool GetActionKeyDown(InputAction action)
	{
		if (!InputEnabled)
		{
			return false;
		}
		foreach (KeyCode actionKey in GetActionKeys(action))
		{
			if (Input.GetKeyDown(actionKey))
			{
				return true;
			}
		}
		return false;
	}

	public static bool GetActionKeyUp(InputAction action)
	{
		if (!InputEnabled)
		{
			return false;
		}
		foreach (KeyCode actionKey in GetActionKeys(action))
		{
			if (Input.GetKeyUp(actionKey))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsMouseInWindow()
	{
		Vector2 vector = Input.mousePosition;
		if (vector.x >= 0f && vector.x <= (float)Screen.width && vector.y >= 0f)
		{
			return vector.y <= (float)Screen.height;
		}
		return false;
	}
}
