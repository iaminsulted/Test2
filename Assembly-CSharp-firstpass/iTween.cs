using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class iTween : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Init(GameObject target)
	{
		iTween.MoveBy(target, Vector3.zero, 0f);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (!(args["from"].GetType() == typeof(Color)))
			{
				Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		iTween.Launch(target, args);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021F1 File Offset: 0x000003F1
	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		iTween.FadeFrom(target, iTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002226 File Offset: 0x00000426
	public static void FadeFrom(GameObject target, Hashtable args)
	{
		iTween.ColorFrom(target, args);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000222F File Offset: 0x0000042F
	public static void FadeTo(GameObject target, float alpha, float time)
	{
		iTween.FadeTo(target, iTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002264 File Offset: 0x00000464
	public static void FadeTo(GameObject target, Hashtable args)
	{
		iTween.ColorTo(target, args);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000226D File Offset: 0x0000046D
	public static void ColorFrom(GameObject target, Color color, float time)
	{
		iTween.ColorFrom(target, iTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000022A4 File Offset: 0x000004A4
	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = iTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (object obj in target.transform)
			{
				Component component = (Transform)obj;
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				iTween.ColorFrom(component.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		if (target.GetComponent<Renderer>())
		{
			color = (color2 = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			color = (color2 = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002538 File Offset: 0x00000738
	public static void ColorTo(GameObject target, Color color, float time)
	{
		iTween.ColorTo(target, iTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002570 File Offset: 0x00000770
	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (object obj in target.transform)
			{
				Component component = (Transform)obj;
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				iTween.ColorTo(component.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002658 File Offset: 0x00000858
	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioFrom(target, iTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000026AC File Offset: 0x000008AC
	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent<AudioSource>())
			{
				Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		Vector2 vector;
		Vector2 vector2;
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002804 File Offset: 0x00000A04
	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioTo(target, iTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002858 File Offset: 0x00000A58
	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000028B3 File Offset: 0x00000AB3
	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		iTween.Stab(target, iTween.Hash(new object[]
		{
			"audioclip",
			audioclip,
			"delay",
			delay
		}));
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000028E3 File Offset: 0x00000AE3
	public static void Stab(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "stab";
		iTween.Launch(target, args);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002904 File Offset: 0x00000B04
	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookFrom(target, iTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000293C File Offset: 0x00000B3C
	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			target.transform.LookAt((Transform)args["looktarget"], ((Vector3?)args["up"]) ?? iTween.Defaults.up);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			target.transform.LookAt((Vector3)args["looktarget"], ((Vector3?)args["up"]) ?? iTween.Defaults.up);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			string a = (string)args["axis"];
			if (!(a == "x"))
			{
				if (!(a == "y"))
				{
					if (a == "z")
					{
						eulerAngles2.x = eulerAngles.x;
						eulerAngles2.y = eulerAngles.y;
					}
				}
				else
				{
					eulerAngles2.x = eulerAngles.x;
					eulerAngles2.z = eulerAngles.z;
				}
			}
			else
			{
				eulerAngles2.y = eulerAngles.y;
				eulerAngles2.z = eulerAngles.z;
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002B13 File Offset: 0x00000D13
	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookTo(target, iTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002B48 File Offset: 0x00000D48
	public static void LookTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002C32 File Offset: 0x00000E32
	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		iTween.MoveTo(target, iTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002C68 File Offset: 0x00000E68
	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002D8B File Offset: 0x00000F8B
	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		iTween.MoveFrom(target, iTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002DC0 File Offset: 0x00000FC0
	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector2;
			Vector3 vector;
			if (flag)
			{
				vector = (vector2 = target.transform.localPosition);
			}
			else
			{
				vector = (vector2 = target.transform.position);
			}
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					vector = ((Transform)args["position"]).position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000030D0 File Offset: 0x000012D0
	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.MoveAdd(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00003105 File Offset: 0x00001305
	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00003136 File Offset: 0x00001336
	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		iTween.MoveBy(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0000316B File Offset: 0x0000136B
	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000319C File Offset: 0x0000139C
	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleTo(target, iTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000031D4 File Offset: 0x000013D4
	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000032F7 File Offset: 0x000014F7
	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleFrom(target, iTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000332C File Offset: 0x0000152C
	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		Vector3 localScale2;
		Vector3 localScale = localScale2 = target.transform.localScale;
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				localScale = ((Transform)args["scale"]).localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.transform.localScale = localScale;
		args["scale"] = localScale2;
		args["type"] = "scale";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000347C File Offset: 0x0000167C
	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.ScaleAdd(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000034B1 File Offset: 0x000016B1
	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000034E2 File Offset: 0x000016E2
	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		iTween.ScaleBy(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003517 File Offset: 0x00001717
	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003548 File Offset: 0x00001748
	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateTo(target, iTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003580 File Offset: 0x00001780
	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000036A3 File Offset: 0x000018A3
	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateFrom(target, iTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000036D8 File Offset: 0x000018D8
	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		Vector3 vector2;
		Vector3 vector;
		if (flag)
		{
			vector = (vector2 = target.transform.localEulerAngles);
		}
		else
		{
			vector = (vector2 = target.transform.eulerAngles);
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				vector = ((Transform)args["rotation"]).eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003872 File Offset: 0x00001A72
	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.RotateAdd(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000038A7 File Offset: 0x00001AA7
	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000038D8 File Offset: 0x00001AD8
	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		iTween.RotateBy(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600002C RID: 44 RVA: 0x0000390D File Offset: 0x00001B0D
	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000393E File Offset: 0x00001B3E
	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakePosition(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00003973 File Offset: 0x00001B73
	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		iTween.Launch(target, args);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000039A4 File Offset: 0x00001BA4
	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakeScale(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000039D9 File Offset: 0x00001BD9
	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		iTween.Launch(target, args);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003A0A File Offset: 0x00001C0A
	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakeRotation(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003A3F File Offset: 0x00001C3F
	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		iTween.Launch(target, args);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003A70 File Offset: 0x00001C70
	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchPosition(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003AA8 File Offset: 0x00001CA8
	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003AF6 File Offset: 0x00001CF6
	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchRotation(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003B2C File Offset: 0x00001D2C
	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003B7A File Offset: 0x00001D7A
	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchScale(target, iTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003BB0 File Offset: 0x00001DB0
	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003C00 File Offset: 0x00001E00
	private void GenerateTargets()
	{
		string text = this.type;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 2361356451U)
		{
			if (num <= 1031692888U)
			{
				if (num != 407568404U)
				{
					if (num != 1031692888U)
					{
						return;
					}
					if (!(text == "color"))
					{
						return;
					}
					if (this.method == "to")
					{
						this.GenerateColorToTargets();
						this.apply = new iTween.ApplyTween(this.ApplyColorToTargets);
						return;
					}
				}
				else
				{
					if (!(text == "move"))
					{
						return;
					}
					string a = this.method;
					if (!(a == "to"))
					{
						if (!(a == "by") && !(a == "add"))
						{
							return;
						}
						this.GenerateMoveByTargets();
						this.apply = new iTween.ApplyTween(this.ApplyMoveByTargets);
						return;
					}
					else
					{
						if (this.tweenArguments.Contains("path"))
						{
							this.GenerateMoveToPathTargets();
							this.apply = new iTween.ApplyTween(this.ApplyMoveToPathTargets);
							return;
						}
						this.GenerateMoveToTargets();
						this.apply = new iTween.ApplyTween(this.ApplyMoveToTargets);
						return;
					}
				}
			}
			else if (num != 1113510858U)
			{
				if (num != 2190941297U)
				{
					if (num != 2361356451U)
					{
						return;
					}
					if (!(text == "punch"))
					{
						return;
					}
					string a = this.method;
					if (a == "position")
					{
						this.GeneratePunchPositionTargets();
						this.apply = new iTween.ApplyTween(this.ApplyPunchPositionTargets);
						return;
					}
					if (a == "rotation")
					{
						this.GeneratePunchRotationTargets();
						this.apply = new iTween.ApplyTween(this.ApplyPunchRotationTargets);
						return;
					}
					if (!(a == "scale"))
					{
						return;
					}
					this.GeneratePunchScaleTargets();
					this.apply = new iTween.ApplyTween(this.ApplyPunchScaleTargets);
					return;
				}
				else
				{
					if (!(text == "scale"))
					{
						return;
					}
					string a = this.method;
					if (a == "to")
					{
						this.GenerateScaleToTargets();
						this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
						return;
					}
					if (a == "by")
					{
						this.GenerateScaleByTargets();
						this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
						return;
					}
					if (!(a == "add"))
					{
						return;
					}
					this.GenerateScaleAddTargets();
					this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
					return;
				}
			}
			else
			{
				if (!(text == "value"))
				{
					return;
				}
				string a = this.method;
				if (a == "float")
				{
					this.GenerateFloatTargets();
					this.apply = new iTween.ApplyTween(this.ApplyFloatTargets);
					return;
				}
				if (a == "vector2")
				{
					this.GenerateVector2Targets();
					this.apply = new iTween.ApplyTween(this.ApplyVector2Targets);
					return;
				}
				if (a == "vector3")
				{
					this.GenerateVector3Targets();
					this.apply = new iTween.ApplyTween(this.ApplyVector3Targets);
					return;
				}
				if (a == "color")
				{
					this.GenerateColorTargets();
					this.apply = new iTween.ApplyTween(this.ApplyColorTargets);
					return;
				}
				if (!(a == "rect"))
				{
					return;
				}
				this.GenerateRectTargets();
				this.apply = new iTween.ApplyTween(this.ApplyRectTargets);
				return;
			}
		}
		else if (num <= 3180049141U)
		{
			if (num != 2784296202U)
			{
				if (num != 3180049141U)
				{
					return;
				}
				if (!(text == "shake"))
				{
					return;
				}
				string a = this.method;
				if (a == "position")
				{
					this.GenerateShakePositionTargets();
					this.apply = new iTween.ApplyTween(this.ApplyShakePositionTargets);
					return;
				}
				if (a == "scale")
				{
					this.GenerateShakeScaleTargets();
					this.apply = new iTween.ApplyTween(this.ApplyShakeScaleTargets);
					return;
				}
				if (!(a == "rotation"))
				{
					return;
				}
				this.GenerateShakeRotationTargets();
				this.apply = new iTween.ApplyTween(this.ApplyShakeRotationTargets);
				return;
			}
			else
			{
				if (!(text == "rotate"))
				{
					return;
				}
				string a = this.method;
				if (a == "to")
				{
					this.GenerateRotateToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyRotateToTargets);
					return;
				}
				if (a == "add")
				{
					this.GenerateRotateAddTargets();
					this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
					return;
				}
				if (!(a == "by"))
				{
					return;
				}
				this.GenerateRotateByTargets();
				this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
				return;
			}
		}
		else if (num != 3764468121U)
		{
			if (num != 3778758817U)
			{
				if (num != 3874444950U)
				{
					return;
				}
				if (!(text == "look"))
				{
					return;
				}
				if (this.method == "to")
				{
					this.GenerateLookToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyLookToTargets);
					return;
				}
			}
			else
			{
				if (!(text == "stab"))
				{
					return;
				}
				this.GenerateStabTargets();
				this.apply = new iTween.ApplyTween(this.ApplyStabTargets);
			}
		}
		else
		{
			if (!(text == "audio"))
			{
				return;
			}
			if (this.method == "to")
			{
				this.GenerateAudioToTargets();
				this.apply = new iTween.ApplyTween(this.ApplyAudioToTargets);
				return;
			}
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00004120 File Offset: 0x00002320
	private void GenerateRectTargets()
	{
		this.rects = new Rect[3];
		this.rects[0] = (Rect)this.tweenArguments["from"];
		this.rects[1] = (Rect)this.tweenArguments["to"];
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000417C File Offset: 0x0000237C
	private void GenerateColorTargets()
	{
		this.colors = new Color[1, 3];
		this.colors[0, 0] = (Color)this.tweenArguments["from"];
		this.colors[0, 1] = (Color)this.tweenArguments["to"];
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000041DC File Offset: 0x000023DC
	private void GenerateVector3Targets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (Vector3)this.tweenArguments["from"];
		this.vector3s[1] = (Vector3)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x0000428C File Offset: 0x0000248C
	private void GenerateVector2Targets()
	{
		this.vector2s = new Vector2[3];
		this.vector2s[0] = (Vector2)this.tweenArguments["from"];
		this.vector2s[1] = (Vector2)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			Vector3 a = new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0f);
			Vector3 b = new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(a, b));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00004380 File Offset: 0x00002580
	private void GenerateFloatTargets()
	{
		this.floats = new float[3];
		this.floats[0] = (float)this.tweenArguments["from"];
		this.floats[1] = (float)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(this.floats[0] - this.floats[1]);
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000441C File Offset: 0x0000261C
	private void GenerateColorToTargets()
	{
		if (base.GetComponent<Renderer>())
		{
			this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
			{
				this.colors[i, 0] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
				this.colors[i, 1] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
			}
		}
		else if (base.GetComponent<Light>())
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<Light>().color);
		}
		else
		{
			this.colors = new Color[1, 3];
		}
		if (this.tweenArguments.Contains("color"))
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				this.colors[j, 1] = (Color)this.tweenArguments["color"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("r"))
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					this.colors[k, 1].r = (float)this.tweenArguments["r"];
				}
			}
			if (this.tweenArguments.Contains("g"))
			{
				for (int l = 0; l < this.colors.GetLength(0); l++)
				{
					this.colors[l, 1].g = (float)this.tweenArguments["g"];
				}
			}
			if (this.tweenArguments.Contains("b"))
			{
				for (int m = 0; m < this.colors.GetLength(0); m++)
				{
					this.colors[m, 1].b = (float)this.tweenArguments["b"];
				}
			}
			if (this.tweenArguments.Contains("a"))
			{
				for (int n = 0; n < this.colors.GetLength(0); n++)
				{
					this.colors[n, 1].a = (float)this.tweenArguments["a"];
				}
			}
		}
		if (this.tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < this.colors.GetLength(0); num++)
			{
				this.colors[num, 1].a = (float)this.tweenArguments["amount"];
			}
			return;
		}
		if (this.tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < this.colors.GetLength(0); num2++)
			{
				this.colors[num2, 1].a = (float)this.tweenArguments["alpha"];
			}
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00004768 File Offset: 0x00002968
	private void GenerateAudioToTargets()
	{
		this.vector2s = new Vector2[3];
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent<AudioSource>())
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
			this.Dispose();
		}
		this.vector2s[0] = (this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch));
		if (this.tweenArguments.Contains("volume"))
		{
			this.vector2s[1].x = (float)this.tweenArguments["volume"];
		}
		if (this.tweenArguments.Contains("pitch"))
		{
			this.vector2s[1].y = (float)this.tweenArguments["pitch"];
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00004884 File Offset: 0x00002A84
	private void GenerateStabTargets()
	{
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent<AudioSource>())
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			base.gameObject.AddComponent<AudioSource>();
			this.audioSource = base.GetComponent<AudioSource>();
			this.audioSource.playOnAwake = false;
		}
		this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
		if (this.tweenArguments.Contains("pitch"))
		{
			this.audioSource.pitch = (float)this.tweenArguments["pitch"];
		}
		if (this.tweenArguments.Contains("volume"))
		{
			this.audioSource.volume = (float)this.tweenArguments["volume"];
		}
		this.time = this.audioSource.clip.length / this.audioSource.pitch;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000049A8 File Offset: 0x00002BA8
	private void GenerateLookToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = this.thisTransform.eulerAngles;
		if (this.tweenArguments.Contains("looktarget"))
		{
			if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				this.thisTransform.LookAt((Transform)this.tweenArguments["looktarget"], ((Vector3?)this.tweenArguments["up"]) ?? iTween.Defaults.up);
			}
			else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				this.thisTransform.LookAt((Vector3)this.tweenArguments["looktarget"], ((Vector3?)this.tweenArguments["up"]) ?? iTween.Defaults.up);
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
			this.Dispose();
		}
		this.vector3s[1] = this.thisTransform.eulerAngles;
		this.thisTransform.eulerAngles = this.vector3s[0];
		if (this.tweenArguments.Contains("axis"))
		{
			string a = (string)this.tweenArguments["axis"];
			if (!(a == "x"))
			{
				if (!(a == "y"))
				{
					if (a == "z")
					{
						this.vector3s[1].x = this.vector3s[0].x;
						this.vector3s[1].y = this.vector3s[0].y;
					}
				}
				else
				{
					this.vector3s[1].x = this.vector3s[0].x;
					this.vector3s[1].z = this.vector3s[0].z;
				}
			}
			else
			{
				this.vector3s[1].y = this.vector3s[0].y;
				this.vector3s[1].z = this.vector3s[0].z;
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00004D30 File Offset: 0x00002F30
	private void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])this.tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				this.Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])this.tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				this.Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (this.thisTransform.position != array2[0])
		{
			if (!this.tweenArguments.Contains("movetopath") || (bool)this.tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		this.vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			this.vector3s[1] = this.thisTransform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, this.vector3s, num, array2.Length);
		this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
		this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
		if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[this.vector3s.Length];
			Array.Copy(this.vector3s, array4, this.vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			this.vector3s = new Vector3[array4.Length];
			Array.Copy(array4, this.vector3s, array4.Length);
		}
		this.path = new iTween.CRSpline(this.vector3s);
		if (this.tweenArguments.Contains("speed"))
		{
			float num2 = iTween.PathLength(this.vector3s);
			this.time = num2 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00005010 File Offset: 0x00003210
	private void GenerateMoveToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localPosition);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = this.thisTransform.position);
		}
		if (this.tweenArguments.Contains("position"))
		{
			if (this.tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["position"];
				this.vector3s[1] = transform.position;
			}
			else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["position"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000527C File Offset: 0x0000347C
	private void GenerateMoveByTargets()
	{
		this.vector3s = new Vector3[6];
		this.vector3s[4] = this.thisTransform.eulerAngles;
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = this.thisTransform.position));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = this.vector3s[0] + (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = this.vector3s[0].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = this.vector3s[0].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = this.vector3s[0].z + (float)this.tweenArguments["z"];
			}
		}
		this.thisTransform.Translate(this.vector3s[1], this.space);
		this.vector3s[5] = this.thisTransform.position;
		this.thisTransform.position = this.vector3s[0];
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000054F0 File Offset: 0x000036F0
	private void GenerateScaleToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localScale);
		if (this.tweenArguments.Contains("scale"))
		{
			if (this.tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["scale"];
				this.vector3s[1] = transform.localScale;
			}
			else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000056E0 File Offset: 0x000038E0
	private void GenerateScaleByTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00005870 File Offset: 0x00003A70
	private void GenerateScaleAddTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00005A00 File Offset: 0x00003C00
	private void GenerateRotateToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = this.thisTransform.localEulerAngles);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = this.thisTransform.eulerAngles);
		}
		if (this.tweenArguments.Contains("rotation"))
		{
			if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["rotation"];
				this.vector3s[1] = transform.eulerAngles;
			}
			else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00005CB8 File Offset: 0x00003EB8
	private void GenerateRotateAddTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = this.thisTransform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00005E54 File Offset: 0x00004054
	private void GenerateRotateByTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = this.thisTransform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + 360f * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + 360f * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + 360f * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x0000601C File Offset: 0x0000421C
	private void GenerateShakePositionTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[3] = this.thisTransform.eulerAngles;
		this.vector3s[0] = this.thisTransform.position;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
			return;
		}
		if (this.tweenArguments.Contains("x"))
		{
			this.vector3s[1].x = (float)this.tweenArguments["x"];
		}
		if (this.tweenArguments.Contains("y"))
		{
			this.vector3s[1].y = (float)this.tweenArguments["y"];
		}
		if (this.tweenArguments.Contains("z"))
		{
			this.vector3s[1].z = (float)this.tweenArguments["z"];
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00006140 File Offset: 0x00004340
	private void GenerateShakeScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = this.thisTransform.localScale;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
			return;
		}
		if (this.tweenArguments.Contains("x"))
		{
			this.vector3s[1].x = (float)this.tweenArguments["x"];
		}
		if (this.tweenArguments.Contains("y"))
		{
			this.vector3s[1].y = (float)this.tweenArguments["y"];
		}
		if (this.tweenArguments.Contains("z"))
		{
			this.vector3s[1].z = (float)this.tweenArguments["z"];
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0000624C File Offset: 0x0000444C
	private void GenerateShakeRotationTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.isLocal ? this.thisTransform.localEulerAngles : this.thisTransform.eulerAngles);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
			return;
		}
		if (this.tweenArguments.Contains("x"))
		{
			this.vector3s[1].x = (float)this.tweenArguments["x"];
		}
		if (this.tweenArguments.Contains("y"))
		{
			this.vector3s[1].y = (float)this.tweenArguments["y"];
		}
		if (this.tweenArguments.Contains("z"))
		{
			this.vector3s[1].z = (float)this.tweenArguments["z"];
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00006370 File Offset: 0x00004570
	private void GeneratePunchPositionTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[4] = this.thisTransform.eulerAngles;
		this.vector3s[0] = this.thisTransform.position;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
			return;
		}
		if (this.tweenArguments.Contains("x"))
		{
			this.vector3s[1].x = (float)this.tweenArguments["x"];
		}
		if (this.tweenArguments.Contains("y"))
		{
			this.vector3s[1].y = (float)this.tweenArguments["y"];
		}
		if (this.tweenArguments.Contains("z"))
		{
			this.vector3s[1].z = (float)this.tweenArguments["z"];
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000064B4 File Offset: 0x000046B4
	private void GeneratePunchRotationTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = this.thisTransform.eulerAngles;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
			return;
		}
		if (this.tweenArguments.Contains("x"))
		{
			this.vector3s[1].x = (float)this.tweenArguments["x"];
		}
		if (this.tweenArguments.Contains("y"))
		{
			this.vector3s[1].y = (float)this.tweenArguments["y"];
		}
		if (this.tweenArguments.Contains("z"))
		{
			this.vector3s[1].z = (float)this.tweenArguments["z"];
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000065E0 File Offset: 0x000047E0
	private void GeneratePunchScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = this.thisTransform.localScale;
		this.vector3s[1] = Vector3.zero;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
			return;
		}
		if (this.tweenArguments.Contains("x"))
		{
			this.vector3s[1].x = (float)this.tweenArguments["x"];
		}
		if (this.tweenArguments.Contains("y"))
		{
			this.vector3s[1].y = (float)this.tweenArguments["y"];
		}
		if (this.tweenArguments.Contains("z"))
		{
			this.vector3s[1].z = (float)this.tweenArguments["z"];
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00006700 File Offset: 0x00004900
	private void ApplyRectTargets()
	{
		this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
		this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
		this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
		this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
		this.tweenArguments["onupdateparams"] = this.rects[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.rects[1];
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0000686C File Offset: 0x00004A6C
	private void ApplyColorTargets()
	{
		this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
		this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
		this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
		this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
		this.tweenArguments["onupdateparams"] = this.colors[0, 2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.colors[0, 1];
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000069E8 File Offset: 0x00004BE8
	private void ApplyVector3Targets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector3s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector3s[1];
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00006B10 File Offset: 0x00004D10
	private void ApplyVector2Targets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector2s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector2s[1];
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00006BF4 File Offset: 0x00004DF4
	private void ApplyFloatTargets()
	{
		this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
		this.tweenArguments["onupdateparams"] = this.floats[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.floats[1];
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00006C74 File Offset: 0x00004E74
	private void ApplyColorToTargets()
	{
		for (int i = 0; i < this.colors.GetLength(0); i++)
		{
			this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
			this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
			this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
			this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
		}
		if (base.GetComponent<Renderer>())
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				base.GetComponent<Renderer>().materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
			}
		}
		else if (base.GetComponent<Light>())
		{
			base.GetComponent<Light>().color = this.colors[0, 2];
		}
		if (this.percentage == 1f)
		{
			if (base.GetComponent<Renderer>())
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					base.GetComponent<Renderer>().materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
				}
				return;
			}
			if (base.GetComponent<Light>())
			{
				base.GetComponent<Light>().color = this.colors[0, 1];
			}
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00006EBC File Offset: 0x000050BC
	private void ApplyAudioToTargets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.audioSource.volume = this.vector2s[2].x;
		this.audioSource.pitch = this.vector2s[2].y;
		if (this.percentage == 1f)
		{
			this.audioSource.volume = this.vector2s[1].x;
			this.audioSource.pitch = this.vector2s[1].y;
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00006FCE File Offset: 0x000051CE
	private void ApplyStabTargets()
	{
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00006FD0 File Offset: 0x000051D0
	private void ApplyMoveToPathTargets()
	{
		this.preUpdate = this.thisTransform.position;
		float value = this.ease(0f, 1f, this.percentage);
		if (this.isLocal)
		{
			this.thisTransform.localPosition = this.path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		else
		{
			this.thisTransform.position = this.path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			float num;
			if (this.tweenArguments.Contains("lookahead"))
			{
				num = (float)this.tweenArguments["lookahead"];
			}
			else
			{
				num = iTween.Defaults.lookAhead;
			}
			float value2 = this.ease(0f, 1f, Mathf.Min(1f, this.percentage + num));
			this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(value2, 0f, 1f));
		}
		this.postUpdate = this.thisTransform.position;
		if (this.physics)
		{
			this.thisTransform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00007154 File Offset: 0x00005354
	private void ApplyMoveToTargets()
	{
		this.preUpdate = this.thisTransform.position;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			this.thisTransform.localPosition = this.vector3s[2];
		}
		else
		{
			this.thisTransform.position = this.vector3s[2];
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				this.thisTransform.localPosition = this.vector3s[1];
			}
			else
			{
				this.thisTransform.position = this.vector3s[1];
			}
		}
		this.postUpdate = this.thisTransform.position;
		if (this.physics)
		{
			this.thisTransform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000072F8 File Offset: 0x000054F8
	private void ApplyMoveByTargets()
	{
		this.preUpdate = this.thisTransform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = this.thisTransform.eulerAngles;
			this.thisTransform.eulerAngles = this.vector3s[4];
		}
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.thisTransform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			this.thisTransform.eulerAngles = eulerAngles;
		}
		this.postUpdate = this.thisTransform.position;
		if (this.physics)
		{
			this.thisTransform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x000074C0 File Offset: 0x000056C0
	private void ApplyScaleToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.thisTransform.localScale = this.vector3s[2];
		if (this.percentage == 1f)
		{
			this.thisTransform.localScale = this.vector3s[1];
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x000075D4 File Offset: 0x000057D4
	private void ApplyLookToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[2]);
			return;
		}
		this.thisTransform.rotation = Quaternion.Euler(this.vector3s[2]);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000076F0 File Offset: 0x000058F0
	private void ApplyRotateToTargets()
	{
		this.preUpdate = this.thisTransform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			this.thisTransform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				this.thisTransform.localRotation = Quaternion.Euler(this.vector3s[1]);
			}
			else
			{
				this.thisTransform.rotation = Quaternion.Euler(this.vector3s[1]);
			}
		}
		this.postUpdate = this.thisTransform.eulerAngles;
		if (this.physics)
		{
			this.thisTransform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000078AC File Offset: 0x00005AAC
	private void ApplyRotateAddTargets()
	{
		this.preUpdate = this.thisTransform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.thisTransform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = this.thisTransform.eulerAngles;
		if (this.physics)
		{
			this.thisTransform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00007A1C File Offset: 0x00005C1C
	private void ApplyShakePositionTargets()
	{
		if (this.isLocal)
		{
			this.preUpdate = this.thisTransform.localPosition;
		}
		else
		{
			this.preUpdate = this.thisTransform.position;
		}
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = this.thisTransform.eulerAngles;
			this.thisTransform.eulerAngles = this.vector3s[3];
		}
		if (this.percentage == 0f)
		{
			this.thisTransform.Translate(this.vector3s[1], this.space);
		}
		if (this.isLocal)
		{
			this.thisTransform.localPosition = this.vector3s[0];
		}
		else
		{
			this.thisTransform.position = this.vector3s[0];
		}
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		if (this.isLocal)
		{
			this.thisTransform.localPosition += this.vector3s[2];
		}
		else
		{
			this.thisTransform.position += this.vector3s[2];
		}
		if (this.tweenArguments.Contains("looktarget"))
		{
			this.thisTransform.eulerAngles = eulerAngles;
		}
		this.postUpdate = this.thisTransform.position;
		if (this.physics)
		{
			this.thisTransform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00007C60 File Offset: 0x00005E60
	private void ApplyShakeScaleTargets()
	{
		if (this.percentage == 0f)
		{
			this.thisTransform.localScale = this.vector3s[1];
		}
		this.thisTransform.localScale = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		this.thisTransform.localScale += this.vector3s[2];
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00007D90 File Offset: 0x00005F90
	private void ApplyShakeRotationTargets()
	{
		this.preUpdate = (this.isLocal ? this.thisTransform.localEulerAngles : this.thisTransform.eulerAngles);
		if (this.percentage == 0f)
		{
			this.thisTransform.Rotate(this.vector3s[1], this.space);
		}
		if (this.isLocal)
		{
			this.thisTransform.localEulerAngles = this.vector3s[0];
		}
		else
		{
			this.thisTransform.eulerAngles = this.vector3s[0];
		}
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		this.thisTransform.Rotate(this.vector3s[2], this.space);
		this.postUpdate = (this.isLocal ? this.thisTransform.localEulerAngles : this.thisTransform.eulerAngles);
		if (this.physics)
		{
			if (this.isLocal)
			{
				this.thisTransform.localEulerAngles = this.preUpdate;
			}
			else
			{
				this.thisTransform.eulerAngles = this.preUpdate;
			}
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00007F78 File Offset: 0x00006178
	private void ApplyPunchPositionTargets()
	{
		this.preUpdate = this.thisTransform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = this.thisTransform.eulerAngles;
			this.thisTransform.eulerAngles = this.vector3s[4];
		}
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		this.thisTransform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			this.thisTransform.eulerAngles = eulerAngles;
		}
		this.postUpdate = this.thisTransform.position;
		if (this.physics)
		{
			this.thisTransform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00008230 File Offset: 0x00006430
	private void ApplyPunchRotationTargets()
	{
		this.preUpdate = this.thisTransform.eulerAngles;
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		this.thisTransform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = this.thisTransform.eulerAngles;
		if (this.physics)
		{
			this.thisTransform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00008490 File Offset: 0x00006690
	private void ApplyPunchScaleTargets()
	{
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		this.thisTransform.localScale = this.vector3s[0] + this.vector3s[2];
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00008681 File Offset: 0x00006881
	private IEnumerator TweenDelay()
	{
		this.delayStarted = Time.time;
		yield return new WaitForSeconds(this.delay);
		if (this.wasPaused)
		{
			this.wasPaused = false;
			this.TweenStart();
		}
		yield break;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00008690 File Offset: 0x00006890
	private void TweenStart()
	{
		this.CallBack("onstart");
		if (!this.loop)
		{
			this.ConflictCheck();
			this.GenerateTargets();
		}
		if (this.type == "stab")
		{
			this.audioSource.PlayOneShot(this.audioSource.clip);
		}
		if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
		{
			this.EnableKinematic();
		}
		this.isRunning = true;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0000876F File Offset: 0x0000696F
	private IEnumerator TweenRestart()
	{
		if (this.delay > 0f)
		{
			this.delayStarted = Time.time;
			yield return new WaitForSeconds(this.delay);
		}
		this.loop = true;
		this.TweenStart();
		yield break;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0000877E File Offset: 0x0000697E
	private void TweenUpdate()
	{
		this.apply();
		this.CallBack("onupdate");
		this.UpdatePercentage();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0000879C File Offset: 0x0000699C
	private void TweenComplete()
	{
		this.isRunning = false;
		if (this.percentage > 0.5f)
		{
			this.percentage = 1f;
		}
		else
		{
			this.percentage = 0f;
		}
		this.apply();
		if (this.type == "value")
		{
			this.CallBack("onupdate");
		}
		if (this.loopType == iTween.LoopType.none)
		{
			this.Dispose();
		}
		else
		{
			this.TweenLoop();
		}
		this.CallBack("oncomplete");
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00008820 File Offset: 0x00006A20
	private void TweenLoop()
	{
		this.DisableKinematic();
		iTween.LoopType loopType = this.loopType;
		if (loopType == iTween.LoopType.loop)
		{
			this.percentage = 0f;
			this.runningTime = 0f;
			this.apply();
			base.StartCoroutine("TweenRestart");
			return;
		}
		if (loopType != iTween.LoopType.pingPong)
		{
			return;
		}
		this.reverse = !this.reverse;
		this.runningTime = 0f;
		base.StartCoroutine("TweenRestart");
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00008898 File Offset: 0x00006A98
	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		return new Rect(iTween.FloatUpdate(currentValue.x, targetValue.x, speed), iTween.FloatUpdate(currentValue.y, targetValue.y, speed), iTween.FloatUpdate(currentValue.width, targetValue.width, speed), iTween.FloatUpdate(currentValue.height, targetValue.height, speed));
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000088FC File Offset: 0x00006AFC
	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 a = targetValue - currentValue;
		currentValue += a * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0000892C File Offset: 0x00006B2C
	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 a = targetValue - currentValue;
		currentValue += a * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0000895C File Offset: 0x00006B5C
	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000897B File Offset: 0x00006B7B
	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		iTween.ColorUpdate(target, args);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000899A File Offset: 0x00006B9A
	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		iTween.FadeUpdate(target, iTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000089D0 File Offset: 0x00006BD0
	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (object obj in target.transform)
			{
				iTween.ColorUpdate(((Transform)obj).gameObject, args);
			}
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		if (target.GetComponent<Renderer>())
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = array[3];
			return;
		}
		if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00008CD0 File Offset: 0x00006ED0
	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		iTween.ColorUpdate(target, iTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00008D08 File Offset: 0x00006F08
	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent<AudioSource>())
			{
				Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00008E98 File Offset: 0x00007098
	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioUpdate(target, iTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00008EEC File Offset: 0x000070EC
	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000911C File Offset: 0x0000731C
	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateUpdate(target, iTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00009154 File Offset: 0x00007354
	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00009379 File Offset: 0x00007579
	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleUpdate(target, iTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000093B0 File Offset: 0x000075B0
	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			iTween.LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000096C3 File Offset: 0x000078C3
	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		iTween.MoveUpdate(target, iTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000096F8 File Offset: 0x000078F8
	public static void LookUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				target.transform.LookAt((Transform)args["looktarget"], ((Vector3?)args["up"]) ?? iTween.Defaults.up);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				target.transform.LookAt((Vector3)args["looktarget"], ((Vector3?)args["up"]) ?? iTween.Defaults.up);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				string a = (string)args["axis"];
				if (!(a == "x"))
				{
					if (!(a == "y"))
					{
						if (a == "z")
						{
							array[4].x = array[0].x;
							array[4].y = array[0].y;
						}
					}
					else
					{
						array[4].x = array[0].x;
						array[4].z = array[0].z;
					}
				}
				else
				{
					array[4].y = array[0].y;
					array[4].z = array[0].z;
				}
				target.transform.eulerAngles = array[4];
			}
			return;
		}
		Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00009A45 File Offset: 0x00007C45
	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookUpdate(target, iTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00009A7C File Offset: 0x00007C7C
	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = iTween.PathControlPointGenerator(array);
		Vector3 a = iTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 vector = iTween.Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00009B0C File Offset: 0x00007D0C
	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] pts = iTween.PathControlPointGenerator(path);
		Vector3 a = iTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector = iTween.Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00009B6A File Offset: 0x00007D6A
	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00009B83 File Offset: 0x00007D83
	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00009B98 File Offset: 0x00007D98
	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00009BE4 File Offset: 0x00007DE4
	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00009C2C File Offset: 0x00007E2C
	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00009C6B File Offset: 0x00007E6B
	public static void DrawLine(Vector3[] line)
	{
		if (line.Length != 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00009C81 File Offset: 0x00007E81
	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			iTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00009C94 File Offset: 0x00007E94
	public static void DrawLine(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00009CDC File Offset: 0x00007EDC
	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00009D1F File Offset: 0x00007F1F
	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length != 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00009D35 File Offset: 0x00007F35
	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			iTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00009D48 File Offset: 0x00007F48
	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00009D90 File Offset: 0x00007F90
	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00009DD3 File Offset: 0x00007FD3
	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length != 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00009DE9 File Offset: 0x00007FE9
	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			iTween.DrawLineHelper(line, color, "handles");
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00009DFC File Offset: 0x00007FFC
	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00009E44 File Offset: 0x00008044
	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "handles");
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00009E87 File Offset: 0x00008087
	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00009E95 File Offset: 0x00008095
	public static void DrawPath(Vector3[] path)
	{
		if (path.Length != 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00009EAB File Offset: 0x000080AB
	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			iTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00009EC0 File Offset: 0x000080C0
	public static void DrawPath(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00009F08 File Offset: 0x00008108
	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00009F4B File Offset: 0x0000814B
	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length != 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00009F61 File Offset: 0x00008161
	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			iTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00009F74 File Offset: 0x00008174
	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00009FBC File Offset: 0x000081BC
	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00009FFF File Offset: 0x000081FF
	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length != 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000A015 File Offset: 0x00008215
	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			iTween.DrawPathHelper(path, color, "handles");
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0000A028 File Offset: 0x00008228
	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000A070 File Offset: 0x00008270
	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "handles");
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000A0B4 File Offset: 0x000082B4
	public static void Resume(GameObject target)
	{
		Component[] array = target.GetComponents<iTween>();
		array = array;
		for (int i = 0; i < array.Length; i++)
		{
			((iTween)array[i]).enabled = true;
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000A0E8 File Offset: 0x000082E8
	public static void Resume(GameObject target, bool includechildren)
	{
		iTween.Resume(target);
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.Resume(((Transform)obj).gameObject, true);
			}
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000A150 File Offset: 0x00008350
	public static void Resume(GameObject target, string type)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween.enabled = true;
			}
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000A1B4 File Offset: 0x000083B4
	public static void Resume(GameObject target, string type, bool includechildren)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween.enabled = true;
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.Resume(((Transform)obj).gameObject, type, true);
			}
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x0000A270 File Offset: 0x00008470
	public static void Resume()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			iTween.Resume((GameObject)iTween.tweens[i]["target"]);
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0000A2B4 File Offset: 0x000084B4
	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			GameObject value = (GameObject)iTween.tweens[i]["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Resume((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000A328 File Offset: 0x00008528
	public static void Pause(GameObject target)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if (iTween.delay > 0f)
			{
				iTween.delay -= Time.time - iTween.delayStarted;
				iTween.StopCoroutine("TweenDelay");
			}
			iTween.isPaused = true;
			iTween.enabled = false;
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000A394 File Offset: 0x00008594
	public static void Pause(GameObject target, bool includechildren)
	{
		iTween.Pause(target);
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.Pause(((Transform)obj).gameObject, true);
			}
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000A3FC File Offset: 0x000085FC
	public static void Pause(GameObject target, string type)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween.delay > 0f)
				{
					iTween.delay -= Time.time - iTween.delayStarted;
					iTween.StopCoroutine("TweenDelay");
				}
				iTween.isPaused = true;
				iTween.enabled = false;
			}
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000A49C File Offset: 0x0000869C
	public static void Pause(GameObject target, string type, bool includechildren)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween.delay > 0f)
				{
					iTween.delay -= Time.time - iTween.delayStarted;
					iTween.StopCoroutine("TweenDelay");
				}
				iTween.isPaused = true;
				iTween.enabled = false;
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.Pause(((Transform)obj).gameObject, type, true);
			}
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000A590 File Offset: 0x00008790
	public static void Pause()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			iTween.Pause((GameObject)iTween.tweens[i]["target"]);
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000A5D4 File Offset: 0x000087D4
	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			GameObject value = (GameObject)iTween.tweens[i]["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Pause((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0000A647 File Offset: 0x00008847
	public static int Count()
	{
		return iTween.tweens.Count;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000A654 File Offset: 0x00008854
	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = iTween.tweens[i];
			if (((string)hashtable["type"] + (string)hashtable["method"]).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000A6CC File Offset: 0x000088CC
	public static int Count(GameObject target)
	{
		Component[] components = target.GetComponents<iTween>();
		return components.Length;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000A6E4 File Offset: 0x000088E4
	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000A748 File Offset: 0x00008948
	public static void Stop()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			iTween.Stop((GameObject)iTween.tweens[i]["target"]);
		}
		iTween.tweens.Clear();
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000A794 File Offset: 0x00008994
	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			GameObject value = (GameObject)iTween.tweens[i]["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Stop((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000A808 File Offset: 0x00008A08
	public static void StopByName(string name)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			GameObject value = (GameObject)iTween.tweens[i]["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.StopByName((GameObject)arrayList[j], name);
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000A87C File Offset: 0x00008A7C
	public static void Stop(GameObject target)
	{
		Component[] array = target.GetComponents<iTween>();
		array = array;
		for (int i = 0; i < array.Length; i++)
		{
			((iTween)array[i]).Dispose();
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000A8B0 File Offset: 0x00008AB0
	public static void Stop(GameObject target, bool includechildren)
	{
		iTween.Stop(target);
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.Stop(((Transform)obj).gameObject, true);
			}
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000A918 File Offset: 0x00008B18
	public static void Stop(GameObject target, string type)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween.Dispose();
			}
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000A97C File Offset: 0x00008B7C
	public static void StopByName(GameObject target, string name)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if (iTween._name == name)
			{
				iTween.Dispose();
			}
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000A9C0 File Offset: 0x00008BC0
	public static void Stop(GameObject target, string type, bool includechildren)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if ((iTween.type + iTween.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween.Dispose();
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.Stop(((Transform)obj).gameObject, type, true);
			}
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0000AA78 File Offset: 0x00008C78
	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		Component[] array = target.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if (iTween._name == name)
			{
				iTween.Dispose();
			}
		}
		if (includechildren)
		{
			foreach (object obj in target.transform)
			{
				iTween.StopByName(((Transform)obj).gameObject, name, true);
			}
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0000AB10 File Offset: 0x00008D10
	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError("Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x0000AB5A File Offset: 0x00008D5A
	private iTween(Hashtable h)
	{
		this.tweenArguments = h;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0000AB69 File Offset: 0x00008D69
	private void Awake()
	{
		this.thisTransform = base.transform;
		this.RetrieveArgs();
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000AB88 File Offset: 0x00008D88
	private IEnumerator Start()
	{
		if (this.delay > 0f)
		{
			yield return base.StartCoroutine("TweenDelay");
		}
		this.TweenStart();
		yield break;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000AB98 File Offset: 0x00008D98
	private void Update()
	{
		if (this.isRunning && !this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
					return;
				}
				this.TweenComplete();
				return;
			}
			else
			{
				if (this.percentage > 0f)
				{
					this.TweenUpdate();
					return;
				}
				this.TweenComplete();
			}
		}
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000ABF4 File Offset: 0x00008DF4
	private void FixedUpdate()
	{
		if (this.isRunning && this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
					return;
				}
				this.TweenComplete();
				return;
			}
			else
			{
				if (this.percentage > 0f)
				{
					this.TweenUpdate();
					return;
				}
				this.TweenComplete();
			}
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000AC50 File Offset: 0x00008E50
	private void LateUpdate()
	{
		if (this.tweenArguments.Contains("looktarget") && this.isRunning && (this.type == "move" || this.type == "shake" || this.type == "punch"))
		{
			iTween.LookUpdate(base.gameObject, this.tweenArguments);
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000ACBE File Offset: 0x00008EBE
	private void OnEnable()
	{
		if (this.isRunning)
		{
			this.EnableKinematic();
		}
		if (this.isPaused)
		{
			this.isPaused = false;
			if (this.delay > 0f)
			{
				this.wasPaused = true;
				this.ResumeDelay();
			}
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000ACF7 File Offset: 0x00008EF7
	private void OnDisable()
	{
		this.DisableKinematic();
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000AD00 File Offset: 0x00008F00
	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0000AD60 File Offset: 0x00008F60
	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] pts = iTween.PathControlPointGenerator(path);
		Vector3 to = iTween.Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector = iTween.Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector, to);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			to = vector;
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000ADDC File Offset: 0x00008FDC
	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000AEC4 File Offset: 0x000090C4
	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 a = pts[num2];
		Vector3 a2 = pts[num2 + 1];
		Vector3 vector = pts[num2 + 2];
		Vector3 b = pts[num2 + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0000AFC8 File Offset: 0x000091C8
	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = iTween.GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		iTween.tweens.Insert(0, args);
		target.AddComponent<iTween>();
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0000B020 File Offset: 0x00009220
	private static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		foreach (object obj in args)
		{
			DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
			hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
		}
		foreach (object obj2 in hashtable)
		{
			DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
			if (dictionaryEntry2.Value.GetType() == typeof(int))
			{
				float num = (float)((int)dictionaryEntry2.Value);
				args[dictionaryEntry2.Key] = num;
			}
			if (dictionaryEntry2.Value.GetType() == typeof(double))
			{
				float num2 = (float)((double)dictionaryEntry2.Value);
				args[dictionaryEntry2.Key] = num2;
			}
		}
		foreach (object obj3 in args)
		{
			DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
			hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value);
		}
		args = hashtable2;
		return args;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000B1C4 File Offset: 0x000093C4
	private static string GenerateID()
	{
		return Guid.NewGuid().ToString();
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0000B1E4 File Offset: 0x000093E4
	private void RetrieveArgs()
	{
		foreach (Hashtable hashtable in iTween.tweens)
		{
			if ((GameObject)hashtable["target"] == base.gameObject)
			{
				this.tweenArguments = hashtable;
				break;
			}
		}
		this.id = (string)this.tweenArguments["id"];
		this.type = (string)this.tweenArguments["type"];
		this._name = (string)this.tweenArguments["name"];
		this.method = (string)this.tweenArguments["method"];
		if (this.tweenArguments.Contains("time"))
		{
			this.time = (float)this.tweenArguments["time"];
		}
		else
		{
			this.time = iTween.Defaults.time;
		}
		if (base.GetComponent<Rigidbody>() != null)
		{
			this.physics = true;
		}
		if (this.tweenArguments.Contains("delay"))
		{
			this.delay = (float)this.tweenArguments["delay"];
		}
		else
		{
			this.delay = iTween.Defaults.delay;
		}
		if (this.tweenArguments.Contains("namedcolorvalue"))
		{
			if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(iTween.NamedValueColor))
			{
				this.namedcolorvalue = (iTween.NamedValueColor)this.tweenArguments["namedcolorvalue"];
				goto IL_1F3;
			}
			try
			{
				this.namedcolorvalue = (iTween.NamedValueColor)Enum.Parse(typeof(iTween.NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true);
				goto IL_1F3;
			}
			catch
			{
				Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
				this.namedcolorvalue = iTween.NamedValueColor._Color;
				goto IL_1F3;
			}
		}
		this.namedcolorvalue = iTween.Defaults.namedColorValue;
		IL_1F3:
		if (this.tweenArguments.Contains("looptype"))
		{
			if (this.tweenArguments["looptype"].GetType() == typeof(iTween.LoopType))
			{
				this.loopType = (iTween.LoopType)this.tweenArguments["looptype"];
				goto IL_299;
			}
			try
			{
				this.loopType = (iTween.LoopType)Enum.Parse(typeof(iTween.LoopType), (string)this.tweenArguments["looptype"], true);
				goto IL_299;
			}
			catch
			{
				Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
				this.loopType = iTween.LoopType.none;
				goto IL_299;
			}
		}
		this.loopType = iTween.LoopType.none;
		IL_299:
		if (this.tweenArguments.Contains("easetype"))
		{
			if (this.tweenArguments["easetype"].GetType() == typeof(iTween.EaseType))
			{
				this.easeType = (iTween.EaseType)this.tweenArguments["easetype"];
				goto IL_347;
			}
			try
			{
				this.easeType = (iTween.EaseType)Enum.Parse(typeof(iTween.EaseType), (string)this.tweenArguments["easetype"], true);
				goto IL_347;
			}
			catch
			{
				Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
				this.easeType = iTween.Defaults.easeType;
				goto IL_347;
			}
		}
		this.easeType = iTween.Defaults.easeType;
		IL_347:
		if (this.tweenArguments.Contains("space"))
		{
			if (this.tweenArguments["space"].GetType() == typeof(Space))
			{
				this.space = (Space)this.tweenArguments["space"];
				goto IL_3F5;
			}
			try
			{
				this.space = (Space)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true);
				goto IL_3F5;
			}
			catch
			{
				Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
				this.space = iTween.Defaults.space;
				goto IL_3F5;
			}
		}
		this.space = iTween.Defaults.space;
		IL_3F5:
		if (this.tweenArguments.Contains("islocal"))
		{
			this.isLocal = (bool)this.tweenArguments["islocal"];
		}
		else
		{
			this.isLocal = iTween.Defaults.isLocal;
		}
		if (this.tweenArguments.Contains("ignoretimescale"))
		{
			this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
		}
		else
		{
			this.useRealTime = iTween.Defaults.useRealTime;
		}
		this.GetEasingFunction();
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000B6A0 File Offset: 0x000098A0
	private void GetEasingFunction()
	{
		switch (this.easeType)
		{
		case iTween.EaseType.easeInQuad:
			this.ease = new iTween.EasingFunction(this.easeInQuad);
			return;
		case iTween.EaseType.easeOutQuad:
			this.ease = new iTween.EasingFunction(this.easeOutQuad);
			return;
		case iTween.EaseType.easeInOutQuad:
			this.ease = new iTween.EasingFunction(this.easeInOutQuad);
			return;
		case iTween.EaseType.easeInCubic:
			this.ease = new iTween.EasingFunction(this.easeInCubic);
			return;
		case iTween.EaseType.easeOutCubic:
			this.ease = new iTween.EasingFunction(this.easeOutCubic);
			return;
		case iTween.EaseType.easeInOutCubic:
			this.ease = new iTween.EasingFunction(this.easeInOutCubic);
			return;
		case iTween.EaseType.easeInQuart:
			this.ease = new iTween.EasingFunction(this.easeInQuart);
			return;
		case iTween.EaseType.easeOutQuart:
			this.ease = new iTween.EasingFunction(this.easeOutQuart);
			return;
		case iTween.EaseType.easeInOutQuart:
			this.ease = new iTween.EasingFunction(this.easeInOutQuart);
			return;
		case iTween.EaseType.easeInQuint:
			this.ease = new iTween.EasingFunction(this.easeInQuint);
			return;
		case iTween.EaseType.easeOutQuint:
			this.ease = new iTween.EasingFunction(this.easeOutQuint);
			return;
		case iTween.EaseType.easeInOutQuint:
			this.ease = new iTween.EasingFunction(this.easeInOutQuint);
			return;
		case iTween.EaseType.easeInSine:
			this.ease = new iTween.EasingFunction(this.easeInSine);
			return;
		case iTween.EaseType.easeOutSine:
			this.ease = new iTween.EasingFunction(this.easeOutSine);
			return;
		case iTween.EaseType.easeInOutSine:
			this.ease = new iTween.EasingFunction(this.easeInOutSine);
			return;
		case iTween.EaseType.easeInExpo:
			this.ease = new iTween.EasingFunction(this.easeInExpo);
			return;
		case iTween.EaseType.easeOutExpo:
			this.ease = new iTween.EasingFunction(this.easeOutExpo);
			return;
		case iTween.EaseType.easeInOutExpo:
			this.ease = new iTween.EasingFunction(this.easeInOutExpo);
			return;
		case iTween.EaseType.easeInCirc:
			this.ease = new iTween.EasingFunction(this.easeInCirc);
			return;
		case iTween.EaseType.easeOutCirc:
			this.ease = new iTween.EasingFunction(this.easeOutCirc);
			return;
		case iTween.EaseType.easeInOutCirc:
			this.ease = new iTween.EasingFunction(this.easeInOutCirc);
			return;
		case iTween.EaseType.linear:
			this.ease = new iTween.EasingFunction(this.linear);
			return;
		case iTween.EaseType.spring:
			this.ease = new iTween.EasingFunction(this.spring);
			return;
		case iTween.EaseType.easeInBounce:
			this.ease = new iTween.EasingFunction(this.easeInBounce);
			return;
		case iTween.EaseType.easeOutBounce:
			this.ease = new iTween.EasingFunction(this.easeOutBounce);
			return;
		case iTween.EaseType.easeInOutBounce:
			this.ease = new iTween.EasingFunction(this.easeInOutBounce);
			return;
		case iTween.EaseType.easeInBack:
			this.ease = new iTween.EasingFunction(this.easeInBack);
			return;
		case iTween.EaseType.easeOutBack:
			this.ease = new iTween.EasingFunction(this.easeOutBack);
			return;
		case iTween.EaseType.easeInOutBack:
			this.ease = new iTween.EasingFunction(this.easeInOutBack);
			return;
		case iTween.EaseType.easeInElastic:
			this.ease = new iTween.EasingFunction(this.easeInElastic);
			return;
		case iTween.EaseType.easeOutElastic:
			this.ease = new iTween.EasingFunction(this.easeOutElastic);
			return;
		case iTween.EaseType.easeInOutElastic:
			this.ease = new iTween.EasingFunction(this.easeInOutElastic);
			return;
		default:
			return;
		}
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000B99C File Offset: 0x00009B9C
	private void UpdatePercentage()
	{
		if (this.useRealTime)
		{
			this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
		}
		else
		{
			this.runningTime += Time.deltaTime;
		}
		if (this.reverse)
		{
			this.percentage = 1f - this.runningTime / this.time;
		}
		else
		{
			this.percentage = this.runningTime / this.time;
		}
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0000BA20 File Offset: 0x00009C20
	private void CallBack(string callbackType)
	{
		if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
		{
			GameObject gameObject;
			if (this.tweenArguments.Contains(callbackType + "target"))
			{
				gameObject = (GameObject)this.tweenArguments[callbackType + "target"];
			}
			else
			{
				gameObject = base.gameObject;
			}
			if (this.tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
				return;
			}
			Debug.LogError("iTween Error: Callback method references must be passed as a String!");
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000BAF4 File Offset: 0x00009CF4
	private void Dispose()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			if ((string)iTween.tweens[i]["id"] == this.id)
			{
				iTween.tweens.RemoveAt(i);
				break;
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0000BB50 File Offset: 0x00009D50
	private void ConflictCheck()
	{
		Component[] array = base.GetComponents<iTween>();
		foreach (iTween iTween in array)
		{
			if (iTween.type == "value")
			{
				return;
			}
			if (iTween.isRunning && iTween.type == this.type)
			{
				if (iTween.method != this.method)
				{
					return;
				}
				if (iTween.tweenArguments.Count != this.tweenArguments.Count)
				{
					iTween.Dispose();
					return;
				}
				foreach (object obj in this.tweenArguments)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (!iTween.tweenArguments.Contains(dictionaryEntry.Key))
					{
						iTween.Dispose();
						return;
					}
					if (!iTween.tweenArguments[dictionaryEntry.Key].Equals(this.tweenArguments[dictionaryEntry.Key]) && (string)dictionaryEntry.Key != "id")
					{
						iTween.Dispose();
						return;
					}
				}
				this.Dispose();
			}
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x0000BCA0 File Offset: 0x00009EA0
	private void EnableKinematic()
	{
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000BCA2 File Offset: 0x00009EA2
	private void DisableKinematic()
	{
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0000BCA4 File Offset: 0x00009EA4
	private void ResumeDelay()
	{
		base.StartCoroutine("TweenDelay");
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0000BCB2 File Offset: 0x00009EB2
	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000BCBC File Offset: 0x00009EBC
	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) * 0.5f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x0000BD28 File Offset: 0x00009F28
	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000BD8C File Offset: 0x00009F8C
	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000BD9A File Offset: 0x00009F9A
	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0000BDB0 File Offset: 0x00009FB0
	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return -end * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000BE04 File Offset: 0x0000A004
	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x0000BE14 File Offset: 0x0000A014
	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000BE34 File Offset: 0x0000A034
	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x0000BE85 File Offset: 0x0000A085
	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x0000BE97 File Offset: 0x0000A097
	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x0000BEBC File Offset: 0x0000A0BC
	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value + start;
		}
		value -= 2f;
		return -end * 0.5f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000BF12 File Offset: 0x0000A112
	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000BF26 File Offset: 0x0000A126
	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0000BF4C File Offset: 0x0000A14C
	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000BFA5 File Offset: 0x0000A1A5
	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value * 1.5707964f) + end + start;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0000BFBF File Offset: 0x0000A1BF
	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * 1.5707964f) + start;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000BFD6 File Offset: 0x0000A1D6
	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end * 0.5f * (Mathf.Cos(3.1415927f * value) - 1f) + start;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000BFFA File Offset: 0x0000A1FA
	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0000C01C File Offset: 0x0000A21C
	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000C040 File Offset: 0x0000A240
	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000C0F4 File Offset: 0x0000A2F4
	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000C160 File Offset: 0x0000A360
	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - this.easeOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000C18C File Offset: 0x0000A38C
	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0000C228 File Offset: 0x0000A428
	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return this.easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return this.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000C28C File Offset: 0x0000A48C
	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000C2C0 File Offset: 0x0000A4C0
	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000C2FC File Offset: 0x0000A4FC
	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000C378 File Offset: 0x0000A578
	private float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000C3EC File Offset: 0x0000A5EC
	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000C494 File Offset: 0x0000A694
	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x0000C534 File Offset: 0x0000A734
	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x04000001 RID: 1
	public static List<Hashtable> tweens = new List<Hashtable>();

	// Token: 0x04000002 RID: 2
	public string id;

	// Token: 0x04000003 RID: 3
	public string type;

	// Token: 0x04000004 RID: 4
	public string method;

	// Token: 0x04000005 RID: 5
	public iTween.EaseType easeType;

	// Token: 0x04000006 RID: 6
	public float time;

	// Token: 0x04000007 RID: 7
	public float delay;

	// Token: 0x04000008 RID: 8
	public iTween.LoopType loopType;

	// Token: 0x04000009 RID: 9
	public bool isRunning;

	// Token: 0x0400000A RID: 10
	public bool isPaused;

	// Token: 0x0400000B RID: 11
	public string _name;

	// Token: 0x0400000C RID: 12
	private float runningTime;

	// Token: 0x0400000D RID: 13
	private float percentage;

	// Token: 0x0400000E RID: 14
	private float delayStarted;

	// Token: 0x0400000F RID: 15
	private bool kinematic;

	// Token: 0x04000010 RID: 16
	private bool isLocal;

	// Token: 0x04000011 RID: 17
	private bool loop;

	// Token: 0x04000012 RID: 18
	private bool reverse;

	// Token: 0x04000013 RID: 19
	private bool wasPaused;

	// Token: 0x04000014 RID: 20
	private bool physics;

	// Token: 0x04000015 RID: 21
	private Hashtable tweenArguments;

	// Token: 0x04000016 RID: 22
	private Space space;

	// Token: 0x04000017 RID: 23
	private iTween.EasingFunction ease;

	// Token: 0x04000018 RID: 24
	private iTween.ApplyTween apply;

	// Token: 0x04000019 RID: 25
	private AudioSource audioSource;

	// Token: 0x0400001A RID: 26
	private Vector3[] vector3s;

	// Token: 0x0400001B RID: 27
	private Vector2[] vector2s;

	// Token: 0x0400001C RID: 28
	private Color[,] colors;

	// Token: 0x0400001D RID: 29
	private float[] floats;

	// Token: 0x0400001E RID: 30
	private Rect[] rects;

	// Token: 0x0400001F RID: 31
	private iTween.CRSpline path;

	// Token: 0x04000020 RID: 32
	private Vector3 preUpdate;

	// Token: 0x04000021 RID: 33
	private Vector3 postUpdate;

	// Token: 0x04000022 RID: 34
	private iTween.NamedValueColor namedcolorvalue;

	// Token: 0x04000023 RID: 35
	private float lastRealTime;

	// Token: 0x04000024 RID: 36
	private bool useRealTime;

	// Token: 0x04000025 RID: 37
	private Transform thisTransform;

	// Token: 0x02000202 RID: 514
	// (Invoke) Token: 0x060010C8 RID: 4296
	private delegate float EasingFunction(float start, float end, float Value);

	// Token: 0x02000203 RID: 515
	// (Invoke) Token: 0x060010CC RID: 4300
	private delegate void ApplyTween();

	// Token: 0x02000204 RID: 516
	public enum EaseType
	{
		// Token: 0x04000BFF RID: 3071
		easeInQuad,
		// Token: 0x04000C00 RID: 3072
		easeOutQuad,
		// Token: 0x04000C01 RID: 3073
		easeInOutQuad,
		// Token: 0x04000C02 RID: 3074
		easeInCubic,
		// Token: 0x04000C03 RID: 3075
		easeOutCubic,
		// Token: 0x04000C04 RID: 3076
		easeInOutCubic,
		// Token: 0x04000C05 RID: 3077
		easeInQuart,
		// Token: 0x04000C06 RID: 3078
		easeOutQuart,
		// Token: 0x04000C07 RID: 3079
		easeInOutQuart,
		// Token: 0x04000C08 RID: 3080
		easeInQuint,
		// Token: 0x04000C09 RID: 3081
		easeOutQuint,
		// Token: 0x04000C0A RID: 3082
		easeInOutQuint,
		// Token: 0x04000C0B RID: 3083
		easeInSine,
		// Token: 0x04000C0C RID: 3084
		easeOutSine,
		// Token: 0x04000C0D RID: 3085
		easeInOutSine,
		// Token: 0x04000C0E RID: 3086
		easeInExpo,
		// Token: 0x04000C0F RID: 3087
		easeOutExpo,
		// Token: 0x04000C10 RID: 3088
		easeInOutExpo,
		// Token: 0x04000C11 RID: 3089
		easeInCirc,
		// Token: 0x04000C12 RID: 3090
		easeOutCirc,
		// Token: 0x04000C13 RID: 3091
		easeInOutCirc,
		// Token: 0x04000C14 RID: 3092
		linear,
		// Token: 0x04000C15 RID: 3093
		spring,
		// Token: 0x04000C16 RID: 3094
		easeInBounce,
		// Token: 0x04000C17 RID: 3095
		easeOutBounce,
		// Token: 0x04000C18 RID: 3096
		easeInOutBounce,
		// Token: 0x04000C19 RID: 3097
		easeInBack,
		// Token: 0x04000C1A RID: 3098
		easeOutBack,
		// Token: 0x04000C1B RID: 3099
		easeInOutBack,
		// Token: 0x04000C1C RID: 3100
		easeInElastic,
		// Token: 0x04000C1D RID: 3101
		easeOutElastic,
		// Token: 0x04000C1E RID: 3102
		easeInOutElastic,
		// Token: 0x04000C1F RID: 3103
		punch
	}

	// Token: 0x02000205 RID: 517
	public enum LoopType
	{
		// Token: 0x04000C21 RID: 3105
		none,
		// Token: 0x04000C22 RID: 3106
		loop,
		// Token: 0x04000C23 RID: 3107
		pingPong
	}

	// Token: 0x02000206 RID: 518
	public enum NamedValueColor
	{
		// Token: 0x04000C25 RID: 3109
		_Color,
		// Token: 0x04000C26 RID: 3110
		_SpecColor,
		// Token: 0x04000C27 RID: 3111
		_Emission,
		// Token: 0x04000C28 RID: 3112
		_ReflectColor
	}

	// Token: 0x02000207 RID: 519
	public static class Defaults
	{
		// Token: 0x04000C29 RID: 3113
		public static float time = 1f;

		// Token: 0x04000C2A RID: 3114
		public static float delay = 0f;

		// Token: 0x04000C2B RID: 3115
		public static iTween.NamedValueColor namedColorValue = iTween.NamedValueColor._Color;

		// Token: 0x04000C2C RID: 3116
		public static iTween.LoopType loopType = iTween.LoopType.none;

		// Token: 0x04000C2D RID: 3117
		public static iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

		// Token: 0x04000C2E RID: 3118
		public static float lookSpeed = 3f;

		// Token: 0x04000C2F RID: 3119
		public static bool isLocal = false;

		// Token: 0x04000C30 RID: 3120
		public static Space space = Space.Self;

		// Token: 0x04000C31 RID: 3121
		public static bool orientToPath = false;

		// Token: 0x04000C32 RID: 3122
		public static Color color = Color.white;

		// Token: 0x04000C33 RID: 3123
		public static float updateTimePercentage = 0.05f;

		// Token: 0x04000C34 RID: 3124
		public static float updateTime = 1f * iTween.Defaults.updateTimePercentage;

		// Token: 0x04000C35 RID: 3125
		public static float lookAhead = 0.05f;

		// Token: 0x04000C36 RID: 3126
		public static bool useRealTime = false;

		// Token: 0x04000C37 RID: 3127
		public static Vector3 up = Vector3.up;
	}

	// Token: 0x02000208 RID: 520
	private class CRSpline
	{
		// Token: 0x060010D0 RID: 4304 RVA: 0x00036E52 File Offset: 0x00035052
		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00036E78 File Offset: 0x00035078
		public Vector3 Interp(float t)
		{
			int num = this.pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 a = this.pts[num2];
			Vector3 a2 = this.pts[num2 + 1];
			Vector3 vector = this.pts[num2 + 2];
			Vector3 b = this.pts[num2 + 3];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
		}

		// Token: 0x04000C38 RID: 3128
		public Vector3[] pts;
	}
}
