using UnityEngine;

[RequireComponent(typeof(CameraBody))]
public class CameraBodyGUI : MonoBehaviour
{
	private CameraBody _cb;

	private bool on = true;

	private int hOffset = 50;

	private int wOffset = 100;

	private void Awake()
	{
		_cb = GetComponent<CameraBody>();
	}

	private void Update()
	{
		on ^= Input.GetKeyDown(KeyCode.U);
	}

	private void OnGUI()
	{
		if (on)
		{
			DrawGUI();
		}
	}

	private void DrawGUI()
	{
		DrawFilmPlaneOffset();
		DrawFocusDistance();
		DrawFStop();
		DrawFocalLength();
	}

	private void DrawFilmPlaneOffset()
	{
		Vector3 filmPlaneOffset = _cb.FilmPlaneOffset;
		GUI.Label(new Rect(wOffset, Screen.height - hOffset - 40, 50f, 20f), "fpo.x");
		GUI.Label(new Rect(wOffset, Screen.height - hOffset - 20, 50f, 20f), "fpo.y");
		GUI.Label(new Rect(wOffset, Screen.height - hOffset, 50f, 20f), "fpo.z");
		if (GUI.RepeatButton(new Rect(50 + wOffset, Screen.height - hOffset - 40, 20f, 20f), "-"))
		{
			filmPlaneOffset.x -= 1f;
		}
		if (GUI.RepeatButton(new Rect(50 + wOffset, Screen.height - hOffset - 20, 20f, 20f), "-"))
		{
			filmPlaneOffset.y -= 1f;
		}
		if (GUI.RepeatButton(new Rect(50 + wOffset, Screen.height - hOffset, 20f, 20f), "-"))
		{
			filmPlaneOffset.z -= 1f;
		}
		if (GUI.RepeatButton(new Rect(70 + wOffset, Screen.height - hOffset - 40, 20f, 20f), "+"))
		{
			filmPlaneOffset.x += 1f;
		}
		if (GUI.RepeatButton(new Rect(70 + wOffset, Screen.height - hOffset - 20, 20f, 20f), "+"))
		{
			filmPlaneOffset.y += 1f;
		}
		if (GUI.RepeatButton(new Rect(70 + wOffset, Screen.height - hOffset, 20f, 20f), "+"))
		{
			filmPlaneOffset.z += 1f;
		}
		_cb.FilmPlaneOffset = filmPlaneOffset;
	}

	private void DrawFocusDistance()
	{
		GUI.Label(new Rect(100 + wOffset, Screen.height - hOffset - 40, 50f, 20f), "f dist");
		if (GUI.RepeatButton(new Rect(150 + wOffset, Screen.height - hOffset - 40, 20f, 20f), "-"))
		{
			_cb.FocusDistance--;
		}
		if (GUI.RepeatButton(new Rect(170 + wOffset, Screen.height - hOffset - 40, 20f, 20f), "+"))
		{
			_cb.FocusDistance++;
		}
	}

	private void DrawFStop()
	{
		GUI.Label(new Rect(100 + wOffset, Screen.height - hOffset - 20, 50f, 20f), "fstop");
		if (GUI.Button(new Rect(150 + wOffset, Screen.height - hOffset - 20, 20f, 20f), "-") && _cb.IndexOfFStop > 0)
		{
			_cb.IndexOfFStop--;
		}
		if (GUI.Button(new Rect(170 + wOffset, Screen.height - hOffset - 20, 20f, 20f), "+") && _cb.IndexOfFStop < FStop.list.Length)
		{
			_cb.IndexOfFStop++;
		}
	}

	private void DrawFocalLength()
	{
		GUI.Label(new Rect(100 + wOffset, Screen.height - hOffset, 50f, 20f), "f length");
		if (GUI.Button(new Rect(150 + wOffset, Screen.height - hOffset, 20f, 20f), "-"))
		{
			_cb.IndexOfLens--;
		}
		if (GUI.Button(new Rect(170 + wOffset, Screen.height - hOffset, 20f, 20f), "+"))
		{
			_cb.IndexOfLens++;
		}
	}
}
