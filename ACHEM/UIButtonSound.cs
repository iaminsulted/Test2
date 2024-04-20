using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class UIButtonSound : MonoBehaviour
{
	public enum ButtonType
	{
		Silent,
		VeryLight,
		Light,
		Medium,
		Heavy,
		Heaviest,
		Settings
	}

	public ButtonType type = ButtonType.VeryLight;

	private void Start()
	{
	}

	public void OnClick()
	{
		switch (type)
		{
		case ButtonType.Light:
			AudioManager.Play2DSFX("SFX_UI_Light");
			break;
		case ButtonType.Medium:
			AudioManager.Play2DSFX("SFX_UI_Medium");
			break;
		case ButtonType.Heavy:
			AudioManager.Play2DSFX("SFX_UI_Login");
			break;
		case ButtonType.Heaviest:
			AudioManager.Play2DSFX("SFX_UI_Play");
			break;
		case ButtonType.Settings:
			AudioManager.Play2DSFX("SFX_UI_Settings");
			break;
		case ButtonType.Silent:
		case ButtonType.VeryLight:
			break;
		}
	}
}
