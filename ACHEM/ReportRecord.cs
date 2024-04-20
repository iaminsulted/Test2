using System;

public class ReportRecord
{
	public string user;

	public DateTime reportTime;

	public ReportRecord(string user, DateTime reportTime)
	{
		this.user = user;
		this.reportTime = reportTime;
	}
}
