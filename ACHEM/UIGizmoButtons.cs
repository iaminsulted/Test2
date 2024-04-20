using System;
using UnityEngine;

public class UIGizmoButtons : UIWindow
{
	public UIWidget Bounds;

	public UIMultiButton posRotScale;

	public UIMultiButton localGlobal;

	public UIButton btnSettings;

	public UIButton btnRemove;

	public UIButton btnReset;

	public UIButton btnDuplicate;

	public UIGrid grid;

	private Gizmo gizmo;

	public void Show(Gizmo gizmo)
	{
		Clear();
		this.gizmo = gizmo;
		Gizmo obj = this.gizmo;
		obj.onUpdate = (Action)Delegate.Combine(obj.onUpdate, new Action(OnUpdate));
	}

	private void Clear()
	{
		if (gizmo != null)
		{
			Gizmo obj = gizmo;
			obj.onUpdate = (Action)Delegate.Remove(obj.onUpdate, new Action(OnUpdate));
			UnityEngine.Object.Destroy(gizmo.gameObject);
			gizmo = null;
		}
	}

	private void OnDestroy()
	{
		Clear();
	}

	private void Start()
	{
		UpdatePosition();
		posRotScale.Init((int)Gizmo.currentType);
		localGlobal.Init((int)Gizmo.currentMode);
		updateButtonState();
	}

	private void OnEnable()
	{
		UIMultiButton uIMultiButton = posRotScale;
		uIMultiButton.onClick = (Action)Delegate.Combine(uIMultiButton.onClick, new Action(OnPosRotScaleClick));
		UIMultiButton uIMultiButton2 = localGlobal;
		uIMultiButton2.onClick = (Action)Delegate.Combine(uIMultiButton2.onClick, new Action(OnLocalGlobalClick));
		UIEventListener uIEventListener = UIEventListener.Get(btnSettings.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSettingsClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnRemove.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnRemoveClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnReset.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnResetClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnDuplicate.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnDuplicateClicked));
	}

	private void OnDisable()
	{
		UIMultiButton uIMultiButton = posRotScale;
		uIMultiButton.onClick = (Action)Delegate.Remove(uIMultiButton.onClick, new Action(OnPosRotScaleClick));
		UIMultiButton uIMultiButton2 = localGlobal;
		uIMultiButton2.onClick = (Action)Delegate.Remove(uIMultiButton2.onClick, new Action(OnLocalGlobalClick));
		UIEventListener uIEventListener = UIEventListener.Get(btnSettings.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSettingsClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnRemove.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnRemoveClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnReset.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnResetClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnDuplicate.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnDuplicateClicked));
	}

	private void updateButtonState()
	{
		posRotScale.gameObject.SetActive(Gizmo.isArrowsShowing);
		localGlobal.gameObject.SetActive(Gizmo.isArrowsShowing);
		btnRemove.gameObject.SetActive(!Gizmo.isArrowsShowing);
		btnReset.gameObject.SetActive(!Gizmo.isArrowsShowing);
		btnDuplicate.gameObject.SetActive(!Gizmo.isArrowsShowing);
		grid.Reposition();
	}

	private void OnSettingsClicked(GameObject go)
	{
		gizmo.ShowArrows(isArrowsShowing: true);
		updateButtonState();
	}

	private void OnRemoveClicked(GameObject go)
	{
		HousingManager.houseInstance.DeleteSelectedItem();
	}

	private void OnDuplicateClicked(GameObject go)
	{
		HousingManager.houseInstance.DuplicateSelected();
	}

	private void OnResetClicked(GameObject go)
	{
		gizmo.Revert();
	}

	private void OnPosRotScaleClick()
	{
		gizmo.SetGizmoType((Gizmo.GizmoType)posRotScale.CurrrentOption);
	}

	private void OnLocalGlobalClick()
	{
		gizmo.SetGizmoMode((Gizmo.GizmoMode)localGlobal.CurrrentOption);
	}

	private void UpdatePosition()
	{
		Camera cam = Game.Instance.cam;
		Camera uiCamera = Game.Instance.uiCamera;
		Vector2 vector = cam.WorldToScreenPoint(gizmo.center);
		Vector2 vector2 = new Vector2(uiCamera.pixelWidth / 12, uiCamera.pixelHeight / 7);
		vector += vector2;
		base.transform.position = uiCamera.ScreenToWorldPoint(vector);
		Vector3[] worldCorners = uiCamera.GetWorldCorners();
		if (Bounds.worldCorners[1].y > worldCorners[1].y)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + (worldCorners[1].y - Bounds.worldCorners[1].y), base.transform.position.z);
		}
	}

	private void Update()
	{
		UpdatePosition();
	}

	protected void OnUpdate()
	{
	}
}
