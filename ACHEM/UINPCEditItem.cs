using System.Collections.Generic;
using UnityEngine;

public class UINPCEditItem : MonoBehaviour
{
	public new UILabel name;

	public UIInput textField;

	public UIInput largeTextField;

	public UIToggle checkField;

	public UIPopupList dropField;

	public GameObject ApopsBtn;

	public GameObject ApopBtn;

	public int type;

	public void ConfigurePropertyInputUI(string name, int type, object value, List<string> dropOptions)
	{
		this.name.text = name;
		this.type = type;
		textField.gameObject.SetActive(value: false);
		if (largeTextField != null)
		{
			largeTextField.gameObject.SetActive(value: false);
		}
		checkField.gameObject.SetActive(value: false);
		dropField.gameObject.SetActive(value: false);
		if (ApopsBtn != null)
		{
			ApopsBtn.SetActive(name == "Apop ID");
		}
		if (ApopBtn != null)
		{
			ApopBtn.SetActive(value: false);
		}
		switch (type)
		{
		case 0:
			textField.gameObject.SetActive(value: true);
			textField.value = (string)value;
			if (ApopBtn != null)
			{
				ApopBtn.SetActive(name == "Apop ID" && !string.IsNullOrEmpty((string)value));
			}
			break;
		case 1:
			checkField.gameObject.SetActive(value: true);
			checkField.value = (bool)value;
			break;
		case 2:
			dropField.gameObject.SetActive(value: true);
			dropField.items = new List<string>(dropOptions);
			dropField.value = (((int)value >= dropOptions.Count || (int)value < 0 || value == null) ? dropOptions[0] : dropOptions[(int)value]);
			break;
		case 3:
			largeTextField.gameObject.SetActive(value: true);
			largeTextField.value = (string)value;
			break;
		}
	}

	public string GetTextValue()
	{
		if (type == 0)
		{
			if (!string.IsNullOrEmpty(textField.value))
			{
				return textField.value;
			}
			return "";
		}
		if (!string.IsNullOrEmpty(largeTextField.value))
		{
			return largeTextField.value;
		}
		return "";
	}

	public bool GetCheckValue()
	{
		return checkField.value;
	}

	public string GetDropValue()
	{
		return dropField.value;
	}

	public int GetDropIndex()
	{
		return dropField.items.IndexOf(dropField.value);
	}

	public void OpenApop()
	{
		if (!string.IsNullOrEmpty(textField.value))
		{
			int result = (int.TryParse(textField.value, out result) ? result : (-1));
			if (result > 0)
			{
				AEC.getInstance().sendRequest(new RequestOpenApopAdmin(new List<int> { result }));
			}
		}
	}

	public void OpenApops()
	{
		AEC.getInstance().sendRequest(new RequestOpenApops());
	}

	private void OnClick()
	{
	}

	public void OnTooltip(bool show)
	{
	}
}
