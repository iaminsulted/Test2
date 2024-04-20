using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ProCamsLensDataTable
{
	public class FOVData
	{
		public int _focalLength;

		public float _nodalOffset;

		public float _realHFOV;

		public float _unityVFOV;

		public FOVData(int focalLength, float nodalOffset, float realFOV, float unityFOV)
		{
			_focalLength = focalLength;
			_nodalOffset = nodalOffset;
			_realHFOV = realFOV;
			_unityVFOV = unityFOV;
		}
	}

	public class LensKitData
	{
		public string _lensKit;

		public List<FOVData> _fovDataset = new List<FOVData>();
	}

	public class FilmFormatData
	{
		public string _formatName;

		public float _aspect;

		public List<ScreenSize> _screenSizes = new List<ScreenSize>();

		public List<LensKitData> _lensKits = new List<LensKitData>();

		public void AddFocalLengthData(string lensKit, int focalLength, float nodalOffset, float realFOV, float unityFOV)
		{
			LensKitData lensKitData = null;
			int count = _lensKits.Count;
			for (int i = 0; i < count; i++)
			{
				if (_lensKits[i]._lensKit.Equals(lensKit))
				{
					lensKitData = _lensKits[i];
					break;
				}
			}
			if (lensKitData == null)
			{
				lensKitData = new LensKitData();
				lensKitData._lensKit = lensKit;
				_lensKits.Add(lensKitData);
			}
			lensKitData._fovDataset.Add(new FOVData(focalLength, nodalOffset, realFOV, unityFOV));
		}

		public LensKitData GetLensKitData(int index)
		{
			if (index >= 0 && index < _lensKits.Count)
			{
				return _lensKits[index];
			}
			return null;
		}

		public FOVData GetFOVData(int lensKitIndex, int lensIndex)
		{
			LensKitData lensKitData = GetLensKitData(lensKitIndex);
			if (lensKitData != null && lensIndex >= 0 && lensIndex < lensKitData._fovDataset.Count)
			{
				return lensKitData._fovDataset[lensIndex];
			}
			return null;
		}
	}

	private static ProCamsLensDataTable _singleton;

	private List<FilmFormatData> _filmFormats;

	public static ProCamsLensDataTable Instance
	{
		get
		{
			if (_singleton == null)
			{
				_singleton = new ProCamsLensDataTable();
				_singleton.LoadData();
			}
			return _singleton;
		}
	}

	public List<FilmFormatData>.Enumerator FilmFormatDataEmumerator => _filmFormats.GetEnumerator();

	public int NumFilmFormats => _filmFormats.Count;

	public FilmFormatData GetFilmFormat(string formatName)
	{
		int count = _filmFormats.Count;
		for (int i = 0; i < count; i++)
		{
			if (_filmFormats[i]._formatName == formatName)
			{
				return _filmFormats[i];
			}
		}
		if (count > 0)
		{
			return _filmFormats[0];
		}
		return null;
	}

	public ScreenSize GetScreenSize(FilmFormatData format, string screenSizeName)
	{
		if (format != null)
		{
			foreach (ScreenSize screenSize in format._screenSizes)
			{
				if (screenSize.name == screenSizeName)
				{
					return screenSize;
				}
			}
		}
		return default(ScreenSize);
	}

	public void LoadData()
	{
		LoadLensData();
	}

	public void LoadLensData()
	{
		_filmFormats = new List<FilmFormatData>();
		TextAsset textAsset = (TextAsset)Resources.Load("CinemaSuite_LensData", typeof(TextAsset));
		if (textAsset == null)
		{
			Debug.LogError("File 'CinemaSuite_LensData.txt' is not found in Resources folder. Unable to load lens data.");
			return;
		}
		FilmFormatData filmFormatData = null;
		string[] array = textAsset.text.Split("\n"[0]);
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			string text = array[i].Trim();
			if (text.StartsWith("#"))
			{
				continue;
			}
			int length = text.Length;
			if (filmFormatData == null)
			{
				if (text.StartsWith("Name="))
				{
					int num2 = text.IndexOf("=") + 1;
					if (num2 < length)
					{
						string text2 = text.Substring(num2).Trim();
						if (text2.Length != 0)
						{
							filmFormatData = new FilmFormatData();
							filmFormatData._formatName = text2;
						}
					}
				}
				else if (length != 0)
				{
					Debug.LogError("Invalid data at line: " + i);
				}
			}
			else if (length == 0)
			{
				_filmFormats.Add(filmFormatData);
				filmFormatData = null;
			}
			else if (text.StartsWith("Aspect="))
			{
				int num3 = text.IndexOf("=") + 1;
				if (num3 >= length)
				{
					continue;
				}
				string text3 = text.Substring(num3).Trim();
				int num4 = text3.IndexOf(":");
				if (num4 > 0 && num4 < length)
				{
					string text4 = text3.Substring(0, num4).Trim();
					string text5 = text3.Substring(num4 + 1).Trim();
					float result = 0f;
					float result2 = 0f;
					if (!float.TryParse(text4, NumberStyles.Number, CultureInfo.InvariantCulture, out result) || result <= 0f)
					{
						Debug.LogError("Invalid number: " + text4 + " at line " + (i + 1));
						break;
					}
					if (!float.TryParse(text5, NumberStyles.Number, CultureInfo.InvariantCulture, out result2) || result2 <= 0f)
					{
						Debug.LogError("Invalid number: " + text5 + " at line " + (i + 1));
						break;
					}
					filmFormatData._aspect = result / result2;
				}
			}
			else if (text.StartsWith("ScreenSize"))
			{
				int num5 = text.IndexOf("=") + 1;
				if (num5 < length)
				{
					string[] array2 = text.Substring(num5).Split(","[0]);
					if (array2 == null || array2.Length != 2)
					{
						Debug.LogError("Invalid screen size entry at line " + (i + 1));
						break;
					}
					string name = array2[0].Trim();
					int result3 = 0;
					if (!int.TryParse(array2[1].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out result3) || result3 <= 0)
					{
						Debug.LogError("Invalid screen size at line " + (i + 1));
						break;
					}
					filmFormatData._screenSizes.Add(new ScreenSize(name, result3));
				}
			}
			else
			{
				if (!text.StartsWith("FocalLength"))
				{
					continue;
				}
				int num6 = text.IndexOf("=") + 1;
				if (num6 < length)
				{
					string[] array3 = text.Substring(num6).Split(","[0]);
					if (array3 == null || array3.Length != 5)
					{
						Debug.LogError("Invalid data for focal length at line " + (i + 1));
						break;
					}
					string text6 = array3[0].Trim();
					if (text6.Length == 0)
					{
						Debug.LogError("Invalid lens kit name at line " + (i + 1));
						break;
					}
					int result4 = 0;
					if (!int.TryParse(array3[1].TrimEnd('m'), NumberStyles.Number, CultureInfo.InvariantCulture, out result4) || result4 <= 0)
					{
						Debug.LogError("Invalid focal length at line " + (i + 1));
						break;
					}
					float result5 = 0f;
					if (!float.TryParse(array3[2].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out result5) || result5 < 0f)
					{
						Debug.LogError("Invalid nodal offset at line " + (i + 1));
						break;
					}
					float result6 = 0f;
					if (!float.TryParse(array3[3].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out result6) || result6 <= 0f)
					{
						Debug.LogError("Invalid real FOV at line " + (i + 1));
						break;
					}
					float result7 = 0f;
					if (!float.TryParse(array3[4].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out result7) || result7 <= 0f)
					{
						Debug.LogError("Invalid Unity FOV at line " + (i + 1));
						break;
					}
					filmFormatData.AddFocalLengthData(text6, result4, result5, result6, result7);
				}
			}
		}
	}
}
