using UnityEngine;

public interface ITrackable
{
	Transform TrackedTransform { get; }

	void Track(IndicatorFX indicatorFX);

	void Untrack();
}
