using System;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
	private MovementState previousState;

	private MovementState state;

	public MovementState State
	{
		get
		{
			return state;
		}
		set
		{
			previousState = state;
			state = value;
			if (previousState == MovementState.None && state != 0)
			{
				this.Moved?.Invoke();
			}
		}
	}

	public bool IsEnabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			if (base.enabled != value)
			{
				base.enabled = value;
				if (!base.enabled)
				{
					Stop();
				}
			}
		}
	}

	public event Action Jumped;

	public event Action Moved;

	public void Jump()
	{
		OnJump();
	}

	protected void OnJump()
	{
		if (base.enabled && this.Jumped != null)
		{
			this.Jumped();
		}
	}

	public virtual void LookAt2D(Transform target)
	{
		if (!(target == null))
		{
			float num = Util.SignedAngleBetween(Vector3.forward, target.position - base.transform.position, Vector3.up);
			if (Mathf.Abs(base.transform.rotation.eulerAngles.y - num) > 1f)
			{
				base.transform.rotation = Quaternion.Euler(0f, num, 0f);
			}
		}
	}

	public virtual void Stop()
	{
		State = MovementState.None;
	}

	public virtual bool IsMoving()
	{
		return State.IsMoving();
	}
}
