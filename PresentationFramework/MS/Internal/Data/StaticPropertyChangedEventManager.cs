using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000241 RID: 577
	internal class StaticPropertyChangedEventManager : WeakEventManager
	{
		// Token: 0x06001646 RID: 5702 RVA: 0x0015A0BF File Offset: 0x001590BF
		private StaticPropertyChangedEventManager()
		{
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0015A0C7 File Offset: 0x001590C7
		public static void AddHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			StaticPropertyChangedEventManager.CurrentManager.PrivateAddHandler(type, handler, propertyName);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0015A0F8 File Offset: 0x001590F8
		public static void RemoveHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			StaticPropertyChangedEventManager.CurrentManager.PrivateRemoveHandler(type, handler, propertyName);
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0015A129 File Offset: 0x00159129
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<PropertyChangedEventArgs>();
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void StartListening(object source)
		{
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void StopListening(object source)
		{
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0015A130 File Offset: 0x00159130
		protected override bool Purge(object source, object data, bool purgeAll)
		{
			StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)data;
			bool result = typeRecord.Purge(purgeAll);
			if (!purgeAll && typeRecord.IsEmpty)
			{
				base.Remove(typeRecord.Type);
			}
			return result;
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x0015A164 File Offset: 0x00159164
		private static StaticPropertyChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(StaticPropertyChangedEventManager);
				StaticPropertyChangedEventManager staticPropertyChangedEventManager = (StaticPropertyChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (staticPropertyChangedEventManager == null)
				{
					staticPropertyChangedEventManager = new StaticPropertyChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, staticPropertyChangedEventManager);
				}
				return staticPropertyChangedEventManager;
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0015A19C File Offset: 0x0015919C
		private void PrivateAddHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			using (base.WriteLock)
			{
				StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)base[type];
				if (typeRecord == null)
				{
					typeRecord = new StaticPropertyChangedEventManager.TypeRecord(type, this);
					base[type] = typeRecord;
					typeRecord.StartListening();
				}
				typeRecord.AddHandler(handler, propertyName);
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0015A1FC File Offset: 0x001591FC
		private void PrivateRemoveHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			using (base.WriteLock)
			{
				StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)base[type];
				if (typeRecord != null)
				{
					typeRecord.RemoveHandler(handler, propertyName);
					if (typeRecord.IsEmpty)
					{
						typeRecord.StopListening();
						base.Remove(typeRecord.Type);
					}
				}
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0015A260 File Offset: 0x00159260
		private void OnStaticPropertyChanged(StaticPropertyChangedEventManager.TypeRecord typeRecord, PropertyChangedEventArgs args)
		{
			WeakEventManager.ListenerList listenerList;
			using (base.ReadLock)
			{
				listenerList = typeRecord.GetListenerList(args.PropertyName);
				listenerList.BeginUse();
			}
			try
			{
				base.DeliverEventToList(null, args, listenerList);
			}
			finally
			{
				listenerList.EndUse();
			}
			if (listenerList == typeRecord.ProposedAllListenersList)
			{
				using (base.WriteLock)
				{
					typeRecord.StoreAllListenersList((WeakEventManager.ListenerList<PropertyChangedEventArgs>)listenerList);
				}
			}
		}

		// Token: 0x04000C57 RID: 3159
		private static readonly string AllListenersKey = "<All Listeners>";

		// Token: 0x04000C58 RID: 3160
		private static readonly string StaticPropertyChanged = "StaticPropertyChanged";

		// Token: 0x02000A03 RID: 2563
		private class TypeRecord
		{
			// Token: 0x06008485 RID: 33925 RVA: 0x00325E60 File Offset: 0x00324E60
			public TypeRecord(Type type, StaticPropertyChangedEventManager manager)
			{
				this._type = type;
				this._manager = manager;
				this._dict = new HybridDictionary(true);
			}

			// Token: 0x17001DC4 RID: 7620
			// (get) Token: 0x06008486 RID: 33926 RVA: 0x00325E8D File Offset: 0x00324E8D
			public Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x17001DC5 RID: 7621
			// (get) Token: 0x06008487 RID: 33927 RVA: 0x00325E95 File Offset: 0x00324E95
			public bool IsEmpty
			{
				get
				{
					return this._dict.Count == 0;
				}
			}

			// Token: 0x17001DC6 RID: 7622
			// (get) Token: 0x06008488 RID: 33928 RVA: 0x00325EA5 File Offset: 0x00324EA5
			public WeakEventManager.ListenerList ProposedAllListenersList
			{
				get
				{
					return this._proposedAllListenersList;
				}
			}

			// Token: 0x17001DC7 RID: 7623
			// (get) Token: 0x06008489 RID: 33929 RVA: 0x00325EAD File Offset: 0x00324EAD
			private static MethodInfo OnStaticPropertyChangedMethodInfo
			{
				get
				{
					return typeof(StaticPropertyChangedEventManager.TypeRecord).GetMethod("OnStaticPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				}
			}

			// Token: 0x0600848A RID: 33930 RVA: 0x00325EC8 File Offset: 0x00324EC8
			public void StartListening()
			{
				EventInfo @event = this._type.GetEvent(StaticPropertyChangedEventManager.StaticPropertyChanged, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.TypeRecord.OnStaticPropertyChangedMethodInfo);
					@event.AddEventHandler(null, handler);
				}
			}

			// Token: 0x0600848B RID: 33931 RVA: 0x00325F0C File Offset: 0x00324F0C
			public void StopListening()
			{
				EventInfo @event = this._type.GetEvent(StaticPropertyChangedEventManager.StaticPropertyChanged, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.TypeRecord.OnStaticPropertyChangedMethodInfo);
					@event.RemoveEventHandler(null, handler);
				}
			}

			// Token: 0x0600848C RID: 33932 RVA: 0x00325F4F File Offset: 0x00324F4F
			private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				this.HandleStaticPropertyChanged(e);
			}

			// Token: 0x0600848D RID: 33933 RVA: 0x00325F58 File Offset: 0x00324F58
			public void HandleStaticPropertyChanged(PropertyChangedEventArgs e)
			{
				this._manager.OnStaticPropertyChanged(this, e);
			}

			// Token: 0x0600848E RID: 33934 RVA: 0x00325F68 File Offset: 0x00324F68
			public void AddHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
			{
				StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
				if (propertyRecord == null)
				{
					propertyRecord = new StaticPropertyChangedEventManager.PropertyRecord(propertyName, this);
					this._dict[propertyName] = propertyRecord;
					propertyRecord.StartListening(this._type);
				}
				propertyRecord.AddHandler(handler);
				this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
				this._proposedAllListenersList = null;
				this._manager.ScheduleCleanup();
			}

			// Token: 0x0600848F RID: 33935 RVA: 0x00325FD4 File Offset: 0x00324FD4
			public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
			{
				StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
				if (propertyRecord != null)
				{
					propertyRecord.RemoveHandler(handler);
					if (propertyRecord.IsEmpty)
					{
						this._dict.Remove(propertyName);
					}
					this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
					this._proposedAllListenersList = null;
				}
			}

			// Token: 0x06008490 RID: 33936 RVA: 0x00326028 File Offset: 0x00325028
			public WeakEventManager.ListenerList GetListenerList(string propertyName)
			{
				WeakEventManager.ListenerList listenerList3;
				if (!string.IsNullOrEmpty(propertyName))
				{
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList = (propertyRecord == null) ? null : propertyRecord.List;
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord2 = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[string.Empty];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList2 = (propertyRecord2 == null) ? null : propertyRecord2.List;
					if (listenerList2 == null)
					{
						if (listenerList != null)
						{
							listenerList3 = listenerList;
						}
						else
						{
							listenerList3 = WeakEventManager.ListenerList.Empty;
						}
					}
					else if (listenerList != null)
					{
						listenerList3 = new WeakEventManager.ListenerList<PropertyChangedEventArgs>(listenerList.Count + listenerList2.Count);
						int i = 0;
						int count = listenerList.Count;
						while (i < count)
						{
							listenerList3.Add(listenerList[i]);
							i++;
						}
						int j = 0;
						int count2 = listenerList2.Count;
						while (j < count2)
						{
							listenerList3.Add(listenerList2[j]);
							j++;
						}
					}
					else
					{
						listenerList3 = listenerList2;
					}
				}
				else
				{
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord3 = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[StaticPropertyChangedEventManager.AllListenersKey];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList4 = (propertyRecord3 == null) ? null : propertyRecord3.List;
					if (listenerList4 == null)
					{
						int num = 0;
						foreach (object obj in this._dict)
						{
							num += ((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj).Value).List.Count;
						}
						listenerList4 = new WeakEventManager.ListenerList<PropertyChangedEventArgs>(num);
						foreach (object obj2 in this._dict)
						{
							WeakEventManager.ListenerList list = ((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj2).Value).List;
							int k = 0;
							int count3 = list.Count;
							while (k < count3)
							{
								listenerList4.Add(list.GetListener(k));
								k++;
							}
						}
						this._proposedAllListenersList = listenerList4;
					}
					listenerList3 = listenerList4;
				}
				return listenerList3;
			}

			// Token: 0x06008491 RID: 33937 RVA: 0x00326244 File Offset: 0x00325244
			public void StoreAllListenersList(WeakEventManager.ListenerList<PropertyChangedEventArgs> list)
			{
				if (this._proposedAllListenersList == list)
				{
					this._dict[StaticPropertyChangedEventManager.AllListenersKey] = new StaticPropertyChangedEventManager.PropertyRecord(StaticPropertyChangedEventManager.AllListenersKey, this, list);
					this._proposedAllListenersList = null;
				}
			}

			// Token: 0x06008492 RID: 33938 RVA: 0x00326274 File Offset: 0x00325274
			public bool Purge(bool purgeAll)
			{
				bool flag = false;
				if (!purgeAll)
				{
					if (!BaseAppContextSwitches.EnableWeakEventMemoryImprovements)
					{
						ICollection keys = this._dict.Keys;
						string[] array = new string[keys.Count];
						keys.CopyTo(array, 0);
						for (int i = array.Length - 1; i >= 0; i--)
						{
							if (!(array[i] == StaticPropertyChangedEventManager.AllListenersKey))
							{
								StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[array[i]];
								if (propertyRecord.Purge())
								{
									flag = true;
								}
								if (propertyRecord.IsEmpty)
								{
									propertyRecord.StopListening(this._type);
									this._dict.Remove(array[i]);
								}
							}
						}
					}
					else
					{
						IDictionaryEnumerator enumerator = this._dict.GetEnumerator();
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Key;
							if (!(text == StaticPropertyChangedEventManager.AllListenersKey))
							{
								StaticPropertyChangedEventManager.PropertyRecord propertyRecord2 = (StaticPropertyChangedEventManager.PropertyRecord)enumerator.Value;
								if (propertyRecord2.Purge())
								{
									flag = true;
								}
								if (propertyRecord2.IsEmpty)
								{
									propertyRecord2.StopListening(this._type);
									this._toRemove.Add(text);
								}
							}
						}
						if (this._toRemove.Count > 0)
						{
							foreach (string key in this._toRemove)
							{
								this._dict.Remove(key);
							}
							this._toRemove.Clear();
							this._toRemove.TrimExcess();
						}
					}
					if (flag)
					{
						this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
						this._proposedAllListenersList = null;
					}
					if (this.IsEmpty)
					{
						this.StopListening();
					}
				}
				else
				{
					flag = true;
					this.StopListening();
					foreach (object obj in this._dict)
					{
						((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj).Value).StopListening(this._type);
					}
				}
				return flag;
			}

			// Token: 0x04004053 RID: 16467
			private Type _type;

			// Token: 0x04004054 RID: 16468
			private HybridDictionary _dict;

			// Token: 0x04004055 RID: 16469
			private StaticPropertyChangedEventManager _manager;

			// Token: 0x04004056 RID: 16470
			private WeakEventManager.ListenerList<PropertyChangedEventArgs> _proposedAllListenersList;

			// Token: 0x04004057 RID: 16471
			private List<string> _toRemove = new List<string>();
		}

		// Token: 0x02000A04 RID: 2564
		private class PropertyRecord
		{
			// Token: 0x06008493 RID: 33939 RVA: 0x0032648C File Offset: 0x0032548C
			public PropertyRecord(string propertyName, StaticPropertyChangedEventManager.TypeRecord owner) : this(propertyName, owner, new WeakEventManager.ListenerList<PropertyChangedEventArgs>())
			{
			}

			// Token: 0x06008494 RID: 33940 RVA: 0x0032649B File Offset: 0x0032549B
			public PropertyRecord(string propertyName, StaticPropertyChangedEventManager.TypeRecord owner, WeakEventManager.ListenerList<PropertyChangedEventArgs> list)
			{
				this._propertyName = propertyName;
				this._typeRecord = owner;
				this._list = list;
			}

			// Token: 0x17001DC8 RID: 7624
			// (get) Token: 0x06008495 RID: 33941 RVA: 0x003264B8 File Offset: 0x003254B8
			public bool IsEmpty
			{
				get
				{
					return this._list.IsEmpty;
				}
			}

			// Token: 0x17001DC9 RID: 7625
			// (get) Token: 0x06008496 RID: 33942 RVA: 0x003264C5 File Offset: 0x003254C5
			public WeakEventManager.ListenerList<PropertyChangedEventArgs> List
			{
				get
				{
					return this._list;
				}
			}

			// Token: 0x17001DCA RID: 7626
			// (get) Token: 0x06008497 RID: 33943 RVA: 0x003264CD File Offset: 0x003254CD
			private static MethodInfo OnStaticPropertyChangedMethodInfo
			{
				get
				{
					return typeof(StaticPropertyChangedEventManager.PropertyRecord).GetMethod("OnStaticPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				}
			}

			// Token: 0x06008498 RID: 33944 RVA: 0x003264E8 File Offset: 0x003254E8
			public void StartListening(Type type)
			{
				string name = this._propertyName + "Changed";
				EventInfo @event = type.GetEvent(name, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.PropertyRecord.OnStaticPropertyChangedMethodInfo);
					@event.AddEventHandler(null, handler);
				}
			}

			// Token: 0x06008499 RID: 33945 RVA: 0x00326534 File Offset: 0x00325534
			public void StopListening(Type type)
			{
				string name = this._propertyName + "Changed";
				EventInfo @event = type.GetEvent(name, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.PropertyRecord.OnStaticPropertyChangedMethodInfo);
					@event.RemoveEventHandler(null, handler);
				}
			}

			// Token: 0x0600849A RID: 33946 RVA: 0x0032657F File Offset: 0x0032557F
			private void OnStaticPropertyChanged(object sender, EventArgs e)
			{
				this._typeRecord.HandleStaticPropertyChanged(new PropertyChangedEventArgs(this._propertyName));
			}

			// Token: 0x0600849B RID: 33947 RVA: 0x00326598 File Offset: 0x00325598
			public void AddHandler(EventHandler<PropertyChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				this._list.AddHandler(handler);
			}

			// Token: 0x0600849C RID: 33948 RVA: 0x003265D0 File Offset: 0x003255D0
			public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				this._list.RemoveHandler(handler);
			}

			// Token: 0x0600849D RID: 33949 RVA: 0x00326608 File Offset: 0x00325608
			public bool Purge()
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				return this._list.Purge();
			}

			// Token: 0x04004058 RID: 16472
			private string _propertyName;

			// Token: 0x04004059 RID: 16473
			private WeakEventManager.ListenerList<PropertyChangedEventArgs> _list;

			// Token: 0x0400405A RID: 16474
			private StaticPropertyChangedEventManager.TypeRecord _typeRecord;
		}
	}
}
