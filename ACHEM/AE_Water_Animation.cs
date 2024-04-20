using UnityEngine;

public class AE_Water_Animation : MonoBehaviour
{
	private void Update()
	{
		if ((bool)GetComponent<Renderer>())
		{
			Material sharedMaterial = GetComponent<Renderer>().sharedMaterial;
			if ((bool)sharedMaterial)
			{
				Vector4 vector = sharedMaterial.GetVector("_WaveSpeed");
				float @float = sharedMaterial.GetFloat("_WaveScale");
				float num = Time.time / 20f;
				Vector4 vector2 = vector * (num * @float);
				Vector4 value = new Vector4(Mathf.Repeat(vector2.x, vector.x), Mathf.Repeat(vector2.y, vector.y), Mathf.Repeat(vector2.z, vector.z), Mathf.Repeat(vector2.w, vector.w));
				sharedMaterial.SetVector("_WaveOffset", value);
			}
		}
	}
}
