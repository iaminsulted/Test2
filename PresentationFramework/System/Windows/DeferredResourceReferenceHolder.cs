using System;

namespace System.Windows
{
	// Token: 0x020003BF RID: 959
	internal class DeferredResourceReferenceHolder : DeferredResourceReference
	{
		// Token: 0x0600286F RID: 10351 RVA: 0x0019544C File Offset: 0x0019444C
		internal DeferredResourceReferenceHolder(object resourceKey, object value) : base(null, null)
		{
			this._keyOrValue = new object[]
			{
				resourceKey,
				value
			};
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x0019546A File Offset: 0x0019446A
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return this.Value;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x00195474 File Offset: 0x00194474
		internal override Type GetValueType()
		{
			object value = this.Value;
			if (value == null)
			{
				return null;
			}
			return value.GetType();
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002872 RID: 10354 RVA: 0x00195493 File Offset: 0x00194493
		internal override object Key
		{
			get
			{
				return ((object[])this._keyOrValue)[0];
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x001954A2 File Offset: 0x001944A2
		// (set) Token: 0x06002874 RID: 10356 RVA: 0x001954B1 File Offset: 0x001944B1
		internal override object Value
		{
			get
			{
				return ((object[])this._keyOrValue)[1];
			}
			set
			{
				((object[])this._keyOrValue)[1] = value;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x001954C1 File Offset: 0x001944C1
		internal override bool IsUnset
		{
			get
			{
				return this.Value == DependencyProperty.UnsetValue;
			}
		}
	}
}
