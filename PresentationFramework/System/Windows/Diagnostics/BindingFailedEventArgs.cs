using System;
using System.Diagnostics;
using System.Windows.Data;

namespace System.Windows.Diagnostics
{
	// Token: 0x02000440 RID: 1088
	public class BindingFailedEventArgs : EventArgs
	{
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x001DCF14 File Offset: 0x001DBF14
		public TraceEventType EventType { get; }

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x001DCF1C File Offset: 0x001DBF1C
		public int Code { get; }

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600350D RID: 13581 RVA: 0x001DCF24 File Offset: 0x001DBF24
		public string Message { get; }

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600350E RID: 13582 RVA: 0x001DCF2C File Offset: 0x001DBF2C
		public BindingExpressionBase Binding { get; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x001DCF34 File Offset: 0x001DBF34
		public object[] Parameters { get; }

		// Token: 0x06003510 RID: 13584 RVA: 0x001DCF3C File Offset: 0x001DBF3C
		internal BindingFailedEventArgs(TraceEventType eventType, int code, string message, BindingExpressionBase binding, params object[] parameters)
		{
			this.EventType = eventType;
			this.Code = code;
			this.Message = (message ?? string.Empty);
			this.Binding = binding;
			this.Parameters = (parameters ?? Array.Empty<object>());
		}
	}
}
