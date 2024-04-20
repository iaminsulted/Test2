using UnityEngine;

public class CharacterLevel : MonoBehaviour
{
	public UILabel LevelNumber;

	private void Start()
	{
		LevelNumber.text = Entities.Instance.me.Level.ToString();
	}
}
