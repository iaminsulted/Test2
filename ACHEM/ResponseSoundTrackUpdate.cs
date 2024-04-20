internal class ResponseSoundTrackUpdate : Response
{
	public int soundTrackID;

	public ResponseSoundTrackUpdate()
	{
		type = 8;
		cmd = 6;
	}

	public ResponseSoundTrackUpdate(int soundTrackID)
	{
		type = 8;
		cmd = 6;
		this.soundTrackID = soundTrackID;
	}
}
