internal struct TimeStamp
{
	public float timeSent;

	public float timeArrived;

	public int ID;

	public TimeStamp(float timeSent, int ID)
	{
		this.timeSent = timeSent;
		this.ID = ID;
		timeArrived = -1f;
	}

	public float GetPing()
	{
		return timeArrived - timeSent;
	}
}
