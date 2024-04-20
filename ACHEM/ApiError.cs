public class ApiError
{
	public int Code;

	public string Message = "";

	public ApiError(ApiErrorCode code, string str)
	{
		Code = (int)code;
		Message = str;
	}

	public override string ToString()
	{
		return "Error " + Code + ": " + Message;
	}
}
