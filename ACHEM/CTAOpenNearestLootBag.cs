using UnityEngine;

public class CTAOpenNearestLootBag : ClientTriggerAction
{
	protected override void OnExecute()
	{
		InvokeRepeating("CheckForLoot", 0f, 0.5f);
	}

	private void CheckForLoot()
	{
		ComLoot comLoot = null;
		float num = float.MaxValue;
		foreach (ComLoot value in LootBags.Bags.Values)
		{
			float num2 = Vector3.Distance(base.transform.position, value.Position);
			if (num2 < num)
			{
				comLoot = value;
				num = num2;
			}
		}
		if (comLoot != null)
		{
			CancelInvoke("CheckForLoot");
			UILoot.Load(comLoot);
		}
	}
}
