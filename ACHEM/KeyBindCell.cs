using System;
using UnityEngine;

public class KeyBindCell : MonoBehaviour
{
	public UILabel KeyLabel;

	public UILabel KeyInput;

	public Keybind keybind;

	public Action<KeyBindCell> Clicked;

	public void Init(Keybind keymap)
	{
		keybind = new Keybind(keymap);
		KeyLabel.text = keybind.name;
		KeyInput.text = ArtixUnity.KeyCodeName(keymap.key);
	}

	public void Reset()
	{
		KeyInput.text = ArtixUnity.KeyCodeName(keybind.key);
	}

	public void SetKey(KeyCode k)
	{
		keybind.key = k;
		KeyInput.text = ArtixUnity.KeyCodeName(k);
	}

	public void onclick()
	{
		KeyInput.text = "Press A Key";
		if (Clicked != null)
		{
			Clicked(this);
		}
	}
}
