using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BlendCamerasPostFx : MonoBehaviour
{
	public Material mat;

	public Camera cam;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!SelectionManager.Instance.GetSelection().Any())
		{
			Graphics.Blit(source, destination);
			return;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		cam.targetTexture = temporary;
		ApplyMaterialPostFx component = cam.GetComponent<ApplyMaterialPostFx>();
		component.secondTex = source;
		component.secondTex = Shader.GetGlobalTexture("_CameraDepthTexture");
		cam.Render();
		mat.SetTexture("_SecondTex", temporary);
		Graphics.Blit(source, destination, mat, 0);
		cam.targetTexture = null;
		RenderTexture.ReleaseTemporary(temporary);
	}
}
