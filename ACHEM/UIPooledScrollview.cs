using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIPooledScrollview : MonoBehaviour
{
	public GameObject TemplateGO;

	public UIScrollBar ScrollBar;

	[HideInInspector]
	public UIScrollView scrollView;

	[HideInInspector]
	public Transform PoolRoot;

	private float ItemHeight;

	private float ItemPixelHeight;

	private float ScrollViewHeight;

	private int poolSize;

	private int itemOffset;

	private UIWidget emptyWidget;

	private UIPanel mainPanel;

	private Action<int> onGetNewSelection;

	private Action onUpdate;

	public void Init(Action<int> onGetNewSelection, int poolSize)
	{
		TemplateGO.SetActive(value: false);
		this.poolSize = poolSize;
		this.onGetNewSelection = onGetNewSelection;
		mainPanel = GetComponent<UIPanel>();
		GameObject gameObject = new GameObject("ScrollViewRoot");
		gameObject.layer = Layers.NGUI;
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		scrollView = gameObject.AddComponent<UIScrollView>();
		scrollView.verticalScrollBar = ScrollBar;
		scrollView.movement = UIScrollView.Movement.Vertical;
		scrollView.panel.depth = mainPanel.depth + 1;
		scrollView.scrollWheelFactor = 1f;
		scrollView.disableDragIfFits = true;
		GameObject gameObject2 = new GameObject("PoolObjectRoot");
		gameObject2.layer = Layers.NGUI;
		gameObject2.transform.SetParent(base.transform);
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		PoolRoot = gameObject2.transform;
		GameObject gameObject3 = new GameObject("Invisible Widget");
		gameObject3.layer = Layers.NGUI;
		gameObject3.transform.SetParent(gameObject.transform, worldPositionStays: false);
		gameObject3.transform.localScale = Vector3.one;
		emptyWidget = gameObject3.gameObject.AddComponent<UIWidget>();
		emptyWidget.pivot = UIWidget.Pivot.TopLeft;
		emptyWidget.autoResizeBoxCollider = true;
		emptyWidget.width = (int)mainPanel.width;
		emptyWidget.height = (int)mainPanel.height;
		scrollView.panel.SetRect(0f, 0f, mainPanel.width, mainPanel.height);
		emptyWidget.transform.position = scrollView.panel.worldCorners[1];
		scrollView.ResetPosition();
		ItemHeight = NGUIMath.CalculateAbsoluteWidgetBounds(TemplateGO.transform).size.y;
		ItemPixelHeight = NGUIMath.CalculateRelativeWidgetBounds(TemplateGO.transform).size.y;
		float num = ItemHeight / ItemPixelHeight;
		float height = mainPanel.height;
		ScrollViewHeight = height * num;
		UpdatePoolRootPosition();
	}

	public void ResetScrollview()
	{
		scrollView.ResetPosition();
	}

	public void StartRefreshOnUpdate()
	{
		GetNewSelectionRange();
		onUpdate = GetNewSelectionRange;
	}

	public void MoveObjectToIndexLocation(GameObject go, int index)
	{
		go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0f - (float)index * ItemPixelHeight + (float)itemOffset, 0f);
	}

	public void UpdateWidgetSize(int itemCount, int itemOffset = 0)
	{
		this.itemOffset = itemOffset;
		emptyWidget.height = (int)(ItemPixelHeight * (float)itemCount) - itemOffset;
		if (IsContentSmallerThanScrollView())
		{
			scrollView.ResetPosition();
			return;
		}
		Vector3 vector = emptyWidget.worldCorners[0] - scrollView.panel.worldCorners[0];
		if (vector.y > 0f)
		{
			scrollView.MoveAbsolute(Vector3.down * vector.y);
		}
	}

	public bool IsContentSmallerThanScrollView()
	{
		float num = ItemPixelHeight / ItemHeight;
		return (float)emptyWidget.height < ScrollViewHeight * num;
	}

	public int GetCenterIndex()
	{
		Vector3 vector = emptyWidget.worldCorners[1];
		Vector3 vector2 = mainPanel.worldCorners[1];
		float num = vector.y - vector2.y + ScrollViewHeight / 2f;
		float num2 = ItemHeight / ItemPixelHeight;
		return Mathf.FloorToInt((num + (float)itemOffset * num2) / ItemHeight);
	}

	private void GetNewSelectionRange()
	{
		Vector3 vector = emptyWidget.worldCorners[1];
		Vector3 vector2 = mainPanel.worldCorners[1];
		float num = vector.y - vector2.y + ScrollViewHeight / 2f;
		float num2 = ItemHeight / ItemPixelHeight;
		int num3 = Mathf.FloorToInt((num + (float)itemOffset * num2) / ItemHeight) - (poolSize / 2 - 1);
		if (num3 < 0)
		{
			num3 = 0;
		}
		onGetNewSelection?.Invoke(num3);
	}

	private void UpdatePoolRootPosition()
	{
		if (PoolRoot != null)
		{
			PoolRoot.transform.position = new Vector3(PoolRoot.transform.position.x, emptyWidget.worldCorners[1].y, 0f);
		}
	}

	private void Update()
	{
		UpdatePoolRootPosition();
		onUpdate?.Invoke();
	}
}
