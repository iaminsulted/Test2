using System.Collections.Generic;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseMail : Response
{
	public List<MailMessage> mailbox;

	public List<RewardItem> rewards;

	public List<Item> rewardItems;

	public ResponseMail()
	{
		type = 51;
		cmd = 1;
	}
}
