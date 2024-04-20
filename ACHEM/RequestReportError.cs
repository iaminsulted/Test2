public class RequestReportError : Request
{
	public int UserID;

	public int CharID;

	public int ClientID;

	public int DevBuild;

	public string ClientDisplayVersion;

	public string DeviceUniqueIdentifier;

	public string Title;

	public string Message;

	public string StackTrace;

	public string Context;

	public string Form;

	public RequestReportError()
	{
		type = 27;
		cmd = 3;
		Title = "";
		Message = "";
		StackTrace = "";
		Context = "";
		Form = "";
	}
}
