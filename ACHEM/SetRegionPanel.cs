using UnityEngine;

public class SetRegionPanel : MonoBehaviour
{
	private UIDragObject myPanel;

	public string myName;

	private void Start()
	{
		myPanel = GetComponent<UIDragObject>();
		UIPanel component = base.transform.parent.GetComponent<UIPanel>();
		myPanel.panelRegion = component;
		base.transform.name = myName;
	}

	private void Update()
	{
	}
}
