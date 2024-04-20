using UnityEngine;

public class UIRegionListItem : UIItem
{
	public RegionData region;

	public GameObject icon_lock;

	public GameObject icon_quest;

	public MapRegionsBtn mapRegionsBtn;

	public MapNode mapNode;

	public void Load(RegionData rd)
	{
		region = rd;
		NameLabel.text = region.Name;
		icon_quest.SetActive(Session.MyPlayerData.HasQuestInRegion(region.ID));
	}
}
