using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Item Required")]
public class IAItemRequired : InteractionRequirement
{
	public int ItemID;

	public int Quantity;

	public ComparisonType Comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		int num = 0;
		foreach (InventoryItem item in Session.MyPlayerData.items)
		{
			if (item.ID == ItemID)
			{
				num += item.Qty;
			}
		}
		return Comparison switch
		{
			ComparisonType.Equal => num == Quantity, 
			ComparisonType.GreaterThan => num > Quantity, 
			ComparisonType.GreaterThanOrEqual => num >= Quantity, 
			ComparisonType.LessThan => num < Quantity, 
			ComparisonType.LessThanOrEqual => num <= Quantity, 
			_ => false, 
		};
	}

	public void OnEnable()
	{
		Session.MyPlayerData.ItemAdded += OnItemAdded;
		Session.MyPlayerData.ItemRemoved += OnItemAdded;
		Session.MyPlayerData.ItemUpdated += OnItemAdded;
	}

	private void OnItemAdded(InventoryItem iItem)
	{
		if (iItem.ID == ItemID)
		{
			OnRequirementUpdate();
		}
	}

	public void OnDisable()
	{
		Session.MyPlayerData.ItemAdded -= OnItemAdded;
		Session.MyPlayerData.ItemRemoved -= OnItemAdded;
		Session.MyPlayerData.ItemUpdated -= OnItemAdded;
	}
}
