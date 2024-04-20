using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "X_SkipToClaim", menuName = "Tutorial/Step/Skip To Claim", order = 1)]
public class SkipToClaimStep : TutorialStep
{
	public override IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		base.IsStarted = true;
		if (Session.MyPlayerData.merges.Any((Merge x) => x.ItemID == 3859))
		{
			TutorialSequenceManager.SkipToStep(4);
		}
		else
		{
			Complete();
		}
	}
}
