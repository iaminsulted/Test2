using System.Collections.Generic;

public interface IiconsContext
{
	void ContextSelect(int i);

	void ShowContextMenu(IiconsContext parent, List<string> args, List<UIContextIconsMenu.IconImg> imgIcons);
}
