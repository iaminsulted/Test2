using System;
using UnityEngine;

[RequireComponent(typeof(Stereo3D))]
public class Stereo3DGUI : MonoBehaviour
{
	private Stereo3D _s3d;

	private bool[] b;

	private bool on = true;

	private void Awake()
	{
		_s3d = GetComponent<Stereo3D>();
	}

	private void Start()
	{
		b = new bool[Enum.GetNames(typeof(StereoState)).Length];
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
		int num = 50;
		GUI.Label(new Rect(Screen.width / 2, Screen.height - num - 40, 90f, 20f), "Convergence");
		_s3d.Convergence = GUI.HorizontalSlider(new Rect(Screen.width / 2 + 90, Screen.height - num - 35, 80f, 20f), _s3d.Convergence, _s3d.MinConvergence, _s3d.MaxConvergence);
		GUI.Label(new Rect(Screen.width / 2, Screen.height - num - 20, 90f, 20f), "Interaxial");
		_s3d.Interaxial = GUI.HorizontalSlider(new Rect(Screen.width / 2 + 90, Screen.height - num - 15, 80f, 20f), _s3d.Interaxial, _s3d.MinInteraxial, _s3d.MaxInteraxial);
		for (int i = 0; i < b.Length; i++)
		{
			bool flag = b[i];
			if (b[i])
			{
				GUI.Toggle(new Rect(Screen.width / 2 + 220, Screen.height - num - 60 + 20 * i, 100f, 20f), b[i], Enum.GetNames(typeof(StereoState))[i]);
			}
			else
			{
				b[i] = GUI.Toggle(new Rect(Screen.width / 2 + 220, Screen.height - num - 60 + 20 * i, 100f, 20f), b[i], Enum.GetNames(typeof(StereoState))[i]);
			}
			if (b[i] && !flag)
			{
				b = new bool[Enum.GetNames(typeof(StereoState)).Length];
				b[i] = true;
				_s3d.SetMode((StereoState)i);
			}
		}
	}
}
