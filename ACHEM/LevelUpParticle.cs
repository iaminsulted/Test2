using TMPro;
using UnityEngine;

public class LevelUpParticle : MonoBehaviour
{
	public TextMeshProUGUI frontText;

	public GameObject shield;

	public Animator swordAndShieldAnimator;

	private void Start()
	{
		if ((bool)frontText)
		{
			frontText.text = base.transform.parent.GetComponent<EntityController>().Entity.Level.ToString() ?? "";
		}
		Invoke("TurnOnShields", 5f);
		Invoke("TurnOffShield", 15f);
	}

	private void TurnOnShields()
	{
		shield.SetActive(value: true);
		AudioManager.Play3DSFX("SFX_LevelUP", SFXType.UI, base.transform);
	}

	private void TurnOffShield()
	{
		swordAndShieldAnimator.SetTrigger("Finish");
	}

	public void DestroyNow()
	{
		Object.Destroy(base.gameObject);
	}
}
