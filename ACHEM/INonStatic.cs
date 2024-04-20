using UnityEngine;

public interface INonStatic
{
	Transform transformParent { get; set; }

	Transform parent { get; set; }

	Transform Target { get; }
}
