using System;

public class MailMessage
{
	public int id;

	public int charid;

	public DateTime startDate;

	public DateTime endDate;

	public string message;

	public int itemReward;

	public int goldReward;

	public int dcReward;

	public bool hasSeen;

	public bool favorited;

	public bool hasRedeemed;

	public bool hasDeleted;

	public string subject;

	public string sender;

	public string icon;

	public MailMessage()
	{
	}

	public MailMessage(int charid, string subject, string sender, string message)
	{
		this.charid = charid;
		this.subject = subject;
		this.sender = sender;
		this.message = message;
	}

	public MailMessage(int id, DateTime startDate, DateTime endDate, string message, int itemReward, int goldReward, int dcReward, bool hasSeen, bool favorited, bool hasRedeemed, string subject, string sender, string icon, bool hasDeleted)
		: this(id, startDate, endDate, message, itemReward, goldReward, dcReward, hasSeen, favorited, hasRedeemed, hasDeleted)
	{
		this.subject = subject;
		this.sender = sender;
		this.icon = icon;
	}

	public MailMessage(int id, DateTime startDate, DateTime endDate, string message, int itemReward, int goldReward, int dcReward, bool hasSeen, bool favorited, bool hasRedeemed, bool hasDeleted)
		: this(id, startDate, endDate, message, itemReward, goldReward, dcReward)
	{
		this.id = id;
		this.startDate = startDate;
		this.endDate = endDate;
		this.message = message;
		this.itemReward = itemReward;
		this.goldReward = goldReward;
		this.dcReward = dcReward;
		this.hasSeen = hasSeen;
		this.favorited = favorited;
		this.hasRedeemed = hasRedeemed;
		this.hasDeleted = hasDeleted;
	}

	public MailMessage(int id, DateTime startDate, DateTime endDate, string message, int itemReward, int goldReward, int dcReward)
	{
		this.id = id;
		this.startDate = startDate;
		this.endDate = endDate;
		this.message = message;
		this.itemReward = itemReward;
		this.goldReward = goldReward;
		this.dcReward = dcReward;
	}
}
