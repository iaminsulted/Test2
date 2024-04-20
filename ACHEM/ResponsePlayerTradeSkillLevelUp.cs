using StatCurves;

public class ResponsePlayerTradeSkillLevelUp : Response
{
	public int playerID;

	public TradeSkillType tradeSkillType;

	public int level;

	public int xp;

	public int xpToLevel;

	public ResponsePlayerTradeSkillLevelUp()
	{
		type = 17;
		cmd = 29;
	}
}
