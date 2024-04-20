public abstract class SpellMachine : OwnerMachine
{
	public MachineSpellType spell = MachineSpellType.Attack;

	public MachineSpellMode spellMode;

	public int spellValue = 1;

	public float castAsLevelMultiplier = 1f;

	public bool isAoE;

	public float radius = 7f;

	public CombatSolver.MachineTargetType target;
}
