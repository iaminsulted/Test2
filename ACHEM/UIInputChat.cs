using System;
using UnityEngine;

[RequireComponent(typeof(UIInput))]
public class UIInputChat : MonoBehaviour
{
	public static Color32 DefaultColor = new Color32(128, 128, 128, 140);

	public static Color32 SelectedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200);

	public UIInput mInput;

	public UIButtonColor btnBG;

	public bool IsSelected => mInput.isSelected;

	public event Action<bool> onFocus;

	public event Action<string> onChatSubmit;

	public event Action<string> onInputChanged;

	public void Update()
	{
		if (!Game.Instance.isInputEnabled)
		{
			return;
		}
		if (!UICamera.inputHasFocus && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !mInput.isSelected)
		{
			mInput.isSelected = true;
		}
		if (Input.GetKeyDown(KeyCode.Slash) && !mInput.isSelected)
		{
			mInput.selectAllTextOnFocus = false;
			mInput.isSelected = true;
			mInput.value = "/";
			if (this.onFocus != null)
			{
				this.onFocus(obj: true);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) && mInput.isSelected)
		{
			mInput.isSelected = false;
		}
	}

	private void OnClick()
	{
	}

	public void OnSubmit(string text)
	{
		mInput.value = "";
		mInput.isSelected = false;
		if (this.onChatSubmit != null)
		{
			this.onChatSubmit(text);
		}
	}

	public void OnInputChanged(UIInput input)
	{
		if (this.onInputChanged != null)
		{
			this.onInputChanged(input.value);
		}
	}

	public void SetPositionX(int x)
	{
	}

	private void OnSelect(bool selected)
	{
		Debug.Log("OnSelect: " + selected);
		this.onFocus(selected);
		btnBG.defaultColor = (selected ? SelectedColor : DefaultColor);
	}

	public void FocusInput()
	{
		if (!mInput.isSelected)
		{
			mInput.isSelected = true;
		}
	}
}
