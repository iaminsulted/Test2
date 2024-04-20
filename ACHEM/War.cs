public class War
{
	public int WarID;

	public string Name;

	public int Target;

	public int Progress;

	public int ProgressSinceLastSave;

	public string ColorFill;

	public string ColorFrame;

	public string ColorText;

	public float ProgressPercent => (float)Progress / (float)Target;
}
