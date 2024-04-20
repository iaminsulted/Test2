using UnityEngine;

public class ParticleAccentOnBar : MonoBehaviour
{
	public ParticleSystem[] ParticleEmitter;

	private float particleXLocation;

	public void ShowParticle()
	{
		ParticleSystem[] particleEmitter = ParticleEmitter;
		for (int i = 0; i < particleEmitter.Length; i++)
		{
			particleEmitter[i].Play();
		}
	}

	public void MoveParticle(float val, int barWidth, bool barGoesLeft)
	{
		particleXLocation = (float)(barWidth - 2) * val + 1f;
		if (barGoesLeft)
		{
			particleXLocation *= -1f;
		}
		base.transform.localPosition = new Vector3(particleXLocation, base.transform.localPosition.y, -100f);
	}
}
