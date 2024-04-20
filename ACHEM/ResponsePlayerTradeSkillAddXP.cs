using StatCurves;

public class ResponsePlayerTradeSkillAddXP : Response
{
	public TradeSkillType tradeSkillType;

	public int xpFinal;

	public int xpGained;

	public ResponsePlayerTradeSkillAddXP()
	{
		type = 17;
		cmd = 28;
	}

	public ResponsePlayerTradeSkillAddXP(TradeSkillType tradeSkillType, int xpFinal, int xpGained)
	{
		type = 17;
		cmd = 28;
		this.tradeSkillType = tradeSkillType;
		this.xpFinal = xpFinal;
		this.xpGained = xpGained;
	}
}
