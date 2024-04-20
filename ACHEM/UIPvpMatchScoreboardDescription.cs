using UnityEngine;

public class UIPvpMatchScoreboardDescription : MonoBehaviour
{
	[SerializeField]
	private UILabel map;

	[SerializeField]
	private UILabel mode;

	public void Init(string map, string mode)
	{
		this.map.text = map;
		this.mode.text = mode;
	}
}
