using CinemaProCams;
using UnityEngine;

[RequireComponent(typeof(CameraBody))]
[ExecuteInEditMode]
public class Stereo3D : MonoBehaviour
{
	private CameraBody _cameraBody;

	private RenderTexture _leftCamRT;

	private RenderTexture _rightCamRT;

	private GameObject _leftCam;

	private GameObject _rightCam;

	[SerializeField]
	private int _selectedRig;

	[SerializeField]
	private Material _stereoMaterial;

	[SerializeField]
	private float _minInteraxial;

	[SerializeField]
	private float _maxInteraxial;

	[SerializeField]
	private float _interaxial;

	[SerializeField]
	private float _minConvergence;

	[SerializeField]
	private float _maxConvergence;

	[SerializeField]
	private float _convergence;

	[SerializeField]
	private StereoState _sstate;

	[SerializeField]
	private int _targetFrameRate;

	private GameObject _trackObject;

	public bool ShowBody;

	public CameraBody CB => _cameraBody;

	public int SelectedRig
	{
		get
		{
			return _selectedRig;
		}
		set
		{
			_selectedRig = value;
		}
	}

	public Material StereoMaterial => _stereoMaterial;

	public float MinInteraxial => _minInteraxial;

	public float MaxInteraxial => _maxInteraxial;

	public float Interaxial
	{
		get
		{
			return _interaxial;
		}
		set
		{
			_interaxial = value;
		}
	}

	public float MinConvergence => _minConvergence;

	public float MaxConvergence => _maxConvergence;

	public float Convergence
	{
		get
		{
			return _convergence;
		}
		set
		{
			_convergence = value;
		}
	}

	public StereoState State
	{
		get
		{
			return _sstate;
		}
		set
		{
			_sstate = value;
		}
	}

	public int TargetFrameRate { get; set; }

