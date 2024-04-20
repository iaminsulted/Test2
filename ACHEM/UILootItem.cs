public class UILootItem : UIItem
{
	public UISprite CD;

	private ComLoot lootBag;

	public void InitLoot(ComLoot lootBag)
	{
		this.lootBag = lootBag;
	}

	public void Update()
	{
		if (lootBag != null)
		{
			float timeRemaining = lootBag.TimeRemaining;
			CD.fillAmount = 1f - timeRemaining / lootBag.Duration;
		}
	}
}
