internal class ResponsePvpMatchSoundTrackCountdown : Response
{
	public int soundTrackID;

	public ResponsePvpMatchSoundTrackCountdown()
	{
		type = 20;
		cmd = 11;
	}

	public ResponsePvpMatchSoundTrackCountdown(int soundTrackID)
	{
		type = 20;
		cmd = 11;
		this.soundTrackID = soundTrackID;
	}
}
