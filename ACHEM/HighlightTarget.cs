using UnityEngine;
using UnityEngine.Serialization;

public class HighlightTarget : MonoBehaviour
{
	[FormerlySerializedAs("ButtonName")]
	public HighlightTargetName Target;

	private void OnEnable()
	{
		HighlightTargets.Add(this);
	}

	private void OnDisable()
	{
		HighlightTargets.Remove(this);
	}
}
