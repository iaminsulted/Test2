using System;

namespace System.Windows.Data
{
	// Token: 0x0200046A RID: 1130
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ValueConversionAttribute : Attribute
	{
		// Token: 0x06003A26 RID: 14886 RVA: 0x001EFB52 File Offset: 0x001EEB52
		public ValueConversionAttribute(Type sourceType, Type targetType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06003A27 RID: 14887 RVA: 0x001EFB90 File Offset: 0x001EEB90
		public Type SourceType
		{
			get
			{
				return this._sourceType;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x001EFB98 File Offset: 0x001EEB98
		public Type TargetType
		{
			get
			{
				return this._targetType;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06003A29 RID: 14889 RVA: 0x001EFBA0 File Offset: 0x001EEBA0
		// (set) Token: 0x06003A2A RID: 14890 RVA: 0x001EFBA8 File Offset: 0x001EEBA8
		public Type ParameterType
		{
			get
			{
				return this._parameterType;
			}
			set
			{
				this._parameterType = value;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public override object TypeId
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x001EFBB1 File Offset: 0x001EEBB1
		public override int GetHashCode()
		{
			return this._sourceType.GetHashCode() + this._targetType.GetHashCode();
		}

		// Token: 0x04001D97 RID: 7575
		private Type _sourceType;

		// Token: 0x04001D98 RID: 7576
		private Type _targetType;

		// Token: 0x04001D99 RID: 7577
		private Type _parameterType;
	}
}
