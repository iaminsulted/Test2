using UnityEngine;

public class SM_materialColorRandomizer : MonoBehaviour
{
	public Color color1 = new Color(0.6f, 0.6f, 0.6f, 1f);

	public Color color2 = new Color(0.4f, 0.4f, 0.4f, 1f);

	public bool unifiedcolor = true;

	private float colr;

	private float colg;

	private float colb;

	private float cola;

	public void Start()
	{
		if (!unifiedcolor)
		{
			colr = Random.Range(color1.r, color2.r);
			colg = Random.Range(color1.g, color2.g);
			colb = Random.Range(color1.b, color2.b);
			cola = Random.Range(color1.a, color2.a);
		}
		if (unifiedcolor)
		{
			float value = Random.value;
			colr = Mathf.Min(color1.r, color2.r) + Mathf.Abs(color1.r - color2.r) * value;
			colg = Mathf.Min(color1.g, color2.g) + Mathf.Abs(color1.g - color2.g) * value;
			colb = Mathf.Min(color1.b, color2.b) + Mathf.Abs(color1.b - color2.b) * value;
		}
		GetComponent<Renderer>().material.color = new Color(colr, colg, colb, cola);
	}
}
