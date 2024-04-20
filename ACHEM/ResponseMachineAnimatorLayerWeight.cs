public class ResponseMachineAnimatorLayerWeight : Response
{
	public int npcID;

	public int layerID;

	public float crossfade;

	public float weight;

	public ResponseMachineAnimatorLayerWeight(int npcID, int layerID, float crossfade, float weight)
	{
		type = 19;
		cmd = 8;
		this.npcID = npcID;
		this.layerID = layerID;
		this.crossfade = crossfade;
		this.weight = weight;
	}
}
