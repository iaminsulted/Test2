using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "X_ShowMessageBox", menuName = "Tutorial/Step/Message Box", order = 1)]
public class ShowMessageBoxStep : TutorialStep
{
	public string Title;

	public string Message;

	public override IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		base.IsStarted = true;
		Game.Instance.DisableControls();
		MessageBox.Show(Title, Message, delegate
		{
			UIWindow.ClearWindows();
			Game.Instance.EnableControls();
			Complete();
		});
	}
}
