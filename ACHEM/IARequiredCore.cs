using System;

public class IARequiredCore
{
	public event Action Updated;

	protected void OnRequirementUpdate()
	{
		if (this.Updated != null)
		{
			this.Updated();
		}
	}

	public virtual void SerializationSetup()
	{
	}

	public virtual bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return true;
	}
}
