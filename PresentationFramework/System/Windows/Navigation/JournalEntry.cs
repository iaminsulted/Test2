using System;
using System.IO.Packaging;
using System.Runtime.Serialization;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Utility;

namespace System.Windows.Navigation
{
	// Token: 0x020005B1 RID: 1457
	[Serializable]
	public class JournalEntry : DependencyObject, ISerializable
	{
		// Token: 0x06004670 RID: 18032 RVA: 0x0022626C File Offset: 0x0022526C
		internal JournalEntry(JournalEntryGroupState jeGroupState, Uri uri)
		{
			this._jeGroupState = jeGroupState;
			if (jeGroupState != null)
			{
				jeGroupState.GroupExitEntry = this;
			}
			this.Source = uri;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x0022628C File Offset: 0x0022528C
		protected JournalEntry(SerializationInfo info, StreamingContext context)
		{
			this._id = info.GetInt32("_id");
			this._source = (Uri)info.GetValue("_source", typeof(Uri));
			this._entryType = (JournalEntryType)info.GetValue("_entryType", typeof(JournalEntryType));
			this._jeGroupState = (JournalEntryGroupState)info.GetValue("_jeGroupState", typeof(JournalEntryGroupState));
			this._customContentState = (CustomContentState)info.GetValue("_customContentState", typeof(CustomContentState));
			this._rootViewerState = (CustomJournalStateInternal)info.GetValue("_rootViewerState", typeof(CustomJournalStateInternal));
			this.Name = info.GetString("Name");
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00226364 File Offset: 0x00225364
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_id", this._id);
			info.AddValue("_source", this._source);
			info.AddValue("_entryType", this._entryType);
			info.AddValue("_jeGroupState", this._jeGroupState);
			info.AddValue("_customContentState", this._customContentState);
			info.AddValue("_rootViewerState", this._rootViewerState);
			info.AddValue("Name", this.Name);
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x002263FB File Offset: 0x002253FB
		public static string GetName(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				return null;
			}
			return (string)dependencyObject.GetValue(JournalEntry.NameProperty);
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x00226412 File Offset: 0x00225412
		public static void SetName(DependencyObject dependencyObject, string name)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(JournalEntry.NameProperty, name);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x0022642E File Offset: 0x0022542E
		public static bool GetKeepAlive(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (bool)dependencyObject.GetValue(JournalEntry.KeepAliveProperty);
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x0022644E File Offset: 0x0022544E
		public static void SetKeepAlive(DependencyObject dependencyObject, bool keepAlive)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(JournalEntry.KeepAliveProperty, keepAlive);
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x0022646A File Offset: 0x0022546A
		// (set) Token: 0x06004678 RID: 18040 RVA: 0x00226472 File Offset: 0x00225472
		public Uri Source
		{
			get
			{
				return this._source;
			}
			set
			{
				this._source = BindUriHelper.GetUriRelativeToPackAppBase(value);
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06004679 RID: 18041 RVA: 0x00226480 File Offset: 0x00225480
		// (set) Token: 0x0600467A RID: 18042 RVA: 0x00226488 File Offset: 0x00225488
		public CustomContentState CustomContentState
		{
			get
			{
				return this._customContentState;
			}
			internal set
			{
				this._customContentState = value;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x00226491 File Offset: 0x00225491
		// (set) Token: 0x0600467C RID: 18044 RVA: 0x002264A3 File Offset: 0x002254A3
		public string Name
		{
			get
			{
				return (string)base.GetValue(JournalEntry.NameProperty);
			}
			set
			{
				base.SetValue(JournalEntry.NameProperty, value);
			}
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsPageFunction()
		{
			return false;
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsAlive()
		{
			return false;
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x002264B4 File Offset: 0x002254B4
		internal virtual void SaveState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			if (!this.IsAlive())
			{
				if (this._jeGroupState.JournalDataStreams == null)
				{
					this._jeGroupState.JournalDataStreams = new DataStreams();
				}
				this._jeGroupState.JournalDataStreams.Save(contentObject);
			}
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x00226508 File Offset: 0x00225508
		internal virtual void RestoreState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			if (!this.IsAlive())
			{
				DataStreams journalDataStreams = this._jeGroupState.JournalDataStreams;
				if (journalDataStreams != null)
				{
					journalDataStreams.Load(contentObject);
					journalDataStreams.Clear();
				}
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00226547 File Offset: 0x00225547
		internal virtual bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			if (this.Source != null)
			{
				return navigator.Navigate(this.Source, new NavigateInfo(this.Source, navMode, this));
			}
			Invariant.Assert(false, "Cannot navigate to a journal entry that does not have a Source.");
			return false;
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00226580 File Offset: 0x00225580
		internal static string GetDisplayName(Uri uri, Uri siteOfOrigin)
		{
			if (!uri.IsAbsoluteUri)
			{
				return uri.ToString();
			}
			string text;
			if (string.Compare(uri.Scheme, PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) == 0)
			{
				Uri uri2 = BaseUriHelper.MakeRelativeToSiteOfOriginIfPossible(uri);
				if (!uri2.IsAbsoluteUri)
				{
					text = new Uri(siteOfOrigin, uri2).ToString();
				}
				else
				{
					string text2 = uri.AbsolutePath + uri.Query + uri.Fragment;
					string text3;
					string value;
					string text4;
					string text5;
					BaseUriHelper.GetAssemblyNameAndPart(new Uri(text2, UriKind.Relative), out text3, out value, out text4, out text5);
					if (!string.IsNullOrEmpty(value))
					{
						text = text3;
					}
					else
					{
						text = text2;
					}
				}
			}
			else
			{
				text = uri.ToString();
			}
			if (!string.IsNullOrEmpty(text) && text[0] == '/')
			{
				text = text.Substring(1);
			}
			return text;
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x00226630 File Offset: 0x00225630
		// (set) Token: 0x06004684 RID: 18052 RVA: 0x00226638 File Offset: 0x00225638
		internal JournalEntryGroupState JEGroupState
		{
			get
			{
				return this._jeGroupState;
			}
			set
			{
				this._jeGroupState = value;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06004685 RID: 18053 RVA: 0x00226641 File Offset: 0x00225641
		// (set) Token: 0x06004686 RID: 18054 RVA: 0x00226649 File Offset: 0x00225649
		internal int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x00226652 File Offset: 0x00225652
		internal Guid NavigationServiceId
		{
			get
			{
				return this._jeGroupState.NavigationServiceId;
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x0022665F File Offset: 0x0022565F
		// (set) Token: 0x06004689 RID: 18057 RVA: 0x00226667 File Offset: 0x00225667
		internal JournalEntryType EntryType
		{
			get
			{
				return this._entryType;
			}
			set
			{
				this._entryType = value;
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00226670 File Offset: 0x00225670
		internal bool IsNavigable()
		{
			return this._entryType == JournalEntryType.Navigable;
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x0600468B RID: 18059 RVA: 0x0022667B File Offset: 0x0022567B
		internal uint ContentId
		{
			get
			{
				return this._jeGroupState.ContentId;
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x0600468C RID: 18060 RVA: 0x00226688 File Offset: 0x00225688
		// (set) Token: 0x0600468D RID: 18061 RVA: 0x00226690 File Offset: 0x00225690
		internal CustomJournalStateInternal RootViewerState
		{
			get
			{
				return this._rootViewerState;
			}
			set
			{
				this._rootViewerState = value;
			}
		}

		// Token: 0x0400256C RID: 9580
		public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof(string), typeof(JournalEntry), new PropertyMetadata(string.Empty));

		// Token: 0x0400256D RID: 9581
		public static readonly DependencyProperty KeepAliveProperty = DependencyProperty.RegisterAttached("KeepAlive", typeof(bool), typeof(JournalEntry), new PropertyMetadata(false));

		// Token: 0x0400256E RID: 9582
		private int _id;

		// Token: 0x0400256F RID: 9583
		private JournalEntryGroupState _jeGroupState;

		// Token: 0x04002570 RID: 9584
		private Uri _source;

		// Token: 0x04002571 RID: 9585
		private JournalEntryType _entryType;

		// Token: 0x04002572 RID: 9586
		private CustomContentState _customContentState;

		// Token: 0x04002573 RID: 9587
		private CustomJournalStateInternal _rootViewerState;
	}
}
