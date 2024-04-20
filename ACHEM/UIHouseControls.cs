using System;
using UnityEngine;

public class UIHouseControls : MonoBehaviour
{
	private static UIHouseControls instance;

	public UIWidget ScaledWidget;

	public UIButton BtnInv;

	public UIButton BtnEdit;

	public UIButton BtnFinishEdit;

	public UIButton BtnSave;

	public UIButton BtnSettings;

	public UIButton BtnAdmin;

	public UIButton BtnExit;

	public UIGrid Grid;

	public bool isGuest;

	private void Start()
	{
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnInv.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnInventoryClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnEdit.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnEditClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnFinishEdit.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnFinishEditClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(BtnSave.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnSaveClicked));
		UIEventListener uIEventListener5 = UIEventListener.Get(BtnExit.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnExitClicked));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnInv.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnInventoryClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnEdit.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnEditClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnFinishEdit.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnFinishEditClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(BtnSave.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnSaveClicked));
		UIEventListener uIEventListener5 = UIEventListener.Get(BtnExit.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnExitClicked));
	}

	public void ShowOwnerControls()
	{
		BtnInv.transform.parent.gameObject.SetActive(value: true);
		BtnEdit.transform.parent.gameObject.SetActive(value: true);
		BtnSave.transform.parent.gameObject.SetActive(value: true);
		BtnExit.transform.parent.gameObject.SetActive(value: true);
		Grid.Reposition();
	}

	public void ShowGuestControls()
	{
		BtnExit.transform.parent.gameObject.SetActive(value: true);
		BtnInv.transform.parent.gameObject.SetActive(value: false);
		BtnEdit.transform.parent.gameObject.SetActive(value: false);
		BtnFinishEdit.transform.parent.gameObject.SetActive(value: false);
		BtnSave.transform.parent.gameObject.SetActive(value: false);
		isGuest = true;
		Grid.Reposition();
	}

	public void HideControls()
	{
		BtnExit.transform.parent.gameObject.SetActive(value: false);
		BtnInv.transform.parent.gameObject.SetActive(value: false);
		BtnEdit.transform.parent.gameObject.SetActive(value: false);
		BtnSave.transform.parent.gameObject.SetActive(value: false);
		BtnFinishEdit.transform.parent.gameObject.SetActive(value: false);
		Grid.Reposition();
	}

	public void OnMenuResize(float sizeUnused)
	{
		if (ScaledWidget != null)
		{
			float num = SettingsManager.MenuSize;
			ScaledWidget.transform.localScale = new Vector3(num, num, num);
		}
	}

	public void OnEditModeChanged(bool isEditing)
	{
		if (!isGuest)
		{
			if (isEditing)
			{
				BtnFinishEdit.transform.parent.gameObject.SetActive(value: true);
				BtnEdit.transform.parent.gameObject.SetActive(value: false);
			}
			else
			{
				BtnEdit.transform.parent.gameObject.SetActive(value: true);
				BtnFinishEdit.transform.parent.gameObject.SetActive(value: false);
			}
			Grid.Reposition();
		}
	}

	public void OnInventoryClicked(GameObject obj)
	{
		UIHouseItemList.Show();
	}

	public void OnEditClicked(GameObject obj)
	{
		HousingManager.houseInstance.EnterEditMode();
	}

	public void OnFinishEditClicked(GameObject obj)
	{
		HousingManager.houseInstance.ExitEditMode();
	}

	public void OnSaveClicked(GameObject obj)
	{
		HousingManager.houseInstance.ExitEditMode();
		HousingManager.houseInstance.RequestSave();
	}

	public void OnSettingsClicked(GameObject obj)
	{
	}

	public void OnAdminClicked(GameObject obj)
	{
	}

	public void OnExitClicked(GameObject obj)
	{
		HousingManager.houseInstance.ExitEditMode();
		HousingManager.houseInstance.RequestExit();
	}

	private void Update()
	{
	}
}
