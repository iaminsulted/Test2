using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000288 RID: 648
	[Serializable]
	internal class JournalEntryPageFunctionType : JournalEntryPageFunctionSaver, ISerializable
	{
		// Token: 0x06001889 RID: 6281 RVA: 0x00160EFC File Offset: 0x0015FEFC
		internal JournalEntryPageFunctionType(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
			string assemblyQualifiedName = pageFunction.GetType().AssemblyQualifiedName;
			this._typeName = new SecurityCriticalDataForSet<string>(assemblyQualifiedName);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00160F29 File Offset: 0x0015FF29
		protected JournalEntryPageFunctionType(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._typeName = new SecurityCriticalDataForSet<string>(info.GetString("_typeName"));
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00160F49 File Offset: 0x0015FF49
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_typeName", this._typeName.Value);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00160F69 File Offset: 0x0015FF69
		internal override void SaveState(object contentObject)
		{
			base.SaveState(contentObject);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00160F74 File Offset: 0x0015FF74
		internal override PageFunctionBase ResumePageFunction()
		{
			Invariant.Assert(this._typeName.Value != null, "JournalEntry does not contain the Type for the PageFunction to be created");
			Type type = Type.GetType(this._typeName.Value);
			PageFunctionBase pageFunctionBase;
			try
			{
				pageFunctionBase = (PageFunctionBase)Activator.CreateInstance(type);
			}
			catch (Exception innerException)
			{
				throw new Exception(SR.Get("FailedResumePageFunction", new object[]
				{
					this._typeName.Value
				}), innerException);
			}
			this.InitializeComponent(pageFunctionBase);
			this.RestoreState(pageFunctionBase);
			return pageFunctionBase;
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00161000 File Offset: 0x00160000
		private void InitializeComponent(PageFunctionBase pageFunction)
		{
			IComponentConnector componentConnector = pageFunction as IComponentConnector;
			if (componentConnector != null)
			{
				componentConnector.InitializeComponent();
			}
		}

		// Token: 0x04000D56 RID: 3414
		private SecurityCriticalDataForSet<string> _typeName;
	}
}
