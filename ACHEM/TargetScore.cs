using UnityEngine;

public class TargetScore : MonoBehaviour
{
	public int maxScore;

	public UIGrid grid;

	public UISprite leftFill;

	public UISprite rightFill;

	public int leftTeamScore;

	public int rightTeamScore;

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		for (int num = grid.transform.childCount - 1; num >= 0; num--)
		{
			grid.transform.GetChild(num).gameObject.SetActive(value: false);
		}
		if (maxScore > 0)
		{
			if ((maxScore - 1) * 2 + 1 > grid.transform.childCount)
			{
				for (int i = 0; i < grid.transform.childCount; i++)
				{
					grid.transform.GetChild(i).gameObject.SetActive(value: true);
				}
				int num2 = (maxScore - 1) * 2 - grid.transform.childCount + 1;
				for (int j = 0; j < num2; j++)
				{
					Object.Instantiate(Resources.Load("UIElements/Divider") as GameObject, grid.transform);
				}
			}
			else
			{
				for (int k = 0; k < (maxScore - 1) * 2 + 1; k++)
				{
					grid.transform.GetChild(k).gameObject.SetActive(value: true);
				}
			}
		}
		grid.cellWidth = 80f / (float)maxScore;
		grid.Reposition();
	}

	private void Update()
	{
		leftFill.fillAmount = (float)leftTeamScore / (float)maxScore;
		rightFill.fillAmount = (float)leftTeamScore / (float)maxScore;
	}
}
