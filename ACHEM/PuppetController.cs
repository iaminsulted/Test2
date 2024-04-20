using UnityEngine;

public class PuppetController : MonoBehaviour
{
	private Transform puppet;

	public Transform puppetJamState;

	public Transform puppetBPS;

	public Transform lookNode;

	private int oldJamState;

	private float layerLerp;

	private Vector3 currentVelocity;

	private Vector3 oldPos;

	public bool dynamicWalk = true;

	public Rigidbody rb;

	private Animator charAnim;

	private void Start()
	{
		puppet = base.gameObject.transform;
		charAnim = base.gameObject.GetComponent<Animator>();
		oldJamState = 0;
		oldPos = puppet.transform.localPosition;
	}

	private float getBPS()
	{
		return Mathf.RoundToInt(puppetBPS.localPosition.y * 100f);
	}

	private int getJamState()
	{
		return Mathf.RoundToInt(puppetJamState.localPosition.y * 100f);
	}

	private Vector2 getLookVector()
	{
		return new Vector2(lookNode.localPosition.x * 100f, lookNode.localPosition.y * 100f);
	}

	private void FixedUpdate()
	{
		if (dynamicWalk)
		{
			currentVelocity = base.transform.InverseTransformDirection(rb.velocity);
			if (currentVelocity != Vector3.zero)
			{
				if (layerLerp < 1f)
				{
					layerLerp += 0.06f;
					if (layerLerp > 1f)
					{
						layerLerp = 1f;
					}
				}
				charAnim.SetLayerWeight(charAnim.GetLayerIndex("Movement"), layerLerp);
				charAnim.SetFloat("XBlend", currentVelocity.x);
				charAnim.SetFloat("ZBlend", currentVelocity.z);
			}
			else
			{
				if (layerLerp > 0f)
				{
					layerLerp -= 0.06f;
					if (layerLerp < 0f)
					{
						layerLerp = 0f;
					}
				}
				charAnim.SetLayerWeight(charAnim.GetLayerIndex("Movement"), layerLerp);
			}
		}
		if (getJamState() != oldJamState)
		{
			oldJamState = Mathf.RoundToInt(getJamState());
			charAnim.CrossFade("Jam" + oldJamState, 0.5f, charAnim.GetLayerIndex("FullAction"));
		}
		charAnim.SetFloat("Tempo", getBPS());
		if (getLookVector() != Vector2.zero)
		{
			charAnim.SetFloat("XLook", getLookVector().x);
			charAnim.SetFloat("YLook", getLookVector().y);
		}
		oldPos = puppet.transform.localPosition;
	}

	private void LateUpdate()
	{
		puppet.position = rb.gameObject.transform.position;
		puppet.rotation = rb.gameObject.transform.rotation;
	}
}
