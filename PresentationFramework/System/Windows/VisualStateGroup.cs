using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows
{
	// Token: 0x020003DE RID: 990
	[ContentProperty("States")]
	[RuntimeNameProperty("Name")]
	public class VisualStateGroup : DependencyObject
	{
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x00199ED5 File Offset: 0x00198ED5
		// (set) Token: 0x06002997 RID: 10647 RVA: 0x00199EDD File Offset: 0x00198EDD
		public string Name { get; set; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x00199EE6 File Offset: 0x00198EE6
		public IList States
		{
			get
			{
				if (this._states == null)
				{
					this._states = new FreezableCollection<VisualState>();
				}
				return this._states;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002999 RID: 10649 RVA: 0x00199F01 File Offset: 0x00198F01
		public IList Transitions
		{
			get
			{
				if (this._transitions == null)
				{
					this._transitions = new FreezableCollection<VisualTransition>();
				}
				return this._transitions;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x00199F1C File Offset: 0x00198F1C
		// (set) Token: 0x0600299B RID: 10651 RVA: 0x00199F24 File Offset: 0x00198F24
		public VisualState CurrentState { get; internal set; }

		// Token: 0x0600299C RID: 10652 RVA: 0x00199F30 File Offset: 0x00198F30
		internal VisualState GetState(string stateName)
		{
			for (int i = 0; i < this.States.Count; i++)
			{
				VisualState visualState = (VisualState)this.States[i];
				if (visualState.Name == stateName)
				{
					return visualState;
				}
			}
			return null;
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600299D RID: 10653 RVA: 0x00199F76 File Offset: 0x00198F76
		internal Collection<Storyboard> CurrentStoryboards
		{
			get
			{
				if (this._currentStoryboards == null)
				{
					this._currentStoryboards = new Collection<Storyboard>();
				}
				return this._currentStoryboards;
			}
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x00199F94 File Offset: 0x00198F94
		internal void StartNewThenStopOld(FrameworkElement element, params Storyboard[] newStoryboards)
		{
			for (int i = 0; i < this.CurrentStoryboards.Count; i++)
			{
				if (this.CurrentStoryboards[i] != null)
				{
					this.CurrentStoryboards[i].Remove(element);
				}
			}
			this.CurrentStoryboards.Clear();
			for (int j = 0; j < newStoryboards.Length; j++)
			{
				if (newStoryboards[j] != null)
				{
					newStoryboards[j].Begin(element, HandoffBehavior.SnapshotAndReplace, true);
					this.CurrentStoryboards.Add(newStoryboards[j]);
				}
			}
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0019A00E File Offset: 0x0019900E
		internal void RaiseCurrentStateChanging(FrameworkElement stateGroupsRoot, VisualState oldState, VisualState newState, FrameworkElement control)
		{
			if (this.CurrentStateChanging != null)
			{
				this.CurrentStateChanging(stateGroupsRoot, new VisualStateChangedEventArgs(oldState, newState, control, stateGroupsRoot));
			}
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x0019A02E File Offset: 0x0019902E
		internal void RaiseCurrentStateChanged(FrameworkElement stateGroupsRoot, VisualState oldState, VisualState newState, FrameworkElement control)
		{
			if (this.CurrentStateChanged != null)
			{
				this.CurrentStateChanged(stateGroupsRoot, new VisualStateChangedEventArgs(oldState, newState, control, stateGroupsRoot));
			}
		}

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x060029A1 RID: 10657 RVA: 0x0019A050 File Offset: 0x00199050
		// (remove) Token: 0x060029A2 RID: 10658 RVA: 0x0019A088 File Offset: 0x00199088
		public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanged;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060029A3 RID: 10659 RVA: 0x0019A0C0 File Offset: 0x001990C0
		// (remove) Token: 0x060029A4 RID: 10660 RVA: 0x0019A0F8 File Offset: 0x001990F8
		public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanging;

		// Token: 0x04001501 RID: 5377
		private Collection<Storyboard> _currentStoryboards;

		// Token: 0x04001502 RID: 5378
		private FreezableCollection<VisualState> _states;

		// Token: 0x04001503 RID: 5379
		private FreezableCollection<VisualTransition> _transitions;
	}
}
