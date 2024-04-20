using UnityEngine;

public class PhysicsAddExplosionForce : MonoBehaviour
{
	public Transform forcePosition;

	public float force;

	public float radius;

	public float upwardsModifier;

	public ForceMode mode;

	private void Start()
	{
		Rigidbody[] componentsInChildren = GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].AddExplosionForce(force, forcePosition.position, radius, upwardsModifier, mode);
		}
	}
}
