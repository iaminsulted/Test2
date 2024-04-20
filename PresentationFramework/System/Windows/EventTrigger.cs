using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x0200035F RID: 863
	[ContentProperty("Actions")]
	public class EventTrigger : TriggerBase, IAddChild
	{
		// Token: 0x0600208A RID: 8330 RVA: 0x00175AAC File Offset: 0x00174AAC
		public EventTrigger()
		{
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00175AB4 File Offset: 0x00174AB4
		public EventTrigger(RoutedEvent routedEvent)
		{
			this.RoutedEvent = routedEvent;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x00175AC3 File Offset: 0x00174AC3
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00175ACC File Offset: 0x00174ACC
		protected virtual void AddChild(object value)
		{
			TriggerAction triggerAction = value as TriggerAction;
			if (triggerAction == null)
			{
				throw new ArgumentException(SR.Get("EventTriggerBadAction", new object[]
				{
					value.GetType().Name
				}));
			}
			this.Actions.Add(triggerAction);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x00175B13 File Offset: 0x00174B13
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00175B1C File Offset: 0x00174B1C
		protected virtual void AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x00175B25 File Offset: 0x00174B25
		// (set) Token: 0x06002091 RID: 8337 RVA: 0x00175B30 File Offset: 0x00174B30
		public RoutedEvent RoutedEvent
		{
			get
			{
				return this._routedEvent;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"EventTrigger"
					}));
				}
				if (this._routedEventHandler != null)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"EventTrigger"
					}));
				}
				this._routedEvent = value;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x00175B9E File Offset: 0x00174B9E
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x00175BA6 File Offset: 0x00174BA6
		[DefaultValue(null)]
		public string SourceName
		{
			get
			{
				return this._sourceName;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"EventTrigger"
					}));
				}
				this._sourceName = value;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x00175BD5 File Offset: 0x00174BD5
		// (set) Token: 0x06002095 RID: 8341 RVA: 0x00175BDD File Offset: 0x00174BDD
		internal int TriggerChildIndex
		{
			get
			{
				return this._childId;
			}
			set
			{
				this._childId = value;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x00175BE6 File Offset: 0x00174BE6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerActionCollection Actions
		{
			get
			{
				if (this._actions == null)
				{
					this._actions = new TriggerActionCollection();
					this._actions.Owner = this;
				}
				return this._actions;
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00175C10 File Offset: 0x00174C10
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			base.OnInheritanceContextChangedCore(args);
			if (this._actions == null)
			{
				return;
			}
			for (int i = 0; i < this._actions.Count; i++)
			{
				DependencyObject dependencyObject = this._actions[i];
				if (dependencyObject != null && dependencyObject.InheritanceContext == this)
				{
					dependencyObject.OnInheritanceContextChanged(args);
				}
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00175C63 File Offset: 0x00174C63
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeActions()
		{
			return this._actions != null && this._actions.Count > 0;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00175C80 File Offset: 0x00174C80
		internal sealed override void Seal()
		{
			if (this.PropertyValues.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("EventTriggerDoNotSetProperties"));
			}
			if (base.HasEnterActions || base.HasExitActions)
			{
				throw new InvalidOperationException(SR.Get("EventTriggerDoesNotEnterExit"));
			}
			if (this._routedEvent != null && this._actions != null && this._actions.Count > 0)
			{
				this._actions.Seal(this);
			}
			base.Seal();
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00175CFC File Offset: 0x00174CFC
		internal static void ProcessTriggerCollection(FrameworkElement triggersHost)
		{
			TriggerCollection value = EventTrigger.TriggerCollectionField.GetValue(triggersHost);
			if (value != null)
			{
				for (int i = 0; i < value.Count; i++)
				{
					EventTrigger.ProcessOneTrigger(triggersHost, value[i]);
				}
			}
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00175D38 File Offset: 0x00174D38
		internal static void ProcessOneTrigger(FrameworkElement triggersHost, TriggerBase triggerBase)
		{
			EventTrigger eventTrigger = triggerBase as EventTrigger;
			if (eventTrigger != null)
			{
				eventTrigger._source = FrameworkElement.FindNamedFrameworkElement(triggersHost, eventTrigger.SourceName);
				EventTrigger.EventTriggerSourceListener @object = new EventTrigger.EventTriggerSourceListener(eventTrigger, triggersHost);
				eventTrigger._routedEventHandler = new RoutedEventHandler(@object.Handler);
				eventTrigger._source.AddHandler(eventTrigger.RoutedEvent, eventTrigger._routedEventHandler, false);
				return;
			}
			throw new InvalidOperationException(SR.Get("TriggersSupportsEventTriggersOnly"));
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00175DA4 File Offset: 0x00174DA4
		internal static void DisconnectAllTriggers(FrameworkElement triggersHost)
		{
			TriggerCollection value = EventTrigger.TriggerCollectionField.GetValue(triggersHost);
			if (value != null)
			{
				for (int i = 0; i < value.Count; i++)
				{
					EventTrigger.DisconnectOneTrigger(triggersHost, value[i]);
				}
			}
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00175DE0 File Offset: 0x00174DE0
		internal static void DisconnectOneTrigger(FrameworkElement triggersHost, TriggerBase triggerBase)
		{
			EventTrigger eventTrigger = triggerBase as EventTrigger;
			if (eventTrigger != null)
			{
				eventTrigger._source.RemoveHandler(eventTrigger.RoutedEvent, eventTrigger._routedEventHandler);
				eventTrigger._routedEventHandler = null;
				return;
			}
			throw new InvalidOperationException(SR.Get("TriggersSupportsEventTriggersOnly"));
		}

		// Token: 0x04000FE8 RID: 4072
		private RoutedEvent _routedEvent;

		// Token: 0x04000FE9 RID: 4073
		private string _sourceName;

		// Token: 0x04000FEA RID: 4074
		private int _childId;

		// Token: 0x04000FEB RID: 4075
		private TriggerActionCollection _actions;

		// Token: 0x04000FEC RID: 4076
		internal static readonly UncommonField<TriggerCollection> TriggerCollectionField = new UncommonField<TriggerCollection>(null);

		// Token: 0x04000FED RID: 4077
		private RoutedEventHandler _routedEventHandler;

		// Token: 0x04000FEE RID: 4078
		private FrameworkElement _source;

		// Token: 0x02000A79 RID: 2681
		internal class EventTriggerSourceListener
		{
			// Token: 0x06008652 RID: 34386 RVA: 0x0032A480 File Offset: 0x00329480
			internal EventTriggerSourceListener(EventTrigger trigger, FrameworkElement host)
			{
				this._owningTrigger = trigger;
				this._owningTriggerHost = host;
			}

			// Token: 0x06008653 RID: 34387 RVA: 0x0032A498 File Offset: 0x00329498
			internal void Handler(object sender, RoutedEventArgs e)
			{
				TriggerActionCollection actions = this._owningTrigger.Actions;
				for (int i = 0; i < actions.Count; i++)
				{
					actions[i].Invoke(this._owningTriggerHost);
				}
			}

			// Token: 0x04004179 RID: 16761
			private EventTrigger _owningTrigger;

			// Token: 0x0400417A RID: 16762
			private FrameworkElement _owningTriggerHost;
		}
	}
}
