using UnityEngine;

public class ShowOverdraw : MonoBehaviour
{
	public Shader replacementShader;

	public string shader;

	private bool enableOD;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			showOverdraw();
		}
	}

	public void showOverdraw()
	{
		if (!enableOD)
		{
			if (replacementShader == null)
			{
				replacementShader = Shader.Find(shader);
			}
			if (replacementShader != null)
			{
				Camera.main.SetReplacementShader(replacementShader, "");
				Camera.main.backgroundColor = new Color(0f, 0f, 0f, 0f);
			}
		}
		else
		{
			Camera.main.SetReplacementShader(null, "RenderType");
		}
		enableOD = !enableOD;
	}
}
