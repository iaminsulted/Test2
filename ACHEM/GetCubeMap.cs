using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class GetCubeMap : MonoBehaviour
{
	public enum encodetype
	{
		PNG,
		JPG
	}

	public enum WorldDirection
	{
		left,
		right,
		front,
		back
	}

	private Camera sourceCamera;

	private int cubeMapRes = 512;

	private bool createMipMaps;

	[HideInInspector]
	public string Location = string.Empty;

	public string Name = "AtlasBG";

	public encodetype ImageType;

	private WorldDirection ImageDirection = WorldDirection.front;

	private Texture2D tempup2;

	private RenderTexture renderTex;

	private Cubemap cube;

	private Texture2D[] texture2Ds;

	private Texture2D virtualPhoto;

	private BloomOptimized bloom;

	private DepthOfField dof;

	private LayerMask layerMask;

	public void CreateDialogueCube(string imageTypeInput)
	{
		texture2Ds = new Texture2D[5];
		sourceCamera = GetComponent<Camera>();
		layerMask = sourceCamera.cullingMask;
		sourceCamera.cullingMask = Layers.GetCutsceneMask(0);
		float fieldOfView = sourceCamera.fieldOfView;
		sourceCamera.fieldOfView = 90f;
		if ((bool)SettingsManager.UseBloom)
		{
			bloom = sourceCamera.GetComponent<BloomOptimized>();
			bloom.enabled = false;
		}
		if ((bool)SettingsManager.UseDepthOfField)
		{
			dof = sourceCamera.GetComponent<DepthOfField>();
			dof.enabled = false;
		}
		_ = base.transform.position;
		Vector3 eulerAngles = base.transform.eulerAngles;
		virtualPhoto = new Texture2D(1024, 1024);
		for (int i = 0; i < texture2Ds.Length; i++)
		{
			if (i == 0)
			{
				base.transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
			}
			if (i == 1)
			{
				base.transform.eulerAngles = new Vector3(0f, eulerAngles.y + 90f, 0f);
			}
			if (i == 2)
			{
				base.transform.eulerAngles = new Vector3(0f, eulerAngles.y - 90f, 0f);
			}
			if (i == 3)
			{
				base.transform.eulerAngles = new Vector3(-90f, eulerAngles.y, 0f);
			}
			if (i == 4)
			{
				base.transform.eulerAngles = new Vector3(90f, eulerAngles.y, 0f);
			}
			RenderTexture targetTexture = sourceCamera.targetTexture;
			RenderTexture renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
			renderTexture.antiAliasing = 8;
			renderTexture.anisoLevel = 16;
			sourceCamera.targetTexture = renderTexture;
			RenderTexture.active = sourceCamera.targetTexture;
			sourceCamera.Render();
			texture2Ds[i] = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, mipChain: false);
			texture2Ds[i].ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
			texture2Ds[i].Apply();
			RenderTexture.active = targetTexture;
			sourceCamera.targetTexture = targetTexture;
			if (i == 0)
			{
				virtualPhoto.SetPixels(256, 256, 512, 512, texture2Ds[i].GetPixels());
			}
			if (i == 1)
			{
				Color[] pixels = texture2Ds[i].GetPixels(0, 0, 256, 512);
				virtualPhoto.SetPixels(768, 256, 256, 512, pixels);
			}
			if (i == 2)
			{
				Color[] pixels2 = texture2Ds[i].GetPixels(256, 0, 256, 512);
				virtualPhoto.SetPixels(0, 256, 256, 512, pixels2);
			}
			if (i == 3)
			{
				Color[] pixels3 = texture2Ds[i].GetPixels(0, 0, 512, 256);
				virtualPhoto.SetPixels(256, 768, 512, 256, pixels3);
			}
			if (i == 4)
			{
				Color[] pixels4 = texture2Ds[i].GetPixels(0, 256, 512, 256);
				virtualPhoto.SetPixels(256, 0, 512, 256, pixels4);
			}
		}
		virtualPhoto.Apply();
		base.transform.eulerAngles = eulerAngles;
		sourceCamera.fieldOfView = fieldOfView;
		sourceCamera.cullingMask = layerMask;
		if ((bool)SettingsManager.UseBloom)
		{
			bloom = sourceCamera.GetComponent<BloomOptimized>();
			bloom.enabled = true;
		}
		if ((bool)SettingsManager.UseDepthOfField)
		{
			dof = sourceCamera.GetComponent<DepthOfField>();
			dof.enabled = true;
		}
		texture2Ds = null;
		if (imageTypeInput.ToLower() == "jpg" || imageTypeInput.ToLower() == "jpeg")
		{
			ImageType = encodetype.JPG;
		}
		else
		{
			ImageType = encodetype.PNG;
		}
		byte[] bytes = ((ImageType == encodetype.PNG) ? virtualPhoto.EncodeToPNG() : virtualPhoto.EncodeToJPG(80));
		string text = OurTempSquareImageLocation();
		File.WriteAllBytes(text, bytes);
		Debug.Log("Image written: " + text);
	}

	[ContextMenu("cube")]
	public void createcube()
	{
		cube = new Cubemap(cubeMapRes, TextureFormat.RGB24, mipChain: false);
		sourceCamera = GetComponent<Camera>();
		sourceCamera.RenderToCubemap(cube);
		virtualPhoto = new Texture2D(1024, 1024, TextureFormat.RGB24, mipChain: false);
		if (cube != null)
		{
			switch (ImageDirection)
			{
			case WorldDirection.front:
				GenerateImage(CubemapFace.NegativeX, "left");
				GenerateImage(CubemapFace.PositiveX, "right");
				GenerateImage(CubemapFace.PositiveZ, "front");
				break;
			case WorldDirection.left:
				GenerateImage(CubemapFace.NegativeZ, "left");
				GenerateImage(CubemapFace.PositiveZ, "right");
				GenerateImage(CubemapFace.NegativeX, "front");
				break;
			case WorldDirection.right:
				GenerateImage(CubemapFace.PositiveZ, "left");
				GenerateImage(CubemapFace.NegativeZ, "right");
				GenerateImage(CubemapFace.PositiveX, "front");
				break;
			case WorldDirection.back:
				GenerateImage(CubemapFace.PositiveZ, "left");
				GenerateImage(CubemapFace.NegativeZ, "right");
				GenerateImage(CubemapFace.PositiveX, "front");
				break;
			}
			Color32[] pixels = virtualPhoto.GetPixels32();
			Array.Reverse(pixels);
			virtualPhoto.SetPixels32(pixels);
			virtualPhoto.Apply();
			GenerateImage(CubemapFace.NegativeY, "down");
			GenerateImage(CubemapFace.PositiveY, "up");
			byte[] bytes = ((ImageType == encodetype.PNG) ? virtualPhoto.EncodeToPNG() : virtualPhoto.EncodeToJPG());
			string text = OurTempSquareImageLocation();
			File.WriteAllBytes(text, bytes);
			Debug.Log("Image written: " + text);
		}
		else
		{
			Debug.LogError("WTF");
		}
	}

	private string OurTempSquareImageLocation()
	{
		string obj = ((Location == string.Empty) ? (Application.persistentDataPath + "/") : (Location + "/"));
		string text = ((ImageType == encodetype.PNG) ? (Name + ".png") : (Name + ".jpg"));
		return obj + text;
	}

	private Color32[] rotateSquare(Color32[] arr, double phi, Texture2D originTexture)
	{
		double num = Math.Sin(phi);
		double num2 = Math.Cos(phi);
		Color32[] pixels = originTexture.GetPixels32();
		int width = originTexture.width;
		int height = originTexture.height;
		int num3 = width / 2;
		int num4 = height / 2;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				pixels[i * width + j] = new Color32(0, 0, 0, 0);
				int num5 = (int)(num2 * (double)(j - num3) + num * (double)(i - num4) + (double)num3);
				int num6 = (int)((0.0 - num) * (double)(j - num3) + num2 * (double)(i - num4) + (double)num4);
				if (num5 > -1 && num5 < width && num6 > -1 && num6 < height)
				{
					pixels[i * width + j] = arr[num6 * width + num5];
				}
			}
		}
		return pixels;
	}

	public void GenerateImage(CubemapFace face, string name, int xoffset = 0, int yoffset = 0, bool flipx = true)
	{
		Color[] pixels = cube.GetPixels(face);
		Texture2D texture2D = new Texture2D(512, 512, TextureFormat.RGB24, mipChain: false);
		texture2D.SetPixels(0, 0, 512, 512, pixels);
		switch (ImageDirection)
		{
		case WorldDirection.left:
			if (name == "up")
			{
				Color32[] pixels4 = rotateSquare(texture2D.GetPixels32(), Math.PI / 2.0, texture2D);
				texture2D.SetPixels32(pixels4);
			}
			else if (name == "down")
			{
				Color32[] pixels5 = rotateSquare(texture2D.GetPixels32(), -Math.PI / 2.0, texture2D);
				texture2D.SetPixels32(pixels5);
			}
			break;
		case WorldDirection.front:
			if (name == "up" || name == "down")
			{
				Color32[] pixels6 = rotateSquare(texture2D.GetPixels32(), Math.PI, texture2D);
				texture2D.SetPixels32(pixels6);
			}
			break;
		case WorldDirection.right:
			if (name == "up")
			{
				Color32[] pixels2 = rotateSquare(texture2D.GetPixels32(), -Math.PI / 2.0, texture2D);
				texture2D.SetPixels32(pixels2);
			}
			else if (name == "down")
			{
				Color32[] pixels3 = rotateSquare(texture2D.GetPixels32(), Math.PI / 2.0, texture2D);
				texture2D.SetPixels32(pixels3);
			}
			break;
		}
		texture2D.Apply();
		SetImageData(texture2D, name);
	}

	public void SetImageData(Texture2D c, string direction)
	{
		switch (direction)
		{
		case "up":
		{
			Color[] pixels4 = c.GetPixels(0, 0, 512, 256);
			virtualPhoto.SetPixels(256, 768, 512, 256, pixels4);
			break;
		}
		case "down":
		{
			Color[] pixels3 = c.GetPixels(0, 256, 512, 256);
			virtualPhoto.SetPixels(256, 0, 512, 256, pixels3);
			break;
		}
		case "left":
		{
			Color[] pixels2 = c.GetPixels(256, 0, 256, 512);
			virtualPhoto.SetPixels(0, 256, 256, 512, pixels2);
			break;
		}
		case "right":
		{
			Color[] pixels = c.GetPixels(0, 0, 256, 512);
			virtualPhoto.SetPixels(768, 256, 256, 512, pixels);
			break;
		}
		case "front":
			virtualPhoto.SetPixels(256, 256, 512, 512, c.GetPixels());
			break;
		}
		virtualPhoto.Apply();
	}

	public RenderTexture GetRenderTexture()
	{
		if (renderTex != null)
		{
			return renderTex;
		}
		renderTex = new RenderTexture(cubeMapRes, cubeMapRes, 16);
		renderTex.dimension = TextureDimension.Cube;
		renderTex.hideFlags = HideFlags.HideAndDontSave;
		renderTex.autoGenerateMips = createMipMaps;
		sourceCamera.RenderToCubemap(renderTex);
		return renderTex;
	}
}
