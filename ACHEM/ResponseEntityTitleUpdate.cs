public class ResponseEntityTitleUpdate : Response
{
	public int EntityID;

	public int Title;

	public string TitleName;

	public ResponseEntityTitleUpdate()
	{
		type = 17;
		cmd = 17;
	}

	public ResponseEntityTitleUpdate(int entityID, int title, string titleName)
	{
		type = 17;
		cmd = 17;
		EntityID = entityID;
		Title = title;
		TitleName = titleName;
	}
}
