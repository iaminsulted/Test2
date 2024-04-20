public class NPCNodeAnimation
{
	public readonly string anim;

	public readonly float nextAnimDelay;

	public NPCNodeAnimation(string anim, float nextAnimDelay)
	{
		this.anim = anim;
		this.nextAnimDelay = nextAnimDelay;
	}
}
