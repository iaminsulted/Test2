using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "X_HighlightCraftBar", menuName = "Tutorial/Step/Highlight Craft Bar", order = 1)]
public class HighlightCraftBarStep : TutorialStep
{
	public bool DontCloseHighlight;

	private GameObject targetGO;

	public override IEnumerator Start()
	{
		UITutorialHighlighter.ShowFadeOnly();
		while (targetGO == null)
		{
			targetGO = HighlightTargets.Get(HighlightTargetName.CraftBar);
			yield return new WaitForEndOfFrame();
		}
		base.IsStarted = true;
		UIMergeDetail.CraftingCompleted += OnCraftingCompleted;
		UITutorialHighlighter.ShowFadeAndHighlightTarget(targetGO);
	}

	private void OnCraftingCompleted()
	{
		UIMergeDetail.CraftingCompleted -= OnCraftingCompleted;
		if (!DontCloseHighlight)
		{
			UITutorialHighlighter.Close();
		}
		else
		{
			UITutorialHighlighter.HidePointer();
		}
		Complete();
	}
}
