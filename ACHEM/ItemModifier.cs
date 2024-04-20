public class ItemModifier
{
	public enum ModifierRarity
	{
		common = 1,
		uncommon,
		rare,
		epic,
		legendary,
		mythic
	}

	public int id;

	public string name;

	public float maxHealth;

	public float attack;

	public float armor;

	public float evasion;

	public float crit;

	public float haste;

	public ModifierRarity rarity;

	public static void OpenAugmentGraph(ItemModifier modifier)
	{
		AugmentGraph.Show(modifier);
	}

	public static string getModifierIcon(int modifierRarity)
	{
		return modifierRarity switch
		{
			2 => "Modifier_Rare_Icon_green", 
			3 => "Modifier_Rare_Icon_blue", 
			4 => "Modifier_Rare_Icon_purple", 
			5 => "Modifier_Rare_Icon_red", 
			6 => "Modifier_Rare_Icon_diamond", 
			_ => "", 
		};
	}
}
