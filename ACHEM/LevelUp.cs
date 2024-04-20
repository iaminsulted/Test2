using UnityEngine;

public class LevelUp : MonoBehaviour
{
	public GameObject dragonOpenMouth;

	public static void Show()
	{
		Object.Instantiate(Resources.Load<GameObject>("UIElements/LevelUp"), UIManager.Instance.transform);
	}

	public void Remove()
	{
		Game.Instance.uigame.ShowNewStatsButton();
		Object.Destroy(base.transform.parent.gameObject);
	}

	private void StartTween()
	{
		TweenPosition component = dragonOpenMouth.GetComponent<TweenPosition>();
		TweenScale component2 = dragonOpenMouth.GetComponent<TweenScale>();
		TweenAlpha component3 = dragonOpenMouth.GetComponent<TweenAlpha>();
		component.to = dragonOpenMouth.transform.parent.InverseTransformPoint(Game.Instance.uigame.targetPositionForLevelUpDragon.position);
		component.from = dragonOpenMouth.transform.localPosition;
		component.enabled = true;
		base.transform.GetComponent<Animator>().enabled = false;
		component.PlayForward();
		component2.PlayForward();
		component3.PlayForward();
	}

	private void ActivateParticles()
	{
	}

	public void ShowUINButtonForNewStats()
	{
		Game.Instance.uigame.ShowNewStatsButton();
	}
}
