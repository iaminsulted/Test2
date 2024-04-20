using System.IO;

namespace CinemaSuite.Utility;

public class Files
{
	public const string ERROR_FORMAT_DUPLICATE_FILE_NAME = "The file {0}.{2} already exists in target folder. Saved as {1}.{2}.";

	public static string GetUniqueFilename(string folder, string filename, string extension)
	{
		int num = 1;
		while (File.Exists($"{folder}/{filename}{num}.{extension}"))
		{
			num++;
		}
		return $"{filename}{num}";
	}
}
