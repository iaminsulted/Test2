using UnityEngine;

namespace AQ3D.DialogueSystem;

public class DialogueSlot : MonoBehaviour
{
	public PlayerAssetController player;

	public Animator anim;

	private const string idleState = "Idle";

	private const string fightState = "Fight";

	public void Clear()
	{
		player = null;
		anim = null;
		if (base.transform.childCount > 0)
		{
			Object.Destroy(base.transform.GetChild(0).gameObject);
		}
	}

	public void Init(GameObject go)
	{
		go.transform.SetParent(base.transform, worldPositionStays: false);
		if ((bool)go.GetComponentInChildren<PlayerAssetController>())
		{
			player = go.GetComponentInChildren<PlayerAssetController>();
		}
		if ((bool)go.GetComponentInChildren<Animator>())
		{
			anim = go.GetComponentInChildren<Animator>();
		}
	}

	public void PlayAnimation(int dialogid, int frameid, string animationstate, int slotid)
	{
		if (!(anim != null))
		{
			return;
		}
		if (animationstate == "Idle" || animationstate == "Fight")
		{
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				anim.Play("Idle");
				if (animationstate == "Idle")
				{
					anim.SetFloat("IdleBlend", 0f);
				}
				else
				{
					anim.SetFloat("IdleBlend", 0.5f);
				}
			}
		}
		else if (string.IsNullOrEmpty(animationstate))
		{
			anim.Play("Idle");
			anim.SetFloat("IdleBlend", 0f);
		}
		else if (!anim.HasAnimatorState(animationstate))
		{
			Debug.LogError("Animation not found: '" + animationstate + "' in DialogID: " + dialogid + " FrameID: " + frameid + " SlotID: " + slotid);
		}
		else
		{
			anim.Play(animationstate);
		}
	}
}
