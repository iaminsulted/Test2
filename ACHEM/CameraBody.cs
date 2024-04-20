using System;
using CinemaProCams;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBody : MonoBehaviour
{
	private static RenderTexture _cameraPreview;

	[SerializeField]
	private UnitOfMeasure _unitOfMeasure;

	[SerializeField]
	private string _filmFormatName;

	[SerializeField]
	private string _screenSizeName;

	[SerializeField]
	private Camera _nodalCamera;

	[SerializeField]
	private CSDOFScatter[] _dofComponents;

	[SerializeField]
	private Transform _focusTransform;

	[SerializeField]
	private Transform _rigTransform;

	[SerializeField]
	private Vector3 _fpOffset;

	[SerializeField]
	private ProCamsLensDataTable.FOVData[] _lensFOVList;

	[SerializeField]
	private int _fstopIndex;

	[SerializeField]
	private int _lensIndex;

	[SerializeField]
	private int _lensKitIndex;

	[SerializeField]
	private string _cameraSpecs;

	[SerializeField]
	private string _cameraDesc;

	[SerializeField]
	private string _lensKitName;

	[SerializeField]
	private float _dofFarLimit;

	[SerializeField]
	private float _dofNearLimit;

	[SerializeField]
	private float _dofDistTotal;

	[SerializeField]
	private float _focusDist;

	[SerializeField]
	private bool _centerOnSubject;

	[SerializeField]
	private bool _clickToFocus;

	[SerializeField]
	private bool _showGizmos = true;

	public bool ShowBody;

	public static RenderTexture CameraPreview
	{
		get
		{
			return _cameraPreview;
		}
		set
		{
			_cameraPreview = value;
		}
	}

	public UnitOfMeasure UnitOfMeasure
	{
		get
		{
			return _unitOfMeasure;
		}
		set
		{
			_unitOfMeasure = value;
		}
	}

	public string FilmFormatName
	{
		get
		{
			return _filmFormatName;
		}
		set
		{
			_filmFormatName = value;
		}
	}

	public string ScreenSizeName
	{
		get
		{
			return _screenSizeName;
		}
		set
		{
			_screenSizeName = value;
		}
	}

	public Camera NodalCamera
	{
		get
		{
			return _nodalCamera;
		}
		set
		{
			_nodalCamera = value;
		}
	}

	public CSDOFScatter[] DepthOfField
	{
		get
		{
			return _dofComponents;
		}
		set
		{
			_dofComponents = value;
		}
	}

	public Transform FocusTransform
	{
		get
		{
			return _focusTransform;
		}
		set
		{
			_focusTransform = value;
		}
	}

	public Transform RigTransform
	{
		get
		{
			return _rigTransform;
		}
		set
		{
			_rigTransform = value;
		}
	}

	public Vector3 FilmPlaneOffset
	{
		get
		{
			return _fpOffset;
		}
		set
		{
			_fpOffset = value;
		}
	}

	public ProCamsLensDataTable.FOVData[] LensFOVList
	{
		get
		{
			return _lensFOVList;
		}
		set
		{
			_lensFOVList = value;
		}
	}

	public int IndexOfFStop
	{
		get
		{
			return _fstopIndex;
		}
		set
		{
			_fstopIndex = value;
		}
	}

	public int IndexOfLens
	{
		get
		{
			return _lensIndex;
		}
		set
		{
			_lensIndex = value;
		}
	}

	public int IndexOfLensKit
	{
		get
		{
			return _lensKitIndex;
		}
		set
		{
			_lensKitIndex = value;
		}
	}

	public string CameraSpecs
	{
		get
		{
			return _cameraSpecs;
		}
		set
		{
			_cameraSpecs = value;
		}
	}

	public string CameraDescription
	{
		get
		{
			return _cameraDesc;
		}
		set
		{
			_cameraDesc = value;
		}
	}

	public string LensKitName
	{
		get
		{
			return _lensKitName;
		}
		set
		{
			_lensKitName = value;
		}
	}

	public float DOFFarLimit
	{
		get
		{
			return _dofFarLimit;
		}
		set
		{
			_dofFarLimit = value;
		}
	}

	public float DOFNearLimit
	{
		get
		{
			return _dofNearLimit;
		}
		set
		{
			_dofNearLimit = value;
		}
	}

	public float DOFTotal
	{
		get
		{
			return _dofDistTotal;
		}
		set
		{
			_dofDistTotal = value;
		}
	}

	public float FocusDistance
	{
		get
		{
			return _focusDist;
		}
		set
		{
			_focusDist = value;
		}
	}

	public bool CenterOnSubject
	{
		get
		{
			return _centerOnSubject;
		}
		set
		{
			_centerOnSubject = value;
		}
	}

	public bool ClickToFocus
	{
		get
		{
			return _clickToFocus;
		}
		set
		{
			_clickToFocus = value;
		}
	}

	public bool ShowGizmos
	{
		get
		{
			return _showGizmos;
		}
		set
		{
			_showGizmos = value;
		}
	}

	private void Awake()
	{
		_nodalCamera = GetComponent<Camera>();
		_dofComponents = GetComponents<CSDOFScatter>();
		if (_nodalCamera == null)
		{
			CreateNodalCamera();
			_nodalCamera = GetComponentInChildren<Camera>();
		}
	}

	private void Start()
	{
		ProCamsLensDataTable.FilmFormatData filmFormat = ProCamsLensDataTable.Instance.GetFilmFormat(_filmFormatName);
		if (filmFormat != null)
		{
			ProCamsLensDataTable.LensKitData lensKitData = filmFormat.GetLensKitData(_lensKitIndex);
			if (lensKitData != null)
			{
				_lensFOVList = lensKitData._fovDataset.ToArray();
			}
		}
	}

	private void Update()
	{
		CallUpdate();
	}

	private void OnDrawGizmos()
	{
		if (_showGizmos && _lensFOVList != null)
		{
			float num = _dofNearLimit * 0.3048f;
			float num2 = _dofFarLimit * 0.3048f;
			float num3 = _focusDist;
			if (_unitOfMeasure == UnitOfMeasure.Imperial)
			{
				num = _dofNearLimit * 0.3048f;
				num2 = _dofFarLimit * 0.3048f;
				num3 = _focusDist * 0.3048f;
			}
			Transform transform = _nodalCamera.transform;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
			Vector3 vector = Vector3.forward * num;
			Vector3 vector2 = Vector3.forward * num2;
			Vector3 vector3 = Vector3.forward * num3;
			Vector3 vector4 = Vector3.forward * _nodalCamera.nearClipPlane;
			Vector3 vector5 = Vector3.forward * _nodalCamera.farClipPlane;
			float num4 = 0f;
			ProCamsLensDataTable.FilmFormatData filmFormat = ProCamsLensDataTable.Instance.GetFilmFormat(_filmFormatName);
			if (filmFormat != null)
			{
				num4 = Lens.GetHorizontalFOV(filmFormat._aspect, _lensFOVList[_lensIndex]._unityVFOV);
			}
			float num5 = Mathf.Tan(MathF.PI / 180f * (num4 / 2f));
			float num6 = num5 * num;
			float num7 = num5 * num2;
			float num8 = num5 * num3;
			float num9 = num5 * _nodalCamera.nearClipPlane;
			float num10 = num5 * _nodalCamera.farClipPlane;
			Vector3 vector6 = Vector3.right * num6;
			Vector3 vector7 = Vector3.right * num7;
			Vector3 vector8 = Vector3.right * num8;
			Vector3 vector9 = Vector3.right * num9;
			Vector3 vector10 = Vector3.right * num10;
			num4 = _lensFOVList[_lensIndex]._unityVFOV;
			float num11 = Mathf.Tan(MathF.PI / 180f * (num4 / 2f));
			num6 = num11 * num;
			num7 = num11 * num2;
			num8 = num11 * num3;
			num9 = num11 * _nodalCamera.nearClipPlane;
			num10 = num11 * _nodalCamera.farClipPlane;
			Vector3 vector11 = Vector3.up * num6;
			Vector3 vector12 = Vector3.up * num7;
			Vector3 vector13 = Vector3.up * num8;
			Vector3 vector14 = Vector3.up * num9;
			Vector3 vector15 = Vector3.up * num10;
			Vector3 from = vector - vector6 - vector11;
			Vector3 vector16 = vector - vector6 + vector11;
			Vector3 vector17 = vector + vector6 - vector11;
			Vector3 vector18 = vector + vector6 + vector11;
			Vector3 from2 = vector3 - vector8 - vector13;
			Vector3 vector19 = vector3 - vector8 + vector13;
			Vector3 vector20 = vector3 + vector8 - vector13;
			Vector3 to = vector3 + vector8 + vector13;
			Vector3 vector21 = vector2 - vector7 - vector12;
			Vector3 vector22 = vector2 - vector7 + vector12;
			Vector3 vector23 = vector2 + vector7 - vector12;
			Vector3 to2 = vector2 + vector7 + vector12;
			Vector3 from3 = vector4 - vector9 - vector14;
			Vector3 from4 = vector4 - vector9 + vector14;
			Vector3 from5 = vector4 + vector9 - vector14;
			Vector3 from6 = vector4 + vector9 + vector14;
			Vector3 vector24 = vector5 - vector10 - vector15;
			Vector3 vector25 = vector5 - vector10 + vector15;
			Vector3 vector26 = vector5 + vector10 - vector15;
			Vector3 to3 = vector5 + vector10 + vector15;
			Gizmos.color = Color.white;
			Gizmos.DrawLine(vector24, vector26);
			Gizmos.DrawLine(vector25, to3);
			Gizmos.DrawLine(vector24, vector25);
			Gizmos.DrawLine(vector26, to3);
			Gizmos.DrawLine(from3, vector24);
			Gizmos.DrawLine(from5, vector26);
			Gizmos.DrawLine(from4, vector25);
			Gizmos.DrawLine(from6, to3);
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(from, vector17);
			Gizmos.DrawLine(from, vector16);
			Gizmos.DrawLine(vector16, vector18);
			Gizmos.DrawLine(vector17, vector18);
			if (_dofFarLimit >= 0f)
			{
				Gizmos.DrawLine(vector21, vector23);
				Gizmos.DrawLine(vector22, to2);
				Gizmos.DrawLine(vector21, vector22);
				Gizmos.DrawLine(vector23, to2);
				Gizmos.DrawLine(from, vector21);
				Gizmos.DrawLine(vector17, vector23);
				Gizmos.DrawLine(vector16, vector22);
				Gizmos.DrawLine(vector18, to2);
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(from2, vector19);
			Gizmos.DrawLine(from2, vector20);
			Gizmos.DrawLine(vector20, to);
			Gizmos.DrawLine(vector19, to);
			Gizmos.DrawLine(Vector3.zero, vector3);
			if (ShowBody)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.01f, 0.01f, 0.02f));
				Gizmos.DrawWireCube(Vector3.forward * -0.1f, new Vector3(0.05f, 0.08f, 0.175f));
			}
		}
	}

	public void CallUpdate()
	{
		if (_focusDist < 0f)
		{
			_focusDist = 0f;
		}
		if (_fstopIndex < 0 || _fstopIndex >= FStop.list.Length)
		{
			_fstopIndex = 0;
		}
		if (_lensFOVList == null)
		{
			ProCamsLensDataTable.FilmFormatData filmFormat = ProCamsLensDataTable.Instance.GetFilmFormat(_filmFormatName);
			if (filmFormat != null)
			{
				ProCamsLensDataTable.LensKitData lensKitData = filmFormat.GetLensKitData(_lensKitIndex);
				if (lensKitData != null)
				{
					_lensFOVList = lensKitData._fovDataset.ToArray();
				}
			}
		}
		if (_lensIndex < 0 || _lensIndex >= _lensFOVList.Length)
		{
			_lensIndex = 0;
		}
		UpdateTransforms();
		NodalCamera.fieldOfView = _lensFOVList[_lensIndex]._unityVFOV;
		UpdateDepthOfField();
	}

	private void UpdateTransforms()
	{
		if (_rigTransform != null)
		{
			base.transform.position = _rigTransform.position;
			base.transform.rotation = _rigTransform.rotation;
		}
	}

	private void UpdateDepthOfField()
	{
		CalculateDepthOfField();
		_dofComponents = _nodalCamera.GetComponentsInChildren<CSDOFScatter>();
		CSDOFScatter[] dofComponents = _dofComponents;
		foreach (CSDOFScatter cSDOFScatter in dofComponents)
		{
			cSDOFScatter.aperture = (float)_lensFOVList[_lensIndex]._focalLength / FStop.list[_fstopIndex].fstop;
			if (_unitOfMeasure == UnitOfMeasure.Imperial)
			{
				cSDOFScatter.focalLength = ProCamsUtility.Convert(_focusDist, Units.Foot, Units.Meter);
			}
			else
			{
				cSDOFScatter.focalLength = _focusDist;
			}
		}
	}

	private void CreateNodalCamera()
	{
		base.gameObject.AddComponent<Camera>();
		base.gameObject.AddComponent<CSDOFScatter>();
	}

	private void CalculateDepthOfField()
	{
		float num = ProCamsUtility.Convert(_lensFOVList[_lensIndex]._focalLength, Units.Millimeter, Units.Inch);
		float fstop = FStop.list[_fstopIndex].fstop;
		float num2 = ((_unitOfMeasure != UnitOfMeasure.Imperial) ? ProCamsUtility.Convert(_focusDist, Units.Meter, Units.Inch) : ProCamsUtility.Convert(_focusDist, Units.Foot, Units.Inch));
		float num3 = num * num / (fstop * 0.001f);
		float num4 = num3 * num2 / (num3 + (num2 - num));
		float num5 = num3 * num2 / (num3 - (num2 - num));
		float val = num5 - num4;
		val = ProCamsUtility.Convert(val, Units.Inch, Units.Foot);
		val = ProCamsUtility.Truncate(val, 2);
		num4 = ProCamsUtility.Convert(num4, Units.Inch, Units.Foot);
		num4 = ProCamsUtility.Truncate(num4, 2);
		num5 = ProCamsUtility.Convert(num5, Units.Inch, Units.Foot);
		num5 = ProCamsUtility.Truncate(num5, 2);
		_dofDistTotal = val;
		_dofNearLimit = num4;
		_dofFarLimit = num5;
		num2 = ProCamsUtility.Convert(num2, Units.Inch, Units.Meter);
	}
}
