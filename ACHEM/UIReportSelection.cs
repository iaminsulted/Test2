using UnityEngine;

public class UIReportSelection : MonoBehaviour
{
	public UILabel label;

	public UICharReport uiParent;

	public bool bitSelect;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetColor(Color c)
	{
		UIButton component = base.gameObject.GetComponent<UIButton>();
		if (component != null)
		{
			component.defaultColor = c;
		}
	}

	public void ToggleSelect()
	{
		bitSelect = !bitSelect;
	}

	public void OnClick()
	{
		uiParent.OnSelectedItem(this);
	}
}
