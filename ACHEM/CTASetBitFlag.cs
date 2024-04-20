using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Set Bit Flag")]
public class CTASetBitFlag : ClientTriggerAction
{
	public string BitFlagName;

	public byte BitFlagIndex;

	protected override void OnExecute()
	{
		Game.Instance.SendBitFlagUpdateRequest(BitFlagName, BitFlagIndex, value: true);
	}
}
