using UnityEngine;

[RequireComponent(typeof(UISlider))]
public class TweenSlider : UITweener
{
	[Range(0f, 1f)]
	public float from;

	[Range(0f, 1f)]
	public float to;

	private UISlider _slider;

	public UISlider slider
	{
		get
		{
			return _slider;
		}
		private set
		{
			_slider = value;
		}
	}

	public float Value
	{
		get
		{
			return _slider.value;
		}
		set
		{
			_slider.value = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (_slider == null)
		{
			_slider = GetComponent<UISlider>();
		}
		_slider.value = Mathf.Lerp(from, to, factor);
	}

	public static TweenSlider Begin(UISlider slider, float duration, float value)
	{
		TweenSlider tweenSlider = UITweener.Begin<TweenSlider>(slider.gameObject, duration);
		if (tweenSlider != null)
		{
			tweenSlider.from = tweenSlider.Value;
			tweenSlider.to = value;
			tweenSlider.slider = slider;
			if (duration <= 0f)
			{
				tweenSlider.Sample(1f, isFinished: true);
				tweenSlider.enabled = false;
			}
		}
		return tweenSlider;
	}

	public static TweenSlider Begin(UISlider slider, float value, float speed, float maxDuration)
	{
		TweenSlider tweenSlider = UITweener.Begin<TweenSlider>(slider.gameObject, 1f);
		if (tweenSlider != null)
		{
			tweenSlider.from = tweenSlider.Value;
			tweenSlider.to = value;
			tweenSlider.slider = slider;
			tweenSlider.duration = Mathf.Max(0.1f, Mathf.Min(Mathf.Abs(tweenSlider.to = tweenSlider.from) / speed, maxDuration));
			if (speed < 0f)
			{
				tweenSlider.Sample(1f, isFinished: true);
				tweenSlider.enabled = false;
			}
		}
		return tweenSlider;
	}

	private void Awake()
	{
		slider = GetComponent<UISlider>();
	}

	private void OnDisable()
	{
		Sample(1f, isFinished: true);
		base.enabled = false;
	}
}
