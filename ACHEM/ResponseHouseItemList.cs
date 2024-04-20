using System.Collections.Generic;
using Assets.Scripts.NetworkClient.CommClasses;

public class ResponseHouseItemList : Response
{
	public int ItemID;

	public List<ComHouseItemListData> hItems;
}
