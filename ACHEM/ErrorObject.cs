public class ErrorObject
{
	private string title;

	private string message;

	private string stackTrace;

	private string form;

	private string context;

	public string Title => title;

	public string Message => message;

	public string StackTrace => stackTrace;

	public string Context => context;

	public string Form => form;

	public ErrorObject(string title, string message, string stackTrace, string context, string form)
	{
		this.title = ((title != null) ? title : "");
		this.message = ((message != null) ? message : "");
		this.stackTrace = ((stackTrace != null) ? stackTrace : "");
		this.context = ((context != null) ? context : "");
		this.form = ((form != null) ? form : "");
	}
}
