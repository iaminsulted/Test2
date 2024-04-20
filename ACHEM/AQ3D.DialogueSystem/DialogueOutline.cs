using System.Collections.Generic;

namespace AQ3D.DialogueSystem;

public class DialogueOutline
{
	public int ID;

	public DialogueSceneType SceneType;

	public DialogueLightingType LightingType;

	public string BackgroundImage;

	public string BackgroundPrefab;

	public BundleInfo BackgroundBundle;

	public ComVector3 scenePostionOffset;

	public ComVector3 sceneRotation;

	public string SoundTrack;

	public BundleInfo SoundTrackBundle;

	public List<DialogueCharacter> Characters = new List<DialogueCharacter>();

	public DialogueFrame[] FrameCollection;

	public string CompleteAction;
}
