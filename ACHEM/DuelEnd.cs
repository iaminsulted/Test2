using UnityEngine;

public class DuelEnd : MonoBehaviour
{
	private bool won;

	public UILabel Text;

	public GameObject Win;

	public GameObject Lose;

	public static void Show(bool won)
	{
		Object.Instantiate(Resources.Load<GameObject>("UIElements/DuelEnd"), UIManager.Instance.transform).GetComponent<DuelEnd>().won = won;
	}

	private void Start()
	{
		Text.text = (won ? "Victory!" : "Defeat!");
		Win.SetActive(won);
		Lose.SetActive(!won);
	}

	public void Remove()
	{
		Object.Destroy(base.gameObject);
	}
}
