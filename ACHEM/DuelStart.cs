using UnityEngine;

public class DuelStart : MonoBehaviour
{
	public int CounterTime;

	public UILabel CounterText;

	public Animator Anime;

	public ParticleSystem ParticleSystem;

	public static void Show()
	{
		Object.Instantiate(Resources.Load<GameObject>("UIElements/DuelStart"), UIManager.Instance.transform);
	}

	private void Start()
	{
		ParticleSystem.gameObject.SetActive(value: false);
		InvokeRepeating("CountDown", 1f, 1f);
	}

	private void CountDown()
	{
		CounterTime--;
		CounterText.text = CounterTime.ToString();
		if (CounterTime <= 0)
		{
			Anime.SetBool("Start", value: true);
			ActivateParticles();
			CounterText.text = "Duel!";
			Invoke("Remove", 0.5f);
		}
	}

	private void Remove()
	{
		Object.Destroy(base.transform.parent.gameObject);
	}

	private void ActivateParticles()
	{
		ParticleSystem.gameObject.SetActive(value: true);
		ParticleSystem.Play();
	}
}
