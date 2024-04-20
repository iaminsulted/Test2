using UnityEngine;

public class ProCamsUtility
{
	public static float Convert(float val, Units cfrom, Units to)
	{
		switch (cfrom)
		{
		case Units.Millimeter:
			switch (to)
			{
			case Units.Centimeter:
				return val * 0.1f;
			case Units.Meter:
				return val * 0.001f;
			case Units.Inch:
				return val * 0.0393701f;
			case Units.Foot:
				return val * 0.00328084f;
			}
			break;
		case Units.Centimeter:
			switch (to)
			{
			case Units.Millimeter:
				return val * 10f;
			case Units.Meter:
				return val * 0.01f;
			case Units.Inch:
				return val * 0.393701f;
			case Units.Foot:
				return val * 0.0328084f;
			}
			break;
		case Units.Meter:
			switch (to)
			{
			case Units.Centimeter:
				return val * 100f;
			case Units.Millimeter:
				return val * 1000f;
			case Units.Inch:
				return val * 39.3701f;
			case Units.Foot:
				return val * 3.28084f;
			}
			break;
		case Units.Inch:
			switch (to)
			{
			case Units.Centimeter:
				return val * 2.54f;
			case Units.Meter:
				return val * 0.0254f;
			case Units.Millimeter:
				return val * 25.4f;
			case Units.Foot:
				return val * 0.0833333f;
			}
			break;
		case Units.Foot:
			switch (to)
			{
			case Units.Centimeter:
				return val * 30.48f;
			case Units.Meter:
				return val * 0.3048f;
			case Units.Inch:
				return val * 12f;
			case Units.Millimeter:
				return val * 304.8f;
			}
			break;
		}
		return 0f;
	}

	public static float Truncate(float val, int decimalPlaces)
	{
		float num = 1f;
		for (int i = 0; i < decimalPlaces; i++)
		{
			num *= 10f;
		}
		val *= num;
		val = Mathf.Floor(val);
		val /= num;
		return val;
	}
}
