using UnityEngine;

public class PositionGizmoHandle : AxisGizmoHandle
{
	public Vector3 GetNewTargetPosition(Transform target, Vector3 delta)
	{
		return target.position + delta;
	}
}
