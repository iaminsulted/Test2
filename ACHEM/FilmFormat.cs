public class FilmFormat
{
	public string name;

	public float filmWidth;

	public float filmHeight;

	public readonly float aspectRatio;

	public ScreenSize[] screenSizes;

	public FilmFormat(string name, float filmWidth, float filmHeight, float aspectRatio, ScreenSize[] screenSizes)
	{
		this.name = name;
		this.filmWidth = filmWidth;
		this.filmHeight = filmHeight;
		this.aspectRatio = aspectRatio;
		this.screenSizes = screenSizes;
	}
}
