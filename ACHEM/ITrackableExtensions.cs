using UnityEngine;

public static class ITrackableExtensions
{
	public static bool IsNull(this ITrackable trackable)
	{
		if (trackable != null)
		{
			if (trackable is Component)
			{
				return trackable as Component == null;
			}
			return false;
		}
		return true;
	}
}
