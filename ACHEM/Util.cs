using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public static class Util
{
	public static string Clipboard
	{
		get
		{
			return UniPasteBoard.GetClipBoardString();
		}
		set
		{
			UniPasteBoard.SetClipBoardString(value);
		}
	}

	public static bool BitGet(long num, int index)
	{
		return (num & (1L << index - 1)) != 0;
	}

	public static long BitSet(long num, int index)
	{
		num |= 1L << index - 1;
		return num;
	}

	public static long BitClear(long num, int index)
	{
		num &= ~(1L << index - 1);
		return num;
	}

	public static void trace(string sender, object message)
	{
		string text = message.ToString();
		Debug.Log("> " + sender + ": " + text);
	}

	public static TValue Find<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
	{
		source.TryGetValue(key, out var value);
		return value;
	}

	public static string ReplaceAt(this string input, int index, char newChar)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		char[] array = input.ToCharArray();
		array[index] = newChar;
		return new string(array);
	}

	public static string RemoveExtension(this string fileName)
	{
		string result = string.Copy(fileName);
		int num = fileName.LastIndexOf(".");
		if (num >= 0)
		{
			result = fileName.Substring(0, num);
		}
		return result;
	}

	public static string ToTitleCase(this string stringToConvert)
	{
		string text = stringToConvert[0].ToString();
		if (stringToConvert.Length <= 0)
		{
			return stringToConvert;
		}
		return text.ToUpper() + stringToConvert.Substring(1);
	}

	public static Vector3 Round(this Vector3 v)
	{
		return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
	}

	public static Color32 ToColor32(this int value)
	{
		Color32 result = default(Color32);
		result.r = (byte)(0xFFu & (uint)(value >> 24));
		result.g = (byte)(0xFFu & (uint)(value >> 16));
		result.b = (byte)(0xFFu & (uint)(value >> 8));
		result.a = (byte)(0xFFu & (uint)value);
		return result;
	}

	public static Color32 ToColor32(this string hex)
	{
		if ((hex.Length != 6 && hex.Length != 8) || !int.TryParse(hex, NumberStyles.HexNumber, null, out var result))
		{
			throw new NotSupportedException(hex);
		}
		if (hex.Length == 6)
		{
			result = (result << 8) + 255;
		}
		return result.ToColor32();
	}

	public static string GetPath(this Transform current)
	{
		if (current.parent == null)
		{
			return "/" + current.name;
		}
		return current.parent.GetPath() + "/" + current.name;
	}

	public static bool InHierarchy(this Transform current, Transform tform)
	{
		do
		{
			if (current == tform)
			{
				return true;
			}
			current = current.parent;
		}
		while (current != null);
		return false;
	}

	public static string GetPath(this Component component)
	{
		return component.transform.GetPath() + " - " + component.GetType().ToString();
	}

	public static T GetComponentInParents<T>(this GameObject go) where T : Component
	{
		if (go == null)
		{
			return null;
		}
		object component = go.GetComponent<T>();
		if (component == null)
		{
			Transform parent = go.transform.parent;
			while (parent != null && component == null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
		}
		return (T)component;
	}

	public static float Angle2D(this Vector3 a)
	{
		return Mathf.Atan2(a.x, a.z) * 57.29578f;
	}

	public static T[] GetComponentsInChildrenNonRecursive<T>(this GameObject go, bool includeInactive = false) where T : Component
	{
		if (go == null)
		{
			return null;
		}
		List<T> list = new List<T>();
		foreach (Transform item in go.transform)
		{
			if (item.gameObject.activeSelf)
			{
				T component = item.GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		return list.ToArray();
	}

	public static List<Transform> GetAllChildren(this Transform parent)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in parent)
		{
			list.Add(item);
		}
		return list;
	}

	public static bool HasAnimatorState(this Animator animator, string animState)
	{
		for (int i = 0; i < animator.layerCount; i++)
		{
			if (animator.HasState(i, Animator.StringToHash(animState)))
			{
				return true;
			}
		}
		return false;
	}

	private static Bounds _getMeshBounds(GameObject go, ref Bounds bounds)
	{
		foreach (Transform item in go.transform)
		{
			_getMeshBounds(item.gameObject, ref bounds);
		}
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component != null)
		{
			bounds.Encapsulate(component.bounds);
		}
		Collider component2 = go.GetComponent<Collider>();
		if (component2 != null)
		{
			bounds.Encapsulate(component2.bounds);
		}
		return bounds;
	}

	public static Bounds GetMeshBounds(GameObject go)
	{
		Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
		return _getMeshBounds(go, ref bounds);
	}

	public static void calculateMeshTangents(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int num = triangles.Length;
		int num2 = vertices.Length;
		Vector3[] array = new Vector3[num2];
		Vector3[] array2 = new Vector3[num2];
		Vector4[] array3 = new Vector4[num2];
		long num3 = 0L;
		long num4 = 0L;
		for (num3 = 0L; num3 < num; num3 += 3)
		{
			long num5 = triangles[num3];
			long num6 = triangles[num3 + 1];
			num4 = triangles[num3 + 2];
			Vector3 vector = vertices[num5];
			Vector3 vector2 = vertices[num6];
			Vector3 vector3 = vertices[num4];
			Vector3 vector4 = uv[num5];
			Vector3 vector5 = uv[num6];
			Vector3 vector6 = uv[num4];
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			array[num5] += vector7;
			array[num6] += vector7;
			array[num4] += vector7;
			array2[num5] += vector8;
			array2[num6] += vector8;
			array2[num4] += vector8;
		}
		for (num3 = 0L; num3 < num2; num3++)
		{
			Vector3 normal = normals[num3];
			Vector3 tangent = array[num3];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			array3[num3].x = tangent.x;
			array3[num3].y = tangent.y;
			array3[num3].z = tangent.z;
			array3[num3].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), array2[num3]) < 0f) ? (-1f) : 1f);
		}
		mesh.tangents = array3;
	}

	public static float OldSignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
	{
		float num = Vector3.Angle(a, b);
		float num2 = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));
		return num * num2;
	}

	public static float SignedAngleBetween(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	public static Transform SearchHierarchyForBone(this Transform current, string name)
	{
		if (current.name == name)
		{
			return current;
		}
		for (int i = 0; i < current.childCount; i++)
		{
			Transform transform = current.GetChild(i).SearchHierarchyForBone(name);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public static void SetLayerRecursively(this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		foreach (Transform item in gameObject.transform)
		{
			item.gameObject.SetLayerRecursively(layer);
		}
	}

	public static void SwitchLayerRecursively(this GameObject gameObject, int fromlayer, int tolayer)
	{
		if (gameObject.layer == fromlayer)
		{
			gameObject.layer = tolayer;
		}
		foreach (Transform item in gameObject.transform)
		{
			item.gameObject.SwitchLayerRecursively(fromlayer, tolayer);
		}
	}

	public static string TokenGenerator()
	{
		System.Random random = new System.Random(DateTime.Now.Millisecond);
		int num = 20;
		string text = "";
		for (int i = 1; i <= num; i++)
		{
			text += (char)(65 + random.Next(0, 26));
		}
		return text;
	}

	public static string Base64Encode(string plainText)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
	}

	public static string Base64Decode(string base64EncodedData)
	{
		byte[] bytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string Reverse(string s)
	{
		char[] array = s.ToCharArray();
		Array.Reverse(array);
		return new string(array);
	}

	public static ComparisonType GetComparison(string str)
	{
		switch (str)
		{
		case "=":
			return ComparisonType.Equal;
		case ">=":
			return ComparisonType.GreaterThanOrEqual;
		case "<=":
			return ComparisonType.LessThanOrEqual;
		case ">":
			return ComparisonType.GreaterThan;
		case "<":
			return ComparisonType.LessThan;
		default:
			Debug.LogError("Invalid comparison type: " + str);
			return ComparisonType.Equal;
		}
	}

	public static Color SetAlpha(this Color color, float val)
	{
		color.a = val;
		return color;
	}
}
