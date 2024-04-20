public class UIStackingWindow : UIWindow
{
	protected override void Init()
	{
		UIWindow.HideCurrentWindow();
		base.Init();
	}

	public virtual void Back()
	{
		base.Close();
		UIWindow.ShowLastWindow();
	}

	public override void Close()
	{
		UIWindow.ClearWindows();
	}
}
