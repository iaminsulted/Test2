using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000287 RID: 647
	[Serializable]
	internal abstract class JournalEntryPageFunctionSaver : JournalEntryPageFunction, ISerializable
	{
		// Token: 0x06001883 RID: 6275 RVA: 0x00160DAE File Offset: 0x0015FDAE
		internal JournalEntryPageFunctionSaver(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00160DB8 File Offset: 0x0015FDB8
		protected JournalEntryPageFunctionSaver(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._returnEventSaver = (ReturnEventSaver)info.GetValue("_returnEventSaver", typeof(ReturnEventSaver));
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00160DE2 File Offset: 0x0015FDE2
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_returnEventSaver", this._returnEventSaver);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00160E00 File Offset: 0x0015FE00
		internal override void SaveState(object contentObject)
		{
			PageFunctionBase pageFunctionBase = (PageFunctionBase)contentObject;
			this._returnEventSaver = pageFunctionBase._Saver;
			base.SaveState(contentObject);
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00160E28 File Offset: 0x0015FE28
		internal override void RestoreState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			PageFunctionBase pageFunctionBase = (PageFunctionBase)contentObject;
			if (pageFunctionBase == null)
			{
				throw new Exception(SR.Get("InvalidPageFunctionType", new object[]
				{
					contentObject.GetType()
				}));
			}
			pageFunctionBase.ParentPageFunctionId = base.ParentPageFunctionId;
			pageFunctionBase.PageFunctionId = base.PageFunctionId;
			pageFunctionBase._Saver = this._returnEventSaver;
			pageFunctionBase._Resume = true;
			base.RestoreState(pageFunctionBase);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00160EA0 File Offset: 0x0015FEA0
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			IDownloader downloader = navigator as IDownloader;
			NavigationService navigationService = (downloader != null) ? downloader.Downloader : null;
			PageFunctionBase content = (navigationService != null && navigationService.ContentId == base.ContentId) ? ((PageFunctionBase)navigationService.Content) : this.ResumePageFunction();
			return navigator.Navigate(content, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04000D55 RID: 3413
		private ReturnEventSaver _returnEventSaver;
	}
}
