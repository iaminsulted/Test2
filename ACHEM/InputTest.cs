using UnityEngine;

public class InputTest : MonoBehaviour
{
	public int UpButton = 119;

	public int DownButton = 100;

	public int AttackButton = 49;

	public bool SetButtons;

	public int count = -1;

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (SetButtons && Event.current != null && Event.current.isKey)
		{
			Debug.Log("event processed");
			if (count == -1)
			{
				count = 0;
			}
			switch (count)
			{
			case 0:
			{
				KeyCode keyCode3 = Event.current.keyCode;
				Debug.Log(keyCode3);
				UpButton = (int)keyCode3;
				count = 1;
				break;
			}
			case 1:
			{
				KeyCode keyCode2 = Event.current.keyCode;
				Debug.Log(keyCode2);
				DownButton = (int)keyCode2;
				count = 2;
				break;
			}
			case 2:
			{
				KeyCode keyCode = Event.current.keyCode;
				Debug.Log(keyCode);
				AttackButton = (int)keyCode;
				count = -1;
				SetButtons = false;
				break;
			}
			}
		}
	}

	private void Update()
	{
	}
}
