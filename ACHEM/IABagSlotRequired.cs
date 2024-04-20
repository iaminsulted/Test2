using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA BagSlot Required")]
public class IABagSlotRequired : InteractionRequirement
{
	public int Quantity;

	public ComparisonType Comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		int num = myPlayerData.BagSlots - myPlayerData.CountItemsInBank();
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

	public void OnDisable()
	{
		Session.MyPlayerData.ItemAdded -= OnItemAdded;
		Session.MyPlayerData.ItemRemoved -= OnItemAdded;
		Session.MyPlayerData.ItemUpdated -= OnItemAdded;
	}

	private void OnItemAdded(InventoryItem iItem)
	{
		OnRequirementUpdate();
	}
}
