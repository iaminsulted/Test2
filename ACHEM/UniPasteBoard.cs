using UniPasteBoardPlugin;

public class UniPasteBoard
{
	public static string GetClipBoardString()
	{
		return UniPasteBoardPlugin.UniPasteBoard.GetClipBoardString();
	}

	public static void SetClipBoardString(string text)
	{
		UniPasteBoardPlugin.UniPasteBoard.SetClipBoardString(text);
	}
}
