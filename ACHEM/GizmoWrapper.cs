using UnityEngine;

public class GizmoWrapper : MonoBehaviour
{
	public GameObject gizmoGO;

	public Gizmo gizmo;

	public void Init(GameObject gizmoGO, Gizmo gizmo)
	{
		this.gizmoGO = gizmoGO;
		this.gizmo = gizmo;
	}

	public void OnDestroy()
	{
		Object.Destroy(gizmoGO);
	}
}
