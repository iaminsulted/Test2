using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200001E RID: 30
	[AddComponentMenu("")]
	public class ImageEffects
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00012D5C File Offset: 0x00010F5C
		public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
		{
			if (source.texelSize.y < 0f)
			{
				center.y = 1f - center.y;
				angle = -angle;
			}
			Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
			material.SetMatrix("_RotationMatrix", value);
			material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
			material.SetFloat("_Angle", angle * 0.017453292f);
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00012E07 File Offset: 0x00011007
		[Obsolete("Use Graphics.Blit(source,dest) instead")]
		public static void Blit(RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00012E10 File Offset: 0x00011010
		[Obsolete("Use Graphics.Blit(source, destination, material) instead")]
		public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest, material);
		}
	}
}
