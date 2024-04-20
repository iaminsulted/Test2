using System.Collections.Generic;

public class ResponseScoreSync : Response
{
	public Dictionary<int, int> teamScores;

	public ResponseScoreSync(Dictionary<int, int> teamScores)
	{
		type = 8;
		cmd = 5;
		this.teamScores = teamScores;
	}
}
