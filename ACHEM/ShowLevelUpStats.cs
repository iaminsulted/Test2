using StatCurves;
using UnityEngine;

public class ShowLevelUpStats : UIWindow
{
	public UILabel healthLabel;

	public UILabel attackLabel;

	public UILabel armorLabel;

	public UILabel critLabel;

	public UILabel evasionLabel;

	public UILabel hasteLabel;

	public GameObject lockIcon;

	public UISprite Chest;

	public UILabel treasureChestText;

	public AudioSource chestAura;

	private float increaseTo;

	private void OnEnable()
	{
		increaseTo = GameCurves.GetExpectedStatFromLevel(Entities.Instance.me.Level) - GameCurves.GetExpectedStatFromLevel(Entities.Instance.me.Level - 1);
		increaseTo = Mathf.Round(increaseTo);
		healthLabel.text = "+" + increaseTo;
		attackLabel.text = "+" + increaseTo;
		armorLabel.text = "+" + increaseTo;
		critLabel.text = "+" + increaseTo;
		evasionLabel.text = "+" + increaseTo;
		hasteLabel.text = "+" + increaseTo;
		lockIcon.SetActive(value: false);
		if (Entities.Instance.me.Level < 4)
		{
			Chest.color = new Color32(168, 168, 168, byte.MaxValue);
			lockIcon.SetActive(value: true);
			treasureChestText.text = "Start Earning Chests at Level 4!";
		}
	}

	private void Start()
	{
		base.Init();
	}

	public void ShakeCamera()
	{
		Game.Instance.camController.PlayCameraShake(SpellCamShake.Style.Jitter);
		AudioManager.Play2DSFX("SFX_StatIncrease");
	}

	public void ShakeCameraHard()
	{
		Game.Instance.camController.PlayCameraShake(SpellCamShake.Style.Jitter, 1.2f, 0.3f);
		AudioManager.Play2DSFX("Chest_Catch_Heavy");
	}

	public void PlayAuraSound()
	{
		chestAura.Play();
	}

	public override void Close()
	{
		base.Close();
	}

	public void ShowTreausreChestShop()
	{
		if (Entities.Instance.me.Level >= 4)
		{
			UIWindow.ClearWindows();
			UIPreviewLoot.LoadShop();
		}
	}
}
