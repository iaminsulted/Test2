public class UICustomActionTravel : UICustomActionItem
{
	protected override void OpenSelectMenu()
	{
		if (!IsLocked())
		{
			UICustomActionSelection.Load(UICustomActionSelection.Mode.TravelForm, SlotNumber);
		}
	}
}
