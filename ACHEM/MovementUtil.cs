public static class MovementUtil
{
	public static bool IsMoving(this MovementState state)
	{
		if (!state.IsForward() && !state.IsBackward() && !state.IsLeftStrafe())
		{
			return state.IsRightStrafe();
		}
		return true;
	}

	public static bool IsStrafeOnly(this MovementState state)
	{
		if ((state.IsRightStrafe() || state.IsLeftStrafe()) && !state.IsForward() && !state.IsBackward())
		{
			return !state.IsRotating();
		}
		return false;
	}

	public static bool IsRotating(this MovementState state)
	{
		if (!state.IsLeftRotate())
		{
			return state.IsRightRotate();
		}
		return true;
	}

	public static bool IsForward(this MovementState state)
	{
		if ((state & MovementState.Forward) == MovementState.Forward)
		{
			return (state & MovementState.Backward) != MovementState.Backward;
		}
		return false;
	}

	public static bool IsBackward(this MovementState state)
	{
		if ((state & MovementState.Backward) == MovementState.Backward)
		{
			return (state & MovementState.Forward) != MovementState.Forward;
		}
		return false;
	}

	public static bool IsLeftRotate(this MovementState state)
	{
		if ((state & MovementState.LeftRotate) == MovementState.LeftRotate)
		{
			return (state & MovementState.RightRotate) != MovementState.RightRotate;
		}
		return false;
	}

	public static bool IsLeftStrafe(this MovementState state)
	{
		if ((state & MovementState.LeftStrafe) == MovementState.LeftStrafe)
		{
			return (state & MovementState.RightStrafe) != MovementState.RightStrafe;
		}
		return false;
	}

	public static bool IsRightRotate(this MovementState state)
	{
		if ((state & MovementState.RightRotate) == MovementState.RightRotate)
		{
			return (state & MovementState.LeftRotate) != MovementState.LeftRotate;
		}
		return false;
	}

	public static bool IsRightStrafe(this MovementState state)
	{
		if ((state & MovementState.RightStrafe) == MovementState.RightStrafe)
		{
			return (state & MovementState.LeftStrafe) != MovementState.LeftStrafe;
		}
		return false;
	}
}
