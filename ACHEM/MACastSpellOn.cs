using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Interactivity/Machine Actions/MA Cast Spell On")]
public class MACastSpellOn : ListenerAction
{
	public int SpellId;

	public MachineSpellMode SpellMode;

	[Range(1f, 100f)]
	[FormerlySerializedAs("Level")]
	public int SpellModeValue;

	public float CastAsLevelMultiplier = 1f;

	public NPCSpawn Target;
}
