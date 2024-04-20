using UnityEngine;

namespace JabberwockyStudio;

[RequireComponent(typeof(Animator))]
public class RandomStartTimeOnEnable : MonoBehaviour
{
	[Tooltip("Index of the layer of the default state")]
	public int _layerIndex;

	private void OnEnable()
	{
		Animator component = GetComponent<Animator>();
		component.Play(component.GetCurrentAnimatorStateInfo(_layerIndex).fullPathHash, -1, Random.Range(0f, 1f));
	}
}
