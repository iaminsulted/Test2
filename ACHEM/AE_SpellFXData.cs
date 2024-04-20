using UnityEngine;

public class AE_SpellFXData : MonoBehaviour
{
	public enum Type
	{
		Undefined,
		Projectile,
		Impact,
		Aura
	}

	public Type type;

	public Vector3 offset;

	public bool isSticky;
}
