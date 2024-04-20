using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "X_HighlightButton", menuName = "Tutorial/Step/Highlight Button", order = 1)]
public class HighlightButtonStep : TutorialStep
{
	public HighlightTargetName ButtonName;

	public HighlightPointerLocation PointerLocation;

	public Vector2 PointerOffset;

	public bool DontCloseHighlight;

	private GameObject buttonGO;

	private UIButton uibutton;

	private UIItem uiItem;

	private UIDragScrollView dragScrollView;

	public override IEnumerator Start()
	{
		UITutorialHighlighter.ShowFadeOnly();
		while (buttonGO == null)
		{
			buttonGO = HighlightTargets.Get(ButtonName);
			yield return new WaitForEndOfFrame();
		}
		base.IsStarted = true;
		uibutton = buttonGO.GetComponentInChildren<UIButton>(includeInactive: true);
		if (uibutton != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(uibutton.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
		}
		else
		{
			uiItem = buttonGO.GetComponentInChildren<UIItem>(includeInactive: true);
			uiItem.Clicked += OnClick;
		}
		dragScrollView = buttonGO.GetComponent<UIDragScrollView>();
		if (dragScrollView != null)
		{
			dragScrollView.enabled = false;
		}
		UITutorialHighlighter.ShowFadeAndHighlightTarget(buttonGO, PointerLocation, PointerOffset);
	}

	private void OnClick(GameObject gameObject)
	{
		UIEventListener uIEventListener = UIEventListener.Get(uibutton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
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
