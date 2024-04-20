using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000289 RID: 649
	[Serializable]
	internal class JournalEntryPageFunctionUri : JournalEntryPageFunctionSaver, ISerializable
	{
		// Token: 0x0600188F RID: 6287 RVA: 0x0016101D File Offset: 0x0016001D
		internal JournalEntryPageFunctionUri(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction, Uri markupUri) : base(jeGroupState, pageFunction)
		{
			this._markupUri = markupUri;
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0016102E File Offset: 0x0016002E
		protected JournalEntryPageFunctionUri(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._markupUri = (Uri)info.GetValue("_markupUri", typeof(Uri));
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00161058 File Offset: 0x00160058
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_markupUri", this._markupUri);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00161074 File Offset: 0x00160074
		internal override PageFunctionBase ResumePageFunction()
		{
			PageFunctionBase pageFunctionBase = Application.LoadComponent(this._markupUri, true) as PageFunctionBase;
			this.RestoreState(pageFunctionBase);
			return pageFunctionBase;
		}

		// Token: 0x04000D57 RID: 3415
		private Uri _markupUri;
	}
}
