using UnityEngine;

namespace Assets.Scripts.UI;

public class PtrWatermark : MonoBehaviour
{
	public UILabel buildLabel;

	public UILabel platformLabel;

	public void Init(int buildNumber)
	{
		buildLabel.text = "PTRb" + buildNumber;
		platformLabel.text = Platform.GetPlatformName();
	}
}
