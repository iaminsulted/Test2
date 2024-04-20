using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Message Box")]
public class CTAMessageBox : ClientTriggerAction
{
	public string Title;

	public string Message;

	public string OKLabel = "OK";

	public ClientTriggerAction Action;

	protected override void OnExecute()
	{
		MessageBox.Show(Title, Message, OKLabel, Callback);
	}

	private void Callback()
	{
		if (Action != null)
		{
			Action.Execute();
		}
	}
}
