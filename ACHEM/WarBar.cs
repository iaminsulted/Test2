using UnityEngine;

public class WarBar : MonoBehaviour
{
	public UISprite Icon;

	public UILabel Label;

	public UISlider Slider;

	public UISprite Fill;

	public UISprite FrameLeft;

	public UISprite FrameRight;

	public void Init(War war)
	{
		Label.text = war.Name;
		Slider.value = war.ProgressPercent;
		Fill.color = war.ColorFill.ToColor32();
		UISprite frameLeft = FrameLeft;
		Color color2 = (FrameRight.color = war.ColorFrame.ToColor32());
		frameLeft.color = color2;
		Label.color = war.ColorText.ToColor32();
		Label.text = war.Name;
		Label.text += ((war.ProgressPercent >= 1f) ? " (Winner)" : (" (" + (int)((float)war.Progress * 100f / (float)war.Target) + "%)"));
	}
}
