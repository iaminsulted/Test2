using UnityEngine;

public class UISliderAnimation : MonoBehaviour
{
	private ParticleAccentOnBar particleAccentOnBar;

	private float percentChangePerSecond = 0.5f;

	private float fillEndAmount;

	private UIProgressBar slider;

	private int barWidth;

	private bool barGoesLeft;

	private void Awake()
	{
		base.enabled = false;
	}

	public void Animate(UIProgressBar Slider, float FillEndAmount, float percentChangePerSecond, int BarWidth = 0, bool BarGoesLeft = false, ParticleAccentOnBar ParticleAccentOnBar = null)
	{
		slider = Slider;
		fillEndAmount = FillEndAmount;
		this.percentChangePerSecond = percentChangePerSecond;
		barWidth = BarWidth;
		barGoesLeft = BarGoesLeft;
		if (ParticleAccentOnBar != null)
		{
			particleAccentOnBar = ParticleAccentOnBar;
			ParticleAccentOnBar.ShowParticle();
		}
		base.enabled = true;
	}

	private void LateUpdate()
	{
		UpdateSlider();
		if (particleAccentOnBar != null)
		{
			particleAccentOnBar.MoveParticle(slider.value, barWidth, barGoesLeft);
		}
		if (slider.value == fillEndAmount)
		{
			base.enabled = false;
		}
	}

	private void UpdateSlider()
	{
		if (fillEndAmount >= slider.value)
		{
			slider.value += GetSliderDelta(percentChangePerSecond, fillEndAmount, slider.value);
		}
		else if (slider.value < 1f)
		{
			slider.value += GetSliderDelta(percentChangePerSecond, 1f, slider.value);
		}
		else
		{
			slider.value = 0f;
		}
	}

	private float GetSliderDelta(float rate, float newValue, float oldValue)
	{
		if (rate == -1f)
		{
			return Mathf.Abs(newValue - oldValue);
		}
		float value = Time.deltaTime * rate;
		value = Mathf.Clamp(value, 0f, Mathf.Abs(newValue - oldValue));
		return (newValue - oldValue > 0f) ? value : (0f - value);
	}
}
