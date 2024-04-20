using UnityEngine;

public class ScreenSound : MonoBehaviour
{
	public AudioClip AudioScreenChange;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void PlayScreenSound()
	{
		NGUITools.PlaySound(AudioScreenChange, 2f);
	}
}
