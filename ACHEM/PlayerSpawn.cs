using System;
using UnityEngine;

[Serializable]
public class PlayerSpawn : MonoBehaviour
{
	public int ID;

	public bool display = true;

	private void Start()
	{
		if (Entities.Instance.me.AccessLevel > 50 && display)
		{
			TextMesh textMesh = new GameObject("ID Label").AddComponent<TextMesh>();
			textMesh.transform.SetParent(base.transform);
			textMesh.text = ID.ToString();
			textMesh.transform.localPosition = new Vector3(0f, -0.5f, 0f);
			textMesh.characterSize = 0.1f;
			textMesh.fontStyle = FontStyle.Bold;
			textMesh.fontSize = 40;
			textMesh.anchor = TextAnchor.MiddleCenter;
			textMesh.alignment = TextAlignment.Center;
			textMesh.gameObject.AddComponent<simpleBillboard>().Flip = true;
			textMesh.gameObject.layer = Layers.TRIGGERS;
		}
	}
}
