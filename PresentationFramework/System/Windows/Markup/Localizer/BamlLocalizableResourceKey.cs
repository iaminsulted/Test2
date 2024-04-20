using System;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x0200053D RID: 1341
	public class BamlLocalizableResourceKey
	{
		// Token: 0x06004259 RID: 16985 RVA: 0x0021B4F8 File Offset: 0x0021A4F8
		internal BamlLocalizableResourceKey(string uid, string className, string propertyName, string assemblyName)
		{
			if (uid == null)
			{
				throw new ArgumentNullException("uid");
			}
			if (className == null)
			{
				throw new ArgumentNullException("className");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._uid = uid;
			this._className = className;
			this._propertyName = propertyName;
			this._assemblyName = assemblyName;
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0021B552 File Offset: 0x0021A552
		public BamlLocalizableResourceKey(string uid, string className, string propertyName) : this(uid, className, propertyName, null)
		{
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x0600425B RID: 16987 RVA: 0x0021B55E File Offset: 0x0021A55E
		public string Uid
		{
			get
			{
				return this._uid;
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x0021B566 File Offset: 0x0021A566
		public string ClassName
		{
			get
			{
				return this._className;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x0600425D RID: 16989 RVA: 0x0021B56E File Offset: 0x0021A56E
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x0021B576 File Offset: 0x0021A576
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x0021B57E File Offset: 0x0021A57E
		public bool Equals(BamlLocalizableResourceKey other)
		{
			return other != null && (this._uid == other._uid && this._className == other._className) && this._propertyName == other._propertyName;
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x0021B5BE File Offset: 0x0021A5BE
		public override bool Equals(object other)
		{
			return this.Equals(other as BamlLocalizableResourceKey);
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x0021B5CC File Offset: 0x0021A5CC
		public override int GetHashCode()
		{
			return this._uid.GetHashCode() ^ this._className.GetHashCode() ^ this._propertyName.GetHashCode();
		}

		// Token: 0x040024F9 RID: 9465
		private string _uid;

		// Token: 0x040024FA RID: 9466
		private string _className;

		// Token: 0x040024FB RID: 9467
		private string _propertyName;

		// Token: 0x040024FC RID: 9468
		private string _assemblyName;
	}
}
