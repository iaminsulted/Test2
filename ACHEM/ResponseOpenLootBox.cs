using System.Collections.Generic;

public class ResponseOpenLootBox : Response
{
	public List<LootBoxRewardItem> Items;

	public int MC;

	public int MCTotal;

	public ResponseOpenLootBox()
	{
		type = 10;
		cmd = 9;
	}

	public ResponseOpenLootBox(List<LootBoxRewardItem> items, int mc, int mctotal)
	{
		type = 10;
		cmd = 9;
		Items = items;
		MC = mc;
		MCTotal = mctotal;
	}
}
