using UnityEngine;

public class UIDeveloper : MonoBehaviour
{
	public UIInput devBtnApop;

	public UIToggle alwaysShowDevBtn;

	private void OnEnable()
	{
		devBtnApop.value = SettingsManager.DevBtnApopID;
		alwaysShowDevBtn.value = SettingsManager.DevBtnAlwaysShow;
	}

	public void ResetToDefault()
	{
		devBtnApop.value = SettingsManager.DevBtnApopID.Default;
		alwaysShowDevBtn.value = SettingsManager.DevBtnAlwaysShow.Default;
	}

	public void OnDevBtnApopChanged()
	{
		if (int.TryParse(devBtnApop.value, out var _))
		{
			SettingsManager.DevBtnApopID.Set(devBtnApop.value);
		}
	}

	public void OnAlwaysShowDevBtnChanged()
	{
		SettingsManager.DevBtnAlwaysShow.Set(alwaysShowDevBtn.value);
	}
}
