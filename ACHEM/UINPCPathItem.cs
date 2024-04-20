using UnityEngine;

public class UINPCPathItem : MonoBehaviour
{
	public UILabel title;

	public UILabel description;

	public int PathID;

	public Vector3 vector;

	public int RotationY;

	public void Load(int pathID, Vector3 vector, int RotationY)
	{
		PathID = pathID;
		this.vector = vector;
		this.RotationY = RotationY;
		title.text = vector.x + ", " + vector.y + ", " + vector.z;
		description.text = "Path ID: " + pathID + " - Rotation Y: " + RotationY;
	}

	private void OnClick()
	{
		UINPCEditor.Instance.OnPathClicked(this);
	}

	public void OnTooltip(bool show)
	{
	}
}
