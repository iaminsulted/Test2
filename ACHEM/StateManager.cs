using UnityEngine;

public class StateManager : MonoBehaviour
{
	private static StateManager _instance;

	public const string SCENE_LOGIN = "scene.login";

	public const string SCENE_CHARSELECT = "scene.charselect";

	public const string SCENE_CHARCREATE = "scene.charcreate";

	public const string SCENE_SERVERSELECT = "scene.serverselect";

	public const string SCENE_GAME = "scene.game";

	public const string SCENE_INITIALIZE = "scene.initialize";

	private static State currentState;

	public static StateManager Instance => _instance;

	public static State CurrentState => currentState;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	public void LoadState(string sceneName)
	{
		if (currentState != null)
		{
			currentState.Close();
			Object.Destroy(currentState.gameObject);
			currentState = null;
			Resources.UnloadUnusedAssets();
		}
		Debug.Log("Switching State to :" + sceneName);
		GameObject obj = (GameObject)Object.Instantiate(Resources.Load(sceneName));
		obj.name = sceneName;
		currentState = obj.GetComponent<State>();
		currentState.Init();
	}
}
