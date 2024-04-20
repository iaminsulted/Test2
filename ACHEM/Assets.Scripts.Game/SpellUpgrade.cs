using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Game;

public class SpellUpgrade
{
	public int reqEffectID;

	public bool isSpellOverride;

	public int reqClass;

	public int reqClassRank;

	public JObject upgradeData;
}
