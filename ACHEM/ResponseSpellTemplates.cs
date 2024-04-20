using System.Collections.Generic;

public class ResponseSpellTemplates : Response
{
	public List<SpellTemplate> list = new List<SpellTemplate>();

	public ResponseSpellTemplates()
	{
		type = 14;
	}
}
