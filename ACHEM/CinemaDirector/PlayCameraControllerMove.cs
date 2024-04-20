using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Camera Controller Move", "Play", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class PlayCameraControllerMove : CinemaActorEvent
{
	private CameraController cameraController;

	public void Awake()
	{
		cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
	}

	public override void Trigger(GameObject actor)
	{
		if (cameraController != null)
		{
			cameraController.MoveInFrontOfObjects();
		}
	}
}
