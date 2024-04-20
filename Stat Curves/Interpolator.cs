using System;

namespace StatCurves
{
	// Token: 0x02000006 RID: 6
	public class Interpolator
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002C88 File Offset: 0x00000E88
		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public static float Clamp01(float x)
		{
			return (x > 1f) ? 1f : ((x < 0f) ? 0f : x);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public static float Linear(float start, float end, float value)
		{
			return Interpolator.Lerp(start, end, value);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002CF4 File Offset: 0x00000EF4
		public static float CLerp(float start, float end, float value)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Math.Abs((num2 - num) * 0.5f);
			bool flag = end - start < -num3;
			float result;
			if (flag)
			{
				float num4 = (num2 - start + end) * value;
				result = start + num4;
			}
			else
			{
				bool flag2 = end - start > num3;
				if (flag2)
				{
					float num4 = -(num2 - end + start) * value;
					result = start + num4;
				}
				else
				{
					result = start + (end - start) * value;
				}
			}
			return result;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D6C File Offset: 0x00000F6C
		public static float Spring(float start, float end, float value)
		{
			value = Interpolator.Clamp01(value);
			value = (float)((Math.Sin((double)value * 3.141592653589793 * (double)(0.2f + 2.5f * value * value * value)) * Math.Pow((double)(1f - value), 2.200000047683716) + (double)value) * (double)(1f + 1.2f * (1f - value)));
			return start + (end - start) * value;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public static float EaseInQuad(float start, float end, float value)
		{
			end -= start;
			return end * value * value + start;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002E04 File Offset: 0x00001004
		public static float EaseOutQuad(float start, float end, float value)
		{
			end -= start;
			return -end * value * (value - 2f) + start;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002E2C File Offset: 0x0000102C
		public static float EaseInOutQuad(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				result = end * 0.5f * value * value + start;
			}
			else
			{
				value -= 1f;
				result = -end * 0.5f * (value * (value - 2f) - 1f) + start;
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002E8C File Offset: 0x0000108C
		public static float EaseInCubic(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value + start;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002EAC File Offset: 0x000010AC
		public static float EaseOutCubic(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (value * value * value + 1f) + start;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002EDC File Offset: 0x000010DC
		public static float EaseInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				result = end * 0.5f * value * value * value + start;
			}
			else
			{
				value -= 2f;
				result = end * 0.5f * (value * value * value + 2f) + start;
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002F3C File Offset: 0x0000113C
		public static float EaseInQuart(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value * value + start;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002F60 File Offset: 0x00001160
		public static float EaseOutQuart(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return -end * (value * value * value * value - 1f) + start;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002F94 File Offset: 0x00001194
		public static float EaseInOutQuart(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				result = end * 0.5f * value * value * value * value + start;
			}
			else
			{
				value -= 2f;
				result = -end * 0.5f * (value * value * value * value - 2f) + start;
			}
			return result;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002FF8 File Offset: 0x000011F8
		public static float EaseInQuint(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value * value * value + start;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000301C File Offset: 0x0000121C
		public static float EaseOutQuint(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (value * value * value * value * value + 1f) + start;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003050 File Offset: 0x00001250
		public static float EaseInOutQuint(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				result = end * 0.5f * value * value * value * value * value + start;
			}
			else
			{
				value -= 2f;
				result = end * 0.5f * (value * value * value * value * value + 2f) + start;
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000030B8 File Offset: 0x000012B8
		public static float EaseInSine(float start, float end, float value)
		{
			end -= start;
			return -end * (float)(Math.Cos((double)value * 1.5707963267948966) + (double)end + (double)start);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000030EC File Offset: 0x000012EC
		public static float EaseOutSine(float start, float end, float value)
		{
			end -= start;
			return end * (float)Math.Sin((double)value * 1.5707963267948966) + start;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000311C File Offset: 0x0000131C
		public static float EaseInOutSine(float start, float end, float value)
		{
			end -= start;
			return -end * 0.5f * (float)(Math.Cos(3.141592653589793 * (double)value) - 1.0) + start;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000315C File Offset: 0x0000135C
		public static float EaseInExpo(float start, float end, float value)
		{
			end -= start;
			return end * (float)Math.Pow(2.0, (double)(10f * (value - 1f))) + start;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003194 File Offset: 0x00001394
		public static float EaseOutExpo(float start, float end, float value)
		{
			end -= start;
			return end * ((float)(-(float)Math.Pow(2.0, (double)(-10f * value))) + 1f) + start;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000031D0 File Offset: 0x000013D0
		public static float EaseInOutExpo(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				result = end * 0.5f * (float)Math.Pow(2.0, (double)(10f * (value - 1f))) + start;
			}
			else
			{
				value -= 1f;
				result = end * 0.5f * ((float)(-(float)Math.Pow(2.0, (double)(-10f * value))) + 2f) + start;
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003258 File Offset: 0x00001458
		public static float EaseInCirc(float start, float end, float value)
		{
			end -= start;
			return -end * ((float)Math.Sqrt((double)(1f - value * value)) - 1f) + start;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000328C File Offset: 0x0000148C
		public static float EaseOutCirc(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (float)Math.Sqrt((double)(1f - value * value)) + start;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000032C0 File Offset: 0x000014C0
		public static float EaseInOutCirc(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				result = -end * 0.5f * ((float)Math.Sqrt((double)(1f - value * value)) - 1f) + start;
			}
			else
			{
				value -= 2f;
				result = end * 0.5f * ((float)Math.Sqrt((double)(1f - value * value)) + 1f) + start;
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000333C File Offset: 0x0000153C
		public static float EaseInBounce(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			return end - Interpolator.EaseOutBounce(0f, end, num - value) + start;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000336C File Offset: 0x0000156C
		public static float EaseOutBounce(float start, float end, float value)
		{
			value /= 1f;
			end -= start;
			bool flag = value < 0.36363637f;
			float result;
			if (flag)
			{
				result = end * (7.5625f * value * value) + start;
			}
			else
			{
				bool flag2 = value < 0.72727275f;
				if (flag2)
				{
					value -= 0.54545456f;
					result = end * (7.5625f * value * value + 0.75f) + start;
				}
				else
				{
					bool flag3 = (double)value < 0.9090909090909091;
					if (flag3)
					{
						value -= 0.8181818f;
						result = end * (7.5625f * value * value + 0.9375f) + start;
					}
					else
					{
						value -= 0.95454544f;
						result = end * (7.5625f * value * value + 0.984375f) + start;
					}
				}
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003424 File Offset: 0x00001624
		public static float EaseInOutBounce(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			bool flag = value < num * 0.5f;
			float result;
			if (flag)
			{
				result = Interpolator.EaseInBounce(0f, end, value * 2f) * 0.5f + start;
			}
			else
			{
				result = Interpolator.EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003494 File Offset: 0x00001694
		public static float EaseInBack(float start, float end, float value)
		{
			end -= start;
			value /= 1f;
			float num = 1.70158f;
			return end * value * value * ((num + 1f) * value - num) + start;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000034D0 File Offset: 0x000016D0
		public static float EaseOutBack(float start, float end, float value)
		{
			float num = 1.70158f;
			end -= start;
			value -= 1f;
			return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003510 File Offset: 0x00001710
		public static float EaseInOutBack(float start, float end, float value)
		{
			float num = 1.70158f;
			end -= start;
			value /= 0.5f;
			bool flag = value < 1f;
			float result;
			if (flag)
			{
				num *= 1.525f;
				result = end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
			}
			else
			{
				value -= 2f;
				num *= 1.525f;
				result = end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003598 File Offset: 0x00001798
		public static float EaseInElastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			bool flag = value == 0f;
			float result;
			if (flag)
			{
				result = start;
			}
			else
			{
				bool flag2 = (value /= num) == 1f;
				if (flag2)
				{
					result = start + end;
				}
				else
				{
					bool flag3 = num3 == 0f || num3 < Math.Abs(end);
					float num4;
					if (flag3)
					{
						num3 = end;
						num4 = num2 / 4f;
					}
					else
					{
						num4 = num2 / 6.2831855f * (float)Math.Asin((double)(end / num3));
					}
					result = -(num3 * (float)Math.Pow(2.0, (double)(10f * (value -= 1f))) * (float)Math.Sin((double)((value * num - num4) * 6.2831855f / num2))) + start;
				}
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003670 File Offset: 0x00001870
		public static float EaseOutElastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			bool flag = value == 0f;
			float result;
			if (flag)
			{
				result = start;
			}
			else
			{
				bool flag2 = (value /= num) == 1f;
				if (flag2)
				{
					result = start + end;
				}
				else
				{
					bool flag3 = num3 == 0f || num3 < Math.Abs(end);
					float num4;
					if (flag3)
					{
						num3 = end;
						num4 = num2 * 0.25f;
					}
					else
					{
						num4 = num2 / 6.2831855f * (float)Math.Asin((double)(end / num3));
					}
					result = num3 * (float)Math.Pow(2.0, (double)(-10f * value)) * (float)Math.Sin((double)(value * num - num4) * 6.283185307179586 / (double)num2) + end + start;
				}
			}
			return result;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003744 File Offset: 0x00001944
		public static float EaseInOutElastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			bool flag = value == 0f;
			float result;
			if (flag)
			{
				result = start;
			}
			else
			{
				bool flag2 = (value /= num * 0.5f) == 2f;
				if (flag2)
				{
					result = start + end;
				}
				else
				{
					bool flag3 = num3 == 0f || num3 < Math.Abs(end);
					float num4;
					if (flag3)
					{
						num3 = end;
						num4 = num2 / 4f;
					}
					else
					{
						num4 = num2 / 6.2831855f * (float)Math.Asin((double)(end / num3));
					}
					bool flag4 = value < 1f;
					if (flag4)
					{
						result = -0.5f * (num3 * (float)Math.Pow(2.0, (double)(10f * (value -= 1f))) * (float)Math.Sin((double)(value * num - num4) * 6.283185307179586 / (double)num2)) + start;
					}
					else
					{
						result = num3 * (float)Math.Pow(2.0, (double)(-10f * (value -= 1f))) * (float)Math.Sin((double)(value * num - num4) * 6.283185307179586 / (double)num2) * 0.5f + end + start;
					}
				}
			}
			return result;
		}

		// Token: 0x02000018 RID: 24
		public enum AxisType
		{
			// Token: 0x040000A4 RID: 164
			X,
			// Token: 0x040000A5 RID: 165
			Y,
			// Token: 0x040000A6 RID: 166
			Z
		}

		// Token: 0x02000019 RID: 25
		public enum EaseType
		{
			// Token: 0x040000A8 RID: 168
			EaseInQuad,
			// Token: 0x040000A9 RID: 169
			EaseOutQuad,
			// Token: 0x040000AA RID: 170
			EaseInOutQuad,
			// Token: 0x040000AB RID: 171
			EaseInCubic,
			// Token: 0x040000AC RID: 172
			EaseOutCubic,
			// Token: 0x040000AD RID: 173
			EaseInOutCubic,
			// Token: 0x040000AE RID: 174
			EaseInQuart,
			// Token: 0x040000AF RID: 175
			EaseOutQuart,
			// Token: 0x040000B0 RID: 176
			EaseInOutQuart,
			// Token: 0x040000B1 RID: 177
			EaseInQuint,
			// Token: 0x040000B2 RID: 178
			EaseOutQuint,
			// Token: 0x040000B3 RID: 179
			EaseInOutQuint,
			// Token: 0x040000B4 RID: 180
			EaseInSine,
			// Token: 0x040000B5 RID: 181
			EaseOutSine,
			// Token: 0x040000B6 RID: 182
			EaseInOutSine,
			// Token: 0x040000B7 RID: 183
			EaseInExpo,
			// Token: 0x040000B8 RID: 184
			EaseOutExpo,
			// Token: 0x040000B9 RID: 185
			EaseInOutExpo,
			// Token: 0x040000BA RID: 186
			EaseInCirc,
			// Token: 0x040000BB RID: 187
			EaseOutCirc,
			// Token: 0x040000BC RID: 188
			EaseInOutCirc,
			// Token: 0x040000BD RID: 189
			Linear,
			// Token: 0x040000BE RID: 190
			Spring,
			// Token: 0x040000BF RID: 191
			EaseInBounce,
			// Token: 0x040000C0 RID: 192
			EaseOutBounce,
			// Token: 0x040000C1 RID: 193
			EaseInOutBounce,
			// Token: 0x040000C2 RID: 194
			EaseInBack,
			// Token: 0x040000C3 RID: 195
			EaseOutBack,
			// Token: 0x040000C4 RID: 196
			EaseInOutBack,
			// Token: 0x040000C5 RID: 197
			EaseInElastic,
			// Token: 0x040000C6 RID: 198
			EaseOutElastic,
			// Token: 0x040000C7 RID: 199
			EaseInOutElastic
		}

		// Token: 0x0200001A RID: 26
		public enum SpaceType
		{
			// Token: 0x040000C9 RID: 201
			Local,
			// Token: 0x040000CA RID: 202
			Global,
			// Token: 0x040000CB RID: 203
			Parent
		}

		// Token: 0x0200001B RID: 27
		public enum TransformationType
		{
			// Token: 0x040000CD RID: 205
			Snap,
			// Token: 0x040000CE RID: 206
			Current,
			// Token: 0x040000CF RID: 207
			Interpolate
		}
	}
}
