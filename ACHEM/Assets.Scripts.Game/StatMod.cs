using StatCurves;

namespace Assets.Scripts.Game;

public class StatMod
{
	public enum ModType
	{
		Base,
		Current,
		Expected
	}

	public Stat stat;

	public ModType type;

	public bool useInverseHealth;

	public float percent;

	public float flat;

	public bool hideDesc;

	public bool IsDynamic => useInverseHealth;
}
