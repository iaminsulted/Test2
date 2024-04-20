using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "X_HighlightItemByIDButton", menuName = "Tutorial/Step/Highlight Item By ID Button", order = 1)]
public class HighlightItemButtonByIDStep : TutorialStep
{
	public int ItemID;

	public HighlightPointerLocation PointerLocation;

	public Vector2 PointerOffset;

	public bool DontCloseHighlight;

	private GameObject buttonGO;

	private UIItem uiItem;

	private UIDragScrollView dragScrollView;

	public override IEnumerator Start()
	{
		UITutorialHighlighter.ShowFadeOnly();
		while (buttonGO == null)
		{
			buttonGO = HighlightTargets.Get(HighlightTargetName.ItemButton, (GameObject x) => x.GetComponent<UIItem>().Item.ID == ItemID);
			yield return new WaitForEndOfFrame();
		}
		base.IsStarted = true;
		uiItem = buttonGO.GetComponentInChildren<UIItem>(includeInactive: true);
		uiItem.Clicked += OnClick;
		dragScrollView = buttonGO.GetComponent<UIDragScrollView>();
		if (dragScrollView != null)
		{
			dragScrollView.enabled = false;
		}
		UITutorialHighlighter.ShowFadeAndHighlightTarget(buttonGO, PointerLocation, PointerOffset);
	}

	private void OnClick(UIItem item)
	{
		if (dragScrollView != null)
		{
			dragScrollView.enabled = true;
		}
		item.Clicked -= OnClick;
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
