using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ApplyMaterialPostFx : MonoBehaviour
{
	public Material mat;

	public Texture secondTex;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		mat.SetTexture("_SecondTex", secondTex);
		Graphics.Blit(source, destination, mat, 0);
	}
}
