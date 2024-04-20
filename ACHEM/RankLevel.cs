using UnityEngine;

public class RankLevel : MonoBehaviour
{
	public UILabel RankNumber;

	private void Start()
	{
		RankNumber.text = Session.MyPlayerData.EquippedClassRank.ToString();
	}
}
