using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B6 RID: 694
	[Serializable]
	internal class DataStreams
	{
		// Token: 0x06001A00 RID: 6656 RVA: 0x001629FA File Offset: 0x001619FA
		internal DataStreams()
		{
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x00162A0E File Offset: 0x00161A0E
		internal bool HasAnyData
		{
			get
			{
				return (this._subStreams != null && this._subStreams.Count > 0) || (this._customJournaledObjects != null && this._customJournaledObjects.Count > 0);
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00162A40 File Offset: 0x00161A40
		private bool HasSubStreams(object key)
		{
			return this._subStreams != null && this._subStreams.Contains(key);
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00162A58 File Offset: 0x00161A58
		private ArrayList GetSubStreams(object key)
		{
			ArrayList arrayList = (ArrayList)this._subStreams[key];
			if (arrayList == null)
			{
				arrayList = new ArrayList(3);
				this._subStreams[key] = arrayList;
			}
			return arrayList;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x00162A90 File Offset: 0x00161A90
		private ArrayList SaveSubStreams(UIElement element)
		{
			ArrayList arrayList = null;
			if (element != null && element.PersistId != 0)
			{
				LocalValueEnumerator localValueEnumerator = element.GetLocalValueEnumerator();
				while (localValueEnumerator.MoveNext())
				{
					LocalValueEntry localValueEntry = localValueEnumerator.Current;
					FrameworkPropertyMetadata frameworkPropertyMetadata = localValueEntry.Property.GetMetadata(element.DependencyObjectType) as FrameworkPropertyMetadata;
					if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Journal && !(localValueEntry.Value is Expression) && localValueEntry.Property != Frame.SourceProperty)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList(3);
						}
						object value = element.GetValue(localValueEntry.Property);
						byte[] dataBytes = null;
						if (value != null && !(value is Uri))
						{
							MemoryStream memoryStream = new MemoryStream();
							this.Formatter.Serialize(memoryStream, value);
							dataBytes = memoryStream.ToArray();
							((IDisposable)memoryStream).Dispose();
						}
						arrayList.Add(new SubStream(localValueEntry.Property.Name, dataBytes));
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00162B84 File Offset: 0x00161B84
		private void SaveState(object node)
		{
			UIElement uielement = node as UIElement;
			if (uielement == null)
			{
				return;
			}
			int persistId = uielement.PersistId;
			if (persistId != 0)
			{
				ArrayList arrayList = this.SaveSubStreams(uielement);
				if (arrayList != null && !this._subStreams.Contains(persistId))
				{
					this._subStreams[persistId] = arrayList;
				}
				IJournalState journalState = node as IJournalState;
				if (journalState != null)
				{
					object journalState2 = journalState.GetJournalState(JournalReason.NewContentNavigation);
					if (journalState2 != null)
					{
						if (this._customJournaledObjects == null)
						{
							this._customJournaledObjects = new HybridDictionary(2);
						}
						if (!this._customJournaledObjects.Contains(persistId))
						{
							this._customJournaledObjects[persistId] = journalState2;
						}
					}
				}
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x00162C2C File Offset: 0x00161C2C
		internal void PrepareForSerialization()
		{
			if (this._customJournaledObjects != null)
			{
				foreach (object obj in this._customJournaledObjects)
				{
					((CustomJournalStateInternal)((DictionaryEntry)obj).Value).PrepareForSerialization();
				}
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00162C98 File Offset: 0x00161C98
		private void LoadSubStreams(UIElement element, ArrayList subStreams)
		{
			for (int i = 0; i < subStreams.Count; i++)
			{
				SubStream subStream = (SubStream)subStreams[i];
				DependencyProperty dependencyProperty = DependencyProperty.FromName(subStream._propertyName, element.GetType());
				if (dependencyProperty != null)
				{
					object value = null;
					if (subStream._data != null)
					{
						value = this.Formatter.Deserialize(new MemoryStream(subStream._data));
					}
					element.SetValue(dependencyProperty, value);
				}
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00162D04 File Offset: 0x00161D04
		private void LoadState(object node)
		{
			UIElement uielement = node as UIElement;
			if (uielement == null)
			{
				return;
			}
			int persistId = uielement.PersistId;
			if (persistId != 0)
			{
				if (this.HasSubStreams(persistId))
				{
					ArrayList subStreams = this.GetSubStreams(persistId);
					this.LoadSubStreams(uielement, subStreams);
				}
				if (this._customJournaledObjects != null && this._customJournaledObjects.Contains(persistId))
				{
					CustomJournalStateInternal state = (CustomJournalStateInternal)this._customJournaledObjects[persistId];
					IJournalState journalState = node as IJournalState;
					if (journalState != null)
					{
						journalState.RestoreJournalState(state);
					}
				}
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x00162D90 File Offset: 0x00161D90
		private void WalkLogicalTree(object node, DataStreams.NodeOperation operation)
		{
			if (node != null)
			{
				operation(node);
			}
			DependencyObject dependencyObject = node as DependencyObject;
			if (dependencyObject == null)
			{
				return;
			}
			IEnumerator enumerator = LogicalTreeHelper.GetChildren(dependencyObject).GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				object node2 = enumerator.Current;
				this.WalkLogicalTree(node2, operation);
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00162DD7 File Offset: 0x00161DD7
		internal void Save(object root)
		{
			if (this._subStreams == null)
			{
				this._subStreams = new HybridDictionary(3);
			}
			else
			{
				this._subStreams.Clear();
			}
			this.WalkLogicalTree(root, new DataStreams.NodeOperation(this.SaveState));
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00162E0D File Offset: 0x00161E0D
		internal void Load(object root)
		{
			if (this.HasAnyData)
			{
				this.WalkLogicalTree(root, new DataStreams.NodeOperation(this.LoadState));
			}
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x00162E2A File Offset: 0x00161E2A
		internal void Clear()
		{
			this._subStreams = null;
			this._customJournaledObjects = null;
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x00162E3A File Offset: 0x00161E3A
		private BinaryFormatter Formatter
		{
			get
			{
				if (DataStreams._formatter == null)
				{
					DataStreams._formatter = new BinaryFormatter();
				}
				return DataStreams._formatter;
			}
		}

		// Token: 0x04000D95 RID: 3477
		[ThreadStatic]
		private static BinaryFormatter _formatter;

		// Token: 0x04000D96 RID: 3478
		private HybridDictionary _subStreams = new HybridDictionary(3);

		// Token: 0x04000D97 RID: 3479
		private HybridDictionary _customJournaledObjects;

		// Token: 0x02000A10 RID: 2576
		// (Invoke) Token: 0x060084C6 RID: 33990
		private delegate void NodeOperation(object node);
	}
}
