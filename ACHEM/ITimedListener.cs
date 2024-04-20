public interface ITimedListener
{
	float LastServerTimestamp { get; }

	void Init();

	void SyncToServer(bool last);
}
