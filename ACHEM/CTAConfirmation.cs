using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Confirmation")]
public class CTAConfirmation : ClientTriggerAction
{
	public string Title;

	public string Message;

	public string YesLabel = "Yes";

	public string NoLabel = "No";

	public bool Closable;

	public bool EnableCollider;

	public ClientTriggerAction YesAction;

	public ClientTriggerAction NoAction;

	protected override void OnExecute()
	{
		Confirmation.Show(Title, Message, YesLabel, NoLabel, Callback, Closable, EnableCollider);
	}

	private void Callback(bool yes)
	{
		if (yes)
		{
			if (YesAction != null)
			{
				YesAction.Execute();
			}
		}
		else if (NoAction != null)
		{
			NoAction.Execute();
		}
	}
}
