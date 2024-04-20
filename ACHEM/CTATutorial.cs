using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Tutorial")]
public class CTATutorial : CTAAsync
{
	public Tutorial Tutorial;

	public float delay;

	private void FinalExecute()
	{
		if (!UITutorialPopup.Show(Tutorial, base.OnComplete))
		{
			TutorialSequenceManager.Show(Tutorial, base.OnComplete);
		}
	}

	protected override void OnExecute()
	{
		if (delay > 0f)
		{
			Invoke("FinalExecute", delay);
		}
		else
		{
			FinalExecute();
		}
	}
}
