using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "X_HighlightClassItemButton", menuName = "Tutorial/Step/Highlight Class Item Button", order = 1)]
public class HighlightClassItemButtonStep : TutorialStep
{
	private static int numClassesToCheck = 5;

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
			GameObject gameObject = null;
			int num = -1;
			List<GameObject> all = HighlightTargets.GetAll(HighlightTargetName.ItemButton);
			int num2 = 0;
			foreach (GameObject item in all)
			{
				UIInventoryClass component = item.GetComponent<UIInventoryClass>();
				if (!(component == null))
				{
					bool equipped = component.combatClass.Equipped;
					bool flag = Session.MyPlayerData.OwnsClass(component.combatClass.ID);
					bool flag2 = Session.MyPlayerData.IsClassAvailable(component.combatClass);
					int num3 = 0;
					if (!equipped && !flag && flag2 && component.combatClass.ClassTokenCost == 0)
					{
						num3 = 2;
					}
					else if (!equipped && flag)
					{
						num3 = 1;
					}
					if (num3 > num)
					{
						gameObject = item;
						num = num3;
					}
					num2++;
					if (num2 == numClassesToCheck)
					{
						break;
					}
				}
			}
			buttonGO = gameObject;
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
