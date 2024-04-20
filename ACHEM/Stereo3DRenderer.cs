using System;
using UnityEngine;

[ExecuteInEditMode]
public class Stereo3DRenderer : MonoBehaviour
{
	private Stereo3D _s3d;

	private Material _stereoMaterial;

	private void Start()
	{
		_s3d = GetComponent<Stereo3D>();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture.active = destination;
		GL.PushMatrix();
		GL.LoadOrtho();
		switch (_s3d.State)
		{
		case StereoState.Interlace:
			_s3d.StereoMaterial.SetPass(0);
			break;
		case StereoState.ReversedInterlace:
			_s3d.StereoMaterial.SetPass(0);
			break;
		case StereoState.Anaglyph:
			_s3d.StereoMaterial.SetPass(1);
			break;
		}
		DrawQuad();
		GL.PopMatrix();
		RenderTexture.active = null;
	}

	private void DrawQuad()
	{
		GL.Begin(7);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(0f, 0f, 0.1f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(1f, 0f, 0.1f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(1f, 1f, 0.1f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(0f, 1f, 0.1f);
		GL.End();
	}

	private Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		Matrix4x4 result = default(Matrix4x4);
		float value = 2f * near / (right - left);
		float value2 = 2f * near / (top - bottom);
		float value3 = (right + left) / (right - left);
		float value4 = (top + bottom) / (top - bottom);
		float value5 = (0f - (far + near)) / (far - near);
		float value6 = (0f - 2f * far * near) / (far - near);
		float value7 = 1f;
		result[0, 0] = value;
		result[0, 1] = 0f;
		result[0, 2] = value3;
		result[0, 3] = 0f;
		result[1, 0] = 0f;
		result[1, 1] = value2;
		result[1, 2] = value4;
		result[1, 3] = 0f;
		result[2, 0] = 0f;
		result[2, 1] = 0f;
		result[2, 2] = value5;
		result[2, 3] = value6;
		result[3, 0] = 0f;
		result[3, 1] = 0f;
		result[3, 2] = value7;
		result[3, 3] = 0f;
		return result;
	}

	private Matrix4x4 ProjectionMatrix(bool isLeftCam)
	{
		float aspect = _s3d.CB.NodalCamera.aspect;
		Camera nodalCamera = _s3d.CB.NodalCamera;
		float num = nodalCamera.fieldOfView / 180f * MathF.PI;
		float num2 = aspect;
		float num3 = nodalCamera.nearClipPlane * Mathf.Tan(num * 0.5f);
		float num4 = nodalCamera.nearClipPlane / (_s3d.Convergence + nodalCamera.nearClipPlane);
		float left;
		float right;
		if (isLeftCam)
		{
			left = (0f - num2) * num3 + _s3d.Interaxial / 2f * num4;
			right = num2 * num3 + _s3d.Interaxial / 2f * num4;
		}
		else
		{
			left = (0f - num2) * num3 - _s3d.Interaxial / 2f * num4;
			right = num2 * num3 - _s3d.Interaxial / 2f * num4;
		}
		return PerspectiveOffCenter(left, right, 0f - num3, num3, nodalCamera.nearClipPlane, nodalCamera.farClipPlane);
	}
}
