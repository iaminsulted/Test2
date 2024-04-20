using UnityEngine;

public class SM_effectCaster : MonoBehaviour
{
	public GameObject moveThis;

	public RaycastHit hit;

	public GameObject[] createThis;

	public float cooldown;

	public float changeCooldown;

	public int selected;

	private float rndNr;

	private GameObject effect;

	public void Start()
	{
		selected = createThis.Length - 1;
	}

	public void Update()
	{
		if (cooldown > 0f)
		{
			cooldown -= Time.deltaTime;
		}
		if (changeCooldown > 0f)
		{
			changeCooldown -= Time.deltaTime;
		}
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
		{
			moveThis.transform.position = hit.point;
			if (Input.GetMouseButton(0) && cooldown <= 0f)
			{
				effect = Object.Instantiate(createThis[selected], moveThis.transform.position, moveThis.transform.rotation);
				if (effect.tag == "raiseEffect")
				{
					Vector3 position = new Vector3(effect.transform.position.x, effect.transform.position.y + 1.5f, effect.transform.position.z);
					effect.transform.position = position;
				}
				cooldown = 0.15f;
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) && changeCooldown <= 0f)
		{
			selected++;
			if (selected > createThis.Length - 1)
			{
				selected = 0;
			}
			changeCooldown = 0.1f;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && changeCooldown <= 0f)
		{
			selected--;
			if (selected < 0)
			{
				selected = createThis.Length - 1;
			}
			changeCooldown = 0.1f;
		}
	}
}
