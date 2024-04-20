using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000242 RID: 578
	internal class ValueChangedEventManager : WeakEventManager
	{
		// Token: 0x06001652 RID: 5714 RVA: 0x0015A30E File Offset: 0x0015930E
		private ValueChangedEventManager()
		{
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0015A321 File Offset: 0x00159321
		public static void AddListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			ValueChangedEventManager.CurrentManager.PrivateAddListener(source, listener, pd);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0015A34C File Offset: 0x0015934C
		public static void RemoveListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			ValueChangedEventManager.CurrentManager.PrivateRemoveListener(source, listener, pd);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0015A377 File Offset: 0x00159377
		public static void AddHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetInvocationList().Length != 1)
			{
				throw new NotSupportedException(SR.Get("NoMulticastHandlers"));
			}
			ValueChangedEventManager.CurrentManager.PrivateAddHandler(source, handler, pd);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0015A3AF File Offset: 0x001593AF
		public static void RemoveHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetInvocationList().Length != 1)
			{
				throw new NotSupportedException(SR.Get("NoMulticastHandlers"));
			}
			ValueChangedEventManager.CurrentManager.PrivateRemoveHandler(source, handler, pd);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0015A3E7 File Offset: 0x001593E7
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<ValueChangedEventArgs>();
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void StartListening(object source)
		{
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void StopListening(object source)
		{
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0015A3F0 File Offset: 0x001593F0
		protected override bool Purge(object source, object data, bool purgeAll)
		{
			bool result = false;
			HybridDictionary hybridDictionary = (HybridDictionary)data;
			if (!BaseAppContextSwitches.EnableWeakEventMemoryImprovements)
			{
				ICollection keys = hybridDictionary.Keys;
				PropertyDescriptor[] array = new PropertyDescriptor[keys.Count];
				keys.CopyTo(array, 0);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					bool flag = purgeAll || source == null;
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[array[i]];
					if (!flag)
					{
						if (valueChangedRecord.Purge())
						{
							result = true;
						}
						flag = valueChangedRecord.IsEmpty;
					}
					if (flag)
					{
						valueChangedRecord.StopListening();
						if (!purgeAll)
						{
							hybridDictionary.Remove(array[i]);
						}
					}
				}
			}
			else
			{
				IDictionaryEnumerator enumerator = hybridDictionary.GetEnumerator();
				while (enumerator.MoveNext())
				{
					bool flag2 = purgeAll || source == null;
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord2 = (ValueChangedEventManager.ValueChangedRecord)enumerator.Value;
					if (!flag2)
					{
						if (valueChangedRecord2.Purge())
						{
							result = true;
						}
						flag2 = valueChangedRecord2.IsEmpty;
					}
					if (flag2)
					{
						valueChangedRecord2.StopListening();
						if (!purgeAll)
						{
							this._toRemove.Add((PropertyDescriptor)enumerator.Key);
						}
					}
				}
				if (this._toRemove.Count > 0)
				{
					foreach (PropertyDescriptor key in this._toRemove)
					{
						hybridDictionary.Remove(key);
					}
					this._toRemove.Clear();
					this._toRemove.TrimExcess();
				}
			}
			if (hybridDictionary.Count == 0)
			{
				result = true;
				if (source != null)
				{
					base.Remove(source);
				}
			}
			return result;
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x0015A574 File Offset: 0x00159574
		private static ValueChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(ValueChangedEventManager);
				ValueChangedEventManager valueChangedEventManager = (ValueChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (valueChangedEventManager == null)
				{
					valueChangedEventManager = new ValueChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, valueChangedEventManager);
				}
				return valueChangedEventManager;
			}
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0015A5A9 File Offset: 0x001595A9
		private void PrivateAddListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			this.AddListener(source, pd, listener, null);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0015A5B5 File Offset: 0x001595B5
		private void PrivateRemoveListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			this.RemoveListener(source, pd, listener, null);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0015A5C1 File Offset: 0x001595C1
		private void PrivateAddHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			this.AddListener(source, pd, null, handler);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0015A5CD File Offset: 0x001595CD
		private void PrivateRemoveHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			this.RemoveListener(source, pd, null, handler);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0015A5DC File Offset: 0x001595DC
		private void AddListener(object source, PropertyDescriptor pd, IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
		{
			using (base.WriteLock)
			{
				HybridDictionary hybridDictionary = (HybridDictionary)base[source];
				if (hybridDictionary == null)
				{
					hybridDictionary = new HybridDictionary();
					base[source] = hybridDictionary;
				}
				ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[pd];
				if (valueChangedRecord == null)
				{
					valueChangedRecord = new ValueChangedEventManager.ValueChangedRecord(this, source, pd);
					hybridDictionary[pd] = valueChangedRecord;
				}
				valueChangedRecord.Add(listener, handler);
				base.ScheduleCleanup();
			}
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0015A65C File Offset: 0x0015965C
		private void RemoveListener(object source, PropertyDescriptor pd, IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
		{
			using (base.WriteLock)
			{
				HybridDictionary hybridDictionary = (HybridDictionary)base[source];
				if (hybridDictionary != null)
				{
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[pd];
					if (valueChangedRecord != null)
					{
						valueChangedRecord.Remove(listener, handler);
						if (valueChangedRecord.IsEmpty)
						{
							hybridDictionary.Remove(pd);
						}
					}
					if (hybridDictionary.Count == 0)
					{
						base.Remove(source);
					}
				}
			}
		}

		// Token: 0x04000C59 RID: 3161
		private List<PropertyDescriptor> _toRemove = new List<PropertyDescriptor>();

		// Token: 0x02000A05 RID: 2565
		private class ValueChangedRecord
		{
			// Token: 0x0600849E RID: 33950 RVA: 0x0032663C File Offset: 0x0032563C
			public ValueChangedRecord(ValueChangedEventManager manager, object source, PropertyDescriptor pd)
			{
				this._manager = manager;
				this._source = source;
				this._pd = pd;
				this._eventArgs = new ValueChangedEventArgs(pd);
				pd.AddValueChanged(source, new EventHandler(this.OnValueChanged));
			}

			// Token: 0x17001DCB RID: 7627
			// (get) Token: 0x0600849F RID: 33951 RVA: 0x00326690 File Offset: 0x00325690
			public bool IsEmpty
			{
				get
				{
					bool flag = this._listeners.IsEmpty;
					if (!flag && this.HasIgnorableListeners)
					{
						flag = true;
						int i = 0;
						int count = this._listeners.Count;
						while (i < count)
						{
							if (!this.IsIgnorable(this._listeners.GetListener(i).Target))
							{
								flag = false;
								break;
							}
							i++;
						}
					}
					return flag;
				}
			}

			// Token: 0x060084A0 RID: 33952 RVA: 0x003266F0 File Offset: 0x003256F0
			public void Add(IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				if (handler != null)
				{
					this._listeners.AddHandler(handler);
					if (!this.HasIgnorableListeners && this.IsIgnorable(handler.Target))
					{
						this.HasIgnorableListeners = true;
						return;
					}
				}
				else
				{
					this._listeners.Add(listener);
				}
			}

			// Token: 0x060084A1 RID: 33953 RVA: 0x00326754 File Offset: 0x00325754
			public void Remove(IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				if (handler != null)
				{
					this._listeners.RemoveHandler(handler);
				}
				else
				{
					this._listeners.Remove(listener);
				}
				if (this.IsEmpty)
				{
					this.StopListening();
				}
			}

			// Token: 0x060084A2 RID: 33954 RVA: 0x003267A8 File Offset: 0x003257A8
			public bool Purge()
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				return this._listeners.Purge();
			}

			// Token: 0x060084A3 RID: 33955 RVA: 0x003267DC File Offset: 0x003257DC
			public void StopListening()
			{
				if (this._source != null)
				{
					this._pd.RemoveValueChanged(this._source, new EventHandler(this.OnValueChanged));
					this._source = null;
				}
			}

			// Token: 0x060084A4 RID: 33956 RVA: 0x0032680C File Offset: 0x0032580C
			private void OnValueChanged(object sender, EventArgs e)
			{
				using (this._manager.ReadLock)
				{
					this._listeners.BeginUse();
				}
				try
				{
					this._manager.DeliverEventToList(sender, this._eventArgs, this._listeners);
				}
				finally
				{
					this._listeners.EndUse();
				}
			}

			// Token: 0x17001DCC RID: 7628
			// (get) Token: 0x060084A5 RID: 33957 RVA: 0x00326880 File Offset: 0x00325880
			// (set) Token: 0x060084A6 RID: 33958 RVA: 0x00326888 File Offset: 0x00325888
			private bool HasIgnorableListeners { get; set; }

			// Token: 0x060084A7 RID: 33959 RVA: 0x00326891 File Offset: 0x00325891
			private bool IsIgnorable(object target)
			{
				return target is ValueTable;
			}

			// Token: 0x0400405C RID: 16476
			private PropertyDescriptor _pd;

			// Token: 0x0400405D RID: 16477
			private ValueChangedEventManager _manager;

			// Token: 0x0400405E RID: 16478
			private object _source;

			// Token: 0x0400405F RID: 16479
			private WeakEventManager.ListenerList<ValueChangedEventArgs> _listeners = new WeakEventManager.ListenerList<ValueChangedEventArgs>();

			// Token: 0x04004060 RID: 16480
			private ValueChangedEventArgs _eventArgs;
		}
	}
}
