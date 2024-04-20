using UnityEngine;

public class UniPasteBoardDemo : MonoBehaviour
{
	private string s = "Input here...";

	private int screenWidth;

	private void Start()
	{
		screenWidth = Screen.width;
	}

	private void OnGUI()
	{
		s = GUILayout.TextArea(s, GUILayout.Width(screenWidth), GUILayout.MinHeight(200f));
		if (GUILayout.Button("Copy to System paste board", GUILayout.Height(80f)))
		{
			UniPasteBoard.SetClipBoardString(s);
		}
		if (GUILayout.Button("Paste From System paste board", GUILayout.Height(80f)))
		{
			s = UniPasteBoard.GetClipBoardString();
		}
	}
}
