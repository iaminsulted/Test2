using UnityEngine;

public class SM_TransRimShaderFader : MonoBehaviour
{
	public float startStr = 2f;

	public float speed = 3f;

	private float timeGoes;

	private float currStr;

	private Renderer r;

	public void Start()
	{
		r = GetComponent<Renderer>();
	}

	public void Update()
	{
		timeGoes += Time.deltaTime * speed * startStr;
		currStr = startStr - timeGoes;
		r.material.SetFloat("_AllPower", currStr);
	}
}
