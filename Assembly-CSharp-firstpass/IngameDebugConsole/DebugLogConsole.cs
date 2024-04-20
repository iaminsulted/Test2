using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace IngameDebugConsole
{
	// Token: 0x020001E9 RID: 489
	public static class DebugLogConsole
	{
		// Token: 0x06000F3F RID: 3903 RVA: 0x0002C7EC File Offset: 0x0002A9EC
		static DebugLogConsole()
		{
			DebugLogConsole.parseFunctions = new Dictionary<Type, DebugLogConsole.ParseFunction>
			{
				{
					typeof(string),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseString)
				},
				{
					typeof(bool),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseBool)
				},
				{
					typeof(int),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseInt)
				},
				{
					typeof(uint),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseUInt)
				},
				{
					typeof(long),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseLong)
				},
				{
					typeof(ulong),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseULong)
				},
				{
					typeof(byte),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseByte)
				},
				{
					typeof(sbyte),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseSByte)
				},
				{
					typeof(short),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseShort)
				},
				{
					typeof(ushort),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseUShort)
				},
				{
					typeof(char),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseChar)
				},
				{
					typeof(float),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseFloat)
				},
				{
					typeof(double),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseDouble)
				},
				{
					typeof(decimal),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseDecimal)
				},
				{
					typeof(Vector2),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseVector2)
				},
				{
					typeof(Vector3),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseVector3)
				},
				{
					typeof(Vector4),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseVector4)
				},
				{
					typeof(GameObject),
					new DebugLogConsole.ParseFunction(DebugLogConsole.ParseGameObject)
				}
			};
			DebugLogConsole.typeReadableNames = new Dictionary<Type, string>
			{
				{
					typeof(string),
					"String"
				},
				{
					typeof(bool),
					"Boolean"
				},
				{
					typeof(int),
					"Integer"
				},
				{
					typeof(uint),
					"Unsigned Integer"
				},
				{
					typeof(long),
					"Long"
				},
				{
					typeof(ulong),
					"Unsigned Long"
				},
				{
					typeof(byte),
					"Byte"
				},
				{
					typeof(sbyte),
					"Short Byte"
				},
				{
					typeof(short),
					"Short"
				},
				{
					typeof(ushort),
					"Unsigned Short"
				},
				{
					typeof(char),
					"Char"
				},
				{
					typeof(float),
					"Float"
				},
				{
					typeof(double),
					"Double"
				},
				{
					typeof(decimal),
					"Decimal"
				},
				{
					typeof(Vector2),
					"Vector2"
				},
				{
					typeof(Vector3),
					"Vector3"
				},
				{
					typeof(Vector4),
					"Vector4"
				},
				{
					typeof(GameObject),
					"GameObject"
				}
			};
			HashSet<Assembly> hashSet = new HashSet<Assembly>
			{
				Assembly.GetAssembly(typeof(DebugLogConsole))
			};
			try
			{
				hashSet.Add(Assembly.Load("Assembly-CSharp"));
			}
			catch
			{
			}
			foreach (Assembly assembly in hashSet)
			{
				Type[] exportedTypes = assembly.GetExportedTypes();
				for (int i = 0; i < exportedTypes.Length; i++)
				{
					foreach (MethodInfo methodInfo in exportedTypes[i].GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public))
					{
						object[] customAttributes = methodInfo.GetCustomAttributes(typeof(ConsoleMethodAttribute), false);
						for (int k = 0; k < customAttributes.Length; k++)
						{
							ConsoleMethodAttribute consoleMethodAttribute = customAttributes[k] as ConsoleMethodAttribute;
							if (consoleMethodAttribute != null)
							{
								DebugLogConsole.AddCommand(consoleMethodAttribute.Command, consoleMethodAttribute.Description, methodInfo, null);
							}
						}
					}
				}
			}
			DebugLogConsole.AddCommandStatic("help", "Prints all commands", "LogAllCommands", typeof(DebugLogConsole));
			DebugLogConsole.AddCommandStatic("sysinfo", "Prints system information", "LogSystemInfo", typeof(DebugLogConsole));
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0002CD00 File Offset: 0x0002AF00
		public static void LogAllCommands()
		{
			int num = 25;
			foreach (KeyValuePair<string, ConsoleMethodInfo> keyValuePair in DebugLogConsole.methods)
			{
				if (keyValuePair.Value.IsValid())
				{
					num += 3 + keyValuePair.Value.signature.Length;
				}
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			stringBuilder.Append("Available commands:");
			foreach (KeyValuePair<string, ConsoleMethodInfo> keyValuePair2 in DebugLogConsole.methods)
			{
				if (keyValuePair2.Value.IsValid())
				{
					stringBuilder.Append("\n- ").Append(keyValuePair2.Value.signature);
				}
			}
			Debug.Log(stringBuilder.Append("\n").ToString());
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0002CE00 File Offset: 0x0002B000
		public static void LogSystemInfo()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.Append("Rig: ").AppendSysInfoIfPresent(SystemInfo.deviceModel, null).AppendSysInfoIfPresent(SystemInfo.processorType, null).AppendSysInfoIfPresent(SystemInfo.systemMemorySize, "MB RAM").Append(SystemInfo.processorCount).Append(" cores\n");
			stringBuilder.Append("OS: ").Append(SystemInfo.operatingSystem).Append("\n");
			stringBuilder.Append("GPU: ").Append(SystemInfo.graphicsDeviceName).Append(" ").Append(SystemInfo.graphicsMemorySize).Append("MB ").Append(SystemInfo.graphicsDeviceVersion).Append(SystemInfo.graphicsMultiThreaded ? " multi-threaded\n" : "\n");
			stringBuilder.Append("Data Path: ").Append(Application.dataPath).Append("\n");
			stringBuilder.Append("Persistent Data Path: ").Append(Application.persistentDataPath).Append("\n");
			stringBuilder.Append("StreamingAssets Path: ").Append(Application.streamingAssetsPath).Append("\n");
			stringBuilder.Append("Temporary Cache Path: ").Append(Application.temporaryCachePath).Append("\n");
			stringBuilder.Append("Device ID: ").Append(SystemInfo.deviceUniqueIdentifier).Append("\n");
			stringBuilder.Append("Max Texture Size: ").Append(SystemInfo.maxTextureSize).Append("\n");
			stringBuilder.Append("Max Cubemap Size: ").Append(SystemInfo.maxCubemapSize).Append("\n");
			stringBuilder.Append("Accelerometer: ").Append(SystemInfo.supportsAccelerometer ? "supported\n" : "not supported\n");
			stringBuilder.Append("Gyro: ").Append(SystemInfo.supportsGyroscope ? "supported\n" : "not supported\n");
			stringBuilder.Append("Location Service: ").Append(SystemInfo.supportsLocationService ? "supported\n" : "not supported\n");
			stringBuilder.Append("Compute Shaders: ").Append(SystemInfo.supportsComputeShaders ? "supported\n" : "not supported\n");
			stringBuilder.Append("Shadows: ").Append(SystemInfo.supportsShadows ? "supported\n" : "not supported\n");
			stringBuilder.Append("Instancing: ").Append(SystemInfo.supportsInstancing ? "supported\n" : "not supported\n");
			stringBuilder.Append("Motion Vectors: ").Append(SystemInfo.supportsMotionVectors ? "supported\n" : "not supported\n");
			stringBuilder.Append("3D Textures: ").Append(SystemInfo.supports3DTextures ? "supported\n" : "not supported\n");
			stringBuilder.Append("3D Render Textures: ").Append(SystemInfo.supports3DRenderTextures ? "supported\n" : "not supported\n");
			stringBuilder.Append("2D Array Textures: ").Append(SystemInfo.supports2DArrayTextures ? "supported\n" : "not supported\n");
			stringBuilder.Append("Cubemap Array Textures: ").Append(SystemInfo.supportsCubemapArrayTextures ? "supported" : "not supported");
			Debug.Log(stringBuilder.Append("\n").ToString());
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0002D152 File Offset: 0x0002B352
		private static StringBuilder AppendSysInfoIfPresent(this StringBuilder sb, string info, string postfix = null)
		{
			if (info != "n/a")
			{
				sb.Append(info);
				if (postfix != null)
				{
					sb.Append(postfix);
				}
				sb.Append(" ");
			}
			return sb;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0002D181 File Offset: 0x0002B381
		private static StringBuilder AppendSysInfoIfPresent(this StringBuilder sb, int info, string postfix = null)
		{
			if (info > 0)
			{
				sb.Append(info);
				if (postfix != null)
				{
					sb.Append(postfix);
				}
				sb.Append(" ");
			}
			return sb;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0002D1A7 File Offset: 0x0002B3A7
		public static void AddCommandInstance(string command, string description, string methodName, object instance)
		{
			if (instance == null)
			{
				Debug.LogError("Instance can't be null!");
				return;
			}
			DebugLogConsole.AddCommand(command, description, methodName, instance.GetType(), instance);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0002D1C6 File Offset: 0x0002B3C6
		public static void AddCommandStatic(string command, string description, string methodName, Type ownerType)
		{
			DebugLogConsole.AddCommand(command, description, methodName, ownerType, null);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0002D1D2 File Offset: 0x0002B3D2
		public static void RemoveCommand(string command)
		{
			if (!string.IsNullOrEmpty(command))
			{
				DebugLogConsole.methods.Remove(command);
			}
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0002D1E8 File Offset: 0x0002B3E8
		public static string GetAutoCompleteCommand(string commandStart)
		{
			foreach (KeyValuePair<string, ConsoleMethodInfo> keyValuePair in DebugLogConsole.methods)
			{
				if (keyValuePair.Key.StartsWith(commandStart))
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0002D250 File Offset: 0x0002B450
		private static void AddCommand(string command, string description, string methodName, Type ownerType, object instance = null)
		{
			if (string.IsNullOrEmpty(command))
			{
				Debug.LogError("Command name can't be empty!");
				return;
			}
			command = command.Trim();
			if (command.IndexOf(' ') >= 0)
			{
				Debug.LogError("Command name can't contain whitespace: " + command);
				return;
			}
			MethodInfo method = ownerType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				Debug.LogError(methodName + " does not exist in " + ((ownerType != null) ? ownerType.ToString() : null));
				return;
			}
			DebugLogConsole.AddCommand(command, description, method, instance);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0002D2D0 File Offset: 0x0002B4D0
		private static void AddCommand(string command, string description, MethodInfo method, object instance = null)
		{
			ParameterInfo[] array = method.GetParameters();
			if (array == null)
			{
				array = new ParameterInfo[0];
			}
			bool flag = true;
			Type[] array2 = new Type[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Type parameterType = array[i].ParameterType;
				if (!DebugLogConsole.parseFunctions.ContainsKey(parameterType))
				{
					flag = false;
					break;
				}
				array2[i] = parameterType;
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				stringBuilder.Append(command).Append(": ");
				if (!string.IsNullOrEmpty(description))
				{
					stringBuilder.Append(description).Append(" -> ");
				}
				stringBuilder.Append(method.DeclaringType.ToString()).Append(".").Append(method.Name).Append("(");
				for (int j = 0; j < array2.Length; j++)
				{
					Type type = array2[j];
					string name;
					if (!DebugLogConsole.typeReadableNames.TryGetValue(type, out name))
					{
						name = type.Name;
					}
					stringBuilder.Append(name);
					if (j < array2.Length - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				stringBuilder.Append(")");
				Type returnType = method.ReturnType;
				if (returnType != typeof(void))
				{
					string name2;
					if (!DebugLogConsole.typeReadableNames.TryGetValue(returnType, out name2))
					{
						name2 = returnType.Name;
					}
					stringBuilder.Append(" : ").Append(name2);
				}
				DebugLogConsole.methods[command] = new ConsoleMethodInfo(method, array2, instance, stringBuilder.ToString());
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0002D45C File Offset: 0x0002B65C
		public static void ExecuteCommand(string command)
		{
			if (command == null)
			{
				return;
			}
			command = command.Trim();
			if (command.Length == 0)
			{
				return;
			}
			DebugLogConsole.commandArguments.Clear();
			int num = DebugLogConsole.IndexOfChar(command, ' ', 0);
			DebugLogConsole.commandArguments.Add(command.Substring(0, num));
			for (int i = num + 1; i < command.Length; i++)
			{
				if (command[i] != ' ')
				{
					int num2 = DebugLogConsole.IndexOfDelimiter(command[i]);
					if (num2 >= 0)
					{
						num = DebugLogConsole.IndexOfChar(command, DebugLogConsole.inputDelimiters[num2][1], i + 1);
						DebugLogConsole.commandArguments.Add(command.Substring(i + 1, num - i - 1));
					}
					else
					{
						num = DebugLogConsole.IndexOfChar(command, ' ', i + 1);
						DebugLogConsole.commandArguments.Add(command.Substring(i, num - i));
					}
					i = num;
				}
			}
			ConsoleMethodInfo consoleMethodInfo;
			if (!DebugLogConsole.methods.TryGetValue(DebugLogConsole.commandArguments[0], out consoleMethodInfo))
			{
				Debug.LogWarning("Can't find command: " + DebugLogConsole.commandArguments[0]);
				return;
			}
			if (!consoleMethodInfo.IsValid())
			{
				Debug.LogWarning("Method no longer valid (instance dead): " + DebugLogConsole.commandArguments[0]);
				return;
			}
			if (consoleMethodInfo.parameterTypes.Length != DebugLogConsole.commandArguments.Count - 1)
			{
				Debug.LogWarning("Parameter count mismatch: " + consoleMethodInfo.parameterTypes.Length.ToString() + " parameters are needed");
				return;
			}
			Debug.Log("Executing command: " + DebugLogConsole.commandArguments[0]);
			object[] array = new object[consoleMethodInfo.parameterTypes.Length];
			for (int j = 0; j < consoleMethodInfo.parameterTypes.Length; j++)
			{
				string text = DebugLogConsole.commandArguments[j + 1];
				Type type = consoleMethodInfo.parameterTypes[j];
				DebugLogConsole.ParseFunction parseFunction;
				if (!DebugLogConsole.parseFunctions.TryGetValue(type, out parseFunction))
				{
					Debug.LogError("Unsupported parameter type: " + type.Name);
					return;
				}
				object obj;
				if (!parseFunction(text, out obj))
				{
					Debug.LogError("Couldn't parse " + text + " to " + type.Name);
					return;
				}
				array[j] = obj;
			}
			object obj2 = consoleMethodInfo.method.Invoke(consoleMethodInfo.instance, array);
			if (consoleMethodInfo.method.ReturnType != typeof(void))
			{
				if (obj2 == null || obj2.Equals(null))
				{
					Debug.Log("Value returned: null");
					return;
				}
				Debug.Log("Value returned: " + obj2.ToString());
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0002D6D0 File Offset: 0x0002B8D0
		private static int IndexOfDelimiter(char c)
		{
			for (int i = 0; i < DebugLogConsole.inputDelimiters.Length; i++)
			{
				if (c == DebugLogConsole.inputDelimiters[i][0])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0002D704 File Offset: 0x0002B904
		private static int IndexOfChar(string command, char c, int startIndex)
		{
			int num = command.IndexOf(c, startIndex);
			if (num < 0)
			{
				num = command.Length;
			}
			return num;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0002D726 File Offset: 0x0002B926
		private static bool ParseString(string input, out object output)
		{
			output = input;
			return input.Length > 0;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0002D734 File Offset: 0x0002B934
		private static bool ParseBool(string input, out object output)
		{
			if (input == "1" || input.ToLowerInvariant() == "true")
			{
				output = true;
				return true;
			}
			if (input == "0" || input.ToLowerInvariant() == "false")
			{
				output = false;
				return true;
			}
			output = false;
			return false;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002D79C File Offset: 0x0002B99C
		private static bool ParseInt(string input, out object output)
		{
			int num;
			bool result = int.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0002D7BC File Offset: 0x0002B9BC
		private static bool ParseUInt(string input, out object output)
		{
			uint num;
			bool result = uint.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0002D7DC File Offset: 0x0002B9DC
		private static bool ParseLong(string input, out object output)
		{
			long num;
			bool result = long.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0002D7FC File Offset: 0x0002B9FC
		private static bool ParseULong(string input, out object output)
		{
			ulong num;
			bool result = ulong.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0002D81C File Offset: 0x0002BA1C
		private static bool ParseByte(string input, out object output)
		{
			byte b;
			bool result = byte.TryParse(input, out b);
			output = b;
			return result;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0002D83C File Offset: 0x0002BA3C
		private static bool ParseSByte(string input, out object output)
		{
			sbyte b;
			bool result = sbyte.TryParse(input, out b);
			output = b;
			return result;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0002D85C File Offset: 0x0002BA5C
		private static bool ParseShort(string input, out object output)
		{
			short num;
			bool result = short.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0002D87C File Offset: 0x0002BA7C
		private static bool ParseUShort(string input, out object output)
		{
			ushort num;
			bool result = ushort.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0002D89C File Offset: 0x0002BA9C
		private static bool ParseChar(string input, out object output)
		{
			char c;
			bool result = char.TryParse(input, out c);
			output = c;
			return result;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0002D8BC File Offset: 0x0002BABC
		private static bool ParseFloat(string input, out object output)
		{
			float num;
			bool result = float.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0002D8DC File Offset: 0x0002BADC
		private static bool ParseDouble(string input, out object output)
		{
			double num;
			bool result = double.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0002D8FC File Offset: 0x0002BAFC
		private static bool ParseDecimal(string input, out object output)
		{
			decimal num;
			bool result = decimal.TryParse(input, out num);
			output = num;
			return result;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0002D919 File Offset: 0x0002BB19
		private static bool ParseVector2(string input, out object output)
		{
			return DebugLogConsole.CreateVectorFromInput(input, typeof(Vector2), out output);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0002D92C File Offset: 0x0002BB2C
		private static bool ParseVector3(string input, out object output)
		{
			return DebugLogConsole.CreateVectorFromInput(input, typeof(Vector3), out output);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0002D93F File Offset: 0x0002BB3F
		private static bool ParseVector4(string input, out object output)
		{
			return DebugLogConsole.CreateVectorFromInput(input, typeof(Vector4), out output);
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0002D952 File Offset: 0x0002BB52
		private static bool ParseGameObject(string input, out object output)
		{
			output = GameObject.Find(input);
			return true;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0002D960 File Offset: 0x0002BB60
		private static bool CreateVectorFromInput(string input, Type vectorType, out object output)
		{
			List<string> list = new List<string>(input.Replace(',', ' ').Trim().Split(' ', StringSplitOptions.None));
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i] = list[i].Trim();
				if (list[i].Length == 0)
				{
					list.RemoveAt(i);
				}
			}
			float[] array = new float[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				float num;
				if (!float.TryParse(list[i], out num))
				{
					if (vectorType == typeof(Vector3))
					{
						output = default(Vector3);
					}
					else if (vectorType == typeof(Vector2))
					{
						output = default(Vector2);
					}
					else
					{
						output = default(Vector4);
					}
					return false;
				}
				array[i] = num;
			}
			if (vectorType == typeof(Vector3))
			{
				Vector3 vector = default(Vector3);
				int i;
				for (i = 0; i < array.Length; i++)
				{
					if (i >= 3)
					{
						break;
					}
					vector[i] = array[i];
				}
				while (i < 3)
				{
					vector[i] = 0f;
					i++;
				}
				output = vector;
			}
			else if (vectorType == typeof(Vector2))
			{
				Vector2 vector2 = default(Vector2);
				int i;
				for (i = 0; i < array.Length; i++)
				{
					if (i >= 2)
					{
						break;
					}
					vector2[i] = array[i];
				}
				while (i < 2)
				{
					vector2[i] = 0f;
					i++;
				}
				output = vector2;
			}
			else
			{
				Vector4 vector3 = default(Vector4);
				int i;
				for (i = 0; i < array.Length; i++)
				{
					if (i >= 4)
					{
						break;
					}
					vector3[i] = array[i];
				}
				while (i < 4)
				{
					vector3[i] = 0f;
					i++;
				}
				output = vector3;
			}
			return true;
		}

		// Token: 0x04000AB9 RID: 2745
		private static Dictionary<string, ConsoleMethodInfo> methods = new Dictionary<string, ConsoleMethodInfo>();

		// Token: 0x04000ABA RID: 2746
		private static Dictionary<Type, DebugLogConsole.ParseFunction> parseFunctions;

		// Token: 0x04000ABB RID: 2747
		private static Dictionary<Type, string> typeReadableNames;

		// Token: 0x04000ABC RID: 2748
		private static List<string> commandArguments = new List<string>(8);

		// Token: 0x04000ABD RID: 2749
		private static readonly string[] inputDelimiters = new string[]
		{
			"\"\"",
			"{}",
			"()",
			"[]"
		};

		// Token: 0x020002DB RID: 731
		// (Invoke) Token: 0x060012FE RID: 4862
		public delegate bool ParseFunction(string input, out object output);
	}
}
