using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MovementController
{
	private bool isPathing;

	private List<Vector3> path;

	private float speed;

	private List<float> pathSegmentDistance = new List<float>();

	private float distanceTraveled;

	private float tsMove;

	private Vector3 PathPosition;

	private ResponseMovePath.PathType pathType;

	private float finalPathRotation = float.MaxValue;

	private float finalTurnTime;

	public Vector3 Movement
	{
		get
		{
			if (!isPathing)
			{
				return Vector3.zero;
			}
			return PathPosition - base.transform.position;
		}
	}

	public float Speed
	{
		get
		{
			if (!(Movement.magnitude > 0f))
			{
				return 0f;
			}
			return speed;
		}
	}

	private void Start()
	{
		PathPosition = base.transform.position;
	}

	private void Update()
	{
		UpdatePathPosition();
	}

	public override bool IsMoving()
	{
		return isPathing;
	}

	public override void Stop()
	{
		isPathing = false;
		path = null;
	}

	private float PathDistance(List<Vector3> path)
	{
		float num = 0f;
		for (int num2 = path.Count - 1; num2 >= 1; num2--)
		{
			num += (path[num2] - path[num2 - 1]).magnitude;
		}
		return num;
	}

	public void SyncPosition(Vector3 position, float finalRotation)
	{
		if ((position.xz() - base.transform.position.xz()).magnitude < 0.1f)
		{
			position = base.transform.position;
		}
		Move(new List<Vector3>
		{
			base.transform.position,
			position
		}, ResponseMovePath.PathType.Sync, 10f, GameTime.realtimeSinceServerStartup, useRotation: true, finalRotation);
	}

	public void Move(List<Vector3> path, ResponseMovePath.PathType pathType, float speed, float startTS, bool useRotation, float finalRotation)
	{
		if (useRotation)
		{
			finalPathRotation = finalRotation;
		}
		PathPosition = base.transform.position;
		if (isPathing)
		{
			Vector3 vector = new Vector3(PathPosition.x, PathPosition.y, PathPosition.z);
			float num = PathDistance(path) / speed;
			startTS += ((path[0] - path[1]).magnitude - (vector - path[1]).magnitude) / speed;
			path[0] = vector;
			speed = PathDistance(path) / num;
		}
		isPathing = true;
		this.path = path;
		this.speed = speed;
		distanceTraveled = 0f;
		tsMove = startTS;
		this.pathType = pathType;
		pathSegmentDistance.Clear();
		for (int i = 0; i < path.Count - 1; i++)
		{
			pathSegmentDistance.Add((path[i] - path[i + 1]).magnitude);
		}
	}

	public void SetMoveSpeed(float speed)
	{
		UpdatePathPosition();
		this.speed = speed;
	}

	private void UpdatePathPosition()
	{
		if (!isPathing)
		{
			return;
		}
		distanceTraveled = speed * (GameTime.realtimeSinceServerStartup - tsMove);
		float num = distanceTraveled;
		for (int i = 0; i < pathSegmentDistance.Count; i++)
		{
			if (num < pathSegmentDistance[i])
			{
				float t = num / pathSegmentDistance[i];
				PathPosition = Vector3.Lerp(path[i], path[i + 1], t);
				if (pathType == ResponseMovePath.PathType.Move)
				{
					float y = Util.SignedAngleBetween(Vector3.forward, path[i + 1] - path[i], Vector3.up);
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(0f, y, 0f), 0.2f);
				}
				if ((PathPosition.xz() - base.transform.position.xz()).magnitude > 2f || (PathPosition - base.transform.position).magnitude > 10f)
				{
					base.transform.position = PathPosition;
				}
				return;
			}
			num -= pathSegmentDistance[i];
		}
		if (finalPathRotation != float.MaxValue)
		{
			float num2 = Mathf.Clamp01(finalTurnTime / 0.2f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(0f, finalPathRotation, 0f), num2);
			finalTurnTime += Time.deltaTime;
			if (num2 < 1f)
			{
				return;
			}
		}
		else
		{
			float y2 = Util.SignedAngleBetween(Vector3.forward, path[path.Count - 1] - path[path.Count - 2], Vector3.up);
			base.transform.rotation = Quaternion.Euler(0f, y2, 0f);
		}
		PathPosition = path[path.Count - 1];
		finalPathRotation = float.MaxValue;
		finalTurnTime = 0f;
		isPathing = false;
		path = null;
	}
}
