using UnityEngine;

public class UIScoreboard : MonoBehaviour
{
	public ScoreBar scoreBar;

	public UIGrid grid;

	public UIWidget portrait;

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				if (visible)
				{
					scoreBar.gameObject.SetActive(value: true);
					scoreBar.Init();
					grid.Reposition();
					OnScoreUpdated();
					portrait.bottomAnchor.Set(portrait.bottomAnchor.target, portrait.bottomAnchor.relative, portrait.bottomAnchor.absolute - 65);
					portrait.topAnchor.Set(portrait.topAnchor.target, portrait.topAnchor.relative, portrait.topAnchor.absolute - 65);
				}
				else
				{
					scoreBar.gameObject.SetActive(value: false);
					portrait.bottomAnchor.Set(portrait.bottomAnchor.target, portrait.bottomAnchor.relative, portrait.bottomAnchor.absolute + 65);
					portrait.topAnchor.Set(portrait.topAnchor.target, portrait.topAnchor.relative, portrait.topAnchor.absolute + 65);
				}
			}
		}
	}

	protected virtual void Awake()
	{
		scoreBar.gameObject.SetActive(value: false);
		visible = false;
		Scoreboard.ScoreUpdated += OnScoreUpdated;
	}

	protected virtual void OnDestroy()
	{
		Scoreboard.ScoreUpdated -= OnScoreUpdated;
	}

	private void OnScoreUpdated()
	{
		scoreBar.UpdateScore();
	}
}
