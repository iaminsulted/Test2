namespace Assets.Scripts.Game;

public class SpellChannel
{
	public float duration;

	public int[][] tickActionIDs = new int[0][];

	public int tickCount => tickActionIDs.GetLength(0);
}
