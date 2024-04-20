using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISettingKeyBinding : MonoBehaviour
{
	public UIScrollView scrollView;

	private List<KeyBindCell> Buttons = new List<KeyBindCell>();

	public Transform KeyObjectParent;

	public GameObject KeyBindObject;

	public GameObject TitleBindObject;

	private KeyBindCell currentKeyBindCell;

	private void Awake()
	{
		Init();
	}

	private void OnEnable()
	{
		scrollView.ResetPosition();
		ResetKeys();
	}

	private void OnDisable()
	{
		currentKeyBindCell = null;
		if (Game.Instance != null)
		{
			Game.Instance.EnableControls();
		}
	}

	private void Init()
	{
		TitleBindObject.SetActive(value: false);
		KeyBindObject.SetActive(value: false);
		List<Keybind> list = SettingsManager.Keybinds.Select((KeyValuePair<InputAction, Keybind> entry) => entry.Value).ToList();
		GameObject obj = UnityEngine.Object.Instantiate(TitleBindObject);
		obj.GetComponent<UITableHeading>().Init();
		obj.transform.SetParent(KeyObjectParent, worldPositionStays: false);
		obj.SetActive(value: true);
		foreach (Keybind item in list)
		{
			if (item.action == InputAction.Target_Next)
			{
				GameObject obj2 = UnityEngine.Object.Instantiate(TitleBindObject);
				obj2.GetComponent<UITableHeading>().Init("Combat");
				obj2.transform.SetParent(KeyObjectParent, worldPositionStays: false);
				obj2.SetActive(value: true);
			}
			else if (item.action == InputAction.UI_Toggle_Inventory)
			{
				GameObject obj3 = UnityEngine.Object.Instantiate(TitleBindObject);
				obj3.GetComponent<UITableHeading>().Init("Interface");
				obj3.transform.SetParent(KeyObjectParent, worldPositionStays: false);
				obj3.SetActive(value: true);
			}
			GameObject obj4 = UnityEngine.Object.Instantiate(KeyBindObject);
			obj4.transform.SetParent(KeyObjectParent, worldPositionStays: false);
			obj4.SetActive(value: true);
			KeyBindCell component = obj4.GetComponent<KeyBindCell>();
			component.Init(item);
			component.Clicked = (Action<KeyBindCell>)Delegate.Combine(component.Clicked, new Action<KeyBindCell>(OnKeyBindClicked));
			Buttons.Add(component);
		}
		KeyObjectParent.GetComponent<UITable>().Reposition();
	}

	private void ResetKeys()
	{
		foreach (KeyBindCell button in Buttons)
		{
			button.Init(SettingsManager.Keybinds[button.keybind.action]);
		}
		currentKeyBindCell = null;
	}

	public void ResetToDefault()
	{
		foreach (KeyBindCell button in Buttons)
		{
			button.Init(SettingsManager.Default_Key_Maps[button.keybind.action]);
		}
		currentKeyBindCell = null;
		if (Game.Instance != null)
		{
			Game.Instance.EnableControls();
		}
	}

	private void OnKeyBindClicked(KeyBindCell cell)
	{
		if (currentKeyBindCell != null)
		{
			currentKeyBindCell.Reset();
		}
		currentKeyBindCell = ((currentKeyBindCell == cell) ? null : cell);
		if (Game.Instance != null)
		{
			Game.Instance.DisableControls();
		}
	}

	public void Save()
	{
		SettingsManager.SaveKeySettings(Buttons.Select((KeyBindCell p) => p.keybind).ToList());
		Notification.ShowText("Keybinds saved successfully!");
		if (Game.Instance != null)
		{
			Game.Instance.EnableControls();
		}
	}

	private bool IsKeyInUse(KeyCode code)
	{
		foreach (KeyBindCell button in Buttons)
		{
			if (button.keybind.key == code)
			{
				return true;
			}
		}
		return false;
	}

	private KeyBindCell GetKeyBindCellByKeyCode(KeyCode code)
	{
		foreach (KeyBindCell button in Buttons)
		{
			if (button.keybind.key == code)
			{
				return button;
			}
		}
		return null;
	}

	private void OnGUI()
	{
		if (currentKeyBindCell == null || !Event.current.isKey)
		{
			return;
		}
		KeyCode keyCode = Event.current.keyCode;
		KeyCode key = currentKeyBindCell.keybind.key;
		if (keyCode == KeyCode.Escape)
		{
			currentKeyBindCell.Reset();
			currentKeyBindCell = null;
		}
		else
		{
			if (InputManager.KeyCodeBlackList.Contains(keyCode))
			{
				return;
			}
			if (keyCode != key)
			{
				if (IsKeyInUse(keyCode))
				{
					KeyBindCell keyBindCellByKeyCode = GetKeyBindCellByKeyCode(keyCode);
					keyBindCellByKeyCode.SetKey(key);
					Notification.ShowWarning(keyBindCellByKeyCode.KeyLabel.text + " rebound to '" + key.ToString() + "'");
				}
				currentKeyBindCell.SetKey(keyCode);
			}
			else
			{
				currentKeyBindCell.Reset();
				if (Game.Instance != null)
				{
					Game.Instance.EnableControls();
				}
			}
			currentKeyBindCell = null;
		}
	}
}
