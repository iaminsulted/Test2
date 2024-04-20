internal class ResponsePvpMatchEnd : Response
{
	public PvpMatchInfo matchInfo;

	public PvpRewardInfo rewardInfo;

	public ResponsePvpMatchEnd()
	{
		type = 20;
		cmd = 13;
	}

	public ResponsePvpMatchEnd(PvpMatchInfo matchInfo, PvpRewardInfo rewardInfo)
	{
		type = 20;
		cmd = 13;
		this.matchInfo = matchInfo;
		this.rewardInfo = rewardInfo;
	}
}
