using UnityEngine;

namespace JabberwockyStudio;

[RequireComponent(typeof(Animator))]
public class RandomStartTime : MonoBehaviour
{
	[Tooltip("Index of the layer of the default state")]
	public int _layerIndex;

	private void Start()
	{
		Animator component = GetComponent<Animator>();
		component.Play(component.GetCurrentAnimatorStateInfo(_layerIndex).fullPathHash, -1, Random.Range(0f, 1f));
	}
}