	private void Awake()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		_cameraBody = GetComponent<CameraBody>();
		foreach (Transform item in _cameraBody.NodalCamera.transform)
		{
			if (!flag && (flag = item.name == "left_cam"))
			{
				_leftCam = item.gameObject;
			}
			if (!flag2 && (flag2 = item.name == "right_cam"))
			{
				_rightCam = item.gameObject;
			}
			if (!flag3 && (flag3 = item.name == "track"))
			{
				_trackObject = item.gameObject;
			}
		}
		if (!flag)
		{
			_leftCam = new GameObject("left_cam", typeof(Camera), typeof(CSDOFScatter));
		}
		if (!flag2)
		{
			_rightCam = new GameObject("right_cam", typeof(Camera), typeof(CSDOFScatter));
		}
		if (!flag3)
		{
			_trackObject = new GameObject("track");
		}
		_leftCam.transform.parent = _cameraBody.NodalCamera.transform;
		_rightCam.transform.parent = _cameraBody.NodalCamera.transform;
		_trackObject.transform.parent = _cameraBody.NodalCamera.transform;
		_leftCam.transform.position = Vector3.zero;
		_rightCam.transform.position = Vector3.zero;
		if (_leftCamRT != null)
		{
			_leftCamRT.Release();
		}
		if (_rightCamRT != null)
		{
			_rightCamRT.Release();
		}
		_leftCamRT = new RenderTexture(Screen.width, Screen.height, 24);
		_rightCamRT = new RenderTexture(Screen.width, Screen.height, 24);
	}

	private void Start()
	{
		SetMode(_sstate);
	}

	private void OnApplicationQuit()
	{
		_leftCamRT.Release();
		_rightCamRT.Release();
	}

	public void SetDefault()
	{
		Camera nodalCamera = _cameraBody.NodalCamera;
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.renderingPath = nodalCamera.renderingPath;
		component2.renderingPath = nodalCamera.renderingPath;
	}

	public void SetTargetTextures(RenderTexture l, RenderTexture r)
	{
		if (_leftCamRT.width != Screen.width || _leftCamRT.height != Screen.height)
		{
			_leftCamRT.Release();
			_leftCamRT = new RenderTexture(Screen.width, Screen.height, 24);
		}
		if (_rightCamRT.width != Screen.width || _rightCamRT.height != Screen.height)
		{
			_rightCamRT.Release();
			_rightCamRT = new RenderTexture(Screen.width, Screen.height, 24);
		}
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.targetTexture = _leftCamRT;
		component2.targetTexture = _rightCamRT;
		_stereoMaterial.SetTexture("_LeftTex", _rightCamRT);
		_stereoMaterial.SetTexture("_RightTex", _leftCamRT);
	}

	public void SetParentCamera()
	{
		_leftCam.transform.parent = _cameraBody.NodalCamera.transform;
		_rightCam.transform.parent = _cameraBody.NodalCamera.transform;
		_trackObject.transform.parent = _cameraBody.NodalCamera.transform;
		_leftCam.transform.position = Vector3.zero;
		_rightCam.transform.position = Vector3.zero;
		Camera nodalCamera = _cameraBody.NodalCamera;
		nodalCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		nodalCamera.clearFlags = CameraClearFlags.Nothing;
	}

	private void SetInterlace()
	{
		SetDefault();
		SetTargetTextures(_leftCamRT, _rightCamRT);
		Camera nodalCamera = _cameraBody.NodalCamera;
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.depth = nodalCamera.depth - 2f;
		component2.depth = nodalCamera.depth - 1f;
		component.enabled = true;
		component2.enabled = true;
		component.rect = new Rect(0f, 0f, 1f, 1f);
		component2.rect = new Rect(0f, 0f, 1f, 1f);
		UpdateView();
		SetParentCamera();
	}

	private void SetReversedInterlace()
	{
		SetDefault();
		SetTargetTextures(_rightCamRT, _leftCamRT);
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.targetTexture = _leftCamRT;
		component2.targetTexture = _rightCamRT;
		_stereoMaterial.SetTexture("_LeftTex", _leftCamRT);
		_stereoMaterial.SetTexture("_RightTex", _rightCamRT);
		Camera nodalCamera = _cameraBody.NodalCamera;
		component.depth = nodalCamera.depth - 2f;
		component2.depth = nodalCamera.depth - 1f;
		component.enabled = true;
		component2.enabled = true;
		component.rect = new Rect(0f, 0f, 1f, 1f);
		component2.rect = new Rect(0f, 0f, 1f, 1f);
		UpdateView();
		SetParentCamera();
	}

	private void SetAnaglyph()
	{
		SetDefault();
		SetTargetTextures(_leftCamRT, _rightCamRT);
		_stereoMaterial.SetVector("_Balance_Left_R", new Vector4(1f, 0f, 0f, 0f));
		_stereoMaterial.SetVector("_Balance_Right_G", new Vector4(0f, 1f, 0f, 0f));
		_stereoMaterial.SetVector("_Balance_Right_B", new Vector4(0f, 0f, 1f, 0f));
		Camera nodalCamera = _cameraBody.NodalCamera;
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.depth = nodalCamera.depth - 2f;
		component2.depth = nodalCamera.depth - 1f;
		component.enabled = true;
		component2.enabled = true;
		component.rect = new Rect(0f, 0f, 1f, 1f);
		component2.rect = new Rect(0f, 0f, 1f, 1f);
		UpdateView();
		SetParentCamera();
	}

	private void SetShutter()
	{
		SetDefault();
		Camera nodalCamera = _cameraBody.NodalCamera;
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.targetTexture = null;
		component2.targetTexture = null;
		component.depth = nodalCamera.depth + 1f;
		component2.depth = nodalCamera.depth + 1f;
		component.enabled = true;
		component2.enabled = false;
		component.rect = new Rect(0f, 0f, 1f, 1f);
		component2.rect = new Rect(0f, 0f, 1f, 1f);
		UpdateView();
		SetParentCamera();
		QualitySettings.vSyncCount = 1;
	}

	private void SetSideBySide(bool isReversed)
	{
		SetDefault();
		Camera nodalCamera = _cameraBody.NodalCamera;
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.targetTexture = null;
		component2.targetTexture = null;
		component.depth = nodalCamera.depth + 1f;
		component2.depth = nodalCamera.depth + 1f;
		component.enabled = true;
		component2.enabled = false;
		if (!isReversed)
		{
			component.rect = new Rect(0f, 0f, 0.5f, 1f);
			component2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
		}
		else
		{
			component.rect = new Rect(0.5f, 0f, 0.5f, 1f);
			component2.rect = new Rect(0f, 0f, 0.5f, 1f);
		}
		UpdateView();
		SetParentCamera();
	}

	private void SetTopBottom(bool isReversed)
	{
		SetDefault();
		Camera nodalCamera = _cameraBody.NodalCamera;
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		component.targetTexture = null;
		component2.targetTexture = null;
		component.depth = nodalCamera.depth + 1f;
		component2.depth = nodalCamera.depth + 1f;
		component.enabled = true;
		component2.enabled = false;
		if (isReversed)
		{
			component.rect = new Rect(0f, 0f, 1f, 0.5f);
			component2.rect = new Rect(0f, 0.5f, 1f, 0.5f);
		}
		else
		{
			component.rect = new Rect(0f, 0.5f, 1f, 0.5f);
			component2.rect = new Rect(0f, 0f, 1f, 0.5f);
		}
		UpdateView();
		SetParentCamera();
	}

	private void OnDrawGizmos()
	{
		if (ShowBody)
		{
			Gizmos.color = Color.red;
			Gizmos.matrix = Matrix4x4.TRS(_leftCam.transform.position, _leftCam.transform.rotation, _leftCam.transform.lossyScale);
			Gizmos.DrawLine(Vector3.zero, Vector3.forward * 60f);
			Gizmos.DrawWireSphere(Vector3.zero, 0.01f);
			Gizmos.DrawWireCube(Vector3.back * 0.06f, new Vector3(0.03f, 0.04f, 0.1f));
			Gizmos.matrix = Matrix4x4.TRS(_rightCam.transform.position, _rightCam.transform.rotation, _rightCam.transform.lossyScale);
			Gizmos.DrawLine(Vector3.zero, Vector3.forward * 60f);
			Gizmos.DrawWireSphere(Vector3.zero, 0.01f);
			Gizmos.DrawWireCube(Vector3.back * 0.06f, new Vector3(0.03f, 0.04f, 0.1f));
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.2f, 0.1f, 0.2f));
		}
	}

	private void Update()
	{
		SetMode(_sstate);
		CallUpdate();
	}

	private void CallUpdate()
	{
		SelectRig();
		Controls();
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		switch (_sstate)
		{
		case StereoState.Interlace:
			CameraUpdate();
			break;
		case StereoState.ReversedInterlace:
			CameraUpdate();
			break;
		case StereoState.SideBySide:
			component2.enabled = true;
			component.enabled = true;
			break;
		case StereoState.ReversedSideBySide:
			component2.enabled = true;
			component.enabled = true;
			break;
		case StereoState.TopBottom:
			component2.enabled = true;
			component.enabled = true;
			break;
		case StereoState.ReversedTopBottom:
			component2.enabled = true;
			component.enabled = true;
			break;
		case StereoState.Anaglyph:
			CameraUpdate();
			break;
		}
	}

	private void SelectRig()
	{
		switch (_selectedRig)
		{
		case 0:
			_minInteraxial = 0f;
			_maxInteraxial = 0.07f;
			_minConvergence = -5f;
			_maxConvergence = 3f;
			break;
		case 1:
			_minInteraxial = 0f;
			_maxInteraxial = 0.1f;
			_minConvergence = -1f;
			_maxConvergence = 3f;
			break;
		case 2:
			_minInteraxial = 0f;
			_maxInteraxial = 0.2f;
			_minConvergence = 0f;
			_maxConvergence = 3f;
			break;
		}
	}

	private void Controls()
	{
		if (_interaxial < _maxInteraxial && Input.GetKey(KeyCode.E))
		{
			_interaxial += Time.deltaTime * 0.1f;
			if (_interaxial > _maxInteraxial)
			{
				_interaxial = _maxInteraxial;
			}
		}
		if (_interaxial > _minInteraxial && Input.GetKey(KeyCode.Q))
		{
			_interaxial -= Time.deltaTime * 0.1f;
			if (_interaxial < _minInteraxial)
			{
				_interaxial = _minInteraxial;
			}
		}
		if (_convergence < _maxConvergence && Input.GetKey(KeyCode.LeftShift))
		{
			_convergence += Time.deltaTime * 2f;
			if (_convergence > _maxConvergence)
			{
				_convergence = _maxConvergence;
			}
		}
		if (_convergence > _minConvergence && Input.GetKey(KeyCode.LeftControl))
		{
			_convergence -= Time.deltaTime * 2f;
			if (_convergence < _minConvergence)
			{
				_convergence = _minConvergence;
			}
		}
		if (Input.GetKey(KeyCode.N))
		{
			_cameraBody.FocusDistance += 10f * Time.deltaTime;
		}
		if (_cameraBody.FocusDistance > 0f && Input.GetKey(KeyCode.B))
		{
			_cameraBody.FocusDistance -= 10f * Time.deltaTime;
		}
	}

	private void CameraUpdate()
	{
		_leftCam.GetComponent<Camera>().fieldOfView = _cameraBody.NodalCamera.fieldOfView;
		_leftCam.GetComponent<Camera>().nearClipPlane = _cameraBody.NodalCamera.nearClipPlane;
		_leftCam.GetComponent<Camera>().farClipPlane = _cameraBody.NodalCamera.farClipPlane;
		_rightCam.GetComponent<Camera>().fieldOfView = _cameraBody.NodalCamera.fieldOfView;
		_rightCam.GetComponent<Camera>().nearClipPlane = _cameraBody.NodalCamera.nearClipPlane;
		_rightCam.GetComponent<Camera>().farClipPlane = _cameraBody.NodalCamera.farClipPlane;
	}

	private void Converge()
	{
		Vector3 currentVelocity = new Vector3(0f, 0f, 10f);
		_trackObject.transform.position = Vector3.SmoothDamp(_trackObject.transform.position, base.transform.position + base.transform.forward * _cameraBody.FocusDistance, ref currentVelocity, 0.15f);
	}

	private void LateUpdate()
	{
		UpdateView();
	}

	private void UpdateView()
	{
		Camera component = _leftCam.GetComponent<Camera>();
		Camera component2 = _rightCam.GetComponent<Camera>();
		_leftCam.transform.parent = _cameraBody.NodalCamera.transform;
		_rightCam.transform.parent = _cameraBody.NodalCamera.transform;
		_leftCam.transform.position = _cameraBody.NodalCamera.transform.position + Vector3.Cross(_cameraBody.NodalCamera.transform.forward, _cameraBody.NodalCamera.transform.up) * (_interaxial / 2f);
		_rightCam.transform.position = _cameraBody.NodalCamera.transform.position + -Vector3.Cross(_cameraBody.NodalCamera.transform.forward, _cameraBody.NodalCamera.transform.up) * (_interaxial / 2f);
		component.projectionMatrix = _cameraBody.NodalCamera.projectionMatrix;
		component2.projectionMatrix = _cameraBody.NodalCamera.projectionMatrix;
		if (_convergence > _maxConvergence)
		{
			_convergence = _maxConvergence;
		}
		if (_convergence < _minConvergence)
		{
			_convergence = _minConvergence;
		}
		_leftCam.transform.localRotation = Quaternion.identity;
		_leftCam.transform.Rotate(_leftCam.transform.up, _convergence);
		_rightCam.transform.localRotation = Quaternion.identity;
		_rightCam.transform.Rotate(_rightCam.transform.up, 0f - _convergence);
	}

	public void SetMode(StereoState state)
	{
		if (!_stereoMaterial)
		{
			_stereoMaterial = (Material)Resources.Load("Stereo3D");
		}
		_sstate = state;
		switch (state)
		{
		case StereoState.Interlace:
			SetInterlace();
			GetComponentInChildren<Stereo3DRenderer>().enabled = true;
			break;
		case StereoState.ReversedInterlace:
			SetReversedInterlace();
			GetComponentInChildren<Stereo3DRenderer>().enabled = true;
			break;
		case StereoState.SideBySide:
			SetSideBySide(isReversed: false);
			GetComponentInChildren<Stereo3DRenderer>().enabled = false;
			break;
		case StereoState.ReversedSideBySide:
			SetSideBySide(isReversed: true);
			GetComponentInChildren<Stereo3DRenderer>().enabled = false;
			break;
		case StereoState.TopBottom:
			SetTopBottom(isReversed: false);
			GetComponentInChildren<Stereo3DRenderer>().enabled = false;
			break;
		case StereoState.ReversedTopBottom:
			SetTopBottom(isReversed: true);
			GetComponentInChildren<Stereo3DRenderer>().enabled = false;
			break;
		case StereoState.Anaglyph:
			SetAnaglyph();
			GetComponentInChildren<Stereo3DRenderer>().enabled = true;
			break;
		}
	}
}
