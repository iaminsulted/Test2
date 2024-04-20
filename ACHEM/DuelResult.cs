public class DuelResult
{
	private bool isWinner;

	public DuelResult(bool isWinner)
	{
		this.isWinner = isWinner;
	}

	public void Show()
	{
		DuelEnd.Show(isWinner);
		Player me = Entities.Instance.me;
		Player playerById = Entities.Instance.GetPlayerById(me.DuelOpponentID);
		if (playerById != null)
		{
			playerById.DuelOpponentID = -1;
			playerById.RefreshReaction();
		}
		me.DuelOpponentID = -1;
		me.CheckPvpState();
		Session.MyPlayerData.duelResult = null;
	}
}
