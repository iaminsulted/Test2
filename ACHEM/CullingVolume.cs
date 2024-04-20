using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CullingVolume : MonoBehaviour
{
	public List<Renderer> cullRenderers = new List<Renderer>();

	public List<Light> cullLights = new List<Light>();
}
