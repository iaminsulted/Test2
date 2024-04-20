public class TouchMovementController : ClientMovementController
{
	private UIJoystick jsLeft;

	public void Awake()
	{
		jsLeft = UIGame.Instance.joystick;
		jsLeft.DoubleClicked += OnJSDoubleClick;
	}

	public void OnDestroy()
	{
		jsLeft.DoubleClicked -= OnJSDoubleClick;
	}

	private void OnJSDoubleClick()
	{
		base.IsAutoRunEnabled = !base.IsAutoRunEnabled;
	}

	public void Update()
	{
		GetJSState();
		ApplyAutoRun();
	}

	private void GetJSState()
	{
		base.State = MovementState.None;
		if (jsLeft.position.sqrMagnitude >= 0.5f)
		{
			if (jsLeft.position.y <= -0.5f)
			{
				base.State |= MovementState.Backward;
			}
			else if (jsLeft.position.y >= 0.5f)
			{
				base.State |= MovementState.Forward;
			}
			if (jsLeft.position.x >= 0.5f)
			{
				base.State |= MovementState.RightStrafe;
			}
			if (jsLeft.position.x <= -0.5f)
			{
				base.State |= MovementState.LeftStrafe;
			}
		}
	}
}
