using UnityEngine;

public class DisableFog : MonoBehaviour
{
	private bool isFogEnabled;

	private void OnPreRender()
	{
		isFogEnabled = RenderSettings.fog;
		RenderSettings.fog = false;
	}

	private void OnPostRender()
	{
		RenderSettings.fog = isFogEnabled;
	}
}
