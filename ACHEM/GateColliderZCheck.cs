using UnityEngine;

public class GateColliderZCheck : MonoBehaviour
{
	private Player player;

	private Collider collide;

	private void Start()
	{
		player = Entities.Instance.me;
		collide = GetComponent<Collider>();
	}

	private void Update()
	{
		if (player.position.z > base.transform.position.z)
		{
			collide.isTrigger = false;
		}
		else
		{
			collide.isTrigger = true;
		}
	}
}
