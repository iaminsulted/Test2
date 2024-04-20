using System;
using UnityEngine;

public class UIScrollViewSpringToTop : MonoBehaviour
{
	private UIScrollView scrollView;

	private Vector3 startPos = Vector3.zero;

	public void Start()
	{
		scrollView = GetComponent<UIScrollView>();
		UIScrollView uIScrollView = scrollView;
		uIScrollView.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(uIScrollView.onDragFinished, new UIScrollView.OnDragNotification(dragFinished));
		UIScrollView uIScrollView2 = scrollView;
		uIScrollView2.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Combine(uIScrollView2.onDragStarted, new UIScrollView.OnDragNotification(dragStart));
		startPos = scrollView.gameObject.transform.localPosition;
	}

	private void dragFinished()
	{
		if (scrollView != null)
		{
			if (!scrollView.shouldMoveVertically && scrollView.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
			{
				SpringPanel.Begin(scrollView.GetComponent<UIPanel>().gameObject, startPos, 13f).strength = 8f;
			}
		}
		else
		{
			Debug.Log("grid or scroll view is null FUNC:dragFinished POS:PackageTypeClick.cs");
		}
	}

	private void dragStart()
	{
	}
}
