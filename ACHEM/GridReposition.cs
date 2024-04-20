using UnityEngine;

public class GridReposition : MonoBehaviour
{
	public UIGrid grid;

	private void OnEnable()
	{
		grid.Reposition();
	}

	private void OnDisable()
	{
		grid.Reposition();
	}
}
