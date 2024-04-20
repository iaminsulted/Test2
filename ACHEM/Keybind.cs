using UnityEngine;

public class Keybind
{
	public string name;

	public KeyCode key;

	public InputAction action;

	public Keybind()
	{
	}

	public Keybind(Keybind keybind)
	{
		name = keybind.name;
		key = keybind.key;
		action = keybind.action;
	}
}
