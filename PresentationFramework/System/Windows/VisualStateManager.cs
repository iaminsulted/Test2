using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003DF RID: 991
	public class VisualStateManager : DependencyObject
	{
		// Token: 0x060029A6 RID: 10662 RVA: 0x0019A130 File Offset: 0x00199130
		private static bool GoToStateCommon(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, bool useTransitions)
		{
			if (stateName == null)
			{
				throw new ArgumentNullException("stateName");
			}
			if (stateGroupsRoot == null)
			{
				return false;
			}
			IList<VisualStateGroup> visualStateGroupsInternal = VisualStateManager.GetVisualStateGroupsInternal(stateGroupsRoot);
			if (visualStateGroupsInternal == null)
			{
				return false;
			}
			VisualStateGroup group;
			VisualState visualState;
			VisualStateManager.TryGetState(visualStateGroupsInternal, stateName, out group, out visualState);
			VisualStateManager customVisualStateManager = VisualStateManager.GetCustomVisualStateManager(stateGroupsRoot);
			if (customVisualStateManager != null)
			{
				return customVisualStateManager.GoToStateCore(control, stateGroupsRoot, stateName, group, visualState, useTransitions);
			}
			return visualState != null && VisualStateManager.GoToStateInternal(control, stateGroupsRoot, group, visualState, useTransitions);
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x0019A190 File Offset: 0x00199190
		public static bool GoToState(FrameworkElement control, string stateName, bool useTransitions)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			FrameworkElement stateGroupsRoot = control.StateGroupsRoot;
			return VisualStateManager.GoToStateCommon(control, stateGroupsRoot, stateName, useTransitions);
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x0019A1BB File Offset: 0x001991BB
		public static bool GoToElementState(FrameworkElement stateGroupsRoot, string stateName, bool useTransitions)
		{
			if (stateGroupsRoot == null)
			{
				throw new ArgumentNullException("stateGroupsRoot");
			}
			return VisualStateManager.GoToStateCommon(null, stateGroupsRoot, stateName, useTransitions);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x0019A1D4 File Offset: 0x001991D4
		protected virtual bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			return VisualStateManager.GoToStateInternal(control, stateGroupsRoot, group, state, useTransitions);
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x0019A1E3 File Offset: 0x001991E3
		public static VisualStateManager GetCustomVisualStateManager(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return obj.GetValue(VisualStateManager.CustomVisualStateManagerProperty) as VisualStateManager;
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x0019A203 File Offset: 0x00199203
		public static void SetCustomVisualStateManager(FrameworkElement obj, VisualStateManager value)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			obj.SetValue(VisualStateManager.CustomVisualStateManagerProperty, value);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x0019A220 File Offset: 0x00199220
		internal static Collection<VisualStateGroup> GetVisualStateGroupsInternal(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			bool flag;
			if (obj.GetValueSource(VisualStateManager.VisualStateGroupsProperty, null, out flag) != BaseValueSourceInternal.Default)
			{
				return obj.GetValue(VisualStateManager.VisualStateGroupsProperty) as Collection<VisualStateGroup>;
			}
			return null;
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x0019A25E File Offset: 0x0019925E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static IList GetVisualStateGroups(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return obj.GetValue(VisualStateManager.VisualStateGroupsProperty) as IList;
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x0019A280 File Offset: 0x00199280
		internal static bool TryGetState(IList<VisualStateGroup> groups, string stateName, out VisualStateGroup group, out VisualState state)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				VisualStateGroup visualStateGroup = groups[i];
				VisualState state2 = visualStateGroup.GetState(stateName);
				if (state2 != null)
				{
					group = visualStateGroup;
					state = state2;
					return true;
				}
			}
			group = null;
			state = null;
			return false;
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x0019A2C0 File Offset: 0x001992C0
		private static bool GoToStateInternal(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (stateGroupsRoot == null)
			{
				throw new ArgumentNullException("stateGroupsRoot");
			}
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			if (group == null)
			{
				throw new InvalidOperationException();
			}
			VisualState lastState = group.CurrentState;
			if (lastState == state)
			{
				return true;
			}
			VisualTransition transition = useTransitions ? VisualStateManager.GetTransition(stateGroupsRoot, group, lastState, state) : null;
			Storyboard storyboard = VisualStateManager.GenerateDynamicTransitionAnimations(stateGroupsRoot, group, state, transition);
			if (transition == null || (transition.GeneratedDuration == VisualStateManager.DurationZero && (transition.Storyboard == null || transition.Storyboard.Duration == VisualStateManager.DurationZero)))
			{
				if (transition != null && transition.Storyboard != null)
				{
					group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
					{
						transition.Storyboard,
						state.Storyboard
					});
				}
				else
				{
					group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
					{
						state.Storyboard
					});
				}
				group.RaiseCurrentStateChanging(stateGroupsRoot, lastState, state, control);
				group.RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
			}
			else
			{
				transition.DynamicStoryboardCompleted = false;
				storyboard.Completed += delegate(object sender, EventArgs e)
				{
					if (transition.Storyboard == null || transition.ExplicitStoryboardCompleted)
					{
						if (VisualStateManager.ShouldRunStateStoryboard(control, stateGroupsRoot, state, group))
						{
							group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
							{
								state.Storyboard
							});
						}
						group.RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
					}
					transition.DynamicStoryboardCompleted = true;
				};
				if (transition.Storyboard != null && transition.ExplicitStoryboardCompleted)
				{
					EventHandler transitionCompleted = null;
					transitionCompleted = delegate(object sender, EventArgs e)
					{
						if (transition.DynamicStoryboardCompleted)
						{
							if (VisualStateManager.ShouldRunStateStoryboard(control, stateGroupsRoot, state, group))
							{
								group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
								{
									state.Storyboard
								});
							}
							group.RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
						}
						transition.Storyboard.Completed -= transitionCompleted;
						transition.ExplicitStoryboardCompleted = true;
					};
					transition.ExplicitStoryboardCompleted = false;
					transition.Storyboard.Completed += transitionCompleted;
				}
				group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
				{
					transition.Storyboard,
					storyboard
				});
				group.RaiseCurrentStateChanging(stateGroupsRoot, lastState, state, control);
			}
			group.CurrentState = state;
			return true;
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0019A57C File Offset: 0x0019957C
		private static bool ShouldRunStateStoryboard(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualState state, VisualStateGroup group)
		{
			bool flag = true;
			bool flag2 = true;
			if (control != null && !control.IsVisible)
			{
				flag = (PresentationSource.CriticalFromVisual(control) != null);
			}
			if (stateGroupsRoot != null && !stateGroupsRoot.IsVisible)
			{
				flag2 = (PresentationSource.CriticalFromVisual(stateGroupsRoot) != null);
			}
			return flag && flag2 && state == group.CurrentState;
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0019A5C7 File Offset: 0x001995C7
		protected void RaiseCurrentStateChanging(VisualStateGroup stateGroup, VisualState oldState, VisualState newState, FrameworkElement control, FrameworkElement stateGroupsRoot)
		{
			if (stateGroup == null)
			{
				throw new ArgumentNullException("stateGroup");
			}
			if (newState == null)
			{
				throw new ArgumentNullException("newState");
			}
			if (stateGroupsRoot == null)
			{
				return;
			}
			stateGroup.RaiseCurrentStateChanging(stateGroupsRoot, oldState, newState, control);
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x0019A5F6 File Offset: 0x001995F6
		protected void RaiseCurrentStateChanged(VisualStateGroup stateGroup, VisualState oldState, VisualState newState, FrameworkElement control, FrameworkElement stateGroupsRoot)
		{
			if (stateGroup == null)
			{
				throw new ArgumentNullException("stateGroup");
			}
			if (newState == null)
			{
				throw new ArgumentNullException("newState");
			}
			if (stateGroupsRoot == null)
			{
				return;
			}
			stateGroup.RaiseCurrentStateChanged(stateGroupsRoot, oldState, newState, control);
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0019A628 File Offset: 0x00199628
		private static Storyboard GenerateDynamicTransitionAnimations(FrameworkElement root, VisualStateGroup group, VisualState newState, VisualTransition transition)
		{
			IEasingFunction easingFunction = null;
			Storyboard storyboard = new Storyboard();
			if (transition != null)
			{
				if (transition.GeneratedDuration != VisualStateManager.DurationZero)
				{
					storyboard.Duration = transition.GeneratedDuration;
				}
				easingFunction = transition.GeneratedEasingFunction;
			}
			else
			{
				storyboard.Duration = new Duration(TimeSpan.Zero);
			}
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> dictionary = VisualStateManager.FlattenTimelines(group.CurrentStoryboards);
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> dictionary2 = VisualStateManager.FlattenTimelines((transition != null) ? transition.Storyboard : null);
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> dictionary3 = VisualStateManager.FlattenTimelines(newState.Storyboard);
			foreach (KeyValuePair<VisualStateManager.TimelineDataToken, Timeline> keyValuePair in dictionary2)
			{
				dictionary.Remove(keyValuePair.Key);
				dictionary3.Remove(keyValuePair.Key);
			}
			foreach (KeyValuePair<VisualStateManager.TimelineDataToken, Timeline> keyValuePair2 in dictionary3)
			{
				Timeline timeline = VisualStateManager.GenerateToAnimation(root, keyValuePair2.Value, easingFunction, true);
				if (timeline != null)
				{
					timeline.Duration = storyboard.Duration;
					storyboard.Children.Add(timeline);
				}
				dictionary.Remove(keyValuePair2.Key);
			}
			foreach (KeyValuePair<VisualStateManager.TimelineDataToken, Timeline> keyValuePair3 in dictionary)
			{
				Timeline timeline2 = VisualStateManager.GenerateFromAnimation(root, keyValuePair3.Value, easingFunction);
				if (timeline2 != null)
				{
					timeline2.Duration = storyboard.Duration;
					storyboard.Children.Add(timeline2);
				}
			}
			return storyboard;
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x0019A7D4 File Offset: 0x001997D4
		private static Timeline GenerateFromAnimation(FrameworkElement root, Timeline timeline, IEasingFunction easingFunction)
		{
			Timeline timeline2 = null;
			if (timeline is ColorAnimation || timeline is ColorAnimationUsingKeyFrames)
			{
				timeline2 = new ColorAnimation
				{
					EasingFunction = easingFunction
				};
			}
			else if (timeline is DoubleAnimation || timeline is DoubleAnimationUsingKeyFrames)
			{
				timeline2 = new DoubleAnimation
				{
					EasingFunction = easingFunction
				};
			}
			else if (timeline is PointAnimation || timeline is PointAnimationUsingKeyFrames)
			{
				timeline2 = new PointAnimation
				{
					EasingFunction = easingFunction
				};
			}
			if (timeline2 != null)
			{
				VisualStateManager.CopyStoryboardTargetProperties(root, timeline, timeline2);
			}
			return timeline2;
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x0019A84C File Offset: 0x0019984C
		private static Timeline GenerateToAnimation(FrameworkElement root, Timeline timeline, IEasingFunction easingFunction, bool isEntering)
		{
			Timeline timeline2 = null;
			Color? targetColor = VisualStateManager.GetTargetColor(timeline, isEntering);
			if (targetColor != null)
			{
				timeline2 = new ColorAnimation
				{
					To = targetColor,
					EasingFunction = easingFunction
				};
			}
			if (timeline2 == null)
			{
				double? targetDouble = VisualStateManager.GetTargetDouble(timeline, isEntering);
				if (targetDouble != null)
				{
					timeline2 = new DoubleAnimation
					{
						To = targetDouble,
						EasingFunction = easingFunction
					};
				}
			}
			if (timeline2 == null)
			{
				Point? targetPoint = VisualStateManager.GetTargetPoint(timeline, isEntering);
				if (targetPoint != null)
				{
					timeline2 = new PointAnimation
					{
						To = targetPoint,
						EasingFunction = easingFunction
					};
				}
			}
			if (timeline2 != null)
			{
				VisualStateManager.CopyStoryboardTargetProperties(root, timeline, timeline2);
			}
			return timeline2;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x0019A8DC File Offset: 0x001998DC
		private static void CopyStoryboardTargetProperties(FrameworkElement root, Timeline source, Timeline destination)
		{
			if (source != null || destination != null)
			{
				string targetName = Storyboard.GetTargetName(source);
				DependencyObject dependencyObject = Storyboard.GetTarget(source);
				PropertyPath targetProperty = Storyboard.GetTargetProperty(source);
				if (dependencyObject == null && !string.IsNullOrEmpty(targetName))
				{
					dependencyObject = (root.FindName(targetName) as DependencyObject);
				}
				if (targetName != null)
				{
					Storyboard.SetTargetName(destination, targetName);
				}
				if (dependencyObject != null)
				{
					Storyboard.SetTarget(destination, dependencyObject);
				}
				if (targetProperty != null)
				{
					Storyboard.SetTargetProperty(destination, targetProperty);
				}
			}
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x0019A93C File Offset: 0x0019993C
		internal static VisualTransition GetTransition(FrameworkElement element, VisualStateGroup group, VisualState from, VisualState to)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			VisualTransition visualTransition = null;
			VisualTransition visualTransition2 = null;
			int num = -1;
			IList<VisualTransition> list = (IList<VisualTransition>)group.Transitions;
			if (list != null)
			{
				foreach (VisualTransition visualTransition3 in list)
				{
					if (visualTransition2 == null && visualTransition3.IsDefault)
					{
						visualTransition2 = visualTransition3;
					}
					else
					{
						int num2 = -1;
						VisualState state = group.GetState(visualTransition3.From);
						VisualState state2 = group.GetState(visualTransition3.To);
						if (from == state)
						{
							num2++;
						}
						else if (state != null)
						{
							continue;
						}
						if (to == state2)
						{
							num2 += 2;
						}
						else if (state2 != null)
						{
							continue;
						}
						if (num2 > num)
						{
							num = num2;
							visualTransition = visualTransition3;
						}
					}
				}
			}
			return visualTransition ?? visualTransition2;
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x0019AA2C File Offset: 0x00199A2C
		private static Color? GetTargetColor(Timeline timeline, bool isEntering)
		{
			ColorAnimation colorAnimation = timeline as ColorAnimation;
			if (colorAnimation != null)
			{
				if (colorAnimation.From == null)
				{
					return colorAnimation.To;
				}
				return colorAnimation.From;
			}
			else
			{
				ColorAnimationUsingKeyFrames colorAnimationUsingKeyFrames = timeline as ColorAnimationUsingKeyFrames;
				if (colorAnimationUsingKeyFrames == null)
				{
					return null;
				}
				if (colorAnimationUsingKeyFrames.KeyFrames.Count == 0)
				{
					return null;
				}
				return new Color?(colorAnimationUsingKeyFrames.KeyFrames[isEntering ? 0 : (colorAnimationUsingKeyFrames.KeyFrames.Count - 1)].Value);
			}
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x0019AAB4 File Offset: 0x00199AB4
		private static double? GetTargetDouble(Timeline timeline, bool isEntering)
		{
			DoubleAnimation doubleAnimation = timeline as DoubleAnimation;
			if (doubleAnimation != null)
			{
				if (doubleAnimation.From == null)
				{
					return doubleAnimation.To;
				}
				return doubleAnimation.From;
			}
			else
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = timeline as DoubleAnimationUsingKeyFrames;
				if (doubleAnimationUsingKeyFrames == null)
				{
					return null;
				}
				if (doubleAnimationUsingKeyFrames.KeyFrames.Count == 0)
				{
					return null;
				}
				return new double?(doubleAnimationUsingKeyFrames.KeyFrames[isEntering ? 0 : (doubleAnimationUsingKeyFrames.KeyFrames.Count - 1)].Value);
			}
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x0019AB3C File Offset: 0x00199B3C
		private static Point? GetTargetPoint(Timeline timeline, bool isEntering)
		{
			PointAnimation pointAnimation = timeline as PointAnimation;
			if (pointAnimation != null)
			{
				if (pointAnimation.From == null)
				{
					return pointAnimation.To;
				}
				return pointAnimation.From;
			}
			else
			{
				PointAnimationUsingKeyFrames pointAnimationUsingKeyFrames = timeline as PointAnimationUsingKeyFrames;
				if (pointAnimationUsingKeyFrames == null)
				{
					return null;
				}
				if (pointAnimationUsingKeyFrames.KeyFrames.Count == 0)
				{
					return null;
				}
				return new Point?(pointAnimationUsingKeyFrames.KeyFrames[isEntering ? 0 : (pointAnimationUsingKeyFrames.KeyFrames.Count - 1)].Value);
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x0019ABC4 File Offset: 0x00199BC4
		private static Dictionary<VisualStateManager.TimelineDataToken, Timeline> FlattenTimelines(Storyboard storyboard)
		{
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> result = new Dictionary<VisualStateManager.TimelineDataToken, Timeline>();
			VisualStateManager.FlattenTimelines(storyboard, result);
			return result;
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x0019ABE0 File Offset: 0x00199BE0
		private static Dictionary<VisualStateManager.TimelineDataToken, Timeline> FlattenTimelines(Collection<Storyboard> storyboards)
		{
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> result = new Dictionary<VisualStateManager.TimelineDataToken, Timeline>();
			for (int i = 0; i < storyboards.Count; i++)
			{
				VisualStateManager.FlattenTimelines(storyboards[i], result);
			}
			return result;
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x0019AC14 File Offset: 0x00199C14
		private static void FlattenTimelines(Storyboard storyboard, Dictionary<VisualStateManager.TimelineDataToken, Timeline> result)
		{
			if (storyboard == null)
			{
				return;
			}
			for (int i = 0; i < storyboard.Children.Count; i++)
			{
				Timeline timeline = storyboard.Children[i];
				Storyboard storyboard2 = timeline as Storyboard;
				if (storyboard2 != null)
				{
					VisualStateManager.FlattenTimelines(storyboard2, result);
				}
				else
				{
					result[new VisualStateManager.TimelineDataToken(timeline)] = timeline;
				}
			}
		}

		// Token: 0x04001504 RID: 5380
		public static readonly DependencyProperty CustomVisualStateManagerProperty = DependencyProperty.RegisterAttached("CustomVisualStateManager", typeof(VisualStateManager), typeof(VisualStateManager), null);

		// Token: 0x04001505 RID: 5381
		private static readonly DependencyPropertyKey VisualStateGroupsPropertyKey = DependencyProperty.RegisterAttachedReadOnly("VisualStateGroups", typeof(IList), typeof(VisualStateManager), new FrameworkPropertyMetadata(new ObservableCollectionDefaultValueFactory<VisualStateGroup>()));

		// Token: 0x04001506 RID: 5382
		public static readonly DependencyProperty VisualStateGroupsProperty = VisualStateManager.VisualStateGroupsPropertyKey.DependencyProperty;

		// Token: 0x04001507 RID: 5383
		private static readonly Duration DurationZero = new Duration(TimeSpan.Zero);

		// Token: 0x02000A97 RID: 2711
		private struct TimelineDataToken : IEquatable<VisualStateManager.TimelineDataToken>
		{
			// Token: 0x060086CF RID: 34511 RVA: 0x0032B69A File Offset: 0x0032A69A
			public TimelineDataToken(Timeline timeline)
			{
				this._target = Storyboard.GetTarget(timeline);
				this._targetName = Storyboard.GetTargetName(timeline);
				this._targetProperty = Storyboard.GetTargetProperty(timeline);
			}

			// Token: 0x060086D0 RID: 34512 RVA: 0x0032B6C0 File Offset: 0x0032A6C0
			public bool Equals(VisualStateManager.TimelineDataToken other)
			{
				bool flag;
				if (this._targetName != null)
				{
					flag = (other._targetName == this._targetName);
				}
				else if (this._target != null)
				{
					flag = (other._target == this._target);
				}
				else
				{
					flag = (other._target == null && other._targetName == null);
				}
				if (flag && other._targetProperty.Path == this._targetProperty.Path && other._targetProperty.PathParameters.Count == this._targetProperty.PathParameters.Count)
				{
					bool result = true;
					int i = 0;
					int count = this._targetProperty.PathParameters.Count;
					while (i < count)
					{
						if (other._targetProperty.PathParameters[i] != this._targetProperty.PathParameters[i])
						{
							result = false;
							break;
						}
						i++;
					}
					return result;
				}
				return false;
			}

			// Token: 0x060086D1 RID: 34513 RVA: 0x0032B7A8 File Offset: 0x0032A7A8
			public override int GetHashCode()
			{
				int num = (this._target != null) ? this._target.GetHashCode() : 0;
				int num2 = (this._targetName != null) ? this._targetName.GetHashCode() : 0;
				int num3 = (this._targetProperty != null && this._targetProperty.Path != null) ? this._targetProperty.Path.GetHashCode() : 0;
				return ((this._targetName != null) ? num2 : num) ^ num3;
			}

			// Token: 0x04004283 RID: 17027
			private DependencyObject _target;

			// Token: 0x04004284 RID: 17028
			private string _targetName;

			// Token: 0x04004285 RID: 17029
			private PropertyPath _targetProperty;
		}
	}
}
