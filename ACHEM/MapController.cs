using UnityEngine;

public class MapController : UIWindow
{
	public GameObject currentRegion;

	public GameObject myWordMapBg;

	public GameObject myRegion;

	public int mapZoom;

	public string myChoice;

	public int myCellID;

	public int mySpawnID;

	public GameObject RegionPopUp;

	public UILabel RegionPopUpTitle;

	public UILabel RegionPopUpLevel;

	public UILabel RegionPopUpDescription;

	public GameObject MyTeleportToRegion;

	public GameObject[] MapPrefabs = new GameObject[10];

	private UIMainMenu mainMenu;

	private void Awake()
	{
		UIWindow.ClearWindows();
		Init();
	}

	private void Start()
	{
		Debug.Log("Log ____________ " + Game.Instance.AreaData.name);
		Debug.Log("Log ____________ " + MapPrefabs[1].name);
		string text = Game.Instance.AreaData.name;
		for (int i = 0; i < MapPrefabs.Length; i++)
		{
			if (MapPrefabs[i].name == text)
			{
				LoadRegion(MapPrefabs[i]);
			}
		}
	}

	public void SetTeleportInfo(string s, int c, int id)
	{
		myChoice = s;
		myCellID = c;
		mySpawnID = id;
	}

	public void GoToNode(string nodeName, string nodeDescription, string choice, int CellID, int SpawnID)
	{
		myChoice = choice;
		myCellID = CellID;
		mySpawnID = SpawnID;
		RegionPopUp.SetActive(value: true);
		RegionPopUpTitle.text = nodeName;
		RegionPopUpDescription.text = nodeDescription;
	}

	public void LoadRegion(GameObject TeleportToRegion)
	{
		MyTeleportToRegion = TeleportToRegion;
		if ((bool)currentRegion)
		{
			Object.Destroy(currentRegion);
		}
		myRegion.transform.SetParent(base.transform);
		myRegion.transform.localScale = Vector3.one;
		myRegion.SetActive(value: true);
		currentRegion = myRegion;
		myWordMapBg.SetActive(value: false);
	}

	public void RemoveRegionPopUp()
	{
		RegionPopUp.SetActive(value: false);
	}

	public void TeleportPlayer()
	{
		RemoveRegionPopUp();
	}
}
