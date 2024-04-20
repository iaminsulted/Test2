using UnityEngine;

public class RemoteMovementController : MovementController
{
	private bool syncRot;

	private bool syncPos;

	private Vector3 diffpos = Vector3.zero;

	private Vector3 currpos = Vector3.zero;

	private float currY;

	private float targetY;

	private float syncPosTime;

	private float syncRotTime;

	public void Update()
	{
		if (syncRot)
		{
			syncRotTime += Time.deltaTime * 5f;
			base.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(currY, targetY, syncRotTime), 0f);
			if (Mathf.Abs(targetY - base.transform.eulerAngles.y) < 1f)
			{
				syncRot = false;
			}
		}
		if (syncPos)
		{
			Vector3 vector = currpos;
			syncPosTime = Mathf.Clamp01(syncPosTime + Time.deltaTime * 4f);
			currpos = diffpos * syncPosTime;
			base.transform.position += currpos - vector;
			if ((diffpos - currpos).sqrMagnitude < 0.001f)
			{
				syncPos = false;
			}
		}
	}

	public void UpdateRemote(ResponseMovement r)
	{
		base.State = r.state;
		Vector3 vector = new Vector3(r.posX, r.posY, r.posZ);
		if ((base.transform.position - vector).sqrMagnitude > 16f)
		{
			base.transform.position = vector;
			return;
		}
		syncPos = true;
		syncRot = true;
		syncPosTime = 0f;
		currpos = Vector3.zero;
		diffpos = vector - base.transform.position;
		syncRotTime = 0f;
		currY = base.transform.eulerAngles.y;
		targetY = r.rotY;
		float num = targetY - currY;
		if (num > 180f)
		{
			targetY -= 360f;
		}
		else if (num < -180f)
		{
			targetY += 360f;
		}
		if (r.cmd == 4)
		{
			OnJump();
		}
	}

	public override void Stop()
	{
		base.Stop();
		syncPos = false;
		syncRot = false;
	}
}
