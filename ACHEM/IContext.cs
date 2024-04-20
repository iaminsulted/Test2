using System.Collections.Generic;

public interface IContext
{
	void ContextSelect(int i);

	void ShowContextMenu(IContext parent, List<string> args);
}
